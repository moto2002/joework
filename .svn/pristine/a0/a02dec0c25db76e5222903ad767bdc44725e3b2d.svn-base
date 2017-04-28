using Assets.Scripts.Framework;
using Assets.Scripts.UI;
using CSProtocol;
using ResData;
using System;

namespace Assets.Scripts.GameSystem
{
	[MessageHandlerClass]
	public class CFriendRecruitNetCore
	{
		public static void Send_INTIMACY_RELATION_REQUEST(ulong ulluid, uint worldID, ushort rewardID)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1370u);
			cSPkg.stPkgData.get_stRecruitmentRewardReq().stUin.ullUid = ulluid;
			cSPkg.stPkgData.get_stRecruitmentRewardReq().stUin.dwLogicWorldId = worldID;
			cSPkg.stPkgData.get_stRecruitmentRewardReq().wRecruitRewardId = rewardID;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		[MessageHandler(1371)]
		public static void On_Send_INTIMACY_RELATION_REQUEST(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			SCPKG_RECRUITMENT_REWARD_RSP stRecruitmentRewardRsp = msg.stPkgData.get_stRecruitmentRewardRsp();
			CFriendRecruit friendRecruit = Singleton<CFriendContoller>.get_instance().model.friendRecruit;
			ResRecruitmentReward cfgReward = friendRecruit.GetCfgReward(stRecruitmentRewardRsp.wRecruitRewardId);
			if (cfgReward.bRewardType == 2)
			{
				friendRecruit.SetBITS(cfgReward.bRewardBit, true);
			}
			CFriendRecruit.RecruitData recruitData = friendRecruit.GetRecruitData(stRecruitmentRewardRsp.stUin.ullUid, stRecruitmentRewardRsp.stUin.dwLogicWorldId);
			recruitData.SetReward(stRecruitmentRewardRsp.wRecruitRewardId, CFriendRecruit.RewardState.Getted);
			if (friendRecruit.SuperReward.rewardID == stRecruitmentRewardRsp.wRecruitRewardId)
			{
				friendRecruit.SuperReward.state = CFriendRecruit.RewardState.Getted;
			}
		}

		[MessageHandler(1372)]
		public static void On_RECRUITMENT_REWARD_ERR_NTF(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			SCPKG_RECRUITMENT_REWARD_ERR_NTF stRecruitmentRewardErrNtf = msg.stPkgData.get_stRecruitmentRewardErrNtf();
			string strContent = string.Empty;
			switch (stRecruitmentRewardErrNtf.dwErrorCode)
			{
			case 1u:
				strContent = Singleton<CTextManager>.get_instance().GetText("CS_RECRUITMENTWARD_NOTINTABLE");
				break;
			case 2u:
				strContent = Singleton<CTextManager>.get_instance().GetText("CS_RECRUITMENTWARD_LEVEL_NOTENOUGH");
				break;
			case 3u:
				strContent = Singleton<CTextManager>.get_instance().GetText("CS_RECRUITMENTWARD_NOTRECRUITMENT");
				break;
			case 4u:
				strContent = Singleton<CTextManager>.get_instance().GetText("CS_RECRUITMENTWARD_GETED");
				break;
			case 5u:
				strContent = Singleton<CTextManager>.get_instance().GetText("CS_RECRUITMENTWARD_OTHER");
				break;
			case 6u:
				strContent = Singleton<CTextManager>.get_instance().GetText("CS_RECRUITMENTWARD_SUPER");
				break;
			default:
				strContent = string.Format("no RECRUITMENT REWARD ERR code:{0}", stRecruitmentRewardErrNtf.dwErrorCode);
				break;
			}
			Singleton<CUIManager>.GetInstance().OpenTips(strContent, false, 1.5f, null, new object[0]);
		}

		[MessageHandler(1373)]
		public static void On_RECRUITMENT_ERR_NTF(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			SCPKG_RECRUITMENT_ERR_NTF stRecruitmentErrNtf = msg.stPkgData.get_stRecruitmentErrNtf();
			if (stRecruitmentErrNtf.dwErrorCode != 0u)
			{
				Singleton<CUIManager>.GetInstance().OpenTips(UT.ErrorCode_String(stRecruitmentErrNtf.dwErrorCode), false, 1.5f, null, new object[0]);
			}
		}

		[MessageHandler(1374)]
		public static void On_CHG_RECRUITMENT_NTF(CSPkg msg)
		{
			SCPKG_CHG_RECRUITMENT_NTF stChgRecruitmentNtf = msg.stPkgData.get_stChgRecruitmentNtf();
			CFriendRecruit friendRecruit = Singleton<CFriendContoller>.get_instance().model.friendRecruit;
			if (stChgRecruitmentNtf.bChgRecruitmentType == 1)
			{
				COMDT_FRIEND_INFO info = Singleton<CFriendContoller>.get_instance().model.GetInfo(CFriendModel.FriendType.GameFriend, stChgRecruitmentNtf.stUin.ullUid, stChgRecruitmentNtf.stUin.dwLogicWorldId);
				if (info != null)
				{
					if (stChgRecruitmentNtf.bChgRecruitmentType == 1)
					{
						friendRecruit.SetZhaoMuZhe(info);
					}
					if (stChgRecruitmentNtf.bChgRecruitmentType == 2)
					{
						friendRecruit.SetBeiZhaoMuZheRewardData(info);
					}
				}
			}
			else if (stChgRecruitmentNtf.bChgRecruitmentType == 2)
			{
				friendRecruit.RemoveRecruitData(stChgRecruitmentNtf.stUin.ullUid, stChgRecruitmentNtf.stUin.dwLogicWorldId);
			}
		}
	}
}
