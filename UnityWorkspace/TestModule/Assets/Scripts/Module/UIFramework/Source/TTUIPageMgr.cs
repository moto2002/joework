using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TinyTeam.UI;

public class TTUIPageMgr
{
    private static TTUIPageMgr m_instance;
    public static TTUIPageMgr Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new TTUIPageMgr();
            return m_instance;
        }
    }

    public TTUIRoot m_uiRoot;
    public TTUIPageMgr()
    {
        this.m_uiRoot = new TTUIRoot();
    }

    //all pages with the union type
    private Dictionary<string, TTUIPage> m_allPages;
    public Dictionary<string, TTUIPage> allPages
    { get { return m_allPages; } }


    //control 1>2>3>4>5 each page close will back show the previus page.
    private List<TTUIPage> m_currentPageNodes;
    public List<TTUIPage> currentPageNodes
    { get { return m_currentPageNodes; } }

    #region  api

    public bool CheckIfNeedBack(TTUIPage page)
    {
        return page != null && page.CheckIfNeedBack();
    }

    /// <summary>
    /// make the target node to the top.
    /// </summary>
    public void PopNode(TTUIPage page)
    {
        if (m_currentPageNodes == null)
        {
            m_currentPageNodes = new List<TTUIPage>();
        }

        if (page == null)
        {
            Debug.LogError("[UI] page popup is null.");
            return;
        }

        //sub pages should not need back.
        if (CheckIfNeedBack(page) == false)
        {
            return;
        }

        bool _isFound = false;
        for (int i = 0; i < m_currentPageNodes.Count; i++)
        {
            if (m_currentPageNodes[i].Equals(page))
            {
                m_currentPageNodes.RemoveAt(i);
                m_currentPageNodes.Add(page);
                _isFound = true;
                break;
            }
        }

        //if dont found in old nodes
        //should add in nodelist.
        if (!_isFound)
        {
            m_currentPageNodes.Add(page);
        }

        //after pop should hide the old node if need.
        HideOldNodes();
    }

    public void HideOldNodes()
    {
        if (m_currentPageNodes.Count < 0) return;
        TTUIPage topPage = m_currentPageNodes[m_currentPageNodes.Count - 1];
        if (topPage.mode == UIMode.HideOther)
        {
            //form bottm to top.
            for (int i = m_currentPageNodes.Count - 2; i >= 0; i--)
            {
                m_currentPageNodes[i].Hide();
            }
        }
    }

    public void ClearNodes()
    {
        m_currentPageNodes.Clear();
    }

    private void ShowPage<T>(Action callback, object pageData, bool isAsync) where T : TTUIPage, new()
    {
        Type t = typeof(T);
        string pageName = t.ToString();

        if (m_allPages != null && m_allPages.ContainsKey(pageName))
        {
            ShowPage(pageName, m_allPages[pageName], callback, pageData, isAsync);
        }
        else
        {
            T instance = new T();
            ShowPage(pageName, instance, callback, pageData, isAsync);
        }
    }

    private void ShowPage(string pageName, TTUIPage pageInstance, Action callback, object pageData, bool isAsync)
    {
        if (string.IsNullOrEmpty(pageName) || pageInstance == null)
        {
            Debug.LogError("[UI] show page error with :" + pageName + " maybe null instance.");
            return;
        }

        if (m_allPages == null)
        {
            m_allPages = new Dictionary<string, TTUIPage>();
        }

        TTUIPage page = null;
        if (m_allPages.ContainsKey(pageName))
        {
            page = m_allPages[pageName];
        }
        else
        {
            m_allPages.Add(pageName, pageInstance);
            page = pageInstance;
        }

        //if active before,wont active again.
        //if (page.isActive() == false)
        {
            //before show should set this data if need. maybe.!!
            page.m_data = pageData;

            if (isAsync)
                page.Show(callback);
            else
                page.Show();
        }
    }

    /// <summary>
    /// Sync Show Page
    /// </summary>
    public void ShowPage<T>() where T : TTUIPage, new()
    {
        ShowPage<T>(null, null, false);
    }

    /// <summary>
    /// Sync Show Page With Page Data Input.
    /// </summary>
    public void ShowPage<T>(object pageData) where T : TTUIPage, new()
    {
        ShowPage<T>(null, pageData, false);
    }

    public void ShowPage(string pageName, TTUIPage pageInstance)
    {
        ShowPage(pageName, pageInstance, null, null, false);
    }

    public void ShowPage(string pageName, TTUIPage pageInstance, object pageData)
    {
        ShowPage(pageName, pageInstance, null, pageData, false);
    }

    /// <summary>
    /// Async Show Page with Async loader bind in 'TTUIBind.Bind()'
    /// </summary>
    public void ShowPage<T>(Action callback) where T : TTUIPage, new()
    {
        ShowPage<T>(callback, null, true);
    }

    public void ShowPage<T>(Action callback, object pageData) where T : TTUIPage, new()
    {
        ShowPage<T>(callback, pageData, true);
    }

    /// <summary>
    /// Async Show Page with Async loader bind in 'TTUIBind.Bind()'
    /// </summary>
    public void ShowPage(string pageName, TTUIPage pageInstance, Action callback)
    {
        ShowPage(pageName, pageInstance, callback, null, true);
    }

    public void ShowPage(string pageName, TTUIPage pageInstance, Action callback, object pageData)
    {
        ShowPage(pageName, pageInstance, callback, pageData, true);
    }

    /// <summary>
    /// 隐藏当前页面，显示上一层页面
    /// </summary>
    public void HidePage()
    {
        //Debug.Log("Back&Close PageNodes Count:" + m_currentPageNodes.Count);

        if (m_currentPageNodes == null || m_currentPageNodes.Count <= 1) return;

        TTUIPage closePage = m_currentPageNodes[m_currentPageNodes.Count - 1];
        m_currentPageNodes.RemoveAt(m_currentPageNodes.Count - 1);
        closePage.Hide();

        //show older page.
        //TODO:Sub pages.belong to root node.
        if (m_currentPageNodes.Count > 0)
        {
            TTUIPage page = m_currentPageNodes[m_currentPageNodes.Count - 1];
            if (page.isAsyncUI)
                ShowPage(page.name, page, null);
            else
                ShowPage(page.name, page);
        }
    }

    /// <summary>
    /// 隐藏指定的页面
    /// </summary>
    public void HidePage(TTUIPage target)
    {
        if (target == null || target.isActive() == false) return;

        //如果这个是nodes里面的最顶部
        if (m_currentPageNodes != null && m_currentPageNodes.Count >= 1 && m_currentPageNodes[m_currentPageNodes.Count - 1] == target)
        {
            m_currentPageNodes.RemoveAt(m_currentPageNodes.Count - 1);
            target.Hide();

            //show older page.
            //TODO:Sub pages.belong to root node.
            if (m_currentPageNodes.Count > 0)
            {
                TTUIPage page = m_currentPageNodes[m_currentPageNodes.Count - 1];
                if (page.isAsyncUI)
                    ShowPage(page.name, page, null);
                else
                    ShowPage(page.name, page);
            }
        }
        else if (target.CheckIfNeedBack())
        {
            for (int i = 0; i < m_currentPageNodes.Count; i++)
            {
                if (m_currentPageNodes[i] == target)
                {
                    m_currentPageNodes.RemoveAt(i);
                    target.Hide();
                    break;
                }
            }
        }

        target.Hide();
    }

    public void HidePage<T>() where T : TTUIPage
    {
        Type t = typeof(T);
        string pageName = t.ToString();

        if (m_allPages != null && m_allPages.ContainsKey(pageName))
        {
            HidePage(m_allPages[pageName]);
        }
        else
        {
            Debug.LogError(pageName + "havnt show yet!");
        }
    }

    public void HidePage(string pageName)
    {
        if (m_allPages != null && m_allPages.ContainsKey(pageName))
        {
            HidePage(m_allPages[pageName]);
        }
        else
        {
            Debug.LogError(pageName + " havnt show yet!");
        }
    }

    /// <summary>
    /// 销毁指定页面
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void ClosePage<T>() where T : TTUIPage
    {
        Type t = typeof(T);
        string pageName = t.ToString();

        if (m_allPages != null && m_allPages.ContainsKey(pageName))
        {
            TTUIPage tempPage = m_allPages[pageName];
            if (m_currentPageNodes.Contains(tempPage))
            {
                m_currentPageNodes.Remove(tempPage);
            }
            m_allPages.Remove(pageName);
            tempPage.Close();
        }
        else
        {
            Debug.Log(pageName + ":havnt show yet!");
        }
    }

   
    #endregion

}
