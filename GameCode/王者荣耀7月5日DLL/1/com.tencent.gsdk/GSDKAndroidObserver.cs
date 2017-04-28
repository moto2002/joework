using System;
using UnityEngine;

namespace com.tencent.gsdk
{
	internal class GSDKAndroidObserver : AndroidJavaProxy
	{
		private static GSDKAndroidObserver instance = new GSDKAndroidObserver();

		public static GSDKAndroidObserver Instance
		{
			get
			{
				return GSDKAndroidObserver.instance;
			}
		}

		private GSDKAndroidObserver() : base("com.tencent.gsdk.GSDKObserver")
		{
		}

		public void OnStartSpeedNotify(int type, int flag, string desc)
		{
			StartSpeedRet ret = new StartSpeedRet(type, flag, desc);
			GSDK.notify(ret);
		}
	}
}
