using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrockBottom : MonoBehaviour
{
    public System.Action onCollision;
    public System.Action onAttackPlayer;
    public System.Action onTriggerPlayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.LogFormat("<color=blue>bottom</color>");
            if(onCollision != null)
                this.onCollision();
            if(onAttackPlayer != null)
                this.onAttackPlayer();
        }
    }
}
