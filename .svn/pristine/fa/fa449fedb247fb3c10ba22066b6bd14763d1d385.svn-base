//一般在游戏开始前，会播放一个CG动画，点击屏幕救你可以关闭了
	
	
	
	
	//事先装个QuickTime
	
using UnityEngine;
using System.Collections;

/// <summary>
/// 视频文件放在StreamingAssets文件夹下
/// </summary>
public class TestHeadMoiveOnMobile : MonoBehaviour
{
	void Start()
	{
		Handheld.PlayFullScreenMovie("test.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
	}
	
	void Update()
	{
		
	}
	
	void OnGUI()
	{
		//视频播放时触摸屏幕视频关闭
		if (GUI.Button(new Rect(20, 10, 300, 50), "PLAY ContronlMode.CancleOnTouch"))
		{
			Handheld.PlayFullScreenMovie("test.mp4",Color.black,FullScreenMovieControlMode.CancelOnInput);
		}
		
		//视频播放时弹出控件，控制视频暂停，播放，全屏等
		if (GUI.Button(new Rect(20, 90, 200, 50), "PLAY ControlMode.Full"))
		{
			Handheld.PlayFullScreenMovie("test.mp4", Color.black, FullScreenMovieControlMode.Full);
		}
		
		//视频播放时无法停止，当其播放完一次 后自动关闭
		if (GUI.Button(new Rect(20, 170, 200, 50), "PLAY ControlMode.Hidden"))
		{
			Handheld.PlayFullScreenMovie("test.mp4", Color.black, FullScreenMovieControlMode.Hidden);
		}
		
		//视频播放时弹出控件，可控制播放进度
		if (GUI.Button(new Rect(20, 250, 200, 50), "PLAY ControlMode.Minimal"))
		{
			Handheld.PlayFullScreenMovie("test.mp4", Color.black, FullScreenMovieControlMode.Minimal);
		}
		
		if (GUI.Button(new Rect(20, 320, 200, 50), "PLAY ControlMode.Minimal"))
		{
			Application.LoadLevel("main");
		}
	}
}

