using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameLogic
{
	public class CHostHeroDamage
	{
		private PoolObjHandle<ActorRoot> m_actorHero;

		private PoolObjHandle<ActorRoot> m_actorKiller;

		private int m_iDamageIntervalTimeMax = 10000;

		private int m_iDamageStatisticsTimeMax = 30000;

		private Dictionary<uint, DAMAGE_ACTOR_INFO> m_listDamageActorValue = new Dictionary<uint, DAMAGE_ACTOR_INFO>();

		private ulong m_ulHeroDeadTime;

		private uint[] m_arrHurtValue = new uint[4];

		public void Init(PoolObjHandle<ActorRoot> actorRoot)
		{
			this.m_actorHero = actorRoot;
			this.m_iDamageIntervalTimeMax = (int)GameDataMgr.GetGlobeValue(298);
			this.m_iDamageStatisticsTimeMax = (int)GameDataMgr.GetGlobeValue(299);
		}

		public void UnInit()
		{
			if (this.m_actorHero)
			{
				this.m_actorHero.Release();
			}
			if (this.m_listDamageActorValue != null)
			{
				this.m_listDamageActorValue.Clear();
			}
		}

		public uint GetHostHeroObjId()
		{
			uint result = 0u;
			if (this.m_actorHero)
			{
				result = this.m_actorHero.get_handle().ObjID;
			}
			return result;
		}

		private void AddDamageValue(ref PoolObjHandle<ActorRoot> actor, SkillSlotType slotType, int iValue, HurtTypeDef hurtType)
		{
			if (this.m_listDamageActorValue != null && slotType <= SkillSlotType.SLOT_SKILL_VALID && actor)
			{
				uint objID = actor.get_handle().ObjID;
				this.DeleteTimeoutDamageValue(objID, 0uL);
				DAMAGE_ACTOR_INFO dAMAGE_ACTOR_INFO;
				if (!this.m_listDamageActorValue.TryGetValue(objID, ref dAMAGE_ACTOR_INFO))
				{
					dAMAGE_ACTOR_INFO = default(DAMAGE_ACTOR_INFO);
					dAMAGE_ACTOR_INFO.actorType = actor.get_handle().TheActorMeta.ActorType;
					dAMAGE_ACTOR_INFO.actorName = actor.get_handle().name;
					dAMAGE_ACTOR_INFO.ConfigId = actor.get_handle().TheActorMeta.ConfigId;
					if (dAMAGE_ACTOR_INFO.actorType == ActorTypeDef.Actor_Type_Monster)
					{
						MonsterWrapper monsterWrapper = actor.get_handle().AsMonster();
						dAMAGE_ACTOR_INFO.bMonsterType = monsterWrapper.GetActorSubType();
						dAMAGE_ACTOR_INFO.actorSubType = monsterWrapper.GetActorSubSoliderType();
					}
					Player player = Singleton<GamePlayerCenter>.get_instance().GetPlayer(actor.get_handle().TheActorMeta.PlayerId);
					if (player != null)
					{
						dAMAGE_ACTOR_INFO.playerName = player.Name;
					}
					dAMAGE_ACTOR_INFO.listDamage = new SortedList<ulong, DOUBLE_INT_INFO[]>();
					this.m_listDamageActorValue.Add(objID, dAMAGE_ACTOR_INFO);
				}
				ulong logicFrameTick = Singleton<FrameSynchr>.get_instance().LogicFrameTick;
				DOUBLE_INT_INFO[] array;
				if (!dAMAGE_ACTOR_INFO.listDamage.TryGetValue(logicFrameTick, ref array))
				{
					array = new DOUBLE_INT_INFO[12];
					dAMAGE_ACTOR_INFO.listDamage.Add(logicFrameTick, array);
				}
				if (array[(int)slotType].iValue == 0)
				{
					array[(int)slotType].iKey = (int)hurtType;
				}
				DOUBLE_INT_INFO[] expr_170_cp_0 = array;
				expr_170_cp_0[(int)slotType].iValue = expr_170_cp_0[(int)slotType].iValue + iValue;
				dAMAGE_ACTOR_INFO.listDamage.set_Item(logicFrameTick, array);
				this.m_listDamageActorValue.set_Item(objID, dAMAGE_ACTOR_INFO);
			}
		}

		private void DeleteTimeoutDamageValue(uint uiObjId, ulong ulTime = 0uL)
		{
			if (this.m_listDamageActorValue != null)
			{
				ulong num;
				if (ulTime == 0uL)
				{
					num = Singleton<FrameSynchr>.get_instance().LogicFrameTick;
				}
				else
				{
					num = ulTime;
				}
				DAMAGE_ACTOR_INFO dAMAGE_ACTOR_INFO;
				if (this.m_listDamageActorValue.TryGetValue(uiObjId, ref dAMAGE_ACTOR_INFO))
				{
					int count = dAMAGE_ACTOR_INFO.listDamage.get_Count();
					if (count > 0)
					{
						ulong num2 = dAMAGE_ACTOR_INFO.listDamage.get_Keys().get_Item(count - 1);
						if (num - num2 > (ulong)((long)this.m_iDamageIntervalTimeMax))
						{
							dAMAGE_ACTOR_INFO.listDamage.Clear();
						}
						else
						{
							IEnumerator<KeyValuePair<ulong, DOUBLE_INT_INFO[]>> enumerator = dAMAGE_ACTOR_INFO.listDamage.GetEnumerator();
							while (enumerator.MoveNext())
							{
								KeyValuePair<ulong, DOUBLE_INT_INFO[]> current = enumerator.get_Current();
								ulong key = current.get_Key();
								if (num - key <= (ulong)((long)this.m_iDamageStatisticsTimeMax))
								{
									break;
								}
								dAMAGE_ACTOR_INFO.listDamage.Remove(key);
							}
						}
						this.m_listDamageActorValue.set_Item(uiObjId, dAMAGE_ACTOR_INFO);
					}
				}
			}
		}

		private void DeleteDamageValue(uint uiObjId)
		{
			if (this.m_listDamageActorValue != null && this.m_listDamageActorValue.ContainsKey(uiObjId))
			{
				this.m_listDamageActorValue.Remove(uiObjId);
			}
		}

		public void OnActorDamage(ref HurtEventResultInfo prm)
		{
			if (prm.src == this.m_actorHero && prm.hurtInfo.hurtType != HurtTypeDef.Therapic && prm.atker)
			{
				this.AddDamageValue(ref prm.atker, prm.hurtInfo.atkSlot, prm.hurtTotal, prm.hurtInfo.hurtType);
			}
		}

		public void OnActorDead(ref GameDeadEventParam prm)
		{
			if (prm.src == this.m_actorHero)
			{
				if (prm.orignalAtker && prm.orignalAtker.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero)
				{
					this.m_actorKiller = prm.orignalAtker;
				}
				else if (prm.src.get_handle().ActorControl.IsKilledByHero())
				{
					this.m_actorKiller = prm.src.get_handle().ActorControl.LastHeroAtker;
				}
				else
				{
					this.m_actorKiller = prm.atker;
				}
				this.m_ulHeroDeadTime = Singleton<FrameSynchr>.get_instance().LogicFrameTick;
			}
		}

		public void OnActorRevive(ref DefaultGameEventParam prm)
		{
			if (prm.src == this.m_actorHero)
			{
				if (this.m_listDamageActorValue != null)
				{
					this.m_listDamageActorValue.Clear();
				}
				if (this.m_actorKiller)
				{
					this.m_actorKiller.Release();
				}
				this.m_ulHeroDeadTime = 0uL;
			}
		}

		public bool GetActorDamage(uint uiObjId, ref DOUBLE_INT_INFO[] arrDamageInfo)
		{
			DAMAGE_ACTOR_INFO dAMAGE_ACTOR_INFO;
			if (this.m_listDamageActorValue == null || this.m_listDamageActorValue.get_Count() <= 0 || arrDamageInfo == null || !this.m_listDamageActorValue.TryGetValue(uiObjId, ref dAMAGE_ACTOR_INFO))
			{
				return false;
			}
			int[] array = new int[12];
			int count = dAMAGE_ACTOR_INFO.listDamage.get_Count();
			if (count <= 0)
			{
				return false;
			}
			if (this.m_ulHeroDeadTime - dAMAGE_ACTOR_INFO.listDamage.get_Keys().get_Item(count - 1) > (ulong)((long)this.m_iDamageIntervalTimeMax))
			{
				return false;
			}
			IEnumerator<KeyValuePair<ulong, DOUBLE_INT_INFO[]>> enumerator = dAMAGE_ACTOR_INFO.listDamage.GetEnumerator();
			while (enumerator.MoveNext())
			{
				ulong arg_A6_0 = this.m_ulHeroDeadTime;
				KeyValuePair<ulong, DOUBLE_INT_INFO[]> current = enumerator.get_Current();
				if (arg_A6_0 - current.get_Key() <= (ulong)((long)this.m_iDamageStatisticsTimeMax))
				{
					KeyValuePair<ulong, DOUBLE_INT_INFO[]> current2 = enumerator.get_Current();
					DOUBLE_INT_INFO[] value = current2.get_Value();
					if (value != null)
					{
						for (int i = 0; i <= 11; i++)
						{
							array[i] += value[i].iValue;
						}
					}
				}
			}
			for (int j = 0; j <= 11; j++)
			{
				arrDamageInfo[j].iValue = array[j];
				arrDamageInfo[j].iKey = j;
			}
			Array.Sort<DOUBLE_INT_INFO>(arrDamageInfo, (DOUBLE_INT_INFO p1, DOUBLE_INT_INFO p2) => p2.iValue.CompareTo(p1.iValue));
			return true;
		}

		public int GetAllActorsTotalDamageAndTopActorId(ref uint[] arrObjId, int iTopCount, ref uint uiAllTotalDamage, ref uint[] arrDiffTypeHurtValue)
		{
			int num = 0;
			uiAllTotalDamage = 0u;
			if (this.m_listDamageActorValue != null && this.m_listDamageActorValue.get_Count() > 0)
			{
				int num2 = 0;
				DOUBLE_INT_INFO[] array = new DOUBLE_INT_INFO[this.m_listDamageActorValue.get_Count()];
				Dictionary<uint, DAMAGE_ACTOR_INFO>.Enumerator enumerator = this.m_listDamageActorValue.GetEnumerator();
				while (enumerator.MoveNext())
				{
					int num3 = 0;
					KeyValuePair<uint, DAMAGE_ACTOR_INFO> current = enumerator.get_Current();
					int count = current.get_Value().listDamage.get_Count();
					if (count > 0)
					{
						ulong arg_A5_0 = this.m_ulHeroDeadTime;
						KeyValuePair<uint, DAMAGE_ACTOR_INFO> current2 = enumerator.get_Current();
						if (arg_A5_0 - current2.get_Value().listDamage.get_Keys().get_Item(count - 1) <= (ulong)((long)this.m_iDamageIntervalTimeMax))
						{
							KeyValuePair<uint, DAMAGE_ACTOR_INFO> current3 = enumerator.get_Current();
							IEnumerator<KeyValuePair<ulong, DOUBLE_INT_INFO[]>> enumerator2 = current3.get_Value().listDamage.GetEnumerator();
							while (enumerator2.MoveNext())
							{
								ulong arg_F2_0 = this.m_ulHeroDeadTime;
								KeyValuePair<ulong, DOUBLE_INT_INFO[]> current4 = enumerator2.get_Current();
								if (arg_F2_0 - current4.get_Key() <= (ulong)((long)this.m_iDamageStatisticsTimeMax))
								{
									KeyValuePair<ulong, DOUBLE_INT_INFO[]> current5 = enumerator2.get_Current();
									DOUBLE_INT_INFO[] value = current5.get_Value();
									if (value != null)
									{
										for (int i = 0; i <= 11; i++)
										{
											num3 += value[i].iValue;
											if (value[i].iKey >= 0 && value[i].iKey < 4)
											{
												arrDiffTypeHurtValue[value[i].iKey] += (uint)value[i].iValue;
											}
										}
									}
								}
							}
							if (num3 > 0)
							{
								uiAllTotalDamage += (uint)num3;
								if (this.m_actorKiller)
								{
									uint arg_1D7_0 = this.m_actorKiller.get_handle().ObjID;
									KeyValuePair<uint, DAMAGE_ACTOR_INFO> current6 = enumerator.get_Current();
									if (arg_1D7_0 != current6.get_Key())
									{
										DOUBLE_INT_INFO[] arg_1F4_0_cp_0 = array;
										int arg_1F4_0_cp_1 = num2;
										KeyValuePair<uint, DAMAGE_ACTOR_INFO> current7 = enumerator.get_Current();
										arg_1F4_0_cp_0[arg_1F4_0_cp_1].iKey = (int)current7.get_Key();
										array[num2].iValue = num3;
										num2++;
									}
								}
							}
						}
					}
				}
				Array.Sort<DOUBLE_INT_INFO>(array, (DOUBLE_INT_INFO p1, DOUBLE_INT_INFO p2) => p2.iValue.CompareTo(p1.iValue));
				int num4 = 0;
				while (num4 < iTopCount && num4 < num2)
				{
					if (!this.m_actorKiller || (long)array[num4].iKey != (long)((ulong)this.m_actorKiller.get_handle().ObjID))
					{
						if (array[num4].iValue <= 0)
						{
							break;
						}
						arrObjId[num4] = (uint)array[num4].iKey;
						num++;
					}
					num4++;
				}
			}
			return num;
		}

		public int GetSkillSlotHurtType(uint uiObjId, SkillSlotType slotType)
		{
			DAMAGE_ACTOR_INFO dAMAGE_ACTOR_INFO;
			if (slotType >= SkillSlotType.SLOT_SKILL_0 && slotType <= SkillSlotType.SLOT_SKILL_VALID && this.m_listDamageActorValue != null && this.m_listDamageActorValue.get_Count() > 0 && this.m_listDamageActorValue.TryGetValue(uiObjId, ref dAMAGE_ACTOR_INFO))
			{
				IEnumerator<KeyValuePair<ulong, DOUBLE_INT_INFO[]>> enumerator = dAMAGE_ACTOR_INFO.listDamage.GetEnumerator();
				while (enumerator.MoveNext())
				{
					KeyValuePair<ulong, DOUBLE_INT_INFO[]> current = enumerator.get_Current();
					DOUBLE_INT_INFO[] value = current.get_Value();
					if (value != null && value[(int)slotType].iValue != 0)
					{
						return value[(int)slotType].iKey;
					}
				}
			}
			return -1;
		}

		public bool GetKillerObjId(ref uint uiObjId, ref ActorTypeDef actorType)
		{
			if (this.m_actorKiller)
			{
				uiObjId = this.m_actorKiller.get_handle().ObjID;
				actorType = this.m_actorKiller.get_handle().TheActorMeta.ActorType;
				return true;
			}
			return false;
		}

		public ulong GetHostHeroDeadTime()
		{
			return this.m_ulHeroDeadTime;
		}

		public bool GetDamageActorInfo(uint uiObjId, ref string actorName, ref string playerName, ref ActorTypeDef actorType, ref int iConfigId, ref byte actorSubType, ref byte bMonsterType)
		{
			DAMAGE_ACTOR_INFO dAMAGE_ACTOR_INFO;
			if (this.m_listDamageActorValue != null && this.m_listDamageActorValue.get_Count() > 0 && this.m_listDamageActorValue.TryGetValue(uiObjId, ref dAMAGE_ACTOR_INFO))
			{
				actorType = dAMAGE_ACTOR_INFO.actorType;
				actorName = dAMAGE_ACTOR_INFO.actorName;
				playerName = dAMAGE_ACTOR_INFO.playerName;
				iConfigId = dAMAGE_ACTOR_INFO.ConfigId;
				actorSubType = dAMAGE_ACTOR_INFO.actorSubType;
				bMonsterType = dAMAGE_ACTOR_INFO.bMonsterType;
				return true;
			}
			return false;
		}
	}
}
