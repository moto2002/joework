using System;

namespace Assets.Scripts.GameLogic
{
	public struct ChangeSkillEventParam
	{
		public SkillSlotType slot;

		public int skillID;

		public int changeTime;

		public ChangeSkillEventParam(SkillSlotType _slot, int _id, int _time)
		{
			this.slot = _slot;
			this.skillID = _id;
			this.changeTime = _time;
		}
	}
}
