using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

/// <summary>
/// 下载任务
/// </summary>
public class DownloadTask
{
    public string Url { get; set; }//路径

    public string FileName { get; set; }//文件名

    public string MD5 { get; set; }//md5值

    public int Size { get; set; }//文件大小

    #region ...
    //    public bool bDownloadAgain = false;//是否重新下载
    //
    //    public bool bFineshed = false;//是否完成

    //    public Action<long> BytesReceived { get; set; }
    //
    //    public Action<Exception> Error { get; set; }
    //
    //    public Action Finished { get; set; }
    //
    //    public Action<int> Progress { get; set; }
    //
    //    public Action<long> TotalBytesToReceive { get; set; }
    //
    //    public Action<int, long, long> TotalProgress { get; set; }
    //
    //    public void OnBytesReceived(long size)
    //    {
    //        if (this.BytesReceived != null)
    //        {
    //            this.BytesReceived(size);
    //        }
    //    }
    //
    //    public void OnError(Exception ex)
    //    {
    //        if (this.Error != null)
    //        {
    //            this.Error(ex);
    //        }
    //    }
    //
    //    public void OnFinished()
    //    {
    //        if (this.Finished != null)
    //        {
    //            this.Finished();
    //        }
    //    }
    //
    //    public void OnProgress(int p)
    //    {
    //        if (this.Progress != null)
    //        {
    //            this.Progress(p);
    //        }
    //    }
    //
    //    public void OnTotalBytesToReceive(long size)
    //    {
    //        if (this.TotalBytesToReceive != null)
    //        {
    //            this.TotalBytesToReceive(size);
    //        }
    //    }
    //
    //    public void OnTotalProgress(int p, long totalSize, long receivedSize)
    //    {
    //        if (this.TotalProgress != null)
    //        {
    //            this.TotalProgress(p, totalSize, receivedSize);
    //        }
    //    }
    #endregion

}

/// <summary>
/// 下载器
/// </summary>
public class DownloadMgr
{
    private static DownloadMgr instance;
    public static DownloadMgr GetInstance()
    {
        if (instance == null)
            instance = new DownloadMgr();
        return instance;
    }

    private readonly WebClient mWebClient = new WebClient();
    private Action<string> asynDownloadTxtCallBack;//异步下载txt的完成的回调
    private Action<string, int, int, int> downloadProgressChangedCallBack;//下载进度的回调
    private Action<string> oneTaskFinished;//一个任务完成的回调

    public DownloadMgr()
    {
        this.mWebClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.webClient_DownloadProgressChanged);
        this.mWebClient.DownloadFileCompleted += new AsyncCompletedEventHandler(this.webClient_DownloadFileCompleted);
        this.mWebClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.webClient_DownloadStringCompleted);
    }

    public void InitDownloadCallBacks(Action<string> _asynDownloadTxtCallBack, Action<string, int, int, int> _downloadProgressChangedCallBack, Action<string> _oneTaskFinished)
    {
        this.asynDownloadTxtCallBack = _asynDownloadTxtCallBack;
        this.downloadProgressChangedCallBack = _downloadProgressChangedCallBack;
        this.oneTaskFinished = _oneTaskFinished;
    }

    private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        string url = e.UserState.ToString();
        int progress = e.ProgressPercentage;
        int received = (int)e.BytesReceived;
        int total = (int)e.TotalBytesToReceive;

        if (this.downloadProgressChangedCallBack != null)
        {
            this.downloadProgressChangedCallBack(url, progress, received, total);
        }
        Thread.Sleep(100);
    }

    private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
    {
        if (e.Error != null)
        {
            this.HandleDownloadError(e.Error);
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
            return this.mWebClient.DownloadString(url);
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
            this.mWebClient.DownloadFileAsync(new Uri(task.Url), task.FileName, task.Url);
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
                Debug.LogError("-----------------Webclient ConnectFailure-------------");
            }
            else if (e.Message.Contains("(404) Not Found") || e.Message.Contains("403"))
            {
                Debug.LogError("-----------------WebClient NotFount-------------");

            }
            else if (e.Message.Contains("Disk full"))
            {
                Debug.LogError("-----------------WebClient Disk full-------------");
            }
            else if (e.Message.Contains("timed out") || e.Message.Contains("Error getting response stream"))
            {
                Debug.LogError("-----------------WebClient timed out-------------");
            }
            else if (e.Message.Contains("Sharing violation on path"))
            {
                Debug.LogError("-----------------WebClient Sharing violation on path-------------");
            }
            else
            {
                Debug.LogError(e.Message);
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
