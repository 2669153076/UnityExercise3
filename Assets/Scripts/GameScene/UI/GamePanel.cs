using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : PanelBase
{
    public Image hpImg;
    public Text hpText;

    public float hpImgLength;

    public Text waveText;
    public Text moneyText;

    public Button quitBtn;

    public Transform towersTrans;   //造塔按钮的父对象
    public List<TowerBtn> towerBtnList = new List<TowerBtn>();

    private TowerCreatePoint curSelTowerPoint;

    private bool checkInput;    //是否检测造塔输入

    protected override void Init()
    {
        hpImgLength = (hpImg.transform as RectTransform).rect.width;


        quitBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel();
            SceneManager.LoadScene("BeginScene");
            UIManager.Instance.ShowPanel<BeginPanel>();

            //其他
            
        });

        //隐藏下方造塔按钮
        towersTrans.gameObject.SetActive(false);
        //锁定并隐藏鼠标
        Cursor.lockState = CursorLockMode.Confined;
    }

    /// <summary>
    /// 更新防御塔血量
    /// </summary>
    /// <param name="curHp"></param>
    /// <param name="maxHp"></param>
    public void UpdateHpImp(int curHp,int maxHp)
    {
        hpText.text = curHp + "/" + maxHp;
        (hpImg.transform as RectTransform).offsetMax = new Vector2((float)(curHp-maxHp)/maxHp*hpImgLength, (hpImg.transform as RectTransform).offsetMax.y);
    }

    /// <summary>
    /// 更新敌人波数
    /// </summary>
    /// <param name="curWaveNum"></param>
    /// <param name="maxWaveNum"></param>
    public void UpdateWaveText(int curWaveNum,int maxWaveNum)
    {
        waveText.text = curWaveNum + "/" + maxWaveNum;
    }

    /// <summary>
    /// 更新当前金钱
    /// </summary>
    /// <param name="money"></param>
    public void UpdateMoneyText(int money)
    {
        moneyText.text = money.ToString();
    }

    /// <summary>
    /// 更新造塔相关UI
    /// </summary>
    /// <param name="point"></param>
    public void UpdateTowerInfo(TowerCreatePoint point)
    {
        curSelTowerPoint = point;

        if (curSelTowerPoint == null)
        {
            //不能造塔
            checkInput = false;
            towersTrans.gameObject.SetActive(false);
        }
        else
        {
            checkInput = true;
            towersTrans.gameObject.SetActive(true);

            if (curSelTowerPoint.towerInfo == null)
            {
                for (int i = 0; i < towerBtnList.Count; i++)
                {
                    towerBtnList[i].gameObject.SetActive(true);
                    towerBtnList[i].InitInfo(curSelTowerPoint.towerIds[i], "数字键" + (i + 1));
                }
            }
            else
            {
                for (int i = 0; i < towerBtnList.Count; i++)
                {
                    towerBtnList[i].gameObject.SetActive(false);
                }
                towerBtnList[1].gameObject.SetActive(true);
                towerBtnList[1].InitInfo(curSelTowerPoint.towerInfo.nextLevId, "空格键");
            }
        }
    }


    protected override void Update()
    {
        base.Update();

        if (!checkInput)
            return;

        if(curSelTowerPoint.towerInfo == null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                curSelTowerPoint.CreateTower(curSelTowerPoint.towerIds[0]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                curSelTowerPoint.CreateTower(curSelTowerPoint.towerIds[1]);

            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                curSelTowerPoint.CreateTower(curSelTowerPoint.towerIds[2]);
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                curSelTowerPoint.CreateTower(curSelTowerPoint.towerInfo.nextLevId);
            }

        }
    }
}
