using UnityEngine;
using System.Collections;

/// <summary>
/// 拖动并旋转物体，用力的方式.
/// author:zhouzhanglin
/// </summary>
public class DragMoveAndRotate : MonoBehaviour {

	private Vector3 m_pos;
	private Rigidbody m_rb;
	private Transform m_trans;
	private bool m_isDown = false;

	[Tooltip("拖动时的缓冲，跟Rigidbody的Drag属性也有关系.")]
	public float dragDamp = 0.1f;

	[Tooltip("旋转的速度 ，跟Rigidbody的Angular Drag属性也有关系.")]
	public float rotateSpeed = 1f;

	[Tooltip("射线检测距离.")]
	public float raycastDistance = 100f;

	[Tooltip("射线检测的Layer")]
	public LayerMask[] rayCastMasks;

	// Use this for initialization
	void Start () {
		m_trans = transform;
		m_rb = GetComponent<Rigidbody> ();
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
						m_pos = Input.mousePosition;
					}
				}
			}

		}else if(Input.GetMouseButton(0) && m_isDown){
			Vector3 curr = Input.mousePosition;
			Vector3 delta = curr - m_pos;

			Vector3 screenPos = Camera.main.WorldToScreenPoint (m_trans.position);
			float torqueForce = screenPos.y < curr.y ? delta.x : -delta.x;
			m_rb.AddRelativeTorque(Vector3.up * Time.deltaTime * torqueForce*rotateSpeed);

			Vector3 dragForce = new Vector3 (Input.mousePosition.x-screenPos.x , 0f, Input.mousePosition.y - screenPos.y);
			m_rb.AddForce ( dragForce*dragDamp*Time.deltaTime, ForceMode.VelocityChange);

			m_pos = curr;
		}else if(Input.GetMouseButtonUp(0)){
			m_isDown = false;
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
}
