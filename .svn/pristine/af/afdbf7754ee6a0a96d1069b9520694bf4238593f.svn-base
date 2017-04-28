using UnityEngine;
using System.Collections;

public class GameIsoWorld : MonoBehaviour {

	public IsoScene scene;
	public IsoObject isoObjectPrefab;

	public PathGrid grid;

	private float m_touchTime = 0;
	private Vector3 m_touchPos ;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;
		grid = new PathGrid(scene.spanX,scene.spanZ);
		grid.SetAllWalkable(true);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			m_touchTime = Time.realtimeSinceStartup;
			m_touchPos = Input.mousePosition;
		}
		else if(Input.GetMouseButtonUp(0))
		{
			if(Time.realtimeSinceStartup-m_touchTime<0.5f && Vector3.Distance(Input.mousePosition,m_touchPos)<20f){
				Click();
			}
		}
	}


	void Click(){
		Vector3 scenePos = scene.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		Vector2 nodePos = IsoUtil.ScreenToIsoGrid(scene.size,scenePos.x,scenePos.y);

		int nodeX = (int)nodePos.x;
		int nodeZ = (int)nodePos.y;
		if(grid.CheckInGrid(nodeX,nodeZ)  && grid.GetNode(nodeX,nodeZ).walkable){
			IsoObject obj = (IsoObject)Instantiate(isoObjectPrefab);
			obj.SetNodePosition(nodeX,nodeZ);
			if(obj.GetWalkable(grid)){
				scene.AddIsoObject(obj);
				obj.SetNodePosition(nodeX,nodeZ);
				obj.transform.localScale = Vector3.one;
				obj.SetWalkable(false,grid);
			}
			else
			{
				Destroy(obj.gameObject);
			}
		}
	}
}
