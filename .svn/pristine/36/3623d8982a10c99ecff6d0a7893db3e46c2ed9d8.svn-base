using UnityEngine;
using System.Collections;

public class boxcollisioncircle : MonoBehaviour {

	public Transform boxTrans;
	public Transform circleTrans;
	
	Rectangle rect;
	Circle circle;

	void Start () 
	{
	
		rect = new Rectangle(10f,5f,new Vector2(boxTrans.position.x,boxTrans.position.z));
		circle = new Circle(3f,new Vector2(circleTrans.position.x ,circleTrans.position.z));

		GameObject range = GameObject.Instantiate( Resources.Load("range") as GameObject);
		range.transform.parent = circleTrans;
		range.transform.localRotation = Quaternion.identity;
		range.transform.localPosition = Vector3.zero;
		range.transform.localScale = Vector3.one * circle.radius * 2f;
	}

	void Update () 
	{
     
        rect.Refresh(new Vector2(boxTrans.position.x,boxTrans.position.z),boxTrans.rotation);
		circle.Refresh(new Vector2(circleTrans.position.x ,circleTrans.position.z));

		if(IntersectionDetection.Intersect(rect,circle))
		{
			Debug.Log("in");
		}
		else
		{
			Debug.Log("not in");
		}

        //boxTrans.Rotate(new Vector3(0, 0.5f, 0));
    }
}
