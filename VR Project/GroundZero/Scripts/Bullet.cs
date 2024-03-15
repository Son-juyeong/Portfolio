using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private Rigidbody rBody;
    private Vector3 targetPos;
    public float moveSpeed = 5f;
    public Action onAttackPlayer;
   
    [SerializeField] AudioClip playerGetHitSound;
    // Start is called before the first frame update
    void Start()
    {
        this.target = GameObject.Find("CenterEyeAnchor").transform;
        this.targetPos = this.target.position;
        this.rBody = this.GetComponent<Rigidbody>();
    }

    public void Init(Action callback)
    {
        this.onAttackPlayer = callback;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.LookAt(this.target);
        this.rBody.MovePosition(this.rBody.position + this.transform.forward * this.moveSpeed * Time.fixedDeltaTime);
        if (this.transform.position.z < this.target.position.z)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.LogFormat("<color=yellow>gameObject: {0}, tag: {1}</color>", collision.gameObject.name, collision.tag);
        if (collision.CompareTag("Player"))
        {
            Debug.Log("attack player");
            this.onAttackPlayer();
            collision.gameObject.GetComponent<AudioSource>().PlayOneShot(this.playerGetHitSound, 1.0f);
        }
        Destroy(this.gameObject);
    }
}
