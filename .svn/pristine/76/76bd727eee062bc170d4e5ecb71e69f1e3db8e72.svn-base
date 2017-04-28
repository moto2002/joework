using UnityEngine;
using System.Collections;

/// <summary>
/// 用来测试RenderTexturePainter和PaintCompleteChecker
/// </summary>
public class RenderPainterController : MonoBehaviour {

	public RenderTexturePainter painter;
	private bool m_isEraser = false;
	private float m_alpha = 1f;
	private bool m_clickDraw = false;

	public Texture[] penTexs;
	private int m_penTexIndex;

	public PaintCompleteChecker checker;
	public bool enableCheckComplete=false;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate=60;
		if(painter) 
			m_isEraser = painter.isEraser;
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetMouseButtonDown(0)){
			if(painter&&m_clickDraw) {
				painter.ClickDraw(Input.mousePosition);
				if(enableCheckComplete && checker)
					checker.ClickDraw(Input.mousePosition);
			}
		}
		if(Input.GetMouseButton(0)){
			if(painter) {
				if(!m_clickDraw)
					painter.Drawing(Input.mousePosition);
			}
			if(enableCheckComplete && checker)
				checker.Drawing(Input.mousePosition);
		}
		if(Input.GetMouseButtonUp(0)){
			if(painter) 
				painter.EndDraw();
			if(enableCheckComplete && checker)
				checker.EndDraw();
		}
	}


	void OnGUI(){
		if(painter)
		{
			if(GUI.Button(new Rect(10,10,100,40),"Reset Canvas")){
				painter.ResetCanvas();
				if(enableCheckComplete && checker ){
					checker.Reset();
				}
			}
			m_isEraser = GUI.Toggle(new Rect(120,10,100,40),m_isEraser,"Is Earse","Button");
			if(m_isEraser!=painter.isEraser){
				painter.SetIsEraser(m_isEraser);
			}
			m_clickDraw = GUI.Toggle(new Rect(240,10,100,40),m_clickDraw,"Click Draw","Button");

			GUI.color = Color.white;
			GUI.Label( new Rect(10, 60, 200, 40) ,"Brush Scale :"+painter.brushScale.ToString("N2"));
			painter.brushScale = GUI.HorizontalSlider(new Rect(10, 80, 200, 40), painter.brushScale , 0.1F, 5F);

			GUI.color = Color.white;
			GUI.Label( new Rect(10, 100, 200, 40) ,"Canvas Alpha :"+m_alpha.ToString("N2"));
			m_alpha = GUI.HorizontalSlider(new Rect(10, 120, 200, 40), m_alpha , 0F, 1F);
			painter.SetCanvasAlpha(m_alpha);

			if(penTexs.Length>1 && GUI.Button(new Rect(220,60,150,40),"Change PenTexture")){
				++m_penTexIndex;
				if(m_penTexIndex>=penTexs.Length) m_penTexIndex = 0;
				painter.penTex = penTexs[m_penTexIndex];
			}

			enableCheckComplete = GUI.Toggle(new Rect(220,110,150,40),enableCheckComplete,"Check Progress","Button");


			if(enableCheckComplete && checker ){
				GUI.Label( new Rect(10,140,200,40),"Progress:"+checker.Progress.ToString("N2"));
			}
		}
	}
}
