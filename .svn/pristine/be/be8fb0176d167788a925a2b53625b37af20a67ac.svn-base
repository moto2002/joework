using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/*
void OnClick(SpriteLayerInputManager.TouchEvent evt){
	print(evt.pointerOnObject.name);
}

void OnDown( SpriteLayerInputManager.TouchEvent evt){
	print(evt.pressObject.name);
	transform.DOScale(m_localScale*1.1f,0.1f);
}

void OnUp( SpriteLayerInputManager.TouchEvent evt){
	transform.DOScale(m_localScale,0.1f);
}
*/
/// Sprite touch事件的分发和管理，Sprite上面需要添加Collider2D组件，触发顺序根据z
/// messageReceiver或者SpriteLayerInputManager下面的Sprite会接收到的事件
/// </summary>
public class SpriteLayerInputManager : MonoBehaviour {

	public class TouchEvent{
		public Vector3 pressPosition; //world position
		public Vector3 position; //world position
		public Vector3 detal; //world position detal
		public GameObject pressObject = null ; //按下时的对象
		public GameObject pointerOnObject = null ; //当前鼠标下面的对象
	}

	private TouchEvent m_event ;
	private bool m_isTouchDown = false;
	private float m_touchTimeDelta = 0f;
	private Collider2D m_touchTarget = null ;

	//需不需要判断是否在UI上面
	public bool isCheckOnUGUI = true;
	//哪些层接受touch
	public LayerMask layerMask = -1;
	//计算使用的Camera , 默认为Camera.main
	public Camera raycastCamera;
	//消息的接收者，可以为null，为null则每个触发对象都会收到消息
	public GameObject messageReceiver = null ;
	//多少时间内算点击
	public float clickDelta=0.5f;

	void Start(){
		if(raycastCamera==null){
			raycastCamera = Camera.main;
		}
	}

	void Update()
	{
		bool flag = true;
		if(isCheckOnUGUI && InputUtil.CheckMouseOnUI()){
			flag = false; 
		}
		if( flag && Input.touchCount<2){
			if(Input.GetMouseButtonDown(0)){
				m_isTouchDown = true;
				OnTouchDown();
			}
			else if(m_isTouchDown && Input.GetMouseButtonUp(0)){
				m_isTouchDown = false;
				OnTouchUp();
			}

			if(m_isTouchDown && Input.GetMouseButton(0)){
				OnTouchMove();
			}
		}
	}

	void OnTouchDown(){
		m_touchTimeDelta = Time.realtimeSinceStartup;
		m_event = new TouchEvent();

		m_event.pressPosition = raycastCamera.ScreenToWorldPoint(Input.mousePosition);
		m_event.position = m_event.pressPosition;
		m_touchTarget = Physics2D.OverlapPoint(m_event.pressPosition,layerMask);

		if(m_touchTarget){
			m_event.pressObject = m_touchTarget.gameObject;
			SendTouchMessage("OnDown");
		}
	}

	void OnTouchUp(){
		m_event.position = raycastCamera.ScreenToWorldPoint( Input.mousePosition);
		m_touchTarget = Physics2D.OverlapPoint(m_event.pressPosition,layerMask);
		if(m_touchTarget!=null){
			m_event.pointerOnObject = m_touchTarget.gameObject;
			if(Time.realtimeSinceStartup-m_touchTimeDelta<clickDelta && m_event.pressObject== m_event.pointerOnObject){
				SendTouchMessage("OnClick");
			}
			SendTouchMessage("OnUp");
		}
		else
		{
			m_event.pointerOnObject = null;
			SendTouchMessage("OnUp");
		}
		m_event = null;
		m_touchTarget = null;
		m_touchTimeDelta = 0f;
	}

	void OnTouchMove(){
		Vector3 pos = raycastCamera.ScreenToWorldPoint(Input.mousePosition);
		m_event.detal = pos-m_event.position;
		m_event.position = pos;
		m_touchTarget = Physics2D.OverlapPoint(m_event.position,layerMask);
		if(m_touchTarget!=null){
			m_event.pointerOnObject = m_touchTarget.gameObject;
			SendTouchMessage("OnMove");
		}
		else
		{
			m_event.pointerOnObject = null;
			SendTouchMessage("OnMove");
		}
	}

	void SendTouchMessage(string method){
		if(messageReceiver==null){
			if(m_event.pointerOnObject)
			{
				m_event.pointerOnObject.SendMessage(method,m_event,SendMessageOptions.DontRequireReceiver);
			}
			if(m_event.pressObject && m_event.pointerOnObject!=m_event.pressObject){
				m_event.pressObject.SendMessage(method,m_event,SendMessageOptions.DontRequireReceiver);
			}
		}else{
			messageReceiver.SendMessage(method,m_event,SendMessageOptions.DontRequireReceiver);
		}
	}
}
