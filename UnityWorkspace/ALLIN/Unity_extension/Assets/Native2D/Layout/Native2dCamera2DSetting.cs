using UnityEngine;
using System.Collections;

/// <summary>
/// 设置2d Camera 适配方式，由于unity默认是FixedHeight ，所以此设置只针对FixedWidth
/// </summary>
[ExecuteInEditMode]
public class Native2dCamera2DSetting : MonoBehaviour
{
	
	public enum FixType
	{
		FixWidth ,
		FixHeight
	}
	
	//设计宽高.
	public int designWidth, designHeight;
	public FixType fixType;
	
	void Awake()
	{
		if (fixType == FixType.FixWidth)
		{
			int currentWidth = Screen.width;
			int currentHeight = Screen.height;
			Camera cam = GetComponent<Camera>();
			
			float standard_aspect = 1.0f * designWidth / designHeight;
			float device_aspect = 1.0f * currentWidth / currentHeight;
			
			cam.orthographicSize = standard_aspect / device_aspect * designHeight*0.005f ;
		}
	}
	
	#if UNITY_EDITOR
	void LateUpdate()
	{
		if(Application.platform== RuntimePlatform.OSXEditor||Application.platform== RuntimePlatform.WindowsEditor){
			if (fixType == FixType.FixWidth)
			{
				int currentWidth = Screen.width;
				int currentHeight = Screen.height;
				Camera cam = GetComponent<Camera>();

				float standard_aspect = 1.0f * designWidth / designHeight;
				float device_aspect = 1.0f * currentWidth / currentHeight;

				cam.orthographicSize = standard_aspect / device_aspect * designHeight * 0.005f;
			}
		}
	}
	#endif
}
