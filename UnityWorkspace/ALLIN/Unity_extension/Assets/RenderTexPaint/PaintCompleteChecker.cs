using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 涂抹完成判断
/// author:zhouzhanglin
/// </summary>
[RequireComponent(typeof(RenderTexturePainter))]
public class PaintCompleteChecker : MonoBehaviour {

	#if UNITY_EDITOR
	//网格初始值
	public bool gridDefaultStatus=false;

	//笔刷大小
	[Range(0.1f,1f)]
	public float brushSize = 0.2f;

	public Color enableColor = new Color(0f,0f,1f,0.4f);
	public Color disableColor = new Color(1f,0.92f,0.016f,0.1f);

	public Dictionary<string,Rect> gridsDic;
	public Dictionary<string,bool> enablesDic;
	#endif


	[Header("Check Data File")]
	//拖文件到这上面
	public ScribbleCheckData checkData;

	//是否可以reset数据，如果为否，则直接操作原数据，为true就会产生一个副本
	public bool canResetData = false;

	private RenderTexturePainter m_painter;
	public RenderTexturePainter painter{
		get { return m_painter; }
	}


	private bool m_isDown;
	private Vector3 m_prevMousePosition;
	private Vector2 m_lerpSize ;
	private int m_totalCount;
	private List<Vector2> m_checkPoints;
	private List<Vector2> m_removePoints;

	/// <summary>
	/// 完成的进度 0-1
	/// </summary>
	/// <value>The progress.</value>
	public float Progress{
		get {return 1f-(float)m_checkPoints.Count/m_totalCount; }
	}

	void Start(){
		m_painter = GetComponent<RenderTexturePainter>();
		#if UNITY_EDITOR
		canResetData = true;
		#endif
		Reset();
	}

	/// <summary>
	/// Reset
	/// </summary>
	public void Reset(){
		if(checkData!=null){
			int count = checkData.checkPoints.Count;
			m_totalCount = count;

			if(canResetData)
				m_checkPoints = new List<Vector2>();
			else 
				m_checkPoints = checkData.checkPoints;
			m_removePoints = new List<Vector2>();

			#if UNITY_EDITOR
			gridsDic = new Dictionary<string,Rect>();
			enablesDic = new Dictionary<string,bool>();
			#endif

			for(int i=0;i<count;++i){
				Vector2 v = checkData.checkPoints[i];

				if(canResetData)
					m_checkPoints.Add(v);

				#if UNITY_EDITOR
				Rect rect = new Rect( v.x-checkData.gridSize.x*0.005f,v.y-checkData.gridSize.y*0.005f, checkData.gridSize.x*0.01f,checkData.gridSize.y*0.01f);
				string key = v.x+"-"+v.y;
				gridsDic[key]=rect;
				enablesDic[key] = true;
				#endif
			}
		}
		if(m_painter && m_painter.penTex){
			float w = m_painter.penTex.width*m_painter.brushScale*0.005f;
			float h = m_painter.penTex.height*m_painter.brushScale*0.005f;
			m_lerpSize = new Vector2(w,h); 
		}
	}

	public void ClickDraw(Vector3 screenPos , Camera camera=null , bool isReverse=false){
		if (camera == null) camera = Camera.main;
		Vector3 localPos= transform.InverseTransformPoint(camera.ScreenToWorldPoint(screenPos));

		float w = m_lerpSize.x;
		float h = m_lerpSize.y;
		float lerpDamp = Mathf.Min(w,h);
		Rect brushSize = new Rect((localPos.x-w*0.5f),(localPos.y-h*0.5f),w,h);

		if(isReverse)
		{
			for(int i=0;i<m_removePoints.Count;++i){
				Vector2 point = m_removePoints[i];
				if(Vector2.Distance(point,brushSize.center)<lerpDamp*0.75f){
					m_checkPoints.Add(point);
					m_removePoints.RemoveAt(i);
					--i;

					#if UNITY_EDITOR
					string key = point.x+"-"+point.y;
					Rect rect = new Rect( point.x-checkData.gridSize.x*0.005f,point.y-checkData.gridSize.y*0.005f, checkData.gridSize.x*0.01f,checkData.gridSize.y*0.01f);
					gridsDic[key]=rect;
					enablesDic[key] = true;
					#endif
				}
			}
		}
		else
		{
			for(int i=0;i<m_checkPoints.Count;++i){
				Vector2 point = m_checkPoints[i];
				if(Vector2.Distance(point,brushSize.center)<lerpDamp*0.75f){
					m_removePoints.Add(point);
					m_checkPoints.RemoveAt(i);
					--i;

					#if UNITY_EDITOR
					string key = point.x+"-"+point.y;
					gridsDic.Remove(key);
					enablesDic.Remove(key);
					#endif
				}
			}
		}

	}

	/// <summary>
	/// 移动动时draw
	/// </summary>
	/// <param name="screenPos">Screen position.</param>
	/// <param name="camera">Camera.</param>
	public void Drawing(Vector3 screenPos , Camera camera=null){
		if (camera == null) camera = Camera.main;
		Vector3 localPos= transform.InverseTransformPoint(camera.ScreenToWorldPoint(screenPos));

		if(!m_isDown){
			m_isDown = true;
			m_prevMousePosition = localPos;
		}

		if(m_isDown){
			LerpDraw(localPos,m_prevMousePosition);
			m_prevMousePosition = localPos;
		}
	}

	/// <summary>
	/// 结束draw
	/// </summary>
	public void EndDraw(){
		m_isDown = false;
	}

	void LerpDraw(Vector3 current , Vector3 prev){
		float distance = Vector2.Distance(current, prev);
		if(distance>0f){
			float w = m_lerpSize.x;
			float h = m_lerpSize.y;
	
			float lerpDamp = Mathf.Min(w,h);
			Vector2 pos;
			for (float i = 0; i < distance; i += lerpDamp)
			{
				float lDelta = i / distance;
				float lDifx = current.x - prev.x;
				float lDify = current.y - prev.y;
				pos.x = prev.x + (lDifx * lDelta);
				pos.y = prev.y + (lDify * lDelta);

				Rect brushSize = new Rect((pos.x-w*0.5f),(pos.y-h*0.5f),w,h);
				for(int j=0;j<m_checkPoints.Count;++j){
					Vector2 point = m_checkPoints[j];
					if(Vector2.Distance(point,brushSize.center)<lerpDamp*0.75f){
						m_checkPoints.RemoveAt(j);
						--j;

						#if UNITY_EDITOR
						string key = point.x+"-"+point.y;
						gridsDic.Remove(key);
						enablesDic.Remove(key);
						#endif
					}
				}
			}
		}
	}

	#if UNITY_EDITOR
	void OnDrawGizmos(){
		if(gridsDic!=null && enablesDic!=null){

			Matrix4x4 cubeTransform = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
			Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
			Gizmos.matrix *= cubeTransform;

			foreach(string key in gridsDic.Keys)
			{
				Rect rect = gridsDic[key];
				if(enablesDic[key]){
					Gizmos.color = enableColor;
				}
				else{
					Gizmos.color = disableColor;
				}
				Vector3 center = new Vector3(rect.x+rect.width*0.5f,rect.y+rect.height*0.5f);
				Vector3 size = new Vector3(rect.width,rect.height,0.1f);

				Gizmos.DrawWireCube(center,size);
			}

			Gizmos.matrix = oldGizmosMatrix;
		}
	}
	#endif
}
