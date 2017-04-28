using Assets.Scripts.Common;
using Assets.Scripts.GameLogic;
using Assets.Scripts.GameLogic.GameKernal;
using System;
using UnityEngine;

namespace AGE
{
	[EventCategory("MMGame/Drama")]
	public class AddGoldInBattle : TickEvent
	{
		public int iGoldToAdd;

		public override bool SupportEditMode()
		{
			return true;
		}

		protected override void CopyData(BaseEvent src)
		{
			base.CopyData(src);
			AddGoldInBattle addGoldInBattle = src as AddGoldInBattle;
			this.iGoldToAdd = addGoldInBattle.iGoldToAdd;
		}

		public override BaseEvent Clone()
		{
			AddGoldInBattle addGoldInBattle = ClassObjPool<AddGoldInBattle>.Get();
			addGoldInBattle.CopyData(this);
			return addGoldInBattle;
		}

		public override void Process(Action _action, Track _track)
		{
			base.Process(_action, _track);
			Player hostPlayer = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer();
			if (hostPlayer != null && hostPlayer.Captain && hostPlayer.Captain.get_handle().ValueComponent != null)
			{
				hostPlayer.Captain.get_handle().ValueComponent.ChangeGoldCoinInBattle(this.iGoldToAdd, true, false, default(Vector3), false, default(PoolObjHandle<ActorRoot>));
			}
			else if (hostPlayer == null)
			{
				DebugHelper.Assert(false, "invalid host player");
			}
			else if (!hostPlayer.Captain)
			{
				DebugHelper.Assert(false, "invalid host player captain");
			}
			else if (hostPlayer.Captain.get_handle().ValueComponent == null)
			{
				DebugHelper.Assert(false, "invalid host player captain->valuecomponent");
			}
		}
	}
}
