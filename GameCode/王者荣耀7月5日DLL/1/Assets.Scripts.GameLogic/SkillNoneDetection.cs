using System;

namespace Assets.Scripts.GameLogic
{
	[SkillBaseDetection]
	public class SkillNoneDetection : SkillBaseDetection
	{
		public override bool Detection(SkillSlot slot)
		{
			return true;
		}
	}
}
