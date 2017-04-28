using Assets.Scripts.Common;
using Assets.Scripts.GameSystem;
using ResData;
using System;

namespace Assets.Scripts.GameLogic
{
	[PassiveCondition]
	public class ActorInGrassCondition : PassiveCondition
	{
		private bool bTrigger;

		public override void Init(PoolObjHandle<ActorRoot> _source, PassiveEvent _event, ref ResDT_SkillPassiveCondition _config)
		{
			this.bTrigger = false;
			base.Init(_source, _event, ref _config);
			Singleton<GameEventSys>.get_instance().AddEventHandler<ActorInGrassParam>(GameEventDef.Event_ActorInGrass, new RefAction<ActorInGrassParam>(this.SetInGrassStatusByTrigger));
		}

		public override void UnInit()
		{
			base.UnInit();
			Singleton<GameEventSys>.get_instance().RmvEventHandler<ActorInGrassParam>(GameEventDef.Event_ActorInGrass, new RefAction<ActorInGrassParam>(this.SetInGrassStatusByTrigger));
		}

		public override void Reset()
		{
		}

		public void SetInGrassStatusByFog()
		{
			if (Singleton<GameFowManager>.get_instance().m_pFieldObj != null)
			{
				this.bTrigger = (Singleton<GameFowManager>.get_instance().QueryAttr(this.sourceActor.get_handle().location) == FieldObj.EViewBlockType.Grass);
			}
		}

		public void SetInGrassStatusByTrigger(ref ActorInGrassParam param)
		{
			if (param._src == this.sourceActor)
			{
				this.bTrigger = param._bInGrass;
			}
		}

		public override bool Fit()
		{
			if (FogOfWar.enable)
			{
				this.SetInGrassStatusByFog();
			}
			return this.bTrigger;
		}
	}
}
