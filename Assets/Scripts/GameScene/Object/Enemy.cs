using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    private Animator animator;  //动画组件  
    private NavMeshAgent agent; //寻路组件
    private EnemyInfo enemyInfo;    //不改变的基础数据

    private int curHp;
    public bool isDead = false;

    private float frontAtkTime; //上一次攻击的时间节点

    private float stopDistance = 5; //停止距离

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
     if(isDead) return;

        //animator.SetBool("Run", agent.velocity != Vector3.zero);
        if (Vector3.Distance(transform.position, ProtectedTower.Instance.transform.position) < stopDistance && Time.time- frontAtkTime >= enemyInfo.atkOffset)
        {
            animator.SetTrigger("Atk");
            frontAtkTime = Time.time;
        }
    }

    public void InitInfo(EnemyInfo info)
    {
        enemyInfo = info;
        print(enemyInfo.animPath);
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(enemyInfo.animPath);
        curHp = info.hp;
        //速度与加速度相同 希望没有明显的加速运动 使其为匀速运动
        agent.speed = agent.acceleration = info.moveSpeed;
        //转向速度
        agent.angularSpeed = info.roundSpeed;
    }

    public void GetHit(int dmg)
    {
        if (isDead)
            return;

        curHp -= dmg;
        animator.SetTrigger("Damage");

        if(curHp < 0)
        {
            Dead();
        }
        else
        {
            GameDataMgr.Instance.PlaySound("Music/Wound");
        }
    }

    public void Dead()
    {
        isDead = true;
        //agent.isStopped = true;
        agent.enabled = false;
        animator.SetBool("Dead",true);
        GameDataMgr.Instance.PlaySound("Music/dead");

        GameLevelMgr.Instance.player.AddMoney(100);
    }

    /// <summary>
    /// 死亡动画完毕后调用
    /// </summary>
    public void DeadEvent()
    {
        //GameLevelMgr.Instance.UpdateEnemyNum(-1);   //通知场景管理器，怪物数量减1
        GameLevelMgr.Instance.RemoveEnemy(this);   //通知场景管理器，怪物数量减1
        Destroy(gameObject);//销毁自己
        GameLevelMgr.Instance.CheckSuccess();   //检测是否胜利

        if(GameLevelMgr.Instance.CheckSuccess())
        {
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();
            panel.UpdateText(GameDataMgr.Instance.playerData.money, true);
        }
    }

    /// <summary>
    /// 出生后移动
    /// </summary>
    public void BornOver()
    {
        //设置路径
        agent.SetDestination(ProtectedTower.Instance.transform.position);
        //agent.stoppingDistance = stopDistance;

        //播放移动动画
        animator.SetBool("Run", true);
    }

    /// <summary>
    /// 伤害检测
    /// </summary>
    public void AtkEvent()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward + transform.up, 1, 1 << LayerMask.NameToLayer("ProtectedTower"));

        GameDataMgr.Instance.PlaySound("Music/Eat");

        for (int i = 0; i < colliders.Length; i++)
        {
            if(ProtectedTower.Instance.gameObject == colliders[i].gameObject)
            {
                ProtectedTower.Instance.GetHit(enemyInfo.atk);
            }
        }
    }
}
