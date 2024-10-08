using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameLevelMgr
{
    private static GameLevelMgr instance= new GameLevelMgr();
    public static GameLevelMgr Instance=>instance;

    private GameLevelMgr() { }

    public Player player;


    //所有的出怪点
    private List<EnemyCreatePoint> enemyCreatePoints = new List<EnemyCreatePoint>();

    //当前波数
    private int curWave = 0;
    //最大波数
    private int maxWave = 0;
    //当前场景怪物数量
    //private int curEnemyNum = 0;

    //记录场景中的怪物
    private List<Enemy> enemyList = new List<Enemy>();

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="sceneInfo"></param>
    public void InitInfo(SceneInfo sceneInfo)
    {
        UIManager.Instance.ShowPanel<GamePanel>();

        RoleInfo roleInfo = GameDataMgr.Instance.curSelRole;
        Transform roleTrans = GameObject.Find("RoleBornPos").transform;

        //实例化玩家
        GameObject roleObj = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.resPath), roleTrans.position,Quaternion.identity);

        player = roleObj.GetComponent<Player>();
        //初始化玩家角色属性
        player.InitPlayerInfo(roleInfo.atk, sceneInfo.money);

        Camera.main.GetComponent<CameraMove>().SetTarget(roleObj.transform);
        //初始化防御塔血量
        ProtectedTower.Instance.UpdateHp(sceneInfo.towerHp,sceneInfo.towerHp);
    }

    /// <summary>
    /// 记录出怪点
    /// </summary>
    /// <param name="point"></param>
    public void AddEnemyCreatePoint(EnemyCreatePoint point)
    {
        enemyCreatePoints.Add(point);
    }

    /// <summary>
    /// 检测是否胜利
    /// </summary>
    /// <returns></returns>
    public bool CheckSuccess()
    {
        for (int i = 0; i < enemyCreatePoints.Count; i++)
        {
            if (!enemyCreatePoints[i].AllEnemyCreateOver())
            {
                return false;
            }
        }
        if(enemyList.Count> 0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 更新一共有多少波怪
    /// 最大波数 = 每一个出怪点的波数相加
    /// </summary>
    /// <param name="num">每一个出怪点有多少波怪物</param>
    public void UpdateMaxWave(int num)
    {
        maxWave += num;
        curWave = 0;
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveText(curWave, maxWave);
    }

    /// <summary>
    /// 更新当前波数
    /// </summary>
    /// <param name="num">出了几波怪物</param>
    public void UpdateCurWave(int num)
    {
        curWave += num;
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveText(curWave, maxWave);
    }

    ///// <summary>
    ///// 更新场景中怪物数量
    ///// </summary>
    ///// <param name="num"></param>
    //public void UpdateEnemyNum(int num)
    //{
    //    curEnemyNum += num;
    //}

    public void AddEnemy(Enemy enemy)
    {
        enemyList.Add(enemy);
    }
    public void RemoveEnemy(Enemy enemy)
    {
        enemyList.Remove(enemy);
    }
    /// <summary>
    /// 找到炮台攻击范围内的单个怪物
    /// </summary>
    /// <param name="pos">炮台的位置</param>
    /// <param name="range">炮台的攻击范围</param>
    /// <returns></returns>
    public Enemy FindSignalEnemy(Vector3 pos,int range)
    {
        for(int i = 0; i < enemyList.Count; i++)
        {
            if (!enemyList[i].isDead && Vector3.Distance(pos, enemyList[i].transform.position) <= range)
            {
                return enemyList[i];
            }
        }
        return null;
    }
    /// <summary>
    /// 找到炮台攻击范围内的所有怪物
    /// </summary>
    /// <param name="pos">炮台的位置</param>
    /// <param name="range">炮台的攻击范围</param>
    /// <returns></returns>
    public List<Enemy> FindMultiEnemys(Vector3 pos, int range)
    {
        List<Enemy> enemys = new List<Enemy>();

        for (int i = 0; i < enemyList.Count; i++)
        {
            if (!enemyList[i].isDead && Vector3.Distance(pos, enemyList[i].transform.position) <= range)
            {
                enemys.Add(enemyList[i]);
            }
        }
        return enemys;
    }

    /// <summary>
    /// 清空数据，避免影响下一个关卡
    /// </summary>
    public void Clear()
    {
        enemyCreatePoints.Clear();
        enemyList.Clear();
        curWave = maxWave = 0;
        enemyCreatePoints = null;
    }
}
