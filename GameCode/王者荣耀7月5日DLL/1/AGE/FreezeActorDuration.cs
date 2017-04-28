using Assets.Scripts.Common;
using Assets.Scripts.GameLogic;
using Pathfinding.RVO;
using System;
using UnityEngine;

namespace AGE
{
	[EventCategory("MMGame/Skill")]
	public class FreezeActorDuration : DurationEvent
	{
		[ObjectTemplate(new Type[]
		{

		})]
		public int targetId;

		public int freezeHeight;

		private PoolObjHandle<ActorRoot> actorObj;

		private RVOController rovController;

		private string curAnimName;

		public override void OnUse()
		{
			base.OnUse();
			this.targetId = 0;
			this.freezeHeight = 0;
			this.actorObj.Release();
			this.curAnimName = null;
			this.rovController = null;
		}

		public override BaseEvent Clone()
		{
			FreezeActorDuration freezeActorDuration = ClassObjPool<FreezeActorDuration>.Get();
			freezeActorDuration.CopyData(this);
			return freezeActorDuration;
		}

		protected override void CopyData(BaseEvent src)
		{
			base.CopyData(src);
			FreezeActorDuration freezeActorDuration = src as FreezeActorDuration;
			this.targetId = freezeActorDuration.targetId;
			this.freezeHeight = freezeActorDuration.freezeHeight;
			this.actorObj = freezeActorDuration.actorObj;
			this.curAnimName = freezeActorDuration.curAnimName;
			this.rovController = freezeActorDuration.rovController;
		}

		private void PauseAnimation()
		{
			AnimPlayComponent animControl = this.actorObj.get_handle().AnimControl;
			if (animControl == null)
			{
				return;
			}
			this.curAnimName = animControl.GetCurAnimName();
			if (this.actorObj.get_handle().ActorMesh == null || this.actorObj.get_handle().ActorMeshAnimation == null)
			{
				return;
			}
			AnimationState animationState = this.actorObj.get_handle().ActorMeshAnimation[this.curAnimName];
			if (animationState != null)
			{
				animationState.speed = 0f;
			}
			animControl.bPausePlay = true;
		}

		private void RecoverAnimation()
		{
			AnimPlayComponent animControl = this.actorObj.get_handle().AnimControl;
			if (this.actorObj.get_handle().ActorMesh == null || this.actorObj.get_handle().ActorMeshAnimation == null)
			{
				return;
			}
			AnimationState animationState = this.actorObj.get_handle().ActorMeshAnimation[this.curAnimName];
			if (animationState != null)
			{
				animationState.speed = 1f;
			}
			if (animControl != null)
			{
				animControl.bPausePlay = false;
				animControl.UpdatePlay();
			}
		}

		public override void Enter(Action _action, Track _track)
		{
			base.Enter(_action, _track);
			this.actorObj = _action.GetActorHandle(this.targetId);
			if (!this.actorObj)
			{
				return;
			}
			ObjWrapper actorControl = this.actorObj.get_handle().ActorControl;
			if (actorControl == null)
			{
				return;
			}
			this.PauseAnimation();
			actorControl.TerminateMove();
			actorControl.ClearMoveCommand();
			actorControl.ForceAbortCurUseSkill();
			this.actorObj.get_handle().ActorControl.AddNoAbilityFlag(ObjAbilityType.ObjAbility_Move);
			this.actorObj.get_handle().ActorControl.AddNoAbilityFlag(ObjAbilityType.ObjAbility_Freeze);
			this.actorObj.get_handle().ActorControl.AddDisableSkillFlag(SkillSlotType.SLOT_SKILL_COUNT, false);
			if (this.freezeHeight > 0 && this.actorObj.get_handle().isMovable)
			{
				VInt vInt = 0;
				VInt3 location = this.actorObj.get_handle().location;
				PathfindingUtility.GetGroundY(location, out vInt);
				location.y = vInt.i + this.freezeHeight;
				this.actorObj.get_handle().location = location;
			}
		}

		public override void Process(Action _action, Track _track, int _localTime)
		{
			base.Process(_action, _track, _localTime);
		}

		public override void Leave(Action _action, Track _track)
		{
			base.Leave(_action, _track);
			if (this.actorObj)
			{
				this.RecoverAnimation();
				this.actorObj.get_handle().ActorControl.RmvNoAbilityFlag(ObjAbilityType.ObjAbility_Move);
				this.actorObj.get_handle().ActorControl.RmvNoAbilityFlag(ObjAbilityType.ObjAbility_Freeze);
				this.actorObj.get_handle().ActorControl.RmvDisableSkillFlag(SkillSlotType.SLOT_SKILL_COUNT, false);
				if (this.freezeHeight > 0 && this.actorObj.get_handle().isMovable)
				{
					VInt vInt = 0;
					VInt3 location = this.actorObj.get_handle().location;
					location.y -= this.freezeHeight;
					PathfindingUtility.GetGroundY(location, out vInt);
					if (location.y < vInt.i)
					{
						location.y = vInt.i;
					}
					this.actorObj.get_handle().location = location;
				}
			}
		}
	}
}
