using Assets.Scripts.GameLogic;
using System;

namespace behaviac
{
	internal class Condition_bt_WrapperAI_Hero_HeroCommonAutoAI_node24 : Condition
	{
		protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
		{
			ObjBehaviMode curBehavior = ((ObjAgent)pAgent).GetCurBehavior();
			ObjBehaviMode objBehaviMode = ObjBehaviMode.UseSkill_3;
			bool flag = curBehavior == objBehaviMode;
			return (!flag) ? EBTStatus.BT_FAILURE : EBTStatus.BT_SUCCESS;
		}
	}
}
