using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.UI;
using CSProtocol;
using ResData;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	[MessageHandlerClass]
	public class CGuildMatchSystem : Singleton<CGuildMatchSystem>
	{
		public enum enGuildMatchFormWidget
		{
			GuildHead_Image,
			GuildName_Text,
			GuildMatchScore_Text,
			Team_List,
			RankTab_List,
			OnlineNum_Text,
			GuildScore_List,
			MemberScore_List,
			Invite_List,
			SelfMemberRank_Panel,
			SelfMemberRankRank_Panel,
			SelfMemberRankHead_Panel,
			SelfMemberRankName_Panel,
			SelfMemberRankScore_Panel,
			MatchOpenTime_Text,
			RefreshGameState_Timer,
			LeftMatchCnt_Text,
			SelfGuildRank_Panel,
			SelfGuildRankRank_Panel,
			SelfGuildRankHead_Panel,
			SelfGuildRankName_Panel,
			SelfGuildRankScore_Panel,
			RankSubTitle_Panel,
			RankSubTitle_Text,
			RankSubTitleSlider_Text,
			RankSubTitle_Slider,
			MoreLeader_Text
		}

		public enum enGuildMatchRecordFormWidget
		{
			Record_List
		}

		public enum enRankTab
		{
			GuildScore,
			MemberScore,
			MemberInvite,
			Count
		}

		public class TeamPlayerInfo
		{
			public ulong Uid;

			public string Name;

			public string HeadUrl;

			public bool IsReady;

			public TeamPlayerInfo(ulong uid, string name, string headUrl, bool isReady)
			{
				this.Uid = uid;
				this.Name = name;
				this.HeadUrl = headUrl;
				this.IsReady = isReady;
			}
		}

		public const int TeamSlotCount = 5;

		public const int InviteMessageBoxAutoCloseTimeSeconds = 10;

		public const int RemindButtonCdSeconds = 10;

		public const int NeedRequestNewRecordTimeMilliSeconds = 300000;

		private const int GuildMatchRuleTextIndex = 15;

		private bool m_isNeedRequestNewRecord = true;

		public static readonly string GuildMatchFormPath = "UGUI/Form/System/Guild/Form_Guild_Match.prefab";

		public static readonly string GuildMatchRecordFormPath = "UGUI/Form/System/Guild/Form_Guild_Match_Record.prefab";

		private CUIFormScript m_form;

		private CUIFormScript m_matchRecordForm;

		private List<KeyValuePair<ulong, ListView<CGuildMatchSystem.TeamPlayerInfo>>> m_teamInfos;

		private ListView<GuildMemInfo> m_guildMemberInviteList;

		private ListView<GuildMemInfo> m_guildMemberScoreList;

		private CSDT_RANKING_LIST_ITEM_INFO[] m_guildSeasonScores;

		private CSDT_RANKING_LIST_ITEM_INFO[] m_guildWeekScores;

		private COMDT_GUILD_MATCH_HISTORY_INFO[] m_matchRecords;

		public bool m_isGuildMatchBtnClicked;

		public override void Init()
		{
			base.Init();
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_OnMatchFormOpened, new CUIEventManager.OnUIEventHandler(this.OnMatchFormOpened));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_OnMatchFormClosed, new CUIEventManager.OnUIEventHandler(this.OnMatchFormClosed));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_OpenMatchForm, new CUIEventManager.OnUIEventHandler(this.OnOpenMatchForm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_OpenMatchRecordForm, new CUIEventManager.OnUIEventHandler(this.OnOpenMatchRecordForm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_StartGame, new CUIEventManager.OnUIEventHandler(this.OnStartGame));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_OBGame, new CUIEventManager.OnUIEventHandler(this.OnOBGame));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_ReadyGame, new CUIEventManager.OnUIEventHandler(this.OnReadyGame));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_CancelReadyGame, new CUIEventManager.OnUIEventHandler(this.OnCancelReadyGame));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_RankTabChanged, new CUIEventManager.OnUIEventHandler(this.OnRankTabChanged));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_GuildScoreListElementEnabled, new CUIEventManager.OnUIEventHandler(this.OnGuildScoreListElementEnabled));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_MemberScoreListElementEnabled, new CUIEventManager.OnUIEventHandler(this.OnMemberScoreListElementEnabled));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_MemberInviteListElementEnabled, new CUIEventManager.OnUIEventHandler(this.OnMemberInviteListElementEnabled));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_Invite, new CUIEventManager.OnUIEventHandler(this.OnInvite));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_Kick, new CUIEventManager.OnUIEventHandler(this.OnKick));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_KickConfirm, new CUIEventManager.OnUIEventHandler(this.OnKickConfirm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_AppointOrCancelLeader, new CUIEventManager.OnUIEventHandler(this.OnAppointOrCancelLeader));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_AppointOrCancelLeaderConfirm, new CUIEventManager.OnUIEventHandler(this.OnAppointOrCancelLeaderConfirm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_Accept_Invite, new CUIEventManager.OnUIEventHandler(this.OnAcceptInvite));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_Refuse_Invite, new CUIEventManager.OnUIEventHandler(this.OnRefuseInvite));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_RefreshGameStateTimeout, new CUIEventManager.OnUIEventHandler(this.OnRefreshGameStateTimeout));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_ObWaitingTimeout, new CUIEventManager.OnUIEventHandler(this.OnObWaitingTimeout));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_RecordListElementEnabled, new CUIEventManager.OnUIEventHandler(this.OnRecordListElementEnabled));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_RankSubTitleSliderValueChanged, new CUIEventManager.OnUIEventHandler(this.OnSubRankSliderValueChanged));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_OpenMatchFormAndReadyGame, new CUIEventManager.OnUIEventHandler(this.OnOpenMatchFormAndReadyGame));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_Remind_Ready, new CUIEventManager.OnUIEventHandler(this.OnRemindReady));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_Open_Rule, new CUIEventManager.OnUIEventHandler(this.OnOpenRule));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_Team_List_Element_Enabled, new CUIEventManager.OnUIEventHandler(this.OnTeamListElementEnabled));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_InviteConfirm, new CUIEventManager.OnUIEventHandler(this.OnInviteConfirm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Guild_Match_RemindButtonCdOver, new CUIEventManager.OnUIEventHandler(this.OnRemindButtonCdOver));
			Singleton<EventRouter>.GetInstance().AddEventHandler<SCPKG_GET_RANKING_LIST_RSP>("Guild_Get_Guild_Match_Season_Rank", new Action<SCPKG_GET_RANKING_LIST_RSP>(this.OnGetGuildMatchSeasonRank));
			Singleton<EventRouter>.GetInstance().AddEventHandler<SCPKG_GET_RANKING_LIST_RSP>("Guild_Get_Guild_Match_Week_Rank", new Action<SCPKG_GET_RANKING_LIST_RSP>(this.OnGetGuildMatchWeekRank));
		}

		public void Clear()
		{
			if (this.m_teamInfos != null)
			{
				this.m_teamInfos.Clear();
				this.m_teamInfos = null;
			}
			if (this.m_guildMemberInviteList != null)
			{
				this.m_guildMemberInviteList.Clear();
				this.m_guildMemberInviteList = null;
			}
			if (this.m_guildMemberScoreList != null)
			{
				this.m_guildMemberScoreList.Clear();
				this.m_guildMemberScoreList = null;
			}
			this.m_guildSeasonScores = null;
			this.m_guildWeekScores = null;
			this.m_matchRecords = null;
			this.m_isNeedRequestNewRecord = true;
		}

		public void CreateGuildMatchAllTeams()
		{
			if (this.m_teamInfos != null)
			{
				this.m_teamInfos.Clear();
				this.m_teamInfos = null;
			}
			this.m_teamInfos = new List<KeyValuePair<ulong, ListView<CGuildMatchSystem.TeamPlayerInfo>>>();
			ListView<GuildMemInfo> guildMemberInfos = CGuildHelper.GetGuildMemberInfos();
			if (guildMemberInfos == null)
			{
				return;
			}
			for (int i = 0; i < guildMemberInfos.get_Count(); i++)
			{
				if (guildMemberInfos.get_Item(i).GuildMatchInfo.ullTeamLeaderUid > 0uL)
				{
					bool flag = false;
					for (int j = 0; j < this.m_teamInfos.get_Count(); j++)
					{
						if (this.m_teamInfos.get_Item(j).get_Key() == guildMemberInfos.get_Item(i).GuildMatchInfo.ullTeamLeaderUid)
						{
							if (this.IsTeamLeader(guildMemberInfos.get_Item(i).stBriefInfo.uulUid, this.m_teamInfos.get_Item(j).get_Key()))
							{
								CGuildMatchSystem.TeamPlayerInfo teamPlayerInfo = this.CreateTeamPlayerInfoObj(guildMemberInfos.get_Item(i));
								this.m_teamInfos.get_Item(j).get_Value().Insert(0, teamPlayerInfo);
							}
							else if (!this.FindAndReplaceEmptyPlayerSlot(this.m_teamInfos.get_Item(j).get_Value(), guildMemberInfos.get_Item(i)))
							{
								CGuildMatchSystem.TeamPlayerInfo teamPlayerInfo2 = this.CreateTeamPlayerInfoObj(guildMemberInfos.get_Item(i));
								if (this.m_teamInfos.get_Item(j).get_Value().get_Count() < 5)
								{
									this.m_teamInfos.get_Item(j).get_Value().Add(teamPlayerInfo2);
								}
							}
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.CreateNewTeam(guildMemberInfos.get_Item(i));
					}
				}
			}
		}

		private void CreateNewTeam(GuildMemInfo guildMemberInfo)
		{
			if (guildMemberInfo == null || this.m_teamInfos == null)
			{
				return;
			}
			ListView<CGuildMatchSystem.TeamPlayerInfo> listView = new ListView<CGuildMatchSystem.TeamPlayerInfo>();
			listView.Add(this.CreateTeamPlayerInfoObj(guildMemberInfo));
			KeyValuePair<ulong, ListView<CGuildMatchSystem.TeamPlayerInfo>> keyValuePair = new KeyValuePair<ulong, ListView<CGuildMatchSystem.TeamPlayerInfo>>(guildMemberInfo.GuildMatchInfo.ullTeamLeaderUid, listView);
			if (this.IsSameTeamWithSelf(keyValuePair.get_Key()))
			{
				this.m_teamInfos.Insert(0, keyValuePair);
			}
			else
			{
				this.m_teamInfos.Add(keyValuePair);
			}
		}

		private bool IsTeamLeader(ulong playerUid, ulong teamLeaderUid)
		{
			return playerUid == teamLeaderUid;
		}

		private bool IsSameTeamWithSelf(ulong playerTeamLeaderUid)
		{
			return playerTeamLeaderUid == CGuildHelper.GetPlayerGuildMemberInfo().GuildMatchInfo.ullTeamLeaderUid;
		}

		private bool IsSelfBelongedTeamLeader()
		{
			return Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID == CGuildHelper.GetPlayerGuildMemberInfo().GuildMatchInfo.ullTeamLeaderUid;
		}

		public bool IsSelfInAnyTeam()
		{
			ulong playerUllUID = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID;
			if (this.m_teamInfos != null)
			{
				for (int i = 0; i < this.m_teamInfos.get_Count(); i++)
				{
					for (int j = 0; j < this.m_teamInfos.get_Item(i).get_Value().get_Count(); j++)
					{
						if (this.m_teamInfos.get_Item(i).get_Value().get_Item(j).Uid == playerUllUID)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		private bool IsReadyForGame(ListView<CGuildMatchSystem.TeamPlayerInfo> teamPlayerInfos, ulong playerUid)
		{
			if (teamPlayerInfos != null)
			{
				for (int i = 0; i < teamPlayerInfos.get_Count(); i++)
				{
					if (teamPlayerInfos.get_Item(i).Uid == playerUid)
					{
						return teamPlayerInfos.get_Item(i).IsReady;
					}
				}
			}
			return false;
		}

		private bool IsTeamAllPlayerReadyForGame(ListView<CGuildMatchSystem.TeamPlayerInfo> teamPlayerInfos, ulong teamLeaderUid)
		{
			if (teamPlayerInfos == null)
			{
				return false;
			}
			if (teamPlayerInfos.get_Count() < 5)
			{
				return false;
			}
			for (int i = 0; i < 5; i++)
			{
				if (teamPlayerInfos.get_Item(i).Uid == 0uL || (!teamPlayerInfos.get_Item(i).IsReady && !this.IsTeamLeader(teamPlayerInfos.get_Item(i).Uid, teamLeaderUid)))
				{
					return false;
				}
			}
			return true;
		}

		public bool IsInTeam(ulong playerTeamLeaderUid, ulong teamLeaderUid)
		{
			return playerTeamLeaderUid == teamLeaderUid;
		}

		private void AddMemberToTeam(ulong memberNewTeamLeaderUid, GuildMemInfo memberInfo)
		{
			if (this.m_teamInfos == null || memberInfo == null)
			{
				return;
			}
			for (int i = 0; i < this.m_teamInfos.get_Count(); i++)
			{
				if (memberNewTeamLeaderUid == this.m_teamInfos.get_Item(i).get_Key())
				{
					if (!this.FindAndReplaceEmptyPlayerSlot(this.m_teamInfos.get_Item(i).get_Value(), memberInfo))
					{
						CGuildMatchSystem.TeamPlayerInfo teamPlayerInfo = this.CreateTeamPlayerInfoObj(memberInfo);
						if (this.m_teamInfos.get_Item(i).get_Value().get_Count() < 5)
						{
							this.m_teamInfos.get_Item(i).get_Value().Add(teamPlayerInfo);
						}
					}
					if (CGuildHelper.IsSelf(memberInfo.stBriefInfo.uulUid) && i != 0)
					{
						KeyValuePair<ulong, ListView<CGuildMatchSystem.TeamPlayerInfo>> keyValuePair = this.m_teamInfos.get_Item(i);
						this.m_teamInfos.RemoveAt(i);
						this.m_teamInfos.Insert(0, keyValuePair);
					}
					return;
				}
			}
		}

		private bool FindAndReplaceEmptyPlayerSlot(ListView<CGuildMatchSystem.TeamPlayerInfo> teamPlayerInfos, GuildMemInfo memberInfo)
		{
			for (int i = 0; i < teamPlayerInfos.get_Count(); i++)
			{
				if (teamPlayerInfos.get_Item(i).Uid == 0uL)
				{
					teamPlayerInfos.get_Item(i).Uid = memberInfo.stBriefInfo.uulUid;
					teamPlayerInfos.get_Item(i).Name = memberInfo.stBriefInfo.sName;
					teamPlayerInfos.get_Item(i).HeadUrl = memberInfo.stBriefInfo.szHeadUrl;
					teamPlayerInfos.get_Item(i).IsReady = Convert.ToBoolean(memberInfo.GuildMatchInfo.bIsReady);
					return true;
				}
			}
			return false;
		}

		public void SetTeamInfo(SCPKG_GUILD_MATCH_MEMBER_CHG_NTF ntf)
		{
			if (this.m_teamInfos == null)
			{
				DebugHelper.Assert(false, "m_teamInfos is null!!!");
				return;
			}
			for (int i = 0; i < (int)ntf.bCnt; i++)
			{
				ulong ullUid = ntf.astChgInfo[i].ullUid;
				ulong ullTeamLeaderUid = ntf.astChgInfo[i].ullTeamLeaderUid;
				this.RemoveMemberFromOldTeam(ullUid);
				if (ullUid == ullTeamLeaderUid)
				{
					this.CreateNewTeam(CGuildHelper.GetGuildMemberInfoByUid(ullUid));
				}
				else if (ullTeamLeaderUid > 0uL)
				{
					this.AddMemberToTeam(ullTeamLeaderUid, CGuildHelper.GetGuildMemberInfoByUid(ullUid));
				}
			}
		}

		private bool IsNeedRefreshRankTab()
		{
			if (this.m_form != null)
			{
				CUIListScript component = this.m_form.GetWidget(4).GetComponent<CUIListScript>();
				int elementAmount = component.GetElementAmount();
				if ((elementAmount == 2 && this.IsSelfBelongedTeamLeader()) || (elementAmount == 3 && !this.IsSelfBelongedTeamLeader()))
				{
					return true;
				}
			}
			return false;
		}

		private void RemoveMemberFromOldTeam(ulong memberUid)
		{
			if (this.m_teamInfos == null)
			{
				return;
			}
			for (int i = this.m_teamInfos.get_Count() - 1; i >= 0; i--)
			{
				if (this.IsTeamLeader(memberUid, this.m_teamInfos.get_Item(i).get_Key()))
				{
					this.m_teamInfos.RemoveAt(i);
					this.PromptKickedFromTeamTip(memberUid);
				}
				else
				{
					for (int j = 0; j < this.m_teamInfos.get_Item(i).get_Value().get_Count(); j++)
					{
						if (memberUid == this.m_teamInfos.get_Item(i).get_Value().get_Item(j).Uid)
						{
							this.RemoveMemberFromTeam(this.m_teamInfos.get_Item(i).get_Value().get_Item(j));
							this.PromptKickedFromTeamTip(memberUid);
						}
					}
				}
			}
		}

		private void PromptKickedFromTeamTip(ulong memberUid)
		{
			if (CGuildHelper.IsSelf(memberUid) && Utility.IsCanShowPrompt())
			{
				Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("GuildMatch_Kicked_Tip"), true, 1.5f, null, new object[0]);
			}
		}

		public void SetTeamInfo(SCPKG_CHG_GUILD_MATCH_LEADER_NTF ntf)
		{
			if (this.m_teamInfos == null)
			{
				DebugHelper.Assert(false, "m_teamInfos is null!!!");
				return;
			}
			if (ntf.ullTeamLeaderUid > 0uL)
			{
				for (int i = 0; i < this.m_teamInfos.get_Count(); i++)
				{
					if (this.IsTeamLeader(ntf.ullTeamLeaderUid, this.m_teamInfos.get_Item(i).get_Key()))
					{
						return;
					}
				}
				GuildMemInfo guildMemberInfoByUid = CGuildHelper.GetGuildMemberInfoByUid(ntf.ullUid);
				this.CreateNewTeam(guildMemberInfoByUid);
			}
			else
			{
				for (int j = 0; j < this.m_teamInfos.get_Count(); j++)
				{
					if (this.IsTeamLeader(ntf.ullUid, this.m_teamInfos.get_Item(j).get_Key()))
					{
						this.m_teamInfos.RemoveAt(j);
						break;
					}
				}
			}
		}

		public void SetTeamMemberReadyState(SCPKG_SET_GUILD_MATCH_READY_NTF ntf)
		{
			if (this.m_teamInfos == null)
			{
				DebugHelper.Assert(false, "m_teamInfos is null!!!");
				return;
			}
			bool flag = false;
			for (int i = 0; i < (int)ntf.bCnt; i++)
			{
				for (int j = 0; j < this.m_teamInfos.get_Count(); j++)
				{
					for (int k = 0; k < this.m_teamInfos.get_Item(j).get_Value().get_Count(); k++)
					{
						if (this.m_teamInfos.get_Item(j).get_Value().get_Item(k).Uid == ntf.astInfo[i].ullUid)
						{
							this.m_teamInfos.get_Item(j).get_Value().get_Item(k).IsReady = Convert.ToBoolean(ntf.astInfo[i].bIsReady);
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
		}

		public void SetTeamMemberReadyState(ulong teamMemberUid, bool isReady)
		{
			if (this.m_teamInfos == null)
			{
				DebugHelper.Assert(false, "m_teamInfos is null!!!");
				return;
			}
			for (int i = 0; i < this.m_teamInfos.get_Count(); i++)
			{
				for (int j = 0; j < this.m_teamInfos.get_Item(i).get_Value().get_Count(); j++)
				{
					if (this.m_teamInfos.get_Item(i).get_Value().get_Item(j).Uid == teamMemberUid)
					{
						this.m_teamInfos.get_Item(i).get_Value().get_Item(j).IsReady = isReady;
						return;
					}
				}
			}
		}

		private void RemoveMemberFromTeam(CGuildMatchSystem.TeamPlayerInfo teamPlayerInfo)
		{
			teamPlayerInfo.Uid = 0uL;
			teamPlayerInfo.Name = string.Empty;
			teamPlayerInfo.HeadUrl = string.Empty;
			teamPlayerInfo.IsReady = false;
		}

		private void OnMatchFormOpened(CUIEvent uiEvent)
		{
			CChatUT.EnterGuildMatch();
		}

		private void OnMatchFormClosed(CUIEvent uiEvent)
		{
			CChatUT.LeaveGuildMatch();
		}

		private void OnOpenMatchForm(CUIEvent uiEvent)
		{
			Singleton<CBattleGuideManager>.get_instance().OpenBannerDlgByBannerGuideId(11u, null, false);
			this.m_isGuildMatchBtnClicked = true;
			Singleton<CGuildInfoView>.GetInstance().DelGuildMatchBtnNewFlag();
			this.OpenMatchForm(false);
		}

		private void OnOpenMatchRecordForm(CUIEvent uiEvent)
		{
			if (this.m_isNeedRequestNewRecord)
			{
				this.m_isNeedRequestNewRecord = false;
				Singleton<CTimerManager>.GetInstance().AddTimer(300000, 1, new CTimer.OnTimeUpHandler(this.OnRequestNewRecordTimeout));
				this.RequestGetGuildMatchHistory();
			}
			else
			{
				this.OpenMatchRecordForm();
			}
		}

		private void OnRequestNewRecordTimeout(int timerSequence)
		{
			this.m_isNeedRequestNewRecord = true;
		}

		private void OnStartGame(CUIEvent uiEvent)
		{
			GuildMemInfo playerGuildMemberInfo = CGuildHelper.GetPlayerGuildMemberInfo();
			if (CGuildHelper.IsGuildMatchReachMatchCntLimit((int)playerGuildMemberInfo.GuildMatchInfo.bWeekMatchCnt))
			{
				Singleton<CUIManager>.GetInstance().OpenTips(this.GetReachMatchCntLimitTip(), false, 1.5f, null, new object[0]);
				return;
			}
			this.RequestStartGuildMatch();
		}

		private void OnOBGame(CUIEvent uiEvent)
		{
			this.RequestObGuildMatch(uiEvent.m_eventParams.commonUInt64Param1);
		}

		private void OnReadyGame(CUIEvent uiEvent)
		{
			GuildMemInfo playerGuildMemberInfo = CGuildHelper.GetPlayerGuildMemberInfo();
			if (CGuildHelper.IsGuildMatchReachMatchCntLimit((int)playerGuildMemberInfo.GuildMatchInfo.bWeekMatchCnt))
			{
				Singleton<CUIManager>.GetInstance().OpenTips(this.GetReachMatchCntLimitTip(), false, 1.5f, null, new object[0]);
				return;
			}
			if (!this.IsInGuildMatchTime())
			{
				Singleton<CUIManager>.GetInstance().OpenTips("GuildMatch_Not_Open", true, 1.5f, null, new object[0]);
			}
			this.RequestSetGuildMatchReady(true);
		}

		private void OpenMatchForm(bool isReadyGame = false)
		{
			this.m_form = Singleton<CUIManager>.GetInstance().OpenForm(CGuildMatchSystem.GuildMatchFormPath, false, true);
			this.RefreshGuildMatchForm();
			this.RequestGetGuildMatchSeasonRank();
			this.RequestGetGuildMatchWeekRank();
			if (isReadyGame)
			{
				Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Guild_Match_ReadyGame);
			}
		}

		private void OpenMatchRecordForm()
		{
			this.m_matchRecordForm = Singleton<CUIManager>.GetInstance().OpenForm(CGuildMatchSystem.GuildMatchRecordFormPath, false, true);
			this.RefreshMatchRecordForm();
		}

		private string GetReachMatchCntLimitTip()
		{
			return Singleton<CTextManager>.GetInstance().GetText((!CGuildHelper.IsGuildMatchLeaderPosition(Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID)) ? "GuildMatch_Normal_Player_Match_Times_Reach_Limit" : "GuildMatch_Leader_Match_Times_Reach_Limit");
		}

		private void OnCancelReadyGame(CUIEvent uiEvent)
		{
			this.RequestSetGuildMatchReady(false);
		}

		private void OnRankTabChanged(CUIEvent uiEvent)
		{
			CUIFormScript srcFormScript = uiEvent.m_srcFormScript;
			if (srcFormScript == null)
			{
				return;
			}
			CUIListScript component = srcFormScript.GetWidget(4).GetComponent<CUIListScript>();
			GameObject widget = srcFormScript.GetWidget(6);
			GameObject widget2 = srcFormScript.GetWidget(7);
			GameObject widget3 = srcFormScript.GetWidget(8);
			GameObject widget4 = srcFormScript.GetWidget(17);
			GameObject widget5 = srcFormScript.GetWidget(9);
			GameObject widget6 = srcFormScript.GetWidget(22);
			switch (component.GetSelectedIndex())
			{
			case 0:
				widget.CustomSetActive(true);
				widget2.CustomSetActive(false);
				widget3.CustomSetActive(false);
				widget4.CustomSetActive(true);
				widget5.CustomSetActive(false);
				widget6.CustomSetActive(true);
				this.RefreshGuildScoreRankList();
				this.SetSelfGuildScorePanel(widget4);
				this.RefreshSubRankPanel(CGuildMatchSystem.enRankTab.GuildScore);
				break;
			case 1:
				widget2.CustomSetActive(true);
				widget.CustomSetActive(false);
				widget3.CustomSetActive(false);
				widget4.CustomSetActive(false);
				widget5.CustomSetActive(true);
				widget6.CustomSetActive(true);
				this.RefreshMemberScoreRankList();
				this.SetMemberScoreListElement(widget5, this.GetSelfIndexInGuildMemberScoreList());
				this.RefreshSubRankPanel(CGuildMatchSystem.enRankTab.MemberScore);
				break;
			case 2:
				widget3.CustomSetActive(true);
				widget.CustomSetActive(false);
				widget2.CustomSetActive(false);
				widget4.CustomSetActive(false);
				widget5.CustomSetActive(false);
				widget6.CustomSetActive(false);
				break;
			}
		}

		private void RefreshGuildScoreRankList()
		{
			if (this.m_form == null)
			{
				return;
			}
			CUIListScript component = this.m_form.GetWidget(6).GetComponent<CUIListScript>();
			CSDT_RANKING_LIST_ITEM_INFO[] guildScoreRankInfo = this.GetGuildScoreRankInfo();
			int elementAmount = (guildScoreRankInfo != null) ? guildScoreRankInfo.Length : 0;
			component.SetElementAmount(elementAmount);
		}

		private void RefreshMemberScoreRankList()
		{
			if (this.m_form == null)
			{
				return;
			}
			if (this.m_guildMemberScoreList == null)
			{
				this.m_guildMemberScoreList = new ListView<GuildMemInfo>();
			}
			this.m_guildMemberScoreList.Clear();
			ListView<GuildMemInfo> guildMemberInfos = CGuildHelper.GetGuildMemberInfos();
			this.m_guildMemberScoreList.AddRange(guildMemberInfos);
			if (this.IsCurrentSeasonScoreTab())
			{
				this.m_guildMemberScoreList.Sort(new Comparison<GuildMemInfo>(this.SortGuildMemberSeasonScoreList));
			}
			else
			{
				this.m_guildMemberScoreList.Sort(new Comparison<GuildMemInfo>(this.SortGuildMemberWeekScoreList));
			}
			CUIListScript component = this.m_form.GetWidget(7).GetComponent<CUIListScript>();
			component.SetElementAmount(this.m_guildMemberScoreList.get_Count());
		}

		private int SortGuildMemberSeasonScoreList(GuildMemInfo info1, GuildMemInfo info2)
		{
			return -info1.GuildMatchInfo.dwScore.CompareTo(info2.GuildMatchInfo.dwScore);
		}

		private int SortGuildMemberWeekScoreList(GuildMemInfo info1, GuildMemInfo info2)
		{
			return -info1.GuildMatchInfo.dwWeekScore.CompareTo(info2.GuildMatchInfo.dwScore);
		}

		private bool IsCurrentSeasonScoreTab()
		{
			if (this.m_form == null)
			{
				return false;
			}
			Slider component = this.m_form.GetWidget(25).GetComponent<Slider>();
			return (int)component.value == 0;
		}

		private void InitMemberInviteList()
		{
			if (this.m_form == null)
			{
				return;
			}
			this.m_guildMemberInviteList = Singleton<CInviteSystem>.GetInstance().CreateGuildMemberInviteList();
			this.m_guildMemberInviteList.Sort(new Comparison<GuildMemInfo>(CGuildHelper.GuildMemberComparisonForInvite));
			Singleton<CInviteSystem>.GetInstance().SendGetGuildMemberGameStateReq();
			CUITimerScript component = this.m_form.GetWidget(15).GetComponent<CUITimerScript>();
			Singleton<CInviteSystem>.GetInstance().SetAndStartRefreshGuildMemberGameStateTimer(component);
		}

		public void RefreshGuildMatchGuildMemberInvitePanel()
		{
			if (this.m_guildMemberInviteList != null)
			{
				this.m_guildMemberInviteList.Sort(new Comparison<GuildMemInfo>(CGuildHelper.GuildMemberComparisonForInvite));
				this.SetInviteGuildMemberData();
			}
		}

		public void SetInviteGuildMemberData()
		{
			if (this.m_form == null)
			{
				return;
			}
			ListView<GuildMemInfo> guildMemberInviteList = this.GetGuildMemberInviteList();
			if (guildMemberInviteList == null)
			{
				return;
			}
			int count = guildMemberInviteList.get_Count();
			int num = 0;
			this.RefreshInviteGuildMemberList(count);
			for (int i = 0; i < count; i++)
			{
				if (CGuildHelper.IsMemberOnline(guildMemberInviteList.get_Item(i)))
				{
					num++;
				}
			}
			Text component = this.m_form.GetWidget(5).GetComponent<Text>();
			component.text = Singleton<CTextManager>.GetInstance().GetText("Common_Online_Member", new string[]
			{
				num.ToString(),
				count.ToString()
			});
		}

		public void RefreshInviteGuildMemberList(int allGuildMemberLen)
		{
			if (this.m_form == null)
			{
				return;
			}
			CUIListScript component = this.m_form.GetWidget(8).GetComponent<CUIListScript>();
			component.SetElementAmount(allGuildMemberLen);
		}

		private void OnGuildScoreListElementEnabled(CUIEvent uiEvent)
		{
			this.SetGuildScoreListElement(uiEvent.m_srcWidget, uiEvent.m_srcWidgetIndexInBelongedList);
		}

		private void OnMemberScoreListElementEnabled(CUIEvent uiEvent)
		{
			this.SetMemberScoreListElement(uiEvent.m_srcWidget, uiEvent.m_srcWidgetIndexInBelongedList);
		}

		private void SetGuildScoreListElement(GameObject listElementGo, int guildScoreListIndex)
		{
			if (listElementGo == null)
			{
				return;
			}
			CSDT_RANKING_LIST_ITEM_INFO[] guildScoreRankInfo = this.GetGuildScoreRankInfo();
			if (guildScoreRankInfo == null)
			{
				return;
			}
			if (guildScoreListIndex < 0 || guildScoreListIndex >= guildScoreRankInfo.Length)
			{
				DebugHelper.Assert(false, "guildScoreListIndex out of range: " + guildScoreListIndex);
				return;
			}
			CSDT_RANKING_LIST_ITEM_INFO cSDT_RANKING_LIST_ITEM_INFO = guildScoreRankInfo[guildScoreListIndex];
			if (cSDT_RANKING_LIST_ITEM_INFO == null)
			{
				return;
			}
			Transform transform = listElementGo.transform;
			Transform rankTransform = transform.Find("rank");
			Image component = transform.Find("imgHeadBg/imgHead").GetComponent<Image>();
			Text component2 = transform.Find("txtName").GetComponent<Text>();
			Text component3 = transform.Find("txtScore").GetComponent<Text>();
			CUICommonSystem.SetRankDisplay((uint)(guildScoreListIndex + 1), rankTransform);
			component.SetSprite(CUIUtility.s_Sprite_Dynamic_GuildHead_Dir + cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stGuildMatch().dwGuildHeadID, this.m_form, true, false, false, false);
			component2.text = StringHelper.UTF8BytesToString(ref cSDT_RANKING_LIST_ITEM_INFO.stExtraInfo.stDetailInfo.get_stGuildMatch().szGuildName);
			component3.text = cSDT_RANKING_LIST_ITEM_INFO.dwRankScore.ToString();
		}

		private void SetSelfGuildScorePanel(GameObject listElementGo)
		{
			if (listElementGo == null)
			{
				return;
			}
			Transform transform = listElementGo.transform;
			Transform rankTransform = transform.Find("rank");
			Image component = transform.Find("imgHeadBg/imgHead").GetComponent<Image>();
			Text component2 = transform.Find("txtName").GetComponent<Text>();
			Text component3 = transform.Find("txtScore").GetComponent<Text>();
			CUICommonSystem.SetRankDisplay(this.GetSelfGuildScoreRankNo(), rankTransform);
			component.SetSprite(CGuildHelper.GetGuildHeadPath(), this.m_form, true, false, false, false);
			component2.text = CGuildHelper.GetGuildName();
			component3.text = ((!this.IsCurrentSeasonScoreTab()) ? CGuildHelper.GetGuildMatchWeekScore().ToString() : CGuildHelper.GetGuildMatchSeasonScore().ToString());
		}

		private void RefreshSubRankPanel(CGuildMatchSystem.enRankTab rankTab)
		{
			if (this.m_form == null)
			{
				return;
			}
			Text component = this.m_form.GetWidget(23).GetComponent<Text>();
			if (rankTab == CGuildMatchSystem.enRankTab.GuildScore)
			{
				component.text = Singleton<CTextManager>.GetInstance().GetText("GuildMatch_Guild_Score_Rank");
			}
			else if (rankTab == CGuildMatchSystem.enRankTab.MemberScore)
			{
				component.text = Singleton<CTextManager>.GetInstance().GetText("GuildMatch_Member_Score_Rank");
			}
		}

		private uint GetSelfGuildScoreRankNo()
		{
			CSDT_RANKING_LIST_ITEM_INFO[] guildScoreRankInfo = this.GetGuildScoreRankInfo();
			if (guildScoreRankInfo != null)
			{
				ulong guildUid = CGuildHelper.GetGuildUid();
				for (int i = 0; i < guildScoreRankInfo.Length; i++)
				{
					if (guildUid == ulong.Parse(StringHelper.UTF8BytesToString(ref guildScoreRankInfo[i].szOpenID)))
					{
						return guildScoreRankInfo[i].dwRankNo;
					}
				}
			}
			return 0u;
		}

		private void SetMemberScoreListElement(GameObject listElementGo, int guildMemberScoreListIndex)
		{
			if (listElementGo == null || this.m_guildMemberScoreList == null)
			{
				return;
			}
			if (guildMemberScoreListIndex < 0 || guildMemberScoreListIndex >= this.m_guildMemberScoreList.get_Count())
			{
				DebugHelper.Assert(false, "guildMemberScoreListIndex out of range: " + guildMemberScoreListIndex);
				return;
			}
			GuildMemInfo guildMemInfo = this.m_guildMemberScoreList.get_Item(guildMemberScoreListIndex);
			if (guildMemInfo == null)
			{
				return;
			}
			Transform transform = listElementGo.transform;
			Transform rankTransform = transform.Find("rank");
			CUIHttpImageScript component = transform.Find("imgHeadBg/imgHead").GetComponent<CUIHttpImageScript>();
			Image component2 = transform.Find("NobeIcon").GetComponent<Image>();
			Image component3 = transform.Find("imgHeadBg/NobeImag").GetComponent<Image>();
			Text component4 = transform.Find("txtName").GetComponent<Text>();
			Text component5 = transform.Find("txtScore").GetComponent<Text>();
			CUICommonSystem.SetRankDisplay((uint)(guildMemberScoreListIndex + 1), rankTransform);
			component.SetImageUrl(CGuildHelper.GetHeadUrl(guildMemInfo.stBriefInfo.szHeadUrl));
			MonoSingleton<NobeSys>.GetInstance().SetNobeIcon(component2, CGuildHelper.GetNobeLevel(guildMemInfo.stBriefInfo.uulUid, guildMemInfo.stBriefInfo.stVip.level), false);
			MonoSingleton<NobeSys>.GetInstance().SetHeadIconBk(component3, CGuildHelper.GetNobeHeadIconId(guildMemInfo.stBriefInfo.uulUid, guildMemInfo.stBriefInfo.stVip.headIconId));
			component4.text = guildMemInfo.stBriefInfo.sName;
			component5.text = ((!this.IsCurrentSeasonScoreTab()) ? guildMemInfo.GuildMatchInfo.dwWeekScore.ToString() : guildMemInfo.GuildMatchInfo.dwScore.ToString());
		}

		private void OnMemberInviteListElementEnabled(CUIEvent uiEvent)
		{
			if (this.m_guildMemberInviteList == null)
			{
				return;
			}
			int srcWidgetIndexInBelongedList = uiEvent.m_srcWidgetIndexInBelongedList;
			GameObject srcWidget = uiEvent.m_srcWidget;
			if (srcWidgetIndexInBelongedList >= 0 && srcWidgetIndexInBelongedList < this.m_guildMemberInviteList.get_Count())
			{
				CInviteView.UpdateGuildMemberListElement(srcWidget, this.m_guildMemberInviteList.get_Item(srcWidgetIndexInBelongedList), true);
			}
		}

		private void OnInvite(CUIEvent uiEvent)
		{
			if (this.IsTeamFull(Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID))
			{
				Singleton<CUIManager>.GetInstance().OpenTips("GuildMatch_Team_Member_Full", true, 1.5f, null, new object[0]);
				return;
			}
			ulong commonUInt64Param = uiEvent.m_eventParams.commonUInt64Param1;
			GuildMemInfo guildMemberInfoByUid = CGuildHelper.GetGuildMemberInfoByUid(commonUInt64Param);
			if (guildMemberInfoByUid != null && CGuildHelper.IsInGuildMatchJoinLimitTime(guildMemberInfoByUid))
			{
				Singleton<CUIManager>.GetInstance().OpenTips("GuildMatch_Member_Join_Time_Limit_Join_Match_Tip", true, 1.5f, null, new object[0]);
				return;
			}
			string empty = string.Empty;
			if (this.IsMemberInOtherTeam(commonUInt64Param, ref empty))
			{
				string text = Singleton<CTextManager>.GetInstance().GetText("GuildMatch_Member_Already_In_Other_Team", new string[]
				{
					empty
				});
				Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(text, enUIEventID.Guild_Match_InviteConfirm, enUIEventID.None, new stUIEventParams
				{
					commonUInt64Param1 = commonUInt64Param,
					commonGameObject = uiEvent.m_srcWidget
				}, false);
				return;
			}
			this.RealInvite(commonUInt64Param, uiEvent.m_srcWidget);
		}

		private void OnInviteConfirm(CUIEvent uiEvent)
		{
			this.RealInvite(uiEvent.m_eventParams.commonUInt64Param1, uiEvent.m_eventParams.commonGameObject);
		}

		private void RealInvite(ulong invitedMemberUid, GameObject inviteBtnGo)
		{
			this.RequestInviteGuildMatchMember(invitedMemberUid);
			CInviteView.SetInvitedRelatedWidgets(inviteBtnGo, inviteBtnGo.transform.parent.Find("Online").GetComponent<Text>());
		}

		private bool IsMemberInOtherTeam(ulong invitedMemberUid, ref string teamName)
		{
			GuildMemInfo guildMemberInfoByUid = CGuildHelper.GetGuildMemberInfoByUid(invitedMemberUid);
			if (guildMemberInfoByUid != null && guildMemberInfoByUid.GuildMatchInfo.ullTeamLeaderUid > 0uL && guildMemberInfoByUid.GuildMatchInfo.ullTeamLeaderUid != Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID)
			{
				teamName = this.GetTeamName(guildMemberInfoByUid.GuildMatchInfo.ullTeamLeaderUid);
				return true;
			}
			return false;
		}

		private void OnKick(CUIEvent uiEvent)
		{
			stUIEventParams par = default(stUIEventParams);
			par.commonUInt64Param1 = uiEvent.m_eventParams.commonUInt64Param1;
			par.commonBool = uiEvent.m_eventParams.commonBool;
			bool commonBool = uiEvent.m_eventParams.commonBool;
			Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(Singleton<CTextManager>.GetInstance().GetText((!commonBool) ? "GuildMatch_Kick_Confirm_Msg" : "GuildMatch_Leave_Team_Confirm_Msg"), enUIEventID.Guild_Match_KickConfirm, enUIEventID.None, par, false);
		}

		private void OnKickConfirm(CUIEvent uiEvent)
		{
			ulong commonUInt64Param = uiEvent.m_eventParams.commonUInt64Param1;
			bool commonBool = uiEvent.m_eventParams.commonBool;
			if (commonBool)
			{
				this.RequestLeaveGuildMatchTeam();
			}
			else
			{
				this.RequestKickGuildMatchMember(commonUInt64Param);
			}
		}

		private void OnAppointOrCancelLeader(CUIEvent uiEvent)
		{
			if (!CGuildSystem.HasAppointMatchLeaderAuthority())
			{
				Singleton<CUIManager>.GetInstance().OpenTips("Guild_Appoint_You_Have_No_Authority", true, 1.5f, null, new object[0]);
				return;
			}
			GuildMemInfo currentSelectedMemberInfo = Singleton<CGuildModel>.GetInstance().CurrentSelectedMemberInfo;
			if (currentSelectedMemberInfo == null)
			{
				DebugHelper.Assert(false, "guildMemInfo is null!!!");
				return;
			}
			if (!CGuildHelper.IsGuildMatchLeaderPosition(currentSelectedMemberInfo))
			{
				if (this.IsLeaderNumFull())
				{
					Singleton<CUIManager>.GetInstance().OpenTips("GuildMatch_Leader_Full", true, 1.5f, null, new object[0]);
					return;
				}
				if (CGuildHelper.IsInGuildMatchJoinLimitTime(currentSelectedMemberInfo))
				{
					Singleton<CUIManager>.GetInstance().OpenTips("GuildMatch_Member_Join_Time_Limit_Appoint_Leader_Tip", true, 1.5f, null, new object[0]);
					return;
				}
			}
			string text = Singleton<CTextManager>.GetInstance().GetText((!CGuildHelper.IsGuildMatchLeaderPosition(currentSelectedMemberInfo)) ? "GuildMatch_Apooint_Leader_Confirm" : "GuildMatch_Cancel_Leader_Confirm", new string[]
			{
				currentSelectedMemberInfo.stBriefInfo.sName
			});
			Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(text, enUIEventID.Guild_Match_AppointOrCancelLeaderConfirm, enUIEventID.None, false);
		}

		private void OnAppointOrCancelLeaderConfirm(CUIEvent uiEvent)
		{
			GuildMemInfo currentSelectedMemberInfo = Singleton<CGuildModel>.GetInstance().CurrentSelectedMemberInfo;
			if (currentSelectedMemberInfo == null)
			{
				DebugHelper.Assert(false, "guildMemInfo is null!!!");
				return;
			}
			this.RequestChangeGuildMatchLeader(currentSelectedMemberInfo.stBriefInfo.uulUid, !CGuildHelper.IsGuildMatchLeaderPosition(currentSelectedMemberInfo));
		}

		private void OnAcceptInvite(CUIEvent uiEvent)
		{
			this.RequestDealGuildMatchMemberInvite(uiEvent.m_eventParams.commonUInt64Param1, true);
		}

		private void OnRefuseInvite(CUIEvent uiEvent)
		{
			this.RequestDealGuildMatchMemberInvite(uiEvent.m_eventParams.commonUInt64Param1, false);
		}

		private void OnRefreshGameStateTimeout(CUIEvent uiEvent)
		{
			this.RequestGetGuildMemberGameState();
		}

		private void OnObWaitingTimeout(CUIEvent uiEvent)
		{
			Transform parent = uiEvent.m_srcWidget.transform.parent;
			Button component = parent.GetComponent<Button>();
			CUICommonSystem.SetButtonEnable(component, true, true, true);
		}

		private void OnRecordListElementEnabled(CUIEvent uiEvent)
		{
			GameObject srcWidget = uiEvent.m_srcWidget;
			if (srcWidget == null || this.m_matchRecords == null)
			{
				return;
			}
			if (uiEvent.m_srcWidgetIndexInBelongedList < 0 || uiEvent.m_srcWidgetIndexInBelongedList >= this.m_matchRecords.Length)
			{
				return;
			}
			Transform transform = srcWidget.transform;
			COMDT_GUILD_MATCH_HISTORY_INFO cOMDT_GUILD_MATCH_HISTORY_INFO = this.m_matchRecords[this.m_matchRecords.Length - 1 - uiEvent.m_srcWidgetIndexInBelongedList];
			if (cOMDT_GUILD_MATCH_HISTORY_INFO == null)
			{
				return;
			}
			Text component = transform.Find("txtMatchTime").GetComponent<Text>();
			component.text = Utility.GetUtcToLocalTimeStringFormat((ulong)cOMDT_GUILD_MATCH_HISTORY_INFO.dwMatchTime, Singleton<CTextManager>.GetInstance().GetText("Common_DateTime_Format2"));
			CUIListScript component2 = transform.Find("MatchMemberList").GetComponent<CUIListScript>();
			component2.SetElementAmount(5);
			int num = 1;
			for (int i = 0; i < (int)cOMDT_GUILD_MATCH_HISTORY_INFO.bMemNum; i++)
			{
				CUIListElementScript elemenet;
				if (Convert.ToBoolean(cOMDT_GUILD_MATCH_HISTORY_INFO.astMemInfo[i].bIsTeamLeader))
				{
					elemenet = component2.GetElemenet(0);
				}
				else
				{
					elemenet = component2.GetElemenet(num++);
				}
				this.SetMatchRecordMemberListElement(elemenet, cOMDT_GUILD_MATCH_HISTORY_INFO.astMemInfo[i]);
			}
			Text component3 = transform.Find("txtMatchScore").GetComponent<Text>();
			component3.text = cOMDT_GUILD_MATCH_HISTORY_INFO.iScore.ToString();
		}

		private void OnSubRankSliderValueChanged(CUIEvent uiEvent)
		{
			CUIFormScript srcFormScript = uiEvent.m_srcFormScript;
			if (srcFormScript == null)
			{
				return;
			}
			Text component = srcFormScript.GetWidget(24).GetComponent<Text>();
			int num = (int)uiEvent.m_eventParams.sliderValue;
			component.text = Singleton<CTextManager>.GetInstance().GetText((num != 0) ? "GuildMatch_Slider_Text_Week" : "GuildMatch_Slider_Text_Season");
			CUIListScript component2 = srcFormScript.GetWidget(4).GetComponent<CUIListScript>();
			if (component2.GetSelectedIndex() == 0)
			{
				this.RefreshGuildScoreRankList();
				GameObject widget = srcFormScript.GetWidget(17);
				this.SetSelfGuildScorePanel(widget);
			}
			else if (component2.GetSelectedIndex() == 1)
			{
				this.RefreshMemberScoreRankList();
				GameObject widget2 = srcFormScript.GetWidget(9);
				this.SetMemberScoreListElement(widget2, this.GetSelfIndexInGuildMemberScoreList());
			}
		}

		private void OnOpenMatchFormAndReadyGame(CUIEvent uiEvent)
		{
			this.OpenMatchForm(true);
		}

		private void OnRemindReady(CUIEvent uiEvent)
		{
			this.RequestGuildMatchRemind(uiEvent.m_eventParams.commonUInt64Param1);
			GameObject srcWidget = uiEvent.m_srcWidget;
			Button component = srcWidget.GetComponent<Button>();
			CUICommonSystem.SetButtonEnable(component, false, false, true);
			CUITimerScript cUITimerScript = srcWidget.GetComponent<CUITimerScript>();
			if (cUITimerScript == null)
			{
				cUITimerScript = srcWidget.AddComponent<CUITimerScript>();
			}
			cUITimerScript.SetTotalTime(10f);
			cUITimerScript.SetTimerEventId(enTimerEventType.TimeUp, enUIEventID.Guild_Match_RemindButtonCdOver);
			cUITimerScript.StartTimer();
		}

		private void OnRemindButtonCdOver(CUIEvent uiEvent)
		{
			if (uiEvent.m_srcWidget != null)
			{
				Button component = uiEvent.m_srcWidget.GetComponent<Button>();
				if (component != null)
				{
					CUICommonSystem.SetButtonEnable(component, true, true, true);
				}
			}
		}

		private void OnOpenRule(CUIEvent uiEvent)
		{
			Singleton<CUIManager>.GetInstance().OpenInfoForm(15);
		}

		private void OnTeamListElementEnabled(CUIEvent uiEvent)
		{
			if (this.m_teamInfos == null)
			{
				return;
			}
			int srcWidgetIndexInBelongedList = uiEvent.m_srcWidgetIndexInBelongedList;
			if (srcWidgetIndexInBelongedList >= 0 && srcWidgetIndexInBelongedList < this.m_teamInfos.get_Count())
			{
				CUIListElementScript teamListElement = uiEvent.m_srcWidgetScript as CUIListElementScript;
				this.SetTeamListElement(teamListElement, this.m_teamInfos.get_Item(srcWidgetIndexInBelongedList).get_Key(), this.m_teamInfos.get_Item(srcWidgetIndexInBelongedList).get_Value());
			}
		}

		private void SetMatchRecordMemberListElement(CUIListElementScript memberListElement, COMDT_GUILD_MATCH_HISTORY_MEMBER_INFO recordMemberInfo)
		{
			if (memberListElement != null)
			{
				Transform transform = memberListElement.transform;
				CUIHttpImageScript component = transform.Find("imgHead").GetComponent<CUIHttpImageScript>();
				component.SetImageUrl(CGuildHelper.GetHeadUrl(StringHelper.UTF8BytesToString(ref recordMemberInfo.szHeadUrl)));
				Text component2 = transform.Find("txtName").GetComponent<Text>();
				component2.text = StringHelper.UTF8BytesToString(ref recordMemberInfo.szName);
				GameObject gameObject = transform.Find("imgLeaderMark").gameObject;
				gameObject.CustomSetActive(Convert.ToBoolean(recordMemberInfo.bIsTeamLeader));
			}
		}

		private void OnGetGuildMatchSeasonRank(SCPKG_GET_RANKING_LIST_RSP rsp)
		{
			this.m_guildSeasonScores = new CSDT_RANKING_LIST_ITEM_INFO[rsp.stRankingListDetail.get_stOfSucc().dwItemNum];
			int num = 0;
			while ((long)num < (long)((ulong)rsp.stRankingListDetail.get_stOfSucc().dwItemNum))
			{
				this.m_guildSeasonScores[num] = rsp.stRankingListDetail.get_stOfSucc().astItemDetail[num];
				num++;
			}
			if (this.IsCurrentSeasonScoreTab())
			{
				this.RefreshGuildScoreRankListAndSelfGuildScorePanel();
			}
		}

		private void OnGetGuildMatchWeekRank(SCPKG_GET_RANKING_LIST_RSP rsp)
		{
			this.m_guildWeekScores = new CSDT_RANKING_LIST_ITEM_INFO[rsp.stRankingListDetail.get_stOfSucc().dwItemNum];
			int num = 0;
			while ((long)num < (long)((ulong)rsp.stRankingListDetail.get_stOfSucc().dwItemNum))
			{
				this.m_guildWeekScores[num] = rsp.stRankingListDetail.get_stOfSucc().astItemDetail[num];
				num++;
			}
			if (!this.IsCurrentSeasonScoreTab())
			{
				this.RefreshGuildScoreRankListAndSelfGuildScorePanel();
			}
		}

		private void RefreshGuildScoreRankListAndSelfGuildScorePanel()
		{
			if (this.m_form != null)
			{
				CUIListScript component = this.m_form.GetWidget(4).GetComponent<CUIListScript>();
				if (component.GetSelectedIndex() == 0)
				{
					this.RefreshGuildScoreRankList();
					GameObject widget = this.m_form.GetWidget(17);
					this.SetSelfGuildScorePanel(widget);
				}
			}
		}

		private void RefreshGuildMatchForm()
		{
			this.RefreshGuildHead();
			this.RefreshGuildName();
			this.RefreshGuildMatchScore();
			this.RefreshTeamList();
			this.RefreshGuildMatchLeftMatchCnt();
			this.RefreshGuildMatchOpenTime();
			this.InitRankTabList();
		}

		private void RefreshGuildHead()
		{
			if (this.m_form == null)
			{
				return;
			}
			Image component = this.m_form.GetWidget(0).GetComponent<Image>();
			string prefabPath = CUIUtility.s_Sprite_Dynamic_GuildHead_Dir + Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.briefInfo.dwHeadId;
			component.SetSprite(prefabPath, this.m_form, true, false, false, false);
		}

		private void RefreshGuildName()
		{
			if (this.m_form == null)
			{
				return;
			}
			Text component = this.m_form.GetWidget(1).GetComponent<Text>();
			component.text = CGuildHelper.GetGuildName();
		}

		private void RefreshGuildMatchScore()
		{
			if (this.m_form == null)
			{
				return;
			}
			Text component = this.m_form.GetWidget(2).GetComponent<Text>();
			component.text = Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.GuildMatchInfo.dwScore.ToString();
		}

		private void RefreshGuildMatchLeftMatchCnt()
		{
			if (this.m_form == null)
			{
				return;
			}
			GuildMemInfo playerGuildMemberInfo = CGuildHelper.GetPlayerGuildMemberInfo();
			if (playerGuildMemberInfo == null)
			{
				DebugHelper.Assert(false, "selfMemInfo is null!!!");
			}
			int guildMatchLeftCntInCurRound = CGuildHelper.GetGuildMatchLeftCntInCurRound((int)playerGuildMemberInfo.GuildMatchInfo.bWeekMatchCnt);
			Text component = this.m_form.GetWidget(16).GetComponent<Text>();
			if (guildMatchLeftCntInCurRound <= 0)
			{
				component.text = string.Concat(new object[]
				{
					"<color=red>",
					guildMatchLeftCntInCurRound,
					"</color>",
					Singleton<CTextManager>.GetInstance().GetText("Common_Times")
				});
			}
			else
			{
				component.text = string.Concat(new object[]
				{
					"<color=white>",
					guildMatchLeftCntInCurRound,
					"</color>",
					Singleton<CTextManager>.GetInstance().GetText("Common_Times")
				});
			}
		}

		private void RefreshGuildMatchOpenTime()
		{
			if (this.m_form == null)
			{
				return;
			}
			ResRewardMatchTimeInfo resRewardMatchTimeInfo = null;
			uint dwConfValue = GameDataMgr.guildMiscDatabin.GetDataByKey(47u).dwConfValue;
			GameDataMgr.matchTimeInfoDict.TryGetValue(GameDataMgr.GetDoubleKey(6u, dwConfValue), ref resRewardMatchTimeInfo);
			Text component = this.m_form.GetWidget(14).GetComponent<Text>();
			component.text = resRewardMatchTimeInfo.szTimeTips;
		}

		public void RefreshTeamList()
		{
			if (this.m_form == null || this.m_teamInfos == null)
			{
				return;
			}
			CUIListScript component = this.m_form.GetWidget(3).GetComponent<CUIListScript>();
			component.SetElementAmount(this.m_teamInfos.get_Count());
			this.RefreshMoreLeaderPanel();
		}

		public void RefreshMoreLeaderPanel()
		{
			GameObject widget = this.m_form.GetWidget(26);
			int curLeaderNum = this.GetCurLeaderNum();
			int totalLeaderNum = this.GetTotalLeaderNum();
			if (CGuildSystem.HasAppointMatchLeaderAuthority() && curLeaderNum < totalLeaderNum)
			{
				widget.CustomSetActive(true);
				Text component = widget.GetComponent<Text>();
				component.text = Singleton<CTextManager>.GetInstance().GetText("Guild_Match_Appoint_More_Team_Leader_Tip", new string[]
				{
					curLeaderNum.ToString(),
					totalLeaderNum.ToString()
				});
			}
			else
			{
				widget.CustomSetActive(false);
			}
		}

		private void InitRankTabList()
		{
			if (this.m_form == null)
			{
				return;
			}
			ListView<string> listView = new ListView<string>();
			listView.Add(Singleton<CTextManager>.GetInstance().GetText("GuildMatch_GuildMatchGuildScore"));
			listView.Add(Singleton<CTextManager>.GetInstance().GetText("GuildMatch_GuildMatchMemberScore"));
			if (this.IsSelfBelongedTeamLeader())
			{
				listView.Add(Singleton<CTextManager>.GetInstance().GetText("Common_Invite"));
			}
			CUIListScript component = this.m_form.GetWidget(4).GetComponent<CUIListScript>();
			component.SetElementAmount(listView.get_Count());
			for (int i = 0; i < listView.get_Count(); i++)
			{
				CUIListElementScript elemenet = component.GetElemenet(i);
				Text component2 = elemenet.transform.Find("txtName").GetComponent<Text>();
				component2.text = listView.get_Item(i);
			}
			if (this.IsSelfBelongedTeamLeader())
			{
				component.SelectElement(2, true);
				this.InitMemberInviteList();
			}
			else
			{
				component.SelectElement(0, true);
			}
		}

		private void SetTeamListElement(CUIListElementScript teamListElement, ulong teamLeaderUid, ListView<CGuildMatchSystem.TeamPlayerInfo> teamPlayerInfos)
		{
			if (teamListElement == null || teamPlayerInfos == null)
			{
				return;
			}
			Transform transform = teamListElement.transform;
			if (teamPlayerInfos.get_Item(0) != null)
			{
				Text component = transform.Find("imgTeamTitleBg/txtTeamName").GetComponent<Text>();
				component.text = Singleton<CTextManager>.GetInstance().GetText("GuildMatch_Team_Name", new string[]
				{
					teamPlayerInfos.get_Item(0).Name
				});
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			GuildMemInfo playerGuildMemberInfo = CGuildHelper.GetPlayerGuildMemberInfo();
			Transform transform2 = transform.Find("imgTeamTitleBg/pnlStatusAndOperation");
			GameObject gameObject = transform2.Find("btnStartGame").gameObject;
			GameObject gameObject2 = transform2.Find("btnReadyGame").gameObject;
			GameObject gameObject3 = transform2.Find("btnCancelReadyGame").gameObject;
			GameObject gameObject4 = transform2.Find("btnOBGame").gameObject;
			GameObject gameObject5 = transform2.Find("TagTeamStatusBlue").gameObject;
			GameObject gameObject6 = transform2.Find("TagTeamStatusYellow").gameObject;
			GameObject gameObject7 = transform2.Find("txtTeamStatus").gameObject;
			gameObject7.CustomSetActive(true);
			Text component2 = gameObject7.GetComponent<Text>();
			gameObject5.CustomSetActive(false);
			gameObject6.CustomSetActive(true);
			if (this.IsInTeam(playerGuildMemberInfo.GuildMatchInfo.ullTeamLeaderUid, teamLeaderUid))
			{
				if (this.IsTeamLeader(masterRoleInfo.playerUllUID, teamLeaderUid))
				{
					gameObject.CustomSetActive(true);
					gameObject2.CustomSetActive(false);
					gameObject3.CustomSetActive(false);
					component2.text = Singleton<CTextManager>.GetInstance().GetText("GuildMatch_Status_Prepare");
					bool flag = this.IsTeamAllPlayerReadyForGame(teamPlayerInfos, teamLeaderUid);
					CUICommonSystem.SetButtonEnable(gameObject.GetComponent<Button>(), flag, flag, true);
				}
				else if (this.IsReadyForGame(teamPlayerInfos, masterRoleInfo.playerUllUID))
				{
					gameObject.CustomSetActive(false);
					gameObject2.CustomSetActive(false);
					gameObject3.CustomSetActive(true);
					component2.text = Singleton<CTextManager>.GetInstance().GetText("GuildMatch_Status_In_Prepare");
				}
				else
				{
					gameObject.CustomSetActive(false);
					gameObject2.CustomSetActive(true);
					gameObject3.CustomSetActive(false);
					component2.text = Singleton<CTextManager>.GetInstance().GetText("GuildMatch_Status_Please_Prepare");
				}
				gameObject4.CustomSetActive(false);
			}
			else
			{
				gameObject.CustomSetActive(false);
				gameObject2.CustomSetActive(false);
				gameObject3.CustomSetActive(false);
				ulong obUid = this.GetObUid(teamLeaderUid);
				gameObject4.CustomSetActive(obUid > 0uL);
				uint num = 0u;
				bool flag2 = this.IsInObDelayedTime(teamLeaderUid, out num);
				if (flag2)
				{
					CUICommonSystem.SetButtonEnable(gameObject4.GetComponent<Button>(), false, false, true);
					CUITimerScript component3 = gameObject4.transform.Find("obWaitingTimer").GetComponent<CUITimerScript>();
					component3.SetTotalTime(num);
					component3.StartTimer();
				}
				if (obUid > 0uL)
				{
					CUIEventScript component4 = gameObject4.GetComponent<CUIEventScript>();
					component4.m_onClickEventParams.commonUInt64Param1 = obUid;
				}
				if (flag2)
				{
					component2.text = string.Empty;
				}
				else
				{
					string text = this.GetMatchStartedTimeStr(teamLeaderUid);
					if (string.IsNullOrEmpty(text))
					{
						text = Singleton<CTextManager>.GetInstance().GetText("GuildMatch_Status_Prepare");
					}
					else
					{
						gameObject5.CustomSetActive(true);
						gameObject6.CustomSetActive(false);
					}
					component2.text = text;
				}
			}
			GuildMemInfo guildMemberInfoByUid = CGuildHelper.GetGuildMemberInfoByUid(teamLeaderUid);
			GameObject gameObject8 = transform.Find("imgContinuousWin").gameObject;
			if (guildMemberInfoByUid.GuildMatchInfo.bContinueWin > 0)
			{
				gameObject8.CustomSetActive(true);
				Text component5 = gameObject8.transform.Find("txtContinuousWin").GetComponent<Text>();
				component5.text = guildMemberInfoByUid.GuildMatchInfo.bContinueWin + Singleton<CTextManager>.GetInstance().GetText("Common_Continues_Win");
			}
			else
			{
				gameObject8.CustomSetActive(false);
			}
			Text component6 = transform.Find("imgTeamScore/txtTeamScore").GetComponent<Text>();
			component6.text = guildMemberInfoByUid.GuildMatchInfo.dwScore.ToString();
			CUIListScript component7 = transform.Find("PlayerList").GetComponent<CUIListScript>();
			component7.SetElementAmount(5);
			for (int i = 0; i < 5; i++)
			{
				this.SetPlayerListElement(component7.GetElemenet(i), (i >= teamPlayerInfos.get_Count()) ? null : teamPlayerInfos.get_Item(i), teamLeaderUid);
			}
		}

		private void SetPlayerListElement(CUIListElementScript playerListElement, CGuildMatchSystem.TeamPlayerInfo playerInfo, ulong teamLeaderUid)
		{
			if (playerListElement == null)
			{
				return;
			}
			Transform transform = playerListElement.transform;
			GameObject gameObject = transform.Find("imgQuestion").gameObject;
			GameObject gameObject2 = transform.Find("btnKick").gameObject;
			GameObject gameObject3 = transform.Find("imgHead").gameObject;
			GameObject gameObject4 = transform.Find("txtPlayerName").gameObject;
			GameObject gameObject5 = transform.Find("imgLeader").gameObject;
			GameObject gameObject6 = transform.Find("imgReady").gameObject;
			if (this.IsSlotOccupied(playerInfo))
			{
				gameObject.CustomSetActive(false);
				gameObject3.CustomSetActive(true);
				gameObject4.CustomSetActive(true);
				gameObject5.CustomSetActive(playerListElement.m_index == 0);
				Text component = gameObject4.GetComponent<Text>();
				component.text = playerInfo.Name;
				if (CGuildHelper.IsSelf(playerInfo.Uid))
				{
					component.color = CUIUtility.s_Text_Color_Self;
				}
				else
				{
					component.color = CUIUtility.s_Text_Color_White;
				}
				CUIHttpImageScript component2 = gameObject3.GetComponent<CUIHttpImageScript>();
				component2.SetImageUrl(CGuildHelper.GetHeadUrl(playerInfo.HeadUrl));
				bool flag = playerInfo.Uid == Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID;
				bool flag2 = this.IsTeamLeader(Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID, teamLeaderUid);
				gameObject6.CustomSetActive(this.IsSameTeamWithSelf(teamLeaderUid) && !this.IsTeamLeader(playerInfo.Uid, teamLeaderUid) && playerInfo.IsReady);
				if ((flag || flag2) && !this.IsTeamLeader(playerInfo.Uid, teamLeaderUid) && !playerInfo.IsReady)
				{
					gameObject2.CustomSetActive(true);
					CUIEventScript component3 = gameObject2.GetComponent<CUIEventScript>();
					component3.m_onClickEventParams.commonUInt64Param1 = playerInfo.Uid;
					component3.m_onClickEventParams.commonBool = flag;
				}
				else
				{
					gameObject2.CustomSetActive(false);
				}
			}
			else
			{
				gameObject.CustomSetActive(true);
				gameObject2.CustomSetActive(false);
				gameObject3.CustomSetActive(false);
				gameObject4.CustomSetActive(false);
				gameObject5.CustomSetActive(false);
				gameObject6.CustomSetActive(false);
			}
		}

		private bool IsSlotOccupied(CGuildMatchSystem.TeamPlayerInfo playerInfo)
		{
			return playerInfo != null && playerInfo.Uid > 0uL;
		}

		private void RefreshMatchRecordForm()
		{
			if (this.m_matchRecordForm == null || this.m_matchRecords == null || this.m_matchRecords.Length == 0)
			{
				return;
			}
			CUIListScript component = this.m_matchRecordForm.GetWidget(0).GetComponent<CUIListScript>();
			component.SetElementAmount(this.m_matchRecords.Length);
		}

		public ListView<GuildMemInfo> GetGuildMemberInviteList()
		{
			return this.m_guildMemberInviteList;
		}

		private bool IsTeamFull(ulong teamLeaderUid)
		{
			if (this.m_teamInfos != null)
			{
				int i = 0;
				while (i < this.m_teamInfos.get_Count())
				{
					if (this.m_teamInfos.get_Item(i).get_Key() == teamLeaderUid)
					{
						if (this.m_teamInfos.get_Item(i).get_Value().get_Count() < 5)
						{
							return false;
						}
						for (int j = 0; j < this.m_teamInfos.get_Item(i).get_Value().get_Count(); j++)
						{
							if (!this.IsSlotOccupied(this.m_teamInfos.get_Item(i).get_Value().get_Item(j)))
							{
								return false;
							}
						}
						return true;
					}
					else
					{
						i++;
					}
				}
			}
			return false;
		}

		private CGuildMatchSystem.TeamPlayerInfo CreateTeamPlayerInfoObj(GuildMemInfo guildMemInfo)
		{
			return new CGuildMatchSystem.TeamPlayerInfo(guildMemInfo.stBriefInfo.uulUid, guildMemInfo.stBriefInfo.sName, guildMemInfo.stBriefInfo.szHeadUrl, Convert.ToBoolean(guildMemInfo.GuildMatchInfo.bIsReady));
		}

		private bool IsNeedOpenGuildMatchForm(SCPKG_GUILD_MATCH_MEMBER_CHG_NTF ntf)
		{
			if (Singleton<CUIManager>.GetInstance().GetForm(CGuildMatchSystem.GuildMatchFormPath) != null || !Utility.IsCanShowPrompt())
			{
				return false;
			}
			for (int i = 0; i < (int)ntf.bCnt; i++)
			{
				if (ntf.astChgInfo[i].ullUid == Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID && ntf.astChgInfo[i].ullTeamLeaderUid > 0uL)
				{
					return true;
				}
			}
			return false;
		}

		private bool IsLeaderNumFull()
		{
			int curLeaderNum = this.GetCurLeaderNum();
			int totalLeaderNum = this.GetTotalLeaderNum();
			return curLeaderNum >= totalLeaderNum;
		}

		private int GetCurLeaderNum()
		{
			ListView<GuildMemInfo> guildMemberInfos = CGuildHelper.GetGuildMemberInfos();
			int num = 0;
			for (int i = 0; i < guildMemberInfos.get_Count(); i++)
			{
				if (Convert.ToBoolean(guildMemberInfos.get_Item(i).GuildMatchInfo.bIsLeader))
				{
					num++;
				}
			}
			return num;
		}

		private int GetTotalLeaderNum()
		{
			ResGuildLevel dataByKey = GameDataMgr.guildLevelDatabin.GetDataByKey((long)CGuildHelper.GetGuildLevel());
			if (dataByKey != null)
			{
				return (int)dataByKey.bTeamCnt;
			}
			return 0;
		}

		private int GetSelfIndexInGuildMemberScoreList()
		{
			if (this.m_guildMemberScoreList == null)
			{
				return -1;
			}
			for (int i = 0; i < this.m_guildMemberScoreList.get_Count(); i++)
			{
				if (this.m_guildMemberScoreList.get_Item(i).stBriefInfo.uulUid == Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID)
				{
					return i;
				}
			}
			return -1;
		}

		private ulong GetObUid(ulong teamLeaderUid)
		{
			ListView<COMDT_GUILD_MATCH_OB_INFO> guildMatchObInfos = Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.GuildMatchObInfos;
			if (guildMatchObInfos != null)
			{
				for (int i = 0; i < guildMatchObInfos.get_Count(); i++)
				{
					if (teamLeaderUid == guildMatchObInfos.get_Item(i).ullUid)
					{
						return teamLeaderUid;
					}
				}
			}
			return 0uL;
		}

		private string GetMatchStartedTimeStr(ulong teamLeaderUid)
		{
			ListView<COMDT_GUILD_MATCH_OB_INFO> guildMatchObInfos = Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.GuildMatchObInfos;
			uint num = 0u;
			if (guildMatchObInfos != null)
			{
				for (int i = 0; i < guildMatchObInfos.get_Count(); i++)
				{
					if (teamLeaderUid == guildMatchObInfos.get_Item(i).ullUid)
					{
						num = guildMatchObInfos.get_Item(i).dwBeginTime;
						break;
					}
				}
			}
			if (num > 0u)
			{
				uint num2 = (uint)(CRoleInfo.GetCurrentUTCTime() - (int)num);
				TimeSpan timeSpan = new TimeSpan((long)((ulong)num2 * 10000000uL));
				return Singleton<CTextManager>.GetInstance().GetText("GuildMatch_Status_Game_Started", new string[]
				{
					((int)timeSpan.get_TotalMinutes()).ToString()
				});
			}
			return string.Empty;
		}

		private bool IsInObDelayedTime(ulong teamLeaderUid, out uint obWaitingTime)
		{
			ListView<COMDT_GUILD_MATCH_OB_INFO> guildMatchObInfos = Singleton<CGuildModel>.GetInstance().CurrentGuildInfo.GuildMatchObInfos;
			uint num = 0u;
			if (guildMatchObInfos != null)
			{
				for (int i = 0; i < guildMatchObInfos.get_Count(); i++)
				{
					if (teamLeaderUid == guildMatchObInfos.get_Item(i).ullUid)
					{
						num = guildMatchObInfos.get_Item(i).dwBeginTime;
						break;
					}
				}
			}
			if (num > 0u)
			{
				uint num2 = (uint)(CRoleInfo.GetCurrentUTCTime() - (int)num);
				uint dwConfValue = GameDataMgr.globalInfoDatabin.GetDataByKey(215u).dwConfValue;
				if (dwConfValue > num2)
				{
					obWaitingTime = dwConfValue - num2;
					return true;
				}
			}
			obWaitingTime = 0u;
			return false;
		}

		public bool IsInObDelayedTime(ulong obUid)
		{
			uint num;
			return this.IsInObDelayedTime(obUid, out num);
		}

		private CSDT_RANKING_LIST_ITEM_INFO[] GetGuildScoreRankInfo()
		{
			return (!this.IsCurrentSeasonScoreTab()) ? this.m_guildWeekScores : this.m_guildSeasonScores;
		}

		public List<COBSystem.stOBGuild> GetGuidMatchObInfo()
		{
			List<COBSystem.stOBGuild> list = new List<COBSystem.stOBGuild>();
			GuildInfo currentGuildInfo = Singleton<CGuildModel>.GetInstance().CurrentGuildInfo;
			if (currentGuildInfo == null || currentGuildInfo.GuildMatchObInfos == null)
			{
				return list;
			}
			for (int i = 0; i < currentGuildInfo.GuildMatchObInfos.get_Count(); i++)
			{
				if (currentGuildInfo.GuildMatchObInfos.get_Item(i).dwBeginTime > 0u)
				{
					for (int j = 0; j < currentGuildInfo.GuildMatchObInfos.get_Item(i).astHeroInfo.Length; j++)
					{
						GuildMemInfo guildMemberInfoByUid = CGuildHelper.GetGuildMemberInfoByUid(currentGuildInfo.GuildMatchObInfos.get_Item(i).astHeroInfo[j].ullUid);
						if (guildMemberInfoByUid != null)
						{
							list.Add(new COBSystem.stOBGuild
							{
								obUid = currentGuildInfo.GuildMatchObInfos.get_Item(i).ullUid,
								playerUid = currentGuildInfo.GuildMatchObInfos.get_Item(i).astHeroInfo[j].ullUid,
								playerName = guildMemberInfoByUid.stBriefInfo.sName,
								teamName = this.GetTeamName(currentGuildInfo.GuildMatchObInfos.get_Item(i).ullUid),
								headUrl = CGuildHelper.GetHeadUrl(guildMemberInfoByUid.stBriefInfo.szHeadUrl),
								dwHeroID = currentGuildInfo.GuildMatchObInfos.get_Item(i).astHeroInfo[j].dwHeroID,
								dwStartTime = currentGuildInfo.GuildMatchObInfos.get_Item(i).dwBeginTime,
								bGrade = (byte)CLadderSystem.ConvertEloToRank(guildMemberInfoByUid.stBriefInfo.dwScoreOfRank),
								dwClass = guildMemberInfoByUid.stBriefInfo.dwClassOfRank,
								dwObserveNum = currentGuildInfo.GuildMatchObInfos.get_Item(i).dwOBCnt
							});
						}
					}
				}
			}
			return list;
		}

		private string GetTeamName(ulong teamLeaderUid)
		{
			GuildMemInfo guildMemberInfoByUid = CGuildHelper.GetGuildMemberInfoByUid(teamLeaderUid);
			if (guildMemberInfoByUid != null)
			{
				return guildMemberInfoByUid.stBriefInfo.sName;
			}
			return string.Empty;
		}

		public bool IsInGuildMatchTime()
		{
			uint dwConfValue = GameDataMgr.guildMiscDatabin.GetDataByKey(47u).dwConfValue;
			return CUICommonSystem.GetMatchOpenState(6, dwConfValue).matchState == enMatchOpenState.enMatchOpen_InActiveTime;
		}

		private void RequestChangeGuildMatchLeader(ulong leaderUid, bool isAppoint)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5301u);
			cSPkg.stPkgData.get_stChgGuildMatchLeaderReq().ullUid = leaderUid;
			cSPkg.stPkgData.get_stChgGuildMatchLeaderReq().bAppoint = Convert.ToByte(isAppoint);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		private void RequestInviteGuildMatchMember(ulong memberUid)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5303u);
			cSPkg.stPkgData.get_stInviteGuildMatchMemberReq().ullUid = memberUid;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		private void RequestDealGuildMatchMemberInvite(ulong teamLeaderUid, bool isAgree)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5305u);
			cSPkg.stPkgData.get_stDealGuildMatchMemberInvite().ullTeamLeaderUid = teamLeaderUid;
			cSPkg.stPkgData.get_stDealGuildMatchMemberInvite().bAgree = Convert.ToByte(isAgree);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		private void RequestKickGuildMatchMember(ulong memberUid)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5307u);
			cSPkg.stPkgData.get_stKickGuildMatchMemberReq().ullUid = memberUid;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		private void RequestLeaveGuildMatchTeam()
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5308u);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		private void RequestStartGuildMatch()
		{
			Singleton<CUIManager>.GetInstance().OpenSendMsgAlert(5, enUIEventID.None);
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5310u);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		private void RequestGetGuildMemberGameState()
		{
			if (this.m_guildMemberInviteList == null)
			{
				DebugHelper.Assert(false, "m_guildMemberInviteList is null!!!");
				return;
			}
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(2035u);
			CSPKG_GET_GUILD_MEMBER_GAME_STATE_REQ stGetGuildMemberGameStateReq = cSPkg.stPkgData.get_stGetGuildMemberGameStateReq();
			int num = 0;
			for (int i = 0; i < this.m_guildMemberInviteList.get_Count(); i++)
			{
				if (CGuildHelper.IsMemberOnline(this.m_guildMemberInviteList.get_Item(i)))
				{
					stGetGuildMemberGameStateReq.MemberUid[num] = this.m_guildMemberInviteList.get_Item(i).stBriefInfo.uulUid;
					num++;
				}
			}
			stGetGuildMemberGameStateReq.iMemberCnt = num;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		private void RequestSetGuildMatchReady(bool isReady)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5311u);
			cSPkg.stPkgData.get_stSetGuildMatchReadyReq().bIsReady = Convert.ToByte(isReady);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		public void RequestObGuildMatch(ulong obUid)
		{
			Singleton<CUIManager>.GetInstance().OpenSendMsgAlert(5, enUIEventID.None);
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5317u);
			cSPkg.stPkgData.get_stOBGuildMatchReq().ullOBUid = obUid;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		public void RequestGuildMatchRemind(ulong remindUid)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5322u);
			cSPkg.stPkgData.get_stGuildMatchRemindReq().ullRemindUid = remindUid;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		private void RequestGetGuildMatchHistory()
		{
			Singleton<CUIManager>.GetInstance().OpenSendMsgAlert(5, enUIEventID.None);
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5319u);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		public void RequestGetGuildMatchSeasonRank()
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(2602u);
			cSPkg.stPkgData.get_stGetRankingListReq().iStart = 1;
			cSPkg.stPkgData.get_stGetRankingListReq().bNumberType = 66;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		public void RequestGetGuildMatchWeekRank()
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(2602u);
			cSPkg.stPkgData.get_stGetRankingListReq().iStart = 1;
			cSPkg.stPkgData.get_stGetRankingListReq().bNumberType = 67;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		public void RequestGuildOBCount()
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5324u);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		[MessageHandler(5302)]
		public static void ReceiveChangeGuildMatchLeaderNtf(CSPkg msg)
		{
			Singleton<CGuildModel>.GetInstance().SetGuildMatchMemberInfo(msg.stPkgData.get_stChgGuildMatchLeaderNtf());
			Singleton<CGuildMatchSystem>.GetInstance().SetTeamInfo(msg.stPkgData.get_stChgGuildMatchLeaderNtf());
			Singleton<CPlayerInfoSystem>.GetInstance().SetAppointMatchLeaderBtn();
			Singleton<CGuildInfoView>.GetInstance().RefreshMemberPanel();
			Singleton<CGuildMatchSystem>.GetInstance().RefreshTeamList();
			if (Singleton<CGuildMatchSystem>.GetInstance().IsNeedRefreshRankTab())
			{
				Singleton<CGuildMatchSystem>.GetInstance().InitRankTabList();
			}
			if (CGuildSystem.HasAppointMatchLeaderAuthority())
			{
				Singleton<CUIManager>.GetInstance().OpenTips("GuildMatch_Appoint_Or_Leader_Success", true, 1.5f, null, new object[0]);
			}
		}

		[MessageHandler(5321)]
		public static void ReceiveChangeGuildMatchLeaderRsp(CSPkg msg)
		{
			SCPKG_CHG_GUILD_MATCH_LEADER_RSP stChgGuildMatchLeaderRsp = msg.stPkgData.get_stChgGuildMatchLeaderRsp();
			if (CGuildSystem.IsError(stChgGuildMatchLeaderRsp.bErrorCode))
			{
				return;
			}
		}

		[MessageHandler(5304)]
		public static void ReceiveInviteGuildMatchMemberNtf(CSPkg msg)
		{
			SCPKG_INVITE_GUILD_MATCH_MEMBER_NTF stInviteGuildMatchMemberNtf = msg.stPkgData.get_stInviteGuildMatchMemberNtf();
			if (!Singleton<CInviteSystem>.GetInstance().IsCanBeInvited(stInviteGuildMatchMemberNtf.ullTeamLeaderUid, (uint)CGuildHelper.GetGuildLogicWorldId()))
			{
				return;
			}
			GuildMemInfo guildMemberInfoByUid = CGuildHelper.GetGuildMemberInfoByUid(stInviteGuildMatchMemberNtf.ullTeamLeaderUid);
			if (guildMemberInfoByUid != null)
			{
				stUIEventParams par = default(stUIEventParams);
				par.commonUInt64Param1 = stInviteGuildMatchMemberNtf.ullTeamLeaderUid;
				Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancelAndAutoClose(Singleton<CTextManager>.GetInstance().GetText("GuildMatch_Invited_Msg", new string[]
				{
					guildMemberInfoByUid.stBriefInfo.sName
				}), enUIEventID.Guild_Match_Accept_Invite, enUIEventID.Guild_Match_Refuse_Invite, par, false, 10, enUIEventID.None);
			}
		}

		[MessageHandler(5306)]
		public static void ReceiveGuildMatchMemberInviteRsp(CSPkg msg)
		{
			SCPKG_GUILD_MATCH_MEMBER_INVITE_RSP stGuildMatchMemberInviteRsp = msg.stPkgData.get_stGuildMatchMemberInviteRsp();
			if (stGuildMatchMemberInviteRsp.bErrorCode == 30 && Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID == stGuildMatchMemberInviteRsp.ullInviter)
			{
				GuildMemInfo guildMemberInfoByUid = CGuildHelper.GetGuildMemberInfoByUid(stGuildMatchMemberInviteRsp.ullInvitee);
				string text = Singleton<CTextManager>.GetInstance().GetText("GuildMatch_Invitee_Refuse", new string[]
				{
					guildMemberInfoByUid.stBriefInfo.sName
				});
				Singleton<CUIManager>.GetInstance().OpenTips(text, false, 1.5f, null, new object[0]);
			}
		}

		[MessageHandler(5309)]
		public static void ReceiveGuildMatchMemberChangeNtf(CSPkg msg)
		{
			Singleton<CGuildModel>.GetInstance().SetGuildMatchMemberInfo(msg.stPkgData.get_stGuildMatchMemberChgNtf());
			Singleton<CGuildMatchSystem>.GetInstance().SetTeamInfo(msg.stPkgData.get_stGuildMatchMemberChgNtf());
			if (Singleton<CGuildMatchSystem>.GetInstance().IsNeedOpenGuildMatchForm(msg.stPkgData.get_stGuildMatchMemberChgNtf()))
			{
				Singleton<CGuildMatchSystem>.GetInstance().OpenMatchForm(false);
			}
			else
			{
				Singleton<CGuildMatchSystem>.GetInstance().RefreshTeamList();
				if (Singleton<CGuildMatchSystem>.GetInstance().IsNeedRefreshRankTab())
				{
					Singleton<CGuildMatchSystem>.GetInstance().InitRankTabList();
				}
			}
		}

		[MessageHandler(5312)]
		public static void ReceiveSetGuildMatchReadyRsp(CSPkg msg)
		{
			CGuildSystem.IsError(msg.stPkgData.get_stSetGuildMatchReadyRsp().bErrorCode);
		}

		[MessageHandler(5313)]
		public static void ReceiveSetGuildMatchReadyNtf(CSPkg msg)
		{
			Singleton<CGuildModel>.GetInstance().SetGuildMatchMemberReadyState(msg.stPkgData.get_stSetGuildMatchReadyNtf());
			Singleton<CGuildMatchSystem>.GetInstance().SetTeamMemberReadyState(msg.stPkgData.get_stSetGuildMatchReadyNtf());
			Singleton<CGuildMatchSystem>.GetInstance().RefreshTeamList();
		}

		[MessageHandler(5314)]
		public static void ReceiveGuildMatchScoreChangeNtf(CSPkg msg)
		{
			Singleton<CGuildModel>.GetInstance().SetGuildMatchScore(msg.stPkgData.get_stGuildMatchScoreChgNtf());
			Singleton<CGuildMatchSystem>.GetInstance().RefreshGuildMatchScore();
		}

		[MessageHandler(5315)]
		public static void ReceiveStartGuildMatchRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			CGuildSystem.IsError(msg.stPkgData.get_stStartGuildMatchRsp().bErrorCode);
		}

		[MessageHandler(5316)]
		public static void ReceiveGuildMatchObInfoChg(CSPkg msg)
		{
			GuildInfo currentGuildInfo = Singleton<CGuildModel>.GetInstance().CurrentGuildInfo;
			if (currentGuildInfo == null || currentGuildInfo.GuildMatchObInfos == null)
			{
				return;
			}
			COMDT_GUILD_MATCH_OB_INFO stChgInfo = msg.stPkgData.get_stGuildMatchOBInfoChg().stChgInfo;
			ulong ullUid = stChgInfo.ullUid;
			uint dwBeginTime = stChgInfo.dwBeginTime;
			bool flag = false;
			for (int i = currentGuildInfo.GuildMatchObInfos.get_Count() - 1; i >= 0; i--)
			{
				if (currentGuildInfo.GuildMatchObInfos.get_Item(i).ullUid == ullUid)
				{
					if (dwBeginTime > 0u)
					{
						currentGuildInfo.GuildMatchObInfos.get_Item(i).dwBeginTime = dwBeginTime;
						currentGuildInfo.GuildMatchObInfos.get_Item(i).dwOBCnt = stChgInfo.dwOBCnt;
						currentGuildInfo.GuildMatchObInfos.get_Item(i).astHeroInfo = new COMDT_GUILD_MATCH_PLAYER_HERO[5];
						for (int j = 0; j < 5; j++)
						{
							currentGuildInfo.GuildMatchObInfos.get_Item(i).astHeroInfo[j] = new COMDT_GUILD_MATCH_PLAYER_HERO();
							currentGuildInfo.GuildMatchObInfos.get_Item(i).astHeroInfo[j].ullUid = stChgInfo.astHeroInfo[j].ullUid;
							currentGuildInfo.GuildMatchObInfos.get_Item(i).astHeroInfo[j].dwHeroID = stChgInfo.astHeroInfo[j].dwHeroID;
						}
					}
					else
					{
						currentGuildInfo.GuildMatchObInfos.RemoveAt(i);
					}
					flag = true;
				}
			}
			if (!flag)
			{
				currentGuildInfo.GuildMatchObInfos.Add(stChgInfo);
			}
		}

		[MessageHandler(5318)]
		public static void ReceiveObGuildMatchRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			if (msg.stPkgData.get_stOBGuildMatchRsp().iResult == 0)
			{
				if (Singleton<WatchController>.GetInstance().StartObserve(msg.stPkgData.get_stOBGuildMatchRsp().stTgwinfo))
				{
					Singleton<CUIManager>.GetInstance().OpenSendMsgAlert(5, enUIEventID.None);
				}
			}
			else
			{
				Singleton<CUIManager>.get_instance().OpenTips(string.Format("OB_Error_{0}", msg.stPkgData.get_stOBGuildMatchRsp().iResult), true, 1.5f, null, new object[0]);
			}
		}

		[MessageHandler(5323)]
		public static void ReceiveGuildMatchRemindNtf(CSPkg msg)
		{
			if (!Utility.IsCanShowPrompt())
			{
				return;
			}
			Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(Singleton<CTextManager>.GetInstance().GetText("Guild_Match_Remind_Msg"), enUIEventID.Guild_Match_OpenMatchFormAndReadyGame, enUIEventID.None, false);
		}

		[MessageHandler(5320)]
		public static void ReceiveGetGuildMatchHistoryRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			SCPKG_GET_GUILD_MATCH_HISTORY_RSP stGetGuildMatchHistoryRsp = msg.stPkgData.get_stGetGuildMatchHistoryRsp();
			Singleton<CGuildMatchSystem>.GetInstance().m_matchRecords = new COMDT_GUILD_MATCH_HISTORY_INFO[(int)stGetGuildMatchHistoryRsp.bMatchNum];
			for (int i = 0; i < Singleton<CGuildMatchSystem>.GetInstance().m_matchRecords.Length; i++)
			{
				Singleton<CGuildMatchSystem>.GetInstance().m_matchRecords[i] = stGetGuildMatchHistoryRsp.astMatchInfo[i];
			}
			Singleton<CGuildMatchSystem>.GetInstance().OpenMatchRecordForm();
		}

		[MessageHandler(5325)]
		public static void ReceiveGuildOBCountRsp(CSPkg msg)
		{
			Singleton<COBSystem>.GetInstance().SetGuildMatchOBCount(msg.stPkgData.get_stGetGuildMatchOBCntRsp());
		}
	}
}
