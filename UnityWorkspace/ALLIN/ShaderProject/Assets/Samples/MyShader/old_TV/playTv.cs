using UnityEngine;
using System.Collections;

public class playTv : MonoBehaviour
{
	public MovieTexture movie;

	void Start () 
	{
		this.GetComponent<Renderer>().material.SetTexture("_TVTex",movie);
		movie.loop = true;
		movie.Play();
	}
}
