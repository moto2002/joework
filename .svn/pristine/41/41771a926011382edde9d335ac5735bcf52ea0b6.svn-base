using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour 
{
	public Transform target;
	public Vector3 direction;
	Vector3 targetVec;
	float speed = 10.0f;
	bool open= false;
	float rotateSpeed = 90.0f;

	void Start ()
	{

	}
	

	void Update () 
	{
		if (this.open) 
		{
			targetVec = new Vector3 (this.target.position.x,this.target.position.y-0.5f,this.target.position.z);

			//方向
			this.direction = (targetVec - this.transform.position).normalized;
			
			//画线
			Debug.DrawLine (targetVec,this.transform.position);

			#region Time Debug
			Debug.Log("--------------------------");
			Debug.Log("Time.deltTime:"+Time.deltaTime);
			Debug.Log("Time.time:"+ Time.time);
			Debug.Log("Time.realtimeSinceStartup:"+Time.realtimeSinceStartup);
			Debug.Log("--------------------------");
			#endregion

			#region 朝向目标物体
			//第三个参数t：t取大于1的值并没有什么用，和值为1一样的效果。可以看出，t可能是个规范化的值,就是在0-1之间
			this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (this.direction), Time.deltaTime * speed);
			#endregion


			#region 和目标物体一样的朝向
//			Quaternion wantedRotation=Quaternion.FromToRotation(transform.position,target.position);
//			float _angle =Quaternion.Angle(transform.rotation,wantedRotation);
//			Debug.Log("angle:"+_angle);
//			float t=this.rotateSpeed /_angle* Time.deltaTime;
//			this.transform.rotation=Quaternion.Slerp(transform.rotation, target.rotation,t);
			#endregion
		


		}

	}

	void OnGUI()
	{
		if (GUILayout.Button ("朝向目标--开关")) 
		{
			this.open = !this.open;
		}
	}
}
