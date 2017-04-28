using UnityEngine;
using ShiHuanJue.Debuger;

public class Test : MonoBehaviour
{
    void Start()
    {
        LogHelper.m_logLevel = 
        LogHelper.Log("debug");
        LogHelper.LogError("error");
        LogHelper.LogWarning("warning");

        GameObject go =  GameObject.Find("fsdfsd");
        GameObject.Instantiate(go);

    }
}
