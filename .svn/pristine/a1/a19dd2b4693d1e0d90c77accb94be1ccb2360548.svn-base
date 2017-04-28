using UnityEngine;
using System.Collections;
using com.shihuanjue.game.Event;

public class TestEvent : MonoBehaviour
{
	void Start ()
	{
		EventDispatcher.AddEventListener (Events.TestEvent.TestJoe, this.OnTestEvent);
		//EventDispatcher.TriggerEvent (Events.TestEvent.TestJoe);

		EventDispatcher.AddEventListener (Events.TestEvent.TestJoe, this.OnTestEvent2);
	//	EventDispatcher.TriggerEvent (Events.TestEvent.TestJoe);

		EventDispatcher.RemoveEventListener (Events.TestEvent.TestJoe, this.OnTestEvent2);
	//	EventDispatcher.TriggerEvent (Events.TestEvent.TestJoe);

		EventDispatcher.RemoveEventListener (Events.TestEvent.TestJoe, this.OnTestEvent);
		EventDispatcher.TriggerEvent (Events.TestEvent.TestJoe);

		EventDispatcher.AddEventListener<int,bool> (Events.TestEvent.TestJoe, this.OnTestEvent3);
		EventDispatcher.TriggerEvent<int,bool> (Events.TestEvent.TestJoe,5,true);
	}

	void OnTestEvent()
	{
		Debug.Log ("测试1");
	}

	void OnTestEvent2()
	{
		Debug.Log ("测试2");
	}

	void OnTestEvent3(int a, bool b)
	{

		Debug.Log (a);
		Debug.Log (b);
	}
}
