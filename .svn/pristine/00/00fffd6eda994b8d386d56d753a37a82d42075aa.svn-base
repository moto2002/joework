using System;
using UnityEngine;

public class TimerManager : Singleton<TimerManager>
{
    private Action m_callback = null;
    private Action<uint> m_onTimer = null;
    private uint m_seconds = 0;
    private float timeActive = 0f;
    private bool timerIsActive = false;

    public float GetCurrentTime()
    {
        return this.timeActive;
    }

    public string GetCurrentTimeString()
    {
        return this.timeActive.ToString("00.00");
    }

    public uint GetSeconds()
    {
        return this.m_seconds;
    }

    public bool IsTimerRunning()
    {
        return this.timerIsActive;
    }

    public void OnDisable()
    {
        if (this.IsTimerRunning())
        {
            this.StopTimer();
        }
    }

    public void StartTimer()
    {
        this.timerIsActive = true;
        this.timeActive = 0f;
    }

    public void StartTimer(uint seconds, Action<uint> onTimer, Action callback)
    {
        this.m_seconds = seconds;
        this.m_onTimer = onTimer;
        this.m_callback = callback;
        if (this.m_seconds > 0)
        {
            this.StartTimer();
        }
        else
        {
            this.StopTimer();
            if (this.m_callback != null)
            {
                this.m_callback();
            }
        }
    }

    public void StopTimer()
    {
        this.timerIsActive = false;
        this.timeActive = 0f;
    }

    private void Update()
    {
        if (this.timerIsActive)
        {
            this.timeActive += Time.deltaTime * 1000f;
            if (this.timeActive > 1000f)
            {
                this.timeActive = 0f;
                this.m_seconds--;
                if (this.m_seconds > 0)
                {
                    if (this.m_onTimer != null)
                    {
                        this.m_onTimer(this.m_seconds);
                    }
                }
                else
                {
                    this.StopTimer();
                    if (this.m_callback != null)
                    {
                        this.m_callback();
                    }
                }
            }
        }
    }
}

