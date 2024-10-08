using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RoleSelPanel : PanelBase
{
    public Button leftBtn;  //上一个角色
    public Button rightBtn; //下一个角色
    public Button backBtn;  //返回按钮
    public Button startBtn; //开始按钮
    public Button unlockBtn;    //解锁按钮
    public Text unlockText; //解锁所需要的金钱
    public Text moneyText;  //当前拥有的金钱
    public Text nameText;   //游戏角色名字

    private Transform rolePos;  //游戏角色位置

    private GameObject roleObj; //当前角色物体
    private RoleInfo curRoleInfo;   //当前使用的角色数据
    private int curRoleIndex;   //当前使用的数据的索引

    protected override void Init()
    {
        rolePos = GameObject.Find("RolePos").transform;

        moneyText.text = GameDataMgr.Instance.playerData.money.ToString();

        startBtn.onClick.AddListener(() =>
        {
            //记录当前选择的角色
            GameDataMgr.Instance.curSelRole = curRoleInfo; 

            //隐藏自己 显示场景选择界面
            UIManager.Instance.HidePanel();
            UIManager.Instance.ShowPanel<SceneSelPanel>();
        });
        backBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel(true);
            Camera.main.GetComponent<CameraAnimator>().TurnRight(() =>
            {
                UIManager.Instance.ShowPanel<BeginPanel>();
            });
        });
        unlockBtn.onClick.AddListener(() =>
        {
            PlayerData playerData = GameDataMgr.Instance.playerData;

            if(playerData.money>=curRoleInfo.lockMoney)
            {
                playerData.money-=curRoleInfo.lockMoney;
                moneyText.text = playerData.money.ToString();
                playerData.role.Add(curRoleInfo.id);
                GameDataMgr.Instance.SavePlayerData();

                UpdateUnlockBtn();
                UIManager.Instance.ShowPanel<TipPanel>().UpdateTipText("购买成功！");
            }
            else
            {
                UIManager.Instance.ShowPanel<TipPanel>().UpdateTipText("金钱不足，购买失败！");
            }
        });
        leftBtn.onClick.AddListener(() =>
        {
            --curRoleIndex;
            if (curRoleIndex < 0)
            {
                curRoleIndex = GameDataMgr.Instance.roleInfoList.Count - 1;
            }
            UpdateHeroObj();
        });
        rightBtn.onClick.AddListener(() =>
        {
            ++curRoleIndex;
            if (curRoleIndex >= GameDataMgr.Instance.roleInfoList.Count)
            {
                curRoleIndex = 0;
            }
            UpdateHeroObj();
        });

        UpdateHeroObj();
        UpdateUnlockBtn();
    }

    /// <summary>
    /// 更新角色模型
    /// </summary>
    private void UpdateHeroObj()
    {
        if (roleObj != null)
        {
            Destroy(roleObj);
            roleObj = null;
        }

        curRoleInfo = GameDataMgr.Instance.roleInfoList[curRoleIndex];

        roleObj = Instantiate(Resources.Load<GameObject>(curRoleInfo.resPath), rolePos.position, rolePos.rotation, rolePos);

        //开始场景不需要 Player脚本
        if (roleObj.GetComponent<Player>() != null)
            Destroy(roleObj.GetComponent<Player>());

        nameText.text = curRoleInfo.name;

        UpdateUnlockBtn();
    
    }

    /// <summary>
    /// 更新解锁按钮
    /// </summary>
    private void UpdateUnlockBtn()
    {
        //如果该角色需要解锁的金钱大于0 并且 玩家并没有解锁该角色
        if (curRoleInfo.lockMoney > 0 && !GameDataMgr.Instance.playerData.role.Contains(curRoleInfo.id))
        {
            //显示解锁按钮、更新解锁需要的金钱
            unlockBtn.gameObject.SetActive(true);
            unlockText.text = "￥" + curRoleInfo.lockMoney;
            //隐藏开始按钮
            startBtn.gameObject.SetActive(false);
        }
        else
        {
            unlockBtn.gameObject.SetActive(false);
            startBtn.gameObject.SetActive(true);
        }
    }

    public override void HideSelf(UnityAction callback)
    {
        if(roleObj!= null)
        {
            DestroyImmediate(roleObj);
            roleObj = null;
        }
        base.HideSelf(callback);

        
    }
}
