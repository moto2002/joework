using AGE;
using Assets.Scripts.Common;
using CSProtocol;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameLogic
{
	public class TriggerActionJungle : TriggerActionBase
	{
		public TriggerActionJungle(TriggerActionWrapper inWrapper, int inTriggerId) : base(inWrapper, inTriggerId)
		{
		}

		public override void Destroy()
		{
		}

		public override RefParamOperator TriggerEnter(PoolObjHandle<ActorRoot> src, PoolObjHandle<ActorRoot> atker, ITrigger inTrigger)
		{
			if (FogOfWar.enable)
			{
				return null;
			}
			ActorInGrassParam actorInGrassParam = new ActorInGrassParam(src, true);
			Singleton<GameEventSys>.get_instance().SendEvent<ActorInGrassParam>(GameEventDef.Event_ActorInGrass, ref actorInGrassParam);
			this.ModifyHorizonMarks(src, inTrigger, true);
			return null;
		}

		public override void TriggerUpdate(PoolObjHandle<ActorRoot> src, PoolObjHandle<ActorRoot> atker, ITrigger inTrigger)
		{
		}

		public override void TriggerLeave(PoolObjHandle<ActorRoot> src, ITrigger inTrigger)
		{
			if (FogOfWar.enable)
			{
				return;
			}
			ActorInGrassParam actorInGrassParam = new ActorInGrassParam(src, false);
			Singleton<GameEventSys>.get_instance().SendEvent<ActorInGrassParam>(GameEventDef.Event_ActorInGrass, ref actorInGrassParam);
			this.ModifyHorizonMarks(src, inTrigger, false);
		}

		private void ModifyHorizonMarks(PoolObjHandle<ActorRoot> src, ITrigger inTrigger, bool enterOrLeave)
		{
			if (src)
			{
				int num = (!enterOrLeave) ? -1 : 1;
				AreaEventTrigger areaEventTrigger = inTrigger as AreaEventTrigger;
				List<PoolObjHandle<ActorRoot>> actors = areaEventTrigger.GetActors((PoolObjHandle<ActorRoot> enr) => enr.get_handle().TheActorMeta.ActorCamp != src.get_handle().TheActorMeta.ActorCamp);
				for (int i = 0; i < actors.get_Count(); i++)
				{
					PoolObjHandle<ActorRoot> poolObjHandle = actors.get_Item(i);
					poolObjHandle.get_handle().HorizonMarker.AddShowMark(src.get_handle().TheActorMeta.ActorCamp, HorizonConfig.ShowMark.Jungle, num * 1);
					src.get_handle().HorizonMarker.AddShowMark(poolObjHandle.get_handle().TheActorMeta.ActorCamp, HorizonConfig.ShowMark.Jungle, num * 1);
				}
				COM_PLAYERCAMP[] othersCmp = BattleLogic.GetOthersCmp(src.get_handle().TheActorMeta.ActorCamp);
				for (int j = 0; j < othersCmp.Length; j++)
				{
					src.get_handle().HorizonMarker.AddHideMark(othersCmp[j], HorizonConfig.HideMark.Jungle, num * 1, false);
				}
			}
		}
	}
}
