using UnityEngine;
using System.Collections;

public class Main2 : MonoBehaviour 
{
	float rotateSpeed = 5f;
	Vector3 clickWorldPos  = Vector3.zero;
	Vector3 direction = Vector3.zero;

	CharacterController mCharacterController;
	float moveSpeed = 2f;

	void Start () 
	{
		mCharacterController = this.GetComponent<CharacterController>();
	}

	void Update () 
	{
		this.InputCheck ();

		if (clickWorldPos != Vector3.zero) 
		{
			Vector3 tempVec = (clickWorldPos - this.transform.position);
			direction = tempVec.normalized;
			this.ToWards (direction);
			this.ToMove(tempVec);
	
		} 
		else 
		{

		}

		//画线
		Debug.DrawLine (clickWorldPos,this.transform.position,Color.red);
	
	}

	public void InputCheck()
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			int layerMask = this.GetLayerMaskValue("Ground");
			if (Physics.Raycast(ray,out hit,1000f,layerMask)) 
			{
				//Debug.Log(hit.point);
				this.clickWorldPos = hit.point;
			}
		}
	}

	public void ToWards(Vector3 _direction)
	{
		this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (_direction), Time.deltaTime * rotateSpeed);
	}

	public void ToMove(Vector3 _vec)
	{

		if (_vec.sqrMagnitude <= 1.0f)
		{
			mCharacterController.SimpleMove (Vector3.zero);
		} 
		else 
		{
			Vector3 moveStep = _vec.normalized * moveSpeed;
			mCharacterController.SimpleMove(moveStep);
		}
	}

	public int GetLayerMaskValue(string s)
	{
		LayerMask mask;
		mask = 1 << LayerMask.NameToLayer (s);
		return mask.value;
	}
}
