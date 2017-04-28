using Assets.Scripts.GameLogic;
using Assets.Scripts.GameLogic.GameKernal;
using ResData;
using System;

namespace Assets.Scripts.GameSystem
{
	[VoiceInteraction(0)]
	public class VoiceInteractionKill : VoiceInteraction
	{
		public override void Init(ResVoiceInteraction InInteractionCfg)
		{
			base.Init(InInteractionCfg);
			Singleton<GameEventSys>.get_instance().AddEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.onActorDead));
		}

		public override void Unit()
		{
			Singleton<GameEventSys>.get_instance().RmvEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.onActorDead));
			base.Unit();
		}

		private void onActorDead(ref GameDeadEventParam prm)
		{
			if (!this.ForwardCheck())
			{
				return;
			}
			if (prm.orignalAtker && prm.orignalAtker.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero && prm.orignalAtker.get_handle().TheActorMeta.ConfigId == base.groupID && prm.src && prm.src.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero && this.CheckTriggerDistance(ref prm.orignalAtker, ref prm.src) && base.ValidateTriggerActor(ref prm.src))
			{
				Player hostPlayer = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer();
				if (hostPlayer != null && hostPlayer.Captain && this.CheckReceiveDistance(ref hostPlayer.Captain, ref prm.src))
				{
					this.TryTrigger(ref prm.orignalAtker, ref prm.src, ref prm.orignalAtker);
				}
			}
		}
	}
}
