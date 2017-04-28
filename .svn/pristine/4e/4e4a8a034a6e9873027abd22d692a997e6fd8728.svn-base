using UnityEngine;
using System.Collections;
using System;
using ShiHuanJue.Event;

/// <summary>
/// 渲染帧由本地驱动，60帧/s, 30帧/s
/// 渲染定时器
/// </summary>
public class Timer
{
    float time;       //当前时间
    int currentCount; //当前次数
    bool isLimited;   //是不是有限次数,默认是无限次:_repeatCount = -1
    float timeStep;   //间隔
    int repeatCount;  //重复次数

    string eventName;
    Action callback;

    public Timer(float _timeStep = 0,int _repeatCount = -1)
    {
        this.time = 0;
        this.currentCount = 0;
        this.isLimited = false; 
        this.timeStep = _timeStep;
        this.repeatCount = _repeatCount;
    }

    public void Start()
    {
        //说明设置了次数
        if (this.repeatCount != -1 && this.repeatCount > 0)
        {
            this.isLimited = true;
        }
        //this.time = Time.realtimeSinceStartup;
        this.time = 0;
        EventDispatcher.AddEventListener(EventsConst.FrameWork.Update, this.OnEnterFrame);
    }

    private void OnEnterFrame()
    {
        //确保第一帧是立马执行的，而不是要等一个timestep
        if (this.time == 0 || Time.realtimeSinceStartup - this.time >= this.timeStep)
        {
            if (this.isLimited)
            {
                if (this.currentCount == this.repeatCount)
                    this.Stop();

                EventDispatcher.TriggerEvent(this.eventName);
                this.currentCount++;
                this.time = Time.realtimeSinceStartup;
            }
            else
            {
                EventDispatcher.TriggerEvent(this.eventName);
                this.time = Time.realtimeSinceStartup;
            }
        }
    }

    public void Stop()
    {
        EventDispatcher.RemoveEventListener(EventsConst.FrameWork.Update, this.OnEnterFrame);
        EventDispatcher.RemoveEventListener(this.eventName, this.callback);
    }

    /// <summary>
    ///每个定时器都有直接单独的生命周期，所以传进来的eventName必须是唯一的
    ///如果不是唯一的，当一个定时器完成了，触发事件时，会同时触发计时器没结束的，就bug了
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddListener(string name,Action action)
    {
        this.eventName = name;
        this.callback = action;
        EventDispatcher.AddEventListener(this.eventName, this.callback);
        this.Start();
    }

    /// <summary>
    /// 以时间来做事件名字，确保唯一性
    /// </summary>
    /// <param name="action"></param>
    public void AddListener(Action action)
    {
        this.eventName = System.DateTime.Now.ToString();
        this.callback = action;
        EventDispatcher.AddEventListener(this.eventName, this.callback);
        this.Start();
    }
}
