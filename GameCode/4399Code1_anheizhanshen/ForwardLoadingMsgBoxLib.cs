using System;

public class ForwardLoadingMsgBoxLib
{
    private static ForwardLoadingMsgBoxLib m_instance = new ForwardLoadingMsgBoxLib();

    private ForwardLoadingMsgBoxLib()
    {
    }

    public void Hide()
    {
        if (PluginCallback.Instance.Hide != null)
        {
            PluginCallback.Instance.Hide();
        }
    }

    public bool IsHide()
    {
        if (PluginCallback.Instance.IsHide != null)
        {
            return PluginCallback.Instance.IsHide();
        }
        return false;
    }

    public void ShowMsgBox(string okText, string cancelText, string content, Action<bool> onClick)
    {
        if (PluginCallback.Instance.ShowMsgBox != null)
        {
            PluginCallback.Instance.ShowMsgBox(okText, cancelText, content, onClick);
        }
    }

    public void ShowRetryMsgBox(string content, Action<bool> onClick)
    {
        if (PluginCallback.Instance.ShowRetryMsgBox != null)
        {
            PluginCallback.Instance.ShowRetryMsgBox(content, onClick);
        }
    }

    public static ForwardLoadingMsgBoxLib Instance
    {
        get
        {
            return m_instance;
        }
    }
}

