using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCreatePoint : MonoBehaviour
{

    private GameObject towerObj = null;    
    public TowerInfo towerInfo = null;

    public List<int> towerIds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateTower(int id)
    {
        TowerInfo info  = GameDataMgr.Instance.towerInfoList[id-1];
        if (info.money > GameLevelMgr.Instance.player.money)
        {
            return;
        }
        GameLevelMgr.Instance.player.AddMoney(-info.money);

        if(towerObj!= null)
        {
            Destroy(towerObj);
            towerObj = null;
        }
        towerObj = Instantiate(Resources.Load<GameObject>(info.resPath),transform.position,Quaternion.identity);
        towerObj.GetComponent<Tower>().InitInfo(info);

        towerInfo = info;

        if(towerInfo.nextLevId!=0)
            UIManager.Instance.GetPanel<GamePanel>().UpdateTowerInfo(this);
        else
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateTowerInfo(null);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (towerInfo != null && towerInfo.nextLevId == 0)
            return;
        UIManager.Instance.GetPanel<GamePanel>().UpdateTowerInfo(this);

    }

    private void OnTriggerExit(Collider other)
    {
        UIManager.Instance.GetPanel<GamePanel>().UpdateTowerInfo(null);
    }
}
