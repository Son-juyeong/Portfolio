using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleTop : MonoBehaviour
{
    private Collider2D topCollider;
    public System.Action onChangeState;
    // Start is called before the first frame update
    void Start()
    {
        this.topCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Change State!!!");
            this.onChangeState();
        }
    }
}
