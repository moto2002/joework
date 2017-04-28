using System;

public class PluginCallback
{
    public Action Hide;
    public Func<bool> IsHide;
    private static PluginCallback m_instance = new PluginCallback();
    public Action<int> SetLoadingStatus;
    public Action<string> SetLoadingStatusTip;
    public Action<bool> ShowGlobleLoadingUI;
    public Action<string, string, string, Action<bool>> ShowMsgBox;
    public Action<string, Action<bool>> ShowRetryMsgBox;

    private PluginCallback()
    {
    }

    public static PluginCallback Instance
    {
        get
        {
            return m_instance;
        }
    }
}

