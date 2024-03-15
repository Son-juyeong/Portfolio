using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    protected Rigidbody2D rBody;
    protected MushroomBottom bottom;
    protected MushroomBody body;
    public System.Action onEaten;
    void Start()
    {
        this.rBody=GetComponent<Rigidbody2D>();
        this.rBody.bodyType = RigidbodyType2D.Kinematic;
        this.rBody.velocity = Vector2.up * 20f;
        this.body = GetComponentInChildren<MushroomBody>();
        this.body.onEaten = () =>
        {
            Debug.Log("Player eats me");
            //this.onEaten();
            this.Eaten();
        };
        this.bottom=this.GetComponentInChildren<MushroomBottom>();
        this.bottom.onExitBrock = () =>
        {
            this.bottom.ChangeTriggertoCollision();
            this.body.ActiveCollider();
            this.Move();
        };
        this.bottom.onEaten = () =>
        {
            Debug.Log("Player eats me");
            //this.onEaten();
            this.Eaten();
        };
    }

    public void Move()
    {
        this.rBody.bodyType = RigidbodyType2D.Dynamic;
        this.rBody.velocity = Vector2.right * 30f;
        Debug.Log(this.rBody.velocity);
    }

    public virtual void Eaten()
    {
        Destroy(this.gameObject);
    }
}
