using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMain : MonoBehaviour
{
    [SerializeField] UIUpgradeController upgradeController;
    [SerializeField] Button btnStart;
    [SerializeField] GameObject[] upgradeBtns;
    private Animator startBtnAnim;

    void Start()
    {
        DataManager.Instance.LoadUpgradeDatas();
        DataManager.Instance.LoadPlayerDatas();
        DataManager.Instance.LoadEnemyDatas();
        DataManager.Instance.LoadGunDatas();

        InfoManager.Instance.LoadInfos();
        this.upgradeController.onClickUpgradeBtn = (info) =>
        {
            var playerInfo = InfoManager.Instance.GetPlayerInfo();
            var upgradeData = DataManager.Instance.GetUpgradeData(info.id);
            var playerData = DataManager.Instance.GetPlayerDatas()[0];
            int value = upgradeData.value[info.level - 1];
            switch (info.id)
            {
                case 300:
                    playerInfo.maxHp = playerData.playerHp + value;
                    Debug.LogFormat("player maxHp: {0}", playerInfo.maxHp);
                    break;
                case 301:
                    var gunInfos = InfoManager.Instance.GetGunInfos();
                    var gunDatas = DataManager.Instance.GetGunDatas();
                    for (int i = 0; i < gunInfos.Count; i++)
                    {
                        gunInfos[i].damage = gunDatas[i].gunDamage + value;
                        Debug.LogFormat("gun{0} damage: {1}", gunInfos[i].id, gunInfos[i].damage);
                    }
                    break;
                case 302:
                    playerInfo.healthCanValue = playerData.healthCanValue + value;
                    Debug.LogFormat("player healthCanValue: {0}", playerInfo.healthCanValue);
                    break;
                case 303:
                    playerInfo.healthCanAmount = value;
                    Debug.LogFormat("player healthCanAmount: {0}", playerInfo.healthCanAmount);
                    break;
                case 304:
                    playerInfo.shieldGauge = playerData.shieldGauge + value;
                    Debug.LogFormat("player shieldGauge: {0}", playerInfo.shieldGauge);
                    break;
                case 305:
                    var enemyInfos = InfoManager.Instance.GetEnemyInfos();
                    var enemyDatas = DataManager.Instance.GetEnemyDatas();
                    for (int i = 0; i < enemyInfos.Count; i++)
                    {
                        enemyInfos[i].scrap = enemyDatas[i].scrap + value;
                        Debug.LogFormat("enemy{0} scrap: {1}", enemyInfos[i].enemyType, enemyInfos[i].scrap);
                    }
                    break;
            }
            playerInfo.scrap -= upgradeData.price[info.level - 1];
            info.level++;
            this.upgradeController.UpdateScreen();
            InfoManager.Instance.SaveInfos();
        };
        this.btnStart.onClick.AddListener(() =>
        {
            StartCoroutine(CoStartBtnAnim());
            SceneManager.LoadScene("GameScene");
        });
        this.upgradeController.Init();
    }

    private IEnumerator CoStartBtnAnim()
    {
        this.startBtnAnim = this.btnStart.GetComponent<Animator>();
        this.startBtnAnim.SetBool("isStart", true);
        yield return new WaitForSeconds(0.16f);
        this.startBtnAnim.SetBool("isStart", false);
    }

    public void upgradeBtnAnim(int number)
    {
        StartCoroutine(CoUpgradeBtnAnim(number));
    }

    private IEnumerator CoUpgradeBtnAnim(int number)
    {
        this.upgradeBtns[number].GetComponent<Transform>().localScale = new Vector3(1.2f, 1.2f, 1.2f);
        yield return new WaitForSeconds(0.15f);
        this.upgradeBtns[number].GetComponent<Transform>().localScale = Vector3.one;
    }
}