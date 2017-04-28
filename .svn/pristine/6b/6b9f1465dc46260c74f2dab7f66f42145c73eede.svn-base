using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 填色工具，通过类似魔术棒算法获取相似的区域
/// </summary>
public class ColorFillTool : MonoBehaviour,IPointerClickHandler ,IDragHandler{

	public Texture2D srcTexture; //图片,需要设置成read/write，取消mipmap
	public int size = 1024; //图片大小 

	private Color32[] m_srcColor32;//源图片的颜色

	private Texture2D m_desTexture; //颜色后的图片
	private byte[] m_desBytes;

	private RawImage m_img ;
	private RectTransform m_rectTrans;
	private bool m_inited = false;
	private int m_dragTick = 0;

	// Use this for initialization
	IEnumerator Start () {
		m_rectTrans = GetComponent<RectTransform>();
		m_srcColor32 = srcTexture.GetPixels32();

		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();

		m_desTexture = new Texture2D(size,size,TextureFormat.RGBA32,false,false);
		m_desBytes = m_desTexture.GetRawTextureData();
		int len = m_desBytes.Length;
		for(int i=0;i<len;++i){
			m_desBytes[i] = 0;
		}
		this.m_desTexture.LoadRawTextureData(this.m_desBytes);
		this.m_desTexture.Apply(false);

		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();

		m_img = GetComponent<RawImage>();
		m_img.texture = m_desTexture;

		//初始化图像颜色搜索node
		m_nodes = new List<List<Node>>();
		for (int i = 0; i < size; ++i)
		{
			List<Node> nn = new List<Node>();
			for (int j = 0; j <size; ++j)
			{
				nn.Add( new Node(i,j));
			}
			m_nodes.Add(nn);
		}

		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		//设置叶子节点
		for (int i = 0; i < size; ++i)
		{
			for (int j = 0; j <size; ++j)
			{
				Node n = m_nodes[i][j];
				if(i>0)	n.left = m_nodes[i-1][j];
				if(i<size-1)	n.right = m_nodes[i+1][j];
				if(j<size-1)	n.down = m_nodes[i][j+1];
				if(j>0)	n.up = m_nodes[i][j-1];
			}

		}
		m_inited = true;
	}

	/// <summary>
	/// 拖动时填充颜色
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnDrag(PointerEventData eventData)
	{
		if (eventData.dragging)
		{
			++m_dragTick; //防止 过快
			if(m_dragTick==2){
				OnPointerClick(eventData);
				m_dragTick = 0;
			}
		}
	}

	/// <summary>
	/// 点击时填充颜色
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerClick (PointerEventData eventData)
	{
		if(m_inited && Input.touchCount<2){
			Vector2 localPoint;
			if(RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rectTrans,eventData.position,Camera.main,out localPoint)){
				localPoint += new Vector2(m_rectTrans.sizeDelta.x * m_rectTrans.pivot.x,m_rectTrans.sizeDelta.y * m_rectTrans.pivot.y);
				float sc = size/m_rectTrans.sizeDelta.x;

				int x = Mathf.FloorToInt(localPoint.x*sc);
				int y = Mathf.FloorToInt(localPoint.y*sc);
				int index = y*size+x;
				Color32 srcC = m_srcColor32[index];

				index *= 4;
				if(srcC.a<255 && m_desBytes[index+3]<10 ){//&& 255!=desBytes[index] && 255!=desBytes[index+1] & 255!=desBytes[index+2]
					DrawByClosedList( Search(x,y,srcC),srcC);
				}
			}
		}
	}

	/// <summary>
	/// 清除所有的填充的颜色
	/// </summary>
	private void ClearColors(){
		int len = m_desBytes.Length;
		for(int i=0;i<len;++i){
			m_desBytes[i] = 0;
		}
		this.m_desTexture.LoadRawTextureData(this.m_desBytes);
		this.m_desTexture.Apply(false);
	}

	/// <summary>
	/// 设置颜色
	/// </summary>
	/// <param name="closedList">Closed list.</param>
	/// <param name="c">C.</param>
	private void DrawByClosedList(List<Node> closedList,Color32 c)
	{
		int len = closedList.Count;
		byte r = (byte)Random.Range(10,256);
		byte g = (byte)Random.Range(10,256);
		byte b = (byte)Random.Range(10,256);
		for(int i=0;i<len;++i){
			Node node = closedList[i];
			int index = Mathf.FloorToInt(node.v.y*size+node.v.x)*4;
			this.m_desBytes[index] = r;
			this.m_desBytes[index+1] = g;
			this.m_desBytes[index+2] = b;
			this.m_desBytes[index+3] = 255;
		}
		this.m_desTexture.LoadRawTextureData(this.m_desBytes);
		this.m_desTexture.Apply(false);
	}



	void OnGUI(){
		if(GUI.Button(new Rect(10,10,100,60),"Clear")){
			ClearColors();
		}
	}





	#region A*搜索

	/// <summary>
	/// A*节点
	/// </summary>
	class Node{
		public Node left,right,up,down;

		public Vector2 v = Vector2.zero;
		public bool isClosed = false;
		public bool isOpened = false;
		public bool isOpenOrClosed(){
			return isClosed || isOpened ;
		}
		public Node(){}
		public Node(int x, int y){
			v.x = x;
			v.y = y;
		}

	}

	private List<List<Node>> m_nodes ;
	private List<Node> m_openList = new List<Node>();
	private List<Node> m_closedList = new List<Node>();

	private List<Node> Search(int x, int y, Color32 c)
	{
		for (int i = 0; i < size; ++i)
		{
			for (int j = 0; j <size; ++j)
			{
				Node n = m_nodes[i][j];
				n.isClosed = false;
				n.isOpened = false;
			}
		}


		m_openList.Clear();
		m_closedList.Clear();
		m_openList.Add(m_nodes[x][y]);

		while(m_openList.Count>0){
			Node node = m_openList[0].up;
			if (node!=null && isTheSameColor(node) && !node.isOpenOrClosed())
			{
				node.isOpened=true;
				m_openList.Add(node);
			}
			node = m_openList[0].down;
			if (node!=null && isTheSameColor(node) && !node.isOpenOrClosed())
			{
				node.isOpened=true;
				m_openList.Add(node);
			}
			node = m_openList[0].left;
			if (node!=null && isTheSameColor(node) && !node.isOpenOrClosed())
			{
				node.isOpened=true;
				m_openList.Add(node);
			}
			node = m_openList[0].right;
			if (node!=null && isTheSameColor(node) && !node.isOpenOrClosed())
			{
				node.isOpened=true;
				m_openList.Add(node);
			}
			m_openList[0].isClosed = true;
			m_closedList.Add(m_openList[0]);
			m_openList.RemoveAt(0);
			//如果区域太大，则返回 . 大于 256*256
			if(m_closedList.Count>65536) {
				m_closedList.Clear();
				break;
			}
		}
		return m_closedList;
	}

	/// <summary>
	/// 是否是相同的颜色
	/// </summary>
	/// <returns><c>true</c>, if the same color was ised, <c>false</c> otherwise.</returns>
	/// <param name="node">Node.</param>
	private bool isTheSameColor(Node node){
		int index = Mathf.FloorToInt(node.v.y*size+node.v.x);
		Color32 srcC = m_srcColor32[index];
		if(srcC.a==0 || srcC.r>0 || srcC.g>0 || srcC.b>0) return true;
//		else{ //如果叶子节点也符合条件的话，则此节点就符合条件
//			
//			Node left = node.left;
//			index = Mathf.FloorToInt(left.v.y*size+left.v.x);
//			srcC = m_srcColor32[index];
//			if(srcC.a==0 || srcC.r>0 || srcC.g>0 || srcC.b>0) return true;
//
//			Node right = node.right;
//			index = Mathf.FloorToInt(right.v.y*size+right.v.x);
//			srcC = m_srcColor32[index];
//			if(srcC.a==0 || srcC.r>0 || srcC.g>0 || srcC.b>0) return true;
//
//			Node up = node.up;
//			index = Mathf.FloorToInt(up.v.y*size+up.v.x);
//			srcC = m_srcColor32[index];
//			if(srcC.a==0 || srcC.r>0 || srcC.g>0 || srcC.b>0) return true;
//
//			Node down = node.down;
//			index = Mathf.FloorToInt(down.v.y*size+down.v.x);
//			srcC = m_srcColor32[index];
//			if(srcC.a==0 || srcC.r>0 || srcC.g>0 || srcC.b>0) return true;
//		}
		return false;
	}

	#endregion
}
