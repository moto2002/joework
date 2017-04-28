using Assets.Scripts.Common;
using ResData;
using System;

namespace Assets.Scripts.GameLogic
{
	[PassiveCondition]
	public class ActorUseSkillCondition : PassiveCondition
	{
		private bool bTrigger;

		public override void Init(PoolObjHandle<ActorRoot> _source, PassiveEvent _event, ref ResDT_SkillPassiveCondition _config)
		{
			this.bTrigger = false;
			base.Init(_source, _event, ref _config);
			Singleton<GameSkillEventSys>.get_instance().AddEventHandler<ActorSkillEventParam>(GameSkillEventDef.AllEvent_UseSkill, new GameSkillEvent<ActorSkillEventParam>(this.onActorUseSkill));
		}

		public override void UnInit()
		{
			Singleton<GameSkillEventSys>.get_instance().RmvEventHandler<ActorSkillEventParam>(GameSkillEventDef.AllEvent_UseSkill, new GameSkillEvent<ActorSkillEventParam>(this.onActorUseSkill));
		}

		private void onActorUseSkill(ref ActorSkillEventParam _prm)
		{
			if (_prm.src != this.sourceActor)
			{
				return;
			}
			if (this.localParams[0] == 0)
			{
				this.bTrigger = true;
			}
			else if ((this.localParams[0] & 1 << (int)_prm.slot) > 0)
			{
				this.bTrigger = true;
			}
		}

		public override void Reset()
		{
			this.bTrigger = false;
		}

		public override bool Fit()
		{
			return this.bTrigger;
		}
	}
}
