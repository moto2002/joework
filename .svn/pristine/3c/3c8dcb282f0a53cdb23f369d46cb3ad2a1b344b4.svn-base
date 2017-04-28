using System;

namespace Assets.Scripts.GameLogic
{
	public static class SelectTargetHelper
	{
		public static ActorRoot GetTarget(SkillSlot UseSlot)
		{
			bool flag = Singleton<SkillSelectControl>.GetInstance().IsLowerHpMode();
			int srchR;
			if (UseSlot.SkillObj.AppointType == 1)
			{
				srchR = UseSlot.SkillObj.GetMaxSearchDistance(UseSlot.GetSkillLevel());
			}
			else
			{
				srchR = UseSlot.SkillObj.cfgData.iMaxAttackDistance;
			}
			ActorRoot result;
			if (flag)
			{
				result = Singleton<TargetSearcher>.GetInstance().GetLowestHpTarget(UseSlot.Actor.get_handle(), srchR, TargetPriority.TargetPriority_Hero, UseSlot.SkillObj.cfgData.dwSkillTargetFilter, true, true);
			}
			else
			{
				result = Singleton<TargetSearcher>.GetInstance().GetNearestEnemy(UseSlot.Actor.get_handle(), srchR, TargetPriority.TargetPriority_Hero, UseSlot.SkillObj.cfgData.dwSkillTargetFilter, true);
			}
			return result;
		}
	}
}
