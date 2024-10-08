using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public abstract class PanelBase : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private float alphaSpeed = 10;
    private UnityAction hideCallback = null;    //面板隐藏后执行的函数

    //当前是否显示
    private bool isShow = false;

    protected virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if(_canvasGroup == null)
        {
            _canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Init();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isShow && _canvasGroup.alpha != 1)
        {
            _canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if (_canvasGroup.alpha >= 1)
            {
                _canvasGroup.alpha = 1;

            }
        }
        else if (!isShow && _canvasGroup.alpha != 0)
        {
            _canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if (_canvasGroup.alpha <= 0)
            {
                _canvasGroup.alpha = 0;
                hideCallback?.Invoke();
            }
        }
    }


    protected abstract void Init();

    public virtual void ShowSelf()
    {
        _canvasGroup.alpha = 0;
        isShow = true;
    }

    public virtual void HideSelf(UnityAction callback)
    {
        _canvasGroup.alpha = 1;
        isShow = false;
        hideCallback = callback;
    }

    public virtual void OnEnter()
    {
        _canvasGroup.interactable = true;
    }
    public virtual void OnExit()
    {
        _canvasGroup.interactable = false;
    }
}
