using AGE;
using Assets.Scripts.Common;
using ResData;
using System;

namespace Assets.Scripts.GameLogic
{
	public struct SSkillFuncContext
	{
		public PoolObjHandle<ActorRoot> inTargetObj;

		public PoolObjHandle<ActorRoot> inOriginator;

		public SkillUseContext inUseContext;

		public ResDT_SkillFunc inSkillFunc;

		public ESkillFuncStage inStage;

		public SSkillFuncIntParam[] LocalParams;

		public PoolObjHandle<Action> inAction;

		public PoolObjHandle<BuffSkill> inBuffSkill;

		public HurtAttackerInfo inCustomData;

		public int inDoCount;

		public int inOverlayCount;

		public bool inLastEffect;

		public int inEffectCount;

		public int inEffectCountInSingleTrigger;

		public int inMarkCount;

		public int iSkillLevel
		{
			get
			{
				if (this.inBuffSkill && this.inBuffSkill.get_handle().cfgData != null)
				{
					byte b = this.inBuffSkill.get_handle().cfgData.bGrowthType;
					b %= 10;
					PoolObjHandle<ActorRoot> poolObjHandle = this.inOriginator;
					SkillSlotType slot = this.inUseContext.SlotType;
					if (poolObjHandle && poolObjHandle.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Monster)
					{
						MonsterWrapper monsterWrapper = poolObjHandle.get_handle().ActorControl as MonsterWrapper;
						if (monsterWrapper != null)
						{
							PoolObjHandle<ActorRoot> hostActor = monsterWrapper.hostActor;
							if (hostActor && hostActor.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero && monsterWrapper.spawnSkillSlot != SkillSlotType.SLOT_SKILL_VALID)
							{
								poolObjHandle = hostActor;
								slot = monsterWrapper.spawnSkillSlot;
							}
						}
					}
					if (b == 0)
					{
						if (poolObjHandle && poolObjHandle.get_handle().SkillControl != null)
						{
							SkillSlot skillSlot = poolObjHandle.get_handle().SkillControl.GetSkillSlot(slot);
							if (skillSlot != null)
							{
								return skillSlot.GetSkillLevel();
							}
							if (poolObjHandle.get_handle().ValueComponent != null)
							{
								return poolObjHandle.get_handle().ValueComponent.actorSoulLevel;
							}
						}
					}
					else if (b == 1)
					{
						if (poolObjHandle && poolObjHandle.get_handle().ValueComponent != null)
						{
							return poolObjHandle.get_handle().ValueComponent.actorSoulLevel;
						}
					}
					else if (b > 1 && b <= 4 && poolObjHandle && poolObjHandle.get_handle().SkillControl != null)
					{
						SkillSlot skillSlot2 = poolObjHandle.get_handle().SkillControl.GetSkillSlot((SkillSlotType)(b - 1));
						if (skillSlot2 != null)
						{
							return skillSlot2.GetSkillLevel();
						}
						if (poolObjHandle.get_handle().ValueComponent != null)
						{
							return poolObjHandle.get_handle().ValueComponent.actorSoulLevel;
						}
					}
				}
				return 1;
			}
		}

		public int iSkillFuncInterval
		{
			get
			{
				int num = 1;
				if (this.inBuffSkill && this.inBuffSkill.get_handle().cfgData != null)
				{
					byte bGrowthType = this.inBuffSkill.get_handle().cfgData.bGrowthType;
					num = (int)(bGrowthType / 10);
					if (num == 0)
					{
						num = 1;
					}
				}
				return num;
			}
		}

		public void InitSkillFuncContext()
		{
		}

		public int GetSkillFuncParam(int _index, bool _bGrow)
		{
			if (_index < 0 || _index + 1 > 8)
			{
				DebugHelper.Assert(false, "GetSkillFuncParam: index = {0}", new object[]
				{
					_index
				});
			}
			if (_bGrow)
			{
				int num = this.inSkillFunc.astSkillFuncParam[_index].iParam;
				int iParam = this.inSkillFunc.astSkillFuncGroup[_index].iParam;
				int num2 = iParam * ((this.iSkillLevel - 1) / this.iSkillFuncInterval);
				num += num2;
				if (this.inMarkCount != 0)
				{
					num *= this.inMarkCount;
				}
				else
				{
					num *= this.inOverlayCount;
				}
				return num;
			}
			return this.inSkillFunc.astSkillFuncParam[_index].iParam;
		}
	}
}
