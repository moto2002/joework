using Mogo.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngineInternal;

public class VersionManager
{
    public string defContent = "";
    public readonly bool IsPlatformUpdate = false;

    static VersionManager()
    {
        Instance = new VersionManager();
    }

    private VersionManager()
    {
    }

    public void AddListeners()
    {
        EventDispatcher.AddEventListener<string>(VersionEvent.AddMD5Content, new Action<string>(this.AddMD5Content));
        EventDispatcher.AddEventListener(VersionEvent.GetContentMD5, new Action(this.GetContentMD5));
    }

    private void AddMD5Content(string newContent)
    {
        LoggerHelper.Debug("Adding MD5 Content", true, 0);
        this.defContent = this.defContent + newContent;
    }

    private void AsynDownloadApk(Action<int, int, string> taskProgress, string fileName, string url, Action<int, long, long> progress, Action finished, Action<Exception> error)
    {
        DownloadTask task = new DownloadTask {
            FileName = fileName,
            Url = url,
            Finished = finished,
            Error = error,
            TotalProgress = progress,
            MD5 = this.ServerVersion.ApkMd5
        };
        LoggerHelper.Info("down load apk & md5: " + url + " " + this.ServerVersion.ApkMd5, true);
        List<DownloadTask> list = new List<DownloadTask> {
            task
        };
        DownloadMgr.Instance.tasks = list;
        Action<int, int, string> action = delegate (int total, int current, string filename) {
            if (taskProgress != null)
            {
                taskProgress(total, current, filename);
            }
        };
        DownloadMgr.Instance.TaskProgress = action;
        StreamingAssetManager go = null;
        ResourceIndexInfo.Instance.Init(Application.get_streamingAssetsPath() + "/ResourceIndexInfo.txt", delegate {
            Action action = null;
            LoggerHelper.Info("资源索引信息:" + ResourceIndexInfo.Instance.GetLeftFilePathes().Length, true);
            if (ResourceIndexInfo.Instance.GetLeftFilePathes().Length == 0)
            {
                go = new StreamingAssetManager {
                    UpdateProgress = false,
                    ApkFinished = true
                };
            }
            else
            {
                go = new StreamingAssetManager {
                    UpdateProgress = false
                };
                if (action == null)
                {
                    action = delegate {
                        LoggerHelper.Debug("打开资源导出完成的标识11ApkFinished", true, 0);
                        go.ApkFinished = true;
                    };
                }
                go.AllFinished = action;
            }
        }, null);
        DownloadMgr.Instance.AllDownloadFinished = delegate {
            LoggerHelper.Info("APK download finish, wait for export finish:" + fileName, true);
            if (go != null)
            {
                go.UpdateProgress = true;
                LoggerHelper.Debug("打开导出进度显示:" + go.ApkFinished, true, 0);
                while (!go.ApkFinished)
                {
                    Thread.Sleep(500);
                }
                LoggerHelper.Info("APK and export download finish.", true);
                go = null;
            }
            DriverLib.Invoke(() => this.InstallApk(fileName));
            if (finished != null)
            {
                finished();
            }
            LoggerHelper.Debug("apk安装成功", true, 0);
        };
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        while (go == null)
        {
            Thread.Sleep(50);
            if (stopwatch.ElapsedMilliseconds > 0xbb8L)
            {
                break;
            }
        }
        stopwatch.Stop();
        if (!((go == null) || go.ApkFinished))
        {
            LoggerHelper.Debug("apk下载同时导出资源", true, 0);
            go.UpdateApkExport();
        }
        DownloadMgr.Instance.CheckDownLoadList();
    }

    private void AsynDownloadUpdatePackage(Action<bool> fileDecompress, Action<int, int, string> taskProgress, Action<int, long, long> progress, Action finished, Action<Exception> error)
    {
        LoggerHelper.Debug("下载包列表", true, 0);
        this.DownloadPackageInfoList(delegate (IEnumerable<PackageInfo> packageInfoList) {
            List<KeyValuePair<string, string>> downloadList = (from packageInfo in packageInfoList
                where (packageInfo.LowVersion.Compare(this.LocalVersion.ResouceVersionInfo) >= 0) && (packageInfo.HighVersion.Compare(this.ServerVersion.ResouceVersionInfo) <= 0)
                select new KeyValuePair<string, string>(packageInfo.HighVersion.ToString(), packageInfo.Name)).ToList<KeyValuePair<string, string>>();
            string packageUrl = this.ServerVersion.PackageUrl;
            if (downloadList.Count != 0)
            {
                LoggerHelper.Debug("开始下载包列表", true, 0);
                this.DownDownloadPackageList(fileDecompress, packageUrl, downloadList, taskProgress, progress, finished, error);
            }
            else
            {
                LoggerHelper.Debug("更新包数目为0", true, 0);
                if (finished != null)
                {
                    finished();
                }
            }
        }, () => error(new Exception("DownloadPackageInfoList error.")));
    }

    public void BeforeCheck(Action<bool> AsynResult, Action OnError)
    {
        new CheckTimeout().AsynIsNetworkTimeout(delegate (bool success) {
            Action<string> asynResult = null;
            if (success)
            {
                if (asynResult == null)
                {
                    asynResult = delegate (string serverVersion) {
                        if (File.Exists(SystemConfig.ServerVersionPath))
                        {
                            serverVersion = Utils.LoadFile(SystemConfig.ServerVersionPath);
                            LoggerHelper.Info("serverVersion exist:\n" + serverVersion, true);
                        }
                        this.ServerVersion = this.GetVersionInXML(serverVersion);
                        if (this.ServerVersion.IsDefault())
                        {
                            if (OnError != null)
                            {
                                OnError();
                            }
                        }
                        else
                        {
                            LoggerHelper.Debug("服务器程序版本: " + this.ServerVersion.ProgramVersionInfo, true, 0);
                            LoggerHelper.Debug("服务器资源版本: " + this.ServerVersion.ResouceVersionInfo, true, 0);
                            LoggerHelper.Debug("服务器包列表: " + this.ServerVersion.PackageList, true, 0);
                            LoggerHelper.Debug("服务器包地址: " + this.ServerVersion.PackageUrl, true, 0);
                            LoggerHelper.Debug("服务器Apk地址: " + this.ServerVersion.ApkUrl, true, 0);
                            LoggerHelper.Debug("服务器md5地址: " + this.ServerVersion.PackageMd5List, true, 0);
                            bool flag = this.ServerVersion.ProgramVersionInfo.Compare(this.LocalVersion.ProgramVersionInfo) > 0;
                            bool flag2 = this.ServerVersion.ResouceVersionInfo.Compare(this.LocalVersion.ResouceVersionInfo) > 0;
                            AsynResult(flag || flag2);
                        }
                    };
                }
                DownloadMgr.Instance.AsynDownLoadText(SystemConfig.GetCfgInfoUrl("version"), asynResult, OnError);
            }
            else if (OnError != null)
            {
                OnError();
            }
        });
    }

    public bool CheckAndDownload(Action<bool> fileDecompress, Action<int, int, string> taskProgress, Action<int, long, long> progress, Action finished, Action<Exception> error)
    {
        if (this.ServerVersion.ProgramVersionInfo.Compare(this.LocalVersion.ProgramVersionInfo) > 0)
        {
            LoggerHelper.Debug("服务器apk版本高，下载apk", true, 0);
            string apkUrl = this.ServerVersion.ApkUrl;
            string resourceFolder = SystemConfig.ResourceFolder;
            if (Application.get_platform() == 11)
            {
                resourceFolder = Application.get_persistentDataPath() + "/temp/";
            }
            if (!Directory.Exists(resourceFolder))
            {
                Directory.CreateDirectory(resourceFolder);
            }
            string fileName = resourceFolder + Utils.GetFileName(apkUrl, '/');
            Action action = delegate {
            };
            if (!this.IsPlatformUpdate)
            {
                this.AsynDownloadApk(taskProgress, fileName, apkUrl, progress, action, error);
            }
            else
            {
                this.SetPlatformUpdateCallback();
                this.ExportStreamingAssetWhenDownloadApk(new Action(this.PlatformUpdate));
            }
            return true;
        }
        if (this.ServerVersion.ResouceVersionInfo.Compare(this.LocalVersion.ResouceVersionInfo) > 0)
        {
            LoggerHelper.Debug("服务器资源版本号比本地版本号大", true, 0);
            this.AsynDownloadUpdatePackage(fileDecompress, taskProgress, progress, finished, error);
            return true;
        }
        if (finished != null)
        {
            finished();
        }
        return false;
    }

    public void CheckVersion(Action<bool> fileDecompress, Action<int, int, string> taskProgress, Action<int, long, long> progress, Action finished, Action<Exception> error)
    {
        this.BeforeCheck(delegate (bool result) {
            Action check = null;
            if (result)
            {
                if (check == null)
                {
                    check = (Action) (() => this.CheckAndDownload(fileDecompress, taskProgress, progress, finished, error));
                }
                this.ModalCheckNetwork(check);
            }
            else
            {
                LoggerHelper.Debug("不需要更新apk和pkg", true, 0);
                if (finished != null)
                {
                    finished();
                }
            }
        }, () => error(new Exception("download version file time out.")));
    }

    private void DownDownloadPackageList(Action<bool> fileDecompress, string packageUrl, List<KeyValuePair<string, string>> downloadList, Action<int, int, string> taskProgress, Action<int, long, long> progress, Action finished, Action<Exception> error)
    {
        List<DownloadTask> list = new List<DownloadTask>();
        for (int i = 0; i < downloadList.Count; i++)
        {
            LoggerHelper.Debug("收集数据包，生成任务:" + i, true, 0);
            KeyValuePair<string, string> kvp = downloadList[i];
            LoggerHelper.Debug("下载文件名：" + kvp.Value, true, 0);
            string localFile = SystemConfig.ResourceFolder + kvp.Value;
            Action action = delegate {
                LoggerHelper.Debug("下载完成，进入完成回调", true, 0);
                LoggerHelper.Debug("本地文件：" + File.Exists(localFile), true, 0);
                if (File.Exists(localFile))
                {
                    FileAccessManager.DecompressFile(localFile);
                }
                LoggerHelper.Debug("解压完成，保存版本信息到version.xml", true, 0);
                if (File.Exists(localFile))
                {
                    File.Delete(localFile);
                }
                this.LocalVersion.ResouceVersionInfo = new VersionCodeInfo(kvp.Key);
                this.SaveVersion(this.LocalVersion);
            };
            string str = packageUrl + kvp.Value;
            DownloadTask item = new DownloadTask {
                FileName = localFile,
                Url = str,
                Finished = action,
                Error = error,
                TotalProgress = progress
            };
            string key = kvp.Value;
            LoggerHelper.Debug(item.FileName + "::fileNoEXtension::" + key, true, 0);
            if (this.ServerVersion.PackageMD5Dic.ContainsKey(key))
            {
                item.MD5 = this.ServerVersion.PackageMD5Dic[key];
                list.Add(item);
            }
            else
            {
                error(new Exception("down pkg not exit :" + key));
                return;
            }
        }
        Action action2 = delegate {
            LoggerHelper.Debug("更新包全部下载完成", true, 0);
            finished();
        };
        Action<int, int, string> action3 = delegate (int total, int current, string filename) {
            if (taskProgress != null)
            {
                taskProgress(total, current, filename);
            }
        };
        Action<bool> action4 = delegate (bool decompress) {
            if (fileDecompress != null)
            {
                fileDecompress(decompress);
            }
        };
        LoggerHelper.Debug("所有任务收集好了,开启下载", true, 0);
        DownloadMgr.Instance.tasks = list;
        DownloadMgr.Instance.AllDownloadFinished = action2;
        DownloadMgr.Instance.TaskProgress = action3;
        DownloadMgr.Instance.FileDecompress = action4;
        DownloadMgr.Instance.CheckDownLoadList();
    }

    private void DownloadPackageInfoList(Action<IEnumerable<PackageInfo>> AsynResult, Action OnError)
    {
        DownloadMgr.Instance.AsynDownLoadText(this.ServerVersion.PackageMd5List, delegate (string content) {
            SecurityElement element = XMLParser.LoadXML(content);
            if (element == null)
            {
                if (OnError != null)
                {
                    OnError();
                }
            }
            else
            {
                List<PackageInfo> list = new List<PackageInfo>();
                foreach (object obj2 in element.Children)
                {
                    PackageInfo item = new PackageInfo();
                    SecurityElement element2 = obj2 as SecurityElement;
                    string str = element2.Attribute("n");
                    item.Name = str;
                    string str2 = str.Substring(7, str.Length - 11);
                    string version = str2.Substring(0, str2.IndexOf('-'));
                    item.LowVersion = new VersionCodeInfo(version);
                    string str4 = str2.Substring(str2.IndexOf('-') + 1);
                    item.HighVersion = new VersionCodeInfo(str4);
                    item.Md5 = element2.Text;
                    list.Add(item);
                    this.ServerVersion.PackageMD5Dic.Add(item.Name, item.Md5);
                }
                AsynResult(list);
            }
        }, OnError);
    }

    internal void ExportStreamingAssetWhenDownloadApk(Action finished)
    {
        ResourceIndexInfo.Instance.Init(Application.get_streamingAssetsPath() + "/ResourceIndexInfo.txt", delegate {
            LoggerHelper.Debug("资源索引信息:" + ResourceIndexInfo.Instance.GetLeftFilePathes().Length, true, 0);
            if (ResourceIndexInfo.Instance.GetLeftFilePathes().Length != 0)
            {
                StreamingAssetManager manager2 = new StreamingAssetManager {
                    AllFinished = finished
                };
                manager2.UpdateApkExport();
            }
            else
            {
                LoggerHelper.Debug("没有streamingAssets,不需要导出", true, 0);
                finished();
            }
        }, null);
    }

    private void GetContentMD5()
    {
        this.defContentBytes = Utils.CreateMD5(Encoding.UTF8.GetBytes(this.defContent));
    }

    public string GetPackageName(string currentVersion, string newVersion)
    {
        return ("package" + currentVersion + "-" + newVersion + ".pkg");
    }

    public VersionManagerInfo GetVersionInXML(string xml)
    {
        SecurityElement element = XMLParser.LoadXML(xml);
        if (((element != null) && (element.Children != null)) && (element.Children.Count != 0))
        {
            VersionManagerInfo info = new VersionManagerInfo();
            PropertyInfo[] properties = typeof(VersionManagerInfo).GetProperties();
            using (IEnumerator enumerator = element.Children.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Func<PropertyInfo, bool> predicate = null;
                    SecurityElement item = (SecurityElement) enumerator.Current;
                    if (predicate == null)
                    {
                        predicate = t => t.Name == item.Tag;
                    }
                    PropertyInfo info2 = properties.FirstOrDefault<PropertyInfo>(predicate);
                    if (info2 != null)
                    {
                        info2.SetValue(info, item.Text, null);
                    }
                }
            }
            return info;
        }
        return new VersionManagerInfo();
    }

    public void Init()
    {
        LoggerHelper.Debug("VersionManager Init", true, 0);
        this.AddListeners();
    }

    private void InstallApk(string apkPath)
    {
        LoggerHelper.Info("Call Install apk: " + apkPath, true);
        Application.OpenURL(apkPath);
        Application.Quit();
    }

    public void LoadLocalVersion()
    {
        if (File.Exists(SystemConfig.VersionPath))
        {
            string xml = Utils.LoadFile(SystemConfig.VersionPath);
            this.LocalVersion = this.GetVersionInXML(xml);
            TextAsset asset = Resources.Load("version") as TextAsset;
            if (!((asset == null) || string.IsNullOrEmpty(asset.get_text())))
            {
                this.LocalVersion.ProgramVersionInfo = this.GetVersionInXML(asset.get_text()).ProgramVersionInfo;
            }
            LoggerHelper.Info("program version : " + this.LocalVersion.ProgramVersion + " resource version :" + this.LocalVersion.ResouceVersion, true);
        }
        else
        {
            this.LocalVersion = new VersionManagerInfo();
            LoggerHelper.Info("cannot find local version,export from streaming assets", true);
            TextAsset asset2 = Resources.Load("version") as TextAsset;
            if (asset2 != null)
            {
                XMLParser.SaveText(SystemConfig.VersionPath, asset2.get_text());
            }
        }
    }

    public void ModalCheckNetwork(Action check)
    {
        Action<bool> onClick = null;
        LoggerHelper.Debug("----------------ModalCheckNetwork----ModalCheckNetwork----ModalCheckNetwork-----------------", true, 0);
        if (Application.get_internetReachability() == 1)
        {
            LoggerHelper.Debug("3G network", true, 0);
            Dictionary<int, DefaultLanguageData> dataMap = DefaultUI.dataMap;
            if (onClick == null)
            {
                onClick = delegate (bool confirm) {
                    if (confirm)
                    {
                        LoggerHelper.Debug("3G network confirm", true, 0);
                        ForwardLoadingMsgBoxLib.Instance.Hide();
                        check();
                    }
                    else
                    {
                        LoggerHelper.Debug("3G network cancel", true, 0);
                        ForwardLoadingMsgBoxLib.Instance.Hide();
                        Application.Quit();
                    }
                };
            }
            ForwardLoadingMsgBoxLib.Instance.ShowMsgBox(dataMap[6].content, dataMap[7].content, dataMap[5].content, onClick);
        }
        else
        {
            check();
        }
    }

    public void OpenUrl()
    {
        LoggerHelper.Debug("打开一个url", true, 0);
        AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("OpenUrl", new object[0]);
    }

    public void PlatformUpdate()
    {
        LoggerHelper.Debug("安卓上更新apk", true, 0);
        AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("updateVersion", new object[0]);
    }

    public void RemoveListeners()
    {
        EventDispatcher.RemoveEventListener<string>(VersionEvent.AddMD5Content, new Action<string>(this.AddMD5Content));
        EventDispatcher.RemoveEventListener(VersionEvent.GetContentMD5, new Action(this.GetContentMD5));
    }

    private void SaveVersion(VersionManagerInfo version)
    {
        PropertyInfo[] properties = typeof(VersionManagerInfo).GetProperties();
        SecurityElement element = new SecurityElement("root");
        foreach (PropertyInfo info in properties)
        {
            element.AddChild(new SecurityElement(info.Name, info.GetGetMethod().Invoke(version, null) as string));
        }
        XMLParser.SaveText(SystemConfig.VersionPath, element.ToString());
    }

    private void SetPlatformUpdateCallback()
    {
        LoggerHelper.Info("Init PlatformUpateCallback", true);
        string str = "PlatformUpdateCallback";
        string[] textArray1 = new string[] { "F:/BaiduYunDownload/anhei/client/Assets/Plugins/Libs/LoaderLib.dll", "System.Void VersionManager::SetPlatformUpdateCallback()" };
        APIUpdaterRuntimeServices.AddComponent(DriverLib.Instance.get_gameObject(), string.Format("Assembly: {0} Method: {1}", (object[]) textArray1), str);
        AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("setUpdateCallBack", new object[] { "Driver" });
    }

    public byte[] defContentBytes { get; private set; }

    public static VersionManager Instance
    {
        [CompilerGenerated]
        get
        {
            return <Instance>k__BackingField;
        }
        [CompilerGenerated]
        set
        {
            <Instance>k__BackingField = value;
        }
    }

    public VersionManagerInfo LocalVersion { get; private set; }

    public VersionManagerInfo ServerVersion { get; private set; }
}

