using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

/// <summary>
/// 通过Render texture 实现画笔.
/// author:zhouzhanglin
/// </summary>
public class RenderTexturePainter : MonoBehaviour {

	#region enums
	public enum PaintType
	{
		Scribble,//对图片进行涂和擦除.
		DrawLine,//画线.
		DrawColorfulLine,//画彩色线，画笔贴图必须要有alpha=1的部分，否则不会显示
		None = 100
	}
	public enum RenderTexDepth{
		Depth0 = 0,
		Depth16 = 16,
		Depth24 = 24,
	}
	#endregion

	#if UNITY_EDITOR
	public Color gizmosColor = Color.red;
	#endif

	[Header("Paint Canvas Setting")]
	//画布大小
	public bool useSourceTexSize=true;
	public int canvasWidth=512;
	public int canvasHeight=512;

	//默认背景颜色
	public Color canvasColor = new Color(1,1,1,0);
	public string sortingLayer = "Default";
	public int sortingOrder = 0;

	[Header("Painter Setting")]
	public PaintType paintType = PaintType.Scribble;
	//画笔方式.
	public Texture penTex, sourceTex;

	public Shader paintShader,scribbleShader;

	//纯色方式.
	public Color penColor=new Color(1, 0, 0, 1);

	//笔刷缩放值
	[Range(0.1f,5f)]
	public float brushScale = 1f;

	//插值，越大性能越好，但是大了可能出现断层现象
	[Range(0.01f,0.1f)]
	public float drawLerpDamp = 0.02f;

	//是否为擦除.
	public bool isEraser = false;


	[Header(" Colorfull paint Setting")]
	//彩色方式
	public Color[] paintColorful ;

	//速度变化频率，越大变化越慢
	public float colorChangeRate = 1f;
	private int m_colorfulIndex = 1;
	private float m_colorfulTime = 0f;


	[Header("Auto Setting")]
	//是否自动初始化.
	public bool isAutoInit = false;
	public bool isAutoDestroy = true;
	public bool isShowSource = false;



	private RenderTexture m_rt;
	public RenderTexture renderTexture{ get{ return m_rt; } }

	private bool m_inited = false;
	private bool m_isDown = false;
	private Vector3 m_prevMousePosition;
	private Material m_penMat,m_canvasMat;
	private Vector2 m_sourceTexScale;

	public bool isInited{ get { return m_inited; } }
	public Material penMat{ get{ return m_penMat; } }
	public Material canvasMat{ get{ return m_canvasMat; } }

	private Rect m_uv = new Rect(0f,0f,1f,1f);

	// Use this for initialization
	void Start () {
		if (isAutoInit) {
			Init();
		}
	}

	public void Init()
	{
		if(!m_inited){
			m_inited = true;

			if(useSourceTexSize&&sourceTex){
				canvasWidth = sourceTex.width;
				canvasHeight = sourceTex.height;
			}

			m_rt = new RenderTexture(canvasWidth,canvasHeight,0,RenderTextureFormat.ARGB32);
			m_rt.useMipMap = false;

			//canvas
			if(paintType== PaintType.Scribble){
				m_canvasMat = CreateMat(scribbleShader,canvasColor,BlendMode.SrcAlpha,BlendMode.OneMinusSrcAlpha,1,0.02f);
				CreateQuad(m_canvasMat);
				m_canvasMat.SetTexture("_SourceTex",sourceTex);
				m_canvasMat.SetTexture("_RenderTex",m_rt);
			}
			else if(paintType== PaintType.DrawLine || paintType== PaintType.DrawColorfulLine){
				m_canvasMat = CreateMat(paintShader,canvasColor,BlendMode.One,BlendMode.OneMinusSrcAlpha,1,0.02f);
				CreateQuad(m_canvasMat);
				m_canvasMat.mainTexture = m_rt;
			}
			else
			{
				m_canvasMat = CreateMat(paintShader,canvasColor,BlendMode.SrcAlpha,BlendMode.OneMinusSrcAlpha,1,0.02f);
				CreateQuad(m_canvasMat);
				m_canvasMat.mainTexture = m_rt;
			}

			if(isEraser)
			{
				canvasColor.a = 1f;
				m_penMat = CreateMat(paintShader,penColor,BlendMode.Zero,BlendMode.OneMinusSrcAlpha,1,0.01f);
			}
			else
			{
				if(paintType== PaintType.Scribble){
					canvasColor.a = 0f;
					m_penMat = CreateMat(paintShader,penColor,BlendMode.SrcAlpha,BlendMode.One,1,0.01f);

				}else if(paintType== PaintType.DrawLine || paintType== PaintType.DrawColorfulLine){
					m_penMat = CreateMat(paintShader,penColor,BlendMode.One,BlendMode.OneMinusSrcAlpha,penColor.a,0.01f);
					m_penMat.SetFloat("_Cutoff",1f);
				}else if(paintType== PaintType.None){
					m_penMat = CreateMat(paintShader,penColor,BlendMode.SrcAlpha,BlendMode.OneMinusSrcAlpha,1,0.01f);
				}
			}

			if(isShowSource){
				if(m_rt && sourceTex){
					Graphics.SetRenderTarget (m_rt);
					Graphics.Blit(sourceTex,m_rt);
					RenderTexture.active = null;
				}
			}else{
				ResetCanvas();
			}

		}
	}

	/// <summary>
	/// 设置是否为擦除
	/// </summary>
	/// <param name="value">If set to <c>true</c> value.</param>
	public void SetIsEraser( bool value){
		if(isEraser!=value){
			isEraser = value;
			m_penMat.SetFloat("_Cutoff",0f);
			if(isEraser){
				m_penMat.SetFloat("_BlendSrc",(int)BlendMode.Zero);
				m_penMat.SetFloat("_BlendDst",(int)BlendMode.OneMinusSrcAlpha);
			}else{
				m_penMat.SetFloat("_BlendSrc",(int)BlendMode.SrcAlpha);
				if(paintType== PaintType.DrawLine || paintType== PaintType.DrawColorfulLine){
					m_penMat.SetFloat("_Cutoff",1f);
					m_penMat.SetFloat("_BlendDst",(int)BlendMode.OneMinusSrcAlpha);
				}
				else if(paintType== PaintType.None){
					m_penMat.SetFloat("_BlendDst",(int)BlendMode.SrcAlpha);
				}
				else {
					m_penMat.SetFloat("_BlendDst",(int)BlendMode.One);
				}
			}
		}
	}

	/// <summary>
	/// 设置画笔颜色
	/// </summary>
	/// <param name="c">C.</param>
	public void SetPenColor( Color c){
		penColor = c;
		if(paintType== PaintType.DrawLine){
			m_canvasMat.color=penColor;
		}
	}

	/// <summary>
	/// 设置画布颜色
	/// </summary>
	/// <param name="c">C.</param>
	public void SetCanvasColor(Color c){
		canvasColor = c;
		m_canvasMat.color = canvasColor;
	}

	/// <summary>
	/// Sets the canvas alpha. 0-1
	/// </summary>
	/// <param name="alpha">Alpha.</param>
	public void SetCanvasAlpha(float alpha){
		canvasColor.a = alpha;
		m_canvasMat.SetFloat("_Alpha",alpha);
	}
	public float GetCanvasAlpha(){
		return m_canvasMat.GetFloat("_Alpha");
	}

	/// <summary>
	/// 重新设置原图
	/// </summary>
	/// <param name="sourceTex">Source tex.</param>
	public void SetSourceTexture( Texture2D sourceTex){
		this.sourceTex = sourceTex;
		if(m_canvasMat){
			m_canvasMat.SetTexture("_SourceTex",sourceTex);
		}
	}

	/// <summary>
	///  Draw一次，用于点击draw
	/// </summary>
	/// <param name="screenPos">Screen position. 屏幕坐标</param>
	/// <param name="camera">Camera. 为空时是Camera.main</param>
	/// <param name="pen">Pen. 为null时是默认的画笔贴图</param>
	public void ClickDraw(Vector3 screenPos , Camera camera=null , Texture pen=null , Material drawMat = null , RenderTexture rt=null){
		if (camera == null) camera = Camera.main;
		if(pen==null) pen = penTex;
		if(drawMat==null) drawMat = m_penMat;
		if(rt==null) rt = m_rt;
		Vector3 uvPos= SpriteHitPoint2UV(camera.ScreenToWorldPoint(screenPos));

		if(m_uv.Contains(uvPos))
		{
			screenPos = new Vector3(uvPos.x * canvasWidth, canvasHeight - uvPos.y * canvasHeight,0f);
			float w = pen.width;
			float h = pen.height;
			Rect rect = new Rect((screenPos.x-w*0.5f),(screenPos.y-h*0.5f),w,h);
			m_uv.width=canvasWidth;
			m_uv.height=canvasHeight;
			if(Intersect(ref rect,ref m_uv))
			{
				GL.PushMatrix();
				GL.LoadPixelMatrix(0, canvasWidth, canvasHeight, 0);
				RenderTexture.active = rt;
				Graphics.DrawTexture(rect,pen,drawMat);
				RenderTexture.active = null;
				GL.PopMatrix();
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
		Vector3 uvPos= SpriteHitPoint2UV(camera.ScreenToWorldPoint(screenPos));
		screenPos = new Vector3(uvPos.x * canvasWidth, canvasHeight - uvPos.y * canvasHeight,0f);
		if(!m_isDown){
			m_isDown = true;
			m_prevMousePosition = screenPos;
		}

		if(m_isDown){
			if(paintType== PaintType.DrawColorfulLine){
				Color currC = paintColorful[m_colorfulIndex];
				penColor = Color.Lerp(penColor,currC,Time.deltaTime*colorChangeRate);
				m_colorfulTime+=Time.deltaTime*colorChangeRate;
				if(m_colorfulTime>1f){
					m_colorfulTime =0f;
					++m_colorfulIndex;
					if(m_colorfulIndex>=paintColorful.Length){
						m_colorfulIndex = 0;
					}
				}
				m_penMat.color=penColor;
			}
			else if(paintType== PaintType.DrawLine){
				m_penMat.color=penColor;
			}
			GL.PushMatrix();
			GL.LoadPixelMatrix(0, canvasWidth, canvasHeight, 0);
			RenderTexture.active = m_rt;
			LerpDraw(ref screenPos,ref m_prevMousePosition);
			RenderTexture.active = null;
			GL.PopMatrix();
			m_prevMousePosition = screenPos;
		}
	}

	/// <summary>
	/// 结束draw
	/// </summary>
	public void EndDraw(){
		m_isDown = false;
	}

	/// <summary>
	/// 重置画布，如果是scribble擦除，会显示原图
	/// </summary>
	public void ResetCanvas()
	{
		if(m_rt){
			Graphics.SetRenderTarget (m_rt);
			Color c = canvasColor ;
			if(isEraser){
				c.a = 1f;
				GL.Clear(true,true,c);
			}else{
				c.a = 0f;
				if(paintType== PaintType.DrawLine||paintType== PaintType.DrawColorfulLine){
					c = new Color(0,0,0,0);
				}
				GL.Clear(true,true,c);
			}
			RenderTexture.active = null;
		}
	}

	/// <summary>
	/// 显示画完成状态，如果是涂抹，则显示原图，如果是擦除，设置清除画布
	/// </summary>
	public void ShowScribbleComplete(){
		if(paintType== PaintType.Scribble)
		{
			if(isEraser)
			{
				Graphics.SetRenderTarget (m_rt);
				Color c = canvasColor ;
				c.a = 0f;
				GL.Clear(true,true,c);
			}
			else
			{
				if(sourceTex){
					RenderTexture.active = m_rt;
					Graphics.Blit(sourceTex,m_rt);
					RenderTexture.active = null;
				}
			}
		}

	}

	public void Dispose(){
		m_inited = false;
		if(m_rt){
			ResetCanvas();
			RenderTexture.active = null;
			m_rt.Release();
			m_rt = null;
		}
		MeshFilter meshFilter= gameObject.GetComponent<MeshFilter>();
		if(meshFilter!=null) Destroy(meshFilter);

		MeshRenderer rend = gameObject.GetComponent<MeshRenderer>();
		if(rend!=null) Destroy(rend);

		if(m_canvasMat){
			Destroy(m_canvasMat);
		}
		if(m_penMat){
			Destroy(m_penMat);
		}
	}





	#region private function


	/// <summary>
	/// 创建材质
	/// </summary>
	/// <returns>The mat.</returns>
	/// <param name="shader">Shader.</param>
	/// <param name="c">C.</param>
	/// <param name="src">Source.</param>
	/// <param name="dst">Dst.</param>
	/// <param name="alpha">Alpha.</param>
	/// <param name="cutoff">Cutoff.</param>
	public Material CreateMat(Shader shader ,Color c, BlendMode src , BlendMode dst , float alpha=1f,float cutoff=0f){
		Material m = new Material(shader);
		m.SetFloat("_BlendSrc",(int)src);
		m.SetFloat("_BlendDst",(int)dst);
		m.SetColor("_Color",c);
		m.SetFloat("_Cutoff",cutoff);
		m.SetFloat("_Alpha",alpha);
		return m;
	}


	/// <summary>
	/// Sprite中,hitPoint转uv坐标。hitPoint为世界坐标
	/// </summary>
	/// <returns>The hit point2 U.</returns>
	/// <param name="hitPoint">Hit point.</param>
	Vector2 SpriteHitPoint2UV( Vector3 hitPoint){
		Vector3 localPos=transform.InverseTransformPoint(hitPoint);
		localPos*=100f;
		localPos.x += canvasWidth*0.5f;
		localPos.y += canvasHeight*0.5f;
		return new Vector2(localPos.x/canvasWidth,localPos.y/canvasHeight);
	}

	void LerpDraw(ref Vector3 current ,ref Vector3 prev){
		float distance = Vector2.Distance(current, prev);
		if(distance>0f){
			Vector2 pos;
			float w = penTex.width*brushScale;
			float h = penTex.height*brushScale;
			float lerpDamp = Mathf.Min(w,h)*drawLerpDamp;
			m_uv.width = canvasWidth;
			m_uv.height = canvasHeight;
			for (float i = 0; i < distance; i += lerpDamp)
			{
				float lDelta = i / distance;
				float lDifx = current.x - prev.x;
				float lDify = current.y - prev.y;
				pos.x = prev.x + (lDifx * lDelta);
				pos.y = prev.y + (lDify * lDelta);
				Rect rect = new Rect(pos.x-w*0.5f,pos.y-h*0.5f,w,h);
				if(Intersect(ref m_uv,ref rect))
				{
					Graphics.DrawTexture(rect,penTex,m_penMat);
				}
			}
		}
	}

	void OnDestroy(){
		if(isAutoDestroy){
			Dispose();
		}
	}

	bool Intersect(ref Rect a,ref Rect b ) {
		bool c1 = a.xMin < b.xMax;
		bool c2 = a.xMax > b.xMin;
		bool c3 = a.yMin < b.yMax;
		bool c4 = a.yMax > b.yMin;
		return c1 && c2 && c3 && c4;
	}

	void CreateQuad( Material mat){
		Mesh m = new Mesh();
		m.vertices = new Vector3[]{
			new Vector3(canvasWidth*0.005f ,canvasHeight*0.005f ),
			new Vector3(canvasWidth*0.005f ,-canvasHeight*0.005f ),
			new Vector3(-canvasWidth*0.005f ,-canvasHeight*0.005f),
			new Vector3(-canvasWidth*0.005f ,canvasHeight*0.005f )
		};
		m.uv=new Vector2[]{
			new Vector2(1,1),
			new Vector2(1,0),
			new Vector2(0,0),
			new Vector2(0,1)
		};
		m.triangles=new int[]{0,1,2,2,3,0};
		m.RecalculateBounds();
		m.RecalculateNormals();

		MeshFilter meshFilter= gameObject.GetComponent<MeshFilter>();
		if(meshFilter==null) meshFilter = gameObject.AddComponent<MeshFilter>();
		meshFilter.mesh = m;

		MeshRenderer rend = gameObject.GetComponent<MeshRenderer>();
		if(rend==null) rend = gameObject.AddComponent<MeshRenderer>();
		rend.material = mat;
		rend.sortingLayerName=sortingLayer;
		rend.sortingOrder = sortingOrder;
	}



	#if UNITY_EDITOR
	void OnDrawGizmos(){
		Gizmos.color = gizmosColor;
		Matrix4x4 cubeTransform = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
		Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
		Gizmos.matrix *= cubeTransform;
		Gizmos.DrawWireCube(Vector3.zero,new Vector3(canvasWidth*0.01f,canvasHeight*0.01f,0.1f));
		Gizmos.matrix = oldGizmosMatrix;
	}
	#endif

	#endregion

}
