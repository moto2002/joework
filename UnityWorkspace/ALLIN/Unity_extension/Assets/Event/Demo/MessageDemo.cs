using UnityEngine;
using System.Collections;

public class MessageDemo : MessengerBehaviour {

	// Use this for initialization
	void Start () {
		Messenger.AddListener<bool> ("OK", Callback);
		Messenger.DispatchEvent ("OK",true);
		Messenger.RemoveListener<bool> ("OK", Callback);
		Messenger.DispatchEvent ("OK",true);


		this.DispatchEvent<int> ("NO",1);
	}
	
	void Callback(bool flag){
		print (flag);
	}
}
