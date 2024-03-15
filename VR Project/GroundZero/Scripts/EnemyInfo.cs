using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo
{
    public string enemyType;
    public int scrap;

    public EnemyInfo(string enemyType, int scrap)
    {
        this.enemyType = enemyType;
        this.scrap = scrap;
    }
}