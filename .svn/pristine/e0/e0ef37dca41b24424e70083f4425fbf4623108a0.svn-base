using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.DataCenter;
using Assets.Scripts.GameLogic.GameKernal;
using Assets.Scripts.GameSystem;
using CSProtocol;
using ResData;
using System;

namespace Assets.Scripts.GameLogic
{
	public class PropertyHelper
	{
		private CrypticInt32 _level = 1;

		private CrypticInt32 _curExp = 0;

		private CrypticInt32 _maxExp = 0;

		private CrypticInt32 _star = 1;

		private CrypticInt32 _quality = 1;

		private CrypticInt32 _subQuality = 0;

		private int _epRecFrequency;

		private CrypticInt32 _soulLevel = 1;

		private EnergyType _energyType = EnergyType.NoneResource;

		private ValueDataInfo[] mActorValue = new ValueDataInfo[37];

		public ActorMeta m_theActorMeta;

		public static int[] s_symbolPropValAddArr = new int[37];

		public int SoulLevel
		{
			get
			{
				return this._soulLevel;
			}
			set
			{
				this._soulLevel = value;
			}
		}

		public EnergyType EnergyType
		{
			get
			{
				return this._energyType;
			}
			set
			{
				this._energyType = value;
			}
		}

		public int actorLvl
		{
			get
			{
				return this._level;
			}
			set
			{
				ResHeroLvlUpInfo dataByKey = GameDataMgr.heroLvlUpDatabin.GetDataByKey((uint)value);
				if (dataByKey != null)
				{
					this._level = value;
					this._maxExp = (int)dataByKey.dwExp;
				}
			}
		}

		public int actorStar
		{
			get
			{
				return this._star;
			}
			set
			{
				this._star = value;
				if (value > 1)
				{
					IGameActorDataProvider actorDataProvider = Singleton<ActorDataCenter>.get_instance().GetActorDataProvider(GameActorDataProviderType.StaticLobbyDataProvider);
					this.m_theActorMeta = new ActorMeta
					{
						ConfigId = this.m_theActorMeta.ConfigId
					};
					ActorPerStarLvData actorPerStarLvData = default(ActorPerStarLvData);
					if (actorDataProvider.GetActorStaticPerStarLvData(ref this.m_theActorMeta, (ActorStarLv)value, ref actorPerStarLvData))
					{
						this.mActorValue[5].growValue = actorPerStarLvData.PerLvHp;
						this.mActorValue[1].growValue = actorPerStarLvData.PerLvAd;
						this.mActorValue[2].growValue = actorPerStarLvData.PerLvAp;
						this.mActorValue[3].growValue = actorPerStarLvData.PerLvDef;
						this.mActorValue[4].growValue = actorPerStarLvData.PerLvRes;
					}
				}
			}
		}

		public int actorQuality
		{
			get
			{
				return this._quality;
			}
			set
			{
				this._quality = value;
				this._subQuality = 0;
			}
		}

		public int actorSubQuality
		{
			get
			{
				return this._subQuality;
			}
			set
			{
				this._subQuality = value;
			}
		}

		public int actorMaxExp
		{
			get
			{
				return this._maxExp;
			}
			set
			{
				this._maxExp = value;
			}
		}

		public int actorExp
		{
			get
			{
				return this._curExp;
			}
			set
			{
				this._curExp = value;
			}
		}

		public ValueDataInfo this[RES_FUNCEFT_TYPE key]
		{
			get
			{
				return this.mActorValue[key];
			}
		}

		public PropertyHelper()
		{
			this._level = 1;
			this._curExp = 0;
			this._maxExp = 0;
			this._star = 1;
			this._quality = 1;
			this._subQuality = 0;
			this._soulLevel = 1;
		}

		public void OnHeroSoulLvlUp(PoolObjHandle<ActorRoot> hero, int level)
		{
			PoolObjHandle<ActorRoot> captain = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().Captain;
			if (hero && hero == captain)
			{
				this.SoulLevel = level;
			}
		}

		public int GenericCalculator(ValueDataInfo vd, int baseValue)
		{
			int num = baseValue + (this.SoulLevel - 1) * vd.growValue / 10000;
			long num2 = (long)(num + vd.addValue - vd.decValue) * (long)(10000 + vd.addRatio - vd.decRatio) / 10000L + (long)vd.addValueOffRatio;
			if (vd.maxLimitValue > 0)
			{
				num2 = ((num2 <= (long)vd.maxLimitValue) ? num2 : ((long)vd.maxLimitValue));
			}
			return (int)num2;
		}

		public int GenericBaseCalculator(ValueDataInfo vd, int baseValue)
		{
			return baseValue + (this.SoulLevel - 1) * vd.growValue / 10000;
		}

		public int GrowCalculator(ValueDataInfo vd, ValueDataType type)
		{
			if (type == ValueDataType.TYPE_TOTAL)
			{
				return this.GenericCalculator(vd, vd.baseValue);
			}
			return this.GenericBaseCalculator(vd, vd.baseValue);
		}

		public int EpNumericalCalculator(ValueDataInfo vd, int baseValue)
		{
			int num = baseValue + (this.SoulLevel - 1) * vd.growValue;
			long num2 = (long)(num + vd.addValue - vd.decValue) * (long)(10000 + vd.addRatio - vd.decRatio) / 10000L + (long)vd.addValueOffRatio;
			return (int)num2;
		}

		public int EpBaseNumericalCalculator(ValueDataInfo vd, int baseValue)
		{
			return baseValue + (this.SoulLevel - 1) * vd.growValue;
		}

		public int EpProportionCalculator(ValueDataInfo vd, int baseValue)
		{
			int num = baseValue / 10000 + (this.SoulLevel - 1) * vd.growValue / 10000;
			long num2 = (long)(num + vd.addValue - vd.decValue) * (long)(10000 + vd.addRatio - vd.decRatio) / 10000L + (long)vd.addValueOffRatio;
			return (int)num2;
		}

		public int EpBaseProportionCalculator(ValueDataInfo vd, int baseValue)
		{
			return baseValue / 10000 + (this.SoulLevel - 1) * vd.growValue / 10000;
		}

		public int EpGrowCalculator(ValueDataInfo vd, ValueDataType type)
		{
			if (type == ValueDataType.TYPE_TOTAL)
			{
				return this.EpNumericalCalculator(vd, vd.baseValue);
			}
			return this.EpBaseNumericalCalculator(vd, vd.baseValue);
		}

		public int EpRecCalculator(ValueDataInfo vd, ValueDataType type)
		{
			if (type == ValueDataType.TYPE_TOTAL)
			{
				return this.EpProportionCalculator(vd, vd.baseValue);
			}
			return this.EpBaseProportionCalculator(vd, vd.baseValue);
		}

		public int DynamicAdjustor(ValueDataInfo vd, ValueDataType type)
		{
			if (type == ValueDataType.TYPE_TOTAL)
			{
				return this.GenericCalculator(vd, DynamicProperty.Adjustor(vd));
			}
			return this.GenericBaseCalculator(vd, DynamicProperty.Adjustor(vd));
		}

		public int DynamicAdjustorForMgcEffect(ValueDataInfo vd, ValueDataType type)
		{
			if (type == ValueDataType.TYPE_TOTAL)
			{
				int num = this.GenericCalculator(vd, DynamicProperty.Adjustor(vd));
				return num + vd.totalEftValueByMgc;
			}
			return this.GenericBaseCalculator(vd, DynamicProperty.Adjustor(vd));
		}

		public void Init(ref ActorMeta actorMeta)
		{
			this.InitValueDataArr(ref actorMeta, false);
			IGameActorDataProvider actorDataProvider = Singleton<ActorDataCenter>.get_instance().GetActorDataProvider(GameActorDataProviderType.ServerDataProvider);
			ActorServerData actorServerData = default(ActorServerData);
			if (actorDataProvider.GetActorServerData(ref actorMeta, ref actorServerData))
			{
				this.actorLvl = (int)actorServerData.Level;
				this.actorExp = (int)actorServerData.Exp;
				this.actorStar = (int)actorServerData.Star;
				this.actorQuality = (int)actorServerData.TheQualityInfo.Quality;
				this.actorSubQuality = (int)actorServerData.TheQualityInfo.SubQuality;
			}
			else
			{
				if (actorMeta.ActorType == ActorTypeDef.Actor_Type_Monster)
				{
					IGameActorDataProvider actorDataProvider2 = Singleton<ActorDataCenter>.get_instance().GetActorDataProvider(GameActorDataProviderType.StaticBattleDataProvider);
					ActorStaticData actorStaticData = default(ActorStaticData);
					this.actorLvl = ((!actorDataProvider2.GetActorStaticData(ref actorMeta, ref actorStaticData)) ? 1 : actorStaticData.TheMonsterOnlyInfo.MonsterBaseLevel);
				}
				else
				{
					this.actorLvl = 1;
				}
				this.actorExp = 0;
				this.actorStar = 1;
				this.actorQuality = 1;
				this.actorSubQuality = 0;
			}
		}

		public void Init(COMDT_HEROINFO svrInfo)
		{
			ActorMeta actorMeta = default(ActorMeta);
			ActorMeta actorMeta2 = actorMeta;
			actorMeta2.ConfigId = (int)svrInfo.stCommonInfo.dwHeroID;
			actorMeta = actorMeta2;
			this.InitValueDataArr(ref actorMeta, true);
			this.actorLvl = (int)svrInfo.stCommonInfo.wLevel;
			this.actorExp = (int)svrInfo.stCommonInfo.dwExp;
			this.actorStar = (int)svrInfo.stCommonInfo.wStar;
			this.actorQuality = (int)svrInfo.stCommonInfo.stQuality.wQuality;
			this.actorSubQuality = (int)svrInfo.stCommonInfo.stQuality.wSubQuality;
			this.SetSkinProp(svrInfo.stCommonInfo.dwHeroID, (uint)svrInfo.stCommonInfo.wSkinID, true);
		}

		public void InitValueDataArr(ref ActorMeta theActorMeta, bool bLobby)
		{
			IGameActorDataProvider actorDataProvider;
			if (bLobby)
			{
				actorDataProvider = Singleton<ActorDataCenter>.get_instance().GetActorDataProvider(GameActorDataProviderType.StaticLobbyDataProvider);
			}
			else
			{
				actorDataProvider = Singleton<ActorDataCenter>.get_instance().GetActorDataProvider(GameActorDataProviderType.StaticBattleDataProvider);
			}
			ActorStaticData actorStaticData = default(ActorStaticData);
			actorDataProvider.GetActorStaticData(ref theActorMeta, ref actorStaticData);
			this.m_theActorMeta = theActorMeta;
			this.EnergyType = (EnergyType)actorStaticData.TheBaseAttribute.EpType;
			ResHeroEnergyInfo dataByKey = GameDataMgr.heroEnergyDatabin.GetDataByKey(actorStaticData.TheBaseAttribute.EpType);
			int nMaxLimitValue = (dataByKey == null) ? 0 : dataByKey.iEnergyMax;
			SLevelContext curLvelContext = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
			bool bPvpMode = true;
			if (curLvelContext != null)
			{
				bPvpMode = (bLobby || curLvelContext.IsMobaMode());
			}
			this.mActorValue[5] = new ValueDataInfo(5, actorStaticData.TheBaseAttribute.BaseHp, actorStaticData.TheBaseAttribute.PerLvHp, new ValueCalculator(this.DynamicAdjustorForMgcEffect), (int)actorStaticData.TheBaseAttribute.DynamicProperty, this.GetPropMaxValueLimit(5, bPvpMode));
			DebugHelper.Assert(this.mActorValue[5].totalValue > 0, "Initialize maxhp <= 0");
			this.mActorValue[1] = new ValueDataInfo(1, actorStaticData.TheBaseAttribute.BaseAd, actorStaticData.TheBaseAttribute.PerLvAd, new ValueCalculator(this.DynamicAdjustorForMgcEffect), (int)actorStaticData.TheBaseAttribute.DynamicProperty, this.GetPropMaxValueLimit(1, bPvpMode));
			this.mActorValue[2] = new ValueDataInfo(2, actorStaticData.TheBaseAttribute.BaseAp, actorStaticData.TheBaseAttribute.PerLvAp, new ValueCalculator(this.DynamicAdjustor), (int)actorStaticData.TheBaseAttribute.DynamicProperty, this.GetPropMaxValueLimit(2, bPvpMode));
			this.mActorValue[3] = new ValueDataInfo(3, actorStaticData.TheBaseAttribute.BaseDef, actorStaticData.TheBaseAttribute.PerLvDef, new ValueCalculator(this.DynamicAdjustor), (int)actorStaticData.TheBaseAttribute.DynamicProperty, this.GetPropMaxValueLimit(3, bPvpMode));
			this.mActorValue[4] = new ValueDataInfo(4, actorStaticData.TheBaseAttribute.BaseRes, actorStaticData.TheBaseAttribute.PerLvRes, new ValueCalculator(this.DynamicAdjustor), (int)actorStaticData.TheBaseAttribute.DynamicProperty, this.GetPropMaxValueLimit(4, bPvpMode));
			this.mActorValue[32] = new ValueDataInfo(32, actorStaticData.TheBaseAttribute.BaseEp, actorStaticData.TheBaseAttribute.EpGrowth, new ValueCalculator(this.EpGrowCalculator), 0, nMaxLimitValue);
			this.mActorValue[33] = new ValueDataInfo(33, actorStaticData.TheBaseAttribute.BaseEpRecover, actorStaticData.TheBaseAttribute.PerLvEpRecover, new ValueCalculator(this.EpRecCalculator), 0, this.GetPropMaxValueLimit(33, bPvpMode));
			this.mActorValue[34] = new ValueDataInfo(34, 0, 0, null, 0, this.GetPropMaxValueLimit(34, bPvpMode));
			this.mActorValue[35] = new ValueDataInfo(35, 0, 0, null, 0, this.GetPropMaxValueLimit(35, bPvpMode));
			this.mActorValue[21] = new ValueDataInfo(21, actorStaticData.TheBaseAttribute.Sight, 0, null, 0, this.GetPropMaxValueLimit(21, bPvpMode));
			this.mActorValue[15] = new ValueDataInfo(15, actorStaticData.TheBaseAttribute.MoveSpeed, 0, null, 0, this.GetPropMaxValueLimit(15, bPvpMode));
			this.mActorValue[16] = new ValueDataInfo(16, actorStaticData.TheBaseAttribute.BaseHpRecover, actorStaticData.TheBaseAttribute.PerLvHpRecover, new ValueCalculator(this.GrowCalculator), 0, this.GetPropMaxValueLimit(16, bPvpMode));
			this.mActorValue[18] = new ValueDataInfo(18, actorStaticData.TheBaseAttribute.BaseAtkSpeed, actorStaticData.TheBaseAttribute.PerLvAtkSpeed, new ValueCalculator(this.GrowCalculator), 0, this.GetPropMaxValueLimit(18, bPvpMode));
			this.mActorValue[6] = new ValueDataInfo(6, actorStaticData.TheBaseAttribute.CriticalChance, 0, null, 0, this.GetPropMaxValueLimit(6, bPvpMode));
			this.mActorValue[12] = new ValueDataInfo(12, actorStaticData.TheBaseAttribute.CriticalDamage, 0, null, 0, this.GetPropMaxValueLimit(12, bPvpMode));
			this.mActorValue[11] = new ValueDataInfo(11, 0, 0, null, 0, this.GetPropMaxValueLimit(11, bPvpMode));
			this.mActorValue[22] = new ValueDataInfo(22, 0, 0, null, 0, this.GetPropMaxValueLimit(22, bPvpMode));
			this.mActorValue[23] = new ValueDataInfo(23, 0, 0, null, 0, this.GetPropMaxValueLimit(23, bPvpMode));
			this.mActorValue[13] = new ValueDataInfo(13, 0, 0, null, 0, this.GetPropMaxValueLimit(13, bPvpMode));
			this.mActorValue[14] = new ValueDataInfo(14, 0, 0, null, 0, this.GetPropMaxValueLimit(14, bPvpMode));
			this.mActorValue[7] = new ValueDataInfo(7, 0, 0, null, 0, this.GetPropMaxValueLimit(7, bPvpMode));
			this.mActorValue[8] = new ValueDataInfo(8, 0, 0, null, 0, this.GetPropMaxValueLimit(8, bPvpMode));
			this.mActorValue[19] = new ValueDataInfo(19, 0, 0, null, 0, this.GetPropMaxValueLimit(19, bPvpMode));
			this.mActorValue[9] = new ValueDataInfo(9, 0, 0, null, 0, this.GetPropMaxValueLimit(9, bPvpMode));
			this.mActorValue[10] = new ValueDataInfo(10, 0, 0, null, 0, this.GetPropMaxValueLimit(10, bPvpMode));
			this.mActorValue[17] = new ValueDataInfo(17, 0, 0, null, 0, this.GetPropMaxValueLimit(17, bPvpMode));
			this.mActorValue[20] = new ValueDataInfo(20, 0, 0, null, 0, this.GetPropMaxValueLimitCdReduce(bPvpMode));
			this.mActorValue[24] = new ValueDataInfo(24, 0, 0, null, 0, this.GetPropMaxValueLimit(24, bPvpMode));
			this.mActorValue[25] = new ValueDataInfo(25, 0, 0, null, 0, this.GetPropMaxValueLimit(25, bPvpMode));
			this.mActorValue[26] = new ValueDataInfo(26, 0, 0, null, 0, this.GetPropMaxValueLimit(26, bPvpMode));
			this.mActorValue[27] = new ValueDataInfo(27, 0, 0, null, 0, this.GetPropMaxValueLimit(27, bPvpMode));
			this.mActorValue[28] = new ValueDataInfo(28, 0, 0, null, 0, this.GetPropMaxValueLimit(28, bPvpMode));
			this.mActorValue[29] = new ValueDataInfo(29, 0, 0, null, 0, this.GetPropMaxValueLimit(29, bPvpMode));
			this.mActorValue[30] = new ValueDataInfo(30, 0, 0, null, 0, this.GetPropMaxValueLimit(30, bPvpMode));
			this.mActorValue[31] = new ValueDataInfo(31, 0, 0, null, 0, this.GetPropMaxValueLimit(31, bPvpMode));
			this.mActorValue[36] = new ValueDataInfo(36, 0, 0, null, 0, this.GetPropMaxValueLimit(36, bPvpMode));
		}

		private int GetPropMaxValueLimitCdReduce(bool bPvpMode)
		{
			if (bPvpMode)
			{
				int result = 0;
				SLevelContext curLvelContext = Singleton<BattleLogic>.get_instance().GetCurLvelContext();
				if (curLvelContext != null && curLvelContext.m_cooldownReduceUpperLimit > 0u)
				{
					result = (int)curLvelContext.m_cooldownReduceUpperLimit;
				}
				else
				{
					ResPropertyValueInfo dataByKey = GameDataMgr.propertyValInfo.GetDataByKey(20u);
					if (dataByKey != null)
					{
						result = dataByKey.iMaxLimitValue;
					}
				}
				return result;
			}
			return 0;
		}

		private int GetPropMaxValueLimit(RES_FUNCEFT_TYPE funcType, bool bPvpMode)
		{
			if (bPvpMode)
			{
				ResPropertyValueInfo dataByKey = GameDataMgr.propertyValInfo.GetDataByKey((uint)funcType);
				return (dataByKey == null) ? 0 : dataByKey.iMaxLimitValue;
			}
			return 0;
		}

		public void SetChangeEvent(RES_FUNCEFT_TYPE key, ValueChangeDelegate func)
		{
			if (this.mActorValue[key] != null)
			{
				this.mActorValue[key].ChangeEvent += func;
				func();
			}
		}

		public void ChangeFuncEft(RES_FUNCEFT_TYPE key, RES_VALUE_TYPE type, int val, bool bOffRatio = false)
		{
			if (this.mActorValue[key] != null)
			{
				ValueDataInfo.ChangeValueData(ref this.mActorValue[key], type, val, bOffRatio);
			}
		}

		public void SetSkinProp(uint heroId, uint skinId, bool bWear)
		{
			ResHeroSkin heroSkin = CSkinInfo.GetHeroSkin(heroId, skinId);
			DebugHelper.Assert(heroSkin != null, "Skin==null");
			for (int i = 0; i < 15; i++)
			{
				ushort wType = heroSkin.astAttr[i].wType;
				byte bValType = heroSkin.astAttr[i].bValType;
				int iValue = heroSkin.astAttr[i].iValue;
				if (wType != 0 && iValue != 0)
				{
					if (bWear)
					{
						this.ChangeFuncEft(wType, bValType, iValue, true);
					}
					else
					{
						this.ChangeFuncEft(wType, bValType, -iValue, true);
					}
				}
			}
		}

		public void AddSymbolPageAttToProp(ref ActorMeta meta, bool bPVPLevel)
		{
			for (int i = 0; i < 37; i++)
			{
				PropertyHelper.s_symbolPropValAddArr[i] = 0;
			}
			IGameActorDataProvider actorDataProvider = Singleton<ActorDataCenter>.GetInstance().GetActorDataProvider(GameActorDataProviderType.ServerDataProvider);
			ActorServerRuneData actorServerRuneData = default(ActorServerRuneData);
			for (int j = 0; j < 30; j++)
			{
				if (actorDataProvider.GetActorServerRuneData(ref meta, (ActorRunelSlot)j, ref actorServerRuneData))
				{
					ResSymbolInfo dataByKey = GameDataMgr.symbolInfoDatabin.GetDataByKey(actorServerRuneData.RuneId);
					if (dataByKey != null)
					{
						if (bPVPLevel)
						{
							for (int k = 0; k < dataByKey.astFuncEftList.Length; k++)
							{
								int wType = (int)dataByKey.astFuncEftList[k].wType;
								int bValType = (int)dataByKey.astFuncEftList[k].bValType;
								int iValue = dataByKey.astFuncEftList[k].iValue;
								if (wType != 0 && wType < 37 && iValue != 0)
								{
									if (bValType == 0)
									{
										PropertyHelper.s_symbolPropValAddArr[wType] += iValue;
									}
									else if (bValType == 1)
									{
										this.ChangeFuncEft(wType, bValType, iValue, true);
									}
								}
							}
						}
						else
						{
							for (int l = 0; l < dataByKey.astPveEftList.Length; l++)
							{
								int wType = (int)dataByKey.astPveEftList[l].wType;
								int bValType = (int)dataByKey.astPveEftList[l].bValType;
								int iValue = dataByKey.astPveEftList[l].iValue;
								if (wType != 0 && wType < 37 && iValue != 0)
								{
									if (bValType == 0)
									{
										PropertyHelper.s_symbolPropValAddArr[wType] += iValue;
									}
									else if (bValType == 1)
									{
										this.ChangeFuncEft(wType, bValType, iValue, true);
									}
								}
							}
						}
					}
				}
			}
			for (int m = 0; m < 37; m++)
			{
				int num = PropertyHelper.s_symbolPropValAddArr[m] / 100;
				if (num > 0)
				{
					this.ChangeFuncEft(m, 0, num, true);
				}
			}
		}

		public ValueDataInfo[] GetActorValue()
		{
			return this.mActorValue;
		}
	}
}
