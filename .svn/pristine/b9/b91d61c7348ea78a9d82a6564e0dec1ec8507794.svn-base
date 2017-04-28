using System;
using System.Collections.Generic;
using UnityEngine;

public static class Messager
{
    private static Dictionary<string, Delegate> eventDic = new Dictionary<string, Delegate>();

    #region AddListener

    private static void OnListenerAdding(string eventType, Delegate inDelegate)
    {
        if (!eventDic.ContainsKey(eventType))
        {
            eventDic.Add(eventType, null);
        }
        Delegate d = eventDic[eventType];
        if (d != null && d.GetType() != inDelegate.GetType())
        {
            Debug.LogError(eventType + " 消息参数" + d.GetType().Name + "类型不一致, 应该为： " + inDelegate.GetType().Name + "\n");
        }
    }

    public static void AddListener(string eventType, Callback handler)
    {
        OnListenerAdding(eventType, handler);
        eventDic[eventType] = (Callback)eventDic[eventType] + handler;
    }
    public static void AddListener<T>(string eventType, Callback<T> handler)
    {
        OnListenerAdding(eventType, handler);
        eventDic[eventType] = (Callback<T>)eventDic[eventType] + handler;
    }
    public static void AddListener<T, U>(string eventType, Callback<T, U> handler)
    {
        OnListenerAdding(eventType, handler);
        eventDic[eventType] = (Callback<T, U>)eventDic[eventType] + handler;
    }
    public static void AddListener<T, U, V>(string eventType, Callback<T, U, V> handler)
    {
        OnListenerAdding(eventType, handler);
        eventDic[eventType] = (Callback<T, U, V>)eventDic[eventType] + handler;
    }
    public static void AddListener<T, U, V, W>(string eventType, Callback<T, U, V, W> handler)
    {
        OnListenerAdding(eventType, handler);
        eventDic[eventType] = (Callback<T, U, V, W>)eventDic[eventType] + handler;
    }

    #endregion

    #region RemoveListener

    private static void OnListenerRemoving(string inName, Delegate inDelegate)
    {
        if (eventDic.ContainsKey(inName))
        {
            Delegate d = eventDic[inName];

            if (d == null)
            {
                Debug.LogError(inName + " 消息没有回调方法! \n");
            }
            else if (d.GetType() != inDelegate.GetType())
            {
                Debug.LogError(inName + " 消息回调参数类型不一致，应该为：" + inDelegate.GetType().Name + "\n");
            }
        }
        else
        {
            Debug.LogError(inName + " 消息不存在! \n");
        }
    }

    public static void RemoveListener(string inName, Callback inHandler)
    {
        OnListenerRemoving(inName, inHandler);
        eventDic[inName] = (Callback)eventDic[inName] - inHandler;
        OnListenerRemoved(inName);
    }
    public static void RemoveListener<T>(string inName, Callback<T> inHandler)
    {
        OnListenerRemoving(inName, inHandler);
        eventDic[inName] = (Callback<T>)eventDic[inName] - inHandler;
        OnListenerRemoved(inName);
    }
    public static void RemoveListener<T, U>(string inName, Callback<T, U> inHandler)
    {
        OnListenerRemoving(inName, inHandler);
        eventDic[inName] = (Callback<T, U>)eventDic[inName] - inHandler;
        OnListenerRemoved(inName);
    }
    public static void RemoveListener<T, U, V>(string inName, Callback<T, U, V> inHandler)
    {
        OnListenerRemoving(inName, inHandler);
        eventDic[inName] = (Callback<T, U, V>)eventDic[inName] - inHandler;
        OnListenerRemoved(inName);
    }
    public static void RemoveListener<T, U, V, W>(string inName, Callback<T, U, V, W> inHandler)
    {
        OnListenerRemoving(inName, inHandler);
        eventDic[inName] = (Callback<T, U, V, W>)eventDic[inName] - inHandler;
        OnListenerRemoved(inName);
    }

    static private void OnListenerRemoved(string inName)
    {
        if (eventDic[inName] == null)
        {
            eventDic.Remove(inName);
        }
    }

    #endregion

    #region Broadcast

    static private bool OnBroadcasting(string inName)
    {
        bool hasListener = true;
        if (!eventDic.ContainsKey(inName))
        {
            hasListener = false; // 消息没有监听者
        }
        return hasListener;
    }

    public static void Broadcast(string inName)
    {
        if (!OnBroadcasting(inName)) return;
        Delegate d;
        if (eventDic.TryGetValue(inName, out d))
        {
            Callback callback = d as Callback;
            if (callback != null)
            {
                callback();
            }
            else
            {
                Debug.LogError(new Exception("Message LogError: " + inName + " 消息的回调方法为空!\n").ToString());
            }
        }
    }
    public static void Broadcast<T>(string inName, T arg1)
    {
        if (!OnBroadcasting(inName)) return;
        Delegate d;
        if (eventDic.TryGetValue(inName, out d))
        {
            Callback<T> callback = d as Callback<T>;
            if (callback != null)
            {
                callback(arg1);
            }
            else
            {
                Debug.LogError(new Exception("Message LogError: " + inName + " 消息的回调方法为空!\n").ToString());
            }
        }
    }
    public static void Broadcast<T, U>(string inName, T arg1, U arg2)
    {
        if (!OnBroadcasting(inName)) return;
        Delegate d;
        if (eventDic.TryGetValue(inName, out d))
        {
            Callback<T, U> callback = d as Callback<T, U>;
            if (callback != null)
            {
                callback(arg1, arg2);
            }
            else
            {
                Debug.LogError(new Exception("Message LogError: " + inName + " 消息的回调方法为空!\n").ToString());
            }
        }
    }
    public static void Broadcast<T, U, V>(string inName, T arg1, U arg2, V arg3)
    {
        if (!OnBroadcasting(inName)) return;
        Delegate d;
        if (eventDic.TryGetValue(inName, out d))
        {
            Callback<T, U, V> callback = d as Callback<T, U, V>;
            if (callback != null)
            {
                callback(arg1, arg2, arg3);
            }
            else
            {
                Debug.LogError(new Exception("Message LogError: " + inName + " 消息的回调方法为空!\n").ToString());
            }
        }
    }
    public static void Broadcast<T, U, V, W>(string inName, T arg1, U arg2, V arg3, W arg4)
    {
        if (!OnBroadcasting(inName)) return;
        Delegate d;
        if (eventDic.TryGetValue(inName, out d))
        {
            Callback<T, U, V, W> callback = d as Callback<T, U, V, W>;
            if (callback != null)
            {
                callback(arg1, arg2, arg3, arg4);
            }
            else
            {
                Debug.LogError(new Exception("Message LogError: " + inName + " 消息的回调方法为空!\n").ToString());
            }
        }
    }

    #endregion
}

