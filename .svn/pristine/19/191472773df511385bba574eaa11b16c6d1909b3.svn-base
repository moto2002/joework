using UnityEngine;
using System.Collections;

/// <summary>
/// unity2D的锚点.
/// author:zhouzhanglin
/// </summary>
[ExecuteInEditMode]
public class Native2dAnchor : MonoBehaviour {
	
	public enum Position
	{
		CENTER,
		TOP,
		BOTTOM,
		LEFT,
		RIGHT,
		TOP_LEFT,
		TOP_RIGHT,
		BOTTOM_LEFT,
		BOTTOM_RIGHT
	}
	
	public Position position=Position.CENTER;
	public bool updateAlways = false;
	
	IEnumerator Start()
	{
		SetPosition();
		yield return new WaitForEndOfFrame();
		if(!updateAlways)
			SetPosition();
	}
	
	void LateUpdate()
	{
		if(Application.platform== RuntimePlatform.OSXEditor||Application.platform== RuntimePlatform.WindowsEditor){
			SetPosition();
		}else if(updateAlways){
			SetPosition();
		}
	}
	
	void SetPosition()
	{
		switch (position)
		{
		case Position.CENTER:
			transform.position = Native2dScreenUtil.GetScreenCenter();
			break;
		case Position.TOP:
			transform.position = Native2dScreenUtil.GetScreenTopCenter();
			break;
		case Position.BOTTOM:
			transform.position = Native2dScreenUtil.GetScreenBottomCenter();
			break;
		case Position.LEFT:
			transform.position = Native2dScreenUtil.GetScreenMiddleLeft();
			break;
		case Position.RIGHT:
			transform.position = Native2dScreenUtil.GetScreenMiddleRight();
			break;
		case Position.TOP_LEFT:
			transform.position = Native2dScreenUtil.GetScreenTopLeft();
			break;
		case Position.TOP_RIGHT:
			transform.position = Native2dScreenUtil.GetScreenTopRight();
			break;
		case Position.BOTTOM_LEFT:
			transform.position = Native2dScreenUtil.GetScreenBottomLeft();
			break;
		case Position.BOTTOM_RIGHT:
			transform.position = Native2dScreenUtil.GetScreenBottomRight();
			break;
		}
	}
}
