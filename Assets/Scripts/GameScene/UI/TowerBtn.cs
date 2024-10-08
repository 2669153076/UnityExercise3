using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 组合控件
/// 造塔按钮
/// </summary>
public class TowerBtn : MonoBehaviour
{
    public Button btn;
    public Text title;
    public Text money;
    
    /// <summary>
    /// 更新显示
    /// </summary>
    /// <param name="id"></param>
    /// <param name="title"></param>
    public void InitInfo(int id,string title)
    {
        TowerInfo info = GameDataMgr.Instance.towerInfoList[id - 1];
        btn.image.sprite = Resources.Load<Sprite>(info.imgResPath );
        money.text = "$" + info.money;
        this.title.text = title;

    }
}
