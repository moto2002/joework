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
	public class CLadderSystem : Singleton<CLadderSystem>
	{
		public const int NUM_LADDER_BY_TEAM_2 = 2;

		public const int NUM_LADDER_BY_TEAM_5 = 5;

		public const ushort LADDER_RULE_ID = 1;

		public const ushort LADDER_BRAVE_SCORE_RULE_ID = 22;

		public const int MAX_NEED_SCORE = 5;

		public const string LadderLatestShowKingFormTimePrefKey = "Ladder_LatestShowKingFormTimePrefKey";

		public static uint REQ_PLAYER_LEVEL;

		public static uint REQ_HERO_NUM;

		public static int MAX_RANK_LEVEL;

		public static readonly string FORM_LADDER_ENTRY = "UGUI/Form/System/PvP/Ladder/Form_LadderEntry.prefab";

		public static readonly string FORM_LADDER_HISTORY = "UGUI/Form/System/PvP/Ladder/Form_LadderHistory.prefab";

		public static readonly string FORM_LADDER_RECENT = "UGUI/Form/System/PvP/Ladder/Form_RecentLadderMatch.prefab";

		public static readonly string FORM_LADDER_REWARD = "UGUI/Form/System/PvP/Ladder/Form_LadderReward.prefab";

		public static readonly string FORM_LADDER_KING = "UGUI/Form/System/PvP/Ladder/Form_LadderKing.prefab";

		public static readonly string FORM_LADDER_GAMEINFO = "UGUI/Form/System/PvP/Ladder/Form_LadderGameInfo.prefab";

		public static readonly string FORM_AD_PREFAB = "UGUI/Form/System/IDIPNotice/Form_LobbyADForm.prefab";

		private COMDT_RANKDETAIL currentRankDetail;

		private List<COMDT_RANK_CURSEASON_FIGHT_RECORD> currentSeasonGames;

		private List<COMDT_RANK_PASTSEASON_FIGHT_RECORD> historySeasonData;

		private static string image_name = "UGUI/Sprite/Dynamic/Competition/million";

		public COMDT_RANKDETAIL GetCurrentRankDetail()
		{
			return this.currentRankDetail;
		}

		public static uint GetRankBattleMapID()
		{
			uint result = 0u;
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapID_Ladder_Normal"), ref result);
			return result;
		}

		public override void Init()
		{
			base.Init();
			CLadderSystem.REQ_PLAYER_LEVEL = GameDataMgr.globalInfoDatabin.GetDataByKey(100u).dwConfValue;
			CLadderSystem.REQ_HERO_NUM = GameDataMgr.globalInfoDatabin.GetDataByKey(101u).dwConfValue;
			CLadderSystem.MAX_RANK_LEVEL = GameDataMgr.rankGradeDatabin.Count();
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Matching_OpenLadder, new CUIEventManager.OnUIEventHandler(this.OnOpenLadder));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Ladder_StartMatching, new CUIEventManager.OnUIEventHandler(this.OnLadder_BeginMatch));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Ladder_ShowHistory, new CUIEventManager.OnUIEventHandler(this.OnLadder_ShowHistory));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Ladder_ShowRecent, new CUIEventManager.OnUIEventHandler(this.OnLadder_ShowRecent));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Ladder_ShowRules, new CUIEventManager.OnUIEventHandler(this.OnLadder_ShowRules));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Ladder_ShowBraveScoreRule, new CUIEventManager.OnUIEventHandler(this.OnLadder_ShowBraveScoreRule));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Ladder_ShowGameInfo, new CUIEventManager.OnUIEventHandler(this.OnLadder_ShowGameInfo));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Ladder_ExpandHistoryItem, new CUIEventManager.OnUIEventHandler(this.OnLadder_ExpandHistoryItem));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Ladder_ShrinkHistoryItem, new CUIEventManager.OnUIEventHandler(this.OnLadder_ShrinkHistoryItem));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Ladder_ConfirmSeasonRank, new CUIEventManager.OnUIEventHandler(this.OnLadder_ConfirmSeasonRank));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Ladder_ReqGetSeasonReward, new CUIEventManager.OnUIEventHandler(this.OnLadder_ReqGetSeasonReward));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Ladder_GetSeasonRewardDone, new CUIEventManager.OnUIEventHandler(this.OnLadder_GetSeasonRewardDone));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Ladder_OnEntryFormOpened, new CUIEventManager.OnUIEventHandler(this.OnLadder_EntryFormOpened));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Ladder_OnEntryFormClosed, new CUIEventManager.OnUIEventHandler(this.OnLadder_EntryFormClosed));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Ladder_OnClickBpGuide, new CUIEventManager.OnUIEventHandler(this.OnLadder_OnClickBpGuide));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Matching_ADButton, new CUIEventManager.OnUIEventHandler(this.OnMatching_ADButton));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Matching_ADForm_Close, new CUIEventManager.OnUIEventHandler(this.OnMatching_ADForm_Close));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Ladder_OnClickShowRecentUsedHero, new CUIEventManager.OnUIEventHandler(this.OnClickShowRecentUsedHero));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Ladder_OnClickHideRecentUsedHero, new CUIEventManager.OnUIEventHandler(this.OnClickHideRecentUsedHero));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Ladder_GetSkinReward, new CUIEventManager.OnUIEventHandler(this.OnLadder_GetSkinReward));
			Singleton<EventRouter>.GetInstance().AddEventHandler<uint, uint, uint>("HeroSkinAdd", new Action<uint, uint, uint>(this.OnNtyAddSkin));
		}

		public override void UnInit()
		{
			this.currentRankDetail = null;
			if (this.currentSeasonGames != null)
			{
				this.currentSeasonGames.Clear();
				this.currentSeasonGames = null;
			}
			if (this.historySeasonData != null)
			{
				this.historySeasonData.Clear();
				this.historySeasonData = null;
			}
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Matching_OpenLadder, new CUIEventManager.OnUIEventHandler(this.OnOpenLadder));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Ladder_StartMatching, new CUIEventManager.OnUIEventHandler(this.OnLadder_BeginMatch));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Ladder_ShowHistory, new CUIEventManager.OnUIEventHandler(this.OnLadder_ShowHistory));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Ladder_ShowRecent, new CUIEventManager.OnUIEventHandler(this.OnLadder_ShowRecent));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Ladder_ShowRules, new CUIEventManager.OnUIEventHandler(this.OnLadder_ShowRules));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Ladder_ExpandHistoryItem, new CUIEventManager.OnUIEventHandler(this.OnLadder_ExpandHistoryItem));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Ladder_ShrinkHistoryItem, new CUIEventManager.OnUIEventHandler(this.OnLadder_ShrinkHistoryItem));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Ladder_ConfirmSeasonRank, new CUIEventManager.OnUIEventHandler(this.OnLadder_ConfirmSeasonRank));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Ladder_ReqGetSeasonReward, new CUIEventManager.OnUIEventHandler(this.OnLadder_ReqGetSeasonReward));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Ladder_GetSeasonRewardDone, new CUIEventManager.OnUIEventHandler(this.OnLadder_GetSeasonRewardDone));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Matching_ADButton, new CUIEventManager.OnUIEventHandler(this.OnMatching_ADButton));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Matching_ADForm_Close, new CUIEventManager.OnUIEventHandler(this.OnMatching_ADForm_Close));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Ladder_OnClickShowRecentUsedHero, new CUIEventManager.OnUIEventHandler(this.OnClickShowRecentUsedHero));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Ladder_OnClickHideRecentUsedHero, new CUIEventManager.OnUIEventHandler(this.OnClickHideRecentUsedHero));
			base.UnInit();
		}

		private void UpdateRankInfo(ref SCPKG_UPDRANKINFO_NTF newData)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				masterRoleInfo.m_rankGrade = newData.bCurGrade;
				masterRoleInfo.m_rankClass = newData.dwCurClass;
				masterRoleInfo.m_rankSeasonHighestGrade = newData.stRankInfo.bMaxSeasonGrade;
				masterRoleInfo.m_rankSeasonHighestClass = newData.stRankInfo.dwMaxSeasonClass;
				masterRoleInfo.m_rankHistoryHighestGrade = newData.bMaxGradeOfRank;
				masterRoleInfo.m_rankHistoryHighestClass = newData.stRankInfo.dwTopClassOfRank;
				masterRoleInfo.m_rankCurSeasonStartTime = (ulong)newData.stRankInfo.dwSeasonStartTime;
			}
			this.currentRankDetail = newData.stRankInfo;
			bool flag = this.currentRankDetail.bState == 2 && this.currentRankDetail.bGetReward == 0;
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CLadderSystem.FORM_LADDER_ENTRY);
			if (form)
			{
				CLadderView.InitLadderEntry(form, ref this.currentRankDetail, this.IsQualified());
				if (flag)
				{
					CUIFormScript form2 = Singleton<CUIManager>.GetInstance().OpenForm(CLadderSystem.FORM_LADDER_REWARD, false, true);
					CLadderView.InitRewardForm(form2, ref this.currentRankDetail);
				}
			}
		}

		public bool IsQualified()
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			return masterRoleInfo != null && masterRoleInfo.PvpLevel >= CLadderSystem.REQ_PLAYER_LEVEL && (long)masterRoleInfo.GetHaveHeroCountWithoutBanHeroID(false, 3, CLadderSystem.GetRankBattleMapID()) >= (long)((ulong)CLadderSystem.REQ_HERO_NUM);
		}

		public bool IsLevelQualified()
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			return masterRoleInfo != null && masterRoleInfo.PvpLevel >= CLadderSystem.REQ_PLAYER_LEVEL;
		}

		private bool CanOpenLadderEntry()
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo == null)
			{
				return false;
			}
			if (masterRoleInfo.PvpLevel < CLadderSystem.REQ_PLAYER_LEVEL)
			{
				Singleton<CUIManager>.GetInstance().OpenTips("Activity_Open", true, 1f, null, new object[]
				{
					CLadderSystem.REQ_PLAYER_LEVEL
				});
				return false;
			}
			if (Singleton<CMatchingSystem>.GetInstance().IsInMatching)
			{
				Singleton<CUIManager>.GetInstance().OpenTips("PVP_Matching", true, 1.5f, null, new object[0]);
				return false;
			}
			if (!this.IsInCredit())
			{
				Singleton<CUIManager>.GetInstance().OpenTips("Credit_Forbid_Ladder", true, 1.5f, null, new object[0]);
				return false;
			}
			if (!CLadderSystem.IsInSeason())
			{
				Singleton<CUIManager>.GetInstance().OpenTips("Rank_Not_In_Season", true, 1.5f, null, new object[0]);
				return false;
			}
			if (this.IsQualified() && masterRoleInfo.m_rankGrade <= 0)
			{
				Singleton<CUIManager>.GetInstance().OpenTips("Ladder_Data_Error", false, 1.5f, null, new object[0]);
				return false;
			}
			return true;
		}

		private void AddRecentGameData(COMDT_RANK_CURSEASON_FIGHT_RECORD gameData)
		{
			if (this.currentSeasonGames == null)
			{
				this.currentSeasonGames = new List<COMDT_RANK_CURSEASON_FIGHT_RECORD>();
			}
			this.currentSeasonGames.Add(gameData);
			this.currentSeasonGames.Sort(new Comparison<COMDT_RANK_CURSEASON_FIGHT_RECORD>(CLadderSystem.ComparisonGameData));
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CLadderSystem.FORM_LADDER_GAMEINFO);
			if (form)
			{
				CLadderView.SetGameInfoRecentPanel(form, this.currentRankDetail, this.currentSeasonGames);
			}
			CUIFormScript form2 = Singleton<CUIManager>.GetInstance().GetForm(CLadderSystem.FORM_LADDER_RECENT);
			if (form2)
			{
				CLadderView.InitLadderRecent(form2, this.currentSeasonGames);
			}
		}

		private void AddRecentSeasonData(COMDT_RANK_PASTSEASON_FIGHT_RECORD gameData)
		{
			if (this.historySeasonData == null)
			{
				this.historySeasonData = new List<COMDT_RANK_PASTSEASON_FIGHT_RECORD>();
			}
			this.historySeasonData.Add(gameData);
			this.historySeasonData.Sort(new Comparison<COMDT_RANK_PASTSEASON_FIGHT_RECORD>(CLadderSystem.ComparisonHistoryData));
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CLadderSystem.FORM_LADDER_HISTORY);
			if (form)
			{
				CLadderView.InitLadderHistory(form, this.historySeasonData);
			}
		}

		public bool IsShowButtonIn5()
		{
			bool result = false;
			ResGlobalInfo resGlobalInfo = new ResGlobalInfo();
			if (GameDataMgr.svr2CltCfgDict.TryGetValue(21u, ref resGlobalInfo))
			{
				result = (resGlobalInfo.dwConfValue > 0u);
			}
			return result;
		}

		private void OpenLadderEntry()
		{
			bool flag = this.currentRankDetail != null && this.currentRankDetail.bState == 2 && this.currentRankDetail.bGetReward == 0;
			if (!this.CanOpenLadderEntry())
			{
				if (flag)
				{
					CUIFormScript form = Singleton<CUIManager>.GetInstance().OpenForm(CLadderSystem.FORM_LADDER_REWARD, false, true);
					CLadderView.InitRewardForm(form, ref this.currentRankDetail);
				}
				return;
			}
			CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(CLadderSystem.FORM_LADDER_ENTRY, false, true);
			if (cUIFormScript)
			{
				CLadderView.InitLadderEntry(cUIFormScript, ref this.currentRankDetail, this.IsQualified());
				this.PromptSkinRewardIfNeed();
				if (flag)
				{
					CUIFormScript form2 = Singleton<CUIManager>.GetInstance().OpenForm(CLadderSystem.FORM_LADDER_REWARD, false, true);
					CLadderView.InitRewardForm(form2, ref this.currentRankDetail);
				}
				if (this.IsShowLadderKingForm())
				{
					CUIFormScript form3 = Singleton<CUIManager>.GetInstance().OpenForm(CLadderSystem.FORM_LADDER_KING, false, true);
					CLadderView.InitKingForm(form3, ref this.currentRankDetail);
					PlayerPrefs.SetInt("Ladder_LatestShowKingFormTimePrefKey", CRoleInfo.GetCurrentUTCTime());
				}
				bool show = this.IsShowButtonIn5();
				CLadderView.ShowRankButtonIn5(cUIFormScript, show);
			}
		}

		private bool IsShowLadderKingForm()
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			return CLadderView.IsSuperKing(masterRoleInfo.m_rankGrade, (uint)((byte)masterRoleInfo.m_rankClass)) && this.IsFirstShowLadderKingFormBeforeDailyRefreshTime();
		}

		private bool IsFirstShowLadderKingFormBeforeDailyRefreshTime()
		{
			uint todayStartTimeSeconds = Utility.GetTodayStartTimeSeconds();
			int @int = PlayerPrefs.GetInt("Ladder_LatestShowKingFormTimePrefKey");
			int currentUTCTime = CRoleInfo.GetCurrentUTCTime();
			return (ulong)todayStartTimeSeconds > (ulong)((long)@int) || @int > currentUTCTime;
		}

		private void OnOpenLadder(CUIEvent uiEvent)
		{
			Button button = (!uiEvent.m_srcWidget) ? null : uiEvent.m_srcWidget.GetComponent<Button>();
			if (button)
			{
				if (button.interactable)
				{
					Singleton<CNewbieAchieveSys>.GetInstance().trackFlag = CNewbieAchieveSys.TrackFlag.None;
					this.GetRankData();
				}
				else
				{
					if (!this.IsLevelQualified())
					{
						Singleton<CUIManager>.GetInstance().OpenTips("Activity_Open", true, 1f, null, new object[]
						{
							CLadderSystem.REQ_PLAYER_LEVEL
						});
						return;
					}
					if (!Singleton<SCModuleControl>.get_instance().GetActiveModule(16))
					{
						Singleton<CUIManager>.get_instance().OpenMessageBox(Singleton<SCModuleControl>.get_instance().PvpAndPvpOffTips, false);
						return;
					}
				}
			}
			else
			{
				if (!this.IsLevelQualified())
				{
					Singleton<CUIManager>.GetInstance().OpenTips("Activity_Open", true, 1f, null, new object[]
					{
						CLadderSystem.REQ_PLAYER_LEVEL
					});
					return;
				}
				if (!Singleton<SCModuleControl>.get_instance().GetActiveModule(16))
				{
					Singleton<CUIManager>.get_instance().OpenMessageBox(Singleton<SCModuleControl>.get_instance().PvpAndPvpOffTips, false);
					return;
				}
				this.GetRankData();
			}
		}

		private void OnLadder_BeginMatch(CUIEvent uiEvent)
		{
			Button button = (!uiEvent.m_srcWidget) ? null : uiEvent.m_srcWidget.GetComponent<Button>();
			if (button && button.interactable)
			{
				if (Singleton<CMatchingSystem>.GetInstance().IsInMatching)
				{
					Singleton<CUIManager>.GetInstance().OpenTips("PVP_Matching", true, 1.5f, null, new object[0]);
					return;
				}
				this.BeginMatch();
			}
		}

		private void OnLadder_ShowHistory(CUIEvent uiEvent)
		{
			CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(CLadderSystem.FORM_LADDER_HISTORY, false, true);
			if (cUIFormScript)
			{
				CLadderView.InitLadderHistory(cUIFormScript, this.historySeasonData);
			}
		}

		private void OnLadder_ShowRecent(CUIEvent uiEvent)
		{
			CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(CLadderSystem.FORM_LADDER_RECENT, false, true);
			if (cUIFormScript)
			{
				CLadderView.InitLadderRecent(cUIFormScript, this.currentSeasonGames);
			}
		}

		private void OnLadder_ShowRules(CUIEvent uiEvent)
		{
			ResRuleText dataByKey = GameDataMgr.s_ruleTextDatabin.GetDataByKey(1u);
			if (dataByKey != null)
			{
				string title = StringHelper.UTF8BytesToString(ref dataByKey.szTitle);
				string info = StringHelper.UTF8BytesToString(ref dataByKey.szContent);
				Singleton<CUIManager>.GetInstance().OpenInfoForm(title, info);
			}
		}

		private void OnLadder_ShowBraveScoreRule(CUIEvent uiEvent)
		{
			ResRuleText dataByKey = GameDataMgr.s_ruleTextDatabin.GetDataByKey(22u);
			if (dataByKey != null)
			{
				string title = StringHelper.UTF8BytesToString(ref dataByKey.szTitle);
				string info = StringHelper.UTF8BytesToString(ref dataByKey.szContent);
				Singleton<CUIManager>.GetInstance().OpenInfoForm(title, info);
			}
		}

		private void OnLadder_ShowGameInfo(CUIEvent uiEvent)
		{
			CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(CLadderSystem.FORM_LADDER_GAMEINFO, false, true);
			if (cUIFormScript != null)
			{
				CLadderView.InitLadderGameInfo(cUIFormScript, this.currentRankDetail, this.currentSeasonGames);
			}
		}

		private void OnLadder_ExpandHistoryItem(CUIEvent uiEvent)
		{
			if (uiEvent.m_srcWidget && uiEvent.m_srcWidget.transform.parent)
			{
				Transform parent = uiEvent.m_srcWidget.transform.parent.parent;
				if (parent)
				{
					CLadderView.OnHistoryItemChange(parent.gameObject, true);
					CUIEventScript component = uiEvent.m_srcWidget.GetComponent<CUIEventScript>();
					if (component)
					{
						component.m_onClickEventID = enUIEventID.Ladder_ShrinkHistoryItem;
					}
				}
			}
		}

		private void OnLadder_ShrinkHistoryItem(CUIEvent uiEvent)
		{
			if (uiEvent.m_srcWidget && uiEvent.m_srcWidget.transform.parent)
			{
				Transform parent = uiEvent.m_srcWidget.transform.parent.parent;
				if (parent)
				{
					CLadderView.OnHistoryItemChange(parent.gameObject, false);
					CUIEventScript component = uiEvent.m_srcWidget.GetComponent<CUIEventScript>();
					if (component)
					{
						component.m_onClickEventID = enUIEventID.Ladder_ExpandHistoryItem;
					}
				}
			}
		}

		private void OnLadder_ConfirmSeasonRank(CUIEvent uiEvent)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				CLadderView.ShowSeasonEndGetRewardForm(masterRoleInfo.m_rankSeasonHighestGrade);
			}
		}

		private void OnLadder_ReqGetSeasonReward(CUIEvent uiEvent)
		{
			this.ReqGetSeasonReward();
		}

		private void OnLadder_GetSeasonRewardDone(CUIEvent uiEvent)
		{
			Singleton<CUIManager>.GetInstance().CloseForm(CLadderSystem.FORM_LADDER_REWARD);
		}

		private void OnLadder_EntryFormOpened(CUIEvent uiEvent)
		{
		}

		private void OnLadder_EntryFormClosed(CUIEvent uiEvent)
		{
		}

		private void OnLadder_OnClickBpGuide(CUIEvent uiEvent)
		{
			Singleton<CBattleGuideManager>.GetInstance().OpenBannerDlgByBannerGuideId(9u, null, false);
		}

		private void OnMatching_ADButton(CUIEvent uiEvent)
		{
			CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(CLadderSystem.FORM_AD_PREFAB, false, true);
			Image component = cUIFormScript.transform.Find("Panel/Image").GetComponent<Image>();
			component.SetSprite(CLadderSystem.image_name, cUIFormScript, true, false, false, false);
			component.gameObject.CustomSetActive(true);
		}

		private void OnMatching_ADForm_Close(CUIEvent uiEvent)
		{
			Singleton<CUIManager>.GetInstance().CloseForm(CLadderSystem.FORM_AD_PREFAB);
		}

		private void OnClickShowRecentUsedHero(CUIEvent uiEvent)
		{
			if (uiEvent.m_eventParams.togleIsOn)
			{
				this.ReqHideRecentUsedHero(false);
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				if (masterRoleInfo != null)
				{
					CLadderSystem.SetRecentUsedHeroMask(ref masterRoleInfo.recentUseHero.dwCtrlMask, 1, false);
				}
				CMiShuSystem.SendUIClickToServer(enUIClickReprotID.rp_OpenRecentUseHero);
			}
		}

		private void OnClickHideRecentUsedHero(CUIEvent uiEvent)
		{
			if (uiEvent.m_eventParams.togleIsOn)
			{
				this.ReqHideRecentUsedHero(true);
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				if (masterRoleInfo != null)
				{
					CLadderSystem.SetRecentUsedHeroMask(ref masterRoleInfo.recentUseHero.dwCtrlMask, 1, true);
				}
				CMiShuSystem.SendUIClickToServer(enUIClickReprotID.rp_CloseRecentUseHero);
			}
		}

		private void OnLadder_GetSkinReward(CUIEvent uiEvent)
		{
			this.ReqGetLadderRewardSkin();
		}

		private void OnNtyAddSkin(uint heroId, uint skinId, uint addReason)
		{
			if (addReason == 7u)
			{
				enFormPriority priority = enFormPriority.Priority3;
				CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(Singleton<SettlementSystem>.GetInstance()._profitFormName);
				if (form != null)
				{
					priority = form.m_priority + 1;
				}
				CUICommonSystem.ShowNewHeroOrSkin(heroId, skinId, enUIEventID.None, true, 10, false, null, priority, 0u, 0);
			}
		}

		private void BeginMatch()
		{
			CMatchingSystem.ReqStartSingleMatching(CLadderSystem.GetRankBattleMapID(), false, 3);
		}

		private void GetRankData()
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(2610u);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		private void ReqGetSeasonReward()
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(2915u);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		private void ReqHideRecentUsedHero(bool bHide)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5500u);
			cSPkg.stPkgData.get_stShowRecentUsedHeroReq().bTurnOn = Convert.ToByte(!bHide);
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false) && masterRoleInfo != null)
			{
				CLadderSystem.SetRecentUsedHeroMask(ref masterRoleInfo.recentUseHero.dwCtrlMask, 1, bHide);
			}
		}

		private void ReqGetLadderRewardSkin()
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5332u);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		[MessageHandler(2611)]
		public static void GetRankInfoRsp(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<CLadderSystem>.GetInstance().OpenLadderEntry();
		}

		[MessageHandler(2601)]
		public static void ReceiveRankInfo(CSPkg msg)
		{
			SCPKG_UPDRANKINFO_NTF stUpdateRankInfo = msg.stPkgData.get_stUpdateRankInfo();
			Singleton<CLadderSystem>.GetInstance().UpdateRankInfo(ref stUpdateRankInfo);
		}

		[MessageHandler(2913)]
		public static void ReceiveRankSeasonInfo(CSPkg msg)
		{
			if (Singleton<CLadderSystem>.GetInstance().currentSeasonGames == null)
			{
				Singleton<CLadderSystem>.GetInstance().currentSeasonGames = new List<COMDT_RANK_CURSEASON_FIGHT_RECORD>();
			}
			Singleton<CLadderSystem>.GetInstance().currentSeasonGames.Clear();
			for (int i = 0; i < (int)msg.stPkgData.get_stRankCurSeasonHistory().bNum; i++)
			{
				Singleton<CLadderSystem>.GetInstance().currentSeasonGames.Add(msg.stPkgData.get_stRankCurSeasonHistory().astRecord[i]);
			}
			Singleton<CLadderSystem>.GetInstance().currentSeasonGames.Sort(new Comparison<COMDT_RANK_CURSEASON_FIGHT_RECORD>(CLadderSystem.ComparisonGameData));
		}

		[MessageHandler(2914)]
		public static void ReceiveRankHistoryInfo(CSPkg msg)
		{
			if (Singleton<CLadderSystem>.GetInstance().historySeasonData == null)
			{
				Singleton<CLadderSystem>.GetInstance().historySeasonData = new List<COMDT_RANK_PASTSEASON_FIGHT_RECORD>();
			}
			Singleton<CLadderSystem>.GetInstance().historySeasonData.Clear();
			for (int i = 0; i < (int)msg.stPkgData.get_stRankPastSeasonHistory().bNum; i++)
			{
				Singleton<CLadderSystem>.GetInstance().historySeasonData.Add(msg.stPkgData.get_stRankPastSeasonHistory().astRecord[i]);
			}
			Singleton<CLadderSystem>.GetInstance().historySeasonData.Sort(new Comparison<COMDT_RANK_PASTSEASON_FIGHT_RECORD>(CLadderSystem.ComparisonHistoryData));
		}

		[MessageHandler(2917)]
		public static void AddCurrentSeasonRecord(CSPkg msg)
		{
			Singleton<CLadderSystem>.GetInstance().AddRecentGameData(msg.stPkgData.get_stNtfAddCurSeasonRecord().stRecord);
		}

		[MessageHandler(2918)]
		public static void AddHistorySeasonRecord(CSPkg msg)
		{
			Singleton<CLadderSystem>.GetInstance().AddRecentSeasonData(msg.stPkgData.get_stNtfAddPastSeasonRecord().stRecord);
		}

		[MessageHandler(2916)]
		public static void OnReceiveRankSeasonReward(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			if (msg.stPkgData.get_stGetRankRewardRsp().bErrCode == 0)
			{
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				if (masterRoleInfo != null && Singleton<CLadderSystem>.GetInstance().currentRankDetail != null)
				{
					Singleton<SettlementSystem>.GetInstance().SetLadderDisplayOldAndNewGrade(1u, 1u, (uint)masterRoleInfo.m_rankSeasonHighestGrade, Singleton<CLadderSystem>.GetInstance().currentRankDetail.dwScore);
					Singleton<SettlementSystem>.GetInstance().ShowLadderSettleFormWithoutSettle();
				}
				Singleton<CLadderSystem>.GetInstance().currentRankDetail.bGetReward = 1;
			}
			else
			{
				string strContent = string.Empty;
				if (msg.stPkgData.get_stGetRankRewardRsp().bErrCode == 1)
				{
					strContent = Singleton<CTextManager>.GetInstance().GetText("GETRANKREWARD_ERR_STATE_INVALID");
				}
				else if (msg.stPkgData.get_stGetRankRewardRsp().bErrCode == 2)
				{
					strContent = Singleton<CTextManager>.GetInstance().GetText("GETRANKREWARD_ERR_STATE_HaveGet");
				}
				else if (msg.stPkgData.get_stGetRankRewardRsp().bErrCode == 3)
				{
					strContent = Singleton<CTextManager>.GetInstance().GetText("GETRANKREWARD_ERR_STATE_Others");
				}
				else
				{
					strContent = Singleton<CTextManager>.GetInstance().GetText("GETRANKREWARD_ERR_STATE_None");
				}
				Singleton<CUIManager>.GetInstance().OpenMessageBox(strContent, false);
			}
		}

		public static bool IsInSeason()
		{
			ulong num = (ulong)((long)CRoleInfo.GetCurrentUTCTime());
			using (DictionaryView<uint, ResRankSeasonConf>.Enumerator enumerator = GameDataMgr.rankSeasonDict.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<uint, ResRankSeasonConf> current = enumerator.get_Current();
					if (num >= current.get_Value().ullStartTime && num <= current.get_Value().ullEndTime)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool IsInCredit()
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			ResGlobalInfo resGlobalInfo;
			return masterRoleInfo != null && GameDataMgr.svr2CltCfgDict.TryGetValue(5u, ref resGlobalInfo) && masterRoleInfo.creditScore > resGlobalInfo.dwConfValue;
		}

		private static int ComparisonGameData(COMDT_RANK_CURSEASON_FIGHT_RECORD a, COMDT_RANK_CURSEASON_FIGHT_RECORD b)
		{
			if (a.dwFightTime > b.dwFightTime)
			{
				return -1;
			}
			if (a.dwFightTime < b.dwFightTime)
			{
				return 1;
			}
			return 0;
		}

		private static int ComparisonHistoryData(COMDT_RANK_PASTSEASON_FIGHT_RECORD a, COMDT_RANK_PASTSEASON_FIGHT_RECORD b)
		{
			if (a.dwSeaEndTime > b.dwSeaEndTime)
			{
				return -1;
			}
			if (a.dwSeaEndTime < b.dwSeaEndTime)
			{
				return 1;
			}
			return 0;
		}

		public static int ConvertEloToRank(uint elo)
		{
			int count = GameDataMgr.rankGradeDatabin.count;
			int num = (int)(elo / 16u);
			int result = count;
			for (int i = 1; i <= count; i++)
			{
				ResRankGradeConf dataByKey = GameDataMgr.rankGradeDatabin.GetDataByKey((long)i);
				num -= (int)dataByKey.dwGradeUpNeedScore;
				if (num <= 0)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		public static int GetCurXingByEloAndRankLv(uint elo, uint lv)
		{
			int count = GameDataMgr.rankGradeDatabin.count;
			int num = (int)(elo / 16u);
			if ((ulong)lv >= (ulong)((long)count))
			{
				num = ((num < 163) ? 0 : (num - 163));
			}
			else
			{
				for (int i = 1; i < count; i++)
				{
					ResRankGradeConf dataByKey = GameDataMgr.rankGradeDatabin.GetDataByKey((long)i);
					if ((long)num <= (long)((ulong)dataByKey.dwGradeUpNeedScore))
					{
						break;
					}
					num -= (int)dataByKey.dwGradeUpNeedScore;
				}
			}
			return num;
		}

		public bool IsHaveFightRecord(bool isSelf, int rankGrade, int rankStar)
		{
			if (isSelf)
			{
				if (this.currentRankDetail != null)
				{
					return this.currentRankDetail.dwTotalFightCnt > 0u && this.currentRankDetail.bState == 1;
				}
			}
			else if (rankGrade > 1 || rankStar > 0)
			{
				return true;
			}
			return false;
		}

		public uint GetContinuousWinCountForExtraStar()
		{
			return this.GetContinuousWinCountForExtraStar((uint)Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_rankGrade);
		}

		public uint GetContinuousWinCountForExtraStar(uint rankGrade)
		{
			ResRankGradeConf dataByKey = GameDataMgr.rankGradeDatabin.GetDataByKey(rankGrade);
			if (dataByKey != null)
			{
				return dataByKey.dwConWinCnt;
			}
			return 0u;
		}

		public static byte GetRankBigGrade(byte rankGrade)
		{
			ResRankGradeConf dataByKey = GameDataMgr.rankGradeDatabin.GetDataByKey((uint)rankGrade);
			if (dataByKey != null)
			{
				return dataByKey.bBelongBigGrade;
			}
			return 0;
		}

		public static bool IsMaxRankGrade(byte rankGrade)
		{
			return (int)rankGrade >= CLadderSystem.MAX_RANK_LEVEL;
		}

		public static bool IsRecentUsedHeroMaskSet(ref uint CtrlMask, COM_RECENT_USED_HERO_MASK mask)
		{
			return ((ulong)CtrlMask & (ulong)mask) > 0uL;
		}

		public static void SetRecentUsedHeroMask(ref uint CtrlMask, COM_RECENT_USED_HERO_MASK mask, bool bOpen)
		{
			if (bOpen)
			{
				CtrlMask |= mask;
			}
			else
			{
				CtrlMask &= ~mask;
			}
		}

		[MessageHandler(5205)]
		public static void OnMatchTeamDestroyNtf(CSPkg msg)
		{
			byte bReason = msg.stPkgData.get_stMatchTeamDestroyNtf().stDetail.bReason;
			if (bReason == 20)
			{
				string text = UT.Bytes2String(msg.stPkgData.get_stMatchTeamDestroyNtf().stDetail.stReasonDetail.szLeaveAcntName);
				string text2 = Singleton<CTextManager>.get_instance().GetText("Err_Invite_Result_3", new string[]
				{
					text
				});
				Singleton<CUIManager>.get_instance().OpenTips(text2, false, 1.5f, null, new object[0]);
				Singleton<EventRouter>.get_instance().BroadCastEvent<byte, string>(EventID.INVITE_TEAM_ERRCODE_NTF, bReason, string.Empty);
			}
			else
			{
				Singleton<CUIManager>.get_instance().OpenTips(string.Format("CSProtocolMacros.SCID_MATCHTEAM_DESTROY_NTF bReason = {0} ", bReason), false, 1.5f, null, new object[0]);
			}
		}

		public bool IsUseBpMode()
		{
			return this.IsUseBpMode(Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_rankGrade);
		}

		public bool IsUseBpMode(byte rankGrade)
		{
			ResGlobalInfo resGlobalInfo = null;
			return GameDataMgr.svr2CltCfgDict != null && GameDataMgr.svr2CltCfgDict.TryGetValue(14u, ref resGlobalInfo) && resGlobalInfo != null && (uint)rankGrade >= resGlobalInfo.dwConfValue;
		}

		public string GetLadderSeasonName(ulong time)
		{
			using (DictionaryView<uint, ResRankSeasonConf>.Enumerator enumerator = GameDataMgr.rankSeasonDict.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<uint, ResRankSeasonConf> current = enumerator.get_Current();
					if (current.get_Value().ullStartTime <= time && time < current.get_Value().ullEndTime)
					{
						return current.get_Value().szSeasonName;
					}
				}
			}
			return string.Empty;
		}

		public byte GetHistorySeasonGrade(ulong time)
		{
			if (Singleton<CLadderSystem>.GetInstance().historySeasonData != null)
			{
				for (int i = 0; i < Singleton<CLadderSystem>.GetInstance().historySeasonData.get_Count(); i++)
				{
					if ((ulong)Singleton<CLadderSystem>.GetInstance().historySeasonData.get_Item(i).dwSeaStartTime <= time && time < (ulong)Singleton<CLadderSystem>.GetInstance().historySeasonData.get_Item(i).dwSeaEndTime)
					{
						return Singleton<CLadderSystem>.GetInstance().historySeasonData.get_Item(i).bGrade;
					}
				}
			}
			return 0;
		}

		public bool IsCurSeason(ulong time)
		{
			DictionaryView<uint, ResRankSeasonConf> rankSeasonDict = GameDataMgr.rankSeasonDict;
			if (rankSeasonDict != null)
			{
				DictionaryView<uint, ResRankSeasonConf>.Enumerator enumerator = rankSeasonDict.GetEnumerator();
				while (enumerator.MoveNext())
				{
					KeyValuePair<uint, ResRankSeasonConf> current = enumerator.get_Current();
					if (current.get_Value().ullStartTime == Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_rankCurSeasonStartTime)
					{
						KeyValuePair<uint, ResRankSeasonConf> current2 = enumerator.get_Current();
						if (current2.get_Value().ullStartTime <= time)
						{
							KeyValuePair<uint, ResRankSeasonConf> current3 = enumerator.get_Current();
							if (time < current3.get_Value().ullEndTime)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public bool IsValidGrade(int grade)
		{
			return grade > 0 && grade <= CLadderSystem.MAX_RANK_LEVEL;
		}

		public uint GetBraveScoreMax(uint rankGrade)
		{
			ResRankGradeConf resRankGradeConf = GameDataMgr.rankGradeDatabin.FindIf((ResRankGradeConf x) => (uint)x.bGrade == rankGrade);
			if (resRankGradeConf != null)
			{
				return resRankGradeConf.dwAddStarScore;
			}
			return 0u;
		}

		public uint GetSelfBraveScoreMax()
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null && masterRoleInfo.m_rankGrade > 0)
			{
				return this.GetBraveScoreMax((uint)masterRoleInfo.m_rankGrade);
			}
			return 0u;
		}

		public string GetRewardDesc(byte rankGrade)
		{
			byte rankBigGrade = CLadderSystem.GetRankBigGrade(rankGrade);
			int num = GameDataMgr.rankGradeDatabin.Count();
			for (int i = 0; i < num; i++)
			{
				ResRankGradeConf dataByIndex = GameDataMgr.rankGradeDatabin.GetDataByIndex(i);
				if (dataByIndex != null && dataByIndex.bBelongBigGrade == rankBigGrade)
				{
					return dataByIndex.szRewardDesc;
				}
			}
			return string.Empty;
		}

		public bool IsNeedPromptSkinReward()
		{
			return this.IsCanGetSkinReward() && !this.IsGotSkinReward();
		}

		public bool IsCanGetSkinReward()
		{
			uint dwConfValue = GameDataMgr.globalInfoDatabin.GetDataByKey(316u).dwConfValue;
			return (uint)Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_rankGrade >= dwConfValue && this.currentRankDetail != null && this.currentRankDetail.dwTotalFightCnt > 0u;
		}

		public bool IsGotSkinReward()
		{
			uint dwConfValue = GameDataMgr.globalInfoDatabin.GetDataByKey(300u).dwConfValue;
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			return masterRoleInfo.IsHaveHeroSkin(dwConfValue, false);
		}

		public CUseable GetSkinRewardUseable()
		{
			uint dwConfValue = GameDataMgr.globalInfoDatabin.GetDataByKey(300u).dwConfValue;
			return CUseableManager.CreateUseable(7, dwConfValue, 0);
		}

		public void PromptSkinRewardIfNeed()
		{
			if (this.IsNeedPromptSkinReward())
			{
				CUseable[] items = new CUseable[]
				{
					this.GetSkinRewardUseable()
				};
				string text = Singleton<CTextManager>.GetInstance().GetText("Ladder_Get_Skin_Reward_Msg_Title");
				Singleton<CUIManager>.GetInstance().OpenAwardTip(items, text, true, enUIEventID.Ladder_GetSkinReward, false, false, "Form_Award");
			}
		}
	}
}
