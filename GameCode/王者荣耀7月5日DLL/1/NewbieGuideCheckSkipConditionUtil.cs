using Assets.Scripts.GameSystem;
using ResData;
using System;

public class NewbieGuideCheckSkipConditionUtil
{
	public static NewbieGuideSkipConditionType TranslateToSkipCond(int inNewbieType)
	{
		NewbieGuideSkipConditionType result = NewbieGuideSkipConditionType.Invalid;
		switch (inNewbieType)
		{
		case 0:
			result = NewbieGuideSkipConditionType.hasCompleteBaseGuide;
			return result;
		case 1:
			result = NewbieGuideSkipConditionType.hasCompleteEquipping;
			return result;
		case 2:
			result = NewbieGuideSkipConditionType.hasCompleteHeroAdv;
			return result;
		case 3:
			result = NewbieGuideSkipConditionType.hasSummonedHero;
			return result;
		case 4:
			result = NewbieGuideSkipConditionType.hasCompleteHeroStar;
			return result;
		case 5:
			result = NewbieGuideSkipConditionType.hasHeroSkillUpgraded;
			return result;
		case 6:
			result = NewbieGuideSkipConditionType.hasCompleteLottery;
			return result;
		case 7:
			result = NewbieGuideSkipConditionType.hasOverThreeHeroes;
			return result;
		case 8:
			result = NewbieGuideSkipConditionType.hasRewardTaskPvp;
			return result;
		case 9:
			result = NewbieGuideSkipConditionType.hasBoughtHero;
			return result;
		case 10:
			result = NewbieGuideSkipConditionType.hasBoughtItem;
			return result;
		case 11:
			result = NewbieGuideSkipConditionType.hasGotChapterReward;
			return result;
		case 12:
			result = NewbieGuideSkipConditionType.hasMopup;
			return result;
		case 13:
			result = NewbieGuideSkipConditionType.hasEnteredPvP;
			return result;
		case 14:
			result = NewbieGuideSkipConditionType.hasEnteredTrial;
			return result;
		case 15:
			result = NewbieGuideSkipConditionType.hasEnteredZhuangzi;
			return result;
		case 16:
			result = NewbieGuideSkipConditionType.hasEnteredBurning;
			return result;
		case 17:
			result = NewbieGuideSkipConditionType.hasEnteredElitePvE;
			return result;
		case 18:
			result = NewbieGuideSkipConditionType.hasEnteredGuild;
			return result;
		case 19:
			result = NewbieGuideSkipConditionType.hasUsedSymbol;
			return result;
		case 20:
			result = NewbieGuideSkipConditionType.hasEnteredMysteryShop;
			return result;
		case 21:
			result = NewbieGuideSkipConditionType.hasComplete33Guide;
			return result;
		case 22:
		case 23:
		case 24:
		case 25:
		case 28:
		case 31:
		case 35:
		case 36:
		case 37:
		case 38:
		case 39:
		case 40:
		case 41:
		case 42:
		case 43:
		case 45:
		case 46:
		case 47:
		case 48:
		case 49:
		case 50:
		case 51:
		case 52:
		case 53:
		case 54:
		case 55:
		case 56:
		case 57:
			IL_F6:
			switch (inNewbieType)
			{
			case 80:
				result = NewbieGuideSkipConditionType.hasCoinDrawFive;
				return result;
			case 81:
				result = NewbieGuideSkipConditionType.hasComplete11Match;
				return result;
			case 82:
			case 84:
				IL_117:
				switch (inNewbieType)
				{
				case 98:
					result = NewbieGuideSkipConditionType.hasCompleteTrainLevel55;
					break;
				case 100:
					result = NewbieGuideSkipConditionType.hasDiamondDraw;
					break;
				}
				return result;
			case 83:
				result = NewbieGuideSkipConditionType.hasCompleteCoronaGuide;
				return result;
			case 85:
				result = NewbieGuideSkipConditionType.hasCompleteTrainLevel33;
				return result;
			}
			goto IL_117;
		case 26:
			result = NewbieGuideSkipConditionType.hasCompleteHumanAi33Match;
			return result;
		case 27:
			result = NewbieGuideSkipConditionType.hasCompleteHuman33Match;
			return result;
		case 29:
			result = NewbieGuideSkipConditionType.hasIncreaseEquip;
			return result;
		case 30:
			result = NewbieGuideSkipConditionType.hasAdvancedEquip;
			return result;
		case 32:
			result = NewbieGuideSkipConditionType.hasCompleteHeroUp;
			return result;
		case 33:
			result = NewbieGuideSkipConditionType.hasEnteredTournament;
			return result;
		case 34:
			result = NewbieGuideSkipConditionType.hasRewardTaskPve;
			return result;
		case 44:
			result = NewbieGuideSkipConditionType.hasCompleteHumanAi33;
			return result;
		case 58:
			result = NewbieGuideSkipConditionType.hasManufacuredSymbol;
			return result;
		}
		goto IL_F6;
	}

	public static int TranslateFromSkipCond(NewbieGuideSkipConditionType inCondType)
	{
		int result = -1;
		switch (inCondType)
		{
		case NewbieGuideSkipConditionType.hasCompleteEquipping:
			result = 1;
			return result;
		case NewbieGuideSkipConditionType.hasRewardTaskPvp:
			result = 8;
			return result;
		case NewbieGuideSkipConditionType.hasCompleteHeroAdv:
			result = 2;
			return result;
		case NewbieGuideSkipConditionType.hasSummonedHero:
			result = 3;
			return result;
		case NewbieGuideSkipConditionType.hasOverThreeHeroes:
			result = 7;
			return result;
		case NewbieGuideSkipConditionType.hasCompleteHeroStar:
			result = 4;
			return result;
		case NewbieGuideSkipConditionType.hasHeroSkillUpgraded:
			result = 5;
			return result;
		case NewbieGuideSkipConditionType.hasBoughtHero:
			result = 9;
			return result;
		case NewbieGuideSkipConditionType.hasBoughtItem:
			result = 10;
			return result;
		case NewbieGuideSkipConditionType.hasGotChapterReward:
			result = 11;
			return result;
		case NewbieGuideSkipConditionType.hasMopup:
			result = 12;
			return result;
		case NewbieGuideSkipConditionType.hasEnteredPvP:
			result = 13;
			return result;
		case NewbieGuideSkipConditionType.hasEnteredTrial:
			result = 14;
			return result;
		case NewbieGuideSkipConditionType.hasEnteredZhuangzi:
			result = 15;
			return result;
		case NewbieGuideSkipConditionType.hasEnteredBurning:
			result = 16;
			return result;
		case NewbieGuideSkipConditionType.hasEnteredElitePvE:
			result = 17;
			return result;
		case NewbieGuideSkipConditionType.hasEnteredGuild:
			result = 18;
			return result;
		case NewbieGuideSkipConditionType.hasUsedSymbol:
			result = 19;
			return result;
		case NewbieGuideSkipConditionType.hasEnteredMysteryShop:
			result = 20;
			return result;
		case NewbieGuideSkipConditionType.hasAdvancedEquip:
			result = 30;
			return result;
		case NewbieGuideSkipConditionType.hasCompleteBaseGuide:
			result = 0;
			return result;
		case NewbieGuideSkipConditionType.hasCompleteHumanAi33Match:
			result = 26;
			return result;
		case NewbieGuideSkipConditionType.hasCompleteHuman33Match:
			result = 27;
			return result;
		case NewbieGuideSkipConditionType.hasComplete33Guide:
			result = 21;
			return result;
		case NewbieGuideSkipConditionType.hasCompleteLottery:
			result = 6;
			return result;
		case NewbieGuideSkipConditionType.hasIncreaseEquip:
			result = 29;
			return result;
		case NewbieGuideSkipConditionType.hasRewardTaskPve:
			result = 34;
			return result;
		case NewbieGuideSkipConditionType.hasCompleteHeroUp:
			result = 32;
			return result;
		case NewbieGuideSkipConditionType.hasEnteredTournament:
			result = 33;
			return result;
		case NewbieGuideSkipConditionType.hasCompleteHumanAi33:
			result = 44;
			return result;
		case NewbieGuideSkipConditionType.hasManufacuredSymbol:
			result = 58;
			return result;
		case NewbieGuideSkipConditionType.hasCoinDrawFive:
			result = 80;
			return result;
		case NewbieGuideSkipConditionType.hasCompleteTrainLevel55:
			result = 98;
			return result;
		case NewbieGuideSkipConditionType.hasComplete11Match:
			result = 81;
			return result;
		case NewbieGuideSkipConditionType.hasCompleteTrainLevel33:
			result = 85;
			return result;
		case NewbieGuideSkipConditionType.hasDiamondDraw:
			result = 100;
			return result;
		case NewbieGuideSkipConditionType.hasCompleteCoronaGuide:
			result = 83;
			return result;
		}
		DebugHelper.Assert(false);
		return result;
	}

	public static bool CheckSkipCondition(NewbieGuideSkipConditionItem item, uint[] param)
	{
		switch (item.wType)
		{
		case 1:
		{
			bool result = false;
			if (param != null && param.Length > 0)
			{
				if (param[0] == item.Param[0])
				{
					result = Singleton<CAdventureSys>.GetInstance().IsLevelFinished((int)param[0]);
				}
			}
			else
			{
				result = Singleton<CAdventureSys>.GetInstance().IsLevelFinished((int)item.Param[0]);
			}
			return result;
		}
		case 2:
			return MonoSingleton<NewbieGuideManager>.GetInstance().IsNewbieGuideComplete(item.Param[0]);
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
		case 11:
		case 12:
		case 13:
		case 14:
		case 15:
		case 16:
		case 17:
		case 18:
		case 19:
		case 20:
		case 21:
		case 22:
		case 23:
		case 24:
		case 31:
		case 32:
		case 33:
		case 34:
		case 35:
		case 36:
		case 37:
		case 38:
		case 40:
		case 41:
		case 45:
		case 46:
		case 47:
		case 48:
		case 51:
		{
			int num = NewbieGuideCheckSkipConditionUtil.TranslateFromSkipCond((NewbieGuideSkipConditionType)item.wType);
			return num == -1 || Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().IsGuidedStateSet(num);
		}
		case 30:
			return MonoSingleton<NewbieGuideManager>.GetInstance().IsNewbieBitSet((int)item.Param[0]);
		case 42:
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				CUseableContainer useableContainer = masterRoleInfo.GetUseableContainer(enCONTAINER_TYPE.ITEM);
				if (useableContainer != null)
				{
					int useableStackCount = useableContainer.GetUseableStackCount(5, item.Param[0]);
					return useableStackCount >= 2;
				}
			}
			return false;
		}
		case 43:
		{
			int num2 = NewbieGuideCheckSkipConditionUtil.TranslateFromSkipCond((NewbieGuideSkipConditionType)item.wType);
			if (num2 == -1)
			{
				return true;
			}
			if (item.Param[0] == 0u)
			{
				return Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().IsGuidedStateSet(num2);
			}
			return !Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().IsGuidedStateSet(num2);
		}
		case 44:
		{
			CRoleInfo masterRoleInfo2 = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			return masterRoleInfo2.IsNewbieAchieveSet((int)(item.Param[0] + (uint)NewbieGuideManager.WEAKGUIDE_BIT_OFFSET));
		}
		case 49:
		{
			CRoleInfo masterRoleInfo3 = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo3 == null)
			{
				return false;
			}
			bool flag = masterRoleInfo3.IsGuidedStateSet(89);
			bool flag2 = masterRoleInfo3.IsGuidedStateSet(90);
			bool arg_25E_0 = flag || flag2;
			return masterRoleInfo3.IsGuidedStateSet(89) || masterRoleInfo3.IsGuidedStateSet(90);
		}
		}
		return true;
	}
}
