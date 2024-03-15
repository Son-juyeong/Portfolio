using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class RightHandController : MonoBehaviour
    {
        public System.Action<Vector3, GameObject> OnHitEnemy;
        public System.Action OnShoot;
        public System.Action shoot;
        public System.Action OnChangeSecondGun;
        
        [SerializeField] Transform shootTrans;
        [SerializeField] Transform shootDistance;
        [SerializeField] GameObject gunLaserGo;
        [SerializeField] GameObject impactEffectGo;
        [SerializeField] GameObject gunGo;
        [SerializeField] Material[] mat;
        [SerializeField] AudioClip shootStart;
        [SerializeField] AudioClip shootImpact;
        [SerializeField] AudioClip shootStart2;
        [SerializeField] private AudioSource audioSourceShootstart;
        [SerializeField] private AudioSource audioSourceImpact;
        public GameObject gunLaserBeamGo;
        private GameObject laserImpactGo;
        private int count = 0;
        

        public bool isAttack = false;
        public bool isSecondGun = false;
        public float radius=0.1f;

    // --------------------------------------Second Gun LJE------------------------------------------
        [SerializeField] private GameObject shootEffectGo;
        [SerializeField] private Transform effectTrans;
        [SerializeField] private GameObject fireMuzzleEffect;


    // Start is called before the first frame update
    void Start()
        {
            //this.audioSource = GetComponent<AudioSource>();
            this.gunLaserBeamGo = this.gunLaserGo;
            this.laserImpactGo = Instantiate<GameObject>(this.impactEffectGo);
            this.gunLaserBeamGo.SetActive(false);
            this.laserImpactGo.SetActive(false);


        // --------------------------------------Second Gun LJE------------------------------------------
            this.shootEffectGo = Instantiate<GameObject>(shootEffectGo);
            this.shootEffectGo.SetActive(false);
            this.fireMuzzleEffect = Instantiate<GameObject>(fireMuzzleEffect);
            this.fireMuzzleEffect.SetActive(false);

    }

        // Update is called once per frame
        void Update()
        {
       
        if (this.isSecondGun && count==0)
        {
           // Debug.Log("Gun Changed. Second Gun is Activated");
            var mat = this.gunGo.GetComponentInChildren<MeshRenderer>();
            mat.material = this.mat[0];
            this.OnChangeSecondGun();
            count=1;
        }

        if (isAttack)
        {
            this.gunLaserBeamGo.transform.position = this.shootTrans.position;

            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                //Debug.Log("right Hand Index Trigger");
                if (this.isSecondGun)
                {
                    Debug.Log("Gun Changed. Second Gun is Activated");
                    //var mat = this.gunGo.GetComponentInChildren<MeshRenderer>();
                    //mat.material = this.mat[0];
                    this.StartCoroutine(CoShootEffect());
                    this.StartCoroutine(CoFireEffect());
                    this.shoot();
                }
                else
                {
                    StartCoroutine(this.CoLaserBeam());
                    StartCoroutine(this.CoCheckImpact());
                }
               

                this.OnShoot();//use gun Energy
            }



        }
        }


        private IEnumerator CoLaserBeam()
        {
            //this.gunLaserBeamGo.transform.position = this.shootTrans.position;
            this.gunLaserBeamGo.SetActive(true);
           // this.audioSource.pitch = 1.5f;
            this.audioSourceShootstart.PlayOneShot(this.shootStart, 1.2f);
            this.gunLaserBeamGo.transform.LookAt(this.shootDistance.position);//hit nothing



            yield return new WaitForSeconds(0.2f);
            this.gunLaserBeamGo.SetActive(false);
            this.laserImpactGo.SetActive(false);
            
    }
        private IEnumerator CoCheckImpact()
        {
            //-----------------------------check impact---------------------------------------


            Ray ray = new Ray(this.shootTrans.position, this.gunLaserBeamGo.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * 50f, Color.red, 0.3f);
            var layerMask = 3 << LayerMask.NameToLayer("Monster");
            RaycastHit hit;

            if (Physics.SphereCast(ray.origin, this.radius,ray.direction, out hit, 50.0f))
            {
            // Debug.Log("Hit Monster!!");
            if (!hit.collider.gameObject.CompareTag("Player")&& !hit.collider.gameObject.CompareTag("ChargeArea") && !hit.collider.gameObject.CompareTag("Vehicle") && !hit.collider.gameObject.CompareTag("EnergyCell"))
                //if (!hit.collider.gameObject.CompareTag("Vehicle")&& !hit.collider.gameObject.CompareTag("Player"))
                { this.CreateImpactEffect(hit.point);
                this.audioSourceShootstart.PlayOneShot(this.shootImpact, 1.5f);
            }

                if (hit.collider.gameObject.CompareTag("Enemy1")|| hit.collider.gameObject.CompareTag("Enemy2")|| hit.collider.gameObject.CompareTag("Enemy3"))
                {
                    Debug.Log("Enemy");
                    this.OnHitEnemy(hit.point, hit.collider.gameObject);
                }
            //------------------------------------------------Zeppelin------------------------------------------------------------
            if (hit.collider.CompareTag("Zeppelin"))
            {
                var zeppelinCtrl = hit.collider.gameObject.GetComponentInParent<ZeppelinController>();
                zeppelinCtrl.Hit();
            }
            //--------------------------------------------------------------------------------------------------------------------

            var particleSys = this.gunLaserBeamGo.GetComponent<ParticleSystemRenderer>();
                particleSys.lengthScale = hit.distance;
            }
        else
        {
            var particleSys = this.gunLaserBeamGo.GetComponent<ParticleSystemRenderer>();
            particleSys.lengthScale = 12f;
        }
            //-----------------------------------------------------------------------------------
            yield return null;
        }
        private void CreateImpactEffect(Vector3 pos)
        {
            this.laserImpactGo.transform.position = pos;
            this.laserImpactGo.SetActive(true);
        }
    IEnumerator CoShootEffect()
    {
        this.audioSourceShootstart.PlayOneShot(this.shootStart2,1.0f);

        this.shootEffectGo.transform.position = this.effectTrans.position;
        this.shootEffectGo.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        this.shootEffectGo.SetActive(false);
    }

    IEnumerator CoFireEffect()
    {
        this.fireMuzzleEffect.transform.position = this.effectTrans.position;
        this.fireMuzzleEffect.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        this.fireMuzzleEffect.SetActive(false);
    }
}

