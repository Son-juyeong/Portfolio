using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardObject : MonoBehaviour
{
    private bool isSelected;
    public float rotSpeed;

    // Update is called once per frame
    void Update()
    {
        if(!isSelected)
            this.transform.Rotate(Vector3.up* rotSpeed * Time.deltaTime, Space.World);
    }

    public void OnWhenSelected()
    {
        this.isSelected = true;
    }

    public void OnWhenUnselected()
    {
        this.isSelected=false;
    }
}
