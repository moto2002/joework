using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public enum EPageLevel
{
    Normal,
    Fixed,
    PopUp,
    None,      //独立的窗口
}

public enum EPageMode
{
    DoNothing,
    HideOther,     // 闭其他界面
    NeedBack,      // 点击返回按钮关闭当前,不关闭其他界面(需要调整好层级关系)
    NoNeedBack,    // 关闭TopBar,关闭其他界面,不加入backSequence队列
}

public class XPageLoadBind
{
    public static void Bind(XPage xPage)
    {

            //bind for your loader api to load UI.
            xPage.delegateSyncLoadUI = Resources.Load;
            //xPage.delegateAsyncLoadUI = UILoader.Load;
    }
}

public class XPage
{
    public string m_pageName;
    public string m_loadPath;
    public GameObject m_pageInst;
    public Transform m_pageTrans;
    public EPageLevel m_pageLevel = EPageLevel.None;
    public EPageMode m_pageMode = EPageMode.DoNothing;

    //delegate load ui function.
    public Func<string, Object> delegateSyncLoadUI = null;
    public Action<string, Action<Object>> delegateAsyncLoadUI = null;

    public XPage(string pageName,string loadPath)
    {
        m_pageName = pageName;
        m_loadPath = loadPath;
    }

    public void Awake()
    {
        Debug.LogError("call awake :(" + m_pageName + "Ctrl)");

        //设置level和mode

    }

    public void Start()
    {
        AnchorUIGameObject();
        Debug.LogError("call start :(" + m_pageName + "Ctrl)");
    }

    public void Rest()
    {
        Debug.LogError("call rest :(" + m_pageName + "Ctrl)");
    }

    public void Hide()
    {

    }

    public void Destroy()
    {
        Debug.LogError("call destroy :(" + m_pageName + "Ctrl)");
    }



    public void LoadSync(Action<GameObject> callback)
    {
        //1:instance UI
        if (this.m_pageInst == null && string.IsNullOrEmpty(m_loadPath) == false)
        {
            GameObject go = null;
            if (delegateSyncLoadUI != null)
            {
                Object o = delegateSyncLoadUI(m_loadPath);
                go = o != null ? GameObject.Instantiate(o) as GameObject : null;
            }
            else
            {
                go = GameObject.Instantiate(Resources.Load(m_loadPath)) as GameObject;
            }

            //protected.
            if (go == null)
            {
                Debug.LogError("[UI] Cant sync load your ui prefab.");
                return;
            }

            m_pageInst = go;
            m_pageTrans = go.transform;

            if (callback!=null)
                callback(go);
        }
        else
        {
            if (callback != null)
                callback(m_pageInst);
        }
    }

    public void LoadAsync(Action<GameObject> callback)
    {
        XPageRoot.Instance.StartCoroutine(AsyncShow(callback));
    }

    IEnumerator AsyncShow(Action<GameObject> callback)
    {
        //1:Instance UI
        //FIX:support this is manager multi gameObject,instance by your self.
        if (this.m_pageInst == null && string.IsNullOrEmpty(m_loadPath) == false)
        {
            GameObject go = null;
            bool _loading = true;
            delegateAsyncLoadUI(m_loadPath, (o) =>
            {
                go = o != null ? GameObject.Instantiate(o) as GameObject : null;
   
                _loading = false;

                m_pageInst = go;
                m_pageTrans = go.transform;

                if (callback != null)
                    callback(go);
            });

            float _t0 = Time.realtimeSinceStartup;
            while (_loading)
            {
                if (Time.realtimeSinceStartup - _t0 >= 10.0f)
                {
                    Debug.LogError("[UI] WTF async load your ui prefab timeout!");
                    yield break;
                }
                yield return null;
            }
        }
        else
        {
            if (callback != null)
                callback(m_pageInst);
        }
    }

    protected void AnchorUIGameObject()
    {
        if (XPageRoot.Instance == null || m_pageInst == null)
            return;

        GameObject ui = m_pageInst;

        //check if this is ugui or (ngui)?
        Vector3 anchorPos = Vector3.zero;
        Vector2 sizeDel = Vector2.zero;
        Vector3 scale = Vector3.one;
        if (ui.GetComponent<RectTransform>() != null)
        {
            anchorPos = ui.GetComponent<RectTransform>().anchoredPosition;
            sizeDel = ui.GetComponent<RectTransform>().sizeDelta;
            scale = ui.GetComponent<RectTransform>().localScale;
        }
        else
        {
            anchorPos = ui.transform.localPosition;
            scale = ui.transform.localScale;
        }

        EPageLevel type = this.m_pageLevel;
        if (type == EPageLevel.Fixed)
        {
            ui.transform.SetParent(XPageRoot.Instance.fixedRoot);
        }
        else if (type == EPageLevel.Normal)
        {
            ui.transform.SetParent(XPageRoot.Instance.normalRoot);
        }
        else if (type == EPageLevel.PopUp)
        {
            ui.transform.SetParent(XPageRoot.Instance.popupRoot);
        }


        if (ui.GetComponent<RectTransform>() != null)
        {
            ui.GetComponent<RectTransform>().anchoredPosition = anchorPos;
            ui.GetComponent<RectTransform>().sizeDelta = sizeDel;
            ui.GetComponent<RectTransform>().localScale = scale;
        }
        else
        {
            ui.transform.localPosition = anchorPos;
            ui.transform.localScale = scale;
        }
    }
}
