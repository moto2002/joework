using System;
using System.Runtime.CompilerServices;

public class ThreadDownloadBreakPoint
{
    public ThreadDownloadBreakPoint()
    {
    }

    public ThreadDownloadBreakPoint(DownloadMgr mgr, DownloadTask task)
    {
        this.Mgr = mgr;
        this.Task = task;
    }

    internal void Download()
    {
        this.Mgr.DownloadFileBreakPoint(this.Task.Url, this.Task.FileName);
    }

    public DownloadMgr Mgr { get; set; }

    public DownloadTask Task { get; set; }
}

