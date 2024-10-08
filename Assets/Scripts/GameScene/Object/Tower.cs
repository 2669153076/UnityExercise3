using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Transform head;  
    public Transform shootPos;

    private float roundSpeed = 20;   //旋转速度

    private TowerInfo towerInfo;
    private Enemy target;   //攻击目标  
    private List<Enemy> targets;

    private float curTime;  //计时 计算攻击间隔时间
    private Vector3 enemyPos;//怪物位置


    // Update is called once per frame
    void Update()
    {
        if(towerInfo.atkType == 1)
        {
            //单体攻击

            //目标为空 目标死亡 目标超出攻击范围
            if (target == null || target.isDead || Vector3.Distance(transform.position, target.transform.position) > towerInfo.atkRange)
            {
                target = GameLevelMgr.Instance.FindSignalEnemy(transform.position, towerInfo.atkRange);
            }
            if (target == null)
                return;
            
            enemyPos = target.transform.position;
            enemyPos.y = head.position.y;   //修改Y轴，使炮台不要上下旋转

            head.rotation =Quaternion.Slerp(head.rotation,Quaternion.LookRotation(enemyPos-head.position), roundSpeed*Time.deltaTime);

            if (Vector3.Angle(head.forward, enemyPos - head.position) < 5 && Time.time - curTime >= towerInfo.atkDelayTime)
            {
                target.GetHit(towerInfo.atk);

                GameDataMgr.Instance.PlaySound("Music/Tower");

                GameObject effect = Instantiate(Resources.Load<GameObject>(towerInfo.effectPath),shootPos.position,shootPos.rotation);
                GameObject.Destroy(effect, 0.1f);

                curTime = Time.time;
            }
        }
        else if(towerInfo.atkType == 2)
        {
            //群体攻击
            targets = GameLevelMgr.Instance.FindMultiEnemys(transform.position, towerInfo.atkRange);
            if (targets.Count > 0 && Time.time - curTime >= towerInfo.atkDelayTime)
            {

                GameObject effect = Instantiate(Resources.Load<GameObject>(towerInfo.effectPath), transform.position, transform.rotation);
                GameObject.Destroy(effect, 0.1f);
                for (int i = 0; i < targets.Count; i++)
                {
                    targets[i].GetHit(towerInfo.atk);
                }
                //播放音效

                curTime = Time.time;
            }

        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(TowerInfo info)
    {
        towerInfo = info;
    }
}
