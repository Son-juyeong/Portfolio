using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Floating : MonoBehaviour
{
    [SerializeField]
    private Transform[] arrFloatingTransform;
    public float floatingPow = 1f;
    public float waterDrag;
    public float waterAngluarDrag;
    public float airDrag;
    public float airAngluarDrag;
    private bool isUnderWater;
    private int underWaterFloatingCount;
    private Rigidbody rBody;
    void Start()
    {
        this.rBody = GetComponent<Rigidbody>();
        this.rBody.drag = this.airDrag;
        this.rBody.angularDrag = this.airAngluarDrag;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < arrFloatingTransform.Length; i++) {
            float distance = this.arrFloatingTransform[i].position.y - WaveManager.Instance.GetWaveHeight(this.arrFloatingTransform[i].position);
            //Debug.Log(distance);
            if (distance < 0)
            {
                this.rBody.AddForceAtPosition(this.arrFloatingTransform[i].up* floatingPow * Mathf.Abs(distance), this.arrFloatingTransform[i].position, ForceMode.Force);
                underWaterFloatingCount++;
                if (!isUnderWater)
                {
                    this.isUnderWater = true;
                    SwitchDrag();
                }
            } 
        }
        if(isUnderWater&& underWaterFloatingCount==0)
        {
            this.isUnderWater = false;
            SwitchDrag();
        }
        underWaterFloatingCount = 0;
    }

    private void SwitchDrag()
    {
        if(this.isUnderWater)
        {
            this.rBody.drag = this.waterDrag;
            this.rBody.angularDrag = this.waterAngluarDrag;
        }
        else
        {
            this.rBody.drag = this.airDrag;
            this.rBody.angularDrag= this.airAngluarDrag;
        }
    }
}
