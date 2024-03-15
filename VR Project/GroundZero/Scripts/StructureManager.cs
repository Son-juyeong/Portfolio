using System.Collections;
using System.Collections.Generic;
using UnityEngine;



  
    public class StructureManager : MonoBehaviour
    {
    private Queue<List<Structure>> qStructures = new Queue<List<Structure>>();
    private bool install;
    private bool uninstall;

    public void InstallStructure(Rail rail)
    {
        if (!(this.install ^= true)) return;
        List<Structure> list = new List<Structure>();
        int type = Random.Range((int)Structure.eType.ARCHITECTURE1, (int)Structure.eType.ARCHITECTURE3 + 1);
        GameObject go = StructurePoolManager.Instance.EnableStructure(type);
        go.transform.position = rail.transform.position;
        list.Add(go.GetComponent<Structure>());
        int rand = Random.Range(0, 2);
        GameObject pole;
        if (rand % 2 == 0)
        {
            pole = StructurePoolManager.Instance.EnableStructure((int)Structure.eType.LAMP);
        }
        else
        {
            pole = StructurePoolManager.Instance.EnableStructure(((int)Structure.eType.CLOCK));
        }
        float[] dirx = new float[] { -1, 1 };
        pole.transform.position = rail.transform.position + Vector3.left * dirx[Random.Range(0, dirx.Length)] * Random.Range(5f, 7f) + Vector3.back * Random.Range(1f, 15f);
        list.Add(pole.GetComponent<Structure>());
        qStructures.Enqueue(list);
    }

    public void UninstallStructure()
    {
        if (!(this.uninstall ^= true) || qStructures.Count == 0) return;
        List<Structure> list = qStructures.Dequeue();
        for (int i = 0; i < list.Count; i++)
        {
            StructurePoolManager.Instance.DisableStructure(list[i].gameObject);
        }
    }
}
       
    
