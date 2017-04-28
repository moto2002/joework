using UnityEngine;
using System.Collections;

public class Brush
{
	static Brush inst;
	public static Brush Inst()
	{
		if (inst == null)
			inst = new Brush ();
		return inst;
	}

	bool unBrush = false;

	FindPath fp = new FindPath();

	public void Brushing(Vector3 inputPos)
	{
		unBrush = false;
		Comm (inputPos);
	}
	public void UnBrushing(Vector3 inputPos)
	{
		unBrush = true;
		Comm (inputPos);
	}


	private void Comm(Vector3 hitPos)
	{
		Ray ray;
		RaycastHit hit;
		LayerMask mask;
		ray = Camera.main.ScreenPointToRay (hitPos);

		if(Physics.Raycast(ray,out hit))
		{		
			Node node = fp.GetFromPositon(hit.point);
		    //Debug.Log(node.canWalk);

			GameObject go = node.cube;
		

			if(!unBrush)
			{
				//if(node.canWalk != true)
				//{
					node.canWalk = true;
					go.GetComponent<MeshRenderer>().material.color = Color.green;
				//}

			}
			else
			{
				//if(node.canWalk!=false)
				//{
					node.canWalk = false;
					go.GetComponent<MeshRenderer>().material.color = Color.red;
				//}
		
			}

		}
	}

}
