using Mogo.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StreamingAssetManager
{
    private string[] allPathes;
    public bool ApkFinished = false;
    private bool bUpdateProgress = true;
    private int index = 0;
    private int taskBatchCount = 10;
    private string tempFile = (SystemConfig.ResourceFolder + "temp");
    private bool way2Compute = true;

    private bool CheckNeedFirstExport()
    {
        if (File.Exists(SystemConfig.VersionPath))
        {
            if (SystemSwitch.UseFileSystem)
            {
                if (File.Exists(SystemConfig.ResourceFolder + MogoFileSystem.FILE_NAME))
                {
                    return false;
                }
            }
            else if (Directory.Exists(SystemConfig.ResourceFolder + "data") && File.Exists(SystemConfig.ResourceFolder + DriverLib.FileName))
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator ExportAll(string[] paths)
    {
        Action<WWW> loading = null;
        if (paths.Length != 1)
        {
            LoggerHelper.Debug("ExportAll more than one", true, 0);
            while (this.index < paths.Length)
            {
                string streamPath = null;
                string iteratorVariable1 = paths[this.index++];
                string path = SystemConfig.ResourceFolder + iteratorVariable1;
                if (!File.Exists(path))
                {
                    streamPath = Utils.GetStreamPath(iteratorVariable1);
                    WWW iteratorVariable3 = new WWW(streamPath);
                    yield return iteratorVariable3;
                    if ((iteratorVariable3.get_bytes() != null) && (iteratorVariable3.get_bytes().Length != 0))
                    {
                        this.tempFile = path + "_temp";
                        XMLParser.SaveBytes(this.tempFile, iteratorVariable3.get_bytes());
                        File.Move(this.tempFile, path);
                        iteratorVariable3 = null;
                    }
                    else
                    {
                        LoggerHelper.Error("file not exist: " + streamPath, true);
                    }
                }
                if (this.UpdateProgress)
                {
                    float num;
                    if (this.way2Compute)
                    {
                        num = ((float) (this.index * 100)) / ((float) this.taskBatchCount);
                        DefaultUI.Loading((int) num);
                        DefaultUI.SetLoadingStatusTip(DefaultUI.dataMap.Get<int, DefaultLanguageData>(3).content, new object[0]);
                    }
                    else
                    {
                        num = ((((float) (this.index - (paths.Length - this.taskBatchCount))) % ((float) this.taskBatchCount)) * 100f) / ((float) this.taskBatchCount);
                        DefaultUI.Loading((int) num);
                        DefaultUI.SetLoadingStatusTip(DefaultUI.dataMap.Get<int, DefaultLanguageData>(10).content, new object[] { Path.GetFileName(iteratorVariable1) });
                    }
                    DefaultUI.Loading((int) num);
                }
            }
            this.allPathes = null;
            if (this.AllFinished != null)
            {
                LoggerHelper.Debug("导出资源AllFinished", true, 0);
                this.AllFinished();
            }
            else
            {
                LoggerHelper.Debug("导出资源AllFinished为null", true, 0);
            }
            yield break;
        }
        Action<byte[]> loaded = null;
        LoggerHelper.Debug("ExportAll one", true, 0);
        string fullpath = null;
        string fileName = paths[0];
        string target = SystemConfig.ResourceFolder + fileName;
        LoggerHelper.Debug("export target:" + target, true, 0);
        if (!File.Exists(target))
        {
            LoggerHelper.Debug("target not exit", true, 0);
            fullpath = Utils.GetStreamPath(fileName);
            if (loading == null)
            {
                loading = delegate (WWW www) {
                    DriverLib.Instance.StartCoroutine(this.Loading(www, delegate (float p) {
                        DefaultUI.Loading((int) p);
                        DefaultUI.SetLoadingStatusTip(DefaultUI.dataMap.Get<int, DefaultLanguageData>(4).content, new object[0]);
                    }));
                };
            }
            if (loaded == null)
            {
                loaded = delegate (byte[] pkg) {
                    if ((pkg != null) && (pkg.Length != 0))
                    {
                        this.tempFile = target + "_temp";
                        XMLParser.SaveBytes(this.tempFile, pkg);
                        File.Move(this.tempFile, target);
                        pkg = null;
                    }
                    else
                    {
                        LoggerHelper.Error("export one file not exist: " + fullpath, true);
                    }
                    this.allPathes = null;
                    if (this.AllFinished != null)
                    {
                        LoggerHelper.Debug("导出一个资源AllFinished", true, 0);
                        this.AllFinished();
                    }
                    else
                    {
                        LoggerHelper.Debug("导出一个资源AllFinished为null", true, 0);
                    }
                };
            }
            DriverLib.Instance.StartCoroutine(this.LoadWWW(fullpath, loading, loaded));
        }
    }

    public void FirstExport()
    {
        if (!this.CheckNeedFirstExport())
        {
            LoggerHelper.Debug("firstExport not export", true, 0);
            this.AllFinished();
        }
        else
        {
            LoggerHelper.Debug("firstExport export one", true, 0);
            this.way2Compute = true;
            List<string> list = new List<string>();
            if (SystemSwitch.UseFileSystem)
            {
                list.Add(MogoFileSystem.FILE_NAME);
            }
            else
            {
                List<string> firstTimeResourceFilePathes = ResourceIndexInfo.Instance.GetFirstTimeResourceFilePathes();
                List<string> metaList = ResourceIndexInfo.Instance.MetaList;
                list.AddRange(firstTimeResourceFilePathes);
                list.AddRange(metaList);
            }
            this.allPathes = list.ToArray();
            this.taskBatchCount = this.allPathes.Length;
            DriverLib.Instance.StartCoroutine(this.ExportAll(this.allPathes));
        }
    }

    public IEnumerator Loading(WWW www, Action<float> loading)
    {
        while ((www != null) && !www.get_isDone())
        {
            if (loading != null)
            {
                loading(www.get_progress());
            }
            yield return null;
        }
    }

    public IEnumerator LoadWWW(string path, Action<WWW> loading, Action<byte[]> loaded)
    {
        WWW iteratorVariable0 = new WWW(path);
        if (loading != null)
        {
            loading(iteratorVariable0);
        }
        yield return iteratorVariable0;
        if (loaded != null)
        {
            loaded(iteratorVariable0.get_bytes());
        }
        iteratorVariable0 = null;
    }

    public void UpdateApkExport()
    {
        LoggerHelper.Info("更新apk时导出资源", true);
        this.allPathes = null;
        this.way2Compute = VersionManager.Instance.IsPlatformUpdate;
        this.allPathes = ResourceIndexInfo.Instance.GetLeftFilePathes();
        this.taskBatchCount = this.allPathes.Length;
        DriverLib.Instance.StartCoroutine(this.ExportAll(this.allPathes));
    }

    public Action AllFinished { get; set; }

    public bool UpdateProgress
    {
        get
        {
            return this.bUpdateProgress;
        }
        set
        {
            if (value && (this.allPathes != null))
            {
                this.taskBatchCount = this.allPathes.Length - this.index;
            }
            this.bUpdateProgress = value;
        }
    }



}

