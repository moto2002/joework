using Assets.Scripts.GameLogic;
using ResData;
using System;

namespace behaviac
{
	internal class Condition_bt_WrapperAI_Monster_BTMonsterPassive_zhouwang_node42 : Condition
	{
		protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
		{
			SkillSlotType inSlot = (SkillSlotType)((int)pAgent.GetVariable(7107675u));
			SkillTargetRule skillTargetRule = ((ObjAgent)pAgent).GetSkillTargetRule(inSlot);
			SkillTargetRule skillTargetRule2 = 1;
			bool flag = skillTargetRule == skillTargetRule2;
			return (!flag) ? EBTStatus.BT_FAILURE : EBTStatus.BT_SUCCESS;
		}
	}
}
