using Assets.Scripts.Common;
using System;

namespace Assets.Scripts.GameLogic
{
	[Energy(EnergyType.MadnessResource)]
	public class Madness : BaseEnergyLogic
	{
		public override void Init(PoolObjHandle<ActorRoot> _actor)
		{
			this.energyType = EnergyType.MadnessResource;
			base.Init(_actor);
		}

		protected override void UpdateEpValue()
		{
			this._actorEp += this.actor.get_handle().ValueComponent.mActorValue[33].totalValue / 5;
		}
	}
}
