using LJE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private Animator anim;
    public System.Action onTriggerBullet;
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip shieldBulletSound;

    void Start()
    {
        this.anim = this.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet")) return;

        if (other.CompareTag("Bullet"))
        {
            Debug.LogFormat("<color=blue>ShieldBullet: {0}</color>", other.name);
            StartCoroutine(CoShieldAnim());
            StartCoroutine(CoControllerVibe(0.1f, 0.1f, OVRInput.Controller.LTouch));
            this.onTriggerBullet();
            //Debug.LogFormat("<color=yellow>{0}</color>", other);
            Destroy(other.gameObject);
            InfoManager.Instance.blockedBullet++;
            this.audio.PlayOneShot(this.shieldBulletSound, 1.0f);
        }
    }

    private IEnumerator CoShieldAnim()
    {
        this.anim.SetBool("isShield", true);
        Debug.Log("<color=green>AnimationDone</color>");
        yield return new WaitForSeconds(0.16f);
        this.anim.SetBool("isShield", false);
    }

    private IEnumerator CoControllerVibe(float frequency, float amplitude, OVRInput.Controller controllermask)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, controllermask);
        yield return new WaitForSeconds(0.15f);
        OVRInput.SetControllerVibration(0, 0, controllermask);
    }
}
