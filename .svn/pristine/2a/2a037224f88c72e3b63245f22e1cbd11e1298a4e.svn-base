using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {


    public Texture2D drawImage;
	public bool isSprite = false;

    private Painter _paint;
    private bool _selectedEraser;
    private bool _selectedDrawImg;
	private bool _selectdPenAlpha;
	private bool _isDrawLine;
	private bool _isDrawColorfulLine;
	private bool _isYellow;

	private 
	// Use this for initialization
	void Start () {
		_paint = GetComponent<Painter> ();
		Application.targetFrameRate = 60;
	}
	
	void OnGUI()
    {
		_selectedDrawImg = GUI.Toggle(new Rect(0, 0, 100, 30), _selectedDrawImg, "Draw Img?");

		_selectedEraser = GUI.Toggle(new Rect(0, 30, 100, 30), _selectedEraser, "Eraser?");
		_paint.isEraser = _selectedEraser;
        if (!_selectedDrawImg)
        {
			_selectdPenAlpha = GUI.Toggle(new Rect(85, 30, 100, 30), _selectdPenAlpha, "Pen Alpha?");
			_paint.penAlphaEnable = _selectdPenAlpha;

			_isDrawLine = GUI.Toggle(new Rect(85, 0, 100, 30), _isDrawLine, "Is Draw Line");
			if(_isDrawLine){

				_isDrawColorfulLine = GUI.Toggle(new Rect(190, 0, 100, 30), _isDrawColorfulLine, "colorful?");
				if(_isDrawColorfulLine){
					_paint.paintType= Painter.PaintType.DrawColorfulLine;
				}else{
					_paint.paintType= Painter.PaintType.DrawLine;

					_isYellow = GUI.Toggle(new Rect(260, 0, 100, 30), _isYellow, "Yellow");
					if(_isYellow){
						_paint.paintColor = new Color32(0xff,0xcc,0,0xff);
					}
					else
					{
						_paint.paintColor =new Color32(0xff, 0, 0, 0xff);
					}
				}

			}else{
				_paint.paintType= Painter.PaintType.Scribble;
			}
        }
	}

 
    //test draw img
    void Update()
    {
        if (_selectedDrawImg)
        {
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
					if(isSprite){
						Vector2 point = _paint.SpriteHitPoint2UV(hit.point);
						_paint.DrawTexture(drawImage, point.x, point.y);
					}
					else
					{
						Vector2 point = hit.textureCoord;
						_paint.DrawTexture(drawImage, point.x, point.y);
					}
                }
            }
        }
    }

}
