using Assets.Scripts.Framework;
using ResData;
using System;

namespace Assets.Scripts.GameLogic.DataCenter
{
	internal class ActorStaticLobbyDataProvider : ActorStaticDataProviderBase
	{
		protected override bool BuildHeroData(ref ActorMeta actorMeta, ref ActorStaticData actorData)
		{
			ResHeroCfgInfo dataByKey = GameDataMgr.heroDatabin.GetDataByKey((uint)actorData.TheActorMeta.ConfigId);
			if (dataByKey == null)
			{
				base.ErrorMissingHeroConfig((uint)actorData.TheActorMeta.ConfigId);
				return false;
			}
			actorData.TheBaseAttribute.EpType = dataByKey.dwEnergyType;
			actorData.TheBaseAttribute.BaseEp = dataByKey.iEnergy;
			actorData.TheBaseAttribute.EpGrowth = dataByKey.iEnergyGrowth;
			actorData.TheBaseAttribute.BaseEpRecover = dataByKey.iEnergyRec;
			actorData.TheBaseAttribute.PerLvEpRecover = dataByKey.iEnergyRecGrowth;
			actorData.TheBaseAttribute.BaseHp = dataByKey.iBaseHP;
			actorData.TheBaseAttribute.PerLvHp = dataByKey.iHpGrowth;
			actorData.TheBaseAttribute.BaseAd = dataByKey.iBaseATT;
			actorData.TheBaseAttribute.PerLvAd = dataByKey.iAtkGrowth;
			actorData.TheBaseAttribute.BaseAp = dataByKey.iBaseINT;
			actorData.TheBaseAttribute.PerLvAp = dataByKey.iSpellGrowth;
			actorData.TheBaseAttribute.BaseAtkSpeed = dataByKey.iBaseAtkSpd;
			actorData.TheBaseAttribute.PerLvAtkSpeed = dataByKey.iAtkSpdAddLvlup;
			actorData.TheBaseAttribute.BaseDef = dataByKey.iBaseDEF;
			actorData.TheBaseAttribute.PerLvDef = dataByKey.iDefGrowth;
			actorData.TheBaseAttribute.BaseRes = dataByKey.iBaseRES;
			actorData.TheBaseAttribute.PerLvRes = dataByKey.iResistGrowth;
			actorData.TheBaseAttribute.BaseHpRecover = dataByKey.iBaseHPAdd;
			actorData.TheBaseAttribute.PerLvHpRecover = dataByKey.iHPAddLvlup;
			actorData.TheBaseAttribute.CriticalChance = dataByKey.iCritRate;
			actorData.TheBaseAttribute.CriticalDamage = dataByKey.iCritEft;
			actorData.TheBaseAttribute.Sight = dataByKey.iSightR;
			actorData.TheBaseAttribute.MoveSpeed = dataByKey.iBaseSpeed;
			actorData.TheBaseAttribute.SoulExpGained = 0;
			actorData.TheBaseAttribute.GoldCoinInBattleGained = 0;
			actorData.TheBaseAttribute.GoldCoinInBattleGainedFloatRange = 0;
			actorData.TheBaseAttribute.ClashMark = 0u;
			actorData.TheBaseAttribute.RandomPassiveSkillRule = 0;
			actorData.TheBaseAttribute.PassiveSkillID1 = dataByKey.iPassiveID1;
			actorData.TheBaseAttribute.PassiveSkillID2 = dataByKey.iPassiveID2;
			actorData.TheBaseAttribute.DeadControl = (dataByKey.dwDeadControl == 1u);
			actorData.TheHeroOnlyInfo.HeroCapability = (int)dataByKey.bMainJob;
			actorData.TheHeroOnlyInfo.HeroDamageType = (int)dataByKey.bDamageType;
			actorData.TheHeroOnlyInfo.HeroAttackType = (int)dataByKey.bAttackType;
			actorData.TheHeroOnlyInfo.InitialStar = dataByKey.iInitialStar;
			actorData.TheHeroOnlyInfo.RecommendStandPos = dataByKey.iRecommendPosition;
			actorData.TheHeroOnlyInfo.AttackDistanceType = (int)dataByKey.bAttackDistanceType;
			actorData.TheHeroOnlyInfo.HeroNamePinYin = dataByKey.szNamePinYin;
			actorData.TheResInfo.Name = StringHelper.UTF8BytesToString(ref dataByKey.szName);
			actorData.TheResInfo.ResPath = StringHelper.UTF8BytesToString(ref dataByKey.szCharacterInfo);
			actorData.ProviderType = GameActorDataProviderType.StaticLobbyDataProvider;
			return true;
		}

		protected override bool BuildMonsterData(ref ActorMeta actorMeta, ref ActorStaticData actorData)
		{
			ResMonsterCfgInfo dataCfgInfo = MonsterDataHelper.GetDataCfgInfo(actorData.TheActorMeta.ConfigId, (int)actorData.TheActorMeta.Difficuty);
			if (dataCfgInfo == null)
			{
				dataCfgInfo = MonsterDataHelper.GetDataCfgInfo(actorData.TheActorMeta.ConfigId, 1);
			}
			if (dataCfgInfo == null)
			{
				base.ErrorMissingMonsterConfig((uint)actorData.TheActorMeta.ConfigId);
				return false;
			}
			actorData.TheBaseAttribute.EpType = 1u;
			actorData.TheBaseAttribute.BaseHp = dataCfgInfo.iBaseHP;
			actorData.TheBaseAttribute.PerLvHp = 0;
			actorData.TheBaseAttribute.BaseAd = dataCfgInfo.iBaseATT;
			actorData.TheBaseAttribute.PerLvAd = 0;
			actorData.TheBaseAttribute.BaseAp = dataCfgInfo.iBaseINT;
			actorData.TheBaseAttribute.PerLvAp = 0;
			actorData.TheBaseAttribute.BaseAtkSpeed = 0;
			actorData.TheBaseAttribute.PerLvAtkSpeed = 0;
			actorData.TheBaseAttribute.BaseDef = dataCfgInfo.iBaseDEF;
			actorData.TheBaseAttribute.PerLvDef = 0;
			actorData.TheBaseAttribute.BaseRes = dataCfgInfo.iBaseRES;
			actorData.TheBaseAttribute.PerLvRes = 0;
			actorData.TheBaseAttribute.BaseHpRecover = dataCfgInfo.iBaseHPAdd;
			actorData.TheBaseAttribute.PerLvHpRecover = 0;
			actorData.TheBaseAttribute.CriticalChance = 0;
			actorData.TheBaseAttribute.CriticalDamage = 0;
			actorData.TheBaseAttribute.Sight = dataCfgInfo.iSightR;
			actorData.TheBaseAttribute.MoveSpeed = dataCfgInfo.iBaseSpeed;
			actorData.TheBaseAttribute.SoulExpGained = dataCfgInfo.iSoulExp;
			actorData.TheBaseAttribute.GoldCoinInBattleGained = (int)dataCfgInfo.wStartingGoldCoinInBattle;
			actorData.TheBaseAttribute.GoldCoinInBattleGainedFloatRange = (int)dataCfgInfo.bGoldCoinInBattleRange;
			actorData.TheBaseAttribute.DynamicProperty = dataCfgInfo.dwDynamicPropertyCfg;
			actorData.TheBaseAttribute.ClashMark = dataCfgInfo.dwClashMark;
			actorData.TheResInfo.Name = StringHelper.UTF8BytesToString(ref dataCfgInfo.szName);
			actorData.TheResInfo.ResPath = StringHelper.UTF8BytesToString(ref dataCfgInfo.szCharacterInfo);
			actorData.ProviderType = GameActorDataProviderType.StaticLobbyDataProvider;
			return true;
		}

		protected override bool BuildOrganData(ref ActorMeta actorMeta, ref ActorStaticData actorData)
		{
			ResOrganCfgInfo dataCfgInfoByCurLevelDiff = OrganDataHelper.GetDataCfgInfoByCurLevelDiff(actorData.TheActorMeta.ConfigId);
			if (dataCfgInfoByCurLevelDiff == null)
			{
				base.ErrorMissingOrganConfig((uint)actorData.TheActorMeta.ConfigId);
				return false;
			}
			actorData.TheBaseAttribute.EpType = 1u;
			actorData.TheBaseAttribute.BaseHp = dataCfgInfoByCurLevelDiff.iBaseHP;
			actorData.TheBaseAttribute.PerLvHp = dataCfgInfoByCurLevelDiff.iHPLvlup;
			actorData.TheBaseAttribute.BaseAd = dataCfgInfoByCurLevelDiff.iBaseATT;
			actorData.TheBaseAttribute.PerLvAd = dataCfgInfoByCurLevelDiff.iATTLvlup;
			actorData.TheBaseAttribute.BaseAp = dataCfgInfoByCurLevelDiff.iBaseINT;
			actorData.TheBaseAttribute.PerLvAp = dataCfgInfoByCurLevelDiff.iINTLvlup;
			actorData.TheBaseAttribute.BaseAtkSpeed = 0;
			actorData.TheBaseAttribute.PerLvAtkSpeed = dataCfgInfoByCurLevelDiff.iAtkSpdAddLvlup;
			actorData.TheBaseAttribute.BaseDef = dataCfgInfoByCurLevelDiff.iBaseDEF;
			actorData.TheBaseAttribute.PerLvDef = dataCfgInfoByCurLevelDiff.iDEFLvlup;
			actorData.TheBaseAttribute.BaseRes = dataCfgInfoByCurLevelDiff.iBaseRES;
			actorData.TheBaseAttribute.PerLvRes = dataCfgInfoByCurLevelDiff.iRESLvlup;
			actorData.TheBaseAttribute.BaseHpRecover = dataCfgInfoByCurLevelDiff.iBaseHPAdd;
			actorData.TheBaseAttribute.PerLvHpRecover = dataCfgInfoByCurLevelDiff.iHPAddLvlup;
			actorData.TheBaseAttribute.CriticalChance = 0;
			actorData.TheBaseAttribute.CriticalDamage = 0;
			actorData.TheBaseAttribute.Sight = dataCfgInfoByCurLevelDiff.iSightR;
			actorData.TheBaseAttribute.MoveSpeed = dataCfgInfoByCurLevelDiff.iBaseSpeed;
			actorData.TheBaseAttribute.SoulExpGained = dataCfgInfoByCurLevelDiff.iSoulExp;
			actorData.TheBaseAttribute.GoldCoinInBattleGained = (int)dataCfgInfoByCurLevelDiff.wGoldCoinInBattle;
			actorData.TheBaseAttribute.GoldCoinInBattleGainedFloatRange = 0;
			actorData.TheBaseAttribute.DynamicProperty = dataCfgInfoByCurLevelDiff.dwDynamicPropertyCfg;
			actorData.TheBaseAttribute.ClashMark = dataCfgInfoByCurLevelDiff.dwClashMark;
			actorData.TheResInfo.Name = StringHelper.UTF8BytesToString(ref dataCfgInfoByCurLevelDiff.szName);
			actorData.TheResInfo.ResPath = StringHelper.UTF8BytesToString(ref dataCfgInfoByCurLevelDiff.szCharacterInfo);
			actorData.TheOrganOnlyInfo.PhyArmorHurtRate = dataCfgInfoByCurLevelDiff.iPhyArmorHurtRate;
			actorData.TheOrganOnlyInfo.AttackRouteID = dataCfgInfoByCurLevelDiff.iAktRouteID;
			actorData.TheOrganOnlyInfo.DeadEnemySoldier = dataCfgInfoByCurLevelDiff.iDeadEnemySoldier;
			actorData.TheOrganOnlyInfo.NoEnemyAddPhyDef = dataCfgInfoByCurLevelDiff.iNoEnemyAddPhyDef;
			actorData.TheOrganOnlyInfo.NoEnemyAddMgcDef = dataCfgInfoByCurLevelDiff.iNoEnemyAddMgcDef;
			actorData.TheOrganOnlyInfo.HorizonRadius = dataCfgInfoByCurLevelDiff.iHorizonRadius;
			actorData.ProviderType = GameActorDataProviderType.StaticLobbyDataProvider;
			return true;
		}
	}
}
