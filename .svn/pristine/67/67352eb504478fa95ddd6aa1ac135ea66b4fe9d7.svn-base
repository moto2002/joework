using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// untiy3d Native2d 屏幕坐标工具类.
/// 这里面的Camer.main是Native2d中的相机.
/// author:zhouzhanglin
/// </summary>
public class Native2dScreenUtil
{
	
	/// <summary>
	/// Native2D中屏幕的宽.
	/// </summary>
	/// <returns>The screen width.</returns>
	public static float GetScreenWidth(){
		return -Camera.main.ScreenToWorldPoint(new Vector3(-Screen.width / 2f, 0, 0)).x;
	}
	
	/// <summary>
	/// Native2D中屏幕的高.
	/// </summary>
	/// <returns>The screen height.</returns>
	public static float GetScreenHeight(){
		return -Camera.main.ScreenToWorldPoint(new Vector3(0, -Screen.height / 2f, 0)).y;
	}

	public static Vector2 GetScreenCenter(){
		return (Vector2)Camera.main.transform.position;
	}
	public static Vector2 GetScreenTopLeft(){
		Vector3 camPos = Camera.main.transform.position;
		return new Vector2((-GetScreenWidth()+camPos.x)/2,(GetScreenHeight()+camPos.y*3f)/2);
	}
	public static Vector2 GetScreenTopCenter(){
		Vector3 camPos = Camera.main.transform.position;
		return new Vector2( camPos.x,(GetScreenHeight()+camPos.y*3f)/2);
	}
	public static Vector2 GetScreenTopRight(){
		Vector3 camPos = Camera.main.transform.position;
		return new Vector2( (GetScreenWidth()+camPos.x*3)/2,(GetScreenHeight()+camPos.y*3f)/2);
	}
	public static Vector2 GetScreenBottomLeft(){
		Vector3 camPos = Camera.main.transform.position;
		return new Vector2( (-GetScreenWidth()+camPos.x)/2, (-GetScreenHeight()+camPos.y)/2);
	}
	public static Vector2 GetScreenBottomCenter(){
		Vector3 camPos = Camera.main.transform.position;
		return new Vector2( camPos.x,(-GetScreenHeight()+camPos.y)/2);
	}
	public static Vector2 GetScreenBottomRight(){
		Vector3 camPos = Camera.main.transform.position;
		return new Vector2( (GetScreenWidth()+camPos.x*3)/2,(-GetScreenHeight()+camPos.y)/2);
	}
	public static Vector2 GetScreenMiddleLeft(){
		Vector3 camPos = Camera.main.transform.position;
		return new Vector2( (-GetScreenWidth()+camPos.x)/2,camPos.y);
	}
	public static Vector2 GetScreenMiddleRight(){
		Vector3 camPos = Camera.main.transform.position;
		return new Vector2( (GetScreenWidth()+camPos.x*3)/2,camPos.y);
	}
}

