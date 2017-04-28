using Mogo.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;

public class VersionManagerInfo
{
    public Dictionary<string, string> PackageMD5Dic = new Dictionary<string, string>();
    public VersionCodeInfo ProgramVersionInfo = new VersionCodeInfo("0.0.0.1");
    public VersionCodeInfo ResouceVersionInfo = new VersionCodeInfo("0.0.0.0");

    public VersionManagerInfo()
    {
        this.PackageList = string.Empty;
        this.PackageUrl = string.Empty;
        this.ApkUrl = string.Empty;
    }

    public bool IsDefault()
    {
        return ((((this.ProgramVersionInfo.Compare(new VersionCodeInfo("0.0.0.1")) == 0) && (this.ResouceVersionInfo.Compare(new VersionCodeInfo("0.0.0.0")) == 0)) && ((this.PackageList == string.Empty) && (this.PackageUrl == string.Empty))) && (this.ApkUrl == string.Empty));
    }

    public void ReadMd5FromXML(string packageMD5Content)
    {
        SecurityElement element = XMLParser.LoadXML(packageMD5Content);
        if (element != null)
        {
            foreach (SecurityElement element2 in element.Children)
            {
                this.PackageMD5Dic[element2.Attribute("n")] = element2.Text;
            }
        }
    }

    public override string ToString()
    {
        return this.ResouceVersionInfo.ToString();
    }

    public string ApkMd5 { get; set; }

    public string ApkUrl { get; set; }

    public string PackageList { get; set; }

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
}

