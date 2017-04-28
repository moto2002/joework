using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteDrag))]
public class DragSpriteTest : MonoBehaviour {

	private SpriteDrag drag;

	// Use this for initialization
	void Start () {
		drag = GetComponent<SpriteDrag>();
		drag.OnDragAction += Drag_OnMouseDragAction;
	}

	void Drag_OnMouseDragAction (SpriteDrag obj)
	{
		//限制拖的区域
		Vector3 pos = obj.transform.position;
		Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
		if(screenPos.x>Screen.width*0.5f){
			screenPos.x = Screen.width*0.5f;
			obj.transform.position = Camera.main.ScreenToWorldPoint(screenPos);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
