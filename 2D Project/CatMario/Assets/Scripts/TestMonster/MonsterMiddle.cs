using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMiddle : MonoBehaviour
{
    private Collider2D middleCollider;
    public System.Action onAttackPlayer;
    public System.Action onTouchObstacle;
    // Start is called before the first frame update
    void Start()
    {
        this.middleCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            this.onAttackPlayer();
        }
        if (collision.CompareTag("Obstacle"))
        {
            this.onTouchObstacle();
        }
    }
}
