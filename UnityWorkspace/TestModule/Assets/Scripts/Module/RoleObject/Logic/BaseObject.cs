using UnityEngine;
using System.Collections;

public class BaseObject:IBaseObject
{
	protected uint m_objectKey;
	protected Vector3 m_position;
	protected Vector3 m_rotation;
	protected Vector3 m_scale;
	protected ERoleType m_roleType;
	protected EFlag m_flag;

	public BaseObject(uint objectKey)
	{
		this.m_objectKey = objectKey;
		this.m_roleType = ERoleType.None;
		this.m_flag = EFlag.None;
	}

	public virtual void InitLogic(Vector3 position,Vector3 rotation,Vector3 scale)
	{
		this.m_position = position;
		this.m_rotation = rotation;
		this.m_scale = scale;
	}

	public Vector3 GetPosition()
	{
		return this.m_position;
	}
	public Vector3 GetRotation()
	{
		return this.m_rotation;
	}
	public Vector3 GetScale()
	{
		return this.m_scale;
	}

	public virtual void Init(IBaseObject iBaseObject)
	{
	
	}

	public virtual void Hide()
	{

	}

	public virtual void Destroy()
	{

	}
}
