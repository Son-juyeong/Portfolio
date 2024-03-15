using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandController : MonoBehaviour
{
    [SerializeField] GameObject originalLeftHandGo;
    [SerializeField] GameObject grabLeftHandGo;
    [SerializeField] Transform itemTrans;
    [SerializeField] AudioSource audioSourceShiled;
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip ShieldActive;
    [SerializeField] AudioClip healSound;
    private GameObject grabGo;

    private int count = 0;

    public enum eState
    {// 상태 정의
        IDLE, GRAB, DEFENSE, ATTACK
    }
    public eState state = eState.IDLE;//Player의 현재 상태
    public System.Action<GameObject> OnHeal;

    // ------------------------------------------------LJE Shiled-------------------------------------

    [SerializeField] private GameObject leftHandShield;
    [SerializeField] private Shield shield;
    public System.Action getUp;
    public System.Action<int> onChangeValue;
    //private int shieldMaxValue = 100;
    private int shieldMaxValue;
    private int shieldValue;
    private float elapsedTime;
    private float timeSpeed = 7f;
    private float recoverSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        this.shieldMaxValue = InfoManager.Instance.GetPlayerInfo().shieldGauge;
        this.shieldValue = this.shieldMaxValue;
        this.onChangeValue(this.shieldValue);

        this.shield.onTriggerBullet = () =>
        {
            this.shieldValue -= 5;
            Debug.Log(this.shieldValue);
            this.onChangeValue(this.shieldValue);
        };
    }

    // Update is called once per frame
    void Update()
    {
        //if (this.leftHandShield.activeSelf)
        //{
        //    Debug.Log("Shield Activated");
        //    int count = 0;

        //    var audiosource = this.audioSourceShiled.GetComponent<AudioSource>();
        //    if (count == 0)
        //    {
        //        audiosource.PlayOneShot(this.ShieldActive, 1.0f);
        //        count = 1;
        //    }
                
        //}
     
        if (this.grabGo != null)
        {
            Animator anim = this.grabGo.GetComponent<Animator>();
            GameObject go = this.grabGo.GetComponentInChildren<ParticleSystem>(true)?.gameObject;



            if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) && this.grabGo != null)
            {
                this.state = eState.GRAB;
                this.originalLeftHandGo.SetActive(false);
                this.grabLeftHandGo.SetActive(true);
                if (this.grabGo.CompareTag("Item"))
                {
                    anim.SetInteger("State", 1);
                    go.SetActive(true);
                }

                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
                {//while grabbing, if pushed left index trigger
                    Debug.Log("left Hand Index Trigger");
                   
                    if (this.grabGo.CompareTag("Item"))
                    {
                        this.grabGo.SetActive(false);
                        this.audio.PlayOneShot(this.healSound, 1.0f);
                        this.grabGo.transform.position = this.itemTrans.position;
                        this.OnHeal(this.grabGo);
                       
                    }
                }
            }
            else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
            {
                this.state = eState.IDLE;
                this.originalLeftHandGo.SetActive(true);
                this.grabLeftHandGo.SetActive(false);
                if (this.grabGo.CompareTag("Item"))
                {
                    anim.SetInteger("State", 0);
                    go.SetActive(false);//off particle effect
                   
                }
                this.grabGo = null;
            }
        }

        
        // ------------------------------------------------LJE Shiled-------------------------------------
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && this.state != eState.GRAB)
        {
            this.state = eState.DEFENSE;
            this.originalLeftHandGo.SetActive(false);
            this.grabLeftHandGo.SetActive(false);
            this.leftHandShield.SetActive(true);

            if (this.count == 0)
            {
                this.audioSourceShiled.PlayOneShot(this.ShieldActive, 1.2f);
                this.count = 1;
            }
            this.elapsedTime += (Time.deltaTime * this.timeSpeed);
            if (this.elapsedTime >= 1)
            {
                this.shieldValue--;
                this.onChangeValue(this.shieldValue);
                this.elapsedTime = 0f;
            }
            if (this.shieldValue <= 0f)
            {
                this.leftHandShield.SetActive(false);
                this.originalLeftHandGo.SetActive(true);
                this.shieldValue = 0;
                this.onChangeValue(this.shieldValue);
            }
        }
        else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            this.state = eState.IDLE;
            this.originalLeftHandGo.SetActive(true);
            this.leftHandShield.SetActive(false);
            this.elapsedTime = 0;
            this.onChangeValue(this.shieldValue);
            this.count = 0;
        }
        else
        {
            this.elapsedTime += (Time.deltaTime * this.recoverSpeed);
            if (this.elapsedTime >= 1)
            {
                this.shieldValue++;
                this.onChangeValue(this.shieldValue);
                this.elapsedTime = 0f;

                if (this.shieldValue > this.shieldMaxValue)
                {
                    this.shieldValue = this.shieldMaxValue;
                    this.onChangeValue(this.shieldValue);
                }
            }
        }
        //if(this.state != eState.GRAB)
        //{
        //    this.getUp();
        //}

    }

    private void OnTriggerEnter(Collider other)
    {
        this.grabGo = other.gameObject;

    }
}