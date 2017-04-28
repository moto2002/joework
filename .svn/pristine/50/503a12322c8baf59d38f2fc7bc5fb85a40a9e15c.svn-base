using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using LuaFramework;

public class ResVersion
{
    public string versionNumber = string.Empty;
    public List<DownloadTask> tasks = new List<DownloadTask>();
}

/// <summary>
/// 版本管理器
/// 把首包资源解包:copy到沙盒路径
/// www读取本地的version.txt,因为Android环境比较特殊,不能用io,PC和IOS可以用
/// 异步下载web服务器上的version.txt
/// 进行对比,取出差异,封装成任务,抛给下载器
/// </summary>
public class VersionMgr : Manager
{
    private Queue<DownloadTask> m_needDownloadTasks = new Queue<DownloadTask>();
    private ResVersion m_localVersion = new ResVersion();
    private ResVersion m_remoteVersion = new ResVersion();
    private Action m_versionMgrCallback;
    private DownloadTask m_currTask;
    private string m_versionTxtName = "files.txt";
    private string m_remoteVersionData = string.Empty;

    private string m_remoteUrl = string.Empty;
    public string RemoteUrl
    {
        get
        {
            return AppConst.WebUrl;
        }
    }

    private string m_localPath = string.Empty;
    public string LocalPath
    {
        get
        {
            return Util.DataPath;
        }
    }

    /// <summary>
    ///执行版本管理
    /// </summary>
    /// <param name="callback">Callback.</param>
    public void InitStart(Action callback)
    {
        this.m_versionMgrCallback = callback;

        if (AppConst.DebugMode)//测试模式，不进行解包和更新
        {
            if (m_versionMgrCallback != null)
                m_versionMgrCallback();
        }

        this.m_remoteUrl = this.RemoteUrl;
        this.m_localPath = this.LocalPath;

        //首包资源已经解包
        if (this.CheckResource())
        {
            //已经解包过了，那么开始更新版本校验
            StartUpdateVersion();
        }
        else
        {
            //dispatcher.Dispatch (NotiConst.UPDATE_STATE, "开始解包,不消耗流量");
            //没有解包过，开始解包，完了之后再开始版本校验
            StartCoroutine(OnExtractResource());
        }
    }

    IEnumerator Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (this.m_versionMgrCallback != null)
            this.m_versionMgrCallback();
    }

    private void StartUpdateVersion()
    {
        if (AppConst.UpdateMode)
        {
            //先读取本地version.txt
            StartCoroutine(LoadLocalVersionData());
        }
        else
        {
            //test
            StartCoroutine(Delay(1f));
        }
    }
    /// <summary>
    /// 检查首包资源是不是已经解包
    /// 自己可添加检查文件列表逻辑
    /// </summary>
    public bool CheckResource()
    {
        bool isExists = Directory.Exists(Util.DataPath) && File.Exists(Util.DataPath + m_versionTxtName);
        return isExists;
    }

    /// <summary>
    /// 释放资源，把streamasset下的资源拷贝到平台对应的沙盒目录
    /// </summary>
    IEnumerator OnExtractResource()
    {
        string resPath = Util.AppContentPath(); //游戏包资源目录
        string dataPath = Util.DataPath;        //数据目录

        if (Directory.Exists(dataPath))
            Directory.Delete(dataPath, true);

        Directory.CreateDirectory(dataPath);

        string infile = resPath + m_versionTxtName;
        string outfile = dataPath + m_versionTxtName;

        if (File.Exists(outfile))
            File.Delete(outfile);

        string message = "正在解包文件:>" + m_versionTxtName;

        Debug.Log("infile:" + infile);
        Debug.Log("outfile:" + outfile);
        Debug.Log(message);
        //dispatcher.Dispatch (NotiConst.UPDATE_MESSAGE, message);

        //只有Android平台要流方式www，其它平台都可以IO方式
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW www = new WWW(infile);
            yield return www;

            if (www.isDone)
            {
                File.WriteAllBytes(outfile, www.bytes);
            }
            yield return 0;
        }
        else
        {
            File.Copy(infile, outfile, true);
        }

        yield return new WaitForEndOfFrame();

        //释放所有文件到数据目录
        string[] files = File.ReadAllLines(outfile);
        for (int i = 0; i < files.Length; i++)
        {
            if (i == 0) //第一行不处理
                continue;

            string file = files[i];
            string[] fs = file.Split('|');

            infile = resPath + fs[0];
            outfile = dataPath + fs[0];

            message = "正在解包文件:>" + fs[0];
            Debug.Log("正在解包文件:>" + infile);
            //dispatcher.Dispatch (NotiConst.UPDATE_MESSAGE, message);

            string dir = Path.GetDirectoryName(outfile);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (Application.platform == RuntimePlatform.Android)
            {
                WWW www = new WWW(infile);
                yield return www;

                if (www.isDone)
                {
                    File.WriteAllBytes(outfile, www.bytes);
                }
                yield return 0;
            }
            else
            {
                if (File.Exists(outfile))
                {
                    File.Delete(outfile);
                }
                File.Copy(infile, outfile, true);
            }
            yield return new WaitForEndOfFrame();
        }
        message = "解包完成!!!";
        Debug.Log(message);
        //dispatcher.Dispatch (NotiConst.UPDATE_MESSAGE,"");
        //dispatcher.Dispatch (NotiConst.UPDATE_STATE, message);
        //dispatcher.Dispatch (NotiConst.UPDATE_PROGRESS, 0.5f);

        yield return new WaitForSeconds(0.1f);

        message = string.Empty;

        //释放完成，开始启动更新资源
        StartUpdateVersion();
    }

    IEnumerator LoadLocalVersionData()
    {
        Debug.Log("开始启动更新流程");
        WWW www = new WWW("file://" + this.m_localPath + m_versionTxtName);
        yield return www;
        this.ParseLocalVersionData(www.text);
    }

    private void ParseLocalVersionData(string localData)
    {
        this.m_localVersion = this.CommParse(localData);

        //解析完本地的，要去下载web服务器的来解析了哦
        this.LoadRemoteVersionData();
    }

    private void LoadRemoteVersionData()
    {
        DownloadTask _tempTask = new DownloadTask();
        _tempTask.Url = this.m_remoteUrl + m_versionTxtName;

        DownloadMgr.GetInstance().InitDownloadCallBacks(this.ParseRemoteVersionData, this.OneTaskProgressChanged, null);
        DownloadMgr.GetInstance().AsynDownLoadText(_tempTask);
    }

    private void ParseRemoteVersionData(string remoteData)
    {
        remoteData = remoteData.Trim();
        this.m_remoteVersionData = remoteData;
        this.m_remoteVersion = this.CommParse(remoteData);

        //都解析完了的话，开始进行差异对比
        this.VersionDataDifference();
    }

    private ResVersion CommParse(string data)
    {
        ResVersion tempVersion = new ResVersion();

        string versionInfo = data;
        string[] line = versionInfo.Split('\n');
        for (int i = 0; i < line.Length - 1; i++)
        {
            if (i == 0)
            {//第一行特别处理一下
                string[] date = line[0].Split('|');
                tempVersion.versionNumber = date[1];
            }
            else
            {
                string[] linedata = line[i].Split('|');
                if (linedata.Length == 3)
                {
                    DownloadTask task = new DownloadTask();
                    task.FileName = linedata[0];
                    task.MD5 = linedata[1];
                    task.Size = int.Parse(linedata[2]);
                    tempVersion.tasks.Add(task);
                }
                else
                {
                    Debug.Log("linedata.Length != 3");
                }
            }
        }

        return tempVersion;
    }

    /// <summary>
    /// 解析版本号
    /// eg.0.1.5.150
    /// 主版本号，子版本号，修正版本号，编译版本号
    /// </summary>
    /// <param name="number">Number.</param>
    private int[] ParseVersionNumber(string number)
    {
        string[] tempStrs = number.Split('.');
        List<int> tempIntList = new List<int>();
        for (int i = 0; i < tempStrs.Length; i++)
        {
            tempIntList.Add(int.Parse(tempStrs[i]));
        }
        return tempIntList.ToArray();
    }

    /// <summary>
    /// 版本更新等级
    /// </summary>
    public enum EVersionUpdateLevel
    {
        ERROR,
        NO_NEED_UPDATE,
        //不需要更新
        DOWNLOAD_AGAIN,
        //主版本号不一致，必须重下app
        UPDATE_ASSETS_ONLY,
        //子版本号不一致（有新功能添加，有bug修复，有数据调整），只需要在线更新资源
    }

    private EVersionUpdateLevel CheckVersionNumber(string localVersion, string remoteVersion)
    {
        int[] localVersionNumbers = ParseVersionNumber(localVersion);
        int[] remoteVersionNumbers = ParseVersionNumber(remoteVersion);

        if (remoteVersionNumbers[0] > localVersionNumbers[0])
        {
            return EVersionUpdateLevel.DOWNLOAD_AGAIN;
        }
        else if (remoteVersionNumbers[0] == localVersionNumbers[0])
        {
            if (remoteVersionNumbers[1] > localVersionNumbers[1])
            {
                return EVersionUpdateLevel.UPDATE_ASSETS_ONLY;
            }
            else if (remoteVersionNumbers[1] == localVersionNumbers[1])
            {
                if (remoteVersionNumbers[2] > localVersionNumbers[2])
                {
                    return EVersionUpdateLevel.UPDATE_ASSETS_ONLY;
                }
                else if (remoteVersionNumbers[2] == localVersionNumbers[2])
                {
                    return EVersionUpdateLevel.NO_NEED_UPDATE;
                }
                else
                {
                    return EVersionUpdateLevel.ERROR;
                }
            }
            else
            {
                return EVersionUpdateLevel.ERROR;
            }
        }
        else
        {
            return EVersionUpdateLevel.ERROR;
        }
    }

    /// <summary>
    /// 版本差异对比
    /// </summary>
    private void VersionDataDifference()
    {
        EVersionUpdateLevel level = CheckVersionNumber(this.m_localVersion.versionNumber, this.m_remoteVersion.versionNumber);
        if (level == EVersionUpdateLevel.DOWNLOAD_AGAIN)
        {
            Debug.Log("跳转AppStore");
        }
        else if (level == EVersionUpdateLevel.UPDATE_ASSETS_ONLY)
        {
            Debug.Log("小版本资源更新，取出差异，封装成任务，抛给下载器");
            for (int i = 0; i < this.m_remoteVersion.tasks.Count; i++)
            {
                string name = this.m_remoteVersion.tasks[i].FileName;
                string md5 = this.m_remoteVersion.tasks[i].MD5;

                DownloadTask tempTask = this.m_localVersion.tasks.Find(x => x.FileName == name);
                if (tempTask != null)
                {
                    if (tempTask.MD5 != md5)
                    {
                        //这是更新了的资源，需要下载
                        this.m_needDownloadTasks.Enqueue(this.m_remoteVersion.tasks[i]);
                    }
                }
                else
                {
                    //这是新添加的资源，需要下载
                    this.m_needDownloadTasks.Enqueue(this.m_remoteVersion.tasks[i]);
                }
            }

            if (this.m_needDownloadTasks.Count > 0)
            {
                this.PushDownloadTask();
            }
            else
            {
                Debug.Log("no res need to download");
                this.AllDownloadTaskFinished(false);
            }

        }
        else if (level == EVersionUpdateLevel.NO_NEED_UPDATE)
        {
            Debug.Log("版本一样，不用更新");
            this.AllDownloadTaskFinished(false);
        }
        else if (level == EVersionUpdateLevel.ERROR)
        {
            Debug.Log("远程版本号比本地版本号还小，有误！");
        }
    }

    //进度
    private void OneTaskProgressChanged(string url, int progress, int receive, int total)
    {
        Debug.Log("url：" + url + "进度：" + progress + "received:" + receive + "total:" + total);
    }

    //有一个任务完成了，把它保存到本地
    private void OneTaskFinished(string url)
    {
        Debug.Log(url + "完成");
        this.PopDownloadTask();
    }

    //所有的下载任务都完成了
    private void AllDownloadTaskFinished(bool updateVersionTxt)
    {
        Debug.Log("All Task Finish");

        if (updateVersionTxt)
        {
            //替换本地的Version.txt为web服务器上的,保持版本最新
            Util.CreateFile(this.m_localPath, m_versionTxtName, this.m_remoteVersionData);
            Debug.Log(m_versionTxtName + "已更新");
        }

        this.m_localVersion = null;
        this.m_remoteVersion = null;
        this.m_remoteVersionData = null;

        //释放downloadmgr
        DownloadMgr.GetInstance().Dispose();

        if (m_versionMgrCallback != null)
            this.m_versionMgrCallback();
    }


    /// <summary>
    /// 抛给下载器进行下载
    /// </summary>
    private void PushDownloadTask()
    {
        DownloadTask firstTask = this.m_needDownloadTasks.Peek();
        this.m_currTask = firstTask;
        firstTask.Url = this.m_remoteUrl + firstTask.FileName;

        //设置保存本地的目录
        firstTask.FileName = this.m_localPath + firstTask.FileName;
        string path = Path.GetDirectoryName(firstTask.FileName);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        DownloadMgr.GetInstance().InitDownloadCallBacks(null, this.OneTaskProgressChanged, this.OneTaskFinished);
        DownloadMgr.GetInstance().AsynDownLoadFile(firstTask);
    }

    private void PopDownloadTask()
    {
        if (this.m_needDownloadTasks.Contains(this.m_currTask))
        {
            DownloadTask firstTask = this.m_needDownloadTasks.Dequeue();

            if (this.m_needDownloadTasks.Count == 0)
            {
                this.AllDownloadTaskFinished(true);
            }
            else
            {
                this.PushDownloadTask();
            }
        }
        else
        {
            Debug.Log("error");
        }
    }

    void OnDestroy()
    {
        Debug.Log("~VersionManager was destroy");
    } 
}

