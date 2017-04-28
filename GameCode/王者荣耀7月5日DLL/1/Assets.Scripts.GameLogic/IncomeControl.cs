using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameSystem;
using Assets.Scripts.Sound;
using ResData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
	public class IncomeControl
	{
		private const int MaxContiKillNum = 7;

		private const int MaxContiDeadnum = 7;

		private const int MAX_INCOM_RULE = 7;

		private const int MAX_INCOME_TYPE_CNT = 3;

		private ResIncomeAllocRule[][] m_incomeAllocRules = new ResIncomeAllocRule[3][];

		private List<stActorIncome> m_actorIncomes = new List<stActorIncome>();

		private ListView<ActorRoot> m_allocIncomeRelatedHeros = new ListView<ActorRoot>();

		public bool m_isExpCompensate;

		public ushort m_originalGoldCoinInBattle;

		public List<int> m_compensateRateList = new List<int>();

		private ListView<ResSoulLvlUpInfo> m_allocSoulLvlList = new ListView<ResSoulLvlUpInfo>();

		private ListView<ResSoulAddition> m_incomeAdditionList = new ListView<ResSoulAddition>();

		private bool bSoulGrow;

		private bool bPvpMode;

		public void StartFight()
		{
			this.ResetAllocSoulLvlMap();
			this.ResetIncomeAdditionList();
		}

		public void ResetAllocSoulLvlMap()
		{
			this.ClearAllocSoulLvlList();
			SLevelContext curLvelContext = Singleton<BattleLogic>.get_instance().GetCurLvelContext();
			DebugHelper.Assert(curLvelContext != null);
			if (curLvelContext == null)
			{
				return;
			}
			if (!Singleton<BattleLogic>.get_instance().m_LevelContext.IsSoulGrow())
			{
				return;
			}
			uint soulAllocId = curLvelContext.m_soulAllocId;
			int mapID = curLvelContext.m_mapID;
			HashSet<object> hashSet;
			if (soulAllocId > 0u)
			{
				hashSet = GameDataMgr.soulLvlUpDatabin.GetDataByKey(soulAllocId);
			}
			else
			{
				hashSet = GameDataMgr.soulLvlUpDatabin.GetDataByIndex(0);
			}
			if (hashSet != null)
			{
				HashSet<object>.Enumerator enumerator = hashSet.GetEnumerator();
				while (enumerator.MoveNext())
				{
					ResSoulLvlUpInfo resSoulLvlUpInfo = enumerator.get_Current() as ResSoulLvlUpInfo;
					if (resSoulLvlUpInfo != null)
					{
						this.m_allocSoulLvlList.Add(resSoulLvlUpInfo);
					}
				}
			}
		}

		public void ClearAllocSoulLvlList()
		{
			this.m_allocSoulLvlList.Clear();
		}

		public ResSoulLvlUpInfo QuerySoulLvlUpInfo(uint inLevel)
		{
			int count = this.m_allocSoulLvlList.get_Count();
			for (int i = 0; i < count; i++)
			{
				ResSoulLvlUpInfo resSoulLvlUpInfo = this.m_allocSoulLvlList.get_Item(i);
				if (resSoulLvlUpInfo != null)
				{
					if (resSoulLvlUpInfo.dwLevel == inLevel)
					{
						return resSoulLvlUpInfo;
					}
				}
			}
			return null;
		}

		public ListView<ResSoulLvlUpInfo> GetSoulLvlUpInfoList()
		{
			return this.m_allocSoulLvlList;
		}

		public void ResetIncomeAdditionList()
		{
			this.m_incomeAdditionList.Clear();
			SLevelContext curLvelContext = Singleton<BattleLogic>.get_instance().GetCurLvelContext();
			DebugHelper.Assert(curLvelContext != null);
			if (curLvelContext == null)
			{
				return;
			}
			if (!Singleton<BattleLogic>.get_instance().m_LevelContext.IsSoulGrow())
			{
				return;
			}
			uint soulAllocId = curLvelContext.m_soulAllocId;
			int mapID = curLvelContext.m_mapID;
			HashSet<object> hashSet;
			if (soulAllocId > 0u)
			{
				hashSet = GameDataMgr.soulAdditionDatabin.GetDataByKey(soulAllocId);
			}
			else
			{
				hashSet = GameDataMgr.soulAdditionDatabin.GetDataByIndex(0);
			}
			if (hashSet != null)
			{
				HashSet<object>.Enumerator enumerator = hashSet.GetEnumerator();
				while (enumerator.MoveNext())
				{
					ResSoulAddition resSoulAddition = enumerator.get_Current() as ResSoulAddition;
					if (resSoulAddition != null)
					{
						this.m_incomeAdditionList.Add(resSoulAddition);
					}
				}
			}
		}

		public ResSoulAddition QueryIncomeAdditonInfo(int heroState)
		{
			int count = this.m_incomeAdditionList.get_Count();
			for (int i = 0; i < count; i++)
			{
				ResSoulAddition resSoulAddition = this.m_incomeAdditionList.get_Item(i);
				if (resSoulAddition != null)
				{
					if (resSoulAddition.iHeroKillType == heroState)
					{
						return resSoulAddition;
					}
				}
			}
			return null;
		}

		private void InitIncomeRule(uint incomeRuleID)
		{
			for (int i = 0; i < 3; i++)
			{
				this.m_incomeAllocRules[i] = new ResIncomeAllocRule[7];
				for (int j = 0; j < 7; j++)
				{
					this.m_incomeAllocRules[i][j] = null;
				}
			}
			if (incomeRuleID == 0u)
			{
				return;
			}
			GameDataMgr.incomeAllocDatabin.Accept(delegate(ResIncomeAllocRule rule)
			{
				if (rule != null && rule.dwSoulID == incomeRuleID)
				{
					this.m_incomeAllocRules[(int)rule.wIncomeType][(int)rule.wTargetType] = rule;
				}
			});
		}

		public void InitExpCompensateInfo(byte bIsOpenExpCompensate, ref ResDT_ExpCompensateInfo[] expCompensateInfo)
		{
			this.m_isExpCompensate = (bIsOpenExpCompensate > 0);
			if (this.m_isExpCompensate && expCompensateInfo != null)
			{
				this.m_compensateRateList.Clear();
				int num = 0;
				for (int i = 0; i < expCompensateInfo.Length; i++)
				{
					int bLevelDiff = (int)expCompensateInfo[i].bLevelDiff;
					int dwExtraExpRate = (int)expCompensateInfo[i].dwExtraExpRate;
					if (bLevelDiff > 0)
					{
						for (int j = this.m_compensateRateList.get_Count(); j < bLevelDiff; j++)
						{
							this.m_compensateRateList.Add(num);
						}
						this.m_compensateRateList.Add(dwExtraExpRate);
						num = dwExtraExpRate;
					}
				}
			}
		}

		public void Init(SLevelContext _levelContext)
		{
			uint levelIncomeRuleID = IncomeControl.GetLevelIncomeRuleID(_levelContext, this);
			this.InitIncomeRule(levelIncomeRuleID);
			this.bSoulGrow = Singleton<BattleLogic>.GetInstance().m_LevelContext.IsSoulGrow();
			this.bPvpMode = _levelContext.IsMobaMode();
			Singleton<GameEventSys>.get_instance().RmvEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.OnActorDead));
			Singleton<GameEventSys>.get_instance().AddEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.OnActorDead));
			this.m_actorIncomes.Clear();
			if (this.m_originalGoldCoinInBattle > 0)
			{
				List<PoolObjHandle<ActorRoot>> heroActors = Singleton<GameObjMgr>.GetInstance().HeroActors;
				for (int i = 0; i < heroActors.get_Count(); i++)
				{
					if (heroActors.get_Item(i))
					{
						heroActors.get_Item(i).get_handle().ValueComponent.ChangeGoldCoinInBattle((int)this.m_originalGoldCoinInBattle, true, false, default(Vector3), false, default(PoolObjHandle<ActorRoot>));
					}
				}
			}
		}

		public void uninit()
		{
			Singleton<GameEventSys>.get_instance().RmvEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.OnActorDead));
			this.ClearAllocSoulLvlList();
			this.m_incomeAdditionList.Clear();
		}

		private void OnActorDead(ref GameDeadEventParam prm)
		{
			PoolObjHandle<ActorRoot> src = prm.src;
			PoolObjHandle<ActorRoot> poolObjHandle = prm.orignalAtker;
			if (!src || !poolObjHandle || prm.bImmediateRevive)
			{
				return;
			}
			if (this.bSoulGrow)
			{
				if (src.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_EYE)
				{
					EyeWrapper eyeWrapper = src.get_handle().ActorControl as EyeWrapper;
					if (eyeWrapper == null || eyeWrapper.bLifeTimeOver)
					{
						return;
					}
				}
				if (src.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero && poolObjHandle.get_handle().TheActorMeta.ActorType != ActorTypeDef.Actor_Type_Hero && src.get_handle().ActorControl.IsKilledByHero())
				{
					poolObjHandle = src.get_handle().ActorControl.LastHeroAtker;
				}
				if (poolObjHandle)
				{
					this.OnActorDeadIncomeSoul(ref src, ref poolObjHandle);
					if (poolObjHandle.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero || src.get_handle().TheActorMeta.ActorType != ActorTypeDef.Actor_Type_Hero)
					{
						this.OnActorDeadIncomeGoldCoinInBattle(ref src, ref poolObjHandle);
					}
				}
			}
			if (!this.bPvpMode)
			{
				this.OnMonsterDeadGold(ref prm);
			}
		}

		private void OnMonsterDeadGold(ref GameDeadEventParam prm)
		{
			MonsterWrapper monsterWrapper = prm.src.get_handle().AsMonster();
			PoolObjHandle<ActorRoot> orignalAtker = prm.orignalAtker;
			if (monsterWrapper == null || monsterWrapper.cfgInfo == null)
			{
				return;
			}
			RES_MONSTER_TYPE bMonsterType = monsterWrapper.cfgInfo.bMonsterType;
			if (bMonsterType != 2)
			{
				return;
			}
			if (!orignalAtker || orignalAtker.get_handle().EffectControl == null)
			{
				return;
			}
			orignalAtker.get_handle().EffectControl.PlayDyingGoldEffect(prm.src);
			Singleton<CSoundManager>.get_instance().PlayBattleSound("Glod_Get", prm.src, prm.src.get_handle().gameObject);
		}

		private void OnActorDeadIncomeSoul(ref PoolObjHandle<ActorRoot> target, ref PoolObjHandle<ActorRoot> attacker)
		{
			if (!target || !attacker)
			{
				return;
			}
			ResIncomeAllocRule resIncomeAllocRule = null;
			switch (target.get_handle().TheActorMeta.ActorType)
			{
			case ActorTypeDef.Actor_Type_Hero:
				resIncomeAllocRule = this.m_incomeAllocRules[1][3];
				break;
			case ActorTypeDef.Actor_Type_Monster:
			{
				MonsterWrapper monsterWrapper = target.get_handle().AsMonster();
				if (monsterWrapper != null)
				{
					RES_MONSTER_TYPE bMonsterType = monsterWrapper.cfgInfo.bMonsterType;
					if (bMonsterType == 2)
					{
						resIncomeAllocRule = this.m_incomeAllocRules[1][4];
					}
					else if (bMonsterType == 1)
					{
						resIncomeAllocRule = this.m_incomeAllocRules[1][2];
					}
				}
				break;
			}
			case ActorTypeDef.Actor_Type_Organ:
				resIncomeAllocRule = this.m_incomeAllocRules[1][1];
				break;
			case ActorTypeDef.Actor_Type_EYE:
				resIncomeAllocRule = this.m_incomeAllocRules[1][6];
				break;
			}
			if (resIncomeAllocRule != null)
			{
				ResDT_IncomeAttackRule incomeAllocRuleByAtker = this.GetIncomeAllocRuleByAtker(ref attacker, resIncomeAllocRule);
				if (incomeAllocRuleByAtker != null)
				{
					this.AllocIncome(ref target, ref attacker, incomeAllocRuleByAtker, enIncomeType.Soul, resIncomeAllocRule.IncomeChangeRate, resIncomeAllocRule.iComputerChangeRate);
				}
			}
		}

		private ResDT_IncomeAttackRule GetIncomeAllocRuleByAtker(ref PoolObjHandle<ActorRoot> attacker, ResIncomeAllocRule incomeRule)
		{
			if (incomeRule == null || !attacker)
			{
				return null;
			}
			RES_INCOME_ATTACKER_TYPE rES_INCOME_ATTACKER_TYPE = 1;
			switch (attacker.get_handle().TheActorMeta.ActorType)
			{
			case ActorTypeDef.Actor_Type_Hero:
				rES_INCOME_ATTACKER_TYPE = 4;
				break;
			case ActorTypeDef.Actor_Type_Monster:
			{
				MonsterWrapper monsterWrapper = attacker.get_handle().AsMonster();
				if (monsterWrapper != null)
				{
					RES_MONSTER_TYPE bMonsterType = monsterWrapper.cfgInfo.bMonsterType;
					if (bMonsterType == 2)
					{
						rES_INCOME_ATTACKER_TYPE = 5;
					}
					else if (bMonsterType == 1)
					{
						rES_INCOME_ATTACKER_TYPE = 3;
					}
				}
				break;
			}
			case ActorTypeDef.Actor_Type_Organ:
				rES_INCOME_ATTACKER_TYPE = 2;
				break;
			}
			for (int i = 0; i < 4; i++)
			{
				if (incomeRule.astIncomeRule[i].bAttakerType == 1 || rES_INCOME_ATTACKER_TYPE == incomeRule.astIncomeRule[i].bAttakerType)
				{
					return incomeRule.astIncomeRule[i];
				}
			}
			return null;
		}

		private void OnActorDeadIncomeGoldCoinInBattle(ref PoolObjHandle<ActorRoot> target, ref PoolObjHandle<ActorRoot> attacker)
		{
			if (!target || !attacker)
			{
				return;
			}
			if (ActorHelper.IsHostCtrlActor(ref attacker) && (target.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Monster || target.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero))
			{
				if (attacker.get_handle().EffectControl != null)
				{
					attacker.get_handle().EffectControl.PlayDyingGoldEffect(target);
				}
				Singleton<CSoundManager>.GetInstance().PlayBattleSound("Glod_Get", target, target.get_handle().gameObject);
			}
			ResIncomeAllocRule resIncomeAllocRule = null;
			switch (target.get_handle().TheActorMeta.ActorType)
			{
			case ActorTypeDef.Actor_Type_Hero:
				resIncomeAllocRule = this.m_incomeAllocRules[2][3];
				break;
			case ActorTypeDef.Actor_Type_Monster:
			{
				MonsterWrapper monsterWrapper = target.get_handle().AsMonster();
				if (monsterWrapper != null)
				{
					RES_MONSTER_TYPE bMonsterType = monsterWrapper.cfgInfo.bMonsterType;
					if (bMonsterType == 2)
					{
						if (monsterWrapper.cfgInfo.bSoldierType == 7 || monsterWrapper.cfgInfo.bSoldierType == 8 || monsterWrapper.cfgInfo.bSoldierType == 9)
						{
							resIncomeAllocRule = this.m_incomeAllocRules[2][5];
						}
						else
						{
							resIncomeAllocRule = this.m_incomeAllocRules[2][4];
						}
					}
					else if (bMonsterType == 1)
					{
						resIncomeAllocRule = this.m_incomeAllocRules[2][2];
					}
				}
				break;
			}
			case ActorTypeDef.Actor_Type_Organ:
				resIncomeAllocRule = this.m_incomeAllocRules[2][1];
				break;
			case ActorTypeDef.Actor_Type_EYE:
				resIncomeAllocRule = this.m_incomeAllocRules[2][6];
				break;
			}
			if (resIncomeAllocRule != null)
			{
				ResDT_IncomeAttackRule incomeAllocRuleByAtker = this.GetIncomeAllocRuleByAtker(ref attacker, resIncomeAllocRule);
				if (incomeAllocRuleByAtker != null)
				{
					this.AllocIncome(ref target, ref attacker, incomeAllocRuleByAtker, enIncomeType.GoldCoinInBattle, resIncomeAllocRule.IncomeChangeRate, resIncomeAllocRule.iComputerChangeRate);
				}
			}
		}

		private void AllocIncome(ref PoolObjHandle<ActorRoot> target, ref PoolObjHandle<ActorRoot> attacker, ResDT_IncomeAttackRule allocIncomeRule, enIncomeType incomeType, int[] soulExpIncomeChangeRate, int computerChangeRate)
		{
			uint actorKilledIncome = this.GetActorKilledIncome(ref target, incomeType);
			this.m_actorIncomes.Clear();
			for (int i = 0; i < allocIncomeRule.astIncomeMemberArr.Length; i++)
			{
				this.m_allocIncomeRelatedHeros.Clear();
				RES_INCOME_MEMBER_CHOOSE_TYPE wMemberChooseType = allocIncomeRule.astIncomeMemberArr[i].wMemberChooseType;
				this.GetAllocIncomeRelatedHeros(ref this.m_allocIncomeRelatedHeros, ref target, ref attacker, wMemberChooseType, allocIncomeRule, i);
				this.AllocIncomeToHeros(ref this.m_actorIncomes, this.m_allocIncomeRelatedHeros, ref target, ref attacker, incomeType, actorKilledIncome, allocIncomeRule, i, soulExpIncomeChangeRate, computerChangeRate);
				this.m_allocIncomeRelatedHeros.Clear();
			}
			if (incomeType == enIncomeType.GoldCoinInBattle && target && target.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero)
			{
				this.AllocGoldRewardToCamp(ref this.m_actorIncomes, ref attacker, ref target);
			}
			for (int j = 0; j < this.m_actorIncomes.get_Count(); j++)
			{
				if (this.m_actorIncomes.get_Item(j).m_actorRoot != null && this.m_actorIncomes.get_Item(j).m_actorRoot.TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero)
				{
					uint num = this.m_actorIncomes.get_Item(j).m_incomeValue;
					if (incomeType == enIncomeType.Soul && this.m_isExpCompensate)
					{
						num = this.GetCompensateExp(this.m_actorIncomes.get_Item(j).m_actorRoot, this.m_actorIncomes.get_Item(j).m_incomeValue);
					}
					if (incomeType == enIncomeType.Soul)
					{
						this.m_actorIncomes.get_Item(j).m_actorRoot.ValueComponent.AddSoulExp((int)num, false, AddSoulType.Income);
					}
					else if (incomeType == enIncomeType.GoldCoinInBattle)
					{
						bool isLastHit = object.ReferenceEquals(attacker.get_handle(), this.m_actorIncomes.get_Item(j).m_actorRoot);
						this.m_actorIncomes.get_Item(j).m_actorRoot.ValueComponent.ChangeGoldCoinInBattle((int)num, true, true, target.get_handle().myTransform.position, isLastHit, target);
					}
					if (incomeType == enIncomeType.Soul && this.m_actorIncomes.get_Item(j).m_actorRoot.IsHostCamp() && num > 0u)
					{
						Singleton<CBattleSystem>.GetInstance().CreateBattleFloatDigit((int)num, DIGIT_TYPE.ReceiveSpirit, this.m_actorIncomes.get_Item(j).m_actorRoot.gameObject.transform.position);
					}
				}
			}
			this.m_actorIncomes.Clear();
		}

		private uint GetActorKilledIncome(ref PoolObjHandle<ActorRoot> target, enIncomeType incomeType)
		{
			if (!target)
			{
				return 0u;
			}
			if (target.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero)
			{
				return this.GetHeroKilledIncome(ref target, incomeType);
			}
			if (incomeType == enIncomeType.Soul)
			{
				return Singleton<BattleLogic>.GetInstance().dynamicProperty.GetDynamicSoulExp(target.get_handle().TheStaticData.TheBaseAttribute.DynamicProperty, target.get_handle().TheStaticData.TheBaseAttribute.SoulExpGained);
			}
			if (incomeType == enIncomeType.GoldCoinInBattle)
			{
				return Singleton<BattleLogic>.GetInstance().dynamicProperty.GetDynamicGoldCoinInBattle(target.get_handle().TheStaticData.TheBaseAttribute.DynamicProperty, target.get_handle().TheStaticData.TheBaseAttribute.GoldCoinInBattleGained, target.get_handle().TheStaticData.TheBaseAttribute.GoldCoinInBattleGainedFloatRange);
			}
			return 0u;
		}

		public uint GetHeroKilledIncome(ref PoolObjHandle<ActorRoot> target, enIncomeType incomeType)
		{
			if (!target)
			{
				return 0u;
			}
			int heroKillOrDeadState = this.GetHeroKillOrDeadState(ref target);
			ResSoulAddition resSoulAddition = this.QueryIncomeAdditonInfo(heroKillOrDeadState);
			int num = 10000;
			if (resSoulAddition != null)
			{
				if (incomeType == enIncomeType.Soul)
				{
					num = resSoulAddition.iExpAddRate;
				}
				else if (incomeType == enIncomeType.GoldCoinInBattle)
				{
					num = resSoulAddition.iGoldCoinInBattleAddRate;
				}
			}
			int actorSoulLevel = target.get_handle().ValueComponent.actorSoulLevel;
			ResSoulLvlUpInfo resSoulLvlUpInfo = this.QuerySoulLvlUpInfo((uint)actorSoulLevel);
			uint num2 = 0u;
			uint num3 = 0u;
			if (resSoulLvlUpInfo != null)
			{
				if (incomeType == enIncomeType.Soul)
				{
					num2 = resSoulLvlUpInfo.dwKilledExp;
					num3 = resSoulLvlUpInfo.dwExtraKillExp;
				}
				else if (incomeType == enIncomeType.GoldCoinInBattle)
				{
					num2 = (uint)resSoulLvlUpInfo.wKillGoldCoinInBattle;
					num3 = (uint)resSoulLvlUpInfo.wExtraKillGoldCoinInBattle;
				}
			}
			uint num4 = (uint)((ulong)num2 * (ulong)((long)num) / 10000uL);
			if (Singleton<BattleStatistic>.get_instance().GetCampScore(1) + Singleton<BattleStatistic>.get_instance().GetCampScore(2) == 1)
			{
				num4 += num3;
			}
			return num4;
		}

		private void GetAllocIncomeRelatedHeros(ref ListView<ActorRoot> relatedHeros, ref PoolObjHandle<ActorRoot> target, ref PoolObjHandle<ActorRoot> attacker, RES_INCOME_MEMBER_CHOOSE_TYPE chooseType, ResDT_IncomeAttackRule allocIncomeRule, int paramIndex)
		{
			switch (chooseType)
			{
			case 1:
			{
				List<PoolObjHandle<ActorRoot>> heroActors = Singleton<GameObjMgr>.get_instance().HeroActors;
				int count = heroActors.get_Count();
				for (int i = 0; i < count; i++)
				{
					if (heroActors.get_Item(i).get_handle().TheActorMeta.ActorCamp == attacker.get_handle().TheActorMeta.ActorCamp && heroActors.get_Item(i).get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero && (!heroActors.get_Item(i).get_handle().ActorControl.IsDeadState || allocIncomeRule.astIncomeMemberArr[paramIndex].bDeadAddIncome > 0))
					{
						relatedHeros.Add(heroActors.get_Item(i).get_handle());
					}
				}
				break;
			}
			case 2:
			{
				if (attacker.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero && (!attacker.get_handle().ActorControl.IsDeadState || allocIncomeRule.astIncomeMemberArr[paramIndex].bDeadAddIncome > 0))
				{
					relatedHeros.Add(attacker.get_handle());
				}
				List<PoolObjHandle<ActorRoot>> heroActors2 = Singleton<GameObjMgr>.get_instance().HeroActors;
				int count2 = heroActors2.get_Count();
				for (int j = 0; j < count2; j++)
				{
					if (!(heroActors2.get_Item(j) == attacker))
					{
						if (heroActors2.get_Item(j).get_handle().TheActorMeta.ActorCamp == attacker.get_handle().TheActorMeta.ActorCamp && heroActors2.get_Item(j).get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero && this.IsActorInRange(heroActors2.get_Item(j), target, allocIncomeRule.astIncomeMemberArr[paramIndex].iRangeRadius) && (!heroActors2.get_Item(j).get_handle().ActorControl.IsDeadState || allocIncomeRule.astIncomeMemberArr[paramIndex].bDeadAddIncome > 0))
						{
							relatedHeros.Add(heroActors2.get_Item(j).get_handle());
						}
					}
				}
				break;
			}
			case 3:
				if (attacker.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero && (!attacker.get_handle().ActorControl.IsDeadState || allocIncomeRule.astIncomeMemberArr[paramIndex].bDeadAddIncome > 0))
				{
					relatedHeros.Add(attacker.get_handle());
				}
				break;
			case 4:
			{
				HashSet<uint> assistSet = BattleLogic.GetAssistSet(target, attacker, true);
				HashSet<uint>.Enumerator enumerator = assistSet.GetEnumerator();
				while (enumerator.MoveNext())
				{
					PoolObjHandle<ActorRoot> actor = Singleton<GameObjMgr>.GetInstance().GetActor(enumerator.get_Current());
					if (actor.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero && (!actor.get_handle().ActorControl.IsDeadState || allocIncomeRule.astIncomeMemberArr[paramIndex].bDeadAddIncome > 0))
					{
						relatedHeros.Add(actor.get_handle());
					}
				}
				break;
			}
			case 5:
			{
				if (attacker.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero && (!attacker.get_handle().ActorControl.IsDeadState || allocIncomeRule.astIncomeMemberArr[paramIndex].bDeadAddIncome > 0))
				{
					relatedHeros.Add(attacker.get_handle());
				}
				HashSet<uint> assistSet2 = BattleLogic.GetAssistSet(target, attacker, true);
				HashSet<uint>.Enumerator enumerator2 = assistSet2.GetEnumerator();
				while (enumerator2.MoveNext())
				{
					PoolObjHandle<ActorRoot> actor2 = Singleton<GameObjMgr>.GetInstance().GetActor(enumerator2.get_Current());
					if (actor2.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero && !relatedHeros.Contains(actor2.get_handle()) && (!actor2.get_handle().ActorControl.IsDeadState || allocIncomeRule.astIncomeMemberArr[paramIndex].bDeadAddIncome > 0))
					{
						relatedHeros.Add(actor2.get_handle());
					}
				}
				break;
			}
			}
		}

		private int GetHeroKillOrDeadState(ref PoolObjHandle<ActorRoot> target)
		{
			int result = 0;
			if (target)
			{
				HeroWrapper heroWrapper = target.get_handle().ActorControl as HeroWrapper;
				if (heroWrapper != null)
				{
					if (heroWrapper.ContiKillNum > 0)
					{
						if (heroWrapper.ContiKillNum >= 7)
						{
							result = 7;
						}
						else
						{
							result = heroWrapper.ContiKillNum;
						}
					}
					else if (heroWrapper.ContiDeadNum >= 7)
					{
						result = -7;
					}
					else
					{
						result = -heroWrapper.ContiDeadNum;
					}
				}
			}
			return result;
		}

		private void AllocGoldRewardToCamp(ref List<stActorIncome> actorIncomes, ref PoolObjHandle<ActorRoot> attaker, ref PoolObjHandle<ActorRoot> target)
		{
			if (!attaker || !target)
			{
				return;
			}
			int heroKillOrDeadState = this.GetHeroKillOrDeadState(ref target);
			ResSoulAddition resSoulAddition = this.QueryIncomeAdditonInfo(heroKillOrDeadState);
			if (resSoulAddition != null && resSoulAddition.dwGoldRewardValue > 0u)
			{
				List<PoolObjHandle<ActorRoot>> heroActors = Singleton<GameObjMgr>.get_instance().HeroActors;
				int count = heroActors.get_Count();
				for (int i = 0; i < count; i++)
				{
					if (heroActors.get_Item(i).get_handle().TheActorMeta.ActorCamp == attaker.get_handle().TheActorMeta.ActorCamp && heroActors.get_Item(i).get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero)
					{
						this.PutActorToIncomeList(ref actorIncomes, heroActors.get_Item(i).get_handle(), enIncomeType.GoldCoinInBattle, resSoulAddition.dwGoldRewardValue);
					}
				}
			}
		}

		private void AllocIncomeToHeros(ref List<stActorIncome> actorIncomes, ListView<ActorRoot> relatedHeros, ref PoolObjHandle<ActorRoot> target, ref PoolObjHandle<ActorRoot> attacker, enIncomeType incomeType, uint incomeValue, ResDT_IncomeAttackRule allocIncomeRule, int paramIndex, int[] soulExpIncomeChangeRate, int computerChangeRate)
		{
			int count = relatedHeros.get_Count();
			if (count <= 0)
			{
				return;
			}
			ResDT_AllocRuleParam resDT_AllocRuleParam = allocIncomeRule.astIncomeMemberArr[paramIndex];
			int num = ((count <= 5) ? count : 5) - 1;
			int num2 = soulExpIncomeChangeRate[num];
			incomeValue = (uint)((ulong)((uint)((ulong)incomeValue * (ulong)((long)num2) / 10000uL)) * (ulong)((long)resDT_AllocRuleParam.iIncomeRate) / 10000uL);
			uint num3 = 0u;
			if (resDT_AllocRuleParam.wDivideType == 1)
			{
				num3 = (uint)((ulong)incomeValue / (ulong)((long)count));
			}
			else if (resDT_AllocRuleParam.wDivideType == 2)
			{
				num3 = incomeValue;
			}
			for (int i = 0; i < count; i++)
			{
				uint num4 = num3;
				if (incomeType == enIncomeType.Soul)
				{
					if (relatedHeros.get_Item(i) == attacker.get_handle())
					{
						num4 = (uint)((ulong)num3 * (ulong)((long)(10000 + relatedHeros.get_Item(i).BuffHolderComp.GetSoulExpAddRate(target))) / 10000uL);
					}
				}
				else if (incomeType == enIncomeType.GoldCoinInBattle && relatedHeros.get_Item(i) == attacker.get_handle())
				{
					num4 = (uint)((ulong)num3 * (ulong)((long)(10000 + relatedHeros.get_Item(i).BuffHolderComp.GetCoinAddRate(target))) / 10000uL);
				}
				this.PutActorToIncomeList(ref actorIncomes, relatedHeros.get_Item(i), incomeType, num4);
				if (incomeType == enIncomeType.Soul)
				{
					AddSoulExpEventParam addSoulExpEventParam = new AddSoulExpEventParam(target, relatedHeros.get_Item(i).SelfPtr, (int)num4);
					Singleton<GameEventSys>.get_instance().SendEvent<AddSoulExpEventParam>(GameEventDef.Event_AddExpValue, ref addSoulExpEventParam);
				}
				else if (incomeType == enIncomeType.GoldCoinInBattle)
				{
				}
			}
		}

		private bool IsActorInRange(PoolObjHandle<ActorRoot> actor, PoolObjHandle<ActorRoot> referActor, int range)
		{
			if (range == 0)
			{
				return true;
			}
			long num = (long)range * (long)range;
			long sqrMagnitudeLong2D = (actor.get_handle().location - referActor.get_handle().location).get_sqrMagnitudeLong2D();
			return sqrMagnitudeLong2D < num;
		}

		private void PutActorToIncomeList(ref List<stActorIncome> actorIncomes, ActorRoot actorRoot, enIncomeType incomeType, uint incomeValue)
		{
			stActorIncome stActorIncome = default(stActorIncome);
			for (int i = 0; i < actorIncomes.get_Count(); i++)
			{
				if (actorIncomes.get_Item(i).m_actorRoot == actorRoot)
				{
					stActorIncome.m_actorRoot = actorRoot;
					stActorIncome.m_incomeType = incomeType;
					stActorIncome.m_incomeValue = actorIncomes.get_Item(i).m_incomeValue + incomeValue;
					actorIncomes.set_Item(i, stActorIncome);
					return;
				}
			}
			stActorIncome.m_actorRoot = actorRoot;
			stActorIncome.m_incomeType = incomeType;
			stActorIncome.m_incomeValue = incomeValue;
			actorIncomes.Add(stActorIncome);
		}

		private uint GetCompensateExp(ActorRoot actorRoot, uint exp)
		{
			if (!this.m_isExpCompensate || this.m_compensateRateList.get_Count() == 0)
			{
				return exp;
			}
			int heroMaxLevel = Singleton<GameObjMgr>.GetInstance().GetHeroMaxLevel();
			int num = heroMaxLevel - actorRoot.ValueComponent.actorSoulLevel;
			int num2 = 0;
			if (num >= 0 && num < this.m_compensateRateList.get_Count())
			{
				num2 = this.m_compensateRateList.get_Item(num);
			}
			else if (num >= this.m_compensateRateList.get_Count())
			{
				num2 = this.m_compensateRateList.get_Item(this.m_compensateRateList.get_Count() - 1);
			}
			exp = (uint)((ulong)exp * (ulong)((long)(10000 + num2)) / 10000uL);
			return exp;
		}

		public static uint GetLevelIncomeRuleID(SLevelContext _levelContext, IncomeControl inControl)
		{
			inControl.m_isExpCompensate = false;
			inControl.m_originalGoldCoinInBattle = 0;
			inControl.m_compensateRateList.Clear();
			uint soulID = _levelContext.m_soulID;
			if (_levelContext.IsMobaMode())
			{
				inControl.InitExpCompensateInfo(_levelContext.m_isOpenExpCompensate, ref _levelContext.m_expCompensateInfo);
				inControl.m_originalGoldCoinInBattle = _levelContext.m_originalGoldCoinInBattle;
			}
			return soulID;
		}
	}
}
