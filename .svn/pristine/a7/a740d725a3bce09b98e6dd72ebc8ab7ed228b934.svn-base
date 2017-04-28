using System;

public interface IWWW
{
    void DoUrl(string url, Action<Exception, string> onFinish, float timeout,string values = null, bool bValuesPost = false);
    void Update();

    bool InUse
    {
        get;
    }
}

public class TimeoutEx : Exception
{
    public TimeoutEx(string str)
    {
        throw new System.ArgumentException(str); 
    }
}