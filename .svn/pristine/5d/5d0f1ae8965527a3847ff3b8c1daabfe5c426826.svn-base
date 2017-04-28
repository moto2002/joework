using UnityEngine;
using System.Collections;

/// <summary>
/// view
/// </summary>
public class ViewHeroObject : ViewBaseObject, IHeroObject
{
	private IHeroObject m_logicHeroObject;

	public override void Init (IBaseObject ibaseObject)
	{
		base.Init (ibaseObject);
		this.m_logicHeroObject = ibaseObject as IHeroObject;

		Debug.Log ("hero view init");
		
		//for test
		GameObject skin = GameObject.CreatePrimitive (PrimitiveType.Cube);
		this.m_transform = skin.transform;
		this.m_transform.position = m_logicHeroObject.GetPosition ();
		this.m_transform.rotation = Quaternion.Euler (m_logicHeroObject.GetRotation());
		this.m_transform.localScale = m_logicHeroObject.GetScale ();
	}

	public override void Hide ()
	{
		base.Hide ();
	}

	public override void Destroy ()
	{
		base.Destroy ();
	}

}
