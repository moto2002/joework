using System;
using System.Runtime.CompilerServices;

public class DownloadTask
{
    public bool bDownloadAgain = false;
    public bool bFineshed = false;

    public void OnBytesReceived(long size)
    {
        if (this.BytesReceived != null)
        {
            this.BytesReceived(size);
        }
    }

    public void OnError(Exception ex)
    {
        if (this.Error != null)
        {
            this.Error(ex);
        }
    }

    public void OnFinished()
    {
        if (this.Finished != null)
        {
            this.Finished();
        }
    }

    public void OnProgress(int p)
    {
        if (this.Progress != null)
        {
            this.Progress(p);
        }
    }

    public void OnTotalBytesToReceive(long size)
    {
        if (this.TotalBytesToReceive != null)
        {
            this.TotalBytesToReceive(size);
        }
    }

    public void OnTotalProgress(int p, long totalSize, long receivedSize)
    {
        if (this.TotalProgress != null)
        {
            this.TotalProgress(p, totalSize, receivedSize);
        }
    }

    public Action<long> BytesReceived { get; set; }

    public Action<Exception> Error { get; set; }

    public string FileName { get; set; }

    public Action Finished { get; set; }

    public string MD5 { get; set; }

    public Action<int> Progress { get; set; }

    public Action<long> TotalBytesToReceive { get; set; }

    public Action<int, long, long> TotalProgress { get; set; }

    public string Url { get; set; }
}

