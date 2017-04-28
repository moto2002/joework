using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 旋转物体。注意：使用的是主相机(mainCamera)
/// send Message:OnSpinWithMouseDown,OnSpinWithMouseMove,OnSpinWithMouseRelease,OnSpinWithMouseRotate
/// author:zhouzhanglin
/// 2015/3/13
/// </summary>
public class SpinWithMouse3D : MonoBehaviour {

	public enum RotationAxis
	{
		DirX,DirY,DirZ
	}
	public enum DragDirection
	{
		LeftAndRight,UpAndDown,Left,Right,Up,Down
	}

	private Vector3 m_screenPos;
	private Transform m_trans;
	private float m_rotateToAngleDamp;
	private bool m_isDown = false;

	public Action OnMouseDonwAction, OnMouseDragAction, OnMouseUpAction,OnMouseRotateAction;

	[Tooltip("是否发送鼠标事件 OnSpinWithMouseDown,OnSpinWithMouseMove,OnSpinWithMouseRelease,OnSpinWithMouseRotate .")]
	public bool isSendMouseEvent = false;

	[Tooltip("旋转轴.")]
	public RotationAxis rotationAxis = RotationAxis.DirZ;

	[Tooltip("旋转的空间.")]
	public Space rotationSpace=Space.Self;

	[Tooltip("鼠标允许拖动的方向.鼠标在此方向上移动，物体才会旋转.")]
	public DragDirection dragDir = DragDirection.UpAndDown;

	[Tooltip("拖动时的旋转速度.")]
	public float dragRotationSpeed = 2f;

	[Tooltip("射线检测距离.")]
	public float raycastDistance = 100f;

	[Tooltip("射线检测的Layer")]
	public LayerMask[] rayCastMasks;

	#region MonoBehaviour内置
	void Awake(){
		m_trans = transform;
	}

	void Update(){
		if(Input.GetMouseButtonDown(0)){
			if(!m_isDown){
				int mask = GetLayerMask(rayCastMasks);
				RaycastHit hit;
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, raycastDistance, mask))
				{
					if (hit.collider.gameObject == gameObject)
					{
						m_isDown = true;
						StopAllCoroutines ();
						m_screenPos = Camera.main.WorldToScreenPoint (Input.mousePosition);
						if (OnMouseDonwAction != null)
							OnMouseDonwAction ();
					}
				}
			}
		}else if(Input.GetMouseButton(0) && m_isDown){
			Vector3 currentScreen = Camera.main.WorldToScreenPoint (Input.mousePosition);
			if (dragDir == DragDirection.UpAndDown) {
				Rotate ( m_screenPos.y - currentScreen.y);
			} else if (dragDir == DragDirection.LeftAndRight) {
				Rotate (currentScreen.z - m_screenPos.z);
			}else if (dragDir == DragDirection.Left) {
				if(currentScreen.z>m_screenPos.z)
					Rotate (currentScreen.z - m_screenPos.z);
			}else if (dragDir == DragDirection.Right) {
				if(currentScreen.z<m_screenPos.z)
					Rotate (currentScreen.z - m_screenPos.z);
			}else if (dragDir == DragDirection.Up) {
				if(currentScreen.y<m_screenPos.y)
					Rotate ( m_screenPos.y - currentScreen.y);
			}else if (dragDir == DragDirection.Down) {
				if(currentScreen.y>m_screenPos.y)
					Rotate ( m_screenPos.y - currentScreen.y);
			}
			m_screenPos = currentScreen;
			if (OnMouseDragAction!=null)
				OnMouseDragAction ();
		}else if(Input.GetMouseButtonUp(0) && m_isDown){
			m_isDown = false;
			if (OnMouseUpAction != null)
				OnMouseUpAction ();
		}
	}
	/// <summary>
	/// 根据LayerMask数组来获取 射线检测的所有层.
	/// </summary>
	/// <returns>The layer mask.</returns>
	/// <param name="masks">Masks.</param>
	private int GetLayerMask(LayerMask[] masks)
	{
		if(masks==null || masks.Length==0) return -1;
		int mask = 1<<masks[0];
		for (int i = 1; i < masks.Length; i++)
		{
			mask = mask|1<<masks[i].value;
		}
		return mask;
	}
	#endregion



	private void Rotate( float delta){
		float change = delta*dragRotationSpeed*Time.deltaTime ;
		if (rotationSpace == Space.Self) {
			if (rotationAxis == RotationAxis.DirZ) {
				m_trans.localRotation = Quaternion.Euler(0f, 0f ,change ) * m_trans.localRotation;
			}else if (rotationAxis == RotationAxis.DirX) {
				m_trans.localRotation = Quaternion.Euler(change, 0f ,0f ) * m_trans.localRotation;
			}else if (rotationAxis == RotationAxis.DirY) {
				m_trans.localRotation = Quaternion.Euler(0f, change, 0f ) * m_trans.localRotation;
			}
		} else {
			if (rotationAxis == RotationAxis.DirZ) {
				m_trans.rotation = Quaternion.Euler(0f, 0f ,change ) * m_trans.rotation;
			}else if (rotationAxis == RotationAxis.DirX) {
				m_trans.rotation = Quaternion.Euler(change, 0f ,0f ) * m_trans.rotation;
			}else if (rotationAxis == RotationAxis.DirY) {
				m_trans.rotation = Quaternion.Euler(0f, change, 0f ) * m_trans.rotation;
			}
		}
		if (OnMouseRotateAction != null)
			OnMouseRotateAction ();
	}

	/// <summary>
	/// 旋转到一定的角度.
	/// </summary>
	/// <param name="angle">Angle.</param>
	/// <param name="rotateToAngleDamp">旋转的动画缓冲值.</param>
	public void RotationToAngle( float angle ,float rotateToAngleDamp=0.5f ){
		this.m_rotateToAngleDamp = rotateToAngleDamp;
		StartCoroutine ("RotationToAngleTween",angle);
	}

	private IEnumerator RotationToAngleTween( float angle ){
		if (rotationSpace == Space.Self) {

			if (rotationAxis == RotationAxis.DirZ) {
				while(Mathf.Abs(m_trans.localEulerAngles.z-angle)>0.01f){
					float temp = Mathf.Lerp(m_trans.localEulerAngles.z,angle,m_rotateToAngleDamp);
					Vector3 localRotation = m_trans.localEulerAngles;
					localRotation.z = temp;
					m_trans.localEulerAngles = localRotation;
					yield return new WaitForFixedUpdate();
				}
				Vector3 rotation = m_trans.localEulerAngles;
				rotation.z = angle;
				m_trans.localEulerAngles = rotation;
			}
			else if (rotationAxis == RotationAxis.DirX){
				while(Mathf.Abs(m_trans.localEulerAngles.x-angle)>0.01f){
					float temp = Mathf.Lerp(m_trans.localEulerAngles.x,angle,m_rotateToAngleDamp);
					Vector3 localRotation = m_trans.localEulerAngles;
					localRotation.x = temp;
					m_trans.localEulerAngles = localRotation;
					yield return new WaitForFixedUpdate();
				}
				Vector3 rotation = m_trans.localEulerAngles;
				rotation.x = angle;
				m_trans.localEulerAngles = rotation;
			}
			else if (rotationAxis == RotationAxis.DirY){
				while(Mathf.Abs(m_trans.localEulerAngles.y-angle)>0.01f){
					float temp = Mathf.Lerp(m_trans.localEulerAngles.y,angle,m_rotateToAngleDamp);
					Vector3 localRotation = m_trans.localEulerAngles;
					localRotation.y = temp;
					m_trans.localEulerAngles = localRotation;
					yield return new WaitForFixedUpdate();
				}
				Vector3 rotation = m_trans.localEulerAngles;
				rotation.y = angle;
				m_trans.localEulerAngles = rotation;
			}

		}else {

			if (rotationAxis == RotationAxis.DirZ) {
				while(Mathf.Abs(m_trans.eulerAngles.z-angle)>0.01f){
					float temp = Mathf.Lerp(m_trans.eulerAngles.z,angle,m_rotateToAngleDamp);
					Vector3 angles = m_trans.eulerAngles;
					angles.z = temp;
					m_trans.eulerAngles = angles;
					yield return new WaitForFixedUpdate();
				}
				Vector3 rotation = m_trans.eulerAngles;
				rotation.z = angle;
				m_trans.eulerAngles = rotation;
			}
			else if (rotationAxis == RotationAxis.DirX){
				while(Mathf.Abs(m_trans.eulerAngles.x-angle)>0.01f){
					float temp = Mathf.Lerp(m_trans.eulerAngles.x,angle,m_rotateToAngleDamp);
					Vector3 angles = m_trans.eulerAngles;
					angles.x = temp;
					m_trans.eulerAngles = angles;
					yield return new WaitForFixedUpdate();
				}
				Vector3 rotation = m_trans.eulerAngles;
				rotation.x = angle;
				m_trans.eulerAngles = rotation;
			}
			else if (rotationAxis == RotationAxis.DirY){
				while(Mathf.Abs(m_trans.eulerAngles.y-angle)>0.01f){
					float temp = Mathf.Lerp(m_trans.eulerAngles.y,angle,m_rotateToAngleDamp);
					Vector3 localRotation = m_trans.eulerAngles;
					localRotation.y = temp;
					m_trans.eulerAngles = localRotation;
					yield return new WaitForFixedUpdate();
				}
				Vector3 rotation = m_trans.eulerAngles;
				rotation.y = angle;
				m_trans.eulerAngles = rotation;
			}

		}
			


	}
}
