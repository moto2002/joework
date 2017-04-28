using Assets.Scripts.Common;
using ResData;
using System;

namespace Assets.Scripts.GameLogic
{
	[PassiveCondition]
	public class ExposeCondition : PassiveCondition
	{
		private bool bTrigger;

		public override void Init(PoolObjHandle<ActorRoot> _source, PassiveEvent _event, ref ResDT_SkillPassiveCondition _config)
		{
			this.bTrigger = false;
			base.Init(_source, _event, ref _config);
			Singleton<GameEventSys>.get_instance().AddEventHandler<DefaultGameEventParam>(GameEventDef.Event_ActorBeAttack, new RefAction<DefaultGameEventParam>(this.Expose));
		}

		public override void UnInit()
		{
			base.UnInit();
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_ActorBeAttack, new RefAction<DefaultGameEventParam>(this.Expose));
		}

		public override void Reset()
		{
			this.bTrigger = false;
		}

		public void Expose(ref DefaultGameEventParam param)
		{
			if (param.atker == this.sourceActor)
			{
				this.bTrigger = true;
			}
		}

		public override bool Fit()
		{
			return this.bTrigger;
		}
	}
}
