using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HiddenBrockTrigger : MonoBehaviour
{
    public System.Action onAppearBrock;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Trigger");
            float y = collision.transform.position.y;
            this.onAppearBrock();
        }
    }
}
