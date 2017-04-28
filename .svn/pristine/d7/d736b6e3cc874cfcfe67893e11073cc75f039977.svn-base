using Assets.Scripts.Common;
using System;

namespace Assets.Scripts.GameLogic
{
	public class CBattleValueAdjust
	{
		private bool m_bSoulGrow;

		public void Init()
		{
			this.UnInit();
			this.m_bSoulGrow = Singleton<BattleLogic>.GetInstance().m_LevelContext.IsSoulGrow();
			Singleton<GameEventSys>.get_instance().AddEventHandler<PoolObjHandle<ActorRoot>>(GameEventDef.Event_ActorStartFight, new RefAction<PoolObjHandle<ActorRoot>>(this.onActorStartFight));
		}

		public void UnInit()
		{
			Singleton<GameEventSys>.get_instance().RmvEventHandler<PoolObjHandle<ActorRoot>>(GameEventDef.Event_ActorStartFight, new RefAction<PoolObjHandle<ActorRoot>>(this.onActorStartFight));
		}

		private void onActorStartFight(ref PoolObjHandle<ActorRoot> src)
		{
			if (!src)
			{
				return;
			}
			if (this.m_bSoulGrow && src.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero && src.get_handle().ValueComponent != null)
			{
				SLevelContext curLvelContext = Singleton<BattleLogic>.get_instance().GetCurLvelContext();
				if (curLvelContext != null && curLvelContext.m_birthLevelConfig > 0)
				{
					src.get_handle().ValueComponent.ForceSetSoulLevel(curLvelContext.m_birthLevelConfig);
					src.get_handle().ValueComponent.SetHpAndEpToInitialValue(10000, 10000);
				}
				else
				{
					src.get_handle().ValueComponent.actorSoulLevel = 1;
				}
				if (CheatCommandReplayEntry.heroPerformanceTest)
				{
					src.get_handle().ValueComponent.actorSoulLevel = 4;
					Singleton<BattleLogic>.GetInstance().AutoLearnSkill(src);
				}
			}
		}

		public static void SetPropValue(ValueDataInfo info, int balanceVal)
		{
			info.baseValue = balanceVal;
			info.growValue = 0;
			info.addValue = 0;
			info.decValue = 0;
			info.addRatio = 0;
			info.decRatio = 0;
		}
	}
}
