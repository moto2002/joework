using Assets.Scripts.Common;
using Assets.Scripts.GameLogic;
using System;
using UnityEngine;

namespace AGE
{
	[EventCategory("MMGame/Skill")]
	internal class MoveActorDuration : DurationCondition
	{
		[ObjectTemplate(new Type[]
		{

		})]
		public int destId = -1;

		[ObjectTemplate(new Type[]
		{

		})]
		public int actorId;

		public VInt3 destPos = VInt3.zero;

		public VInt3 moveDir = VInt3.zero;

		public ActorMoveType moveType;

		public int moveDistance;

		public int minMoveDistance;

		public int moveSpeed;

		public int acceleration;

		public bool enableRotate = true;

		public int rotationTime;

		public bool teleport;

		public bool IgnoreCollision;

		public bool bRecordPosition;

		public bool bUseRecordPosition;

		public bool bForbidMoveFollowing;

		private bool done_;

		private Quaternion fromRot = Quaternion.identity;

		private Quaternion toRot = Quaternion.identity;

		private VInt3 srcPos = VInt3.zero;

		private VInt3 finalPos = VInt3.zero;

		private VInt3 dir = VInt3.zero;

		private int moveTick;

		private int lastTime_;

		private PoolObjHandle<ActorRoot> actor_;

		private int lastMoveSpeed;

		private int lastLerpMoveSpeed;

		public bool shouldUseAcceleration
		{
			get
			{
				return this.moveType == ActorMoveType.Directional && this.acceleration != 0;
			}
		}

		public override BaseEvent Clone()
		{
			MoveActorDuration moveActorDuration = ClassObjPool<MoveActorDuration>.Get();
			moveActorDuration.CopyData(this);
			return moveActorDuration;
		}

		protected override void CopyData(BaseEvent src)
		{
			base.CopyData(src);
			MoveActorDuration moveActorDuration = src as MoveActorDuration;
			this.destId = moveActorDuration.destId;
			this.actorId = moveActorDuration.actorId;
			this.destPos = moveActorDuration.destPos;
			this.moveDir = moveActorDuration.moveDir;
			this.moveType = moveActorDuration.moveType;
			this.moveDistance = moveActorDuration.moveDistance;
			this.minMoveDistance = moveActorDuration.minMoveDistance;
			this.moveSpeed = moveActorDuration.moveSpeed;
			this.acceleration = moveActorDuration.acceleration;
			this.lastMoveSpeed = moveActorDuration.lastMoveSpeed;
			this.lastLerpMoveSpeed = moveActorDuration.lastLerpMoveSpeed;
			this.enableRotate = moveActorDuration.enableRotate;
			this.rotationTime = moveActorDuration.rotationTime;
			this.teleport = moveActorDuration.teleport;
			this.IgnoreCollision = moveActorDuration.IgnoreCollision;
			this.bRecordPosition = moveActorDuration.bRecordPosition;
			this.bUseRecordPosition = moveActorDuration.bUseRecordPosition;
			this.bForbidMoveFollowing = moveActorDuration.bForbidMoveFollowing;
			this.done_ = moveActorDuration.done_;
			this.fromRot = moveActorDuration.fromRot;
			this.toRot = moveActorDuration.toRot;
			this.dir = moveActorDuration.dir;
			this.moveTick = moveActorDuration.moveTick;
			this.lastTime_ = moveActorDuration.lastTime_;
			this.actor_ = moveActorDuration.actor_;
		}

		public override void OnUse()
		{
			base.OnUse();
			this.destId = -1;
			this.actorId = 0;
			this.destPos = VInt3.zero;
			this.moveDir = VInt3.zero;
			this.moveType = ActorMoveType.Target;
			this.moveDistance = 0;
			this.moveSpeed = 0;
			this.acceleration = 0;
			this.enableRotate = true;
			this.rotationTime = 0;
			this.teleport = false;
			this.IgnoreCollision = false;
			this.done_ = false;
			this.fromRot = Quaternion.identity;
			this.toRot = Quaternion.identity;
			this.srcPos = VInt3.zero;
			this.finalPos = VInt3.zero;
			this.dir = VInt3.zero;
			this.moveTick = 0;
			this.lastTime_ = 0;
			this.actor_.Release();
			this.lastMoveSpeed = 0;
			this.lastLerpMoveSpeed = 0;
			this.bForbidMoveFollowing = false;
		}

		public override void Enter(Action _action, Track _track)
		{
			this.done_ = false;
			this.lastTime_ = 0;
			base.Enter(_action, _track);
			this.actor_ = _action.GetActorHandle(this.actorId);
			if (!this.actor_ || (!this.teleport && this.moveSpeed == 0 && this.acceleration == 0))
			{
				return;
			}
			this.srcPos = this.actor_.get_handle().location;
			if (this.bForbidMoveFollowing)
			{
				this.actor_.get_handle().ActorControl.AddNoAbilityFlag(ObjAbilityType.ObjAbility_MoveProtect);
			}
			if (this.bUseRecordPosition && this.actor_.get_handle().SkillControl != null)
			{
				this.destPos = this.actor_.get_handle().SkillControl.RecordPosition;
				this.dir = this.destPos - this.srcPos;
				int magnitude2D = (this.destPos - this.srcPos).get_magnitude2D();
				this.moveDistance += magnitude2D;
			}
			else if (this.moveType == ActorMoveType.Target)
			{
				PoolObjHandle<ActorRoot> actorHandle = _action.GetActorHandle(this.destId);
				if (!this.actor_ || !actorHandle)
				{
					this.actor_.Release();
					return;
				}
				int num;
				if (actorHandle.get_handle().ActorControl.GetNoAbilityFlag(ObjAbilityType.ObjAbility_MoveProtect) && actorHandle.get_handle().TheActorMeta.ActorCamp != this.actor_.get_handle().TheActorMeta.ActorCamp)
				{
					num = 0;
				}
				else
				{
					this.dir = actorHandle.get_handle().location - this.srcPos;
					num = (actorHandle.get_handle().location - this.srcPos).get_magnitude2D();
				}
				this.moveDistance += num;
			}
			else if (this.moveType == ActorMoveType.Position)
			{
				this.dir = this.destPos - this.srcPos;
				int magnitude2D2 = (this.destPos - this.srcPos).get_magnitude2D();
				if (magnitude2D2 < this.minMoveDistance)
				{
					magnitude2D2 = this.minMoveDistance;
				}
				this.moveDistance += magnitude2D2;
			}
			else if (this.moveType == ActorMoveType.Directional)
			{
				this.dir = this.actor_.get_handle().forward;
			}
			this.dir.y = 0;
			this.actor_.get_handle().ActorControl.TerminateMove();
			if (this.bRecordPosition && this.actor_.get_handle().SkillControl != null)
			{
				this.actor_.get_handle().SkillControl.RecordPosition = this.actor_.get_handle().location;
			}
			if (!this.actor_.get_handle().isMovable || this.dir.get_sqrMagnitudeLong() <= 1L)
			{
				this.actor_.Release();
				this.done_ = true;
				return;
			}
			VInt3 vInt = this.dir;
			if (!this.bUseRecordPosition)
			{
				this.finalPos = this.srcPos + vInt.NormalizeTo(this.moveDistance);
			}
			else
			{
				this.finalPos = this.actor_.get_handle().SkillControl.RecordPosition;
				if (this.teleport)
				{
					this.actor_.get_handle().ActorControl.AddNoAbilityFlag(ObjAbilityType.ObjAbility_MoveProtect);
				}
			}
			if (!PathfindingUtility.IsValidTarget(this.actor_, this.finalPos))
			{
				bool flag = false;
				if (this.moveType == ActorMoveType.Directional)
				{
					this.IgnoreCollision = false;
				}
				else
				{
					VInt3 pos = PathfindingUtility.FindValidTarget(this.actor_, this.finalPos, this.actor_.get_handle().location, 10000, out flag);
					if (!flag)
					{
						this.IgnoreCollision = false;
					}
					else
					{
						VInt vInt2 = 0;
						PathfindingUtility.GetGroundY(pos, out vInt2);
						pos.y = vInt2.i;
						this.finalPos = pos;
						this.moveDistance = (this.finalPos - this.actor_.get_handle().location).get_magnitude2D();
					}
				}
			}
			if (!this.shouldUseAcceleration)
			{
				this.moveTick = this.moveDistance * 1000 / this.moveSpeed;
			}
			else
			{
				long num2 = (long)this.moveDistance;
				long num3 = (long)this.acceleration;
				long num4 = (long)this.moveSpeed;
				this.moveTick = (int)IntMath.Divide(((long)IntMath.Sqrt(num4 * num4 + 2L * num3 * num2) - num4) * 1000L, num3);
				this.lastMoveSpeed = this.moveSpeed;
				this.lastLerpMoveSpeed = this.moveSpeed;
			}
			this.actor_.get_handle().ActorControl.AddNoAbilityFlag(ObjAbilityType.ObjAbility_Move);
			this.actor_.get_handle().ObjLinker.AddCustomMoveLerp(new CustomMoveLerpFunc(this.ActionMoveLerp));
			this.fromRot = this.actor_.get_handle().rotation;
			this.actor_.get_handle().MovementComponent.SetRotate(this.dir, true);
			if (this.rotationTime > 0)
			{
				this.toRot = Quaternion.LookRotation((Vector3)this.actor_.get_handle().forward);
			}
			else
			{
				this.actor_.get_handle().rotation = Quaternion.LookRotation((Vector3)this.actor_.get_handle().forward);
			}
		}

		public void Teleport()
		{
			VInt groundY = this.actor_.get_handle().groundY;
			VInt3 vInt = this.dir.NormalizeTo(this.moveDistance);
			VInt3 vInt2 = this.actor_.get_handle().location + vInt;
			bool flag = false;
			if (PathfindingUtility.IsValidTarget(this.actor_, vInt2))
			{
				PathfindingUtility.GetGroundY(vInt2, out groundY);
				vInt2.y = groundY.i;
				this.actor_.get_handle().location = vInt2;
			}
			else
			{
				VInt3 vInt3 = PathfindingUtility.FindValidTarget(this.actor_, vInt2, this.actor_.get_handle().location, 10000, out flag);
				if (flag)
				{
					PathfindingUtility.GetGroundY(vInt3, out groundY);
					vInt3.y = groundY.i;
					this.actor_.get_handle().location = vInt3;
				}
				else
				{
					vInt = PathfindingUtility.Move(this.actor_.get_handle(), vInt, out groundY, out this.actor_.get_handle().hasReachedNavEdge, null);
					this.actor_.get_handle().location += vInt;
				}
			}
			if (this.bUseRecordPosition)
			{
				this.actor_.get_handle().ActorControl.RmvNoAbilityFlag(ObjAbilityType.ObjAbility_MoveProtect);
			}
			this.actor_.get_handle().groundY = groundY;
			this.done_ = true;
		}

		public override void Process(Action _action, Track _track, int _localTime)
		{
			if (this.done_ || !this.actor_)
			{
				return;
			}
			bool flag = this.lastTime_ < this.rotationTime;
			int num = _localTime - this.lastTime_;
			this.lastTime_ = _localTime;
			if (this.teleport)
			{
				this.Teleport();
				return;
			}
			if (flag)
			{
				float t = Mathf.Min(1f, (float)(_localTime / this.rotationTime));
				Quaternion rotation = Quaternion.Slerp(this.fromRot, this.toRot, t);
				this.actor_.get_handle().rotation = rotation;
			}
			if (this.moveTick - num <= 0)
			{
				num = this.moveTick;
				this.done_ = true;
			}
			else
			{
				this.moveTick -= num;
			}
			VInt3 vInt = this.dir;
			VInt3 vInt2;
			if (!this.shouldUseAcceleration)
			{
				vInt2 = vInt.NormalizeTo(this.moveSpeed * num / 1000);
			}
			else
			{
				long num2 = (long)this.lastMoveSpeed * (long)num + (long)this.acceleration * (long)num * (long)num / 2L / 1000L;
				num2 /= 1000L;
				vInt2 = vInt.NormalizeTo((int)num2);
				this.lastMoveSpeed += this.acceleration * num / 1000;
			}
			VInt groundY = this.actor_.get_handle().groundY;
			if (!this.IgnoreCollision)
			{
				vInt2 = PathfindingUtility.Move(this.actor_.get_handle(), vInt2, out groundY, out this.actor_.get_handle().hasReachedNavEdge, null);
			}
			if (this.actor_.get_handle().MovementComponent.isFlying)
			{
				int y = this.actor_.get_handle().location.y;
				this.actor_.get_handle().location += vInt2;
				VInt3 location = this.actor_.get_handle().location;
				location.y = y;
				this.actor_.get_handle().location = location;
			}
			else
			{
				this.actor_.get_handle().location += vInt2;
			}
			this.actor_.get_handle().groundY = groundY;
			base.Process(_action, _track, _localTime);
		}

		private void SetFinalPos()
		{
			VInt vInt = 0;
			PathfindingUtility.GetGroundY(this.finalPos, out vInt);
			this.finalPos.y = vInt.i;
			this.actor_.get_handle().location = this.finalPos;
		}

		public override void Leave(Action _action, Track _track)
		{
			if (!this.actor_)
			{
				return;
			}
			if (this.bForbidMoveFollowing)
			{
				this.actor_.get_handle().ActorControl.RmvNoAbilityFlag(ObjAbilityType.ObjAbility_MoveProtect);
			}
			if (this.IgnoreCollision)
			{
				bool flag = false;
				_action.refParams.GetRefParam("_HitTargetHero", ref flag);
				if (!flag)
				{
					this.SetFinalPos();
				}
				else
				{
					VInt3 zero = VInt3.zero;
					_action.refParams.GetRefParam("_HitTargetHeroPos", ref zero);
					if (!PathfindingUtility.IsValidTarget(this.actor_, zero))
					{
						this.SetFinalPos();
					}
					else
					{
						this.actor_.get_handle().location = zero;
					}
				}
			}
			this.actor_.get_handle().ObjLinker.RmvCustomMoveLerp(new CustomMoveLerpFunc(this.ActionMoveLerp));
			this.actor_.get_handle().ActorControl.RmvNoAbilityFlag(ObjAbilityType.ObjAbility_Move);
			this.done_ = true;
		}

		private void ActionMoveLerp(ActorRoot actor, uint nDelta, bool bReset)
		{
			if (actor == null)
			{
				return;
			}
			if (this.done_ || this.teleport)
			{
				actor.myTransform.position = (Vector3)actor.location;
			}
			else
			{
				VInt vInt = 0;
				VInt3 vInt2 = this.dir;
				VInt3 vInt3;
				if (!this.shouldUseAcceleration)
				{
					vInt3 = vInt2.NormalizeTo(this.moveSpeed * (int)nDelta / 1000);
				}
				else
				{
					long num = (long)this.lastLerpMoveSpeed * (long)((ulong)nDelta) + (long)this.acceleration * (long)((ulong)nDelta) * (long)((ulong)nDelta) / 2L / 1000L;
					num /= 1000L;
					vInt3 = vInt2.NormalizeTo((int)num);
					this.lastLerpMoveSpeed += (int)((long)this.acceleration * (long)((ulong)nDelta)) / 1000;
				}
				Vector3 position = actor.myTransform.position;
				if (!this.IgnoreCollision)
				{
					vInt3 = PathfindingUtility.MoveLerp(actor, (VInt3)position, vInt3, out vInt);
				}
				if (actor.MovementComponent.isFlying)
				{
					float y = position.y;
					actor.myTransform.position += (Vector3)vInt3;
					Vector3 position2 = actor.myTransform.position;
					position2.y = y;
					actor.myTransform.position = position2;
				}
				else
				{
					actor.myTransform.position += (Vector3)vInt3;
				}
			}
		}

		public override bool Check(Action _action, Track _track)
		{
			return this.done_;
		}
	}
}
