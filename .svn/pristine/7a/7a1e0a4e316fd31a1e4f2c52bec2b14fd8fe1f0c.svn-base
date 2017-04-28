using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.GameSystem;
using ResData;
using System;

public class NewbieGuideCheckTriggerConditionUtil
{
	public static uint AvailableHeroId;

	public static uint AvailableItemId;

	public static CTask AvailableTask;

	public static int AvailableSymbolPos;

	public static uint AvailableSymbolId;

	public static bool CheckTriggerCondition(uint id, NewbieGuideTriggerConditionItem condition)
	{
		switch (condition.wType)
		{
		case 1:
			return NewbieGuideCheckTriggerConditionUtil.CheckCompleteNewbieDungeonCondition(condition);
		case 2:
			return NewbieGuideCheckTriggerConditionUtil.CheckCompleteNormalDungeonCondition(condition);
		case 3:
			return NewbieGuideCheckTriggerConditionUtil.CheckOwnCompleteNewbieGuideCondition(condition);
		case 4:
			return NewbieGuideCheckTriggerConditionUtil.CheckUnCompleteNewbieGuideCondition(condition);
		case 5:
			return false;
		case 6:
			return false;
		case 7:
			NewbieGuideCheckTriggerConditionUtil.AvailableTask = null;
			return Singleton<CTaskSys>.get_instance().model.AnyTaskOfState(1, 0, out NewbieGuideCheckTriggerConditionUtil.AvailableTask);
		case 8:
			return false;
		case 9:
			return false;
		case 10:
			return false;
		case 11:
		{
			uint num = condition.Param[0];
			uint num2 = condition.Param[1];
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				uint haveHeroCount = (uint)masterRoleInfo.GetHaveHeroCount(false);
				bool result = false;
				switch (num2)
				{
				case 0u:
					result = (haveHeroCount == num);
					break;
				case 1u:
					result = (haveHeroCount > num);
					break;
				case 2u:
					result = (haveHeroCount < num);
					break;
				default:
					DebugHelper.Assert(false);
					break;
				}
				return result;
			}
			return false;
		}
		case 12:
		{
			bool flag = false;
			uint num3 = condition.Param[0];
			RES_SHOPBUY_COINTYPE coinType = (condition.Param[1] != 0u) ? 4 : 2;
			CRoleInfo masterRoleInfo2 = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo2 != null)
			{
				if (num3 > 0u)
				{
					flag = masterRoleInfo2.CheckHeroBuyable(num3, coinType);
				}
				else
				{
					ListView<ResHeroCfgInfo>.Enumerator enumerator = CHeroDataFactory.GetAllHeroList().GetEnumerator();
					while (enumerator.MoveNext())
					{
						num3 = enumerator.get_Current().dwCfgID;
						flag |= masterRoleInfo2.CheckHeroBuyable(num3, coinType);
						if (flag)
						{
							break;
						}
					}
				}
			}
			if (flag)
			{
				NewbieGuideCheckTriggerConditionUtil.AvailableHeroId = num3;
			}
			return flag;
		}
		case 13:
			return Singleton<CShopSystem>.GetInstance().IsNormalShopItemsInited();
		case 14:
			return Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().GoldCoin >= condition.Param[0] && Singleton<CFunctionUnlockSys>.GetInstance().FucIsUnlock(12) && Singleton<CShopSystem>.GetInstance().IsNormalShopItemsInited();
		case 15:
			return CAdventureSys.IsChapterFullStar(Singleton<CAdventureSys>.GetInstance().currentChapter, Singleton<CAdventureSys>.GetInstance().currentDifficulty);
		case 16:
		{
			uint num4 = condition.Param[0];
			return num4 > 0u && Singleton<CAdventureSys>.GetInstance().IsLevelFinished((int)num4) && CAdventureSys.IsLevelFullStar((int)num4);
		}
		case 17:
			return Singleton<CFunctionUnlockSys>.GetInstance().FucIsUnlock(10);
		case 18:
		{
			CRoleInfo masterRoleInfo3 = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			CUseableContainer useableContainer = masterRoleInfo3.GetUseableContainer(enCONTAINER_TYPE.ITEM);
			int useableStackCount = useableContainer.GetUseableStackCount(2, CAdventureSys.MOPUP_TICKET_ID);
			return (long)useableStackCount >= (long)((ulong)condition.Param[0]);
		}
		case 19:
			return Singleton<CFunctionUnlockSys>.GetInstance().FucIsUnlock(3);
		case 20:
			return Singleton<CFunctionUnlockSys>.GetInstance().FucIsUnlock(5);
		case 21:
			return Singleton<CFunctionUnlockSys>.GetInstance().FucIsUnlock(4);
		case 22:
			return Singleton<CFunctionUnlockSys>.GetInstance().FucIsUnlock(6);
		case 23:
			return Singleton<CFunctionUnlockSys>.GetInstance().FucIsUnlock(13);
		case 24:
			return Singleton<CFunctionUnlockSys>.GetInstance().FucIsUnlock(8);
		case 25:
			return Singleton<CFunctionUnlockSys>.GetInstance().FucIsUnlock(11) && Singleton<CShopSystem>.GetInstance().IsMysteryShopAvailable();
		case 26:
			return Singleton<CFunctionUnlockSys>.GetInstance().FucIsUnlock(15);
		case 27:
			return Singleton<CFunctionUnlockSys>.GetInstance().FucIsUnlock(16);
		case 28:
			return false;
		case 30:
			return Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().IsGuidedStateSet(26);
		case 31:
			return Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().IsGuidedStateSet(27);
		case 32:
			return Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().IsGuidedStateSet(21);
		case 33:
		{
			CRoleInfo masterRoleInfo4 = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			return masterRoleInfo4 != null && masterRoleInfo4.GoldCoin >= condition.Param[0];
		}
		case 34:
			return false;
		case 36:
			return false;
		case 37:
		{
			bool flag2 = false;
			uint num5 = condition.Param[0];
			CRoleInfo masterRoleInfo5 = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo5 != null)
			{
				CUseableContainer useableContainer2 = masterRoleInfo5.GetUseableContainer(enCONTAINER_TYPE.ITEM);
				if (useableContainer2 != null)
				{
					CUseable useableByBaseID = useableContainer2.GetUseableByBaseID(2, num5);
					if (useableByBaseID != null)
					{
						flag2 = true;
					}
				}
			}
			NewbieGuideCheckTriggerConditionUtil.AvailableItemId = ((!flag2) ? 0u : num5);
			return flag2;
		}
		case 38:
			return Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().IsGuidedStateSet(9);
		case 39:
		{
			uint num6 = condition.Param[0];
			CRoleInfo masterRoleInfo6 = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			return masterRoleInfo6 != null && (long)masterRoleInfo6.GetHaveHeroCount(false) >= (long)((ulong)num6);
		}
		case 40:
		{
			CRoleInfo masterRoleInfo7 = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo7 != null)
			{
				masterRoleInfo7.m_symbolInfo.CheckAnyWearSymbol(out NewbieGuideCheckTriggerConditionUtil.AvailableSymbolPos, out NewbieGuideCheckTriggerConditionUtil.AvailableSymbolId, 2);
				return (long)NewbieGuideCheckTriggerConditionUtil.AvailableSymbolPos == (long)((ulong)condition.Param[0]);
			}
			return false;
		}
		case 41:
			return Singleton<CFunctionUnlockSys>.GetInstance().FucIsUnlock(9);
		case 42:
			return Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().IsGuidedStateSet(44);
		case 43:
		{
			uint num7 = condition.Param[0];
			CRoleInfo masterRoleInfo8 = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			return masterRoleInfo8 != null && masterRoleInfo8.SymbolCoin >= num7;
		}
		case 44:
			return Singleton<CFunctionUnlockSys>.GetInstance().FucIsUnlock(22);
		case 45:
			return CAddSkillSys.IsSelSkillAvailable();
		case 46:
			return NewbieGuideCheckTriggerConditionUtil.CheckOwnCompleteNewWeakGuideCondition(condition);
		case 47:
			return NewbieGuideCheckTriggerConditionUtil.CheckOwnCompleteNewWeakGuideCondition(condition);
		case 48:
		{
			CRoleInfo masterRoleInfo9 = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			return masterRoleInfo9.m_symbolInfo.m_pageCount > 1;
		}
		case 49:
		{
			LevelRewardData levelRewardData = Singleton<CTaskSys>.get_instance().model.GetLevelRewardData((int)condition.Param[0]);
			return levelRewardData != null && !levelRewardData.m_bHasGetReward;
		}
		case 50:
		{
			CRoleInfo masterRoleInfo10 = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			return masterRoleInfo10.m_freeDrawInfo[4].dwLeftFreeDrawCnt > 0;
		}
		case 51:
			return Singleton<CFunctionUnlockSys>.get_instance().FucIsUnlock(condition.Param[0]);
		case 52:
		{
			CRoleInfo masterRoleInfo11 = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			return masterRoleInfo11 != null && masterRoleInfo11.IsOldPlayer() && !masterRoleInfo11.IsOldPlayerGuided();
		}
		case 53:
		{
			SLevelContext curLvelContext = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
			return curLvelContext != null && curLvelContext.IsMultilModeWithWarmBattle();
		}
		case 54:
		{
			bool result2 = false;
			CRoleInfo masterRoleInfo12 = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo12 != null)
			{
				result2 = masterRoleInfo12.IsGuidedStateSet(98);
			}
			return result2;
		}
		case 55:
			return CBattleGuideManager.EnableHeroVictoryTips();
		case 56:
			return Singleton<GameReplayModule>.GetInstance().HasRecord;
		case 57:
		{
			SLevelContext curLvelContext2 = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
			return curLvelContext2 != null && (long)curLvelContext2.m_mapID == (long)((ulong)condition.Param[0]);
		}
		}
		return false;
	}

	private static bool CheckOwnCompleteNewbieGuideCondition(NewbieGuideTriggerConditionItem condition)
	{
		return MonoSingleton<NewbieGuideManager>.GetInstance().IsNewbieGuideComplete(condition.Param[0]);
	}

	private static bool CheckOwnCompleteNewWeakGuideCondition(NewbieGuideTriggerConditionItem condition)
	{
		return MonoSingleton<NewbieGuideManager>.GetInstance().IsWeakLineComplete(condition.Param[0]);
	}

	private static bool CheckCompleteNormalDungeonCondition(NewbieGuideTriggerConditionItem condition)
	{
		return Singleton<CAdventureSys>.GetInstance().IsLevelFinished((int)condition.Param[0]);
	}

	private static bool CheckCompleteNewbieDungeonCondition(NewbieGuideTriggerConditionItem condition)
	{
		return Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().IsGuidedStateSet(0);
	}

	private static bool CheckUnCompleteNewbieGuideCondition(NewbieGuideTriggerConditionItem condition)
	{
		bool flag = MonoSingleton<NewbieGuideManager>.GetInstance().IsNewbieGuideComplete(condition.Param[0]);
		return !flag;
	}
}
