using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CGameEventList
{
	List<dgProEvent> Lists;

	public CGameEventList()
	{
		this.Lists = new List<dgProEvent> ();
	}
	
	public void Dispatch(CGameEvent _CGameEvent)
	{
		for (int i = 0; i < this.Lists.Count; i++) 
		{
			this.Lists[i](_CGameEvent);
		}
	}
	
	public void Add(dgProEvent _ProEvent)
	{
		if (!this.Lists.Contains (_ProEvent))
		{
			this.Lists.Add(_ProEvent);
		}
	}
	
	public void Remove(dgProEvent _ProEvent)
	{
		if(this.Lists.Contains(_ProEvent))
			this.Lists.Remove(_ProEvent);
	}
}