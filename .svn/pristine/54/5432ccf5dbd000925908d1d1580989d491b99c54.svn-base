using UnityEngine;
using System.Collections;

public class TestFilePointDownload : MonoBehaviour
{
    FilePointDownLoad filePointDownload = new FilePointDownLoad();

    void Start()
    {
        filePointDownload.DownLoad("http://download.mozilla.org/?product=firefox-1.5.0.7&os=win&lang=en-US", Application.dataPath+"/test/2.zip", () =>
        {
            Debug.Log("finish");
        });
    }

    void Update()
    {

    }

    private void OnApplicationQuit()
    {
        filePointDownload.Close();
    }
}
