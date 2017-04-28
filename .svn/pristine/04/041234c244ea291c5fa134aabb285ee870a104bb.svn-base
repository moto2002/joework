using UnityEngine;
using System.Collections;
using System;
using ShiHuanJue.Event;

/// <summary>
/// 定时器
/// 渲染帧有本地驱动，eg：30帧/s
/// </summary>
public class Timer
{
    private int m_repeatCount;  //重复次数
    private float m_interval;   //间隔
    private Action m_callback;  //回调
    private bool m_isLimited;   //是不是有限次数
    private int m_currentIndex; //当前是第几次
    private float m_currTime;    //当前时间
    private string m_eventName="";

    /// <summary>
    /// 构造定时器
    /// </summary>
    /// <param name="repeatCount">重复次数，-1为无限次</param>
    /// <param name="interval">间隔</param>
    /// <param name="callback">定时器结束回调</param>
    public Timer(int repeatCount = -1, float interval = 0 ,Action callback = null)
    {
        this.m_repeatCount = repeatCount;
        this.m_interval = interval;
        this.m_callback = callback;
        this.m_isLimited = this.m_repeatCount == -1 ? false : true;
        this.m_currTime = 0;
        this.m_currentIndex = 0;
        this.m_eventName = MyTools.GetTime().ToString();// DateTime.Now.ToString();
        EventDispatcher.AddEventListener(this.m_eventName, this.m_callback);
        EventDispatcher.AddEventListener(EventsConst.FrameWork.Update, this.OnEnterFrame);
    }

    private void OnEnterFrame()
    {
        if (m_isLimited)//有限次
        {
            if (Time.realtimeSinceStartup - this.m_currTime >= this.m_interval)
            {
                if (this.m_currentIndex == this.m_repeatCount)
                    this.Stop();

                EventDispatcher.TriggerEvent(this.m_eventName);
                this.m_currentIndex++;
                this.m_currTime = Time.realtimeSinceStartup;
            }
        }
        else//无限次
        {
            //确保第一帧是立马执行的，而不是要等一个间隔
            if (this.m_currTime == 0 || Time.realtimeSinceStartup - this.m_currTime >= this.m_interval)
            {
                EventDispatcher.TriggerEvent(this.m_eventName);
                this.m_currTime = Time.realtimeSinceStartup;
            }
        }
    }

    public void Stop()
    {
        EventDispatcher.RemoveEventListener(EventsConst.FrameWork.Update, this.OnEnterFrame);
        EventDispatcher.RemoveEventListener(this.m_eventName, this.m_callback);

        this.m_repeatCount = 0;
        this.m_interval = 0;
        this.m_callback = null;
        this.m_isLimited = false;
        this.m_currTime = 0;
        this.m_currentIndex = 0;
        this.m_eventName = "";
    }
}
