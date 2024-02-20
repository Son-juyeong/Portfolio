using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBottom : MonoBehaviour
{
    public System.Action onAttackPlayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            this.onAttackPlayer();
        }
    }
}
