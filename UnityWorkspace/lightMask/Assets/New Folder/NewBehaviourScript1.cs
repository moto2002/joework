using UnityEngine;
using System.Collections;

public class NewBehaviourScript1 : MonoBehaviour {
	public  float speed =3f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 a=  new Vector2(Input.GetAxis ("Horizontal")*speed,Input.GetAxis ("Vertical")*speed);
		GetComponent<Rigidbody2D>().AddForce (a);
	
	}
}
