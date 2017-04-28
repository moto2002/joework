using UnityEngine;
using System.Collections;

public class SpriteMapTest : MonoBehaviour {

	public MapLayer mapLayer;
	public Transform scaleAnchor;

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(1f);
		//因为scaleMap需要MapViewport层的局部坐标，所以需要把ScaleAnchor坐标转到MapViewport层
		Vector2 anchor = new Vector2(
			scaleAnchor.localPosition.x+mapLayer.transform.localPosition.x,
			scaleAnchor.localPosition.y+mapLayer.transform.localPosition.y
		);
		while(true){
			mapLayer.ScaleMap(1.002f,anchor.x,anchor.y);
			if(mapLayer.transform.localScale.x<3f){
				yield return new WaitForEndOfFrame();
			}else{
				break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
