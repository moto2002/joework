using UnityEngine;
using System.Collections;
using System;

public class EventTest : MonoBehaviour
{
    private void Awake()
    {
        EventRouter.instance.AddEventHandler("test1", OnTest1);
        EventRouter.instance.AddEventHandler<string, int>("test2", OnTest2);
    }

    void Start()
    {
        EventRouter.instance.BroadCastEvent("test1");
        EventRouter.instance.BroadCastEvent<string, int>("test2", "joe", 26);
    }

    private void OnTest1()
    {
        Debug.Log("test1");
    }

    private void OnTest2(string arg1, int arg2)
    {
        Debug.Log("test2"+arg1+arg2);
    }

    private void OnDestroy()
    {
        EventRouter.instance.RemoveEventHandler("test1",OnTest1);
        EventRouter.instance.RemoveEventHandler<string, int>("test2", OnTest2);
    }
}
