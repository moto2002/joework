
//#define NoUpdateValues
using UnityEngine;
using System.Collections;
using System.Net;
using System.Collections.Generic;
using System;

public class MyWWW:IWWW
{
    class MyWebClient : WebClient
    {
        public float timeout;

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest req = base.GetWebRequest(address);
            if (timeout > 0)
               
                req.Timeout = (int)(timeout * 1000);
            return req;
        }
    }

    MyWebClient client = new MyWebClient();

    public MyWWW()
    {
        client.DownloadStringCompleted += onDownloadStringCompleted;
       
#if NoUpdateValues
        client.UploadStringCompleted += onUploadStringCompleted;
#else
        client.UploadValuesCompleted += onUploadValuesCompleted;
#endif

    }

    public void DoUrl(string url, Action<Exception, string> onFinish, float timeout, string values = null, bool bValuesPost = false)
    {
        if (onFinish == null)
        {
            Debug.LogError("must have onFinish");
            return;

        }
        lock (_LockObj)
        {
            if (curEvent != null)
            {
                Debug.LogError("call Dourl one by one.");
                return;
            }
            bDown = false;
            curEvent = onFinish;
            client.timeout = timeout;
        }


        if (values != null)
        {
            if (bValuesPost)
            {
#if  NoUpdateValues
                string data = "values=" + Uri.EscapeDataString(values);
                data += "&t=tx";
                client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                client.UploadStringAsync(new Uri(url), data);

#else
                System.Collections.Specialized.NameValueCollection coll = new System.Collections.Specialized.NameValueCollection();
                coll ["values"] = values;
                coll ["t"] = "tx";
                client.UploadValuesAsync(new Uri(url), coll);
#endif
                return;
            } else
            {
                url += "&values=" + Uri.EscapeDataString(values);

            }
        }

        Uri u = new Uri(url);
        //Debug.Log("Uri=" + u);
        client.DownloadStringAsync(u);
    }

    Exception curExce;
    string curString;
    Action<Exception, string> curEvent;
    bool bDown = false;
    class LockObj
    {
    }
    LockObj _LockObj = new LockObj();

    void onDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
    {
        //Debug.Log("func:onDownloadStringCompleted");
        lock (_LockObj)
        {
            curExce = e.Error;
            curString = e.Result;
            bDown = true;
        }
    }
#if NoUpdateValues
    void onUploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
    {
        lock (_LockObj)
        {
            curExce = e.Error;
            curString = (e.Result);
            bDown = true;
        }
    }
#else
    void onUploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
    {
        lock (_LockObj)
        {
            curExce = e.Error;
            curString = System.Text.Encoding.UTF8.GetString(e.Result);
            bDown = true;
        }
    }
#endif


    public void Update()
    {
        if (bDown)
        {
            Exception _curExce = null;
            Action<Exception, string> _curEvent = null;
            string _curString = null;
            lock (_LockObj)
            {
                if (curEvent != null)
                {
                    bDown = false;
                    _curEvent = curEvent;
                    _curString = curString;
                    _curExce = curExce;
                    curEvent = null;
                    curExce = null;
                    curString = null;
                }
            }
            if (_curEvent != null)
            {
                _curEvent(_curExce, _curString);
            }

        }
    }

    public bool InUse
    {
        get
        {
            lock (_LockObj)
            {
                return (curEvent != null);
            }
        }
    }
}
