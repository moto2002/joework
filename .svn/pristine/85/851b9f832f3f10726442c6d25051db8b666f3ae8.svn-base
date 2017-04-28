using System;

namespace Assets.Scripts.GameLogic
{
	[SkillBaseSelectTarget]
	public class SkillSelectMyself : SkillBaseSelectTarget
	{
		public override ActorRoot SelectTarget(SkillSlot UseSlot)
		{
			if (UseSlot.SlotType == SkillSlotType.SLOT_SKILL_4)
			{
				SkillChooseTargetEventParam skillChooseTargetEventParam = new SkillChooseTargetEventParam(UseSlot.Actor, UseSlot.Actor, 1);
				Singleton<GameEventSys>.get_instance().SendEvent<SkillChooseTargetEventParam>(GameEventDef.Event_ActorBeChosenAsTarget, ref skillChooseTargetEventParam);
			}
			return UseSlot.Actor.get_handle();
		}

		public override VInt3 SelectTargetDir(SkillSlot UseSlot)
		{
			return UseSlot.Actor.get_handle().forward;
		}
	}
}
