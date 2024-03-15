using Meta.WitAi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBullet : MonoBehaviour
{
    private Transform shootDistance;
    private Rigidbody rb;
    public float moveSpeed = 5f;
    private Transform destroyTrans;
   // public System.Action OnHitEnemy;

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody>();
        this.shootDistance = GameObject.Find("ShootDistance").transform;
        this.transform.LookAt(this.shootDistance.position);
        this.destroyTrans = GameObject.Find("DestroyTrans").transform;
    }

    void FixedUpdate()
    {
        //this.rb.MovePosition(this.rb.position + this.transform.forward * this.moveSpeed * Time.fixedDeltaTime);
        //if (this.transform.position.z > GameObject.Find("DestroyTrans").transform.position.z)
        //{
        //    Destroy(this.gameObject);
        //}
        this.rb.AddForce(this.transform.forward * this.moveSpeed, ForceMode.VelocityChange);
        if (this.transform.position.z > this.destroyTrans.position.z)
        {
            Destroy(this.gameObject);
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        //if (!collision.collider.CompareTag("Enemy1") && !collision.collider.CompareTag("Enemy2") && !collision.collider.CompareTag("Enemy3")&&!collision.collider.CompareTag("Zeppelin")) return;

        if (collision.collider.CompareTag("Enemy1")|| collision.collider.CompareTag("Enemy2")|| collision.collider.CompareTag("Enemy3"))
        {
            Debug.Log("<color=lime>onHitEnemy</color>");
            Destroy(this.gameObject);
            var rightHand = this.gameObject.GetComponentInParent<MissileBulletGenerator>().missileGun;
            rightHand.OnHitEnemy(collision.contacts[0].point, collision.gameObject);
        }

        if (collision.collider.CompareTag("Zeppelin"))
        {
            var zeppelinCtrl = collision.collider.gameObject.GetComponentInParent<ZeppelinController>();
            zeppelinCtrl.Hit();
        }
        Destroy(this.gameObject);
    }
}