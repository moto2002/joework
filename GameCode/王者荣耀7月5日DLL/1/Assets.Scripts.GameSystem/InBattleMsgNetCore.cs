using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using Assets.Scripts.UI;
using CSProtocol;
using System;

namespace Assets.Scripts.GameSystem
{
	[MessageHandlerClass]
	public class InBattleMsgNetCore
	{
		public static void SendInBattleMsg_PreConfig(uint id, COM_INBATTLE_CHAT_TYPE msgType, uint heroID)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1300u);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.bType = 7;
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.select(7L);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stInBattle().bChatType = msgType;
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stInBattle().stChatInfo.select(msgType);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stInBattle().stFrom.dwAcntHeroID = heroID;
			if (msgType == 1)
			{
				cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stInBattle().stChatInfo.get_stSignalID().dwTextID = id;
			}
			else if (msgType == 2)
			{
				cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stInBattle().stChatInfo.get_stBubbleID().dwTextID = id;
			}
			Singleton<NetworkModule>.GetInstance().SendGameMsg(ref cSPkg, 0u);
		}

		public static void SendInBattleMsg_InputChat(string content, byte camp)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1300u);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.bType = 7;
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.select(7L);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stInBattle().bChatType = 3;
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stInBattle().stChatInfo.select(3L);
			cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stInBattle().stChatInfo.get_stContentStr().bCampLimit = camp;
			StringHelper.StringToUTF8Bytes(content, ref cSPkg.stPkgData.get_stChatReq().stChatMsg.stContent.get_stInBattle().stChatInfo.get_stContentStr().szContent);
			Singleton<NetworkModule>.GetInstance().SendGameMsg(ref cSPkg, 0u);
		}

		public static void SendShortCut_Config(ListView<TabElement> list)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5224u);
			for (int i = 0; i < (int)Singleton<InBattleMsgMgr>.get_instance().totalCount; i++)
			{
				if (i < list.get_Count())
				{
					TabElement tabElement = list.get_Item(i);
					if (tabElement != null)
					{
						COMDT_SELFDEFINE_DETAIL_CHATINFO cOMDT_SELFDEFINE_DETAIL_CHATINFO = cSPkg.stPkgData.get_stSelfDefineChatInfoChgReq().stChatInfo.astChatMsg[i];
						cOMDT_SELFDEFINE_DETAIL_CHATINFO.bChatType = 1;
						cOMDT_SELFDEFINE_DETAIL_CHATINFO.stChatInfo.select(1L);
						cOMDT_SELFDEFINE_DETAIL_CHATINFO.stChatInfo.get_stSignalID().dwTextID = tabElement.cfgId;
					}
				}
			}
			cSPkg.stPkgData.get_stSelfDefineChatInfoChgReq().stChatInfo.bMsgCnt = Singleton<InBattleMsgMgr>.get_instance().totalCount;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		[MessageHandler(5225)]
		public static void OnSendShortCut_Config_Rsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			SCPKG_SELFDEFINE_CHATINFO_CHG_RSP stSelfDefineChatInfoChgRsp = msg.stPkgData.get_stSelfDefineChatInfoChgRsp();
			if (stSelfDefineChatInfoChgRsp.bResult == 0)
			{
				Singleton<CUIManager>.get_instance().OpenTips("修改成功", false, 1.5f, null, new object[0]);
			}
			else
			{
				Singleton<CUIManager>.get_instance().OpenTips("err code:" + stSelfDefineChatInfoChgRsp.bResult, false, 1.5f, null, new object[0]);
			}
		}

		[MessageHandler(5240)]
		public static void OnObserveTipsCome(CSPkg msg)
		{
			InBattleInputChat inputChat = Singleton<InBattleMsgMgr>.get_instance().m_InputChat;
			if (inputChat != null)
			{
				SCPKG_OBTIPS_NTF stOBTipsNtf = msg.stPkgData.get_stOBTipsNtf();
				COM_OBNUM_TIPS_TYPE bTipsType = stOBTipsNtf.bTipsType;
				string text = string.Empty;
				Player playerByUid = Singleton<GamePlayerCenter>.GetInstance().GetPlayerByUid(stOBTipsNtf.ullRecomUid);
				if (playerByUid != null)
				{
					text = playerByUid.Name;
				}
				InBattleInputChat.InBatChatEntity ent = inputChat.ConstructColorFlagEnt(string.Format(Singleton<CTextManager>.GetInstance().GetText(bTipsType.ToString()), stOBTipsNtf.dwOBNum, text));
				inputChat.Add(ent);
			}
		}
	}
}
