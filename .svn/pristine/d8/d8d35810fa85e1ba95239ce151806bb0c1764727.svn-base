using System;
using UnityEngine;
using System.Collections.Generic;

public class TimerManager
{
    public interface ITimer : IDisposable
    {
        /// <summary>
        /// 当前回调序号
        /// </summary>
        uint Index { get; }
        /// <summary>
        /// 剩余次数
        /// <para>值为-1时，无限计时</para>
        /// </summary>
        int Count { get; }
        /// <summary>
        /// 时间到
        /// </summary>
        bool Timeup();
        /// <summary>
        /// 回调
        /// </summary>
        Callback<int> CallBack { get; }
        /// <summary>
        /// 重置计时器
        /// </summary>
        /// <param name="inSpan">间隔时间，值为-1时为每帧回调</param>
        /// <param name="inCallBack">回调</param>
        /// <param name="inCount">次数</param>
        void Reset(float inSpan, Callback<int> inCallBack, int inCount);
    }

    private static GameObject handler;
    /// <summary>
    /// 增加计时器
    /// </summary>
    /// <param name="span">时间间隔, 值为 -1 时每帧回调</param>
    /// <param name="action">每次的回调，参数为当前计数次数,第一次: 1，最后一次: Count</param>
    /// <param name="count">执行次数,值为 -1 时无限循环</param>
    /// <returns>计时器接口</returns>
    public static ITimer Add(float span, Callback<int> action, int count = 1)
    {
        if (handler == null)
        {
            handler = new GameObject("TimeHandler");
            handler.AddComponent<TimeHandler>().DestroyEvent = () => { handler = null; };
        }
        else if (!handler.GetComponent<TimeHandler>())
        {
            handler.AddComponent<TimeHandler>().DestroyEvent = () => { handler = null; };
        }
        return handler.GetComponent<TimeHandler>().Add(new Timer(span, action, count));
    }

    /// <summary>
    /// 移除计时器
    /// </summary>
    /// <param name="timer">计时器接口</param>
    public static void Remove(ITimer timer)
    {
        if (timer == null)
            return;
        if (handler != null && handler.GetComponent<TimeHandler>())
        {
            handler.GetComponent<TimeHandler>().Remove(timer);
        }
    }

    /// <summary>
    /// 清空所有计时器
    /// </summary>
    public static void Clear()
    {
        if (handler.GetComponent<TimeHandler>())
        {
            handler.GetComponent<TimeHandler>().Clear();
        }
    }

    private class Timer : ITimer
    {
        private float span;
        private float bornTime;

        /// <summary>
        /// 当前次数
        /// </summary>
        public uint Index { get; private set; }
        /// <summary>
        /// 剩余次数
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// 回调委托
        /// </summary>
        public Callback<int> CallBack { get; set; }
        /// <summary>
        /// 是否已经被释放
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="inSpan">间隔时间，值为-1时为每帧回调</param>
        /// <param name="inCallBack">回调</param>
        /// <param name="inCount">次数</param>
        public Timer(float inSpan, Callback<int> inCallBack, int inCount = 1)
        {
            Disposed = false;
            Index = 0;
            Count = inCount > -2 ? inCount : 0;
            span = inSpan;
            CallBack = inCallBack;
            bornTime = Time.realtimeSinceStartup;
        }

        /// <summary>
        /// 重置计时器
        /// </summary>
        /// <param name="inSpan">间隔时间，值为-1时为每帧回调</param>
        /// <param name="inCallBack">回调</param>
        /// <param name="inCount">次数</param>
        public void Reset(float inSpan, Callback<int> inCallBack, int inCount)
        {
            Disposed = false;
            Index = 0;
            span = inSpan == -1 ? span : inSpan;
            Count = inCount >= -1 ? inCount : 0;
            CallBack = inCallBack;
            bornTime = Time.realtimeSinceStartup;
        }

        public bool Timeup()
        {
            if (Count != 0)
            {
                if (Time.realtimeSinceStartup - bornTime >= span)
                {
                    Index++;
                    if (Count != 0 && Count != -1)
                    {
                        Count--;
                    }
                    bornTime = Time.realtimeSinceStartup;
                    return true;
                }
            }
            return false;
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                Disposed = true;
                CallBack = null;
            }
        }
    }

    private class TimeHandler : MonoBehaviour
    {
        private int count;
        private int removeCount;
        internal Action DestroyEvent;
        private List<ITimer> timerList = new List<ITimer>();
        private List<ITimer> removerList = new List<ITimer>();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public ITimer Add(ITimer timer)
        {
            timerList.Add(timer);
            return timer;
        }

        public void Remove(ITimer timer)
        {
            if (timer != null && !removerList.Contains(timer))
            {
                removerList.Add(timer);
            }
        }

        public void Clear()
        {
            count = timerList.Count;
            for (int i = 0; i < count; i++)
            {
                timerList[i].Dispose();
            }
            timerList.Clear();
            removerList.Clear();
        }

        private void LateUpdate()
        {
            if (removerList.Count > 0)
            {
                removeCount = removerList.Count;
                for (int i = 0; i < removeCount; i++)
                {
                    if (timerList.Contains(removerList[i]))
                    {
                        removerList[i].Dispose();
                        timerList.Remove(removerList[i]);
                    }
                }
                removerList.Clear();
            }
            count = timerList.Count;
            if (count < 1) return;
            for (int i = 0; i < count; i++)
            {
                if ((timerList[i]).Timeup())
                {
                    if ((timerList[i]).CallBack != null)
                    {
                        (timerList[i]).CallBack((int)timerList[i].Index);
                    }
                    else if (timerList[i] == null ||
                            (timerList[i]).CallBack == null ||
                            (timerList[i]).Count == 0)
                    {
                        removerList.Add(timerList[i]);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (DestroyEvent != null)
            {
                DestroyEvent();
            }
        }
    }
}

