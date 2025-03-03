using Assets.Scripts.GameLogic;
using System;

namespace behaviac
{
	internal class Condition_bt_WrapperAI_Hero_HeroCommonAutoAI_node1007 : Condition
	{
		protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
		{
			uint objID = (uint)pAgent.GetVariable(1128863647u);
			ActorTypeDef actorType = ((ObjAgent)pAgent).GetActorType(objID);
			ActorTypeDef actorTypeDef = ActorTypeDef.Actor_Type_Hero;
			bool flag = actorType != actorTypeDef;
			return (!flag) ? EBTStatus.BT_FAILURE : EBTStatus.BT_SUCCESS;
		}
	}
}
