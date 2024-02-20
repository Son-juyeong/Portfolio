using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleMiddle : MonoBehaviour
{
    private Collider2D middleCollider;
    public System.Action onChangeState;
    public System.Action onAttackPlayer;
    public System.Action onAttackMonster;
    void Start()
    {
        this.middleCollider = GetComponent<Collider2D>();
        this.middleCollider.isTrigger = true;
    }

    public void ChangeCollisionMode()
    {
        this.middleCollider.isTrigger ^= true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Change State!!!");
            this.onChangeState();
        }   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {

        }
    }
}
