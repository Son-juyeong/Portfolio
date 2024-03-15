using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.CompilerServices;

public class GameMain : MonoBehaviour
{
    [SerializeField] Canvas worldCanvas;
    [SerializeField] Canvas worldCanvas2;
    [SerializeField] Canvas rightHandCanvas;
    [SerializeField] TextMeshProUGUI guideText;
    [SerializeField] GameObject uiHPBarPrefab;
    [SerializeField] GameObject uiDamageTextPrefab;
    [SerializeField] GameObject uiEnergyCellPrefab;
    [SerializeField] GameObject gunEnergyCellGo;
    [SerializeField] GameObject uiPlayerHpTextGo;
    [SerializeField] GameObject uiPlayerHpSliderGo;
    [SerializeField] GameObject uiPlayerScoreTextGo;
    [SerializeField] GameObject playerDieGo;
    [SerializeField] GameObject originalBgmGo;
    [SerializeField] GameObject dieBgmGo;
    [SerializeField] GameObject clearBgmGo;

    [SerializeField] EnergyCellController energyCellForCharge;
    [SerializeField] RightHandController rightHand;
    [SerializeField] LeftHandController leftHand;
    [SerializeField] Transform uiEnergyCellTransform;
    [SerializeField] Transform worldUITransform;

    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip waveChangeSound;
    [SerializeField] AudioClip monsterDieSound;
    public bool isFirstHP = false;

    private bool isDamageUp = false;
    private int addValue;
    private int isDamageCount = 0;

    private bool isMaxHpUp = false;
    private int isMaxHpCount = 0;

    private bool isAutoCharging1 = false;
    private int autoChargeValue;

    private bool isAutoCharging2 = false;

    public bool isScopeExpand = false;

    private GameObject hpBarUIGo;
    private GameObject damageTextUIGo;
    private GameObject energyTextUIGo;
    private PlayDamageTextAnim playDamageAnim;
    private Action<Enemy1Move> OnGenerateEnemy;

    private int currPlayerMaxHp;
    private int currPlayerHp;
    private int currScore;
    private int currShiledGauge;
    private int currGunId;

    private string currGunType;
    private int currGunDamage;
    private int currGunEnergy;
    private int currGunMaxEnergy;

    private List<GameObject> enemyHpBarPools = new List<GameObject>();
    private List<GameObject> activeHPBar = new List<GameObject>();


    //----------------------------------SJY Map Main-----------------------------------------------------------
    [SerializeField] private MapController mapController;
    [SerializeField] private CamMove camMove;


    //----------------------------------SJY Enemy1Move Main-----------------------------------------------------------
    public enum eWave
    {
        WAVE1, WAVE2, WAVE3
    }
    private eWave wave;

    [SerializeField] private Transform[] arrTargetPos;
    private List<Enemy1Move> listEnemies = new List<Enemy1Move>();
    private List<bool> isTargeted = new List<bool>();
    private int EnemyCount = 2;

    private int[] maxWaveEnemyCount = new int[3] { 3, 6, 9 };
    private int remainEnemyCount;
    //----------------------------------SJY EnemyAttack-----------------------------------------------------------
    private float enemyAttackCoolTime = 4f;
    private float enemyAttackelapsedTime;
    private bool isEnemyAttack;
    private int enemyAttackIdx = 0;


    //----------------------------------SJY Reward-----------------------------------------------------------------
    [SerializeField] private RewardGroupController rewardGroupCtrl;
    [SerializeField] private GameObject waveCompleteEffectPrefab;
    [SerializeField] private GameObject waveCompletePhrasePrefab;
    private RewardData grabRewardData;
    private bool isGrabReward;
    public float maxUsingRewardTime = 3f;
    private float usingRewardElapsedTime;
    private Dictionary<int, int> dicGetRewards = new Dictionary<int, int>();
    //--------------------------------------------SJY Zeppelin-----------------------------------------------------
    [SerializeField] private point point;

    //--------------------------------------------SJY Data&Info----------------------------------------------------------
    private PlayerInfo playerInfo;
    private int healthCanAmount;
    private bool isReloadStart;
    private bool isPlayerDie;

    //----------------------------------------playerHpAnim------------------------------------------------------
    [SerializeField] private GameObject playerHpUiGo;
    //private Animator playerHpAnim;


    private void Awake()
    {
        DataManager.Instance.LoadPlayerDatas();
        DataManager.Instance.LoadGunDatas();
        DataManager.Instance.LoadEnemyDatas();
        InfoManager.Instance.LoadInfos();
    }
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Start");

        this.guideText.gameObject.SetActive(false);
        this.worldCanvas.transform.SetParent(this.worldUITransform);
        this.worldCanvas.transform.localPosition = Vector3.zero;
        this.worldCanvas.transform.localRotation = Quaternion.identity;

        this.energyTextUIGo = this.uiEnergyCellPrefab;
        this.rightHandCanvas.transform.SetParent(this.uiEnergyCellTransform);
        this.rightHandCanvas.transform.localPosition = Vector3.zero;
        this.rightHandCanvas.transform.localRotation = Quaternion.identity;


        List<PlayerData> playerData = DataManager.Instance.GetPlayerDatas();
        List<GunData> gunData = DataManager.Instance.GetGunDatas();
        List<EnemyData> enemyData = DataManager.Instance.GetEnemyDatas();

        Debug.LogFormat("player Data Count: {0}", playerData.Count);
        this.playerInfo = InfoManager.Instance.GetPlayerInfo();
        for (int i = 0; i < playerData.Count; i++)
        {//get player data
            if (playerData[i].playerUserName == "Unknown")
            {
                Debug.Log("default user");
                //this.currPlayerMaxHp = playerData[i].playerHp;
                this.currPlayerMaxHp = this.playerInfo.maxHp;
                //this.currPlayerHp = playerData[i].playerHp;
                this.currPlayerHp = this.currPlayerMaxHp;
                this.currScore = playerData[i].score;
                //this.currShiledGauge = playerData[i].shieldGauge;
                this.currShiledGauge = playerInfo.shieldGauge;
                this.currGunId = playerData[i].currGunId;
            }
        }
        this.healthCanAmount = this.playerInfo.healthCanAmount;

        //this.uiPlayerHpSliderGo.GetComponent<Slider>().maxValue = this.currPlayerHp;
        this.uiPlayerHpSliderGo.GetComponent<Slider>().maxValue = this.currPlayerMaxHp;

        Debug.LogFormat("Gun Data Count: {0}", gunData.Count);
        //for (int i = 0; i < gunData.Count; i++)
        //{
        //    if (this.currGunId == gunData[i].gunId)
        //    {//Find player's current gun's Data on GunData 
        //        Debug.Log(gunData[i].gunType);
        //        this.currGunType = gunData[i].gunType;
        //        this.currGunDamage = gunData[i].gunDamage;
        //        this.currGunEnergy = gunData[i].gunEnergyCell;
        //        this.currGunMaxEnergy = gunData[i].gunEnergyCell;

        //    }
        //}
        var currGunData = DataManager.Instance.GetGunData(this.currGunId);
        this.currGunType = currGunData.gunType;
        this.currGunDamage = InfoManager.Instance.GetGunInfo(this.currGunId).damage;
        this.currGunMaxEnergy = currGunData.gunEnergyCell;

        this.rightHand.OnChangeSecondGun = () =>
        {//change gun data

            //for (int i = 0; i < gunData.Count; i++)
            //{
            //    if (gunData[i].gunId == 1001)
            //    {//Find player's current gun's Data on GunData 
            //        Debug.Log(gunData[i].gunType);
            //        this.currGunType = gunData[i].gunType;
            //        this.currGunDamage = gunData[i].gunDamage;
            //        this.currGunEnergy = gunData[i].gunEnergyCell;
            //        this.currGunMaxEnergy = gunData[i].gunEnergyCell;

            //    }
            //}
            this.currGunId = 1001;
            var currGunData = DataManager.Instance.GetGunData(this.currGunId);
            this.currGunType = currGunData.gunType;
            this.currGunDamage = InfoManager.Instance.GetGunInfo(this.currGunId).damage;
            this.currGunMaxEnergy = currGunData.gunEnergyCell;
            this.currGunEnergy = this.currGunMaxEnergy;
        };
        this.enemyHpBarPool();





        this.damageTextUIGo = Instantiate(this.uiDamageTextPrefab, this.worldCanvas.transform);
        this.damageTextUIGo.SetActive(false);
        this.playDamageAnim = this.damageTextUIGo.GetComponent<PlayDamageTextAnim>();





        this.OnGenerateEnemy = (enemy) =>
        {
            var go = this.CreateHpBar(enemy.hpBarPoint.position);
            this.activeHPBar.Add(go);
            for (int j = 0; j < enemyData.Count; j++)
            {
                if (enemy.tag == enemyData[j].enemyType)
                {

                    Debug.Log(enemyData[j].enemyType);
                    var slider = go.GetComponent<Slider>();
                    slider.maxValue = enemyData[j].enemyHp;
                    Debug.Log(slider.maxValue);
                    slider.value = slider.maxValue;//reset slider value as full state
                                                   //this.listEnemies[j].currHP = (int)slider.value; //set EnemyHp
                                                   //Debug.Log(listEnemies[j].gameObject);
                    enemy.OnGetData(enemyData[j]);

                }


            }


        };
        this.rightHand.OnHitEnemy = (hitPos, hitObject) =>
            {
                // Debug.LogFormat("Hit Enemy! Point: {0}", hitPos);
                StartCoroutine(this.CoShowDamageText(hitPos));
                //this.playDamageAnim.PlayAnim();
                for (int i = 0; i < this.listEnemies.Count; i++)
                {
                    if (hitObject == this.listEnemies[i].gameObject)
                    {
                        Debug.LogFormat("Object: {0} , Damage : {1}", i, currGunDamage);
                        this.activeHPBar[i].GetComponent<Slider>().value -= currGunDamage;
                        InfoManager.Instance.totalDamage += currGunDamage;  //GAMEOVER에 데이터 보내기

                        hitObject.GetComponent<Enemy1Move>().OnGetHit(currGunDamage);
                        if (this.activeHPBar[i].GetComponent<Slider>().value <= 0)
                        {
                            this.activeHPBar[i].SetActive(false);
                            this.activeHPBar.Remove(this.activeHPBar[i]);
                        }
                    }
                }

            };

        this.rightHand.OnShoot = () =>
        {
            this.currGunEnergy -= 1;
        };

        this.energyCellForCharge.OnCharge = () =>
        {
            this.currGunEnergy = this.currGunMaxEnergy;//reset energyCell as full state
            this.gunEnergyCellGo.SetActive(true);
            StartCoroutine(this.CoResetEnergyCellItem());
            if (!isReloadStart)
            {
                this.isReloadStart = true;
                this.guideText.gameObject.SetActive(false);
                this.StartCoroutine(this.CoStartSpawnEnemy1());
            }
        };

        this.leftHand.OnHeal = (grabObject) =>
        {
            Debug.Log("Heal");
            //this.currPlayerHp += 20;
            this.currPlayerHp += this.playerInfo.healthCanValue;
            this.healthCanAmount--;
            if (healthCanAmount > 0)
            {
                StartCoroutine(this.CoRespawnItem(grabObject));
            }
        };
        //----------------------------------SJY Map Main-----------------------------------------------------------

        Vector3 camPos = Vector3.zero;
        this.mapController.onInformInitPos = (pos) =>
        {
            camPos = pos;
        };
        this.mapController.onInformRoute = (rail) =>
        {
            this.camMove.UpdateRoute(rail.GetRoute());
        };
        this.mapController.Init();
        this.camMove.Init(camPos);

        //----------------------------------SJY Enemy1Move Main-----------------------------------------------------------
        // EnemyPool.instance.LoadAll();

        for (int i = 0; i < arrTargetPos.Length; i++)
        {
            isTargeted.Add(false);
        }
        //this.StartCoroutine(this.CoStartSpawnEnemy1());
        string textStartGuide = "시작하려면 탄창을 충전이 필요합니다.\n탄창을 그랩하여 총에 가져다 대세요.";
        this.GuideGame(textStartGuide);

        //----------------------------------SJY Reward---------------------------------------------------------------------
        DataManager.Instance.LoadRewardDatas();
        this.rewardGroupCtrl.onWhenSelect = (rewardData) =>
        {
            //Debug.LogFormat("<color=yellow>grab reward: {0}</color>", rewardData.name);
            this.grabRewardData = rewardData;
            this.isGrabReward = true;
        };
        this.rewardGroupCtrl.onWhenUnselect = () =>
        {
            //Debug.LogFormat("unhand reward");
            this.grabRewardData = null;
            this.isGrabReward = false;
        };

        this.rewardGroupCtrl.Init();


        //----------------------------------------SJY Zeppelin-------------------------------------------------------
        this.currScore = 0;
        this.uiPlayerScoreTextGo.GetComponent<TMP_Text>().text = this.currScore.ToString();

        this.point.itemTrigger = () =>
        {
            this.currScore += 10;
            this.uiPlayerScoreTextGo.GetComponent<TMP_Text>().text = this.currScore.ToString();
        };
    }

    private IEnumerator CoRespawnItem(GameObject go)
    {
        yield return new WaitForSeconds(5f);
        go.SetActive(true);
    }
    private IEnumerator CoResetEnergyCellItem()
    {
        this.energyCellForCharge.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        // Debug.Log("Rest Energy Item");
        this.energyCellForCharge.transform.SetParent(this.energyCellForCharge.energyCellOriginalPoint);
        this.energyCellForCharge.transform.localPosition = Vector3.zero;
        this.energyCellForCharge.transform.localRotation = Quaternion.identity;
        this.energyCellForCharge.gameObject.SetActive(true);
    }
    private void enemyHpBarPool()
    {

        for (int i = 0; i < this.EnemyCount; i++)
        {
            GameObject go = Instantiate(this.uiHPBarPrefab, this.worldCanvas2.transform);
            go.SetActive(false);
            this.enemyHpBarPools.Add(go);
        }
    }
    private GameObject GetEnemyHpBarInPool()
    {
        foreach (GameObject hpBar in enemyHpBarPools)
        {
            if (hpBar.activeSelf == false)
            {
                return hpBar;
            }
        }
        return null;
    }
    private GameObject CreateHpBar(Vector3 position)
    {
        GameObject go = this.GetEnemyHpBarInPool();
        go.transform.position = position;
        go.SetActive(true);
        return go;
    }

    private IEnumerator CoShowDamageText(Vector3 UIPos)
    {
        this.damageTextUIGo.transform.position = UIPos;
        this.damageTextUIGo.SetActive(true);

        var count = 0;
        while (true)
        {
            var pos = this.damageTextUIGo.transform.position;
            var pos2 = new Vector3(pos.x, pos.y + 0.5f, pos.z);
            pos = Vector3.Lerp(pos, pos2, 0.1f);
            this.damageTextUIGo.transform.position = pos;
            yield return null;
            count++;
            if (count > 15)
            {
                break;
            }
        }
        var anim = this.damageTextUIGo.GetComponent<Animator>();
        anim.SetInteger("State", 1);
        var text = this.damageTextUIGo.GetComponent<TextMeshProUGUI>();
        text.text = string.Format("{0}", this.currGunDamage);
        yield return new WaitForSeconds(0.3f);
       anim.SetInteger("State", 0);
        this.damageTextUIGo.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (this.isPlayerDie) return;
        if (this.currPlayerHp <= 0)
        {
            this.originalBgmGo.SetActive(false);
            this.dieBgmGo.SetActive(true);
            this.dieBgmGo.GetComponent<AudioSource>().volume = 1.5f;
            this.currPlayerHp = 0;
            this.playerInfo.scrap += this.currScore;
            this.playerInfo.maxScore = Mathf.Max(this.currScore, this.playerInfo.maxScore);
            this.isPlayerDie = true;
            this.playerDieGo.GetComponent<PlayerDie>().PlayerDeath();
            this.uiHPBarPrefab.SetActive(false);
        }

        if (this.currPlayerHp >= this.currPlayerMaxHp)
        {
            this.currPlayerHp = this.currPlayerMaxHp;
        }
        if (this.isDamageUp && isDamageCount == 0)
        {
            this.currGunDamage += this.addValue;
            isDamageCount = 1;
        }
        if (this.isMaxHpUp && isMaxHpCount == 0)
        {
            this.currPlayerMaxHp += this.addValue;
            this.uiPlayerHpSliderGo.GetComponent<Slider>().maxValue = this.currPlayerMaxHp;
            isMaxHpCount = 1;
        }

        if (this.isScopeExpand)
        {
            var psr = this.rightHand.gunLaserBeamGo.GetComponent<ParticleSystem>().main;
            psr.startSizeMultiplier = 0.6f;
            this.rightHand.radius = 1f;
        }

        //enemy 공격 구현=======================================================================================
        if (this.listEnemies.Count > 0 && !isEnemyAttack)
        {
            this.enemyAttackelapsedTime += Time.deltaTime;
            if (this.enemyAttackelapsedTime > this.enemyAttackCoolTime)
            {
                this.listEnemies[(enemyAttackIdx++ % this.listEnemies.Count)].AttackPlayer();
                this.isEnemyAttack = true;
            }
        }
        else
        {
            this.enemyAttackelapsedTime = 0f;
            this.isEnemyAttack = false;
        }
        //=======================================================================================
        //-------------------------------------SJY Reward------------------------------------------------------
        if (isGrabReward)
        {
            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
            {
                this.usingRewardElapsedTime += Time.deltaTime;
                if (this.usingRewardElapsedTime > maxUsingRewardTime)
                {
                    this.UseReward();
                }
            }
            else
            {
                this.usingRewardElapsedTime = 0f;
            }
        }
        //------------------------------------------------------------------------------------------------------

        if (activeHPBar.Count != 0)
        {
            for (int i = 0; i < this.listEnemies.Count; i++)
            {
                //Debug.LogFormat("{0}, {1}", this.activeHPBar[i], this.listEnemies[i]);
                this.activeHPBar[i].transform.position = this.listEnemies[i].hpBarPoint.position;

            }
        }
        //change energyCell UI's text
        this.energyTextUIGo.GetComponent<TextMeshProUGUI>().text = string.Format("{0}", this.currGunEnergy);

        //change player HP UI
        this.uiPlayerHpTextGo.GetComponent<TextMeshProUGUI>().text = string.Format("{0}", this.currPlayerHp);
        this.uiPlayerHpSliderGo.GetComponent<Slider>().value = this.currPlayerHp;
        //change player's score
        this.uiPlayerScoreTextGo.GetComponent<TextMeshProUGUI>().text = string.Format("{0}", this.currScore);
        InfoManager.Instance.score = this.currScore; //GAMEOVER 데이터 보내기

        this.rightHand.isAttack = true;
        if (this.currGunEnergy <= 0)
        {
            if (this.isAutoCharging2)
            {
                this.currGunEnergy = this.currGunMaxEnergy;
            }
            this.gunEnergyCellGo.SetActive(false);
            this.rightHand.isAttack = false;
        }

        if (isFirstHP)
        {
            this.guideText.gameObject.SetActive(true);
            this.guideText.text = "물약을 잡아 HP를 회복하세요.";
        }


    }
    //Methods
    //----------------------------------SJY Enemy1Move Main-----------------------------------------------------------

    private IEnumerator CoStartSpawnEnemy1()
    {
        this.remainEnemyCount = maxWaveEnemyCount[(int)this.wave];
        for (int i = 0; i < this.EnemyCount; i++)
        {
            yield return new WaitForSeconds(5f);
            this.SpawnEnemy1();
        }
    }

    private void SpawnEnemy1()
    {
        float x = UnityEngine.Random.Range(-3f, 3f);
        float z = UnityEngine.Random.Range(10f, 15f);
        while (true)
        {
            int layer = 1 << 3 | 1 << 6 | 1 << 7;
            //Debug.Log(layer.ToBinaryString());
            //Debug.Log(enemy.gameObject.layer.ToString());
            Collider[] hit = new Collider[10];
            int num = Physics.OverlapSphereNonAlloc(this.arrTargetPos[2].position + Vector3.right * x
                + Vector3.forward * z, 2f, hit, layer);
            if (num == 0)
                break;
            x = UnityEngine.Random.Range(-3f, 3f);
            z = UnityEngine.Random.Range(10f, 20f);
        }

        Vector3 pos = this.arrTargetPos[2].position + Vector3.right * x + Vector3.forward * z;
        int idx = 0;
        int number = UnityEngine.Random.Range(0, 10);
        switch (this.wave)
        {
            case eWave.WAVE1:
                idx = (int)EnemyMove.eEnemyType.Enemy1;
                break;
            case eWave.WAVE2:
                if (number < 3)
                {
                    idx = (int)EnemyMove.eEnemyType.Enemy1;
                }
                else
                    idx = (int)EnemyMove.eEnemyType.Enemy2;
                break;
            case eWave.WAVE3:
                if (number < 2)
                {
                    idx = (int)EnemyMove.eEnemyType.Enemy1;
                }
                else if (number < 4)
                {
                    idx = (int)EnemyMove.eEnemyType.Enemy2;
                }
                else
                {
                    idx = (int)EnemyMove.eEnemyType.Enemy3;
                }
                break;
        }

        GameObject enemyGo = EnemyPoolManager.instance.EnableEnemy(idx);
        Enemy1Move enemy = enemyGo.GetComponent<Enemy1Move>();
        enemy.Init(pos);
        listEnemies.Add(enemy);
        enemy.onChangeTarget = (idx) =>
        {
            Debug.Log("target change!");
            Debug.LogFormat("<color=yellow>{0} preTargetPos: {1}</color>", enemy.name, arrTargetPos[idx]);
            this.SetTargetPos(enemy);
            this.isTargeted[idx] = false;
        };
        enemy.onDieEnemy = (enemy) =>
        {
            this.remainEnemyCount--;
            this.audio.PlayOneShot(this.monsterDieSound, 1.0f);
            listEnemies.Remove(enemy);
            //this.currScore += 10;
            this.currScore += InfoManager.Instance.GetEnemyInfo(enemy.enemyType.ToString()).scrap;
            if (this.isAutoCharging1)
            {
                this.currGunEnergy += this.autoChargeValue;
                //add
                this.currGunEnergy = Mathf.Clamp(this.currGunEnergy, 0, this.currGunMaxEnergy);
            }
            this.isTargeted[enemy.target.Key] = false;
            EnemyPoolManager.instance.DisableEnemy(enemy.gameObject);
            if (this.remainEnemyCount >= this.EnemyCount)
            {
                Invoke("SpawnEnemy1", 1f);
            }
            else
            {
                if (this.remainEnemyCount == 0)
                {
                    //Debug.LogFormat("{0} completed", this.wave.ToString());
                    if (this.wave == eWave.WAVE3) {
                        this.originalBgmGo.SetActive(false);
                        this.clearBgmGo.SetActive(true);
                        this.playerInfo.scrap += this.currScore;
                        this.playerInfo.maxScore = Mathf.Max(this.currScore, this.playerInfo.maxScore);
                        this.playerDieGo.GetComponent<PlayerDie>().PlayerWin();
                        return;
                    }
                    //this.wave++;
                    //this.StartCoroutine(CoStartSpawnEnemy1());

                    //this.rewardGroupCtrl.ShowRewards();
                    this.StartCoroutine(this.CoCompleteWave());
                }
            }
        };
        //enemy Attack 추가부분================================================================================================
        enemy.onAttackPlayer = () =>
        {
            this.StartCoroutine(CoPlayerHpAnim());
            int damage = DataManager.Instance.GetEnemyData(enemy.tag).enemyDamage;
            Debug.LogFormat("player get {0} damage / current Player's HP: {1}", damage, this.currPlayerHp);
            this.currPlayerHp -= damage;
          
        };

        enemy.onCompleteAttack = () =>
        {
            Debug.Log("enemy attack complete");
            this.isEnemyAttack = false;
            this.enemyAttackelapsedTime = 0f;
        };
        //====================================================================================================================


        this.SetTargetPos(enemy);
        this.OnGenerateEnemy(enemy);
    }

    private IEnumerator CoPlayerHpAnim()
    {
        var playerAnim = this.playerHpUiGo.GetComponent<Animator>();
        this.uiPlayerHpTextGo.GetComponent<TextMeshProUGUI>().color = Color.red;
        playerAnim.SetInteger("isAttack", 1);
        yield return new WaitForSeconds(0.16f);
        this.uiPlayerHpTextGo.GetComponent<TextMeshProUGUI>().color = Color.white;
        playerAnim.SetInteger("isAttack", 0);
    }

    private void SetTargetPos(Enemy1Move enemy)
    {
        List<int> listIndex = new List<int>();
        for (int i = 0; i < this.isTargeted.Count; i++)
        {
            if (!isTargeted[i])
            {
                listIndex.Add(i);
            }
        }
        int rand = UnityEngine.Random.Range(0, listIndex.Count);
        int idx = listIndex[rand];
        enemy.UpdateTargetPos(idx, this.arrTargetPos[idx]);
        this.isTargeted[idx] = true;
        Debug.LogFormat("<color=magenta>{0} curTargetPos: {1}</color>", enemy.name, arrTargetPos[idx]);
    }

    //---------------------------------------------SJY Reward-------------------------------------------------------

    private IEnumerator CoCompleteWave()
    {
        
        yield return new WaitForSeconds(1f);
        GameObject effectGo = Instantiate(this.waveCompleteEffectPrefab, this.arrTargetPos[2].position + Vector3.forward * 5f, Quaternion.identity);
        this.audio.PlayOneShot(this.waveChangeSound, 1.0f);
        yield return new WaitForSeconds(4f);
        GameObject phraseGo = Instantiate(this.waveCompletePhrasePrefab, effectGo.transform.position + Vector3.up * 1f, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        TMP_Text phrase = phraseGo.GetComponentInChildren<TMP_Text>();
        //Debug.LogFormat("<color=cyan>phrase: {0}</color>", phrase);
        while (true)
        {
            phrase.alpha -= 0.1f;
            yield return null;
            if (phrase.alpha <= 0)
                break;
        }
        //yield return new WaitForSeconds(7f);
        Destroy(effectGo);
        Destroy(phraseGo);
        yield return null;
        this.rewardGroupCtrl.ShowRewards();
    }
    private void IncreseGunDamage(float value)
    {
        this.currGunDamage += (int)value;
    }
    private void UseReward()
    {
        int id = this.grabRewardData.id;
        switch (id)
        {
            case 2000:
                Debug.LogFormat("damage({0}) up", this.grabRewardData.value);
                this.isDamageUp = true;

                this.addValue = (int)this.grabRewardData.value;
                break;
            case 2001:
                Debug.LogFormat("maxHp({0}) up", this.grabRewardData.value);
                this.isMaxHpUp = true;
                this.addValue = (int)this.grabRewardData.value;
                break;
            case 2002:
                Debug.LogFormat("autoCharging1");
                this.autoChargeValue = (int)this.grabRewardData.value;
                this.isAutoCharging1 = true;
                break;
            case 2003:
                Debug.LogFormat("autoCharging2");
                this.addValue = (int)this.grabRewardData.value;
                this.isAutoCharging2 = true;
                break;
            case 2004:
                Debug.LogFormat("change gun");
                this.rightHand.isSecondGun = true;
                break;
            case 2005:
                Debug.LogFormat("scope({0}) expands", this.grabRewardData.value);
                this.isScopeExpand = true;
                break;
        }
        if (this.dicGetRewards.ContainsKey(id))
            this.dicGetRewards[id]++;
        else
            this.dicGetRewards.Add(id, 1);
        foreach (var keyvalue in this.dicGetRewards)
        {
            Debug.LogFormat("<color=green>id: {0} count: {1}</color>", keyvalue.Key, keyvalue.Value);
        }
        this.rewardGroupCtrl.HideRewards(id);
        this.wave++;
        this.StartCoroutine(CoStartSpawnEnemy1());
    }

    private void GuideGame(string text)
    {
        this.guideText.text = text;
        this.guideText.gameObject.SetActive(true);
    }
    private void OnDestroy()
    {
        InfoManager.Instance.SaveInfos();
    }
}


