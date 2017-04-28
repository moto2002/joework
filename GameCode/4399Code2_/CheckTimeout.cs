using Mogo.Util;
using System;
using System.IO;

public class CheckTimeout
{
    public void AsynIsNetworkTimeout(Action<bool> AsynResult)
    {
        Action<bool> callback = null;
        string networkUrl;
        if (File.Exists(SystemConfig.CfgPath))
        {
            if (callback == null)
            {
                callback = delegate (bool result) {
                    networkUrl = SystemConfig.GetCfgInfoUrl("serverlist");
                    LoggerHelper.Info(string.Concat(new object[] { "cfg exist. ", result, " networkUrl: ", networkUrl }), true);
                    this.TryAsynDownloadText(networkUrl, AsynResult);
                };
            }
            SystemConfig.LoadCfgInfo(callback);
        }
        else
        {
            networkUrl = Utils.LoadResource(Utils.GetFileNameWithoutExtention("cfg.xml", '/'));
            this.TryAsynDownloadText(networkUrl, AsynResult);
        }
    }

    private void TryAsynDownloadText(string url, Action<bool> AsynResult)
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

