using Mogo.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using UnityEngine;

public class VersionManager
{
    public string defContent = "";

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
        ResourceIndexInfo.Instance.Init(Application.streamingAssetsPath + "/ResourceIndexInfo.txt", delegate {
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
            DriverLib.Invoke(delegate {
                if (RuntimePlatform.IPhonePlayer == Application.platform)
                {
                    Action<bool> onClick = delegate (bool confirm) {
                        if (!confirm)
                        {
                            Application.Quit();
                        }
                    };
                    ForwardLoadingMsgBoxLib.Instance.ShowMsgBox(DefaultUI.dataMap[11].content, DefaultUI.dataMap[7].content, DefaultUI.dataMap[12].content, onClick);
                }
                else
                {
                    this.InstallApk(fileName);
                }
            });
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

    private void AsynDownloadApkNoExport(Action<int, int, string> taskProgress, string fileName, string url, string md5, Action<int, long, long> progress, Action finished, Action<Exception> error)
    {
        DownloadTask task = new DownloadTask {
            FileName = fileName,
            Url = url,
            Finished = finished,
            Error = error,
            TotalProgress = progress,
            MD5 = md5
        };
        LoggerHelper.Info("down load apk & md5: " + url + " " + md5, true);
        DownloadMgr.Instance.tasks = new List<DownloadTask> { task };
        DownloadMgr.Instance.TaskProgress = delegate (int total, int current, string filename) {
            if (taskProgress != null)
            {
                taskProgress(total, current, filename);
            }
        };
        DownloadMgr.Instance.AllDownloadFinished = delegate {
            DriverLib.Invoke(delegate {
                if (RuntimePlatform.IPhonePlayer == Application.platform)
                {
                    Action<bool> onClick = delegate (bool confirm) {
                        if (!confirm)
                        {
                            Application.Quit();
                        }
                    };
                    ForwardLoadingMsgBoxLib.Instance.ShowMsgBox(DefaultUI.dataMap[11].content, DefaultUI.dataMap[7].content, DefaultUI.dataMap[12].content, onClick);
                }
                else
                {
                    this.InstallApk(fileName);
                }
            });
            if (finished != null)
            {
                finished();
            }
            LoggerHelper.Debug("apk安装成功", true, 0);
        };
        DownloadMgr.Instance.CheckDownLoadList();
    }

    public void AsynDownloadUpdatePackage(Action<bool> fileDecompress, VersionCodeInfo serverVersion, VersionCodeInfo localVersion, Dictionary<string, string> packageMD5Dic, string packageUrl, string packageMd5List, Action<int, int, string> taskProgress, Action<int, long, long> progress, Action finished, Action<Exception> error, bool isFirstRes = false, bool forFullPkg = false)
    {
        LoggerHelper.Debug("下载包列表", true, 0);
        this.DownloadPackageInfoList(packageMd5List, delegate (List<PackageInfo> packageInfoList) {
            List<KeyValuePair<string, string>> downloadList = (from packageInfo in packageInfoList
                where (packageInfo.LowVersion.Compare(localVersion) >= 0) && (packageInfo.HighVersion.Compare(serverVersion) <= 0)
                select new KeyValuePair<string, string>(packageInfo.HighVersion.ToString(), packageInfo.Name)).ToList<KeyValuePair<string, string>>();
            if (downloadList.Count != 0)
            {
                LoggerHelper.Debug("开始下载包列表", true, 0);
                this.DownDownloadPackageList(fileDecompress, packageUrl, downloadList, packageMD5Dic, taskProgress, progress, finished, error, isFirstRes, forFullPkg);
            }
            else
            {
                LoggerHelper.Debug("更新包数目为0", true, 0);
                if (finished != null)
                {
                    finished();
                }
            }
        }, () => error(new Exception("DownloadPackageInfoList error.")), isFirstRes);
    }

    public void BeforeCheck(Action<UpdateType> AsynResult, Action OnError)
    {
        this.ServerVersion = new VersionManagerInfo();
        PropertyInfo[] properties = typeof(VersionManagerInfo).GetProperties();
        try
        {
            foreach (PropertyInfo info in properties)
            {
                if (info != null)
                {
                    string cfgInfoUrl = SystemConfig.GetCfgInfoUrl(info.Name);
                    if (!string.IsNullOrEmpty(cfgInfoUrl))
                    {
                        object obj2 = Utils.GetValue(cfgInfoUrl, info.PropertyType);
                        if (obj2 != null)
                        {
                            info.SetValue(this.ServerVersion, obj2, null);
                        }
                    }
                }
            }
        }
        catch (Exception exception)
        {
            LoggerHelper.Error("Get ServerVersion error: " + exception.Message, true);
        }
        LoggerHelper.Debug("服务器程序版本: " + this.ServerVersion.ProgramVersionInfo, true, 0);
        LoggerHelper.Debug("服务器资源版本: " + this.ServerVersion.ResouceVersionInfo, true, 0);
        LoggerHelper.Debug("服务器首包资源版本TinyPackageFirstResourceVersionInfo: " + this.ServerVersion.TinyPackageFirstResourceVersionInfo, true, 0);
        LoggerHelper.Debug("服务器完整包资源版本CompletePackageFullResourceVersionInfo: " + this.ServerVersion.CompletePackageFullResourceVersionInfo, true, 0);
        LoggerHelper.Debug("服务器首包资源版本FirstResourceVersion: " + this.ServerVersion.FirstResourceVersion, true, 0);
        LoggerHelper.Debug("服务器完整包资源版本FullResourceVersion: " + this.ServerVersion.FullResourceVersion, true, 0);
        LoggerHelper.Debug("服务器是否打开完整包下载:" + this.ServerVersion.IsOpenUrl, true, 0);
        LoggerHelper.Debug("服务器Apk地址: " + this.ServerVersion.ApkUrl, true, 0);
        LoggerHelper.Debug("服务器是否打开首包下载:" + this.ServerVersion.IsFirstPkgOpenUrl, true, 0);
        LoggerHelper.Debug("服务器Small Apk地址: " + this.ServerVersion.FirstApkUrl, true, 0);
        LoggerHelper.Debug("服务器增量包地址: " + this.ServerVersion.PackageUrl, true, 0);
        LoggerHelper.Debug("服务器md5地址: " + this.ServerVersion.PackageMd5List, true, 0);
        LoggerHelper.Debug("导出开关: " + this.ServerVersion.ExportSwitch, true, 0);
        LoggerHelper.Debug("平台更新开关: " + this.ServerVersion.IsPlatformUpdate, true, 0);
        LoggerHelper.Debug("聊天语音地址: " + this.ServerVersion.VoiceUrl, true, 0);
        bool flag = this.ServerVersion.CompletePackageFullResourceVersionInfo.Compare(this.LocalVersion.CompletePackageFullResourceVersionInfo) > 0;
        LoggerHelper.Debug("compareFullProgramVersion: " + flag, true, 0);
        bool flag2 = this.ServerVersion.ProgramVersionInfo.Compare(this.LocalVersion.ProgramVersionInfo) > 0;
        LoggerHelper.Debug("compareProgramVersion: " + flag2, true, 0);
        if (flag)
        {
            bool flag3 = this.ServerVersion.TinyPackageFirstResourceVersionInfo.Compare(this.LocalVersion.TinyPackageFirstResourceVersionInfo) > 0;
            AsynResult.BeginInvoke((flag2 || flag3) ? UpdateType.FirstRes : UpdateType.None, null, null);
        }
        else
        {
            bool flag4 = this.ServerVersion.ResouceVersionInfo.Compare(this.LocalVersion.ResouceVersionInfo) > 0;
            LoggerHelper.Debug("compareResourceVersion: " + flag4, true, 0);
            AsynResult.BeginInvoke((flag2 || flag4) ? UpdateType.NormalRes : UpdateType.None, null, null);
        }
    }

    public bool CheckAndDownload(Action<bool> fileDecompress, Action<int, int, string> taskProgress, Action<int, long, long> progress, Action finished, Action<Exception> error)
    {
        if (this.ServerVersion.ProgramVersionInfo.Compare(this.LocalVersion.ProgramVersionInfo) <= 0)
        {
            if (this.ServerVersion.ResouceVersionInfo.Compare(this.LocalVersion.ResouceVersionInfo) > 0)
            {
                LoggerHelper.Debug("服务器资源版本号比本地版本号大", true, 0);
                this.AsynDownloadUpdatePackage(fileDecompress, this.ServerVersion.ResouceVersionInfo, this.LocalVersion.ResouceVersionInfo, this.ServerVersion.PackageMD5Dic, this.ServerVersion.PackageUrl, this.ServerVersion.PackageMd5List, taskProgress, progress, finished, error, false, false);
                return true;
            }
            if (finished != null)
            {
                finished();
            }
            return false;
        }
        LoggerHelper.Debug("服务器apk版本高，下载apk", true, 0);
        string apkUrl = this.ServerVersion.ApkUrl;
        if (this.ServerVersion.IsOpenUrl)
        {
            this.OpenUrl(apkUrl);
            return true;
        }
        string resourceFolder = SystemConfig.ResourceFolder;
        RuntimePlatform platform = Application.platform;
        if (platform != RuntimePlatform.IPhonePlayer)
        {
            if (platform != RuntimePlatform.Android)
            {
                LoggerHelper.Error("Did not define the apkPath of " + Application.platform, true);
            }
            else
            {
                resourceFolder = Application.persistentDataPath + "/temp/";
            }
        }
        else
        {
            resourceFolder = Application.persistentDataPath + "/temp/";
        }
        if (!Directory.Exists(resourceFolder))
        {
            Directory.CreateDirectory(resourceFolder);
        }
        string fileName = resourceFolder + Utils.GetFileName(apkUrl, '/');
        Action action = delegate {
        };
        if (!Instance.ServerVersion.IsPlatformUpdate)
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

    public bool CheckAndDownloadFirstPkg(Action<bool> fileDecompress, Action<int, int, string> taskProgress, Action<int, long, long> progress, Action finished, Action<Exception> error)
    {
        if (this.ServerVersion.ProgramVersionInfo.Compare(this.LocalVersion.ProgramVersionInfo) <= 0)
        {
            if (this.ServerVersion.TinyPackageFirstResourceVersionInfo.Compare(this.LocalVersion.TinyPackageFirstResourceVersionInfo) > 0)
            {
                LoggerHelper.Debug("服务器资源版本号比本地版本号大", true, 0);
                this.AsynDownloadUpdatePackage(fileDecompress, this.ServerVersion.TinyPackageFirstResourceVersionInfo, this.LocalVersion.TinyPackageFirstResourceVersionInfo, this.ServerVersion.FirstPackageMD5Dic, this.ServerVersion.FirstPackageUrl, this.ServerVersion.FirstPackageMd5List, taskProgress, progress, finished, error, true, false);
                return true;
            }
            if (finished != null)
            {
                finished();
            }
            return false;
        }
        LoggerHelper.Debug("服务器apk版本高，下载apk", true, 0);
        string firstApkUrl = this.ServerVersion.FirstApkUrl;
        if (this.ServerVersion.IsFirstPkgOpenUrl)
        {
            this.OpenUrl(firstApkUrl);
            return true;
        }
        string resourceFolder = SystemConfig.ResourceFolder;
        RuntimePlatform platform = Application.platform;
        if (platform != RuntimePlatform.IPhonePlayer)
        {
            if (platform != RuntimePlatform.Android)
            {
                LoggerHelper.Error("Did not define the apkPath of " + Application.platform, true);
            }
            else
            {
                resourceFolder = Application.persistentDataPath + "/temp/";
            }
        }
        else
        {
            resourceFolder = Application.persistentDataPath + "/temp/";
        }
        if (!Directory.Exists(resourceFolder))
        {
            Directory.CreateDirectory(resourceFolder);
        }
        string fileName = resourceFolder + Utils.GetFileName(firstApkUrl, '/');
        if (!Instance.ServerVersion.IsPlatformUpdate)
        {
            this.AsynDownloadApk(taskProgress, fileName, firstApkUrl, progress, delegate {
            }, error);
        }
        else
        {
            this.PlatformUpdate();
        }
        return true;
    }

    public void CheckVersion(Action<bool> fileDecompress, Action<int, int, string> taskProgress, Action<int, long, long> progress, Action finished, Action<Exception> error)
    {
        this.BeforeCheck(delegate (UpdateType result) {
            Action check = null;
            Action action1 = null;
            LoggerHelper.Debug(result, true, 0);
            if (result == UpdateType.NormalRes)
            {
                if (check == null)
                {
                    check = () => this.CheckAndDownload(fileDecompress, taskProgress, progress, finished, error);
                }
                this.ModalCheckNetwork(check, error);
            }
            else if (result == UpdateType.FirstRes)
            {
                if (action1 == null)
                {
                    action1 = delegate {
                        this.LocalVersion.CompletePackageFullResourceVersionInfo = this.ServerVersion.CompletePackageFullResourceVersionInfo;
                        this.SaveVersion(this.LocalVersion);
                        this.CheckVersion(fileDecompress, taskProgress, progress, finished, error);
                    };
                }
                this.CheckAndDownloadFirstPkg(fileDecompress, taskProgress, progress, action1, error);
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

    private void DownDownloadPackageList(Action<bool> fileDecompress, string packageUrl, List<KeyValuePair<string, string>> downloadList, Dictionary<string, string> packageMD5Dic, Action<int, int, string> taskProgress, Action<int, long, long> progress, Action finished, Action<Exception> error, bool isFirstRes = false, bool forFullPkg = false)
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
                LoggerHelper.Debug("本地文件：" + System.IO.File.Exists(localFile), true, 0);
                if (System.IO.File.Exists(localFile))
                {
                    FileAccessManager.DecompressFile(localFile);
                }
                LoggerHelper.Debug("解压完成，保存版本信息到version.xml: " + isFirstRes, true, 0);
                if (System.IO.File.Exists(localFile))
                {
                    System.IO.File.Delete(localFile);
                }
                if (isFirstRes)
                {
                    if (forFullPkg)
                    {
                        this.LocalVersion.CompletePackageFullResourceVersionInfo = new VersionCodeInfo(kvp.Key);
                    }
                    else
                    {
                        this.LocalVersion.TinyPackageFirstResourceVersionInfo = new VersionCodeInfo(kvp.Key);
                    }
                }
                else
                {
                    this.LocalVersion.ResouceVersionInfo = new VersionCodeInfo(kvp.Key);
                }
                this.SaveVersion(this.LocalVersion);
            };
            string str = packageUrl + kvp.Value;
            LoggerHelper.Debug("fileUrl：" + str, true, 0);
            DownloadTask item = new DownloadTask {
                FileName = localFile,
                Url = str,
                Finished = action,
                Error = error,
                TotalProgress = progress
            };
            string key = kvp.Value;
            LoggerHelper.Debug(item.FileName + "::fileNoEXtension::" + key, true, 0);
            if (packageMD5Dic.ContainsKey(key))
            {
                item.MD5 = packageMD5Dic[key];
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

    private void DownloadPackageInfoList(string packageMd5List, Action<List<PackageInfo>> AsynResult, Action OnError, bool isFirstRes = false)
    {
        DownloadMgr.Instance.AsynDownLoadText(packageMd5List, delegate (string content) {
            bool flag;
            if (isFirstRes)
            {
                flag = this.ServerVersion.ReadFirstResourceList(content);
            }
            else
            {
                flag = this.ServerVersion.ReadResourceList(content);
            }
            if (flag)
            {
                if (isFirstRes)
                {
                    AsynResult(this.ServerVersion.FirstPackageInfoList);
                }
                else
                {
                    AsynResult(this.ServerVersion.PackageInfoList);
                }
            }
            else if (OnError != null)
            {
                OnError();
            }
        }, OnError);
    }

    internal void ExportStreamingAssetWhenDownloadApk(Action finished)
    {
        ResourceIndexInfo.Instance.Init(Application.streamingAssetsPath + "/ResourceIndexInfo.txt", delegate {
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

    public List<PackageInfo> GetUpdateDownloadPackageInfoList(Action<List<PackageInfo>> AsynResult, Action OnError)
    {
        DownloadMgr.Instance.AsynDownLoadText(this.ServerVersion.PackageMd5List, delegate (string content) {
            if (this.ServerVersion.ReadResourceList(content))
            {
                AsynResult(this.ServerVersion.PackageInfoList);
            }
            else if (OnError != null)
            {
                OnError();
            }
        }, OnError);
        return null;
    }

    public VersionManagerInfo GetVersionInXML(string xml)
    {
        Exception exception;
        try
        {
            SecurityElement element = XMLParser.LoadXML(xml);
            if (((element != null) && (element.Children != null)) && (element.Children.Count != 0))
            {
                VersionManagerInfo info = new VersionManagerInfo();
                PropertyInfo[] properties = typeof(VersionManagerInfo).GetProperties();
                foreach (SecurityElement element2 in element.Children)
                {
                    try
                    {
                        foreach (PropertyInfo info2 in properties)
                        {
                            if (((info2 != null) && (info2.Name == element2.Tag)) && !string.IsNullOrEmpty(element2.Text))
                            {
                                object obj2 = Utils.GetValue(element2.Text, info2.PropertyType);
                                if (obj2 != null)
                                {
                                    info2.SetValue(info, obj2, null);
                                }
                            }
                        }
                    }
                    catch (Exception exception1)
                    {
                        exception = exception1;
                        LoggerHelper.Error("GetVersionInXML error: " + element2.Tag + "\n" + exception.Message, true);
                    }
                }
                return info;
            }
            return new VersionManagerInfo();
        }
        catch (Exception exception2)
        {
            exception = exception2;
            LoggerHelper.Except(exception, null);
            return new VersionManagerInfo();
        }
    }

    public void Init()
    {
        this.AddListeners();
    }

    private void InstallApk(string apkPath)
    {
        LoggerHelper.Info("Call Install apk: " + apkPath, true);
        AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("InstallApk", new object[] { apkPath });
        TimerHeap.AddTimer(0x3e8, 0, new Action(Application.Quit));
    }

    public void LoadLocalVersion()
    {
        if (System.IO.File.Exists(SystemConfig.VersionPath))
        {
            string xml = Utils.LoadFile(SystemConfig.VersionPath);
            this.LocalVersion = this.GetVersionInXML(xml);
            TextAsset asset = Resources.Load("version") as TextAsset;
            if (!((asset == null) || string.IsNullOrEmpty(asset.text)))
            {
                this.LocalVersion.ProgramVersionInfo = this.GetVersionInXML(asset.text).ProgramVersionInfo;
            }
            LoggerHelper.Info("program version : " + this.LocalVersion.ProgramVersion + " resource version :" + this.LocalVersion.ResouceVersion + " first resource version : " + this.LocalVersion.TinyPackageFirstResourceVersion, true);
        }
        else
        {
            this.LocalVersion = new VersionManagerInfo();
            LoggerHelper.Info("cannot find local version", true);
        }
    }

    public void ModalCheckNetwork(Action check, Action<Exception> error)
    {
        DriverLib.Invoke(delegate {
            Action<List<PackageInfo>> asynResult = null;
            Action onError = null;
            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                if (asynResult == null)
                {
                    asynResult = delegate (List<PackageInfo> packageList) {
                        float num = 0f;
                        int count = 0;
                        if (packageList.Count != 0)
                        {
                            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                            foreach (PackageInfo info in packageList)
                            {
                                if ((info.LowVersion.Compare(this.LocalVersion.ResouceVersionInfo) >= 0) && (info.HighVersion.Compare(this.ServerVersion.ResouceVersionInfo) <= 0))
                                {
                                    list.Add(new KeyValuePair<string, string>(info.HighVersion.ToString(), info.Name));
                                }
                            }
                            if (list.Count != 0)
                            {
                                count = list.Count;
                                foreach (KeyValuePair<string, string> pair in list)
                                {
                                    string requestUriString = this.ServerVersion.PackageUrl + pair.Value;
                                    try
                                    {
                                        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUriString);
                                        HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                                        num += response.ContentLength;
                                        response.Close();
                                        request.Abort();
                                    }
                                    catch (Exception exception)
                                    {
                                        error(exception);
                                    }
                                }
                            }
                        }
                        num /= 1048576f;
                        LoggerHelper.Debug("3G network", true, 0);
                        Dictionary<int, DefaultLanguageData> dataMap = DefaultUI.dataMap;
                        string content = string.Format(dataMap[5].content, count, string.Format("{0:0.00}", num));
                        ForwardLoadingMsgBoxLib.Instance.ShowMsgBox(dataMap[6].content, dataMap[7].content, content, delegate (bool confirm) {
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
                        });
                    };
                }
                if (onError == null)
                {
                    onError = () => error(new Exception("DownloadPackageInfoList error."));
                }
                Instance.GetUpdateDownloadPackageInfoList(asynResult, onError);
            }
            else
            {
                check();
            }
        });
    }

    public void OpenUrl(string url)
    {
        ForwardLoadingMsgBoxLib.Instance.ShowMsgBox(DefaultUI.dataMap[0x7d2].content, DefaultUI.dataMap[7].content, DefaultUI.dataMap[0x7d1].content, delegate (bool b) {
            if (b)
            {
                Application.OpenURL(url);
            }
            Application.Quit();
        });
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

    public void SaveVersion(VersionManagerInfo version)
    {
        LoggerHelper.Debug("call SaveVersion", true, 0);
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
        DriverLib.Instance.gameObject.AddComponent("PlatformUpdateCallback");
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

