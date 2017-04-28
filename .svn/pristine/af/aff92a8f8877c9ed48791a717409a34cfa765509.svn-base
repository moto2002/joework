using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

/// <summary>
/// 用于拖动UGUI控件
/// </summary>
public class UGUIDrag: MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler{
	
	public enum DragBackEffect{
		None,Immediately, TweenPosition, TweenScale , ScaleDestroy , FadeOutDestroy , Destroy
	}

	private Vector3 m_cachePosition;
	private Vector3 m_cacheScale;
	private Vector3 m_cacheRotation;

	private Vector3 m_defaultPosition; //局部坐标
	private Vector3 m_defaultScale;
	private Vector3 m_defaultRotation;

	private Vector3 m_worldPos;
	private Vector3 m_touchDownTargetOffset ;
	private Transform m_parent;
	private bool m_isDown = false;
	private bool m_isDragging = false;
	public bool isDragging{
		get { return m_isDragging && m_isDown; }
	}
	private bool m_canDrag = true;
	public bool canDrag{
		get { return m_canDrag; }
		set { 
			m_canDrag = value; 
			if(!value) {
				m_isDragging = false;
				m_isDown = false;
			}

		}
	}
	public Vector3 dragTargetWorldPos{
		get { return m_worldPos; }
		set { m_worldPos = value; }
	}

	[Tooltip("拖动的对象，默认为自己.")]
	public RectTransform dragTarget;

	[Tooltip("如果为null，则使用mainCamera.")]
	public Camera rayCastCamera = null;

	[Tooltip("射线检测的Layer")]
	public LayerMask rayCastMask;

	[Header("Drag Setting")]
	[Tooltip("在拖动时是否固定在拖动物的原点.")]
	public bool isDragOriginPoint = false;

	[Tooltip("当isDragOriginPoint为true时，拖动时的偏移值.单位像素")]
	public Vector2 dragOffset;

	[Tooltip("主要用于影响层级显示.单位米")]
	public float dragOffsetZ=0f;

	[Tooltip("Drag时的变化的大小.")]
	public float dragChangeScale = 1f;

	[Tooltip("Drag时角度的变化值")]
	public float dragChangeRotate = 0f;

	[Tooltip("拖动时的缓动参数.")]
	[Range(0f,1f)]
	public float dragMoveDamp = 1f;

	[Tooltip("拖动时的所在的父窗器，用于拖动时在UI最上层，如果不填，则在当前层.")]
	public string dragingParent = "Canvas";

	[Tooltip("触发坐标，默认为当前对象")]
	public Transform triggerPos ;

	//要发送的事件名字
	[Header("Event")]
	public bool sendHoverEvent = false;
	public string onHoverMethodName = "OnHover";
	public string onHoverOutMethodName = "OnHoverOut";
	public string onDropMethodName = "OnDrop";

	[Header("Back Effect")]
	[Tooltip("释放时，是否自动返回")]
	public bool releaseAutoBack = false;
	[Tooltip("返回时的效果")]
	public DragBackEffect backEffect = DragBackEffect.None;
	[Tooltip("效果时间")]
	public float backDuring = 0.25f;
	[Tooltip("Tween 的效果")]
	public Ease tweenEase = Ease.Linear;

	public event Action<UGUIDrag,PointerEventData> OnPrevBeginDragAction = null ;
	public event Action<UGUIDrag,PointerEventData> OnBeginDragAction = null ;
	public event Action<UGUIDrag,PointerEventData> OnDragAction = null ;
	public event Action<UGUIDrag,PointerEventData> OnEndDragAction = null ;
	public event Action<UGUIDrag> OnTweenOverAction = null ;
	public delegate bool DragValidCheck(PointerEventData eventData);
	public event DragValidCheck DragValidCheckEvent;

	void OnDisable(){
		m_isDown =false;
		m_isDragging = false;
	}

	// Use this for initialization
	void Start () {
		if(!dragTarget){
			dragTarget =  transform as RectTransform;;
		}

		m_defaultScale = dragTarget.localScale;
		m_defaultRotation = dragTarget.localEulerAngles;
		m_defaultPosition = dragTarget.localPosition;

		if(!triggerPos){
			triggerPos = dragTarget;
		}
		if (!rayCastCamera)
		{
			rayCastCamera = Camera.main;
		}
	}

	public void SetDefaultPosition(){
		if(dragTarget) dragTarget.localPosition = m_defaultPosition;
	}
	public void SetDefaultRotation(){
		if(dragTarget) dragTarget.localEulerAngles = m_defaultScale;
	}
	public void SetDefaultScale(){
		if(dragTarget) dragTarget.localScale = m_defaultScale;
	}


	public void OnBeginDrag (PointerEventData eventData)
	{
		if(!this.enabled || m_isDown) return;

		if(DragValidCheckEvent!=null) {
			if(!DragValidCheckEvent(eventData)){
				m_canDrag = false;
				return;
			}
		}
		if(OnPrevBeginDragAction!=null){
			OnPrevBeginDragAction(this,eventData);
		}

		m_isDown = true;
		m_canDrag = true;
		dragTarget.DOKill();

		this.m_isDragging = true;
		this.GetComponent<Graphic>().raycastTarget = false;
		m_cachePosition = dragTarget.localPosition;
		m_cacheScale = dragTarget.localScale;
		m_cacheRotation = dragTarget.localEulerAngles;
		if(dragChangeScale!=0f){
			dragTarget.DOScale(m_cacheScale*dragChangeScale,0.4f);
		}
		if(dragChangeRotate!=0f){
			dragTarget.DOLocalRotate(m_cacheRotation +new Vector3(0f,0f,dragChangeRotate),0.4f,RotateMode.Fast);
		}

		dragTarget.position += new Vector3(0,0,dragOffsetZ);
		m_worldPos = dragTarget.position;
		Vector3 touchDownMousePos;
		RectTransformUtility.ScreenPointToWorldPointInRectangle(dragTarget,eventData.position,rayCastCamera,out touchDownMousePos);
		m_touchDownTargetOffset = m_worldPos-touchDownMousePos;
		if(!isDragOriginPoint){
			m_worldPos+=m_touchDownTargetOffset;
		}
		m_worldPos += (Vector3)dragOffset*0.01f;

		m_parent = dragTarget.parent;
		if(!string.IsNullOrEmpty(dragingParent)){
			GameObject go = GameObject.Find(dragingParent);
			if(go){
				dragTarget.SetParent(go.transform);
			}
		}

		if(OnBeginDragAction!=null){
			OnBeginDragAction(this,eventData);
		}
	}

	void OnApplicationFocus(bool flag){
		if(!flag && m_canDrag && m_isDragging){
			OnEndDrag(null);
		}
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		if(!this.enabled || !this.m_canDrag || !m_isDown) return;
		m_isDragging = false;
		m_isDown = false;

		DOTween.Kill(dragTarget);
		if(dragChangeScale!=0f){
			dragTarget.DOScale(m_cacheScale,0.25f);
		}
		if(dragChangeRotate!=0f){
			dragTarget.DOLocalRotate(m_cacheRotation,0.25f,RotateMode.Fast);
		}

		if(!string.IsNullOrEmpty(onDropMethodName)){
			Collider2D[] cols = Physics2D.OverlapPointAll(triggerPos.position,rayCastMask,-100f,100f);
			if(cols.Length>0){
				foreach(Collider2D col in cols){
					if(col.gameObject!=gameObject)
						col.SendMessage(onDropMethodName, dragTarget.gameObject , SendMessageOptions.DontRequireReceiver);
				}
				gameObject.SendMessage(onDropMethodName, cols , SendMessageOptions.DontRequireReceiver);
			}
		}
		if(releaseAutoBack){
			BackPosition();
		}else{
			dragTarget.position -= new Vector3(0,0,dragOffsetZ);
			dragTarget.SetParent(m_parent);
		}

		if(OnEndDragAction!=null){
			OnEndDragAction(this,eventData);
		}
		this.GetComponent<Graphic>().raycastTarget = true;
	}


	public void OnDrag(PointerEventData eventData)
	{
		if(!this.enabled  || !m_canDrag || !m_isDown)  return;

		if(!string.IsNullOrEmpty(dragingParent) && !dragTarget.parent.name.Equals(dragingParent)){
			GameObject go = GameObject.Find(dragingParent);
			if(go){
				dragTarget.SetParent(go.transform);
			}
		}

		if (eventData.dragging)
		{
			m_isDragging = true;
			RectTransformUtility.ScreenPointToWorldPointInRectangle(dragTarget,eventData.position,rayCastCamera,out m_worldPos);
			if(!isDragOriginPoint){
				m_worldPos+=m_touchDownTargetOffset;
			}
			m_worldPos += (Vector3)dragOffset*0.01f;
			if(sendHoverEvent){
				Collider2D[] cols = Physics2D.OverlapPointAll(triggerPos.position,rayCastMask,-100f,100f);
				if(cols.Length>0){
					foreach(Collider2D col in cols){
						if(col.gameObject!=gameObject)
							col.SendMessage(onHoverMethodName, dragTarget.gameObject , SendMessageOptions.DontRequireReceiver);
					}
					gameObject.SendMessage(onHoverMethodName, cols , SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					gameObject.SendMessage(onHoverOutMethodName,SendMessageOptions.DontRequireReceiver);
				}
			}
		}

		if(OnDragAction!=null){
			OnDragAction(this,eventData);
		}
	}


	void Update(){
		if(!this.enabled) return;
		if(m_canDrag && m_isDragging){
			dragTarget.position = Vector3.Lerp(dragTarget.position,m_worldPos,dragMoveDamp);
		}
	}

	/// <summary>
	/// 返回到最初位置
	/// </summary>
	public void BackPosition(){
		switch(backEffect)
		{
		case DragBackEffect.Immediately:
			dragTarget.SetParent(m_parent);
			dragTarget.localPosition=m_cachePosition;
			dragTarget.localScale = m_cacheScale;
			dragTarget.localEulerAngles = m_cacheRotation;
			break;
		case DragBackEffect.Destroy:
			Destroy(dragTarget.gameObject);
			break;
		case DragBackEffect.TweenPosition:
			this.enabled = false;
			this.m_canDrag = false;
			dragTarget.SetParent(m_parent);
			dragTarget.DOLocalMove(m_cachePosition,backDuring).SetEase(tweenEase).OnComplete(()=>{
				this.enabled = true;
				this.m_canDrag = true;
				if(OnTweenOverAction!=null){
					OnTweenOverAction(this);
				}
			});
			break;
		case DragBackEffect.TweenScale:
			this.enabled = false;
			this.m_canDrag = false;
			dragTarget.SetParent(m_parent);
			dragTarget.localPosition=m_cachePosition;
			dragTarget.localScale = Vector3.zero;
			dragTarget.DOScale(m_cacheScale,backDuring).SetEase(tweenEase).OnComplete(()=>{
				this.enabled = true;
				this.m_canDrag = true;
				if(OnTweenOverAction!=null){
					OnTweenOverAction(this);
				}
			});
			break;
		case DragBackEffect.ScaleDestroy:
			this.enabled = false;
			this.m_canDrag = false;
			dragTarget.DOScale(Vector3.zero,backDuring).SetEase(tweenEase).OnComplete(()=>{
				Destroy(dragTarget.gameObject);
				if(OnTweenOverAction!=null){
					OnTweenOverAction(this);
				}
			});
			break;
		case DragBackEffect.FadeOutDestroy:
			this.enabled = false;
			this.m_canDrag = false;
			CanvasGroup group = dragTarget.gameObject.AddComponent<CanvasGroup>();
			group.blocksRaycasts = false;
			group.DOFade(0f,backDuring).SetEase(tweenEase).OnComplete(()=>{
				Destroy(dragTarget.gameObject);
				if(OnTweenOverAction!=null){
					OnTweenOverAction(this);
				}
			});
			break;
		}
	}

}
