using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnergyCellController : MonoBehaviour
{
    public Transform energyCellPoint;
    public Transform energyCellOriginalPoint;
    public System.Action OnCharge;
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip chargeSound;
    private bool isCharge;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnGrab()
    {
        Debug.Log("Grab");

    }


    public void OnUnGrab()
    {
       // Debug.Log("unGrab");
        if (this.isCharge)
        {
            this.OnCharge();
            this.transform.SetParent(this.energyCellPoint);
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.Euler(0, 0, 90);
            this.isCharge = false;//reset charge state
        }
        else
        {
           
            this.transform.position = this.energyCellOriginalPoint.position;
            this.transform.localRotation = Quaternion.identity;

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ChargeArea"))
        {
            // Debug.Log("Charge Area");
            this.isCharge = true;
            this.audio.PlayOneShot(this.chargeSound, 1.0f);
            //this.OnCharge();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ChargeArea"))
        {
            // Debug.Log("Exit Area");
            this.isCharge = false;

        }

    }
}
