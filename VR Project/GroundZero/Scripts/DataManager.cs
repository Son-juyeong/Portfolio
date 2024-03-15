using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

    public class DataManager
    {//DataManager.cs manage Datas

        public static readonly DataManager Instance = new DataManager();

        //Dictionaries To save Data
        private Dictionary<string, PlayerData> dicPlayerDatas = new Dictionary<string, PlayerData>();
        private Dictionary<int, GunData> dicGunDatas = new Dictionary<int, GunData>();
        private Dictionary<string, EnemyData> dicEnemyDatas = new Dictionary<string, EnemyData>();


    //reward ===================================================================================================================================
    private Dictionary<int, RewardData> dicRewardDatas = new Dictionary<int, RewardData>();
    //==========================================================================================================================================

    //----------------------------------------SJY UpgradeData----------------------------------------------------------------------------
    private Dictionary<int, UpgradeData> dicUpgradeDatas = new Dictionary<int, UpgradeData>();
    //-----------------------------------------------------------------------------------------------------------------------------------

    //method
    public void LoadPlayerDatas()
        {

          //  if (File.Exists("./Assets/Resources/Data/playerData.json"))
            {
            //read file
            // string json = File.ReadAllText("./Assets/Resources/Data/playerData.json");
            TextAsset asset = Resources.Load<TextAsset>("player_data");
               string json = asset.text;
                // Debug.Log(json);

                var playerDatas = JsonConvert.DeserializeObject<PlayerData[]>(json);
                this.dicPlayerDatas = playerDatas.ToDictionary(x => x.playerUserName);

            }
            //else
            //{
            //    Debug.Log("File doesn't exist!");
            //    return;
            //}
            Debug.LogFormat("Loaded Player Data: {0}", this.dicPlayerDatas.Count);
        }

        public void LoadGunDatas()
        {
            //if (File.Exists("./Assets/Resources/Data/gunData.json"))
            { //read file
            TextAsset asset = Resources.Load<TextAsset>("gunData");
            string json = asset.text;
              //  string json = File.ReadAllText("./Assets/Resources/Data/gunData.json");
                var gunDatas = JsonConvert.DeserializeObject<GunData[]>(json);
                this.dicGunDatas = gunDatas.ToDictionary(x => x.gunId);

            }
            //else
            //{
            //    Debug.Log("File doesn't exist!");
            //    return;
            //}
            Debug.LogFormat("Loaded Gun Data: {0}", this.dicGunDatas.Count);
        }
        public void LoadEnemyDatas()
        {
           // if (File.Exists("./Assets/Resources/Data/enemyData.json"))
            { //read file
            TextAsset asset = Resources.Load<TextAsset>("enemy_data");
            string json = asset.text;

            //string json = File.ReadAllText("./Assets/Resources/Data/enemyData.json");
            var enemyDatas = JsonConvert.DeserializeObject<EnemyData[]>(json);
                this.dicEnemyDatas = enemyDatas.ToDictionary(x => x.enemyType);


            }
            //else
            //{
            //    Debug.Log("File doesn't exist!");
            //    return;
            //}
            Debug.LogFormat("Loaded Enemy Data: {0}", this.dicGunDatas.Count);
        }
    //reward===============================================================================================================
    public void LoadRewardDatas()
    {
        TextAsset asset = Resources.Load<TextAsset>("reward_data");
        string json = asset.text;
        var rewardDatas = JsonConvert.DeserializeObject<RewardData[]>(json);
        this.dicRewardDatas = rewardDatas.ToDictionary(x => x.id);
    }
    //=====================================================================================================================

    //----------------------------------------------------------UpgradeData------------------------------------------------
    public void LoadUpgradeDatas()
    {
        TextAsset asset = Resources.Load<TextAsset>("upgrade_data");
        string json = asset.text;
        var upgradeDatas = JsonConvert.DeserializeObject<UpgradeData[]>(json);
        foreach (var item in upgradeDatas)
        {
            string desc = string.Format(item.desc, item.value[0]);
            Debug.LogFormat("id: {0}, name: {1}, desc: {2}", item.id, item.name, desc);
            foreach (var amount in item.value)
            {
                Debug.LogFormat("amount: {0}", amount);
            }
            foreach (var price in item.price)
            {
                Debug.LogFormat("price: {0}", price);
            }
        }
        this.dicUpgradeDatas = upgradeDatas.ToDictionary(x => x.id);
    }
    //--------------------------------------------------------------------------------------------------------------------

    public List<PlayerData> GetPlayerDatas()
        {
            return this.dicPlayerDatas.Values.ToList();
        }

        public List<GunData> GetGunDatas()
        {
            return this.dicGunDatas.Values.ToList();
        }
        public List<EnemyData> GetEnemyDatas()
        {
            return this.dicEnemyDatas.Values.ToList();
        }

        public EnemyData GetEnemyData(string type)
        {
            return this.dicEnemyDatas[type];
        }
    //reward=============================================================================================

    public List<RewardData> GetRewardDatas()
    {
        return this.dicRewardDatas.Values.ToList();
    }

    public RewardData GetRewardData(int id)
    {
        return this.dicRewardDatas[id];
    }

    //====================================================================================================

    //------------------------------------SJY UpgradeData---------------------------------------------------
    public List<UpgradeData> GetUpgradeDatas()
    {
        return this.dicUpgradeDatas.Values.ToList();
    }

    public UpgradeData GetUpgradeData(int id)
    {
        return this.dicUpgradeDatas[id];
    }
    //------------------------------------------------------------------------------------------------------

    public GunData GetGunData(int id)
    {
        return this.dicGunDatas[id];
    }
}
