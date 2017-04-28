using AGE;
using Assets.Scripts.Common;
using Assets.Scripts.GameSystem;
using CSProtocol;
using System;

namespace Assets.Scripts.GameLogic
{
	public class TriggerActionShenFu : TriggerActionBase
	{
		public TriggerActionShenFu(TriggerActionWrapper inWrapper, int inTriggerId) : base(inWrapper, inTriggerId)
		{
		}

		public override void TriggerUpdate(PoolObjHandle<ActorRoot> src, PoolObjHandle<ActorRoot> atker, ITrigger inTrigger)
		{
			Singleton<ShenFuSystem>.get_instance().OnShenFuEffect(src, (uint)this.UpdateUniqueId, (AreaEventTrigger)inTrigger, this);
			if (src)
			{
				Singleton<EventRouter>.get_instance().BroadCastEvent<COM_PLAYERCAMP, uint, uint>(EventID.BATTLE_SHENFU_EFFECT_CHANGED, src.get_handle().TheActorMeta.ActorCamp, src.get_handle().TheActorMeta.PlayerId, (uint)((AreaEventTrigger)inTrigger).ID);
			}
		}

		public override void OnCoolDown(ITrigger inTrigger)
		{
			Singleton<ShenFuSystem>.get_instance().OnShenfuHalt((uint)this.UpdateUniqueId, (AreaEventTrigger)inTrigger, this);
		}

		public override void OnTriggerStart(ITrigger inTrigger)
		{
			Singleton<ShenFuSystem>.get_instance().OnShenfuStart((uint)this.UpdateUniqueId, (AreaEventTrigger)inTrigger, this);
		}

		public override RefParamOperator TriggerEnter(PoolObjHandle<ActorRoot> src, PoolObjHandle<ActorRoot> atker, ITrigger inTrigger)
		{
			return null;
		}

		public override void Stop()
		{
			Singleton<ShenFuSystem>.get_instance().OnShenFuStopped(this);
		}
	}
}
