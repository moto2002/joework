using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IsoObject : MonoBehaviour {

	private Vector3 m_pos3D;
	private int m_nodeX = 1;
	private int m_nodeZ = 1;
	private List<Vector3> m_spanPosArray = new List<Vector3>();
	private bool m_isRotated = false;
	private float m_centerOffsetY = 0;

	[HideInInspector]
	public bool isSort = false;

	[Header("ISO Setting")]
	public int spanX;
	public int spanZ;
	public int size;

	public List<Vector3> spanPosArray{
		get { return m_spanPosArray; }
	}
	public float x{
		get { return m_pos3D.x; }
		set {
			m_pos3D.x = value;
			UpdateScreenPos();
			UpdateSpanPos();
		}
	}
	public float y{
		get { return m_pos3D.y; }
		set {
			m_pos3D.y = value;
			UpdateScreenPos();
			UpdateSpanPos();
		}
	}
	public float z{
		get { return m_pos3D.z; }
		set {
			m_pos3D.z = value;
			UpdateScreenPos();
			UpdateSpanPos();
		}
	}
	public Vector3 pos3D{
		get { return m_pos3D; }
		set { m_pos3D=value; }
	}
	public int nodeX{
		get {return m_nodeX; }
	}
	public int nodeZ{
		get {return m_nodeZ; }
	}
	public float centerY{
		get {
			return transform.localPosition.y-m_centerOffsetY;
		}
	}
	public float depth{
		//return (this._pos3D.x + this._pos3D.z) * .866 - this._pos3D.y * .707;
		get { return centerY; }
	}


	protected virtual void Start(){
		m_centerOffsetY = size*spanX/2f;
	}

	public virtual void Sort(){
		isSort = true;
	}

	public void RotateX( bool value){
		m_isRotated = value;
		UpdateSpanPos();
	}

	public void UpdateSpanPos(){
		m_spanPosArray.Clear();
		int t1=0;
		int t2=0;
		if(m_isRotated){
			t1 = spanZ;
			t2 = spanX;
		}else{
			t1 = spanX;
			t2 = spanZ;
		}
		for(int i = 0 ;  i<t1 ; i++)
		{
			for(int j = 0 ; j<t2 ; j++)
			{
				Vector3 pos = new Vector3( i*size+x, y, j*size+z );
				m_spanPosArray.Add( pos );
			}
		}
	}

	public void SetNodePosition(int nodeX,int nodeZ)
	{
		m_nodeX = nodeZ;
		m_nodeZ = nodeZ;
		m_pos3D.x = nodeX*size;
		m_pos3D.z = nodeZ*size;
		UpdateScreenPos();
		UpdateSpanPos();

	}

	public void SetScreenPos(float x,float y){
		this.x = x;
		this.y = y;
		m_pos3D.x = x;
		m_pos3D.y = y;
	}

	public void SetWalkable(bool value,PathGrid grid){
		UpdateSpanPos();
		foreach(Vector3 v in m_spanPosArray){
			grid.SetWalkable( Mathf.FloorToInt(v.x/size),Mathf.FloorToInt(v.z/size),value);
		}
	}

	public bool GetWalkable( PathGrid grid){
		bool flag = false;
		foreach(Vector3 v in m_spanPosArray){
			int nodeX = Mathf.FloorToInt(v.x/size);
			int nodeY = Mathf.FloorToInt(v.z/size);
			if(nodeX<0 || nodeX>grid.gridX-1) return false;
			if(nodeY<0 || nodeY>grid.gridZ-1) return false;
			flag = grid.GetNode(nodeX,nodeY).walkable;
			if(!flag) return false;
		}
		return true;
	}

	public bool GetRotatable(PathGrid grid)
	{
		if (spanX==spanZ ) return true;

		SetWalkable(true,grid);
		m_isRotated = ! m_isRotated;
		UpdateSpanPos();
		bool flag = GetWalkable(grid);
		//还原
		m_isRotated = !m_isRotated;
		SetWalkable(false,grid);
		return flag;
	}

	public void UpdateScreenPos(){
		Vector2 ScPos = IsoUtil.IsoToScreen(m_pos3D.x,m_pos3D.y,m_pos3D.z);
		Vector3 pos = transform.localPosition;
		pos.x = ScPos.x ;
		pos.y = ScPos.y ;
		transform.localPosition = pos;
		UpdateSpanPos();
	}

	public virtual void Destroy(){
		m_spanPosArray.Clear();
		m_spanPosArray = null;
	}
}
