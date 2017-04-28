using Assets.Scripts.Framework;
using Assets.Scripts.UI;
using CSProtocol;
using ResData;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	[MessageHandlerClass]
	public class CPlayerInfoSystem : Singleton<CPlayerInfoSystem>
	{
		public enum DetailPlayerInfoSource
		{
			DefaultOthers,
			Self,
			Guild
		}

		public enum enPlayerFormWidget
		{
			Tab,
			Base_Info_Tab,
			Pvp_Info_Tab,
			Change_Name_Button,
			CreditScore_Tab,
			License_Info_Tab,
			License_List,
			Common_Hero_info,
			Rule_Btn,
			Body,
			Juhua,
			Update_Sub_Module_Timer,
			Title,
			PersonSign,
			AppointOrCancelMatchLeader,
			ShareScreenShot
		}

		public enum Tab
		{
			Base_Info,
			Pvp_Info,
			Honor_Info,
			Common_Hero,
			PvpHistory_Info,
			PvpCreditScore_Info,
			Mentor_Info
		}

		public const ushort PlAYER_INFO_RULE_ID = 3;

		public const ushort CREDIT_RULE_ID = 11;

		private CPlayerInfoSystem.DetailPlayerInfoSource _lastDetailSource;

		private bool _isShowGuildAppointViceChairmanBtn;

		private bool _isShowGuildTransferPositionBtn;

		private bool _isShowGuildFireMemberBtn;

		private bool isShowPlayerInfoDirectly = true;

		private CPlayerInfoSystem.Tab m_CurTab;

		public static string sPlayerInfoFormPath = "UGUI/Form/System/Player/Form_Player_Info.prefab";

		private bool m_IsFormOpen;

		private CUIFormScript m_Form;

		private CPlayerProfile m_PlayerProfile = new CPlayerProfile();

		public CPlayerInfoSystem.Tab CurTab
		{
			get
			{
				return this.m_CurTab;
			}
			set
			{
				this.m_CurTab = value;
			}
		}

		public void ShowPlayerDetailInfo(ulong ullUid, int iLogicWorldId, CPlayerInfoSystem.DetailPlayerInfoSource sourceType = CPlayerInfoSystem.DetailPlayerInfoSource.DefaultOthers, bool isShowDirectly = true)
		{
			this._lastDetailSource = sourceType;
			if (this._lastDetailSource == CPlayerInfoSystem.DetailPlayerInfoSource.Self)
			{
				this.m_PlayerProfile.ConvertRoleInfoData(Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo());
				this.OpenForm();
			}
			else if (ullUid > 0uL)
			{
				this.isShowPlayerInfoDirectly = isShowDirectly;
				this.ReqOtherPlayerDetailInfo(ullUid, iLogicWorldId);
			}
		}

		public void ShowPlayerDetailInfo(ulong ullUid, int iLogicWorldId, bool isShowGuildAppointViceChairmanBtn, bool isShowGuildTransferPositionBtn, bool isShowGuildFireMemberBtn)
		{
			this._isShowGuildAppointViceChairmanBtn = isShowGuildAppointViceChairmanBtn;
			this._isShowGuildTransferPositionBtn = isShowGuildTransferPositionBtn;
			this._isShowGuildFireMemberBtn = isShowGuildFireMemberBtn;
			this.ShowPlayerDetailInfo(ullUid, iLogicWorldId, CPlayerInfoSystem.DetailPlayerInfoSource.Guild, true);
		}

		private void ReqOtherPlayerDetailInfo(ulong ullUid, int iLogicWorldId)
		{
			if (ullUid <= 0uL)
			{
				return;
			}
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(2606u);
			cSPkg.stPkgData.get_stGetAcntDetailInfoReq().ullUid = ullUid;
			cSPkg.stPkgData.get_stGetAcntDetailInfoReq().iLogicWorldId = iLogicWorldId;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		[MessageHandler(2607)]
		public static void ResPlyaerDetailInfo(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			if (msg.stPkgData.get_stGetAcntDetailInfoRsp().iErrCode == 0)
			{
				if (Singleton<CPlayerInfoSystem>.GetInstance().isShowPlayerInfoDirectly)
				{
					Singleton<CPlayerInfoSystem>.get_instance().ImpResDetailInfo(msg);
				}
				else
				{
					Singleton<EventRouter>.GetInstance().BroadCastEvent<CSPkg>(EventID.PlayerInfoSystem_Info_Received, msg);
				}
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips(Utility.ProtErrCodeToStr(2607, 163), false, 1.5f, null, new object[0]);
			}
		}

		private void ImpResDetailInfo(CSPkg msg)
		{
			if (msg.stPkgData.get_stGetAcntDetailInfoRsp().iErrCode != 0)
			{
				Singleton<CUIManager>.GetInstance().OpenMessageBox(string.Format("Error Code {0}", msg.stPkgData.get_stGetAcntDetailInfoRsp().iErrCode), false);
				return;
			}
			this.m_PlayerProfile.ConvertServerDetailData(msg.stPkgData.get_stGetAcntDetailInfoRsp().stAcntDetail.get_stOfSucc());
			this.OpenForm();
		}

		[MessageHandler(1194)]
		public static void ChangePersonSgin(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			if (msg.stPkgData.get_stSignatureRsp().dwResult != 0u)
			{
				Singleton<CUIManager>.GetInstance().OpenMessageBox(Utility.ProtErrCodeToStr(1194, (int)msg.stPkgData.get_stSignatureRsp().dwResult), false);
			}
			else
			{
				if (Singleton<CPlayerInfoSystem>.GetInstance().CurTab == CPlayerInfoSystem.Tab.Base_Info)
				{
					Singleton<CPlayerInfoSystem>.GetInstance().UpdateBaseInfo();
				}
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				if (masterRoleInfo != null)
				{
					masterRoleInfo.PersonSign = Singleton<CPlayerInfoSystem>.get_instance().m_PlayerProfile.m_personSign;
				}
			}
		}

		public override void Init()
		{
			base.Init();
			CUIEventManager instance = Singleton<CUIEventManager>.GetInstance();
			instance.AddUIEventListener(enUIEventID.Player_Info_OpenForm, new CUIEventManager.OnUIEventHandler(this.OnPlayerInfo_OpenForm));
			instance.AddUIEventListener(enUIEventID.Player_Info_CloseForm, new CUIEventManager.OnUIEventHandler(this.OnPlayerInfo_CloseForm));
			instance.AddUIEventListener(enUIEventID.Player_Info_Tab_Change, new CUIEventManager.OnUIEventHandler(this.OnPlayerInfoTabChange));
			instance.AddUIEventListener(enUIEventID.Player_Info_Open_Pvp_Info, new CUIEventManager.OnUIEventHandler(this.OnPlayerInfoOpenPvpInfo));
			instance.AddUIEventListener(enUIEventID.Player_Info_Open_Base_Info, new CUIEventManager.OnUIEventHandler(this.OnPlayerInfoOpenBaseInfo));
			instance.AddUIEventListener(enUIEventID.Player_Info_Quit_Game, new CUIEventManager.OnUIEventHandler(this.OnPlayerInfoQuitGame));
			instance.AddUIEventListener(enUIEventID.Player_Info_Quit_Game_Confirm, new CUIEventManager.OnUIEventHandler(this.OnPlayerInfoQuitGameConfirm));
			instance.AddUIEventListener(enUIEventID.Player_Info_Most_Used_Hero_Item_Enable, new CUIEventManager.OnUIEventHandler(this.OnPlayerInfoMostUsedHeroItemEnable));
			instance.AddUIEventListener(enUIEventID.Player_Info_Most_Used_Hero_Item_Click, new CUIEventManager.OnUIEventHandler(this.OnPlayerInfoMostUsedHeroItemClick));
			instance.AddUIEventListener(enUIEventID.Player_Info_Show_Rule, new CUIEventManager.OnUIEventHandler(this.OnPlayerInfoShowRule));
			instance.AddUIEventListener(enUIEventID.Player_Info_License_ListItem_Enable, new CUIEventManager.OnUIEventHandler(this.OnLicenseListItemEnable));
			instance.AddUIEventListener(enUIEventID.Player_Info_Update_Sub_Module, new CUIEventManager.OnUIEventHandler(this.OnUpdateSubModule));
			instance.AddUIEventListener(enUIEventID.WEB_IntegralHall, new CUIEventManager.OnUIEventHandler(this.OpenIntegralHall));
			instance.AddUIEventListener(enUIEventID.OPEN_QQ_Buluo, new CUIEventManager.OnUIEventHandler(this.OpenQQBuluo));
			instance.AddUIEventListener(enUIEventID.Player_Info_Achievement_Trophy_Click, new CUIEventManager.OnUIEventHandler(this.OnAchievementTrophyClick));
			this.m_IsFormOpen = false;
			this.m_CurTab = CPlayerInfoSystem.Tab.Base_Info;
			this.m_Form = null;
			instance.AddUIEventListener(enUIEventID.BuyPick_QQ_VIP, new CUIEventManager.OnUIEventHandler(this.OpenByQQVIP));
			instance.AddUIEventListener(enUIEventID.DeepLink_OnClick, new CUIEventManager.OnUIEventHandler(this.DeepLinkClick));
			Singleton<EventRouter>.GetInstance().AddEventHandler(EventID.NOBE_STATE_CHANGE, new Action(this.UpdateNobeHeadIdx));
			Singleton<EventRouter>.GetInstance().AddEventHandler(EventID.HEAD_IMAGE_FLAG_CHANGE, new Action(this.UpdateHeadFlag));
			Singleton<EventRouter>.GetInstance().AddEventHandler(EventID.NAMECHANGE_PLAYER_NAME_CHANGE, new Action(this.OnPlayerNameChange));
			Singleton<EventRouter>.GetInstance().AddEventHandler<byte, CAchieveItem2>(EventID.ACHIEVE_SERY_SELECT_DONE, new Action<byte, CAchieveItem2>(this.OnTrophySelectDone));
			Singleton<EventRouter>.GetInstance().AddEventHandler(EventID.GAMER_REDDOT_CHANGE, new Action(this.UpdateXinYueBtn));
			Singleton<EventRouter>.GetInstance().AddEventHandler(EventID.ACHIEVE_TROPHY_REWARD_INFO_STATE_CHANGE, new Action(this.OnTrophyStateChange));
		}

		public override void UnInit()
		{
			base.UnInit();
			CUIEventManager instance = Singleton<CUIEventManager>.GetInstance();
			instance.RemoveUIEventListener(enUIEventID.Player_Info_OpenForm, new CUIEventManager.OnUIEventHandler(this.OnPlayerInfo_OpenForm));
			instance.RemoveUIEventListener(enUIEventID.Player_Info_CloseForm, new CUIEventManager.OnUIEventHandler(this.OnPlayerInfo_CloseForm));
			instance.RemoveUIEventListener(enUIEventID.Player_Info_Open_Pvp_Info, new CUIEventManager.OnUIEventHandler(this.OnPlayerInfoOpenPvpInfo));
			instance.RemoveUIEventListener(enUIEventID.Player_Info_Open_Base_Info, new CUIEventManager.OnUIEventHandler(this.OnPlayerInfoOpenBaseInfo));
			instance.RemoveUIEventListener(enUIEventID.BuyPick_QQ_VIP, new CUIEventManager.OnUIEventHandler(this.OpenByQQVIP));
			instance.RemoveUIEventListener(enUIEventID.Player_Info_Quit_Game, new CUIEventManager.OnUIEventHandler(this.OnPlayerInfoQuitGame));
			instance.RemoveUIEventListener(enUIEventID.Player_Info_Quit_Game_Confirm, new CUIEventManager.OnUIEventHandler(this.OnPlayerInfoQuitGameConfirm));
			instance.RemoveUIEventListener(enUIEventID.Player_Info_Most_Used_Hero_Item_Enable, new CUIEventManager.OnUIEventHandler(this.OnPlayerInfoMostUsedHeroItemEnable));
			instance.RemoveUIEventListener(enUIEventID.Player_Info_Most_Used_Hero_Item_Click, new CUIEventManager.OnUIEventHandler(this.OnPlayerInfoMostUsedHeroItemClick));
			instance.RemoveUIEventListener(enUIEventID.Player_Info_Show_Rule, new CUIEventManager.OnUIEventHandler(this.OnPlayerInfoShowRule));
			instance.RemoveUIEventListener(enUIEventID.Player_Info_License_ListItem_Enable, new CUIEventManager.OnUIEventHandler(this.OnLicenseListItemEnable));
			instance.RemoveUIEventListener(enUIEventID.Player_Info_Update_Sub_Module, new CUIEventManager.OnUIEventHandler(this.OnUpdateSubModule));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler(EventID.NOBE_STATE_CHANGE, new Action(this.UpdateNobeHeadIdx));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler(EventID.HEAD_IMAGE_FLAG_CHANGE, new Action(this.UpdateHeadFlag));
			instance.RemoveUIEventListener(enUIEventID.DeepLink_OnClick, new CUIEventManager.OnUIEventHandler(this.DeepLinkClick));
			instance.RemoveUIEventListener(enUIEventID.WEB_IntegralHall, new CUIEventManager.OnUIEventHandler(this.OpenIntegralHall));
			instance.RemoveUIEventListener(enUIEventID.OPEN_QQ_Buluo, new CUIEventManager.OnUIEventHandler(this.OpenQQBuluo));
			instance.RemoveUIEventListener(enUIEventID.Player_Info_Achievement_Trophy_Click, new CUIEventManager.OnUIEventHandler(this.OnAchievementTrophyClick));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<byte, CAchieveItem2>(EventID.ACHIEVE_SERY_SELECT_DONE, new Action<byte, CAchieveItem2>(this.OnTrophySelectDone));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler(EventID.GAMER_REDDOT_CHANGE, new Action(this.UpdateXinYueBtn));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler(EventID.ACHIEVE_TROPHY_REWARD_INFO_STATE_CHANGE, new Action(this.OnTrophyStateChange));
		}

		public CPlayerProfile GetProfile()
		{
			return this.m_PlayerProfile;
		}

		public void OpenPvpInfo()
		{
			this.ShowPlayerDetailInfo(0uL, 0, CPlayerInfoSystem.DetailPlayerInfoSource.Self, true);
		}

		public void OpenBaseInfo()
		{
			this.ShowPlayerDetailInfo(0uL, 0, CPlayerInfoSystem.DetailPlayerInfoSource.Self, true);
		}

		public void OpenForm()
		{
			if (this.m_IsFormOpen)
			{
				this.m_Form = Singleton<CUIManager>.GetInstance().GetForm(CPlayerInfoSystem.sPlayerInfoFormPath);
			}
			else
			{
				this.m_Form = Singleton<CUIManager>.GetInstance().OpenForm(CPlayerInfoSystem.sPlayerInfoFormPath, true, true);
			}
			this.m_IsFormOpen = true;
			this.CurTab = CPlayerInfoSystem.Tab.Base_Info;
			this.InitTab();
			Singleton<CPlayerPvpInfoController>.get_instance().InitUI();
			Singleton<CPlayerCommonHeroInfoController>.get_instance().InitCommonHeroUI();
		}

		private void ProcessNobeHeadIDx(CUIFormScript form, bool bshow)
		{
			GameObject obj = Utility.FindChild(form.gameObject, "pnlBg/pnlBody/pnlBaseInfo/pnlContainer/pnlHead/changeNobeheadicon");
			if (!CPlayerInfoSystem.isSelf(this.m_PlayerProfile.m_uuid))
			{
				obj.CustomSetActive(false);
				return;
			}
			if (CSysDynamicBlock.bVipBlock)
			{
				bshow = false;
			}
			if (bshow)
			{
				obj.CustomSetActive(true);
			}
			else
			{
				obj.CustomSetActive(false);
			}
		}

		private void ProcessQQVIP(CUIFormScript form, bool bShow)
		{
			if (form == null)
			{
				return;
			}
			GameObject obj = Utility.FindChild(form.gameObject, "pnlBg/pnlBody/pnlBaseInfo/pnlContainer/BtnGroup/QQVIPBtn");
			GameObject gameObject = Utility.FindChild(form.gameObject, "pnlBg/pnlBody/pnlBaseInfo/pnlContainer/pnlHead/NameGroup/QQVipIcon");
			if (!CPlayerInfoSystem.isSelf(this.m_PlayerProfile.m_uuid))
			{
				obj.CustomSetActive(false);
				MonoSingleton<NobeSys>.GetInstance().SetOtherQQVipHead(gameObject.GetComponent<Image>(), (int)this.m_PlayerProfile.qqVipMask);
				return;
			}
			GameObject gameObject2 = Utility.FindChild(form.gameObject, "pnlBg/pnlBody/pnlBaseInfo/pnlContainer/BtnGroup/QQVIPBtn/Text");
			if (!bShow)
			{
				obj.CustomSetActive(false);
				gameObject.CustomSetActive(false);
				gameObject2.CustomSetActive(false);
				return;
			}
			if (ApolloConfig.platform == 2)
			{
				obj.CustomSetActive(true);
			}
			else
			{
				obj.CustomSetActive(false);
			}
			gameObject2.CustomSetActive(true);
			gameObject.CustomSetActive(false);
			if (CSysDynamicBlock.bLobbyEntryBlocked)
			{
				obj.CustomSetActive(false);
				gameObject.CustomSetActive(false);
				gameObject2.CustomSetActive(false);
				return;
			}
			if (ApolloConfig.platform == 2)
			{
				obj.CustomSetActive(true);
			}
			else
			{
				obj.CustomSetActive(false);
			}
			Text component = gameObject2.GetComponent<Text>();
			component.text = Singleton<CTextManager>.GetInstance().GetText("QQ_Vip_Buy_Vip");
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				MonoSingleton<NobeSys>.GetInstance().SetMyQQVipHead(gameObject.GetComponent<Image>());
			}
		}

		private void OpenByQQVIP(CUIEvent uiEvent)
		{
			if (ApolloConfig.platform == 2 || ApolloConfig.platform == 3)
			{
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				if (masterRoleInfo != null)
				{
					if (masterRoleInfo.HasVip(16))
					{
						Singleton<ApolloHelper>.GetInstance().PayQQVip("CJCLUBT", Singleton<CTextManager>.GetInstance().GetText("QQ_Vip_XuFei_Super_Vip"), 1);
					}
					else if (masterRoleInfo.HasVip(1))
					{
						Singleton<ApolloHelper>.GetInstance().PayQQVip("LTMCLUB", Singleton<CTextManager>.GetInstance().GetText("QQ_Vip_XuFei_Vip"), 1);
					}
					else
					{
						Singleton<ApolloHelper>.GetInstance().PayQQVip("LTMCLUB", Singleton<CTextManager>.GetInstance().GetText("QQ_Vip_Buy_Vip"), 1);
					}
				}
			}
		}

		public void ProcessCommonQQVip(GameObject parent)
		{
			if (!parent)
			{
				return;
			}
			GameObject gameObject = parent.transform.FindChild("QQSVipIcon").gameObject;
			GameObject gameObject2 = parent.transform.FindChild("QQVipIcon").gameObject;
			gameObject2.CustomSetActive(false);
			gameObject.CustomSetActive(false);
		}

		private void OnPlayerInfo_OpenForm(CUIEvent uiEvent)
		{
			this.ShowPlayerDetailInfo(0uL, 0, CPlayerInfoSystem.DetailPlayerInfoSource.Self, true);
			CMiShuSystem.SendUIClickToServer(enUIClickReprotID.rp_HeroHeadBtn);
		}

		private void OnPlayerInfo_CloseForm(CUIEvent uiEvent)
		{
			if (!this.m_IsFormOpen)
			{
				return;
			}
			this.m_IsFormOpen = false;
			Singleton<CUIManager>.GetInstance().CloseForm(CPlayerInfoSystem.sPlayerInfoFormPath);
			this.m_Form = null;
			Singleton<EventRouter>.GetInstance().BroadCastEvent(EventID.PlayerInfoSystem_Form_Close);
		}

		private void OnPlayerInfoOpenPvpInfo(CUIEvent uiEvent)
		{
			this.OpenPvpInfo();
		}

		private void OnPlayerInfoOpenBaseInfo(CUIEvent uiEvent)
		{
			this.OpenBaseInfo();
		}

		private void OnPlayerInfoQuitGame(CUIEvent uiEvent)
		{
			Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(Singleton<CTextManager>.GetInstance().GetText("Common_QuitGameTips"), enUIEventID.Player_Info_Quit_Game_Confirm, enUIEventID.None, false);
		}

		private void OnPlayerInfoQuitGameConfirm(CUIEvent uiEvent)
		{
			SGameApplication.Quit();
		}

		private void OnPlayerInfoMostUsedHeroItemEnable(CUIEvent uiEvent)
		{
			int srcWidgetIndexInBelongedList = uiEvent.m_srcWidgetIndexInBelongedList;
			GameObject srcWidget = uiEvent.m_srcWidget;
			if (srcWidget == null)
			{
				return;
			}
			GameObject gameObject = srcWidget.transform.Find("heroItem").gameObject;
			ListView<COMDT_MOST_USED_HERO_INFO> listView = this.m_PlayerProfile.MostUsedHeroList();
			if (srcWidgetIndexInBelongedList >= listView.get_Count())
			{
				return;
			}
			COMDT_MOST_USED_HERO_INFO cOMDT_MOST_USED_HERO_INFO = listView.get_Item(srcWidgetIndexInBelongedList);
			this.SetHeroItemData(uiEvent.m_srcFormScript, gameObject, cOMDT_MOST_USED_HERO_INFO);
			Text componetInChild = Utility.GetComponetInChild<Text>(srcWidget, "usedCnt");
			if (componetInChild != null)
			{
				componetInChild.text = string.Format(Singleton<CTextManager>.GetInstance().GetText("Player_Info_Games_Cnt_Label"), cOMDT_MOST_USED_HERO_INFO.dwGameWinNum + cOMDT_MOST_USED_HERO_INFO.dwGameLoseNum);
			}
		}

		private void OnPlayerInfoMostUsedHeroItemClick(CUIEvent uiEvent)
		{
			Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Player_Info_CloseForm, uiEvent.m_eventParams);
			Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.HeroInfo_OpenForm, uiEvent.m_eventParams);
		}

		private void OnPlayerInfoShowRule(CUIEvent uiEvent)
		{
			ushort key;
			if (this.m_CurTab == CPlayerInfoSystem.Tab.PvpCreditScore_Info)
			{
				key = 11;
			}
			else
			{
				key = 3;
			}
			ResRuleText dataByKey = GameDataMgr.s_ruleTextDatabin.GetDataByKey((uint)key);
			if (dataByKey != null)
			{
				string title = StringHelper.UTF8BytesToString(ref dataByKey.szTitle);
				string info = StringHelper.UTF8BytesToString(ref dataByKey.szContent);
				Singleton<CUIManager>.GetInstance().OpenInfoForm(title, info);
			}
		}

		public void SetHeroItemData(CUIFormScript formScript, GameObject listItem, COMDT_MOST_USED_HERO_INFO heroInfo)
		{
			if (listItem == null || heroInfo == null)
			{
				return;
			}
			IHeroData heroData = CHeroDataFactory.CreateHeroData(heroInfo.dwHeroID);
			Transform transform = listItem.transform;
			ResHeroProficiency heroProficiency = CHeroInfo.GetHeroProficiency(heroData.heroType, (int)heroInfo.dwProficiencyLv);
			if (heroProficiency != null)
			{
				listItem.GetComponent<Image>().SetSprite(string.Format("{0}{1}", "UGUI/Sprite/Dynamic/Quality/", StringHelper.UTF8BytesToString(ref heroProficiency.szImagePath)), formScript, true, false, false, false);
			}
			string heroSkinPic = CSkinInfo.GetHeroSkinPic(heroInfo.dwHeroID, 0u);
			CUICommonSystem.SetHeroItemImage(formScript, listItem, heroSkinPic, enHeroHeadType.enIcon, false, false);
			GameObject gameObject = transform.Find("profession").gameObject;
			CUICommonSystem.SetHeroJob(formScript, gameObject, (enHeroJobType)heroData.heroType);
			Text component = transform.Find("heroNameText").GetComponent<Text>();
			component.text = heroData.heroName;
			CUIEventScript component2 = listItem.GetComponent<CUIEventScript>();
			stUIEventParams eventParams = default(stUIEventParams);
			eventParams.openHeroFormPar.heroId = heroData.cfgID;
			eventParams.openHeroFormPar.openSrc = enHeroFormOpenSrc.HeroListClick;
			component2.SetUIEvent(enUIEventType.Click, enUIEventID.Player_Info_Most_Used_Hero_Item_Click, eventParams);
		}

		private void OnPlayerInfoTabChange(CUIEvent uiEvent)
		{
			if (this.m_Form == null)
			{
				return;
			}
			if (!this.m_IsFormOpen)
			{
				return;
			}
			CUIListScript component = uiEvent.m_srcWidget.GetComponent<CUIListScript>();
			if (component == null)
			{
				return;
			}
			int selectedIndex = component.GetSelectedIndex();
			this.CurTab = (CPlayerInfoSystem.Tab)selectedIndex;
			GameObject widget = this.m_Form.GetWidget(1);
			GameObject widget2 = this.m_Form.GetWidget(5);
			GameObject widget3 = this.m_Form.GetWidget(7);
			GameObject widget4 = this.m_Form.GetWidget(8);
			GameObject widget5 = this.m_Form.GetWidget(10);
			GameObject widget6 = this.m_Form.GetWidget(9);
			GameObject widget7 = this.m_Form.GetWidget(12);
			GameObject widget8 = this.m_Form.GetWidget(2);
			GameObject widget9 = this.m_Form.GetWidget(21);
			if (widget7 != null)
			{
				this.SetTitle(this.m_CurTab, widget7.transform);
			}
			Transform transform = this.m_Form.transform.Find("pnlBg/pnlBody/pnlHonorInfo");
			GameObject obj = null;
			if (transform != null)
			{
				obj = transform.gameObject;
			}
			Transform transform2 = this.m_Form.transform.Find("pnlBg/pnlBody/pnlPvPHistory");
			GameObject obj2 = null;
			if (transform2 != null)
			{
				obj2 = transform2.gameObject;
			}
			Transform transform3 = this.m_Form.transform.Find("pnlBg/pnlBody/pnlCreditScoreInfo");
			GameObject obj3 = null;
			if (transform3 != null)
			{
				obj3 = transform3.gameObject;
			}
			switch (this.m_CurTab)
			{
			case CPlayerInfoSystem.Tab.Base_Info:
				widget6.CustomSetActive(true);
				widget5.CustomSetActive(false);
				widget.CustomSetActive(true);
				widget2.CustomSetActive(false);
				obj3.CustomSetActive(false);
				widget3.CustomSetActive(false);
				obj.CustomSetActive(false);
				obj2.CustomSetActive(false);
				widget8.CustomSetActive(false);
				widget9.CustomSetActive(false);
				this.UpdateBaseInfo();
				this.ProcessQQVIP(this.m_Form, true);
				this.ProcessNobeHeadIDx(this.m_Form, true);
				break;
			case CPlayerInfoSystem.Tab.Pvp_Info:
				widget.CustomSetActive(false);
				widget2.CustomSetActive(false);
				widget4.CustomSetActive(true);
				obj3.CustomSetActive(false);
				widget3.CustomSetActive(false);
				obj.CustomSetActive(false);
				obj2.CustomSetActive(false);
				widget8.CustomSetActive(true);
				widget9.CustomSetActive(false);
				this.UpdatePvpInfo2();
				break;
			case CPlayerInfoSystem.Tab.Honor_Info:
				widget.CustomSetActive(false);
				widget2.CustomSetActive(false);
				widget4.CustomSetActive(true);
				obj3.CustomSetActive(false);
				widget3.CustomSetActive(false);
				obj.CustomSetActive(false);
				obj2.CustomSetActive(false);
				widget8.CustomSetActive(false);
				widget9.CustomSetActive(false);
				this.LoadSubModule();
				MonoSingleton<NewbieGuideManager>.GetInstance().CheckTriggerTime(NewbieGuideTriggerTimeType.onClickGloryPoints, new uint[0]);
				Singleton<CUINewFlagSystem>.GetInstance().DelNewFlag(component.GetElemenet(2).gameObject, enNewFlagKey.New_HonorInfo_V1, true);
				break;
			case CPlayerInfoSystem.Tab.Common_Hero:
				widget6.CustomSetActive(true);
				widget5.CustomSetActive(false);
				widget.CustomSetActive(false);
				widget2.CustomSetActive(false);
				obj3.CustomSetActive(false);
				obj.CustomSetActive(false);
				widget4.CustomSetActive(true);
				widget3.CustomSetActive(true);
				obj2.CustomSetActive(false);
				widget8.CustomSetActive(false);
				widget9.CustomSetActive(false);
				Singleton<CPlayerCommonHeroInfoController>.get_instance().UpdateUI();
				break;
			case CPlayerInfoSystem.Tab.PvpHistory_Info:
				widget.CustomSetActive(false);
				widget2.CustomSetActive(false);
				widget4.CustomSetActive(true);
				obj3.CustomSetActive(false);
				widget3.CustomSetActive(false);
				obj.CustomSetActive(false);
				obj2.CustomSetActive(false);
				widget8.CustomSetActive(false);
				widget9.CustomSetActive(false);
				this.LoadSubModule();
				Singleton<CUINewFlagSystem>.GetInstance().DelNewFlag(component.GetElemenet(4).gameObject, enNewFlagKey.New_PvpHistoryInfo_V1, true);
				break;
			case CPlayerInfoSystem.Tab.PvpCreditScore_Info:
				widget6.CustomSetActive(true);
				widget5.CustomSetActive(false);
				widget.CustomSetActive(false);
				widget2.CustomSetActive(false);
				widget4.CustomSetActive(true);
				obj3.CustomSetActive(true);
				widget3.CustomSetActive(false);
				obj.CustomSetActive(false);
				obj2.CustomSetActive(false);
				widget8.CustomSetActive(false);
				widget9.CustomSetActive(false);
				this.LoadSubModule();
				break;
			case CPlayerInfoSystem.Tab.Mentor_Info:
				widget.CustomSetActive(false);
				widget2.CustomSetActive(false);
				widget4.CustomSetActive(false);
				obj3.CustomSetActive(false);
				widget3.CustomSetActive(false);
				obj.CustomSetActive(false);
				obj2.CustomSetActive(false);
				widget8.CustomSetActive(false);
				widget9.CustomSetActive(true);
				this.LoadSubModule();
				Singleton<CUINewFlagSystem>.GetInstance().DelNewFlag(component.GetElemenet(6).gameObject, enNewFlagKey.New_MentorInfo_V1, true);
				break;
			}
			Singleton<EventRouter>.GetInstance().BroadCastEvent(EventID.PlayerInfoSystem_Tab_Change);
		}

		public void LoadSubModule()
		{
			DebugHelper.Assert(this.m_Form != null, "Player Form Is Null");
			if (this.m_Form == null)
			{
				return;
			}
			bool flag = false;
			GameObject widget = this.m_Form.GetWidget(10);
			GameObject widget2 = this.m_Form.GetWidget(9);
			if (widget != null && widget2 != null)
			{
				switch (this.m_CurTab)
				{
				case CPlayerInfoSystem.Tab.Honor_Info:
					flag = Singleton<CPlayerHonorController>.GetInstance().Loaded(this.m_Form);
					if (!flag)
					{
						widget.CustomSetActive(true);
						Singleton<CPlayerHonorController>.GetInstance().Load(this.m_Form);
						widget2.CustomSetActive(false);
					}
					break;
				case CPlayerInfoSystem.Tab.PvpHistory_Info:
					flag = Singleton<CPlayerPvpHistoryController>.GetInstance().Loaded(this.m_Form);
					if (!flag)
					{
						widget.CustomSetActive(true);
						Singleton<CPlayerPvpHistoryController>.GetInstance().Load(this.m_Form);
						widget2.CustomSetActive(false);
					}
					break;
				case CPlayerInfoSystem.Tab.PvpCreditScore_Info:
					flag = Singleton<CPlayerCreaditScoreController>.GetInstance().Loaded(this.m_Form);
					if (!flag)
					{
						widget.CustomSetActive(true);
						Singleton<CPlayerCreaditScoreController>.GetInstance().Load(this.m_Form);
						widget2.CustomSetActive(false);
					}
					break;
				case CPlayerInfoSystem.Tab.Mentor_Info:
					Singleton<CPlayerMentorInfoController>.get_instance().UpdateUI();
					break;
				}
			}
			if (!flag)
			{
				GameObject widget3 = this.m_Form.GetWidget(11);
				if (widget3 != null)
				{
					CUITimerScript component = widget3.GetComponent<CUITimerScript>();
					if (component != null)
					{
						component.ReStartTimer();
					}
				}
			}
			else
			{
				Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Player_Info_Update_Sub_Module);
			}
		}

		private void OnUpdateSubModule(CUIEvent uiEvent)
		{
			DebugHelper.Assert(this.m_Form != null, "Player Form Is Null");
			if (this.m_Form == null)
			{
				return;
			}
			GameObject widget = this.m_Form.GetWidget(10);
			GameObject widget2 = this.m_Form.GetWidget(9);
			widget2.CustomSetActive(true);
			widget.CustomSetActive(false);
			switch (this.m_CurTab)
			{
			case CPlayerInfoSystem.Tab.Honor_Info:
				Singleton<CPlayerHonorController>.GetInstance().Draw(this.m_Form);
				break;
			case CPlayerInfoSystem.Tab.PvpHistory_Info:
				Singleton<CPlayerPvpHistoryController>.GetInstance().Draw(this.m_Form);
				break;
			case CPlayerInfoSystem.Tab.PvpCreditScore_Info:
				Singleton<CPlayerCreaditScoreController>.GetInstance().Draw(this.m_Form);
				break;
			}
		}

		private void RefreshLicenseInfoPanel(CUIFormScript form)
		{
			if (null == form)
			{
				return;
			}
			GameObject widget = form.GetWidget(6);
			if (null == widget)
			{
				return;
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null && masterRoleInfo.m_licenseInfo != null)
			{
				CUIListScript component = widget.GetComponent<CUIListScript>();
				if (component != null)
				{
					component.SetElementAmount(masterRoleInfo.m_licenseInfo.m_licenseList.get_Count());
				}
			}
		}

		private void OnLicenseListItemEnable(CUIEvent uiEvent)
		{
			int srcWidgetIndexInBelongedList = uiEvent.m_srcWidgetIndexInBelongedList;
			GameObject srcWidget = uiEvent.m_srcWidget;
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null && masterRoleInfo.m_licenseInfo != null)
			{
				CLicenseItem licenseItemByIndex = masterRoleInfo.m_licenseInfo.GetLicenseItemByIndex(srcWidgetIndexInBelongedList);
				if (srcWidget != null && licenseItemByIndex != null && licenseItemByIndex.m_resLicenseInfo != null)
				{
					Transform transform = srcWidget.transform.Find("licenseIcon");
					transform.GetComponent<Image>().SetSprite(string.Format("{0}{1}", CUIUtility.s_Sprite_Dynamic_Task_Dir, licenseItemByIndex.m_resLicenseInfo.szIconPath), this.m_Form, true, false, false, false);
					Transform transform2 = srcWidget.transform.Find("licenseNameText");
					transform2.GetComponent<Text>().text = licenseItemByIndex.m_resLicenseInfo.szLicenseName;
					Transform transform3 = srcWidget.transform.Find("licenseStateText");
					if (licenseItemByIndex.m_getSecond > 0u)
					{
						DateTime dateTime = Utility.ToUtcTime2Local((long)((ulong)licenseItemByIndex.m_getSecond));
						transform3.GetComponent<Text>().text = string.Format("<color=#00d519>{0}/{1}/{2}</color>", dateTime.get_Year(), dateTime.get_Month(), dateTime.get_Day());
					}
					else
					{
						transform3.GetComponent<Text>().text = "<color=#fecb2f>未获得</color>";
					}
					Transform transform4 = srcWidget.transform.Find("licenseDescText");
					transform4.GetComponent<Text>().text = licenseItemByIndex.m_resLicenseInfo.szDesc;
				}
			}
		}

		public static bool isSelf(ulong playerUllUID)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			return masterRoleInfo != null && masterRoleInfo.playerUllUID == playerUllUID;
		}

		private void InitTab()
		{
			if (this.m_Form == null || !this.m_IsFormOpen)
			{
				return;
			}
			GameObject widget = this.m_Form.GetWidget(1);
			if (widget != null && widget.activeSelf)
			{
				widget.CustomSetActive(false);
			}
			CPlayerInfoSystem.Tab[] array = (CPlayerInfoSystem.Tab[])Enum.GetValues(typeof(CPlayerInfoSystem.Tab));
			string[] array2 = new string[array.Length];
			byte b = 0;
			while ((int)b < array.Length)
			{
				switch (array[(int)b])
				{
				case CPlayerInfoSystem.Tab.Base_Info:
					array2[(int)b] = Singleton<CTextManager>.GetInstance().GetText("Player_Info_Tab_Base_Info");
					break;
				case CPlayerInfoSystem.Tab.Pvp_Info:
					array2[(int)b] = Singleton<CTextManager>.GetInstance().GetText("Player_Info_Tab_Pvp_Info");
					break;
				case CPlayerInfoSystem.Tab.Honor_Info:
					array2[(int)b] = Singleton<CTextManager>.GetInstance().GetText("Player_Info_Tab_Honor_Info");
					break;
				case CPlayerInfoSystem.Tab.Common_Hero:
					array2[(int)b] = Singleton<CTextManager>.GetInstance().GetText("Player_Info_Tab_Common_Hero_Info");
					break;
				case CPlayerInfoSystem.Tab.PvpHistory_Info:
					array2[(int)b] = Singleton<CTextManager>.GetInstance().GetText("Player_Info_Tab_PvpHistory_Info");
					break;
				case CPlayerInfoSystem.Tab.PvpCreditScore_Info:
					array2[(int)b] = Singleton<CTextManager>.GetInstance().GetText("Player_Info_Tab_Credit_Info");
					break;
				case CPlayerInfoSystem.Tab.Mentor_Info:
					array2[(int)b] = Singleton<CTextManager>.GetInstance().GetText("Player_Info_Tab_Mentor_Info");
					break;
				}
				b += 1;
			}
			GameObject widget2 = this.m_Form.GetWidget(0);
			CUIListScript component = widget2.GetComponent<CUIListScript>();
			if (component != null)
			{
				component.SetElementAmount(array2.Length);
				for (int i = 0; i < component.m_elementAmount; i++)
				{
					CUIListElementScript elemenet = component.GetElemenet(i);
					Text component2 = elemenet.gameObject.transform.Find("Text").GetComponent<Text>();
					component2.text = array2[i];
				}
				component.m_alwaysDispatchSelectedChangeEvent = true;
				component.SelectElement((int)this.CurTab, true);
			}
			Singleton<CUINewFlagSystem>.GetInstance().AddNewFlag(component.GetElemenet(2).gameObject, enNewFlagKey.New_HonorInfo_V1, enNewFlagPos.enTopRight, 1f, 0f, 0f, enNewFlagType.enNewFlag);
			Singleton<CUINewFlagSystem>.GetInstance().AddNewFlag(component.GetElemenet(4).gameObject, enNewFlagKey.New_PvpHistoryInfo_V1, enNewFlagPos.enTopRight, 1f, 0f, 0f, enNewFlagType.enNewFlag);
			Singleton<CUINewFlagSystem>.GetInstance().AddNewFlag(component.GetElemenet(6).gameObject, enNewFlagKey.New_MentorInfo_V1, enNewFlagPos.enTopRight, 1f, 0f, 0f, enNewFlagType.enNewFlag);
		}

		private void SetBaseInfoScrollable(bool scrollable = false)
		{
			if (!this.m_IsFormOpen || this.m_Form == null)
			{
				return;
			}
			GameObject widget = this.m_Form.GetWidget(1);
			if (widget == null || !widget.activeSelf)
			{
				return;
			}
			GameObject gameObject = Utility.FindChild(widget, "pnlContainer/pnlInfo/scrollRect");
			if (gameObject != null)
			{
				RectTransform component = gameObject.GetComponent<RectTransform>();
				ScrollRect component2 = gameObject.GetComponent<ScrollRect>();
				if (component != null)
				{
					if (scrollable)
					{
						component.offsetMin = new Vector2(component.offsetMin.x, 90f);
					}
					else
					{
						component.offsetMin = new Vector2(component.offsetMin.x, 0f);
					}
				}
				if (component2 != null)
				{
					component2.verticalNormalizedPosition = 1f;
				}
			}
		}

		private void DisplayCustomButton()
		{
			if (!this.m_IsFormOpen || this.m_Form == null)
			{
				return;
			}
			GameObject widget = this.m_Form.GetWidget(1);
			if (widget == null || !widget.activeSelf)
			{
				return;
			}
			GameObject obj = Utility.FindChild(widget, "pnlContainer/pnlHead/btnRename");
			GameObject obj2 = Utility.FindChild(widget, "pnlContainer/pnlHead/btnShare");
			switch (this._lastDetailSource)
			{
			case CPlayerInfoSystem.DetailPlayerInfoSource.DefaultOthers:
				obj.CustomSetActive(false);
				obj2.CustomSetActive(false);
				this.SetBaseInfoScrollable(false);
				this.SetAllGuildBtnActive(widget, false);
				this.SetAllFriendBtnActive(widget, false);
				break;
			case CPlayerInfoSystem.DetailPlayerInfoSource.Self:
				obj.CustomSetActive(false);
				obj2.CustomSetActive(false);
				this.SetBaseInfoScrollable(false);
				this.SetAllGuildBtnActive(widget, false);
				this.SetAppointMatchLeaderBtn();
				this.SetAllFriendBtnActive(widget, false);
				break;
			case CPlayerInfoSystem.DetailPlayerInfoSource.Guild:
				obj.CustomSetActive(false);
				obj2.CustomSetActive(false);
				this.SetBaseInfoScrollable(false);
				this.SetSingleGuildBtn(widget);
				this.SetAllFriendBtnActive(widget, false);
				break;
			}
		}

		private void SetAllFriendBtnActive(GameObject root, bool isActive)
		{
			GameObject obj = Utility.FindChild(root, "pnlContainer/BtnGroup/btnSettings");
			GameObject obj2 = Utility.FindChild(root, "pnlContainer/BtnGroup/btnQuit");
			obj.CustomSetActive(isActive);
			obj2.CustomSetActive(isActive);
		}

		private void SetAllGuildBtnActive(GameObject root, bool isActive)
		{
			GameObject obj = Utility.FindChild(root, "pnlContainer/BtnGroup/btnAddFriend");
			GameObject obj2 = Utility.FindChild(root, "pnlContainer/BtnGroup/btnAppointViceChairman");
			GameObject obj3 = Utility.FindChild(root, "pnlContainer/BtnGroup/btnAppointOrCancelMatchLeader");
			GameObject obj4 = Utility.FindChild(root, "pnlContainer/BtnGroup/btnTransferPosition");
			GameObject obj5 = Utility.FindChild(root, "pnlContainer/btnFireMember");
			obj.CustomSetActive(isActive);
			obj2.CustomSetActive(isActive);
			obj3.CustomSetActive(isActive);
			obj4.CustomSetActive(isActive);
			obj5.CustomSetActive(isActive);
		}

		private void SetSingleGuildBtn(GameObject root)
		{
			GameObject obj = Utility.FindChild(root, "pnlContainer/BtnGroup/btnAddFriend");
			GameObject obj2 = Utility.FindChild(root, "pnlContainer/BtnGroup/btnAppointViceChairman");
			GameObject obj3 = Utility.FindChild(root, "pnlContainer/BtnGroup/btnTransferPosition");
			GameObject obj4 = Utility.FindChild(root, "pnlContainer/btnFireMember");
			obj.CustomSetActive(true);
			obj2.CustomSetActive(this._isShowGuildAppointViceChairmanBtn);
			obj3.CustomSetActive(this._isShowGuildTransferPositionBtn);
			obj4.CustomSetActive(this._isShowGuildFireMemberBtn);
			this.SetAppointMatchLeaderBtn();
		}

		public void SetAppointMatchLeaderBtn()
		{
			if (this.m_Form == null)
			{
				return;
			}
			GameObject widget = this.m_Form.GetWidget(14);
			if (CGuildSystem.HasAppointMatchLeaderAuthority())
			{
				widget.CustomSetActive(true);
				this.SetAppointMatchLeaderBtnText(widget);
			}
			else
			{
				widget.CustomSetActive(false);
			}
		}

		private void SetAppointMatchLeaderBtnText(GameObject btnAppointOrCancelMatchLeader)
		{
			CUICommonSystem.SetButtonName(btnAppointOrCancelMatchLeader, (!CGuildHelper.IsGuildMatchLeaderPosition(this.m_PlayerProfile.m_uuid)) ? Singleton<CTextManager>.GetInstance().GetText("GuildMatch_Apooint_Leader") : Singleton<CTextManager>.GetInstance().GetText("GuildMatch_Cancel_Leader"));
		}

		private void UpdateNobeHeadIdx()
		{
			if (!this.m_IsFormOpen || this.m_Form == null)
			{
				return;
			}
			GameObject widget = this.m_Form.GetWidget(1);
			if (widget == null)
			{
				return;
			}
			GameObject gameObject = Utility.FindChild(widget, "pnlContainer/pnlHead/pnlImg/HttpImage/NobeImag");
			if (gameObject)
			{
				MonoSingleton<NobeSys>.GetInstance().SetHeadIconBk(gameObject.GetComponent<Image>(), (int)MonoSingleton<NobeSys>.GetInstance().m_vipInfo.stGameVipClient.dwHeadIconId);
				MonoSingleton<NobeSys>.GetInstance().SetHeadIconBkEffect(gameObject.GetComponent<Image>(), (int)MonoSingleton<NobeSys>.GetInstance().m_vipInfo.stGameVipClient.dwHeadIconId, this.m_Form, 0.9f);
			}
		}

		private void OnPlayerNameChange()
		{
			if (!this.m_IsFormOpen || this.m_Form == null)
			{
				return;
			}
			GameObject widget = this.m_Form.GetWidget(1);
			if (widget == null)
			{
				return;
			}
			Text componetInChild = Utility.GetComponetInChild<Text>(widget, "pnlContainer/pnlHead/NameGroup/txtName");
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (componetInChild != null && masterRoleInfo != null)
			{
				componetInChild.text = masterRoleInfo.Name;
			}
		}

		private void OnPersonSignEndEdit(string personSign)
		{
			if (string.Compare(personSign, this.m_PlayerProfile.m_personSign) != 0)
			{
				this.m_PlayerProfile.m_personSign = personSign;
				CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1193u);
				StringHelper.StringToUTF8Bytes(personSign, ref cSPkg.stPkgData.get_stSignatureReq().szSignatureInfo);
				Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
			}
		}

		private void UpdateHeadFlag()
		{
			if (!this.m_IsFormOpen || this.m_Form == null)
			{
				return;
			}
			GameObject widget = this.m_Form.GetWidget(1);
			if (widget == null)
			{
				return;
			}
			GameObject gameObject = Utility.FindChild(this.m_Form.gameObject, "pnlBg/pnlBody/pnlBaseInfo/pnlContainer/pnlHead/changeNobeheadicon");
			if (gameObject != null)
			{
				bool flag = Singleton<HeadIconSys>.get_instance().UnReadFlagNum > 0u;
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

		private void OnTrophySelectDone(byte idx, CAchieveItem2 item)
		{
			CAchieveInfo2 masterAchieveInfo = CAchieveInfo2.GetMasterAchieveInfo();
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if ((int)idx < masterAchieveInfo.SelectedTrophies.Length)
			{
				masterAchieveInfo.SelectedTrophies[(int)idx] = item;
				CPlayerProfile profile = Singleton<CPlayerInfoSystem>.GetInstance().GetProfile();
				profile.ConvertRoleInfoData(masterRoleInfo);
				this.UpdateBaseInfo();
			}
		}

		private void SetTitle(CPlayerInfoSystem.Tab tab, Transform titleTransform)
		{
			if (titleTransform == null)
			{
				return;
			}
			Text component = titleTransform.GetComponent<Text>();
			if (component == null)
			{
				return;
			}
			switch (tab)
			{
			case CPlayerInfoSystem.Tab.Base_Info:
				component.text = Singleton<CTextManager>.GetInstance().GetText("Player_Info_Tab_Base_Info");
				break;
			case CPlayerInfoSystem.Tab.Pvp_Info:
				component.text = Singleton<CTextManager>.GetInstance().GetText("Player_Info_Tab_Pvp_Info");
				break;
			case CPlayerInfoSystem.Tab.Honor_Info:
				component.text = Singleton<CTextManager>.GetInstance().GetText("Player_Info_Tab_Honor_Info");
				break;
			case CPlayerInfoSystem.Tab.Common_Hero:
				component.text = Singleton<CTextManager>.GetInstance().GetText("Player_Info_Tab_Common_Hero_Info");
				break;
			case CPlayerInfoSystem.Tab.PvpHistory_Info:
				component.text = Singleton<CTextManager>.GetInstance().GetText("Player_Info_Tab_PvpHistory_Info");
				break;
			case CPlayerInfoSystem.Tab.PvpCreditScore_Info:
				component.text = Singleton<CTextManager>.GetInstance().GetText("Player_Info_Tab_Credit_Info");
				break;
			default:
				component.text = Singleton<CTextManager>.GetInstance().GetText("Player_Info_Title");
				break;
			}
		}

		private void UpdateXinYueBtn()
		{
			if (!this.m_IsFormOpen || this.m_Form == null)
			{
				return;
			}
			GameObject widget = this.m_Form.GetWidget(1);
			if (widget == null)
			{
				return;
			}
			Transform transform = widget.transform.Find("pnlContainer/BtnGroup/XYJLBBtn");
			if (transform)
			{
				transform.gameObject.CustomSetActive(!CSysDynamicBlock.bLobbyEntryBlocked);
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				if (masterRoleInfo != null)
				{
					if (masterRoleInfo.ShowGameRedDot)
					{
						CUICommonSystem.AddRedDot(transform.gameObject, enRedDotPos.enTopRight, 0);
					}
					else
					{
						CUICommonSystem.DelRedDot(transform.gameObject);
					}
				}
			}
		}

		private void OnTrophyStateChange()
		{
			if (!this.m_IsFormOpen || this.m_Form == null)
			{
				return;
			}
			if (this.CurTab != CPlayerInfoSystem.Tab.Base_Info)
			{
				return;
			}
			this.ProcessSelectedTrophies();
		}

		private void UpdateBaseInfo()
		{
			if (!this.m_IsFormOpen || this.m_Form == null)
			{
				return;
			}
			GameObject widget = this.m_Form.GetWidget(1);
			if (widget == null)
			{
				return;
			}
			this.DisplayCustomButton();
			if (!CSysDynamicBlock.bSocialBlocked)
			{
				GameObject gameObject = Utility.FindChild(widget, "pnlContainer/pnlHead/pnlImg/HttpImage");
				if (gameObject != null && !string.IsNullOrEmpty(this.m_PlayerProfile.HeadUrl()))
				{
					CUIHttpImageScript component = gameObject.GetComponent<CUIHttpImageScript>();
					component.SetImageUrl(this.m_PlayerProfile.HeadUrl());
					MonoSingleton<NobeSys>.GetInstance().SetNobeIcon(component.GetComponent<Image>(), (int)this.m_PlayerProfile.m_vipInfo.stGameVipClient.dwCurLevel, false);
					GameObject gameObject2 = Utility.FindChild(widget, "pnlContainer/pnlHead/pnlImg/HttpImage/NobeImag");
					if (gameObject2)
					{
						MonoSingleton<NobeSys>.GetInstance().SetHeadIconBk(gameObject2.GetComponent<Image>(), (int)this.m_PlayerProfile.m_vipInfo.stGameVipClient.dwHeadIconId);
						MonoSingleton<NobeSys>.GetInstance().SetHeadIconBkEffect(gameObject2.GetComponent<Image>(), (int)this.m_PlayerProfile.m_vipInfo.stGameVipClient.dwHeadIconId, this.m_Form, 0.9f);
					}
				}
			}
			this.UpdateHeadFlag();
			COM_PRIVILEGE_TYPE cOM_PRIVILEGE_TYPE = this.m_PlayerProfile.PrivilegeType();
			MonoSingleton<NobeSys>.GetInstance().SetGameCenterVisible(Utility.FindChild(widget, "pnlContainer/pnlHead/NameGroup/WXGameCenterIcon"), cOM_PRIVILEGE_TYPE, 1, true, false);
			COM_PRIVILEGE_TYPE privilegeType = (cOM_PRIVILEGE_TYPE != 1) ? 1 : 0;
			MonoSingleton<NobeSys>.GetInstance().SetGameCenterVisible(Utility.FindChild(widget, "pnlContainer/WXGameCenter/WXGameCenterBtn"), privilegeType, 1, true, false);
			MonoSingleton<NobeSys>.GetInstance().SetGameCenterVisible(Utility.FindChild(widget, "pnlContainer/pnlHead/NameGroup/QQGameCenterIcon"), cOM_PRIVILEGE_TYPE, 2, true, false);
			MonoSingleton<NobeSys>.GetInstance().SetGameCenterVisible(Utility.FindChild(widget, "pnlContainer/QQGameCenter/QQGameCenterBtn"), cOM_PRIVILEGE_TYPE, 2, true, false);
			COM_PRIVILEGE_TYPE privilegeType2 = (cOM_PRIVILEGE_TYPE != 2) ? 2 : 0;
			MonoSingleton<NobeSys>.GetInstance().SetGameCenterVisible(Utility.FindChild(widget, "pnlContainer/QQGameCenter/QQGameCenterGrey"), privilegeType2, 2, true, false);
			MonoSingleton<NobeSys>.GetInstance().SetGameCenterVisible(Utility.FindChild(widget, "pnlContainer/pnlHead/NameGroup/GuestGameCenterIcon"), cOM_PRIVILEGE_TYPE, 5, true, false);
			MonoSingleton<NobeSys>.GetInstance().SetGameCenterVisible(Utility.FindChild(widget, "pnlContainer/GuestGameCenter/GuestGameCenterBtn"), cOM_PRIVILEGE_TYPE, 5, true, false);
			Text componetInChild = Utility.GetComponetInChild<Text>(widget, "pnlContainer/pnlHead/Level");
			if (componetInChild != null)
			{
				componetInChild.text = string.Format(Singleton<CTextManager>.GetInstance().GetText("ranking_PlayerLevel"), this.m_PlayerProfile.PvpLevel());
			}
			Text componetInChild2 = Utility.GetComponetInChild<Text>(widget, "pnlContainer/pnlInfo/labelGeiLiDuiYouCnt/numCnt");
			if (componetInChild2 != null)
			{
				componetInChild2.text = this.m_PlayerProfile._geiLiDuiYou.ToString();
			}
			Text componetInChild3 = Utility.GetComponetInChild<Text>(widget, "pnlContainer/pnlInfo/labelKeJingDuiShouCnt/numCnt");
			if (componetInChild3 != null)
			{
				componetInChild3.text = this.m_PlayerProfile._keJingDuiShou.ToString();
			}
			Text componetInChild4 = Utility.GetComponetInChild<Text>(widget, "pnlContainer/pnlInfo/labelHeroCnt/numCnt");
			if (componetInChild4 != null)
			{
				componetInChild4.text = this.m_PlayerProfile.HeroCnt().ToString();
			}
			Text componetInChild5 = Utility.GetComponetInChild<Text>(widget, "pnlContainer/pnlInfo/labelSkinCnt/numCnt");
			if (componetInChild5 != null)
			{
				componetInChild5.text = this.m_PlayerProfile.SkinCnt().ToString();
			}
			Image component2 = Utility.FindChild(widget, "pnlContainer/pnlHead/NameGroup/Gender").GetComponent<Image>();
			component2.gameObject.CustomSetActive(this.m_PlayerProfile.Gender() != 0);
			if (this.m_PlayerProfile.Gender() == 1)
			{
				CUIUtility.SetImageSprite(component2, string.Format("{0}icon/Ico_boy.prefab", "UGUI/Sprite/Dynamic/"), null, true, false, false, false);
			}
			else if (this.m_PlayerProfile.Gender() == 2)
			{
				CUIUtility.SetImageSprite(component2, string.Format("{0}icon/Ico_girl.prefab", "UGUI/Sprite/Dynamic/"), null, true, false, false, false);
			}
			Text componetInChild6 = Utility.GetComponetInChild<Text>(widget, "pnlContainer/pnlHead/NameGroup/txtName");
			if (componetInChild6 != null)
			{
				componetInChild6.text = this.m_PlayerProfile.Name();
			}
			GameObject gameObject3 = Utility.FindChild(widget, "pnlContainer/pnlHead/Status/Rank");
			if (this.m_PlayerProfile.GetRankGrade() == 0)
			{
				if (gameObject3)
				{
					gameObject3.CustomSetActive(false);
				}
			}
			else
			{
				gameObject3.CustomSetActive(true);
				Image image = null;
				Image image2 = null;
				if (gameObject3 != null)
				{
					image = Utility.GetComponetInChild<Image>(gameObject3, "ImgRank");
					image2 = Utility.GetComponetInChild<Image>(gameObject3, "ImgRank/ImgSubRank");
				}
				if (image != null)
				{
					string rankSmallIconPath = CLadderView.GetRankSmallIconPath(this.m_PlayerProfile.GetRankGrade(), (uint)this.m_PlayerProfile.GetRankClass());
					image.SetSprite(rankSmallIconPath, this.m_Form, true, false, false, false);
				}
				if (image2 != null)
				{
					string subRankSmallIconPath = CLadderView.GetSubRankSmallIconPath(this.m_PlayerProfile.GetRankGrade(), (uint)this.m_PlayerProfile.GetRankClass());
					image2.SetSprite(subRankSmallIconPath, this.m_Form, true, false, false, false);
				}
			}
			GameObject gameObject4 = Utility.FindChild(widget, "pnlContainer/pnlHead/Status/HisRank");
			if (this.m_PlayerProfile.GetHistoryHighestRankGrade() == 0)
			{
				if (gameObject4)
				{
					gameObject4.CustomSetActive(false);
				}
			}
			else
			{
				gameObject4.CustomSetActive(true);
				Image image3 = null;
				Image image4 = null;
				if (gameObject4 != null)
				{
					image3 = Utility.GetComponetInChild<Image>(gameObject4, "ImgRank");
					image4 = Utility.GetComponetInChild<Image>(gameObject4, "ImgRank/ImgSubRank");
				}
				if (image3 != null)
				{
					string rankSmallIconPath2 = CLadderView.GetRankSmallIconPath(this.m_PlayerProfile.GetHistoryHighestRankGrade(), this.m_PlayerProfile.GetHistoryHighestRankClass());
					image3.SetSprite(rankSmallIconPath2, this.m_Form, true, false, false, false);
				}
				if (image4 != null)
				{
					string subRankSmallIconPath2 = CLadderView.GetSubRankSmallIconPath(this.m_PlayerProfile.GetHistoryHighestRankGrade(), this.m_PlayerProfile.GetHistoryHighestRankClass());
					image4.SetSprite(subRankSmallIconPath2, this.m_Form, true, false, false, false);
				}
			}
			string empty = string.Empty;
			string empty2 = string.Empty;
			GameObject obj = Utility.FindChild(widget, "pnlContainer/pnlHead/Status/ExtraCoin");
			obj.CustomSetActive(false);
			GameObject obj2 = Utility.FindChild(widget, "pnlContainer/pnlHead/Status/ExtraExp");
			obj2.CustomSetActive(false);
			GameObject gameObject5 = Utility.FindChild(widget, "pnlContainer/pnlHead/GuildInfo/Name");
			GameObject gameObject6 = Utility.FindChild(widget, "pnlContainer/pnlHead/GuildInfo/Position");
			if (gameObject5 != null && gameObject6 != null)
			{
				Text component3 = gameObject5.GetComponent<Text>();
				Text componetInChild7 = Utility.GetComponetInChild<Text>(gameObject6, "Text");
				if (!CGuildSystem.IsInNormalGuild(this.m_PlayerProfile.GuildState) || string.IsNullOrEmpty(this.m_PlayerProfile.GuildName))
				{
					if (component3 != null)
					{
						component3.text = Singleton<CTextManager>.GetInstance().GetText("PlayerInfo_Guild");
					}
					gameObject6.CustomSetActive(false);
				}
				else
				{
					if (component3 != null)
					{
						component3.text = this.m_PlayerProfile.GuildName;
					}
					gameObject6.CustomSetActive(true);
					if (componetInChild7 != null)
					{
						componetInChild7.text = CGuildHelper.GetPositionName(this.m_PlayerProfile.GuildState);
					}
				}
			}
			bool flag = CPlayerInfoSystem.isSelf(this.m_PlayerProfile.m_uuid);
			GameObject widget2 = this.m_Form.GetWidget(3);
			widget2.CustomSetActive(flag);
			GameObject widget3 = this.m_Form.GetWidget(13);
			InputField component4 = widget3.GetComponent<InputField>();
			widget3.CustomSetActive(true);
			if (component4)
			{
				if (string.IsNullOrEmpty(this.m_PlayerProfile.m_personSign))
				{
					widget3.CustomSetActive(flag);
					component4.text = string.Empty;
				}
				else
				{
					component4.text = this.m_PlayerProfile.m_personSign;
				}
				if (flag)
				{
					component4.interactable = true;
					component4.onEndEdit.RemoveAllListeners();
					component4.onEndEdit.AddListener(new UnityAction<string>(this.OnPersonSignEndEdit));
				}
				else
				{
					component4.interactable = false;
				}
			}
			GameObject obj3 = Utility.FindChild(widget, "pnlContainer/BtnGroup/JFQBtn");
			if (ApolloConfig.platform == 2 || ApolloConfig.platform == 3)
			{
				if (!CSysDynamicBlock.bJifenHallBlock)
				{
					obj3.CustomSetActive(flag);
				}
				else
				{
					obj3.CustomSetActive(false);
				}
			}
			else
			{
				obj3.CustomSetActive(false);
			}
			GameObject gameObject7 = Utility.FindChild(widget, "pnlContainer/BtnGroup/BuLuoBtn");
			if (ApolloConfig.platform == 2 || ApolloConfig.platform == 3)
			{
				gameObject7.CustomSetActive(flag);
				if (gameObject7)
				{
					Transform transform = gameObject7.transform.Find("Text");
					if (transform != null)
					{
						transform.GetComponent<Text>().text = "QQ部落";
					}
				}
			}
			else if (ApolloConfig.platform == 1)
			{
				bool bShowWeixinZone = MonoSingleton<PandroaSys>.GetInstance().m_bShowWeixinZone;
				if (bShowWeixinZone)
				{
					gameObject7.CustomSetActive(flag);
					if (gameObject7)
					{
						Transform transform2 = gameObject7.transform.Find("Text");
						if (transform2 != null)
						{
							transform2.GetComponent<Text>().text = "游戏圈";
						}
					}
				}
				else
				{
					gameObject7.CustomSetActive(false);
				}
			}
			else
			{
				gameObject7.CustomSetActive(false);
			}
			if (MonoSingleton<BannerImageSys>.GetInstance().IsWaifaBlockChannel())
			{
				gameObject7.CustomSetActive(false);
			}
			GameObject obj4 = Utility.FindChild(widget, "pnlContainer/BtnGroup/DeepLinkBtn");
			if (ApolloConfig.platform == 2 || ApolloConfig.platform == 3)
			{
				obj4.CustomSetActive(false);
			}
			else
			{
				long curTime = (long)CRoleInfo.GetCurrentUTCTime();
				if (MonoSingleton<BannerImageSys>.GetInstance().DeepLinkInfo.isTimeValid(curTime))
				{
					obj4.CustomSetActive(flag);
				}
				else
				{
					obj4.CustomSetActive(false);
				}
			}
			if (MonoSingleton<BannerImageSys>.GetInstance().IsWaifaBlockChannel())
			{
				obj4.CustomSetActive(false);
			}
			this.UpdateXinYueBtn();
			if (CSysDynamicBlock.bLobbyEntryBlocked)
			{
				Transform transform3 = widget.transform.Find("pnlContainer/pnlHead/changeNobeheadicon");
				if (transform3)
				{
					transform3.gameObject.CustomSetActive(false);
				}
				Transform transform4 = widget.transform.Find("pnlContainer/BtnGroup/BuLuoBtn");
				if (transform4)
				{
					transform4.gameObject.CustomSetActive(false);
				}
				Transform transform5 = widget.transform.Find("pnlContainer/BtnGroup/QQVIPBtn");
				if (transform5)
				{
					transform5.gameObject.CustomSetActive(false);
				}
				Transform transform6 = widget.transform.Find("pnlContainer/BtnGroup/JFQBtn");
				if (transform6)
				{
					transform6.gameObject.CustomSetActive(false);
				}
			}
			Image componetInChild8 = Utility.GetComponetInChild<Image>(widget, "pnlContainer/pnlTrophy/TrophyInfo/Image/Icon");
			Text componetInChild9 = Utility.GetComponetInChild<Text>(widget, "pnlContainer/pnlTrophy/TrophyInfo/Trophy/Level");
			GameObject obj5 = Utility.FindChild(widget, "pnlContainer/pnlTrophy/TrophyInfo/Trophy/Rank");
			Text componetInChild10 = Utility.GetComponetInChild<Text>(widget, "pnlContainer/pnlTrophy/TrophyInfo/Trophy/Rank");
			GameObject obj6 = Utility.FindChild(widget, "pnlContainer/pnlTrophy/TrophyInfo/Button");
			GameObject obj7 = Utility.FindChild(widget, "pnlContainer/pnlTrophy/TrophyInfo/Trophy/txtNotInRank");
			bool flag2 = CPlayerInfoSystem.isSelf(this.m_PlayerProfile.m_uuid);
			if (flag2)
			{
				obj6.CustomSetActive(true);
			}
			else
			{
				obj6.CustomSetActive(false);
			}
			if (componetInChild8 != null)
			{
				CAchieveInfo2 achieveInfo = CAchieveInfo2.GetAchieveInfo(this.m_PlayerProfile.m_iLogicWorldId, this.m_PlayerProfile.m_uuid, false);
				if (achieveInfo.LastDoneTrophyRewardInfo != null)
				{
					componetInChild8.SetSprite(achieveInfo.LastDoneTrophyRewardInfo.GetTrophyImagePath(), this.m_Form, true, false, false, false);
				}
			}
			if (componetInChild9 != null)
			{
				componetInChild9.text = this.m_PlayerProfile._trophyRewardInfoLevel.ToString();
			}
			if (componetInChild10 != null)
			{
				if (this.m_PlayerProfile._trophyRank == 0u)
				{
					obj7.CustomSetActive(true);
					obj5.CustomSetActive(false);
				}
				else
				{
					obj7.CustomSetActive(false);
					obj5.CustomSetActive(true);
					componetInChild10.text = this.m_PlayerProfile._trophyRank.ToString();
				}
			}
			this.ProcessSelectedTrophies();
		}

		private void ProcessSelectedTrophies()
		{
			GameObject widget = this.m_Form.GetWidget(1);
			CAchieveInfo2 achieveInfo = CAchieveInfo2.GetAchieveInfo(this.m_PlayerProfile.m_iLogicWorldId, this.m_PlayerProfile.m_uuid, false);
			bool flag = CPlayerInfoSystem.isSelf(this.m_PlayerProfile.m_uuid);
			bool flag2 = true;
			if (flag)
			{
				ListView<CAchieveItem2> trophies = achieveInfo.GetTrophies(enTrophyState.Finish);
				if (trophies.get_Count() != 0)
				{
					flag2 = false;
				}
			}
			else
			{
				for (int i = 0; i < this.m_PlayerProfile._selectedTrophies.Length; i++)
				{
					if (this.m_PlayerProfile._selectedTrophies[i] != null)
					{
						flag2 = false;
						break;
					}
				}
			}
			ListView<CAchieveItem2> listView = new ListView<CAchieveItem2>();
			if (flag)
			{
				ListView<CAchieveItem2> trophies2 = achieveInfo.GetTrophies(enTrophyState.Finish);
				for (int j = trophies2.get_Count() - 1; j >= 0; j--)
				{
					if (trophies2.get_Item(j) != null && Array.IndexOf<CAchieveItem2>(this.m_PlayerProfile._selectedTrophies, trophies2.get_Item(j)) < 0)
					{
						listView.Add(trophies2.get_Item(j));
					}
				}
			}
			CUIListScript componetInChild = Utility.GetComponetInChild<CUIListScript>(widget, "pnlContainer/pnlTrophy/List");
			if (componetInChild == null)
			{
				DebugHelper.Assert(false, "Player Info selectedTrophyListScript is null!");
				return;
			}
			if (flag2)
			{
				componetInChild.SetElementAmount(0);
				if (flag)
				{
					Text component = componetInChild.GetWidget(0).GetComponent<Text>();
					component.text = Singleton<CTextManager>.GetInstance().GetText("Achievement_Player_Info_Selected_Trophies_Self_No_Data");
				}
				else
				{
					Text component2 = componetInChild.GetWidget(0).GetComponent<Text>();
					component2.text = Singleton<CTextManager>.GetInstance().GetText("Achievement_Player_Info_Selected_Trophies_Other_No_Data");
				}
			}
			else
			{
				componetInChild.SetElementAmount(this.m_PlayerProfile._selectedTrophies.Length);
				Singleton<CTrophySelector>.GetInstance().SelectedTrophies = this.m_PlayerProfile._selectedTrophies;
				for (int k = 0; k < this.m_PlayerProfile._selectedTrophies.Length; k++)
				{
					CUIListElementScript elemenet = componetInChild.GetElemenet(k);
					this.RefreshSelectedAchieveElement(elemenet, this.m_PlayerProfile._selectedTrophies[k], k, flag, listView);
				}
			}
		}

		private void RefreshSelectedAchieveElement(CUIListElementScript elementScript, CAchieveItem2 item, int index, bool isSelf, ListView<CAchieveItem2> filteredTrophies)
		{
			GameObject widget = elementScript.GetWidget(0);
			Image component = widget.GetComponent<Image>();
			GameObject widget2 = elementScript.GetWidget(1);
			GameObject widget3 = elementScript.GetWidget(2);
			Text component2 = widget3.GetComponent<Text>();
			GameObject widget4 = elementScript.GetWidget(3);
			Text component3 = widget4.GetComponent<Text>();
			GameObject widget5 = elementScript.GetWidget(4);
			GameObject widget6 = elementScript.GetWidget(5);
			CUIEventScript component4 = elementScript.GetComponent<CUIEventScript>();
			if (item == null)
			{
				widget.CustomSetActive(false);
				widget6.CustomSetActive(false);
				widget5.CustomSetActive(isSelf && filteredTrophies.get_Count() > 0);
				widget2.CustomSetActive(false);
				widget3.CustomSetActive(false);
				widget4.CustomSetActive(!isSelf || filteredTrophies.get_Count() == 0);
				component3.text = ((!isSelf) ? Singleton<CTextManager>.GetInstance().GetText("Achievement_Status_Not_Chosen") : Singleton<CTextManager>.GetInstance().GetText("Achievement_Status_Not_Done"));
				if (filteredTrophies.get_Count() > 0)
				{
					component4.enabled = true;
					component4.SetUIEvent(enUIEventType.Click, enUIEventID.Achievement_Change_Selected_Trophy, new stUIEventParams
					{
						tag = index
					});
				}
				else
				{
					component4.enabled = false;
				}
			}
			else
			{
				widget.CustomSetActive(true);
				widget6.CustomSetActive(true);
				widget5.CustomSetActive(false);
				widget2.CustomSetActive(isSelf);
				widget3.CustomSetActive(true);
				widget4.CustomSetActive(true);
				CAchieveItem2 cAchieveItem = item.TryToGetMostRecentlyDoneItem();
				component2.text = cAchieveItem.Cfg.szName;
				component.SetSprite(cAchieveItem.GetAchieveImagePath(), elementScript.m_belongedFormScript, true, false, false, false);
				CAchievementSystem.SetAchieveBaseIcon(widget6.transform, cAchieveItem, elementScript.m_belongedFormScript);
				if (cAchieveItem.DoneTime == 0u)
				{
					component3.text = Singleton<CTextManager>.GetInstance().GetText("Achievement_Status_Done");
				}
				else
				{
					component3.text = string.Format("{0:yyyy.M.d} {1}", Utility.ToUtcTime2Local((long)((ulong)cAchieveItem.DoneTime)), Singleton<CTextManager>.GetInstance().GetText("Achievement_Status_Done"));
				}
				component4.enabled = true;
				component4.SetUIEvent(enUIEventType.Click, enUIEventID.Player_Info_Achievement_Trophy_Click, new stUIEventParams
				{
					commonUInt32Param1 = item.ID
				});
				if (isSelf)
				{
					CUIEventScript component5 = widget2.GetComponent<CUIEventScript>();
					component5.SetUIEvent(enUIEventType.Click, enUIEventID.Achievement_Change_Selected_Trophy, new stUIEventParams
					{
						tag = index
					});
				}
			}
		}

		private void UpdatePvpInfo2()
		{
			Singleton<CPlayerPvpInfoController>.get_instance().UpdateUI();
		}

		private void DeepLinkClick(CUIEvent uiEvent)
		{
			if (ApolloConfig.platform == 1 && MonoSingleton<BannerImageSys>.GetInstance().DeepLinkInfo.bLoadSucc)
			{
				Debug.Log(string.Concat(new object[]
				{
					"deeplink ",
					MonoSingleton<BannerImageSys>.GetInstance().DeepLinkInfo.linkType,
					" ",
					MonoSingleton<BannerImageSys>.GetInstance().DeepLinkInfo.linkUrl
				}));
				Singleton<ApolloHelper>.GetInstance().OpenWeiXinDeeplink(MonoSingleton<BannerImageSys>.GetInstance().DeepLinkInfo.linkType, MonoSingleton<BannerImageSys>.GetInstance().DeepLinkInfo.linkUrl);
			}
		}

		private void OpenIntegralHall(CUIEvent uiEvent)
		{
			string text = "http://jfq.qq.com/comm/index_android.html";
			text = string.Format("{0}?partition={1}", text, MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID);
			CUICommonSystem.OpenUrl(text, true, 0);
		}

		private void OpenQQBuluo(CUIEvent uievent)
		{
			if (ApolloConfig.platform == 2 || ApolloConfig.platform == 3)
			{
				string strUrl = "http://xiaoqu.qq.com/cgi-bin/bar/qqgame/handle_ticket?redirect_url=http%3A%2F%2Fxiaoqu.qq.com%2Fmobile%2Fbarindex.html%3F%26_bid%3D%26_wv%3D1027%23bid%3D227061";
				CUICommonSystem.OpenUrl(strUrl, true, 0);
			}
			else if (ApolloConfig.platform == 1)
			{
				string strUrl2 = "http://game.weixin.qq.com/cgi-bin/h5/static/circle/index.html?jsapi=1&appid=wx95a3a4d7c627e07d&auth_type=2&ssid=12";
				CUICommonSystem.OpenUrl(strUrl2, true, 1);
			}
		}

		private void OnAchievementTrophyClick(CUIEvent uiEvent)
		{
			CAchieveInfo2 achieveInfo = CAchieveInfo2.GetAchieveInfo(this.m_PlayerProfile.m_iLogicWorldId, this.m_PlayerProfile.m_uuid, false);
			uint commonUInt32Param = uiEvent.m_eventParams.commonUInt32Param1;
			Singleton<CAchievementSystem>.GetInstance().ShowTrophyDetail(achieveInfo, commonUInt32Param);
		}
	}
}
