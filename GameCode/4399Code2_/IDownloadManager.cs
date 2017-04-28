using System;

public interface IDownloadManager
{
    void AdvancedAsyncDownloadFileAndSaveAs(DownloadUnit[] downloadLists, Action callback);
    void AsyncDownload(string url, Action callback);
    void AsyncDownloadFileAndSaveAs(string url, string localPath);
    void SyncDownload(string url, Action<string> callback);
    void SyncDownloadFileAndSaveAs(string url, string localPath);
}

