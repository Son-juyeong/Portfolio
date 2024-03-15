using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Item : MonoBehaviour
{
    private Transform pointTriggerTrans;
    public float duration = 0.7f;
    private Tweener tweener;

    void Start()
    {
        this.pointTriggerTrans = GameObject.Find("pointTrigger").transform;
        this.tweener = this.transform.DOMove(this.transform.position, this.duration).SetAutoKill(false).SetEase(Ease.InOutQuad);

    }
    void Update()
    {
        this.duration -= Time.deltaTime;
        this.duration = Mathf.Clamp(this.duration, 0.0001f, this.duration);
        this.tweener.ChangeEndValue(this.pointTriggerTrans.position, this.duration, true).Restart();
      
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.LogFormat("<color=yellow>Trigger</color>");
            this.tweener.Kill();
            Destroy(this.gameObject);
        }
    }
}
