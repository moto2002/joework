using Assets.Scripts.GameLogic;
using System;
using UnityEngine;

namespace behaviac
{
	internal class Condition_bt_WrapperAI_Hero_HeroWarmSimpleAI_node407 : Condition
	{
		private int opl_p1;

		public Condition_bt_WrapperAI_Hero_HeroWarmSimpleAI_node407()
		{
			this.opl_p1 = 600;
		}

		protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
		{
			Vector3 aimPos = (Vector3)pAgent.GetVariable(312907864u);
			bool flag = ((ObjAgent)pAgent).IsDistanceToPosLessThanRange(aimPos, this.opl_p1);
			bool flag2 = true;
			bool flag3 = flag == flag2;
			return (!flag3) ? EBTStatus.BT_FAILURE : EBTStatus.BT_SUCCESS;
		}
	}
}
