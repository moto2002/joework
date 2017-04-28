using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace com.tencent.gsdk
{
	public class GSDK
	{
		public delegate void GSDKObserver(StartSpeedRet ret);

		private static AndroidJavaClass sGSDKPlatformClass;

		private static event GSDK.GSDKObserver sSpeedNotifyEvent
		{
			[MethodImpl(32)]
			add
			{
				GSDK.sSpeedNotifyEvent = (GSDK.GSDKObserver)Delegate.Combine(GSDK.sSpeedNotifyEvent, value);
			}
			[MethodImpl(32)]
			remove
			{
				GSDK.sSpeedNotifyEvent = (GSDK.GSDKObserver)Delegate.Remove(GSDK.sSpeedNotifyEvent, value);
			}
		}

		public static void Init(string appid, bool debug)
		{
			GSDKUtils.Logger("android init");
			GSDK.sGSDKPlatformClass = new AndroidJavaClass("com.tencent.gsdk.GSDKPlatform");
			GSDK.sGSDKPlatformClass.CallStatic("GSDKInit", new object[]
			{
				appid,
				debug
			});
			GSDK.sGSDKPlatformClass.CallStatic("GSDKSetObserver", new object[]
			{
				GSDKAndroidObserver.Instance
			});
		}

		public static void GoBack()
		{
			GSDK.sGSDKPlatformClass.CallStatic("GSDKGoBack", new object[0]);
		}

		public static void GoFront()
		{
			GSDK.sGSDKPlatformClass.CallStatic("GSDKGoFront", new object[0]);
		}

		public static void SetUserName(int plat, string openid)
		{
			GSDK.sGSDKPlatformClass.CallStatic("GSDKSetUserName", new object[]
			{
				plat,
				openid
			});
		}

		public static void StartSpeed(string vip, int vport, int htype, string hookModules, int zoneid, int reserved)
		{
			GSDK.sGSDKPlatformClass.CallStatic("GSDKStartSpeed", new object[]
			{
				vip,
				vport,
				htype,
				hookModules,
				zoneid,
				reserved
			});
		}

		public static void EndSpeed(string vip, int vport)
		{
			GSDK.sGSDKPlatformClass.CallStatic("GSDKEndSpeed", new object[]
			{
				vip,
				vport
			});
		}

		public static SpeedInfo GetSpeedInfo(string vip, int vport)
		{
			AndroidJavaObject androidJavaObject = GSDK.sGSDKPlatformClass.CallStatic<AndroidJavaObject>("GSDKGetSpeedInfo", new object[]
			{
				vip,
				vport
			});
			return new SpeedInfo
			{
				state = androidJavaObject.Get<int>("state"),
				netType = androidJavaObject.Get<int>("netType"),
				delay = androidJavaObject.Get<int>("delay")
			};
		}

		public static void SetObserver(GSDK.GSDKObserver d)
		{
			if (d == null)
			{
				return;
			}
			GSDK.sSpeedNotifyEvent = (GSDK.GSDKObserver)Delegate.Combine(GSDK.sSpeedNotifyEvent, d);
		}

		internal static void notify(StartSpeedRet ret)
		{
			if (GSDK.sSpeedNotifyEvent != null)
			{
				GSDK.sSpeedNotifyEvent(ret);
			}
		}
	}
}
