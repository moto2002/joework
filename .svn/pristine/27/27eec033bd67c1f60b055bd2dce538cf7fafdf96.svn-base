using Assets.Scripts.GameLogic;
using System;

namespace behaviac
{
	internal class Assignment_bt_WrapperAI_Soldier_BTSoldierSiege_node107 : Assignment
	{
		protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
		{
			EBTStatus result = EBTStatus.BT_SUCCESS;
			uint objID = (uint)pAgent.GetVariable(1128863647u);
			int srchR = (int)pAgent.GetVariable(2451377514u);
			uint newTargetByPriorityForSiege = ((ObjAgent)pAgent).GetNewTargetByPriorityForSiege(objID, srchR);
			pAgent.SetVariable<uint>("p_tempTargetId", newTargetByPriorityForSiege, 2303639248u);
			return result;
		}
	}
}
