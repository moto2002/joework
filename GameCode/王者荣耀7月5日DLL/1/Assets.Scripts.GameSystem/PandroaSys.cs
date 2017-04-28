using Apollo;
using Assets.Scripts.UI;
using com.tencent.pandora;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	public class PandroaSys : MonoSingleton<PandroaSys>
	{
		private bool m_bShowPopNew;

		private bool m_bInit;

		public bool m_bOpenWeixinZone;

		public bool m_bShowWeixinZone;

		private bool m_bstartOPenRedBox;

		public bool m_bShowBoxBtn;

		public bool m_bShowRedPoint;

		public bool ShowRedPoint
		{
			get
			{
				return this.m_bShowRedPoint;
			}
		}

		protected override void Init()
		{
			base.Init();
		}

		public void PausePandoraSys(bool bPause)
		{
			if (this.m_bInit)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				if (bPause)
				{
					dictionary.set_Item("type", "inMainSence");
					dictionary.set_Item("content", "0");
					Pandora.Instance.Do(dictionary);
				}
				else
				{
					dictionary.set_Item("type", "inMainSence");
					dictionary.set_Item("content", "1");
					Pandora.Instance.Do(dictionary);
				}
			}
		}

		public void InitSys()
		{
			if (!this.m_bInit)
			{
				this.InitEvent();
				Debug.Log("Pandora InitSys");
				this.m_bInit = true;
				this.m_bShowPopNew = false;
				this.m_bShowBoxBtn = false;
				this.m_bShowRedPoint = false;
				Pandora.Instance.Init();
				this.InitPara();
			}
		}

		public void UninitSys()
		{
			Debug.Log("Pandora UnInitSys");
			this.m_bInit = false;
			this.m_bShowPopNew = false;
			this.m_bShowBoxBtn = false;
			this.m_bShowRedPoint = false;
			this.m_bOpenWeixinZone = false;
			this.m_bShowWeixinZone = false;
			CUIEventManager instance = Singleton<CUIEventManager>.GetInstance();
			instance.RemoveUIEventListener(enUIEventID.Pandroa_ShowActBox, new CUIEventManager.OnUIEventHandler(this.OnShowActBox));
			Pandora.Instance.Logout();
		}

		private void InitEvent()
		{
			CUIEventManager instance = Singleton<CUIEventManager>.GetInstance();
			instance.AddUIEventListener(enUIEventID.Pandroa_ShowActBox, new CUIEventManager.OnUIEventHandler(this.OnShowActBox));
		}

		private void OnShowActBox(CUIEvent uiEvent)
		{
			if (this.m_bInit)
			{
				this.ShowActBox();
			}
		}

		public void ShowActiveActBoxBtn(CUIFormScript uiForm)
		{
			if (uiForm == null)
			{
				return;
			}
			if (this.m_bInit)
			{
				string text = Singleton<CTextManager>.GetInstance().GetText("pandroa_Btn_Text");
				Transform transform = uiForm.gameObject.transform.Find("Panel/PandroaBtn");
				if (transform)
				{
					if (this.m_bShowBoxBtn)
					{
						transform.gameObject.CustomSetActive(true);
					}
					else
					{
						transform.gameObject.CustomSetActive(false);
					}
					Transform transform2 = transform.Find("Hotspot");
					if (transform2)
					{
						if (this.m_bShowRedPoint)
						{
							transform2.gameObject.CustomSetActive(true);
						}
						else
						{
							transform2.gameObject.CustomSetActive(false);
						}
					}
					Transform transform3 = transform.Find("Text");
					if (transform3)
					{
						Text component = transform3.GetComponent<Text>();
						if (component)
						{
							component.text = text;
						}
					}
				}
			}
			else
			{
				Transform transform4 = uiForm.gameObject.transform.Find("Panel/PandroaBtn");
				if (transform4)
				{
					transform4.gameObject.CustomSetActive(false);
					Transform transform5 = transform4.Find("Hotspot");
					if (transform5)
					{
						transform5.gameObject.CustomSetActive(false);
					}
				}
			}
		}

		private void InitPara()
		{
			ApolloAccountInfo accountInfo = Singleton<ApolloHelper>.GetInstance().GetAccountInfo(false);
			string text = "Smoba";
			string openId = accountInfo.get_OpenId();
			string text2 = "qq";
			string text3 = string.Empty;
			string text4 = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID.ToString();
			string text5 = string.Empty;
			string text6 = string.Empty;
			using (ListView<ApolloToken>.Enumerator enumerator = accountInfo.get_TokenList().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ApolloToken current = enumerator.get_Current();
					if (ApolloConfig.platform == 1)
					{
						if (current.get_Type() == 1)
						{
							text5 = current.get_Value();
						}
					}
					else if (ApolloConfig.platform == 2)
					{
						if (current.get_Type() == 3)
						{
							text6 = current.get_Value();
						}
						if (current.get_Type() == 1)
						{
							text5 = current.get_Value();
						}
					}
				}
			}
			if (ApolloConfig.platform == 2)
			{
				text2 = "qq";
				if (Application.platform == RuntimePlatform.Android)
				{
					text3 = "1";
				}
				else if (Application.platform == RuntimePlatform.IPhonePlayer)
				{
					text3 = "2";
				}
			}
			else if (ApolloConfig.platform == 1)
			{
				text2 = "wx";
				if (Application.platform == RuntimePlatform.Android)
				{
					text3 = "3";
				}
				else if (Application.platform == RuntimePlatform.IPhonePlayer)
				{
					text3 = "4";
				}
			}
			string text7 = MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID.ToString();
			string text8 = ApolloConfig.appID;
			if (ApolloConfig.platform == 1)
			{
				text8 = ApolloConfig.WXAppID;
			}
			string appVersion = CVersion.GetAppVersion();
			string text9 = "1";
			GameObject gameObject = base.gameObject;
			Pandora.Instance.SetPandoraParent(gameObject);
			Pandora.Instance.SetPanelBaseDepth(1000);
			Pandora.Instance.SetCallback(new Action<Dictionary<string, string>>(this.OnPandoraEvent));
			Pandora.Instance.SetGetDjImageCallback(new Func<GameObject, int, int, int>(this.OnGetDjImageCallback));
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("sOpenId", openId);
			dictionary.Add("sServiceType", text);
			dictionary.Add("sAcountType", text2);
			dictionary.Add("sArea", text3);
			dictionary.Add("sPartition", text7);
			dictionary.Add("sAppId", text8);
			dictionary.Add("sRoleId", text4);
			dictionary.Add("sAccessToken", text5);
			dictionary.Add("sPayToken", text6);
			dictionary.Add("sGameVer", appVersion);
			dictionary.Add("sPlatID", text9);
			Pandora.Instance.SetUserData(dictionary);
		}

		public void InitWechatLink()
		{
			if (ApolloConfig.platform != 1)
			{
				return;
			}
			ApolloAccountInfo accountInfo = Singleton<ApolloHelper>.GetInstance().GetAccountInfo(false);
			string text = "Smoba";
			string openId = accountInfo.get_OpenId();
			string text2 = "qq";
			string text3 = string.Empty;
			string text4 = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID.ToString();
			string text5 = string.Empty;
			string text6 = string.Empty;
			using (ListView<ApolloToken>.Enumerator enumerator = accountInfo.get_TokenList().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ApolloToken current = enumerator.get_Current();
					if (ApolloConfig.platform == 1)
					{
						if (current.get_Type() == 1)
						{
							text5 = current.get_Value();
						}
					}
					else if (ApolloConfig.platform == 2)
					{
						if (current.get_Type() == 3)
						{
							text6 = current.get_Value();
						}
						if (current.get_Type() == 1)
						{
							text5 = current.get_Value();
						}
					}
				}
			}
			if (ApolloConfig.platform == 2)
			{
				text2 = "qq";
				if (Application.platform == RuntimePlatform.Android)
				{
					text3 = "1";
				}
				else if (Application.platform == RuntimePlatform.IPhonePlayer)
				{
					text3 = "2";
				}
			}
			else if (ApolloConfig.platform == 1)
			{
				text2 = "wx";
				if (Application.platform == RuntimePlatform.Android)
				{
					text3 = "3";
				}
				else if (Application.platform == RuntimePlatform.IPhonePlayer)
				{
					text3 = "4";
				}
			}
			string text7 = MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID.ToString();
			string text8 = ApolloConfig.appID;
			if (ApolloConfig.platform == 1)
			{
				text8 = ApolloConfig.WXAppID;
			}
			string appVersion = CVersion.GetAppVersion();
			string text9 = "1";
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("sOpenId", openId);
			dictionary.Add("sServiceType", text);
			dictionary.Add("sAcountType", text2);
			dictionary.Add("sArea", text3);
			dictionary.Add("sPartition", text7);
			dictionary.Add("sAppId", text8);
			dictionary.Add("sRoleId", text4);
			dictionary.Add("sAccessToken", text5);
			dictionary.Add("sPayToken", text6);
			dictionary.Add("sGameVer", appVersion);
			dictionary.Add("sPlatID", text9);
			this.m_bShowWeixinZone = false;
			if (ApolloConfig.platform == 1 && this.m_bOpenWeixinZone)
			{
				int @int = PlayerPrefs.GetInt("SHOW_WEIXINZONE");
				if (@int >= 1)
				{
					this.m_bShowWeixinZone = true;
				}
				WeChatLink.Instance.BeginGetGameZoneUrl(dictionary, new Action<Dictionary<string, string>>(this.OnGetGameZoneUrl));
			}
		}

		public void StartOpenRedBox(int bWin, int bMvp, int bLegaendary, int bPENTAKILL, int bQUATARYKIL, int bTRIPLEKILL)
		{
			Debug.Log("Pandora StartOpenRedBox1");
			if (this.m_bInit)
			{
				this.m_bstartOPenRedBox = true;
				Debug.Log("Pandora StartOpenRedBox2");
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.set_Item("type", "open");
				dictionary.set_Item("content", "RedPacket");
				dictionary.set_Item("is_legendary", bLegaendary.ToString());
				dictionary.set_Item("is_mvp", bMvp.ToString());
				dictionary.set_Item("is_penta_kill", bPENTAKILL.ToString());
				dictionary.set_Item("is_quadra_kill", bQUATARYKIL.ToString());
				dictionary.set_Item("is_triple_kill", bTRIPLEKILL.ToString());
				dictionary.set_Item("is_victory", bWin.ToString());
				Pandora.Instance.Do(dictionary);
			}
		}

		public void StopRedBox()
		{
			Debug.Log("Pandora StopRedBox1");
			if (this.m_bInit && this.m_bstartOPenRedBox)
			{
				this.m_bstartOPenRedBox = false;
				Debug.Log("Pandora StopRedBox2");
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.set_Item("type", "inSettlement");
				dictionary.set_Item("content", "0");
				Pandora.Instance.Do(dictionary);
			}
		}

		private void OnGetGameZoneUrl(Dictionary<string, string> mDic)
		{
			if (mDic != null && mDic.ContainsKey("showGameZone"))
			{
				if (mDic.get_Item("showGameZone") == "1")
				{
					Debug.Log("revce OnGetGameZoneUrl ");
					this.m_bShowWeixinZone = true;
					PlayerPrefs.SetInt("SHOW_WEIXINZONE", 2);
					PlayerPrefs.Save();
				}
				else
				{
					this.m_bShowWeixinZone = false;
					PlayerPrefs.SetInt("SHOW_WEIXINZONE", 0);
					PlayerPrefs.Save();
				}
			}
		}

		public void ShowActBox()
		{
			Debug.Log("Pandora ShowActBox1");
			if (this.m_bInit)
			{
				Debug.Log("Pandora ShowActBox2");
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.set_Item("type", "open");
				dictionary.set_Item("content", "Lucky");
				Pandora.Instance.Do(dictionary);
			}
		}

		public void ShowPopNews()
		{
			if (this.m_bInit && !this.m_bShowPopNew)
			{
				this.m_bShowPopNew = true;
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.set_Item("type", "open");
				dictionary.set_Item("content", "LuckyPop");
				Pandora.Instance.Do(dictionary);
			}
		}

		public void OnPandoraEvent(Dictionary<string, string> dictData)
		{
			com.tencent.pandora.Logger.DEBUG("OnPandoraEvent enter");
			if (!this.m_bInit)
			{
				return;
			}
			if (dictData != null && dictData.ContainsKey("type"))
			{
				string text = dictData.get_Item("type");
				string text2 = string.Empty;
				if (dictData.ContainsKey("content"))
				{
					text2 = dictData.get_Item("content");
				}
				if (text == "redpoint")
				{
					int num = 0;
					int.TryParse(text2, ref num);
					if (num <= 0)
					{
						this.m_bShowRedPoint = false;
					}
					else
					{
						this.m_bShowRedPoint = true;
					}
					Singleton<ActivitySys>.GetInstance().PandroaUpdateBtn();
					Singleton<EventRouter>.get_instance().BroadCastEvent("IDIPNOTICE_UNREAD_NUM_UPDATE");
				}
				else if (text.Equals("showIcon"))
				{
					if (text2.Equals("1"))
					{
						this.m_bShowBoxBtn = true;
					}
					else
					{
						this.m_bShowBoxBtn = false;
					}
					Singleton<ActivitySys>.GetInstance().PandroaUpdateBtn();
				}
				else if (text.Equals("closePopNews"))
				{
					this.m_bShowPopNew = true;
				}
				else if (text.Equals("ShowGameWindow"))
				{
					int num2 = 0;
					int.TryParse(text2, ref num2);
					com.tencent.pandora.Logger.DEBUG("ShowGameWindow:" + num2);
					if (num2 > 0)
					{
						CUICommonSystem.JumpForm(num2, 0, 0);
					}
				}
				else if (text.Equals("refreshDiamond"))
				{
					Debug.Log("pandorasys ShowGameWindow");
					Singleton<CPaySystem>.GetInstance().OnPandroiaPaySuccss();
				}
				else if (text.Equals("share"))
				{
					string text3 = string.Empty;
					string url = string.Empty;
					string title = string.Empty;
					string desc = string.Empty;
					string text4 = string.Empty;
					int num3 = 0;
					if (dictData.ContainsKey("sendType"))
					{
						text3 = dictData.get_Item("sendType");
						num3++;
					}
					if (dictData.ContainsKey("url"))
					{
						url = dictData.get_Item("url");
						num3++;
					}
					if (dictData.ContainsKey("title"))
					{
						title = dictData.get_Item("title");
						num3++;
					}
					if (dictData.ContainsKey("desc"))
					{
						desc = dictData.get_Item("desc");
						num3++;
					}
					if (dictData.ContainsKey("static_kind"))
					{
						text4 = dictData.get_Item("static_kind");
						num3++;
					}
					if (num3 == 5)
					{
						int shareType = 0;
						if (int.TryParse(text3, ref shareType))
						{
							Singleton<ApolloHelper>.GetInstance().ShareRedBox(shareType, url, title, desc);
						}
					}
					else
					{
						Debug.Log("pandroa sys parm error");
					}
				}
			}
		}

		public int OnGetDjImageCallback(GameObject imgObj, int itemType, int itemID)
		{
			if (itemType < 0 || itemID < 0 || imgObj == null)
			{
				return -1;
			}
			CUseable cUseable = CUseableManager.CreateUseable(itemType, (uint)itemID, 0);
			if (cUseable != null)
			{
				string iconPath = cUseable.GetIconPath();
				Image component = imgObj.GetComponent<Image>();
				int result = 0;
				if (component == null)
				{
					imgObj.AddComponent<Image2>();
				}
				else
				{
					result = 1;
				}
				Image component2 = imgObj.GetComponent<Image>();
				if (component2)
				{
					CUIUtility.SetImageSprite(component2, iconPath, null, true, false, false, false);
				}
				return result;
			}
			return -2;
		}
	}
}
