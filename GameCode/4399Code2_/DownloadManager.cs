using Mogo.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DownloadManager : IDownloadManager
{
    private Action m_AfterDownloadAllFiles = null;
    public DownloadUnit[] m_DownloadLists = null;
    private static IDownloadManager m_DownloadManager = null;
    public DownloadObserver m_DownloadObserver = null;
    public int m_nCurrent = 0;

    public void AdvancedAsyncDownloadFileAndSaveAs(DownloadUnit[] downloadLists, Action callback)
    {
        this.m_DownloadLists = downloadLists;
        this.m_AfterDownloadAllFiles = callback;
        this.Next();
    }

    public void AsyncDownload(string url, Action callback)
    {
        LoggerHelper.Info("Download Manager have not implemented AsyncDownload yet", true);
    }

    public void AsyncDownloadFileAndSaveAs(string url, string localPath)
    {
    }

    private IEnumerator DownloadString(string url, Action<string> callback)
    {
        WWW iteratorVariable0 = new WWW(url);
        yield return iteratorVariable0;
        callback(iteratorVariable0.text);
    }

    public void Next()
    {
        if (this.m_DownloadLists.Length <= (1 + this.m_nCurrent))
        {
            this.m_DownloadLists = null;
            this.m_AfterDownloadAllFiles();
            this.m_AfterDownloadAllFiles = null;
        }
        else
        {
            this.m_nCurrent++;
        }
    }

    public void SyncDownload(string url, Action<string> callback)
    {
    }

    public void SyncDownloadFileAndSaveAs(string url, string localPath)
    {
    }

    public static IDownloadManager Singleton
    {
        get
        {
            if (null == m_DownloadManager)
            {
                m_DownloadManager = new DownloadManager();
                ((DownloadManager) m_DownloadManager).m_DownloadObserver = GameObject.Find("Driver").AddComponent<DownloadObserver>();
            }
            return m_DownloadManager;
        }
    }

}

