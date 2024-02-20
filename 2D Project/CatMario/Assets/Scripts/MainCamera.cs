using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private float maxPosX;
    public void Init(float maxPosX)
    {
        this.maxPosX = maxPosX;
        this.transform.position = new Vector3(0, 0, this.transform.position.z);
        Debug.LogFormat("camera init!({0})", this.transform.position);
    }

    public void Move(float playerPosX)
    {
        if(playerPosX>=this.maxPosX)
            return;
        this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, playerPosX, this.maxPosX), this.transform.position.y, this.transform.position.z); 
    }
}
