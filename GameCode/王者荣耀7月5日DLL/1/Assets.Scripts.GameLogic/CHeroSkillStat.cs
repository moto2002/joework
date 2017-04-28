using Assets.Scripts.Framework;
using System;

namespace Assets.Scripts.GameLogic
{
	public class CHeroSkillStat
	{
		public void StartRecord()
		{
			this.Clear();
			Singleton<GameSkillEventSys>.GetInstance().AddEventHandler<BuffChangeEventParam>(GameSkillEventDef.AllEvent_BuffChange, new GameSkillEvent<BuffChangeEventParam>(this.OnActorBuffSkillChange));
			Singleton<GameSkillEventSys>.GetInstance().AddEventHandler<SpawnEyeEventParam>(GameSkillEventDef.Event_SpawnEye, new GameSkillEvent<SpawnEyeEventParam>(this.OnActorSpawnEye));
			Singleton<GameSkillEventSys>.GetInstance().AddEventHandler<DefaultSkillEventParam>(GameSkillEventDef.Event_UpdateSkillUI, new GameSkillEvent<DefaultSkillEventParam>(this.OnActorChangeSkill));
		}

		public void Clear()
		{
			Singleton<GameSkillEventSys>.GetInstance().RmvEventHandler<BuffChangeEventParam>(GameSkillEventDef.AllEvent_BuffChange, new GameSkillEvent<BuffChangeEventParam>(this.OnActorBuffSkillChange));
			Singleton<GameSkillEventSys>.GetInstance().RmvEventHandler<SpawnEyeEventParam>(GameSkillEventDef.Event_SpawnEye, new GameSkillEvent<SpawnEyeEventParam>(this.OnActorSpawnEye));
			Singleton<GameSkillEventSys>.GetInstance().RmvEventHandler<DefaultSkillEventParam>(GameSkillEventDef.Event_UpdateSkillUI, new GameSkillEvent<DefaultSkillEventParam>(this.OnActorChangeSkill));
		}

		private void OnActorBuffSkillChange(ref BuffChangeEventParam prm)
		{
			if (prm.bIsAdd)
			{
				return;
			}
			if (!prm.stBuffSkill || prm.stBuffSkill.get_handle().skillContext == null)
			{
				return;
			}
			if (!prm.stBuffSkill.get_handle().skillContext.Originator || !prm.stBuffSkill.get_handle().skillContext.TargetActor)
			{
				return;
			}
			if (prm.stBuffSkill.get_handle().skillContext.SlotType < SkillSlotType.SLOT_SKILL_1 || prm.stBuffSkill.get_handle().skillContext.SlotType >= SkillSlotType.SLOT_SKILL_COUNT)
			{
				return;
			}
			if (prm.stBuffSkill.get_handle().cfgData.bEffectType != 2)
			{
				return;
			}
			if (prm.stBuffSkill.get_handle().cfgData.bShowType != 1 && prm.stBuffSkill.get_handle().cfgData.bShowType != 3 && prm.stBuffSkill.get_handle().cfgData.bShowType != 4 && prm.stBuffSkill.get_handle().cfgData.bShowType != 5 && prm.stBuffSkill.get_handle().cfgData.bShowType != 6)
			{
				return;
			}
			ulong num = Singleton<FrameSynchr>.GetInstance().LogicFrameTick - prm.stBuffSkill.get_handle().ulStartTime;
			if (prm.stBuffSkill.get_handle().skillContext.Originator.get_handle().SkillControl != null)
			{
				prm.stBuffSkill.get_handle().skillContext.Originator.get_handle().SkillControl.stSkillStat.m_uiStunTime += (uint)num;
			}
			if (prm.stBuffSkill.get_handle().skillContext.TargetActor.get_handle().SkillControl != null)
			{
				prm.stBuffSkill.get_handle().skillContext.TargetActor.get_handle().SkillControl.stSkillStat.m_uiBeStunnedTime += (uint)num;
			}
		}

		private void OnActorSpawnEye(ref SpawnEyeEventParam prm)
		{
			if (!prm.src || prm.src.get_handle().SkillControl == null || prm.src.get_handle().SkillControl.stSkillStat == null)
			{
				return;
			}
			prm.src.get_handle().SkillControl.stSkillStat.m_uiRealSpawnEyeTimes += 1u;
			if (prm.src.get_handle().SkillControl.stSkillStat.m_uiSpawnEyeTimes < 15u)
			{
				prm.src.get_handle().SkillControl.stSkillStat.stEyePostion[(int)((UIntPtr)prm.src.get_handle().SkillControl.stSkillStat.m_uiSpawnEyeTimes)] = prm.pos;
				prm.src.get_handle().SkillControl.stSkillStat.m_uiSpawnEyeTimes += 1u;
			}
		}

		private void OnActorChangeSkill(ref DefaultSkillEventParam prm)
		{
			if (!prm.actor || prm.actor.get_handle().SkillControl == null || prm.actor.get_handle().SkillControl.stSkillStat == null)
			{
				return;
			}
			if (prm.slot == SkillSlotType.SLOT_SKILL_7)
			{
				prm.actor.get_handle().SkillControl.stSkillStat.m_uiEyeSwitchTimes += 1u;
			}
		}
	}
}
