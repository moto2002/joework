using Mogo.Util;
using System;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class DefaultUI
{
    public static Dictionary<int, DefaultLanguageData> dataMap = new Dictionary<int, DefaultLanguageData>();
    public static Dictionary<int, DefaulSoundData> soundDataMap = new Dictionary<int, DefaulSoundData>();

    public static void HideLoading()
    {
        if (PluginCallback.Instance.ShowGlobleLoadingUI != null)
        {
            PluginCallback.Instance.ShowGlobleLoadingUI(false);
        }
    }

    public static void InitLanguageInfo()
    {
        TextAsset asset = Resources.Load("Default/DefaultXml/DefaultChineseData") as TextAsset;
        if (asset != 0)
        {
            SecurityElement xml = XMLParser.LoadXML(asset.text);
            if (xml != null)
            {
                Dictionary<int, Dictionary<string, string>> dictionary = XMLParser.LoadIntMap(xml, "DefaultLanguage");
                foreach (KeyValuePair<int, Dictionary<string, string>> pair in dictionary)
                {
                    DefaultLanguageData data = new DefaultLanguageData {
                        id = pair.Key,
                        content = pair.Value["content"]
                    };
                    dataMap.Add(pair.Key, data);
                }
            }
            else
            {
                LoggerHelper.Error("Default Language xml null.", true);
            }
        }
        else
        {
            LoggerHelper.Error("Default Language textAsset null.", true);
        }
    }

    public static void InitSoundInfo()
    {
        TextAsset asset = Resources.Load("Default/DefaultSoundData") as TextAsset;
        if (asset != 0)
        {
            SecurityElement xml = XMLParser.LoadXML(asset.text);
            if (xml != null)
            {
                Dictionary<int, Dictionary<string, string>> dictionary = XMLParser.LoadIntMap(xml, "DefaultLanguage");
                foreach (KeyValuePair<int, Dictionary<string, string>> pair in dictionary)
                {
                    DefaulSoundData data = new DefaulSoundData {
                        id = pair.Key,
                        path = pair.Value["path"]
                    };
                    soundDataMap.Add(pair.Key, data);
                }
            }
            else
            {
                LoggerHelper.Error("Default Sound xml null.", true);
            }
        }
        else
        {
            LoggerHelper.Error("Default Sound textAsset null.", true);
        }
    }

    public static void Loading(int progress)
    {
        if (PluginCallback.Instance.ShowGlobleLoadingUI != null)
        {
            PluginCallback.Instance.SetLoadingStatus(progress);
        }
    }

    public static void SetLoadingStatusTip(object tip, params object[] args)
    {
        if (PluginCallback.Instance.SetLoadingStatusTip != null)
        {
            PluginCallback.Instance.SetLoadingStatusTip(string.Format(tip.ToString(), args));
        }
    }

    public static void ShowLoading()
    {
        if (PluginCallback.Instance.ShowGlobleLoadingUI != null)
        {
            PluginCallback.Instance.ShowGlobleLoadingUI(true);
        }
    }
}

