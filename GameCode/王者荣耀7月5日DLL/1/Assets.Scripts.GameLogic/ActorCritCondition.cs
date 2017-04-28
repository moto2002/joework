using Assets.Scripts.Common;
using ResData;
using System;

namespace Assets.Scripts.GameLogic
{
	[PassiveCondition]
	public class ActorCritCondition : PassiveCondition
	{
		private bool bTrigger;

		public override void Init(PoolObjHandle<ActorRoot> _source, PassiveEvent _event, ref ResDT_SkillPassiveCondition _config)
		{
			this.bTrigger = false;
			base.Init(_source, _event, ref _config);
			Singleton<GameEventSys>.get_instance().AddEventHandler<DefaultGameEventParam>(GameEventDef.Event_ActorCrit, new RefAction<DefaultGameEventParam>(this.onActorCrit));
		}

		public override void UnInit()
		{
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_ActorCrit, new RefAction<DefaultGameEventParam>(this.onActorCrit));
		}

		public override void Reset()
		{
			this.bTrigger = false;
		}

		private void onActorCrit(ref DefaultGameEventParam _prm)
		{
			if (_prm.src != this.sourceActor)
			{
				return;
			}
			this.bTrigger = true;
		}

		public override bool Fit()
		{
			return this.bTrigger;
		}
	}
}
