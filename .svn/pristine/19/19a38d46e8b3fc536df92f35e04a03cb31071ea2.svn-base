using UnityEngine;
using System.Collections;

public class MapLayer : MonoBehaviour {

	public Vector2 size;//地图的大小

	[Header("Drag Setting")]
	[SerializeField]
	private bool m_dragEnable = true;//是否可拖动
	public bool dragEnable{
		get { return m_dragEnable;}
		set{
			m_dragEnable = value;
			if(!m_dragEnable) {
				m_isDown = false;
			}
		}
	}
	public bool freezeX=false; //X方向上是否不准移动
	public bool freezeY=false; //Y方向上是否不准移动

	[Header("Scale Setting")]
	public bool multiScaleEnable = true;//是否支持多点缩放
	public float minScale=1f;//最小scale
	public float maxScale=1f;//最大scale

	[Header("Init Center Position")]
	public bool centerX;
	public float centerOffsetX = 0f;
	public bool centerY;
	public float centerOffsetY = 0f;

	private SpriteMapViewport m_viewPort;
	private Vector3 m_prevPos ;
	private Vector3 m_endPos;
	private Matrix2D m_matrix;
	private Vector3 m_initPos;
	private bool m_reset = false;
	private bool m_isAutoMoved = false;//是否在自动移动中.
	private float m_autoDamp = 5f;
	private System.Action m_movedCallback = null;
	private float m_moveDamp = 1f;
	private bool m_isDown = false;

	void Awake(){
		m_viewPort = GetComponentInParent<SpriteMapViewport>();
	}

	// Use this for initialization
	void Start () {
		this.m_endPos = base.transform.localPosition;
		this.m_matrix = new Matrix2D();
		if (this.centerX || this.centerY)
		{
			Vector2 zero = Vector2.zero;
			float offsetX = 0f;
			float offsetY = 0f;
			if (this.centerX)
			{
				zero.x = this.size.x * 0.5f;
				offsetX = this.centerOffsetX;
			}
			if (this.centerY)
			{
				zero.y = (this.m_viewPort.viewPort.height - this.size.y) * 0.5f;
				offsetY = this.centerOffsetY;
			}
			this.m_initPos = new Vector3(zero.x, zero.y, 0f);
			this.MovePointToCenter(zero, offsetX, offsetY);
			this.m_isAutoMoved = false;
			base.transform.localPosition = this.m_endPos;
		}
		this.m_initPos = base.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if (m_isAutoMoved)
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition, m_endPos, m_autoDamp*Time.deltaTime);
			if (Vector3.Distance(transform.localPosition, m_endPos) < 0.02f)
			{
				m_isAutoMoved = false;
				if(m_movedCallback!=null){
					System.Action action = m_movedCallback;
					m_movedCallback = null;
					action();
				}
			}
		}
		else
		{
			if (Input.touchCount < 2)
			{
				if (dragEnable)
				{
					if (Input.GetMouseButtonDown(0) && !InputUtil.CheckMouseOnUI())
					{
						OnTouchDown();
					}
					if (Input.GetMouseButton(0))
					{
						OnTouchMove();
					}
					if (Input.GetMouseButtonUp(0))
					{
						this.OnTouchUp();
					}
				}
			}
			else if (multiScaleEnable && !InputUtil.CheckMouseOnUI())
			{
				m_reset = true;
				Touch touch = Input.touches[0];
				Touch touch2 = Input.touches[1];
				Vector2 a = touch.deltaPosition + touch.position;
				Vector2 b = touch2.deltaPosition + touch2.position;
				float sizeDiff = Vector2.Distance(a, b) / Vector2.Distance(touch.position, touch2.position);
				sizeDiff *= sizeDiff;
				Vector3 vector3 = m_viewPort.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
				FixScaleSize(sizeDiff, vector3.x, vector3.y);
			}
			if (dragEnable && (Input.GetAxis("Mouse ScrollWheel") != 0f))
			{
				float num2 = 1f + Input.GetAxis("Mouse ScrollWheel");
				Vector3 vector4 = m_viewPort.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
				FixScaleSize(num2, vector4.x, vector4.y);
			}
			if (m_endPos.x > 0f)
			{
				m_endPos.x = 0f;
			}
			else if (m_endPos.x < ((-size.x * transform.localScale.x) + (m_viewPort.viewPort.width / transform.root.localScale.x)))
			{
				m_endPos.x = (-size.x * transform.localScale.x) + (m_viewPort.viewPort.width / transform.root.localScale.x);
			}
			if (m_endPos.y > 0f)
			{
				m_endPos.y = 0f;
			}
			else if (m_endPos.y < ((-size.y * transform.localScale.y) + (m_viewPort.viewPort.height / transform.root.localScale.y)))
			{
				m_endPos.y = (-size.y * transform.localScale.y) + (m_viewPort.viewPort.height / transform.root.localScale.y);
			}
			if (freezeY)
			{
				m_endPos.y = m_initPos.y;
			}
			if (freezeX)
			{
				m_endPos.x = m_initPos.x;
			}
			transform.localPosition = Vector3.Lerp(transform.localPosition, m_endPos, m_moveDamp);
		}
	}

	void OnTouchDown(){
		m_prevPos = transform.parent.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		m_moveDamp = 1f;
		m_reset = false;
		m_isDown = true;
	}

	void OnTouchMove(){
		if (m_isDown)
		{
			Vector3 vector = transform.parent.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			if (m_reset)
			{
				m_prevPos = vector;
			}
			else
			{
				Vector3 vector2 = vector - m_prevPos;
				m_endPos = transform.localPosition + vector2;
				m_prevPos = vector;
			}
		}
	}

	void OnTouchUp()
	{
		if (m_isDown)
		{
			Vector3 vector2 = transform.parent.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) - m_prevPos;
			m_moveDamp = 0.1f;
			m_endPos += (Vector3) (vector2 * 10f);
		}
		m_isDown = false;
	}

	void FixScaleSize(float sizeDiff,float middleX,float middleY){
		float resultSc = transform.localScale.x*sizeDiff;
		while(resultSc<minScale || resultSc>maxScale){
			sizeDiff/=sizeDiff;
			resultSc = transform.localScale.x*sizeDiff;
		}
		ScaleMap(sizeDiff,middleX,middleY);
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.green;
		Gizmos.DrawLine(new Vector3(transform.position.x,transform.position.y,0),
			new Vector3(transform.position.x,size.y*transform.localScale.y*transform.root.localScale.y+transform.position.y,0));
		
		Gizmos.DrawLine(new Vector3(transform.position.x,size.y*transform.localScale.y*transform.root.localScale.y+transform.position.y,0),
			new Vector3(transform.position.x+size.x*transform.localScale.x*transform.root.localScale.x,size.y*transform.localScale.y*transform.root.localScale.y+transform.position.y,0));
		
		Gizmos.DrawLine(new Vector3(transform.position.x+size.x*transform.localScale.x*transform.root.localScale.x,size.y*transform.localScale.y*transform.root.localScale.y+transform.position.y,0),
			new Vector3(transform.position.x+size.x*transform.localScale.x*transform.root.localScale.x,transform.position.y,0));
		
		Gizmos.DrawLine(new Vector3(transform.position.x+size.x*transform.localScale.x*transform.root.localScale.x,transform.position.y,0),
			new Vector3(transform.position.x,transform.position.y,0));
	}




	#region public function

	/// <summary>
	/// Middle坐标需要是Viewport层的局部坐标
	/// </summary>
	/// <param name="scale">要缩放的大小.</param>
	/// <param name="middleX">Middle x.</param>
	/// <param name="middleY">Middle y.</param>
	public void ScaleMap(float scale,float middleX,float middleY){
		m_matrix.Identity();
		m_matrix.Scale(transform.localScale.x,transform.localScale.y);
		m_matrix.Translate(transform.localPosition.x,transform.localPosition.y);
		m_matrix.tx -= middleX;
		m_matrix.ty -= middleY;

		m_matrix.Scale(scale,scale);

		m_matrix.tx += middleX;
		m_matrix.ty += middleY;

		transform.localScale = new Vector3(m_matrix.a,m_matrix.d,1f);
		transform.localPosition = new Vector3(m_matrix.tx,m_matrix.ty,transform.localPosition.z);

		m_endPos.x = m_matrix.tx;
		m_endPos.y = m_matrix.ty;
	}


	/// <summary>
	/// 把一个点移动到viewport中间 , point是MapLayer层的局部坐标
	/// </summary>
	/// <param name="middleX">Middle x.</param>
	/// <param name="middleY">Middle y.</param>
	public void MovePointToCenter(Vector2 point , float offsetX= 0f,float offsetY=0f){
		//viewport 的中点
		Vector2 viewportCenter = new Vector2(m_viewPort.viewPort.width*0.5f/transform.root.localScale.x,m_viewPort.viewPort.height*0.5f/transform.root.localScale.y);
		point.x = transform.localPosition.x+point.x*transform.localScale.x+offsetX;
		point.y = transform.localPosition.y+point.y*transform.localScale.y+offsetY;
		m_endPos.x = transform.localPosition.x+viewportCenter.x-point.x;
		m_endPos.y = transform.localPosition.y+viewportCenter.y-point.y;

		m_isAutoMoved = true;

		if (m_endPos.x>0) m_endPos.x=0;
		else if(m_endPos.x<-size.x*transform.localScale.x+m_viewPort.viewPort.width)
			m_endPos.x = -size.x*transform.localScale.x+m_viewPort.viewPort.width;

		if (m_endPos.y>0) m_endPos.y=0;
		else if(m_endPos.y<-size.y*transform.localScale.y+m_viewPort.viewPort.height)
			m_endPos.y = -size.y*transform.localScale.y+m_viewPort.viewPort.height;

		if(freezeY){
			m_endPos.y = m_initPos.y;
		}
		if(freezeX){
			m_endPos.x = m_initPos.x;
		}
	}

	/// <summary>
	/// 移动到某一个点, point是MapLayer层的局部坐标
	/// </summary>
	/// <param name="point">Point.</param>
	/// <param name="movedCallback">完成后的回调.</param>
	public void MoveTo(Vector2 point , float speedDamp = 5f, System.Action movedCallback=null)
	{
		m_autoDamp = speedDamp;
		m_movedCallback = movedCallback;
		point.x = transform.localPosition.x+point.x*transform.localScale.x;
		point.y = transform.localPosition.y+point.y*transform.localScale.y;
		m_endPos.x = transform.localPosition.x-point.x;
		m_endPos.y = transform.localPosition.y-point.y;

		m_isAutoMoved = true;

		if (m_endPos.x>0) m_endPos.x=0;
		else if(m_endPos.x<-size.x*transform.localScale.x+m_viewPort.viewPort.width)
			m_endPos.x = -size.x*transform.localScale.x+m_viewPort.viewPort.width;

		if (m_endPos.y>0) m_endPos.y=0;
		else if(m_endPos.y<-size.y*transform.localScale.y+m_viewPort.viewPort.height)
			m_endPos.y = -size.y*transform.localScale.y+m_viewPort.viewPort.height;

		if(freezeY){
			m_endPos.y = m_initPos.y;
		}
		if(freezeX){
			m_endPos.x = m_initPos.x;
		}

	}

	/// <summary>
	/// 停止移动
	/// </summary>
	public void StopMove(){
		m_endPos = transform.localPosition ;
	}
	#endregion
}
