using System;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
	public abstract class MovementState : BaseState
	{
		protected PlayerMovement Parent;

		protected VInt3 Velocity;

		public MovementState(PlayerMovement InParent)
		{
			this.Parent = InParent;
		}

		public virtual void UpdateLogic(int nDelta)
		{
			if (!this.Parent.isRotatingLock)
			{
				this.PointingAtTarget(nDelta);
			}
		}

		protected void UpdateRotateDir(VInt3 targetDir, int dt)
		{
			ActorRoot actor = this.Parent.actor;
			if (targetDir == VInt3.zero)
			{
				return;
			}
			actor.ObjLinker.SetForward(targetDir, this.Parent.actor.ActorControl.curMoveSeq);
			if (actor.ActorControl.GetNoAbilityFlag(ObjAbilityType.ObjAbility_MoveRotate))
			{
				return;
			}
			actor.forward = targetDir;
			if (actor.InCamera)
			{
				Vector3 forward = this.Parent.actor.myTransform.forward;
				((Vector3)targetDir).y = 0f;
				forward.y = 0f;
				VFactor vFactor = VInt3.AngleInt(targetDir, VInt3.forward);
				int arg_B8_0 = targetDir.x;
				VInt3 forward2 = VInt3.forward;
				int arg_CF_0 = arg_B8_0 * forward2.z;
				VInt3 forward3 = VInt3.forward;
				int num = arg_CF_0 - forward3.x * targetDir.z;
				if (num < 0)
				{
					vFactor = VFactor.twoPi - vFactor;
				}
				Quaternion to = Quaternion.AngleAxis(vFactor.get_single() * 57.29578f, Vector3.up);
				actor.rotation = Quaternion.RotateTowards(actor.rotation, to, (float)(this.Parent.RotateSpeed * dt) * 0.001f);
			}
		}

		protected virtual void PointingAtTarget(int delta)
		{
			this.UpdateRotateDir(this.Parent.adjustedDirection, delta);
		}

		public virtual int GetCurrentSpeed()
		{
			return 0;
		}

		public virtual VInt3 GetVelocity()
		{
			return this.Velocity;
		}

		public virtual void ChangeTarget()
		{
		}

		public virtual void ChangeDirection()
		{
		}

		public virtual bool IsOnTarget()
		{
			return this.Parent.isStandOnTarget;
		}

		public virtual bool IsMoving()
		{
			return false;
		}
	}
}
