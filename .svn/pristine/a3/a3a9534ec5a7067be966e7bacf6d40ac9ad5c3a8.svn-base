using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using LuaInterface;
using UnityEngine.UI;

public class EventTriggerListener : UnityEngine.EventSystems.EventTrigger
{
	public delegate void VoidDelegate (GameObject go);
	public VoidDelegate onClick;
	public VoidDelegate onDown;
	public VoidDelegate onEnter;
	public VoidDelegate onExit;
	public VoidDelegate onUp;
	public VoidDelegate onSelect;
	public VoidDelegate onUpdateSelect;

	static public EventTriggerListener Get (GameObject go)
	{
		EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
		if (listener == null) listener = go.AddComponent<EventTriggerListener>();
		return listener;
	}
	public override void OnPointerClick(PointerEventData eventData)
	{
		if(onClick != null) 	onClick(gameObject);
	}
	public override void OnPointerDown (PointerEventData eventData){
		if(onDown != null) onDown(gameObject);
	}
	public override void OnPointerEnter (PointerEventData eventData){
		if(onEnter != null) onEnter(gameObject);
	}
	public override void OnPointerExit (PointerEventData eventData){
		if(onExit != null) onExit(gameObject);
	}
	public override void OnPointerUp (PointerEventData eventData){
		if(onUp != null) onUp(gameObject);
	}
	public override void OnSelect (BaseEventData eventData){
		if(onSelect != null) onSelect(gameObject);
	}
	public override void OnUpdateSelected (BaseEventData eventData){
		if(onUpdateSelect != null) onUpdateSelect(gameObject);
	}

    //-----
    private  Dictionary<string, LuaFunction> buttons = new Dictionary<string, LuaFunction>();

    /// <summary>
    /// 添加单击事件
    /// </summary>
    public  void AddClick(GameObject go, LuaFunction luafunc)
    {
        if (go == null || luafunc == null) return;
        buttons.Add(go.name, luafunc);
        go.GetComponent<Button>().onClick.AddListener(
            delegate () {
                luafunc.Call(go);
            }
        );
    }

    /// <summary>
    /// 删除单击事件
    /// </summary>
    /// <param name="go"></param>
    public  void RemoveClick(GameObject go)
    {
        if (go == null) return;
        LuaFunction luafunc = null;
        if (buttons.TryGetValue(go.name, out luafunc))
        {
            luafunc.Dispose();
            luafunc = null;
            buttons.Remove(go.name);
        }
    }
}