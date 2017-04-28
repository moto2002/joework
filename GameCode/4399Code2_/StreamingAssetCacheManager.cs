using Mogo.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StreamingAssetCacheManager : MonoBehaviour
{
    private static bool _bExportable;
    public static int FrameCount = 10;

    public void Export()
    {
        LoggerHelper.Debug(".................StreamingAssetCacheManager Export..............", true, 0);
        _bExportable = true;
    }

    private static IEnumerator ExportItem()
    {
        Stopwatch iteratorVariable0 = new Stopwatch();
        iteratorVariable0.Start();
        string[] allPathes = null;
        ResourceIndexInfo.Instance.Init(Application.streamingAssetsPath + "/ResourceIndexInfo.txt", delegate {
            allPathes = ResourceIndexInfo.Instance.GetLeftFilePathes();
            LoggerHelper.Debug("Export Items Count:" + allPathes.Length, true, 0);
        }, null);
        Stopwatch iteratorVariable1 = new Stopwatch();
        iteratorVariable1.Start();
        while ((allPathes == null) || (allPathes.Length == 0))
        {
            if (iteratorVariable1.ElapsedMilliseconds > 0xbb8L)
            {
                LoggerHelper.Info("Cache assets timeout", true);
                break;
            }
            LoggerHelper.Debug("allPathes==null", true, 0);
        }
        iteratorVariable1.Stop();
        LoggerHelper.Debug("allPathes!=null", true, 0);
        foreach (string iteratorVariable2 in allPathes)
        {
            while (!_bExportable)
            {
                LoggerHelper.Debug("Game busy,Not Export:" + _bExportable, true, 0);
                yield return 0;
            }
            string path = SystemConfig.ResourceFolder + iteratorVariable2;
            if (!File.Exists(path))
            {
                string streamPath = Utils.GetStreamPath(iteratorVariable2);
                LoggerHelper.Debug("Export：" + streamPath, true, 0);
                WWW iteratorVariable5 = new WWW(streamPath);
                yield return iteratorVariable5;
                LoggerHelper.Debug("Export Finish：" + streamPath, true, 0);
                if ((iteratorVariable5.bytes != null) && (iteratorVariable5.bytes.Length != 0))
                {
                    string fileName = path + "_temp";
                    XMLParser.SaveBytes(fileName, iteratorVariable5.bytes);
                    File.Move(fileName, path);
                    LoggerHelper.Debug("Cache asset to sd card：" + path, true, 0);
                }
                else
                {
                    LoggerHelper.Error(string.Format("file not exist: {0}", iteratorVariable2), true);
                }
                int frameCount = FrameCount;
                while (frameCount-- >= 0)
                {
                    yield return 0;
                }
            }
        }
        LoggerHelper.Info("Cache all assets finish,all time:" + (iteratorVariable0.ElapsedMilliseconds / 0x3e8L), true);
        iteratorVariable0.Stop();
        yield return 0;
    }

    public void Pause()
    {
        LoggerHelper.Debug(".................StreamingAssetCacheManager Pause..............", true, 0);
        _bExportable = false;
    }

    private void Start()
    {
        Instance = this;
        base.StartCoroutine(ExportItem());
    }

    public static StreamingAssetCacheManager Instance
    {
        [CompilerGenerated]
        get
        {
            return <Instance>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            <Instance>k__BackingField = value;
        }
    }

}

