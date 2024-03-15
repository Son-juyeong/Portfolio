using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeController : MonoBehaviour
{
    [SerializeField] UIUpgradeCell[] arrUpgradeCells;
    [SerializeField] GameObject descGo;
    [SerializeField] GameObject lockedGo;
    [SerializeField] TMP_Text[] txtUpgradeDesc;
    [SerializeField] TMP_Text[] txtUpgradeLevel;
    [SerializeField] TMP_Text txtUpgradeName;
    [SerializeField] TMP_Text txtUpgradePrice;
    [SerializeField] TMP_Text txtScrapValue;
    [SerializeField] Button btnUpgrade;
    [SerializeField] GameObject btnFullUpgradeGo;
    private UpgradeInfo selectedCellInfo;

    public System.Action<UpgradeInfo> onClickUpgradeBtn;

    private void Start()
    {
        for (int i = 0; i < this.arrUpgradeCells.Length; i++)
        {
            int idx = i;
            this.arrUpgradeCells[idx].onClickBtn = (info) =>
            {
                this.selectedCellInfo = info;
                this.UpdateScreen();
            };
        }
        this.btnUpgrade.onClick.AddListener(() =>
        {
            this.onClickUpgradeBtn(this.selectedCellInfo);
        });
    }

    public void Init()
    {
        var list = InfoManager.Instance.GetUpgradeInfos();
        for (int i = 0; i < this.arrUpgradeCells.Length; i++)
        {
            UpgradeInfo info = null;
            if (i < list.Count) info = list[i];
            this.arrUpgradeCells[i].Init(info);
        }
        this.selectedCellInfo = list[0];
        this.UpdateScreen();
    }

    public void UpdateScreen()
    {
        var playerInfo = InfoManager.Instance.GetPlayerInfo();
        int scrap = playerInfo.scrap;
        this.txtScrapValue.text = scrap.ToString();
        if (this.selectedCellInfo == null)
        {
            this.descGo.SetActive(false);
            this.lockedGo.SetActive(true);
        }
        else
        {
            this.lockedGo.SetActive(false);
            this.descGo.SetActive(true);
            UpgradeData data = DataManager.Instance.GetUpgradeData(this.selectedCellInfo.id);
            string name = data.name;
            this.txtUpgradeName.text = name;
            for (int i = 0; i < this.txtUpgradeDesc.Length; i++)
            {
                string desc = string.Format(data.desc, data.value[i]);
                this.txtUpgradeDesc[i].text = desc;
                if (i + 1 > this.selectedCellInfo.level)
                {
                    //this.txtUpgradeDesc[i].color = Color.gray;
                    this.txtUpgradeLevel[i].alpha = 0.4f;
                    this.txtUpgradeDesc[i].alpha = 0.4f;
                }
                else
                {
                    //this.txtUpgradeDesc[i].color = Color.white;
                    this.txtUpgradeLevel[i].alpha = 1f;
                    this.txtUpgradeDesc[i].alpha = 1f;
                }
            }
            if (this.selectedCellInfo.level > 4)
            {
                this.btnUpgrade.gameObject.SetActive(false);
                this.btnFullUpgradeGo.SetActive(true);
            }
            else
            {
                this.btnUpgrade.gameObject.SetActive(true);
                this.btnFullUpgradeGo.SetActive(false);
                int price = data.price[this.selectedCellInfo.level - 1];
                this.txtUpgradePrice.text = price.ToString();
                if (price > scrap)
                {
                    this.btnUpgrade.interactable = false;
                }
                else
                    this.btnUpgrade.interactable = true;
            }
        }
    }
}