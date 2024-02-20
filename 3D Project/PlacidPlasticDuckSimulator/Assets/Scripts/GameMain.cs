using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    [SerializeField]
    private GameObject[] duckPrefabs;
    [SerializeField]
    private UIMain uiMain;
    private float elapsedTime;
    private float gaugeTime;
    private int idx;
    void Start()
    {
        Instantiate(duckPrefabs[idx++]);
        this.gaugeTime = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > this.gaugeTime && idx<duckPrefabs.Length)
        {
            Instantiate(duckPrefabs[idx++]);
            elapsedTime = 0;
        }
        this.uiMain.UpdateGauge(this.elapsedTime, this.gaugeTime);
    }
}
