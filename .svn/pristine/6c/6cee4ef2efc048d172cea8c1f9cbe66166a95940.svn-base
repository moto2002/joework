using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using ShiHuanJue.Debuger;

/// <summary>
/// 下载器
/// </summary>
public class DownloadManager
{
    private static DownloadManager instance;
    public static DownloadManager GetInstance()
    {
        if (instance == null)
            instance = new DownloadManager();
        return instance;
    }

    private readonly WebClient mWebClient = new WebClient();
    public Action<string> asynDownloadTxtCallBack;//异步下载txt的完成的回调
    public Action<string,int,int,int> downloadProgressChangedCallBack;//下载进度的回调
    public Action<string> oneTaskFinished;//一个任务完成的回调

    public DownloadManager()
    {
        this.mWebClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.webClient_DownloadProgressChanged);
        this.mWebClient.DownloadFileCompleted += new AsyncCompletedEventHandler(this.webClient_DownloadFileCompleted);
        this.mWebClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.webClient_DownloadStringCompleted);
    }

    private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        string url = e.UserState.ToString();
        int progress = e.ProgressPercentage;
        int received = (int)e.BytesReceived;
        int total = (int)e.TotalBytesToReceive;

        if (this.downloadProgressChangedCallBack != null)
        {
            this.downloadProgressChangedCallBack(url,progress,received,total);
        }
        Thread.Sleep(100);
    }

    private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
    {
        if (e.Error != null)
        {
            this.HandleDownloadError(e.Error);
            //这里要弹出面板给用户，交给PanelMgr吧（未完成）
        }
        else
        {
            if (this.oneTaskFinished != null)
            {
                this.oneTaskFinished(e.UserState.ToString());
            }
        }
    }

    private void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
    {
        if (this.asynDownloadTxtCallBack != null)
        {
            this.asynDownloadTxtCallBack(e.Result);
        }
    }

    /// <summary>
    /// 同步下载Text,我这里用来下载version.txt
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public string DownLoadText(string url)
    {
        try
        {
            return this.mWebClient.DownloadString(url) ;
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            return string.Empty;
        }
    }


    /// <summary>
    /// 异步下载Text
    /// </summary>
    public void AsynDownLoadText(DownloadTask task)
    {
        try
        {
            this.mWebClient.DownloadStringAsync(new Uri(task.Url), task.Url);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    /// <summary>
    /// 异步下载file
    /// </summary>
    /// <param name="task"></param>
    public void AsynDownLoadFile(DownloadTask task)
    {
        try
        {
            this.mWebClient.DownloadFileAsync(new Uri(task.Url),task.FileName, task.Url);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }

    /// <summary>
    /// 显示下载异常情况
    /// </summary>
    /// <param name="e"></param>
    /// <param name="mycontinue"></param>
    /// <param name="again"></param>
    /// <param name="finished"></param>
    void HandleDownloadError(Exception e)
    {
        if (e != null)
        {
            if ((e.Message.Contains("ConnectFailure") || e.Message.Contains("NameResolutionFailure")) || e.Message.Contains("No route to host"))
            {
                LogHelper.LogError("-----------------Webclient ConnectFailure-------------");
            }
            else if (e.Message.Contains("(404) Not Found") || e.Message.Contains("403"))
            {
				LogHelper.LogError("-----------------WebClient NotFount-------------");

            }
            else if (e.Message.Contains("Disk full"))
            {
				LogHelper.LogError("-----------------WebClient Disk full-------------");
            }
            else if (e.Message.Contains("timed out") || e.Message.Contains("Error getting response stream"))
            {
				LogHelper.LogError("-----------------WebClient timed out-------------");
            }
            else if (e.Message.Contains("Sharing violation on path"))
            {
				LogHelper.LogError("-----------------WebClient Sharing violation on path-------------");
            }
            else
            {
				LogHelper.LogError(e.Message);
            }
        }
    }

    /// <summary>
    /// 关闭
    /// </summary>
    public void Dispose()
    {
        this.mWebClient.CancelAsync();
        this.mWebClient.Dispose();
    }
}

