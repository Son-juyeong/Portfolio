using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Reward : MonoBehaviour
{
    [SerializeField] private TMP_Text textName;
    [SerializeField] private TMP_Text textDescription;
    //[SerializeField] private GameObject rewardGo;
    [SerializeField] private GameObject[] arrRewardGo;
    private RewardData rewardData;
    public System.Action<RewardData> onWhenSelect;
    public System.Action onWhenUnselect;
    private Vector3 rewardGoLocalPos = Vector3.up * 0.4f;

    public void Init(System.Action<RewardData> onWhenSelect, System.Action onWhenUnselect)
    {
        this.onWhenSelect = onWhenSelect;
        this.onWhenUnselect = onWhenUnselect;
    }

    public void SetData(int id)
    {
        RewardData data = DataManager.Instance.GetRewardData(id);
        this.rewardData = data;
        this.textName.text = data.name;
        string description = string.Format(data.desc, data.value);
        this.textDescription.text = description;
        this.ActiveGo();
    }

    private void ActiveGo()
    {
        this.InactiveAll();
        this.arrRewardGo[this.rewardData.id % 10].SetActive(true);
    }

    private void InactiveAll()
    {
        foreach (var go in arrRewardGo)
        {
            go.SetActive(false);
        }
    }

    public void OnWhenSelect()
    {
        Debug.LogFormat("{0} selected", this.rewardData.name);
        this.onWhenSelect(this.rewardData);
        InfoManager.Instance.itemEquiped++;
    }

    public void OnWhenUnselect()
    {
        Debug.LogFormat("{0} unselected", this.rewardData.name);
        this.onWhenUnselect();
        this.arrRewardGo[this.rewardData.id % 10].transform.localPosition = this.rewardGoLocalPos;
        this.arrRewardGo[this.rewardData.id % 10].transform.localRotation = Quaternion.identity;
    }
}