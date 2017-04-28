using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DownloadManager : MonoBehaviour, IDownloadManager
{
    private static IDownloadManager m_DownloadManager = null;
    private string m_Temp = string.Empty;

    public void AsyncDownload(string url, Action callback)
    {
    }

    public void AsyncDownloadFileAndSaveAs(string url, string localPath)
    {
    }

    private IEnumerator DownloadString(string url, Action<string> callback)
    {
        WWW iteratorVariable0 = new WWW(url);
        yield return iteratorVariable0;
        callback(iteratorVariable0.get_text());
    }

    public void SyncDownload(string url, Action<string> callback)
    {
        GameObject.Find("Driver").GetComponent<DownloadObserver>().StartCoroutine(this.DownloadString(url, callback));
    }

    public void SyncDownloadFileAndSaveAs(string url, string localPath)
    {
    }

    public string Buffer
    {
        set
        {
            this.m_Temp = value;
        }
    }

    public static IDownloadManager Singleton
    {
        get
        {
            if (null == m_DownloadManager)
            {
                m_DownloadManager = new DownloadManager();
                GameObject.Find("Driver").AddComponent<DownloadObserver>();
            }
            return m_DownloadManager;
        }
    }

}

