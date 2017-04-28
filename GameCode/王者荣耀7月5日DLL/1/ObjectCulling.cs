using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class ObjectCulling : MonoBehaviour
{
	public GameObject obj;

	public Vector3 bound = new Vector3(1f, 1f, 1f);

	public void Awake()
	{
		this.InitBound();
	}

	public void Init(GameObject chld)
	{
		this.obj = chld;
		this.InitBound();
	}

	private void InitBound()
	{
		MeshFilter component = base.GetComponent<MeshFilter>();
		component.mesh = new Mesh();
		component.mesh.bounds = new Bounds(Vector3.zero, this.bound);
	}

	public void OnBecameVisible()
	{
		if (!this.obj.activeSelf)
		{
			this.obj.SetActive(true);
		}
	}

	public void OnBecameInvisible()
	{
		if (this.obj.activeSelf)
		{
			this.obj.SetActive(false);
		}
	}
}
