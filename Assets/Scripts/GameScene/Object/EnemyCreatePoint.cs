using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreatePoint : MonoBehaviour
{
    public int maxWave; //怪物最多波数
    public int enemyNumOneWave; //一波有多少怪物
    private int curNotCreateNumOneWave;  //当前波数还有多少怪物没有创建

    public List<int> enemyIds;  //怪物id 表示可以创建多少种不同的怪物
    private int curWaveEnemyId;  //记录当前波要创建什么id的怪物

    public float createEnemyDelayTime;  //单只怪物创建间隔时间
    public float createWaveDelayTime;  //每波怪物生成间隔时间

    public float firstCreateWaveDelayTime;  //第一波怪物创建的间隔时间

    // Start is called before the first frame update
    void Start()
    {
        Invoke("CreateWave", firstCreateWaveDelayTime);

        GameLevelMgr.Instance.AddEnemyCreatePoint(this);
        GameLevelMgr.Instance.UpdateMaxWave(maxWave);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateWave()
    {
        curWaveEnemyId = enemyIds[Random.Range(0,enemyIds.Count)];    
        curNotCreateNumOneWave = enemyNumOneWave;

        CreateEnemy();

        --maxWave;
        //通知关卡管理器出了一波怪物
        GameLevelMgr.Instance.UpdateCurWave(1);
    }

    /// <summary>
    /// 创建怪物
    /// </summary>
    private void CreateEnemy()
    {
        //记录怪物数据
        EnemyInfo info = GameDataMgr.Instance.enemyInfoList[curWaveEnemyId-1];
        //创建对象
        GameObject enemyObj = Instantiate(Resources.Load<GameObject>(info.resPath), transform.position, Quaternion.identity);
        //初始化
        Enemy enemy = enemyObj.AddComponent<Enemy>();
        enemy.InitInfo(info);

        //通知场景管理器，怪物数量加1
        //GameLevelMgr.Instance.UpdateEnemyNum(1);        
        GameLevelMgr.Instance.AddEnemy(enemy);

        //还未创建的怪物数量 减少
        --curNotCreateNumOneWave;
        if(curNotCreateNumOneWave<=0)
        {
            if (maxWave > 0)
                Invoke("CreateWave", createWaveDelayTime);
        }
        else
        {
            createEnemyDelayTime = Random.Range(1, 5);
            Invoke("CreateEnemy", createEnemyDelayTime);
        }
    
    }

    /// <summary>
    /// 检测所有怪物是否生成完毕
    /// </summary>
    /// <returns></returns>
    public bool AllEnemyCreateOver()
    {
        return curNotCreateNumOneWave == 0 && maxWave == 0;
    }
}
