using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenBrock : MonoBehaviour
{
    private Floor floor;
    public System.Action onNotePlayerReach;
    public void Init()
    {
        this.floor = GetComponentInChildren<Floor>();
        this.floor.onCollision = () =>
        {
            Debug.Log("Player reach ground");
            this.onNotePlayerReach();
        };
    }
}
