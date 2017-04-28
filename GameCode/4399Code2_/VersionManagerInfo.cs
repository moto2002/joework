using Mogo.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;

public class VersionManagerInfo
{
    public VersionCodeInfo CompletePackageFullResourceVersionInfo = new VersionCodeInfo("0.0.0.0");
    public List<PackageInfo> FirstPackageInfoList = new List<PackageInfo>();
    public Dictionary<string, string> FirstPackageMD5Dic = new Dictionary<string, string>();
    public VersionCodeInfo FirstResourceVersionInfo = new VersionCodeInfo("0.0.0.0");
    public VersionCodeInfo FullResourceVersionInfo = new VersionCodeInfo("0.0.0.0");
    public List<PackageInfo> PackageInfoList = new List<PackageInfo>();
    public Dictionary<string, string> PackageMD5Dic = new Dictionary<string, string>();
    public VersionCodeInfo ProgramVersionInfo = new VersionCodeInfo("0.0.0.1");
    public VersionCodeInfo ResouceVersionInfo = new VersionCodeInfo("0.0.0.0");
    public VersionCodeInfo TinyPackageFirstResourceVersionInfo = new VersionCodeInfo("0.0.0.0");

    public VersionManagerInfo()
    {
        this.PackageUrl = string.Empty;
        this.ApkUrl = string.Empty;
        this.FirstApkUrl = string.Empty;
        this.ExportSwitch = false;
        this.IsPlatformUpdate = true;
        this.IsOpenUrl = false;
        this.IsFirstPkgOpenUrl = false;
        this.VoiceUrl = string.Empty;
    }

    public bool ReadFirstResourceList(string packageMD5Content)
    {
        return this.ReadMd5FromXML(packageMD5Content, ref this.FirstPackageMD5Dic, ref this.FirstPackageInfoList);
    }

    public bool ReadMd5FromXML(string packageMD5Content, ref Dictionary<string, string> packageMD5Dic, ref List<PackageInfo> packageInfoList)
    {
        SecurityElement element = XMLParser.LoadXML(packageMD5Content);
        if (element == null)
        {
            return false;
        }
        packageInfoList.Clear();
        packageMD5Dic.Clear();
        foreach (SecurityElement element2 in element.Children)
        {
            try
            {
                PackageInfo item = new PackageInfo();
                string str = element2.Attribute("n");
                item.Name = str;
                string str2 = str.Substring(7, str.Length - 11);
                string version = str2.Substring(0, str2.IndexOf('-'));
                item.LowVersion = new VersionCodeInfo(version);
                string str4 = str2.Substring(str2.IndexOf('-') + 1);
                item.HighVersion = new VersionCodeInfo(str4);
                item.Md5 = element2.Text;
                packageInfoList.Add(item);
                packageMD5Dic[item.Name] = item.Md5;
            }
            catch (Exception exception)
            {
                LoggerHelper.Except(exception, null);
            }
        }
        return true;
    }

    public bool ReadResourceList(string packageMD5Content)
    {
        return this.ReadMd5FromXML(packageMD5Content, ref this.PackageMD5Dic, ref this.PackageInfoList);
    }

    public override string ToString()
    {
        return this.ResouceVersionInfo.ToString();
    }

    public string ApkMd5 { get; set; }

    public string ApkUrl { get; set; }

    public string CompletePackageFullResourceVersion
    {
        get
        {
            return this.CompletePackageFullResourceVersionInfo.ToString();
        }
        set
        {
            this.CompletePackageFullResourceVersionInfo = new VersionCodeInfo(value);
        }
    }

    public bool ExportSwitch { get; set; }

    public string FirstApkMd5 { get; set; }

    public string FirstApkUrl { get; set; }

    public string FirstPackageMd5List { get; set; }

    public string FirstPackageUrl { get; set; }

    public string FirstResourceVersion
    {
        get
        {
            return this.FirstResourceVersionInfo.ToString();
        }
        set
        {
            this.FirstResourceVersionInfo = new VersionCodeInfo(value);
        }
    }

    public string FullResourceVersion
    {
        get
        {
            return this.FullResourceVersionInfo.ToString();
        }
        set
        {
            this.FullResourceVersionInfo = new VersionCodeInfo(value);
        }
    }

    public bool IsFirstPkgOpenUrl { get; set; }

    public bool IsOpenUrl { get; set; }

    public bool IsPlatformUpdate { get; set; }

    public string PackageMd5List { get; set; }

    public string PackageUrl { get; set; }

    public string ProgramVersion
    {
        get
        {
            return this.ProgramVersionInfo.ToString();
        }
        set
        {
            this.ProgramVersionInfo = new VersionCodeInfo(value);
        }
    }

    public string ResouceVersion
    {
        get
        {
            return this.ResouceVersionInfo.ToString();
        }
        set
        {
            this.ResouceVersionInfo = new VersionCodeInfo(value);
        }
    }

    public string TinyPackageFirstResourceVersion
    {
        get
        {
            return this.TinyPackageFirstResourceVersionInfo.ToString();
        }
        set
        {
            this.TinyPackageFirstResourceVersionInfo = new VersionCodeInfo(value);
        }
    }

    public string VoiceUrl { get; set; }
}

