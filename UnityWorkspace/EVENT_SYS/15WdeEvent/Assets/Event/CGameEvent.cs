using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void dgProEvent(CGameEvent _CGameEvent);

public class CGameEvent
{
	public string eventName = "";
	public object sender = null;
	public object data = null;
		
	public CGameEvent(string _eventName)
	{
		this.eventName = _eventName;
	}
}

public class CGameEvent_ShowDebugPanel:CGameEvent
{
	public CGameEvent_ShowDebugPanel(string _EventName):base(_EventName){}
	public int ShowType;
}
public class CGameEvent_GameOver:CGameEvent
{
	public CGameEvent_GameOver(string _EventName):base(_EventName){ Tips=""; }
	public string Tips="";
	public bool IsHero;
	public bool IsChouJiang=false;
	public bool LoadingWaitTimeOut = false;
}
public class CGameEvent_GetSceneNum:CGameEvent
{
	public CGameEvent_GetSceneNum(string _EventName):base(_EventName){}
	public int MaxSceneNum;
}
public class CGameEvent_NewWaitingPlayer:CGameEvent
{
	public CGameEvent_NewWaitingPlayer(string _EventName):base(_EventName){}
	public uint PlayerId;
}
public class CGameEvent_LoginAck:CGameEvent
{
	public CGameEvent_LoginAck(string _EventName):base(_EventName){}
	public byte ErrorCode;
}
public class CGameEvent_SearchPlayerRlt:CGameEvent
{
	public CGameEvent_SearchPlayerRlt(string _EventName):base(_EventName){}
	public bool Sussed=false;
}
public class CGameEvent_msgAckRegisterAccount:CGameEvent
{
	public CGameEvent_msgAckRegisterAccount(string _EventName):base(_EventName){}
}
public class CGameEvent_MessageBox : CGameEvent
{
	public CGameEvent_MessageBox(string _EventName) : base(_EventName) { }
	public string Tips;
	public bool ShowCancelButton = true;
}
