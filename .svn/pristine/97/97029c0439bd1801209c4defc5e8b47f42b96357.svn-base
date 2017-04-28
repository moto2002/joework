using UnityEngine;
using System.Collections;

public class TestEvent : MonoBehaviour 
{
	void Start () 
	{
		CGameEventMgr.Inst ().AddListener ("shihuanjue", OnTest1);
        CGameEventMgr.Inst().AddListener("shihuanjue", OnTest1);
        //CGameEventMgr.Inst ().AddListener ("hello",OnTest1);

        CGameEventMgr.Inst ().Dispatch (new CGameEvent("shihuanjue"));
        //CGameEventMgr.Inst ().Dispatch (new CGameEvent("hello"));
        ////1,2
        ////1
        //CGameEventMgr.Inst ().RemoveListener("shihuanjue",OnTest1);

        //CGameEventMgr.Inst ().Dispatch (new CGameEvent("shihuanjue"));
        //CGameEventMgr.Inst ().Dispatch (new CGameEvent("hello"));
        ////2
        ////1
        //CGameEventMgr.Inst ().RemoveListener ("shihuanjue",OnTest2);

        //CGameEventMgr.Inst ().Dispatch (new CGameEvent("shihuanjue"));
        //// null
	}
	
	void Update () 
	{
	
	}

	void OnTest1(CGameEvent _event)
	{
		Debug.Log ("1");

	}

	void OnTest2(CGameEvent _event)
	{
		Debug.Log ("2");
	}
}
