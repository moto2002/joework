using Assets.Scripts.GameLogic;
using ResData;
using System;

namespace behaviac
{
	internal class Condition_bt_WrapperAI_Hero_HeroCommonAutoAI_node932 : Condition
	{
		private SkillSlotType opl_p0;

		public Condition_bt_WrapperAI_Hero_HeroCommonAutoAI_node932()
		{
			this.opl_p0 = SkillSlotType.SLOT_SKILL_6;
		}

		protected override EBTStatus update_impl(Agent pAgent, EBTStatus childStatus)
		{
			RES_SUMMONERSKILL_TYPE summonerSkillType = ((ObjAgent)pAgent).GetSummonerSkillType(this.opl_p0);
			RES_SUMMONERSKILL_TYPE rES_SUMMONERSKILL_TYPE = 1;
			bool flag = summonerSkillType == rES_SUMMONERSKILL_TYPE;
			return (!flag) ? EBTStatus.BT_FAILURE : EBTStatus.BT_SUCCESS;
		}
	}
}
