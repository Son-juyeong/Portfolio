using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMonster : Monster
{
    private MonsterTop top;
    private MonsterBottom bottom;
    public float moveSpeed = -30f;
    private bool isDie;
    public bool IsDie { get { return isDie; } }

    public System.Action onGetHit;

    public override void Init()
    {
        base.Init();
        Debug.LogFormat("middle:{0}", middle);
        this.top = GetComponentInChildren<MonsterTop>();
        Debug.LogFormat("top:{0}", top);
        if(this.moveSpeed>0)
            this.transform.localScale *= new Vector2(-1, 1);
        this.middle.onTouchObstacle = () =>
        {
            if (this.rBody.velocity.y < 0) return;
            this.moveSpeed *= -1;
            this.rBody.velocity = new Vector2(this.moveSpeed, this.rBody.velocity.y);
            this.transform.localScale *= new Vector2(-1, 1);
        };
        this.bottom = GetComponentInChildren<MonsterBottom>();
        Debug.LogFormat("bottom:{0}", bottom);
        this.bottom.onAttackPlayer = () =>
        {
            if (isDie) return;
            Debug.Log("Player АјАн!");
            this.onAttackPlayer();
        };
        this.top.onGetHit = () =>
        {
            this.isDie = true;
            this.onGetHit();
        };
    }

    public void Move()
    {
        this.rBody.bodyType = RigidbodyType2D.Dynamic;
        this.rBody.velocity += Vector2.right * moveSpeed;
    }

    public void Die()
    {
        this.gameObject.SetActive(false);
    }
}
