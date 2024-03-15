using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyGenerator : MonoBehaviour
{
    public List<GameObject> enemyGoList;

    // Start is called before the first frame update
    void Start()
    {

    }

    public EnemyController Generate(GameEnums.eEnemyType enemyType, Vector3 initPosition)
    {
        int index = (int)enemyType;
        GameObject enemy = this.enemyGoList[index];
        GameObject go = Instantiate(enemy);
        go.name = enemy.name;

        go.transform.position = initPosition;
        return go.GetComponent<EnemyController>();
    }
}
