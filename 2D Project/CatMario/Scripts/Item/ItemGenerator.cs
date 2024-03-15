using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] arrItemGo;
    // Start is called before the first frame update
    
    public void GenerateItem(int type, Transform parent)
    {
        Debug.Log(arrItemGo[type].ToString());
        GameObject go = Instantiate(arrItemGo[type], parent);
        go.transform.SetParent(null);
    }
}
