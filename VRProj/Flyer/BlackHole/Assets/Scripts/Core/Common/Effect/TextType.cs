using System;
using UnityEngine;
using UnityEngine.UI;

public class TextType: ITextTyper
{
    private Text text;
    private float time = 0;
    private string words;
    private bool isActive;
    private int charsPerSecond;
    private TimerManager.ITimer timer;

    private Action done;
    public Action Done
    {
        get
        {
            return done;
        }
        set
        {
            done = value;
        }
    }

    public TextType(Text text)
    {
        this.text = text;
        charsPerSecond = 0;
    }

    public ITextTyper Stop()
    {
        if (timer != null)
        {
            TimerManager.Remove(timer);
            timer = null;
        }
        return this as ITextTyper;
    }

    public ITextTyper Start(int countPerSecond)
    {
        time = 0;
        isActive = true;
        words = text.text;
        charsPerSecond = Mathf.Max(1, countPerSecond);

        if (timer != null)
        {
            TimerManager.Remove(timer);
        }

        timer = TimerManager.Add(-1, (e) =>
        {
            if (isActive)
            {
                try
                {
                    text.text = words.Substring(0, (int)(charsPerSecond * time));
                    time += Time.deltaTime;
                }
                catch (Exception)
                {
                    isActive = false;
                    time = 0;
                    text.text = words;
                    TimerManager.Remove(timer);
                    timer = null;
                    if (done != null)
                        done();
                }
            }
        }, -1);
        return this as ITextTyper;
    }
}