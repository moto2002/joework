using System;
using System.Collections.Generic;
using UnityEngine;

public class GameJoy : Singleton<GameJoy>
{
	public enum VideoQuality
	{
		High,
		Low
	}

	public static long getSystemCurrentTimeMillis
	{
		get
		{
			return GameJoySDK.getSystemCurrentTimeMillis();
		}
	}

	public bool isShowed
	{
		get
		{
			return GameJoySDK.instance.IsShowed();
		}
	}

	public bool isRecording
	{
		get
		{
			return GameJoySDK.instance.IsRecording();
		}
	}

	public string sdkVersion
	{
		get
		{
			return string.Empty;
		}
	}

	public bool isRecordingMoments
	{
		get
		{
			return GameJoySDK.instance.isRecordingMoments();
		}
	}

	public override void Init()
	{
		base.Init();
		if (GameJoySDK.SetupGameJoySDK() == null)
		{
			Debug.LogError("GameRecorder Failed Setup GameRecorder!");
		}
	}

	public static void CheckRecorderAvailability()
	{
		try
		{
			GameJoySDK.CheckSupportRecord();
		}
		catch (Exception var_0_0A)
		{
			GameJoySDK.Log("GameRecorder call Exception ");
			GameJoy.onUnSupport();
		}
	}

	public static void onSupport()
	{
		Singleton<EventRouter>.instance.BroadCastEvent<bool>(EventID.GAMEJOY_AVAILABILITY_CHECK_RESULT, true);
	}

	public static void onUnSupport()
	{
		Singleton<EventRouter>.instance.BroadCastEvent<bool>(EventID.GAMEJOY_AVAILABILITY_CHECK_RESULT, false);
	}

	public static void OnStartMomentsRecording(bool bResult)
	{
		Singleton<EventRouter>.instance.BroadCastEvent<bool>(EventID.GAMEJOY_STARTRECORDING_RESULT, bResult);
	}

	public static void OnStopMomentsRecording(long duration)
	{
		Singleton<EventRouter>.instance.BroadCastEvent<long>(EventID.GAMEJOY_STOPRECORDING_RESULT, duration);
	}

	public void ShowRecorder()
	{
		GameJoySDK.instance.showGameJoyRecorder();
	}

	public void HideRecorder()
	{
		GameJoySDK.instance.dismissGameJoyRecorder();
	}

	public void SetDefaultStartPosition(float x, float y)
	{
		if (y >= 0f && y <= 1f)
		{
			y = 1f - y;
		}
		GameJoySDK.instance.setDefaultStartPosition(x, y);
	}

	public void SetDefaultUploadShareDialogPosition(float x, float y)
	{
		if (y >= 0f && y <= 1f)
		{
			y = 1f - y;
		}
		GameJoySDK.instance.setUploadShareDialogDefaultPosition(x, y);
	}

	public void ShowVideoListDialog()
	{
		GameJoySDK.instance.showVideoListDialog();
	}

	public void CloseVideoListDialog()
	{
		GameJoySDK.instance.closeVideoListDialog();
	}

	public void SetVideoQuality(GameJoy.VideoQuality quality)
	{
		GameJoySDK.instance.setMomentVideoQuality((int)quality);
	}

	public void StartMomentsRecording()
	{
		GameJoySDK.instance.startMomentRecording();
	}

	public void EndMomentsRecording()
	{
		GameJoySDK.instance.endMomentRecording();
	}

	public void GenerateMomentsVideo(List<TimeStamp> timeStampList, string defaultGameTag, Dictionary<string, string> extraInfo)
	{
		GameJoySDK.instance.generateMomentVideo(timeStampList, defaultGameTag, extraInfo);
	}

	public static void checkSDKPermission()
	{
		GameJoy.OnFinishCheckSDKPremission(true);
	}

	public static void OnFinishCheckSDKPremission(bool bResult)
	{
		Debug.Log("OnFinishCheckSDKPremission: " + ((!bResult) ? "NO" : "YES"));
		Singleton<EventRouter>.instance.BroadCastEvent<bool>(EventID.GAMEJOY_SDK_PERMISSION_CHECK_RESULT, bResult);
	}
}
