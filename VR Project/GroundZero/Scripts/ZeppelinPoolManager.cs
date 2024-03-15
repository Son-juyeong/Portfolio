using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeppelinPoolManager : MonoBehaviour
{
    [SerializeField] private GameObject zeppelinPrefab;
    private List<GameObject> listZeppelin = new List<GameObject>();

    public static ZeppelinPoolManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject go = this.GenerateZeppelin();
            go.SetActive(false);
        }
    }

    private GameObject GenerateZeppelin()
    {
        GameObject go = Instantiate(zeppelinPrefab, this.transform);
        this.listZeppelin.Add(go);
        return go;
    }

    public GameObject GetZeppelin()
    {
        GameObject result = null;
        for (int i = 0; i < this.listZeppelin.Count; i++)
        {
            GameObject go = listZeppelin[i];
            if (go.activeSelf == false && go.transform.parent == this.transform)
            {
                go.transform.SetParent(null);
                go.SetActive(true);
                result = go;
                break;
            }
        }
        if (result == null)
        {
            result = this.GenerateZeppelin();
            result.transform.SetParent(null);
        }
        return result;
    }

    public void ReturnZeppelin(GameObject go)
    {
        go.transform.SetParent(this.transform);
        go.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        go.SetActive(false);
    }
}
