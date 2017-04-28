using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IsoScene : IsoObject {
	[Header("Gizmos Grid")]
	public bool showGrid = false;

	private List<IsoObject> m_sprites;

	private PathGrid m_gridData;
	public PathGrid gridData{
		get {
			return m_gridData;
		}
		set {
			m_gridData=value;
		}
	}

	protected override void Start(){
		base.Start();
		m_sprites = new List<IsoObject>();
	}

	public void AddIsoObject(IsoObject obj,bool isSort = true){
		obj.transform.parent=transform;
		m_sprites.Add(obj);
		if(isSort){
			SortIsoObject(obj);
		}
	}

	public void RemoveIsoObject(IsoObject obj){
		m_sprites.Remove(obj);
		Destroy(obj.gameObject);
	}

	public void SortIsoObject(IsoObject obj){
		Vector3 pos = obj.transform.localPosition;
		pos.z = obj.depth;
		obj.transform.localPosition = pos;
	}

	public void SortAll(){
		foreach(IsoObject obj in m_sprites){
			obj.isSort = true;
		}
	}

	public IsoObject GetIsoObjectByNodePos(int nodeX,int nodeZ){
		foreach(IsoObject obj in m_sprites){
			if(obj.x==nodeX && obj.z==nodeZ){
				return obj ;
			}
		}
		return null;
	}

	public List<IsoObject> GetIsoChildren(){
		return m_sprites;
	}

	public void ClearScene(){
		foreach(IsoObject obj in m_sprites){
			Destroy(obj.gameObject);
		}
		m_sprites.Clear();
	}

	public override void Destroy(){
		ClearScene();
		m_sprites = null;
	}

	void OnDrawGizmos(){
		if(showGrid){
			Gizmos.color = new Color(1f,1f,1f,0.4f);

			Vector3 p = Vector3.zero;

			for(int i=0;i<=spanX;i++){
				p.x = i*size;
				p.z = 0;
				Vector3 startPos = IsoUtil.IsoToScreen( p.x,p.y,p.z);
				p.z = spanZ*size;
				Vector3 endPos = IsoUtil.IsoToScreen( p.x,p.y,p.z);
				Gizmos.DrawLine(transform.TransformPoint(startPos),transform.TransformPoint(endPos));
			}

			for(int i=0;i<=spanZ;i++){
				p.z = i*size;
				p.x = 0;
				Vector3 startPos = IsoUtil.IsoToScreen( p.x,p.y,p.z);
				p.x = spanX*size;
				Vector3 endPos = IsoUtil.IsoToScreen( p.x,p.y,p.z);
				Gizmos.DrawLine(transform.TransformPoint(startPos),transform.TransformPoint(endPos));
			}


		}

	}
}
