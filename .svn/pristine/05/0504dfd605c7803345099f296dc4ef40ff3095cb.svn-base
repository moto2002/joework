using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class WWWQueue
{
	public WWWQueue()
	{
		this.www = new MyWWWNative ();
		//this.DoUrl (URL, action, time);
	}


    public IWWW www
    {
        get;
        set;
    }

	Queue<RequestInfo> requests = new Queue<RequestInfo>();

    public class RequestInfo
    {
        string url;
        Action<Exception, string> onFinish;
		float timeout;
        string values;
        bool bValuesPost;
        public RequestInfo(string url, Action<Exception, string> onFinish, float timeout, string values, bool bValuesPost)
        {
            this.url = url;
            this.onFinish = onFinish;
            this.timeout = timeout;
            this.values = values;
            this.bValuesPost = bValuesPost;
        }
        public void DoUrl(IWWW www)
        {
            www.DoUrl(url, onFinish, timeout, values, bValuesPost);
        }
    }

    public void DoUrl(string url, Action<Exception, string> onFinish = null, float timeout = 5f, string values = null, bool bValuesPost = false)
    {
        if (requests.Count == 0 && !www.InUse)
        {
            www.DoUrl(url, onFinish, timeout, values, bValuesPost);
        }
        else
        {
            requests.Enqueue(new RequestInfo(url, onFinish, timeout, values, bValuesPost));
        }
    }

    public void Update()
    {
        www.Update();
        if (!www.InUse && requests.Count > 0)
        {
            requests.Dequeue().DoUrl(www);
        }
    }
}

