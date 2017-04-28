using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace UnityEngine.UI
{
	[RequireComponent(typeof(RectTransform))]
	public class JoystickController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
	{
		public RectTransform handle;
		public float autoReturnSpeed = 30f;
		public float radius = 50.0f;
		public Color pressedColor = Color.white;
		public float pressedScale = 1.0f;
		
		public event Action<JoystickController, Vector2> OnStartJoystickMovement;
		public event Action<JoystickController, Vector2> OnJoystickMovement;
		public event Action<JoystickController> OnEndJoystickMovement;
		
		private bool _returnHandle;
		private RectTransform _canvas;
		private bool _isDragging;

		public bool IsDragging{
			get{ return _isDragging; }
		}
		
		public Vector2 Coordinates
		{
			get
			{
				if (handle.anchoredPosition.magnitude < radius)
					return handle.anchoredPosition / radius;
				return handle.anchoredPosition.normalized;
			}
		}
		public float MoveValue{
			get{
				return Coordinates.magnitude; 
			}
		}
		
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			_returnHandle = false;
			var handleOffset = GetJoystickOffset(eventData);
			handle.anchoredPosition = handleOffset;
			handle.GetComponent<Image>().color = pressedColor;
			handle.localScale = new Vector2(pressedScale, pressedScale);
			if (OnStartJoystickMovement != null)
				OnStartJoystickMovement(this, Coordinates);
		}
		
		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			var handleOffset = GetJoystickOffset(eventData);
			handle.anchoredPosition = handleOffset;
			_isDragging = true;
			if (OnJoystickMovement != null)
				OnJoystickMovement(this, Coordinates);
		}
		
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			handle.GetComponent<Image>().color = Color.white;
			handle.localScale = Vector2.one;
			_returnHandle = true;
			_isDragging = false;
			if (OnEndJoystickMovement != null)
				OnEndJoystickMovement(this);
		}
		
		private Vector2 GetJoystickOffset(PointerEventData eventData)
		{
			Vector3 globalHandle;
			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvas, eventData.position, eventData.pressEventCamera, out globalHandle))
				handle.position = globalHandle;
			var handleOffset = handle.anchoredPosition;
			if (handleOffset.magnitude > radius)
			{
				handleOffset = handleOffset.normalized * radius;
				handle.anchoredPosition = handleOffset;
			}
			return handleOffset;
		}
		
		private void Start()
		{
			_returnHandle = true;
			var touchZone = GetComponent<RectTransform>();
			touchZone.pivot = Vector2.one * 0.5f;
			handle.transform.SetParent(transform);
			var curTransform = transform;
			do
			{
				if (curTransform.GetComponent<Canvas>() != null)
				{
					_canvas = curTransform.GetComponent<RectTransform>();
					break;
				}
				curTransform = transform.parent;
			}
			while (curTransform != null);
		}
		
		private void Update()
		{
			if (_returnHandle)
				if (handle.anchoredPosition.magnitude > Mathf.Epsilon)
					handle.anchoredPosition -= new Vector2(handle.anchoredPosition.x * autoReturnSpeed, handle.anchoredPosition.y * autoReturnSpeed) * Time.deltaTime;
			else
				_returnHandle = false;
		}
	}
}