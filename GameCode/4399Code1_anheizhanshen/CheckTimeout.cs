using Mogo.Util;
using System;
using System.IO;

public class CheckTimeout
{
    public void AsynIsNetworkTimeout(Action<bool> AsynResult)
    {
        string cfgInfoUrl;
        if (File.Exists(SystemConfig.CfgPath))
        {
            bool flag = SystemConfig.LoadCfgInfo();
            cfgInfoUrl = SystemConfig.GetCfgInfoUrl("version");
            LoggerHelper.Info(string.Concat(new object[] { "cfg exist. ", flag, " networkUrl: ", cfgInfoUrl }), true);
        }
        else
        {
            cfgInfoUrl = Utils.LoadResource(Utils.GetFileNameWithoutExtention("cfg.xml", '/'));
        }
        int counter = 0;
        this.TryAsynDownloadText(cfgInfoUrl, counter, AsynResult);
    }

    private void TryAsynDownloadText(string url, int counter, Action<bool> AsynResult)
    {
        DownloadMgr.Instance.AsynDownLoadText(url, delegate (string text) {
            if (!string.IsNullOrEmpty(text))
            {
                AsynResult(true);
            }
            else
            {
                AsynResult(false);
            }
        }, () => AsynResult(false));
    }
}

