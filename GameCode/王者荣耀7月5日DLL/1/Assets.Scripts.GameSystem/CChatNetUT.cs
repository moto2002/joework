using Assets.Scripts.Framework;
using CSProtocol;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameSystem
{
	[MessageHandlerClass]
	public class CChatNetUT
	{
		public static void Send_Private_Chat(ulong ullUid, uint logicWorldId, string text)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1300u);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.bType = 2;
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.select(2L);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stPrivate().stTo.ullUid = ullUid;
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stPrivate().stTo.iLogicWorldID = (int)logicWorldId;
			COMDT_FRIEND_INFO gameOrSnsFriend = Singleton<CFriendContoller>.get_instance().model.GetGameOrSnsFriend(ullUid, logicWorldId);
			if (gameOrSnsFriend != null)
			{
				cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stPrivate().stTo.szName = gameOrSnsFriend.szUserName;
				cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stPrivate().stTo.dwLevel = gameOrSnsFriend.dwLevel;
			}
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stPrivate().szContent = UT.String2Bytes(text);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
			Debug.Log(string.Concat(new object[]
			{
				"--- send private chat, id:",
				ullUid,
				",logicworldid:",
				logicWorldId,
				",content:",
				text
			}));
		}

		public static void Send_Lobby_Chat(string text)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1300u);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.bType = 1;
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.select(1L);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stLogicWord().szContent = UT.String2Bytes(text);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
			Singleton<CChatController>.GetInstance().model.channelMgr.GetChannel(EChatChannel.Lobby).Start_InputCD();
		}

		public static void Send_Guild_Chat(string text)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1300u);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.bType = 4;
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.select(4L);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stGuild().szContent = UT.String2Bytes(text);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		public static void Send_GuildMatchTeam_Chat(string text)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1300u);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.bType = 11;
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.select(11L);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stGuildTeam().szContent = UT.String2Bytes(text);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		public static void Send_Room_Chat(string text)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1300u);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.bType = 3;
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.select(3L);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stRoom().szContent = UT.String2Bytes(text);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		public static void Send_SelectHero_Chat(uint template_id)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1300u);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.bType = 5;
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.select(5L);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stBattle().bChatType = 1;
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stBattle().stChatInfo.select(1L);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stBattle().stChatInfo.get_stContentID().dwTextID = template_id;
			Singleton<NetworkModule>.GetInstance().SendGameMsg(ref cSPkg, 0u);
		}

		public static void Send_SelectHero_Chat(string text)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1300u);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.bType = 5;
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.select(5L);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stBattle().bChatType = 2;
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stBattle().stChatInfo.select(2L);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stBattle().stChatInfo.get_stContentStr().szContent = UT.String2Bytes(text);
			Singleton<NetworkModule>.GetInstance().SendGameMsg(ref cSPkg, 0u);
		}

		public static void Send_Team_Chat(string text)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1300u);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.bType = 6;
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.select(6L);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stTeam().szContent = UT.String2Bytes(text);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		public static void Send_Settle_Chat(string text)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1300u);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.bType = 8;
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.select(8L);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stSettle().szContent = UT.String2Bytes(text);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		public static void Send_Leave_Settle()
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5232u);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		public static void Send_GetChat_Req(EChatChannel channel)
		{
			if (!Singleton<NetworkModule>.GetInstance().lobbySvr.connected)
			{
				return;
			}
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1301u);
			cSPkg.stPkgData.get_stGetChatMsgReq().bChatType = CChatUT.Convert_Channel_ChatMsgType(channel);
			if (channel == EChatChannel.Lobby)
			{
				if (Singleton<CChatController>.GetInstance().model.sysData.lastTimeStamp != 0u)
				{
					cSPkg.stPkgData.get_stGetChatMsgReq().dwLastTimeStamp = Singleton<CChatController>.GetInstance().model.sysData.lastTimeStamp;
					Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
				}
			}
			else
			{
				Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
			}
		}

		[MessageHandler(1302)]
		public static void On_Chat_NTF(CSPkg msg)
		{
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CSPkg>("On_Chat_GetMsg_NTF", msg);
		}

		[MessageHandler(1307)]
		public static void On_Offline_Chat_NTF(CSPkg msg)
		{
			Debug.Log("---Chat On_Offline_Chat_NTF...");
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CSPkg>("Chat_Offline_GetMsg_NTF", msg);
		}

		public static void Send_Clear_Offline(List<int> delIndexList)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1308u);
			cSPkg.stPkgData.get_stCleanOfflineChatReq().bDelChatCnt = (byte)delIndexList.get_Count();
			for (int i = 0; i < delIndexList.get_Count(); i++)
			{
				cSPkg.stPkgData.get_stCleanOfflineChatReq().ChatIndex[i] = delIndexList.get_Item(i);
			}
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
			Debug.Log("---Chat Send_Clear_Offline Req, count:" + delIndexList.get_Count());
		}

		[MessageHandler(1309)]
		public static void ReceivePlayerLeaveSettlementRoomNtf(CSPkg msg)
		{
			Singleton<EventRouter>.GetInstance().BroadCastEvent<CSPkg>("Chat_PlayerLeaveSettle_Ntf", msg);
		}

		public static void RequestGetGuildRecruitReq(uint startTime)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(2289u);
			cSPkg.stPkgData.get_stGetGuildRecruitReq().dwStartTime = startTime;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		[MessageHandler(2290)]
		public static void ReceiveGetGuildRecruitRsp(CSPkg msg)
		{
			Singleton<CChatController>.GetInstance().RefreshGuildRecruitInfo(msg.stPkgData.get_stGetGuildRecruitRsp());
		}
	}
}
