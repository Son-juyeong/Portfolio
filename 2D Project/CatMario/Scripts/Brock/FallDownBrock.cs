using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDownBrock : MonoBehaviour
{
    private Rigidbody2D rBody;
    private Floor top;
    private BrockBottom bottom;
    public System.Action onNotePlayerReach;
    public System.Action onAttackPlayer;
    public void Init()
    {
        this.rBody = GetComponent<Rigidbody2D>();
        this.rBody.isKinematic = true;
        this.top = GetComponentInChildren<Floor>();
        this.top.onCollision = () =>
        {
            Debug.Log("Player reach ground");
            this.onNotePlayerReach();
        };
        this.bottom = GetComponentInChildren<BrockBottom>();
        this.bottom.onAttackPlayer = () =>
        {
            Debug.Log("플레이어 공격");
            this.rBody.isKinematic = true;
            this.onAttackPlayer();
        };
    }

    public void FallDown()
    {
        this.rBody.isKinematic = false;
    }
}
