using UnityEngine;
using System.Collections;

/// <summary>
/// 需要添加3D Collider
/// </summary>
public class Painter : MonoBehaviour {

	public enum PaintType
	{
		Scribble,//对图片进行涂和擦除.
		DrawLine,//画线.
		DrawColorfulLine,//画彩色线
	}

	public enum BrushType{
		CustomBrush, //自定义笔刷方式，通过贴图.
		SolidCircle //纯色方式.
	}

	public BrushType brushType = BrushType.SolidCircle;
	public PaintType paintType = PaintType.Scribble;

	//画笔方式.
	public Texture2D pen , source ;
	//是否支持画笔的透明透明区域.只作用于BrushType为CustomBrush
	public bool penAlphaEnable = true;

	//线的插值，越小线越平滑，不过性能消耗越大.
	public float lerpDamp = 0.01f;
	//是否为擦除.
	public bool isEraser = false;

	//纯色方式.
	public Color32 paintColor=new Color32(0xff, 0, 0, 0xff);

	//彩色方式
	public Color32[] paintColorful ;
	//速度变化频率，越大变化越慢
	public float colorChangeRate = 1f;
	private int m_colorfulIndex = 1;
	private float m_colorfulTime = 0f;

	//如果笔刷类型为CustomBrush，则这个值为笔刷的大小.
	public int brushSize = 16;

	//是否自动初始化.
	public bool isAutoInit = false;

	//init show picture
	public bool initShowPic = false;

	#region private
	private Texture2D _baseTexture ;
	private bool _isDown = false;
	private Vector2 _prevMousePosition;
	private Color32[] _penColors;
	private byte[] _pixels;
	private Color32[] _sourceColors;
	private float _delayDrawTime = 0.04f;
	private float _delayApply = 0f;

	private float _minXPen,_maxXPen;
	private float _minYPen,_maxYPen;
	private float _sourceWidth,_sourceHeight;
	#endregion


	void Start(){
		if (isAutoInit) {
			Init(initShowPic);
		}
	}

	#region public functions
	
	/// <summary>
	/// 设置完pen和source后调用此方法.
	/// </summary>
	public void Init(bool isShowPicture=false)
	{
		if (pen && brushType == BrushType.CustomBrush) {
			brushSize = pen.width;
            _minXPen = 0;
            _maxXPen = source.width - 1;
            _minYPen = -brushSize / 2;
            _maxYPen = source.height - 1;
		}

		if(paintType== PaintType.DrawColorfulLine && paintColorful.Length>0){
			paintColor = paintColorful[0];
		}

		if (_baseTexture != null) {
			Destroy(_baseTexture);
			_baseTexture = null;
		}
		
		if (pen && brushType == BrushType.CustomBrush) {
			_penColors = pen.GetPixels32 ();
		}

		_sourceWidth = source.width;
		_sourceHeight = source.height;
		_baseTexture = new Texture2D(source.width, source.height,TextureFormat.RGBA32,false);
		_baseTexture.wrapMode = TextureWrapMode.Clamp;
		Renderer render = GetComponent<Renderer>();
		render.SetPropertyBlock(null);
		render.material.mainTexture = _baseTexture;
		_pixels = new byte[source.width*source.height*4];
		_sourceColors = source.GetPixels32 ();
		this.ClearImage (isShowPicture);
	}

	public void ClearImage(bool isShowPicture)
	{
		int index = 0;
		for (int i = 0; i < this._baseTexture.height; i++)
		{
			for (int j = 0; j < this._baseTexture.width; j++)
			{
				Color32 c= _sourceColors[index/4];
				this._pixels[index] = c.r;
				this._pixels[index + 1] = c.g;
				this._pixels[index + 2] = c.b;
				if(isShowPicture){
					this._pixels[index + 3] = c.a;
				}
				else{
					this._pixels[index + 3] = 0;
				}
				index += 4;
			}
		}
		this._baseTexture.LoadRawTextureData(this._pixels);
		this._baseTexture.Apply(false);
	}
	
	
	/// <summary>
	/// 画.
	/// </summary>
	/// <param name="pos">屏幕坐标.</param>
	/// <param name="camera">如果为null，则用Camera.main</param>
	public void DrawGraphics(Vector3 pos, Camera camera = null)
	{
		RaycastHit hit;
		if (camera == null) camera = Camera.main;
		Ray ray = camera.ScreenPointToRay(pos);
		if (Physics.Raycast(ray, out hit))
		{
			if (!_isDown)
			{
				_isDown = true;
				_prevMousePosition = hit.textureCoord;
			}
			LerpDraw(hit.textureCoord, _prevMousePosition);
			if(_delayApply>=_delayDrawTime){
				_delayApply = 0;
				this._baseTexture.LoadRawTextureData(this._pixels);
				_baseTexture.Apply(false);
			}
			_delayApply+=Time.deltaTime;
			_prevMousePosition = hit.textureCoord;
		}
	}

	/// <summary>
	/// 如果画布是Sprite
	/// </summary>
	/// <param name="pos">屏幕坐标.</param>
	/// <param name="camera">Camera.</param>
	public void DrawSpriteGraphics(Vector3 pos, Camera camera = null)
	{
		RaycastHit hit;
		if (camera == null) camera = Camera.main;
		Ray ray = camera.ScreenPointToRay(pos);
		if (Physics.Raycast(ray, out hit))
		{
			Vector2 uv = SpriteHitPoint2UV(hit.point);
			if (!_isDown)
			{
				_isDown = true;
				_prevMousePosition = uv;
			}
			LerpDraw(uv, _prevMousePosition);
			if(_delayApply>=_delayDrawTime){
				_delayApply = 0;
				this._baseTexture.LoadRawTextureData(this._pixels);
				_baseTexture.Apply(false);
			}
			_delayApply+=Time.deltaTime;
			_prevMousePosition = uv;
		}
	}

	/// <summary>
	/// Sprite中,hitPoint转uv坐标。hitPoint为世界坐标
	/// </summary>
	/// <returns>The hit point2 U.</returns>
	/// <param name="hitPoint">Hit point.</param>
	public Vector2 SpriteHitPoint2UV( Vector3 hitPoint){
		Vector3 localPos=transform.InverseTransformPoint(hitPoint);
		localPos*=100f;
		localPos.x += _sourceWidth*0.5f;
		localPos.y += _sourceHeight*0.5f;
		return new Vector2(localPos.x/_sourceWidth,localPos.y/_sourceHeight);
	}
	
	/// <summary>
	/// 结束画.
	/// </summary>
	public void DrawEnd()
	{
		_isDown = false;
		this._baseTexture.LoadRawTextureData(this._pixels);
		this._baseTexture.Apply(false);
		_delayApply = 0;

	}
	
	/// <summary>
	/// 是否完成.
	/// </summary>
	/// <param name="temp">可容差的大小,主要用在画时; 如果为擦除，这个值需要小一些.</param>
	/// <returns>如果当前是画，则返回是否画完；如果当前是擦除，则返回是否擦除干净.</returns>
	public bool IsScribbleCompleted(int temp = 100)
	{
		int count = 0;
		if (isEraser)
		{
			for (int i = 0; i < _baseTexture.width; i++)
			{
				for (int j = 0; j < _baseTexture.height; j++)
				{

					if (_baseTexture.GetPixel(i, j).a > 0)
					{
						count++;
						if (count > temp)
						{
							i = _baseTexture.width;
							break;
						}
					}
				}
			}
		}
		else
		{
			for (int i = 0; i < _baseTexture.width - 1; i++)
			{
				for (int j = 0; j < _baseTexture.height - 1; j++)
				{
					Color c = source.GetPixel(i, j);
					if (c.a > 0f && _baseTexture.GetPixel(i, j) != c)
					{
						count++;
						if (count > temp)
						{
							i = _baseTexture.width;
							break;
						}
					}
				}
			}
		}
		return count <= temp;
	}

	/// <summary>
	/// 画Sprite
	/// </summary>
	/// <param name="sr">Sr.</param>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public void DrawSprite( SpriteRenderer sr, float x, float y)
	{
		Texture2D t = sr.sprite.texture;
		DrawTexture(t,x,y);
	}


	/// <summary>
	/// Draws the texture.
	/// </summary>
	/// <param name="img">Image.</param>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="isClear">如果为ture,则alpha会设置为0.</param>
	public void DrawTexture( Texture2D img , float x, float y)
	{
        int startX = Mathf.FloorToInt((x * source.width) - img.width /2);
        int startY = Mathf.FloorToInt((y * source.height) - img.height/ 2);
        int endI = startX + img.width;
        int endJ = startY + img.height;
        float minXPen = 0;
        float maxXPen = source.width-1 ;
        float minYPen = -img.height/2;
        float maxYPen = source.height - 1;
        Color32[] imgColors = img.GetPixels32();

		for(int i = startX; i<endI; i++)
		{
            if (i < minXPen) continue;
			else if(i>maxXPen) continue;
			
			for(int j=startY; j<endJ; j++)
			{ 	
				if(j<minYPen) break;
				else if(j>maxYPen) continue;
				
				int penI = endI-i;
				int penJ = endJ-j;
				if(i>source.width) penI -= i-source.width; 
				if(j>source.height) penJ -= j-source.height;
				
				int idx = j*_baseTexture.width+i;
				if(idx>-1 && idx<_sourceColors.Length){
                    int penIdx = penJ * img.width + penI;
                    if (penIdx > -1 && penIdx < imgColors.Length)
                    {
                        Color32 c = imgColors[penIdx];
                        if (c.a > 0)
                        {
                            idx *= 4;

							if(isEraser){
								_pixels[idx + 3]=0;
							}
							else
							{
								_pixels[idx] = c.r;
								_pixels[idx + 1] = c.g;
								_pixels[idx + 2] = c.b;
								if (c.a > _pixels[idx + 3])
									_pixels[idx + 3] = c.a;
							}
                        }
                    }
				}
				
			}
		}
        this._baseTexture.LoadRawTextureData(this._pixels);
        this._baseTexture.Apply(false);
	}

	#endregion public functions
	

	
	
	// Update is called once per frame
	/*
	 void Update () {
		

		if(Input.GetMouseButtonDown(0)||(Input.touchCount>0 && Input.touches[0].phase==TouchPhase.Began))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray,out hit))
			{
				_prevMousePosition = hit.textureCoord ;
				_isDown = true;
				Draw(hit.textureCoord);
				_baseTexture.Apply();
			}
		}
		else if(Input.GetMouseButtonUp(0)||(Input.touchCount>0 && (Input.touches[0].phase==TouchPhase.Ended||Input.touches[0].phase==TouchPhase.Canceled)))
		{
			_isDown = false ;
		}
		//move
		if(Input.GetMouseButton(0)||(Input.touchCount>0&&Input.touches[0].phase==TouchPhase.Moved))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray,out hit))
			{
				if(!_isDown) {
					_isDown= true;
					_prevMousePosition = hit.textureCoord;
				}
				LerpDraw(hit.textureCoord,_prevMousePosition);
				_baseTexture.Apply();
				_prevMousePosition = hit.textureCoord;
			}

		}

	}
	 */
	
	void LerpDraw(Vector2 current , Vector2 prev)
	{
		if (current == prev)
			return;
		float distance = Vector2.Distance(current, prev);
		for (float i = 0; i < distance; i += lerpDamp)
		{
			float lDelta = i / distance;
			float lDifx = current.x - prev.x;
			float lDify = current.y - prev.y;
			Draw(new Vector2(prev.x + (lDifx * lDelta), prev.y + (lDify * lDelta)));
		}
	}
	
	void Draw( Vector2 textureCoord)
	{
		int startX = Mathf.FloorToInt( (textureCoord.x*source.width)-brushSize/2 );
		int startY = Mathf.FloorToInt((textureCoord.y*source.height)-brushSize/2 );

		if (brushType == BrushType.CustomBrush) {
			int endI  = startX+brushSize;
			int endJ = startY+brushSize;
			PaintByPen (startX, startY, endI, endJ);
		}else if(brushType== BrushType.SolidCircle){
			DrawCircle (startX, startY);
		}
	}

	void PaintByPen(int startX,int startY,int endI,int endJ)
	{
		if(paintType== PaintType.DrawColorfulLine && paintColorful.Length>1){
			Color32 currC = paintColorful[m_colorfulIndex];
			paintColor = Color32.Lerp(paintColor,currC,Time.deltaTime*colorChangeRate);
			m_colorfulTime+=Time.deltaTime*colorChangeRate;
			if(m_colorfulTime>1f){
				m_colorfulTime =0f;
				++m_colorfulIndex;
				if(m_colorfulIndex>=paintColorful.Length){
					m_colorfulIndex = 0;
				}
			}
		}

		for(int i = startX; i<endI; i++)
		{
			if(i<_minXPen) continue;
			else if(i>_maxXPen) continue;

			for(int j=startY; j<endJ; j++)
			{ 	
				if(j<_minYPen) break;
				else if(j>_maxYPen) continue;

				int penI = endI-i;
				int penJ = endJ-j;
				if(i>source.width) penI -= i-source.width; 
				if(j>source.height) penJ -= j-source.height;

				if(isEraser)
				{
					if (penAlphaEnable){
						
						int penIdx = penJ*brushSize+penI;
						if(penIdx>-1&&penIdx<_penColors.Length){
							byte penA =_penColors[penIdx].a;
							if (penA > 0 )
							{
								int idx = j*_baseTexture.width+i;
								if(idx>-1 && idx<_sourceColors.Length){
									int alphaIdx= idx*4+3;
									if(_pixels[alphaIdx]>=0xff-penA)
									{
										_pixels[alphaIdx] = (byte)(0xff-penA);
									}
								}
								
							}
						}
					}
					else
					{
						int penIdx = penJ*brushSize+penI;
						if(penIdx>-1&&penIdx<_penColors.Length){
							byte penA =_penColors[penIdx].a;
							if (penA > 0 )
							{
								int idx = j*_baseTexture.width+i;
								if(idx>-1 && idx<_sourceColors.Length){
									int alphaIdx= idx*4+3;
									_pixels[alphaIdx] = 0;
									
								}
								
							}
						}
					}
				}
				else
				{
					if (penAlphaEnable){
						
						int penIdx = penJ*brushSize+penI;
						if(penIdx>-1&&penIdx<_penColors.Length){
							byte penA =_penColors[penIdx].a;
							if (penA > 0)
							{
								int idx = j*_baseTexture.width+i;
								if(idx>-1 && idx<_sourceColors.Length){
									
									Color32 c = paintColor;
									if(paintType== PaintType.Scribble){
										c = _sourceColors[idx];
										idx *= 4;
										_pixels[idx] = c.r;
										_pixels[idx+1] = c.g;
										_pixels[idx+2] = c.b;
										byte resultA = (byte)(0xff-(0xff-_pixels[idx+3])*(0xff-penA)/0xff);
										if(c.a>resultA){
											_pixels[idx+3] = resultA;
										}else{
											_pixels[idx+3] = c.a;
										}
									}
									else if(paintType== PaintType.DrawLine || paintType== PaintType.DrawColorfulLine){
										idx *= 4;
										//mix the colors
										_pixels[idx] = (byte)((0xff-penA)*_pixels[idx]/0xff + penA*c.r/0xff);
										_pixels[idx+1] = (byte)((0xff-penA)*_pixels[idx+1]/0xff + penA*c.g/0xff);
										_pixels[idx+2] = (byte)((0xff-penA)*_pixels[idx+2]/0xff + penA*c.b/0xff);
										_pixels[idx+3] = (byte)(0xff-(0xff-_pixels[idx+3])*(0xff-penA)/0xff);
									}
								}
								
							}
						}
						
					}
					else
					{
						
						int penIdx = penJ*brushSize+penI;
						if(penIdx>-1&&penIdx<_penColors.Length){
							byte penA =_penColors[penIdx].a;
							if (penA > 0)
							{
								int idx = j*_baseTexture.width+i;
								if(idx>-1 && idx<_sourceColors.Length){
									
									Color32 c = paintColor;
									if(paintType== PaintType.Scribble){
										c = _sourceColors[idx];
									}
									idx *= 4;
									_pixels[idx] = c.r;
									_pixels[idx+1] = c.g;
									_pixels[idx+2] = c.b;
									_pixels[idx+3] = c.a;
								}
								
							}
						}
					}
					
				}
				
			}
		}
	}

	private void DrawCircle(int x, int y)
	{
		x = this.ClampBrushInt(x, this._baseTexture.width - this.brushSize);
		y = this.ClampBrushInt(y, this._baseTexture.height - this.brushSize);
		int index = 0;
		int rSquare = this.brushSize * this.brushSize;
		int maxSize = rSquare << 2;
		int tempBrushSize = this.brushSize<<1;
		if(paintType== PaintType.DrawColorfulLine && paintColorful.Length>1){
			Color32 currC = paintColorful[m_colorfulIndex];
			paintColor = Color32.Lerp(paintColor,currC,Time.deltaTime*colorChangeRate);
			m_colorfulTime+=Time.deltaTime*colorChangeRate;
			if(m_colorfulTime>1f){
				m_colorfulTime =0f;
				++m_colorfulIndex;
				if(m_colorfulIndex>=paintColorful.Length){
					m_colorfulIndex = 0;
				}
			}
		}
		for (int i = 0; i < maxSize; i++)
		{
			int a = (i % tempBrushSize) - this.brushSize;
			int b = (i / tempBrushSize) - this.brushSize;
			if ((a * a) + (b * b) < rSquare)
			{
				index = (((this._baseTexture.width * (y + b)) + x) + a) * 4;
				if(paintType==PaintType.DrawLine|| paintType== PaintType.DrawColorfulLine){
					if(isEraser){
						this._pixels[index + 3] = 0;
					}else{
						this._pixels[index] = paintColor.r;
						this._pixels[index + 1] = paintColor.g;
						this._pixels[index + 2] = paintColor.b;
						this._pixels[index + 3] = paintColor.a;
					}
				}
				else if(paintType==PaintType.Scribble){
					if(isEraser){
						this._pixels[index + 3] = 0;
					}else{
						Color32 c = _sourceColors[index/4];
						this._pixels[index] = c.r;
						this._pixels[index + 1] = c.g;
						this._pixels[index + 2] = c.b;
						this._pixels[index + 3] = c.a;
					}
				}
			}
		}
	}
	
	private int ClampBrushInt(int value, int max)
	{
		return ((value >= brushSize) ? ((value <= max) ? value : max) : brushSize);
	}
}