using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;

    public class ShieldUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text shieldGauge;
        [SerializeField] private LeftHandController leftShield;
        [SerializeField] private Transform uiTransform;
       
    private Canvas parentCanvas;

        void Start()
        {
        this.parentCanvas = this.gameObject.GetComponentInParent<Canvas>();
        this.parentCanvas.gameObject.transform.SetParent(this.uiTransform);
        this.parentCanvas.transform.localPosition = Vector3.zero;
        this.parentCanvas.transform.localRotation = Quaternion.identity;

            //this.gameObject.SetActive(false);

            this.leftShield.getUp = () =>
            {
                this.gameObject.SetActive(false);
            };

            this.leftShield.onChangeValue = (value) =>
            {
                
                this.shieldGauge.text = string.Format("{0}%", value);
            };
        }

        // Update is called once per frame
        void Update()
        { this.parentCanvas.transform.rotation = Quaternion.identity;//fix Canvas' rotation
            //this.transform.position = this.leftShield.transform.position + Vector3.right * 0.1f;

        }
    }
