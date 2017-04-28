using Assets.Scripts.Framework;
using Assets.Scripts.UI;
using CSProtocol;
using System;
using System.Text;

namespace Assets.Scripts.GameSystem
{
	[MessageHandlerClass]
	public class FriendSysNetCore
	{
		public static void Send_Request_Sns_Switch(COM_REFUSE_TYPE type, int tag)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1322u);
			cSPkg.stPkgData.get_stRefuseRecallReq().bRefuseStatus = (byte)tag;
			cSPkg.stPkgData.get_stRefuseRecallReq().bRefuseType = type;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		[MessageHandler(1201)]
		public static void On_SC_Friend_Info(CSPkg msg)
		{
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CSPkg>("Friend_List", msg);
		}

		[MessageHandler(1203)]
		public static void On_SC_Friend_Request_Info(CSPkg msg)
		{
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CSPkg>("Friend_Request_List", msg);
		}

		[MessageHandler(1235)]
		public static void On_SC_Friend_Recommand_Info(CSPkg msg)
		{
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CSPkg>("Friend_Recommand_List", msg);
		}

		[MessageHandler(1239)]
		public static void On_SCID_CMD_BLACKLIST(CSPkg msg)
		{
			Singleton<CFriendContoller>.get_instance().On_SCID_CMD_BLACKLIST(msg);
		}

		public static void Send_Request_RecommandFriend_List(int type)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1234u);
			cSPkg.stPkgData.get_stFRecReq().bType = (byte)type;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		[MessageHandler(1219)]
		public static void On_Send_FriendPower(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			SCPKG_CMD_DONATE_FRIEND_POINT stFriendDonatePointRsp = msg.stPkgData.get_stFriendDonatePointRsp();
			if (stFriendDonatePointRsp.dwResult == 0u)
			{
				UT.Check_AddHeartCD(stFriendDonatePointRsp.stUin);
				Singleton<EventRouter>.GetInstance().BroadCastEvent("Friend_FriendList_Refresh");
				Singleton<EventRouter>.GetInstance().BroadCastEvent<SCPKG_CMD_DONATE_FRIEND_POINT>("Friend_Send_Coin_Done", stFriendDonatePointRsp);
				if (Singleton<CFriendContoller>.get_instance().model.IsSnsFriend(stFriendDonatePointRsp.stUin.ullUid, stFriendDonatePointRsp.stUin.dwLogicWorldId))
				{
					COMDT_FRIEND_INFO info = Singleton<CFriendContoller>.get_instance().model.GetInfo(CFriendModel.FriendType.SNS, stFriendDonatePointRsp.stUin);
					if (info != null)
					{
						string text = Utility.UTF8Convert(info.szOpenId);
						if (!string.IsNullOrEmpty(text) && !CFriendModel.IsOnSnsSwitch(info.dwRefuseFriendBits, 0))
						{
							string text2 = Singleton<CTextManager>.get_instance().GetText("Common_Sns_Tips_3");
							string text3 = Singleton<CTextManager>.get_instance().GetText("Common_Sns_Tips_5");
							string text4 = Singleton<CTextManager>.get_instance().GetText("Common_Sns_Tips_4");
							stUIEventParams param = default(stUIEventParams);
							param.snsFriendEventParams.openId = text;
							Singleton<CUIManager>.get_instance().OpenMessageBoxWithCancel(text2, enUIEventID.Friend_Share_SendCoin, enUIEventID.None, param, text3, text4, false);
							return;
						}
					}
				}
				Singleton<CUIManager>.GetInstance().OpenTips(UT.GetText("Friend_Tips_SendHeartOK"), false, 1.5f, null, new object[0]);
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips(UT.ErrorCode_String(stFriendDonatePointRsp.dwResult), false, 1.5f, null, new object[0]);
			}
		}

		public static void Send_FriendCoin(COMDT_ACNT_UNIQ uniq, COM_FRIEND_TYPE friendType)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1218u);
			cSPkg.stPkgData.get_stFriendDonatePointReq().stUin.ullUid = uniq.ullUid;
			cSPkg.stPkgData.get_stFriendDonatePointReq().stUin.dwLogicWorldId = uniq.dwLogicWorldId;
			cSPkg.stPkgData.get_stFriendDonatePointReq().bFriendType = friendType;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		public static void Send_Serch_Player(string name)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1204u);
			cSPkg.stPkgData.get_stFriendSearchPlayerReq().szUserName = Encoding.get_UTF8().GetBytes(name);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		[MessageHandler(1205)]
		public static void On_SC_Serch_Player(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CSPkg>("Friend_Search", msg);
		}

		public static void Send_Request_BeFriend(ulong ullUid, uint dwLogicWorldId, string veriyText = "", COM_ADD_FRIEND_TYPE addFriendType = 0, int useHeroId = -1)
		{
			if (Singleton<CFriendContoller>.GetInstance().model.IsContain(CFriendModel.FriendType.GameFriend, ullUid, dwLogicWorldId))
			{
				Singleton<CUIManager>.GetInstance().OpenTips(UT.GetText("Friend_Tips_AlreadyBeFriend"), false, 1.5f, null, new object[0]);
				return;
			}
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1206u);
			cSPkg.stPkgData.get_stFriendAddReq().stUin.ullUid = ullUid;
			cSPkg.stPkgData.get_stFriendAddReq().stUin.dwLogicWorldId = dwLogicWorldId;
			if (!string.IsNullOrEmpty(veriyText))
			{
				StringHelper.StringToUTF8Bytes(veriyText, ref cSPkg.stPkgData.get_stFriendAddReq().szVerificationInfo);
			}
			cSPkg.stPkgData.get_stFriendAddReq().stUserSource.bAddFriendType = addFriendType;
			cSPkg.stPkgData.get_stFriendAddReq().stUserSource.stAddFriendInfo.set_stPvp(new COMDT_ADDFRIEND_PVP());
			cSPkg.stPkgData.get_stFriendAddReq().stUserSource.stAddFriendInfo.get_stPvp().dwHeroID = (uint)useHeroId;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		[MessageHandler(1207)]
		public static void On_SC_BeFriend(CSPkg msg)
		{
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CSPkg>("Friend_RequestBeFriend", msg);
		}

		public static void Send_Confrim_BeFriend(COMDT_ACNT_UNIQ uniq)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1210u);
			cSPkg.stPkgData.get_stFriendAddConfirmReq().stUin.ullUid = uniq.ullUid;
			cSPkg.stPkgData.get_stFriendAddConfirmReq().stUin.dwLogicWorldId = uniq.dwLogicWorldId;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		[MessageHandler(1211)]
		public static void On_SC_Confrim_BeFriend(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CSPkg, CFriendModel.FriendType>("Friend_Confrim", msg, CFriendModel.FriendType.RequestFriend);
			Singleton<EventRouter>.GetInstance().BroadCastEvent("Friend_Game_State_Refresh");
		}

		public static void Send_DENY_BeFriend(COMDT_ACNT_UNIQ uniq)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1212u);
			cSPkg.stPkgData.get_stFriendAddDenyReq().stUin.ullUid = uniq.ullUid;
			cSPkg.stPkgData.get_stFriendAddDenyReq().stUin.dwLogicWorldId = uniq.dwLogicWorldId;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		[MessageHandler(1213)]
		public static void On_SC_DENY_BeFriend(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CSPkg>("Friend_Deny", msg);
		}

		public static void Send_Del_Friend(COMDT_FRIEND_INFO info, FriendShower.ItemType type)
		{
			if (type != FriendShower.ItemType.Mentor && type != FriendShower.ItemType.Apprentice)
			{
				CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1208u);
				cSPkg.stPkgData.get_stFriendDelReq().stUin.ullUid = info.stUin.ullUid;
				cSPkg.stPkgData.get_stFriendDelReq().stUin.dwLogicWorldId = info.stUin.dwLogicWorldId;
				Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
			}
			else
			{
				CSPkg cSPkg2 = NetworkModule.CreateDefaultCSPKG(5406u);
				cSPkg2.stPkgData.get_stRemoveMasterReq().stTargetUin.ullUid = info.stUin.ullUid;
				cSPkg2.stPkgData.get_stRemoveMasterReq().stTargetUin.dwLogicWorldId = info.stUin.dwLogicWorldId;
				cSPkg2.stPkgData.get_stRemoveMasterReq().bType = (byte)CFriendContoller.GetSrvMentorTypeByItemType(type);
				if (type != FriendShower.ItemType.Mentor)
				{
					if (type == FriendShower.ItemType.Apprentice)
					{
						cSPkg2.stPkgData.get_stRemoveMasterReq().bSubStudentType = (info.bStudentType & 15);
					}
				}
				else
				{
					cSPkg2.stPkgData.get_stRemoveMasterReq().bSubStudentType = CFriendContoller.m_mentorInfo.bStudentType;
				}
				Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg2, true);
			}
		}

		[MessageHandler(1209)]
		public static void On_SC_Del_Friend(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CSPkg>("Friend_Delete", msg);
			Singleton<EventRouter>.GetInstance().BroadCastEvent("Friend_Game_State_Refresh");
		}

		public static void Send_Invite_Friend_Game(int bReserve)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1214u);
			cSPkg.stPkgData.get_stFriendInviteGameReq().bReserve = (byte)bReserve;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		public static void On_SC_Invite_Friend_Game(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
		}

		[MessageHandler(1233)]
		public static void OnSCID_CMD_NTF_FRIEND_LOGIN_STATUS(CSPkg msg)
		{
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CSPkg>("Friend_Login_NTF", msg);
			SCPKG_CMD_NTF_FRIEND_LOGIN_STATUS stNtfFriendLoginStatus = msg.stPkgData.get_stNtfFriendLoginStatus();
			CFriendModel.FriendType friendType = (stNtfFriendLoginStatus.bFriendType != 1) ? CFriendModel.FriendType.SNS : CFriendModel.FriendType.GameFriend;
			bool flag = stNtfFriendLoginStatus.bLoginStatus == 0;
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CFriendModel.FriendType, ulong, uint, bool>("Chat_Friend_Online_Change", friendType, stNtfFriendLoginStatus.stUin.ullUid, stNtfFriendLoginStatus.stUin.dwLogicWorldId, flag);
		}

		[MessageHandler(1232)]
		public static void On_SC_NTF_FRIEND_DEL(CSPkg msg)
		{
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CSPkg>("Friend_Delete_NTF", msg);
		}

		[MessageHandler(1230)]
		public static void On_SC_NTF_FRIEND_REQUEST(CSPkg msg)
		{
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CSPkg>("Friend_Request_NTF", msg);
		}

		[MessageHandler(1231)]
		public static void On_SC_NTF_FRIEND_ADD(CSPkg msg)
		{
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CSPkg>("Friend_ADD_NTF", msg);
			Singleton<EventRouter>.GetInstance().BroadCastEvent("Friend_Game_State_Refresh");
		}

		[MessageHandler(4100)]
		public static void On_SCID_CMD_NTF_FRIEND_GAME_STATE(CSPkg msg)
		{
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CSPkg>("Friend_GAME_STATE_NTF", msg);
		}

		[MessageHandler(4101)]
		public static void On_SCID_NTF_SNS_FRIEND(CSPkg msg)
		{
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CSPkg>("Friend_SNS_STATE_NTF", msg);
		}

		[MessageHandler(4104)]
		public static void On_SCID_NTF_SNS_FRIEND_NICKNAME(CSPkg msg)
		{
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CSPkg>("Friend_SNS_NICNAME_NTF", msg);
		}

		[MessageHandler(4102)]
		public static void On_SCID_SNS_FRIEND_CHG_PROFILE(CSPkg msg)
		{
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CSPkg>("Friend_SNS_CHG_PROFILE", msg);
		}

		public static void ReCallSnsFriend(COMDT_ACNT_UNIQ uniq, COM_FRIEND_TYPE friendType)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1236u);
			cSPkg.stPkgData.get_stFriendRecallReq().stUin.ullUid = uniq.ullUid;
			cSPkg.stPkgData.get_stFriendRecallReq().stUin.dwLogicWorldId = uniq.dwLogicWorldId;
			cSPkg.stPkgData.get_stFriendRecallReq().bFriendType = friendType;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		[MessageHandler(1237)]
		public static void On_SCID_SNS_FRIEND_RECALLPOINT(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			SCPKG_CMD_RECALL_FRIEND stFriendRecallRsp = msg.stPkgData.get_stFriendRecallRsp();
			if (stFriendRecallRsp.dwResult == 0u)
			{
				UT.Check_AddReCallCD(stFriendRecallRsp.stUin, stFriendRecallRsp.bFriendType);
				Singleton<EventRouter>.GetInstance().BroadCastEvent("Friend_FriendList_Refresh");
				Singleton<CUIManager>.GetInstance().OpenTips(UT.GetText("Common_Sns_Tips_10"), false, 1.5f, null, new object[0]);
				COMDT_FRIEND_INFO friendByUid = Singleton<CFriendContoller>.get_instance().model.getFriendByUid(stFriendRecallRsp.stUin.ullUid, CFriendModel.FriendType.SNS, 0u);
				if (friendByUid != null)
				{
					Singleton<CFriendContoller>.get_instance().ShareTo_SNSFriend_ReCall(Utility.UTF8Convert(friendByUid.szOpenId));
				}
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips(UT.ErrorCode_String(stFriendRecallRsp.dwResult), false, 1.5f, null, new object[0]);
			}
		}

		[MessageHandler(1238)]
		public static void On_SCID_SNS_FRIEND_RECALL_NTF(CSPkg msg)
		{
			Singleton<CFriendContoller>.get_instance().OnReCallFriendNtf(msg);
		}

		[MessageHandler(1324)]
		public static void On_Sns_Switch_Rep(CSPkg msg)
		{
			Singleton<CUIManager>.get_instance().CloseSendMsgAlert();
			Singleton<CFriendContoller>.get_instance().OnSnsSwitchRsp(msg);
		}

		[MessageHandler(1323)]
		public static void On_Sns_Switch_Ntf(CSPkg msg)
		{
			Singleton<CFriendContoller>.get_instance().OnSnsSwitchNtf(msg);
		}

		[MessageHandler(1224)]
		public static void On_SCPKG_CMD_NTF_CHG_INTIMACY(CSPkg msg)
		{
			Singleton<CFriendContoller>.get_instance().OnChangeIntimacy(msg);
		}

		public static void Send_Block(ulong ullUid, uint dwLogicWorldId)
		{
			if (ullUid == 0uL)
			{
				return;
			}
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1325u);
			cSPkg.stPkgData.get_stDeFriendReq().stUin.ullUid = ullUid;
			cSPkg.stPkgData.get_stDeFriendReq().stUin.dwLogicWorldId = dwLogicWorldId;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		[MessageHandler(1326)]
		public static void On_SCPKG_CMD_DEFRIEND(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<CFriendContoller>.get_instance().OnSCPKG_CMD_DEFRIEND(msg);
		}

		public static void Send_Cancle_Block(ulong ullUid, uint dwLogicWorldId)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1327u);
			cSPkg.stPkgData.get_stCancelDeFriendReq().stUin.ullUid = ullUid;
			cSPkg.stPkgData.get_stCancelDeFriendReq().stUin.dwLogicWorldId = dwLogicWorldId;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		[MessageHandler(1328)]
		public static void On_SCPKG_CMD_CANCEL_DEFRIEND(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<CFriendContoller>.get_instance().OnSCPKG_CMD_CANCEL_DEFRIEND(msg);
		}

		public static void Send_Report_Clt_Location(int longitude, int latitude)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1329u);
			cSPkg.stPkgData.get_stLbsReportReq().iLongitude = longitude;
			cSPkg.stPkgData.get_stLbsReportReq().iLatitude = latitude;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		public static void Send_Search_LBS_Req(byte gender, int longitude, int latitude, bool isShowAlert = true)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1330u);
			cSPkg.stPkgData.get_stLbsSearchReq().bGender = gender;
			cSPkg.stPkgData.get_stLbsSearchReq().iLongitude = longitude;
			cSPkg.stPkgData.get_stLbsSearchReq().iLatitude = latitude;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, isShowAlert);
		}

		[MessageHandler(1331)]
		public static void On_SC_Search_LBS_Rsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			SCPKG_CMD_LBS_SEARCH stLbsSearchRsq = msg.stPkgData.get_stLbsSearchRsq();
			if (stLbsSearchRsq.dwResult == 0u)
			{
				Singleton<CFriendContoller>.get_instance().model.ClearLBSData();
				int num = 0;
				while ((long)num < (long)((ulong)stLbsSearchRsq.dwLbsListNum))
				{
					CSDT_LBS_USER_INFO cSDT_LBS_USER_INFO = stLbsSearchRsq.astLbsList[num];
					if (cSDT_LBS_USER_INFO != null)
					{
						if (cSDT_LBS_USER_INFO.stLbsUserInfo.stUin.ullUid != Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID)
						{
							Singleton<CFriendContoller>.get_instance().model.RemoveLBSUser(cSDT_LBS_USER_INFO.stLbsUserInfo.stUin.ullUid, cSDT_LBS_USER_INFO.stLbsUserInfo.stUin.dwLogicWorldId);
							Singleton<CFriendContoller>.get_instance().model.AddLBSUser(cSDT_LBS_USER_INFO);
						}
					}
					num++;
				}
				if (stLbsSearchRsq.dwLbsListNum == 0u)
				{
					string text = Singleton<CTextManager>.get_instance().GetText("LBS_Location_Error");
					Singleton<CFriendContoller>.get_instance().model.searchLBSZero = text;
					if (Singleton<CFriendContoller>.get_instance().view != null && Singleton<CFriendContoller>.get_instance().view.ifnoText != null)
					{
						Singleton<CFriendContoller>.get_instance().view.ifnoText.text = text;
					}
				}
				else
				{
					Singleton<CFriendContoller>.get_instance().model.searchLBSZero = string.Empty;
				}
				Singleton<CFriendContoller>.get_instance().model.SortLBSFriend();
				Singleton<EventRouter>.GetInstance().BroadCastEvent("Friend_LBS_User_Refresh");
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips(UT.ErrorCode_String(stLbsSearchRsq.dwResult), false, 1.5f, null, new object[0]);
			}
		}

		public static void Send_Clear_Location()
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1332u);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		[MessageHandler(5408)]
		public static void OnMentor_RequestList(CSPkg msg)
		{
			Singleton<CFriendContoller>.GetInstance().OnMentor_RequestList(msg);
		}

		[MessageHandler(5409)]
		public static void OnMentor_RequestNTF(CSPkg msg)
		{
			Singleton<CFriendContoller>.GetInstance().OnMentor_Reqest_NTF(msg);
		}

		[MessageHandler(5410)]
		public static void OnMasterStudentInfo(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<CFriendContoller>.get_instance().OnMasterStudentInfo(msg);
		}

		[MessageHandler(5411)]
		public static void OnMasterStudentAdd(CSPkg msg)
		{
			Singleton<CFriendContoller>.get_instance().OnMentorInfoAdd(msg);
		}

		[MessageHandler(5412)]
		public static void OnMasterStudentDel(CSPkg msg)
		{
			Singleton<CFriendContoller>.get_instance().OnMentorInfoRemove(msg);
		}

		[MessageHandler(5403)]
		public static void OnApplyMasterReq(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			if (msg.stPkgData.get_stApplyMasterRsp().iResult == 0)
			{
				Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("Mentor_RequestSent"), false, 1.5f, null, new object[0]);
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips(Utility.ProtErrCodeToStr(5403, msg.stPkgData.get_stApplyMasterRsp().iResult), false, 1.5f, null, new object[0]);
			}
		}

		[MessageHandler(5417)]
		public static void OnMasterStudentLoginStatusNtf(CSPkg msg)
		{
			Singleton<CFriendContoller>.GetInstance().On_Mentor_Login_NTF(msg);
			SCPKG_CMD_NTF_MASTERSTUDENT_LOGIN_STATUS stMasterStudentLoginNtf = msg.stPkgData.get_stMasterStudentLoginNtf();
			byte bType = stMasterStudentLoginNtf.bType;
			CFriendModel.FriendType friendType;
			if (bType != 5)
			{
				if (bType != 6)
				{
					return;
				}
				friendType = CFriendModel.FriendType.Mentor;
			}
			else
			{
				friendType = CFriendModel.FriendType.Apprentice;
			}
			bool flag = stMasterStudentLoginNtf.bLoginStatus == 0;
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CFriendModel.FriendType, ulong, uint, bool>("Chat_Friend_Online_Change", friendType, stMasterStudentLoginNtf.stUin.ullUid, stMasterStudentLoginNtf.stUin.dwLogicWorldId, flag);
		}

		[MessageHandler(5405)]
		public static void OnConfirmMasterRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CSPkg, CFriendModel.FriendType>("Friend_Confrim", msg, CFriendModel.FriendType.MentorRequestList);
			if (msg.stPkgData.get_stConfirmMasterRsp().iResult == 0)
			{
				if (msg.stPkgData.get_stConfirmMasterRsp().bConfirmType == 1)
				{
					Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("Mentor_ConfirmRequest"), false, 1.5f, null, new object[0]);
				}
				else
				{
					Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("Mentor_RefuseRequest"), false, 1.5f, null, new object[0]);
				}
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips(Utility.ProtErrCodeToStr(5405, msg.stPkgData.get_stConfirmMasterRsp().iResult), false, 1.5f, null, new object[0]);
			}
		}

		[MessageHandler(5407)]
		public static void OnRemoveMasterRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			if (msg.stPkgData.get_stRemoveMasterRsp().iResult == 0)
			{
				Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("Mentor_RemovedObject"), false, 1.5f, null, new object[0]);
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips(Utility.ProtErrCodeToStr(5407, msg.stPkgData.get_stRemoveMasterRsp().iResult), false, 1.5f, null, new object[0]);
			}
		}

		[MessageHandler(5416)]
		public static void OnGetStudentListRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<CFriendContoller>.GetInstance().OnMentor_GetStudentList(msg);
		}

		public static void SendGetStudentListReq(CS_STUDENTLIST_TYPE type)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5415u);
			cSPkg.stPkgData.get_stGetStudentListReq().bListType = type;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		[MessageHandler(5414)]
		public static void OnGetMasterAcntDataNtf(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<CFriendContoller>.GetInstance().OnMentor_GetAccountData(msg);
		}

		[MessageHandler(5413)]
		public static void OnGraduateNtf(CSPkg msg)
		{
			Singleton<CFriendContoller>.GetInstance().OnMentor_GraduateNtf(msg);
		}
	}
}
