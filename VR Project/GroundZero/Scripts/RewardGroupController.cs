using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class RewardGroupController : MonoBehaviour
{
    [SerializeField] private Reward[] arrRewards;
    private List<int> listRewardDatasID = new List<int>();
    public System.Action<RewardData> onWhenSelect;
    public System.Action onWhenUnselect;

    public void Init()
    {
        List<RewardData> listRewardDatas = DataManager.Instance.GetRewardDatas();
        this.listRewardDatasID = (from RewardData data in listRewardDatas select data.id).ToList();
        foreach (var data in listRewardDatas)
        {
            Debug.LogFormat("id: {0}", data.id);
            Debug.LogFormat("name: {0}", data.name);
            Debug.LogFormat("desc: {0}", data.desc);
            Debug.LogFormat("value: {0}", data.value);
            Debug.LogFormat("prefab_name: {0}", data.prefab_name);
        }
        foreach (var reward in arrRewards)
        {
            reward.Init(this.onWhenSelect, this.onWhenUnselect);
        }
    }

    public void ShowRewards()
    {
        this.gameObject.SetActive(true);
        List<int> rewardID = this.listRewardDatasID.ToList();
        for (int i = 0; i < arrRewards.Length; i++)
        {
            int idx = Random.Range(0, rewardID.Count);
            Debug.LogFormat("<color=red>남은 리워드 개수: {0}</color>", rewardID.Count);
            int id = rewardID[idx];
            this.arrRewards[i].SetData(id);
            rewardID.Remove(id);
        }
    }

    public void HideRewards(int id)
    {
        listRewardDatasID.Remove(id);

        this.gameObject.SetActive(false);
    }
}

