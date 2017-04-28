using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// 拖放3D物体，使用两种方式，一种用系统OnMouseDown,OnMouseDrag,OnMouseUp事件，另一种用射线检测。
/// send Message:OnDragAndDropDown,OnDragAndDropMove,OnDragAndDropRelease
/// author:zhouzhanglin
/// 2015/3/12
/// </summary>
[RequireComponent(typeof(Collider))]
public class DragAndDrop3D : MonoBehaviour
{
	
	public enum DragBackEffect
	{
		None, Immediately, TweenPosition, TweenScale
	}
	
	private Vector3 m_cachePosition;
	private Vector3 m_cacheScale;
	private Vector3 m_dragOffset;
	private Vector3 m_screenPosition;
	private bool m_haveRigidbody;
	private Rigidbody m_rigidBody;
	private bool m_defaultIsKinematic;
	private Transform m_trans;
	private Collider[] m_colliders = null;
	private bool m_isDown;
	private Vector3 m_currentPosition;
	private LayerMask m_currentLayer;
	private bool m_isTweening=false;
	private Plane m_mousePickPlane = new Plane();

	public event Action<DragAndDrop3D> OnMouseDownAction = null ;
	public event Action<DragAndDrop3D> OnMouseDragAction = null ;
	public event Action<DragAndDrop3D,bool> OnMouseUpAction = null ;
	public event Action<DragAndDrop3D> OnTweenBackAction = null ;

	[HideInInspector]
	public int rayCastMasksLength = 0; //use for editor
	[HideInInspector]
	public int dropLayerMaskLength=0; //use for editor
	
	[Tooltip("拖动的对象，默认为自己.")]
	public Transform dragTarget = null;
	
	[Tooltip("Drag时是否禁用此对象的collider组件.")]
	public bool isDragDisableCollider = false;
	
	[Tooltip("如果为null，则使用mainCamera.")]
	public Camera rayCastCamera = null;
	
	[Tooltip("射线的检测距离，只用于射线检测时.")]
	public float raycastDistance = 100f;
	
	[Tooltip("射线检测的Layer")]
	public LayerMask[] rayCastMasks;
	
	[Tooltip("在拖动时是否固定在拖动物的原点.")]
	public bool isDragOriginPoint = false;
	
	[Tooltip("如果isDragOriginPoint为true,则可以设置拖动时的偏移值.")]
	public Vector3 dragOffset;
	
	[Tooltip("拖动时的缓动参数.")]
	[Range(0f,1f)]
	public float dragMoveDamp = 0.5f;
	
	[Tooltip("移动时在哪个面上移动，如果为null，则在拖动物的Z轴面移动.")]
	public GameObject mousePickLayer = null;

	[Tooltip("pick layer是否是Plane，plane是无区域限制.")]
	public bool pickLayerIsPlane = false;

	[Tooltip("drop的位置是否通过当前鼠标位置计算，如果不是，则通过拖动的物体位置计算.")]
	public bool dropPosByMouse = true;

	[Tooltip("通过物体对象来获取位置")]
	public Transform dropRefPos = null;
	
	[Tooltip("drop容器所在的层.")]
	public LayerMask[] dropLayerMasks;
	
	[Tooltip("drop发生时发送的事件，drop和当前拖动对象都会发送.")]
	public string dropMedthod = "OnDrop";
	
	[Tooltip("如果没有检测到可drop的容器，是否返回原来的位置.")]
	public bool isDropFailBack = true;
	
	[Tooltip("返回原来位置时的效果.")]
	public DragBackEffect dragBackEffect = DragBackEffect.None;
	
	[Tooltip("返回原来位置时的速度.对TweenPosition和TweenScale有用.")]
	public float backEffectSpeed = 10f;

	[Tooltip("拖动的时候在哪个层.")]
	public LayerMask dragLayer;

	#region MonoBehaviour内置方法.
	void Start()
	{
		if (dragTarget)
		{
			m_trans = dragTarget;
		}
		else
		{
			m_trans = transform;
			dragTarget = m_trans;
		}
		m_colliders = GetComponentsInChildren<Collider>();
		if (mousePickLayer)
		{
			mousePickLayer.SetActive(false);
			if(pickLayerIsPlane){
				m_mousePickPlane.SetNormalAndPosition(mousePickLayer.transform.position,mousePickLayer.transform.up);
			}
		}
		if (!rayCastCamera)
		{
			rayCastCamera = Camera.main;
		}
		if(!dropPosByMouse){
			if(!dropRefPos){
				dropRefPos = m_trans;
			}
		}
		m_rigidBody = GetComponent<Rigidbody> ();
		m_currentLayer = m_trans.gameObject.layer;
	}
	void Update()
	{
		if(!this.isActiveAndEnabled) return;
		if (Input.touchCount < 2)
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (!m_isDown)
				{
					int mask = GetLayerMask(rayCastMasks);
					RaycastHit hit;
					if (Physics.Raycast(rayCastCamera.ScreenPointToRay(Input.mousePosition), out hit, raycastDistance, mask))
					{
						if (hit.collider.gameObject == dragTarget.gameObject)
						{
							m_isDown = true;
							OnMouseDownHandler();
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
				m_isDown = false;
				OnMouseUpHandler();
			}
		}
	}

	#endregion
	
	
	private void OnMouseDownHandler()
	{
		if(m_isTweening) return;

		m_cachePosition = m_trans.position;
		m_cacheScale = m_trans.localScale;
		m_currentPosition = m_cachePosition;
		m_dragOffset = Vector3.zero;

		if (isDragDisableCollider)
		{
			EnableColliders(false);
		}
		StopAllCoroutines();
		
		//set layer
		foreach(Transform child in m_trans.GetComponentsInChildren<Transform>()){
			child.gameObject.layer = dragLayer;
		}
		
		if (m_rigidBody)
		{
			m_haveRigidbody = true;
			m_defaultIsKinematic = m_rigidBody.isKinematic;
			m_rigidBody.isKinematic = true;
		}
		else
		{
			m_haveRigidbody = false;
		}
		m_screenPosition = rayCastCamera.WorldToScreenPoint(m_trans.position);
		if (!isDragOriginPoint)
		{
			m_dragOffset = m_trans.position - rayCastCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_screenPosition.z));
		}
		if (mousePickLayer)
		{
			mousePickLayer.SetActive(true);
		}
		if (OnMouseDownAction!=null)
		{
			OnMouseDownAction(this);
		}
	}
	
	private void OnMouseDragHandler()
	{
		if(m_isTweening) return;
		bool canDragMove = false;
		if (mousePickLayer)
		{
			Ray  ray = rayCastCamera.ScreenPointToRay(Input.mousePosition);
			if(pickLayerIsPlane)
			{
				float dis ;
				if(m_mousePickPlane.Raycast(ray,out dis)){
					m_currentPosition = ray.GetPoint(dis);
					canDragMove = true;
				}
			}
			else
			{
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, raycastDistance, 1 << mousePickLayer.layer))
				{
					if (hit.collider.gameObject == mousePickLayer)
					{
						m_currentPosition = hit.point;
						canDragMove = true;
					}
				}
			}
		}
		else
		{
			m_screenPosition = rayCastCamera.WorldToScreenPoint(m_trans.position);
			Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_screenPosition.z);
			m_currentPosition = rayCastCamera.ScreenToWorldPoint(curScreenSpace);
			canDragMove = true;
		}
		if(canDragMove)
		{
			if (!isDragOriginPoint)
			{
				m_currentPosition += m_dragOffset;
			}
			else
			{
				m_currentPosition += dragOffset;
			}
			if (dragMoveDamp > 0f)
			{
				if(m_rigidBody){
					m_rigidBody.MovePosition(Vector3.Lerp(m_trans.position, m_currentPosition, dragMoveDamp));
				}else{
					m_trans.position = Vector3.Lerp(m_trans.position, m_currentPosition, dragMoveDamp);
				}
			}
			else
			{
				if(m_rigidBody){
					m_rigidBody.MovePosition(m_currentPosition);
				}else{
					m_trans.position = m_currentPosition;
				}
			}
			if (OnMouseDragAction!=null)
			{
				OnMouseDragAction(this);
			}
		}
	}
	
	private void OnMouseUpHandler()
	{
		if(m_isTweening) return;
		if (isDragDisableCollider)
		{
			EnableColliders(true);
		}
		if (mousePickLayer)
		{
			mousePickLayer.SetActive(false);
		}
		if (m_haveRigidbody)
		{
			Rigidbody rb = GetComponent<Rigidbody>();
			if (rb)
			{
				rb.isKinematic = m_defaultIsKinematic;
			}
		}
		//check drop
		int mask = GetLayerMask(dropLayerMasks);
		RaycastHit hit;
		bool canDrop = false;
		//用鼠标位置还是引用对象的位置作为drop时的位置
		Vector3 dropPos = Input.mousePosition ;
		if(!dropPosByMouse && dropRefPos){
			dropPos =rayCastCamera.WorldToScreenPoint(dropRefPos.position);
		}
		if (Physics.Raycast(rayCastCamera.ScreenPointToRay(dropPos), out hit, raycastDistance, mask))
		{
			if (hit.collider.gameObject != dragTarget.gameObject)
			{ 
				//set layer
				foreach(Transform child in m_trans.GetComponentsInChildren<Transform>()){
					child.gameObject.layer = m_currentLayer;
				}
				canDrop = true;
				//Exclude myself
				hit.collider.SendMessage(dropMedthod, gameObject, SendMessageOptions.DontRequireReceiver);
				gameObject.SendMessage(dropMedthod, hit.collider.gameObject, SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				BackPosition();
			}
		}
		else
		{
			BackPosition();
		}
		if (OnMouseUpAction!=null)
		{
			OnMouseUpAction(this,canDrop);
		}
	}
	
	/// <summary>
	/// 根据LayerMask数组来获取 射线检测的所有层.
	/// </summary>
	/// <returns>The layer mask.</returns>
	/// <param name="masks">Masks.</param>
	private int GetLayerMask(LayerMask[] masks)
	{
		if(masks.Length==0) return -1;
		int mask = 1<<masks[0];
		for (int i = 1; i < masks.Length; i++)
		{
			mask = mask|1<<masks[i].value;
		}
		return mask;
	}
	
	
	/// <summary>
	/// 返回到原来的位置.
	/// </summary>
	public void BackPosition()
	{
		if (isDropFailBack)
		{
			switch (dragBackEffect)
			{
			case DragBackEffect.TweenPosition:
				EnableColliders(false);
				StartCoroutine("BackTween");
				break;
			case DragBackEffect.Immediately:
				m_trans.position = m_cachePosition;
				//set layer
				foreach(Transform child in m_trans.GetComponentsInChildren<Transform>()){
					child.gameObject.layer = m_currentLayer;
				}
				EnableColliders(true);
				break;
			case DragBackEffect.TweenScale:
				EnableColliders(false);
				m_trans.position = m_cachePosition;
				m_trans.localScale = Vector3.zero;
				StartCoroutine("ScaleTween");
				break;
			default:
				EnableColliders(true);
				break;
			}
		}
	}
	
	private void EnableColliders(bool value)
	{
		if (m_colliders != null && m_colliders.Length > 0)
		{
			for (int i = 0; i < m_colliders.Length; i++)
			{
				m_colliders[i].enabled = value;
			}
		}
	}
	
	private IEnumerator BackTween()
	{
		m_isTweening = true;
		//Prevent dragging
		while (Vector3.Distance(m_trans.position, m_cachePosition) > 0.01f)
		{
			m_trans.position = Vector3.Lerp(m_trans.position, m_cachePosition, backEffectSpeed * Time.fixedDeltaTime);
			yield return new WaitForFixedUpdate();
		}
		m_trans.position = m_cachePosition;
		//Prevent dragging
		EnableColliders(true);
		//set layer
		foreach(Transform child in m_trans.GetComponentsInChildren<Transform>()){
			child.gameObject.layer = m_currentLayer;
		}
		m_isTweening = false;
		if(OnTweenBackAction!=null){
			OnTweenBackAction(this);
		}
	}
	private IEnumerator ScaleTween()
	{
		m_isTweening = true;
		//Prevent dragging
		while (Vector3.Distance(m_trans.localScale, m_cacheScale) > 0.01f)
		{
			m_trans.localScale = Vector3.Lerp(m_trans.localScale, m_cacheScale, backEffectSpeed * Time.fixedDeltaTime);
			yield return new WaitForFixedUpdate();
		}
		m_trans.localScale = m_cacheScale;
		//Prevent dragging
		EnableColliders(true);
		//set layer
		foreach(Transform child in m_trans.GetComponentsInChildren<Transform>()){
			child.gameObject.layer = m_currentLayer;
		}
		m_isTweening = false;
		if(OnTweenBackAction!=null){
			OnTweenBackAction(this);
		}
	}
}