using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : PanelBase

{
    public Text tipText;
    public Button enterBtn;

    protected override void Init()
    {
        enterBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel();
        });
    }


    public void UpdateTipText(string tip)
    {
        tipText.text = tip;
    }

}
