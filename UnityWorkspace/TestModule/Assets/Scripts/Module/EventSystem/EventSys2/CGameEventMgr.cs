using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CGameEventMgr
{
	Dictionary<string, CGameEventList> nameLinkEvents;
	
	private static CGameEventMgr _inst;
	public static CGameEventMgr Inst()
	{
		if(_inst==null)
			_inst = new CGameEventMgr();
		return _inst;
	}

	CGameEventMgr()
	{
		this.nameLinkEvents = new Dictionary<string, CGameEventList>();
	}

	#region [ 监听 ]
	public void AddListener(string _EventName, dgProEvent _ProEvent)
	{
		CGameEventList eventList;
		this.nameLinkEvents.TryGetValue(_EventName, out  eventList);
		if(eventList == null)
		{
			eventList = new CGameEventList();
			this.nameLinkEvents.Add(_EventName, eventList);
		}
		eventList.Add(_ProEvent);
	}
	#endregion

	#region [ 派发 ]
	public void Dispatch(CGameEvent _CGameEvent)
	{
		CGameEventList eventList;
		this.nameLinkEvents.TryGetValue(_CGameEvent.eventName, out  eventList);
		if (eventList != null)
			eventList.Dispatch(_CGameEvent);
		else
		{
			//Debug.Log("Not find event pro func-" + _CGameEvent.eventName);
		}

	}
	#endregion

	#region [ 删除事件 ]
	public void RemoveListener(string EventName)
	{
		this.nameLinkEvents.Remove(EventName);
	}
	
	public void RemoveListener(string EventName, dgProEvent evt)
	{
		CGameEventList eventList;
		this.nameLinkEvents.TryGetValue(EventName, out  eventList);
		if (eventList != null)
		{
			eventList.Remove(evt);
		}
	}
	#endregion

	#region [ 清除所有监听 ]
	public void ClearProEvent()
	{
		this.nameLinkEvents.Clear();
	}
	#endregion

	
}
