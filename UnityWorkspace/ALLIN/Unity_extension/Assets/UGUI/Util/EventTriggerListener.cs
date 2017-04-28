using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class EventTriggerListener : MonoBehaviour,IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler,
IPointerExitHandler, IPointerUpHandler
{
	public delegate void VoidDelegate1(GameObject go, PointerEventData eventData);
	public event VoidDelegate1 OnClick;
    public event VoidDelegate1 OnDown;
	public event VoidDelegate1 OnEnter;
	public event VoidDelegate1 OnExit;
	public event VoidDelegate1 OnUp;

    static public EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null) listener = go.AddComponent<EventTriggerListener>();
        return listener;
    }
	public void OnPointerClick (PointerEventData eventData)
	{
		if (OnClick != null) OnClick(gameObject, eventData);
	}
    public void OnPointerDown(PointerEventData eventData)
    {
        if (OnDown != null) OnDown(gameObject, eventData);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (OnEnter != null) OnEnter(gameObject, eventData);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnExit != null) OnExit(gameObject, eventData);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (OnUp != null) OnUp(gameObject, eventData);
    }
  
}