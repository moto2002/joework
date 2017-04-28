using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.tencent.pandora
{
	public class EventTriggerListener : EventTrigger
	{
		public delegate void VoidDelegate(GameObject go);

		public EventTriggerListener.VoidDelegate onClick;

		public EventTriggerListener.VoidDelegate onDown;

		public EventTriggerListener.VoidDelegate onUp;

		public static EventTriggerListener Get(GameObject go)
		{
			EventTriggerListener eventTriggerListener = go.GetComponent<EventTriggerListener>();
			if (eventTriggerListener == null)
			{
				eventTriggerListener = go.AddComponent<EventTriggerListener>();
			}
			return eventTriggerListener;
		}

		public static EventTriggerListener Get(Transform transform)
		{
			EventTriggerListener eventTriggerListener = transform.GetComponent<EventTriggerListener>();
			if (eventTriggerListener == null)
			{
				eventTriggerListener = transform.gameObject.AddComponent<EventTriggerListener>();
			}
			return eventTriggerListener;
		}

		public override void OnPointerClick(PointerEventData eventData)
		{
			if (this.onClick != null)
			{
				this.onClick(base.gameObject);
			}
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			if (this.onDown != null)
			{
				this.onDown(base.gameObject);
			}
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
			if (this.onUp != null)
			{
				this.onUp(base.gameObject);
			}
		}
	}
}
