using System;
using UnityEngine;

internal class DownloadObserver : MonoBehaviour
{
    private void AsyncDownloadCallBack()
    {
    }

    private void SyncDownloadCallBack(string data)
    {
        DownloadManager.Singleton.Buffer = data;
    }
}

