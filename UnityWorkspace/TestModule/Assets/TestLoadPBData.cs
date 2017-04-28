using UnityEngine;
using System.Collections;
using ProtoBuf;
using System;
using System.IO;
using HuanJueData;


public class TestLoadPBData : MonoBehaviour
{
    void Start()
    {
        //StartCoroutine(loaderPBDataTest((data) =>
        //{
        //    Debug.Log(data.Length);
        //    MemoryStream m2 = new MemoryStream(data);
        //    PBData cfg = ProtoBuf.Serializer.Deserialize<PBData>(m2);
        //    Debug.Log(cfg.HeroInfoDic.Count);
        //}));

        string savePath = Path.Combine(Application.streamingAssetsPath, "PBData.bytes");
        FileStream fs = new FileStream(savePath, FileMode.Open, FileAccess.Read);
        int len = (int)fs.Length;
        Debug.Log(len);
        byte[] array = new byte[len];
        fs.Read(array, 0, len);
        MemoryStream ms = new MemoryStream(array);
        PBData pbData = null;
        try
        {
            pbData = ProtoBuf.Serializer.Deserialize<PBData>(ms);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        foreach (var item in pbData.HeroInfoDic)
        {
            Debug.Log(item.Key + "|" + item.Value.Name);
            //Debug.LogError(item.Value.GetSkillInfo(item.Value.BaseSkill.ToString()).SkillIcon);
        }

        foreach (var item2 in pbData.SkillInfoDic)
        {
            Debug.Log(item2.Key + "|" + item2.Value.SkillName);

        }

    }

    IEnumerator loaderPBDataTest(Action<byte[]> callback)
    {
        WWW www = new WWW(Application.streamingAssetsPath + "/PBData.bytes");
        yield return www;
        if (www.isDone)
        {
            Debug.Log(www.bytesDownloaded);
            callback(www.bytes);
        }
    }
}
