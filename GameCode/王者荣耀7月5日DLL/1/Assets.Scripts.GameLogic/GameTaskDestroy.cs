using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using ResData;
using System;

namespace Assets.Scripts.GameLogic
{
	public class GameTaskDestroy : GameTask
	{
		protected RES_BATTLE_TASK_SUBJECT SubjectType
		{
			get
			{
				return base.Config.iParam1;
			}
		}

		protected int SubjectID
		{
			get
			{
				return base.Config.iParam2;
			}
		}

		public override float Progress
		{
			get
			{
				if (this.SubjectType == 1 || this.SubjectType == 2)
				{
					PoolObjHandle<ActorRoot> actor = Singleton<GameObjMgr>.get_instance().GetActor(new ActorFilterDelegate(this.FilterTargetActor));
					if (actor)
					{
						return base.Progress + (1f - (float)actor.get_handle().ValueComponent.actorHp / (float)actor.get_handle().ValueComponent.actorHpTotal) / (float)this.Target;
					}
				}
				return base.Progress;
			}
		}

		protected override void OnInitial()
		{
		}

		protected override void OnDestroy()
		{
			Singleton<GameEventSys>.get_instance().RmvEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.onActorDead));
		}

		protected override void OnStart()
		{
			Singleton<GameEventSys>.get_instance().AddEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.onActorDead));
		}

		protected override void OnClose()
		{
			Singleton<GameEventSys>.get_instance().RmvEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.onActorDead));
		}

		private bool FilterTargetActor(ref PoolObjHandle<ActorRoot> acr)
		{
			bool flag = true;
			if (this.SubjectType == 1)
			{
				flag &= (acr.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Organ);
			}
			else if (this.SubjectType == 2)
			{
				flag &= (acr.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Monster);
			}
			return flag & this.SubjectID == acr.get_handle().TheActorMeta.ConfigId;
		}

		private void onActorDead(ref GameDeadEventParam prm)
		{
			if ((this.SubjectType == 1 || this.SubjectType == 2) && this.SubjectID == prm.src.get_handle().TheActorMeta.ConfigId)
			{
				base.Current++;
			}
		}
	}
}
