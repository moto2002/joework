using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using CSProtocol;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameLogic
{
	public class CBattleDeadStat
	{
		private List<DeadRecord> m_deadRecordList = new List<DeadRecord>(32);

		private int m_deadMonsterNum;

		public uint m_uiFBTime;

		private int m_baojunEnterCombatTime;

		private int m_baronEnterCombatTime;

		private int m_bigDragonEnterCombatTime;

		private int[] m_arrHeroEnterCombatTime = new int[10];

		public int enemyKillHeroMaxGap;

		public void StartRecord()
		{
			this.Clear();
			this.AddEventHandler();
		}

		public void Clear()
		{
			this.m_deadMonsterNum = 0;
			this.m_deadRecordList.Clear();
			this.RemoveEventHandler();
			this.enemyKillHeroMaxGap = 0;
			for (int i = 0; i < 10; i++)
			{
				this.m_arrHeroEnterCombatTime[0] = 0;
			}
		}

		public void AddEventHandler()
		{
			Singleton<GameEventSys>.get_instance().AddEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.OnDeadRecord));
			Singleton<GameEventSys>.get_instance().AddEventHandler<DefaultGameEventParam>(GameEventDef.Event_ActorEnterCombat, new RefAction<DefaultGameEventParam>(this.OnEnterCombat));
		}

		public void RemoveEventHandler()
		{
			Singleton<GameEventSys>.get_instance().RmvEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.OnDeadRecord));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_ActorEnterCombat, new RefAction<DefaultGameEventParam>(this.OnEnterCombat));
		}

		private void OnEnterCombat(ref DefaultGameEventParam prm)
		{
			if (prm.src && prm.src.get_handle().ActorControl != null && prm.src.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Monster && prm.src.get_handle().ActorControl.GetActorSubType() == 2)
			{
				if (prm.src.get_handle().ActorControl.GetActorSubSoliderType() == 7)
				{
					this.m_baojunEnterCombatTime = (int)Singleton<FrameSynchr>.get_instance().LogicFrameTick;
				}
				else if (prm.src.get_handle().ActorControl.GetActorSubSoliderType() == 8)
				{
					this.m_baronEnterCombatTime = (int)Singleton<FrameSynchr>.get_instance().LogicFrameTick;
				}
				else if (prm.src.get_handle().ActorControl.GetActorSubSoliderType() == 9)
				{
					this.m_bigDragonEnterCombatTime = (int)Singleton<FrameSynchr>.get_instance().LogicFrameTick;
				}
			}
			else if (prm.src && prm.src.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero)
			{
				List<PoolObjHandle<ActorRoot>> heroActors = Singleton<GameObjMgr>.get_instance().HeroActors;
				int count = heroActors.get_Count();
				for (int i = 0; i < count; i++)
				{
					if (heroActors.get_Item(i).get_handle().ObjID == prm.src.get_handle().ObjID && i < 10)
					{
						this.m_arrHeroEnterCombatTime[i] = (int)Singleton<FrameSynchr>.get_instance().LogicFrameTick;
						break;
					}
				}
			}
		}

		private void OnDeadRecord(ref GameDeadEventParam prm)
		{
			if (prm.bImmediateRevive)
			{
				return;
			}
			PoolObjHandle<ActorRoot> src = prm.src;
			PoolObjHandle<ActorRoot> logicAtker = prm.logicAtker;
			if (!src || !logicAtker)
			{
				return;
			}
			if (src.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero)
			{
				DeadRecord deadRecord = new DeadRecord(src.get_handle().TheActorMeta.ActorCamp, src.get_handle().TheActorMeta.ActorType, src.get_handle().TheActorMeta.ConfigId, (int)Singleton<FrameSynchr>.get_instance().LogicFrameTick, logicAtker.get_handle().TheActorMeta.ActorCamp, logicAtker.get_handle().TheActorMeta.PlayerId, logicAtker.get_handle().TheActorMeta.ActorType);
				List<PoolObjHandle<ActorRoot>> heroActors = Singleton<GameObjMgr>.get_instance().HeroActors;
				int count = heroActors.get_Count();
				for (int i = 0; i < count; i++)
				{
					if (heroActors.get_Item(i).get_handle().ObjID == prm.src.get_handle().ObjID && i < 10)
					{
						deadRecord.fightTime = (int)Singleton<FrameSynchr>.get_instance().LogicFrameTick - this.m_arrHeroEnterCombatTime[i];
						break;
					}
				}
				this.m_deadRecordList.Add(deadRecord);
				if (this.m_uiFBTime == 0u)
				{
					this.m_uiFBTime = (uint)Singleton<FrameSynchr>.get_instance().LogicFrameTick;
				}
			}
			else if (src.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Monster)
			{
				DeadRecord deadRecord2 = new DeadRecord(src.get_handle().TheActorMeta.ActorCamp, src.get_handle().TheActorMeta.ActorType, src.get_handle().TheActorMeta.ConfigId, (int)Singleton<FrameSynchr>.get_instance().LogicFrameTick, logicAtker.get_handle().TheActorMeta.ActorCamp, logicAtker.get_handle().TheActorMeta.PlayerId, logicAtker.get_handle().TheActorMeta.ActorType);
				if (src.get_handle().ActorControl != null)
				{
					deadRecord2.actorSubType = src.get_handle().ActorControl.GetActorSubType();
					deadRecord2.actorSubSoliderType = src.get_handle().ActorControl.GetActorSubSoliderType();
					if (deadRecord2.actorSubType == 2)
					{
						if (deadRecord2.actorSubSoliderType == 7)
						{
							deadRecord2.fightTime = (int)Singleton<FrameSynchr>.get_instance().LogicFrameTick - this.m_baojunEnterCombatTime;
						}
						else if (deadRecord2.actorSubSoliderType == 8)
						{
							deadRecord2.fightTime = (int)Singleton<FrameSynchr>.get_instance().LogicFrameTick - this.m_baronEnterCombatTime;
						}
						else if (deadRecord2.actorSubSoliderType == 9)
						{
							deadRecord2.fightTime = (int)Singleton<FrameSynchr>.get_instance().LogicFrameTick - this.m_bigDragonEnterCombatTime;
						}
					}
				}
				this.m_deadRecordList.Add(deadRecord2);
				this.m_deadMonsterNum++;
			}
			else if (src.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Organ && (src.get_handle().TheStaticData.TheOrganOnlyInfo.OrganType == 1 || src.get_handle().TheStaticData.TheOrganOnlyInfo.OrganType == 4 || src.get_handle().TheStaticData.TheOrganOnlyInfo.OrganType == 2))
			{
				DeadRecord deadRecord3 = new DeadRecord(src.get_handle().TheActorMeta.ActorCamp, src.get_handle().TheActorMeta.ActorType, src.get_handle().TheActorMeta.ConfigId, (int)Singleton<FrameSynchr>.get_instance().LogicFrameTick, logicAtker.get_handle().TheActorMeta.ActorCamp, logicAtker.get_handle().TheActorMeta.PlayerId, logicAtker.get_handle().TheActorMeta.ActorType);
				if (src.get_handle().ObjLinker != null)
				{
					deadRecord3.iOrder = src.get_handle().ObjLinker.BattleOrder;
					deadRecord3.actorSubType = (byte)src.get_handle().TheStaticData.TheOrganOnlyInfo.OrganType;
				}
				this.m_deadRecordList.Add(deadRecord3);
			}
		}

		public int GetOrganTimeByOrder(int iOrder)
		{
			for (int i = 0; i < this.m_deadRecordList.get_Count(); i++)
			{
				if (this.m_deadRecordList.get_Item(i).actorType == ActorTypeDef.Actor_Type_Organ && this.m_deadRecordList.get_Item(i).iOrder == iOrder)
				{
					return this.m_deadRecordList.get_Item(i).deadTime;
				}
			}
			return 0;
		}

		public int GetDeadTime(COM_PLAYERCAMP camp, ActorTypeDef actorType, int index)
		{
			int num = 0;
			for (int i = 0; i < this.m_deadRecordList.get_Count(); i++)
			{
				if (actorType == this.m_deadRecordList.get_Item(i).actorType && camp == this.m_deadRecordList.get_Item(i).camp)
				{
					if (num == index)
					{
						return this.m_deadRecordList.get_Item(i).deadTime;
					}
					num++;
				}
			}
			return 0;
		}

		public int GetDeadNum(COM_PLAYERCAMP camp, ActorTypeDef actorType, int subType, int cfgId)
		{
			int num = 0;
			for (int i = 0; i < this.m_deadRecordList.get_Count(); i++)
			{
				if (camp == this.m_deadRecordList.get_Item(i).camp && actorType == this.m_deadRecordList.get_Item(i).actorType && subType == (int)this.m_deadRecordList.get_Item(i).actorSubType && cfgId == this.m_deadRecordList.get_Item(i).cfgId)
				{
					num++;
				}
			}
			return num;
		}

		public int GetDragonDeadTime(int index)
		{
			int num = 0;
			for (int i = 0; i < this.m_deadRecordList.get_Count(); i++)
			{
				if (this.m_deadRecordList.get_Item(i).actorType == ActorTypeDef.Actor_Type_Monster && this.m_deadRecordList.get_Item(i).actorSubType == 2 && (this.m_deadRecordList.get_Item(i).actorSubSoliderType == 9 || this.m_deadRecordList.get_Item(i).actorSubSoliderType == 7))
				{
					if (num == index)
					{
						return this.m_deadRecordList.get_Item(i).deadTime;
					}
					num++;
				}
			}
			return 0;
		}

		public int GetKillDragonNum(COM_PLAYERCAMP killerCamp)
		{
			int num = 0;
			for (int i = 0; i < this.m_deadRecordList.get_Count(); i++)
			{
				if (killerCamp == this.m_deadRecordList.get_Item(i).killerCamp && this.m_deadRecordList.get_Item(i).actorType == ActorTypeDef.Actor_Type_Monster && this.m_deadRecordList.get_Item(i).actorSubType == 2 && (this.m_deadRecordList.get_Item(i).actorSubSoliderType == 9 || this.m_deadRecordList.get_Item(i).actorSubSoliderType == 7))
				{
					num++;
				}
			}
			return num;
		}

		public int GetKillDragonNum()
		{
			int num = 0;
			for (int i = 0; i < this.m_deadRecordList.get_Count(); i++)
			{
				if (this.m_deadRecordList.get_Item(i).actorType == ActorTypeDef.Actor_Type_Monster && this.m_deadRecordList.get_Item(i).actorSubType == 2 && (this.m_deadRecordList.get_Item(i).actorSubSoliderType == 9 || this.m_deadRecordList.get_Item(i).actorSubSoliderType == 7))
				{
					num++;
				}
			}
			return num;
		}

		public int GetAllMonsterDeadNum()
		{
			return this.m_deadMonsterNum;
		}

		public int GetHeroDeadAtTime(uint playerID, int deadTime)
		{
			int num = 0;
			List<DeadRecord> list = new List<DeadRecord>();
			for (int i = 0; i < this.m_deadRecordList.get_Count(); i++)
			{
				DeadRecord deadRecord = this.m_deadRecordList.get_Item(i);
				if (deadRecord.AttackPlayerID == playerID && deadRecord.actorType == ActorTypeDef.Actor_Type_Hero && deadRecord.deadTime < deadTime)
				{
					num++;
				}
			}
			return num;
		}

		public int GetMonsterDeadAtTime(uint playerID, int deadTime)
		{
			int num = 0;
			List<DeadRecord> list = new List<DeadRecord>();
			for (int i = 0; i < this.m_deadRecordList.get_Count(); i++)
			{
				DeadRecord deadRecord = this.m_deadRecordList.get_Item(i);
				if (deadRecord.AttackPlayerID == playerID && deadRecord.actorType == ActorTypeDef.Actor_Type_Monster && deadRecord.actorSubType == 2 && deadRecord.deadTime < deadTime)
				{
					num++;
				}
			}
			return num;
		}

		public int GetSoldierDeadAtTime(uint playerID, int deadTime)
		{
			int num = 0;
			List<DeadRecord> list = new List<DeadRecord>();
			for (int i = 0; i < this.m_deadRecordList.get_Count(); i++)
			{
				DeadRecord deadRecord = this.m_deadRecordList.get_Item(i);
				if (deadRecord.AttackPlayerID == playerID && deadRecord.actorType == ActorTypeDef.Actor_Type_Monster && deadRecord.actorSubType == 1 && deadRecord.deadTime < deadTime)
				{
					num++;
				}
			}
			return num;
		}

		public int GetKillDragonNumAtTime(uint playerID, int deadTime)
		{
			int num = 0;
			List<DeadRecord> list = new List<DeadRecord>();
			for (int i = 0; i < this.m_deadRecordList.get_Count(); i++)
			{
				DeadRecord deadRecord = this.m_deadRecordList.get_Item(i);
				if (deadRecord.AttackPlayerID == playerID && deadRecord.actorType == ActorTypeDef.Actor_Type_Monster && deadRecord.actorSubType == 2 && (deadRecord.actorSubSoliderType == 9 || deadRecord.actorSubSoliderType == 7) && deadRecord.deadTime < deadTime)
				{
					num++;
				}
			}
			return num;
		}

		public int GetKillSpecialMonsterNumAtTime(uint playerID, int deadTime, byte bySoldierType)
		{
			int num = 0;
			for (int i = 0; i < this.m_deadRecordList.get_Count(); i++)
			{
				DeadRecord deadRecord = this.m_deadRecordList.get_Item(i);
				if (deadRecord.AttackPlayerID == playerID && deadRecord.actorType == ActorTypeDef.Actor_Type_Monster && deadRecord.actorSubType == 2 && deadRecord.actorSubSoliderType == bySoldierType && deadRecord.deadTime < deadTime)
				{
					num++;
				}
			}
			return num;
		}

		public int GetDeadNumAtTime(COM_PLAYERCAMP camp, ActorTypeDef actorType, int subType, int deadTime)
		{
			int num = 0;
			for (int i = 0; i < this.m_deadRecordList.get_Count(); i++)
			{
				DeadRecord deadRecord = this.m_deadRecordList.get_Item(i);
				if (camp == deadRecord.camp && deadRecord.actorType == actorType && (int)deadRecord.actorSubType == subType && deadRecord.deadTime < deadTime)
				{
					num++;
				}
			}
			return num;
		}

		public int GetKillBlueBaNumAtTime(uint playerID, int deadTime)
		{
			return this.GetKillSpecialMonsterNumAtTime(playerID, deadTime, 10);
		}

		public int GetKillRedBaNumAtTime(uint playerID, int deadTime)
		{
			return this.GetKillSpecialMonsterNumAtTime(playerID, deadTime, 11);
		}

		public int GetBaronDeadCount()
		{
			int num = 0;
			for (int i = 0; i < this.m_deadRecordList.get_Count(); i++)
			{
				DeadRecord deadRecord = this.m_deadRecordList.get_Item(i);
				if (deadRecord.actorType == ActorTypeDef.Actor_Type_Monster && deadRecord.actorSubType == 2 && deadRecord.actorSubSoliderType == 8)
				{
					num++;
				}
			}
			return num;
		}

		public int GetBaronDeadCount(COM_PLAYERCAMP killerCamp)
		{
			int num = 0;
			for (int i = 0; i < this.m_deadRecordList.get_Count(); i++)
			{
				DeadRecord deadRecord = this.m_deadRecordList.get_Item(i);
				if (killerCamp == deadRecord.killerCamp && deadRecord.actorType == ActorTypeDef.Actor_Type_Monster && deadRecord.actorSubType == 2 && deadRecord.actorSubSoliderType == 8)
				{
					num++;
				}
			}
			return num;
		}

		public int GetBaronDeadTime(int index)
		{
			int num = 0;
			for (int i = 0; i < this.m_deadRecordList.get_Count(); i++)
			{
				DeadRecord deadRecord = this.m_deadRecordList.get_Item(i);
				if (deadRecord.actorType == ActorTypeDef.Actor_Type_Monster && deadRecord.actorSubType == 2 && deadRecord.actorSubSoliderType == 8)
				{
					if (num == index)
					{
						return this.m_deadRecordList.get_Item(i).deadTime;
					}
					num++;
				}
			}
			return 0;
		}

		public byte GetDestroyTowerCount(COM_PLAYERCAMP killerCamp, int TowerType)
		{
			byte b = 0;
			for (int i = 0; i < this.m_deadRecordList.get_Count(); i++)
			{
				DeadRecord deadRecord = this.m_deadRecordList.get_Item(i);
				if (killerCamp == deadRecord.killerCamp && deadRecord.actorType == ActorTypeDef.Actor_Type_Organ && (int)deadRecord.actorSubType == TowerType)
				{
					b += 1;
				}
			}
			return b;
		}

		public int GetTotalNum(COM_PLAYERCAMP camp, ActorTypeDef actorType, byte actorSubType, byte actorSubSoliderType)
		{
			int num = 0;
			for (int i = 0; i < this.m_deadRecordList.get_Count(); i++)
			{
				if (this.m_deadRecordList.get_Item(i).camp == camp && this.m_deadRecordList.get_Item(i).actorType == actorType && this.m_deadRecordList.get_Item(i).actorSubType == actorSubType && this.m_deadRecordList.get_Item(i).actorSubSoliderType == actorSubSoliderType)
				{
					num++;
				}
			}
			return num;
		}

		public DeadRecord GetRecordAtIndex(COM_PLAYERCAMP camp, ActorTypeDef actorType, byte actorSubType, byte actorSubSoliderType, int index)
		{
			int num = 0;
			for (int i = 0; i < this.m_deadRecordList.get_Count(); i++)
			{
				if (this.m_deadRecordList.get_Item(i).camp == camp && this.m_deadRecordList.get_Item(i).actorType == actorType && this.m_deadRecordList.get_Item(i).actorSubType == actorSubType && this.m_deadRecordList.get_Item(i).actorSubSoliderType == actorSubSoliderType)
				{
					if (num == index)
					{
						return this.m_deadRecordList.get_Item(i);
					}
					num++;
				}
			}
			return default(DeadRecord);
		}

		public int GetEnemyKillHeroMaxGap(COM_PLAYERCAMP camp)
		{
			int num = 0;
			int num2 = 0;
			this.enemyKillHeroMaxGap = 0;
			COM_PLAYERCAMP cOM_PLAYERCAMP = 1;
			if (camp == 1)
			{
				cOM_PLAYERCAMP = 2;
			}
			for (int i = 0; i < this.m_deadRecordList.get_Count(); i++)
			{
				if (this.m_deadRecordList.get_Item(i).camp == camp && this.m_deadRecordList.get_Item(i).actorType == ActorTypeDef.Actor_Type_Hero && this.m_deadRecordList.get_Item(i).killerActorType == ActorTypeDef.Actor_Type_Hero)
				{
					num2++;
				}
				else if (this.m_deadRecordList.get_Item(i).camp == cOM_PLAYERCAMP && this.m_deadRecordList.get_Item(i).actorType == ActorTypeDef.Actor_Type_Hero && this.m_deadRecordList.get_Item(i).killerActorType == ActorTypeDef.Actor_Type_Hero)
				{
					num++;
				}
				int num3 = num2 - num;
				if (num3 > this.enemyKillHeroMaxGap)
				{
					this.enemyKillHeroMaxGap = num3;
				}
			}
			return this.enemyKillHeroMaxGap;
		}

		public int GetActorAverageFightTime(COM_PLAYERCAMP camp, ActorTypeDef actorType, int cfgId)
		{
			int num = 0;
			int count = this.m_deadRecordList.get_Count();
			int num2 = 0;
			for (int i = 0; i < count; i++)
			{
				if (camp == this.m_deadRecordList.get_Item(i).camp && actorType == this.m_deadRecordList.get_Item(i).actorType && cfgId == this.m_deadRecordList.get_Item(i).cfgId)
				{
					num++;
					num2 += this.m_deadRecordList.get_Item(i).fightTime;
				}
			}
			return (num != 0) ? (num2 / num) : 0;
		}
	}
}
