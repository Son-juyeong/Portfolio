using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    [SerializeField]
    private Image gaugeImg;

    // Start is called before the first frame update
    void Start()
    {
        gaugeImg.fillAmount = 0;
    }

    public void UpdateGauge(float time, float maxTime)
    {
        this.gaugeImg.fillAmount = time/maxTime;
    }
}
