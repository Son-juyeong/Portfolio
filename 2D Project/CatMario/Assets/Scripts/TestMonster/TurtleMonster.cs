using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleMonster : Monster
{
    public enum eState
    {
        Idle, Attack
    }
    private eState state;
    [SerializeField]
    private GameObject[] arrTurtleStateGo;
    private TurtleIdle idle;
    private TurtleAttack attack;
    private bool isAttack = true;
    private float moveSpeed = 30f;

    public override void Init()
    {
        this.idle = arrTurtleStateGo[(int)eState.Idle].GetComponent<TurtleIdle>();
        this.attack = arrTurtleStateGo[(int)eState.Attack].GetComponent<TurtleAttack>();
        this.transform.localScale = new Vector2(-1, 1);
    }

    public void Move()
    {
        this.rBody.velocity = Vector2.right * this.moveSpeed;
    }
}
