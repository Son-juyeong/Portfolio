using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Rigidbody2D rBody;
    void Start()
    {
        this.rBody = GetComponent<Rigidbody2D>();
        this.rBody.AddForce(Vector2.up * 15000f);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.rBody.velocity.y < 0f)
            Destroy(this.gameObject);
    }
}
