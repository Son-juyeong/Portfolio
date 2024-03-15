using System.Collections;
using System.Collections.Generic;
using UnityEngine;

   public class ZeppelinController : MonoBehaviour
{
    private int zeppelinMaxHp = 3;
    private int zeppelinHp;
    public float speed = 0.7f;
    [SerializeField] private GameObject effect;
    public System.Action<Vector3> die;

    public void Init()
    {
        this.zeppelinHp = this.zeppelinMaxHp;
    }

    void Update()
    {
        //this.transform.Translate(Vector3.forward * this.speed * Time.deltaTime);
    }

    public void Hit()
    {
        this.zeppelinHp--;
        if (this.zeppelinHp == 0)
        {
            this.die(this.transform.position);
            Instantiate(this.effect, this.transform.position, Quaternion.identity);
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
            //ZeppelinPoolManager.instance.ReturnZeppelin(this.gameObject);
        }
    }
}
