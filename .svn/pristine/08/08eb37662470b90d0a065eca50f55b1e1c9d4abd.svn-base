using UnityEngine;
using System.Collections;

public class SpriteMapViewport : MonoBehaviour {

	public Rect viewPort;//范围
	public bool useScreenSize = true; //是否用屏幕的大小

	// Use this for initialization
	void Awake () {
		if(useScreenSize){
			viewPort = new Rect(0,0,Native2dScreenUtil.GetScreenWidth(),Native2dScreenUtil.GetScreenHeight());
			transform.position = new Vector3(-viewPort.width/2,-viewPort.height/2f,transform.position.z);
		}
	}
	

	void OnDrawGizmos(){
		Gizmos.color = Color.yellow;

		Gizmos.DrawLine(new Vector3(transform.position.x,transform.position.y,0),new Vector3(transform.position.x,viewPort.height+transform.position.y,0));

		Gizmos.DrawLine(new Vector3(transform.position.x,viewPort.height+transform.position.y,0),
			new Vector3(transform.position.x+viewPort.width,viewPort.height+transform.position.y,0));
		
		Gizmos.DrawLine(new Vector3(transform.position.x+viewPort.width,viewPort.height+transform.position.y,0),
			new Vector3(transform.position.x+viewPort.width,transform.position.y,0));
		
		Gizmos.DrawLine(new Vector3(transform.position.x+viewPort.width,transform.position.y,0),
			new Vector3(transform.position.x,transform.position.y,0));
	}
}
