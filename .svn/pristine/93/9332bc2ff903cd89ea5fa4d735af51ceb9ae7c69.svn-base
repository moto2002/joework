using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using ResData;
using System;

namespace Assets.Scripts.GameLogic
{
	public class GameTaskOccupy : GameTask
	{
		protected RES_BATTLE_TASK_SUBJECT SubjectType
		{
			get
			{
				return base.Config.iParam1;
			}
		}

		protected int SourceSubj
		{
			get
			{
				return base.Config.iParam2;
			}
		}

		protected int TargetArea
		{
			get
			{
				return base.Config.iParam3;
			}
		}

		protected override void OnInitial()
		{
		}

		protected override void OnDestroy()
		{
			Singleton<TriggerEventSys>.get_instance().OnActorInside -= new TriggerEventDelegate(this.onActorInside);
		}

		protected override void OnStart()
		{
			Singleton<TriggerEventSys>.get_instance().OnActorInside += new TriggerEventDelegate(this.onActorInside);
		}

		protected override void OnClose()
		{
			Singleton<TriggerEventSys>.get_instance().OnActorInside -= new TriggerEventDelegate(this.onActorInside);
		}

		private void onActorInside(AreaEventTrigger sourceTrigger, object param)
		{
			if (sourceTrigger.Mark == this.TargetArea)
			{
				if (this.SubjectType == null && sourceTrigger.HasActorInside((PoolObjHandle<ActorRoot> enr) => enr.get_handle().TheActorMeta.ActorCamp == this.SourceSubj))
				{
					base.Current += (int)param;
				}
				else if (this.SubjectType == 1 && sourceTrigger.HasActorInside((PoolObjHandle<ActorRoot> enr) => enr.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Organ && enr.get_handle().TheActorMeta.ConfigId == this.SourceSubj))
				{
					base.Current += (int)param;
				}
			}
		}
	}
}
