using UnityEngine;
using System.Collections;

public class TestCreate : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		HeroObject heroObj = RoleObjectController.Instance.CreateHeroObject ();
		heroObj.InitLogic (new Vector3 (100,100,100),new Vector3(0,45,0),Vector3.one);
		heroObj.Init (new ViewHeroObject ());


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
