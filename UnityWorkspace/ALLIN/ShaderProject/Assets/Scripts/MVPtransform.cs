using UnityEngine;
using System.Collections;

public class MVPtransform : MonoBehaviour 
{
	Renderer m_renderer;
	Matrix4x4 m_modelMatrix = Matrix4x4.identity;
	Matrix4x4 m_viewMatrix = Matrix4x4.identity;
	Matrix4x4 m_projectionMatrix = Matrix4x4.identity;

	void Start () 
	{
		this.m_renderer = this.GetComponent<Renderer>();
	}
	
	void Update () 
	{
		Matrix4x4 rm = GetRotateMatrix();
		this.m_renderer.material.SetMatrix("_RotateMatrix",rm);

		Matrix4x4 sm = GetScaleMatrix();
		this.m_renderer.material.SetMatrix("_ScaleMatrix",sm);
	}

	Matrix4x4 GetMyMvpMatrix(Transform trans)
	{
		Matrix4x4 mvp;
		m_modelMatrix = trans.localToWorldMatrix;
		m_viewMatrix = Camera.main.worldToCameraMatrix;
		m_projectionMatrix = Camera.main.projectionMatrix;

		//矩阵相乘有顺序，pvm的顺序可能和unity有关
		//更多的情况下，我们用Unity提供的	UNITY_MATRIX_MVP
		//mvp= m_modelMatrix * m_viewMatrix * m_projectionMatrix;
		mvp= m_projectionMatrix *  m_viewMatrix * m_modelMatrix ;

		return mvp;
	}

	//用一个绕y轴的旋转测试一下
	Matrix4x4 GetRotateMatrix()
	{
		Matrix4x4 rm = new Matrix4x4();
		rm[0,0] = Mathf.Cos(Time.realtimeSinceStartup);
		rm[0,2] = Mathf.Sin(Time.realtimeSinceStartup);
		rm[1,1] = 1;
		rm[2,0] = -Mathf.Sin(Time.realtimeSinceStartup);
		rm[2,2] = Mathf.Cos(Time.realtimeSinceStartup);
		rm.m33 = 1;
		return rm;
	}

	Matrix4x4 GetScaleMatrix()
	{
		Matrix4x4 scaleMatrix = new Matrix4x4();
		scaleMatrix.m00=Mathf.Sin(Time.realtimeSinceStartup);
		scaleMatrix.m11 = Mathf.Cos(Time.realtimeSinceStartup);
		scaleMatrix.m22 = Mathf.Sin(Time.realtimeSinceStartup);
		scaleMatrix.m33 = 1;
		return scaleMatrix;
	}
}
