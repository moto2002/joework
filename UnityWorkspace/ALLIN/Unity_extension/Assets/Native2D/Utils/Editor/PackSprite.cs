using UnityEngine;
using System.Collections;
using UnityEditor;

public class PackSprite:Editor {

	[MenuItem("Sprites/Sprite包装/将Sprite包装一层",false,11)]
	static void ExePackSprite(){
		Object[] objs = Selection.GetFiltered(typeof(SpriteRenderer),SelectionMode.Assets);
//		float z = -0.9f;
		foreach(Object obj in objs){
			SpriteRenderer sp = obj as SpriteRenderer;
			if(sp){
				sp.sortingOrder=0;
				GameObject go = new GameObject();
				go.name = "Slot_"+sp.gameObject.name;
				go.transform.parent = sp.transform.parent;
				go.transform.localScale = Vector3.one;
				Vector3 pos = sp.transform.localPosition;
//				pos.z = z;
				go.transform.localPosition = pos;
				sp.transform.parent = go.transform;
				sp.transform.localPosition=Vector3.zero;
			}
//			z+=0.02f;
		}
	}

	[MenuItem("Sprites/Sprite包装/去除Sprite的包装层",false,10)]
	static void DePackSprite(){
		Object[] objs = Selection.GetFiltered(typeof(Transform),SelectionMode.Assets);
		foreach(Object obj in objs){
			Transform sp = obj as Transform;
			if(sp && sp.GetComponent<SpriteRenderer>()==null && sp.childCount==1){
				Transform child = sp.GetChild(0);
				Vector3 pos = sp.localPosition;
				pos.z = 0f;
				child.parent = sp.parent;
				child.localPosition = pos;
				DestroyImmediate(sp.gameObject);
			}
		}
	}
}
