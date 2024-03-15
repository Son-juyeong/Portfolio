using UnityEngine;
using UnityEditor;
using System.Linq;

public class SaveSpritesFromTexture : EditorWindow
{
    [SerializeField]
    private Texture2D textureToSave; // Texture2D to save

    [MenuItem("Custom/Save Sprites From Texture")]
    private static void ShowWindow()
    {
        EditorWindow.GetWindow<SaveSpritesFromTexture>("Save Sprites");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Select Texture2D to Save", EditorStyles.boldLabel);
        textureToSave = EditorGUILayout.ObjectField("Texture", textureToSave, typeof(Texture2D), false) as Texture2D;

        if (GUILayout.Button("Save Sprites") && textureToSave != null)
        {
            SaveSprites();
        }
    }

    private void SaveSprites()
    {
        Texture2D texture = textureToSave;

        // Check if the texture is readable. If not, try to make it readable.
        if (!texture.isReadable)
        {
            Debug.Log("The texture is not readable. Attempting to make it readable...");
            TextureImporter textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(texture)) as TextureImporter;
            if (textureImporter != null)
            {
                textureImporter.isReadable = true;
                textureImporter.SaveAndReimport();
                Debug.Log("Texture is now readable.");
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError("Failed to make the texture readable. Check the import settings of the texture.");
                return;
            }
        }

        //Sprite[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(texture)) as Sprite[];
        Object[] assets = AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(texture));
        Sprite[] sprites = System.Array.FindAll(assets, asset => asset is Sprite).Cast<Sprite>().ToArray();


        Debug.LogFormat("sprites.Length: {0}", sprites.Length);

        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogWarning("No sprites found in the selected Texture2D.");
            return;
        }

        string folderPath = Application.dataPath + "/SavedSprites/";

        // Check if the directory exists, and create it if it doesn't.
        if (!System.IO.Directory.Exists(folderPath))
        {
            System.IO.Directory.CreateDirectory(folderPath);
        }

        foreach (Sprite sprite in sprites)
        {
            Texture2D spriteTexture = SpriteToTexture2D(sprite);
            byte[] bytes = spriteTexture.EncodeToPNG();
            System.IO.File.WriteAllBytes(folderPath + sprite.name + ".png", bytes);
        }

        Debug.Log("Sprites saved successfully to: " + folderPath);
    }

    private Texture2D SpriteToTexture2D(Sprite sprite)
    {
        Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        texture.SetPixels(sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y,
                            (int)sprite.rect.width, (int)sprite.rect.height));
        texture.Apply();
        return texture;
    }
}