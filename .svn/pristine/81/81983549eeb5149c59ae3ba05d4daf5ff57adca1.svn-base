using Assets.Scripts.Common;
using System;

namespace Assets.Scripts.GameLogic
{
	[Energy(EnergyType.MagicResource)]
	public class Magic : BaseEnergyLogic
	{
		public override void Init(PoolObjHandle<ActorRoot> _actor)
		{
			this.energyType = EnergyType.MagicResource;
			base.Init(_actor);
		}

		public override void ResetEpValue(int epPercent)
		{
			this._actorEp = this.actor.get_handle().ValueComponent.mActorValue[32].totalValue * epPercent / 10000;
		}

		protected override void UpdateEpValue()
		{
			this._actorEp += this.actor.get_handle().ValueComponent.mActorValue[33].totalValue / 5;
		}
	}
}
