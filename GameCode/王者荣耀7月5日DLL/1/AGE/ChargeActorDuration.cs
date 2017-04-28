using Assets.Scripts.Common;
using Assets.Scripts.GameLogic;
using System;
using UnityEngine;

namespace AGE
{
	[EventCategory("MMGame/Skill")]
	public class ChargeActorDuration : DurationCondition
	{
		[ObjectTemplate(new Type[]
		{

		})]
		public int triggerID = -1;

		[ObjectTemplate(new Type[]
		{

		})]
		public int targetID = -1;

		public int moveSpeed;

		public int acceleration;

		public int maxMoveSpeed;

		public int lastDistance;

		private PoolObjHandle<ActorRoot> triggerActor;

		private PoolObjHandle<ActorRoot> targetActor;

		private int lastTime_;

		private bool done_;

		private VInt3 dir = VInt3.zero;

		private int curSpeed;

		private int curLerpSpeed;

		protected override void CopyData(BaseEvent src)
		{
			base.CopyData(src);
			ChargeActorDuration chargeActorDuration = src as ChargeActorDuration;
			this.triggerID = chargeActorDuration.triggerID;
			this.targetID = chargeActorDuration.targetID;
			this.moveSpeed = chargeActorDuration.moveSpeed;
			this.lastDistance = chargeActorDuration.lastDistance;
			this.maxMoveSpeed = chargeActorDuration.maxMoveSpeed;
			this.acceleration = chargeActorDuration.acceleration;
		}

		public override BaseEvent Clone()
		{
			ChargeActorDuration chargeActorDuration = ClassObjPool<ChargeActorDuration>.Get();
			chargeActorDuration.CopyData(this);
			return chargeActorDuration;
		}

		public override void OnUse()
		{
			base.OnUse();
			this.triggerID = -1;
			this.targetID = -1;
			this.moveSpeed = 0;
			this.lastDistance = 0;
			this.curSpeed = 0;
			this.curLerpSpeed = 0;
			this.triggerActor.Release();
			this.targetActor.Release();
		}

		public override void Enter(Action _action, Track _track)
		{
			base.Enter(_action, _track);
			this.lastTime_ = 0;
			this.done_ = false;
			this.triggerActor = _action.GetActorHandle(this.triggerID);
			this.targetActor = _action.GetActorHandle(this.targetID);
			if (!this.triggerActor || !this.targetActor || (this.moveSpeed == 0 && this.acceleration == 0))
			{
				return;
			}
			this.curSpeed = this.moveSpeed;
			this.curLerpSpeed = this.moveSpeed;
			this.triggerActor.get_handle().ActorControl.AddNoAbilityFlag(ObjAbilityType.ObjAbility_Move);
			this.triggerActor.get_handle().ObjLinker.AddCustomMoveLerp(new CustomMoveLerpFunc(this.ActionMoveLerp));
			this.dir = this.targetActor.get_handle().location - this.triggerActor.get_handle().location;
		}

		public override void Process(Action _action, Track _track, int _localTime)
		{
			base.Process(_action, _track, _localTime);
			if (!this.triggerActor || !this.targetActor || (this.moveSpeed == 0 && this.acceleration == 0))
			{
				return;
			}
			if (this.done_)
			{
				return;
			}
			VInt3 location = this.triggerActor.get_handle().location;
			VInt3 vInt = this.targetActor.get_handle().location;
			int num = _localTime - this.lastTime_;
			this.lastTime_ = _localTime;
			VInt3 vInt2 = vInt - location;
			long num2 = (long)this.curSpeed * (long)num + (long)this.acceleration * (long)num * (long)num / 2L / 1000L;
			this.curSpeed = Mathf.Min(this.curSpeed + this.acceleration * num / 1000, this.maxMoveSpeed);
			this.curLerpSpeed = this.curSpeed;
			num2 /= 1000L;
			if (num2 >= (long)(vInt2.get_magnitude() - this.lastDistance))
			{
				num2 = (long)Mathf.Max(vInt2.get_magnitude() - this.lastDistance, 0);
				this.done_ = true;
				vInt = location + vInt2.NormalizeTo((int)num2);
				if (!PathfindingUtility.IsValidTarget(this.triggerActor.get_handle(), vInt))
				{
					bool flag = false;
					VInt3 vInt3 = PathfindingUtility.FindValidTarget(this.triggerActor.get_handle(), vInt, this.triggerActor.get_handle().location, 10000, out flag);
					VInt vInt4 = 0;
					PathfindingUtility.GetGroundY(vInt3, out vInt4);
					vInt3.y = vInt4.i;
					vInt = vInt3;
					vInt2 = vInt - this.triggerActor.get_handle().location;
					num2 = (long)vInt2.get_magnitude();
				}
			}
			this.dir = vInt2;
			if (vInt2 != VInt3.zero)
			{
				this.triggerActor.get_handle().rotation = Quaternion.LookRotation((Vector3)vInt2);
			}
			this.triggerActor.get_handle().location += vInt2.NormalizeTo((int)num2);
		}

		public override void Leave(Action _action, Track _track)
		{
			base.Leave(_action, _track);
			if (!this.triggerActor || !this.targetActor || this.moveSpeed == 0)
			{
				return;
			}
			this.done_ = true;
			this.triggerActor.get_handle().ActorControl.RmvNoAbilityFlag(ObjAbilityType.ObjAbility_Move);
			this.triggerActor.get_handle().ObjLinker.RmvCustomMoveLerp(new CustomMoveLerpFunc(this.ActionMoveLerp));
			if (!PathfindingUtility.IsValidTarget(this.triggerActor.get_handle(), this.triggerActor.get_handle().location))
			{
				bool flag = false;
				VInt3 vInt = PathfindingUtility.FindValidTarget(this.triggerActor.get_handle(), this.targetActor.get_handle().location, this.triggerActor.get_handle().location, 10000, out flag);
				VInt vInt2 = 0;
				PathfindingUtility.GetGroundY(vInt, out vInt2);
				vInt.y = vInt2.i;
				this.triggerActor.get_handle().location = vInt;
			}
		}

		private void ActionMoveLerp(ActorRoot actor, uint nDelta, bool bReset)
		{
			if (actor == null)
			{
				return;
			}
			if (this.done_)
			{
				actor.myTransform.position = (Vector3)actor.location;
			}
			else
			{
				VInt vInt = 0;
				VInt3 vInt2 = this.dir;
				long num = (long)this.curLerpSpeed * (long)((ulong)nDelta) + (long)this.acceleration * (long)((ulong)nDelta) * (long)((ulong)nDelta) / 2L / 1000L;
				this.curLerpSpeed = Mathf.Min((int)((long)this.curLerpSpeed + (long)this.acceleration * (long)((ulong)nDelta) / 1000L), this.maxMoveSpeed);
				num /= 1000L;
				VInt3 vInt3 = vInt2.NormalizeTo((int)num);
				actor.myTransform.position += (Vector3)vInt3;
			}
		}

		public override bool Check(Action _action, Track _track)
		{
			return this.done_;
		}
	}
}
