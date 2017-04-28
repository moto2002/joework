using Assets.Scripts.UI;
using System;
using UnityEngine;

public class NetworkAccelerator
{
	private static bool s_inited;

	private static bool s_started;

	private static string key = "827006BE-64F7-4082-B252-33ACF328A3A5";

	private static int KEY_GET_NETDELAY = 100;

	private static int KEY_GET_ACCEL_STAT = 102;

	private static int KEY_GET_ACCEL_EFFECT = 107;

	private static int KEY_LOG_LEVLE = 308;

	public static string PLAYER_PREF_NET_ACC = "NET_ACC";

	public static string PLAYER_PREF_AUTO_NET_ACC = "AUTO_NET_ACC";

	public static string PLAYER_IS_USE_ACC = "PLAYER_IS_USE_ACC";

	public static int LOG_LEVEL_DEBUG = 1;

	public static int LOG_LEVEL_INFO = 2;

	public static int LOG_LEVEL_WARNING = 3;

	public static int LOG_LEVEL_ERROR = 4;

	public static int LOG_LEVEL_FATAL = 5;

	private static bool s_enabled;

	private static bool s_enabledPrepare;

	public static bool EnableForGSDK
	{
		get
		{
			return NetworkAccelerator.s_enabled;
		}
	}

	public static bool SettingUIEnbaled
	{
		get
		{
			return NetworkAccelerator.s_enabledPrepare;
		}
	}

	public static bool enabled
	{
		get
		{
			return NetworkAccelerator.s_enabled;
		}
	}

	public static bool started
	{
		get
		{
			return NetworkAccelerator.s_started;
		}
	}

	public static void Init(bool bforceUse = false)
	{
		Debug.Log("Begin Network Acc");
		Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.NetworkAccelerator_TurnOn, new CUIEventManager.OnUIEventHandler(NetworkAccelerator.OnEventTurnOn));
		Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.NetworkAccelerator_Ignore, new CUIEventManager.OnUIEventHandler(NetworkAccelerator.OnEventTurnIgore));
		NetworkAccelerator.s_enabledPrepare = true;
		if (!bforceUse)
		{
			if (!NetworkAccelerator.IsUseACC())
			{
				Debug.Log("NetAcc player close acc");
				return;
			}
		}
		else
		{
			NetworkAccelerator.SetUseACC(true);
		}
		NetworkAccelerator.s_enabled = true;
		Debug.Log("NetAcc key:" + NetworkAccelerator.key);
		Debug.Log("NetAcc enable & java begin");
		AndroidJavaObject GMContext = null;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			GMContext = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
		AndroidJavaClass GMClass = new AndroidJavaClass("com.subao.gamemaster.GameMaster");
		if (GMClass != null)
		{
			GMContext.Call("runOnUiThread", new object[]
			{
				delegate
				{
					int num = GMClass.CallStatic<int>("init", new object[]
					{
						GMContext,
						1,
						NetworkAccelerator.key,
						"KingsGlory",
						"libapollo.so",
						13001
					});
					if (num >= 0)
					{
						Debug.Log("Initialize GameMaster Success!");
						NetworkAccelerator.s_inited = true;
						NetworkAccelerator.ChangeLogLevel(NetworkAccelerator.LOG_LEVEL_ERROR);
					}
					else
					{
						Debug.LogError("Initialize GameMaster Fail!, ret:" + num);
					}
				}
			});
		}
	}

	public static bool IsUseACC()
	{
		return PlayerPrefs.GetInt(NetworkAccelerator.PLAYER_IS_USE_ACC, 0) <= 0;
	}

	public static void SetUseACC(bool bUse)
	{
		PlayerPrefs.SetInt(NetworkAccelerator.PLAYER_IS_USE_ACC, (!bUse) ? 1 : 0);
		PlayerPrefs.Save();
	}

	public static bool IsNetAccConfigOpen()
	{
		return PlayerPrefs.GetInt(NetworkAccelerator.PLAYER_PREF_NET_ACC, 0) > 0;
	}

	public static bool IsAutoNetAccConfigOpen()
	{
		return PlayerPrefs.GetInt(NetworkAccelerator.PLAYER_PREF_AUTO_NET_ACC, 0) > 0;
	}

	public static void SetNetAccConfig(bool open)
	{
		if (open)
		{
			NetworkAccelerator.Start();
		}
		else
		{
			NetworkAccelerator.Stop();
		}
		PlayerPrefs.SetInt(NetworkAccelerator.PLAYER_PREF_NET_ACC, (!open) ? 0 : 1);
		PlayerPrefs.Save();
	}

	public static void SetAutoNetAccConfig(bool open)
	{
		PlayerPrefs.SetInt(NetworkAccelerator.PLAYER_PREF_AUTO_NET_ACC, (!open) ? 0 : 1);
		PlayerPrefs.Save();
	}

	private static void OnEventTurnOn(CUIEvent uiEvent)
	{
		NetworkAccelerator.SetNetAccConfig(true);
		Singleton<ApolloHelper>.GetInstance().ApolloRepoertEvent("NetACCturnOK", null, true);
	}

	private static void OnEventTurnIgore(CUIEvent uiEvent)
	{
		Singleton<ApolloHelper>.GetInstance().ApolloRepoertEvent("NetACCIgore", null, true);
	}

	public static void ChangeLogLevel(int level)
	{
		if (!NetworkAccelerator.s_inited)
		{
			return;
		}
		long num = (long)Mathf.Clamp(level, 1, 5);
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.subao.gamemaster.GameMaster");
		if (androidJavaClass != null)
		{
			androidJavaClass.CallStatic("setLong", new object[]
			{
				NetworkAccelerator.KEY_LOG_LEVLE,
				num
			});
		}
	}

	private static bool Start()
	{
		if (!NetworkAccelerator.s_inited)
		{
			return NetworkAccelerator.s_started;
		}
		if (!NetworkAccelerator.enabled)
		{
			return false;
		}
		if (NetworkAccelerator.s_started)
		{
			return NetworkAccelerator.s_started;
		}
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.subao.gamemaster.GameMaster");
		bool flag = false;
		if (androidJavaClass != null)
		{
			flag = androidJavaClass.CallStatic<bool>("start", new object[]
			{
				0
			});
		}
		if (flag)
		{
			Debug.Log("Start GameMaster Success!");
			NetworkAccelerator.s_started = true;
		}
		else
		{
			Debug.LogError("Start GameMaster Fail!");
		}
		return NetworkAccelerator.s_started;
	}

	private static bool Stop()
	{
		if (!NetworkAccelerator.s_inited)
		{
			return NetworkAccelerator.s_started;
		}
		if (!NetworkAccelerator.s_started)
		{
			return NetworkAccelerator.s_started;
		}
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.subao.gamemaster.GameMaster");
		bool flag = false;
		if (androidJavaClass != null)
		{
			flag = androidJavaClass.CallStatic<bool>("stop", new object[0]);
		}
		if (flag)
		{
			Debug.Log("Stop GameMaster Success!");
			NetworkAccelerator.ClearUDPCache();
			NetworkAccelerator.s_started = false;
		}
		else
		{
			Debug.LogError("Stop GameMaster Fail!");
		}
		return NetworkAccelerator.s_started;
	}

	public static void SetEchoPort(int port)
	{
		Debug.Log("Set UD Echo Port to :" + port);
		if (!NetworkAccelerator.s_inited)
		{
			return;
		}
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.subao.gamemaster.GameMaster");
		if (androidJavaClass != null)
		{
			androidJavaClass.CallStatic("setUdpEchoPort", new object[]
			{
				port
			});
		}
		Debug.Log("Set UD Echo Port Success!");
	}

	public static void setRecommendationGameIP(string ip, int port)
	{
		Debug.Log(string.Concat(new object[]
		{
			"setRecommendationGameIP :",
			ip,
			", port :",
			port
		}));
		if (!NetworkAccelerator.s_inited)
		{
			return;
		}
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.subao.gamemaster.GameMaster");
		if (androidJavaClass != null)
		{
			androidJavaClass.CallStatic("setRecommendationGameIP", new object[]
			{
				ip,
				port
			});
		}
		Debug.Log("Set setRecommendationGameIP Success!");
	}

	public static void OnNetDelay(int millis)
	{
		if (!NetworkAccelerator.s_inited || NetworkAccelerator.started)
		{
			return;
		}
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.subao.gamemaster.GameMaster");
		if (androidJavaClass != null)
		{
			androidJavaClass.CallStatic("onNetDelay", new object[]
			{
				millis
			});
		}
	}

	public static bool getAccelRecommendation()
	{
		if (!NetworkAccelerator.s_inited || !NetworkAccelerator.enabled || NetworkAccelerator.started)
		{
			return false;
		}
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.subao.gamemaster.GameMaster");
		bool flag = false;
		if (androidJavaClass != null)
		{
			flag = (androidJavaClass.CallStatic<int>("getAccelRecommendation", new object[0]) == 1);
			Debug.Log("getAccelRecommendation :" + flag);
		}
		return flag;
	}

	public static void ClearUDPCache()
	{
		if (!NetworkAccelerator.s_inited)
		{
			return;
		}
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.subao.gamemaster.GameMaster");
		if (androidJavaClass != null)
		{
			androidJavaClass.CallStatic("clearUDPCache", new object[0]);
		}
	}

	public static long GetDelay()
	{
		if (!NetworkAccelerator.s_started)
		{
			return -1L;
		}
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.subao.gamemaster.GameMaster");
		long result = -1L;
		if (androidJavaClass != null)
		{
			result = androidJavaClass.CallStatic<long>("getLong", new object[]
			{
				NetworkAccelerator.KEY_GET_NETDELAY
			});
		}
		return result;
	}

	public static string GetEffect()
	{
		if (!NetworkAccelerator.s_started)
		{
			return null;
		}
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.subao.gamemaster.GameMaster");
		string result = null;
		if (androidJavaClass != null)
		{
			result = androidJavaClass.CallStatic<string>("getString", new object[]
			{
				NetworkAccelerator.KEY_GET_ACCEL_EFFECT
			});
		}
		return result;
	}

	public static int GetNetType()
	{
		int result = -1;
		if (!NetworkAccelerator.s_inited)
		{
			return result;
		}
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.subao.gamemaster.GameMaster");
		if (androidJavaClass != null)
		{
			result = androidJavaClass.CallStatic<int>("getCurrentConnectionType", new object[0]);
		}
		return result;
	}

	public static bool isAccerating()
	{
		if (!NetworkAccelerator.s_started)
		{
			return false;
		}
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.subao.gamemaster.GameMaster");
		bool result = false;
		if (androidJavaClass != null)
		{
			result = androidJavaClass.CallStatic<bool>("isUDPProxy", new object[0]);
		}
		return result;
	}
}
