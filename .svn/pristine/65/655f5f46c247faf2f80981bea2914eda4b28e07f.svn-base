using Assets.Scripts.Common;
using CSProtocol;
using Pathfinding.RVO;
using ResData;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameLogic.SkillFunc
{
	[SkillFuncHandlerClass]
	internal class SkillFuncSpecialDelegator
	{
		[SkillFuncHandler(38, new int[]
		{

		})]
		public static bool OnSkillFuncSightArea(ref SSkillFuncContext inContext)
		{
			if (inContext.inStage == ESkillFuncStage.Enter)
			{
				PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
				if (inTargetObj)
				{
					List<PoolObjHandle<ActorRoot>> heroActors = Singleton<GameObjMgr>.GetInstance().HeroActors;
					for (int i = 0; i < heroActors.get_Count(); i++)
					{
						ActorRoot handle = heroActors.get_Item(i).get_handle();
						if (inTargetObj.get_handle().TheActorMeta.ActorCamp != handle.TheActorMeta.ActorCamp)
						{
							handle.HorizonMarker.SetEnabled(false);
						}
					}
				}
			}
			else if (inContext.inStage == ESkillFuncStage.Leave)
			{
				PoolObjHandle<ActorRoot> inTargetObj2 = inContext.inTargetObj;
				if (inTargetObj2)
				{
					List<PoolObjHandle<ActorRoot>> heroActors2 = Singleton<GameObjMgr>.GetInstance().HeroActors;
					for (int j = 0; j < heroActors2.get_Count(); j++)
					{
						ActorRoot handle2 = heroActors2.get_Item(j).get_handle();
						if (inTargetObj2.get_handle().TheActorMeta.ActorCamp != handle2.TheActorMeta.ActorCamp)
						{
							handle2.HorizonMarker.SetEnabled(true);
						}
					}
				}
			}
			return true;
		}

		[SkillFuncHandler(32, new int[]
		{

		})]
		public static bool OnSkillFuncReviveSoon(ref SSkillFuncContext inContext)
		{
			return true;
		}

		private static void SkillFuncChangeSkillCDImpl(ref SSkillFuncContext inContext, int changeType, int slotMask, int value)
		{
			PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
			if (inTargetObj)
			{
				SkillComponent skillControl = inTargetObj.get_handle().SkillControl;
				if (skillControl != null)
				{
					SkillSlot skillSlot = null;
					for (int i = 0; i < 10; i++)
					{
						if (((slotMask == 0 && i != 0 && i != 4 && i != 5 && i != 7) || (slotMask & 1 << i) > 0) && skillControl.TryGetSkillSlot((SkillSlotType)i, out skillSlot) && skillSlot != null)
						{
							if (changeType == 0)
							{
								skillSlot.ChangeSkillCD(value);
							}
							else
							{
								skillSlot.ChangeMaxCDRate(value);
							}
						}
					}
				}
			}
		}

		[SkillFuncHandler(16, new int[]
		{

		})]
		public static bool OnSkillFuncChangeSkillCD(ref SSkillFuncContext inContext)
		{
			if (inContext.inStage == ESkillFuncStage.Enter)
			{
				int skillFuncParam = inContext.GetSkillFuncParam(0, false);
				int skillFuncParam2 = inContext.GetSkillFuncParam(1, false);
				int skillFuncParam3 = inContext.GetSkillFuncParam(2, false);
				inContext.LocalParams[0].iParam = skillFuncParam;
				inContext.LocalParams[1].iParam = skillFuncParam2;
				inContext.LocalParams[2].iParam = skillFuncParam3;
				SkillFuncSpecialDelegator.SkillFuncChangeSkillCDImpl(ref inContext, skillFuncParam, skillFuncParam2, skillFuncParam3);
			}
			else if (inContext.inStage == ESkillFuncStage.Leave)
			{
				int iParam = inContext.LocalParams[0].iParam;
				int iParam2 = inContext.LocalParams[1].iParam;
				int value = -inContext.LocalParams[2].iParam;
				if (iParam != 0)
				{
					SkillFuncSpecialDelegator.SkillFuncChangeSkillCDImpl(ref inContext, iParam, iParam2, value);
				}
			}
			return true;
		}

		[SkillFuncHandler(39, new int[]
		{

		})]
		public static bool OnSkillFuncInvisible(ref SSkillFuncContext inContext)
		{
			PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
			if (inTargetObj)
			{
				if (inContext.inStage == ESkillFuncStage.Enter)
				{
					inTargetObj.get_handle().HorizonMarker.AddHideMark(3, HorizonConfig.HideMark.Skill, 1, false);
					inTargetObj.get_handle().HorizonMarker.SetTranslucentMark(HorizonConfig.HideMark.Skill, true, false);
				}
				else if (inContext.inStage == ESkillFuncStage.Leave)
				{
					COM_PLAYERCAMP[] othersCmp = BattleLogic.GetOthersCmp(inTargetObj.get_handle().TheActorMeta.ActorCamp);
					for (int i = 0; i < othersCmp.Length; i++)
					{
						if (inTargetObj.get_handle().HorizonMarker.HasHideMark(othersCmp[i], HorizonConfig.HideMark.Skill))
						{
							inTargetObj.get_handle().HorizonMarker.AddHideMark(othersCmp[i], HorizonConfig.HideMark.Skill, -1, false);
						}
					}
					inTargetObj.get_handle().HorizonMarker.SetTranslucentMark(HorizonConfig.HideMark.Skill, false, false);
				}
			}
			return true;
		}

		[SkillFuncHandler(75, new int[]
		{

		})]
		public static bool OnSkillFuncShowMark(ref SSkillFuncContext inContext)
		{
			PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
			int skillFuncParam = inContext.GetSkillFuncParam(0, false);
			if (inTargetObj)
			{
				if (inContext.inStage == ESkillFuncStage.Enter)
				{
					if (skillFuncParam == 1)
					{
						inTargetObj.get_handle().HorizonMarker.AddShowMark(inContext.inOriginator.get_handle().TheActorMeta.ActorCamp, HorizonConfig.ShowMark.Skill, 1);
					}
					else
					{
						inTargetObj.get_handle().HorizonMarker.AddShowMark(3, HorizonConfig.ShowMark.Skill, 1);
					}
				}
				else if (inContext.inStage == ESkillFuncStage.Leave)
				{
					if (skillFuncParam == 1)
					{
						if (inTargetObj.get_handle().HorizonMarker.HasShowMark(inContext.inOriginator.get_handle().TheActorMeta.ActorCamp, HorizonConfig.ShowMark.Skill))
						{
							inTargetObj.get_handle().HorizonMarker.AddShowMark(inContext.inOriginator.get_handle().TheActorMeta.ActorCamp, HorizonConfig.ShowMark.Skill, -1);
						}
					}
					else
					{
						COM_PLAYERCAMP[] othersCmp = BattleLogic.GetOthersCmp(inTargetObj.get_handle().TheActorMeta.ActorCamp);
						for (int i = 0; i < othersCmp.Length; i++)
						{
							if (inTargetObj.get_handle().HorizonMarker.HasShowMark(othersCmp[i], HorizonConfig.ShowMark.Skill))
							{
								inTargetObj.get_handle().HorizonMarker.AddShowMark(othersCmp[i], HorizonConfig.ShowMark.Skill, -1);
							}
						}
					}
				}
			}
			return true;
		}

		[SkillFuncHandler(45, new int[]
		{

		})]
		public static bool OnSkillFuncIgnoreRVO(ref SSkillFuncContext inContext)
		{
			PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
			if (inTargetObj)
			{
				if (inContext.inStage == ESkillFuncStage.Enter)
				{
					RVOController component = inTargetObj.get_handle().gameObject.GetComponent<RVOController>();
					if (component != null)
					{
						component.enabled = false;
					}
				}
				else if (inContext.inStage == ESkillFuncStage.Leave)
				{
					RVOController component = inTargetObj.get_handle().gameObject.GetComponent<RVOController>();
					if (component != null)
					{
						component.enabled = true;
					}
				}
			}
			return true;
		}

		[SkillFuncHandler(46, new int[]
		{

		})]
		public static bool OnSkillFuncHpCondition(ref SSkillFuncContext inContext)
		{
			int skillFuncParam = inContext.GetSkillFuncParam(1, false);
			if (skillFuncParam < 0 || skillFuncParam >= 37)
			{
				return false;
			}
			RES_FUNCEFT_TYPE key = skillFuncParam;
			PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
			if (!inTargetObj)
			{
				return false;
			}
			int skillFuncParam2 = inContext.GetSkillFuncParam(2, false);
			if (inContext.inStage != ESkillFuncStage.Leave)
			{
				int actorHp = inTargetObj.get_handle().ValueComponent.actorHp;
				int totalValue = inTargetObj.get_handle().ValueComponent.mActorValue[5].totalValue;
				int num = 10000 - actorHp * 10000 / totalValue;
				int skillFuncParam3 = inContext.GetSkillFuncParam(3, true);
				int skillFuncParam4 = inContext.GetSkillFuncParam(0, false);
				if (skillFuncParam4 == 0)
				{
					return false;
				}
				int num2 = num * skillFuncParam3 / skillFuncParam4;
				int iParam = inContext.LocalParams[0].iParam;
				if (skillFuncParam2 == 1)
				{
					ValueDataInfo valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[key] >> iParam;
					valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[key] << num2;
				}
				else
				{
					ValueDataInfo valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[key] - iParam;
					valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[key] + num2;
				}
				inContext.LocalParams[0].iParam = num2;
			}
			else if (inContext.inStage == ESkillFuncStage.Leave)
			{
				int iParam2 = inContext.LocalParams[0].iParam;
				if (skillFuncParam2 == 1)
				{
					ValueDataInfo valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[key] >> iParam2;
				}
				else
				{
					ValueDataInfo valueDataInfo = inTargetObj.get_handle().ValueComponent.mActorValue[key] - iParam2;
				}
			}
			return true;
		}

		[SkillFuncHandler(47, new int[]
		{

		})]
		public static bool OnSkillFuncChangeHudStyle(ref SSkillFuncContext inContext)
		{
			PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
			if (inTargetObj)
			{
				if (inContext.inStage == ESkillFuncStage.Enter)
				{
					inTargetObj.get_handle().HudControl.bBossHpBar = true;
				}
				else if (inContext.inStage == ESkillFuncStage.Leave)
				{
					inTargetObj.get_handle().HudControl.bBossHpBar = false;
				}
			}
			return true;
		}

		[SkillFuncHandler(55, new int[]
		{

		})]
		public static bool OnSkillFuncChangeSkill(ref SSkillFuncContext inContext)
		{
			PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
			if (inTargetObj)
			{
				if (inContext.inStage == ESkillFuncStage.Enter)
				{
					int num = inContext.GetSkillFuncParam(0, false);
					int skillFuncParam = inContext.GetSkillFuncParam(1, false);
					int skillFuncParam2 = inContext.GetSkillFuncParam(2, false);
					bool flag = inContext.GetSkillFuncParam(3, false) == 1;
					inContext.LocalParams[0].iParam = num;
					if (inTargetObj.get_handle().BuffHolderComp != null)
					{
						BuffChangeSkillRule changeSkillRule = inTargetObj.get_handle().BuffHolderComp.changeSkillRule;
						if (changeSkillRule != null)
						{
							changeSkillRule.ChangeSkillSlot((SkillSlotType)num, skillFuncParam, skillFuncParam2);
						}
					}
					if (flag && num == 0)
					{
						inTargetObj.get_handle().SkillControl.bImmediateAttack = flag;
					}
				}
				else if (inContext.inStage == ESkillFuncStage.Leave)
				{
					int num = inContext.LocalParams[0].iParam;
					bool flag = inContext.GetSkillFuncParam(3, false) == 1;
					if (inTargetObj.get_handle().BuffHolderComp != null)
					{
						BuffChangeSkillRule changeSkillRule = inTargetObj.get_handle().BuffHolderComp.changeSkillRule;
						if (changeSkillRule != null)
						{
							changeSkillRule.RecoverSkillSlot((SkillSlotType)num);
						}
						if (flag && num == 0)
						{
							inTargetObj.get_handle().SkillControl.bImmediateAttack = false;
							inTargetObj.get_handle().ActorControl.CancelCommonAttackMode();
						}
					}
				}
			}
			return true;
		}

		[SkillFuncHandler(56, new int[]
		{

		})]
		public static bool OnSkillFuncDisableSkill(ref SSkillFuncContext inContext)
		{
			PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
			if (inTargetObj)
			{
				if (inContext.inStage == ESkillFuncStage.Enter)
				{
					for (int i = 0; i < 8; i++)
					{
						int skillFuncParam = inContext.GetSkillFuncParam(i, false);
						if (skillFuncParam == 1)
						{
							inTargetObj.get_handle().ActorControl.AddDisableSkillFlag((SkillSlotType)i, false);
						}
					}
				}
				else if (inContext.inStage == ESkillFuncStage.Leave)
				{
					for (int j = 0; j < 8; j++)
					{
						int skillFuncParam = inContext.GetSkillFuncParam(j, false);
						if (skillFuncParam == 1)
						{
							inTargetObj.get_handle().ActorControl.RmvDisableSkillFlag((SkillSlotType)j, false);
						}
					}
				}
			}
			return true;
		}

		[SkillFuncHandler(70, new int[]
		{

		})]
		public static bool OnSkillFuncRemoveSkillBuff(ref SSkillFuncContext inContext)
		{
			if (inContext.inStage == ESkillFuncStage.Enter)
			{
				PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
				if (inTargetObj)
				{
					int skillFuncParam = inContext.GetSkillFuncParam(0, false);
					inTargetObj.get_handle().BuffHolderComp.RemoveBuff(skillFuncParam);
				}
			}
			return true;
		}

		[SkillFuncHandler(72, new int[]
		{

		})]
		public static bool OnSkillFuncSkillExtraHurt(ref SSkillFuncContext inContext)
		{
			return true;
		}

		[SkillFuncHandler(73, new int[]
		{

		})]
		public static bool OnSkillFuncSkillChangeParam(ref SSkillFuncContext inContext)
		{
			PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
			if (inTargetObj)
			{
				if (inContext.inStage == ESkillFuncStage.Enter)
				{
					int skillFuncParam = inContext.GetSkillFuncParam(0, false);
					int skillFuncParam2 = inContext.GetSkillFuncParam(1, false);
					int skillFuncParam3 = inContext.GetSkillFuncParam(2, false);
					inTargetObj.get_handle().SkillControl.ChangePassiveParam(skillFuncParam, skillFuncParam2, skillFuncParam3);
				}
				else if (inContext.inStage == ESkillFuncStage.Leave)
				{
					int skillFuncParam4 = inContext.GetSkillFuncParam(0, false);
					int skillFuncParam5 = inContext.GetSkillFuncParam(1, false);
					int skillFuncParam6 = inContext.GetSkillFuncParam(3, false);
					inTargetObj.get_handle().SkillControl.ChangePassiveParam(skillFuncParam4, skillFuncParam5, skillFuncParam6);
				}
			}
			return true;
		}

		[SkillFuncHandler(84, new int[]
		{

		})]
		public static bool OnSkillFuncBounceSkillEffect(ref SSkillFuncContext inContext)
		{
			return true;
		}

		[SkillFuncHandler(85, new int[]
		{

		})]
		public static bool OnSkillFuncDecreaseReviveTime(ref SSkillFuncContext inContext)
		{
			PoolObjHandle<ActorRoot> inTargetObj = inContext.inTargetObj;
			if (inTargetObj && inContext.inStage == ESkillFuncStage.Enter)
			{
				int skillFuncParam = inContext.GetSkillFuncParam(0, false);
				int skillFuncParam2 = inContext.GetSkillFuncParam(1, false);
				if (skillFuncParam == 0)
				{
					inTargetObj.get_handle().ActorControl.ReviveCooldown -= skillFuncParam2;
				}
				else if (skillFuncParam == 1)
				{
					inTargetObj.get_handle().ActorControl.ReviveCooldown -= (int)((long)(skillFuncParam2 * inTargetObj.get_handle().ActorControl.CfgReviveCD) / 10000L);
				}
			}
			return true;
		}
	}
}
