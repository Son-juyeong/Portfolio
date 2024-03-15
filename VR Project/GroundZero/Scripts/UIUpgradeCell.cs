using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeCell : MonoBehaviour
{
    private UpgradeInfo info;
    private Button btn;
    public System.Action<UpgradeInfo> onClickBtn;

    void Start()
    {
        this.btn = GetComponent<Button>();
        this.btn.onClick.AddListener(() =>
        {
            this.onClickBtn(this.info);
        });
    }

    public void Init(UpgradeInfo info)
    {
        this.info = info;
    }
}