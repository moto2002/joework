using UnityEngine;
using System.Collections;

public class RoleObjectController 
{
	private static RoleObjectController m_instance;
	public static RoleObjectController Instance
	{
		get
		{
			if(m_instance == null)
				m_instance = new RoleObjectController();
			return m_instance;
		}
	}

	public HeroObject CreateHeroObject()
	{
		uint objectKey = Tools.GetUniqueObjectKey();
		HeroObject heroObject = new HeroObject (objectKey);
		return heroObject;
	}

}
