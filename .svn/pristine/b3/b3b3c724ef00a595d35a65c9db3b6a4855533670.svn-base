using Assets.Scripts.Common;
using Assets.Scripts.GameLogic;
using Pathfinding;
using Pathfinding.RVO;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MPathfinding
{
	public bool enabled = true;

	private bool canMove;

	public bool checkNavNode;

	public int speed = 3000;

	public float rotationSpeed = 5f;

	public int pickNextWaypointDist = 2000;

	public VInt forwardLook = 1000;

	public float endReachedDistance = 0.05f;

	public bool closestOnPathCheck = true;

	protected Seeker seeker;

	protected PoolObjHandle<ActorRoot> actor = default(PoolObjHandle<ActorRoot>);

	protected Path path;

	protected int currentWaypointIndex;

	protected VInt3 lastFoundWaypointPosition;

	protected float lastFoundWaypointTime = -9999f;

	public VInt3 targetSearchPos = VInt3.zero;

	private VInt3 targetPos = VInt3.zero;

	private bool targetPosIsValid;

	protected VInt3 targetPoint;

	protected VInt3 targetDirection;

	public RVOController rvo
	{
		get;
		protected set;
	}

	public bool targetReached
	{
		get;
		protected set;
	}

	public bool hasCollidedWithAgents
	{
		get
		{
			return !(this.rvo == null) && this.rvo.enabled && this.rvo.rvoAgent != null && this.rvo.rvoAgent.hasCollided;
		}
	}

	public void UpdateLogic(int deltaTime)
	{
		if (this.rvo != null && this.rvo.enabled)
		{
			this.rvo.UpdateLogic(deltaTime);
			if (this.targetReached)
			{
				this.rvo.Move(VInt3.zero);
			}
		}
	}

	public void EnableRVO(bool enable)
	{
		if (this.rvo != null)
		{
			this.rvo.enabled = enable;
		}
	}

	public void Reset()
	{
		this.actor.Validate();
		if (this.rvo != null)
		{
			this.rvo.enabled = false;
		}
		this.seeker.RecyclePath();
		this.path = null;
	}

	public bool Init(PoolObjHandle<ActorRoot> InActor)
	{
		this.actor = InActor;
		this.seeker = this.actor.get_handle().GetComponent<Seeker>();
		if (this.seeker == null)
		{
			return false;
		}
		this.rvo = this.actor.get_handle().GetComponent<RVOController>();
		this.seeker.pathCallback = new OnPathDelegate(this.OnPathComplete);
		this.lastFoundWaypointPosition = this.GetFeetPosition();
		if (this.actor.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero || this.actor.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Monster)
		{
			this.checkNavNode = true;
		}
		if (this.rvo != null)
		{
			this.rvo.checkNavNode = this.checkNavNode;
		}
		return true;
	}

	public void InvalidPath()
	{
		this.seeker.RecyclePath();
		this.path = null;
	}

	public void SearchPath(VInt3 target)
	{
		if (this.path != null)
		{
			bool flag = true;
			long num = this.targetSearchPos.XZSqrMagnitude(ref target);
			if (num < 1L)
			{
				if (this.targetReached)
				{
					num = this.actor.get_handle().location.XZSqrMagnitude(ref this.targetPos);
					if (num < 100L)
					{
						flag = false;
					}
				}
				else
				{
					flag = false;
				}
			}
			if (!flag)
			{
				return;
			}
		}
		int num2;
		this.targetPosIsValid = PathfindingUtility.ValidateTarget(this.GetFeetPosition(), target, out this.targetPos, out num2);
		VInt3 feetPosition = this.GetFeetPosition();
		int actorCamp = this.actor.get_handle().TheActorMeta.ActorCamp;
		this.seeker.RecyclePath();
		this.path = null;
		this.path = this.seeker.StartPathEx(ref feetPosition, ref this.targetPos, actorCamp, null, -1);
		if (this.path == null)
		{
			return;
		}
		this.targetSearchPos = target;
		this.canMove = false;
		AstarPath.WaitForPath(this.path);
	}

	public virtual void OnTargetReached()
	{
		this.seeker.RecyclePath();
		this.path = null;
	}

	public virtual void OnPathComplete(Path _p)
	{
		this.path = _p;
		this.currentWaypointIndex = 0;
		this.targetReached = false;
		this.canMove = true;
	}

	public virtual VInt3 GetFeetPosition()
	{
		return this.actor.get_handle().location;
	}

	public void StopMove()
	{
		if (this.rvo != null)
		{
			this.rvo.Move(VInt3.zero);
		}
		this.seeker.RecyclePath();
		this.path = null;
	}

	public void Move(out VInt3 targetDir, int dt)
	{
		targetDir = VInt3.forward;
		if (this.targetReached || !this.canMove)
		{
			return;
		}
		ActorRoot handle = this.actor.get_handle();
		VInt3 location = handle.location;
		VInt3 vInt = this.CaculateDir(location);
		if (this.targetDirection.x != 0 || this.targetDirection.z != 0)
		{
			targetDir = this.targetDirection;
			targetDir.NormalizeTo(1000);
		}
		else
		{
			targetDir = handle.forward;
		}
		VInt3 vInt2 = handle.location;
		VInt3 vInt3 = vInt;
		vInt3 *= this.speed * dt / 1000;
		vInt3 /= 1000f;
		bool flag = this.rvo != null && this.rvo.enabled;
		bool hasReachedNavEdge = false;
		if (this.checkNavNode && !flag)
		{
			VInt groundY;
			vInt3 = PathfindingUtility.Move(this.actor, vInt3, out groundY, out hasReachedNavEdge, null);
			handle.groundY = groundY;
		}
		if ((handle.location - this.targetPos).get_sqrMagnitudeLong2D() <= vInt3.get_sqrMagnitudeLong2D())
		{
			if (this.checkNavNode && !this.targetPosIsValid)
			{
				vInt2 = handle.location + vInt3;
			}
			else
			{
				vInt2 = this.targetPos;
			}
			this.targetReached = true;
			this.OnTargetReached();
		}
		else
		{
			vInt2 = handle.location + vInt3;
		}
		if (flag)
		{
			VInt3 vInt4 = vInt2 - handle.location;
			vInt4 = IntMath.Divide(vInt4, 1000L, (long)dt);
			this.rvo.Move(vInt4);
		}
		else
		{
			handle.location = vInt2;
			handle.hasReachedNavEdge = hasReachedNavEdge;
		}
	}

	protected VInt3 CaculateDir(VInt3 currentPosition)
	{
		if (this.path == null || this.path.vectorPath == null || this.path.vectorPath.get_Count() == 0)
		{
			return VInt3.zero;
		}
		List<VInt3> vectorPath = this.path.vectorPath;
		if (vectorPath.get_Count() == 1)
		{
			vectorPath.Insert(0, currentPosition);
		}
		if (this.currentWaypointIndex >= vectorPath.get_Count())
		{
			this.currentWaypointIndex = vectorPath.get_Count() - 1;
		}
		if (this.currentWaypointIndex <= 1)
		{
			this.currentWaypointIndex = 1;
		}
		while (this.currentWaypointIndex < vectorPath.get_Count() - 1)
		{
			long num = vectorPath.get_Item(this.currentWaypointIndex).XZSqrMagnitude(currentPosition);
			if (num != 0L)
			{
				if (num < (long)this.pickNextWaypointDist * (long)this.pickNextWaypointDist)
				{
					VInt3 vInt = vectorPath.get_Item(this.currentWaypointIndex - 1);
					VInt3 vInt2 = vectorPath.get_Item(this.currentWaypointIndex);
					VInt3 vInt3 = vInt2 - vInt;
					long num2 = (long)vInt3.get_magnitude2D();
					long num3 = VInt3.DotXZLong(currentPosition - vInt, vInt3.NormalizeTo(1000));
					if (num3 >= num2 * 1000L)
					{
						this.lastFoundWaypointPosition = currentPosition;
						this.lastFoundWaypointTime = Time.time;
						this.currentWaypointIndex++;
						continue;
					}
				}
				IL_184:
				this.targetPoint = this.CalculateTargetPoint(currentPosition, vectorPath.get_Item(this.currentWaypointIndex - 1), vectorPath.get_Item(this.currentWaypointIndex));
				this.targetDirection = this.targetPoint - currentPosition;
				this.targetDirection.y = 0;
				return this.targetDirection.NormalizeTo(1000);
			}
			this.lastFoundWaypointPosition = currentPosition;
			this.lastFoundWaypointTime = Time.time;
			this.currentWaypointIndex++;
		}
		goto IL_184;
	}

	protected virtual void RotateTowards(VInt3 dir)
	{
	}

	protected VInt3 CalculateTargetPoint(VInt3 p, VInt3 a, VInt3 b)
	{
		if (a.x == b.x && a.z == b.z)
		{
			return a;
		}
		VFactor vFactor = AstarMath.NearestPointFactorXZ(ref a, ref b, ref p);
		long num = VInt3.Lerp(a, b, vFactor).XZSqrMagnitude(ref p);
		int num2 = IntMath.Sqrt(num);
		long num3 = a.XZSqrMagnitude(ref b);
		int num4 = IntMath.Sqrt(num3);
		if (num4 == 0)
		{
			return b;
		}
		int num5 = Mathf.Clamp(this.forwardLook.i - num2, 0, this.forwardLook.i);
		VFactor vFactor2 = new VFactor((long)num5 * vFactor.den + vFactor.nom * (long)num4, (long)num4 * vFactor.den);
		if (vFactor2 > VFactor.one)
		{
			vFactor2 = VFactor.one;
		}
		else if (vFactor2 < VFactor.zero)
		{
			vFactor2 = VFactor.zero;
		}
		vFactor2.strip();
		return VInt3.Lerp(a, b, vFactor2);
	}
}
