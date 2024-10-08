using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //属性初始化
    private int atk;
    public int money;
    private float roundSpeed = 50;

    private Animator animator;

    public Transform shootPoint;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("HSpeed", Input.GetAxis("Horizontal"));
        animator.SetFloat("VSpeed", Input.GetAxis("Vertical"));

        transform.Rotate(Vector3.up,Input.GetAxis("Mouse X")*roundSpeed*Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetLayerWeight(animator.GetLayerIndex("Crouch Layer"), 1);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetLayerWeight(animator.GetLayerIndex("Crouch Layer"), 0);
        }
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("Roll");
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Atk");
        }

    }

    public void InitPlayerInfo(int atk,int money)
    {
        this.atk = atk;
        this.money = money;

        UpdateMoney();
    }

    /// <summary>
    /// 刀攻击事件
    /// </summary>
    public void KnifeEvent()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward + transform.up, 1, 1 << LayerMask.NameToLayer("Enemy"));

        GameDataMgr.Instance.PlaySound("Music/Knife");
        
        for (int i = 0; i < colliders.Length; i++)
        {
            //获取碰撞到的对象上的怪物脚本，使其受伤
            Enemy enemy = colliders[i].gameObject.GetComponent<Enemy>();
            if(enemy != null&&!enemy.isDead)
            {
                enemy.GetHit(this.atk);
                //避免一次性伤害到其他敌人
                break;
            }
        }
    
    }


    /// <summary>
    /// 枪射击事件
    /// </summary>
    public void ShootEvent()
    {
        RaycastHit[] hits = Physics.RaycastAll(new Ray(shootPoint.position, transform.forward), 1000, 1 << LayerMask.NameToLayer("Enemy"));

        GameDataMgr.Instance.PlaySound("Music/Gun");

        for (int i = 0; i < hits.Length; i++)
        {
            //获取碰撞到的对象上的怪物脚本，使其受伤
            Enemy enemy = hits[i].collider.gameObject.GetComponent<Enemy>();
            if (enemy != null && !enemy.isDead)
            {
                //创建特效
                GameObject effectObj = Instantiate(Resources.Load<GameObject>(GameDataMgr.Instance.curSelRole.hitEffectPath));
                effectObj.transform.position = hits[i].point;
                effectObj.transform.rotation = Quaternion.LookRotation(hits[i].normal);
                GameObject.Destroy(effectObj, 1);

                enemy.GetHit(this.atk);
                //避免一次性伤害到其他敌人
                break;
            }
        }
    }

    public void UpdateMoney()
    {
        UIManager.Instance.GetPanel<GamePanel>().UpdateMoneyText(money); 
    }

    public void AddMoney(int money)
    {
        this.money += money;
        UpdateMoney();
    }
}
