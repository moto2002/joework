using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class testmywww : MonoBehaviour
{
    WWWQueue mywwwqueue;

    string webUrl = "http://120.26.71.230/GameRes/";

    List<string> resNames = new List<string>();

    static string savePath = Application.persistentDataPath + "/Game/";

    void Start()
    {
        Debug.Log("persistentDataPath:" + Application.persistentDataPath);

       // this.resNames.Add("config/testRes.txt");
        //this.resNames.Add("config/1.xml");
        this.resNames.Add("image/logo.png");

        

        mywwwqueue = new WWWQueue();

        for (int i = 0; i < this.resNames.Count; i++)
        {
            mywwwqueue.DoUrl(webUrl + this.resNames[i], testFinish, 5f);
        }

    }

    void testFinish(Exception e, string s)
    {
        Debug.Log("finish/" + s);
       // this.SaveText(savePath + this.resNames[0], s);
    }

    void Update()
    {
        mywwwqueue.Update();
    }



    public void SaveBytes(string fileName, byte[] buffer)
    {
        if (!Directory.Exists(this.GetDirectoryName(fileName)))
        {
            Directory.CreateDirectory(this.GetDirectoryName(fileName));
        }
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
        using (FileStream stream = new FileStream(fileName, FileMode.Create))
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(buffer);
                writer.Flush();
                writer.Close();
            }
            stream.Close();
        }
    }

    public void SaveText(string fileName, string text)
    {
        if (!Directory.Exists(this.GetDirectoryName(fileName)))
        {
            Directory.CreateDirectory(this.GetDirectoryName(fileName));
        }
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
        using (FileStream stream = new FileStream(fileName, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(text);
                writer.Flush();
                writer.Close();
            }
            stream.Close();
        }
    }

    public string GetDirectoryName(string fileName)
    {
        return fileName.Substring(0, fileName.LastIndexOf('/'));
    }
}
