using UnityEngine;
using System.Collections;

public class Water3Time : MonoBehaviour
{
    private Material mat;

	// Use this for initialization
	void Start ()
	{
	    mat = this.transform.GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
	mat.SetFloat("times",Time.realtimeSinceStartup);
	}
}
