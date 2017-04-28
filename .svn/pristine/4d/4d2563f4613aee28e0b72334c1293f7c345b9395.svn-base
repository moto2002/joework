using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Use Get(GameObject inTarget) Method
/// </summary>
public class EventTriggerAssist : EventTrigger
{
    public event Action<PointerEventData> LeftDown;
    public event Action<PointerEventData> RightDown;
    public event Action<PointerEventData> MiddleDown;

    public event Action<PointerEventData> LeftUp;
    public event Action<PointerEventData> RightUp;
    public event Action<PointerEventData> MiddleUp;

    public event Action<PointerEventData> LeftClick;
    public event Action<PointerEventData> RightClick;
    public event Action<PointerEventData> MiddleClick;

    public event Action<PointerEventData> Enter;
    public event Action<PointerEventData> Exit;
    public event Action<PointerEventData> BeginDrag;
    public event Action<PointerEventData> Drag;
    public event Action<PointerEventData> EndDrag;
    public event Action<PointerEventData> Drop;

    public static EventTriggerAssist Get(GameObject inTarget)
    {
        if (!inTarget.GetComponent<EventTriggerAssist>())
            inTarget.AddComponent<EventTriggerAssist>();
        return inTarget.GetComponent<EventTriggerAssist>();
    }

    public static void SetEvent(GameObject gameObject, EventTriggerType eventTriggerType, Action<BaseEventData> action)
    {
        var trigger = gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = gameObject.AddComponent<EventTrigger>();
        if (trigger.triggers == null)
            trigger.triggers = new List<Entry>();

        Entry entry = new Entry();
        entry.eventID = eventTriggerType;
        entry.callback = new TriggerEvent();
        entry.callback.AddListener(new UnityAction<BaseEventData>(action));
        trigger.triggers.Add(entry);
    }

    /// <summary>
    /// Error: Method OnPointerDown has been deprecated. Use LeftDown or MiddleDown or RightDown event instead.
    /// </summary>
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (LeftDown != null)
            {
                LeftDown(eventData);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            if (MiddleDown != null)
            {
                MiddleDown(eventData);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (RightDown != null)
            {
                RightDown(eventData);
            }
        }
    }
    /// <summary>
    /// Error: Method OnPointerUp has been deprecated. Use LeftUp or MiddleUp or RightUp event instead.
    /// </summary>
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (LeftUp != null)
            {
                LeftUp(eventData);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            if (MiddleUp != null)
            {
                MiddleUp(eventData);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (RightUp != null)
            {
                RightUp(eventData);
            }
        }
    }
    /// <summary>
    /// Error: Method OnPointerClick has been deprecated. Use LeftUp or MiddleUp or RightUp event instead.
    /// </summary>
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (LeftClick != null)
            {
                LeftClick(eventData);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            if (MiddleClick != null)
            {
                MiddleClick(eventData);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (RightClick != null)
            {
                RightClick(eventData);
            }
        }
    }
    /// <summary>
    /// Error: Method OnPointerEnter has been deprecated. Use Enter event instead.
    /// </summary>
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (Enter != null)
        {
            Enter(eventData);
        }
    }
    /// <summary>
    /// Error: Method OnPointerExit has been deprecated. Use Exit event instead.
    /// </summary>
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (Exit != null)
        {
            Exit(eventData);
        }
    }
    /// <summary>
    /// Error: Method OnBeginDrag has been deprecated. Use BeginDrag event instead.
    /// </summary>
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (BeginDrag != null)
        {
            BeginDrag(eventData);
        }
    }
    /// <summary>
    /// Error: Method OnDrag has been deprecated. Use Drag event instead.
    /// </summary>
    public override void OnDrag(PointerEventData eventData)
    {
        if (Drag != null)
        {
            Drag(eventData);
        }
    }
    /// <summary>
    /// Error: Method OnEndDrag has been deprecated. Use EndDrag event instead.
    /// </summary>
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (EndDrag != null)
        {
            EndDrag(eventData);
        }
    }
    /// <summary>
    /// Error: Method OnDrop has been deprecated. Use Drop event instead.
    /// </summary>
    public override void OnDrop(PointerEventData eventData)
    {
        if (Drop != null)
        {
            Drop(eventData);
        }
    }

    /// <summary>
    /// 清除该物体上的所有事件
    /// </summary>
    public void Clear()
    {
        LeftDown = null;
        RightDown = null;
        MiddleDown = null;
        LeftUp = null;
        RightUp = null;
        MiddleUp = null;
        LeftClick = null;
        RightClick = null;
        MiddleClick = null;
        Enter = null;
        Exit = null;
        BeginDrag = null;
        Drag = null;
        EndDrag = null;
        Drop = null;
    }

    void OnDestroy()
    {
        Clear();
    }
}