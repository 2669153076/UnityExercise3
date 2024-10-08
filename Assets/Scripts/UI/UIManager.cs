using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private static UIManager instance = new UIManager();
    public static UIManager Instance=>instance;
    private UIManager() {
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTransform = canvas.transform;

        GameObject.DontDestroyOnLoad(canvas);
    }

    private Dictionary<string, PanelBase> panelDic = new Dictionary<string, PanelBase>();
    private Stack<PanelBase> panelStack = new Stack<PanelBase>();

    private Transform canvasTransform;

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T ShowPanel<T>() where T : PanelBase
    {
        if(panelStack.Count>0)
        {
            PanelBase currentPanel=panelStack.Peek();
            currentPanel.OnExit();
        }

        string panelName = typeof(T).Name;  //要显示的面板名称
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));
        panelObj.transform.SetParent(canvasTransform, false);
        //panelObj.gameObject.name = panelName;


        T newPanel = panelObj.GetComponent<T>();

        panelDic.Add(panelName, newPanel);
        panelStack.Push(newPanel);
        newPanel.ShowSelf();
        newPanel.OnEnter();

        return newPanel;
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="isFade">是否淡出完毕后删除面板</param>
    public void HidePanel(bool isFade = true) 
    {
        if(panelStack.Count > 0)
        {
            PanelBase currentPanel = panelStack.Pop();
            if(isFade)
            {
                currentPanel.HideSelf(() => 
                {
                    GameObject.Destroy(currentPanel.gameObject);
                    panelDic.Remove(currentPanel.GetType().Name);
                });
            }
            else
            {
                GameObject.Destroy(currentPanel.gameObject);
                panelDic.Remove(currentPanel.GetType().Name);
            }

            if(panelStack.Count>0 )
            {
                PanelBase previousPanel = panelStack.Peek();
                previousPanel.OnEnter();
            }
        }
    }


    public T GetPanel<T>() where T : PanelBase
    {
        string panelName = typeof(T).Name;

        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }

        return null;
    }
}
