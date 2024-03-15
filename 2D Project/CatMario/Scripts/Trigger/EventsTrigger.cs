using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsTrigger : MonoBehaviour
{
    public System.Action onStartEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Trigger. event start");
            this.onStartEvent();
        }    
    }
}
