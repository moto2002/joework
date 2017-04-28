using UnityEngine;
using System.Collections;

public class ViewBaseObject:IBaseObject
{
	protected Transform m_transform;

	public Vector3 GetPosition()
	{
		return this.m_transform.position;
	}

	public Vector3 GetRotation()
	{
		return this.m_transform.rotation.eulerAngles;
	}

	public Vector3 GetScale()
	{
		return this.m_transform.localScale;
	}

	public virtual void Init(IBaseObject ibaseObject)
	{
	
	}

	public virtual void Hide()
	{
		this.m_transform.gameObject.SetActive (false);
	}

	public virtual void Destroy()
	{
		GameObject.Destroy (this.m_transform.gameObject);
	}
}
