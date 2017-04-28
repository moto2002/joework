using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MessengerBehaviour : MonoBehaviour {

	private Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

	#region AddListener
	private void OnListenerAdding(string eventType, Delegate listenerBeingAdded) {

		if (!eventTable.ContainsKey(eventType)) {
			eventTable.Add(eventType, null );
		}
		
		Delegate d = eventTable[eventType];
		if (d != null && d.GetType() != listenerBeingAdded.GetType()) {
			throw new Exception(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
		}
	}
	//No parameters
	public void AddListener(string eventType, Action handler) {
		OnListenerAdding(eventType, handler);
		eventTable[eventType] = (Action)eventTable[eventType] + handler;
	}
	
	//Single parameter
	public void AddListener<T>(string eventType, Action<T> handler) {
		OnListenerAdding(eventType, handler);
		eventTable[eventType] = (Action<T>)eventTable[eventType] + handler;
	}
	
	//Two parameters
	public void AddListener<T, U>(string eventType, Action<T, U> handler) {
		OnListenerAdding(eventType, handler);
		eventTable[eventType] = (Action<T, U>)eventTable[eventType] + handler;
	}
	
	//Three parameters
	public void AddListener<T, U, V>(string eventType, Action<T, U, V> handler) {
		OnListenerAdding(eventType, handler);
		eventTable[eventType] = (Action<T, U, V>)eventTable[eventType] + handler;
	}
	#endregion



	#region RemoveListener
	private void OnListenerRemoving(string eventType, Delegate listenerBeingRemoved) {
		if (eventTable.ContainsKey(eventType)) {
			Delegate d = eventTable[eventType];
			
			if (d == null) {
				throw new Exception(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", eventType));
			} else if (d.GetType() != listenerBeingRemoved.GetType()) {
				throw new Exception(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
			}
		}
	}
	private void OnListenerRemoved(string eventType) {
		if (eventTable[eventType] == null) {
			eventTable.Remove(eventType);
		}
	}

	//No parameters
	public void RemoveListener(string eventType, Action handler) {
		OnListenerRemoving(eventType, handler);
		if (eventTable.ContainsKey (eventType)) {
			eventTable [eventType] = (Action)eventTable [eventType] - handler;
			OnListenerRemoved (eventType);
		}
	}
	
	//Single parameter
	public void RemoveListener<T>(string eventType, Action<T> handler) {
		OnListenerRemoving(eventType, handler);
		if (eventTable.ContainsKey (eventType)) {
			eventTable [eventType] = (Action<T>)eventTable [eventType] - handler;
			OnListenerRemoved (eventType);
		}
	}
	
	//Two parameters
	public void RemoveListener<T, U>(string eventType, Action<T, U> handler) {
		OnListenerRemoving(eventType, handler);
		if (eventTable.ContainsKey (eventType)) {
			eventTable [eventType] = (Action<T, U>)eventTable [eventType] - handler;
			OnListenerRemoved (eventType);
		}
	}
	
	//Three parameters
	public void RemoveListener<T, U, V>(string eventType, Action<T, U, V> handler) {
		OnListenerRemoving(eventType, handler);
		if (eventTable.ContainsKey (eventType)) {
			eventTable [eventType] = (Action<T, U, V>)eventTable [eventType] - handler;
			OnListenerRemoved (eventType);
		}
	}

	public void RemoveAllListeners()
	{
		eventTable = null;
		eventTable = new Dictionary<string, Delegate>();
	}
	#endregion








	#region Broadcast
	//No parameters
	public void DispatchEvent(string eventType) {
		Delegate d;
		if (eventTable.TryGetValue(eventType, out d)) {
			Action Action = d as Action;
			
			if (Action != null) {
				Action();
			} else {
				throw new Exception(eventType);
			}
		}
	}
	
	//Single parameter
	public void DispatchEvent<T>(string eventType, T arg1) {
		Delegate d;
		if (eventTable.TryGetValue(eventType, out d)) {
			Action<T> Action = d as Action<T>;
			
			if (Action != null) {
				Action(arg1);
			} else {
				throw new Exception(eventType);
			}
		}
	}
	
	//Two parameters
	public void DispatchEvent<T, U>(string eventType, T arg1, U arg2) {
		Delegate d;
		if (eventTable.TryGetValue(eventType, out d)) {
			Action<T, U> Action = d as Action<T, U>;
			
			if (Action != null) {
				Action(arg1, arg2);
			} else {
				throw new Exception(eventType);
			}
		}
	}
	
	//Three parameters
	public void DispatchEvent<T, U, V>(string eventType, T arg1, U arg2, V arg3) {
		Delegate d;
		if (eventTable.TryGetValue(eventType, out d)) {
			Action<T, U, V> Action = d as Action<T, U, V>;
			
			if (Action != null) {
				Action(arg1, arg2, arg3);
			} else {
				throw new Exception(eventType);
			}
		}
	}
	#endregion



	public bool HasEventListener( string eventType){
		return eventTable.ContainsKey (eventType);
	}
	void OnDestroy()
	{
		RemoveAllListeners ();
	}
}
