using UnityEngine;
using System.Collections;
using System;

public class TimerManager : Singleton<TimerManager>
{
    public void StartTimer(uint seconds, Action<uint> onTimer, Action callback)
    {
        GameObject go = new GameObject();
        go.name = "timer" + MyTools.GetTime().ToString();
        TimerHandler hander = go.AddComponent<TimerHandler>();
        hander.StartTimer(seconds, onTimer, ()=> {
            callback();
            GameObject.Destroy(go);
        });
    }
}
