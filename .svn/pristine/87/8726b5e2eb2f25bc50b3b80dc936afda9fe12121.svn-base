using Assets.Scripts.Common;
using ResData;
using System;

namespace Assets.Scripts.GameLogic
{
	[PassiveCondition]
	public class PassiveHpCondition : PassiveCondition
	{
		private bool bHpTrigger;

		public override void Init(PoolObjHandle<ActorRoot> _source, PassiveEvent _event, ref ResDT_SkillPassiveCondition _config)
		{
			this.bHpTrigger = false;
			base.Init(_source, _event, ref _config);
			if (_source)
			{
				_source.get_handle().ValueComponent.HpChgEvent += new ValueChangeDelegate(this.OnHpChange);
			}
		}

		public override void UnInit()
		{
			if (this.sourceActor)
			{
				this.sourceActor.get_handle().ValueComponent.HpChgEvent -= new ValueChangeDelegate(this.OnHpChange);
			}
		}

		public override void Reset()
		{
			this.bHpTrigger = false;
		}

		private void OnHpChange()
		{
			int actorHp = this.sourceActor.get_handle().ValueComponent.actorHp;
			int totalValue = this.sourceActor.get_handle().ValueComponent.mActorValue[5].totalValue;
			int num = actorHp * 10000 / totalValue;
			this.bHpTrigger = false;
			if (this.localParams[0] == 1)
			{
				if (num <= this.localParams[1])
				{
					this.bHpTrigger = true;
				}
			}
			else if (this.localParams[0] == 4 && num >= this.localParams[1])
			{
				this.bHpTrigger = true;
			}
		}

		public override bool Fit()
		{
			return this.bHpTrigger;
		}
	}
}
