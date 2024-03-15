using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField]
    private GameObject ground;
    //[SerializeField]
    //private GameObject[] fallDownBrock;

    [SerializeField]
    private CrackBrock[] arrCrackBrocks;
    [SerializeField]
    private ItemBrock[] arrItemBrocks;
    [SerializeField]
    private Chimney[] arrChimneys;
    [SerializeField]
    private HiddenBrock[] arrHiddenBrocks;
    [SerializeField]
    private FallDownBrock[] arrFallDownBrocks;

    public System.Action onNotePlayerReach;
    public System.Action onNoteBrokenBrock;
    public System.Action<int, Transform> onNoteGenerateItem;
    public System.Action onAttackPlayer;

    // Start is called before the first frame update
    public void Init()
    {
        Floor groundTop = this.ground.GetComponent<Floor>();
        groundTop.onCollision = () =>
        {
            Debug.Log("Player is on the brock");
            this.onNotePlayerReach();
        };
        for(int i=0;i<arrCrackBrocks.Length;i++)
        {
            int idx = i;
            this.arrCrackBrocks[idx].Init();
            this.arrCrackBrocks[idx].onNotePlayerReach = () =>
            {
                Debug.Log("Player is on the brock");
                this.onNotePlayerReach();
            };
            this.arrCrackBrocks[idx].onBrokenBrock = () =>
            {
                this.arrCrackBrocks[idx].BreakBrock();
                this.onNoteBrokenBrock();
            };
        }
        for(int i=0;i< this.arrItemBrocks.Length; i++)
        {
            int idx = i;
            this.arrItemBrocks[idx].Init();
            this.arrItemBrocks[idx].onNotePlayerReach = () =>
            {
                Debug.Log("Player is on the brock");
                this.onNotePlayerReach();
            };
            this.arrItemBrocks[idx].onGenerateItem = (itemType) =>
            {
                Debug.LogFormat("item type: {0}", itemType);
                this.onNoteGenerateItem(itemType, this.arrItemBrocks[idx].transform);
                this.arrItemBrocks[idx].CompleteGenerate();
            };
        }
        for(int i=0;i< this.arrChimneys.Length;i++)
        {
            int idx = i;
            this.arrChimneys[idx].Init();
            this.arrChimneys[idx].onNotePlayerReach = () =>
            {
                Debug.Log("Player is on the brock");
                this.onNotePlayerReach();
            };
        }
        for(int i = 0; i < this.arrHiddenBrocks.Length; i++)
        {
            int idx = i;
            this.arrHiddenBrocks[idx].Init();
            this.arrHiddenBrocks[idx].onNotePlayerReach = () =>
            {
                Debug.Log("Player is on the brock");
                this.onNotePlayerReach();
            };
        }
        for(int i = 0; i < this.arrFallDownBrocks.Length; i++)
        {
            int idx = i;
            this.arrFallDownBrocks[idx].Init();
            this.arrFallDownBrocks[idx].onNotePlayerReach = () =>
            {
                Debug.Log("Player is on the brock");
                this.onNotePlayerReach();
            };
            this.arrFallDownBrocks[idx].onAttackPlayer = () =>
            {
                Debug.Log("Attack");
                this.onAttackPlayer();
            };
        }
    }

    public void SetActiveHiddenBrock(int idx)
    {
        this.arrHiddenBrocks[idx].gameObject.SetActive(true);
    }

    public Transform GetHiddenBrockTransform(int idx)
    {
        return this.arrHiddenBrocks[idx].transform;
    }

    public void StartFallDownEvent(int idx)
    {
        this.arrFallDownBrocks[idx].FallDown();
    }
}
