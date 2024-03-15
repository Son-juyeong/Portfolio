using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBulletGenerator : MonoBehaviour
{
    [SerializeField] private GameObject missileBullet;
    public RightHandController missileGun;
    private Transform shootTrans;

    void Start()
    {
        this.shootTrans = GameObject.Find("ShootTrans").transform;

        this.missileGun.shoot = () =>
        {
            this.GenerateMissileBullet();
        };
    }

    public void GenerateMissileBullet()
    {
        GameObject go = Instantiate(this.missileBullet, this.shootTrans.position, Quaternion.identity);
        go.transform.SetParent(this.transform);
    }

}

