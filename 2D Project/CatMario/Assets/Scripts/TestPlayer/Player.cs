using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TestPlayer
{
    public class Player : MonoBehaviour
    {
        public enum eState
        {
            Idle, Run, Jump
        }
        private eState state;
        private Rigidbody2D rBody;
        public float moveForce = 8000f;
        public float maxMoveSpeed = 300f;
        private float elapsedTime = 0f;
        private float jumpKeyPressTime = 0.15f;
        private bool isJump = true;
        public float jumpForce = 200000f;
        private Animator anim;
        private bool isDie;
        // Start is called before the first frame update
        void Start()
        {
            this.rBody = GetComponent<Rigidbody2D>();
            this.anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isDie) return;
            float dirX = Input.GetAxisRaw("Horizontal");
            //Debug.LogFormat("dirX: {0}", dirX);
            //if (isJump)
            //    this.state = eState.Jump;
            if (dirX != 0)
                this.state = eState.Run;
            else
                this.state = eState.Idle;
            this.anim.SetInteger("State", (int)state);
            if (this.state == eState.Run)
                this.anim.speed = this.rBody.velocity.magnitude * Time.deltaTime * 0.6f;
            this.rBody.AddForce(Vector2.right * dirX * this.moveForce);
            this.rBody.velocity = new Vector2(Mathf.Clamp(this.rBody.velocity.x, -this.maxMoveSpeed, this.maxMoveSpeed), this.rBody.velocity.y);
            if (dirX == 0 && this.rBody.velocity.magnitude > 0)
            {
                //Debug.Log("<color=yellow>°¨¼Ó</color>");
                this.rBody.velocity = new Vector2(this.rBody.velocity.x * 0.9f, this.rBody.velocity.y);
            }
            //Debug.LogFormat("<color=red>velocity: {0}</color>", this.rBody.velocity);
            //Debug.Log(Vector2.right * dirX * this.moveForce);
            //Debug.LogFormat("rBody.velocity.magnitude: {0}", this.rBody.velocity.magnitude);

            if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow))
            {

                //Debug.LogFormat("<color=yellow>elapesdTime: {0}</color>", this.elapsedTime);
                if (!isJump)
                {
                    this.elapsedTime += Time.deltaTime;
                    if (this.elapsedTime >= this.jumpKeyPressTime)
                    {
                        this.JumpLong();
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.UpArrow))
            {
                if (!isJump)
                {
                    if (this.elapsedTime >= this.jumpKeyPressTime)
                    {
                        this.JumpLong();
                    }
                    else
                    {
                        this.JumpShort();
                    }
                }
                elapsedTime = 0f;
            }
        }

        private void JumpShort()
        {
            this.isJump = true;
            this.rBody.AddForce(Vector2.up * this.jumpForce);
            Debug.LogFormat("<color=blue>force: {0}</color>", this.rBody.totalForce);
            this.elapsedTime = 0f;
        }

        private void JumpLong()
        {
            this.isJump = true;
            this.rBody.AddForce(Vector2.up * this.jumpForce * 1.2f);
            Debug.LogFormat("<color=red>force: {0}</color>", this.rBody.totalForce);
            this.elapsedTime = 0f;
        }

        private IEnumerator OnCollisionEnter2D(Collision2D collision)
        {
            if (this.isJump && this.rBody.velocity.y == 0)
            {
                isJump = false;
                yield break;
            }
           
        }
        private IEnumerator OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Monster"))
            {
                this.isDie = true;
                this.rBody.velocity = Vector2.zero;
                this.rBody.bodyType = RigidbodyType2D.Kinematic;
                this.gameObject.GetComponent<Collider2D>().enabled = false;
                yield return new WaitForSeconds(1f);
                this.rBody.bodyType = RigidbodyType2D.Dynamic;
                yield return null;
                this.rBody.AddForce(Vector2.up * this.jumpForce);
                
            }
        }
    }
}