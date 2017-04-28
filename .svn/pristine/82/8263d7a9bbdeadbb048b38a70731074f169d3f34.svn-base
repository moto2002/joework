using Assets.Scripts.Framework;
using Assets.Scripts.UI;
using CSProtocol;
using System;

namespace Assets.Scripts.GameSystem
{
	[MessageHandlerClass]
	public class FriendRelationNetCore
	{
		public static void Send_INTIMACY_RELATION_REQUEST(ulong ulluid, uint worldID, COM_INTIMACY_STATE state, COM_INTIMACY_RELATION_CHG_TYPE chgType)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1360u);
			cSPkg.stPkgData.get_stIntimacyRelationRequestReq().stUin.ullUid = ulluid;
			cSPkg.stPkgData.get_stIntimacyRelationRequestReq().stUin.dwLogicWorldId = worldID;
			cSPkg.stPkgData.get_stIntimacyRelationRequestReq().bIntimacyState = state;
			cSPkg.stPkgData.get_stIntimacyRelationRequestReq().bRelationChgType = chgType;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		[MessageHandler(1361)]
		public static void On_Send_INTIMACY_RELATION_REQUEST(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			SCPKG_CMD_INTIMACY_RELATION_REQUEST stIntimacyRelationRequestRsp = msg.stPkgData.get_stIntimacyRelationRequestRsp();
			if (stIntimacyRelationRequestRsp.dwResult == 0u)
			{
				string strContent = string.Empty;
				COM_INTIMACY_STATE state = 0;
				if (stIntimacyRelationRequestRsp.bRelationChgType == 1)
				{
					if (stIntimacyRelationRequestRsp.bIntimacyState == 1)
					{
						state = 20;
						strContent = UT.FRData().IntimRela_Tips_SendRequestGaySuccess;
					}
					else if (stIntimacyRelationRequestRsp.bIntimacyState == 2)
					{
						state = 22;
						strContent = UT.FRData().IntimRela_Tips_SendRequestLoverSuccess;
					}
				}
				else if (stIntimacyRelationRequestRsp.bRelationChgType == 2)
				{
					if (stIntimacyRelationRequestRsp.bIntimacyState == 1)
					{
						state = 21;
						strContent = UT.FRData().IntimRela_Tips_SendDelGaySuccess;
					}
					else if (stIntimacyRelationRequestRsp.bIntimacyState == 2)
					{
						state = 23;
						strContent = UT.FRData().IntimRela_Tips_SendDelLoverSuccess;
					}
				}
				Singleton<CUIManager>.GetInstance().OpenTips(strContent, false, 1.5f, null, new object[0]);
				CFriendRelationship.FRData.Add(stIntimacyRelationRequestRsp.stUin.ullUid, stIntimacyRelationRequestRsp.stUin.dwLogicWorldId, state, stIntimacyRelationRequestRsp.bRelationChgType, 0u, false);
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips(UT.ErrorCode_String(stIntimacyRelationRequestRsp.dwResult), false, 1.5f, null, new object[0]);
			}
		}

		[MessageHandler(1362)]
		public static void On_NTF_INTIMACY_RELATION_REQUEST(CSPkg msg)
		{
			SCPKG_CMD_NTF_INTIMACY_RELATION_REQUEST stNtfIntimacyRelationRequest = msg.stPkgData.get_stNtfIntimacyRelationRequest();
			if (stNtfIntimacyRelationRequest == null)
			{
				return;
			}
			COM_INTIMACY_STATE state = 0;
			if (stNtfIntimacyRelationRequest.bRelationChgType == 1)
			{
				if (stNtfIntimacyRelationRequest.bIntimacyState == 1)
				{
					state = 20;
				}
				else if (stNtfIntimacyRelationRequest.bIntimacyState == 2)
				{
					state = 22;
				}
			}
			else if (stNtfIntimacyRelationRequest.bRelationChgType == 2)
			{
				if (stNtfIntimacyRelationRequest.bIntimacyState == 1)
				{
					state = 21;
				}
				else if (stNtfIntimacyRelationRequest.bIntimacyState == 2)
				{
					state = 23;
				}
			}
			CFriendRelationship.FRData.Add(stNtfIntimacyRelationRequest.stUin.ullUid, stNtfIntimacyRelationRequest.stUin.dwLogicWorldId, state, stNtfIntimacyRelationRequest.bRelationChgType, 0u, true);
		}

		public static void Send_CHG_INTIMACY_CONFIRM(ulong ulluid, uint worldID, COM_INTIMACY_STATE value, COM_INTIMACY_RELATION_CHG_TYPE type)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1363u);
			cSPkg.stPkgData.get_stChgIntimacyConfirmReq().stUin.ullUid = ulluid;
			cSPkg.stPkgData.get_stChgIntimacyConfirmReq().stUin.dwLogicWorldId = worldID;
			cSPkg.stPkgData.get_stChgIntimacyConfirmReq().bRelationChgType = type;
			cSPkg.stPkgData.get_stChgIntimacyConfirmReq().bIntimacyState = value;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		[MessageHandler(1364)]
		public static void On_Send_CHG_INTIMACY_CONFIRM(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			SCPKG_CMD_CHG_INTIMACY_CONFIRM stChgIntimacyConfirmRsp = msg.stPkgData.get_stChgIntimacyConfirmRsp();
			if (stChgIntimacyConfirmRsp.dwResult == 0u)
			{
				if (stChgIntimacyConfirmRsp.bRelationChgType == 1)
				{
					if (stChgIntimacyConfirmRsp.bIntimacyState == 1)
					{
						CFriendRelationship.FRData.Add(stChgIntimacyConfirmRsp.stUin.ullUid, stChgIntimacyConfirmRsp.stUin.dwLogicWorldId, 1, stChgIntimacyConfirmRsp.bRelationChgType, 0u, false);
					}
					if (stChgIntimacyConfirmRsp.bIntimacyState == 2)
					{
						CFriendRelationship.FRData.Add(stChgIntimacyConfirmRsp.stUin.ullUid, stChgIntimacyConfirmRsp.stUin.dwLogicWorldId, 2, stChgIntimacyConfirmRsp.bRelationChgType, 0u, false);
					}
				}
				if (stChgIntimacyConfirmRsp.bRelationChgType == 2)
				{
					if (stChgIntimacyConfirmRsp.bIntimacyState == 1)
					{
						CFriendRelationship.FRData.Add(stChgIntimacyConfirmRsp.stUin.ullUid, stChgIntimacyConfirmRsp.stUin.dwLogicWorldId, 24, stChgIntimacyConfirmRsp.bRelationChgType, stChgIntimacyConfirmRsp.dwTerminateTime, false);
					}
					if (stChgIntimacyConfirmRsp.bIntimacyState == 2)
					{
						CFriendRelationship.FRData.Add(stChgIntimacyConfirmRsp.stUin.ullUid, stChgIntimacyConfirmRsp.stUin.dwLogicWorldId, 24, stChgIntimacyConfirmRsp.bRelationChgType, stChgIntimacyConfirmRsp.dwTerminateTime, false);
					}
				}
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips(UT.ErrorCode_String(stChgIntimacyConfirmRsp.dwResult), false, 1.5f, null, new object[0]);
			}
		}

		[MessageHandler(1365)]
		public static void On_NTF_CHG_INTIMACY_CONFIRM(CSPkg msg)
		{
			SCPKG_CMD_NTF_CHG_INTIMACY_CONFIRM stNtfChgIntimacyConfirm = msg.stPkgData.get_stNtfChgIntimacyConfirm();
			if (stNtfChgIntimacyConfirm == null)
			{
				return;
			}
			if (stNtfChgIntimacyConfirm.bRelationChgType == 1)
			{
				if (stNtfChgIntimacyConfirm.bIntimacyState == 1)
				{
					CFriendRelationship.FRData.Add(stNtfChgIntimacyConfirm.stUin.ullUid, stNtfChgIntimacyConfirm.stUin.dwLogicWorldId, 1, stNtfChgIntimacyConfirm.bRelationChgType, 0u, false);
				}
				if (stNtfChgIntimacyConfirm.bIntimacyState == 2)
				{
					CFriendRelationship.FRData.Add(stNtfChgIntimacyConfirm.stUin.ullUid, stNtfChgIntimacyConfirm.stUin.dwLogicWorldId, 2, stNtfChgIntimacyConfirm.bRelationChgType, 0u, false);
				}
			}
			if (stNtfChgIntimacyConfirm.bRelationChgType == 2)
			{
				if (stNtfChgIntimacyConfirm.bIntimacyState == 1)
				{
					CFriendRelationship.FRData.Add(stNtfChgIntimacyConfirm.stUin.ullUid, stNtfChgIntimacyConfirm.stUin.dwLogicWorldId, 24, stNtfChgIntimacyConfirm.bRelationChgType, stNtfChgIntimacyConfirm.dwTerminateTime, false);
				}
				if (stNtfChgIntimacyConfirm.bIntimacyState == 2)
				{
					CFriendRelationship.FRData.Add(stNtfChgIntimacyConfirm.stUin.ullUid, stNtfChgIntimacyConfirm.stUin.dwLogicWorldId, 24, stNtfChgIntimacyConfirm.bRelationChgType, stNtfChgIntimacyConfirm.dwTerminateTime, false);
				}
			}
		}

		public static void Send_CHG_INTIMACY_DENY(ulong ulluid, uint worldID, COM_INTIMACY_STATE state, COM_INTIMACY_RELATION_CHG_TYPE type)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1366u);
			cSPkg.stPkgData.get_stChgIntimacyDenyReq().stUin.ullUid = ulluid;
			cSPkg.stPkgData.get_stChgIntimacyDenyReq().stUin.dwLogicWorldId = worldID;
			cSPkg.stPkgData.get_stChgIntimacyDenyReq().bRelationChgType = type;
			cSPkg.stPkgData.get_stChgIntimacyDenyReq().bIntimacyState = state;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		[MessageHandler(1367)]
		public static void On_Send_CHG_INTIMACY_DENY(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			SCPKG_CMD_CHG_INTIMACY_DENY stChgIntimacyDenyRsp = msg.stPkgData.get_stChgIntimacyDenyRsp();
			if (stChgIntimacyDenyRsp.dwResult != 0u)
			{
				Singleton<CUIManager>.GetInstance().OpenTips(UT.ErrorCode_String(stChgIntimacyDenyRsp.dwResult), false, 1.5f, null, new object[0]);
			}
		}

		[MessageHandler(1368)]
		public static void On_NTF_CHG_INTIMACY_DENY(CSPkg msg)
		{
			SCPKG_CMD_NTF_CHG_INTIMACY_DENY stNtfChgIntimacyDeny = msg.stPkgData.get_stNtfChgIntimacyDeny();
			if (stNtfChgIntimacyDeny == null)
			{
				return;
			}
			COMDT_FRIEND_INFO gameOrSnsFriend = Singleton<CFriendContoller>.get_instance().model.GetGameOrSnsFriend(stNtfChgIntimacyDeny.stUin.ullUid, stNtfChgIntimacyDeny.stUin.dwLogicWorldId);
			string strContent = string.Empty;
			if (stNtfChgIntimacyDeny.bRelationChgType == 1)
			{
				if (stNtfChgIntimacyDeny.bIntimacyState == 1)
				{
					if (gameOrSnsFriend != null)
					{
						strContent = string.Format(UT.FRData().IntimRela_Tips_DenyYourRequestGay, UT.Bytes2String(gameOrSnsFriend.szUserName));
					}
					CFriendRelationship.FRData.Add(stNtfChgIntimacyDeny.stUin.ullUid, stNtfChgIntimacyDeny.stUin.dwLogicWorldId, 24, 0, 0u, false);
				}
				if (stNtfChgIntimacyDeny.bIntimacyState == 2)
				{
					if (gameOrSnsFriend != null)
					{
						strContent = string.Format(UT.FRData().IntimRela_Tips_DenyYourRequestLover, UT.Bytes2String(gameOrSnsFriend.szUserName));
					}
					CFriendRelationship.FRData.Add(stNtfChgIntimacyDeny.stUin.ullUid, stNtfChgIntimacyDeny.stUin.dwLogicWorldId, 24, 0, 0u, false);
				}
			}
			if (stNtfChgIntimacyDeny.bRelationChgType == 2)
			{
				if (stNtfChgIntimacyDeny.bIntimacyState == 1)
				{
					if (gameOrSnsFriend != null)
					{
						strContent = string.Format(UT.FRData().IntimRela_Tips_DenyYourDelGay, UT.Bytes2String(gameOrSnsFriend.szUserName));
					}
					CFriendRelationship.FRData.Add(stNtfChgIntimacyDeny.stUin.ullUid, stNtfChgIntimacyDeny.stUin.dwLogicWorldId, 1, 0, 0u, false);
				}
				if (stNtfChgIntimacyDeny.bIntimacyState == 2)
				{
					if (gameOrSnsFriend != null)
					{
						strContent = string.Format(UT.FRData().IntimRela_Tips_DenyYourDelLover, UT.Bytes2String(gameOrSnsFriend.szUserName));
					}
					CFriendRelationship.FRData.Add(stNtfChgIntimacyDeny.stUin.ullUid, stNtfChgIntimacyDeny.stUin.dwLogicWorldId, 2, 0, 0u, false);
				}
			}
			Singleton<CUIManager>.GetInstance().OpenTips(strContent, false, 1.5f, null, new object[0]);
		}

		public static void Send_CHG_INTIMACYPRIORITY(COM_INTIMACY_STATE type)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1369u);
			cSPkg.stPkgData.get_stChgIntimacyPriorityReq().bIntimacyState = type;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
			Singleton<CFriendContoller>.GetInstance().model.FRData.SetFirstChoiseState(type);
		}
	}
}
