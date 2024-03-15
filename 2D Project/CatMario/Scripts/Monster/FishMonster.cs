using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMonster : Monster
{
    public float moveSpeed;
    public override void Init()
    {
        base.Init();
        this.middle.onTouchObstacle = () =>
        {
            Debug.Log("!");
        };
    }
    public void Move()
    {
        if (moveSpeed < 0)
            this.transform.localScale *= new Vector2(1, -1);
        this.rBody.velocity=Vector2.up*moveSpeed;
    }
}
