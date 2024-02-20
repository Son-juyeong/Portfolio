using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTop : MonoBehaviour
{
    public System.Action onGetHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            this.onGetHit();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            this.onGetHit();
        }
    }
}
