using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class InfoManager
{
    public static readonly InfoManager Instance = new InfoManager();
    private Dictionary<int, UpgradeInfo> dicUpgradeInfo = new Dictionary<int, UpgradeInfo>();
    private PlayerInfo playerInfo;
    private Dictionary<int, GunInfo> dicGunInfos = new Dictionary<int, GunInfo>();
    private Dictionary<string, EnemyInfo> dicEnemyInfos = new Dictionary<string, EnemyInfo>();

    private string playerInfoPath = Path.Combine(Application.persistentDataPath, "player_info");
    private string upgradeInfoPath = Path.Combine(Application.persistentDataPath, "upgrade_info");
    private string gunInfoPath = Path.Combine(Application.persistentDataPath, "gun_info");
    private string enemyInfoPath = Path.Combine(Application.persistentDataPath, "enemy_info");

    //-----------LJE GameScore-----------------
    public int kills = 0;
    public int blockedBullet = 0;
    public int totalDamage = 0;
    public int itemEquiped = 0;
    public int score = 0;

    public void LoadInfos()
    {
        if (File.Exists(this.playerInfoPath))
        {
            Debug.Log("기존");
            string json = File.ReadAllText(this.playerInfoPath);
            this.playerInfo = JsonConvert.DeserializeObject<PlayerInfo>(json);

            json = File.ReadAllText(this.upgradeInfoPath);
            var upgradeInfos = JsonConvert.DeserializeObject<UpgradeInfo[]>(json);
            this.dicUpgradeInfo = upgradeInfos.ToDictionary(x => x.id);

            json = File.ReadAllText(this.gunInfoPath);
            var gunInfos = JsonConvert.DeserializeObject<GunInfo[]>(json);
            this.dicGunInfos = gunInfos.ToDictionary(x => x.id);

            json = File.ReadAllText(this.enemyInfoPath);
            var enemyInfos = JsonConvert.DeserializeObject<EnemyInfo[]>(json);
            this.dicEnemyInfos = enemyInfos.ToDictionary(x => x.enemyType);
        }
        else
        {
            Debug.Log("신규");
            var upgradeDatas = DataManager.Instance.GetUpgradeDatas();
            for (int i = 0; i < upgradeDatas.Count; i++)
            {
                var data = upgradeDatas[i];
                var upgradeInfo = new UpgradeInfo(data.id, 1);
                this.dicUpgradeInfo.Add(upgradeInfo.id, upgradeInfo);
            }

            var playerData = DataManager.Instance.GetPlayerDatas()[0];
            this.playerInfo = new PlayerInfo(playerData.id, playerData.playerUserName, playerData.playerHp, playerData.score,
                playerData.shieldGauge, playerData.scrap, playerData.healthCanAmount, playerData.healthCanValue);
            //Debug.LogFormat("playerData shieldGauge: {0}", playerData.shieldGauge);
            //Debug.LogFormat("playerInfo shieldGauge: {0}", this.playerInfo.shieldGauge);

            var gunDatas = DataManager.Instance.GetGunDatas();
            foreach (var gunData in gunDatas)
            {
                var gunInfo = new GunInfo(gunData.gunId, gunData.gunDamage);
                this.dicGunInfos.Add(gunInfo.id, gunInfo);
            }

            var enemyDatas = DataManager.Instance.GetEnemyDatas();
            foreach (var enemyData in enemyDatas)
            {
                var enemyInfo = new EnemyInfo(enemyData.enemyType, enemyData.scrap);
                this.dicEnemyInfos.Add(enemyInfo.enemyType, enemyInfo);
            }

            this.SaveInfos();
        }
    }

    public void SaveInfos()
    {
        string json = JsonConvert.SerializeObject(this.GetUpgradeInfos());
        File.WriteAllText(this.upgradeInfoPath, json);

        json = JsonConvert.SerializeObject(this.playerInfo);
        File.WriteAllText(this.playerInfoPath, json);

        json = JsonConvert.SerializeObject(this.GetGunInfos());
        File.WriteAllText(this.gunInfoPath, json);

        json = JsonConvert.SerializeObject(this.GetEnemyInfos());
        File.WriteAllText(this.enemyInfoPath, json);
    }

    public List<UpgradeInfo> GetUpgradeInfos()
    {
        return this.dicUpgradeInfo.Values.ToList();
    }

    public PlayerInfo GetPlayerInfo()
    {
        return this.playerInfo;
    }

    public List<GunInfo> GetGunInfos()
    {
        return this.dicGunInfos.Values.ToList();
    }

    public GunInfo GetGunInfo(int id)
    {
        return this.dicGunInfos[id];
    }

    public List<EnemyInfo> GetEnemyInfos()
    {
        return this.dicEnemyInfos.Values.ToList();
    }

    public EnemyInfo GetEnemyInfo(string type)
    {
        return this.dicEnemyInfos[type];
    }

    public void InitGameScore()
    {
        kills = 0;
        blockedBullet = 0;
        totalDamage = 0;
        itemEquiped = 0;
        score = 0;
    }
}