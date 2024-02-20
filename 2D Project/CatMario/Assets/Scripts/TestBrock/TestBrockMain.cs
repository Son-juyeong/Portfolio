using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBrockMain : MonoBehaviour
{
    [SerializeField]
    private ItemBrock itemBrock;
    [SerializeField]
    private ItemGenerator itemGenerator;
    
    void Start()
    {
        this.itemBrock.Init();
        this.itemBrock.onGenerateItem = (itemType) =>
        {
            this.itemGenerator.GenerateItem(itemType, this.itemBrock.transform);
            Debug.LogFormat("Generate {0}", itemType);
            this.itemBrock.CompleteGenerate();
        };
        this.itemBrock.onNotePlayerReach = () =>
        {
            Debug.Log("Player reach");
        };
    }
}
