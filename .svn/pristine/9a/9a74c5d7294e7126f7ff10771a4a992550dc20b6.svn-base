using Assets.Scripts.Common;
using System;

namespace Assets.Scripts.GameLogic
{
	[PassiveEvent]
	public class DeadHurtPassiveEvent : PassiveEvent
	{
		private bool bExistBuff;

		public override void Init(PoolObjHandle<ActorRoot> _actor, PassiveSkill _skill)
		{
			this.deltaTime = 0;
			this.bExistBuff = false;
			base.Init(_actor, _skill);
		}

		private void SpawnSkillEffect(int _skillCombineID)
		{
			if (this.sourceActor)
			{
				SkillUseParam skillUseParam = default(SkillUseParam);
				skillUseParam.Init();
				skillUseParam.SetOriginator(this.sourceActor);
				this.sourceActor.get_handle().SkillControl.SpawnBuff(this.sourceActor, ref skillUseParam, _skillCombineID, true);
			}
		}

		public override void UpdateLogic(int _delta)
		{
			if (!this.bExistBuff)
			{
				base.UpdateLogic(_delta);
				if (this.deltaTime <= 0)
				{
					base.Trigger();
					base.Reset();
					this.bExistBuff = true;
					this.deltaTime = 0;
				}
			}
			for (int i = 0; i < this.conditions.get_Count(); i++)
			{
				PassiveCondition passiveCondition = this.conditions.get_Item(i);
				if (passiveCondition.Fit() && this.bExistBuff)
				{
					base.Reset();
					this.bExistBuff = false;
					this.deltaTime = this.cfgData.iCoolDown;
					this.SpawnSkillEffect(this.localParams[0]);
					return;
				}
			}
		}
	}
}
