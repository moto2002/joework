using System;
using UnityEngine;

public class TimerManager
{
    public static TimerBehaviour GetTimer(GameObject target)
    {
        TimerBehaviour component = target.GetComponent<TimerBehaviour>();
        if (component == null)
        {
            component = target.AddComponent<TimerBehaviour>();
        }
        return component;
    }
}

