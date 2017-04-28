using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using ResData;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameLogic
{
	public class BuffHolderComponent : LogicComponent
	{
		public List<BuffSkill> SpawnedBuffList = new List<BuffSkill>();

		public BuffClearRule clearRule;

		public BuffOverlayRule overlayRule;

		public BuffProtectRule protectRule;

		public BuffChangeSkillRule changeSkillRule;

		public BufferMarkRule markRule;

		public BufferLogicEffect logicEffect;

		public bool bRemoveList = true;

		private List<BuffSkill> delBuffList = new List<BuffSkill>(3);

		public override void OnUse()
		{
			base.OnUse();
			this.SpawnedBuffList.Clear();
			this.overlayRule = null;
			this.clearRule = null;
			this.protectRule = null;
			this.changeSkillRule = null;
			this.markRule = null;
			this.logicEffect = null;
			this.bRemoveList = true;
			this.delBuffList.Clear();
		}

		public override void Init()
		{
			this.overlayRule = new BuffOverlayRule();
			this.clearRule = new BuffClearRule();
			this.protectRule = new BuffProtectRule();
			this.changeSkillRule = new BuffChangeSkillRule();
			this.markRule = new BufferMarkRule();
			this.logicEffect = new BufferLogicEffect();
			this.overlayRule.Init(this);
			this.clearRule.Init(this);
			this.protectRule.Init(this);
			this.changeSkillRule.Init(this);
			this.markRule.Init(this);
			this.logicEffect.Init(this);
			base.Init();
		}

		public override void Deactive()
		{
			this.ClearBuff();
			base.Deactive();
		}

		public override void Reactive()
		{
			base.Reactive();
			this.overlayRule.Init(this);
			this.clearRule.Init(this);
			this.protectRule.Init(this);
			this.changeSkillRule.Init(this);
			this.markRule.Init(this);
			this.logicEffect.Init(this);
		}

		public override void UpdateLogic(int nDelta)
		{
			if (this.markRule != null)
			{
				this.markRule.UpdateLogic(nDelta);
			}
		}

		public void AddBuff(BuffSkill inBuff)
		{
			this.SpawnedBuffList.Add(inBuff);
			this.protectRule.AddBuff(inBuff);
			this.logicEffect.AddBuff(inBuff);
			BuffChangeEventParam buffChangeEventParam = new BuffChangeEventParam(true, this.actorPtr, inBuff);
			Singleton<GameSkillEventSys>.GetInstance().SendEvent<BuffChangeEventParam>(GameSkillEventDef.AllEvent_BuffChange, this.actorPtr, ref buffChangeEventParam, GameSkillEventChannel.Channel_HostCtrlActor);
			if (inBuff.cfgData != null && inBuff.cfgData.bIsAssistEffect == 1 && inBuff.skillContext.Originator && this.actor.ValueComponent.actorHp > 0)
			{
				if (this.actor.TheActorMeta.ActorCamp == inBuff.skillContext.Originator.get_handle().TheActorMeta.ActorCamp)
				{
					this.actor.ActorControl.AddHelpSelfActor(inBuff.skillContext.Originator);
				}
				else
				{
					this.actor.ActorControl.AddHurtSelfActor(inBuff.skillContext.Originator);
				}
			}
		}

		public void ActionRemoveBuff(BuffSkill inBuff)
		{
			if (this.SpawnedBuffList.Remove(inBuff))
			{
				PoolObjHandle<BuffSkill> poolObjHandle = new PoolObjHandle<BuffSkill>(inBuff);
				this.protectRule.RemoveBuff(ref poolObjHandle);
				this.logicEffect.RemoveBuff(ref poolObjHandle);
				BuffChangeEventParam buffChangeEventParam = new BuffChangeEventParam(false, this.actorPtr, inBuff);
				Singleton<GameSkillEventSys>.GetInstance().SendEvent<BuffChangeEventParam>(GameSkillEventDef.AllEvent_BuffChange, this.actorPtr, ref buffChangeEventParam, GameSkillEventChannel.Channel_AllActor);
				inBuff.Release();
			}
		}

		public void RemoveBuff(BuffSkill inBuff)
		{
			if (this.SpawnedBuffList.get_Count() == 0)
			{
				return;
			}
			this.delBuffList = this.SpawnedBuffList;
			for (int i = 0; i < this.delBuffList.get_Count(); i++)
			{
				BuffSkill buffSkill = this.delBuffList.get_Item(i);
				if (buffSkill == inBuff)
				{
					BuffChangeEventParam buffChangeEventParam = new BuffChangeEventParam(false, this.actorPtr, inBuff);
					Singleton<GameSkillEventSys>.GetInstance().SendEvent<BuffChangeEventParam>(GameSkillEventDef.AllEvent_BuffChange, this.actorPtr, ref buffChangeEventParam, GameSkillEventChannel.Channel_HostCtrlActor);
					buffSkill.Stop();
					if (inBuff.cfgData.bEffectType == 2 && inBuff.cfgData.bShowType != 2 && this.actorPtr)
					{
						LimitMoveEventParam limitMoveEventParam = new LimitMoveEventParam(0, inBuff.SkillID, this.actorPtr);
						Singleton<GameSkillEventSys>.GetInstance().SendEvent<LimitMoveEventParam>(GameSkillEventDef.AllEvent_CancelLimitMove, this.actorPtr, ref limitMoveEventParam, GameSkillEventChannel.Channel_AllActor);
					}
				}
			}
		}

		public void RemoveBuff(int inSkillCombineId)
		{
			if (this.SpawnedBuffList.get_Count() == 0)
			{
				return;
			}
			this.delBuffList = this.SpawnedBuffList;
			for (int i = 0; i < this.delBuffList.get_Count(); i++)
			{
				BuffSkill buffSkill = this.delBuffList.get_Item(i);
				if (buffSkill != null && buffSkill.SkillID == inSkillCombineId)
				{
					buffSkill.Stop();
					if (buffSkill.cfgData.bEffectType == 2 && buffSkill.cfgData.bShowType != 2 && this.actorPtr)
					{
						LimitMoveEventParam limitMoveEventParam = new LimitMoveEventParam(0, buffSkill.SkillID, this.actorPtr);
						Singleton<GameSkillEventSys>.GetInstance().SendEvent<LimitMoveEventParam>(GameSkillEventDef.AllEvent_CancelLimitMove, this.actorPtr, ref limitMoveEventParam, GameSkillEventChannel.Channel_AllActor);
					}
				}
			}
		}

		public void RemoveSkillEffectGroup(int inGroupID)
		{
			if (this.SpawnedBuffList.get_Count() == 0)
			{
				return;
			}
			this.delBuffList = this.SpawnedBuffList;
			for (int i = 0; i < this.delBuffList.get_Count(); i++)
			{
				BuffSkill buffSkill = this.delBuffList.get_Item(i);
				if (buffSkill != null && buffSkill.cfgData != null && buffSkill.cfgData.iCroupID == inGroupID)
				{
					buffSkill.Stop();
					if (buffSkill.cfgData.bEffectType == 2 && buffSkill.cfgData.bShowType != 2 && this.actorPtr)
					{
						LimitMoveEventParam limitMoveEventParam = new LimitMoveEventParam(0, buffSkill.SkillID, this.actorPtr);
						Singleton<GameSkillEventSys>.GetInstance().SendEvent<LimitMoveEventParam>(GameSkillEventDef.AllEvent_CancelLimitMove, this.actorPtr, ref limitMoveEventParam, GameSkillEventChannel.Channel_AllActor);
					}
				}
			}
		}

		public void ClearEffectTypeBuff(int _typeMask)
		{
			if (this.SpawnedBuffList.get_Count() == 0)
			{
				return;
			}
			this.delBuffList = this.SpawnedBuffList;
			for (int i = 0; i < this.delBuffList.get_Count(); i++)
			{
				BuffSkill buffSkill = this.delBuffList.get_Item(i);
				if ((_typeMask & 1 << (int)buffSkill.cfgData.bEffectType) > 0)
				{
					buffSkill.Stop();
				}
			}
			if (this.markRule != null)
			{
				this.markRule.ClearBufferMark(_typeMask);
			}
		}

		public void ClearBuff()
		{
			this.bRemoveList = false;
			for (int i = 0; i < this.SpawnedBuffList.get_Count(); i++)
			{
				BuffSkill buffSkill = this.SpawnedBuffList.get_Item(i);
				if (buffSkill != null)
				{
					buffSkill.Stop();
				}
			}
			if (this.protectRule != null)
			{
				this.protectRule.ClearBuff();
			}
			if (this.logicEffect != null)
			{
				this.logicEffect.ClearBuff();
			}
			for (int j = 0; j < this.SpawnedBuffList.get_Count(); j++)
			{
				BuffSkill buffSkill = this.SpawnedBuffList.get_Item(j);
				if (buffSkill != null)
				{
					buffSkill.Release();
				}
			}
			this.SpawnedBuffList.Clear();
			this.delBuffList.Clear();
			this.bRemoveList = true;
		}

		public BuffSkill FindBuff(int inSkillCombineId)
		{
			if (this.SpawnedBuffList != null)
			{
				for (int i = 0; i < this.SpawnedBuffList.get_Count(); i++)
				{
					BuffSkill buffSkill = this.SpawnedBuffList.get_Item(i);
					if (buffSkill != null && buffSkill.SkillID == inSkillCombineId)
					{
						return buffSkill;
					}
				}
			}
			return null;
		}

		public int FindBuffCount(int inSkillCombineId)
		{
			int num = 0;
			for (int i = 0; i < this.SpawnedBuffList.get_Count(); i++)
			{
				BuffSkill buffSkill = this.SpawnedBuffList.get_Item(i);
				if (buffSkill != null && buffSkill.SkillID == inSkillCombineId)
				{
					num++;
				}
			}
			return num;
		}

		public int GetSoulExpAddRate(PoolObjHandle<ActorRoot> _target)
		{
			int num = 0;
			ResDT_SkillFunc skillFunc = null;
			if (!_target)
			{
				return num;
			}
			for (int i = 0; i < this.SpawnedBuffList.get_Count(); i++)
			{
				BuffSkill buffSkill = this.SpawnedBuffList.get_Item(i);
				if (buffSkill != null && buffSkill.FindSkillFunc(49, out skillFunc))
				{
					int skillFuncParam = buffSkill.GetSkillFuncParam(skillFunc, 0, false);
					int skillFuncParam2 = buffSkill.GetSkillFuncParam(skillFunc, 1, false);
					int skillFuncParam3 = buffSkill.GetSkillFuncParam(skillFunc, 2, false);
					int skillFuncParam4 = buffSkill.GetSkillFuncParam(skillFunc, 3, false);
					if (this.CheckTargetSubType(skillFuncParam, skillFuncParam2, _target))
					{
						bool flag = true;
						if (skillFuncParam4 > 0)
						{
							flag = this.CheckTargetFromEnemy(this.actorPtr, _target);
						}
						if (flag)
						{
							num += skillFuncParam3;
						}
					}
				}
			}
			return num;
		}

		public int GetCoinAddRate(PoolObjHandle<ActorRoot> _target)
		{
			int num = 0;
			ResDT_SkillFunc skillFunc = null;
			if (!_target)
			{
				return num;
			}
			for (int i = 0; i < this.SpawnedBuffList.get_Count(); i++)
			{
				BuffSkill buffSkill = this.SpawnedBuffList.get_Item(i);
				if (buffSkill != null && buffSkill.FindSkillFunc(71, out skillFunc))
				{
					int skillFuncParam = buffSkill.GetSkillFuncParam(skillFunc, 0, false);
					int skillFuncParam2 = buffSkill.GetSkillFuncParam(skillFunc, 1, false);
					int skillFuncParam3 = buffSkill.GetSkillFuncParam(skillFunc, 2, false);
					int skillFuncParam4 = buffSkill.GetSkillFuncParam(skillFunc, 3, false);
					if (this.CheckTargetSubType(skillFuncParam, skillFuncParam2, _target))
					{
						bool flag = true;
						if (skillFuncParam4 > 0)
						{
							flag = this.CheckTargetFromEnemy(this.actorPtr, _target);
						}
						if (flag)
						{
							num += skillFuncParam3;
						}
					}
				}
			}
			return num;
		}

		private int OnConditionExtraHurt(BuffSkill _buffSkill, PoolObjHandle<ActorRoot> _attack)
		{
			int result = 0;
			ResDT_SkillFunc skillFunc = null;
			if (_buffSkill != null && _buffSkill.FindSkillFunc(44, out skillFunc))
			{
				int skillFuncParam = _buffSkill.GetSkillFuncParam(skillFunc, 0, false);
				int skillFuncParam2 = _buffSkill.GetSkillFuncParam(skillFunc, 1, false);
				int skillFuncParam3 = _buffSkill.GetSkillFuncParam(skillFunc, 2, false);
				int skillFuncParam4 = _buffSkill.GetSkillFuncParam(skillFunc, 3, false);
				bool flag = skillFuncParam == 1;
				int num = (!flag) ? this.actor.ValueComponent.actorHp : _attack.get_handle().ValueComponent.actorHp;
				int num2 = (!flag) ? this.actor.ValueComponent.mActorValue[5].totalValue : _attack.get_handle().ValueComponent.mActorValue[5].totalValue;
				int num3 = num * 10000 / num2;
				if (skillFuncParam3 == 1)
				{
					if (num3 <= skillFuncParam2)
					{
						result = skillFuncParam4;
					}
				}
				else if (skillFuncParam3 == 4 && num3 >= skillFuncParam2)
				{
					result = skillFuncParam4;
				}
			}
			return result;
		}

		private bool CheckTargetFromEnemy(PoolObjHandle<ActorRoot> src, PoolObjHandle<ActorRoot> target)
		{
			bool result = false;
			if (!src || !target)
			{
				return result;
			}
			if (src.get_handle().TheActorMeta.ActorType != ActorTypeDef.Actor_Type_Hero)
			{
				return result;
			}
			if (target.get_handle().TheActorMeta.ActorType != ActorTypeDef.Actor_Type_Monster)
			{
				return src.get_handle().TheActorMeta.ActorCamp != target.get_handle().TheActorMeta.ActorCamp;
			}
			MonsterWrapper monsterWrapper = target.get_handle().AsMonster();
			if (monsterWrapper != null)
			{
				RES_MONSTER_TYPE bMonsterType = monsterWrapper.cfgInfo.bMonsterType;
				if (bMonsterType == 1)
				{
					if (src.get_handle().TheActorMeta.ActorCamp != target.get_handle().TheActorMeta.ActorCamp)
					{
						result = true;
					}
				}
				else if (bMonsterType == 2)
				{
					byte actorSubSoliderType = monsterWrapper.GetActorSubSoliderType();
					if (actorSubSoliderType != 8 && actorSubSoliderType != 9 && actorSubSoliderType != 7 && actorSubSoliderType != 14)
					{
						long num = 0L;
						long num2 = 0L;
						VInt3 bornPos = target.get_handle().BornPos;
						List<PoolObjHandle<ActorRoot>> organActors = Singleton<GameObjMgr>.get_instance().OrganActors;
						int num3 = 0;
						for (int i = 0; i < organActors.get_Count(); i++)
						{
							PoolObjHandle<ActorRoot> poolObjHandle = organActors.get_Item(i);
							if (poolObjHandle.get_handle().TheStaticData.TheOrganOnlyInfo.OrganType == 2)
							{
								VInt3 location = poolObjHandle.get_handle().location;
								if (poolObjHandle.get_handle().TheActorMeta.ActorCamp == src.get_handle().TheActorMeta.ActorCamp)
								{
									num = (bornPos - location).get_sqrMagnitudeLong2D();
								}
								else
								{
									num2 = (bornPos - location).get_sqrMagnitudeLong2D();
								}
								num3++;
								if (num3 >= 2)
								{
									break;
								}
							}
						}
						if (num > num2)
						{
							result = true;
						}
					}
				}
			}
			return result;
		}

		public bool CheckTargetSubType(int typeMask, int typeSubMask, PoolObjHandle<ActorRoot> target)
		{
			if (typeMask == 0)
			{
				return true;
			}
			if (target)
			{
				int actorType = (int)target.get_handle().TheActorMeta.ActorType;
				if ((typeMask & 1 << actorType) > 0)
				{
					if (actorType != 1)
					{
						return true;
					}
					if (typeSubMask == 0)
					{
						return true;
					}
					int actorSubType = (int)target.get_handle().ActorControl.GetActorSubType();
					if (actorSubType == typeSubMask)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool CheckTriggerCondtion(int conditionalType, int iParam, PoolObjHandle<ActorRoot> src, PoolObjHandle<ActorRoot> target)
		{
			return conditionalType == 0 || conditionalType != 1 || this.CheckHpConditional(src, target, iParam);
		}

		public bool CheckHpConditional(PoolObjHandle<ActorRoot> src, PoolObjHandle<ActorRoot> target, int iParam)
		{
			if (!src || !target)
			{
				return false;
			}
			ulong propertyHpRate = TargetProperty.GetPropertyHpRate(src, 5);
			ulong propertyHpRate2 = TargetProperty.GetPropertyHpRate(target, 5);
			return SmartCompare.Compare<ulong>(propertyHpRate, propertyHpRate2, iParam);
		}

		private int OnTargetExtraHurt(BuffSkill _buffSkill, PoolObjHandle<ActorRoot> _attack)
		{
			int result = 0;
			ResDT_SkillFunc skillFunc = null;
			if (_buffSkill != null && _buffSkill.FindSkillFunc(48, out skillFunc))
			{
				int skillFuncParam = _buffSkill.GetSkillFuncParam(skillFunc, 0, false);
				int skillFuncParam2 = _buffSkill.GetSkillFuncParam(skillFunc, 1, false);
				int skillFuncParam3 = _buffSkill.GetSkillFuncParam(skillFunc, 2, false);
				if (this.CheckTargetSubType(skillFuncParam, skillFuncParam2, this.actorPtr))
				{
					result = skillFuncParam3;
				}
			}
			return result;
		}

		private int OnControlExtraHurt(BuffSkill _buffSkill, PoolObjHandle<ActorRoot> _attack)
		{
			int result = 0;
			ResDT_SkillFunc skillFunc = null;
			if (_buffSkill != null && _buffSkill.FindSkillFunc(51, out skillFunc) && this.actor != null)
			{
				for (int i = 0; i < this.actor.BuffHolderComp.SpawnedBuffList.get_Count(); i++)
				{
					BuffSkill buffSkill = this.actor.BuffHolderComp.SpawnedBuffList.get_Item(i);
					if (buffSkill != null && buffSkill.cfgData.bEffectType == 2)
					{
						result = _buffSkill.GetSkillFuncParam(skillFunc, 0, false);
						break;
					}
				}
			}
			return result;
		}

		public int GetExtraHurtOutputRate(PoolObjHandle<ActorRoot> _attack)
		{
			int num = 0;
			if (!_attack)
			{
				return 0;
			}
			for (int i = 0; i < _attack.get_handle().BuffHolderComp.SpawnedBuffList.get_Count(); i++)
			{
				BuffSkill buffSkill = _attack.get_handle().BuffHolderComp.SpawnedBuffList.get_Item(i);
				num += this.OnConditionExtraHurt(buffSkill, _attack);
				num += this.OnTargetExtraHurt(buffSkill, _attack);
				num += this.OnControlExtraHurt(buffSkill, _attack);
			}
			return num;
		}

		private bool OnChangeExtraEffectSkillSlot(PoolObjHandle<ActorRoot> _attack, SkillSlotType _slotType, out SkillSlotType _outSlotType)
		{
			ResDT_SkillFunc skillFunc = null;
			_outSlotType = _slotType;
			for (int i = 0; i < _attack.get_handle().BuffHolderComp.SpawnedBuffList.get_Count(); i++)
			{
				BuffSkill buffSkill = _attack.get_handle().BuffHolderComp.SpawnedBuffList.get_Item(i);
				if (buffSkill != null && buffSkill.FindSkillFunc(78, out skillFunc))
				{
					int skillFuncParam = buffSkill.GetSkillFuncParam(skillFunc, 0, false);
					int skillFuncParam2 = buffSkill.GetSkillFuncParam(skillFunc, 1, false);
					if (_slotType == (SkillSlotType)skillFuncParam)
					{
						_outSlotType = (SkillSlotType)skillFuncParam2;
						return true;
					}
				}
			}
			return false;
		}

		private void OnDamageTriggerEffect(PoolObjHandle<ActorRoot> _selfActor, PoolObjHandle<ActorRoot> _attacker)
		{
			ResDT_SkillFunc skillFunc = null;
			if (!_selfActor || !_attacker || _attacker.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Organ)
			{
				return;
			}
			for (int i = 0; i < _selfActor.get_handle().BuffHolderComp.SpawnedBuffList.get_Count(); i++)
			{
				BuffSkill buffSkill = _selfActor.get_handle().BuffHolderComp.SpawnedBuffList.get_Item(i);
				if (buffSkill != null && buffSkill.FindSkillFunc(84, out skillFunc))
				{
					int skillFuncParam = buffSkill.GetSkillFuncParam(skillFunc, 0, false);
					if (skillFuncParam > 0)
					{
						SkillUseParam skillUseParam = default(SkillUseParam);
						skillUseParam.Init();
						skillUseParam.SetOriginator(_selfActor);
						skillUseParam.bExposing = buffSkill.skillContext.bExposing;
						_selfActor.get_handle().SkillControl.SpawnBuff(_attacker, ref skillUseParam, skillFuncParam, true);
						_selfActor.get_handle().BuffHolderComp.RemoveBuff(buffSkill);
					}
				}
			}
		}

		private void OnDamageExtraEffect(PoolObjHandle<ActorRoot> _attack, SkillSlotType _slotType)
		{
			ResDT_SkillFunc skillFunc = null;
			if (!_attack)
			{
				return;
			}
			for (int i = 0; i < _attack.get_handle().BuffHolderComp.SpawnedBuffList.get_Count(); i++)
			{
				BuffSkill buffSkill = _attack.get_handle().BuffHolderComp.SpawnedBuffList.get_Item(i);
				if (buffSkill != null && buffSkill.FindSkillFunc(33, out skillFunc))
				{
					bool flag = false;
					bool flag2 = true;
					int skillFuncParam = buffSkill.GetSkillFuncParam(skillFunc, 0, false);
					int skillFuncParam2 = buffSkill.GetSkillFuncParam(skillFunc, 1, false);
					int skillFuncParam3 = buffSkill.GetSkillFuncParam(skillFunc, 2, false);
					int skillFuncParam4 = buffSkill.GetSkillFuncParam(skillFunc, 3, false);
					int skillFuncParam5 = buffSkill.GetSkillFuncParam(skillFunc, 4, false);
					int skillFuncParam6 = buffSkill.GetSkillFuncParam(skillFunc, 5, false);
					int skillFuncParam7 = buffSkill.GetSkillFuncParam(skillFunc, 6, false);
					int skillFuncParam8 = buffSkill.GetSkillFuncParam(skillFunc, 7, false);
					if (skillFuncParam3 == 0 && this.CheckTargetSubType(skillFuncParam4, skillFuncParam5, this.actorPtr))
					{
						if (skillFuncParam7 == 0 || this.CheckTriggerCondtion(skillFuncParam7, skillFuncParam8, _attack, this.actorPtr))
						{
							if (skillFuncParam2 == 0)
							{
								flag = true;
							}
							else if ((skillFuncParam2 & 1 << (int)_slotType) > 0)
							{
								flag = true;
							}
							if (skillFuncParam6 > 0)
							{
								flag2 = (Singleton<FrameSynchr>.GetInstance().LogicFrameTick - buffSkill.controlTime >= (ulong)((long)skillFuncParam6));
							}
							if (flag && flag2)
							{
								if (skillFuncParam6 != -2 || !buffSkill.IsNextDestroy())
								{
									SkillUseParam skillUseParam = default(SkillUseParam);
									skillUseParam.Init();
									skillUseParam.SetOriginator(_attack);
									skillUseParam.bExposing = buffSkill.skillContext.bExposing;
									if (buffSkill.skillContext != null)
									{
										if (buffSkill.skillContext.SlotType != SkillSlotType.SLOT_SKILL_COUNT)
										{
											skillUseParam.SlotType = _slotType;
										}
										else
										{
											skillUseParam.SlotType = buffSkill.skillContext.SlotType;
										}
									}
									else
									{
										skillUseParam.SlotType = _slotType;
									}
									_attack.get_handle().SkillControl.SpawnBuff(this.actorPtr, ref skillUseParam, skillFuncParam, true);
									buffSkill.controlTime = Singleton<FrameSynchr>.GetInstance().LogicFrameTick;
									if (skillFuncParam6 == -1 || skillFuncParam6 == -2)
									{
										_attack.get_handle().BuffHolderComp.RemoveBuff(buffSkill);
									}
								}
							}
						}
					}
				}
			}
		}

		public int OnDamage(ref HurtDataInfo _hurt, int _hurtValue)
		{
			if (!_hurt.bLastHurt)
			{
				this.clearRule.CheckBuffClear(1);
			}
			if (!_hurt.bExtraBuff)
			{
				SkillSlotType atkSlot = _hurt.atkSlot;
				SkillSlotType atkSlot2;
				bool flag = this.OnChangeExtraEffectSkillSlot(_hurt.atker, _hurt.atkSlot, out atkSlot2);
				if (flag)
				{
					_hurt.atkSlot = atkSlot2;
				}
				this.OnDamageTriggerEffect(_hurt.target, _hurt.atker);
				this.OnDamageExtraEffect(_hurt.atker, _hurt.atkSlot);
				if (flag)
				{
					_hurt.atkSlot = atkSlot;
				}
			}
			int num = _hurtValue * _hurt.iEffectFadeRate / 10000;
			num = num * _hurt.iOverlayFadeRate / 10000;
			num = this.protectRule.ResistDamage(ref _hurt, num);
			num = BufferLogicEffect.OnDamageExtraEffect(ref _hurt, num);
			num = this.DealDamageContionType(ref _hurt, num);
			this.OnDamageExtraHurtFunc(ref _hurt, _hurt.atkSlot);
			return num;
		}

		private int DealDamageContionType(ref HurtDataInfo _hurt, int _hurtValue)
		{
			if (!_hurt.atker)
			{
				return _hurtValue;
			}
			if (_hurt.iConditionType == 1)
			{
				int nAddHp = _hurtValue * _hurt.iConditionParam / 10000;
				_hurt.atker.get_handle().ActorControl.ReviveHp(nAddHp);
			}
			else if (_hurt.iConditionType == 2 && _hurt.target && _hurt.atker)
			{
				int magnitude2D = (_hurt.atker.get_handle().location - _hurt.target.get_handle().location).get_magnitude2D();
				int num = (int)((long)magnitude2D * (long)_hurtValue * (long)_hurt.iConditionParam / 10000000L);
				_hurtValue += num;
			}
			return _hurtValue;
		}

		public bool IsExistSkillFuncType(int inSkillFuncType)
		{
			ResDT_SkillFunc resDT_SkillFunc = null;
			for (int i = 0; i < this.SpawnedBuffList.get_Count(); i++)
			{
				BuffSkill buffSkill = this.SpawnedBuffList.get_Item(i);
				if (buffSkill != null && buffSkill.FindSkillFunc(inSkillFuncType, out resDT_SkillFunc))
				{
					return true;
				}
			}
			return false;
		}

		public void OnAssistEffect(ref PoolObjHandle<ActorRoot> deadActor)
		{
			ResDT_SkillFunc skillFunc = null;
			for (int i = 0; i < this.SpawnedBuffList.get_Count(); i++)
			{
				BuffSkill buffSkill = this.SpawnedBuffList.get_Item(i);
				if (buffSkill != null && buffSkill.FindSkillFunc(33, out skillFunc))
				{
					int skillFuncParam = buffSkill.GetSkillFuncParam(skillFunc, 0, false);
					int skillFuncParam2 = buffSkill.GetSkillFuncParam(skillFunc, 1, false);
					int skillFuncParam3 = buffSkill.GetSkillFuncParam(skillFunc, 2, false);
					int skillFuncParam4 = buffSkill.GetSkillFuncParam(skillFunc, 3, false);
					int skillFuncParam5 = buffSkill.GetSkillFuncParam(skillFunc, 4, false);
					if (skillFuncParam3 == 2 && this.CheckTargetSubType(skillFuncParam4, skillFuncParam5, deadActor))
					{
						SkillUseParam skillUseParam = default(SkillUseParam);
						skillUseParam.Init();
						skillUseParam.SetOriginator(this.actorPtr);
						skillUseParam.bExposing = buffSkill.skillContext.bExposing;
						this.actor.SkillControl.SpawnBuff(this.actorPtr, ref skillUseParam, skillFuncParam, true);
					}
				}
			}
		}

		private void OnDeadExtraEffect(PoolObjHandle<ActorRoot> _attack)
		{
			if (!_attack)
			{
				return;
			}
			ResDT_SkillFunc skillFunc = null;
			for (int i = 0; i < _attack.get_handle().BuffHolderComp.SpawnedBuffList.get_Count(); i++)
			{
				BuffSkill buffSkill = _attack.get_handle().BuffHolderComp.SpawnedBuffList.get_Item(i);
				if (buffSkill != null && buffSkill.FindSkillFunc(33, out skillFunc))
				{
					int skillFuncParam = buffSkill.GetSkillFuncParam(skillFunc, 0, false);
					int skillFuncParam2 = buffSkill.GetSkillFuncParam(skillFunc, 1, false);
					int skillFuncParam3 = buffSkill.GetSkillFuncParam(skillFunc, 2, false);
					int skillFuncParam4 = buffSkill.GetSkillFuncParam(skillFunc, 3, false);
					int skillFuncParam5 = buffSkill.GetSkillFuncParam(skillFunc, 4, false);
					if (skillFuncParam3 == 1 && this.CheckTargetSubType(skillFuncParam4, skillFuncParam5, this.actorPtr))
					{
						SkillUseParam skillUseParam = default(SkillUseParam);
						skillUseParam.Init();
						skillUseParam.SetOriginator(_attack);
						skillUseParam.bExposing = buffSkill.skillContext.bExposing;
						_attack.get_handle().SkillControl.SpawnBuff(_attack, ref skillUseParam, skillFuncParam, true);
					}
				}
			}
		}

		public void OnDead(PoolObjHandle<ActorRoot> _attack)
		{
			ResDT_SkillFunc skillFunc = null;
			if (this.clearRule != null)
			{
				this.clearRule.CheckBuffNoClear(2);
			}
			if (this.logicEffect != null)
			{
				this.logicEffect.Clear();
			}
			if (this.actorPtr.get_handle().ActorControl.IsKilledByHero())
			{
				_attack = this.actorPtr.get_handle().ActorControl.LastHeroAtker;
			}
			for (int i = 0; i < this.SpawnedBuffList.get_Count(); i++)
			{
				BuffSkill buffSkill = this.SpawnedBuffList.get_Item(i);
				if (buffSkill != null && buffSkill.FindSkillFunc(32, out skillFunc))
				{
					int skillFuncParam = buffSkill.GetSkillFuncParam(skillFunc, 0, false);
					int skillFuncParam2 = buffSkill.GetSkillFuncParam(skillFunc, 1, false);
					bool autoReset = buffSkill.GetSkillFuncParam(skillFunc, 2, false) == 1;
					bool bBaseRevive = buffSkill.GetSkillFuncParam(skillFunc, 3, false) == 0;
					bool bCDReset = buffSkill.GetSkillFuncParam(skillFunc, 4, false) == 1;
					this.actor.ActorControl.SetReviveContext(skillFuncParam, skillFuncParam2, autoReset, bBaseRevive, bCDReset);
					this.RemoveBuff(buffSkill);
				}
				if (this.actor.TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero && _attack && _attack.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero && buffSkill != null && buffSkill.cfgData != null && buffSkill.cfgData.bIsInheritByKiller == 1)
				{
					this.RemoveBuff(buffSkill);
					SkillUseParam skillUseParam = default(SkillUseParam);
					skillUseParam.SetOriginator(_attack);
					skillUseParam.bExposing = buffSkill.skillContext.bExposing;
					_attack.get_handle().SkillControl.SpawnBuff(_attack, ref skillUseParam, buffSkill.SkillID, true);
				}
			}
			this.OnDeadExtraEffect(_attack);
			this.markRule.Clear();
		}

		public void OnDamageExtraValueEffect(ref HurtDataInfo _hurt, PoolObjHandle<ActorRoot> _attack, SkillSlotType _slotType)
		{
			ResDT_SkillFunc skillFunc = null;
			if (!_attack)
			{
				return;
			}
			for (int i = 0; i < _attack.get_handle().BuffHolderComp.SpawnedBuffList.get_Count(); i++)
			{
				BuffSkill buffSkill = _attack.get_handle().BuffHolderComp.SpawnedBuffList.get_Item(i);
				if (_hurt.hurtType == HurtTypeDef.Therapic)
				{
					if (buffSkill != null && buffSkill.FindSkillFunc(64, out skillFunc))
					{
						_hurt.iAddTotalHurtValueRate = _attack.get_handle().ValueComponent.mActorValue[36].addRatio;
						_hurt.iAddTotalHurtValue = _attack.get_handle().ValueComponent.mActorValue[36].addValue;
					}
				}
				else
				{
					if (_slotType == SkillSlotType.SLOT_SKILL_0 && _attack.get_handle().SkillControl != null && _attack.get_handle().SkillControl.bIsLastAtkUseSkill && buffSkill != null && buffSkill.FindSkillFunc(61, out skillFunc))
					{
						int skillFuncParam = buffSkill.GetSkillFuncParam(skillFunc, 0, false);
						int num = buffSkill.GetSkillFuncParam(skillFunc, 1, false);
						int skillFuncParam2 = buffSkill.GetSkillFuncParam(skillFunc, 2, false);
						int skillFuncParam3 = buffSkill.GetSkillFuncParam(skillFunc, 3, false);
						if (skillFuncParam == 1)
						{
							num = num * _hurt.hurtValue / 10000;
							_hurt.hurtValue += num;
							_hurt.adValue += skillFuncParam2;
							_hurt.apValue += skillFuncParam3;
						}
						else
						{
							_hurt.hurtValue += num;
							_hurt.attackInfo.iActorATT = _hurt.attackInfo.iActorATT + skillFuncParam2;
							_hurt.attackInfo.iActorINT = _hurt.attackInfo.iActorINT + skillFuncParam3;
						}
					}
					if (_hurt.target && _hurt.target.get_handle().TheActorMeta.ActorType != ActorTypeDef.Actor_Type_Organ && buffSkill != null && buffSkill.FindSkillFunc(68, out skillFunc))
					{
						int skillFuncParam4 = buffSkill.GetSkillFuncParam(skillFunc, 0, false);
						int num2 = buffSkill.GetSkillFuncParam(skillFunc, 4, false);
						int skillFuncParam5 = buffSkill.GetSkillFuncParam(skillFunc, 5, false);
						if (_hurt.target.get_handle().ValueComponent != null)
						{
							if (skillFuncParam4 == 1)
							{
								num2 = _hurt.target.get_handle().ValueComponent.actorHpTotal * num2 / 10000;
							}
							if (_hurt.target.get_handle().ValueComponent.actorHp <= num2 && _hurt.target.get_handle().ActorControl != null && Singleton<FrameSynchr>.get_instance().LogicFrameTick - _hurt.target.get_handle().ActorControl.lastExtraHurtByLowHpBuffTime >= (ulong)((long)skillFuncParam5))
							{
								_hurt.target.get_handle().ActorControl.lastExtraHurtByLowHpBuffTime = Singleton<FrameSynchr>.get_instance().LogicFrameTick;
								int num3 = buffSkill.GetSkillFuncParam(skillFunc, 1, false);
								int num4 = buffSkill.GetSkillFuncParam(skillFunc, 2, false);
								int num5 = buffSkill.GetSkillFuncParam(skillFunc, 3, false);
								if (skillFuncParam4 == 1)
								{
									num3 = num3 * _hurt.hurtValue / 10000;
									num4 = num4 * _hurt.adValue / 10000;
									num5 = num5 * _hurt.apValue / 10000;
								}
								_hurt.hurtValue += num3;
								_hurt.adValue += num4;
								_hurt.apValue += num5;
							}
						}
					}
				}
			}
		}

		public void OnDamageExtraHurtFunc(ref HurtDataInfo _hurt, SkillSlotType _slotType)
		{
			ResDT_SkillFunc skillFunc = null;
			for (int i = 0; i < this.SpawnedBuffList.get_Count(); i++)
			{
				BuffSkill buffSkill = this.SpawnedBuffList.get_Item(i);
				if (buffSkill != null && buffSkill.FindSkillFunc(80, out skillFunc))
				{
					SkillSlotType skillFuncParam = (SkillSlotType)buffSkill.GetSkillFuncParam(skillFunc, 0, false);
					if (_slotType == skillFuncParam)
					{
						int skillFuncParam2 = buffSkill.GetSkillFuncParam(skillFunc, 1, false);
						_hurt.iReduceDamage += skillFuncParam2;
					}
				}
			}
		}

		public int OnHurtBounceDamage(ref HurtDataInfo hurt, int hp)
		{
			if (hp <= 0)
			{
				return hp;
			}
			if (!hurt.atker || hurt.bBounceHurt)
			{
				return hp;
			}
			ResDT_SkillFunc skillFunc = null;
			int num = hp;
			for (int i = 0; i < this.SpawnedBuffList.get_Count(); i++)
			{
				BuffSkill buffSkill = this.SpawnedBuffList.get_Item(i);
				if (buffSkill != null && buffSkill.FindSkillFunc(83, out skillFunc))
				{
					int skillFuncParam = buffSkill.GetSkillFuncParam(skillFunc, 2, false);
					if ((skillFuncParam & 1 << (int)hurt.atker.get_handle().TheActorMeta.ActorType) <= 0)
					{
						int skillFuncParam2 = buffSkill.GetSkillFuncParam(skillFunc, 0, false);
						int skillFuncParam3 = buffSkill.GetSkillFuncParam(skillFunc, 1, false);
						int num2 = num * skillFuncParam2 / 10000;
						num -= num2;
						HurtDataInfo hurtDataInfo = default(HurtDataInfo);
						HurtAttackerInfo attackInfo = default(HurtAttackerInfo);
						attackInfo.Init(hurt.target, hurt.atker);
						hurtDataInfo.atker = hurt.target;
						hurtDataInfo.target = hurt.atker;
						hurtDataInfo.attackInfo = attackInfo;
						hurtDataInfo.atkSlot = SkillSlotType.SLOT_SKILL_VALID;
						hurtDataInfo.hurtType = (HurtTypeDef)skillFuncParam3;
						hurtDataInfo.extraHurtType = ExtraHurtTypeDef.ExtraHurt_Value;
						hurtDataInfo.hurtValue = num2;
						hurtDataInfo.adValue = 0;
						hurtDataInfo.apValue = 0;
						hurtDataInfo.hpValue = 0;
						hurtDataInfo.loseHpValue = 0;
						hurtDataInfo.iConditionType = 0;
						hurtDataInfo.iConditionParam = 0;
						hurtDataInfo.hurtCount = 0;
						hurtDataInfo.firstHemoFadeRate = 0;
						hurtDataInfo.followUpHemoFadeRate = 0;
						hurtDataInfo.iEffectCountInSingleTrigger = 0;
						hurtDataInfo.bExtraBuff = false;
						hurtDataInfo.gatherTime = 0;
						hurtDataInfo.bBounceHurt = true;
						hurtDataInfo.bLastHurt = false;
						hurtDataInfo.iAddTotalHurtValueRate = 0;
						hurtDataInfo.iAddTotalHurtValue = 0;
						hurtDataInfo.iEffectFadeRate = 10000;
						hurtDataInfo.iOverlayFadeRate = 10000;
						int num3 = hurt.atker.get_handle().HurtControl.TakeBouncesDamage(ref hurtDataInfo);
					}
				}
			}
			return num;
		}
	}
}
