using UnityEngine;
using System.Collections.Generic;

public class TestZip : MonoBehaviour
{
    string[] strs = new string[]
    {
            "nv1.unity3d",
            "nv2.unity3d",
            "nv3.unity3d",
            "nv5.unity3d",
            "nv6.unity3d",
            //"Map_01.wav",
    };

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 100, 50), "测试压缩"))
        {
            List<string> _strList = new List<string>(strs);
            for (int i = 0; i < strs.Length; i++)
            {
                _strList[i] = Application.dataPath + "/from/" + strs[i];
                //Debug.Log(_strList[i]);
            }
            ZipUtility.Zip(_strList.ToArray(), Application.dataPath + "/to/test.zip");
            Debug.Log("测试压缩 OK,保存路径->" + Application.dataPath + "/to/test.zip");

        }

        if (GUI.Button(new Rect(300, 100, 100, 50), "测试解压缩"))
        {
            ZipUtility.UnzipFile(Application.dataPath + "/to/test.zip", Application.dataPath + "/from");
            Debug.Log("测试解压缩 OK,保存路径->" + Application.dataPath + "/from");
        }
    }
}
