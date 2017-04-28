using Mogo.Util;
using System;
using UnityEngine;

public class DriverLib : MonoBehaviour
{
    private static DriverLib m_instance;

    private void Awake()
    {
        m_instance = this;
    }

    public static void Invoke(Action action)
    {
        TimerHeap.AddTimer(0, 0, action);
    }

    public static string FileName
    {
        get
        {
            return "MogoRes";
        }
    }

    public static DriverLib Instance
    {
        get
        {
            return m_instance;
        }
        set
        {
            m_instance = value;
        }
    }
}

