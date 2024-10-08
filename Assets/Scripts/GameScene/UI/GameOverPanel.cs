using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : PanelBase
{
    public Text resultText;
    public Text moneyText;
    public Button enterBtn;

    protected override void Init()
    {
        enterBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel(); //隐藏游戏结束面板
            UIManager.Instance.HidePanel(); //隐藏游戏场景主面板

            GameLevelMgr.Instance.Clear();
            SceneManager.LoadScene("BeginScene");
        });
    }

    public void UpdateText(int money,bool isWin)
    {
        resultText.text = isWin ? "胜利" : "失败";
        moneyText.text = "$" + money;

        GameDataMgr.Instance.playerData.money += money;
        GameDataMgr.Instance.SavePlayerData();
    }

    public override void ShowSelf()
    {
        base.ShowSelf();
        //显示并释放鼠标
        Cursor.lockState = CursorLockMode.None;
    }

}
