using System;
using UnityEngine;

public class Canvas3D : MonoBehaviour
{
	public void Awake()
	{
		this.Reset();
	}

	public void Reset()
	{
		Singleton<Canvas3DImpl>.GetInstance().Reset();
	}

	public void LateUpdate()
	{
		Singleton<Canvas3DImpl>.GetInstance().Update(base.transform);
	}
}
