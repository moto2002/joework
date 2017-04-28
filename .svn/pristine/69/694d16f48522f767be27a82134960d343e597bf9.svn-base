using UnityEngine;
using System.Collections;

/// <summary>
/// 背景铺满屏幕.自动缩放到合适的大小.
/// author:zhouzhanglin
/// </summary>
[ExecuteInEditMode]
public class Native2dAdjustBackground : MonoBehaviour
{
	// Use this for initialization
	void Start () {
		SetScale();
	}
	
	#if UNITY_EDITOR
	void LateUpdate()
	{
		if(Application.platform== RuntimePlatform.OSXEditor||Application.platform== RuntimePlatform.WindowsEditor){
			SetScale();
		}
	}
	#endif
	
	void SetScale()
	{
		SpriteRenderer render2D = GetComponent<SpriteRenderer>();
		
		Vector3 size = render2D.bounds.size;
		size.x /= transform.localScale.x;
		size.y /= transform.localScale.y;
		
		float screenWidth = Native2dScreenUtil.GetScreenWidth();
		float screenHeight = Native2dScreenUtil.GetScreenHeight();
		Transform mtran = transform;
		
		if (size.x / size.y > screenWidth / screenHeight)
		{
			float scale = screenHeight / size.y;
			Vector3 localScale = mtran.localScale;
			localScale.x = scale;
			localScale.y = scale;
			mtran.localScale = localScale;
		}
		else if (size.x / size.y <= screenWidth / screenHeight)
		{
			float scale = screenWidth / size.x;
			Vector3 localScale = mtran.localScale;
			localScale.x = scale;
			localScale.y = scale;
			mtran.localScale = localScale;
		}
	}
}
