using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class CamMove : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        private Queue<Transform> queueRouteTrans = new Queue<Transform>();
        private Transform curDestTrans;
        private bool isReady;
        public float rotSpeed = 1f;
        public float moveSpeed = 1.5f;
        // Update is called once per frame

        public void Init(Vector3 pos)
        {
            this.player.transform.position = pos;
            this.player.transform.rotation = Quaternion.Euler(Vector3.up * 180);
            isReady = true;
        }

        void Update()
        {
            if (!isReady) return;
            this.Move();
        }

        private void Move()
        {
            Vector3 destPos = this.queueRouteTrans.Peek().position;
            while (destPos.z > this.player.transform.position.z)
            {
                Transform trans = this.queueRouteTrans.Dequeue();
                //Debug.Log(trans.gameObject.name);
                destPos = this.queueRouteTrans.Peek().position;
                curDestTrans = this.queueRouteTrans.Peek();
            }
            //this.player.transform.LookAt(curDestTrans);
            Vector3 dir = curDestTrans.position - this.player.transform.position;
            this.player.transform.localRotation = Quaternion.Slerp(this.player.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * this.rotSpeed);
            this.player.transform.Translate(Vector3.forward * this.moveSpeed * Time.deltaTime);
        }

        public void UpdateRoute(Transform[] routes)
        {
            foreach (var route in routes)
            {
                queueRouteTrans.Enqueue(route);
            }
        }
    }
