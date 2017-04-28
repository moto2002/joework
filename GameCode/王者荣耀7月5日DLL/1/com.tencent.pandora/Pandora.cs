using com.tencent.pandora.MiniJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.tencent.pandora
{
	public class Pandora
	{
		public static readonly Pandora Instance = new Pandora();

		private GameObject pandoraGameObject;

		private PandoraImpl pandoraImpl;

		private Logger logger;

		private GameObject panelParent;

		private GameObject pandoraParent;

		private int panelBaseDepth = 100;

		private string logPath = string.Empty;

		private string cachePath = string.Empty;

		private string imgPath = string.Empty;

		private string cookiePath = string.Empty;

		private string tempPath = string.Empty;

		private string streamingAssetsPath = string.Empty;

		private string[] fontResources = new string[]
		{
			"Pandora/FontPrefabs/usefont"
		};

		private string kSDKVersion = "YXZJ-Android-V13";

		private Action<Dictionary<string, string>> callbackForGame;

		private Func<GameObject, int, int, int> callbackForGameDjImage;

		private UserData userData = new UserData();

		private bool isInited;

		public void Init()
		{
			if (this.isInited)
			{
				return;
			}
			string temporaryCachePath = Application.temporaryCachePath;
			this.logPath = temporaryCachePath + "/Pandora/logs";
			this.cachePath = temporaryCachePath + "/Pandora/cache";
			this.imgPath = this.cachePath + "/imgs";
			this.cookiePath = this.cachePath + "/cookies";
			this.tempPath = temporaryCachePath + "/Pandora/temp";
			this.streamingAssetsPath = Application.streamingAssetsPath + "/Pandora";
			this.pandoraGameObject = new GameObject("Pandora GameObject");
			Object.DontDestroyOnLoad(this.pandoraGameObject);
			this.pandoraImpl = this.pandoraGameObject.AddComponent<PandoraImpl>();
			this.logger = this.pandoraGameObject.AddComponent<Logger>();
			this.pandoraGameObject.AddComponent<MidasUtil>();
			this.pandoraImpl.Init();
			this.isInited = true;
		}

		public void SetPanelParent(GameObject thePanelParent)
		{
			Logger.DEBUG(string.Empty);
			this.panelParent = thePanelParent;
		}

		public void SetPandoraParent(GameObject thePandoraParent)
		{
			Logger.DEBUG(string.Empty);
			this.pandoraParent = thePandoraParent;
			if (this.pandoraGameObject != null)
			{
				this.pandoraGameObject.transform.parent = this.pandoraParent.transform;
			}
		}

		public void SetPanelBaseDepth(int depth)
		{
			Logger.DEBUG(depth.ToString());
			this.panelBaseDepth = depth;
		}

		public void SetCallback(Action<Dictionary<string, string>> action)
		{
			Logger.DEBUG(string.Empty);
			this.callbackForGame = action;
		}

		public void SetGetDjImageCallback(Func<GameObject, int, int, int> action)
		{
			Logger.DEBUG(string.Empty);
			this.callbackForGameDjImage = action;
		}

		public int GetGameDjImage(GameObject go, int djType, int djID)
		{
			Logger.DEBUG(string.Empty);
			int result = -1;
			if (this.callbackForGameDjImage != null)
			{
				result = this.callbackForGameDjImage.Invoke(go, djType, djID);
			}
			return result;
		}

		public void CallGame(string strCmdJson)
		{
			Logger.DEBUG(strCmdJson);
			try
			{
				Dictionary<string, object> dictionary = Json.Deserialize(strCmdJson) as Dictionary<string, object>;
				Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
				using (Dictionary<string, object>.Enumerator enumerator = dictionary.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, object> current = enumerator.get_Current();
						if (current.get_Value() == null)
						{
							Logger.DEBUG(strCmdJson);
							dictionary2.set_Item(current.get_Key(), string.Empty);
						}
						else
						{
							dictionary2.set_Item(current.get_Key(), current.get_Value() as string);
						}
					}
				}
				if (this.callbackForGame != null)
				{
					this.callbackForGame.Invoke(dictionary2);
				}
			}
			catch (Exception ex)
			{
				Logger.ERROR(ex.get_Message() + ":" + ex.get_StackTrace());
			}
		}

		public void CallGame(Dictionary<string, string> cmdDict)
		{
			Logger.DEBUG(string.Empty);
			if (this.callbackForGame != null)
			{
				this.callbackForGame.Invoke(cmdDict);
			}
		}

		public void SetUserData(Dictionary<string, string> dictPara)
		{
			string formatMsg = Json.Serialize(dictPara);
			Logger.DEBUG(formatMsg);
			if (this.userData.IsRoleEmpty())
			{
				this.userData.Assgin(dictPara);
				if (this.pandoraImpl != null)
				{
					this.pandoraImpl.Bootup();
				}
			}
			else
			{
				this.userData.Refresh(dictPara);
			}
		}

		public void Logout()
		{
			Logger.DEBUG(string.Empty);
			this.userData.Clear();
			if (this.pandoraImpl != null)
			{
				this.pandoraImpl.LogOut();
			}
		}

		public bool GetTotalSwitch()
		{
			Logger.DEBUG(string.Empty);
			return this.pandoraImpl != null && this.pandoraImpl.GetTotalSwitch();
		}

		public void Do(Dictionary<string, string> cmdDict)
		{
			string formatMsg = Json.Serialize(cmdDict);
			Logger.DEBUG(formatMsg);
			if (this.pandoraImpl != null)
			{
				this.pandoraImpl.Do(cmdDict);
			}
		}

		public void CloseAllPanel()
		{
			Logger.DEBUG(string.Empty);
			if (this.pandoraImpl != null)
			{
				this.pandoraImpl.CloseAllPanel();
			}
		}

		public string GetLogPath()
		{
			return this.logPath;
		}

		public string GetCachePath()
		{
			return this.cachePath;
		}

		public string GetImgPath()
		{
			return this.imgPath;
		}

		public string GetCookiePath()
		{
			return this.cookiePath;
		}

		public string GetTempPath()
		{
			return this.tempPath;
		}

		public string GetStreamingAssetsPath()
		{
			return this.streamingAssetsPath;
		}

		public string[] GetFontResources()
		{
			return this.fontResources;
		}

		public UserData GetUserData()
		{
			return this.userData;
		}

		public string GetSDKVersion()
		{
			return this.kSDKVersion;
		}

		public GameObject GetGameObject()
		{
			return this.pandoraGameObject;
		}

		public GameObject GetPanelParent()
		{
			if (this.panelParent == null)
			{
				GameObject x = GameObject.Find("UI Root");
				if (x != null)
				{
					this.panelParent = x;
				}
			}
			return this.panelParent;
		}

		public LuaScriptMgr GetLuaScriptMgr()
		{
			return this.pandoraImpl.GetLuaScriptMgr();
		}

		public ResourceMgr GetResourceMgr()
		{
			return this.pandoraImpl.GetResourceMgr();
		}

		public NetLogic GetNetLogic()
		{
			return this.pandoraImpl.GetNetLogic();
		}

		public PandoraImpl GetPandoraImpl()
		{
			return this.pandoraImpl;
		}

		public Logger GetLogger()
		{
			return this.logger;
		}

		public int GetPanelBaseDepth()
		{
			return this.panelBaseDepth;
		}
	}
}
