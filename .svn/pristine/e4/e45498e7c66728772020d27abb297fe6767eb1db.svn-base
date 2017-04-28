using ICSharpCode.SharpZipLib.GZip;
using Mogo.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine;

public class DownloadMgr
{
    private static DownloadMgr _mInstance;
    private const bool _showMsg = true;
    private readonly WebClient _webClient;
    public bool IsEditor = false;
    private const string LogPath = "logurl";
    public Action<int, int, string> TaskProgress;
    public List<DownloadTask> tasks = new List<DownloadTask>();

    static DownloadMgr()
    {
        BreakPoint = true;
    }

    public DownloadMgr()
    {
        this.IsEditor = Application.isEditor;
        this._webClient = new WebClient();
        if (!BreakPoint)
        {
            this._webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.webClient_DownloadProgressChanged);
            this._webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(this.webClient_DownloadFileCompleted);
        }
    }

    public bool AsynDownLoadText(DownloadTask task)
    {
        if (!this.ContainKey(task.Url))
        {
            this._webClient.DownloadStringAsync(new Uri(task.Url), task.Url);
            return true;
        }
        return false;
    }

    public void AsynDownLoadText(string url, Action<string> asynResult, Action OnError)
    {
        string u = this.GetRandomParasUrl(url);
        delegate {
            Action action = null;
            string result = this.DownLoadText(u);
            if (string.IsNullOrEmpty(result))
            {
                if (OnError != null)
                {
                    DriverLib.Invoke(OnError);
                }
            }
            else if (asynResult != null)
            {
                if (action == null)
                {
                    action = () => asynResult(result);
                }
                DriverLib.Invoke(action);
            }
        }.BeginInvoke(null, null);
    }

    public void CheckDownLoadList()
    {
        LoggerHelper.Debug("核对下载列表", true, 0);
        if (this.tasks.Count != 0)
        {
            int num = 0;
            foreach (DownloadTask task in this.tasks)
            {
                LoggerHelper.Debug("核对任务：" + task.FileName, true, 0);
                if (!(!task.bFineshed || task.bDownloadAgain))
                {
                    LoggerHelper.Debug("已经完成不用从下：" + task.FileName, true, 0);
                    num++;
                }
                else
                {
                    string directoryName = Path.GetDirectoryName(task.FileName);
                    if (!((directoryName == null) || Directory.Exists(directoryName)))
                    {
                        Directory.CreateDirectory(directoryName);
                        LoggerHelper.Debug("下载目录不存在，创建目录：" + directoryName, true, 0);
                    }
                    if (!BreakPoint)
                    {
                        LoggerHelper.Debug("新的下载或从新下载：" + task.FileName, true, 0);
                        this._webClient.DownloadFileAsync(new Uri(task.Url), task.FileName, task.Url);
                    }
                    else
                    {
                        LoggerHelper.Debug("断点下载：" + task.FileName, true, 0);
                        ThreadDownloadBreakPoint point = new ThreadDownloadBreakPoint(this, task);
                        new Thread(new ThreadStart(point.Download)).Start();
                        LoggerHelper.Debug("开始断点下载：" + task.FileName, true, 0);
                    }
                    break;
                }
            }
            if (num > (this.tasks.Count - 1))
            {
                LoggerHelper.Debug("下载的数据包数量已经达到要求的,删除所有任务和全部任务完成的回调", true, 0);
                this.tasks.Clear();
                this.tasks = null;
                if (this.AllDownloadFinished != null)
                {
                    this.AllDownloadFinished();
                    this.AllDownloadFinished = null;
                }
            }
        }
    }

    private bool ContainKey(string url)
    {
        return Enumerable.Any<DownloadTask>(this.tasks, (Func<DownloadTask, bool>) (task => (url == task.Url)));
    }

    public void Dispose()
    {
        this._webClient.CancelAsync();
        this._webClient.Dispose();
    }

    public bool DownloadFile(string url, string localPath, string bakPath)
    {
        bool flag;
        if (System.IO.File.Exists(bakPath))
        {
            System.IO.File.Delete(bakPath);
        }
        if (System.IO.File.Exists(localPath))
        {
            System.IO.File.Move(localPath, bakPath);
        }
        string directoryName = Utils.GetDirectoryName(localPath);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }
        try
        {
            if (this.IsEditor)
            {
                this._webClient.DownloadFile(this.GetRandomParasUrl(url), localPath);
                return true;
            }
            flag = DownloadGzipFile(this.GetRandomParasUrl(url), localPath);
        }
        catch (Exception exception)
        {
            LoggerHelper.Except(exception, null);
            if (System.IO.File.Exists(bakPath))
            {
                if (System.IO.File.Exists(localPath))
                {
                    System.IO.File.Delete(localPath);
                }
                System.IO.File.Move(bakPath, localPath);
            }
            flag = false;
        }
        finally
        {
            if (System.IO.File.Exists(bakPath))
            {
                System.IO.File.Delete(bakPath);
            }
        }
        return flag;
    }

    public void DownloadFileBreakPoint(string address, string fileName)
    {
        try
        {
            Uri requestUri = new Uri(address);
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUri);
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            long contentLength = response.ContentLength;
            response.Close();
            request.Abort();
            long length = contentLength;
            long allFilePointer = 0L;
            if (System.IO.File.Exists(fileName))
            {
                using (FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    length = contentLength - stream.Length;
                    allFilePointer = stream.Length;
                }
            }
            HttpWebRequest request2 = (HttpWebRequest) WebRequest.Create(requestUri);
            if (length > 0L)
            {
                request2.AddRange((int) allFilePointer, (int) (allFilePointer + length));
                HttpWebResponse response2 = (HttpWebResponse) request2.GetResponse();
                this.ReadBytesFromResponse(address, response2, allFilePointer, length, contentLength, fileName);
                response2.Close();
            }
            request2.Abort();
            this.Finished(address, null);
        }
        catch (Exception exception)
        {
            LoggerHelper.Error("DownloadFileBreakPoint Error：" + exception.Message, true);
            this.Finished(address, exception);
        }
    }

    private void DownloadFinishWithMd5(DownloadTask task)
    {
        string str = Utils.BuildFileMd5(task.FileName);
        if (str.Trim() != task.MD5.Trim())
        {
            if (System.IO.File.Exists(task.FileName))
            {
                System.IO.File.Delete(task.FileName);
            }
            LoggerHelper.Error("断点MD5验证失败，从新下载：" + task.FileName + "--" + str + " vs " + task.MD5, true);
            task.bDownloadAgain = true;
            task.bFineshed = false;
            this.CheckDownLoadList();
        }
        else
        {
            LoggerHelper.Debug("断点下载验证全部通过，下载完成：" + task.FileName, true, 0);
            if (this.FileDecompress != null)
            {
                this.FileDecompress(true);
            }
            task.bDownloadAgain = false;
            task.bFineshed = true;
            task.Finished();
            if (this.FileDecompress != null)
            {
                this.FileDecompress(false);
            }
            LoggerHelper.Debug("断点下载完成后，再次核对下载列表", true, 0);
            this.CheckDownLoadList();
        }
    }

    public static bool DownloadGzipFile(string url, string localPath)
    {
        try
        {
            string str = DownloadGzipString(url);
            if (!string.IsNullOrEmpty(str))
            {
                XMLParser.SaveText(localPath, str);
                return true;
            }
            return false;
        }
        catch (Exception exception)
        {
            LoggerHelper.Except(exception, null);
            return false;
        }
    }

    public static string DownloadGzipString(string url)
    {
        MemoryStream stream2;
        int num;
        byte[] buffer;
        byte[] buffer2;
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
        request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
        HttpWebResponse response = (HttpWebResponse) request.GetResponse();
        if (response.ContentEncoding.ToLower().Contains("gzip"))
        {
            GZipInputStream stream = new GZipInputStream(response.GetResponseStream());
            LoggerHelper.Debug("gzip mode", true, 0);
            stream2 = new MemoryStream();
            num = 0;
            buffer = new byte[0x1000];
            while ((num = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                stream2.Write(buffer, 0, num);
            }
            buffer2 = stream2.ToArray();
            return Encoding.UTF8.GetString(buffer2);
        }
        LoggerHelper.Debug("normal mode", true, 0);
        Stream responseStream = response.GetResponseStream();
        stream2 = new MemoryStream();
        num = 0;
        buffer = new byte[0x1000];
        while ((num = responseStream.Read(buffer, 0, buffer.Length)) != 0)
        {
            stream2.Write(buffer, 0, num);
        }
        buffer2 = stream2.ToArray();
        return Encoding.UTF8.GetString(buffer2);
    }

    public string DownLoadText(string url)
    {
        try
        {
            if (this.IsEditor)
            {
                return this._webClient.DownloadString(this.GetRandomParasUrl(url));
            }
            return DownloadGzipString(this.GetRandomParasUrl(url));
        }
        catch (Exception exception)
        {
            LoggerHelper.Except(exception, null);
            return string.Empty;
        }
    }

    internal void Finished(string url, Exception e = null)
    {
        Action mycontinue = null;
        Action again = null;
        Action finished = null;
        DownloadTask task = this.GetTask(url);
        if (task != null)
        {
            if (e != null)
            {
                LoggerHelper.Error(url + "下载出错:" + e.Message, true);
                if (mycontinue == null)
                {
                    mycontinue = delegate {
                        task.bDownloadAgain = false;
                        task.bFineshed = true;
                        this.CheckDownLoadList();
                    };
                }
                if (again == null)
                {
                    again = delegate {
                        task.bDownloadAgain = true;
                        task.bFineshed = false;
                        this.CheckDownLoadList();
                    };
                }
                if (finished == null)
                {
                    finished = () => this.DownloadFinishWithMd5(task);
                }
                HandleNetworkError(e, mycontinue, again, finished);
            }
            else
            {
                this.DownloadFinishWithMd5(task);
            }
        }
    }

    public string GetRandomParasUrl(string url)
    {
        System.Random random = Utils.CreateRandom();
        return string.Format("{0}?type={1}&ver={2}&sign={3}", new object[] { url.Trim(), random.Next(100), random.Next(100), Guid.NewGuid().ToString().Substring(0, 8) });
    }

    private DownloadTask GetTask(string url)
    {
        return Enumerable.FirstOrDefault<DownloadTask>(this.tasks, (Func<DownloadTask, bool>) (task => (url == task.Url)));
    }

    private static void HandleNetworkError(Exception e, Action mycontinue, Action again, Action finished = null)
    {
        LoggerHelper.Except(e, null);
        Action action = delegate {
            if ((e.Message.Contains("ConnectFailure") || e.Message.Contains("NameResolutionFailure")) || e.Message.Contains("No route to host"))
            {
                LoggerHelper.Error("-----------------Webclient ConnectFailure-------------", true);
                ShowMsgBoxForNetworkDisconnect(again, ":" + e.Message);
            }
            else if (e.Message.Contains("(404) Not Found") || e.Message.Contains("403"))
            {
                LoggerHelper.Error("-----------------WebClient NotFount-------------", true);
                ShowMsgForServerMaintenance(again, ":" + e.Message);
            }
            else if (e.Message.Contains("Disk full"))
            {
                LoggerHelper.Error("-----------------WebClient Disk full-------------", true);
                ShowMsgBoxForDiskFull(again, ":" + e.Message);
            }
            else if (e.Message.Contains("timed out") || e.Message.Contains("Error getting response stream"))
            {
                LoggerHelper.Error("-----------------WebClient timed out-------------", true);
                ShowMsgForTimeout(again, ":" + e.Message);
            }
            else if (e.Message.Contains("Sharing violation on path"))
            {
                LoggerHelper.Error("-----------------WebClient Sharing violation on path-------------", true);
                again();
            }
            else
            {
                ShowMsgForUnknown(again, ":" + e.Message);
            }
        };
        DriverLib.Invoke(action);
    }

    internal void Progress(string url, int progress)
    {
        Action action = delegate {
            if (this.ContainKey(url))
            {
                DownloadTask task = this.GetTask(url);
                task.OnTotalProgress(progress, 0L, 0L);
                if (this.TaskProgress != null)
                {
                    int num = Enumerable.Count<DownloadTask>(this.tasks, (Func<DownloadTask, bool>) (ta => ta.bFineshed));
                    string str = task.FileName.Substring(task.FileName.LastIndexOf("/") + 1);
                    this.TaskProgress(this.tasks.Count, num, str);
                }
            }
        };
        action();
    }

    internal void ReadBytesFromResponse(string requestURL, WebResponse response, long allFilePointer, long length, long totalSize, string fileName)
    {
        try
        {
            int num = (int) length;
            byte[] buffer = new byte[num];
            int srcOffset = 0;
            int offset = 0;
            using (Stream stream = response.GetResponseStream())
            {
                int num4;
                do
                {
                    num4 = stream.Read(buffer, offset, num - offset);
                    offset += num4;
                    if (num4 > 0)
                    {
                        byte[] dst = new byte[num4];
                        Buffer.BlockCopy(buffer, srcOffset, dst, 0, dst.Length);
                        using (FileStream stream2 = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            stream2.Position = allFilePointer;
                            stream2.Write(dst, 0, dst.Length);
                            stream2.Close();
                        }
                        float num5 = ((float) (((int) allFilePointer) + dst.Length)) / ((float) totalSize);
                        this.Progress(requestURL, (int) (num5 * 100f));
                        srcOffset += num4;
                        allFilePointer += num4;
                    }
                }
                while (num4 != 0);
            }
        }
        catch (Exception exception)
        {
            LoggerHelper.Error("ReadBytesFromResponse Error：" + exception.Message, true);
            this.Finished(requestURL, exception);
        }
    }

    private static void ShowMsgBoxForDiskFull(Action again, string msg = "")
    {
        Dictionary<int, DefaultLanguageData> dataMap = DefaultUI.dataMap;
        ForwardLoadingMsgBoxLib.Instance.ShowMsgBox(dataMap.Get<int, DefaultLanguageData>(50).content, dataMap.Get<int, DefaultLanguageData>(7).content, dataMap.Get<int, DefaultLanguageData>(0x35).content + msg, delegate (bool isOk) {
            if (isOk)
            {
                ForwardLoadingMsgBoxLib.Instance.Hide();
                again();
            }
            else
            {
                Application.Quit();
            }
        });
    }

    private static void ShowMsgBoxForNetworkDisconnect(Action again, string msg = "")
    {
        Dictionary<int, DefaultLanguageData> dataMap = DefaultUI.dataMap;
        ForwardLoadingMsgBoxLib.Instance.ShowMsgBox(dataMap.Get<int, DefaultLanguageData>(50).content, dataMap.Get<int, DefaultLanguageData>(0x33).content, dataMap.Get<int, DefaultLanguageData>(0x34).content + msg, delegate (bool isOk) {
            if (isOk)
            {
                ForwardLoadingMsgBoxLib.Instance.Hide();
                again();
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                class2.GetStatic<AndroidJavaObject>("currentActivity").Call("gotoNetworkSetting", new object[0]);
            }
        });
    }

    private static void ShowMsgForServerMaintenance(Action again, string msg = "")
    {
        Dictionary<int, DefaultLanguageData> dataMap = DefaultUI.dataMap;
        ForwardLoadingMsgBoxLib.Instance.ShowMsgBox(dataMap.Get<int, DefaultLanguageData>(50).content, dataMap.Get<int, DefaultLanguageData>(7).content, dataMap.Get<int, DefaultLanguageData>(14).content + msg, delegate (bool isOk) {
            if (isOk)
            {
                ForwardLoadingMsgBoxLib.Instance.Hide();
                again();
            }
            else
            {
                Application.Quit();
            }
        });
    }

    private static void ShowMsgForTimeout(Action again, string msg = "")
    {
        Dictionary<int, DefaultLanguageData> dataMap = DefaultUI.dataMap;
        ForwardLoadingMsgBoxLib.Instance.ShowMsgBox(dataMap.Get<int, DefaultLanguageData>(50).content, dataMap.Get<int, DefaultLanguageData>(7).content, dataMap.Get<int, DefaultLanguageData>(0x34).content + msg, delegate (bool isOk) {
            if (isOk)
            {
                ForwardLoadingMsgBoxLib.Instance.Hide();
                again();
            }
            else
            {
                Application.Quit();
            }
        });
    }

    private static void ShowMsgForUnknown(Action again, string msg = "")
    {
        Dictionary<int, DefaultLanguageData> dataMap = DefaultUI.dataMap;
        ForwardLoadingMsgBoxLib.Instance.ShowMsgBox(dataMap.Get<int, DefaultLanguageData>(50).content, dataMap.Get<int, DefaultLanguageData>(7).content, dataMap.Get<int, DefaultLanguageData>(0x34).content + msg, delegate (bool isOk) {
            if (isOk)
            {
                ForwardLoadingMsgBoxLib.Instance.Hide();
                again();
            }
            else
            {
                Application.Quit();
            }
        });
    }

    public void UploadLogFile(string fileName, string content)
    {
        string s = string.Format("filename={0}&content={1}", fileName, content);
        byte[] bytes = Encoding.UTF8.GetBytes(s);
        WebRequest request = WebRequest.Create(SystemConfig.GetCfgInfoUrl("logurl"));
        request.Method = "post";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = bytes.Length;
        using (Stream stream = request.GetRequestStream())
        {
            stream.Write(bytes, 0, bytes.Length);
        }
    }

    private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
    {
        Action mycontinue = null;
        Action again = null;
        string url = e.UserState.ToString();
        DownloadTask task = this.GetTask(url);
        if (task != null)
        {
            if (e.Error != null)
            {
                if (mycontinue == null)
                {
                    mycontinue = delegate {
                        task.bDownloadAgain = false;
                        task.bFineshed = true;
                        this.CheckDownLoadList();
                    };
                }
                if (again == null)
                {
                    again = delegate {
                        task.bDownloadAgain = true;
                        task.bFineshed = false;
                        this.CheckDownLoadList();
                    };
                }
                HandleNetworkError(e.Error, mycontinue, again, null);
            }
            else
            {
                string str2 = Utils.BuildFileMd5(task.FileName);
                if (str2.Trim() != task.MD5.Trim())
                {
                    if (System.IO.File.Exists(task.FileName))
                    {
                        System.IO.File.Delete(task.FileName);
                    }
                    LoggerHelper.Error("MD5验证失败，从新下载：" + task.FileName + "--" + str2 + " vs " + task.MD5, true);
                    task.bDownloadAgain = true;
                    task.bFineshed = false;
                    this.CheckDownLoadList();
                }
                else
                {
                    LoggerHelper.Debug("下载验证全部通过，下载完成：" + task.FileName, true, 0);
                    if (this.FileDecompress != null)
                    {
                        this.FileDecompress(true);
                    }
                    task.bDownloadAgain = false;
                    task.bFineshed = true;
                    task.Finished();
                    if (this.FileDecompress != null)
                    {
                        this.FileDecompress(false);
                    }
                    LoggerHelper.Debug("下载完成后，再次核对下载列表", true, 0);
                    this.CheckDownLoadList();
                }
            }
        }
    }

    private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        string url = e.UserState.ToString();
        if (this.ContainKey(url))
        {
            DownloadTask task = this.GetTask(url);
            task.OnProgress(e.ProgressPercentage);
            task.OnTotalBytesToReceive(e.TotalBytesToReceive);
            task.OnBytesReceived(e.BytesReceived);
            task.OnTotalProgress(e.ProgressPercentage, e.TotalBytesToReceive, e.BytesReceived);
            if (this.TaskProgress != null)
            {
                int num = (from ta in this.tasks
                    where ta.bFineshed
                    select ta).Count<DownloadTask>();
                string str2 = task.FileName.Substring(task.FileName.LastIndexOf("/") + 1);
                this.TaskProgress(this.tasks.Count, num, str2);
            }
            Thread.Sleep(100);
        }
    }

    public Action AllDownloadFinished { get; set; }

    public static bool BreakPoint
    {
        [CompilerGenerated]
        get
        {
            return <BreakPoint>k__BackingField;
        }
        [CompilerGenerated]
        set
        {
            <BreakPoint>k__BackingField = value;
        }
    }

    public Action<bool> FileDecompress { get; set; }

    public static DownloadMgr Instance
    {
        get
        {
            return (_mInstance ?? (_mInstance = new DownloadMgr()));
        }
    }
}

