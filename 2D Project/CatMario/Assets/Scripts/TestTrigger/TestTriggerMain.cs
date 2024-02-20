using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTriggerMain : MonoBehaviour
{
    [SerializeField]
    private EventsTrigger[] arrTriggerPoints;
    [SerializeField]
    private HiddenBrockTrigger[] arrHiddenBrockTriggers;
    [SerializeField]
    private HiddenBrock[] arrHiddenBrock;
    private void Start()
    {
        for(int i = 0; i < arrTriggerPoints.Length; i++)
        {
            int idx = i;
            arrTriggerPoints[idx].onStartEvent = () =>
            {
                Debug.Log("이벤트 시작");
                arrTriggerPoints[idx].gameObject.SetActive(false);
            };
        }
        for(int i = 0; i < arrHiddenBrockTriggers.Length; i++)
        {
            int idx = i;
            arrHiddenBrockTriggers[idx].onAppearBrock = () =>
            {
                //if (y + 10 < arrHiddenBrockTriggers[idx].transform.position.y)
                //{
                //    Debug.Log("Trigger, onActive");
                //    arrHiddenBrock[idx].gameObject.SetActive(true);
                //    arrHiddenBrockTriggers[idx].gameObject.SetActive(false);
                //}
                //else
                //{
                //    Debug.Log("not Trigger");
                //}
            };
        }
    }
}
