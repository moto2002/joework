using Assets.Scripts.Common;
using System;

namespace Assets.Scripts.GameLogic.SkillFunc
{
	[SkillFuncHandlerClass]
	public class SkillFuncHurtDelegator
	{
		private static int GetEffectFadeRate(ref SSkillFuncContext inContext)
		{
			int iNextDeltaFadeRate = inContext.inBuffSkill.get_handle().cfgData.iNextDeltaFadeRate;
			int iNextLowFadeRate = inContext.inBuffSkill.get_handle().cfgData.iNextLowFadeRate;
			int num = 10000 - (inContext.inEffectCount - 1) * iNextDeltaFadeRate;
			return (num >= iNextLowFadeRate) ? num : iNextLowFadeRate;
		}

		private static int GetOverlayFadeRate(ref SSkillFuncContext inContext)
		{
			PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
			int num = 10000 - inContext.inBuffSkill.get_handle().cfgData.iOverlayFadeRate;
			if (num == 10000)
			{
				return 10000;
			}
			if (num < 10000 && num >= 0 && inTargetObj && inTargetObj.get_handle().BuffHolderComp != null)
			{
				int skillID = inContext.inBuffSkill.get_handle().SkillID;
				if (inContext.inBuffSkill && inContext.inBuffSkill.get_handle().bFirstEffect)
				{
					inContext.inBuffSkill.get_handle().bFirstEffect = false;
					return 10000;
				}
				int num2 = inTargetObj.get_handle().BuffHolderComp.FindBuffCount(skillID);
				if (num2 > 1)
				{
					return num;
				}
			}
			return 10000;
		}

		private static bool HandleSkillFuncHurt(ref SSkillFuncContext inContext, HurtTypeDef hurtType)
		{
			int num = 0;
			if (inContext.inStage == ESkillFuncStage.Enter)
			{
				PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
				PoolObjHandle<ActorRoot> inOriginator = inContext.inOriginator;
				if (inTargetObj && !inTargetObj.get_handle().ActorControl.IsDeadState)
				{
					inContext.inCustomData = default(HurtAttackerInfo);
					inContext.inCustomData.Init(inOriginator, inTargetObj);
					HurtDataInfo hurtDataInfo;
					hurtDataInfo.atker = inOriginator;
					hurtDataInfo.target = inTargetObj;
					hurtDataInfo.attackInfo = inContext.inCustomData;
					hurtDataInfo.atkSlot = inContext.inUseContext.SlotType;
					hurtDataInfo.hurtType = hurtType;
					hurtDataInfo.extraHurtType = (ExtraHurtTypeDef)inContext.GetSkillFuncParam(0, false);
					hurtDataInfo.hurtValue = inContext.GetSkillFuncParam(1, true);
					hurtDataInfo.adValue = inContext.GetSkillFuncParam(2, true);
					hurtDataInfo.apValue = inContext.GetSkillFuncParam(3, true);
					hurtDataInfo.hpValue = inContext.GetSkillFuncParam(4, true);
					hurtDataInfo.loseHpValue = inContext.GetSkillFuncParam(5, true);
					hurtDataInfo.iConditionType = inContext.GetSkillFuncParam(6, false);
					hurtDataInfo.iConditionParam = inContext.GetSkillFuncParam(7, true);
					hurtDataInfo.hurtCount = inContext.inDoCount;
					hurtDataInfo.firstHemoFadeRate = inContext.inBuffSkill.get_handle().cfgData.iFirstLifeStealAttenuation;
					hurtDataInfo.followUpHemoFadeRate = inContext.inBuffSkill.get_handle().cfgData.iFollowUpLifeStealAttenuation;
					hurtDataInfo.iEffectCountInSingleTrigger = inContext.inEffectCountInSingleTrigger;
					hurtDataInfo.bExtraBuff = inContext.inBuffSkill.get_handle().bExtraBuff;
					hurtDataInfo.gatherTime = inContext.inUseContext.GatherTime;
					bool bBounceHurt = inContext.inBuffSkill.get_handle().cfgData.bEffectSubType == 9;
					hurtDataInfo.bBounceHurt = bBounceHurt;
					hurtDataInfo.bLastHurt = inContext.inLastEffect;
					hurtDataInfo.iAddTotalHurtValueRate = 0;
					hurtDataInfo.iAddTotalHurtValue = 0;
					hurtDataInfo.iCanSkillCrit = (int)inContext.inBuffSkill.get_handle().cfgData.bCanSkillCrit;
					hurtDataInfo.iDamageLimit = inContext.inBuffSkill.get_handle().cfgData.iDamageLimit;
					hurtDataInfo.iMonsterDamageLimit = inContext.inBuffSkill.get_handle().cfgData.iMonsterDamageLimit;
					hurtDataInfo.iLongRangeReduction = inContext.inBuffSkill.get_handle().cfgData.iLongRangeReduction;
					hurtDataInfo.iEffectiveTargetType = (int)inContext.inBuffSkill.get_handle().cfgData.bEffectiveTargetType;
					hurtDataInfo.iOverlayFadeRate = SkillFuncHurtDelegator.GetOverlayFadeRate(ref inContext);
					hurtDataInfo.iEffectFadeRate = SkillFuncHurtDelegator.GetEffectFadeRate(ref inContext);
					hurtDataInfo.iReduceDamage = 0;
					num = inTargetObj.get_handle().ActorControl.TakeDamage(ref hurtDataInfo);
					inContext.inAction.get_handle().refParams.AddRefParam("HurtValue", -num);
				}
			}
			else if (inContext.inStage == ESkillFuncStage.Update)
			{
				PoolObjHandle<ActorRoot> inTargetObj2 = inContext.inTargetObj;
				PoolObjHandle<ActorRoot> inOriginator2 = inContext.inOriginator;
				if (inTargetObj2 && !inTargetObj2.get_handle().ActorControl.IsDeadState)
				{
					HurtDataInfo hurtDataInfo2;
					hurtDataInfo2.atker = inOriginator2;
					hurtDataInfo2.target = inTargetObj2;
					hurtDataInfo2.attackInfo = inContext.inCustomData;
					hurtDataInfo2.atkSlot = inContext.inUseContext.SlotType;
					hurtDataInfo2.hurtType = hurtType;
					hurtDataInfo2.extraHurtType = (ExtraHurtTypeDef)inContext.GetSkillFuncParam(0, false);
					hurtDataInfo2.hurtValue = inContext.GetSkillFuncParam(1, true);
					hurtDataInfo2.adValue = inContext.GetSkillFuncParam(2, true);
					hurtDataInfo2.apValue = inContext.GetSkillFuncParam(3, true);
					hurtDataInfo2.hpValue = inContext.GetSkillFuncParam(4, true);
					hurtDataInfo2.loseHpValue = inContext.GetSkillFuncParam(5, true);
					hurtDataInfo2.iConditionType = inContext.GetSkillFuncParam(6, false);
					hurtDataInfo2.iConditionParam = inContext.GetSkillFuncParam(7, true);
					hurtDataInfo2.hurtCount = inContext.inDoCount;
					hurtDataInfo2.firstHemoFadeRate = inContext.inBuffSkill.get_handle().cfgData.iFirstLifeStealAttenuation;
					hurtDataInfo2.followUpHemoFadeRate = inContext.inBuffSkill.get_handle().cfgData.iFollowUpLifeStealAttenuation;
					hurtDataInfo2.iEffectCountInSingleTrigger = inContext.inEffectCountInSingleTrigger;
					hurtDataInfo2.bExtraBuff = inContext.inBuffSkill.get_handle().bExtraBuff;
					hurtDataInfo2.gatherTime = inContext.inUseContext.GatherTime;
					bool bBounceHurt2 = inContext.inBuffSkill.get_handle().cfgData.bEffectSubType == 9;
					hurtDataInfo2.bBounceHurt = bBounceHurt2;
					hurtDataInfo2.bLastHurt = inContext.inLastEffect;
					hurtDataInfo2.iAddTotalHurtValueRate = 0;
					hurtDataInfo2.iAddTotalHurtValue = 0;
					hurtDataInfo2.iCanSkillCrit = (int)inContext.inBuffSkill.get_handle().cfgData.bCanSkillCrit;
					hurtDataInfo2.iDamageLimit = inContext.inBuffSkill.get_handle().cfgData.iDamageLimit;
					hurtDataInfo2.iMonsterDamageLimit = inContext.inBuffSkill.get_handle().cfgData.iMonsterDamageLimit;
					hurtDataInfo2.iLongRangeReduction = inContext.inBuffSkill.get_handle().cfgData.iLongRangeReduction;
					hurtDataInfo2.iEffectiveTargetType = (int)inContext.inBuffSkill.get_handle().cfgData.bEffectiveTargetType;
					hurtDataInfo2.iOverlayFadeRate = SkillFuncHurtDelegator.GetOverlayFadeRate(ref inContext);
					hurtDataInfo2.iEffectFadeRate = SkillFuncHurtDelegator.GetEffectFadeRate(ref inContext);
					hurtDataInfo2.iReduceDamage = 0;
					num = inTargetObj2.get_handle().ActorControl.TakeDamage(ref hurtDataInfo2);
					inContext.inAction.get_handle().refParams.AddRefParam("HurtValue", -num);
				}
			}
			return num != 0;
		}

		[SkillFuncHandler(0, new int[]
		{

		})]
		public static bool OnSkillFuncPhysHurt(ref SSkillFuncContext inContext)
		{
			return inContext.inStage != ESkillFuncStage.Leave && SkillFuncHurtDelegator.HandleSkillFuncHurt(ref inContext, HurtTypeDef.PhysHurt);
		}

		[SkillFuncHandler(1, new int[]
		{

		})]
		public static bool OnSkillFuncMagicHurt(ref SSkillFuncContext inContext)
		{
			return inContext.inStage != ESkillFuncStage.Leave && SkillFuncHurtDelegator.HandleSkillFuncHurt(ref inContext, HurtTypeDef.MagicHurt);
		}

		[SkillFuncHandler(2, new int[]
		{

		})]
		public static bool OnSkillFuncRealHurt(ref SSkillFuncContext inContext)
		{
			return inContext.inStage != ESkillFuncStage.Leave && SkillFuncHurtDelegator.HandleSkillFuncHurt(ref inContext, HurtTypeDef.RealHurt);
		}

		[SkillFuncHandler(3, new int[]
		{

		})]
		public static bool OnSkillFuncAddHp(ref SSkillFuncContext inContext)
		{
			return inContext.inStage != ESkillFuncStage.Leave && SkillFuncHurtDelegator.HandleSkillFuncHurt(ref inContext, HurtTypeDef.Therapic);
		}

		private static int GetSkillFuncProtectValue(ref SSkillFuncContext inContext)
		{
			int result = 0;
			if (inContext.inStage == ESkillFuncStage.Enter)
			{
				PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
				PoolObjHandle<ActorRoot> inOriginator = inContext.inOriginator;
				if (inTargetObj && inOriginator)
				{
					inContext.inCustomData = default(HurtAttackerInfo);
					inContext.inCustomData.Init(inOriginator, inTargetObj);
					HurtDataInfo hurtDataInfo;
					hurtDataInfo.atker = inOriginator;
					hurtDataInfo.target = inTargetObj;
					hurtDataInfo.attackInfo = inContext.inCustomData;
					hurtDataInfo.atkSlot = inContext.inUseContext.SlotType;
					hurtDataInfo.hurtType = HurtTypeDef.PhysHurt;
					hurtDataInfo.extraHurtType = ExtraHurtTypeDef.ExtraHurt_Value;
					hurtDataInfo.hurtValue = inContext.GetSkillFuncParam(1, true);
					hurtDataInfo.adValue = inContext.GetSkillFuncParam(2, true);
					hurtDataInfo.apValue = inContext.GetSkillFuncParam(3, true);
					hurtDataInfo.hpValue = inContext.GetSkillFuncParam(4, true);
					hurtDataInfo.iConditionType = inContext.GetSkillFuncParam(6, false);
					hurtDataInfo.iConditionParam = inContext.GetSkillFuncParam(7, true);
					hurtDataInfo.loseHpValue = 0;
					hurtDataInfo.hurtCount = inContext.inDoCount;
					hurtDataInfo.firstHemoFadeRate = 10000;
					hurtDataInfo.followUpHemoFadeRate = 10000;
					hurtDataInfo.iEffectCountInSingleTrigger = 1;
					hurtDataInfo.bExtraBuff = false;
					hurtDataInfo.gatherTime = inContext.inUseContext.GatherTime;
					hurtDataInfo.bBounceHurt = false;
					hurtDataInfo.bLastHurt = inContext.inLastEffect;
					hurtDataInfo.iAddTotalHurtValueRate = 0;
					hurtDataInfo.iAddTotalHurtValue = 0;
					hurtDataInfo.iCanSkillCrit = (int)inContext.inBuffSkill.get_handle().cfgData.bCanSkillCrit;
					hurtDataInfo.iDamageLimit = inContext.inBuffSkill.get_handle().cfgData.iDamageLimit;
					hurtDataInfo.iMonsterDamageLimit = inContext.inBuffSkill.get_handle().cfgData.iMonsterDamageLimit;
					hurtDataInfo.iLongRangeReduction = inContext.inBuffSkill.get_handle().cfgData.iLongRangeReduction;
					hurtDataInfo.iEffectiveTargetType = (int)inContext.inBuffSkill.get_handle().cfgData.bEffectiveTargetType;
					hurtDataInfo.iOverlayFadeRate = 10000;
					hurtDataInfo.iEffectFadeRate = 10000;
					hurtDataInfo.iReduceDamage = 0;
					result = inTargetObj.get_handle().ActorControl.actor.HurtControl.CommonDamagePart(ref hurtDataInfo);
				}
			}
			return result;
		}

		private static void SendProtectEvent(ref SSkillFuncContext inContext, int type, int changeValue)
		{
			if (changeValue != 0 && inContext.inTargetObj && inContext.inTargetObj.get_handle().BuffHolderComp != null)
			{
				BuffProtectRule protectRule = inContext.inTargetObj.get_handle().BuffHolderComp.protectRule;
				protectRule.SendProtectEvent(type, changeValue);
			}
		}

		[SkillFuncHandler(27, new int[]
		{

		})]
		public static bool OnSkillFuncProtect(ref SSkillFuncContext inContext)
		{
			if (inContext.inStage == ESkillFuncStage.Enter)
			{
				int skillFuncProtectValue = SkillFuncHurtDelegator.GetSkillFuncProtectValue(ref inContext);
				int skillFuncParam = inContext.GetSkillFuncParam(0, false);
				int skillFuncParam2 = inContext.GetSkillFuncParam(5, false);
				int skillFuncParam3 = inContext.GetSkillFuncParam(6, false);
				if (skillFuncParam == 1)
				{
					inContext.inBuffSkill.get_handle().CustomParams[0] += skillFuncProtectValue;
					SkillFuncHurtDelegator.SendProtectEvent(ref inContext, 0, skillFuncProtectValue);
				}
				else if (skillFuncParam == 2)
				{
					inContext.inBuffSkill.get_handle().CustomParams[1] += skillFuncProtectValue;
					SkillFuncHurtDelegator.SendProtectEvent(ref inContext, 1, skillFuncProtectValue);
				}
				else if (skillFuncParam == 3)
				{
					inContext.inBuffSkill.get_handle().CustomParams[2] += skillFuncProtectValue;
					SkillFuncHurtDelegator.SendProtectEvent(ref inContext, 2, skillFuncProtectValue);
				}
				else if (skillFuncParam == 4)
				{
					inContext.inBuffSkill.get_handle().CustomParams[3] += skillFuncProtectValue;
					SkillFuncHurtDelegator.SendProtectEvent(ref inContext, 3, skillFuncProtectValue);
				}
				inContext.inBuffSkill.get_handle().CustomParams[4] = skillFuncParam2;
				inContext.inBuffSkill.get_handle().CustomParams[5] = skillFuncParam3;
				inContext.inBuffSkill.get_handle().SlotType = inContext.inUseContext.SlotType;
				return true;
			}
			return false;
		}

		[SkillFuncHandler(33, new int[]
		{

		})]
		public static bool OnSkillFuncExtraEffect(ref SSkillFuncContext inContext)
		{
			return true;
		}

		[SkillFuncHandler(10, new int[]
		{

		})]
		public static bool OnSkillFuncSuckBlood(ref SSkillFuncContext inContext)
		{
			PoolObjHandle<ActorRoot> inOriginator = inContext.inOriginator;
			if (!inOriginator)
			{
				return false;
			}
			if (inContext.inStage != ESkillFuncStage.Leave)
			{
				int num = 0;
				if (inContext.inAction.get_handle().refParams.GetRefParam("HurtValue", ref num))
				{
					int nAddHp = num * inContext.inSkillFunc.astSkillFuncParam[0].iParam / 10000;
					inOriginator.get_handle().ActorControl.ReviveHp(nAddHp);
				}
			}
			return true;
		}

		[SkillFuncHandler(44, new int[]
		{

		})]
		public static bool OnSkillFuncConditionHurtOut(ref SSkillFuncContext inContext)
		{
			return true;
		}

		[SkillFuncHandler(48, new int[]
		{

		})]
		public static bool OnSkillFuncTargetExtraHurt(ref SSkillFuncContext inContext)
		{
			return true;
		}

		[SkillFuncHandler(49, new int[]
		{

		})]
		public static bool OnSkillFuncTargetExtraExp(ref SSkillFuncContext inContext)
		{
			return true;
		}

		[SkillFuncHandler(50, new int[]
		{

		})]
		public static bool OnSkillFuncAddExp(ref SSkillFuncContext inContext)
		{
			if (inContext.inStage != ESkillFuncStage.Leave)
			{
				PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
				if (inTargetObj)
				{
					int skillFuncParam = inContext.GetSkillFuncParam(0, false);
					inTargetObj.get_handle().ValueComponent.AddSoulExp(skillFuncParam, true, AddSoulType.SkillFunc);
				}
			}
			return true;
		}

		[SkillFuncHandler(52, new int[]
		{

		})]
		public static bool OnSkillFuncImmuneCrit(ref SSkillFuncContext inContext)
		{
			PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
			if (inTargetObj)
			{
				if (inContext.inStage == ESkillFuncStage.Enter)
				{
					inTargetObj.get_handle().ActorControl.AddNoAbilityFlag(ObjAbilityType.ObjAbility_ImmuneCrit);
				}
				else if (inContext.inStage == ESkillFuncStage.Leave)
				{
					inTargetObj.get_handle().ActorControl.RmvNoAbilityFlag(ObjAbilityType.ObjAbility_ImmuneCrit);
				}
			}
			return true;
		}

		[SkillFuncHandler(53, new int[]
		{

		})]
		public static bool OnSkillFuncLimiteMaxHurt(ref SSkillFuncContext inContext)
		{
			PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
			if (inTargetObj)
			{
				if (inContext.inStage == ESkillFuncStage.Enter)
				{
					int skillFuncParam = inContext.GetSkillFuncParam(0, false);
					inTargetObj.get_handle().BuffHolderComp.protectRule.SetLimiteMaxHurt(true, skillFuncParam);
				}
				else if (inContext.inStage == ESkillFuncStage.Leave)
				{
					inTargetObj.get_handle().BuffHolderComp.protectRule.SetLimiteMaxHurt(false, 0);
				}
			}
			return true;
		}

		[SkillFuncHandler(61, new int[]
		{

		})]
		public static bool OnSkillFuncCommonAtkWithMagicHurt(ref SSkillFuncContext inContext)
		{
			return true;
		}

		[SkillFuncHandler(67, new int[]
		{

		})]
		public static bool OnSkillFuncDecHurtRate(ref SSkillFuncContext inContext)
		{
			return true;
		}

		[SkillFuncHandler(68, new int[]
		{

		})]
		public static bool OnSkillFuncExtraHurtWithLowHp(ref SSkillFuncContext inContext)
		{
			return true;
		}

		[SkillFuncHandler(69, new int[]
		{

		})]
		public static bool OnSkillFuncBlindess(ref SSkillFuncContext inContext)
		{
			PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
			if (inTargetObj)
			{
				if (inContext.inStage == ESkillFuncStage.Enter)
				{
					inTargetObj.get_handle().ActorControl.AddNoAbilityFlag(ObjAbilityType.ObjAbility_Blindness);
				}
				else if (inContext.inStage == ESkillFuncStage.Leave)
				{
					inTargetObj.get_handle().ActorControl.RmvNoAbilityFlag(ObjAbilityType.ObjAbility_Blindness);
				}
			}
			return true;
		}

		[SkillFuncHandler(71, new int[]
		{

		})]
		public static bool OnSkillFuncTargetExtraCoin(ref SSkillFuncContext inContext)
		{
			return true;
		}

		[SkillFuncHandler(80, new int[]
		{

		})]
		public static bool OnSkillFuncBlockPhysHurt(ref SSkillFuncContext inContext)
		{
			return true;
		}

		[SkillFuncHandler(82, new int[]
		{

		})]
		public static bool OnSkillFuncSuckBloodSpecialSkill(ref SSkillFuncContext inContext)
		{
			PoolObjHandle<ActorRoot> inOriginator = inContext.inOriginator;
			if (!inOriginator)
			{
				return false;
			}
			if (inContext.inStage != ESkillFuncStage.Leave)
			{
				int num = 0;
				if ((inContext.inSkillFunc.astSkillFuncParam[1].iParam & 1 << (int)inContext.inUseContext.SlotType) == 0)
				{
					return false;
				}
				if (inContext.inAction.get_handle().refParams.GetRefParam("HurtValue", ref num))
				{
					int nAddHp = num * inContext.inSkillFunc.astSkillFuncParam[0].iParam / 10000;
					inOriginator.get_handle().ActorControl.ReviveHp(nAddHp);
				}
			}
			return true;
		}

		[SkillFuncHandler(83, new int[]
		{

		})]
		public static bool OnSkillFuncBounceHurt(ref SSkillFuncContext inContext)
		{
			return true;
		}
	}
}
