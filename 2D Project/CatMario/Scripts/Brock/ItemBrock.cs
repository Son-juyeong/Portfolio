using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBrock : MonoBehaviour
{
    public enum eState
    {
        Idle, Complete
    }
    private eState state;
    [SerializeField]
    private GameObject[] arrItemBrockState;
    [SerializeField]
    private Floor[] arrBrockTop;
    [SerializeField]
    private BrockBottom brockBottomIdle;
    public System.Action onNotePlayerReach;
    public System.Action<int> onGenerateItem;
    [SerializeField]
    private int itemType;
    public void Init()
    {
        this.SetActiveState(eState.Idle);
        for (int i=0;i<this.arrBrockTop.Length;i++)
        {
            int idx = i;
            this.arrBrockTop[idx].onCollision = () =>
            {
                Debug.Log("Player reach ground");
                this.onNotePlayerReach();
            };
        }
        this.brockBottomIdle.onCollision = () =>
        {
            Debug.Log("Generate Item!");
            this.onGenerateItem(this.itemType);
        };
    }

    public void SetActiveState(eState state)
    {
        this.state = state;
        this.InActiveAll();
        this.arrItemBrockState[(int)this.state].SetActive(true);
    }

    private void InActiveAll()
    {
        for(int i=0;i<arrItemBrockState.Length;i++)
        {
            arrItemBrockState[i].SetActive(false);
        }
    }

    public void CompleteGenerate()
    {
        this.SetActiveState(eState.Complete);
    }
}
