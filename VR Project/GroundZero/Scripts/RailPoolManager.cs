using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RailPoolManager : MonoBehaviour
{
    public static RailPoolManager Instance;

    [SerializeField] private GameObject[] arrRailPrefabs;

    private List<List<GameObject>> listRail = new List<List<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Init(int count)
    {
        this.GenerateRail(count);
    }

    private void GenerateRail(int count)
    {
        for (int i = 0; i < arrRailPrefabs.Length; i++)
        {
            listRail.Add(new List<GameObject>());
            for (int j = 0; j < count; j++)
            {
                GameObject go = Instantiate(arrRailPrefabs[i], this.transform);
                listRail[i].Add(go);
                go.SetActive(false);
            }
        }
    }

    public GameObject EnableRail(int idx)
    {
        GameObject result = null;
        for (int i = 0; i < listRail[idx].Count; i++)
        {
            GameObject go = listRail[idx][i];
            if (go.activeSelf == false)
            {
                go.SetActive(true);
                go.transform.SetParent(null);
                result = go;
                break;
            }
        }
        return result;
    }

    public void DisableRail(GameObject go)
    {
        go.transform.SetParent(this.transform);
        go.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        go.SetActive(false);
    }
}
