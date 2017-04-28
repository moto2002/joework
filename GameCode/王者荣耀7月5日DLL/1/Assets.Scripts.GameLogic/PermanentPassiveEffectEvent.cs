using Assets.Scripts.Common;
using System;

namespace Assets.Scripts.GameLogic
{
	[PassiveEvent]
	public class PermanentPassiveEffectEvent : PassiveEvent
	{
		private bool bTriggerFlag;

		public override void Init(PoolObjHandle<ActorRoot> _actor, PassiveSkill _skill)
		{
			this.bTriggerFlag = false;
			base.Init(_actor, _skill);
		}

		private void RemoveSkillEffect()
		{
			PoolObjHandle<ActorRoot> poolObjHandle;
			if (this.triggerActor)
			{
				poolObjHandle = this.triggerActor;
			}
			else
			{
				poolObjHandle = this.sourceActor;
			}
			if (poolObjHandle)
			{
				if (this.localParams[0] != 0)
				{
					poolObjHandle.get_handle().BuffHolderComp.RemoveBuff(this.localParams[0]);
				}
				else if (this.localParams[1] != 0)
				{
					poolObjHandle.get_handle().BuffHolderComp.RemoveBuff(this.localParams[1]);
				}
			}
		}

		public override void UpdateLogic(int _delta)
		{
			base.UpdateLogic(_delta);
			if (base.Fit() && !this.sourceActor.get_handle().ActorControl.IsDeadState)
			{
				if (!this.bTriggerFlag)
				{
					base.Trigger();
					this.bTriggerFlag = true;
				}
			}
			else if (this.bTriggerFlag || this.sourceActor.get_handle().ActorControl.IsDeadState)
			{
				this.bTriggerFlag = false;
				this.RemoveSkillEffect();
			}
		}
	}
}
