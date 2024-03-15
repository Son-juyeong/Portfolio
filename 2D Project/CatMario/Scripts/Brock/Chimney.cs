using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chimney : MonoBehaviour
{
    private Floor floor;
    public System.Action onNotePlayerReach;
    public void Init()
    {
        this.floor=this.GetComponentInChildren<Floor>();
        this.floor.onCollision = () =>
        {
            Debug.Log("Player is on the brock");
            this.onNotePlayerReach();
        };
    }
}
