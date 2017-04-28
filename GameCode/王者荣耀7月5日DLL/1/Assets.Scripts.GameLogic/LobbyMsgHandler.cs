using Apollo;
using Assets.Scripts.Framework;
using Assets.Scripts.GameSystem;
using Assets.Scripts.Sound;
using Assets.Scripts.UI;
using CSProtocol;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
	[MessageHandlerClass]
	public class LobbyMsgHandler
	{
		public class HashCheckInvalide : Exception
		{
			public HashCheckInvalide()
			{
			}

			public HashCheckInvalide(string message) : base(message)
			{
			}

			public HashCheckInvalide(string message, Exception innerException) : base(message, innerException)
			{
			}

			protected HashCheckInvalide(SerializationInfo info, StreamingContext context) : base(info, context)
			{
			}
		}

		private static bool bVisitorSvr;

		public static bool isHostGMAcnt
		{
			get;
			protected set;
		}

		[MessageHandler(1003)]
		public static void onGameLoginEvent(CSPkg msg)
		{
			if (msg.stPkgData.get_stGameLoginRsp().bIsSucc != 0)
			{
				if (msg.stPkgData.get_stGameLoginRsp().bAcntGM != 0)
				{
					Singleton<CheatWindowExternalIntializer>.CreateInstance();
					MonoSingleton<ConsoleWindow>.get_instance().bEnableCheatConsole = true;
				}
				LobbyMsgHandler.isHostGMAcnt = (msg.stPkgData.get_stGameLoginRsp().bAcntGM != 0);
				uint dwApolloEnvFlag = msg.stPkgData.get_stGameLoginRsp().dwApolloEnvFlag;
				if ((dwApolloEnvFlag & 1u) > 0u)
				{
					ApolloConfig.payEnv = "release";
				}
				else
				{
					ApolloConfig.payEnv = "test";
				}
				if ((dwApolloEnvFlag & 2u) > 0u)
				{
					ApolloConfig.payEnabled = true;
				}
				else
				{
					ApolloConfig.payEnabled = false;
				}
				LobbyMsgHandler.SendMidasToken(Singleton<ApolloHelper>.GetInstance().GetAccountInfo(false));
				Singleton<CRoleInfoManager>.GetInstance().Clean();
				Singleton<LobbyLogic>.GetInstance().uPlayerID = msg.stPkgData.get_stGameLoginRsp().dwGameAcntObjID;
				Singleton<LobbyLogic>.GetInstance().ulAccountUid = msg.stPkgData.get_stGameLoginRsp().ullGameAcntUid;
				Singleton<LobbyLogic>.GetInstance().CreateLocalPlayer(msg.stPkgData.get_stGameLoginRsp().dwGameAcntObjID, msg.stPkgData.get_stGameLoginRsp().ullGameAcntUid, MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID);
				Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().InitGuidedStateBits(msg.stPkgData.get_stGameLoginRsp().stNewbieBits);
				Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().InitNewbieAchieveBits(msg.stPkgData.get_stGameLoginRsp().stClientBits);
				Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().InitClientBits(msg.stPkgData.get_stGameLoginRsp().stNewCltBits);
				Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().InitInBattleNewbieBits(msg.stPkgData.get_stGameLoginRsp().stInBattleNewbieBits);
				Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo().SetAttributes(msg.stPkgData.get_stGameLoginRsp());
				Singleton<CTaskSys>.GetInstance().OnInitTask(msg);
				Singleton<CFunctionUnlockSys>.get_instance().OnSetUnlockTipsMask(msg.stPkgData.get_stGameLoginRsp());
				Singleton<CMailSys>.GetInstance().InitLoginRsp(msg.stPkgData.get_stGameLoginRsp());
				Singleton<CFunctionUnlockSys>.GetInstance();
				Singleton<CPurchaseSys>.GetInstance().SetSvrData(ref msg.stPkgData.get_stGameLoginRsp().stShopBuyRcd);
				Singleton<CArenaSystem>.GetInstance().InitServerData(msg.stPkgData.get_stGameLoginRsp().stArenaData);
				for (int i = 0; i < msg.stPkgData.get_stGameLoginRsp().BanTime.Length; i++)
				{
					MonoSingleton<IDIPSys>.GetInstance().SetBanTimeInfo(i, msg.stPkgData.get_stGameLoginRsp().BanTime[i]);
				}
				Singleton<CGuildSystem>.GetInstance().RequestGuildInfo();
				Singleton<CAchievementSystem>.GetInstance().SendReqGetRankingAcountInfo();
				Singleton<RankingSystem>.GetInstance().SendReqRankingDetail();
				Singleton<CNameChangeSystem>.GetInstance().SetPlayerNameChangeCount((int)msg.stPkgData.get_stGameLoginRsp().dwChgNameCnt);
				Singleton<CPlayerPvpHistoryController>.GetInstance().ClearHostData();
				if (msg.stPkgData.get_stGameLoginRsp().bIsVisitorSvr == 1)
				{
					PlayerPrefs.SetString("visitorUid", msg.stPkgData.get_stGameLoginRsp().ullGameAcntUid.ToString());
					LobbyMsgHandler.bVisitorSvr = true;
				}
				else
				{
					LobbyMsgHandler.bVisitorSvr = false;
				}
				Singleton<HeadIconSys>.get_instance().OnHeadIconSyncList(msg.stPkgData.get_stGameLoginRsp().stHeadImage.wHeadImgCnt, msg.stPkgData.get_stGameLoginRsp().stHeadImage.astHeadImgInfo);
				Singleton<EventRouter>.get_instance().BroadCastEvent(EventID.HEAD_IMAGE_FLAG_CHANGE);
				CChatNetUT.Send_GetChat_Req(EChatChannel.Lobby);
				Singleton<BeaconHelper>.GetInstance().Event_CommonReport("Event_LoginMsgResp");
				Singleton<CTaskSys>.get_instance().model.SyncServerLevelRewardFlagData(msg.stPkgData.get_stGameLoginRsp().ullLevelRewardFlag);
				Singleton<CChatController>.get_instance().model.bEnableInBattleInputChat = (msg.stPkgData.get_stGameLoginRsp().bIsInBatInputAllowed == 1);
				CLobbySystem.IsPlatChannelOpen = (msg.stPkgData.get_stGameLoginRsp().bPlatChannelOpen > 0);
				COMDT_SELFDEFINE_CHATINFO stSelfDefineChatInfo = msg.stPkgData.get_stGameLoginRsp().stSelfDefineChatInfo;
				Singleton<InBattleMsgMgr>.get_instance().ParseServerData(stSelfDefineChatInfo);
				Singleton<CMallSystem>.GetInstance().m_PlayerRegisterTime = Utility.ToUtcTime2Local((long)((ulong)msg.stPkgData.get_stGameLoginRsp().dwFirstLoginTime));
			}
		}

		[MessageHandler(2509)]
		public static void OnRoleExtraCoinAndExpChange(CSPkg msg)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null && msg.stPkgData.get_stPropMultipleNtf() != null)
			{
				masterRoleInfo.SetExtraCoinAndExp(msg.stPkgData.get_stPropMultipleNtf().stPropMultiple);
			}
		}

		[MessageHandler(1026)]
		public static void onAskTransferVisitorData(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(Singleton<CTextManager>.GetInstance().GetText("Transfer_Visitor_Data"), enUIEventID.Login_Trans_Visitor_Yes, enUIEventID.Login_Trans_Visitor_No, false);
		}

		[MessageHandler(1403)]
		public static void onPlayerInfoPush(CSPkg msg)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo == null)
			{
				return;
			}
			masterRoleInfo.m_payLevel = msg.stPkgData.get_stAcntDetailInfoRsp().dwPayLevel;
			masterRoleInfo.pvpDetail = msg.stPkgData.get_stAcntDetailInfoRsp().stPvpDetailInfo;
			masterRoleInfo.m_baseGuildInfo.name = StringHelper.UTF8BytesToString(ref msg.stPkgData.get_stAcntDetailInfoRsp().szGuildName);
		}

		[MessageHandler(1404)]
		public static void onRoleHeadUrlChange(CSPkg msg)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				masterRoleInfo.HeadUrl = Singleton<ApolloHelper>.GetInstance().ToSnsHeadUrl(ref msg.stPkgData.get_stAcntHeadUrlChgNtf().szHeadUrl);
			}
		}

		[MessageHandler(5246)]
		public static void OnAcntMobaInfo(CSPkg msg)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				masterRoleInfo.acntMobaInfo = msg.stPkgData.get_stAcntMobaInfoNtf().stMobaInfo;
			}
			Singleton<CRoleRegisterSys>.get_instance().RefreshRecommendTips();
		}

		[MessageHandler(1001)]
		public static void onGameLoginDispatch(CSPkg msg)
		{
		}

		[MessageHandler(1017)]
		public static void onGameLogout(CSPkg msg)
		{
			if (msg.stPkgData.get_stGameLogoutRsp().iLogoutType == 0)
			{
				Singleton<LobbyLogic>.GetInstance().GotoAccLoginPage();
			}
			else if (msg.stPkgData.get_stGameLogoutRsp().iLogoutType == 1)
			{
			}
			if (Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo() != null)
			{
				Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().Clear();
			}
			MonoSingleton<PandroaSys>.GetInstance().UninitSys();
		}

		[MessageHandler(1190)]
		public static void onCSError(CSPkg msg)
		{
			Singleton<CUIManager>.get_instance().CloseSendMsgAlert();
			if (msg.stPkgData.get_stNtfErr().iErrCode == 20)
			{
				Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("Common_Protocol_Error"), enUIEventID.Lobby_ConfirmErrExit, false);
			}
			else if (msg.stPkgData.get_stNtfErr().iErrCode == 21)
			{
				Singleton<LobbyLogic>.GetInstance().GotoAccLoginPage();
				Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("Common_Log_Off_Tip"), enUIEventID.Lobby_ConfirmErrExit, false);
			}
			else if (msg.stPkgData.get_stNtfErr().iErrCode == 34)
			{
				Singleton<LobbyLogic>.GetInstance().GotoAccLoginPage();
				Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("Common_Svr_Acnt_Exception"), enUIEventID.Lobby_ConfirmErrExit, false);
			}
			else if (msg.stPkgData.get_stNtfErr().iErrCode == 22)
			{
				Singleton<LobbyLogic>.GetInstance().GotoAccLoginPage();
				Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("Common_Svr_Shutdown_Tip"), enUIEventID.Lobby_ConfirmErrExit, false);
			}
			else if (msg.stPkgData.get_stNtfErr().iErrCode == 23)
			{
				Singleton<NetworkModule>.GetInstance().CloseAllServerConnect();
				Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
				Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("Common_Svr_Not_WhiteList"), enUIEventID.Lobby_ConfirmErrExit, false);
			}
			else if (msg.stPkgData.get_stNtfErr().iErrCode == 24)
			{
				Singleton<NetworkModule>.GetInstance().CloseAllServerConnect();
				Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
				Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("Common_Svr_In_BlackList"), enUIEventID.Lobby_ConfirmErrExit, false);
			}
			else if (msg.stPkgData.get_stNtfErr().iErrCode == 19)
			{
				Singleton<NetworkModule>.GetInstance().CloseAllServerConnect();
				Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
				Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("Sever_maintaining"), enUIEventID.Lobby_ConfirmErrExit, false);
			}
			else if (msg.stPkgData.get_stNtfErr().iErrCode == 30)
			{
				if (msg.stPkgData.get_stNtfErr().stErrDetail.get_stRegisterNameErrNtf().bIsEvil == 1)
				{
					Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("Register_Name_Invalid_Words"), false);
				}
				else
				{
					Singleton<CRoleRegisterSys>.get_instance().ShowErrorCode(msg.stPkgData.get_stNtfErr().stErrDetail.get_stRegisterNameErrNtf().szUserName);
				}
			}
			else if (msg.stPkgData.get_stNtfErr().iErrCode == 31)
			{
				Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("Room_Name_Invalid_Words"), false);
			}
			else if (msg.stPkgData.get_stNtfErr().iErrCode == 32)
			{
				Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("Register_Reach_Limit"), false);
			}
			else if (msg.stPkgData.get_stNtfErr().iErrCode == 36)
			{
				Singleton<LobbySvrMgr>.get_instance().canReconnect = false;
				Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("Register_Reach_Total_Limit"), false);
			}
			else if (msg.stPkgData.get_stNtfErr().iErrCode == 40)
			{
				Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("Video_Server_Error"), enUIEventID.Lobby_ConfirmErrExit, false);
			}
			else if (msg.stPkgData.get_stNtfErr().iErrCode != 41)
			{
				if (msg.stPkgData.get_stNtfErr().iErrCode == 25)
				{
					Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("VersionIsLow"), enUIEventID.Lobby_ConfirmErrExit, false);
				}
				else if (msg.stPkgData.get_stNtfErr().iErrCode == 33)
				{
					Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("Common_Svr_BanAcnt"), enUIEventID.Lobby_ConfirmErrExit, false);
				}
				else if (msg.stPkgData.get_stNtfErr().iErrCode == 35)
				{
					Singleton<CUIManager>.GetInstance().OpenMessageBox("您已被踢下线", enUIEventID.Lobby_ConfirmErrExit, false);
				}
				else if (msg.stPkgData.get_stNtfErr().iErrCode == 16)
				{
					Singleton<LobbyLogic>.GetInstance().GotoAccLoginPage();
					Singleton<CUIManager>.GetInstance().OpenSendMsgAlert(Singleton<CTextManager>.GetInstance().GetText("ERR_RECONNLOGICWORLDIDINVALID_Tips"), 6, enUIEventID.None);
				}
				else if (msg.stPkgData.get_stNtfErr().iErrCode == 50)
				{
					MonoSingleton<Reconnection>.GetInstance().RequestRelaySyncCacheFrames(false);
				}
				else if (msg.stPkgData.get_stNtfErr().iErrCode == 17)
				{
					Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("VersionIsLow"), enUIEventID.TDir_QuitGame, false);
				}
				else if (msg.stPkgData.get_stNtfErr().iErrCode == 18)
				{
					Singleton<LobbyLogic>.GetInstance().SvrNtfUpdateClient();
				}
				else if (msg.stPkgData.get_stNtfErr().iErrCode == 136)
				{
					Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("Chat_Common_Tips_7"), false);
				}
				else if (msg.stPkgData.get_stNtfErr().iErrCode == 137)
				{
					Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("Chat_Common_Tips_9"), false);
				}
				else if (msg.stPkgData.get_stNtfErr().iErrCode == 138)
				{
					Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("Chat_Common_Tips_8"), false);
				}
				else if (msg.stPkgData.get_stNtfErr().iErrCode == 52)
				{
					Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("ERR_APOLLOPAY_FAST"), false, 1.5f, null, new object[0]);
				}
				else if (msg.stPkgData.get_stNtfErr().iErrCode == 137)
				{
					Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("ERR_InBattleMsg_CD"), false, 1.5f, null, new object[0]);
				}
				else if (msg.stPkgData.get_stNtfErr().iErrCode == 155)
				{
					Singleton<CChatController>.get_instance().model.bEnableInBattleInputChat = false;
					Singleton<InBattleMsgMgr>.get_instance().ServerDisableInputChat();
					Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("ERR_InBattleMsg_Input_Off"), false, 1.5f, null, new object[0]);
				}
				else if (msg.stPkgData.get_stNtfErr().iErrCode == 158)
				{
					Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("CS_ERR_CHEST_HAVESHARED"), false, 1.5f, null, new object[0]);
				}
				else if (msg.stPkgData.get_stNtfErr().iErrCode == 159)
				{
					Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("CS_ERR_CHEST_CONDITION"), false, 1.5f, null, new object[0]);
				}
				else if (msg.stPkgData.get_stNtfErr().iErrCode == 176)
				{
					Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("Login_Error"), enUIEventID.Lobby_ConfirmErrExit, false);
				}
				else if (msg.stPkgData.get_stNtfErr().iErrCode == 177)
				{
					Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("PCU_Limit"), enUIEventID.Lobby_ConfirmErrExit, false);
				}
				else if (msg.stPkgData.get_stNtfErr().iErrCode == 178)
				{
					Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("SecurePwd_Invalid_Pwd"), false);
				}
				else if (msg.stPkgData.get_stNtfErr().iErrCode == 184)
				{
					Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("SecurePwd_Invalid_Pwd"), false);
				}
				else if (msg.stPkgData.get_stNtfErr().iErrCode == 182)
				{
					if (msg.stPkgData.get_stNtfErr().stErrDetail.get_stSwitchErrNtf().bIsLogin == 0)
					{
						Singleton<CUIManager>.GetInstance().OpenMessageBox(Utility.UTF8Convert(msg.stPkgData.get_stNtfErr().stErrDetail.get_stSwitchErrNtf().szTips), false);
					}
					Singleton<SCModuleControl>.get_instance().OnModuleSwitchNtf(msg.stPkgData.get_stNtfErr().stErrDetail.get_stSwitchErrNtf());
				}
				else if (msg.stPkgData.get_stNtfErr().iErrCode == 183)
				{
					Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("REDIRECT_LOGICWORLDID"), enUIEventID.TDir_QuitGame, false);
				}
			}
			Singleton<EventRouter>.get_instance().BroadCastEvent<int>(EventID.ERRCODE_NTF, msg.stPkgData.get_stNtfErr().iErrCode);
		}

		[MessageHandler(1042)]
		public static void onGameLoginLimit(CSPkg msg)
		{
			if (msg.stPkgData.get_stLoginLimitRsp().iErrCode == 33)
			{
				DateTime dateTime = Utility.ToUtcTime2Local((long)((ulong)msg.stPkgData.get_stLoginLimitRsp().dwLimitTime));
				string strContent = string.Format("您已被封号！封号截止时间为{0}年{1}月{2}日{3}时{4}分", new object[]
				{
					dateTime.get_Year(),
					dateTime.get_Month(),
					dateTime.get_Day(),
					dateTime.get_Hour(),
					dateTime.get_Minute()
				});
				Singleton<CUIManager>.GetInstance().OpenMessageBox(strContent, enUIEventID.Lobby_ConfirmErrExit, false);
			}
			else if (msg.stPkgData.get_stLoginLimitRsp().iErrCode == 35)
			{
				DateTime dateTime2 = Utility.ToUtcTime2Local((long)((ulong)msg.stPkgData.get_stLoginLimitRsp().dwLimitTime));
				string strContent2 = string.Format("您已被踢下线！截止时间为{0}年{1}月{2}日{3}时{4}分", new object[]
				{
					dateTime2.get_Year(),
					dateTime2.get_Month(),
					dateTime2.get_Day(),
					dateTime2.get_Hour(),
					dateTime2.get_Minute()
				});
				Singleton<CUIManager>.GetInstance().OpenMessageBox(strContent2, enUIEventID.Lobby_ConfirmErrExit, false);
			}
		}

		[MessageHandler(1014)]
		public static void onGameReloginNow(CSPkg msg)
		{
			Singleton<NetworkModule>.get_instance().ResetLobbySending();
			if (Singleton<LobbyLogic>.get_instance().isLogin)
			{
				Singleton<LobbySvrMgr>.GetInstance().isLogin = false;
				Singleton<LobbyLogic>.get_instance().isLogin = false;
				if (Singleton<WatchController>.GetInstance().IsReplaying)
				{
					Singleton<CLobbySystem>.GetInstance().NeedRelogin = true;
				}
				else
				{
					LobbyMsgHandler.PopupRelogin();
				}
			}
			else
			{
				Singleton<LobbyLogic>.get_instance().LoginGame();
			}
		}

		public static void PopupRelogin()
		{
			Singleton<CUIManager>.GetInstance().OpenMessageBox("与服务器连接丢失，将重新与服务器建立连接。", enUIEventID.Net_SvrNtfReloginNow, false);
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Net_SvrNtfReloginNow, new CUIEventManager.OnUIEventHandler(LobbyMsgHandler.OnConfirmReloginNow));
		}

		public static void OnConfirmReloginNow(CUIEvent uiEvent)
		{
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Net_SvrNtfReloginNow, new CUIEventManager.OnUIEventHandler(LobbyMsgHandler.OnConfirmReloginNow));
			Singleton<GameBuilder>.get_instance().EndGame();
			Singleton<NetworkModule>.get_instance().CloseGameServerConnect(true);
			Singleton<GameEventSys>.get_instance().SendEvent(GameEventDef.Event_LobbyRelogining);
			Singleton<LobbyLogic>.get_instance().LoginGame();
		}

		[MessageHandler(1018)]
		public static void onGameServerLoginKeyReq(CSPkg msg)
		{
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<ApolloAccountInfo>(EventID.ApolloHelper_Login_Success, new Action<ApolloAccountInfo>(LobbyMsgHandler.SendMidasToken));
			Singleton<EventRouter>.GetInstance().AddEventHandler<ApolloAccountInfo>(EventID.ApolloHelper_Login_Success, new Action<ApolloAccountInfo>(LobbyMsgHandler.SendMidasToken));
			Singleton<ApolloHelper>.GetInstance().Login(Singleton<ApolloHelper>.GetInstance().CurPlatform, 0uL, null);
		}

		[MessageHandler(1040)]
		public static void onLobbyOffingRestartReq(CSPkg msg)
		{
			byte bNeedLoginRsp = 0;
			if (!Singleton<LobbyLogic>.get_instance().isLogin)
			{
				bNeedLoginRsp = 1;
				Singleton<NetworkModule>.get_instance().ResetLobbySending();
			}
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1041u);
			cSPkg.stPkgData.get_stOffingRestartRsp().bNeedLoginRsp = bNeedLoginRsp;
			cSPkg.stPkgData.get_stOffingRestartRsp().iLogicWorldID = MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID;
			cSPkg.stPkgData.get_stOffingRestartRsp().bPrivilege = Singleton<ApolloHelper>.GetInstance().GetCurrentLoginPrivilege();
			uint versionNumber = CVersion.GetVersionNumber(GameFramework.AppVersion);
			cSPkg.stPkgData.get_stOffingRestartRsp().iCltAppVersion = (int)versionNumber;
			uint versionNumber2 = CVersion.GetVersionNumber(CVersion.GetUsedResourceVersion());
			cSPkg.stPkgData.get_stOffingRestartRsp().iCltResVersion = (int)versionNumber2;
			byte[] bytes = Encoding.get_ASCII().GetBytes(SystemInfo.deviceUniqueIdentifier);
			if (bytes.Length > 0)
			{
				Buffer.BlockCopy(bytes, 0, cSPkg.stPkgData.get_stOffingRestartRsp().szCltIMEI, 0, (bytes.Length <= 64) ? bytes.Length : 64);
			}
			Singleton<NetworkModule>.get_instance().SendLobbyMsg(ref cSPkg, false);
		}

		[MessageHandler(1030)]
		public static void onLobbyConnectRedirect(CSPkg msg)
		{
			Singleton<NetworkModule>.GetInstance().CloseLobbyServerConnect();
			Singleton<NetworkModule>.GetInstance().lobbySvr.RedirectNewPort(msg.stPkgData.get_stGameConnRedirect().wRedirectGameVport);
		}

		[MessageHandler(1013)]
		public static void onGameLoginFinish(CSPkg msg)
		{
			if (Singleton<LobbyLogic>.get_instance().isLogin)
			{
				return;
			}
			Singleton<LobbySvrMgr>.GetInstance().isLogin = true;
			Singleton<LobbyLogic>.get_instance().isLogin = true;
			Singleton<CUIManager>.get_instance().CloseSendMsgAlert();
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				switch (masterRoleInfo.m_AcntOldType)
				{
				case 0:
					return;
				case 2:
					Singleton<CUIManager>.GetInstance().CloseAllForm(null, true, true);
					NewbieGuideManager.ShowCompleteNewbieGuidePanel();
					return;
				}
				LobbyMsgHandler.FinishGameLogin();
			}
			else
			{
				LobbyMsgHandler.FinishGameLogin();
			}
		}

		[MessageHandler(5241)]
		public static void OnReciveOldAcntType(CSPkg msg)
		{
			SCPKG_ACNT_OLD_TYPE_NTF stAcntOldTypeNtf = msg.stPkgData.get_stAcntOldTypeNtf();
			if (stAcntOldTypeNtf.iErrCode == 0)
			{
				byte bAcntOldType = stAcntOldTypeNtf.bAcntOldType;
				if (bAcntOldType != 2)
				{
					if (bAcntOldType != 3)
					{
						LobbyMsgHandler.FinishGameLogin();
					}
					else
					{
						if (MonoSingleton<NewbieGuideManager>.GetInstance().IsComplteNewbieGuide)
						{
							NewbieGuideManager.CompleteAllNewbieGuide();
						}
						LobbyMsgHandler.FinishGameLogin();
					}
				}
				else
				{
					Singleton<CUIManager>.GetInstance().CloseAllForm(null, true, true);
					NewbieGuideManager.ShowCompleteNewbieGuidePanel();
				}
			}
			else if (stAcntOldTypeNtf.iErrCode == 179)
			{
				Singleton<CUIManager>.get_instance().OpenTips("Old_Acnt_Err_Invalid_Old_Type", true, 1.5f, null, new object[0]);
				LobbyMsgHandler.FinishGameLogin();
			}
		}

		private static void FinishGameLogin()
		{
			bool flag = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().IsGuidedStateSet(0);
			if (CSysDynamicBlock.bNewbieBlocked)
			{
				flag = true;
			}
			if (!flag)
			{
				MonoSingleton<ShareSys>.GetInstance().ClearShareDataMsg();
				Singleton<CRoleRegisterSys>.get_instance().CloseRoleCreateForm();
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				if (masterRoleInfo.acntMobaInfo.iSelectedHeroType == 0)
				{
					Singleton<CRoleRegisterSys>.get_instance().OpenHeroTypeSelectForm();
				}
				else
				{
					LobbyLogic.ReqStartGuideLevel11(false, (uint)masterRoleInfo.acntMobaInfo.iSelectedHeroType);
				}
				MonoSingleton<NewbieGuideManager>.GetInstance().CheckSkipIntoLobby();
				Singleton<CUINewFlagSystem>.GetInstance().SetAllNewFlagKey();
			}
			else
			{
				if (!Singleton<BattleLogic>.get_instance().isRuning)
				{
					DebugHelper.CustomLog("LobbyStateBy onGameLoginFinish");
					Singleton<GameStateCtrl>.GetInstance().GotoState("LobbyState");
				}
				if (!Singleton<LobbySvrMgr>.GetInstance().isFirstLogin)
				{
					Singleton<LobbySvrMgr>.GetInstance().isFirstLogin = true;
					Singleton<CTimerManager>.GetInstance().AddTimer(1000, 1, delegate(int seq)
					{
						if (Singleton<BattleLogic>.GetInstance().isRuning || !CLobbySystem.AutoPopAllow)
						{
							return;
						}
						if (LobbyMsgHandler.bVisitorSvr)
						{
							Singleton<ApolloHelper>.GetInstance().ShowIOSGuestNotice();
						}
						else
						{
							Singleton<ApolloHelper>.GetInstance().ShowNotice(0, "2");
						}
					});
				}
			}
		}

		[MessageHandler(1051)]
		public static void onSingleGameLoad(CSPkg msg)
		{
			if (msg.stPkgData.get_stStartSingleGameRsp().iErrCode == 0)
			{
				Singleton<LobbyLogic>.get_instance().inMultiGame = false;
				Singleton<GameBuilder>.get_instance().StartGame(SingleGameContextFactory.CreateSingleGameContext(msg.stPkgData.get_stStartSingleGameRsp()));
				Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			}
			else
			{
				Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
				Singleton<CHeroSelectBaseSystem>.get_instance().CloseForm();
				if (msg.stPkgData.get_stStartSingleGameRsp().iErrCode == 10)
				{
					if (msg.stPkgData.get_stStartSingleGameRsp().bGameType == 7)
					{
						DateTime banTime = MonoSingleton<IDIPSys>.GetInstance().GetBanTime(12);
						string strContent = string.Format("您被禁止进入六国远征！截止时间为{0}年{1}月{2}日{3}时{4}分", new object[]
						{
							banTime.get_Year(),
							banTime.get_Month(),
							banTime.get_Day(),
							banTime.get_Hour(),
							banTime.get_Minute()
						});
						Singleton<CUIManager>.GetInstance().OpenMessageBox(strContent, false);
					}
					else if (msg.stPkgData.get_stStartSingleGameRsp().bGameType == 8)
					{
						DateTime banTime2 = MonoSingleton<IDIPSys>.GetInstance().GetBanTime(14);
						string strContent2 = string.Format("您被禁止进入武道会！截止时间为{0}年{1}月{2}日{3}时{4}分", new object[]
						{
							banTime2.get_Year(),
							banTime2.get_Month(),
							banTime2.get_Day(),
							banTime2.get_Hour(),
							banTime2.get_Minute()
						});
						Singleton<CUIManager>.GetInstance().OpenMessageBox(strContent2, false);
					}
					else if (msg.stPkgData.get_stStartSingleGameRsp().bGameType == 0)
					{
						DateTime banTime3 = MonoSingleton<IDIPSys>.GetInstance().GetBanTime(10);
						string strContent3 = string.Format("您被禁止进入闯关！截止时间为{0}年{1}月{2}日{3}时{4}分", new object[]
						{
							banTime3.get_Year(),
							banTime3.get_Month(),
							banTime3.get_Day(),
							banTime3.get_Hour(),
							banTime3.get_Minute()
						});
						Singleton<CUIManager>.GetInstance().OpenMessageBox(strContent3, false);
					}
				}
				else
				{
					Singleton<CUIManager>.GetInstance().OpenTips(Utility.ProtErrCodeToStr(1051, msg.stPkgData.get_stStartSingleGameRsp().iErrCode), false, 1.5f, null, new object[0]);
				}
			}
		}

		[MessageHandler(1053)]
		public static void onSingleGameFinish(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<LobbyLogic>.GetInstance().StopSettleMsgTimer();
			Singleton<CBattleHeroInfoPanel>.GetInstance().Hide();
			if (msg.stPkgData.get_stFinSingleGameRsp().iErrCode == 0)
			{
				if (Singleton<BattleLogic>.get_instance().isWaitGameEnd)
				{
					DebugHelper.Assert(Singleton<BattleLogic>.get_instance().m_cachedSvrEndData == null);
					Singleton<BattleLogic>.get_instance().m_cachedSvrEndData = msg;
				}
				else
				{
					LobbyMsgHandler.HandleSingleGameSettle(msg);
				}
			}
		}

		public static void HandleSingleGameSettle(CSPkg msg)
		{
			Singleton<CPlayerPvpHistoryController>.GetInstance().CommitHistoryInfo(msg.stPkgData.get_stFinSingleGameRsp().stDetail.stGameInfo.bGameResult == 1);
			Singleton<SingleGameSettleMgr>.GetInstance().StartSettle(msg.stPkgData.get_stFinSingleGameRsp());
			int iLevelID = msg.stPkgData.get_stFinSingleGameRsp().stDetail.stGameInfo.iLevelID;
			bool flag = msg.stPkgData.get_stFinSingleGameRsp().stDetail.stGameInfo.bGameType == 2;
			bool flag2 = msg.stPkgData.get_stFinSingleGameRsp().stDetail.stGameInfo.bGameType == 1;
			if (!flag && !flag2)
			{
				MonoSingleton<NewbieGuideManager>.GetInstance().CheckSkipCondition(NewbieGuideSkipConditionType.hasCompleteDungeon, new uint[]
				{
					(uint)iLevelID
				});
			}
			SLevelContext curLvelContext = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
			if (curLvelContext != null && curLvelContext.IsMobaMode() && !flag)
			{
				Singleton<BattleLogic>.GetInstance().ShowWinLose(Singleton<WinLose>.get_instance().LastSingleGameWin);
				Singleton<WinLose>.get_instance().LastSingleGameWin = true;
			}
			else
			{
				Singleton<LobbyLogic>.GetInstance().StopSettlePanelTimer();
			}
			if (flag)
			{
				uint dwConfValue = GameDataMgr.globalInfoDatabin.GetDataByKey(118u).dwConfValue;
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				if ((long)Singleton<BattleLogic>.GetInstance().GetCurLvelContext().m_mapID == (long)((ulong)dwConfValue) && masterRoleInfo.IsNewbieAchieveSet(61))
				{
					masterRoleInfo.SetNewbieAchieve(63, true, true);
				}
			}
		}

		[MessageHandler(1078)]
		public static void onMultiGameReady(CSPkg msg)
		{
			if (msg.stPkgData.get_stMultGameReadyNtf().bCamp == 0)
			{
				Singleton<WatchController>.GetInstance().StartJudge(msg.stPkgData.get_stMultGameReadyNtf().stRelayTGW);
			}
			else
			{
				NetworkModule.InitRelayConnnecting(msg.stPkgData.get_stMultGameReadyNtf().stRelayTGW);
			}
		}

		[MessageHandler(1079)]
		public static void onMultiChoosePlayerLeave(CSPkg msg)
		{
			string strContent = Utility.ProtErrCodeToStr(1079, (int)msg.stPkgData.get_stMultGameAbortNtf().bReason);
			Singleton<CUIManager>.GetInstance().OpenTips(strContent, false, 1.5f, null, new object[0]);
			Singleton<CHeroSelectBaseSystem>.get_instance().CloseForm();
			Singleton<CRoomSystem>.GetInstance().CloseRoom();
			Singleton<CMatchingSystem>.GetInstance().CloseMatchingConfirm();
			Singleton<NetworkModule>.GetInstance().CloseGameServerConnect(true);
			if (msg.stPkgData.get_stMultGameAbortNtf().bReason == 17 || msg.stPkgData.get_stMultGameAbortNtf().bReason == 15)
			{
				Singleton<CSoundManager>.GetInstance().PostEvent("UI_matching_lost", null);
			}
			DebugHelper.CustomLog("LobbyStateBy onMultiChoosePlayerLeave");
			Singleton<GameStateCtrl>.GetInstance().GotoState("LobbyState");
			Singleton<LobbyLogic>.GetInstance().inMultiRoom = false;
		}

		[MessageHandler(1075)]
		public static void onMultiGameLoad(CSPkg msg)
		{
			if (Singleton<LobbyLogic>.get_instance().inMultiGame)
			{
				return;
			}
			Singleton<LobbyLogic>.get_instance().inMultiGame = true;
			Singleton<GameBuilder>.get_instance().StartGame(new MultiGameContext(msg.stPkgData.get_stMultGameBeginLoad()));
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
		}

		[MessageHandler(1077)]
		public static void onMultiGameFight(CSPkg msg)
		{
			if (Singleton<BattleLogic>.get_instance().isWaitMultiStart || Singleton<BattleLogic>.get_instance().isFighting)
			{
				return;
			}
			if (MonoSingleton<GameLoader>.get_instance().isLoadStart)
			{
				Singleton<BattleLogic>.get_instance().isWaitMultiStart = true;
				Singleton<FrameSynchr>.get_instance().StartSynchr();
			}
			else
			{
				Singleton<FrameSynchr>.get_instance().StartSynchr();
				Singleton<BattleLogic>.get_instance().StartFightMultiGame();
			}
		}

		[MessageHandler(1081)]
		public static void onMultiGameOver(CSPkg msg)
		{
		}

		public static void HandleGameSettle(bool bSuccess, bool bShouldDisplayWinLose, byte GameResult, COMDT_SETTLE_HERO_RESULT_DETAIL heroSettleInfo, COMDT_RANK_SETTLE_INFO rankInfo, COMDT_ACNT_INFO acntInfo, COMDT_REWARD_MULTIPLE_DETAIL mltDetail, COMDT_PVPSPECITEM_OUTPUT specReward, COMDT_REWARD_DETAIL reward)
		{
			Singleton<BattleLogic>.get_instance().onPreGameSettle();
			if (bSuccess)
			{
				SettleEventParam settleEventParam = default(SettleEventParam);
				settleEventParam.isWin = (GameResult == 1);
				Singleton<GameEventSys>.GetInstance().SendEvent<SettleEventParam>(GameEventDef.Event_SettleComplete, ref settleEventParam);
				if (bShouldDisplayWinLose)
				{
					Singleton<BattleLogic>.GetInstance().ShowWinLose(GameResult == 1);
				}
				if (heroSettleInfo != null)
				{
					Singleton<BattleStatistic>.GetInstance().heroSettleInfo = heroSettleInfo;
				}
				if (rankInfo != null)
				{
					Singleton<BattleStatistic>.GetInstance().rankInfo = rankInfo;
				}
				if (acntInfo != null)
				{
					Singleton<BattleStatistic>.GetInstance().acntInfo = acntInfo;
				}
				Singleton<BattleStatistic>.GetInstance().SpecialItemInfo = specReward;
				Singleton<BattleStatistic>.GetInstance().Rewards = reward;
				if (mltDetail != null)
				{
					Singleton<BattleStatistic>.GetInstance().multiDetail = mltDetail;
				}
			}
			if (Singleton<CUIManager>.get_instance().GetForm(WinLose.m_FormPath) == null)
			{
				Singleton<GameBuilder>.get_instance().EndGame();
			}
		}

		[MessageHandler(1082)]
		public static void onMultiGameSettle(CSPkg msg)
		{
			Singleton<LobbyLogic>.GetInstance().StopSettleMsgTimer();
			Singleton<BattleStatistic>.get_instance().RecordMvp(msg.stPkgData.get_stMultGameSettleGain().stDetail.stGameInfo);
			Singleton<CPlayerPvpHistoryController>.GetInstance().CommitHistoryInfo(msg.stPkgData.get_stMultGameSettleGain().stDetail.stGameInfo.bGameResult == 1);
			DebugHelper.Assert(msg.stPkgData.get_stMultGameSettleGain().iErrCode == 0, "SCID_MULTGAME_SETTLEGAIN Error: {0}", new object[]
			{
				msg.stPkgData.get_stMultGameSettleGain().iErrCode
			});
			if (MonoSingleton<Reconnection>.GetInstance().isProcessingRelayRecover)
			{
				Singleton<GameBuilder>.GetInstance().EndGame();
				DebugHelper.CustomLog("LobbyStateBy onMultiGameSettle");
				Singleton<GameStateCtrl>.GetInstance().GotoState("LobbyState");
				Singleton<CUIManager>.GetInstance().OpenTips("gameWasOver", true, 1.5f, null, new object[0]);
				return;
			}
			if (Singleton<BattleLogic>.get_instance().isWaitGameEnd)
			{
				DebugHelper.Assert(Singleton<BattleLogic>.get_instance().m_cachedSvrEndData == null);
				Singleton<BattleLogic>.get_instance().m_cachedSvrEndData = msg;
			}
			else
			{
				if (msg.stPkgData.get_stMultGameSettleGain().iErrCode == 0)
				{
					SLevelContext.SetMasterPvpDetailWhenGameSettle(msg.stPkgData.get_stMultGameSettleGain().stDetail.stGameInfo);
				}
				LobbyMsgHandler.HandleGameSettle(msg.stPkgData.get_stMultGameSettleGain().iErrCode == 0, true, msg.stPkgData.get_stMultGameSettleGain().stDetail.stGameInfo.bGameResult, msg.stPkgData.get_stMultGameSettleGain().stDetail.stHeroList, msg.stPkgData.get_stMultGameSettleGain().stDetail.stRankInfo, msg.stPkgData.get_stMultGameSettleGain().stDetail.stAcntInfo, msg.stPkgData.get_stMultGameSettleGain().stDetail.stMultipleDetail, msg.stPkgData.get_stMultGameSettleGain().stDetail.stSpecReward, msg.stPkgData.get_stMultGameSettleGain().stDetail.stReward);
			}
			for (int i = 0; i < 8; i++)
			{
				if (((ulong)msg.stPkgData.get_stMultGameSettleGain().stDetail.dwTipsMask & (ulong)(1L << (i & 31))) != 0uL)
				{
					int num = i;
					if (num == 1)
					{
						uint dwConfValue = GameDataMgr.globalInfoDatabin.GetDataByKey(278u).dwConfValue;
						Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("Settlement_WEEK_Gold_Tips_FirstWin", new string[]
						{
							dwConfValue.ToString()
						}), false, 1.5f, null, new object[0]);
					}
				}
			}
			Singleton<NetworkModule>.get_instance().CloseGameServerConnect(true);
			MonoSingleton<VoiceSys>.GetInstance().LeaveRoom();
			try
			{
				List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
				list.Add(new KeyValuePair<string, string>("g_version", CVersion.GetAppVersion()));
				list.Add(new KeyValuePair<string, string>("WorldID", MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID.ToString()));
				list.Add(new KeyValuePair<string, string>("platform", Singleton<ApolloHelper>.GetInstance().CurPlatform.ToString()));
				list.Add(new KeyValuePair<string, string>("openid", "NULL"));
				list.Add(new KeyValuePair<string, string>("error", "0"));
				Singleton<ApolloHelper>.GetInstance().ApolloRepoertEvent("Service_settlement", list, true);
			}
			catch (Exception ex)
			{
				Debug.Log(ex.ToString());
			}
		}

		[MessageHandler(1801)]
		public static void onChooseHeroRsp(CSPkg msg)
		{
			if (msg.stPkgData.get_stAchieveHeroRsp().iResult == 0)
			{
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				masterRoleInfo.InitHero(msg.stPkgData.get_stAchieveHeroRsp().stHeroInfo);
				masterRoleInfo.SetHeroSkinData(msg.stPkgData.get_stAchieveHeroRsp().stHeroSkin);
				int dwConfValue = (int)GameDataMgr.globalInfoDatabin.GetDataByKey(116u).dwConfValue;
				Singleton<LobbyLogic>.GetInstance().ReqStartGuideLevel(dwConfValue, msg.stPkgData.get_stAchieveHeroRsp().stHeroInfo.stCommonInfo.dwHeroID);
			}
		}

		[MessageHandler(1813)]
		public static void onGmAddHeroRsp(CSPkg msg)
		{
			if (msg.stPkgData.get_stGMAddHeroRsp().iResult == 0)
			{
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				masterRoleInfo.InitHero(msg.stPkgData.get_stGMAddHeroRsp().stHeroInfo);
				masterRoleInfo.SetHeroSkinData(msg.stPkgData.get_stGMAddHeroRsp().stHeroSkin);
				Singleton<EventRouter>.get_instance().BroadCastEvent<uint>("HeroAdd", msg.stPkgData.get_stGMAddHeroRsp().stHeroInfo.stCommonInfo.dwHeroID);
			}
		}

		[MessageHandler(1816)]
		public static void onGmUnlockHeroRsp(CSPkg msg)
		{
			if ((int)msg.stPkgData.get_stGMUnlockHeroPVPRsp().chResult == 0)
			{
				uint dwHeroID = msg.stPkgData.get_stGMUnlockHeroPVPRsp().dwHeroID;
				DictionaryView<uint, CHeroInfo>.Enumerator enumerator = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().GetHeroInfoDic().GetEnumerator();
				while (enumerator.MoveNext())
				{
					if (dwHeroID == 0u)
					{
						KeyValuePair<uint, CHeroInfo> current = enumerator.get_Current();
						current.get_Value().MaskBits |= 2u;
					}
					else
					{
						uint arg_78_0 = dwHeroID;
						KeyValuePair<uint, CHeroInfo> current2 = enumerator.get_Current();
						if (arg_78_0 == current2.get_Key())
						{
							KeyValuePair<uint, CHeroInfo> current3 = enumerator.get_Current();
							current3.get_Value().MaskBits |= 2u;
						}
					}
				}
			}
		}

		[MessageHandler(1802)]
		public static void onHeroInfoNty(CSPkg msg)
		{
			CHeroSelectBaseSystem.s_defaultBattleListInfo = msg.stPkgData.get_stAcntHeroInfoNty().stBattleListInfo;
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				masterRoleInfo.SetHeroInfo(msg.stPkgData.get_stAcntHeroInfoNty());
			}
			else
			{
				DebugHelper.Assert(false, "Master RoleInfo is NULL!!!");
			}
		}

		[MessageHandler(1804)]
		public static void onAddHeroNty(CSPkg msg)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			DebugHelper.Assert(masterRoleInfo != null, "onAddHeroNty role is null");
			uint dwHeroID = msg.stPkgData.get_stAddHeroNty().stHeroInfo.stCommonInfo.dwHeroID;
			if (masterRoleInfo != null)
			{
				masterRoleInfo.InitHero(msg.stPkgData.get_stAddHeroNty().stHeroInfo);
				masterRoleInfo.SetHeroSkinData(msg.stPkgData.get_stAddHeroNty().stHeroSkin);
				if (masterRoleInfo.IsValidExperienceHero(dwHeroID))
				{
					Singleton<EventRouter>.get_instance().BroadCastEvent<uint, int>("HeroExperienceAdd", dwHeroID, masterRoleInfo.GetExperienceHeroValidDays(dwHeroID));
				}
			}
			Singleton<EventRouter>.get_instance().BroadCastEvent<uint>("HeroAdd", dwHeroID);
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
		}

		[MessageHandler(1823)]
		public static void onAddHeroSkinNty(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().OnAddHeroSkin(msg.stPkgData.get_stHeroSkinAdd().dwHeroID, msg.stPkgData.get_stHeroSkinAdd().dwSkinID);
			Singleton<EventRouter>.get_instance().BroadCastEvent<uint, uint, uint>("HeroSkinAdd", msg.stPkgData.get_stHeroSkinAdd().dwHeroID, msg.stPkgData.get_stHeroSkinAdd().dwSkinID, msg.stPkgData.get_stHeroSkinAdd().dwFrom);
		}

		[MessageHandler(1829)]
		public static void onGMAddAllHeroSkinNty(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			if (msg.stPkgData.get_stGMAddAllSkillRsp().iResult == 0)
			{
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				if (masterRoleInfo != null)
				{
					masterRoleInfo.OnGmAddAllSkin();
				}
			}
		}

		[MessageHandler(1010)]
		public static void OnMasterInfoUpdate(CSPkg msg)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				masterRoleInfo.OnUpdate(msg.stPkgData.get_stNtfAcntInfoUpd());
			}
		}

		[MessageHandler(1151)]
		public static void OnMasterDianQuanUpdate(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			long llCouponsCnt = msg.stPkgData.get_stAcntCouponsRsp().llCouponsCnt;
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				masterRoleInfo.DianQuan = (ulong)llCouponsCnt;
			}
		}

		[MessageHandler(1011)]
		public static void OnMasterLevelUp(CSPkg msg)
		{
			Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo().OnLevelUp(msg.stPkgData.get_stNtfAcntLevelUp());
		}

		[MessageHandler(1015)]
		public static void OnMasterPvpLevelUp(CSPkg msg)
		{
			Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo().OnPvpLevelUp(msg.stPkgData.get_stNtfAcntPvpLevelUp());
		}

		[MessageHandler(1803)]
		public static void OnHeroExpUp(CSPkg msg)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			masterRoleInfo.SetHeroExp(msg.stPkgData.get_stHeroExpAdd().dwHeroID, (int)msg.stPkgData.get_stHeroExpAdd().wCurLevel, (int)msg.stPkgData.get_stHeroExpAdd().dwCurExp);
		}

		[MessageHandler(1810)]
		public static void OnHeroInfoUpdate(CSPkg msg)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			DebugHelper.Assert(masterRoleInfo != null, "OnHeroInfoUpdate role is null");
			if (masterRoleInfo != null)
			{
				masterRoleInfo.OnHeroInfoUpdate(msg.stPkgData.get_stHeroInfoUpdNtf());
			}
		}

		[MessageHandler(1089)]
		public static void OnMultiGameRecover(CSPkg msg)
		{
			if (!Singleton<NetworkModule>.GetInstance().gameSvr.connected && !Singleton<LobbyLogic>.get_instance().inMultiGame)
			{
				if (msg.stPkgData.get_stMultGameRecoverNtf().bCamp == 0)
				{
					Singleton<WatchController>.GetInstance().StartJudge(msg.stPkgData.get_stMultGameRecoverNtf().stRelayTGW);
				}
				else
				{
					NetworkModule.InitRelayConnnecting(msg.stPkgData.get_stMultGameRecoverNtf().stRelayTGW);
				}
			}
		}

		[MessageHandler(2603)]
		public static void OnGetRankingListRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			if (msg.stPkgData.get_stGetRankingListRsp().iErrCode == 0)
			{
				COM_APOLLO_TRANK_SCORE_TYPE bNumberType = msg.stPkgData.get_stGetRankingListRsp().bNumberType;
				if (bNumberType == 1 || bNumberType == 2 || bNumberType == 5 || bNumberType == 6 || bNumberType == 7 || bNumberType == 8 || bNumberType == 10 || bNumberType == 9 || bNumberType == 12 || bNumberType == 65 || bNumberType == 13)
				{
					Singleton<EventRouter>.GetInstance().BroadCastEvent<SCPKG_GET_RANKING_LIST_RSP>("Ranking_Get_Ranking_List", msg.stPkgData.get_stGetRankingListRsp());
				}
				else if (bNumberType == 64)
				{
					Singleton<EventRouter>.GetInstance().BroadCastEvent<SCPKG_GET_RANKING_LIST_RSP>("Ranking_Get_Ranking_Daily_RankMatch", msg.stPkgData.get_stGetRankingListRsp());
				}
				else if (bNumberType >= 33 && bNumberType <= 63)
				{
					Singleton<EventRouter>.GetInstance().BroadCastEvent<SCPKG_GET_RANKING_LIST_RSP>("UnionRank_Get_Rank_List", msg.stPkgData.get_stGetRankingListRsp());
				}
				else if (bNumberType == 3)
				{
					Singleton<EventRouter>.GetInstance().BroadCastEvent<SCPKG_GET_RANKING_LIST_RSP>("Guild_Get_Power_Ranking", msg.stPkgData.get_stGetRankingListRsp());
				}
				else if (bNumberType == 4)
				{
					Singleton<EventRouter>.GetInstance().BroadCastEvent<SCPKG_GET_RANKING_LIST_RSP, enGuildRankpointRankListType>("Guild_Get_Rankpoint_Ranking", msg.stPkgData.get_stGetRankingListRsp(), enGuildRankpointRankListType.CurrentWeek);
				}
				else if (bNumberType == 16)
				{
					Singleton<EventRouter>.GetInstance().BroadCastEvent<SCPKG_GET_RANKING_LIST_RSP, enGuildRankpointRankListType>("Guild_Get_Rankpoint_Ranking", msg.stPkgData.get_stGetRankingListRsp(), enGuildRankpointRankListType.SeasonBest);
				}
				else if (bNumberType == 22)
				{
					Singleton<EventRouter>.GetInstance().BroadCastEvent<SCPKG_GET_RANKING_LIST_RSP>(EventID.CUSTOM_EQUIP_RANK_LIST_GET, msg.stPkgData.get_stGetRankingListRsp());
				}
				else if (bNumberType == 66)
				{
					Singleton<EventRouter>.GetInstance().BroadCastEvent<SCPKG_GET_RANKING_LIST_RSP>("Guild_Get_Guild_Match_Season_Rank", msg.stPkgData.get_stGetRankingListRsp());
				}
				else if (bNumberType == 67)
				{
					Singleton<EventRouter>.GetInstance().BroadCastEvent<SCPKG_GET_RANKING_LIST_RSP>("Guild_Get_Guild_Match_Week_Rank", msg.stPkgData.get_stGetRankingListRsp());
				}
				return;
			}
			if (msg.stPkgData.get_stGetRankingListRsp().iErrCode == 1)
			{
				return;
			}
			Singleton<CUIManager>.GetInstance().OpenMessageBox(string.Format("Error Code {0}", msg.stPkgData.get_stGetRankingListRsp().iErrCode), false);
		}

		[MessageHandler(2605)]
		public static void OnGetRankingAccountInfoRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			if (msg.stPkgData.get_stGetRankingAcntInfoRsp().iErrCode != 0)
			{
				return;
			}
			Singleton<EventRouter>.GetInstance().BroadCastEvent<SCPKG_GET_RANKING_ACNT_INFO_RSP>("Ranking_Get_Ranking_Account_Info", msg.stPkgData.get_stGetRankingAcntInfoRsp());
			Singleton<EventRouter>.GetInstance().BroadCastEvent<SCPKG_GET_RANKING_ACNT_INFO_RSP>(EventID.ACHIEVE_GET_RANKING_ACCOUNT_INFO, msg.stPkgData.get_stGetRankingAcntInfoRsp());
			Singleton<EventRouter>.GetInstance().BroadCastEvent<SCPKG_GET_RANKING_ACNT_INFO_RSP>("UnionRank_Get_Rank_Account_Info", msg.stPkgData.get_stGetRankingAcntInfoRsp());
		}

		[MessageHandler(1415)]
		public static void OneGetHonorInfoRsp(CSPkg msg)
		{
			COMDT_ACNT_HONORINFO stHonorInfo = msg.stPkgData.get_stHonorInfoRsp().stHonorInfo;
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			DebugHelper.Assert(masterRoleInfo != null, "Master RoleInfo Is null!");
			if (masterRoleInfo == null)
			{
				return;
			}
			if (stHonorInfo == null || stHonorInfo.bHonorCnt < 6)
			{
				Singleton<CRoleInfoManager>.get_instance().InsertHonorOnDuplicateUpdate(ref masterRoleInfo.honorDic, 1, 0);
				Singleton<CRoleInfoManager>.get_instance().InsertHonorOnDuplicateUpdate(ref masterRoleInfo.honorDic, 2, 0);
				Singleton<CRoleInfoManager>.get_instance().InsertHonorOnDuplicateUpdate(ref masterRoleInfo.honorDic, 6, 0);
				Singleton<CRoleInfoManager>.get_instance().InsertHonorOnDuplicateUpdate(ref masterRoleInfo.honorDic, 4, 0);
				Singleton<CRoleInfoManager>.get_instance().InsertHonorOnDuplicateUpdate(ref masterRoleInfo.honorDic, 5, 0);
				Singleton<CRoleInfoManager>.get_instance().InsertHonorOnDuplicateUpdate(ref masterRoleInfo.honorDic, 3, 0);
			}
			if (stHonorInfo != null)
			{
				for (int i = 0; i < (int)stHonorInfo.bHonorCnt; i++)
				{
					COMDT_HONORINFO cOMDT_HONORINFO = stHonorInfo.astHonorInfo[i];
					switch (cOMDT_HONORINFO.iHonorID)
					{
					case 1:
					case 2:
					case 3:
					case 4:
					case 5:
					case 6:
						Singleton<CRoleInfoManager>.get_instance().InsertHonorOnDuplicateUpdate(ref masterRoleInfo.honorDic, cOMDT_HONORINFO.iHonorID, cOMDT_HONORINFO.iHonorPoint);
						break;
					}
				}
				COMDT_HONORINFO cOMDT_HONORINFO2 = new COMDT_HONORINFO();
				if (masterRoleInfo.honorDic.TryGetValue(stHonorInfo.iCurUseHonorID, ref cOMDT_HONORINFO2))
				{
					if (cOMDT_HONORINFO2.iHonorLevel > 0)
					{
						masterRoleInfo.selectedHonorID = stHonorInfo.iCurUseHonorID;
					}
					else
					{
						masterRoleInfo.selectedHonorID = 0;
					}
				}
			}
		}

		[MessageHandler(1414)]
		public static void OnUpdateHonor(CSPkg msg)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			DebugHelper.Assert(masterRoleInfo != null, "Master RoleInfo Is Null!");
			if (masterRoleInfo == null)
			{
				return;
			}
			SCPKG_HONORINFOCHG_RSP stHonorInfoChgRsp = msg.stPkgData.get_stHonorInfoChgRsp();
			Singleton<CRoleInfoManager>.get_instance().InsertHonorOnDuplicateUpdate(ref masterRoleInfo.honorDic, stHonorInfoChgRsp.iHonorID, stHonorInfoChgRsp.iHonorPoint);
			masterRoleInfo.selectedHonorID = msg.stPkgData.get_stHonorInfoChgRsp().iCurUseHonorID;
		}

		[MessageHandler(1087)]
		public static void OnRunawayRSP(CSPkg msg)
		{
			if (msg.stPkgData.get_stRunAwayRsp().bNeedDisplaySettle <= 0)
			{
				LobbyMsgHandler.HandleGameSettle(true, msg.stPkgData.get_stRunAwayRsp().bNeedDisplaySettle > 0, 2, null, null, null, null, null, null);
				Singleton<NetworkModule>.get_instance().CloseGameServerConnect(true);
			}
		}

		public static void SendMidasToken(ApolloAccountInfo accountInfo)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1019u);
			if (accountInfo == null)
			{
				return;
			}
			string text = string.Empty;
			using (ListView<ApolloToken>.Enumerator enumerator = accountInfo.get_TokenList().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ApolloToken current = enumerator.get_Current();
					if (ApolloConfig.platform == 1)
					{
						if (current.get_Type() == 1)
						{
							text = current.get_Value();
						}
					}
					else if (ApolloConfig.platform == 2 || ApolloConfig.platform == 3)
					{
						if (current.get_Type() == 3)
						{
							text = current.get_Value();
						}
					}
					else if (ApolloConfig.platform == 5 && current.get_Type() == 1)
					{
						text = current.get_Value();
					}
				}
			}
			StringHelper.StringToUTF8Bytes(text, ref cSPkg.stPkgData.get_stLoginSynRsp().szOpenKey);
			string text2 = string.Empty;
			if (accountInfo.get_Pf() == string.Empty)
			{
				text2 = "desktop_m_qq-73213123-android-73213123-qq-1104466820-BC569F700D770A26CD422F24FD675F10";
			}
			else
			{
				text2 = accountInfo.get_Pf();
			}
			StringHelper.StringToUTF8Bytes(text2, ref cSPkg.stPkgData.get_stLoginSynRsp().szPf);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<ApolloHelper>.GetInstance().InitPay();
		}

		[MessageHandler(1283)]
		public static void OnCoinGetPathRsp(CSPkg msg)
		{
			SCPKG_COINGETPATH_RSP stCoinGetPathRsp = msg.stPkgData.get_stCoinGetPathRsp();
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				masterRoleInfo.SetCoinGetCntData(stCoinGetPathRsp);
				CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CTaskSys.TASK_FORM_PATH);
				if (form != null)
				{
					Singleton<CTaskSys>.GetInstance().OnRefreshTaskView();
				}
			}
		}
	}
}
