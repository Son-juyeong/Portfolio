using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


    public class EnemyMove : MonoBehaviour
    {

        public enum eEnemyType
        {
            Enemy1, Enemy2, Enemy3
        }

        public eEnemyType enemyType;

        public KeyValuePair<int, Transform> target;
        protected bool isReady;
        public float moveSpeed = 5.5f;
        public float rotSpeed = 8f;
        public float changeTargetTime = 5f;

        public float trackingTargetTime = 2f;
        private float elapsedTime = 3f;

        public float offset = 0.5f;
        public float length = 3.5f;
        public int depth = 3;
        public Action<int> onChangeTarget;
        public Action onStartAttack;
        public Action onOnceAttack;

        protected Rigidbody rBody;
        private List<Tuple<Ray, bool>> listRays = new List<Tuple<Ray, bool>>();
        private Vector3 dir;
        private bool isDetected;

         private float curSpeed;

    //player attack
    //=============================================================================================================================
        [SerializeField] protected Transform[] muzzleTrans;
        [SerializeField] protected GameObject muzzleGo;
        protected Transform playerTrans;
        public int attackCount = 3;
        public float dealLossTime = 0.2f;
        public Action onAttackPlayer;
        public Action onCompleteAttack;
        protected bool isAttack;
        //=============================================================================================================================

    private void FixedUpdate()
        {
            if (!this.isReady) return;
            this.AimPlayer();
            this.Detect();
            this.DecideDir();
            this.Move();
           
        }

        private void Start()
        {
          //  this.playerTrans = GameObject.Find("PlayerCar").transform;
            this.rBody = this.GetComponent<Rigidbody>();
        }

        virtual public void Init(Vector3 pos)
        {
            //..Enemy 위치 지정 후
            this.transform.position = pos;
            //..활성화(보여주기)
            this.gameObject.SetActive(true);
        }

        virtual public void Detect()
        {
            this.listRays.Clear();
            this.isDetected = false;
            Vector3[] pos = new Vector3[] { this.transform.right, -this.transform.right, this.transform.forward };
            for (int i = 0; i < pos.Length; i++)
            {
                Ray ray = new Ray(this.transform.position + Vector3.up * this.offset, pos[i]);
                DrawAndHitRay(ray);
            }
            for (int i = 0; i < 2; i++)
            {
                this.RecurDrawAndHitRay(pos[i], pos[2], 0);
            }
        }

        virtual public void Move()
        {
        //this.curSpeed = this.moveSpeed;
        float posZ = this.transform.position.z;
        float targetPosZ = this.target.Value.position.z;
        if (posZ > targetPosZ + 15)
        {
            this.curSpeed *= 1.2f;
        }
        else if (posZ > targetPosZ + 8 && posZ < targetPosZ + 10)
        {
            //this.curSpeed = this.moveSpeed;
            if (this.curSpeed < this.moveSpeed * 0.75f)
                this.curSpeed *= 1.05f;
            else if (this.curSpeed > this.moveSpeed * 1.3f)
                this.curSpeed *= 0.99f;
        }
        else if (posZ < targetPosZ + 4)
        {
            this.curSpeed *= 0.95f;
        }
        this.curSpeed = Mathf.Clamp(this.curSpeed, this.moveSpeed * 0.6f, this.moveSpeed * 1.6f);
        float curSpeed = this.curSpeed;
        if (this.isDetected) curSpeed *= 0.8f;
        //float velocityY = this.rBody.velocity.y;
        //this.rBody.velocity = this.transform.forward * curSpeed;
        //this.rBody.velocity += Vector3.up * velocityY;
        this.rBody.MovePosition(this.rBody.position + this.transform.forward * curSpeed * Time.fixedDeltaTime);
        //Debug.LogFormat("<color=green>curspeed: {0}</color>", curSpeed);
          }


    virtual public void DecideDir()
        {
            List<List<Ray>> listNotHitRays = new List<List<Ray>>();
            List<Tuple<Ray, bool>> newListRays = listRays.OrderBy(x => Vector3.Distance(x.Item1.direction, this.transform.right)).ToList();
            for (int i = 0; i < newListRays.Count; i++)
            {
                Tuple<Ray, bool> tupleRay = newListRays[i];

                if (!tupleRay.Item2)
                {
                    if (listNotHitRays.Count == 0)
                    {
                        listNotHitRays.Add(new List<Ray>());
                    }
                    listNotHitRays[listNotHitRays.Count - 1].Add(tupleRay.Item1);
                    if (i < newListRays.Count - 1 && newListRays[i + 1].Item2)
                    {
                        listNotHitRays.Add(new List<Ray>());
                    }
                }
            }
            if (this.isDetected)
            {
                this.elapsedTime = 0f;

                int maxCount = 0;
                int maxCountIndex = 0;
                for (int i = 0; i < listNotHitRays.Count; i++)
                {
                    if (listNotHitRays[i].Count > maxCount)
                    {
                        maxCount = listNotHitRays[i].Count;
                        maxCountIndex = i;
                    }
                }

                this.dir = Vector3.zero;
                for (int i = 0; i < listNotHitRays[maxCountIndex].Count; i++)
                {
                    this.dir += listNotHitRays[maxCountIndex][i].direction;
                }
                this.dir /= maxCount;
                this.transform.rotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.LookRotation(this.dir), this.rotSpeed * Time.deltaTime);
            }
            else
            {
                this.elapsedTime += Time.deltaTime;
                if (this.elapsedTime > this.trackingTargetTime)
                {
                    this.dir = this.target.Value.position - this.transform.position;
                    //Debug.LogFormat("<color=red>{0}</color>", Quaternion.LookRotation(this.dir));
                    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(this.dir), this.rotSpeed * Time.deltaTime * 0.6f);
                }
                else
                    this.transform.rotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.LookRotation(this.dir), this.rotSpeed * Time.deltaTime);
            }
        }

        virtual public void RecurDrawAndHitRay(Vector3 pos1, Vector3 pos2, int depth)
        {
            Ray ray = new Ray(this.transform.position + Vector3.up * this.offset, (pos1 + pos2).normalized);
            this.DrawAndHitRay(ray);
            if (depth < this.depth)
            {
                this.RecurDrawAndHitRay(pos1, (pos1 + pos2).normalized, depth + 1);
                this.RecurDrawAndHitRay(pos2, (pos1 + pos2).normalized, depth + 1);
            }
        }

        virtual public void DrawAndHitRay(Ray ray)
        {
            int layerMask = 1 << 3 | 1 << 6 | 1 << 7;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, this.length, layerMask))
            {
                this.isDetected = true;
                Debug.DrawRay(ray.origin, ray.direction.normalized * this.length, Color.red);
                this.listRays.Add(new Tuple<Ray, bool>(ray, true));
            //    Debug.LogFormat("<color=blue>hit</color>: {0}", hit.collider.gameObject);
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction.normalized * this.length, Color.green);
                this.listRays.Add(new Tuple<Ray, bool>(ray, false));
            }
        }

        virtual public void UpdateTargetPos(int idx, Transform targetTrans)
        {
            this.target = new KeyValuePair<int, Transform>(idx, targetTrans);
            if (!isReady)
            {
                isReady = true;
                this.StartCoroutine(this.CoChangeTarget());
            }
        }

        virtual public IEnumerator CoChangeTarget()
        {
            while (true)
            {
                yield return new WaitForSeconds(this.changeTargetTime);
                this.onChangeTarget(this.target.Key);
            }
        }
    //추가 구현(공격)===========================================================================================================================


    //조준
    protected void AimPlayer()
    {
        this.muzzleGo?.transform.LookAt(this.playerTrans);
    }

    //player 공격 method
    virtual public void AttackPlayer()
    {
        StartCoroutine(this.CoAttackPlayer());
    }

    //dealLossTime에 맞춰 player를 공격하는 method
    protected IEnumerator CoAttackPlayer()
    {
        for (int i = 0; i < this.attackCount; i++)
        {
            this.onStartAttack();
            yield return new WaitForSeconds(0.11f);
            for (int j = 0; j < this.muzzleTrans.Length; j++)
            {
               // this.onStartAttack();
                GameObject bulletGo = BulletGenerator.Instance.GenerateBullet(this.muzzleTrans[j]);
                Bullet bullet = bulletGo.GetComponent<Bullet>();
                Debug.Log(bullet);

                
                bullet.Init(() =>
                {
                    this.onAttackPlayer();
                    
                });
            }
            yield return new WaitForSeconds(this.dealLossTime);
            this.onOnceAttack();
                        
        }
        this.onCompleteAttack();        //공격이 완료되면 대리자를 사용하여 TestEnemy1Main.cs에게 공격 완료를 알림.
    }

    //=======================================================================================================================================
    virtual public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, 2f);
        }


    }

