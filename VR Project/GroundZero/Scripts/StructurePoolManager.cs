using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class StructurePoolManager : MonoBehaviour
    {
        public static StructurePoolManager Instance;

        [SerializeField] private GameObject[] arrStructurePrefabs;
        private List<List<GameObject>> listStructures = new List<List<GameObject>>();
        public int count = 10;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }
        //void Start()
        //{
        //    this.InitStructure();
        //}

        public void InitStructure()
        {
            for (int i = 0; i < arrStructurePrefabs.Length; i++)
            {
                listStructures.Add(new List<GameObject>());
                for (int j = 0; j < this.count; j++)
                {
                    GameObject go = this.GenerateStructure(i);
                    go.transform.SetParent(this.transform);
                    go.SetActive(false);
                }
            }
        }

        private GameObject GenerateStructure(int idx)
        {
            GameObject go = Instantiate(this.arrStructurePrefabs[idx]);
            listStructures[idx].Add(go);
            return go;
        }

        public GameObject EnableStructure(int idx)
        {
            GameObject structure = null;
            for (int i = 0; i < this.listStructures[idx].Count; i++)
            {
                GameObject go = this.listStructures[idx][i];
                if (go.activeSelf == false)
                {
                    go.SetActive(true);
                    go.transform.SetParent(null);
                    structure = go;
                    break;
                }
            }
            if (structure == null)
            {
                structure = this.GenerateStructure(idx);
            }
            return structure;
        }

        public void DisableStructure(GameObject go)
        {
            go.transform.SetParent(this.transform);
            go.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            go.SetActive(false);
        }
    }
