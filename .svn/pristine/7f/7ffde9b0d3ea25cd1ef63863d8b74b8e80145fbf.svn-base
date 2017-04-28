using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Diagnostics;
using System;

public class CsharpInvokeBatch 
{
    [MenuItem("Tools/批处理")]
    public static void ExcuteBatch()
    {
        Process proc = null;
        try
        {
            string path = Application.dataPath + "/mybatch/";
            //UnityEngine.Debug.Log(path);
            proc = new Process();
            //proc.StartInfo.WorkingDirectory = targetDir;
            proc.StartInfo.FileName = path + "mybatch.bat";
            //proc.StartInfo.Arguments = string.Format("10");//this is argument
            proc.StartInfo.CreateNoWindow = false;
            proc.Start();
            proc.WaitForExit();
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.Log("Exception Occurred :{0},{1}"+ ex.Message+ ex.StackTrace.ToString());
        }
    }

}
