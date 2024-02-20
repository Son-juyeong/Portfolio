using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    protected Rigidbody2D rBody;
    protected MonsterMiddle middle;
    public System.Action onAttackPlayer;
    public virtual void Init()
    {
        this.rBody = GetComponent<Rigidbody2D>();
        this.middle=this.GetComponentInChildren<MonsterMiddle>();
        this.middle.onAttackPlayer = () =>
        {
            Debug.Log("Player АјАн!");
            this.onAttackPlayer();
        };  
    }
}
