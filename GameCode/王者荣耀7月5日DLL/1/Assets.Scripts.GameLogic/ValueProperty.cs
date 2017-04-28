using Assets.Scripts.Common;
using Assets.Scripts.GameLogic.DataCenter;
using Assets.Scripts.GameSystem;
using ResData;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
	public class ValueProperty : LogicComponent
	{
		private struct RangeConfig
		{
			public int MinValue;

			public int MaxValue;

			public int Attenuation;

			public static int Clamp(int value, int min, int max)
			{
				return (value >= min) ? ((value <= max) ? value : max) : min;
			}

			public bool Intersect(int InBase, int InMin, int InMax, out int OutMin, out int OutMax)
			{
				OutMin = (OutMax = 0);
				int num = this.MinValue + InBase;
				int num2 = this.MaxValue + InBase;
				if (InMax < num)
				{
					return false;
				}
				if (InMin > num2)
				{
					return false;
				}
				OutMin = ValueProperty.RangeConfig.Clamp(InMin, num, num2);
				OutMax = ValueProperty.RangeConfig.Clamp(InMax, num, num2);
				return true;
			}
		}

		private CrypticInt32 _nObjCurHp = 1;

		public PropertyHelper mActorValue;

		public BaseEnergyLogic mEnergy;

		private int nHpRecoveryTick;

		private int nEpRecoveryTick;

		private CrypticInt32 _soulLevel = 1;

		private CrypticInt32 _soulExp = 0;

		private CrypticInt32 _soulMaxExp = 0;

		private CrypticInt32 m_goldCoinInBattle = 0;

		private CrypticInt32 m_goldCoinIncomeInBattle = 0;

		private CrypticInt32 m_MaxGoldCoinIncomeInBattle = 0;

		public ActorValueStatistic ObjValueStatistic;

		private static readonly ValueProperty.RangeConfig[] s_SpeedUpRanges = new ValueProperty.RangeConfig[0];

		private static readonly ValueProperty.RangeConfig[] s_SpeedDownRanges = new ValueProperty.RangeConfig[0];

		public event ValueChangeDelegate HpChgEvent
		{
			[MethodImpl(32)]
			add
			{
				this.HpChgEvent = (ValueChangeDelegate)Delegate.Combine(this.HpChgEvent, value);
			}
			[MethodImpl(32)]
			remove
			{
				this.HpChgEvent = (ValueChangeDelegate)Delegate.Remove(this.HpChgEvent, value);
			}
		}

		public event ValueChangeDelegate SoulLevelChgEvent
		{
			[MethodImpl(32)]
			add
			{
				this.SoulLevelChgEvent = (ValueChangeDelegate)Delegate.Combine(this.SoulLevelChgEvent, value);
			}
			[MethodImpl(32)]
			remove
			{
				this.SoulLevelChgEvent = (ValueChangeDelegate)Delegate.Remove(this.SoulLevelChgEvent, value);
			}
		}

		public int actorHp
		{
			get
			{
				return this._nObjCurHp;
			}
			set
			{
				int num = (value <= this.mActorValue[5].totalValue) ? ((value >= 0) ? value : 0) : this.mActorValue[5].totalValue;
				CrypticInt32 nObjCurHp = this._nObjCurHp;
				if (nObjCurHp != num)
				{
					this._nObjCurHp = num;
					if (this.HpChgEvent != null)
					{
						this.HpChgEvent();
					}
					Singleton<EventRouter>.GetInstance().BroadCastEvent<PoolObjHandle<ActorRoot>, int, int>("HeroHpChange", this.actorPtr, num, this.actorHpTotal);
				}
			}
		}

		public int actorHpTotal
		{
			get
			{
				return this.mActorValue[5].totalValue;
			}
		}

		public int actorEp
		{
			get
			{
				return (this.mEnergy == null) ? 0 : this.mEnergy._actorEp;
			}
			set
			{
				if (this.mEnergy != null)
				{
					this.mEnergy.SetEpValue(value);
				}
			}
		}

		public int actorEpTotal
		{
			get
			{
				return (this.mEnergy == null) ? 1 : this.mEnergy.actorEpTotal;
			}
		}

		public int actorEpRecTotal
		{
			get
			{
				return (this.mEnergy == null) ? 0 : this.mEnergy.actorEpRecTotal;
			}
		}

		public int actorSoulLevel
		{
			get
			{
				return this._soulLevel;
			}
			set
			{
				if (!Singleton<BattleLogic>.get_instance().m_LevelContext.IsSoulGrow())
				{
					return;
				}
				VFactor vFactor = new VFactor((long)this.actorHp, (long)this.mActorValue[5].totalValue);
				VFactor vFactor2 = new VFactor((long)this.actorEp, (long)this.mActorValue[32].totalValue);
				bool flag = value > this._soulLevel || value == 1;
				this._soulLevel = value;
				this.mActorValue.SoulLevel = this.actorSoulLevel;
				this.SetSoulMaxExp();
				this.actorHp = (vFactor * (long)this.mActorValue[5].totalValue).get_roundInt();
				if (this.mActorValue[32].totalValue > 0)
				{
					this.actorEp = (vFactor2 * (long)this.mActorValue[32].totalValue).get_roundInt();
				}
				if (flag && this.SoulLevelChgEvent != null)
				{
					this.SoulLevelChgEvent();
				}
				if (flag)
				{
					if (this.actor.SkillControl != null)
					{
						this.actor.SkillControl.m_iSkillPoint = value - this.actor.SkillControl.GetAllSkillLevel();
					}
					Singleton<EventRouter>.GetInstance().BroadCastEvent<PoolObjHandle<ActorRoot>, int>("HeroSoulLevelChange", this.actorPtr, value);
				}
			}
		}

		public int actorSoulExp
		{
			get
			{
				return this._soulExp;
			}
			set
			{
				this._soulExp = value;
			}
		}

		public int actorSoulMaxExp
		{
			get
			{
				return this._soulMaxExp;
			}
			set
			{
				this._soulMaxExp = value;
			}
		}

		public int actorMoveSpeed
		{
			get
			{
				if (this.mActorValue == null)
				{
					return 0;
				}
				int totalValue = this.mActorValue[15].totalValue;
				int baseValue = this.mActorValue[15].baseValue;
				int num = totalValue - baseValue;
				bool flag = num > 0;
				if ((flag && (ValueProperty.s_SpeedUpRanges == null || ValueProperty.s_SpeedUpRanges.Length == 0)) || (!flag && (ValueProperty.s_SpeedDownRanges == null || ValueProperty.s_SpeedDownRanges.Length == 0)))
				{
					return totalValue;
				}
				return (!flag) ? ValueProperty.HandleSpeedDown(baseValue, totalValue, baseValue) : ValueProperty.HandleSpeedUp(baseValue, totalValue, baseValue);
			}
		}

		public override void OnUse()
		{
			base.OnUse();
			this.ClearVariables();
		}

		public override void Born(ActorRoot owner)
		{
			base.Born(owner);
			this._nObjCurHp = 1;
			this._soulLevel = 1;
			this._soulExp = 0;
			this._soulMaxExp = 0;
			this.m_goldCoinInBattle = 0;
			this.m_goldCoinIncomeInBattle = 0;
		}

		private void ClearVariables()
		{
			this._nObjCurHp = 1;
			this.mActorValue = null;
			this.nHpRecoveryTick = 0;
			this.nEpRecoveryTick = 0;
			this._soulLevel = 1;
			this._soulExp = 0;
			this._soulMaxExp = 0;
			this.HpChgEvent = null;
			this.SoulLevelChgEvent = null;
			this.ObjValueStatistic = null;
			this.m_goldCoinInBattle = 0;
			this.m_goldCoinIncomeInBattle = 0;
			this.m_MaxGoldCoinIncomeInBattle = 0;
			this.mEnergy = null;
		}

		public void ChangeActorEp(int value, int addType)
		{
			if (addType == 0)
			{
				this.actorEp += value;
			}
			else if (addType == 1)
			{
				this.actorEp = (int)((long)this.actorEp + (long)(this.actorEpTotal * value) / 10000L);
			}
		}

		public bool IsEnergyType(EnergyType energyType)
		{
			return this.mActorValue.EnergyType == energyType;
		}

		public override void Init()
		{
			base.Init();
			if (this.mActorValue == null)
			{
				this.mActorValue = new PropertyHelper();
				this.mActorValue.Init(ref this.actor.TheActorMeta);
			}
			if (this.ObjValueStatistic == null)
			{
				this.ObjValueStatistic = new ActorValueStatistic();
			}
			this.mEnergy = Singleton<EnergyCreater<BaseEnergyLogic, EnergyAttribute>>.GetInstance().Create((int)this.mActorValue.EnergyType);
			if (this.mEnergy == null)
			{
				this.mEnergy = new NoEnergy();
			}
			this.mEnergy.Init(this.actorPtr);
			this.actorHp = 0;
			this.actorEp = 0;
			this.SetHpAndEpToInitialValue(10000, 10000);
		}

		public override void Uninit()
		{
			base.Uninit();
			if (this.mEnergy != null)
			{
				this.mEnergy.Uninit();
			}
		}

		public void SetHpAndEpToInitialValue(int hpPercent = 10000, int epPercent = 10000)
		{
			this.actorHp = this.mActorValue[5].totalValue * hpPercent / 10000;
			this.mEnergy.ResetEpValue(epPercent);
		}

		public override void Deactive()
		{
			this.ClearVariables();
			base.Deactive();
		}

		public override void Reactive()
		{
			base.Reactive();
			this.Init();
		}

		public void ForceSoulLevelUp()
		{
			this.ForceSetSoulLevel(this.actorSoulLevel + 1);
		}

		public void ForceSetSoulLevel(int inNewLevel)
		{
			int maxSoulLvl = ValueProperty.GetMaxSoulLvl();
			if (inNewLevel > maxSoulLvl)
			{
				inNewLevel = maxSoulLvl;
			}
			if (inNewLevel < 1)
			{
				inNewLevel = 1;
			}
			this.actorSoulLevel = inNewLevel;
			int num = inNewLevel - 1;
			if (num >= 1)
			{
				ResSoulLvlUpInfo resSoulLvlUpInfo = Singleton<BattleLogic>.get_instance().incomeCtrl.QuerySoulLvlUpInfo((uint)num);
				if (resSoulLvlUpInfo != null)
				{
					this.actorSoulExp = (int)(resSoulLvlUpInfo.dwExp + 1u);
				}
			}
			else
			{
				this.actorSoulExp = 1;
			}
		}

		public void RecoverHp()
		{
			int nAddHp = this.actorHpTotal - this.actorHp;
			this.actor.ActorControl.ReviveHp(nAddHp);
		}

		public void RecoverEp()
		{
			if (!this.actor.ActorControl.IsDeadState)
			{
				this.actorEp = this.actorEpTotal;
			}
		}

		public override void Fight()
		{
			base.Fight();
			this.nHpRecoveryTick = 0;
			this.nEpRecoveryTick = 0;
			DebugHelper.Assert(this.mActorValue != null, "mActorValue = null data is error");
			if (this.mActorValue != null)
			{
				VFactor hpRate = this.GetHpRate();
				DebugHelper.Assert(this.actor != null, "actor is null ? impossible...");
				if (this.actor != null)
				{
					bool bPVPLevel = true;
					SLevelContext curLvelContext = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
					if (curLvelContext != null)
					{
						bPVPLevel = curLvelContext.IsMobaMode();
					}
					this.mActorValue.AddSymbolPageAttToProp(ref this.actor.TheActorMeta, bPVPLevel);
					IGameActorDataProvider actorDataProvider = Singleton<ActorDataCenter>.get_instance().GetActorDataProvider(GameActorDataProviderType.ServerDataProvider);
					ActorServerData actorServerData = default(ActorServerData);
					if (actorDataProvider != null && actorDataProvider.GetActorServerData(ref this.actor.TheActorMeta, ref actorServerData))
					{
						this.mActorValue.SetSkinProp((uint)this.actor.TheActorMeta.ConfigId, actorServerData.SkinId, true);
					}
				}
				this.SetHpByRate(hpRate);
			}
		}

		public override void UpdateLogic(int nDelta)
		{
			this.UpdateHpRecovery(nDelta);
			if (this.mEnergy != null)
			{
				this.mEnergy.UpdateLogic(nDelta);
			}
		}

		private void UpdateHpRecovery(int nDelta)
		{
			if (this.actor.ActorControl.IsDeadState)
			{
				this.nHpRecoveryTick = 0;
			}
			else
			{
				this.nHpRecoveryTick += nDelta;
				if (this.nHpRecoveryTick >= 5000)
				{
					this.actorHp += this.mActorValue[16].totalValue;
					this.nHpRecoveryTick -= 5000;
				}
			}
		}

		public void SetSoulMaxExp()
		{
			ResSoulLvlUpInfo resSoulLvlUpInfo = Singleton<BattleLogic>.get_instance().incomeCtrl.QuerySoulLvlUpInfo((uint)this.actorSoulLevel);
			if (resSoulLvlUpInfo == null)
			{
				return;
			}
			this.actorSoulMaxExp = (int)resSoulLvlUpInfo.dwExp;
		}

		public void AddSoulExp(int addVal, bool bFloatDigit, AddSoulType type)
		{
			if (!Singleton<BattleLogic>.get_instance().m_LevelContext.IsSoulGrow())
			{
				return;
			}
			this.actorSoulExp += addVal;
			while (this.actorSoulExp >= this.actorSoulMaxExp)
			{
				this.actorSoulExp -= this.actorSoulMaxExp;
				int num = this.actorSoulLevel + 1;
				int maxSoulLvl = ValueProperty.GetMaxSoulLvl();
				if (num > maxSoulLvl)
				{
					this.actorSoulLevel = maxSoulLvl;
					this.actorSoulExp = this.actorSoulMaxExp;
					break;
				}
				this.actorSoulLevel = num;
				this.ObjValueStatistic.iSoulExpMax = ((this.ObjValueStatistic.iSoulExpMax <= addVal) ? addVal : this.ObjValueStatistic.iSoulExpMax);
			}
			if (bFloatDigit && addVal > 0 && this.actor.Visible && ActorHelper.IsHostCtrlActor(ref this.actorPtr))
			{
				Singleton<CBattleSystem>.GetInstance().CreateBattleFloatDigit(addVal, DIGIT_TYPE.ReceiveSpirit, this.actor.myTransform.position);
			}
			Singleton<EventRouter>.GetInstance().BroadCastEvent<PoolObjHandle<ActorRoot>, int, int, int>("HeroSoulExpChange", this.actorPtr, addVal, this.actorSoulExp, this.actorSoulMaxExp);
		}

		public void ChangeGoldCoinInBattle(int changeValue, bool isIncome, bool floatDigit = false, Vector3 position = default(Vector3), bool isLastHit = false, PoolObjHandle<ActorRoot> target = default(PoolObjHandle<ActorRoot>))
		{
			int num = this.m_goldCoinInBattle;
			this.m_goldCoinInBattle += changeValue;
			if (changeValue > 0 && isIncome)
			{
				this.m_goldCoinIncomeInBattle += changeValue;
				this.m_MaxGoldCoinIncomeInBattle = ((this.m_MaxGoldCoinIncomeInBattle <= changeValue) ? changeValue : this.m_MaxGoldCoinIncomeInBattle);
			}
			DebugHelper.Assert(this.m_goldCoinInBattle >= 0, "Wo ri, zhe zenme keneng");
			if (floatDigit && changeValue > 0 && isIncome && this.actor.Visible && ActorHelper.IsHostCtrlActor(ref this.actorPtr))
			{
				if (position.x == 0f && position.y == 0f && position.z == 0f)
				{
					position = this.actor.myTransform.position;
				}
				Singleton<CBattleSystem>.GetInstance().CreateBattleFloatDigit(changeValue, (!isLastHit) ? DIGIT_TYPE.ReceiveGoldCoinInBattle : DIGIT_TYPE.ReceiveLastHitGoldCoinInBattle, position);
			}
			Singleton<EventRouter>.GetInstance().BroadCastEvent<PoolObjHandle<ActorRoot>, int, bool, PoolObjHandle<ActorRoot>>("HeroGoldCoinInBattleChange", this.actorPtr, changeValue, isIncome, target);
		}

		public int GetGoldCoinInBattle()
		{
			return this.m_goldCoinInBattle;
		}

		public void ChangePhyAtkByPhyDefence()
		{
			ValueDataInfo valueDataInfo = this.mActorValue[1];
			valueDataInfo.addValue -= valueDataInfo.totalAddValueByDefence;
			long num = (long)this.mActorValue[3].totalValue * (long)valueDataInfo.convertRatioByDefence / 10000L;
			valueDataInfo.totalAddValueByDefence = (int)num;
			valueDataInfo.addValue += valueDataInfo.totalAddValueByDefence;
		}

		public int GetGoldCoinIncomeInBattle()
		{
			return this.m_goldCoinIncomeInBattle;
		}

		public int GetMaxGoldCoinIncomeInBattle()
		{
			return this.m_MaxGoldCoinIncomeInBattle;
		}

		public static int GetMaxSoulLvl()
		{
			return Singleton<BattleLogic>.get_instance().incomeCtrl.GetSoulLvlUpInfoList().get_Count();
		}

		public VFactor GetHpRate()
		{
			DebugHelper.Assert(this.actorHpTotal > 0, " {0} GetHpRate actorHpTotal is zero", new object[]
			{
				this.actor.TheStaticData.TheResInfo.Name
			});
			if (this.actorHpTotal > 0)
			{
				return new VFactor((long)this.actorHp, (long)this.actorHpTotal);
			}
			return VFactor.one;
		}

		public void SetHpByRate(VFactor hpRate)
		{
			DebugHelper.Assert(hpRate.den != 0L, "SetHpByRate hpRate den is zero");
			if (hpRate.den != 0L)
			{
				this.actorHp = (hpRate * (long)this.actorHpTotal).get_roundInt();
			}
		}

		private static int HandleSpeedUp(int baseValue, int totalValue, int orignalSpeed)
		{
			int num = 0;
			bool flag = false;
			for (int i = 0; i < ValueProperty.s_SpeedUpRanges.Length; i++)
			{
				ValueProperty.RangeConfig rangeConfig = ValueProperty.s_SpeedUpRanges[i];
				int num2 = 0;
				int num3 = 0;
				if (rangeConfig.Intersect(baseValue, orignalSpeed, totalValue, out num2, out num3))
				{
					flag = true;
					int num4 = num3 - num2;
					num4 *= 100 - rangeConfig.Attenuation;
					num4 /= 100;
					num += num4;
				}
				else if (flag)
				{
					break;
				}
			}
			return orignalSpeed + num;
		}

		private static int HandleSpeedDown(int baseValue, int totalValue, int orignalSpeed)
		{
			int num = 0;
			bool flag = false;
			for (int i = 0; i < ValueProperty.s_SpeedDownRanges.Length; i++)
			{
				ValueProperty.RangeConfig rangeConfig = ValueProperty.s_SpeedDownRanges[i];
				int num2 = 0;
				int num3 = 0;
				if (rangeConfig.Intersect(baseValue, totalValue, orignalSpeed, out num2, out num3))
				{
					flag = true;
					int num4 = num3 - num2;
					num4 *= 100 - rangeConfig.Attenuation;
					num4 /= 100;
					num += num4;
				}
				else if (flag)
				{
					break;
				}
			}
			return orignalSpeed - num;
		}

		public void OnValuePropertyChangeByMgcEffect()
		{
			int totalEftRatioByMgc = this.actor.ValueComponent.mActorValue[5].totalEftRatioByMgc;
			int totalOldEftRatioByMgc = this.actor.ValueComponent.mActorValue[5].totalOldEftRatioByMgc;
			this.actor.ValueComponent.mActorValue[5].totalEftRatioByMgc = totalOldEftRatioByMgc;
			VFactor vFactor = new VFactor((long)this.actor.ValueComponent.actorHp, (long)this.actor.ValueComponent.mActorValue[5].totalValue);
			this.actor.ValueComponent.mActorValue[5].totalEftRatioByMgc = totalEftRatioByMgc;
			this.actor.ValueComponent.mActorValue[5].totalEftValueByMgc = totalEftRatioByMgc * this.actor.ValueComponent.mActorValue[2].totalValue / 10000;
			if (!this.actor.ActorControl.IsDeadState)
			{
				this.actor.ValueComponent.actorHp = (vFactor * (long)this.actor.ValueComponent.mActorValue[5].totalValue).get_roundInt();
			}
			totalEftRatioByMgc = this.actor.ValueComponent.mActorValue[1].totalEftRatioByMgc;
			this.actor.ValueComponent.mActorValue[1].totalEftValueByMgc = totalEftRatioByMgc * this.actor.ValueComponent.mActorValue[2].totalValue / 10000;
		}
	}
}
