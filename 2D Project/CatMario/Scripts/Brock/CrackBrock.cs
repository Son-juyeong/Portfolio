using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackBrock : MonoBehaviour
{
    public enum eState
    {
        Idle, Broken
    }

    [SerializeField]
    private GameObject[] arrBrockState;
    [SerializeField]
    private GameObject[] arrFragments;
    private Floor top;
    private BrockBottom bottom;
    private eState state;
    public System.Action onNotePlayerReach;
    public System.Action onBrokenBrock;
    public void Init()
    {
        this.top=GetComponentInChildren<Floor>();
        this.top.onCollision = () =>
        {
            Debug.Log("Player reach ground");
            this.onNotePlayerReach();
        };
        this.bottom=GetComponentInChildren<BrockBottom>();
        this.bottom.onCollision = () =>
        {
            Debug.Log("brock is broken");
            this.onBrokenBrock();
        };
        this.SetActiveState(eState.Idle);
    }

    public void SetActiveState(eState state)
    {
        this.InActiveAll();
        this.state = state;
        arrBrockState[(int)this.state].gameObject.SetActive(true);
    }

    private void InActiveAll()
    {
        for(int i=0;i<arrBrockState.Length;i++)
        {
            arrBrockState[i].SetActive(false);
        }
    }

    public void BreakBrock()
    {
        this.SetActiveState(eState.Broken);
        Vector2[] force =
        {
            new Vector2(-2000f, 11000f),
            new Vector2(2000f, 11000f),
            new Vector2(-2400f, 9000f),
            new Vector2(2400f, 9000f)
        };
        for(int i = 0; i < this.arrFragments.Length; i++)
        {
            Rigidbody2D rBody = arrFragments[i].GetComponent<Rigidbody2D>();
            rBody.AddForce(force[i]);
            Debug.LogFormat("{0}: {1}", arrFragments[i], rBody.velocity);
        }
    }
}
