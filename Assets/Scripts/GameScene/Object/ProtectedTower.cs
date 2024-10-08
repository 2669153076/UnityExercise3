using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 防御塔
/// </summary>
public class ProtectedTower : MonoBehaviour
{
    private static ProtectedTower instance;
    public static ProtectedTower Instance => instance;

    private int curHp;
    private int maxHp;
    private bool isDead;

    private void Awake()
    {
        instance = this;
    }


    public void UpdateHp(int curHp, int maxHp)
    {
        this.curHp = curHp;
        this.maxHp = maxHp;

        UIManager.Instance.GetPanel<GamePanel>().UpdateHpImp(curHp, maxHp);
    }

    public void GetHit(int dmg)
    {
        if(isDead) return;

        curHp -= dmg;
        if(curHp<=0)
        {
            curHp = 0;
            isDead = true;

            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();
            panel.UpdateText((int)(GameLevelMgr.Instance.player.money * 0.5f), false);
        }

        UpdateHp(curHp, maxHp);
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
