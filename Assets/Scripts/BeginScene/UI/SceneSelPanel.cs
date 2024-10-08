using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.SceneManagement.SceneHierarchyHooks;

public class SceneSelPanel : PanelBase
{
    public Button startBtn;
    public Button backBtn;
    public Button leftBtn;
    public Button rightBtn;

    public Image sceneImage;
    public Text sceneNameText;
    public Text sceneDescText;

    private int curSceneId;
    private SceneInfo curSceneInfo;

    protected override void Init()
    {
        startBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel();
            //异步切换场景
            AsyncOperation ao = SceneManager.LoadSceneAsync(curSceneInfo.sceneName);
            //场景加载完毕后
            ao.completed += (obj)=>{
                //初始化关卡
                GameLevelMgr.Instance.InitInfo(curSceneInfo);
            };
            
        });
        backBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel();
            UIManager.Instance.ShowPanel<RoleSelPanel>();
        });
        leftBtn.onClick.AddListener(() =>
        {
            curSceneId--;
            if(curSceneId < 0)
            {
                curSceneId = GameDataMgr.Instance.sceneInfoList.Count - 1;
            }
            UpdateSceneInfo();
            });
        rightBtn.onClick.AddListener(() =>
        {
            curSceneId++;
            if (curSceneId >= GameDataMgr.Instance.sceneInfoList.Count)
            {
                curSceneId = 0;
            }
            UpdateSceneInfo();
        });
        UpdateSceneInfo();
    }

    private void UpdateSceneInfo()
    {
        curSceneInfo = GameDataMgr.Instance.sceneInfoList[curSceneId];
        sceneImage.sprite = Resources.Load<Sprite>(curSceneInfo.imgPath);
        sceneNameText.text = "名称：" + curSceneInfo.name;
        sceneDescText.text = "描述：" + curSceneInfo.desc;
    }
}
