using UnityEngine;
using System.Collections;

/// <summary>
/// logic
/// </summary>
public class HeroObject : BaseObject,IHeroObject
{
	private IHeroObject m_ViewHeroObject;

	public HeroObject(uint objectKey):base(objectKey)
	{
		this.m_roleType = ERoleType.Hero;
	}

	public void Init(IHeroObject iHeroObject)
	{
		this.m_ViewHeroObject = iHeroObject;
		this.m_ViewHeroObject.Init (this);
	}

	public override void Init (IBaseObject iBaseObject)
	{
		base.Init (iBaseObject);
		this.m_ViewHeroObject = iBaseObject as IHeroObject;
	}

	public override void Hide ()
	{
		base.Hide ();
	}

	public override void Destroy ()
	{
		base.Destroy ();
	}

	public override void InitLogic (Vector3 position,Vector3 rotation,Vector3 scale)
	{
		base.InitLogic (position,rotation,scale);
		Debug.Log ("hero logic init");
	}

}
