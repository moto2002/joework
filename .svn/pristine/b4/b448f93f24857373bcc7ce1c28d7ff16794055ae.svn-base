using AGE;
using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using System;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
	public abstract class BaseSkill : PooledClassObject
	{
		public int SkillID;

		public string ActionName = string.Empty;

		protected PoolObjHandle<Action> curAction = default(PoolObjHandle<Action>);

		public SkillUseContext skillContext = new SkillUseContext();

		public bool bAgeImmeExcute;

		private ActionStopDelegate OnActionStopDelegate;

		public bool isFinish
		{
			get
			{
				return !this.curAction;
			}
		}

		public PoolObjHandle<Action> CurAction
		{
			get
			{
				return this.curAction;
			}
		}

		public virtual bool isBullet
		{
			get
			{
				return false;
			}
		}

		public virtual bool isBuff
		{
			get
			{
				return false;
			}
		}

		public override void OnUse()
		{
			base.OnUse();
			this.SkillID = 0;
			this.ActionName = string.Empty;
			this.curAction.Release();
			this.skillContext.Reset();
			this.bAgeImmeExcute = false;
			this.OnActionStopDelegate = new ActionStopDelegate(this.OnActionStoped);
		}

		public override void OnRelease()
		{
			this.SkillID = 0;
			this.ActionName = string.Empty;
			this.curAction.Release();
			this.OnActionStopDelegate = null;
			base.OnRelease();
		}

		public virtual void Stop()
		{
			if (this.curAction)
			{
				this.curAction.get_handle().Stop(false);
				this.curAction.Release();
			}
		}

		private bool UseImpl(PoolObjHandle<ActorRoot> user)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			VInt3 value = VInt3.forward;
			switch (this.skillContext.AppointType)
			{
			case 0:
			case 1:
				flag = true;
				break;
			case 2:
				flag2 = true;
				break;
			case 3:
				flag3 = true;
				value = this.skillContext.UseVector;
				if (this.skillContext.TargetID != 0u)
				{
					PoolObjHandle<ActorRoot> actor = Singleton<GameObjMgr>.GetInstance().GetActor(this.skillContext.TargetID);
					if (actor)
					{
						Vector3 vector = actor.get_handle().myTransform.position - user.get_handle().myTransform.position;
						vector.y = 0f;
						vector.Normalize();
						value = (VInt3)vector;
					}
				}
				break;
			case 4:
				flag2 = true;
				flag3 = true;
				value = this.skillContext.EndVector - this.skillContext.UseVector;
				if (value.get_sqrMagnitudeLong() < 1L)
				{
					value = VInt3.forward;
				}
				break;
			}
			if (flag && !this.skillContext.TargetActor)
			{
				return false;
			}
			if (flag)
			{
				this.curAction = new PoolObjHandle<Action>(ActionManager.Instance.PlayAction(this.ActionName, true, false, new GameObject[]
				{
					user.get_handle().gameObject,
					this.skillContext.TargetActor.get_handle().gameObject
				}));
			}
			else
			{
				this.curAction = new PoolObjHandle<Action>(ActionManager.Instance.PlayAction(this.ActionName, true, false, new GameObject[]
				{
					user.get_handle().gameObject
				}));
			}
			if (!this.curAction)
			{
				return false;
			}
			this.curAction.get_handle().onActionStop += this.OnActionStopDelegate;
			this.curAction.get_handle().refParams.AddRefParam("SkillObj", this);
			this.curAction.get_handle().refParams.AddRefParam("SkillContext", this.skillContext);
			if (flag)
			{
				this.curAction.get_handle().refParams.AddRefParam("TargetActor", this.skillContext.TargetActor);
			}
			if (flag2)
			{
				this.curAction.get_handle().refParams.SetRefParam("_TargetPos", this.skillContext.UseVector);
			}
			if (flag3)
			{
				this.curAction.get_handle().refParams.SetRefParam("_TargetDir", value);
			}
			this.curAction.get_handle().refParams.SetRefParam("_BulletPos", this.skillContext.BulletPos);
			this.curAction.get_handle().refParams.SetRefParam("_BulletUseDir", user.get_handle().forward);
			if (this.bAgeImmeExcute)
			{
				this.curAction.get_handle().UpdateLogic((int)Singleton<FrameSynchr>.GetInstance().FrameDelta);
			}
			return true;
		}

		public virtual bool Use(PoolObjHandle<ActorRoot> user, ref SkillUseParam param)
		{
			if (!user || this.skillContext == null || string.IsNullOrEmpty(this.ActionName))
			{
				return false;
			}
			this.skillContext.Copy(ref param);
			return this.UseImpl(user);
		}

		public virtual bool Use(PoolObjHandle<ActorRoot> user)
		{
			return user && this.skillContext != null && !string.IsNullOrEmpty(this.ActionName) && this.UseImpl(user);
		}

		public virtual void OnActionStoped(ref PoolObjHandle<Action> action)
		{
			action.get_handle().onActionStop -= this.OnActionStopDelegate;
			if (!this.curAction)
			{
				return;
			}
			if (action == this.curAction)
			{
				this.curAction.Release();
			}
		}

		public PoolObjHandle<ActorRoot> GetTargetActor()
		{
			SkillUseContext skillUseContext = this.GetSkillUseContext();
			if (skillUseContext == null)
			{
				return new PoolObjHandle<ActorRoot>(null);
			}
			return skillUseContext.TargetActor;
		}

		public SkillUseContext GetSkillUseContext()
		{
			if (!this.curAction)
			{
				return null;
			}
			return this.curAction.get_handle().refParams.GetRefParamObject<SkillUseContext>("SkillContext");
		}
	}
}
