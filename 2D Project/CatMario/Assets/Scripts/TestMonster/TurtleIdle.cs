using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleIdle : MonoBehaviour
{
    private MonsterBottom bottom;
    private TurtleTop top;
    private TurtleMiddle middle;
    public System.Action onAttackPlayer;
    void Start()
    {
        this.bottom = this.transform.GetComponentInChildren<MonsterBottom>();
        this.top=this.GetComponentInChildren<TurtleTop>();
        this.middle = this.GetComponentInChildren<TurtleMiddle>();
        this.bottom.onAttackPlayer = () =>
        {
            Debug.Log("Player АјАн!");
            this.onAttackPlayer();
        };
    }
}
