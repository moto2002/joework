using System;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class CUIGraphBaseScript : CUIComponent
	{
		public static readonly int s_depth = 11;

		public static readonly int s_cullingMask = LayerMask.NameToLayer("UI");

		public Color color = Color.white;

		public int cameraDepth = CUIGraphBaseScript.s_depth;

		[SerializeField]
		protected Vector3[] m_vertexs;

		private static Material s_lineMaterial = null;

		private Camera m_camera;

		protected bool vertexChanged;

		public override void Initialize(CUIFormScript formScript)
		{
			base.Initialize(formScript);
			this.m_camera = base.GetComponent<Camera>();
			if (base.get_camera() == null)
			{
				this.m_camera = base.gameObject.AddComponent<Camera>();
				base.get_camera().depth = (float)CUIGraphBaseScript.s_depth;
				base.get_camera().cullingMask = CUIGraphBaseScript.s_cullingMask;
				base.get_camera().clearFlags = CameraClearFlags.Depth;
			}
			if (CUIGraphBaseScript.s_lineMaterial == null)
			{
				CUIGraphBaseScript.s_lineMaterial = new Material("Shader \"Lines/Colored Blended\" {SubShader { Pass {     Blend SrcAlpha OneMinusSrcAlpha     ZWrite Off Cull Off Fog { Mode Off }     BindChannels {      Bind \"vertex\", vertex Bind \"color\", color }} } }");
				CUIGraphBaseScript.s_lineMaterial.hideFlags = (HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor | HideFlags.NotEditable);
				CUIGraphBaseScript.s_lineMaterial.shader.hideFlags = (HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor | HideFlags.NotEditable);
			}
		}

		protected override void OnDestroy()
		{
			this.m_camera = null;
			base.OnDestroy();
		}

		public Camera GetCamera()
		{
			return this.m_camera;
		}

		public void SetVertexs(Vector3[] vertexs)
		{
			if (vertexs == null)
			{
				return;
			}
			this.m_vertexs = new Vector3[vertexs.Length];
			for (int i = 0; i < vertexs.Length; i++)
			{
				this.m_vertexs[i] = new Vector3(vertexs[i].x, vertexs[i].y, 0f);
			}
			this.vertexChanged = true;
		}

		private void OnPostRender()
		{
			if (this.m_vertexs == null)
			{
				return;
			}
			if (CUIGraphBaseScript.s_lineMaterial == null)
			{
				return;
			}
			CUIGraphBaseScript.s_lineMaterial.SetPass(0);
			this.OnDraw();
		}

		protected virtual void OnDraw()
		{
		}
	}
}
