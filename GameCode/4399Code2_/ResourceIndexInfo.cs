using Mogo.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using UnityEngine;

public class ResourceIndexInfo
{
    private static ResourceIndexInfo m_Instacne = null;
    private Dictionary<string, string> m_ResourceIndexes = new Dictionary<string, string>();
    public List<string> MetaList = new List<string>();

    public void Destroy()
    {
        this.m_ResourceIndexes.Clear();
        m_Instacne = null;
    }

    public bool Exist()
    {
        return (this.m_ResourceIndexes.Count != 0);
    }

    public bool Exsit(string strFileName)
    {
        return this.m_ResourceIndexes.ContainsKey(strFileName);
    }

    public string[] GetAllFullPathes()
    {
        List<string> list = new List<string>();
        foreach (KeyValuePair<string, string> pair in this.m_ResourceIndexes)
        {
            list.Add(pair.Value);
        }
        return list.ToArray();
    }

    public List<string> GetFileNamesByDirectory(string strDir)
    {
        List<string> list = new List<string>();
        foreach (KeyValuePair<string, string> pair in this.m_ResourceIndexes)
        {
            if (pair.Value.StartsWith(strDir))
            {
                list.Add(pair.Value);
            }
        }
        return list;
    }

    public List<string> GetFirstTimeResourceFilePathes()
    {
        List<string> list = new List<string>();
        foreach (KeyValuePair<string, string> pair in this.m_ResourceIndexes)
        {
            if (pair.Value.StartsWith("data/"))
            {
                list.Add(pair.Value);
            }
        }
        list.Add(DriverLib.FileName);
        return list;
    }

    public string GetFullPath(string strFileName)
    {
        if (this.m_ResourceIndexes.ContainsKey(strFileName))
        {
            return this.m_ResourceIndexes[strFileName];
        }
        return "";
    }

    public string[] GetLeftFilePathes()
    {
        List<string> list = new List<string>();
        foreach (KeyValuePair<string, string> pair in this.m_ResourceIndexes)
        {
            if (!(pair.Value.StartsWith("data/") || pair.Value.Contains(DriverLib.FileName)))
            {
                list.Add(pair.Value);
            }
        }
        return list.ToArray();
    }

    public void Init(string strResourceIndexFile, Action finished, Action fail = null)
    {
        Action action = null;
        if (this.m_ResourceIndexes.Count == 0)
        {
            if (action == null)
            {
                action = delegate {
                    if (!File.Exists(SystemConfig.VersionPath))
                    {
                        TextAsset asset = Resources.Load("version") as TextAsset;
                        if (asset != null)
                        {
                            XMLParser.SaveText(SystemConfig.VersionPath, asset.text);
                        }
                    }
                    finished();
                };
            }
            DriverLib.Instance.StartCoroutine(this.ReadTxtFile(strResourceIndexFile, action, fail));
        }
        else
        {
            finished();
        }
    }

    private IEnumerator ReadTxtFile(string strResourceIndexFile, Action finished, Action fail = null)
    {
        if (!SystemSwitch.UseFileSystem)
        {
            this.MetaList.Clear();
            string url = Application.streamingAssetsPath + "/Meta.xml";
            WWW iteratorVariable1 = null;
            if (Application.platform == RuntimePlatform.Android)
            {
                iteratorVariable1 = new WWW(url);
            }
            else if (File.Exists(url))
            {
                iteratorVariable1 = new WWW("file://" + url);
            }
            if (null != iteratorVariable1)
            {
                yield return iteratorVariable1;
                this.MetaList.Add("Meta.xml");
                if (string.IsNullOrEmpty(iteratorVariable1.text))
                {
                    LoggerHelper.Debug("Meta.xml not exit in StreamingAssets", true, 0);
                }
                else
                {
                    SecurityElement element = XMLParser.LoadXML(iteratorVariable1.text);
                    foreach (SecurityElement element2 in element.Children)
                    {
                        string item = element2.Attribute("path");
                        if (item != null)
                        {
                            this.MetaList.Add(item);
                        }
                        else
                        {
                            LoggerHelper.Error("Path not exit in Meta.xml", true);
                        }
                    }
                }
            }
        }
        WWW iteratorVariable2 = null;
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                iteratorVariable2 = new WWW(strResourceIndexFile);
            }
            else if (File.Exists(strResourceIndexFile))
            {
                iteratorVariable2 = new WWW("file://" + strResourceIndexFile);
            }
        }
        catch (Exception exception)
        {
            iteratorVariable2 = null;
            LoggerHelper.Debug(exception + " this message is not harmless,this means the APK is not the most first APK of our game", true, 0);
        }
        if (null != iteratorVariable2)
        {
            yield return iteratorVariable2;
            string[] iteratorVariable3 = iteratorVariable2.text.Split(new char[] { '\n' });
            int iteratorVariable4 = -1;
            for (int j = iteratorVariable3.Length; j > 0; j--)
            {
                iteratorVariable4 = iteratorVariable3[j - 1].LastIndexOf("/");
                if (iteratorVariable4 <= 0)
                {
                    if (iteratorVariable3[j - 1].EndsWith("xml") && !this.m_ResourceIndexes.ContainsKey(iteratorVariable3[j - 1]))
                    {
                        this.m_ResourceIndexes.Add(iteratorVariable3[j - 1], iteratorVariable3[j - 1]);
                    }
                }
                else if (!iteratorVariable3[j - 1].EndsWith("xml"))
                {
                    if (!this.m_ResourceIndexes.ContainsKey(iteratorVariable3[j - 1].Substring(iteratorVariable4 + 1, (iteratorVariable3[j - 1].Length - iteratorVariable4) - 1)))
                    {
                        this.m_ResourceIndexes.Add(iteratorVariable3[j - 1].Substring(iteratorVariable4 + 1, (iteratorVariable3[j - 1].Length - iteratorVariable4) - 1), iteratorVariable3[j - 1]);
                    }
                }
                else if (!this.m_ResourceIndexes.ContainsKey(iteratorVariable3[j - 1]))
                {
                    this.m_ResourceIndexes.Add(iteratorVariable3[j - 1], iteratorVariable3[j - 1]);
                }
            }
            iteratorVariable2.Dispose();
        }
        finished();
    }

    public static ResourceIndexInfo Instance
    {
        get
        {
            if (null == m_Instacne)
            {
                m_Instacne = new ResourceIndexInfo();
            }
            return m_Instacne;
        }
    }

}

