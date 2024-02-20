using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum eState
    {
        Idle, Run, Jump, Fall, Die
    }
    private eState state;
    private Rigidbody2D rBody;
    public float moveForce = 800f;
    public float maxMoveSpeed = 280f;
    private bool isJump = false;
    private bool isFall = true;
    public bool IsFall { get { return isFall; } }
    public float jumpForce = 32200f;
    private Animator anim;
    private float maxPosX;
    private float dampingMoveSpeed = 0.6f;
    private float animationSpeed = 0.8f;
    private int life = 2;
    private bool isDie;
    public bool IsDie { get { return isDie; } }
    public int Life { get { return life; } }

    public System.Action onNoteJumpImpossible;
    public System.Func<GameObject, bool> onQueryMonsterLife;
    public Coroutine coroutineDie;

    public void Init(Vector2 pos, float maxPosX)
    {
        this.isDie = false;
        this.maxPosX = maxPosX;
        this.transform.position = pos;
        this.anim = GetComponent<Animator>();
        this.rBody = GetComponent<Rigidbody2D>();
        Debug.LogFormat("player init!({0})", this.transform.position);
    }

    public void Move(float dirX, float minPosX)
    {
        if (this.isDie) return;
        this.rBody.AddForce(Vector2.right * dirX * this.moveForce);
        this.rBody.velocity = new Vector2(Mathf.Clamp(this.rBody.velocity.x, -this.maxMoveSpeed, this.maxMoveSpeed), this.rBody.velocity.y);
        if (dirX == 0 && this.rBody.velocity.magnitude > 0)
        {
            this.rBody.velocity = new Vector2(this.rBody.velocity.x * this.dampingMoveSpeed, this.rBody.velocity.y);
        }
        float posX = this.transform.position.x;
        float posY = this.transform.position.y;
        this.transform.position = new Vector2(Mathf.Clamp(posX, minPosX, this.maxPosX), posY);
        if (dirX != 0) {
            this.transform.localScale = new Vector2(dirX, 1);
             }
        if (!isJump && !isFall)
        {
            if (dirX == 0)
                this.SetState(eState.Idle);
            else
                this.SetState(eState.Run);
        }
    }

    private void FixedUpdate()
    {
        if (isDie) return;
        if (this.rBody.velocity.y < -1f)
        {
            isJump = false;
            isFall = true;
            this.SetState(eState.Fall);
        }
    }

    public void ReachGround()
    {
        this.isJump = false;
        this.isFall = false;
        this.rBody.velocity *= Vector2.right;
    }

    public void Jump(bool isLong)
    {
        if (isJump == true||isFall==true||this.isDie==true)
        {
            this.onNoteJumpImpossible();
            return;
        }
        this.isJump = true;
        this.isFall = false;
        if (isLong)
            this.rBody.AddForce(Vector2.up * this.jumpForce * 1.2f);
        else
            this.rBody.AddForce(Vector2.up * this.jumpForce);
        this.SetState(eState.Jump);
    }

    private void SetState(eState state)
    {
        this.state = state;
        this.anim.SetInteger("State", (int)this.state);
        this.anim.speed = this.rBody.velocity.magnitude * Time.deltaTime * this.animationSpeed;
    }

    public IEnumerator Die(GameObject monster=null)
    {
        yield return null;
        if (!this.onQueryMonsterLife(monster)) StopCoroutine(Die());
        Debug.Log("³ªÁ×´ÂˆÒ!");
        this.isDie = true;
        this.SetState(eState.Die);
        if (coroutineDie == null)
        {
            coroutineDie = StartCoroutine(this.CoDie());
        }
    }

    private IEnumerator CoDie()
    {
        this.rBody.bodyType = RigidbodyType2D.Kinematic;
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        this.rBody.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.3f);
        //yield return new WaitForSeconds(1f);
        this.rBody.bodyType = RigidbodyType2D.Dynamic;
        this.rBody.AddForce(Vector2.zero);
        //yield return null;
        this.rBody.velocity = Vector2.zero;
        this.rBody.AddForce(Vector2.up * this.jumpForce*1f);
        Debug.Log(this.rBody.totalForce);
    }

    public void AfterAttack()
    {
        isJump = true;
    }
}
