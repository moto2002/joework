using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using ResData;
using System;

namespace Assets.Scripts.GameLogic
{
	public class BaseEnergyLogic
	{
		protected ResHeroEnergyInfo cfgData;

		protected PoolObjHandle<ActorRoot> actor;

		private CrypticInt32 _nObjCurEp = 1;

		public EnergyType energyType = EnergyType.NoneResource;

		protected int deltaTime;

		public virtual int _actorEp
		{
			get
			{
				return this._nObjCurEp;
			}
			protected set
			{
				int num = (value <= this.actor.get_handle().ValueComponent.mActorValue[32].totalValue) ? ((value >= 0) ? value : 0) : this.actor.get_handle().ValueComponent.mActorValue[32].totalValue;
				CrypticInt32 nObjCurEp = this._nObjCurEp;
				if (nObjCurEp != num)
				{
					this._nObjCurEp = num;
					Singleton<EventRouter>.GetInstance().BroadCastEvent<PoolObjHandle<ActorRoot>, int, int>("HeroEnergyChange", this.actor, num, this.actorEpTotal);
				}
				if (num == this.actor.get_handle().ValueComponent.mActorValue[32].totalValue)
				{
					Singleton<EventRouter>.GetInstance().BroadCastEvent<PoolObjHandle<ActorRoot>, int, int>("HeroEnergyMax", this.actor, num, this.actorEpTotal);
				}
			}
		}

		public virtual int actorEpTotal
		{
			get
			{
				return this.actor.get_handle().ValueComponent.mActorValue[32].totalValue;
			}
		}

		public virtual int actorEpRecTotal
		{
			get
			{
				return this.actor.get_handle().ValueComponent.mActorValue[33].totalValue;
			}
		}

		public virtual void SetEpValue(int value)
		{
			this._actorEp = value;
		}

		public virtual void Init(PoolObjHandle<ActorRoot> _actor)
		{
			this.actor = _actor;
			this.cfgData = GameDataMgr.heroEnergyDatabin.GetDataByKey((uint)this.energyType);
			this.deltaTime = 0;
		}

		public virtual void Uninit()
		{
		}

		public virtual void UpdateLogic(int _delta)
		{
			if (this.deltaTime > 0)
			{
				this.deltaTime -= _delta;
				this.deltaTime = ((this.deltaTime <= 0) ? 0 : this.deltaTime);
			}
			if (this.Fit())
			{
				this.UpdateEpValue();
				this.ResetDeltaTime();
			}
		}

		public virtual void ResetEpValue(int epPercent)
		{
		}

		public virtual void EquipmentAddition()
		{
		}

		protected virtual bool Fit()
		{
			return this.deltaTime <= 0 && !this.actor.get_handle().ActorControl.GetNoAbilityFlag(ObjAbilityType.ObjAbility_RecoverEnergy) && !this.actor.get_handle().ActorControl.IsDeadState;
		}

		protected virtual void UpdateEpValue()
		{
		}

		protected virtual void ResetDeltaTime()
		{
			this.deltaTime = this.cfgData.iRecFrequency;
		}
	}
}
