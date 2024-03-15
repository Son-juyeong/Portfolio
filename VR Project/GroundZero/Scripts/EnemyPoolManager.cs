using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

    public class EnemyPoolManager : MonoBehaviour
    {
        public static EnemyPoolManager instance;
        //[SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject[] arrEnemyPrefabs;
        // private List<Enemy1Move> enemyPool = new List<Enemy1Move>();
         private List<List<GameObject>> enemyPool = new List<List<GameObject>>();

        void Awake()
        {
            instance = this;
        }


        private void Start()
        {
            this.GenerateEnemy();
        }

        private void GenerateEnemy()
        {
           // for (int i = 0; i < 2; i++)
            for(int i=0;i<this.arrEnemyPrefabs.Length; i++)    
            {
                List<GameObject> list = new List<GameObject>();
                for (int j = 0; j < 2; j++)
                {
                    GameObject go = Instantiate(arrEnemyPrefabs[i], this.transform);
                    go.SetActive(false);
                    list.Add(go);
                    //GameObject go = Instantiate(enemyPrefab, this.transform);
                    //go.SetActive(false);
                    //enemyPool.Add(go.GetComponent<Enemy1Move>());

                }
                this.enemyPool.Add(list);
                }
        }

        public GameObject EnableEnemy(int idx)
        {
            GameObject result = null;

            for (int i = 0; i < enemyPool[idx].Count; i++)
            {
            //  Debug.Log(enemyPool[idx][i]);
            if (enemyPool[idx][i].activeSelf == false)
            {
                result = enemyPool[idx][i];
                //result.gameObject.SetActive(true);
                result.transform.SetParent(null);
                break;
            }
        }
        if (result == null)
        {
            result = Instantiate(arrEnemyPrefabs[idx]);
            enemyPool[idx].Add(result);
        }
        return result;


        }

        public void DisableEnemy(GameObject enemy)
        {
            enemy.SetActive(false);
            enemy.transform.SetParent(this.transform);
        }

    }


