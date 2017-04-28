
using UnityEngine;
using System.Collections;

public class UniWebViewCube : MonoBehaviour {
	public UniWebDemo webViewDemo;
	
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8
	float startTime = 0;
	bool firstHit = true;

	void Start () {
		startTime = Time.time;
	}
	
	void OnCollisionEnter(Collision col) {
		if (col.gameObject.name == "Target") {
			webViewDemo.ShowAlertInWebview(Time.time - startTime, firstHit);
			firstHit = false;
		}
	}
#endif
}
