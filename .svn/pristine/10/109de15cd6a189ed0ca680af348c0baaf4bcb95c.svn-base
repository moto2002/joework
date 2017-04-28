using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using ShiHuanJue.Debuger;

/// <summary>
/// 版本管理器
/// 把首包资源解包:copy到沙盒路径
/// www读取本地的version.txt,因为Android环境比较特殊,不能用io,PC和IOS可以用,索性统一用www算了
/// 异步下载web服务器上的version.txt
/// 进行对比,取出差异,封装成任务,抛给下载器
/// </summary>
public class VersionManager : Singleton<VersionManager>
{
    //string localPath = "file://" + Application.persistentDataPath + "/" ;
    //string remoteUrl = @"http://120.26.71.230/GameRes/";

    ResVersion LocalVersion = new ResVersion();
    ResVersion RemoteVersion = new ResVersion();
    Action VersionMgrFinished;
    bool isAllFinish = false;

    private string remoteUrl = string.Empty;
    public string RemoteUrl
    {
        get
        {
            return AppConst.WebUrl;
        }
    }

    private string localPath = string.Empty;
    public string LocalPath
    {
        get
        {
            return Util.DataPath;
        }
    }

    string versionTxtName = "Version.txt";

    public void Excute(Action callback)
    {
        this.remoteUrl = this.RemoteUrl;
        this.localPath = this.LocalPath;

        this.VersionMgrFinished = callback;

        //首包资源解包
        if (this.CheckResource())
        {
            //已经解包过了，那么开始更新版本校验
            //先读取本地version.txt
             StartCoroutine(LoadLocalVersionData());
        }
        else
        { 
            //没有解包过，开始解包，完了之后再开始版本校验
            StartCoroutine(OnExtractResource());
        }
    }

    /// <summary>
    /// 检查首包资源是不是已经解包
    /// 自己可添加检查文件列表逻辑
    /// </summary>
    public bool CheckResource()
    {
        bool isExists = Directory.Exists(Util.DataPath) && File.Exists(Util.DataPath + versionTxtName);
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

        string infile = resPath + versionTxtName;
        string outfile = dataPath + versionTxtName;

        if (File.Exists(outfile))
            File.Delete(outfile);

        string message = "正在解包文件:>" + versionTxtName;

        LogHelper.Log("infile:" + infile);
		LogHelper.Log("outfile:" + outfile);
		LogHelper.Log(message);

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
        foreach (var file in files)
        {
            string[] fs = file.Split('|');
            if (fs[0] == "Date") //第一行不处理
                continue;

            infile = resPath + fs[0];
            outfile = dataPath + fs[0];

            message = "正在解包文件:>" + fs[0];
			LogHelper.Log("正在解包文件:>" + infile);

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
		LogHelper.Log(message);

        yield return new WaitForSeconds(0.1f);

        message = string.Empty;

        //释放完成，开始启动更新资源,先读取本地版本文件
        StartCoroutine(LoadLocalVersionData());
    }

    IEnumerator LoadLocalVersionData()
    {
		LogHelper.Log("开始启动更新流程");
        WWW www = new WWW("file://" + this.localPath + versionTxtName);
        yield return www;
        this.ParseLocalVersionData(www.text);
    }

    private void ParseLocalVersionData(string localData)
    {
        this.LocalVersion = this.CommParse(localData);

        //解析完本地的，要去下载web服务器的来解析了哦
        this.LoadRemoteVersionData();
    }

    private void LoadRemoteVersionData()
    {
        DownloadTask _tempTask = new DownloadTask();
        _tempTask.Url = this.remoteUrl + versionTxtName;
        DownloadManager.GetInstance().downloadProgressChangedCallBack = this.OneTaskProgressChanged;
        DownloadManager.GetInstance().asynDownloadTxtCallBack = this.ParseRemoteVersionData;
        DownloadManager.GetInstance().AsynDownLoadText(_tempTask);
    }

    string remoteVersionData = string.Empty;
    private void ParseRemoteVersionData(string remoteData)
    {
        remoteData = remoteData.Trim();
        this.remoteVersionData = remoteData;
        this.RemoteVersion = this.CommParse(remoteData);

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
            if (i == 0)//第一行特别处理一下
            {
                string[] date = line[0].Split('|');
                tempVersion.timeAsUnique = date[1];
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

    Queue<DownloadTask> needDownloadTask = new Queue<DownloadTask>();
    /// <summary>
    /// 版本差异对比
    /// </summary>
    private void VersionDataDifference()
    {
        if (this.LocalVersion.timeAsUnique == this.RemoteVersion.timeAsUnique)
        {
			LogHelper.Log("版本一样，不用更新");
            this.AllDownloadTaskFinished(false);
        }
        else
        {
			LogHelper.Log("版本不一样，取出差异，封装成任务，抛给下载器");
            for (int i = 0; i < this.RemoteVersion.tasks.Count; i++)
            {
                string name = this.RemoteVersion.tasks[i].FileName;
                string md5 = this.RemoteVersion.tasks[i].MD5;

                DownloadTask tempTask = this.LocalVersion.tasks.Find(x => x.FileName == name);
                if (tempTask != null)
                {
                    if (tempTask.MD5 != md5)
                    {
                        //这是更新了的资源，需要下载
                        this.needDownloadTask.Enqueue(this.RemoteVersion.tasks[i]);
                    }
                }
                else
                {
                    //这是新添加的资源，需要下载
                    this.needDownloadTask.Enqueue(this.RemoteVersion.tasks[i]);
                }
            }

            if (this.needDownloadTask.Count > 0)
            {
                this.PushDownloadTask();
            }
            else
            {
				LogHelper.Log("no res need to download");
                this.AllDownloadTaskFinished(false);
            }
        }
    }

    //进度
    private void OneTaskProgressChanged(string url ,int progress,int receive,int total)
    {
		LogHelper.Log("url："+url +"进度：" +  progress + "received:" + receive + "total:" + total);
    }

    //有一个任务完成了，把它保存到本地
    private void OneTaskFinished(string url)
    {
		LogHelper.Log(url + "完成");

        this.PopDownloadTask();
    }

    //所有的下载任务都完成了
    private void AllDownloadTaskFinished(bool updateVersionTxt)
    {
		LogHelper.Log("All Task Finish");

        if (updateVersionTxt)
        {
            //替换本地的Version.txt为web服务器上的,保持版本最新
            Util.CreateFile(this.localPath, versionTxtName, this.remoteVersionData);
			LogHelper.Log(versionTxtName+"已更新");
        }

        this.LocalVersion = null;
        this.RemoteVersion = null;
        this.remoteVersionData = null;

        //释放downloadmgr
        DownloadManager.GetInstance().Dispose();

        this.isAllFinish = true;
    }

    DownloadTask currentTask;
    /// <summary>
    /// 抛给下载器进行下载
    /// </summary>
    private void PushDownloadTask()
    {
        DownloadTask firstTask = this.needDownloadTask.Peek();
        this.currentTask = firstTask;
        firstTask.Url = this.remoteUrl + firstTask.FileName;

        //设置保存本地的目录
        firstTask.FileName = this.localPath + firstTask.FileName;
        string path = Path.GetDirectoryName(firstTask.FileName);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

		DownloadManager.GetInstance().downloadProgressChangedCallBack = this.OneTaskProgressChanged;
        DownloadManager.GetInstance().oneTaskFinished = this.OneTaskFinished;
        DownloadManager.GetInstance().AsynDownLoadFile(firstTask);
    }

    private void PopDownloadTask()
    {
        if (this.needDownloadTask.Contains(this.currentTask))
        {
            DownloadTask firstTask = this.needDownloadTask.Dequeue();

            if (this.needDownloadTask.Count == 0)
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
        
        }
    }

    //操作unity相关的类一定要在主线程中， 蛋疼~!!!
    void Update()
    {
        if (isAllFinish)
        {
			isAllFinish = false;
			LogHelper.Log("版本管理结束");
            this.VersionMgrFinished();
            GameObject.Destroy(this.gameObject);
        }
    }
}
