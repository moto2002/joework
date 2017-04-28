using System;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class CUIGraphLineScript : CUIGraphBaseScript
	{
		public float thickness = 1f;

		public float drawSpeed;

		private float _curPathLen;

		private float _fixPathLen;

		private float _lastDrawTime;

		private int _drawPathIndex;

		public override void Initialize(CUIFormScript formScript)
		{
			base.Initialize(formScript);
		}

		protected override void OnDraw()
		{
			if (this.vertexChanged)
			{
				this.vertexChanged = false;
				this._curPathLen = 0f;
				this._fixPathLen = 0f;
				this._drawPathIndex = ((this.drawSpeed <= 0f) ? this.m_vertexs.Length : 1);
				this._lastDrawTime = Time.time;
			}
			GL.PushMatrix();
			GL.LoadPixelMatrix();
			GL.Begin((this.thickness > 1f) ? 7 : 1);
			GL.Color(this.color);
			for (int i = 1; i < this._drawPathIndex; i++)
			{
				CUIGraphLineScript.GLLine(ref this.m_vertexs[i - 1], ref this.m_vertexs[i], this.thickness);
			}
			if (this._drawPathIndex < this.m_vertexs.Length)
			{
				float num = (Time.time - this._lastDrawTime) * this.drawSpeed;
				this._lastDrawTime = Time.time;
				this._curPathLen += num;
				while (this._drawPathIndex < this.m_vertexs.Length)
				{
					Vector3 b = this.m_vertexs[this._drawPathIndex - 1];
					Vector3 a = this.m_vertexs[this._drawPathIndex];
					float magnitude = (a - b).magnitude;
					if (this._fixPathLen + magnitude > this._curPathLen)
					{
						break;
					}
					this._fixPathLen += magnitude;
					this._drawPathIndex++;
					CUIGraphLineScript.GLLine(ref b, ref a, this.thickness);
				}
				if (this._drawPathIndex < this.m_vertexs.Length)
				{
					Vector3 vector = this.m_vertexs[this._drawPathIndex - 1];
					Vector3 a2 = this.m_vertexs[this._drawPathIndex];
					float d = this._curPathLen - this._fixPathLen;
					Vector3 vector2 = vector + (a2 - vector).normalized * d;
					CUIGraphLineScript.GLLine(ref vector, ref vector2, this.thickness);
				}
			}
			GL.End();
			GL.PopMatrix();
		}

		private static void GLLine(ref Vector3 start, ref Vector3 end, float thickness = 1f)
		{
			if (thickness <= 1f)
			{
				GL.Vertex3(start.x, start.y, start.z);
				GL.Vertex3(end.x, end.y, end.z);
			}
			else
			{
				Vector3 b = end - start;
				Vector3 normalized = b.normalized;
				Vector3 a = new Vector3(normalized.y, -normalized.x);
				Vector3 a2 = start + a * thickness * 0.5f;
				Vector3 a3 = a2 + b;
				Vector3 a4 = a3 - a * thickness;
				Vector3 vector = a4 - b;
				GL.Vertex3(a2.x, a2.y, a2.z);
				GL.Vertex3(a3.x, a3.y, a3.z);
				GL.Vertex3(a4.x, a4.y, a4.z);
				GL.Vertex3(vector.x, vector.y, vector.z);
			}
		}
	}
}
