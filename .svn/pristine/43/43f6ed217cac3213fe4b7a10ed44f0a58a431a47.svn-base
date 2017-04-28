using Assets.Scripts.Common;
using Assets.Scripts.GameLogic;
using System;
using UnityEngine;

namespace AGE
{
	internal class MoveBulletDurationContext
	{
		public int length;

		public int targetId;

		public int destId;

		public ActorMoveType MoveType;

		public VInt3 targetPosition;

		public VInt3 offsetDir;

		public int velocity;

		public int acceleration;

		public int distance;

		public int gravity;

		public bool bMoveRotate;

		public bool bAdjustSpeed;

		public bool bBulletUseDir;

		public bool bUseIndicatorDir;

		public bool bReachDestStop;

		public bool bResetMoveDistance;

		private SkillUseContext skillContext;

		private VInt3 destPosition;

		public int lastTime;

		private int hitHeight;

		public PoolObjHandle<ActorRoot> tarActor;

		public PoolObjHandle<ActorRoot> moveActor;

		private AccelerateMotionControler gravityControler;

		public bool stopCondtion;

		public bool stopLerpCondtion;

		private VInt3 moveDirection;

		private VInt3 lerpDirection;

		private int lastVelocity;

		private int lastLerpVelocity;

		public bool shouldUseAcceleration
		{
			get
			{
				return !this.bAdjustSpeed && this.acceleration != 0;
			}
		}

		public void Reset(MoveBulletDuration InBulletDuration)
		{
			this.length = InBulletDuration.length;
			this.targetId = InBulletDuration.targetId;
			this.destId = InBulletDuration.destId;
			this.MoveType = InBulletDuration.MoveType;
			this.targetPosition = InBulletDuration.targetPosition;
			this.offsetDir = InBulletDuration.offsetDir;
			this.velocity = InBulletDuration.velocity;
			this.acceleration = InBulletDuration.acceleration;
			this.distance = InBulletDuration.distance;
			this.gravity = InBulletDuration.gravity;
			this.bMoveRotate = InBulletDuration.bMoveRotate;
			this.bAdjustSpeed = InBulletDuration.bAdjustSpeed;
			this.bBulletUseDir = InBulletDuration.bBulletUseDir;
			this.bUseIndicatorDir = InBulletDuration.bUseIndicatorDir;
			this.bReachDestStop = InBulletDuration.bReachDestStop;
			this.bResetMoveDistance = InBulletDuration.bResetMoveDistance;
			this.lastVelocity = (this.lastLerpVelocity = 0);
			this.stopLerpCondtion = false;
		}

		public void Reset(BulletTriggerDuration InBulletTrigger)
		{
			this.length = InBulletTrigger.length;
			this.targetId = InBulletTrigger.targetId;
			this.destId = InBulletTrigger.destId;
			this.MoveType = InBulletTrigger.MoveType;
			this.targetPosition = InBulletTrigger.targetPosition;
			this.offsetDir = InBulletTrigger.offsetDir;
			this.velocity = InBulletTrigger.velocity;
			this.acceleration = InBulletTrigger.acceleration;
			this.distance = InBulletTrigger.distance;
			this.gravity = InBulletTrigger.gravity;
			this.bMoveRotate = InBulletTrigger.bMoveRotate;
			this.bAdjustSpeed = InBulletTrigger.bAdjustSpeed;
			this.bBulletUseDir = InBulletTrigger.bBulletUseDir;
			this.bUseIndicatorDir = InBulletTrigger.bUseIndicatorDir;
			this.bReachDestStop = InBulletTrigger.bReachDestStop;
			this.lastVelocity = (this.lastLerpVelocity = 0);
			this.stopLerpCondtion = false;
			this.bResetMoveDistance = false;
		}

		public void CopyData(ref MoveBulletDurationContext r)
		{
			this.length = r.length;
			this.targetId = r.targetId;
			this.destId = r.destId;
			this.MoveType = r.MoveType;
			this.targetPosition = r.targetPosition;
			this.offsetDir = r.offsetDir;
			this.velocity = r.velocity;
			this.acceleration = r.acceleration;
			this.distance = r.distance;
			this.gravity = r.gravity;
			this.bMoveRotate = r.bMoveRotate;
			this.bAdjustSpeed = r.bAdjustSpeed;
			this.bBulletUseDir = r.bBulletUseDir;
			this.bUseIndicatorDir = r.bUseIndicatorDir;
			this.bReachDestStop = r.bReachDestStop;
			this.bResetMoveDistance = r.bResetMoveDistance;
			this.skillContext = r.skillContext;
			this.destPosition = r.destPosition;
			this.lastTime = r.lastTime;
			this.hitHeight = r.hitHeight;
			this.tarActor = r.tarActor;
			this.moveActor = r.moveActor;
			this.gravityControler = r.gravityControler;
			this.stopCondtion = r.stopCondtion;
			this.moveDirection = r.moveDirection;
			this.lerpDirection = r.lerpDirection;
			this.lastVelocity = r.lastVelocity;
			this.lastLerpVelocity = r.lastLerpVelocity;
		}

		public void Enter(Action _action)
		{
			this.skillContext = _action.refParams.GetRefParamObject<SkillUseContext>("SkillContext");
			this.lastTime = 0;
			this.lastVelocity = (this.lastLerpVelocity = this.velocity);
			this.stopCondtion = false;
			this.moveActor = _action.GetActorHandle(this.targetId);
			if (!this.moveActor)
			{
				return;
			}
			this.gravityControler = new AccelerateMotionControler();
			this.moveActor.get_handle().ObjLinker.AddCustomMoveLerp(new CustomMoveLerpFunc(this.ActionMoveLerp));
			if (this.MoveType == ActorMoveType.Target)
			{
				this.tarActor = _action.GetActorHandle(this.destId);
				if (!this.tarActor)
				{
					return;
				}
				this.destPosition = this.tarActor.get_handle().location;
				CActorInfo charInfo = this.tarActor.get_handle().CharInfo;
				if (charInfo != null)
				{
					this.hitHeight = charInfo.iBulletHeight;
					VInt3 vInt = this.moveActor.get_handle().location - this.destPosition;
					vInt.y = 0;
					vInt = vInt.NormalizeTo(1000);
					this.destPosition += IntMath.Divide(vInt, (long)charInfo.iCollisionSize.x, 1000L);
				}
				this.destPosition.y = this.destPosition.y + this.hitHeight;
			}
			else if (this.MoveType == ActorMoveType.Directional)
			{
				VInt3 vInt2 = VInt3.one;
				if (this.skillContext == null)
				{
					return;
				}
				PoolObjHandle<ActorRoot> originator = this.skillContext.Originator;
				if (!originator)
				{
					return;
				}
				if (this.bBulletUseDir)
				{
					_action.refParams.GetRefParam("_BulletUseDir", ref vInt2);
				}
				else if (this.bUseIndicatorDir)
				{
					SkillUseContext refParamObject = _action.refParams.GetRefParamObject<SkillUseContext>("SkillContext");
					VInt3 vInt3;
					if (refParamObject != null && refParamObject.CalcAttackerDir(out vInt3, originator))
					{
						vInt2 = vInt3;
					}
					else
					{
						vInt2 = originator.get_handle().forward;
					}
				}
				else
				{
					vInt2 = originator.get_handle().forward;
				}
				vInt2 = vInt2.RotateY(this.offsetDir.y);
				if (this.bResetMoveDistance)
				{
					int num = 0;
					_action.refParams.GetRefParam("_BulletRealFlyingTime", ref num);
					int num2 = num * this.velocity / 1000;
					this.distance = ((num2 <= 0) ? this.distance : num2);
				}
				this.destPosition = this.moveActor.get_handle().location + vInt2.NormalizeTo(this.distance);
				this.destPosition.y = this.moveActor.get_handle().location.y;
			}
			else if (this.MoveType == ActorMoveType.Position)
			{
				if (this.bReachDestStop)
				{
					this.destPosition = this.targetPosition;
				}
				else
				{
					VInt3 vInt4 = this.targetPosition - this.moveActor.get_handle().location;
					vInt4.y = 0;
					vInt4 = vInt4.NormalizeTo(1000);
					this.destPosition = this.moveActor.get_handle().location + vInt4 * (this.length * this.velocity / 1000);
					VInt vInt5;
					if (PathfindingUtility.GetGroundY(this.destPosition, out vInt5))
					{
						this.destPosition.y = vInt5.i;
					}
				}
			}
			if (this.bAdjustSpeed)
			{
				VInt3 vInt6 = this.destPosition - this.moveActor.get_handle().location;
				int num3 = this.length - 100;
				num3 = ((num3 > 0) ? num3 : this.length);
				this.velocity = (int)IntMath.Divide((long)vInt6.get_magnitude2D() * 1000L, (long)num3);
			}
			if (this.gravity < 0)
			{
				if (this.velocity == 0)
				{
					this.stopCondtion = true;
					return;
				}
				VInt3 vInt7 = this.destPosition - this.moveActor.get_handle().location;
				int num4;
				if (!this.shouldUseAcceleration)
				{
					num4 = (int)IntMath.Divide((long)vInt7.get_magnitude2D() * 1000L, (long)this.velocity);
				}
				else
				{
					long num5 = (long)this.velocity;
					long num6 = (long)this.acceleration;
					long num7 = (long)vInt7.get_magnitude2D();
					long num8 = num5 * num5 + 2L * num6 * num7;
					num4 = (int)IntMath.Divide(((long)IntMath.Sqrt(num8) - num5) * 1000L, num6);
					this.lastVelocity = (this.lastLerpVelocity = this.velocity);
				}
				if (num4 == 0)
				{
					this.stopCondtion = true;
					return;
				}
				VInt vInt8;
				if (PathfindingUtility.GetGroundY(this.destPosition, out vInt8))
				{
					this.gravityControler.InitMotionControler(num4, vInt8.i - this.moveActor.get_handle().location.y, this.gravity);
				}
				else
				{
					this.gravityControler.InitMotionControler(num4, 0, this.gravity);
				}
			}
		}

		public void Leave(Action _action, Track _track)
		{
			if (this.moveActor)
			{
				this.moveActor.get_handle().ObjLinker.RmvCustomMoveLerp(new CustomMoveLerpFunc(this.ActionMoveLerp));
				this.moveActor.get_handle().myTransform.position = (Vector3)this.moveActor.get_handle().location;
				if (this.moveActor.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Bullet)
				{
					BulletWrapper bulletWrapper = this.moveActor.get_handle().ActorControl as BulletWrapper;
					if (bulletWrapper != null && bulletWrapper.GetMoveCollisiong())
					{
						bulletWrapper.SetMoveDelta(0);
					}
				}
			}
			this.skillContext = null;
			this.tarActor.Release();
			this.moveActor.Release();
			this.gravityControler = null;
		}

		private void RotateMoveBullet(VInt3 _dir)
		{
			if (this.MoveType == ActorMoveType.Target || this.MoveType == ActorMoveType.Directional)
			{
				if (_dir == VInt3.zero)
				{
					return;
				}
				this.moveActor.get_handle().forward = _dir.NormalizeTo(1000);
				Quaternion rotation = Quaternion.identity;
				rotation = Quaternion.LookRotation((Vector3)_dir);
				this.moveActor.get_handle().rotation = rotation;
			}
		}

		private void ActionMoveLerp(ActorRoot actor, uint nDelta, bool bReset)
		{
			if (actor == null || this.stopCondtion || this.stopLerpCondtion)
			{
				return;
			}
			Vector3 vector = Vector3.one;
			int num;
			if (!this.shouldUseAcceleration)
			{
				num = this.velocity * (int)nDelta / 1000;
			}
			else
			{
				long num2 = (long)this.lastLerpVelocity * (long)((ulong)nDelta) + (long)this.acceleration * (long)((ulong)nDelta) * (long)((ulong)nDelta) / 2L / 1000L;
				num2 /= 1000L;
				num = (int)num2;
				this.lastLerpVelocity += (int)((long)this.acceleration * (long)((ulong)nDelta)) / 1000;
			}
			vector = actor.myTransform.position;
			if (this.gravity < 0)
			{
				this.lerpDirection.y = 0;
				vector += (Vector3)this.lerpDirection.NormalizeTo(num);
				vector.y += (float)this.gravityControler.GetMotionLerpDistance((int)nDelta) / 1000f;
				VInt vInt;
				if (PathfindingUtility.GetGroundY(this.destPosition, out vInt) && vector.y < (float)vInt)
				{
					vector.y = (float)vInt;
				}
			}
			else
			{
				vector += (Vector3)this.lerpDirection.NormalizeTo(num);
			}
			actor.myTransform.position = vector;
		}

		public void ProcessInner(Action _action, Track _track, int delta)
		{
			VInt3 location = this.moveActor.get_handle().location;
			if (this.MoveType == ActorMoveType.Target && this.tarActor)
			{
				this.destPosition = this.tarActor.get_handle().location;
				if (this.tarActor && this.tarActor.get_handle().CharInfo != null)
				{
					CActorInfo charInfo = this.tarActor.get_handle().CharInfo;
					this.hitHeight = charInfo.iBulletHeight;
					VInt3 vInt = this.moveActor.get_handle().location - this.destPosition;
					vInt.y = 0;
					vInt = vInt.NormalizeTo(1000);
					this.destPosition += IntMath.Divide(vInt, (long)charInfo.iCollisionSize.x, 1000L);
				}
				this.destPosition.y = this.destPosition.y + this.hitHeight;
			}
			this.moveDirection = this.destPosition - location;
			this.lerpDirection = this.moveDirection;
			if (this.bMoveRotate)
			{
				this.RotateMoveBullet(this.moveDirection);
			}
			int num;
			if (!this.shouldUseAcceleration)
			{
				num = this.velocity * delta / 1000;
			}
			else
			{
				long num2 = (long)this.lastVelocity * (long)delta + (long)this.acceleration * (long)delta * (long)delta / 2L / 1000L;
				num2 /= 1000L;
				num = (int)num2;
				this.lastVelocity += this.acceleration * delta / 1000;
			}
			if ((long)num * (long)num >= this.moveDirection.get_sqrMagnitudeLong2D() && this.bReachDestStop)
			{
				int magnitude2D = (this.destPosition - this.moveActor.get_handle().location).get_magnitude2D();
				this.moveActor.get_handle().location = this.destPosition;
				this.stopCondtion = true;
				if (this.moveActor.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Bullet)
				{
					BulletWrapper bulletWrapper = this.moveActor.get_handle().ActorControl as BulletWrapper;
					if (bulletWrapper != null && bulletWrapper.GetMoveCollisiong())
					{
						bulletWrapper.SetMoveDelta(magnitude2D);
					}
				}
			}
			else
			{
				VInt3 location2;
				if (this.gravity < 0)
				{
					this.moveDirection.y = 0;
					location2 = location + this.moveDirection.NormalizeTo(num);
					location2.y += this.gravityControler.GetMotionDeltaDistance(delta);
					VInt vInt2;
					if (PathfindingUtility.GetGroundY(this.destPosition, out vInt2) && location2.y < vInt2.i)
					{
						location2.y = vInt2.i;
					}
				}
				else
				{
					location2 = location + this.moveDirection.NormalizeTo(num);
				}
				if (this.moveActor.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Bullet)
				{
					BulletWrapper bulletWrapper2 = this.moveActor.get_handle().ActorControl as BulletWrapper;
					if (bulletWrapper2 != null && bulletWrapper2.GetMoveCollisiong())
					{
						bulletWrapper2.SetMoveDelta(num);
					}
				}
				this.moveActor.get_handle().location = location2;
			}
			SkillUseContext refParamObject = _action.refParams.GetRefParamObject<SkillUseContext>("SkillContext");
			if (refParamObject != null)
			{
				refParamObject.EffectPos = this.moveActor.get_handle().location;
				refParamObject.EffectDir = this.moveDirection;
			}
		}

		public int ProcessSubdivide(Action _action, Track _track, int _localTime, int _count)
		{
			if (!this.moveActor || this.stopCondtion || _count <= 0)
			{
				return 0;
			}
			int num = _localTime - this.lastTime;
			this.lastTime = _localTime;
			int num2 = num / _count;
			int num3 = num - num2;
			this.lastTime -= num3;
			this.ProcessInner(_action, _track, num2);
			return num3;
		}

		public void Process(Action _action, Track _track, int _localTime)
		{
			if (!this.moveActor || this.stopCondtion)
			{
				return;
			}
			int delta = _localTime - this.lastTime;
			this.lastTime = _localTime;
			this.ProcessInner(_action, _track, delta);
		}
	}
}
