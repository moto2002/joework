using System;

namespace Assets.Scripts.GameLogic
{
	[SkillBaseDetection]
	public class SkillSelectNearestEnermyDetection : SkillBaseDetection
	{
		public override bool Detection(SkillSlot slot)
		{
			int srchR;
			if (slot.SkillObj.AppointType == 1)
			{
				srchR = slot.SkillObj.GetMaxSearchDistance(slot.GetSkillLevel());
			}
			else
			{
				srchR = slot.SkillObj.cfgData.iMaxAttackDistance;
			}
			return Singleton<TargetSearcher>.GetInstance().GetNearestEnemy(slot.Actor.get_handle(), srchR, slot.SkillObj.cfgData.dwSkillTargetFilter, true) != null;
		}
	}
}
