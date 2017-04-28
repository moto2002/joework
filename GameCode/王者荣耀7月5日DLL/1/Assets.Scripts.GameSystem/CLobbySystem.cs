using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.Sound;
using Assets.Scripts.UI;
using CSProtocol;
using ResData;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	internal class CLobbySystem : Singleton<CLobbySystem>
	{
		public enum LobbyFormWidget
		{
			None = -1,
			Reserve,
			RankingBtn,
			SnsHead,
			HeadImgBack,
			Rolling,
			LoudSpeakerRolling,
			LoudSpeakerRollingBg,
			PlatChannel,
			PlatChannelText
		}

		public enum LobbyRankingBtnFormWidget
		{
			RankingBtnPanel
		}

		public enum enSysEntryFormWidget
		{
			WifiIcon,
			WifiInfo,
			WifiPing,
			GlodCoin,
			Dianquan,
			MailBtn,
			SettingBtn,
			Wifi_Bg,
			FriendBtn,
			MianLiuTxt
		}

		public static string LOBBY_FORM_PATH = "UGUI/Form/System/Lobby/Form_Lobby.prefab";

		public static string SYSENTRY_FORM_PATH = "UGUI/Form/System/Lobby/Form_Lobby_SysTray.prefab";

		public static string RANKING_BTN_FORM_PATH = "UGUI/Form/System/Lobby/Form_Lobby_RankingBtn.prefab";

		public static string Ladder_BtnRes_PATH = CUIUtility.s_Sprite_System_Lobby_Dir + "LadderBtnDynamic.prefab";

		public static string Pvp_BtnRes_PATH = CUIUtility.s_Sprite_System_Lobby_Dir + "PvpBtnDynamic.prefab";

		public static string LOBBY_FUN_UNLOCK_PATH = "UGUI/Form/System/Lobby/Form_FunUnLock.prefab";

		private CUIFormScript m_LobbyForm;

		private CUIFormScript m_SysEntryForm;

		private CUIFormScript m_RankingBtnForm;

		private Text m_PlayerName;

		private Text m_PvpLevel;

		private Image m_PvpExpImg;

		private Text m_PvpExpTxt;

		private RankingSystem.RankingSubView m_rankingType = RankingSystem.RankingSubView.Friend;

		private GameObject _rankingBtn;

		private GameObject hero_btn;

		private GameObject symbol_btn;

		private GameObject bag_btn;

		private GameObject task_btn;

		private GameObject social_btn;

		private GameObject addSkill_btn;

		private GameObject achievement_btn;

		public static bool AutoPopAllow = true;

		private static bool _autoPoped = false;

		private int myRankingNo = -1;

		private ListView<COMDT_FRIEND_INFO> rankFriendList;

		private GameObject m_QQbuluBtn;

		private bool m_bInLobby;

		public static uint s_CoinShowMaxValue = 990000u;

		public static uint s_CoinShowStepValue = 10000u;

		public int m_wifiIconCheckTicks = -1;

		public int m_wifiIconCheckMaxTicks = 6;

		private Text m_lblGlodCoin;

		private Text m_lblDianquan;

		private Text m_lblDiamond;

		private GameObject m_wifiIcon;

		private GameObject m_wifiInfo;

		private GameObject m_textMianliu;

		private Text m_ping;

		private GameObject m_SysEntry;

		private DictionaryView<int, GameObject> m_Btns;

		public static string s_noNetStateName = "NoNet";

		public static string[] s_wifiStateName = new string[]
		{
			"Wifi_1",
			"Wifi_2",
			"Wifi_3"
		};

		public static string[] s_netStateName = new string[]
		{
			"Net_1",
			"Net_2",
			"Net_3"
		};

		public static Color[] s_WifiStateColor = new Color[]
		{
			Color.red,
			Color.yellow,
			Color.green
		};

		public bool NeedRelogin;

		public static bool IsPlatChannelOpen
		{
			get;
			set;
		}

		public bool IsInLobbyForm()
		{
			return this.m_bInLobby;
		}

		public override void Init()
		{
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Lobby_OpenLobbyForm, new CUIEventManager.OnUIEventHandler(this.onOpenLobby));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.WEB_OpenURL, new CUIEventManager.OnUIEventHandler(this.onOpenWeb));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Common_WifiCheckTimer, new CUIEventManager.OnUIEventHandler(this.onCommon_WifiCheckTimer));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Common_ShowOrHideWifiInfo, new CUIEventManager.OnUIEventHandler(this.onCommon_ShowOrHideWifiInfo));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Lobby_ConfirmErrExit, new CUIEventManager.OnUIEventHandler(this.onErrorExit));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Lobby_LobbyFormShow, new CUIEventManager.OnUIEventHandler(this.Lobby_LobbyFormShow));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Lobby_LobbyFormHide, new CUIEventManager.OnUIEventHandler(this.Lobby_LobbyFormHide));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Lobby_Close, new CUIEventManager.OnUIEventHandler(this.OnCloseForm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Lobby_PrepareFight_Sub, new CUIEventManager.OnUIEventHandler(this.OnPrepareFight_Sub));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Lobby_PrepareFight_Origin, new CUIEventManager.OnUIEventHandler(this.OnPrepareFight_Origin));
			Singleton<EventRouter>.get_instance().AddEventHandler("MasterAttributesChanged", new Action(this.UpdatePlayerData));
			Singleton<EventRouter>.get_instance().AddEventHandler("TaskUpdated", new Action(this.OnTaskUpdate));
			Singleton<EventRouter>.get_instance().AddEventHandler("Friend_LobbyIconRedDot_Refresh", new Action(this.OnFriendSysIconUpdate));
			Singleton<EventRouter>.get_instance().AddEventHandler(EventID.Mall_Entry_Add_RedDotCheck, new Action(this.OnCheckRedDotByServerVersionWithLobby));
			Singleton<EventRouter>.get_instance().AddEventHandler(EventID.Mall_Entry_Del_RedDotCheck, new Action(this.OnCheckDelMallEntryRedDot));
			Singleton<EventRouter>.get_instance().AddEventHandler("MailUnReadNumUpdate", new Action(this.OnMailUnReadUpdate));
			Singleton<EventRouter>.get_instance().AddEventHandler(EventID.ACHIEVE_TROPHY_REWARD_INFO_STATE_CHANGE, new Action(this.OnAchieveStateUpdate));
			Singleton<EventRouter>.get_instance().AddEventHandler(EventID.SymbolEquipSuc, new Action(this.OnCheckSymbolEquipAlert));
			Singleton<EventRouter>.get_instance().AddEventHandler(EventID.BAG_ITEMS_UPDATE, new Action(this.OnBagItemsUpdate));
			Singleton<EventRouter>.get_instance().AddEventHandler("MasterPvpLevelChanged", new Action(this.OnCheckSymbolEquipAlert));
			Singleton<EventRouter>.get_instance().AddEventHandler<bool>("Guild_Sign_State_Changed", new Action<bool>(this.OnGuildSignStateChanged));
			Singleton<ActivitySys>.GetInstance().OnStateChange += new ActivitySys.StateChangeDelegate(this.ValidateActivitySpot);
			Singleton<EventRouter>.get_instance().AddEventHandler("IDIPNOTICE_UNREAD_NUM_UPDATE", new Action(this.ValidateActivitySpot));
			Singleton<EventRouter>.get_instance().AddEventHandler("MasterJifenChanged", new Action(this.ValidateActivitySpot));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.IDIP_QQVIP_OpenWealForm, new CUIEventManager.OnUIEventHandler(this.OpenQQVIPWealForm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.WEB_OpenHome, new CUIEventManager.OnUIEventHandler(this.OpenWebHome));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Lobby_UnlockAnimation_End, new CUIEventManager.OnUIEventHandler(this.On_Lobby_UnlockAnimation_End));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Lobby_MysteryShopClose, new CUIEventManager.OnUIEventHandler(this.On_Lobby_MysteryShopClose));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.GameCenter_OpenWXRight, new CUIEventManager.OnUIEventHandler(this.OpenWXGameCenterRightForm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.GameCenter_OpenQQRight, new CUIEventManager.OnUIEventHandler(this.OpenQQGameCenterRightForm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Lobby_RankingListElementEnable, new CUIEventManager.OnUIEventHandler(this.OnRankingListElementEnable));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.GameCenter_OpenGuestRight, new CUIEventManager.OnUIEventHandler(this.OpenGuestGameCenterRightForm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Lobby_GotoBattleWebHome, new CUIEventManager.OnUIEventHandler(this.OnGotoBattleWebHome));
			Singleton<EventRouter>.GetInstance().AddEventHandler("CheckNewbieIntro", new Action(this.OnCheckNewbieIntro));
			Singleton<EventRouter>.GetInstance().AddEventHandler("VipInfoHadSet", new Action(this.UpdateQQVIPState));
			Singleton<EventRouter>.GetInstance().AddEventHandler(EventID.NOBE_STATE_CHANGE, new Action(this.UpdateNobeIcon));
			Singleton<EventRouter>.GetInstance().AddEventHandler(EventID.NOBE_STATE_HEAD_CHANGE, new Action(this.UpdateNobeHeadIdx));
			Singleton<EventRouter>.GetInstance().AddEventHandler("MasterPvpLevelChanged", new Action(this.OnPlayerLvlChange));
			Singleton<EventRouter>.GetInstance().AddEventHandler("Rank_Friend_List", new Action(this.RefreshRankList));
			Singleton<EventRouter>.GetInstance().AddEventHandler<RankingSystem.RankingSubView>("Rank_List", new Action<RankingSystem.RankingSubView>(this.RefreshRankList));
			Singleton<EventRouter>.GetInstance().AddEventHandler(EventID.NAMECHANGE_PLAYER_NAME_CHANGE, new Action(this.OnPlayerNameChange));
			Singleton<EventRouter>.GetInstance().AddEventHandler(EventID.HEAD_IMAGE_FLAG_CHANGE, new Action(this.UpdatePlayerData));
			Singleton<EventRouter>.GetInstance().AddEventHandler(EventID.LOBBY_PURE_LOBBY_SHOW, new Action(this.OnPureLobbyShow));
			Singleton<EventRouter>.GetInstance().AddEventHandler(EventID.GAMER_REDDOT_CHANGE, new Action(this.UpdatePlayerData));
		}

		public override void UnInit()
		{
			base.UnInit();
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Lobby_OpenLobbyForm, new CUIEventManager.OnUIEventHandler(this.onOpenLobby));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.WEB_OpenURL, new CUIEventManager.OnUIEventHandler(this.onOpenWeb));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Common_WifiCheckTimer, new CUIEventManager.OnUIEventHandler(this.onCommon_WifiCheckTimer));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Common_ShowOrHideWifiInfo, new CUIEventManager.OnUIEventHandler(this.onCommon_ShowOrHideWifiInfo));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Lobby_LobbyFormShow, new CUIEventManager.OnUIEventHandler(this.Lobby_LobbyFormShow));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Lobby_LobbyFormHide, new CUIEventManager.OnUIEventHandler(this.Lobby_LobbyFormHide));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Lobby_ConfirmErrExit, new CUIEventManager.OnUIEventHandler(this.onErrorExit));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Lobby_Close, new CUIEventManager.OnUIEventHandler(this.OnCloseForm));
			Singleton<EventRouter>.get_instance().RemoveEventHandler("MasterAttributesChanged", new Action(this.UpdatePlayerData));
			Singleton<EventRouter>.get_instance().RemoveEventHandler("TaskUpdated", new Action(this.OnTaskUpdate));
			Singleton<EventRouter>.get_instance().RemoveEventHandler("Friend_LobbyIconRedDot_Refresh", new Action(this.OnFriendSysIconUpdate));
			Singleton<EventRouter>.get_instance().RemoveEventHandler(EventID.Mall_Entry_Add_RedDotCheck, new Action(this.OnCheckRedDotByServerVersionWithLobby));
			Singleton<EventRouter>.get_instance().RemoveEventHandler(EventID.Mall_Entry_Del_RedDotCheck, new Action(this.OnCheckDelMallEntryRedDot));
			Singleton<EventRouter>.get_instance().RemoveEventHandler("MailUnReadNumUpdate", new Action(this.OnMailUnReadUpdate));
			Singleton<EventRouter>.get_instance().RemoveEventHandler(EventID.ACHIEVE_TROPHY_REWARD_INFO_STATE_CHANGE, new Action(this.OnAchieveStateUpdate));
			Singleton<EventRouter>.get_instance().RemoveEventHandler(EventID.SymbolEquipSuc, new Action(this.OnCheckSymbolEquipAlert));
			Singleton<EventRouter>.get_instance().RemoveEventHandler(EventID.BAG_ITEMS_UPDATE, new Action(this.OnBagItemsUpdate));
			Singleton<EventRouter>.get_instance().RemoveEventHandler("MasterPvpLevelChanged", new Action(this.OnCheckSymbolEquipAlert));
			Singleton<ActivitySys>.GetInstance().OnStateChange -= new ActivitySys.StateChangeDelegate(this.ValidateActivitySpot);
			Singleton<EventRouter>.get_instance().RemoveEventHandler("IDIPNOTICE_UNREAD_NUM_UPDATE", new Action(this.ValidateActivitySpot));
			Singleton<EventRouter>.get_instance().RemoveEventHandler("MasterJifenChanged", new Action(this.ValidateActivitySpot));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.IDIP_QQVIP_OpenWealForm, new CUIEventManager.OnUIEventHandler(this.OpenQQVIPWealForm));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.WEB_OpenHome, new CUIEventManager.OnUIEventHandler(this.OpenWebHome));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Lobby_UnlockAnimation_End, new CUIEventManager.OnUIEventHandler(this.On_Lobby_UnlockAnimation_End));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Lobby_MysteryShopClose, new CUIEventManager.OnUIEventHandler(this.On_Lobby_MysteryShopClose));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.GameCenter_OpenWXRight, new CUIEventManager.OnUIEventHandler(this.OpenWXGameCenterRightForm));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Lobby_RankingListElementEnable, new CUIEventManager.OnUIEventHandler(this.OnRankingListElementEnable));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.GameCenter_OpenGuestRight, new CUIEventManager.OnUIEventHandler(this.OpenGuestGameCenterRightForm));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Lobby_GotoBattleWebHome, new CUIEventManager.OnUIEventHandler(this.OnGotoBattleWebHome));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler(EventID.NOBE_STATE_CHANGE, new Action(this.UpdateNobeIcon));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler(EventID.NOBE_STATE_HEAD_CHANGE, new Action(this.UpdateNobeHeadIdx));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler("MasterPvpLevelChanged", new Action(this.OnPlayerLvlChange));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler("Rank_Friend_List", new Action(this.RefreshRankList));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<RankingSystem.RankingSubView>("Rank_List", new Action<RankingSystem.RankingSubView>(this.RefreshRankList));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler(EventID.HEAD_IMAGE_FLAG_CHANGE, new Action(this.UpdatePlayerData));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler("CheckNewbieIntro", new Action(this.OnCheckNewbieIntro));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler("VipInfoHadSet", new Action(this.UpdateQQVIPState));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler(EventID.LOBBY_PURE_LOBBY_SHOW, new Action(this.OnPureLobbyShow));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler(EventID.GAMER_REDDOT_CHANGE, new Action(this.UpdatePlayerData));
		}

		private void onOpenLobby(CUIEvent uiEvent)
		{
			this.m_LobbyForm = Singleton<CUIManager>.GetInstance().OpenForm(CLobbySystem.LOBBY_FORM_PATH, false, true);
			this.m_SysEntryForm = Singleton<CUIManager>.GetInstance().OpenForm(CLobbySystem.SYSENTRY_FORM_PATH, false, true);
			this.m_RankingBtnForm = Singleton<CUIManager>.GetInstance().OpenForm(CLobbySystem.RANKING_BTN_FORM_PATH, false, true);
			this.m_bInLobby = true;
			this.InitForm(this.m_LobbyForm);
			this.InitRankingBtnForm();
			this.InitOther(this.m_LobbyForm);
			this.InitSysEntryForm(this.m_SysEntryForm);
			this.UpdatePlayerData();
			this.OnFriendSysIconUpdate();
			this.OnTaskUpdate();
			this.ValidateActivitySpot();
			this.OnMailUnReadUpdate();
			this.OnCheckSymbolEquipAlert();
			this.OnCheckUpdateClientVersion();
			Singleton<EventRouter>.get_instance().BroadCastEvent(EventID.Mall_Entry_Add_RedDotCheck);
			Singleton<EventRouter>.get_instance().BroadCastEvent(EventID.Mall_Set_Free_Draw_Timer);
			Singleton<CMiShuSystem>.get_instance().CheckMiShuTalk(true);
			Singleton<CMiShuSystem>.get_instance().OnCheckFirstWin(null);
			Singleton<CMiShuSystem>.get_instance().CheckActPlayModeTipsForLobby();
			Singleton<CUINewFlagSystem>.get_instance().ShowNewFlagForBeizhanEntry();
			Singleton<CUINewFlagSystem>.get_instance().ShowNewFlagForMishuEntry();
			Singleton<CUINewFlagSystem>.get_instance().SetNewFlagFormCustomEquipShow(true);
			Singleton<CUINewFlagSystem>.get_instance().SetNewFlagForFriendEntry(true);
			Singleton<CUINewFlagSystem>.get_instance().SetNewFlagForMessageBtnEntry(true);
			Singleton<CUINewFlagSystem>.get_instance().SetNewFlagForOBBtn(true);
			Singleton<CUINewFlagSystem>.get_instance().SetNewFlagForMatch(true);
			Singleton<CUINewFlagSystem>.get_instance().SetNewFlagForSettingEntry(true);
			Singleton<CUINewFlagSystem>.get_instance().ShowNewFlagForAchievementEntry();
			if (Singleton<CLoginSystem>.GetInstance().m_fLoginBeginTime > 0f)
			{
				List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
				list.Add(new KeyValuePair<string, string>("g_version", CVersion.GetAppVersion()));
				list.Add(new KeyValuePair<string, string>("WorldID", MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID.ToString()));
				list.Add(new KeyValuePair<string, string>("platform", Singleton<ApolloHelper>.GetInstance().CurPlatform.ToString()));
				list.Add(new KeyValuePair<string, string>("openid", "NULL"));
				list.Add(new KeyValuePair<string, string>("totaltime", (Time.time - Singleton<CLoginSystem>.GetInstance().m_fLoginBeginTime).ToString()));
				list.Add(new KeyValuePair<string, string>("errorCode", "0"));
				list.Add(new KeyValuePair<string, string>("error_msg", "0"));
				Singleton<ApolloHelper>.GetInstance().ApolloRepoertEvent("Service_Login_InLobby", list, true);
				Singleton<CLoginSystem>.GetInstance().m_fLoginBeginTime = 0f;
			}
			if (this.NeedRelogin)
			{
				this.NeedRelogin = false;
				LobbyMsgHandler.PopupRelogin();
			}
		}

		private void onOpenWeb(CUIEvent uiEvent)
		{
			string strUrl = "http://www.qq.com";
			CUICommonSystem.OpenUrl(strUrl, true, 0);
		}

		private void onCommon_WifiCheckTimer(CUIEvent uiEvent)
		{
			if (!this.m_bInLobby)
			{
				return;
			}
			this.CheckWifi();
			this.CheckMianLiu();
		}

		private void onCommon_ShowOrHideWifiInfo(CUIEvent uiEvent)
		{
			if (!this.m_bInLobby)
			{
				return;
			}
			this.ShowOrHideWifiInfo();
		}

		private void Lobby_LobbyFormShow(CUIEvent uiEvent)
		{
			if (!this.m_bInLobby)
			{
				return;
			}
			this.FullShow();
			CUICommonSystem.CloseCommonTips();
			CUICommonSystem.CloseUseableTips();
			Singleton<CMiShuSystem>.get_instance().CheckActPlayModeTipsForLobby();
			Singleton<CChatController>.get_instance().ShowPanel(true, false);
			Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Lobby_PrepareFight_Sub);
			if (!string.IsNullOrEmpty(Singleton<CFriendContoller>.get_instance().IntimacyUpInfo))
			{
				Singleton<CUIManager>.GetInstance().OpenTips(string.Format(UT.GetText("Intimacy_UpInfo"), Singleton<CFriendContoller>.get_instance().IntimacyUpInfo, Singleton<CFriendContoller>.get_instance().IntimacyUpValue), false, 1.5f, null, new object[0]);
				Singleton<CFriendContoller>.get_instance().IntimacyUpInfo = string.Empty;
				Singleton<CFriendContoller>.get_instance().IntimacyUpValue = 0;
			}
			if (!MonoSingleton<NewbieGuideManager>.GetInstance().isNewbieGuiding)
			{
				Singleton<CAchievementSystem>.GetInstance().ProcessMostRecentlyDoneAchievements(false);
				Singleton<CAchievementSystem>.GetInstance().ProcessPvpBanMsg();
			}
			this.OnFriendSysIconUpdate();
		}

		private void Lobby_LobbyFormHide(CUIEvent uiEvent)
		{
			if (!this.m_bInLobby)
			{
				return;
			}
			this.MiniShow();
		}

		public void SetTopBarPriority(enFormPriority prioRity)
		{
			if (this.m_SysEntryForm == null)
			{
				return;
			}
			CUIFormScript component = this.m_SysEntryForm.GetComponent<CUIFormScript>();
			if (component != null)
			{
				component.SetPriority(prioRity);
			}
		}

		private void onErrorExit(CUIEvent uiEvent)
		{
			SGameApplication.Quit();
		}

		private void UpdatePlayerData()
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo();
			if (masterRoleInfo == null || this.m_LobbyForm == null)
			{
				return;
			}
			if (this.m_PlayerName != null)
			{
				this.m_PlayerName.text = masterRoleInfo.Name;
			}
			if (this.m_PvpExpImg != null)
			{
				this.m_PvpExpImg.CustomFillAmount(CPlayerProfile.Divide(masterRoleInfo.PvpExp, masterRoleInfo.PvpNeedExp));
				this.m_PvpExpTxt.text = masterRoleInfo.PvpExp + "/" + masterRoleInfo.PvpNeedExp;
			}
			if (this.m_PvpLevel != null)
			{
				string text = Singleton<CTextManager>.GetInstance().GetText("ranking_PlayerLevel");
				if (!string.IsNullOrEmpty(text) && this.m_PvpLevel.text != null && masterRoleInfo != null)
				{
					this.m_PvpLevel.text = string.Format(text, masterRoleInfo.PvpLevel);
				}
			}
			if (!CSysDynamicBlock.bSocialBlocked)
			{
				if (this.m_LobbyForm != null && this.m_LobbyForm.gameObject.activeSelf && masterRoleInfo != null)
				{
					GameObject widget = this.m_LobbyForm.GetWidget(2);
					if (widget != null && !string.IsNullOrEmpty(masterRoleInfo.HeadUrl))
					{
						CUIHttpImageScript component = widget.GetComponent<CUIHttpImageScript>();
						component.SetImageUrl(masterRoleInfo.HeadUrl);
						MonoSingleton<NobeSys>.GetInstance().SetNobeIcon(component.GetComponent<Image>(), (int)masterRoleInfo.GetNobeInfo().stGameVipClient.dwCurLevel, false);
						Image component2 = this.m_LobbyForm.GetWidget(3).GetComponent<Image>();
						MonoSingleton<NobeSys>.GetInstance().SetHeadIconBk(component2, (int)masterRoleInfo.GetNobeInfo().stGameVipClient.dwHeadIconId);
						MonoSingleton<NobeSys>.GetInstance().SetHeadIconBkEffect(component2, (int)masterRoleInfo.GetNobeInfo().stGameVipClient.dwHeadIconId, this.m_LobbyForm, 0.7f);
						bool flag = Singleton<HeadIconSys>.get_instance().UnReadFlagNum > 0u || masterRoleInfo.ShowGameRedDot;
						GameObject gameObject = Utility.FindChild(widget, "RedDot");
						if (gameObject != null)
						{
							if (flag)
							{
								CUICommonSystem.AddRedDot(gameObject, enRedDotPos.enTopRight, 0);
							}
							else
							{
								CUICommonSystem.DelRedDot(gameObject);
							}
						}
					}
				}
			}
			else if (this.m_LobbyForm != null && this.m_LobbyForm.gameObject.activeSelf)
			{
				GameObject widget2 = this.m_LobbyForm.GetWidget(2);
				if (widget2 != null)
				{
					CUIHttpImageScript component3 = widget2.GetComponent<CUIHttpImageScript>();
					if (component3)
					{
						MonoSingleton<NobeSys>.GetInstance().SetNobeIcon(component3.GetComponent<Image>(), 0, false);
					}
				}
			}
			if (this.m_lblGlodCoin != null)
			{
				this.m_lblGlodCoin.text = this.GetCoinString(masterRoleInfo.GoldCoin);
			}
			if (this.m_lblDianquan != null)
			{
				this.m_lblDianquan.text = this.GetCoinString((uint)masterRoleInfo.DianQuan);
			}
			if (this.m_lblDiamond != null)
			{
				this.m_lblDiamond.text = this.GetCoinString(masterRoleInfo.Diamond);
			}
		}

		public void ShowHideRankingBtn(bool show)
		{
			if (this._rankingBtn != null)
			{
				if (CSysDynamicBlock.bSocialBlocked)
				{
					this._rankingBtn.CustomSetActive(false);
				}
				else
				{
					this._rankingBtn.CustomSetActive(show);
				}
			}
		}

		public void Play_UnLock_Animation(RES_SPECIALFUNCUNLOCK_TYPE type)
		{
			string text = string.Empty;
			switch (type)
			{
			case 13:
				text = "SocialBtn";
				goto IL_AE;
			case 14:
			case 15:
			case 17:
			case 21:
				IL_3B:
				if (type == 8)
				{
					text = "SymbolBtn";
					goto IL_AE;
				}
				if (type != 28)
				{
					goto IL_AE;
				}
				text = "AchievementBtn";
				goto IL_AE;
			case 16:
				text = "TaskBtn";
				goto IL_AE;
			case 18:
				text = "HeroBtn";
				goto IL_AE;
			case 19:
				text = "BagBtn";
				goto IL_AE;
			case 20:
				text = "FriendBtn";
				goto IL_AE;
			case 22:
				text = "AddedSkillBtn";
				goto IL_AE;
			}
			goto IL_3B;
			IL_AE:
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			stUIEventParams eventParams = default(stUIEventParams);
			eventParams.tag = type;
			CUIFormScript cUIFormScript = Singleton<CUIManager>.get_instance().OpenForm(CLobbySystem.LOBBY_FUN_UNLOCK_PATH, false, true);
			CUIAnimatorScript component = cUIFormScript.GetComponent<CUIAnimatorScript>();
			component.SetUIEvent(enAnimatorEventType.AnimatorEnd, enUIEventID.Lobby_UnlockAnimation_End, eventParams);
			component.PlayAnimator(text);
			Singleton<CSoundManager>.get_instance().PostEvent("UI_hall_system_unlock", null);
		}

		public void Clear()
		{
			this.m_rankingType = RankingSystem.RankingSubView.Friend;
		}

		private void UnInitWidget()
		{
			this._rankingBtn = null;
		}

		private void RefreshRankList(RankingSystem.RankingSubView rankingType)
		{
			this.m_rankingType = rankingType;
			this.RefreshRankList();
		}

		private void RefreshRankList()
		{
			if (this._rankingBtn == null)
			{
				return;
			}
			CUIListScript componetInChild = Utility.GetComponetInChild<CUIListScript>(this._rankingBtn, "RankingList");
			if (this.m_rankingType == RankingSystem.RankingSubView.Friend)
			{
				this.myRankingNo = Singleton<RankingSystem>.get_instance().GetMyFriendRankNo();
				if (this.myRankingNo == -1)
				{
					return;
				}
				this.rankFriendList = Singleton<CFriendContoller>.get_instance().model.GetSortedRankingFriendList(7);
				if (this.rankFriendList == null)
				{
					return;
				}
				int num = this.rankFriendList.get_Count() + 1;
				componetInChild.SetElementAmount(num);
				for (int i = 0; i < num; i++)
				{
					CUIListElementScript elemenet = componetInChild.GetElemenet(i);
					if (!(elemenet == null))
					{
						GameObject gameObject = elemenet.gameObject;
						if (!(gameObject == null))
						{
							this.OnUpdateRankingFriendElement(gameObject, i);
						}
					}
				}
			}
			else if (this.m_rankingType == RankingSystem.RankingSubView.All)
			{
				CSDT_RANKING_LIST_SUCC rankList = Singleton<RankingSystem>.get_instance().GetRankList(RankingSystem.RankingType.Ladder);
				if (rankList != null)
				{
					int num = (int)rankList.dwItemNum;
					componetInChild.SetElementAmount(num);
					for (int j = 0; j < num; j++)
					{
						CUIListElementScript elemenet2 = componetInChild.GetElemenet(j);
						if (!(elemenet2 == null))
						{
							GameObject gameObject2 = elemenet2.gameObject;
							if (!(gameObject2 == null))
							{
								this.OnUpdateRankingAllElement(gameObject2, j);
							}
						}
					}
				}
			}
		}

		private void OnRankingListElementEnable(CUIEvent uiEvent)
		{
			int srcWidgetIndexInBelongedList = uiEvent.m_srcWidgetIndexInBelongedList;
			GameObject srcWidget = uiEvent.m_srcWidget;
			if (this.m_rankingType == RankingSystem.RankingSubView.Friend)
			{
				this.OnUpdateRankingFriendElement(srcWidget, srcWidgetIndexInBelongedList);
			}
			else if (this.m_rankingType == RankingSystem.RankingSubView.All)
			{
				this.OnUpdateRankingAllElement(srcWidget, srcWidgetIndexInBelongedList);
			}
		}

		private void OnUpdateRankingFriendElement(GameObject go, int index)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo();
			string serverUrl = string.Empty;
			int num = (index <= this.myRankingNo) ? 0 : -1;
			Transform transform = go.transform;
			GameObject gameObject = transform.Find("HeadIcon").gameObject;
			GameObject gameObject2 = transform.transform.Find("HeadbgNo1").gameObject;
			GameObject gameObject3 = transform.transform.Find("123No").gameObject;
			int headIdx = 0;
			if (index == this.myRankingNo)
			{
				if (masterRoleInfo != null)
				{
					serverUrl = masterRoleInfo.HeadUrl;
					headIdx = (int)masterRoleInfo.GetNobeInfo().stGameVipClient.dwHeadIconId;
					GameObject gameObject4 = transform.transform.Find("QQVipIcon").gameObject;
					this.SetQQVip(gameObject4, true, 0);
				}
			}
			else if (index + num < this.rankFriendList.get_Count())
			{
				serverUrl = Singleton<ApolloHelper>.GetInstance().ToSnsHeadUrl(ref this.rankFriendList.get_Item(index + num).szHeadUrl);
				headIdx = (int)this.rankFriendList.get_Item(index + num).stGameVip.dwHeadIconId;
				GameObject gameObject5 = transform.transform.Find("QQVipIcon").gameObject;
				this.SetQQVip(gameObject5, false, (int)this.rankFriendList.get_Item(index + num).dwQQVIPMask);
			}
			gameObject2.CustomSetActive(index == 0);
			gameObject3.transform.GetChild(0).gameObject.CustomSetActive(0 == index);
			gameObject3.transform.GetChild(1).gameObject.CustomSetActive(1 == index);
			gameObject3.transform.GetChild(2).gameObject.CustomSetActive(2 == index);
			Image component = transform.transform.Find("NobeImag").GetComponent<Image>();
			if (component)
			{
				MonoSingleton<NobeSys>.GetInstance().SetHeadIconBk(component, headIdx);
			}
			RankingView.SetUrlHeadIcon(gameObject, serverUrl);
		}

		private void OnUpdateRankingAllElement(GameObject go, int index)
		{
			CSDT_RANKING_LIST_SUCC rankList = Singleton<RankingSystem>.get_instance().GetRankList(RankingSystem.RankingType.Ladder);
			if (rankList != null && go != null && index < rankList.astItemDetail.Length)
			{
				string serverUrl = string.Empty;
				Transform transform = go.transform.Find("HeadIcon");
				Transform transform2 = go.transform.Find("HeadbgNo1");
				Transform transform3 = go.transform.Find("123No");
				if (transform != null && transform2 != null && transform3 != null)
				{
					serverUrl = Singleton<ApolloHelper>.GetInstance().ToSnsHeadUrl(ref rankList.astItemDetail[index].stExtraInfo.stDetailInfo.get_stLadderPoint().szHeadUrl);
					transform2.gameObject.CustomSetActive(index == 0);
					RankingView.SetUrlHeadIcon(transform.gameObject, serverUrl);
					transform3.GetChild(0).gameObject.CustomSetActive(0 == index);
					transform3.GetChild(1).gameObject.CustomSetActive(1 == index);
					transform3.GetChild(2).gameObject.CustomSetActive(2 == index);
				}
				int dwHeadIconId = (int)rankList.astItemDetail[index].stExtraInfo.stDetailInfo.get_stLadderPoint().stGameVip.dwHeadIconId;
				Image componetInChild = Utility.GetComponetInChild<Image>(go, "NobeImag");
				if (componetInChild)
				{
					MonoSingleton<NobeSys>.GetInstance().SetHeadIconBk(componetInChild, dwHeadIconId);
				}
				Transform transform4 = go.transform.Find("QQVipIcon");
				if (transform4)
				{
					this.SetQQVip(transform4.gameObject, false, (int)rankList.astItemDetail[index].stExtraInfo.stDetailInfo.get_stLadderPoint().dwVipLevel);
				}
			}
		}

		private void OnPlayerNameChange()
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (this.m_PlayerName != null && masterRoleInfo != null)
			{
				this.m_PlayerName.text = masterRoleInfo.Name;
			}
		}

		private void SetQQVip(GameObject QQVipIcon, bool bSelf, int mask = 0)
		{
			if (QQVipIcon == null)
			{
				return;
			}
			if (ApolloConfig.platform == 2 || ApolloConfig.platform == 3)
			{
				if (CSysDynamicBlock.bLobbyEntryBlocked)
				{
					QQVipIcon.CustomSetActive(false);
					return;
				}
				if (bSelf)
				{
					CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
					if (masterRoleInfo != null)
					{
						MonoSingleton<NobeSys>.GetInstance().SetMyQQVipHead(QQVipIcon.GetComponent<Image>());
					}
				}
				else
				{
					MonoSingleton<NobeSys>.GetInstance().SetOtherQQVipHead(QQVipIcon.GetComponent<Image>(), mask);
				}
			}
			else
			{
				QQVipIcon.CustomSetActive(false);
			}
		}

		private void ProcessQQVIP(CUIFormScript form)
		{
			if (null == form)
			{
				return;
			}
			Transform transform = form.transform.Find("VIPGroup/QQVIpBtn");
			GameObject obj = null;
			if (transform)
			{
				obj = transform.gameObject;
			}
			GameObject gameObject = Utility.FindChild(form.gameObject, "PlayerHead/NameGroup/QQVipIcon");
			if (ApolloConfig.platform == 2 || ApolloConfig.platform == 3)
			{
				if (CSysDynamicBlock.bLobbyEntryBlocked)
				{
					obj.CustomSetActive(false);
					gameObject.CustomSetActive(false);
					return;
				}
				obj.CustomSetActive(true);
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				if (masterRoleInfo != null)
				{
					MonoSingleton<NobeSys>.GetInstance().SetMyQQVipHead(gameObject.GetComponent<Image>());
				}
			}
			else
			{
				obj.CustomSetActive(false);
				gameObject.CustomSetActive(false);
			}
		}

		private void UpdateQQVIPState()
		{
			if (!this.m_bInLobby || this.m_LobbyForm == null)
			{
				return;
			}
			Transform transform = this.m_LobbyForm.transform;
			Transform transform2 = transform.Find("VIPGroup/QQVIpBtn");
			GameObject obj = null;
			if (transform2)
			{
				obj = transform2.gameObject;
			}
			Transform transform3 = transform.Find("PlayerHead/NameGroup/QQVipIcon");
			GameObject obj2 = null;
			GameObject obj3 = null;
			if (transform3 != null)
			{
				obj2 = transform3.gameObject;
			}
			if (ApolloConfig.platform == 2 || ApolloConfig.platform == 3)
			{
				if (CSysDynamicBlock.bLobbyEntryBlocked)
				{
					obj.CustomSetActive(false);
					obj2.CustomSetActive(false);
					obj3.CustomSetActive(false);
					return;
				}
				obj.CustomSetActive(true);
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				if (masterRoleInfo != null)
				{
					if (masterRoleInfo.HasVip(16))
					{
						obj2.CustomSetActive(false);
						obj3.CustomSetActive(true);
					}
					else if (masterRoleInfo.HasVip(1))
					{
						obj2.CustomSetActive(true);
						obj3.CustomSetActive(false);
					}
					else
					{
						obj2.CustomSetActive(false);
						obj3.CustomSetActive(false);
					}
				}
			}
			else
			{
				obj.CustomSetActive(false);
				obj2.CustomSetActive(false);
				obj3.CustomSetActive(false);
			}
		}

		private void OpenQQVIPWealForm(CUIEvent uiEvent)
		{
			string formPath = string.Format("{0}{1}", "UGUI/Form/System/", "IDIPNotice/Form_QQVipPrivilege.prefab");
			CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(formPath, false, true);
			if (cUIFormScript != null)
			{
				Singleton<QQVipWidget>.get_instance().SetData(cUIFormScript.gameObject, cUIFormScript);
			}
		}

		private void ShowPlatformRight()
		{
			if (Singleton<ApolloHelper>.GetInstance().CurPlatform == 5)
			{
				string formPath = string.Format("{0}{1}", "UGUI/Form/System/", "GameCenter/Form_GuestGameCenter.prefab");
				Singleton<CUIManager>.GetInstance().OpenForm(formPath, false, true);
			}
			else if (Singleton<ApolloHelper>.GetInstance().CurPlatform == 2)
			{
				string formPath2 = string.Format("{0}{1}", "UGUI/Form/System/", "GameCenter/Form_QQGameCenter.prefab");
				Singleton<CUIManager>.GetInstance().OpenForm(formPath2, false, true);
			}
			else if (Singleton<ApolloHelper>.GetInstance().CurPlatform == 1)
			{
				string formPath3 = string.Format("{0}{1}", "UGUI/Form/System/", "GameCenter/Form_WXGameCenter.prefab");
				Singleton<CUIManager>.GetInstance().OpenForm(formPath3, false, true);
			}
		}

		private void OpenGuestGameCenterRightForm(CUIEvent uiEvent)
		{
			this.ShowPlatformRight();
		}

		private void OpenWXGameCenterRightForm(CUIEvent uiEvent)
		{
			this.ShowPlatformRight();
		}

		private void OpenQQGameCenterRightForm(CUIEvent uiEvent)
		{
			this.ShowPlatformRight();
		}

		private void OpenWebHome(CUIEvent uiEvent)
		{
			ulong num = 0uL;
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				num = masterRoleInfo.playerUllUID;
			}
			string platformArea = CUICommonSystem.GetPlatformArea();
			string strUrl = string.Concat(new object[]
			{
				"http://yxzj.qq.com/ingame/all/index.shtml?partition=",
				MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID,
				"&roleid=",
				num,
				"&area=",
				platformArea
			});
			CUICommonSystem.OpenUrl(strUrl, true, 0);
			if (CUIRedDotSystem.IsShowRedDotByVersion(enRedID.Lobby_GongLueEntry))
			{
				CUIRedDotSystem.SetRedDotViewByVersion(enRedID.Lobby_GongLueEntry);
				if (this.m_LobbyForm != null)
				{
					Transform transform = this.m_LobbyForm.transform.Find("Popup/SignBtn");
					if (transform != null)
					{
						CUIRedDotSystem.DelRedDot(transform.gameObject);
					}
				}
			}
		}

		private void OnGotoBattleWebHome(CUIEvent uiEvent)
		{
			ulong num = 0uL;
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				num = masterRoleInfo.playerUllUID;
			}
			string platformArea = CUICommonSystem.GetPlatformArea();
			string text = Singleton<CTextManager>.get_instance().GetText("HttpUrl_BattleWebHome");
			text = string.Concat(new object[]
			{
				text,
				"?partition=",
				MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID,
				"&roleid=",
				num,
				"&area=",
				platformArea
			});
			CUICommonSystem.OpenUrl(text, true, 0);
			Singleton<CUINewFlagSystem>.get_instance().SetNewFlagForMatch(false);
			if (CUIRedDotSystem.IsShowRedDotByVersion(enRedID.Lobby_BattleHomeWeb))
			{
				CUIRedDotSystem.SetRedDotViewByVersion(enRedID.Lobby_BattleHomeWeb);
				if (this.m_LobbyForm != null)
				{
					Transform transform = this.m_LobbyForm.transform.Find("Popup/BattleWebHome");
					if (transform != null)
					{
						CUIRedDotSystem.DelRedDot(transform.gameObject);
					}
				}
			}
		}

		protected void OnCloseForm(CUIEvent uiEvt)
		{
			this.m_bInLobby = false;
			this.m_LobbyForm = null;
			this.m_SysEntryForm = null;
			this.m_RankingBtnForm = null;
			this.m_PlayerName = null;
			this.m_PvpLevel = null;
			this.m_PvpExpImg = null;
			this.m_PvpExpTxt = null;
			this.hero_btn = null;
			this.symbol_btn = null;
			this.bag_btn = null;
			this.task_btn = null;
			this.social_btn = null;
			this.addSkill_btn = null;
			this.achievement_btn = null;
			Singleton<CUIManager>.GetInstance().CloseForm(CLobbySystem.SYSENTRY_FORM_PATH);
			Singleton<CUIManager>.GetInstance().CloseForm(CLobbySystem.RANKING_BTN_FORM_PATH);
			this.UnInitWidget();
		}

		protected void OnPrepareFight_Sub(CUIEvent uiEvt)
		{
			Transform transform = this.m_LobbyForm.transform.Find("LobbyBottom/SysEntry/ChatBtn_sub");
			Transform transform2 = this.m_LobbyForm.transform.Find("LobbyBottom/SysEntry/ChatBtn");
			if (transform != null)
			{
				transform.gameObject.CustomSetActive(false);
			}
			if (transform2 != null)
			{
				transform2.gameObject.CustomSetActive(true);
			}
		}

		protected void OnPrepareFight_Origin(CUIEvent uiEvt)
		{
			Transform transform = this.m_LobbyForm.transform.Find("LobbyBottom/SysEntry/ChatBtn_sub");
			Transform transform2 = this.m_LobbyForm.transform.Find("LobbyBottom/SysEntry/ChatBtn");
			if (transform != null)
			{
				transform.gameObject.CustomSetActive(true);
			}
			if (transform2 != null)
			{
				transform2.gameObject.CustomSetActive(false);
			}
			Singleton<CUINewFlagSystem>.get_instance().HideNewFlagForBeizhanEntry();
		}

		private void InitForm(CUIFormScript form)
		{
			Transform transform = form.transform;
			this.m_PlayerName = transform.Find("PlayerHead/NameGroup/PlayerName").GetComponent<Text>();
			this.m_PvpLevel = transform.Find("PlayerHead/pvpLevel").GetComponent<Text>();
			this.m_PvpExpImg = transform.Find("PlayerHead/pvpExp/expBg/imgExp").GetComponent<Image>();
			this.m_PvpExpTxt = transform.Find("PlayerHead/pvpExp/expBg/txtExp").GetComponent<Text>();
			this.hero_btn = transform.Find("LobbyBottom/SysEntry/HeroBtn").gameObject;
			this.symbol_btn = transform.Find("LobbyBottom/SysEntry/SymbolBtn").gameObject;
			this.bag_btn = transform.Find("LobbyBottom/SysEntry/BagBtn").gameObject;
			this.task_btn = transform.Find("LobbyBottom/Newbie").gameObject;
			this.social_btn = transform.Find("LobbyBottom/SysEntry/SocialBtn").gameObject;
			this.addSkill_btn = transform.Find("LobbyBottom/SysEntry/AddedSkillBtn").gameObject;
			this.achievement_btn = transform.Find("LobbyBottom/SysEntry/AchievementBtn").gameObject;
			this.Check_Enable(18);
			this.Check_Enable(8);
			this.Check_Enable(19);
			this.Check_Enable(16);
			this.Check_Enable(13);
			this.Check_Enable(20);
			this.Check_Enable(22);
			this.Check_Enable(28);
			Transform transform2 = transform.Find("BtnCon/PvpBtn");
			Transform transform3 = transform.Find("BtnCon/LadderBtn");
			if (transform2)
			{
				CUICommonSystem.LoadUIPrefab(CLobbySystem.Pvp_BtnRes_PATH, "PvpBtnDynamic", transform2.gameObject, form);
			}
			if (transform3)
			{
				CUICommonSystem.LoadUIPrefab(CLobbySystem.Ladder_BtnRes_PATH, "LadderBtnDynamic", transform3.gameObject, form);
			}
			if (CSysDynamicBlock.bLobbyEntryBlocked)
			{
				Transform transform4 = transform.Find("Popup");
				if (transform4)
				{
					transform4.gameObject.CustomSetActive(false);
				}
				Transform transform5 = transform.Find("BtnCon/CompetitionBtn");
				if (transform5)
				{
					transform5.gameObject.CustomSetActive(false);
				}
				if (this.task_btn)
				{
					this.task_btn.CustomSetActive(false);
				}
				Transform transform6 = transform.Find("DiamondPayBtn");
				if (transform6)
				{
					transform6.gameObject.CustomSetActive(false);
				}
				Transform transform7 = transform.Find("Popup/BattleWebHome");
				if (transform7)
				{
					transform7.gameObject.CustomSetActive(false);
				}
			}
			Button component = transform.Find("BtnCon/LadderBtn").GetComponent<Button>();
			if (component)
			{
				component.interactable = Singleton<CLadderSystem>.GetInstance().IsLevelQualified();
				Transform transform8 = component.transform.Find("Lock");
				if (transform8)
				{
					transform8.gameObject.CustomSetActive(!component.interactable);
					transform8.SetAsLastSibling();
				}
			}
			Button component2 = transform.FindChild("BtnCon/UnionBtn").GetComponent<Button>();
			if (component2)
			{
				bool flag = Singleton<CUnionBattleEntrySystem>.GetInstance().IsUnionFuncLocked();
				component2.interactable = !flag;
				GameObject gameObject = component2.transform.FindChild("Lock").gameObject;
				gameObject.CustomSetActive(flag);
			}
			GameObject gameObject2 = transform.Find("PlayerHead/pvpExp/expEventPanel").gameObject;
			if (gameObject2 != null)
			{
				CUIEventScript cUIEventScript = gameObject2.GetComponent<CUIEventScript>();
				if (cUIEventScript == null)
				{
					cUIEventScript = gameObject2.AddComponent<CUIEventScript>();
					cUIEventScript.Initialize(form);
				}
				CUseable iconUseable = CUseableManager.CreateVirtualUseable(enVirtualItemType.enExp, 0);
				stUIEventParams eventParams = default(stUIEventParams);
				eventParams.iconUseable = iconUseable;
				eventParams.tag = 3;
				cUIEventScript.SetUIEvent(enUIEventType.Down, enUIEventID.Tips_ItemInfoOpen, eventParams);
				cUIEventScript.SetUIEvent(enUIEventType.HoldEnd, enUIEventID.Tips_ItemInfoClose, eventParams);
				cUIEventScript.SetUIEvent(enUIEventType.Click, enUIEventID.Tips_ItemInfoClose, eventParams);
				cUIEventScript.SetUIEvent(enUIEventType.DragEnd, enUIEventID.Tips_ItemInfoClose, eventParams);
			}
			CLobbySystem.RefreshDianQuanPayButton(false);
			GameObject widget = form.GetWidget(7);
			if (CSysDynamicBlock.bLobbyEntryBlocked)
			{
				widget.CustomSetActive(false);
			}
			else
			{
				Text component3 = form.GetWidget(8).GetComponent<Text>();
				if (GameDataMgr.globalInfoDatabin.GetDataByKey(204u).dwConfValue > 0u)
				{
					widget.CustomSetActive(CLobbySystem.IsPlatChannelOpen);
					component3.text = Singleton<CTextManager>.GetInstance().GetText("CrossPlat_Plat_Channel_Open_Lobby_Msg");
				}
				else
				{
					widget.CustomSetActive(!CLobbySystem.IsPlatChannelOpen);
					component3.text = Singleton<CTextManager>.GetInstance().GetText("CrossPlat_Plat_Channel_Not_Open_Lobby_Msg");
				}
			}
		}

		private void InitRankingBtnForm()
		{
			if (this.m_RankingBtnForm == null)
			{
				DebugHelper.Assert(false, "m_RankingBtnForm cannot be null!!!");
				return;
			}
			this._rankingBtn = this.m_RankingBtnForm.GetWidget(0);
			if (this._rankingBtn && CSysDynamicBlock.bSocialBlocked)
			{
				this._rankingBtn.CustomSetActive(false);
			}
			this.RefreshRankList();
		}

		private void InitOther(CUIFormScript m_FormScript)
		{
			Singleton<CTimerManager>.GetInstance().AddTimer(50, 1, new CTimer.OnTimeUpHandler(this.CheckNewbieIntro));
			this.ProcessQQVIP(m_FormScript);
			this.UpdateGameCenterState(m_FormScript);
			MonoSingleton<NobeSys>.GetInstance().ShowDelayNobeTipsInfo();
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null && masterRoleInfo.m_licenseInfo != null)
			{
				masterRoleInfo.m_licenseInfo.ReviewLicenseList();
			}
		}

		private void InitSysEntryForm(CUIFormScript form)
		{
			Transform transform = form.gameObject.transform;
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo();
			this.m_lblGlodCoin = transform.FindChild("PlayerBtn/GoldCoin/Text").GetComponent<Text>();
			this.m_lblDianquan = transform.FindChild("PlayerBtn/Dianquan/Text").GetComponent<Text>();
			this.m_lblDiamond = transform.FindChild("PlayerBtn/Diamond/Text").GetComponent<Text>();
			this.m_wifiIcon = form.GetWidget(0);
			this.m_wifiInfo = form.GetWidget(1);
			this.m_ping = form.GetWidget(2).GetComponent<Text>();
			this.m_textMianliu = form.GetWidget(9);
			this.m_lblGlodCoin.text = this.GetCoinString(masterRoleInfo.GoldCoin);
			this.m_lblDianquan.text = this.GetCoinString((uint)masterRoleInfo.DianQuan);
			this.m_lblDiamond.text = this.GetCoinString(masterRoleInfo.Diamond);
			GameObject gameObject = transform.Find("PlayerBtn/GoldCoin").gameObject;
			if (gameObject != null)
			{
				CUIEventScript cUIEventScript = gameObject.GetComponent<CUIEventScript>();
				if (cUIEventScript == null)
				{
					cUIEventScript = gameObject.AddComponent<CUIEventScript>();
					cUIEventScript.Initialize(form);
				}
				CUseable iconUseable = CUseableManager.CreateCoinUseable(4, (int)masterRoleInfo.GoldCoin);
				stUIEventParams eventParams = default(stUIEventParams);
				eventParams.iconUseable = iconUseable;
				eventParams.tag = 3;
				cUIEventScript.SetUIEvent(enUIEventType.Down, enUIEventID.Tips_ItemInfoOpen, eventParams);
				cUIEventScript.SetUIEvent(enUIEventType.HoldEnd, enUIEventID.Tips_ItemInfoClose, eventParams);
				cUIEventScript.SetUIEvent(enUIEventType.Click, enUIEventID.Tips_ItemInfoClose, eventParams);
				cUIEventScript.SetUIEvent(enUIEventType.DragEnd, enUIEventID.Tips_ItemInfoClose, eventParams);
			}
			GameObject gameObject2 = transform.Find("PlayerBtn/Diamond").gameObject;
			if (gameObject2 != null)
			{
				CUIEventScript cUIEventScript2 = gameObject2.GetComponent<CUIEventScript>();
				if (cUIEventScript2 == null)
				{
					cUIEventScript2 = gameObject2.AddComponent<CUIEventScript>();
					cUIEventScript2.Initialize(form);
				}
				CUseable iconUseable2 = CUseableManager.CreateCoinUseable(10, (int)masterRoleInfo.Diamond);
				stUIEventParams eventParams2 = default(stUIEventParams);
				eventParams2.iconUseable = iconUseable2;
				eventParams2.tag = 3;
				cUIEventScript2.SetUIEvent(enUIEventType.Down, enUIEventID.Tips_ItemInfoOpen, eventParams2);
				cUIEventScript2.SetUIEvent(enUIEventType.HoldEnd, enUIEventID.Tips_ItemInfoClose, eventParams2);
				cUIEventScript2.SetUIEvent(enUIEventType.Click, enUIEventID.Tips_ItemInfoClose, eventParams2);
				cUIEventScript2.SetUIEvent(enUIEventType.DragEnd, enUIEventID.Tips_ItemInfoClose, eventParams2);
			}
			if (!ApolloConfig.payEnabled)
			{
				Transform transform2 = transform.Find("PlayerBtn/Dianquan/Button");
				if (transform2 != null)
				{
					transform2.gameObject.CustomSetActive(false);
				}
			}
			this.m_SysEntry = this.m_LobbyForm.gameObject.transform.Find("LobbyBottom/SysEntry").gameObject;
			this.m_Btns = new DictionaryView<int, GameObject>();
			this.m_Btns.Add(0, this.m_SysEntry.transform.Find("HeroBtn").gameObject);
			this.m_Btns.Add(1, this.m_SysEntry.transform.Find("SymbolBtn").gameObject);
			this.m_Btns.Add(2, this.m_SysEntry.transform.Find("AchievementBtn").gameObject);
			this.m_Btns.Add(3, this.m_SysEntry.transform.Find("BagBtn").gameObject);
			this.m_Btns.Add(5, this.m_SysEntry.transform.Find("SocialBtn").gameObject);
			this.m_Btns.Add(6, form.transform.Find("PlayerBtn/FriendBtn").gameObject);
			this.m_Btns.Add(7, this.m_SysEntry.transform.Find("AddedSkillBtn").gameObject);
			this.m_Btns.Add(8, form.transform.Find("PlayerBtn/MailBtn").gameObject);
			this.m_Btns.Add(9, Utility.FindChild(this.m_LobbyForm.gameObject, "Popup/ActBtn"));
			this.m_Btns.Add(10, Utility.FindChild(this.m_LobbyForm.gameObject, "Popup/BoardBtn"));
			this.m_Btns.Add(4, this.m_LobbyForm.gameObject.transform.Find("LobbyBottom/Newbie/RedDotPanel").gameObject);
		}

		public static void RefreshDianQuanPayButton(bool notifyFromSvr = false)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CLobbySystem.LOBBY_FORM_PATH);
			if (form != null)
			{
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				GameObject gameObject = form.transform.Find("DiamondPayBtn").gameObject;
				CUIEventScript component = gameObject.GetComponent<CUIEventScript>();
				CTextManager instance = Singleton<CTextManager>.GetInstance();
				if (!masterRoleInfo.IsGuidedStateSet(22))
				{
					CUICommonSystem.SetButtonName(gameObject, instance.GetText("Pay_Btn_FirstPay"));
					component.SetUIEvent(enUIEventType.Click, enUIEventID.Pay_OpenFirstPayPanel);
					CUICommonSystem.DelRedDot(gameObject);
				}
				else if (!masterRoleInfo.IsGuidedStateSet(23))
				{
					CUICommonSystem.SetButtonName(gameObject, instance.GetText("Pay_Btn_FirstPay"));
					component.SetUIEvent(enUIEventType.Click, enUIEventID.Pay_OpenFirstPayPanel);
					CUICommonSystem.AddRedDot(gameObject, enRedDotPos.enTopRight, 0);
				}
				else if (!masterRoleInfo.IsGuidedStateSet(24))
				{
					CUICommonSystem.SetButtonName(gameObject, instance.GetText("Pay_Btn_Renewal"));
					component.SetUIEvent(enUIEventType.Click, enUIEventID.Pay_OpenRenewalPanel);
					CUICommonSystem.DelRedDot(gameObject);
				}
				else if (!masterRoleInfo.IsGuidedStateSet(25))
				{
					CUICommonSystem.SetButtonName(gameObject, instance.GetText("Pay_Btn_Renewal"));
					component.SetUIEvent(enUIEventType.Click, enUIEventID.Pay_OpenRenewalPanel);
					CUICommonSystem.AddRedDot(gameObject, enRedDotPos.enTopRight, 0);
				}
				else if (masterRoleInfo.IsClientBitsSet(0))
				{
					CUICommonSystem.SetButtonName(gameObject, instance.GetText("GotoTehuiShopName"));
					component.SetUIEvent(enUIEventType.Click, enUIEventID.Pay_TehuiShop);
				}
				else if (notifyFromSvr)
				{
					masterRoleInfo.SetClientBits(0, true, false);
					CLobbySystem.RefreshDianQuanPayButton(false);
				}
				else
				{
					gameObject.CustomSetActive(false);
				}
			}
		}

		private void UpdateNobeIcon()
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (this.m_LobbyForm != null && this.m_LobbyForm.gameObject.activeSelf && masterRoleInfo != null)
			{
				GameObject widget = this.m_LobbyForm.GetWidget(2);
				if (widget != null)
				{
					CUIHttpImageScript component = widget.GetComponent<CUIHttpImageScript>();
					MonoSingleton<NobeSys>.GetInstance().SetNobeIcon(component.GetComponent<Image>(), (int)masterRoleInfo.GetNobeInfo().stGameVipClient.dwCurLevel, false);
					Image component2 = this.m_LobbyForm.GetWidget(3).GetComponent<Image>();
					MonoSingleton<NobeSys>.GetInstance().SetHeadIconBk(component2, (int)masterRoleInfo.GetNobeInfo().stGameVipClient.dwHeadIconId);
					MonoSingleton<NobeSys>.GetInstance().SetHeadIconBkEffect(component2, (int)masterRoleInfo.GetNobeInfo().stGameVipClient.dwHeadIconId, this.m_LobbyForm, 0.7f);
				}
			}
		}

		private void UpdateNobeHeadIdx()
		{
			int dwHeadIconId = (int)MonoSingleton<NobeSys>.GetInstance().m_vipInfo.stGameVipClient.dwHeadIconId;
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (this.m_LobbyForm != null && this.m_LobbyForm.gameObject.activeSelf && masterRoleInfo != null)
			{
				Image component = this.m_LobbyForm.GetWidget(3).GetComponent<Image>();
				if (component != null)
				{
					MonoSingleton<NobeSys>.GetInstance().SetHeadIconBk(component, dwHeadIconId);
					MonoSingleton<NobeSys>.GetInstance().SetHeadIconBkEffect(component, dwHeadIconId, this.m_LobbyForm, 0.7f);
				}
				this.RefreshRankList();
			}
		}

		private void OnCheckNewbieIntro()
		{
			Singleton<CTimerManager>.GetInstance().AddTimer(100, 1, delegate(int seq)
			{
				this.PopupNewbieIntro();
			});
		}

		private void CheckNewbieIntro(int timerSeq)
		{
			if (!this.PopupNewbieIntro() && !CLobbySystem._autoPoped)
			{
				CLobbySystem._autoPoped = true;
			}
		}

		private void OnPlayerLvlChange()
		{
			if (this.m_LobbyForm)
			{
				Transform transform = this.m_LobbyForm.transform;
				Button component = transform.Find("BtnCon/LadderBtn").GetComponent<Button>();
				if (component)
				{
					component.interactable = Singleton<CLadderSystem>.GetInstance().IsLevelQualified();
					Transform transform2 = component.transform.Find("Lock");
					if (transform2)
					{
						transform2.gameObject.CustomSetActive(!component.interactable);
						transform2.SetAsLastSibling();
					}
				}
				Button component2 = transform.FindChild("BtnCon/UnionBtn").GetComponent<Button>();
				if (component2)
				{
					bool flag = Singleton<CUnionBattleEntrySystem>.GetInstance().IsUnionFuncLocked();
					component2.interactable = !flag;
					GameObject gameObject = component2.transform.FindChild("Lock").gameObject;
					gameObject.CustomSetActive(flag);
				}
			}
		}

		private void StartAutoPopupChain(int timerSeq)
		{
			CLobbySystem.AutoPopAllow &= !MonoSingleton<NewbieGuideManager>.GetInstance().isNewbieGuiding;
			if (CLobbySystem.AutoPopAllow)
			{
				this.AutoPopup1_IDIP();
			}
		}

		private bool PopupNewbieIntro()
		{
			if (CSysDynamicBlock.bNewbieBlocked)
			{
				return true;
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			DebugHelper.Assert(masterRoleInfo != null, "Master Role info is NULL!");
			if (masterRoleInfo != null && !masterRoleInfo.IsNewbieAchieveSet(84) && Singleton<CUIManager>.GetInstance().GetForm("UGUI/Form/System/Newbie/Form_NewbieSettle.prefab") == null)
			{
				masterRoleInfo.SetNewbieAchieve(84, true, true);
				Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Wealfare_CloseForm);
				return true;
			}
			return false;
		}

		private void AutoPopup1_IDIP()
		{
			if (MonoSingleton<IDIPSys>.GetInstance().RedPotState)
			{
				Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.IDIP_CloseForm, new CUIEventManager.OnUIEventHandler(this.OnIDIPClose));
				Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.IDIP_OpenForm);
			}
			else
			{
				this.AutoPopup2_Activity();
			}
		}

		private void OnIDIPClose(CUIEvent uiEvt)
		{
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.IDIP_CloseForm, new CUIEventManager.OnUIEventHandler(this.OnIDIPClose));
			this.AutoPopup2_Activity();
		}

		private void AutoPopup2_Activity()
		{
			if (Singleton<ActivitySys>.GetInstance().CheckReadyForDot(1))
			{
				Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Activity_CloseForm, new CUIEventManager.OnUIEventHandler(this.OnActivityClose));
				Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Activity_OpenForm);
			}
		}

		private void OnActivityClose(CUIEvent uiEvt)
		{
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Activity_CloseForm, new CUIEventManager.OnUIEventHandler(this.OnActivityClose));
		}

		public void Check_Enable(RES_SPECIALFUNCUNLOCK_TYPE type)
		{
			bool flag = Singleton<CFunctionUnlockSys>.get_instance().TipsHasShow(type);
			if (!flag && !Singleton<CFunctionUnlockSys>.get_instance().IsTypeHasCondition(type))
			{
				flag = true;
			}
			this.SetEnable(type, flag);
		}

		private void SetEnable(RES_SPECIALFUNCUNLOCK_TYPE type, bool bShow)
		{
			GameObject gameObject;
			if (type == 18)
			{
				gameObject = this.hero_btn;
			}
			else if (type == 8)
			{
				gameObject = this.symbol_btn;
			}
			else if (type == 19)
			{
				gameObject = this.bag_btn;
			}
			else if (type == 16)
			{
				gameObject = this.task_btn;
			}
			else if (type == 13)
			{
				gameObject = this.social_btn;
			}
			else if (type == 22)
			{
				gameObject = this.addSkill_btn;
			}
			else if (type == 28)
			{
				gameObject = this.achievement_btn;
			}
			else
			{
				gameObject = null;
			}
			if (gameObject == null)
			{
				return;
			}
			gameObject.CustomSetActive(bShow);
		}

		private void On_Lobby_UnlockAnimation_End(CUIEvent uievent)
		{
			Singleton<CUIManager>.get_instance().CloseForm(CLobbySystem.LOBBY_FUN_UNLOCK_PATH);
			Singleton<CSoundManager>.get_instance().PostEvent("UI_hall_system_back", null);
			this.SetEnable(uievent.m_eventParams.tag, true);
		}

		private void On_Lobby_MysteryShopClose(CUIEvent uiEvent)
		{
			GameObject gameObject = Utility.FindChild(uiEvent.m_srcFormScript.gameObject, "Popup/BoardBtn/MysteryShop");
			Debug.LogWarning(string.Format("mystery shop icon on close:{0}", gameObject));
			gameObject.CustomSetActive(false);
		}

		private void UpdateGameCenterState(CUIFormScript form)
		{
			if (null == form)
			{
				return;
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			COM_PRIVILEGE_TYPE privilegeType = (masterRoleInfo != null) ? masterRoleInfo.m_privilegeType : 0;
			GameObject obj = Utility.FindChild(form.gameObject, "VIPGroup/WXGameCenterBtn");
			GameObject obj2 = Utility.FindChild(form.gameObject, "PlayerHead/NameGroup/WXGameCenterIcon");
			MonoSingleton<NobeSys>.GetInstance().SetGameCenterVisible(obj, privilegeType, 1, false, CSysDynamicBlock.bLobbyEntryBlocked);
			MonoSingleton<NobeSys>.GetInstance().SetGameCenterVisible(obj2, privilegeType, 1, true, false);
			GameObject obj3 = Utility.FindChild(form.gameObject, "VIPGroup/QQGameCenterBtn");
			GameObject obj4 = Utility.FindChild(form.gameObject, "PlayerHead/NameGroup/QQGameCenterIcon");
			MonoSingleton<NobeSys>.GetInstance().SetGameCenterVisible(obj3, privilegeType, 2, false, CSysDynamicBlock.bLobbyEntryBlocked);
			MonoSingleton<NobeSys>.GetInstance().SetGameCenterVisible(obj4, privilegeType, 2, true, false);
			GameObject obj5 = Utility.FindChild(form.gameObject, "VIPGroup/GuestGameCenterBtn");
			GameObject obj6 = Utility.FindChild(form.gameObject, "PlayerHead/NameGroup/GuestGameCenterIcon");
			MonoSingleton<NobeSys>.GetInstance().SetGameCenterVisible(obj5, privilegeType, 5, false, CSysDynamicBlock.bLobbyEntryBlocked);
			MonoSingleton<NobeSys>.GetInstance().SetGameCenterVisible(obj6, privilegeType, 5, true, false);
		}

		public void CheckWifi()
		{
			if (this.m_wifiIcon == null || this.m_wifiInfo == null || this.m_ping == null)
			{
				return;
			}
			int num = (int)Singleton<NetworkModule>.GetInstance().lobbyPing;
			num = ((num <= 100) ? num : ((num - 100) * 7 / 10 + 100));
			num = Mathf.Clamp(num, 0, 460);
			uint num2;
			if (num < 100)
			{
				num2 = 2u;
			}
			else if (num < 200)
			{
				num2 = 1u;
			}
			else
			{
				num2 = 0u;
			}
			if (this.m_wifiIconCheckTicks == -1 || this.m_wifiIconCheckTicks >= this.m_wifiIconCheckMaxTicks)
			{
				NetworkReachability internetReachability = Application.internetReachability;
				if (internetReachability == NetworkReachability.NotReachable)
				{
					CUICommonSystem.PlayAnimator(this.m_wifiIcon, CLobbySystem.s_noNetStateName);
				}
				else if (internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
				{
					CUICommonSystem.PlayAnimator(this.m_wifiIcon, CLobbySystem.s_wifiStateName[(int)((UIntPtr)num2)]);
				}
				else if (internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
				{
					CUICommonSystem.PlayAnimator(this.m_wifiIcon, CLobbySystem.s_netStateName[(int)((UIntPtr)num2)]);
				}
				this.m_wifiIconCheckTicks = 0;
			}
			else
			{
				this.m_wifiIconCheckTicks++;
			}
			if (this.m_wifiInfo && this.m_wifiInfo.activeInHierarchy)
			{
				this.m_ping.text = num + "ms";
			}
		}

		public void CheckMianLiu()
		{
			if (this.m_textMianliu != null)
			{
				if (MonoSingleton<CTongCaiSys>.get_instance().IsLianTongIp() && MonoSingleton<CTongCaiSys>.get_instance().IsCanUseTongCai())
				{
					this.m_textMianliu.CustomSetActive(true);
				}
				else
				{
					this.m_textMianliu.CustomSetActive(false);
				}
			}
		}

		public void ShowOrHideWifiInfo()
		{
			if (this.m_wifiInfo != null)
			{
				this.m_wifiInfo.CustomSetActive(!this.m_wifiInfo.activeInHierarchy);
			}
			this.CheckWifi();
		}

		public string GetCoinString(uint coinValue)
		{
			string result = coinValue.ToString();
			if (coinValue > CLobbySystem.s_CoinShowMaxValue)
			{
				int num = (int)(coinValue / CLobbySystem.s_CoinShowStepValue);
				result = string.Format("{0}万", num);
			}
			return result;
		}

		public void FullShow()
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CLobbySystem.SYSENTRY_FORM_PATH);
			if (form == null)
			{
				return;
			}
			Transform transform = form.transform;
			transform.Find("PlayerBtn/MailBtn").gameObject.CustomSetActive(true);
			transform.Find("PlayerBtn/SettingBtn").gameObject.CustomSetActive(true);
			transform.Find("PlayerBtn/FriendBtn").gameObject.CustomSetActive(true);
		}

		public void MiniShow()
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CLobbySystem.SYSENTRY_FORM_PATH);
			if (form == null)
			{
				return;
			}
			Transform transform = form.transform;
			transform.Find("PlayerBtn/MailBtn").gameObject.CustomSetActive(false);
			transform.Find("PlayerBtn/SettingBtn").gameObject.CustomSetActive(false);
			transform.Find("PlayerBtn/FriendBtn").gameObject.CustomSetActive(false);
		}

		public void AddRedDot(enSysEntryID sysEntryId, enRedDotPos redDotPos = enRedDotPos.enTopRight, int count = 0)
		{
			if (this.m_Btns != null)
			{
				GameObject target;
				this.m_Btns.TryGetValue((int)sysEntryId, ref target);
				CUICommonSystem.AddRedDot(target, redDotPos, count);
			}
		}

		public void AddRedDotEx(enSysEntryID sysEntryId, enRedDotPos redDotPos = enRedDotPos.enTopRight, int alertNum = 0)
		{
			if (this.m_Btns != null)
			{
				GameObject target;
				this.m_Btns.TryGetValue((int)sysEntryId, ref target);
				CUICommonSystem.AddRedDot(target, redDotPos, alertNum);
			}
		}

		public void DelRedDot(enSysEntryID sysEntryId)
		{
			if (this.m_Btns != null)
			{
				GameObject target;
				this.m_Btns.TryGetValue((int)sysEntryId, ref target);
				CUICommonSystem.DelRedDot(target);
			}
		}

		private bool checkIsHaveRedDot()
		{
			bool result = false;
			DictionaryView<int, GameObject>.Enumerator enumerator = this.m_Btns.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<int, GameObject> current = enumerator.get_Current();
				if (CUICommonSystem.IsHaveRedDot(current.get_Value()))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		private void OnTaskUpdate()
		{
			CTaskModel model = Singleton<CTaskSys>.get_instance().model;
			model.task_Data.Sort(2);
			model.task_Data.Sort(1);
			model.task_Data.Sort(0);
			int num = Singleton<CTaskSys>.get_instance().model.GetMainTask_RedDotCount();
			num += Singleton<CTaskSys>.get_instance().model.task_Data.GetTask_Count(enTaskTab.TAB_USUAL, CTask.State.Have_Done);
			if (num > 0)
			{
				this.AddRedDot(enSysEntryID.TaskBtn, enRedDotPos.enTopRight, num);
			}
			else
			{
				this.DelRedDot(enSysEntryID.TaskBtn);
			}
		}

		private void OnFriendSysIconUpdate()
		{
			int dataCount = Singleton<CFriendContoller>.GetInstance().model.GetDataCount(CFriendModel.FriendType.RequestFriend);
			int dataCount2 = Singleton<CFriendContoller>.GetInstance().model.GetDataCount(CFriendModel.FriendType.MentorRequestList);
			bool flag = Singleton<CFriendContoller>.GetInstance().model.FRData.HasRedDot();
			bool flag2 = Singleton<CTaskSys>.get_instance().IsMentorTaskRedDot();
			if (dataCount > 0 || dataCount2 > 0 || flag || flag2)
			{
				this.AddRedDot(enSysEntryID.FriendBtn, enRedDotPos.enTopRight, 0);
			}
			else
			{
				this.DelRedDot(enSysEntryID.FriendBtn);
			}
		}

		private void OnCheckRedDotByServerVersionWithLobby()
		{
			if (CUIRedDotSystem.IsShowRedDotByVersion(enRedID.Mall_HeroTab) || CUIRedDotSystem.IsShowRedDotByVersion(enRedID.Mall_HeroSkinTab) || CUIRedDotSystem.IsShowRedDotByVersion(enRedID.Mall_SymbolTab) || CUIRedDotSystem.IsShowRedDotByVersion(enRedID.Mall_SaleTab) || CUIRedDotSystem.IsShowRedDotByVersion(enRedID.Mall_LotteryTab) || CUIRedDotSystem.IsShowRedDotByVersion(enRedID.Mall_RecommendTab) || (CUIRedDotSystem.IsShowRedDotByVersion(enRedID.Mall_MysteryTab) && CUIRedDotSystem.IsShowRedDotByLogic(enRedID.Mall_MysteryTab)) || CUIRedDotSystem.IsShowRedDotByVersion(enRedID.Mall_BoutiqueTab) || CUIRedDotSystem.IsShowRedDotByLogic(enRedID.Mall_SymbolTab))
			{
				this.AddRedDot(enSysEntryID.MallBtn, enRedDotPos.enTopRight, 0);
			}
			this.CheckGotoWebEntryRedDot();
		}

		private void OnCheckDelMallEntryRedDot()
		{
			if (!CUIRedDotSystem.IsShowRedDotByVersion(enRedID.Mall_HeroTab) && !CUIRedDotSystem.IsShowRedDotByVersion(enRedID.Mall_HeroSkinTab) && !CUIRedDotSystem.IsShowRedDotByVersion(enRedID.Mall_SymbolTab) && !CUIRedDotSystem.IsShowRedDotByVersion(enRedID.Mall_SaleTab) && !CUIRedDotSystem.IsShowRedDotByVersion(enRedID.Mall_LotteryTab) && !CUIRedDotSystem.IsShowRedDotByVersion(enRedID.Mall_RecommendTab) && (!CUIRedDotSystem.IsShowRedDotByVersion(enRedID.Mall_MysteryTab) || !CUIRedDotSystem.IsShowRedDotByLogic(enRedID.Mall_MysteryTab)) && !CUIRedDotSystem.IsShowRedDotByVersion(enRedID.Mall_BoutiqueTab) && !CUIRedDotSystem.IsShowRedDotByLogic(enRedID.Mall_SymbolTab))
			{
				this.DelRedDot(enSysEntryID.MallBtn);
			}
		}

		private void CheckGotoWebEntryRedDot()
		{
			if (CUIRedDotSystem.IsShowRedDotByVersion(enRedID.Lobby_GongLueEntry) && this.m_LobbyForm != null)
			{
				Transform transform = this.m_LobbyForm.transform.Find("Popup/SignBtn");
				if (transform != null)
				{
					CUIRedDotSystem.AddRedDot(transform.gameObject, enRedDotPos.enTopRight, 0);
				}
			}
			if (CUIRedDotSystem.IsShowRedDotByVersion(enRedID.Lobby_BattleHomeWeb) && this.m_LobbyForm != null)
			{
				Transform transform2 = this.m_LobbyForm.transform.Find("Popup/BattleWebHome");
				if (transform2 != null)
				{
					CUIRedDotSystem.AddRedDot(transform2.gameObject, enRedDotPos.enTopRight, 0);
				}
			}
		}

		public void ValidateActivitySpot()
		{
			if (this.m_bInLobby)
			{
				if (Singleton<ActivitySys>.GetInstance().CheckReadyForDot(1))
				{
					uint reveivableRedDot = Singleton<ActivitySys>.GetInstance().GetReveivableRedDot(1);
					this.AddRedDotEx(enSysEntryID.ActivityBtn, enRedDotPos.enTopRight, (int)reveivableRedDot);
				}
				else if (MonoSingleton<IDIPSys>.GetInstance().HaveUpdateList)
				{
					this.AddRedDotEx(enSysEntryID.ActivityBtn, enRedDotPos.enTopRight, 0);
				}
				else if (MonoSingleton<PandroaSys>.GetInstance().ShowRedPoint)
				{
					this.AddRedDotEx(enSysEntryID.ActivityBtn, enRedDotPos.enTopRight, 0);
				}
				else
				{
					this.DelRedDot(enSysEntryID.ActivityBtn);
				}
			}
		}

		private void OnMailUnReadUpdate()
		{
			int unReadMailCount = Singleton<CMailSys>.get_instance().GetUnReadMailCount(true);
			if (this.m_LobbyForm != null)
			{
				if (unReadMailCount > 0)
				{
					this.AddRedDot(enSysEntryID.MailBtn, enRedDotPos.enTopRight, 0);
				}
				else
				{
					this.DelRedDot(enSysEntryID.MailBtn);
				}
			}
		}

		private void OnAchieveStateUpdate()
		{
			CAchieveInfo2 masterAchieveInfo = CAchieveInfo2.GetMasterAchieveInfo();
			if (masterAchieveInfo.HasRewardNotGot())
			{
				this.AddRedDot(enSysEntryID.AchievementBtn, enRedDotPos.enTopRight, 0);
			}
			else
			{
				this.DelRedDot(enSysEntryID.AchievementBtn);
			}
		}

		private void OnBagItemsUpdate()
		{
			this.ValidateActivitySpot();
			this.OnCheckSymbolEquipAlert();
		}

		public void OnCheckSymbolEquipAlert()
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo == null)
			{
				return;
			}
			int num;
			uint num2;
			if (masterRoleInfo.m_symbolInfo.CheckAnyWearSymbol(out num, out num2, 0))
			{
				this.AddRedDot(enSysEntryID.SymbolBtn, enRedDotPos.enTopRight, 0);
			}
			else
			{
				this.DelRedDot(enSysEntryID.SymbolBtn);
			}
		}

		private void OnGuildSignStateChanged(bool isSigned)
		{
			if (isSigned)
			{
				if (!Singleton<CGuildMatchSystem>.GetInstance().IsInGuildMatchTime() || Singleton<CGuildMatchSystem>.GetInstance().m_isGuildMatchBtnClicked)
				{
					this.DelRedDot(enSysEntryID.GuildBtn);
				}
			}
			else
			{
				this.AddRedDot(enSysEntryID.GuildBtn, enRedDotPos.enTopRight, 0);
			}
		}

		private void OnPureLobbyShow()
		{
			if (Singleton<CGuildSystem>.GetInstance().IsInNormalGuild())
			{
				if (Singleton<CGuildMatchSystem>.GetInstance().IsInGuildMatchTime() && !Singleton<CGuildMatchSystem>.GetInstance().m_isGuildMatchBtnClicked)
				{
					this.AddRedDot(enSysEntryID.GuildBtn, enRedDotPos.enTopRight, 0);
				}
				else if (CGuildHelper.IsPlayerSigned())
				{
					this.DelRedDot(enSysEntryID.GuildBtn);
				}
			}
		}

		private void OnCheckUpdateClientVersion()
		{
			if (Singleton<LobbyLogic>.get_instance().NeedUpdateClient)
			{
				Singleton<LobbyLogic>.get_instance().NeedUpdateClient = false;
				Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("VersionIsLow"), enUIEventID.None, false);
			}
		}
	}
}
