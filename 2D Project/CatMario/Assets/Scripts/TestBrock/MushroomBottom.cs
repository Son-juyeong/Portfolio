using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBottom : MonoBehaviour
{
    public System.Action onExitBrock;
    public System.Action onEaten;

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        Debug.Log(collision.name);
        if (collision.CompareTag("ItemBox"))
        {
            Debug.Log("Exit Mushroom");
            this.onExitBrock();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.LogFormat("<color=red>{0}</color>", this);
            this.onEaten();
        }
    }

    public void ChangeTriggertoCollision()
    {
        this.gameObject.GetComponentInChildren<Collider2D>().isTrigger = false;
    }
}
