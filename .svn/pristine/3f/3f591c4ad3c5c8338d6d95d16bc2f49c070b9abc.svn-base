using Assets.Scripts.Common;
using Assets.Scripts.GameLogic;
using Assets.Scripts.GameLogic.GameKernal;
using System;

namespace AGE
{
	[EventCategory("MMGame/Skill")]
	public class SetObjBehaviourModeTick : TickEvent
	{
		public ObjBehaviMode Mode;

		public override void OnUse()
		{
			base.OnUse();
			this.Mode = ObjBehaviMode.State_Idle;
		}

		public override BaseEvent Clone()
		{
			SetObjBehaviourModeTick setObjBehaviourModeTick = ClassObjPool<SetObjBehaviourModeTick>.Get();
			setObjBehaviourModeTick.CopyData(this);
			return setObjBehaviourModeTick;
		}

		protected override void CopyData(BaseEvent src)
		{
			base.CopyData(src);
			SetObjBehaviourModeTick setObjBehaviourModeTick = src as SetObjBehaviourModeTick;
			this.Mode = setObjBehaviourModeTick.Mode;
		}

		public override void Process(Action _action, Track _track)
		{
			base.Process(_action, _track);
			PoolObjHandle<ActorRoot> captain = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().Captain;
			if (captain && captain.get_handle().ActorControl != null)
			{
				captain.get_handle().ActorControl.SetObjBehaviMode(this.Mode);
			}
		}
	}
}
