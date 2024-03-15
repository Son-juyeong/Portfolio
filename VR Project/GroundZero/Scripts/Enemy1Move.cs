using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy1Move : EnemyMove
{
    public Action<EnemyData> OnGetData;
    public Action<int> OnGetHit;
    public Transform hpBarPoint;
    public int currHP = 0;
    private int maxHP = 0;

    private Coroutine attackRoutine;
    private Coroutine getHitRoutine;
    public Action<Enemy1Move> onDieEnemy;

    [SerializeField] EnemyController enemyControl;
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip attackSound;
   
    //--------------------------------------Enemy Die effect-----------------------------------------------------
    [SerializeField] GameObject dieEffectPrefab;
    //private GameObject dieEffectGo;
    public float dieEffectOffset;

    private void Awake()
    {
        this.OnGetData = (data) =>
        {
            //this.currHP = data.enemyHp; ;//set HP;
            this.maxHP = data.enemyHp; ;//set HP;
            this.currHP = this.maxHP;
        };

    }
    private void Start()
    {
        this.playerTrans = GameObject.Find("PlayerCar").transform;
        this.rBody = GetComponent<Rigidbody>();
        //this.dieEffectGo = Instantiate(this.dieEffectPrefab, this.transform);
        //this.dieEffectGo.SetActive(false);

        this.OnGetHit = (damage) =>
        {
            this.currHP -= damage;

            if (this.getHitRoutine != null)
            {
                this.StopCoroutine(this.CoGetHitAnimation());
            }
            this.getHitRoutine = StartCoroutine(this.CoGetHitAnimation());
        };

        this.onStartAttack = () =>
        {
            Debug.Log("Attack Player Start!!");
            if (this.attackRoutine != null)
            {
                this.StopCoroutine(this.CoAttackAnimation());

            }
            this.attackRoutine = StartCoroutine(this.CoAttackAnimation());
            this.audio.PlayOneShot(this.attackSound, 0.8f);
            // this.enemyControl.anim.SetInteger("State", 1);
        };
        this.onOnceAttack = () =>
        {
            Debug.Log("Once attack!");
            this.enemyControl.anim.SetInteger("State", 0);
            
        };

    }

    private IEnumerator CoGetHitAnimation()
    {
        this.enemyControl.anim.SetInteger("State", 2);
        yield return new WaitForSeconds(1f);
        this.enemyControl.anim.SetInteger("State", 0);

    }
    private IEnumerator CoAttackAnimation()
    {
        //this.enemyControl.anim.SetInteger("State", 1);
        //yield return new WaitForSeconds(0.3f);
        //this.enemyControl.anim.SetInteger("State", 0);
        this.enemyControl.anim.SetInteger("State", 0);
        yield return null;
        this.enemyControl.anim.SetInteger("State", 1);

    }
    private void FixedUpdate()
    {
        if (!this.isReady) return;
        this.Detect();
        this.DecideDir();
        this.Move();
        this.AimPlayer();
        // Debug.LogFormat("player Trans : {0}", this.playerTrans.position);
        if (this.currHP <= 0)
        {
            Debug.Log("<color=red>Enemy1 Die!!!!!!!!!</color>");
            
            this.StartCoroutine(this.CoDie());

            if (isAttack)
            {
                this.onCompleteAttack();
            }
        }
    }

    public override void Init(Vector3 pos)
    {
        base.Init(pos);
        this.currHP = this.maxHP;
       
    }

    public override void Detect()
    {
        base.Detect();
    }

    public override void DecideDir()
    {
        base.DecideDir();
    }

    public override void Move()
    {
        base.Move();
    }

    public override void RecurDrawAndHitRay(Vector3 pos1, Vector3 pos2, int depth)
    {
        base.RecurDrawAndHitRay(pos1, pos2, depth);
    }

    public override void DrawAndHitRay(Ray ray)
    {
        base.DrawAndHitRay(ray);
    }

    public override void UpdateTargetPos(int idx, Transform targetTrans)
    {
        base.UpdateTargetPos(idx, targetTrans);
    }

    public override IEnumerator CoChangeTarget()
    {
        return base.CoChangeTarget();
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    public override void AttackPlayer()
    {
        base.AttackPlayer();

        //if(this.attackRoutine != null)
        //{
        //    this.StopCoroutine(this.CoAttackAnimation());
        //}
        //this.attackRoutine = StartCoroutine(this.CoAttackAnimation());
    }

    private IEnumerator CoDie()
    {
        //this.dieEffectGo.SetActive(true);
        Instantiate(this.dieEffectPrefab, this.transform.position+Vector3.up*this.dieEffectOffset, Quaternion.identity);
        yield return null;
        this.onDieEnemy(this);
        InfoManager.Instance.kills++;
    }
}
