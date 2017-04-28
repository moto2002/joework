using UnityEngine;
using System.Collections;

public class Tools 
{
	private static uint m_objectKey = 0;
	public static uint GetUniqueObjectKey()
	{
		return m_objectKey++;
	}

}
