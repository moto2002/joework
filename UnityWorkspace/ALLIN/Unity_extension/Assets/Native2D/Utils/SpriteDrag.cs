using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;

/// <summary>
/// Drag and drop sprite.需要dragTarget上面有Collider2D组件
/// Author :zhouzhanglin
/// </summary>
public class SpriteDrag : MonoBehaviour {

	public enum DragBackEffect{
		None,Immediately, TweenPosition, TweenScale , ScaleDestroy , FadeOutDestroy , Destroy
	}

	private Vector3 m_cachePosition; //全局坐标
	private Vector3 m_cacheScale;
	private Vector3 m_cacheRotation;
	private Vector3 m_defaultPosition; //全局坐标
	private Vector3 m_defaultScale;
	private Vector3 m_defaultRotation;
	private Vector3 m_dragOffset;
	private Vector3 m_screenPosition;
	private Vector3 m_currentPosition;
	private Vector3 m_mousePressPosition; //moues按下时的位置
	private float m_downTime; //mouse按下时的Time.realtimeSinceStartup 时间 
	private bool m_isDown;
	private bool m_isDragging = false;
	private bool m_canDrag = true;
	private string m_sortLayerName;

	#region getter/setter
	public float mouseDownTime{
		get { return m_downTime; }
	}
	public Vector3 mousePressPosition{
		get { return m_mousePressPosition; }
	}
	public bool isDragging{
		get { return m_isDragging && m_isDown; }
	}
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
	#endregion

	[Tooltip("拖动的对象，默认为自己.")]
	public Transform dragTarget = null;

	[Tooltip("如果为null，则使用mainCamera.")]
	public Camera rayCastCamera = null;

	[Tooltip("射线检测的Layer")]
	public LayerMask dragRayCastMask=-1;
	public LayerMask dropRayCastMask=-1;
	public bool dragCheckUGUI = false;//drag时是否判断在ugui上

	[Tooltip("判断drag时是否忽略上面")]
	public bool dragIgnoreTop = true;
	[Tooltip("判断drop时是否忽略下面")]
	public bool dropIgnoreBottom = true;

	[Header("Drag Setting")]
	[Tooltip("在拖动时是否固定在拖动物的原点.")]
	public bool isDragOriginPoint = false;

	[Tooltip("当isDragOriginPoint为true时，拖动时的偏移值.")]
	public Vector2 dragOffset;

	[Tooltip("主要用于影响层级显示.")]
	public float dragOffsetZ=0f;

	[Tooltip("Drag时的变化的大小.")]
	public float dragChangeScale = 1f;

	[Tooltip("Drag时角度的变化值")]
	public float dragChangeRotate = 0f;

	[Tooltip("拖动时的缓动参数.")]
	[Range(0f,1f)]
	public float dragMoveDamp = 1f;

	[Tooltip("拖动的时候在哪个层.没有设置的话为当前Sort Layer")]
	public string dragSortLayerName;

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
	public float backDuring = 0.5f;
	[Tooltip("Tween 的效果")]
	public Ease tweenEase = Ease.Linear;

	public event Action<SpriteDrag> OnPrevBeginDragAction = null ;
	public event Action<SpriteDrag> OnBeginDragAction = null ;
	public event Action<SpriteDrag> OnDragAction = null ;
	public event Action<SpriteDrag> OnEndDragAction = null ;
	public event Action<SpriteDrag> OnTweenBackAction = null ;
	public delegate bool DragValidCheck();
	public event DragValidCheck DragValidCheckEvent;

	void OnDisable(){
		m_isDown = false;
		m_isDragging = false;
	}

	// Use this for initialization
	void Start () {
		if (!dragTarget){
			dragTarget = transform;
		}
		if(!triggerPos){
			triggerPos = dragTarget;
		}
		if (!rayCastCamera)
		{
			rayCastCamera = Camera.main;
		}
		m_defaultScale = dragTarget.localScale;
		m_defaultRotation = dragTarget.localEulerAngles;
		m_defaultPosition = dragTarget.position;
		SpriteRenderer spriteRender = dragTarget.GetComponentInChildren<SpriteRenderer>();
		m_sortLayerName = spriteRender.sortingLayerName;
		if(string.IsNullOrEmpty(dragSortLayerName)){
			dragSortLayerName = m_sortLayerName;
		}
	}

	public void SetDefaultPosition(){
		if(dragTarget) dragTarget.position = m_defaultPosition;
	}
	public void SetDefaultRotation(){
		if(dragTarget) dragTarget.localEulerAngles = m_defaultScale;
	}
	public void SetDefaultScale(){
		if(dragTarget) dragTarget.localScale = m_defaultScale;
	}
	
	// Update is called once per frame
	void Update () {
		if(!this.isActiveAndEnabled) return;
		if (Input.touchCount < 2)
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (!m_isDown && !InputUtil.isOnUI)
				{
					if(dragCheckUGUI && InputUtil.CheckMouseOnUI()) return;

					RaycastHit2D[] hits = Physics2D.RaycastAll(rayCastCamera.ScreenToWorldPoint(Input.mousePosition),Vector2.zero, 0, dragRayCastMask);
					if(hits!=null && hits.Length>0){
						foreach(RaycastHit2D hit in hits){
							if (hit.collider.gameObject == gameObject)
							{
								m_isDown = true;
								m_downTime = Time.realtimeSinceStartup;
								m_mousePressPosition = Input.mousePosition;
								break;
							}
							if(dragIgnoreTop) break;
						}
					}
				}
			}
			else if (m_isDown && Input.GetMouseButton(0))
			{
				OnMouseDragHandler();
			}
			else if (m_isDown && Input.GetMouseButtonUp(0))
			{
				OnMouseUpHandler();
			}
		}
	}

	void OnMouseBeginDrag(){
		if(!this.enabled || !m_isDown) return;

		if(DragValidCheckEvent!=null) {
			if(!DragValidCheckEvent()){
				m_canDrag = false;
				return;
			}
		}
		if(OnPrevBeginDragAction!=null){
			OnPrevBeginDragAction(this);
		}

		this.m_canDrag = true;
		this.m_isDragging = true;
		dragTarget.DOKill();

		m_cachePosition = dragTarget.position;
		m_cacheScale = dragTarget.localScale;
		m_cacheRotation = dragTarget.localEulerAngles;
		if(dragChangeScale!=1f){
			dragTarget.DOScale(m_cacheScale*dragChangeScale,0.25f);
		}
		if(dragChangeRotate!=0f){
			dragTarget.DOLocalRotate(m_cacheRotation +new Vector3(0f,0f,dragChangeRotate),0.4f,RotateMode.Fast);
		}

		dragTarget.position+=new Vector3(0,0,dragOffsetZ);
		m_currentPosition = m_cachePosition;
		m_dragOffset = Vector3.zero;

		foreach(SpriteRenderer render in dragTarget.GetComponentsInChildren<SpriteRenderer>()){
			render.sortingLayerName=dragSortLayerName;
		}

		m_screenPosition = rayCastCamera.WorldToScreenPoint(dragTarget.position);
		if (!isDragOriginPoint)
		{
			m_dragOffset = dragTarget.position - rayCastCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_screenPosition.z));
		}
		if (OnBeginDragAction!=null)
		{
			OnBeginDragAction(this);
		}
	}

	void OnMouseDragHandler(){
		if(this.enabled && !m_isDragging){
			OnMouseBeginDrag();
		}

		if(!this.enabled  || !m_isDragging)  return;

		m_screenPosition = rayCastCamera.WorldToScreenPoint(dragTarget.position);
		Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_screenPosition.z);
		m_currentPosition = rayCastCamera.ScreenToWorldPoint(curScreenSpace);
		if (!isDragOriginPoint){
			m_currentPosition += m_dragOffset;
		}else{
			m_currentPosition += (Vector3)dragOffset;
		}
		dragTarget.position = Vector3.Lerp(dragTarget.position, m_currentPosition, dragMoveDamp);
		if(sendHoverEvent){
			Collider2D[] cols = Physics2D.OverlapPointAll(triggerPos.position,dropRayCastMask,-100f,100f);
			if(cols.Length>0){
				foreach(Collider2D col in cols){
					if(col.gameObject!=gameObject)
						col.SendMessage(onHoverMethodName, dragTarget.gameObject , SendMessageOptions.DontRequireReceiver);
					if(dropIgnoreBottom) break;
				}
				gameObject.SendMessage(onHoverMethodName, cols , SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				gameObject.SendMessage(onHoverOutMethodName,SendMessageOptions.DontRequireReceiver);
			}
		}
		if (OnDragAction!=null)
		{
			OnDragAction(this);
		}
	}

	void OnApplicationFocus(bool flag){
		if(!flag && m_canDrag && m_isDragging){
			OnMouseUpHandler();
		}
	}

	void OnMouseUpHandler(){
		m_isDown = false;

		if(!this.enabled || !m_isDragging) return;
		m_isDragging = false;

		DOTween.Kill(dragTarget);
		if(dragChangeScale!=0f){
			dragTarget.DOScale(m_cacheScale,0.25f);
		}
		if(dragChangeRotate!=0f){
			dragTarget.DOLocalRotate(m_cacheRotation,0.25f,RotateMode.Fast);
		}


		if(releaseAutoBack){
			BackPosition();
		}else{
			dragTarget.position -=new Vector3(0,0,dragOffsetZ);
			foreach(SpriteRenderer render in dragTarget.GetComponentsInChildren<SpriteRenderer>()){
				render.sortingLayerName=m_sortLayerName;
			}
		}

		if(!string.IsNullOrEmpty(onDropMethodName)){
			Collider2D[] cols = Physics2D.OverlapPointAll(triggerPos.position,dropRayCastMask,-100f,100f);
			if(cols.Length>0){
				foreach(Collider2D col in cols){
					if(col.gameObject!=gameObject)
						col.SendMessage(onDropMethodName, dragTarget.gameObject , SendMessageOptions.DontRequireReceiver);
					if(dropIgnoreBottom) break;
				}
				gameObject.SendMessage(onDropMethodName, cols , SendMessageOptions.DontRequireReceiver);
			}
		}

		if (OnEndDragAction!=null)
		{
			OnEndDragAction(this);
		}
	}

	/// <summary>
	/// 返回原来位置
	/// </summary>
	public void BackPosition(){
		switch(backEffect)
		{
		case DragBackEffect.Immediately:
			foreach(SpriteRenderer render in GetComponentsInChildren<SpriteRenderer>()){
				render.sortingLayerName=m_sortLayerName;
			}
			dragTarget.position=m_cachePosition;
			break;
		case DragBackEffect.Destroy:
			Destroy(dragTarget.gameObject);
			break;
		case DragBackEffect.TweenPosition:
			this.enabled = false;
			this.m_canDrag = false;
			dragTarget.DOMove(new Vector3(m_cachePosition.x,m_cachePosition.y,dragTarget.position.z),backDuring).SetEase(tweenEase).OnComplete(()=>{
				this.enabled = true;
				this.m_canDrag = true;
				dragTarget.position=m_cachePosition;
				foreach(SpriteRenderer render in GetComponentsInChildren<SpriteRenderer>()){
					render.sortingLayerName=m_sortLayerName;
				}
				if(OnTweenBackAction!=null){
					OnTweenBackAction(this);
				}
			});
			break;
		case DragBackEffect.TweenScale:
			this.enabled = false;
			this.m_canDrag = false;
			foreach(SpriteRenderer render in GetComponentsInChildren<SpriteRenderer>()){
				render.sortingLayerName=m_sortLayerName;
			}
			dragTarget.position=m_cachePosition;
			dragTarget.localScale = Vector3.zero;
			dragTarget.DOScale(m_cacheScale,backDuring).SetEase(tweenEase).OnComplete(()=>{
				this.enabled = true;
				this.m_canDrag = true;
				if(OnTweenBackAction!=null){
					OnTweenBackAction(this);
				}
			});
			break;
		case DragBackEffect.ScaleDestroy:
			this.enabled = false;
			this.m_canDrag = false;
			dragTarget.DOScale(Vector3.zero,backDuring).SetEase(tweenEase).OnComplete(()=>{
				Destroy(dragTarget.gameObject);
				if(OnTweenBackAction!=null){
					OnTweenBackAction(this);
				}
			});
			break;
		case DragBackEffect.FadeOutDestroy:
			this.enabled = false;
			this.m_canDrag = false;
			CanvasGroup group = dragTarget.gameObject.AddComponent<CanvasGroup>();
			group.DOFade(0f,backDuring).SetEase(tweenEase).OnComplete(()=>{
				Destroy(dragTarget.gameObject);
				if(OnTweenBackAction!=null){
					OnTweenBackAction(this);
				}
			});
			break;
		}
	}
}
