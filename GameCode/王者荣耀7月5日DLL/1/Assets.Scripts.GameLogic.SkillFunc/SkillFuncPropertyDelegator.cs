using Assets.Scripts.Common;
using ResData;
using System;

namespace Assets.Scripts.GameLogic.SkillFunc
{
	[SkillFuncHandlerClass]
	internal class SkillFuncPropertyDelegator
	{
		private static int GetChangeValueProperty(ActorRoot targetActor, ref SSkillFuncContext inContext)
		{
			int num = inContext.GetSkillFuncParam(1, true);
			int skillFuncParam = inContext.GetSkillFuncParam(2, true);
			int skillFuncParam2 = inContext.GetSkillFuncParam(3, true);
			ESkillFuncMode skillFuncParam3 = (ESkillFuncMode)inContext.GetSkillFuncParam(4, false);
			int skillFuncParam4 = inContext.GetSkillFuncParam(5, true);
			num += skillFuncParam * targetActor.ValueComponent.mActorValue[1].totalValue / 10000 + skillFuncParam2 * targetActor.ValueComponent.mActorValue[2].totalValue / 10000;
			if (skillFuncParam3 == ESkillFuncMode.SkillFuncMode_Constant)
			{
				return num;
			}
			int num2 = (int)inContext.inSkillFunc.dwSkillFuncFreq;
			if (skillFuncParam4 <= 0)
			{
				DebugHelper.Assert(false, "ESkillFuncMode LastTime error!");
				return num;
			}
			if (num2 <= 0)
			{
				num2 = 30;
			}
			int num3 = skillFuncParam4 / num2;
			if (num3 <= 0)
			{
				return num;
			}
			if (skillFuncParam3 == ESkillFuncMode.SkillFuncMode_Fade)
			{
				if (inContext.inDoCount == 1)
				{
					return num;
				}
				if (inContext.inDoCount - 1 <= num3)
				{
					return -num / num3;
				}
				return 0;
			}
			else
			{
				if (inContext.inDoCount == 1)
				{
					return 0;
				}
				if (inContext.inDoCount - 1 <= num3)
				{
					return num / num3;
				}
				return 0;
			}
		}

		private static void OnSKillFuncChangeEpValue(ref SSkillFuncContext inContext, bool _bAddValue)
		{
			PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
			if (!inTargetObj)
			{
				return;
			}
			int changeValueProperty = SkillFuncPropertyDelegator.GetChangeValueProperty(inTargetObj, ref inContext);
			int skillFuncParam = inContext.GetSkillFuncParam(0, false);
			if (inContext.inStage != ESkillFuncStage.Leave)
			{
				if (_bAddValue)
				{
					inTargetObj.get_handle().ValueComponent.ChangeActorEp(changeValueProperty, skillFuncParam);
				}
				else
				{
					inTargetObj.get_handle().ValueComponent.ChangeActorEp(-changeValueProperty, skillFuncParam);
				}
			}
		}

		private static void OnSkillFuncChangeValueProperty(ref SSkillFuncContext inContext, RES_FUNCEFT_TYPE _defType, bool _bAddValue)
		{
			PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
			if (!inTargetObj)
			{
				return;
			}
			int changeValueProperty = SkillFuncPropertyDelegator.GetChangeValueProperty(inTargetObj, ref inContext);
			int skillFuncParam = inContext.GetSkillFuncParam(0, false);
			if (inContext.inStage != ESkillFuncStage.Leave)
			{
				if (_bAddValue)
				{
					if (skillFuncParam == 0)
					{
						ValueDataInfo valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[_defType] + changeValueProperty;
					}
					else
					{
						ValueDataInfo valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[_defType] << changeValueProperty;
					}
					SSkillFuncIntParam[] expr_8E_cp_0 = inContext.LocalParams;
					int expr_8E_cp_1 = 0;
					expr_8E_cp_0[expr_8E_cp_1].iParam = expr_8E_cp_0[expr_8E_cp_1].iParam + changeValueProperty;
				}
				else
				{
					if (skillFuncParam == 0)
					{
						ValueDataInfo valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[_defType] - changeValueProperty;
					}
					else
					{
						ValueDataInfo valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[_defType] >> changeValueProperty;
					}
					SSkillFuncIntParam[] expr_F3_cp_0 = inContext.LocalParams;
					int expr_F3_cp_1 = 0;
					expr_F3_cp_0[expr_F3_cp_1].iParam = expr_F3_cp_0[expr_F3_cp_1].iParam + changeValueProperty;
				}
			}
			else if (_bAddValue)
			{
				if (skillFuncParam == 0)
				{
					ValueDataInfo valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[_defType] - inContext.LocalParams[0].iParam;
				}
				else
				{
					ValueDataInfo valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[_defType] >> inContext.LocalParams[0].iParam;
				}
			}
			else if (skillFuncParam == 0)
			{
				ValueDataInfo valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[_defType] + inContext.LocalParams[0].iParam;
			}
			else
			{
				ValueDataInfo valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[_defType] << inContext.LocalParams[0].iParam;
			}
		}

		[SkillFuncHandler(6, new int[]
		{
			7
		})]
		public static bool OnSkillFuncChangeMoveSpd(ref SSkillFuncContext inContext)
		{
			if (inContext.inSkillFunc.bSkillFuncType == 6)
			{
				SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 15, true);
			}
			else
			{
				SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 15, false);
			}
			return true;
		}

		[SkillFuncHandler(8, new int[]
		{
			9
		})]
		public static bool OnSkillFuncChangeAtk(ref SSkillFuncContext inContext)
		{
			if (inContext.inSkillFunc.bSkillFuncType == 8)
			{
				SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 1, true);
			}
			else
			{
				SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 1, false);
			}
			return true;
		}

		[SkillFuncHandler(4, new int[]
		{
			5
		})]
		public static bool OnSkillFuncChangeAtkSpd(ref SSkillFuncContext inContext)
		{
			if (inContext.inSkillFunc.bSkillFuncType == 4)
			{
				SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 18, true);
			}
			else
			{
				SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 18, false);
			}
			return true;
		}

		[SkillFuncHandler(11, new int[]
		{
			12
		})]
		public static bool OnSkillFuncChangeDefend(ref SSkillFuncContext inContext)
		{
			if (inContext.inSkillFunc.bSkillFuncType == 11)
			{
				SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 3, true);
			}
			else
			{
				SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 3, false);
			}
			return true;
		}

		[SkillFuncHandler(13, new int[]
		{
			14
		})]
		public static bool OnSkillFuncChangeResist(ref SSkillFuncContext inContext)
		{
			if (inContext.inSkillFunc.bSkillFuncType == 13)
			{
				SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 4, true);
			}
			else
			{
				SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 4, false);
			}
			return true;
		}

		[SkillFuncHandler(17, new int[]
		{
			18
		})]
		public static bool OnSkillFuncChangeAp(ref SSkillFuncContext inContext)
		{
			if (inContext.inSkillFunc.bSkillFuncType == 17)
			{
				SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 2, true);
			}
			else
			{
				SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 2, false);
			}
			return true;
		}

		[SkillFuncHandler(21, new int[]
		{
			22
		})]
		public static bool OnSkillFuncChangeMaxHp(ref SSkillFuncContext inContext)
		{
			VFactor hpByRate = VFactor.one;
			if (inContext.inTargetObj && inContext.inTargetObj.get_handle().ValueComponent != null)
			{
				hpByRate = inContext.inTargetObj.get_handle().ValueComponent.GetHpRate();
			}
			if (inContext.inSkillFunc.bSkillFuncType == 21)
			{
				SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 5, true);
			}
			else
			{
				SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 5, false);
			}
			if (inContext.inTargetObj && inContext.inTargetObj.get_handle().ValueComponent != null)
			{
				inContext.inTargetObj.get_handle().ValueComponent.SetHpByRate(hpByRate);
			}
			return true;
		}

		[SkillFuncHandler(19, new int[]
		{
			20
		})]
		public static bool OnSkillFuncChangeCritStrikeRate(ref SSkillFuncContext inContext)
		{
			if (inContext.inSkillFunc.bSkillFuncType == 19)
			{
				SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 6, true);
			}
			else
			{
				SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 6, false);
			}
			return true;
		}

		[SkillFuncHandler(23, new int[]
		{
			24
		})]
		public static bool OnSkillFuncChangeDefStrike(ref SSkillFuncContext inContext)
		{
			if (inContext.inSkillFunc.bSkillFuncType == 23)
			{
				SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 7, true);
			}
			else
			{
				SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 7, false);
			}
			return true;
		}

		[SkillFuncHandler(25, new int[]
		{
			26
		})]
		public static bool OnSkillFuncChangeRessStrike(ref SSkillFuncContext inContext)
		{
			if (inContext.inSkillFunc.bSkillFuncType == 25)
			{
				SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 8, true);
			}
			else
			{
				SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 8, false);
			}
			return true;
		}

		[SkillFuncHandler(35, new int[]
		{

		})]
		public static bool OnSkillFuncPhysHemo(ref SSkillFuncContext inContext)
		{
			SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 9, true);
			return true;
		}

		[SkillFuncHandler(36, new int[]
		{

		})]
		public static bool OnSkillFuncMagicHemo(ref SSkillFuncContext inContext)
		{
			SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 10, true);
			return true;
		}

		[SkillFuncHandler(37, new int[]
		{

		})]
		public static bool OnSkillFuncHurtReduceRate(ref SSkillFuncContext inContext)
		{
			SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 30, true);
			return true;
		}

		[SkillFuncHandler(40, new int[]
		{

		})]
		public static bool OnSkillFuncHurtOutputRate(ref SSkillFuncContext inContext)
		{
			SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 31, true);
			return true;
		}

		[SkillFuncHandler(41, new int[]
		{

		})]
		public static bool OnSkillFuncReduceControl(ref SSkillFuncContext inContext)
		{
			SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 17, true);
			return true;
		}

		[SkillFuncHandler(42, new int[]
		{

		})]
		public static bool OnSkillFuncReduceCD(ref SSkillFuncContext inContext)
		{
			SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 20, true);
			return true;
		}

		[SkillFuncHandler(43, new int[]
		{

		})]
		public static bool OnSkillFuncAnticrit(ref SSkillFuncContext inContext)
		{
			SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 11, true);
			return true;
		}

		[SkillFuncHandler(57, new int[]
		{

		})]
		public static bool OnSkillFuncCritEffect(ref SSkillFuncContext inContext)
		{
			SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 12, true);
			return true;
		}

		[SkillFuncHandler(58, new int[]
		{

		})]
		public static bool OnSkillFuncAddEp(ref SSkillFuncContext inContext)
		{
			if (inContext.inTargetObj && inContext.inTargetObj.get_handle().ValueComponent.IsEnergyType(EnergyType.MagicResource))
			{
				SkillFuncPropertyDelegator.OnSKillFuncChangeEpValue(ref inContext, true);
				return true;
			}
			return false;
		}

		[SkillFuncHandler(74, new int[]
		{

		})]
		public static bool OnSkillFuncChangeHeroMadnessEp(ref SSkillFuncContext inContext)
		{
			if (inContext.inTargetObj && inContext.inTargetObj.get_handle().ValueComponent.IsEnergyType(EnergyType.MadnessResource))
			{
				SkillFuncPropertyDelegator.OnSKillFuncChangeEpValue(ref inContext, true);
				return true;
			}
			return false;
		}

		[SkillFuncHandler(76, new int[]
		{

		})]
		public static bool OnSkillFuncChangeHeroEnergyEp(ref SSkillFuncContext inContext)
		{
			if (inContext.inTargetObj && inContext.inTargetObj.get_handle().ValueComponent.IsEnergyType(EnergyType.EnergyResource))
			{
				SkillFuncPropertyDelegator.OnSKillFuncChangeEpValue(ref inContext, true);
				return true;
			}
			return false;
		}

		[SkillFuncHandler(77, new int[]
		{

		})]
		public static bool OnSkillFuncChangeHeroFuryEp(ref SSkillFuncContext inContext)
		{
			if (inContext.inTargetObj && inContext.inTargetObj.get_handle().ValueComponent.IsEnergyType(EnergyType.FuryResource))
			{
				SkillFuncPropertyDelegator.OnSKillFuncChangeEpValue(ref inContext, true);
				return true;
			}
			return false;
		}

		[SkillFuncHandler(59, new int[]
		{

		})]
		public static bool OnSkillFuncChangePhyArmorHurtRate(ref SSkillFuncContext inContext)
		{
			SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 34, true);
			return true;
		}

		[SkillFuncHandler(60, new int[]
		{

		})]
		public static bool OnSkillFuncChangeMgcArmorHurtRate(ref SSkillFuncContext inContext)
		{
			SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 35, true);
			return true;
		}

		[SkillFuncHandler(62, new int[]
		{

		})]
		public static bool OnSkillFuncChangeMgcRate(ref SSkillFuncContext inContext)
		{
			if (inContext.inStage == ESkillFuncStage.Enter)
			{
				int skillFuncParam = inContext.GetSkillFuncParam(3, false);
				int num = inContext.inTargetObj.get_handle().ValueComponent.mActorValue[2].totalEftRatio += skillFuncParam;
			}
			else if (inContext.inStage == ESkillFuncStage.Leave)
			{
				int skillFuncParam2 = inContext.GetSkillFuncParam(3, false);
				int num = inContext.inTargetObj.get_handle().ValueComponent.mActorValue[2].totalEftRatio -= skillFuncParam2;
			}
			return true;
		}

		[SkillFuncHandler(63, new int[]
		{

		})]
		public static bool OnSkillFuncChangeMgcEffect(ref SSkillFuncContext inContext)
		{
			if (inContext.inStage == ESkillFuncStage.Enter)
			{
				int skillFuncParam = inContext.GetSkillFuncParam(0, false);
				int skillFuncParam2 = inContext.GetSkillFuncParam(2, false);
				int skillFuncParam3 = inContext.GetSkillFuncParam(4, false);
				inContext.inTargetObj.get_handle().ValueComponent.mActorValue[1].totalEftRatioByMgc += skillFuncParam2;
				inContext.inTargetObj.get_handle().ValueComponent.mActorValue[5].totalEftRatioByMgc += skillFuncParam3;
				if (inContext.inTargetObj && inContext.inTargetObj.get_handle().ValueComponent != null)
				{
					ValueProperty valueComponent = inContext.inTargetObj.get_handle().ValueComponent;
					valueComponent.OnValuePropertyChangeByMgcEffect();
					inContext.inTargetObj.get_handle().ValueComponent.mActorValue[2].ChangeEvent -= new ValueChangeDelegate(valueComponent.OnValuePropertyChangeByMgcEffect);
					inContext.inTargetObj.get_handle().ValueComponent.mActorValue[2].ChangeEvent += new ValueChangeDelegate(valueComponent.OnValuePropertyChangeByMgcEffect);
				}
			}
			else if (inContext.inStage == ESkillFuncStage.Leave)
			{
				int skillFuncParam4 = inContext.GetSkillFuncParam(2, false);
				int skillFuncParam5 = inContext.GetSkillFuncParam(4, false);
				inContext.inTargetObj.get_handle().ValueComponent.mActorValue[1].totalEftRatioByMgc -= skillFuncParam4;
				inContext.inTargetObj.get_handle().ValueComponent.mActorValue[5].totalEftRatioByMgc -= skillFuncParam5;
				if (inContext.inTargetObj && inContext.inTargetObj.get_handle().ValueComponent != null)
				{
					ValueProperty valueComponent2 = inContext.inTargetObj.get_handle().ValueComponent;
					inContext.inTargetObj.get_handle().ValueComponent.mActorValue[2].ChangeEvent -= new ValueChangeDelegate(valueComponent2.OnValuePropertyChangeByMgcEffect);
				}
			}
			return true;
		}

		[SkillFuncHandler(65, new int[]
		{

		})]
		public static bool OnSkillFuncChangeMoveSpdWhenInOutBattle(ref SSkillFuncContext inContext)
		{
			SkillFuncPropertyDelegator.OnSkillFuncChangeValueProperty(ref inContext, 15, true);
			return true;
		}

		[SkillFuncHandler(64, new int[]
		{

		})]
		public static bool OnSkillFuncRecoveryEffect(ref SSkillFuncContext inContext)
		{
			PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
			if (!inTargetObj)
			{
				return false;
			}
			int skillFuncParam = inContext.GetSkillFuncParam(0, false);
			int skillFuncParam2 = inContext.GetSkillFuncParam(1, false);
			int skillFuncParam3 = inContext.GetSkillFuncParam(4, false);
			if (inContext.inStage == ESkillFuncStage.Enter)
			{
				if (skillFuncParam == 0)
				{
					ValueDataInfo valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[9] + skillFuncParam3;
					valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[10] + skillFuncParam3;
					valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[36] + skillFuncParam2;
				}
				else
				{
					ValueDataInfo valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[9] << skillFuncParam3;
					valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[10] << skillFuncParam3;
					valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[36] << skillFuncParam2;
				}
			}
			else if (inContext.inStage == ESkillFuncStage.Leave)
			{
				if (skillFuncParam == 0)
				{
					ValueDataInfo valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[9] - skillFuncParam3;
					valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[10] - skillFuncParam3;
					valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[36] - skillFuncParam2;
				}
				else
				{
					ValueDataInfo valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[9] >> skillFuncParam3;
					valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[10] >> skillFuncParam3;
					valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[36] >> skillFuncParam2;
				}
			}
			return true;
		}

		[SkillFuncHandler(79, new int[]
		{

		})]
		public static bool OnSkillFuncIncAtkWithDeffend(ref SSkillFuncContext inContext)
		{
			int skillFuncParam = inContext.GetSkillFuncParam(0, false);
			if (inContext.inStage == ESkillFuncStage.Enter)
			{
				inContext.inTargetObj.get_handle().ValueComponent.mActorValue[1].convertRatioByDefence += skillFuncParam;
				inContext.inTargetObj.get_handle().ValueComponent.mActorValue[3].ChangeEvent += new ValueChangeDelegate(inContext.inTargetObj.get_handle().ValueComponent.ChangePhyAtkByPhyDefence);
				inContext.inTargetObj.get_handle().ValueComponent.ChangePhyAtkByPhyDefence();
			}
			else if (inContext.inStage == ESkillFuncStage.Leave)
			{
				inContext.inTargetObj.get_handle().ValueComponent.mActorValue[1].convertRatioByDefence -= skillFuncParam;
				inContext.inTargetObj.get_handle().ValueComponent.mActorValue[3].ChangeEvent -= new ValueChangeDelegate(inContext.inTargetObj.get_handle().ValueComponent.ChangePhyAtkByPhyDefence);
				inContext.inTargetObj.get_handle().ValueComponent.ChangePhyAtkByPhyDefence();
			}
			return true;
		}

		[SkillFuncHandler(81, new int[]
		{

		})]
		public static bool OnSkillFuncChangeVision(ref SSkillFuncContext inContext)
		{
			int skillFuncParam = inContext.GetSkillFuncParam(1, false);
			int skillFuncParam2 = inContext.GetSkillFuncParam(2, false);
			if (inContext.inStage == ESkillFuncStage.Enter)
			{
				inContext.inTargetObj.get_handle().HorizonMarker.SightRadius += skillFuncParam;
			}
			else if (inContext.inStage == ESkillFuncStage.Leave)
			{
				inContext.inTargetObj.get_handle().HorizonMarker.SightRadius -= skillFuncParam;
			}
			return true;
		}
	}
}
