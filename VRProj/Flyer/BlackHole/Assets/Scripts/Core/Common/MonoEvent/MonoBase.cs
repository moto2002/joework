using System;
using UnityEngine;

public class MonoBase : MonoBehaviour
{
    public Action AwakeEvent;
    public Action StartEvent;
    public Action DestroyEvent;

    private void Awake()
    {
        if (AwakeEvent != null)
        {
            AwakeEvent();
        }
    }

    private void Start()
    {
        if (StartEvent != null)
        {
            StartEvent();
        }
    }

    protected virtual void OnDestroy()
    {
        AwakeEvent = null;
        StartEvent = null;
        if (DestroyEvent != null)
        {
            DestroyEvent();
        }
    }

    protected void DestroyWhenNullEvent()
    {
        if (this != null)
        {
            Destroy(this);
        }
    }
}
