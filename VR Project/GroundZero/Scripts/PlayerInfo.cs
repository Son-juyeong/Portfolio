using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public string id;
    public string userName;
    public int maxHp;
    public int maxScore;
    public int shieldGauge;
    public int scrap;
    public int healthCanAmount;
    public int healthCanValue;

    public PlayerInfo(string id, string userName, int maxHp, int maxScore, int shieldGauge, int scrap, int healthCanAmount, int healthCanValue)
    {
        this.id = id;
        this.userName = userName;
        this.maxHp = maxHp;
        this.maxScore = maxScore;
        this.shieldGauge = shieldGauge;
        this.scrap = scrap;
        this.healthCanAmount = healthCanAmount;
        this.healthCanValue = healthCanValue;
    }
}