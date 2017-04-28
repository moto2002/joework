using UnityEngine;
using System.Collections.Generic;
using System;

public class XPageMgr
{
    private static XPageMgr m_inst;
    public static XPageMgr Inst
    {
        get
        {
            if (m_inst == null)
                m_inst = new XPageMgr();
            return m_inst;
        }
    }

    //public Stack<XPage> m_pagePool = new Stack<XPage>();//页面的池子，栈，先进后出
    public XPage currShowXPage;//当前正打开的界面
    public Dictionary<string, XPage> m_pageDic = new Dictionary<string, XPage>();

    /// <summary>
    /// 获取所有面板
    /// </summary>
    /// <returns></returns>
    private List<XPage> GetAllPages()
    {
        return new List<XPage>(m_pageDic.Values);
    }

   /// <summary>
   /// 检查面板打开类型
   /// </summary>
   /// <param name="currXPage"></param>
    private void CheckPageMode(XPage currXPage)
    {
        if (currXPage.m_pageMode == EPageMode.DoNothing)
        {

        }
        else if (currXPage.m_pageMode == EPageMode.HideOther)
        {

        }
        else if (currXPage.m_pageMode == EPageMode.NeedBack)
        {

        }
        else if (currXPage.m_pageMode == EPageMode.NoNeedBack)
        {

        }

    }

    /// <summary>
    /// 检测面板是否在队列里
    /// </summary>
    /// <param name="pageName"></param>
    /// <returns></returns>
    private bool CheckPageExist(string pageName)
    {
        if (m_pageDic.ContainsKey(pageName))
        {
            return true;
        }
        else
        {
            Debug.LogError(pageName + "is not in pageDic");
            return false;
        }
    }

    /// <summary>
    /// 用相对路径获取面板名称
    /// </summary>
    /// <param name="pageLoadPath"></param>
    /// <returns></returns>
    private string GetPageName(string pageLoadPath)
    {
        string pageName = pageLoadPath.Substring(pageLoadPath.LastIndexOf("/") + 1);
        return pageName;
    }

    /// <summary>
    /// 打开面板
    /// </summary>
    /// <param name="isSync">是否同步加载</param>
    /// <param name="pageLoadPath">加载的相对路径</param>
    public void ShowPage(bool isSync, string pageLoadPath)
    {
        string pageName = GetPageName(pageLoadPath);

        bool isExist = CheckPageExist(pageName);

        XPage currXPage = null;
        if (isExist)
        {
            currXPage = m_pageDic[pageName];

            CheckPageMode(currXPage);

            currXPage.Rest();

            currShowXPage = currXPage;
        }
        else
        {
            //add
            currXPage = new XPage(pageName, pageLoadPath);
            currXPage.Awake();
            XPageLoadBind.Bind(currXPage);
            if (isSync)
            {
                currXPage.LoadSync((go) =>
                {
                    CheckPageMode(currXPage);

                    currXPage.Start();

                    m_pageDic.Add(pageName, currXPage);
                });
            }
            else
            {
                currXPage.LoadAsync((go) =>
                {
                    currXPage.Awake();

                    CheckPageMode(currXPage);

                    m_pageDic.Add(pageName, currXPage);
                });
            }
        }
    }

    /// <summary>
    /// 销毁所有面板
    /// </summary>
    public void CloseAllPages()
    {
        List<XPage> allPages = GetAllPages();
        for (int i = 0; i < allPages.Count; i++)
        {
            allPages[i].Destroy();
        }

        m_pageDic.Clear();
        m_pageDic = null;
    }

    /// <summary>
    /// 销毁当前正打开的面板
    /// </summary>
    public void CloseCurrPages()
    {
        if (currShowXPage != null)
        {
            currShowXPage.Destroy();
            m_pageDic.Remove(currShowXPage.m_pageName);
        }
        else
        {
            Debug.LogError("currShowPage is null");
        }
    }

    /// <summary>
    /// 销毁指定面板
    /// </summary>
    /// <param name="pageName"></param>
    public void ClosePage(string pageName)
    {
        if (CheckPageExist(pageName))
        {
            m_pageDic[pageName].Destroy();
            m_pageDic.Remove(pageName);
        }
    }

}
