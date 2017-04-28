using System;

namespace Assets.Scripts.GameLogic
{
	[SkillBaseSelectTarget]
	public class SkillSelectMyDirection : SkillBaseSelectTarget
	{
		public override ActorRoot SelectTarget(SkillSlot UseSlot)
		{
			return Singleton<TargetSearcher>.GetInstance().GetNearestEnemy(UseSlot.Actor, UseSlot.SkillObj.GetMaxSearchDistance(UseSlot.GetSkillLevel()), TargetPriority.TargetPriority_Hero, UseSlot.SkillObj.cfgData.dwSkillTargetFilter, true);
		}

		public override VInt3 SelectTargetDir(SkillSlot UseSlot)
		{
			return UseSlot.Actor.get_handle().forward;
		}
	}
}
