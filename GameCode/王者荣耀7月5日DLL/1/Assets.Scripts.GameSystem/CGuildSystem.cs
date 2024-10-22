using Assets.Scripts.Framework;
using Assets.Scripts.UI;
using CSProtocol;
using ResData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameSystem
{
	[MessageHandlerClass]
	public class CGuildSystem : Singleton<CGuildSystem>
	{
		public enum enGuildSearchSrc
		{
			Normal,
			From_Mail_Hyper_Link,
			From_Chat_Hyper_Link
		}

		public const byte PREPAREGUILD_FRIEND_LIMIT = 10;

		public const int MaxGuildLevel = 19;

		public const int NumberNotInRank = 0;

		public const int GuildRankItemCountPerPage = 20;

		public const int GuildNameCharacterUpperLimit = 7;

		public const int SpecialGuildIconNum = 3;

		private CGuildModel m_Model;

		private CGuildListController m_listController;

		private CGuildInfoController m_infoController;

		public static uint s_showCoinProfitTipMaxLevel = 10u;

		public static uint[] s_coinProfitPercentage;

		public static uint s_rankpointProfitMax = 200u;

		public static uint s_lastByGameRankpoint;

		public static bool s_isApplyAndRecommendListEmpty = true;

		public static bool s_isGuildMaxGrade;

		public static bool s_isGuildHighestMatchScore;

		public CGuildModel Model
		{
			get
			{
				return this.m_Model;
			}
		}

		public static bool IsError(byte bResult)
		{
			switch (bResult)
			{
			case 0:
				return false;
			case 1:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_NAME_DUP", true, 1.5f, null, new object[0]);
				return true;
			case 2:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_DB", true, 1.5f, null, new object[0]);
				return true;
			case 3:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_CREATE_QUEUE_FULL", true, 1.5f, null, new object[0]);
				return true;
			case 4:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_NO_PREPARE_GUILD", true, 1.5f, null, new object[0]);
				return true;
			case 5:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_MEMBER_FULL", true, 1.5f, null, new object[0]);
				return true;
			case 6:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_HAS_INVITED", true, 1.5f, null, new object[0]);
				return true;
			case 7:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_HAS_RECOMMEND", true, 1.5f, null, new object[0]);
				return true;
			case 8:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_HAS_GUILD", true, 1.5f, null, new object[0]);
				return true;
			case 9:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_GUILD_NOT_EXIST", true, 1.5f, null, new object[0]);
				return true;
			case 10:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_LEVEL_LIMIT", true, 1.5f, null, new object[0]);
				return true;
			case 11:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_INVITED_EXPIRE", true, 1.5f, null, new object[0]);
				return true;
			case 12:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_UPGRADE", true, 1.5f, null, new object[0]);
				return true;
			case 13:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_FIRE_CNT_LIMIT", true, 1.5f, null, new object[0]);
				return true;
			case 14:
				Singleton<CUIManager>.GetInstance().OpenTips("Guild_Err_Deal_Self_Recommend_Fail", true, 1.5f, null, new object[0]);
				return true;
			case 15:
				Singleton<CUIManager>.GetInstance().OpenTips("Guild_Err_Donate_Count_Limit", true, 1.5f, null, new object[0]);
				return true;
			case 16:
				Singleton<CUIManager>.GetInstance().OpenTips("Guild_Err_Duplicate_Has_Got_Dividend", true, 1.5f, null, new object[0]);
				return true;
			case 17:
				Singleton<CUIManager>.GetInstance().OpenTips("Guild_Name_Contain_Invalid_Character", true, 1.5f, null, new object[0]);
				return true;
			case 18:
				Singleton<CUIManager>.GetInstance().OpenTips("Guild_Bulletin_Contain_Invalid_Character", true, 1.5f, null, new object[0]);
				return true;
			case 19:
				Singleton<CUIManager>.GetInstance().OpenTips("Guild_Symbol_Err_Factory_Level_Limit", true, 1.5f, null, new object[0]);
				return true;
			case 20:
				Singleton<CUIManager>.GetInstance().OpenTips("Guild_Symbol_Err_Construct_Not_Enough", true, 1.5f, null, new object[0]);
				return true;
			case 21:
				Singleton<CUIManager>.GetInstance().OpenTips("Guild_Symbol_Err_History_Construct_Not_Enough", true, 1.5f, null, new object[0]);
				return true;
			case 22:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_UPGRADE_GUILD_FAIL", true, 1.5f, null, new object[0]);
				return true;
			case 23:
				Singleton<CUIManager>.GetInstance().OpenTips("Guild_Signed", true, 1.5f, null, new object[0]);
				return true;
			case 24:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_NOT_SAME_LOGICWORLD", true, 1.5f, null, new object[0]);
				return true;
			case 25:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_MATCH_OTHER", true, 1.5f, null, new object[0]);
				return true;
			case 26:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_MATCH_MEMBER_CNT", true, 1.5f, null, new object[0]);
				return true;
			case 27:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_MATCH_MEMBER_NOT_READY", true, 1.5f, null, new object[0]);
				return true;
			case 28:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_MATCH_MEMBER_VERSION", true, 1.5f, null, new object[0]);
				return true;
			case 29:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_MATCH_CNT_LIMIT", true, 1.5f, null, new object[0]);
				return true;
			case 31:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_MATCH_TEAM_FULL", true, 1.5f, null, new object[0]);
				return true;
			case 32:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_MATCH_GRADE_LIMIT", true, 1.5f, null, new object[0]);
				return true;
			case 33:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_GUILD_MATCHING", true, 1.5f, null, new object[0]);
				return true;
			case 34:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_JOIN_TIME_LIMIT", true, 1.5f, null, new object[0]);
				return true;
			case 35:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_TEAM_LEADER_FULL", true, 1.5f, null, new object[0]);
				return true;
			case 36:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_SEND_GUILD_RECRUIT_CD", true, 1.5f, null, new object[0]);
				return true;
			case 37:
				Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_APPLY_EXIST", true, 1.5f, null, new object[0]);
				return true;
			}
			Singleton<CUIManager>.GetInstance().OpenTips("GUILD_ERR_UNKNOWN", true, 1.5f, null, new object[0]);
			return true;
		}

		public override void Init()
		{
			base.Init();
			this.m_Model = Singleton<CGuildModel>.GetInstance();
			this.m_listController = Singleton<CGuildListController>.GetInstance();
			this.m_infoController = Singleton<CGuildInfoController>.GetInstance();
			this.InitSomeDatabin();
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_HyperLink_Click, new CUIEventManager.OnUIEventHandler(this.On_OpenForm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_OpenForm, new CUIEventManager.OnUIEventHandler(this.On_OpenForm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_CloseForm, new CUIEventManager.OnUIEventHandler(this.On_CloseForm));
			Singleton<EventRouter>.GetInstance().AddEventHandler<SCPKG_GET_RANKING_LIST_RSP>("Guild_Get_Power_Ranking", new Action<SCPKG_GET_RANKING_LIST_RSP>(this.OnGetPowerRanking));
			Singleton<EventRouter>.GetInstance().AddEventHandler<SCPKG_GET_RANKING_LIST_RSP, enGuildRankpointRankListType>("Guild_Get_Rankpoint_Ranking", new Action<SCPKG_GET_RANKING_LIST_RSP, enGuildRankpointRankListType>(this.OnGetRankpointRanking));
			Singleton<EventRouter>.GetInstance().AddEventHandler(EventID.LOBBY_STATE_LEAVE, new Action(this.OnLobbyStateLeave));
			Singleton<EventRouter>.GetInstance().AddEventHandler(EventID.NAMECHANGE_PLAYER_NAME_CHANGE, new Action(this.OnPlayerNameChange));
		}

		public void Clear()
		{
			if (this.m_Model != null)
			{
				for (int i = 0; i < 4; i++)
				{
					this.m_Model.RankpointRankLastGottenTimes[i] = 0;
					this.m_Model.RankpointRankGottens[i] = false;
				}
				this.m_Model.IsLocalDataInited = false;
				this.m_Model.ClearAppliedGuildDic();
				this.m_Model.ClearInvitedFriendDic();
			}
		}

		private void InitSomeDatabin()
		{
			CGuildSystem.s_showCoinProfitTipMaxLevel = GameDataMgr.guildMiscDatabin.GetDataByKey(35u).dwConfValue;
			CGuildSystem.s_rankpointProfitMax = GameDataMgr.guildMiscDatabin.GetDataByKey(40u).dwConfValue;
			int count = GameDataMgr.guildLevelDatabin.count;
			CGuildSystem.s_coinProfitPercentage = new uint[count];
			for (int i = 0; i < count; i++)
			{
				ResGuildLevel dataByKey = GameDataMgr.guildLevelDatabin.GetDataByKey((long)(i + 1));
				if (dataByKey != null)
				{
					CGuildSystem.s_coinProfitPercentage[i] = dataByKey.dwGameGoldBuffRate / 100u;
				}
				else
				{
					CGuildSystem.s_coinProfitPercentage[i] = 0u;
				}
			}
		}

		public void RequestGuildInfo()
		{
			if (this.IsInNormalGuild())
			{
				this.m_infoController.RequestGuildInfo();
				this.m_infoController.RequestApplyList(0);
			}
		}

		public static bool HasManageAuthority()
		{
			COM_PLAYER_GUILD_STATE guildState = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState;
			return guildState == 3 || guildState == 4;
		}

		public static bool HasAppointViceChairmanAuthority()
		{
			COM_PLAYER_GUILD_STATE guildState = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState;
			return guildState == 3;
		}

		public static bool CanBeAppointedToViceChairman(COM_PLAYER_GUILD_STATE memberGuildState)
		{
			return memberGuildState == 5 || memberGuildState == 6;
		}

		public static bool HasAppointMatchLeaderAuthority()
		{
			COM_PLAYER_GUILD_STATE guildState = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState;
			return guildState == 3;
		}

		public static bool CanRecommendSelfAsChairman()
		{
			COM_PLAYER_GUILD_STATE guildState = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState;
			return guildState == 4;
		}

		public static bool HasFireMemberAuthority(COM_PLAYER_GUILD_STATE memberGuildState)
		{
			COM_PLAYER_GUILD_STATE guildState = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState;
			if (guildState == 3)
			{
				if (memberGuildState == 4 || memberGuildState == 5 || memberGuildState == 6)
				{
					return true;
				}
			}
			else if (guildState == 4 && (memberGuildState == 5 || memberGuildState == 6))
			{
				return true;
			}
			return false;
		}

		public static bool HasTransferPositionAuthority()
		{
			COM_PLAYER_GUILD_STATE guildState = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState;
			return guildState == 3;
		}

		public static bool HasGuildSettingAuthority()
		{
			COM_PLAYER_GUILD_STATE guildState = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState;
			return guildState == 3 || guildState == 4;
		}

		public static bool HasGuildNameChangeAuthority()
		{
			COM_PLAYER_GUILD_STATE guildState = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState;
			return guildState == 3;
		}

		public static bool HasManageQQGroupAuthority()
		{
			COM_PLAYER_GUILD_STATE guildState = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState;
			return guildState == 3;
		}

		public static bool HasWirteGuildMailAuthority()
		{
			COM_PLAYER_GUILD_STATE guildState = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState;
			return guildState == 3 || guildState == 4;
		}

		public static bool IsInNormalGuild(COM_PLAYER_GUILD_STATE playerGuildState)
		{
			return playerGuildState == 3 || playerGuildState == 4 || playerGuildState == 5 || playerGuildState == 6;
		}

		public bool IsInNormalGuild()
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo == null || masterRoleInfo.m_baseGuildInfo == null)
			{
				return false;
			}
			COM_PLAYER_GUILD_STATE guildState = masterRoleInfo.m_baseGuildInfo.guildState;
			return CGuildSystem.IsInNormalGuild(guildState);
		}

		public bool IsInPrepareGuild()
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			return masterRoleInfo != null && masterRoleInfo.m_baseGuildInfo != null && (masterRoleInfo.m_baseGuildInfo.guildState == 1 || masterRoleInfo.m_baseGuildInfo.guildState == 2);
		}

		public bool CanInvite(COMDT_FRIEND_INFO info)
		{
			return Utility.IsSameLogicWorldWithSelf((int)info.stUin.dwLogicWorldId) && this.IsInNormalGuild() && CGuildSystem.HasManageAuthority() && info.bGuildState == 0 && info.dwPvpLvl >= CGuildHelper.GetGuildMemberMinPvpLevel();
		}

		public bool CanRecommend(COMDT_FRIEND_INFO info)
		{
			return Utility.IsSameLogicWorldWithSelf((int)info.stUin.dwLogicWorldId) && this.IsInNormalGuild() && !CGuildSystem.HasManageAuthority() && info.bGuildState == 0 && info.dwPvpLvl >= CGuildHelper.GetGuildMemberMinPvpLevel();
		}

		public bool HasInvited(ulong uid)
		{
			int inviteTimeInfoByUid = this.m_Model.GetInviteTimeInfoByUid(uid);
			if (inviteTimeInfoByUid == -1)
			{
				return false;
			}
			int dwConfValue = (int)GameDataMgr.guildMiscDatabin.GetDataByKey(12u).dwConfValue;
			int currentUTCTime = CRoleInfo.GetCurrentUTCTime();
			return currentUTCTime < inviteTimeInfoByUid + dwConfValue;
		}

		public bool HasRecommended(ulong uid)
		{
			int recommendTimeInfoByUid = this.m_Model.GetRecommendTimeInfoByUid(uid);
			if (recommendTimeInfoByUid == -1)
			{
				return false;
			}
			int dwConfValue = (int)GameDataMgr.guildMiscDatabin.GetDataByKey(13u).dwConfValue;
			int currentUTCTime = CRoleInfo.GetCurrentUTCTime();
			return currentUTCTime < recommendTimeInfoByUid + dwConfValue;
		}

		private void On_OpenForm(CUIEvent uiEvent)
		{
			if (GameDataMgr.guildMiscDatabin.GetDataByKey(23u).dwConfValue == 0u)
			{
				Singleton<CUIManager>.GetInstance().OpenTips("Common_System_Not_Open_Tip", true, 1.5f, null, new object[0]);
				return;
			}
			if (Singleton<CFunctionUnlockSys>.get_instance().FucIsUnlock(13))
			{
				if (this.IsInNormalGuild())
				{
					this.m_infoController.OpenForm();
				}
				else
				{
					this.m_listController.OpenForm();
				}
			}
			else
			{
				ResSpecialFucUnlock dataByKey = GameDataMgr.specialFunUnlockDatabin.GetDataByKey(13u);
				Singleton<CUIManager>.get_instance().OpenTips(Utility.UTF8Convert(dataByKey.szLockedTip), false, 1.5f, null, new object[0]);
			}
		}

		private void On_CloseForm(CUIEvent uiEvent)
		{
		}

		public void SearchGuild(ulong guildId, string guildName, byte searchSrc, bool isPrepareGuild)
		{
			Singleton<CUIManager>.GetInstance().OpenSendMsgAlert(5, enUIEventID.None);
			CSPkg cSPkg;
			if (isPrepareGuild)
			{
				cSPkg = NetworkModule.CreateDefaultCSPKG(2241u);
				cSPkg.stPkgData.get_stSearchPreGuildReq().ullGuildID = guildId;
				StringHelper.StringToUTF8Bytes(guildName, ref cSPkg.stPkgData.get_stSearchPreGuildReq().szGuildName);
				cSPkg.stPkgData.get_stSearchPreGuildReq().bSearchType = searchSrc;
			}
			else
			{
				cSPkg = NetworkModule.CreateDefaultCSPKG(2231u);
				cSPkg.stPkgData.get_stSearchGuildReq().ullGuildID = guildId;
				StringHelper.StringToUTF8Bytes(guildName, ref cSPkg.stPkgData.get_stSearchGuildReq().szGuildName);
				cSPkg.stPkgData.get_stSearchGuildReq().bSearchType = searchSrc;
			}
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		private void ResetRankpointAllRankInfo()
		{
			this.m_Model.CurrentGuildInfo.RankInfo.weekRankPoint = 0u;
			for (int i = 0; i < this.m_Model.CurrentGuildInfo.listMemInfo.get_Count(); i++)
			{
				this.m_Model.CurrentGuildInfo.listMemInfo.get_Item(i).RankInfo.weekRankPoint = 0u;
			}
		}

		public void OnGetPowerRanking(SCPKG_GET_RANKING_LIST_RSP rsp)
		{
			ListView<GuildInfo> listView = new ListView<GuildInfo>();
			byte b = 0;
			while ((uint)b < rsp.stRankingListDetail.get_stOfSucc().dwItemNum)
			{
				GuildInfo guildInfo = new GuildInfo();
				CSDT_RANKING_LIST_ITEM_INFO cSDT_RANKING_LIST_ITEM_INFO = rsp.stRankingListDetail.get_stOfSucc().astItemDetail[(int)b];
				guildInfo.briefInfo.uulUid = ulong.Parse(StringHelper.UTF8BytesToString(ref cSDT_RANKING_LIST_ITEM_INFO.szOpenID));
				guildInfo.briefInfo.Rank = cSDT_RANKING_LIST_ITEM_INFO.dwRankNo;
				guildInfo.briefInfo.Ability = cSDT_RANKING_LIST_ITEM_INFO.dwRankScore;
				guildInfo.briefInfo.dwHeadId = cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stGuildPower().dwGuildHeadID;
				guildInfo.briefInfo.sName = StringHelper.UTF8BytesToString(ref cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stGuildPower().szGuildName);
				guildInfo.briefInfo.bLevel = cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stGuildPower().bGuildLevel;
				guildInfo.briefInfo.bMemberNum = cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stGuildPower().bMemberNum;
				guildInfo.briefInfo.dwSettingMask = cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stGuildPower().dwSettingMask;
				guildInfo.briefInfo.LevelLimit = cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stGuildPower().bLimitLevel;
				guildInfo.briefInfo.GradeLimit = cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stGuildPower().bLimitGrade;
				guildInfo.chairman.stBriefInfo.szHeadUrl = StringHelper.UTF8BytesToString(ref cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stGuildPower().szChairManHeadUrl);
				guildInfo.chairman.stBriefInfo.sName = StringHelper.UTF8BytesToString(ref cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stGuildPower().szChairManName);
				guildInfo.chairman.stBriefInfo.dwLevel = cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stGuildPower().dwChairManLv;
				guildInfo.chairman.stBriefInfo.stVip.score = cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stGuildPower().stChairManVip.dwScore;
				guildInfo.chairman.stBriefInfo.stVip.level = cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stGuildPower().stChairManVip.dwCurLevel;
				guildInfo.chairman.stBriefInfo.stVip.headIconId = cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stGuildPower().stChairManVip.dwHeadIconId;
				guildInfo.briefInfo.sBulletin = StringHelper.UTF8BytesToString(ref cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stGuildPower().szGuildNotice);
				guildInfo.star = cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stGuildPower().dwStar;
				guildInfo.RankInfo.totalRankPoint = cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stGuildPower().dwTotalRankPoint;
				listView.Add(guildInfo);
				b += 1;
			}
			bool flag = CGuildHelper.IsFirstGuildListPage(rsp);
			Singleton<EventRouter>.GetInstance().BroadCastEvent<ListView<GuildInfo>, bool>("Receive_Guild_List_Success", listView, flag);
		}

		public void OnGetRankpointRanking(SCPKG_GET_RANKING_LIST_RSP rsp, enGuildRankpointRankListType rankListType)
		{
			CSDT_RANKING_LIST_SUCC stOfSucc = rsp.stRankingListDetail.get_stOfSucc();
			if (rankListType == enGuildRankpointRankListType.CurrentWeek || rankListType == enGuildRankpointRankListType.LastWeek)
			{
				rankListType = ((stOfSucc.bImage != 0) ? enGuildRankpointRankListType.LastWeek : enGuildRankpointRankListType.CurrentWeek);
			}
			CGuildSystem.RefreshRankpointRankInfoList(rankListType, stOfSucc.dwItemNum, stOfSucc.astItemDetail);
			if (Singleton<CUIManager>.GetInstance().GetForm("UGUI/Form/System/Guild/Form_Guild_RankpointRank.prefab") != null)
			{
				if (rankListType == enGuildRankpointRankListType.CurrentWeek || rankListType == enGuildRankpointRankListType.LastWeek)
				{
					Singleton<CGuildInfoView>.GetInstance().RefreshRankpointRankList(null);
				}
				else
				{
					Singleton<CGuildInfoView>.GetInstance().RefreshRankpointSeasonRankList(null);
				}
			}
		}

		public void OnLobbyStateLeave()
		{
			CGuildSystem.s_isGuildMaxGrade = CGuildHelper.IsGuildMaxGrade();
			CGuildSystem.s_isGuildHighestMatchScore = CGuildHelper.IsGuildHighestMatchScore();
		}

		private void OnPlayerNameChange()
		{
			GuildInfo currentGuildInfo = this.m_Model.CurrentGuildInfo;
			GuildMemInfo playerGuildMemberInfo = CGuildHelper.GetPlayerGuildMemberInfo();
			if (currentGuildInfo != null && playerGuildMemberInfo != null)
			{
				string name = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().Name;
				if (currentGuildInfo.chairman.stBriefInfo.uulUid == playerGuildMemberInfo.stBriefInfo.uulUid)
				{
					currentGuildInfo.chairman.stBriefInfo.sName = name;
				}
				playerGuildMemberInfo.stBriefInfo.sName = name;
			}
		}

		private static void RefreshRankpointRankInfoList(enGuildRankpointRankListType rankListType, uint itemNum, CSDT_RANKING_LIST_ITEM_INFO[] itemDetails)
		{
			Singleton<CGuildModel>.GetInstance().RankpointRankGottens[(int)rankListType] = true;
			Singleton<CGuildModel>.GetInstance().RankpointRankLastGottenTimes[(int)rankListType] = CRoleInfo.GetCurrentUTCTime();
			ListView<RankpointRankInfo> listView = Singleton<CGuildModel>.GetInstance().RankpointRankInfoLists[(int)rankListType];
			listView.Clear();
			int num = 0;
			while ((long)num < (long)((ulong)itemNum))
			{
				CSDT_RANKING_LIST_ITEM_INFO info = itemDetails[num];
				RankpointRankInfo rankpointRankInfo = CGuildSystem.CreateRankpointRankInfo(info, rankListType);
				listView.Add(rankpointRankInfo);
				num++;
			}
		}

		private static RankpointRankInfo CreateRankpointRankInfo(CSDT_RANKING_LIST_ITEM_INFO info, enGuildRankpointRankListType rankListType)
		{
			if (info.dwRankNo == 0u)
			{
				return CGuildHelper.CreatePlayerGuildRankpointRankInfo(rankListType);
			}
			return new RankpointRankInfo
			{
				guildId = ulong.Parse(StringHelper.UTF8BytesToString(ref info.szOpenID)),
				rankNo = info.dwRankNo,
				rankScore = info.dwRankScore,
				guildHeadId = info.stExtraInfo.stDetailInfo.get_stGuildRankPoint().dwGuildHeadID,
				guildName = StringHelper.UTF8BytesToString(ref info.stExtraInfo.stDetailInfo.get_stGuildRankPoint().szGuildName),
				guildLevel = info.stExtraInfo.stDetailInfo.get_stGuildRankPoint().bGuildLevel,
				memberNum = info.stExtraInfo.stDetailInfo.get_stGuildRankPoint().bMemberNum,
				star = info.stExtraInfo.stDetailInfo.get_stGuildRankPoint().dwStar
			};
		}

		[MessageHandler(2204)]
		public static void ReceivePrepareGuildList(CSPkg msg)
		{
			SCPKG_GET_PREPARE_GUILD_LIST_RSP stGetPrepareGuildListRsp = msg.stPkgData.get_stGetPrepareGuildListRsp();
			ListView<PrepareGuildInfo> listView = new ListView<PrepareGuildInfo>();
			for (byte b = 0; b < stGetPrepareGuildListRsp.bGuildNum; b += 1)
			{
				COMDT_PREPARE_GUILD_BRIEF_INFO cOMDT_PREPARE_GUILD_BRIEF_INFO = stGetPrepareGuildListRsp.astGuildList[(int)b];
				PrepareGuildInfo prepareGuildInfo = new PrepareGuildInfo();
				prepareGuildInfo.stBriefInfo.uulUid = cOMDT_PREPARE_GUILD_BRIEF_INFO.ullGuildID;
				prepareGuildInfo.stBriefInfo.dwLogicWorldId = cOMDT_PREPARE_GUILD_BRIEF_INFO.iLogicWorldID;
				prepareGuildInfo.stBriefInfo.sName = StringHelper.UTF8BytesToString(ref cOMDT_PREPARE_GUILD_BRIEF_INFO.szName);
				prepareGuildInfo.stBriefInfo.bMemCnt = cOMDT_PREPARE_GUILD_BRIEF_INFO.bMemberNum;
				prepareGuildInfo.stBriefInfo.dwHeadId = cOMDT_PREPARE_GUILD_BRIEF_INFO.dwHeadID;
				prepareGuildInfo.stBriefInfo.dwRequestTime = cOMDT_PREPARE_GUILD_BRIEF_INFO.dwRequestTime;
				prepareGuildInfo.stBriefInfo.sBulletin = StringHelper.UTF8BytesToString(ref cOMDT_PREPARE_GUILD_BRIEF_INFO.szNotice);
				prepareGuildInfo.stBriefInfo.IsOnlyFriend = Convert.ToBoolean(cOMDT_PREPARE_GUILD_BRIEF_INFO.bIsOnlyFriend);
				prepareGuildInfo.stBriefInfo.stCreatePlayer.uulUid = cOMDT_PREPARE_GUILD_BRIEF_INFO.stCreatePlayer.ullUid;
				prepareGuildInfo.stBriefInfo.stCreatePlayer.dwLogicWorldId = cOMDT_PREPARE_GUILD_BRIEF_INFO.stCreatePlayer.iLogicWorldID;
				prepareGuildInfo.stBriefInfo.stCreatePlayer.dwLevel = cOMDT_PREPARE_GUILD_BRIEF_INFO.stCreatePlayer.dwLevel;
				prepareGuildInfo.stBriefInfo.stCreatePlayer.sName = StringHelper.UTF8BytesToString(ref cOMDT_PREPARE_GUILD_BRIEF_INFO.stCreatePlayer.szName);
				prepareGuildInfo.stBriefInfo.stCreatePlayer.szHeadUrl = StringHelper.UTF8BytesToString(ref cOMDT_PREPARE_GUILD_BRIEF_INFO.stCreatePlayer.szHeadUrl);
				prepareGuildInfo.stBriefInfo.stCreatePlayer.stVip.score = cOMDT_PREPARE_GUILD_BRIEF_INFO.stCreatePlayer.stVip.dwScore;
				prepareGuildInfo.stBriefInfo.stCreatePlayer.stVip.level = cOMDT_PREPARE_GUILD_BRIEF_INFO.stCreatePlayer.stVip.dwCurLevel;
				prepareGuildInfo.stBriefInfo.stCreatePlayer.stVip.headIconId = cOMDT_PREPARE_GUILD_BRIEF_INFO.stCreatePlayer.stVip.dwHeadIconId;
				if (cOMDT_PREPARE_GUILD_BRIEF_INFO.dwRequestTime < Singleton<CGuildSystem>.GetInstance().Model.m_PrepareGuildOldestRequestTime)
				{
					Singleton<CGuildSystem>.GetInstance().Model.m_PrepareGuildOldestRequestTime = cOMDT_PREPARE_GUILD_BRIEF_INFO.dwRequestTime;
				}
				listView.Add(prepareGuildInfo);
			}
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<EventRouter>.GetInstance().BroadCastEvent<ListView<PrepareGuildInfo>, uint, byte, byte>("Receive_PrepareGuild_List_Success", listView, stGetPrepareGuildListRsp.dwTotalCnt, stGetPrepareGuildListRsp.bPageID, stGetPrepareGuildListRsp.bGuildNum);
		}

		[MessageHandler(2206)]
		public static void ReceiveGuildInfo(CSPkg msg)
		{
			if (CGuildSystem.IsError(msg.stPkgData.get_stGetGuildInfoRsp().bResult))
			{
				Singleton<EventRouter>.GetInstance().BroadCastEvent("Receive_Guild_Info_Failed");
				return;
			}
			COMDT_GUILD_INFO stGuildInfo = msg.stPkgData.get_stGetGuildInfoRsp().stGuildInfo;
			GuildInfo guildInfo = new GuildInfo();
			guildInfo.briefInfo = CGuildSystem.GetLocalGuildBriefInfo(stGuildInfo.stBriefInfo);
			guildInfo.uulCreatedTime = stGuildInfo.ullBuildTime;
			guildInfo.dwActive = stGuildInfo.dwActive;
			guildInfo.dwCoinPool = stGuildInfo.dwCoinPool;
			guildInfo.dwGuildMoney = stGuildInfo.dwGuildMoney;
			guildInfo.listMemInfo.Clear();
			int num = Mathf.Min((int)stGuildInfo.stBriefInfo.bMemberNum, stGuildInfo.astMemberInfo.Length);
			byte b = 0;
			while ((int)b < num)
			{
				GuildMemInfo guildMemInfo = new GuildMemInfo();
				COMDT_GUILD_MEMBER_INFO cOMDT_GUILD_MEMBER_INFO = stGuildInfo.astMemberInfo[(int)b];
				guildMemInfo.stBriefInfo.uulUid = cOMDT_GUILD_MEMBER_INFO.stBriefInfo.ullUid;
				guildMemInfo.stBriefInfo.dwLogicWorldId = cOMDT_GUILD_MEMBER_INFO.stBriefInfo.iLogicWorldID;
				guildMemInfo.stBriefInfo.szHeadUrl = StringHelper.UTF8BytesToString(ref cOMDT_GUILD_MEMBER_INFO.stBriefInfo.szHeadUrl);
				guildMemInfo.stBriefInfo.dwLevel = cOMDT_GUILD_MEMBER_INFO.stBriefInfo.dwLevel;
				guildMemInfo.stBriefInfo.dwAbility = cOMDT_GUILD_MEMBER_INFO.stBriefInfo.dwAbility;
				guildMemInfo.stBriefInfo.sName = StringHelper.UTF8BytesToString(ref cOMDT_GUILD_MEMBER_INFO.stBriefInfo.szName);
				guildMemInfo.stBriefInfo.dwGameEntity = cOMDT_GUILD_MEMBER_INFO.stBriefInfo.dwGameEntity;
				guildMemInfo.dwConstruct = cOMDT_GUILD_MEMBER_INFO.dwConstruct;
				guildMemInfo.enPosition = cOMDT_GUILD_MEMBER_INFO.bPosition;
				guildMemInfo.TotalContruct = cOMDT_GUILD_MEMBER_INFO.dwTotalConstruct;
				guildMemInfo.CurrActive = cOMDT_GUILD_MEMBER_INFO.dwCurrActive;
				guildMemInfo.WeekActive = cOMDT_GUILD_MEMBER_INFO.dwWeekActive;
				guildMemInfo.DonateCnt = cOMDT_GUILD_MEMBER_INFO.bDonateCnt;
				guildMemInfo.DonateNum = cOMDT_GUILD_MEMBER_INFO.dwDonateNum;
				guildMemInfo.WeekDividend = cOMDT_GUILD_MEMBER_INFO.dwWeekDividend;
				guildMemInfo.RankInfo.maxRankPoint = cOMDT_GUILD_MEMBER_INFO.stRankInfo.dwMaxRankPoint;
				guildMemInfo.RankInfo.totalRankPoint = cOMDT_GUILD_MEMBER_INFO.stRankInfo.dwTotalRankPoint;
				guildMemInfo.RankInfo.killCnt = cOMDT_GUILD_MEMBER_INFO.stRankInfo.dwKillCnt;
				guildMemInfo.RankInfo.deadCnt = cOMDT_GUILD_MEMBER_INFO.stRankInfo.dwDeadCnt;
				guildMemInfo.RankInfo.assistCnt = cOMDT_GUILD_MEMBER_INFO.stRankInfo.dwAssistCnt;
				guildMemInfo.RankInfo.weekRankPoint = cOMDT_GUILD_MEMBER_INFO.stRankInfo.dwWeekRankPoint;
				guildMemInfo.RankInfo.byGameRankPoint = cOMDT_GUILD_MEMBER_INFO.stRankInfo.dwGameRP;
				guildMemInfo.RankInfo.isSigned = (cOMDT_GUILD_MEMBER_INFO.stRankInfo.bSignIn > 0);
				guildMemInfo.stBriefInfo.stVip.score = cOMDT_GUILD_MEMBER_INFO.stBriefInfo.stVip.dwScore;
				guildMemInfo.stBriefInfo.stVip.level = cOMDT_GUILD_MEMBER_INFO.stBriefInfo.stVip.dwCurLevel;
				guildMemInfo.stBriefInfo.stVip.headIconId = cOMDT_GUILD_MEMBER_INFO.stBriefInfo.stVip.dwHeadIconId;
				guildMemInfo.stBriefInfo.dwScoreOfRank = cOMDT_GUILD_MEMBER_INFO.stBriefInfo.dwScoreOfRank;
				guildMemInfo.stBriefInfo.dwClassOfRank = cOMDT_GUILD_MEMBER_INFO.stBriefInfo.dwClassOfRank;
				guildMemInfo.LastLoginTime = cOMDT_GUILD_MEMBER_INFO.dwLastLoginTime;
				guildMemInfo.JoinTime = cOMDT_GUILD_MEMBER_INFO.dwJoinTime;
				CGuildSystem.SetLocalMemberGuildMatchInfo(guildMemInfo.GuildMatchInfo, cOMDT_GUILD_MEMBER_INFO.stGuildMatchInfo);
				if (guildMemInfo.stBriefInfo.uulUid == Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID)
				{
					Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState = guildMemInfo.enPosition;
					CGuildSystem.s_lastByGameRankpoint = guildMemInfo.RankInfo.byGameRankPoint;
					Singleton<EventRouter>.GetInstance().BroadCastEvent<bool>("Guild_Sign_State_Changed", guildMemInfo.RankInfo.isSigned);
				}
				if (guildMemInfo.enPosition == 3)
				{
					guildInfo.chairman = guildMemInfo;
				}
				guildInfo.listMemInfo.Add(guildMemInfo);
				b += 1;
			}
			guildInfo.listBuildingInfo.Clear();
			int num2 = Mathf.Min((int)stGuildInfo.bBuildingCnt, stGuildInfo.astBuildingInfo.Length);
			for (int i = 0; i < num2; i++)
			{
				GuildBuildingInfo guildBuildingInfo = new GuildBuildingInfo();
				guildBuildingInfo.type = stGuildInfo.astBuildingInfo[i].bBuildingType;
				guildBuildingInfo.level = stGuildInfo.astBuildingInfo[i].bLevel;
				guildInfo.listBuildingInfo.Add(guildBuildingInfo);
			}
			guildInfo.listSelfRecommendInfo.Clear();
			if (Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState == 3)
			{
				int num3 = Mathf.Min((int)stGuildInfo.bSelfRecommendCnt, stGuildInfo.astSelfRecommendInfo.Length);
				for (int j = 0; j < num3; j++)
				{
					GuildSelfRecommendInfo guildSelfRecommendInfo = new GuildSelfRecommendInfo();
					guildSelfRecommendInfo.uid = stGuildInfo.astSelfRecommendInfo[j].ullUid;
					guildSelfRecommendInfo.time = stGuildInfo.astSelfRecommendInfo[j].dwTime;
					guildInfo.listSelfRecommendInfo.Add(guildSelfRecommendInfo);
				}
			}
			guildInfo.RankInfo.totalRankPoint = stGuildInfo.stRankInfo.dwTotalRankPoint;
			guildInfo.RankInfo.seasonStartTime = stGuildInfo.stRankInfo.dwSeasonStartTime;
			guildInfo.RankInfo.weekRankPoint = stGuildInfo.stRankInfo.dwWeekRankPoint;
			Singleton<CNameChangeSystem>.GetInstance().SetGuildNameChangeCount((int)stGuildInfo.dwChangeNameCnt);
			guildInfo.star = stGuildInfo.dwStar;
			guildInfo.groupGuildId = stGuildInfo.dwGroupGuildID;
			guildInfo.groupOpenId = StringHelper.UTF8BytesToString(ref stGuildInfo.szGroupOpenID);
			CGuildSystem.SetLocalGuildMatchInfo(guildInfo.GuildMatchInfo, stGuildInfo.stGuildMatchInfo);
			CGuildSystem.SetLocalGuildMatchObInfo(guildInfo.GuildMatchObInfos, stGuildInfo);
			CGuildSystem.s_isGuildMaxGrade = CGuildHelper.IsGuildMaxGrade();
			CGuildSystem.s_isGuildHighestMatchScore = CGuildHelper.IsGuildHighestMatchScore();
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<EventRouter>.GetInstance().BroadCastEvent<GuildInfo>("Receive_Guild_Info_Success", guildInfo);
			Singleton<EventRouter>.GetInstance().BroadCastEvent<COMDT_GUILD_REWARDPOINT_LIST>("UnionRank_Get_Rank_Tram_Account_Point", msg.stPkgData.get_stGetGuildInfoRsp().stGuildPoint);
		}

		[MessageHandler(2208)]
		public static void ReceivePrepareGuildInfo(CSPkg msg)
		{
			if (CGuildSystem.IsError(msg.stPkgData.get_stGetPrepareGuildInfoRsp().bResult))
			{
				Singleton<EventRouter>.GetInstance().BroadCastEvent("Receive_PrepareGuild_Info_Failed");
				return;
			}
			COMDT_PREPARE_GUILD_INFO stGuildInfo = msg.stPkgData.get_stGetPrepareGuildInfoRsp().stGuildInfo;
			PrepareGuildInfo currentPrepareGuildInfo = Singleton<CGuildSystem>.GetInstance().Model.CurrentPrepareGuildInfo;
			currentPrepareGuildInfo.stBriefInfo.uulUid = stGuildInfo.stBriefInfo.ullGuildID;
			currentPrepareGuildInfo.stBriefInfo.dwLogicWorldId = stGuildInfo.stBriefInfo.iLogicWorldID;
			currentPrepareGuildInfo.stBriefInfo.sName = StringHelper.UTF8BytesToString(ref stGuildInfo.stBriefInfo.szName);
			currentPrepareGuildInfo.stBriefInfo.bMemCnt = stGuildInfo.stBriefInfo.bMemberNum;
			currentPrepareGuildInfo.stBriefInfo.dwHeadId = stGuildInfo.stBriefInfo.dwHeadID;
			currentPrepareGuildInfo.stBriefInfo.dwRequestTime = stGuildInfo.stBriefInfo.dwRequestTime;
			currentPrepareGuildInfo.stBriefInfo.sBulletin = StringHelper.UTF8BytesToString(ref stGuildInfo.stBriefInfo.szNotice);
			currentPrepareGuildInfo.stBriefInfo.IsOnlyFriend = Convert.ToBoolean(stGuildInfo.stBriefInfo.bIsOnlyFriend);
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.uulUid = stGuildInfo.stBriefInfo.stCreatePlayer.ullUid;
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.dwGameEntity = stGuildInfo.stBriefInfo.stCreatePlayer.dwGameEntity;
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.szHeadUrl = StringHelper.UTF8BytesToString(ref stGuildInfo.stBriefInfo.stCreatePlayer.szHeadUrl);
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.dwLevel = stGuildInfo.stBriefInfo.stCreatePlayer.dwLevel;
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.dwLogicWorldId = stGuildInfo.stBriefInfo.stCreatePlayer.iLogicWorldID;
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.sName = StringHelper.UTF8BytesToString(ref stGuildInfo.stBriefInfo.stCreatePlayer.szName);
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.stVip.score = stGuildInfo.stBriefInfo.stCreatePlayer.stVip.dwScore;
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.stVip.level = stGuildInfo.stBriefInfo.stCreatePlayer.stVip.dwCurLevel;
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.stVip.headIconId = stGuildInfo.stBriefInfo.stCreatePlayer.stVip.dwHeadIconId;
			currentPrepareGuildInfo.m_MemList.Clear();
			int num = Mathf.Min((int)stGuildInfo.stBriefInfo.bMemberNum, stGuildInfo.astMemberInfo.Length);
			byte b = 0;
			while ((int)b < num)
			{
				GuildMemInfo guildMemInfo = new GuildMemInfo();
				guildMemInfo.stBriefInfo.uulUid = stGuildInfo.astMemberInfo[(int)b].ullUid;
				guildMemInfo.stBriefInfo.dwLogicWorldId = stGuildInfo.astMemberInfo[(int)b].iLogicWorldID;
				guildMemInfo.stBriefInfo.szHeadUrl = StringHelper.UTF8BytesToString(ref stGuildInfo.astMemberInfo[(int)b].szHeadUrl);
				guildMemInfo.stBriefInfo.dwLevel = stGuildInfo.astMemberInfo[(int)b].dwLevel;
				guildMemInfo.stBriefInfo.sName = StringHelper.UTF8BytesToString(ref stGuildInfo.astMemberInfo[(int)b].szName);
				guildMemInfo.stBriefInfo.dwGameEntity = stGuildInfo.astMemberInfo[(int)b].dwGameEntity;
				guildMemInfo.stBriefInfo.stVip.score = stGuildInfo.astMemberInfo[(int)b].stVip.dwScore;
				guildMemInfo.stBriefInfo.stVip.level = stGuildInfo.astMemberInfo[(int)b].stVip.dwCurLevel;
				guildMemInfo.stBriefInfo.stVip.headIconId = stGuildInfo.astMemberInfo[(int)b].stVip.dwHeadIconId;
				currentPrepareGuildInfo.m_MemList.Add(guildMemInfo);
				b += 1;
			}
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<EventRouter>.GetInstance().BroadCastEvent<PrepareGuildInfo>("Receive_PrepareGuild_Info_Success", currentPrepareGuildInfo);
		}

		[MessageHandler(2222)]
		public static void ReceiveApplyJoinGuildRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			stAppliedGuildInfo stAppliedGuildInfo = default(stAppliedGuildInfo);
			COMDT_GUILD_BRIEF_INFO stGuildBriefInfo = msg.stPkgData.get_stApplyJoinGuildRsp().stGuildBriefInfo;
			if (CGuildSystem.IsError(msg.stPkgData.get_stApplyJoinGuildRsp().bResult))
			{
				Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState = Singleton<CGuildSystem>.GetInstance().Model.m_PlayerGuildLastState;
				if (msg.stPkgData.get_stApplyJoinGuildRsp().bResult == 2 && msg.stPkgData.get_stApplyJoinGuildRsp().dwApplyTime != 0u)
				{
					stAppliedGuildInfo.stBriefInfo = CGuildSystem.GetLocalGuildBriefInfo(stGuildBriefInfo);
					stAppliedGuildInfo.dwApplyTime = msg.stPkgData.get_stApplyJoinGuildRsp().dwApplyTime;
					Singleton<CGuildSystem>.GetInstance().Model.AddAppliedGuildInfo(stAppliedGuildInfo, true);
				}
				Singleton<EventRouter>.GetInstance().BroadCastEvent<stAppliedGuildInfo>("Receive_Apply_Guild_Join_Failed", stAppliedGuildInfo);
				return;
			}
			stAppliedGuildInfo.stBriefInfo = CGuildSystem.GetLocalGuildBriefInfo(stGuildBriefInfo);
			stAppliedGuildInfo.dwApplyTime = msg.stPkgData.get_stApplyJoinGuildRsp().dwApplyTime;
			Singleton<CGuildSystem>.GetInstance().Model.AddAppliedGuildInfo(stAppliedGuildInfo, true);
			GuildExtInfo expr_117 = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_extGuildInfo;
			expr_117.bApplyJoinGuildNum += 1;
			Singleton<CGuildSystem>.GetInstance().Model.m_PlayerGuildLastState = 0;
			Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState = 0;
			Singleton<EventRouter>.GetInstance().BroadCastEvent<stAppliedGuildInfo>("Receive_Apply_Guild_Join_Success", stAppliedGuildInfo);
		}

		[MessageHandler(2223)]
		public static void ReceiveApplyJoinGuildNtf(CSPkg msg)
		{
			int currentUTCTime = CRoleInfo.GetCurrentUTCTime();
			COMDT_GUILD_MEMBER_BRIEF_INFO stApplyInfo = msg.stPkgData.get_stJoinGuildApplyNtf().stApplyInfo;
			stApplicantInfo applicant = default(stApplicantInfo);
			applicant.stBriefInfo.uulUid = stApplyInfo.ullUid;
			applicant.stBriefInfo.dwGameEntity = stApplyInfo.dwGameEntity;
			applicant.stBriefInfo.szHeadUrl = StringHelper.UTF8BytesToString(ref stApplyInfo.szHeadUrl);
			applicant.stBriefInfo.dwLevel = stApplyInfo.dwLevel;
			applicant.stBriefInfo.dwAbility = stApplyInfo.dwAbility;
			applicant.stBriefInfo.dwLogicWorldId = stApplyInfo.iLogicWorldID;
			applicant.stBriefInfo.sName = StringHelper.UTF8BytesToString(ref stApplyInfo.szName);
			applicant.stBriefInfo.stVip.score = stApplyInfo.stVip.dwScore;
			applicant.stBriefInfo.stVip.level = stApplyInfo.stVip.dwCurLevel;
			applicant.stBriefInfo.stVip.headIconId = stApplyInfo.stVip.dwHeadIconId;
			applicant.stBriefInfo.dwScoreOfRank = stApplyInfo.dwScoreOfRank;
			applicant.stBriefInfo.dwClassOfRank = stApplyInfo.dwClassOfRank;
			applicant.dwApplyTime = currentUTCTime;
			Singleton<CGuildSystem>.GetInstance().Model.AddApplicant(applicant);
			Singleton<EventRouter>.GetInstance().BroadCastEvent("Guild_New_Applicant");
			CGuildSystem.s_isApplyAndRecommendListEmpty = false;
		}

		[MessageHandler(2212)]
		public static void ReceivePrepareGuildJoinRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			if (CGuildSystem.IsError(msg.stPkgData.get_stJoinPrepareGuildRsp().bResult))
			{
				Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState = Singleton<CGuildSystem>.GetInstance().Model.m_PlayerGuildLastState;
				Singleton<EventRouter>.GetInstance().BroadCastEvent<PrepareGuildInfo>("Receive_PrepareGuild_Join_Rsp", null);
				return;
			}
			COMDT_PREPARE_GUILD_INFO stGuildInfo = msg.stPkgData.get_stJoinPrepareGuildRsp().stGuildInfo;
			PrepareGuildInfo currentPrepareGuildInfo = Singleton<CGuildSystem>.GetInstance().Model.CurrentPrepareGuildInfo;
			currentPrepareGuildInfo.stBriefInfo.uulUid = stGuildInfo.stBriefInfo.ullGuildID;
			currentPrepareGuildInfo.stBriefInfo.dwLogicWorldId = stGuildInfo.stBriefInfo.iLogicWorldID;
			currentPrepareGuildInfo.stBriefInfo.sName = StringHelper.UTF8BytesToString(ref stGuildInfo.stBriefInfo.szName);
			currentPrepareGuildInfo.stBriefInfo.bMemCnt = stGuildInfo.stBriefInfo.bMemberNum;
			currentPrepareGuildInfo.stBriefInfo.dwHeadId = stGuildInfo.stBriefInfo.dwHeadID;
			currentPrepareGuildInfo.stBriefInfo.sBulletin = StringHelper.UTF8BytesToString(ref stGuildInfo.stBriefInfo.szNotice);
			currentPrepareGuildInfo.stBriefInfo.dwRequestTime = stGuildInfo.stBriefInfo.dwRequestTime;
			currentPrepareGuildInfo.stBriefInfo.IsOnlyFriend = Convert.ToBoolean(stGuildInfo.stBriefInfo.bIsOnlyFriend);
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.uulUid = stGuildInfo.stBriefInfo.stCreatePlayer.ullUid;
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.dwLogicWorldId = stGuildInfo.stBriefInfo.stCreatePlayer.iLogicWorldID;
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.szHeadUrl = StringHelper.UTF8BytesToString(ref stGuildInfo.stBriefInfo.stCreatePlayer.szHeadUrl);
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.dwLevel = stGuildInfo.stBriefInfo.stCreatePlayer.dwLevel;
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.sName = StringHelper.UTF8BytesToString(ref stGuildInfo.stBriefInfo.stCreatePlayer.szName);
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.dwGameEntity = stGuildInfo.stBriefInfo.stCreatePlayer.dwGameEntity;
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.stVip.score = stGuildInfo.stBriefInfo.stCreatePlayer.stVip.dwScore;
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.stVip.level = stGuildInfo.stBriefInfo.stCreatePlayer.stVip.dwCurLevel;
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.stVip.headIconId = stGuildInfo.stBriefInfo.stCreatePlayer.stVip.dwHeadIconId;
			currentPrepareGuildInfo.m_MemList.Clear();
			int num = Mathf.Min((int)stGuildInfo.stBriefInfo.bMemberNum, stGuildInfo.astMemberInfo.Length);
			byte b = 0;
			while ((int)b < num)
			{
				GuildMemInfo guildMemInfo = new GuildMemInfo();
				guildMemInfo.stBriefInfo.uulUid = stGuildInfo.astMemberInfo[(int)b].ullUid;
				guildMemInfo.stBriefInfo.dwLogicWorldId = stGuildInfo.astMemberInfo[(int)b].iLogicWorldID;
				guildMemInfo.stBriefInfo.szHeadUrl = StringHelper.UTF8BytesToString(ref stGuildInfo.astMemberInfo[(int)b].szHeadUrl);
				guildMemInfo.stBriefInfo.dwLevel = stGuildInfo.astMemberInfo[(int)b].dwLevel;
				guildMemInfo.stBriefInfo.sName = StringHelper.UTF8BytesToString(ref stGuildInfo.astMemberInfo[(int)b].szName);
				guildMemInfo.stBriefInfo.dwGameEntity = stGuildInfo.astMemberInfo[(int)b].dwGameEntity;
				guildMemInfo.stBriefInfo.stVip.score = stGuildInfo.astMemberInfo[(int)b].stVip.dwScore;
				guildMemInfo.stBriefInfo.stVip.level = stGuildInfo.astMemberInfo[(int)b].stVip.dwCurLevel;
				guildMemInfo.stBriefInfo.stVip.headIconId = stGuildInfo.astMemberInfo[(int)b].stVip.dwHeadIconId;
				currentPrepareGuildInfo.m_MemList.Add(guildMemInfo);
				b += 1;
			}
			Singleton<CGuildSystem>.GetInstance().Model.m_PlayerGuildLastState = 2;
			Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState = 2;
			Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.uulUid = currentPrepareGuildInfo.stBriefInfo.uulUid;
			Singleton<EventRouter>.GetInstance().BroadCastEvent<PrepareGuildInfo>("Receive_PrepareGuild_Join_Success", currentPrepareGuildInfo);
		}

		[MessageHandler(2214)]
		public static void ReceiveGuildAddNtf(CSPkg msg)
		{
			COMDT_GUILD_INFO stGuildInfo = msg.stPkgData.get_stAddGuildNtf().stGuildInfo;
			GuildInfo currentGuildInfo = Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo;
			currentGuildInfo.briefInfo = CGuildSystem.GetLocalGuildBriefInfo(stGuildInfo.stBriefInfo);
			currentGuildInfo.uulCreatedTime = stGuildInfo.ullBuildTime;
			currentGuildInfo.dwActive = stGuildInfo.dwActive;
			currentGuildInfo.dwCoinPool = stGuildInfo.dwCoinPool;
			currentGuildInfo.dwGuildMoney = stGuildInfo.dwGuildMoney;
			currentGuildInfo.listMemInfo.Clear();
			int num = Mathf.Min((int)stGuildInfo.stBriefInfo.bMemberNum, stGuildInfo.astMemberInfo.Length);
			byte b = 0;
			while ((int)b < num)
			{
				GuildMemInfo guildMemInfo = new GuildMemInfo();
				guildMemInfo.stBriefInfo.uulUid = stGuildInfo.astMemberInfo[(int)b].stBriefInfo.ullUid;
				guildMemInfo.stBriefInfo.dwLogicWorldId = stGuildInfo.astMemberInfo[(int)b].stBriefInfo.iLogicWorldID;
				guildMemInfo.stBriefInfo.szHeadUrl = StringHelper.UTF8BytesToString(ref stGuildInfo.astMemberInfo[(int)b].stBriefInfo.szHeadUrl);
				guildMemInfo.stBriefInfo.dwLevel = stGuildInfo.astMemberInfo[(int)b].stBriefInfo.dwLevel;
				guildMemInfo.stBriefInfo.sName = StringHelper.UTF8BytesToString(ref stGuildInfo.astMemberInfo[(int)b].stBriefInfo.szName);
				guildMemInfo.stBriefInfo.dwGameEntity = stGuildInfo.astMemberInfo[(int)b].stBriefInfo.dwGameEntity;
				guildMemInfo.dwConstruct = stGuildInfo.astMemberInfo[(int)b].dwConstruct;
				guildMemInfo.enPosition = stGuildInfo.astMemberInfo[(int)b].bPosition;
				guildMemInfo.TotalContruct = stGuildInfo.astMemberInfo[(int)b].dwTotalConstruct;
				guildMemInfo.CurrActive = stGuildInfo.astMemberInfo[(int)b].dwCurrActive;
				guildMemInfo.WeekActive = stGuildInfo.astMemberInfo[(int)b].dwWeekActive;
				guildMemInfo.DonateCnt = stGuildInfo.astMemberInfo[(int)b].bDonateCnt;
				guildMemInfo.DonateNum = stGuildInfo.astMemberInfo[(int)b].dwDonateNum;
				guildMemInfo.WeekDividend = stGuildInfo.astMemberInfo[(int)b].dwWeekDividend;
				guildMemInfo.RankInfo.killCnt = stGuildInfo.astMemberInfo[(int)b].stRankInfo.dwKillCnt;
				guildMemInfo.RankInfo.deadCnt = stGuildInfo.astMemberInfo[(int)b].stRankInfo.dwDeadCnt;
				guildMemInfo.RankInfo.assistCnt = stGuildInfo.astMemberInfo[(int)b].stRankInfo.dwAssistCnt;
				guildMemInfo.RankInfo.weekRankPoint = stGuildInfo.astMemberInfo[(int)b].stRankInfo.dwWeekRankPoint;
				guildMemInfo.RankInfo.byGameRankPoint = stGuildInfo.astMemberInfo[(int)b].stRankInfo.dwGameRP;
				guildMemInfo.RankInfo.isSigned = (stGuildInfo.astMemberInfo[(int)b].stRankInfo.bSignIn > 0);
				guildMemInfo.stBriefInfo.stVip.score = stGuildInfo.astMemberInfo[(int)b].stBriefInfo.stVip.dwScore;
				guildMemInfo.stBriefInfo.stVip.level = stGuildInfo.astMemberInfo[(int)b].stBriefInfo.stVip.dwCurLevel;
				guildMemInfo.stBriefInfo.stVip.headIconId = stGuildInfo.astMemberInfo[(int)b].stBriefInfo.stVip.dwHeadIconId;
				guildMemInfo.stBriefInfo.dwScoreOfRank = stGuildInfo.astMemberInfo[(int)b].stBriefInfo.dwScoreOfRank;
				guildMemInfo.stBriefInfo.dwClassOfRank = stGuildInfo.astMemberInfo[(int)b].stBriefInfo.dwClassOfRank;
				guildMemInfo.LastLoginTime = stGuildInfo.astMemberInfo[(int)b].dwLastLoginTime;
				guildMemInfo.JoinTime = stGuildInfo.astMemberInfo[(int)b].dwJoinTime;
				CGuildSystem.SetLocalMemberGuildMatchInfo(guildMemInfo.GuildMatchInfo, stGuildInfo.astMemberInfo[(int)b].stGuildMatchInfo);
				if (guildMemInfo.stBriefInfo.uulUid == Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID)
				{
					Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState = guildMemInfo.enPosition;
					CGuildSystem.s_lastByGameRankpoint = guildMemInfo.RankInfo.byGameRankPoint;
				}
				if (guildMemInfo.enPosition == 3)
				{
					currentGuildInfo.chairman = guildMemInfo;
				}
				currentGuildInfo.listMemInfo.Add(guildMemInfo);
				b += 1;
			}
			currentGuildInfo.listBuildingInfo.Clear();
			int num2 = Mathf.Min((int)stGuildInfo.bBuildingCnt, stGuildInfo.astBuildingInfo.Length);
			for (int i = 0; i < num2; i++)
			{
				GuildBuildingInfo guildBuildingInfo = new GuildBuildingInfo();
				guildBuildingInfo.type = stGuildInfo.astBuildingInfo[i].bBuildingType;
				guildBuildingInfo.level = stGuildInfo.astBuildingInfo[i].bLevel;
				currentGuildInfo.listBuildingInfo.Add(guildBuildingInfo);
			}
			currentGuildInfo.RankInfo.totalRankPoint = stGuildInfo.stRankInfo.dwTotalRankPoint;
			currentGuildInfo.RankInfo.seasonStartTime = stGuildInfo.stRankInfo.dwSeasonStartTime;
			currentGuildInfo.RankInfo.weekRankPoint = stGuildInfo.stRankInfo.dwWeekRankPoint;
			currentGuildInfo.star = stGuildInfo.dwStar;
			currentGuildInfo.groupGuildId = stGuildInfo.dwGroupGuildID;
			currentGuildInfo.groupOpenId = StringHelper.UTF8BytesToString(ref stGuildInfo.szGroupOpenID);
			CGuildSystem.SetLocalGuildMatchInfo(currentGuildInfo.GuildMatchInfo, stGuildInfo.stGuildMatchInfo);
			CGuildSystem.SetLocalGuildMatchObInfo(currentGuildInfo.GuildMatchObInfos, stGuildInfo);
			Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.uulUid = currentGuildInfo.briefInfo.uulUid;
			Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.name = currentGuildInfo.briefInfo.sName;
			Singleton<CGuildSystem>.GetInstance().Model.m_PlayerGuildLastState = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState;
			Singleton<CGuildSystem>.GetInstance().Model.RemoveAppliedGuildInfo(currentGuildInfo.briefInfo.uulUid);
			Singleton<EventRouter>.GetInstance().BroadCastEvent<GuildInfo>("Guild_Create_Or_Add_Success", currentGuildInfo);
			Singleton<EventRouter>.GetInstance().BroadCastEvent("Guild_Enter_Guild");
		}

		[MessageHandler(2216)]
		public static void ReceivePrepareGuildBreakNtf(CSPkg msg)
		{
			Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState = 0;
			Singleton<CGuildSystem>.GetInstance().Model.ResetCurrentPrepareGuildInfo();
		}

		[MessageHandler(2210)]
		public static void ReceivePrepareGuildCreateRsp(CSPkg msg)
		{
			SCPKG_CREATE_GUILD_RSP stCreateGuildRsp = msg.stPkgData.get_stCreateGuildRsp();
			if (CGuildSystem.IsError(stCreateGuildRsp.bResult))
			{
				Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState = Singleton<CGuildSystem>.get_instance().Model.m_PlayerGuildLastState;
				Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
				Singleton<EventRouter>.GetInstance().BroadCastEvent("Receive_PrepareGuild_Create_Failed");
				return;
			}
			Singleton<CGuildSystem>.GetInstance().Model.m_PlayerGuildLastState = (Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState = 1);
			Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.uulUid = stCreateGuildRsp.stGuildInfo.stBriefInfo.ullGuildID;
			PrepareGuildInfo currentPrepareGuildInfo = Singleton<CGuildSystem>.GetInstance().Model.CurrentPrepareGuildInfo;
			currentPrepareGuildInfo.stBriefInfo.uulUid = stCreateGuildRsp.stGuildInfo.stBriefInfo.ullGuildID;
			currentPrepareGuildInfo.stBriefInfo.dwLogicWorldId = stCreateGuildRsp.stGuildInfo.stBriefInfo.iLogicWorldID;
			currentPrepareGuildInfo.stBriefInfo.sName = StringHelper.UTF8BytesToString(ref stCreateGuildRsp.stGuildInfo.stBriefInfo.szName);
			currentPrepareGuildInfo.stBriefInfo.bMemCnt = stCreateGuildRsp.stGuildInfo.stBriefInfo.bMemberNum;
			currentPrepareGuildInfo.stBriefInfo.dwHeadId = stCreateGuildRsp.stGuildInfo.stBriefInfo.dwHeadID;
			currentPrepareGuildInfo.stBriefInfo.sBulletin = StringHelper.UTF8BytesToString(ref stCreateGuildRsp.stGuildInfo.stBriefInfo.szNotice);
			currentPrepareGuildInfo.stBriefInfo.dwRequestTime = stCreateGuildRsp.stGuildInfo.stBriefInfo.dwRequestTime;
			currentPrepareGuildInfo.stBriefInfo.IsOnlyFriend = Convert.ToBoolean(stCreateGuildRsp.stGuildInfo.stBriefInfo.bIsOnlyFriend);
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.uulUid = stCreateGuildRsp.stGuildInfo.stBriefInfo.stCreatePlayer.ullUid;
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.dwLogicWorldId = stCreateGuildRsp.stGuildInfo.stBriefInfo.stCreatePlayer.iLogicWorldID;
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.szHeadUrl = StringHelper.UTF8BytesToString(ref stCreateGuildRsp.stGuildInfo.stBriefInfo.stCreatePlayer.szHeadUrl);
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.dwLevel = stCreateGuildRsp.stGuildInfo.stBriefInfo.stCreatePlayer.dwLevel;
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.sName = StringHelper.UTF8BytesToString(ref stCreateGuildRsp.stGuildInfo.stBriefInfo.stCreatePlayer.szName);
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.dwGameEntity = stCreateGuildRsp.stGuildInfo.stBriefInfo.stCreatePlayer.dwGameEntity;
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.stVip.score = stCreateGuildRsp.stGuildInfo.stBriefInfo.stCreatePlayer.stVip.dwScore;
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.stVip.level = stCreateGuildRsp.stGuildInfo.stBriefInfo.stCreatePlayer.stVip.dwCurLevel;
			currentPrepareGuildInfo.stBriefInfo.stCreatePlayer.stVip.headIconId = stCreateGuildRsp.stGuildInfo.stBriefInfo.stCreatePlayer.stVip.dwHeadIconId;
			currentPrepareGuildInfo.m_MemList.Clear();
			int num = Mathf.Min((int)stCreateGuildRsp.stGuildInfo.stBriefInfo.bMemberNum, stCreateGuildRsp.stGuildInfo.astMemberInfo.Length);
			byte b = 0;
			while ((int)b < num)
			{
				GuildMemInfo guildMemInfo = new GuildMemInfo();
				guildMemInfo.stBriefInfo.uulUid = stCreateGuildRsp.stGuildInfo.astMemberInfo[(int)b].ullUid;
				guildMemInfo.stBriefInfo.dwLogicWorldId = stCreateGuildRsp.stGuildInfo.astMemberInfo[(int)b].iLogicWorldID;
				guildMemInfo.stBriefInfo.szHeadUrl = StringHelper.UTF8BytesToString(ref stCreateGuildRsp.stGuildInfo.astMemberInfo[(int)b].szHeadUrl);
				guildMemInfo.stBriefInfo.dwLevel = stCreateGuildRsp.stGuildInfo.astMemberInfo[(int)b].dwLevel;
				guildMemInfo.stBriefInfo.sName = StringHelper.UTF8BytesToString(ref stCreateGuildRsp.stGuildInfo.astMemberInfo[(int)b].szName);
				guildMemInfo.stBriefInfo.dwGameEntity = stCreateGuildRsp.stGuildInfo.astMemberInfo[(int)b].dwGameEntity;
				guildMemInfo.stBriefInfo.stVip.score = stCreateGuildRsp.stGuildInfo.astMemberInfo[(int)b].stVip.dwScore;
				guildMemInfo.stBriefInfo.stVip.level = stCreateGuildRsp.stGuildInfo.astMemberInfo[(int)b].stVip.dwCurLevel;
				guildMemInfo.stBriefInfo.stVip.headIconId = stCreateGuildRsp.stGuildInfo.astMemberInfo[(int)b].stVip.dwHeadIconId;
				currentPrepareGuildInfo.m_MemList.Add(guildMemInfo);
				b += 1;
			}
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<EventRouter>.GetInstance().BroadCastEvent<PrepareGuildInfo>("Receive_PrepareGuild_Create_Success", currentPrepareGuildInfo);
		}

		[MessageHandler(2218)]
		public static void ReceiveGuildSettingModifyRsp(CSPkg msg)
		{
			SCPKG_MODIFY_GUILD_SETTING_RSP stModifyGuildSettingRsp = msg.stPkgData.get_stModifyGuildSettingRsp();
			if (CGuildSystem.IsError(stModifyGuildSettingRsp.bResult))
			{
				Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
				Singleton<EventRouter>.GetInstance().BroadCastEvent("Guild_Setting_Modify_Failed");
				return;
			}
			Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo.briefInfo.dwSettingMask = stModifyGuildSettingRsp.dwSettingMask;
			Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo.briefInfo.LevelLimit = stModifyGuildSettingRsp.bLimitLevel;
			Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo.briefInfo.GradeLimit = stModifyGuildSettingRsp.bLimitGrade;
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<EventRouter>.GetInstance().BroadCastEvent<uint>("Guild_Setting_Modify_Success", stModifyGuildSettingRsp.dwSettingMask);
		}

		[MessageHandler(2224)]
		public static void ReceiveNewMemberNtf(CSPkg msg)
		{
			COMDT_GUILD_MEMBER_INFO stNewMember = msg.stPkgData.get_stNewMemberJoinGuildNtf().stNewMember;
			GuildInfo currentGuildInfo = Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo;
			GuildMemInfo guildMemInfo = new GuildMemInfo();
			guildMemInfo.stBriefInfo.uulUid = stNewMember.stBriefInfo.ullUid;
			guildMemInfo.stBriefInfo.dwGameEntity = stNewMember.stBriefInfo.dwGameEntity;
			guildMemInfo.stBriefInfo.szHeadUrl = StringHelper.UTF8BytesToString(ref stNewMember.stBriefInfo.szHeadUrl);
			guildMemInfo.stBriefInfo.dwLevel = stNewMember.stBriefInfo.dwLevel;
			guildMemInfo.stBriefInfo.dwLogicWorldId = stNewMember.stBriefInfo.iLogicWorldID;
			guildMemInfo.stBriefInfo.sName = StringHelper.UTF8BytesToString(ref stNewMember.stBriefInfo.szName);
			guildMemInfo.dwConstruct = stNewMember.dwConstruct;
			guildMemInfo.enPosition = stNewMember.bPosition;
			guildMemInfo.TotalContruct = stNewMember.dwTotalConstruct;
			guildMemInfo.CurrActive = stNewMember.dwCurrActive;
			guildMemInfo.WeekActive = stNewMember.dwWeekActive;
			guildMemInfo.DonateCnt = stNewMember.bDonateCnt;
			guildMemInfo.DonateNum = stNewMember.dwDonateNum;
			guildMemInfo.WeekDividend = stNewMember.dwWeekDividend;
			guildMemInfo.RankInfo.maxRankPoint = stNewMember.stRankInfo.dwMaxRankPoint;
			guildMemInfo.RankInfo.totalRankPoint = stNewMember.stRankInfo.dwTotalRankPoint;
			guildMemInfo.RankInfo.killCnt = stNewMember.stRankInfo.dwKillCnt;
			guildMemInfo.RankInfo.deadCnt = stNewMember.stRankInfo.dwDeadCnt;
			guildMemInfo.RankInfo.assistCnt = stNewMember.stRankInfo.dwAssistCnt;
			guildMemInfo.RankInfo.weekRankPoint = stNewMember.stRankInfo.dwWeekRankPoint;
			guildMemInfo.RankInfo.byGameRankPoint = stNewMember.stRankInfo.dwGameRP;
			guildMemInfo.RankInfo.isSigned = (stNewMember.stRankInfo.bSignIn > 0);
			guildMemInfo.stBriefInfo.stVip.score = stNewMember.stBriefInfo.stVip.dwScore;
			guildMemInfo.stBriefInfo.stVip.level = stNewMember.stBriefInfo.stVip.dwCurLevel;
			guildMemInfo.stBriefInfo.stVip.headIconId = stNewMember.stBriefInfo.stVip.dwHeadIconId;
			guildMemInfo.stBriefInfo.dwScoreOfRank = stNewMember.stBriefInfo.dwScoreOfRank;
			guildMemInfo.stBriefInfo.dwClassOfRank = stNewMember.stBriefInfo.dwClassOfRank;
			guildMemInfo.LastLoginTime = stNewMember.dwLastLoginTime;
			guildMemInfo.JoinTime = stNewMember.dwJoinTime;
			CGuildSystem.SetLocalMemberGuildMatchInfo(guildMemInfo.GuildMatchInfo, stNewMember.stGuildMatchInfo);
			if (!currentGuildInfo.listMemInfo.Contains(guildMemInfo))
			{
				currentGuildInfo.listMemInfo.Add(guildMemInfo);
				GuildInfo expr_2A0_cp_0 = currentGuildInfo;
				expr_2A0_cp_0.briefInfo.bMemberNum = expr_2A0_cp_0.briefInfo.bMemberNum + 1;
			}
			Singleton<EventRouter>.GetInstance().BroadCastEvent("Guild_New_Member");
		}

		[MessageHandler(2228)]
		public static void ReceiveMemberQuitNtf(CSPkg msg)
		{
			ulong ullQuitUid = msg.stPkgData.get_stQuitGuildNtf().ullQuitUid;
			for (int i = Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo.listMemInfo.get_Count() - 1; i >= 0; i--)
			{
				GuildMemInfo guildMemInfo = Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo.listMemInfo.get_Item(i);
				if (guildMemInfo.stBriefInfo.uulUid == ullQuitUid)
				{
					Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo.listMemInfo.RemoveAt(i);
					GuildInfo expr_8C_cp_0 = Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo;
					expr_8C_cp_0.briefInfo.bMemberNum = expr_8C_cp_0.briefInfo.bMemberNum - 1;
				}
			}
			Singleton<EventRouter>.GetInstance().BroadCastEvent("Guild_Member_Quit");
		}

		[MessageHandler(2220)]
		public static void ReceiveApplyListRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			SCPKG_GET_GUILD_APPLY_LIST_RSP stGetGuildApplyListRsp = msg.stPkgData.get_stGetGuildApplyListRsp();
			if (!Singleton<CGuildInfoView>.GetInstance().IsShow())
			{
				if (stGetGuildApplyListRsp.bApplyCnt > 0)
				{
					CGuildSystem.s_isApplyAndRecommendListEmpty = false;
				}
				return;
			}
			int currentUTCTime = CRoleInfo.GetCurrentUTCTime();
			stApplicantInfo stApplicantInfo = default(stApplicantInfo);
			List<stApplicantInfo> list = new List<stApplicantInfo>();
			for (byte b = 0; b < stGetGuildApplyListRsp.bApplyCnt; b += 1)
			{
				COMDT_GUILD_MEMBER_BRIEF_INFO cOMDT_GUILD_MEMBER_BRIEF_INFO = stGetGuildApplyListRsp.astApplyInfo[(int)b];
				stApplicantInfo.stBriefInfo.uulUid = cOMDT_GUILD_MEMBER_BRIEF_INFO.ullUid;
				stApplicantInfo.stBriefInfo.dwLogicWorldId = cOMDT_GUILD_MEMBER_BRIEF_INFO.iLogicWorldID;
				stApplicantInfo.stBriefInfo.sName = StringHelper.UTF8BytesToString(ref cOMDT_GUILD_MEMBER_BRIEF_INFO.szName);
				stApplicantInfo.stBriefInfo.dwLevel = cOMDT_GUILD_MEMBER_BRIEF_INFO.dwLevel;
				stApplicantInfo.stBriefInfo.dwAbility = cOMDT_GUILD_MEMBER_BRIEF_INFO.dwAbility;
				stApplicantInfo.stBriefInfo.dwGameEntity = cOMDT_GUILD_MEMBER_BRIEF_INFO.dwGameEntity;
				stApplicantInfo.stBriefInfo.szHeadUrl = StringHelper.UTF8BytesToString(ref cOMDT_GUILD_MEMBER_BRIEF_INFO.szHeadUrl);
				stApplicantInfo.stBriefInfo.stVip.score = cOMDT_GUILD_MEMBER_BRIEF_INFO.stVip.dwScore;
				stApplicantInfo.stBriefInfo.stVip.level = cOMDT_GUILD_MEMBER_BRIEF_INFO.stVip.dwCurLevel;
				stApplicantInfo.stBriefInfo.stVip.headIconId = cOMDT_GUILD_MEMBER_BRIEF_INFO.stVip.dwHeadIconId;
				stApplicantInfo.stBriefInfo.dwScoreOfRank = cOMDT_GUILD_MEMBER_BRIEF_INFO.dwScoreOfRank;
				stApplicantInfo.stBriefInfo.dwClassOfRank = cOMDT_GUILD_MEMBER_BRIEF_INFO.dwClassOfRank;
				stApplicantInfo.stBriefInfo.gender = cOMDT_GUILD_MEMBER_BRIEF_INFO.bGender;
				stApplicantInfo.dwApplyTime = currentUTCTime;
				list.Add(stApplicantInfo);
			}
			Singleton<CGuildInfoController>.GetInstance().OnReceiveApplyListSuccess(list);
		}

		[MessageHandler(2227)]
		public static void ReceiveGuildQuitRsp(CSPkg msg)
		{
			if (CGuildSystem.IsError(msg.stPkgData.get_stQuitGuildRsp().bResult))
			{
				Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
				Singleton<EventRouter>.GetInstance().BroadCastEvent("Guild_Quit_Failed");
			}
			else
			{
				Singleton<CGuildSystem>.GetInstance().Model.m_PlayerGuildLastState = 0;
				Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState = 0;
				Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.uulUid = 0uL;
				Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.name = string.Empty;
				Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_extGuildInfo.dwLastQuitGuildTime = (uint)CRoleInfo.GetCurrentUTCTime();
				Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
				Singleton<EventRouter>.GetInstance().BroadCastEvent("Guild_Quit_Success");
				Singleton<EventRouter>.GetInstance().BroadCastEvent("Guild_Leave_Guild");
			}
		}

		[MessageHandler(2230)]
		public static void ReceiveGuildInviteRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			stInvitedFriend friend = default(stInvitedFriend);
			if (CGuildSystem.IsError(msg.stPkgData.get_stGuildInviteRsp().bResult))
			{
				if (msg.stPkgData.get_stGuildInviteRsp().bResult == 6)
				{
					friend.uulUid = msg.stPkgData.get_stGuildInviteRsp().ullBeInviteUid;
					friend.dwInviteTime = (int)msg.stPkgData.get_stGuildInviteRsp().dwInviteTime;
					Singleton<CGuildSystem>.GetInstance().Model.AddInvitedFriend(friend, true);
					Singleton<CUIManager>.GetInstance().OpenTips("Guild_Friend_Has_Been_Invited_Tip", true, 1.5f, null, new object[0]);
					Singleton<EventRouter>.GetInstance().BroadCastEvent("Guild_Invite_Success");
				}
				return;
			}
			friend.uulUid = msg.stPkgData.get_stGuildInviteRsp().ullBeInviteUid;
			friend.dwInviteTime = (int)msg.stPkgData.get_stGuildInviteRsp().dwInviteTime;
			Singleton<CGuildSystem>.GetInstance().Model.AddInvitedFriend(friend, true);
			Singleton<CGuildSystem>.GetInstance().Model.AddInviteTimeInfo(friend.uulUid, friend.dwInviteTime);
			Singleton<CUIManager>.GetInstance().OpenTips("Guild_Invite_Success_Tip", true, 1.5f, null, new object[0]);
			Singleton<EventRouter>.GetInstance().BroadCastEvent("Guild_Invite_Success");
		}

		[MessageHandler(2240)]
		public static void ReceiveDealGuildInviteRsp(CSPkg msg)
		{
			CGuildSystem.IsError(msg.stPkgData.get_stDealGuildInviteRsp().bResult);
		}

		[MessageHandler(2235)]
		public static void ReceiveGuildRecommendRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			if (CGuildSystem.IsError(msg.stPkgData.get_stGuildRecommendRsp().bResult))
			{
				return;
			}
			Singleton<CGuildSystem>.GetInstance().Model.AddRecommendTimeInfo(msg.stPkgData.get_stGuildRecommendRsp().ullAcntUid, (int)msg.stPkgData.get_stGuildRecommendRsp().dwRecommendTime);
			Singleton<CUIManager>.GetInstance().OpenTips("Guild_Recommend_Success_Tip", true, 1.5f, null, new object[0]);
			Singleton<EventRouter>.GetInstance().BroadCastEvent("Guild_Recommend_Success");
		}

		[MessageHandler(2236)]
		public static void ReceiveGuildRecommendNtf(CSPkg msg)
		{
			COMDT_GUILD_RECOMMEND_INFO stRecommendInfo = msg.stPkgData.get_stGuildRecommendNtf().stRecommendInfo;
			stRecommendInfo info = default(stRecommendInfo);
			info.uid = stRecommendInfo.ullUid;
			info.logicWorldID = stRecommendInfo.iLogicWorldID;
			info.headUrl = StringHelper.UTF8BytesToString(ref stRecommendInfo.szHeadUrl);
			info.name = StringHelper.UTF8BytesToString(ref stRecommendInfo.szName);
			info.level = stRecommendInfo.dwLevel;
			info.ability = stRecommendInfo.dwAbility;
			info.recommendName = StringHelper.UTF8BytesToString(ref stRecommendInfo.szRecommendName);
			info.stVip.score = stRecommendInfo.stVip.dwScore;
			info.stVip.level = stRecommendInfo.stVip.dwCurLevel;
			info.stVip.headIconId = stRecommendInfo.stVip.dwHeadIconId;
			info.rankScore = stRecommendInfo.dwScoreOfRank;
			info.rankClass = stRecommendInfo.dwRankClass;
			info.gender = stRecommendInfo.bGender;
			Singleton<CGuildSystem>.GetInstance().Model.AddRecommendInfo(info);
			CGuildSystem.s_isApplyAndRecommendListEmpty = false;
		}

		[MessageHandler(2238)]
		public static void ReceiveRecommendListRsp(CSPkg msg)
		{
			stRecommendInfo stRecommendInfo = default(stRecommendInfo);
			List<stRecommendInfo> list = new List<stRecommendInfo>();
			SCPKG_GET_GUILD_RECOMMEND_LIST_RSP stGetGuildRecommendListRsp = msg.stPkgData.get_stGetGuildRecommendListRsp();
			if (!Singleton<CGuildInfoView>.GetInstance().IsShow())
			{
				if (stGetGuildRecommendListRsp.dwTotalCnt > 0u)
				{
					CGuildSystem.s_isApplyAndRecommendListEmpty = false;
				}
				return;
			}
			for (int i = 0; i < (int)stGetGuildRecommendListRsp.bCount; i++)
			{
				COMDT_GUILD_RECOMMEND_INFO cOMDT_GUILD_RECOMMEND_INFO = stGetGuildRecommendListRsp.astRecommendInfo[i];
				stRecommendInfo.uid = cOMDT_GUILD_RECOMMEND_INFO.ullUid;
				stRecommendInfo.logicWorldID = cOMDT_GUILD_RECOMMEND_INFO.iLogicWorldID;
				stRecommendInfo.headUrl = StringHelper.UTF8BytesToString(ref cOMDT_GUILD_RECOMMEND_INFO.szHeadUrl);
				stRecommendInfo.name = StringHelper.UTF8BytesToString(ref cOMDT_GUILD_RECOMMEND_INFO.szName);
				stRecommendInfo.level = cOMDT_GUILD_RECOMMEND_INFO.dwLevel;
				stRecommendInfo.ability = cOMDT_GUILD_RECOMMEND_INFO.dwAbility;
				stRecommendInfo.recommendName = StringHelper.UTF8BytesToString(ref cOMDT_GUILD_RECOMMEND_INFO.szRecommendName);
				stRecommendInfo.stVip.score = cOMDT_GUILD_RECOMMEND_INFO.stVip.dwScore;
				stRecommendInfo.stVip.level = cOMDT_GUILD_RECOMMEND_INFO.stVip.dwCurLevel;
				stRecommendInfo.stVip.headIconId = cOMDT_GUILD_RECOMMEND_INFO.stVip.dwHeadIconId;
				stRecommendInfo.rankScore = cOMDT_GUILD_RECOMMEND_INFO.dwScoreOfRank;
				stRecommendInfo.rankClass = cOMDT_GUILD_RECOMMEND_INFO.dwRankClass;
				stRecommendInfo.gender = cOMDT_GUILD_RECOMMEND_INFO.bGender;
				list.Add(stRecommendInfo);
			}
			Singleton<CGuildInfoController>.GetInstance().OnReceiveRecommendListSuccess(list);
		}

		[MessageHandler(2232)]
		public static void ReceiveSearchGuildRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			SCPKG_SEARCH_GUILD_RSP stSearchGuildRsp = msg.stPkgData.get_stSearchGuildRsp();
			if (CGuildSystem.IsError(stSearchGuildRsp.bResult))
			{
				return;
			}
			GuildInfo guildInfo = new GuildInfo();
			CSDT_RANKING_LIST_ITEM_INFO stGuildInfo = stSearchGuildRsp.stGuildInfo;
			guildInfo.briefInfo.uulUid = ulong.Parse(StringHelper.UTF8BytesToString(ref stGuildInfo.szOpenID));
			guildInfo.briefInfo.Rank = stGuildInfo.dwRankNo;
			guildInfo.briefInfo.Ability = stGuildInfo.dwRankScore;
			COMDT_RANKING_LIST_ITEM_EXTRA_GUILD_POWER stGuildPower = stGuildInfo.stExtraInfo.stDetailInfo.get_stGuildPower();
			guildInfo.briefInfo.bLevel = stGuildPower.bGuildLevel;
			guildInfo.briefInfo.bMemberNum = stGuildPower.bMemberNum;
			guildInfo.briefInfo.dwHeadId = stGuildPower.dwGuildHeadID;
			guildInfo.briefInfo.sBulletin = StringHelper.UTF8BytesToString(ref stGuildPower.szGuildNotice);
			guildInfo.briefInfo.sName = StringHelper.UTF8BytesToString(ref stGuildPower.szGuildName);
			guildInfo.briefInfo.dwSettingMask = stGuildPower.dwSettingMask;
			guildInfo.briefInfo.LevelLimit = stGuildPower.bLimitLevel;
			guildInfo.briefInfo.GradeLimit = stGuildPower.bLimitGrade;
			guildInfo.chairman.stBriefInfo.sName = StringHelper.UTF8BytesToString(ref stGuildPower.szChairManName);
			guildInfo.chairman.stBriefInfo.szHeadUrl = StringHelper.UTF8BytesToString(ref stGuildPower.szChairManHeadUrl);
			guildInfo.chairman.stBriefInfo.dwLevel = stGuildPower.dwChairManLv;
			guildInfo.chairman.stBriefInfo.stVip.score = stGuildPower.stChairManVip.dwScore;
			guildInfo.chairman.stBriefInfo.stVip.level = stGuildPower.stChairManVip.dwCurLevel;
			guildInfo.chairman.stBriefInfo.stVip.headIconId = stGuildPower.stChairManVip.dwHeadIconId;
			guildInfo.star = stGuildPower.dwStar;
			guildInfo.RankInfo.totalRankPoint = stGuildPower.dwTotalRankPoint;
			Singleton<EventRouter>.GetInstance().BroadCastEvent<GuildInfo, int>("Receive_Guild_Search_Success", guildInfo, (int)stSearchGuildRsp.bSearchType);
		}

		[MessageHandler(2242)]
		public static void ReceiveSearchPrepareGuildRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			SCPKG_SEARCH_PREGUILD_RSP stSearchPreGuildRsp = msg.stPkgData.get_stSearchPreGuildRsp();
			if (CGuildSystem.IsError(stSearchPreGuildRsp.bResult))
			{
				return;
			}
			COMDT_PREPARE_GUILD_BRIEF_INFO stPreGuildBriefInfo = stSearchPreGuildRsp.stPreGuildBriefInfo;
			PrepareGuildInfo prepareGuildInfo = new PrepareGuildInfo();
			prepareGuildInfo.stBriefInfo.uulUid = stPreGuildBriefInfo.ullGuildID;
			prepareGuildInfo.stBriefInfo.dwLogicWorldId = stPreGuildBriefInfo.iLogicWorldID;
			prepareGuildInfo.stBriefInfo.sName = StringHelper.UTF8BytesToString(ref stPreGuildBriefInfo.szName);
			prepareGuildInfo.stBriefInfo.bMemCnt = stPreGuildBriefInfo.bMemberNum;
			prepareGuildInfo.stBriefInfo.dwHeadId = stPreGuildBriefInfo.dwHeadID;
			prepareGuildInfo.stBriefInfo.dwRequestTime = stPreGuildBriefInfo.dwRequestTime;
			prepareGuildInfo.stBriefInfo.sBulletin = StringHelper.UTF8BytesToString(ref stPreGuildBriefInfo.szNotice);
			prepareGuildInfo.stBriefInfo.stCreatePlayer.uulUid = stPreGuildBriefInfo.stCreatePlayer.ullUid;
			prepareGuildInfo.stBriefInfo.stCreatePlayer.dwLogicWorldId = stPreGuildBriefInfo.stCreatePlayer.iLogicWorldID;
			prepareGuildInfo.stBriefInfo.stCreatePlayer.dwLevel = stPreGuildBriefInfo.stCreatePlayer.dwLevel;
			prepareGuildInfo.stBriefInfo.stCreatePlayer.sName = StringHelper.UTF8BytesToString(ref stPreGuildBriefInfo.stCreatePlayer.szName);
			prepareGuildInfo.stBriefInfo.stCreatePlayer.szHeadUrl = StringHelper.UTF8BytesToString(ref stPreGuildBriefInfo.stCreatePlayer.szHeadUrl);
			prepareGuildInfo.stBriefInfo.stCreatePlayer.stVip.score = stPreGuildBriefInfo.stCreatePlayer.stVip.dwScore;
			prepareGuildInfo.stBriefInfo.stCreatePlayer.stVip.level = stPreGuildBriefInfo.stCreatePlayer.stVip.dwCurLevel;
			prepareGuildInfo.stBriefInfo.stCreatePlayer.stVip.headIconId = stPreGuildBriefInfo.stCreatePlayer.stVip.dwHeadIconId;
			Singleton<EventRouter>.GetInstance().BroadCastEvent<PrepareGuildInfo, int>("Receive_Search_Prepare_Guild_Success", prepareGuildInfo, (int)stSearchPreGuildRsp.bSearchType);
		}

		[MessageHandler(2244)]
		public static void ReceiveBuildingUpgradeRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			SCPKG_GUILD_BUILDING_UPGRADE_RSP stGuildBuildingUpgradeRsp = msg.stPkgData.get_stGuildBuildingUpgradeRsp();
			if (CGuildSystem.IsError(stGuildBuildingUpgradeRsp.bResult))
			{
				return;
			}
			string text = Singleton<CTextManager>.GetInstance().GetText("Guild_Guild_Building_Upgrade_Success_Tip", new string[]
			{
				CGuildHelper.GetBuildingName((int)stGuildBuildingUpgradeRsp.bBuildingType),
				stGuildBuildingUpgradeRsp.bCurLevel.ToString(),
				stGuildBuildingUpgradeRsp.dwCostMoney.ToString()
			});
			Singleton<CUIManager>.GetInstance().OpenTips(text, false, 1.5f, null, new object[0]);
		}

		[MessageHandler(2245)]
		public static void ReceiveBuildingLevelChangeNtf(CSPkg msg)
		{
			SCPKG_GUILD_BUILDING_LEVEL_CHANGE_NTF stGuildBuildingLvChgNtf = msg.stPkgData.get_stGuildBuildingLvChgNtf();
			GuildBuildingInfo guildBuildingInfo = new GuildBuildingInfo();
			guildBuildingInfo.type = stGuildBuildingLvChgNtf.bBuildingType;
			guildBuildingInfo.level = stGuildBuildingLvChgNtf.bCurLevel;
			Singleton<CGuildSystem>.GetInstance().Model.SetBuildingInfoList(guildBuildingInfo);
			if (guildBuildingInfo.type == 1)
			{
				Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo.briefInfo.bLevel = guildBuildingInfo.level;
				Singleton<CGuildInfoView>.GetInstance().RefreshInfoPanelGuildMemberCount();
				Singleton<CGuildInfoView>.GetInstance().RefreshProfitPanel();
			}
		}

		[MessageHandler(2250)]
		public static void ReceiveGuildAppointPositionRsp(CSPkg msg)
		{
			if (CGuildSystem.IsError(msg.stPkgData.get_stAppointPositionRsp().bResult))
			{
				return;
			}
			Singleton<CUIManager>.GetInstance().OpenTips("Guild_Appoint_Success", true, 1.5f, null, new object[0]);
		}

		[MessageHandler(2251)]
		public static void ReceiveGuildPositionChgNtf(CSPkg msg)
		{
			int bCount = (int)msg.stPkgData.get_stGuildPositionChgNtf().bCount;
			COMDT_MEMBER_POSITION[] astChgInfo = msg.stPkgData.get_stGuildPositionChgNtf().astChgInfo;
			ListView<GuildMemInfo> listMemInfo = Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo.listMemInfo;
			for (int i = 0; i < bCount; i++)
			{
				for (int j = 0; j < listMemInfo.get_Count(); j++)
				{
					if (astChgInfo[i].ullUid == listMemInfo.get_Item(j).stBriefInfo.uulUid)
					{
						listMemInfo.get_Item(j).enPosition = astChgInfo[i].bPosition;
						if (listMemInfo.get_Item(j).enPosition == 3)
						{
							Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo.chairman = listMemInfo.get_Item(j);
						}
						if (astChgInfo[i].ullUid == Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID)
						{
							Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState = astChgInfo[i].bPosition;
						}
					}
				}
			}
			Singleton<CGuildInfoView>.GetInstance().RefreshInfoPanel();
			Singleton<CGuildInfoView>.GetInstance().RefreshMemberPanel();
		}

		[MessageHandler(2253)]
		public static void ReceiveGuildFireMemberRsp(CSPkg msg)
		{
			if (CGuildSystem.IsError(msg.stPkgData.get_stFireGuildMemberRsp().bResult))
			{
				return;
			}
			ulong ullUid = msg.stPkgData.get_stFireGuildMemberRsp().ullUid;
			string sName = Singleton<CGuildSystem>.GetInstance().Model.GetGuildMemberInfoByUid(ullUid).stBriefInfo.sName;
			Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("Guild_Fire_Member_Success", new string[]
			{
				sName
			}), true, 1.5f, null, new object[0]);
		}

		[MessageHandler(2254)]
		public static void ReceiveGuildFireMemberNtf(CSPkg msg)
		{
			if (msg.stPkgData.get_stFireGuildMemberNtf().ullUid == Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID)
			{
				Singleton<CUIManager>.GetInstance().OpenTips("Guild_You_Have_Been_Fired", true, 1.5f, null, new object[0]);
				Singleton<CGuildInfoView>.GetInstance().CloseForm();
				Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState = 0;
				Singleton<EventRouter>.GetInstance().BroadCastEvent("Guild_Leave_Guild");
			}
			else
			{
				ListView<GuildMemInfo> listMemInfo = Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo.listMemInfo;
				for (int i = listMemInfo.get_Count() - 1; i >= 0; i--)
				{
					if (msg.stPkgData.get_stFireGuildMemberNtf().ullUid == listMemInfo.get_Item(i).stBriefInfo.uulUid)
					{
						listMemInfo.RemoveAt(i);
						GuildInfo expr_D7_cp_0 = Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo;
						expr_D7_cp_0.briefInfo.bMemberNum = expr_D7_cp_0.briefInfo.bMemberNum - 1;
						break;
					}
				}
				Singleton<CFriendContoller>.get_instance().model.SetGameFriendGuildState(msg.stPkgData.get_stFireGuildMemberNtf().ullUid, 0);
				Singleton<CGuildInfoView>.GetInstance().RefreshInfoPanel();
				Singleton<CGuildInfoView>.GetInstance().RefreshMemberPanel();
			}
		}

		[MessageHandler(2256)]
		public static void ReceiveGuildSelfCommendRsp(CSPkg msg)
		{
			if (CGuildSystem.IsError(msg.stPkgData.get_stGuildSelfRecommemdRsp().bResult))
			{
				return;
			}
			Singleton<CUIManager>.GetInstance().OpenTips("Guild_Self_Recommend_Success", true, 1.5f, null, new object[0]);
		}

		[MessageHandler(2257)]
		public static void ReceiveGuildSelfCommendNtf(CSPkg msg)
		{
			if (Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_baseGuildInfo.guildState != 3)
			{
				return;
			}
			ulong ullUid = msg.stPkgData.get_stGuildSelfRecommemdNtf().stSelfRecommend.ullUid;
			uint dwTime = msg.stPkgData.get_stGuildSelfRecommemdNtf().stSelfRecommend.dwTime;
			Singleton<CGuildSystem>.GetInstance().Model.AddSelfRecommendInfo(ullUid, dwTime);
		}

		[MessageHandler(2261)]
		public static void ReceiveGuildDonateRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			if (CGuildSystem.IsError(msg.stPkgData.get_stGuildDonateRsp().bResult))
			{
				return;
			}
			string donateSuccessTip = CGuildHelper.GetDonateSuccessTip(Singleton<CGuildModel>.GetInstance().CurrentDonateType);
			Singleton<CUIManager>.GetInstance().OpenTips(donateSuccessTip, false, 1.5f, null, new object[0]);
			SCPKG_GUILD_DONATE_RSP stGuildDonateRsp = msg.stPkgData.get_stGuildDonateRsp();
			Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo.dwGuildMoney = stGuildDonateRsp.dwGuildMoney;
			Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo.dwCoinPool = stGuildDonateRsp.dwCoinPool;
			GuildMemInfo playerGuildMemberInfo = Singleton<CGuildSystem>.GetInstance().Model.GetPlayerGuildMemberInfo();
			if (playerGuildMemberInfo != null)
			{
				playerGuildMemberInfo.DonateCnt = stGuildDonateRsp.bDonateCnt;
				playerGuildMemberInfo.dwConstruct = stGuildDonateRsp.dwConstruct;
				playerGuildMemberInfo.TotalContruct = stGuildDonateRsp.dwTotalConstruct;
			}
		}

		[MessageHandler(2263)]
		public static void ReceiveGetGuildDividendRsp(CSPkg msg)
		{
			if (CGuildSystem.IsError(msg.stPkgData.get_stGetGuildDividendRsp().bResult))
			{
				return;
			}
			uint weekDividend = Singleton<CGuildModel>.GetInstance().GetPlayerGuildMemberInfo().WeekDividend;
			string text = Singleton<CTextManager>.GetInstance().GetText("Guild_Get_Dividend_Success", new string[]
			{
				weekDividend.ToString()
			});
			Singleton<CUIManager>.GetInstance().OpenTips(text, false, 1.5f, null, new object[0]);
		}

		[MessageHandler(2264)]
		public static void ReceiveGuildCrossDayRsp(CSPkg msg)
		{
			SCPKG_GUILD_CROSS_DAY_NTF stGuildCrossDayNtf = msg.stPkgData.get_stGuildCrossDayNtf();
			Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.dwGuildMoney = stGuildCrossDayNtf.dwGuildMoney;
			CGuildHelper.SetHallBuildingLevel(stGuildCrossDayNtf.bBuildingHallLevel);
			Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.dwActive = stGuildCrossDayNtf.dwGuildActive;
			GuildMemInfo playerGuildMemberInfo = Singleton<CGuildModel>.GetInstance().GetPlayerGuildMemberInfo();
			playerGuildMemberInfo.CurrActive = stGuildCrossDayNtf.dwCurrActive;
			playerGuildMemberInfo.DonateCnt = stGuildCrossDayNtf.bDonateCnt;
			playerGuildMemberInfo.RankInfo.maxRankPoint = stGuildCrossDayNtf.dwMaxRankPoint;
			playerGuildMemberInfo.RankInfo.isSigned = false;
			playerGuildMemberInfo.RankInfo.byGameRankPoint = 0u;
			CGuildSystem.s_lastByGameRankpoint = 0u;
		}

		[MessageHandler(2265)]
		public static void ReceiveGuildCrossWeekRsp(CSPkg msg)
		{
			SCPKG_GUILD_CROSS_WEEK_NTF stGuildCrossWeekNtf = msg.stPkgData.get_stGuildCrossWeekNtf();
			Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.dwCoinPool = stGuildCrossWeekNtf.dwCoinPool;
			Singleton<CGuildModel>.GetInstance().GetPlayerGuildMemberInfo().WeekActive = stGuildCrossWeekNtf.dwWeekActive;
			Singleton<CGuildModel>.GetInstance().GetPlayerGuildMemberInfo().WeekDividend = stGuildCrossWeekNtf.dwWeekDividend;
		}

		[MessageHandler(2266)]
		public static void ReceiveMemberTopKDANtf(CSPkg msg)
		{
			SCPKG_MEMBER_TOP_KDA_NTF stMemberTopKDANtf = msg.stPkgData.get_stMemberTopKDANtf();
			GuildMemInfo guildMemberInfoByUid = Singleton<CGuildModel>.GetInstance().GetGuildMemberInfoByUid(stMemberTopKDANtf.ullMemberUid);
			if (guildMemberInfoByUid == null)
			{
				DebugHelper.Assert(false, "CGuildSystem.ReceiveMemberTopKDANtf(): info is null, rsp.ullMemberUid={0}", new object[]
				{
					stMemberTopKDANtf.ullMemberUid
				});
				return;
			}
			guildMemberInfoByUid.RankInfo.killCnt = stMemberTopKDANtf.dwKillCnt;
			guildMemberInfoByUid.RankInfo.deadCnt = stMemberTopKDANtf.dwDeadCnt;
			guildMemberInfoByUid.RankInfo.assistCnt = stMemberTopKDANtf.dwAssistCnt;
		}

		[MessageHandler(2267)]
		public static void ReceiveMemberRankPointNtf(CSPkg msg)
		{
			SCPKG_MEMBER_RANK_POINT_NTF stMemberRankPointNtf = msg.stPkgData.get_stMemberRankPointNtf();
			GuildMemInfo guildMemberInfoByUid = Singleton<CGuildModel>.GetInstance().GetGuildMemberInfoByUid(stMemberRankPointNtf.ullMemberUid);
			if (guildMemberInfoByUid != null)
			{
				guildMemberInfoByUid.RankInfo.maxRankPoint = stMemberRankPointNtf.dwMaxRankPoint;
				guildMemberInfoByUid.RankInfo.totalRankPoint = stMemberRankPointNtf.dwTotalRankPoint;
				guildMemberInfoByUid.RankInfo.weekRankPoint = stMemberRankPointNtf.dwWeekRankPoint;
				if (CGuildHelper.IsSelf(stMemberRankPointNtf.ullMemberUid))
				{
					CGuildSystem.s_lastByGameRankpoint = guildMemberInfoByUid.RankInfo.byGameRankPoint;
				}
				guildMemberInfoByUid.RankInfo.byGameRankPoint = stMemberRankPointNtf.dwGameRP;
			}
			else
			{
				DebugHelper.Assert(false, "CGuildSystem.ReceiveMemberRankPointNtf(): info is null!!!");
			}
			Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.RankInfo.totalRankPoint = stMemberRankPointNtf.dwGuildRankPoint;
			Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.RankInfo.weekRankPoint = stMemberRankPointNtf.dwGuildWeekRankPoint;
		}

		[MessageHandler(2268)]
		public static void ReceiveGuildRankResetNtf(CSPkg msg)
		{
			Singleton<CGuildSystem>.GetInstance().ResetRankpointAllRankInfo();
		}

		[MessageHandler(2215)]
		public static void ReceiveMemberOnlineNtf(CSPkg msg)
		{
			SCPKG_MEMBER_ONLINE_NTF stMemberOnlineNtf = msg.stPkgData.get_stMemberOnlineNtf();
			GuildMemInfo guildMemberInfoByUid = Singleton<CGuildSystem>.GetInstance().Model.GetGuildMemberInfoByUid(stMemberOnlineNtf.ullMemberUid);
			if (guildMemberInfoByUid != null)
			{
				guildMemberInfoByUid.stBriefInfo.dwGameEntity = stMemberOnlineNtf.dwGameEntity;
				guildMemberInfoByUid.LastLoginTime = stMemberOnlineNtf.dwLastLoginTime;
				if (stMemberOnlineNtf.dwGameEntity == 0u)
				{
					guildMemberInfoByUid.GuildMatchInfo.bIsReady = 0;
					Singleton<CGuildMatchSystem>.GetInstance().SetTeamMemberReadyState(stMemberOnlineNtf.ullMemberUid, false);
					Singleton<CGuildMatchSystem>.GetInstance().RefreshTeamList();
				}
			}
		}

		[MessageHandler(2271)]
		public static void ReceiveGuildConstructChgRsp(CSPkg msg)
		{
			GuildMemInfo playerGuildMemberInfo = Singleton<CGuildSystem>.GetInstance().Model.GetPlayerGuildMemberInfo();
			DebugHelper.Assert(playerGuildMemberInfo != null, "CGuildSystem.ReceiveGuildConstructChgRsp(): playerGuildMemberInfo is null!!!");
			if (playerGuildMemberInfo != null)
			{
				playerGuildMemberInfo.dwConstruct = msg.stPkgData.get_stGuildConstructChg().dwCurConstruct;
			}
		}

		[MessageHandler(2273)]
		public static void ReceiveChgGuildHeadIdRsp(CSPkg msg)
		{
			SCPKG_CHG_GUILD_HEADID_RSP stChgGuildHeadIDRsp = msg.stPkgData.get_stChgGuildHeadIDRsp();
			if (CGuildSystem.IsError(stChgGuildHeadIDRsp.bResult))
			{
				return;
			}
			Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo.briefInfo.dwHeadId = stChgGuildHeadIDRsp.dwHeadID;
			Singleton<EventRouter>.GetInstance().BroadCastEvent("Guild_Setting_Modify_Icon_Success");
		}

		[MessageHandler(2275)]
		public static void ReceiveChgGuildNoticeRsp(CSPkg msg)
		{
			SCPKG_CHG_GUILD_NOTICE_RSP stChgGuildNoticeRsp = msg.stPkgData.get_stChgGuildNoticeRsp();
			if (CGuildSystem.IsError(stChgGuildNoticeRsp.bResult))
			{
				return;
			}
			Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo.briefInfo.sBulletin = StringHelper.UTF8BytesToString(ref stChgGuildNoticeRsp.szNotice);
			Singleton<EventRouter>.GetInstance().BroadCastEvent("Guild_Modify_Bulletin_Success");
		}

		[MessageHandler(2277)]
		public static void ReceiveUpgradeGuildByDianQuanRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			if (CGuildSystem.IsError(msg.stPkgData.get_stUpgradeGuildByCouponsRsp().bResult))
			{
				return;
			}
			Singleton<CUIManager>.GetInstance().OpenTips("Guild_Extend_Member_Limit_Success_Tip", true, 1.5f, null, new object[0]);
		}

		[MessageHandler(2279)]
		public static void ReceiveGuildSignInRsp(CSPkg msg)
		{
			if (CGuildSystem.IsError((byte)msg.stPkgData.get_stGuildSignInRsp().iResult))
			{
				return;
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo == null)
			{
				return;
			}
			int num = (int)((masterRoleInfo.m_rankGrade <= 0) ? 1 : masterRoleInfo.m_rankGrade);
			ResRankGradeConf dataByKey = GameDataMgr.rankGradeDatabin.GetDataByKey((long)num);
			if (dataByKey == null)
			{
				return;
			}
			uint dwGuildSignInPoint = dataByKey.dwGuildSignInPoint;
			string text = Singleton<CTextManager>.GetInstance().GetText("Guild_Sign_Success_Tip", new string[]
			{
				dwGuildSignInPoint.ToString()
			});
			Singleton<CGuildInfoView>.GetInstance().OpenSignSuccessForm(text);
			Singleton<EventRouter>.GetInstance().BroadCastEvent<bool>("Guild_Sign_State_Changed", true);
		}

		[MessageHandler(2280)]
		public static void ReceiveGuildSeasonResetNtf(CSPkg msg)
		{
			GuildInfo currentGuildInfo = Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo;
			if (currentGuildInfo == null)
			{
				return;
			}
			SCPKG_GUILD_SEASON_RESET_NTF stGuildSeasonResetNtf = msg.stPkgData.get_stGuildSeasonResetNtf();
			currentGuildInfo.RankInfo.totalRankPoint = 0u;
			currentGuildInfo.GuildMatchInfo.dwScore = stGuildSeasonResetNtf.dwGuildMatchScore;
			currentGuildInfo.GuildMatchInfo.dwLastSeasonRankNo = stGuildSeasonResetNtf.dwGuildMatchLastSeasonRankNo;
			for (int i = 0; i < currentGuildInfo.listMemInfo.get_Count(); i++)
			{
				GuildMemInfo guildMemInfo = currentGuildInfo.listMemInfo.get_Item(i);
				guildMemInfo.RankInfo.totalRankPoint = 0u;
				guildMemInfo.GuildMatchInfo.dwScore = 0u;
			}
		}

		[MessageHandler(5331)]
		public static void ReceiveGuildWeekResetNtf(CSPkg msg)
		{
			GuildInfo currentGuildInfo = Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo;
			if (currentGuildInfo == null)
			{
				return;
			}
			SCPKG_GUILD_MATCH_WEEK_RESET_NTF stGuildMatchWeekResetNtf = msg.stPkgData.get_stGuildMatchWeekResetNtf();
			currentGuildInfo.GuildMatchInfo.dwWeekScore = 0u;
			currentGuildInfo.GuildMatchInfo.dwLastRankNo = stGuildMatchWeekResetNtf.dwGuildMatchLastRankNo;
			for (int i = 0; i < currentGuildInfo.listMemInfo.get_Count(); i++)
			{
				GuildMemInfo guildMemInfo = currentGuildInfo.listMemInfo.get_Item(i);
				guildMemInfo.GuildMatchInfo.bContinueWin = 0;
				guildMemInfo.GuildMatchInfo.dwWeekScore = 0u;
				guildMemInfo.GuildMatchInfo.bWeekMatchCnt = 0;
			}
		}

		[MessageHandler(2614)]
		public static void ReceiveGetRankListBySpecialScoreRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			SCPKG_GET_RANKLIST_BY_SPECIAL_SCORE_RSP stGetRankListBySpecialScoreRsp = msg.stPkgData.get_stGetRankListBySpecialScoreRsp();
			if (stGetRankListBySpecialScoreRsp.bNumberType != 16)
			{
				return;
			}
			CGuildSystem.RefreshRankpointRankInfoList(enGuildRankpointRankListType.SeasonSelf, stGetRankListBySpecialScoreRsp.dwItemNum, stGetRankListBySpecialScoreRsp.astItemDetail);
			Singleton<CGuildInfoView>.GetInstance().RefreshRankpointSeasonRankList(null);
		}

		[MessageHandler(2616)]
		public static void ReceiveGetPlayerGuildRankInfoRsp(CSPkg msg)
		{
			SCPKG_GET_SPECIAL_GUILD_RANK_INFO_RSP stGetSpecialGuildRankInfoRsp = msg.stPkgData.get_stGetSpecialGuildRankInfoRsp();
			if (stGetSpecialGuildRankInfoRsp.bNumberType == 16)
			{
				RankpointRankInfo info = CGuildSystem.CreateRankpointRankInfo(stGetSpecialGuildRankInfoRsp.stItemDetail, enGuildRankpointRankListType.SeasonSelf);
				Singleton<CGuildInfoView>.GetInstance().SetRankpointPlayerGuildRank(info, null);
			}
			else if (stGetSpecialGuildRankInfoRsp.bNumberType >= 51 || stGetSpecialGuildRankInfoRsp.bNumberType <= 59)
			{
				Singleton<EventRouter>.GetInstance().BroadCastEvent<SCPKG_GET_SPECIAL_GUILD_RANK_INFO_RSP>("UnionRank_Get_Rank_Team_Account_Info", stGetSpecialGuildRankInfoRsp);
			}
		}

		[MessageHandler(1431)]
		public static void ReceiveSendGuildMailRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<CUIManager>.GetInstance().CloseForm(CMailSys.MAIL_WRITE_FORM_PATH);
			SCPKG_SEND_GUILD_MAIL_RSP stSendGuildMailRsp = msg.stPkgData.get_stSendGuildMailRsp();
			if (stSendGuildMailRsp.iResult == 0)
			{
				Singleton<CUIManager>.GetInstance().OpenTips("Mail_Send_Success", true, 1.5f, null, new object[0]);
				CGuildHelper.SetSendGuildMailCnt(stSendGuildMailRsp.bSendGuildMailCnt);
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips(Utility.ProtErrCodeToStr(1431, stSendGuildMailRsp.iResult), false, 1.5f, null, new object[0]);
			}
		}

		[MessageHandler(2286)]
		public static void ReceiveGetGuildEventRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<CGuildInfoView>.GetInstance().OpenLogForm(msg.stPkgData.get_stGuildEventRsp());
		}

		[MessageHandler(2288)]
		public static void ReceiveSendGuildRecruitRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			if (CGuildSystem.IsError(msg.stPkgData.get_stSendGuildRecruitRsp().bErrCode))
			{
				return;
			}
			Singleton<CUIManager>.GetInstance().OpenTips("Guild_Recruit_Send_Recruit_Success", true, 1.5f, null, new object[0]);
		}

		private static void SetLocalMemberGuildMatchInfo(COMDT_MEMBER_GUILD_MATCH_INFO localInfo, COMDT_MEMBER_GUILD_MATCH_INFO protocolInfo)
		{
			localInfo.bIsLeader = protocolInfo.bIsLeader;
			localInfo.ullTeamLeaderUid = protocolInfo.ullTeamLeaderUid;
			localInfo.dwScore = protocolInfo.dwScore;
			localInfo.bContinueWin = protocolInfo.bContinueWin;
			localInfo.dwWeekScore = protocolInfo.dwWeekScore;
			localInfo.bIsReady = protocolInfo.bIsReady;
			localInfo.bWeekMatchCnt = protocolInfo.bWeekMatchCnt;
		}

		private static void SetLocalGuildMatchInfo(COMDT_GUILD_MATCH_INFO localInfo, COMDT_GUILD_MATCH_INFO protocolInfo)
		{
			localInfo.dwScore = protocolInfo.dwScore;
			localInfo.dwWeekScore = protocolInfo.dwWeekScore;
			localInfo.dwLastRankNo = protocolInfo.dwLastRankNo;
			localInfo.dwUpdRankNoTime = protocolInfo.dwUpdRankNoTime;
			localInfo.dwLastSeasonRankNo = protocolInfo.dwLastSeasonRankNo;
		}

		private static void SetLocalGuildMatchObInfo(ListView<COMDT_GUILD_MATCH_OB_INFO> localInfo, COMDT_GUILD_INFO protocolGuildInfo)
		{
			localInfo.Clear();
			int bOBMatchCnt = (int)protocolGuildInfo.bOBMatchCnt;
			COMDT_GUILD_MATCH_OB_INFO[] astGuildMatchOBInfo = protocolGuildInfo.astGuildMatchOBInfo;
			for (int i = 0; i < bOBMatchCnt; i++)
			{
				localInfo.Add(astGuildMatchOBInfo[i]);
			}
		}

		private static stGuildBriefInfo GetLocalGuildBriefInfo(COMDT_GUILD_BRIEF_INFO protocolInfo)
		{
			return new stGuildBriefInfo
			{
				uulUid = protocolInfo.ullGuildID,
				sName = StringHelper.UTF8BytesToString(ref protocolInfo.szName),
				bLevel = protocolInfo.bLevel,
				bMemberNum = protocolInfo.bMemberNum,
				dwHeadId = protocolInfo.dwHeadID,
				sBulletin = StringHelper.UTF8BytesToString(ref protocolInfo.szNotice),
				dwSettingMask = protocolInfo.dwSettingMask,
				LevelLimit = protocolInfo.bLevelLimit,
				GradeLimit = protocolInfo.bGradeLimit
			};
		}

		[MessageHandler(2282)]
		public static void ReceiveGetGroupGuildIdNtf(CSPkg msg)
		{
			GuildInfo currentGuildInfo = Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo;
			if (currentGuildInfo != null)
			{
				currentGuildInfo.groupGuildId = msg.stPkgData.get_stGetGroupGuildIDNtf().dwGroupGuildID;
				Debug.Log("Got 32 bit guild id from server: " + currentGuildInfo.groupGuildId);
				if (CGuildSystem.HasManageQQGroupAuthority())
				{
					Singleton<CGuildInfoView>.GetInstance().BindQQGroup();
				}
			}
		}

		[MessageHandler(2284)]
		public static void ReceiveSetGuildGroupOpenIdNtf(CSPkg msg)
		{
			GuildInfo currentGuildInfo = Singleton<CGuildSystem>.GetInstance().Model.CurrentGuildInfo;
			if (currentGuildInfo != null)
			{
				currentGuildInfo.groupOpenId = StringHelper.UTF8BytesToString(ref msg.stPkgData.get_stSetGuildGroupOpenIDNtf().szGroupOpenID);
				Debug.Log("Got guildOpenId from server: " + currentGuildInfo.groupOpenId);
			}
		}
	}
}
