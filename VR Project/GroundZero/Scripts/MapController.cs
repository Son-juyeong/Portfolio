using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class MapController : MonoBehaviour
    {
        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private Transform playerTrans;
        [SerializeField] private StructureManager structureManager;
        private Queue<GameObject> queueFloor = new Queue<GameObject>();
        private Queue<Rail> queueRail = new Queue<Rail>();
        private Stack<Rail.eType> stackRail = new Stack<Rail.eType>();
        private int maxStackSize = 3;
        public float offset = 112f;
        public float dis = 16f;
        private int count;
        private Transform lastFloorTrans;
        private Vector3 railPos;
        public System.Action<Vector3> onInformInitPos;
        public System.Action<Rail> onInformRoute;
    //------------------------------------------------------Zeppelin--------------------------------------------------
    [SerializeField] private GameObject[] itemPrefabs;
    private Queue<GameObject> queueZeppelin = new Queue<GameObject>();
    private int installZeppelin;
    private int mod = 3;
    //----------------------------------------------------------------------------------------------------------------

    public void Init()
        {

        //-----------------------------------------------Zeppelin----------------------------------------------------
        ZeppelinPoolManager.instance.Init();
        //-----------------------------------------------------------------------------------------------------------
        this.count = (int)(offset / dis * 2) + 1;
            RailPoolManager.Instance.Init(this.count);
            StructurePoolManager.Instance.InitStructure();
            this.InitFloor();
            this.InitRailAndZeppelin();
        }

        void Update()
        {
            this.UpdateMap();
        }

        private void UpdateMap()
        {
            while (queueFloor.Count > 0 && queueFloor.Peek().transform.position.z > +playerTrans.position.z + this.offset + this.dis)
            {
                GameObject floorGo = queueFloor.Dequeue();
                this.UnInstallRail();
                Vector3 pos = lastFloorTrans.position + Vector3.forward * -this.dis;
                floorGo.transform.position = pos;
                railPos = pos + Vector3.right * this.CalculateRailOffset();
               
                this.InstallRail();
            //---------------------------------------------------------Zeppelin------------------------------------------
            if ((this.installZeppelin++ % mod) == 0)
            {
                //this.UnstallZeppelin();
                this.InstallZeppelin();
            }
            while (queueZeppelin.Count > 0 && queueZeppelin.Peek().transform.position.z > this.playerTrans.position.z + this.offset)
                this.UinstallZeppelin();
            //-----------------------------------------------------------------------------------------------------------

            queueFloor.Enqueue(floorGo);
                lastFloorTrans = floorGo.transform;
            }
        }

        private void InitFloor()
        {
            for (int i = 0; i < count; i++)
            {
                GameObject floorGo = Instantiate(floorPrefab);
                floorGo.transform.position = playerTrans.position + Vector3.forward * (offset - i * this.dis);
                queueFloor.Enqueue(floorGo);
                if (i == count - 1)
                    lastFloorTrans = floorGo.transform;
            }
        }

    private void InitRailAndZeppelin()
    {
        RailPoolManager.Instance.Init(this.count);
        for (int i = 0; i < this.count; i++)
        {
            float railOffset = this.CalculateRailOffset();
            railPos = playerTrans.position + Vector3.forward * (offset - i * this.dis) + Vector3.right * railOffset;
            if (railPos.z == 0)
                this.onInformInitPos(railPos);
            if (railPos.z >= 0)
            {
                this.InstallMRail();
            }
            else
                this.InstallRail();
            //---------------------------------------Zeppelin-----------------------------------------------
            if ((this.installZeppelin++ % mod) == 0)
                this.InstallZeppelin();
            //----------------------------------------------------------------------------------------------
        }
    }
    private float CalculateRailOffset()
        {
            float railOffset = stackRail.Count;
            if (railOffset > 0)
            {
                if (stackRail.Peek() == Rail.eType.Left)
                {
                    railOffset *= 4;
                }
                else
                {
                    railOffset *= -4;
                }
            }
            return railOffset;
        }

        private void InstallRail()
        {
            int rand = Random.Range(0, 10);
            if (rand < 2)                   //LRail
            {
                if (this.stackRail.Count == this.maxStackSize && this.stackRail.Peek() == Rail.eType.Left)
                {
                    //Debug.Log(stackRail.Peek());
                    this.InstallRRail();
                }
                else
                    this.InstallLRail();
            }
            else if (rand < 4)              //RRail
            {
                if (this.stackRail.Count == this.maxStackSize && this.stackRail.Peek() == Rail.eType.Right)
                    this.InstallLRail();
                else
                    this.InstallRRail();
            }
            else                            //MRail
            {
                this.InstallMRail();
            }
        }

        private void InstallLRail()
        {
            GameObject go = RailPoolManager.Instance.EnableRail((int)Rail.eType.Left);
            Rail rail = go.GetComponent<Rail>();
            queueRail.Enqueue(rail);
            go.transform.position = this.railPos;
            //structure 생성
            structureManager.InstallStructure(rail);

            //루트 알려주기
            this.onInformRoute(rail);

            //stack에 저장
            if (this.stackRail.Count == 0 || this.stackRail.Peek() == Rail.eType.Left)
            {
                stackRail.Push(Rail.eType.Left);
            }
            else if (this.stackRail.Peek() == Rail.eType.Right)
            {
                stackRail.Pop();
            }
            //Debug.LogFormat("<color=yellow>stack count:{0}</color>", stackRail.Count);
        }

        private void InstallMRail()
        {
            GameObject go = RailPoolManager.Instance.EnableRail((int)Rail.eType.Middle);
            Rail rail = go.GetComponent<Rail>();
            queueRail.Enqueue(rail);
            go.transform.position = this.railPos;

            structureManager.InstallStructure(rail);

            this.onInformRoute(rail);
            //Debug.LogFormat("<color=yellow>stack count:{0}</color>", stackRail.Count);
        }

        private void InstallRRail()
        {
            GameObject go = RailPoolManager.Instance.EnableRail((int)Rail.eType.Right);
            Rail rail = go.GetComponent<Rail>();
            queueRail.Enqueue(rail);
            go.transform.position = this.railPos;

            structureManager.InstallStructure(rail);

            this.onInformRoute(rail);
            if (this.stackRail.Count == 0 || this.stackRail.Peek() == Rail.eType.Right)
            {
                stackRail.Push(Rail.eType.Right);
            }
            else if (this.stackRail.Peek() == Rail.eType.Left)
            {
                stackRail.Pop();
            }
            //Debug.LogFormat("<color=yellow>stack count:{0}</color>", stackRail.Count);
        }

        private void UnInstallRail()
        {
            Rail rail = queueRail.Dequeue();
            RailPoolManager.Instance.DisableRail(rail.gameObject);
            structureManager.UninstallStructure();
        }


    //--------------------------------------------------------Zeppelin----------------------------------------------------
    private void InstallZeppelin()
    {
        GameObject zeppelinGo = ZeppelinPoolManager.instance.GetZeppelin();
        Vector3 pos = this.railPos + Vector3.right * Random.Range(-10f, 10f) + Vector3.up * 6f + Vector3.forward * Random.Range(-12f, -3f);
        zeppelinGo.transform.position = pos;
        this.queueZeppelin.Enqueue(zeppelinGo);
        var zeppelin = zeppelinGo.GetComponent<ZeppelinController>();
        zeppelin.die = (pos) =>
        {
            this.StartCoroutine(this.CoCreateItem(pos));
        };
        zeppelin.Init();
    }

    private void UinstallZeppelin()
    {
        GameObject zeppelinGo = this.queueZeppelin.Dequeue();
        ZeppelinPoolManager.instance.ReturnZeppelin(zeppelinGo);
    }

    private IEnumerator CoCreateItem(Vector3 pos)
    {
        for (int i = 0; i < 10; i++)
        {
            var rotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
            yield return new WaitForSeconds(0.1f);

            //아이템 2가지 중 랜덤 생성
            Instantiate(itemPrefabs[Random.Range(0, this.itemPrefabs.Length)], pos, rotation);
        }
    }
    //--------------------------------------------------------------------------------------------------------------------
}
