using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public System.Action onCollision;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.LogFormat("<color=red>top</color>");
            if(onCollision != null)
                this.onCollision();
        }
    }
}
