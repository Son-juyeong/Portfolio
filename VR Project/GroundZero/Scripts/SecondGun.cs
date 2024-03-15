using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondGun : MonoBehaviour
{
    [SerializeField] private GameObject shootEffectGo;   
    [SerializeField] private Transform effectTrans;
    [SerializeField] private Transform missileBullet;
    [SerializeField] private GameObject fireMuzzleEffect;

    public System.Action shoot;


    void Start()
    {
        this.shootEffectGo = Instantiate<GameObject>(shootEffectGo);
        this.shootEffectGo.SetActive(false);
        this.fireMuzzleEffect = Instantiate<GameObject>(fireMuzzleEffect);
        this.fireMuzzleEffect.SetActive(false);
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            this.shoot();
            this.StartCoroutine(CoShootEffect());
            this.StartCoroutine(CoFireEffect());
            //this.StartCoroutine(CoShootEffect1());
        }
    }

    IEnumerator CoShootEffect()
    {
        this.shootEffectGo.transform.position = this.effectTrans.position;
        this.shootEffectGo.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        this.shootEffectGo.SetActive(false);
    }

    IEnumerator CoFireEffect()
    {
        this.fireMuzzleEffect.transform.position = this.effectTrans.position;
        this.fireMuzzleEffect.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        this.fireMuzzleEffect.SetActive(false);
    }


}
