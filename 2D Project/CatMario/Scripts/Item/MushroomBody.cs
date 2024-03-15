using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBody : MonoBehaviour
{
    public System.Action onEaten;
    private void Start()
    {
        this.GetComponent<Collider2D>().enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            this.onEaten();
        }
    }

    public void ActiveCollider()
    {
        this.GetComponent<Collider2D>().enabled = true;
    }
}
