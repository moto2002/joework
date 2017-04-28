using System;

namespace Assets.Scripts.GameLogic
{
	[PassiveEvent]
	public class PassiveLifeRandomEvent : PassiveRandomEvent
	{
		public override void UpdateLogic(int _delta)
		{
			if (this.sourceActor && !this.sourceActor.get_handle().ActorControl.IsDeadState)
			{
				base.UpdateLogic(_delta);
			}
		}
	}
}
