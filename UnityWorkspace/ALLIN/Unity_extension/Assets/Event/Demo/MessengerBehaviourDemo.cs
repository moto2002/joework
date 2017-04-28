using UnityEngine;
using System.Collections;

public class MessengerBehaviourDemo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		MessageDemo demo = GetComponent<MessageDemo> ();
		demo.AddListener<int>("NO",(int flag)=>{
			print(flag);
		});
	}
	

}
