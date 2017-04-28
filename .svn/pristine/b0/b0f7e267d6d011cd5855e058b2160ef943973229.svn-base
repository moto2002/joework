using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using System;
using UnityEngine;

namespace AGE
{
	[EventCategory("MMGame/Skill")]
	internal class RotateActorDuration : DurationCondition
	{
		[ObjectTemplate(new Type[]
		{

		})]
		public int targetId;

		public int rotateSpeed;

		private int lastTime;

		private PoolObjHandle<ActorRoot> actorTarget;

		private VInt3 destDir = VInt3.zero;

		private bool bNeedRotate;

		private int curRotateSpd;

		public override BaseEvent Clone()
		{
			RotateActorDuration rotateActorDuration = ClassObjPool<RotateActorDuration>.Get();
			rotateActorDuration.CopyData(this);
			return rotateActorDuration;
		}

		protected override void CopyData(BaseEvent src)
		{
			base.CopyData(src);
			RotateActorDuration rotateActorDuration = src as RotateActorDuration;
			this.targetId = rotateActorDuration.targetId;
			this.rotateSpeed = rotateActorDuration.rotateSpeed;
			this.lastTime = rotateActorDuration.lastTime;
			this.actorTarget = rotateActorDuration.actorTarget;
			this.destDir = rotateActorDuration.destDir;
			this.bNeedRotate = rotateActorDuration.bNeedRotate;
			this.curRotateSpd = rotateActorDuration.curRotateSpd;
		}

		public override void OnUse()
		{
			base.OnUse();
			this.targetId = 0;
			this.rotateSpeed = 0;
			this.lastTime = 0;
			this.actorTarget.Release();
			this.destDir = VInt3.zero;
			this.bNeedRotate = false;
			this.curRotateSpd = 0;
		}

		public override void Enter(Action _action, Track _track)
		{
			base.Enter(_action, _track);
			this.actorTarget = _action.GetActorHandle(this.targetId);
			this.lastTime = 0;
			if (!this.actorTarget)
			{
				return;
			}
			this.bNeedRotate = false;
			this.curRotateSpd = 0;
			this.actorTarget.get_handle().ObjLinker.AddCustomRotateLerp(new CustomRotateLerpFunc(this.ActionRotateLerp));
		}

		public override void Process(Action _action, Track _track, int _localTime)
		{
			if (!this.actorTarget)
			{
				return;
			}
			int num = _localTime - this.lastTime;
			this.lastTime = _localTime;
			if (this.actorTarget.get_handle().ActorControl.curMoveCommand != null)
			{
				FrameCommand<MoveDirectionCommand> frameCommand = (FrameCommand<MoveDirectionCommand>)this.actorTarget.get_handle().ActorControl.curMoveCommand;
				VInt3 vInt = this.actorTarget.get_handle().forward;
				VInt3 right = VInt3.right;
				this.destDir = right.RotateY((int)frameCommand.cmdData.Degree);
				if (this.destDir != vInt)
				{
					this.bNeedRotate = true;
					this.curRotateSpd = this.rotateSpeed;
					int num2 = this.destDir.x * vInt.z - vInt.x * this.destDir.z;
					if (num2 == 0)
					{
						int num3 = VInt3.Dot(this.destDir, vInt);
						if (num3 >= 0)
						{
							return;
						}
						this.curRotateSpd = this.rotateSpeed;
					}
					else if (num2 < 0)
					{
						this.curRotateSpd = -this.rotateSpeed;
					}
					VFactor vFactor = VInt3.AngleInt(this.destDir, vInt);
					VFactor vFactor2 = VFactor.pi * (long)num * (long)this.curRotateSpd / 180L / 1000L;
					if (vFactor <= vFactor2)
					{
						vInt = vInt.RotateY(ref vFactor);
						this.bNeedRotate = false;
					}
					else
					{
						vInt = vInt.RotateY(ref vFactor2);
					}
					this.actorTarget.get_handle().MovementComponent.SetRotate(vInt, true);
				}
			}
			else
			{
				this.destDir = this.actorTarget.get_handle().forward;
				this.bNeedRotate = false;
				this.curRotateSpd = 0;
			}
			base.Process(_action, _track, _localTime);
		}

		public override void Leave(Action _action, Track _track)
		{
			if (!this.actorTarget)
			{
				return;
			}
			this.actorTarget.get_handle().ObjLinker.RmvCustomRotateLerp(new CustomRotateLerpFunc(this.ActionRotateLerp));
			this.curRotateSpd = 0;
			this.bNeedRotate = false;
		}

		public void ActionRotateLerp(ActorRoot actor, uint nDelta)
		{
			if (actor == null || !this.bNeedRotate || this.curRotateSpd == 0)
			{
				return;
			}
			int num = (int)(nDelta * (uint)this.curRotateSpd * 10u / 1000u);
			Quaternion to = Quaternion.LookRotation((Vector3)actor.forward.RotateY(num));
			actor.myTransform.rotation = Quaternion.RotateTowards(actor.myTransform.rotation, to, (float)((long)Mathf.Abs(this.curRotateSpd) * (long)((ulong)nDelta)) * 0.001f);
		}
	}
}
