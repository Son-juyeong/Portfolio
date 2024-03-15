using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPotionSpawn : MonoBehaviour
{
    [SerializeField] Transform originalTrans;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.gameObject.activeSelf)
        {
            Debug.Log("healPotion unactivated. respawn start!");
            StartCoroutine(this.CoReSpawnHealPotion());
        }   
    }

    private IEnumerator CoReSpawnHealPotion()
    {
        yield return new WaitForSeconds(5f);
        
        this.gameObject.transform.SetParent(this.originalTrans);
        this.gameObject.transform.localPosition = Vector3.zero;
        this.gameObject.transform.localRotation = Quaternion.Euler(0,0,90);
        Debug.LogFormat("gameObject: {0} / position:{1}", this.gameObject, this.gameObject.transform);
        this.gameObject.SetActive(true);
    }

}
