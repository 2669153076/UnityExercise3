using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : PanelBase
{
    public Button startBtn;
    public Button settingBtn;
    public Button aboutBtn;
    public Button quitBtn;

    protected override void Init()
    {
        startBtn.onClick.AddListener(() =>
        {
            Camera.main.GetComponent<CameraAnimator>().TurnLeft(() =>
            {
                UIManager.Instance.ShowPanel<RoleSelPanel>();
            });

            UIManager.Instance.HidePanel();
        });
        settingBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<SettingPanel>();
        });
        aboutBtn.onClick.AddListener(() =>
        {

        });
        quitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });

    }
}
