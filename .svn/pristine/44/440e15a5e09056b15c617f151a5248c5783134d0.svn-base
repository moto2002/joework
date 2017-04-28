using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using CSProtocol;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
	public class CPlayerKDAStat
	{
		private DictionaryView<uint, PlayerKDA> m_PlayerKDA = new DictionaryView<uint, PlayerKDA>();

		public CCampKDAStat m_CampKdaStat;

		public DictionaryView<uint, PlayerKDA>.Enumerator GetEnumerator()
		{
			return this.m_PlayerKDA.GetEnumerator();
		}

		public PlayerKDA GetHostKDA()
		{
			PlayerKDA result;
			this.m_PlayerKDA.TryGetValue(Singleton<GamePlayerCenter>.get_instance().HostPlayerId, ref result);
			return result;
		}

		public PlayerKDA GetPlayerKDA(uint playerId)
		{
			PlayerKDA result;
			this.m_PlayerKDA.TryGetValue(playerId, ref result);
			return result;
		}

		public HeroKDA GetHeroKDA(uint objID)
		{
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				ListView<HeroKDA>.Enumerator enumerator2 = current.get_Value().GetEnumerator();
				while (enumerator2.MoveNext())
				{
					if (enumerator2.get_Current().actorHero.get_handle().ObjID == objID)
					{
						return enumerator2.get_Current();
					}
				}
			}
			return null;
		}

		public float GetTeamKDA(COM_PLAYERCAMP camp)
		{
			float num = 0f;
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				if (current.get_Value().PlayerCamp == camp)
				{
					float arg_46_0 = num;
					KeyValuePair<uint, PlayerKDA> current2 = enumerator.get_Current();
					num = arg_46_0 + current2.get_Value().KDAValue;
				}
			}
			return num;
		}

		public int GetTeamKillNum(COM_PLAYERCAMP camp)
		{
			int num = 0;
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				if (current.get_Value().PlayerCamp == camp)
				{
					int arg_42_0 = num;
					KeyValuePair<uint, PlayerKDA> current2 = enumerator.get_Current();
					num = arg_42_0 + current2.get_Value().numKill;
				}
			}
			return num;
		}

		public int GetTeamDeadNum(COM_PLAYERCAMP camp)
		{
			int num = 0;
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				if (current.get_Value().PlayerCamp == camp)
				{
					int arg_42_0 = num;
					KeyValuePair<uint, PlayerKDA> current2 = enumerator.get_Current();
					num = arg_42_0 + current2.get_Value().numDead;
				}
			}
			return num;
		}

		public int GetTeamAssistNum(COM_PLAYERCAMP camp)
		{
			int num = 0;
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				if (current.get_Value().PlayerCamp == camp)
				{
					int arg_42_0 = num;
					KeyValuePair<uint, PlayerKDA> current2 = enumerator.get_Current();
					num = arg_42_0 + current2.get_Value().numAssist;
				}
			}
			return num;
		}

		public int GetTeamHurt(COM_PLAYERCAMP camp)
		{
			int num = 0;
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				if (current.get_Value().PlayerCamp == camp)
				{
					int arg_42_0 = num;
					KeyValuePair<uint, PlayerKDA> current2 = enumerator.get_Current();
					num = arg_42_0 + current2.get_Value().TotalHurt;
				}
			}
			return num;
		}

		public int GetTeamBeHurt(COM_PLAYERCAMP camp)
		{
			int num = 0;
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				if (current.get_Value().PlayerCamp == camp)
				{
					int arg_42_0 = num;
					KeyValuePair<uint, PlayerKDA> current2 = enumerator.get_Current();
					num = arg_42_0 + current2.get_Value().TotalBeHurt;
				}
			}
			return num;
		}

		public int GetTeamCoin(COM_PLAYERCAMP camp)
		{
			int num = 0;
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				if (current.get_Value().PlayerCamp == camp)
				{
					int arg_42_0 = num;
					KeyValuePair<uint, PlayerKDA> current2 = enumerator.get_Current();
					num = arg_42_0 + current2.get_Value().TotalCoin;
				}
			}
			return num;
		}

		public void StartKDARecord()
		{
			this.reset();
			this.initialize();
		}

		public void GenerateStatData()
		{
			int num = (int)GameDataMgr.globalInfoDatabin.GetDataByKey(188u).dwConfValue;
			int num2 = 0;
			int num3 = 0;
			int num4 = (int)GameDataMgr.globalInfoDatabin.GetDataByKey(189u).dwConfValue;
			int num5 = 0;
			int num6 = (int)GameDataMgr.globalInfoDatabin.GetDataByKey(190u).dwConfValue;
			int num7 = 0;
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				ListView<HeroKDA>.Enumerator enumerator2 = current.get_Value().GetEnumerator();
				while (enumerator2.MoveNext())
				{
					if (num < enumerator2.get_Current().numKill)
					{
						num = enumerator2.get_Current().numKill;
					}
					if (num2 < enumerator2.get_Current().hurtToEnemy)
					{
						num2 = enumerator2.get_Current().hurtToEnemy;
					}
					if (num3 < enumerator2.get_Current().hurtTakenByEnemy)
					{
						num3 = enumerator2.get_Current().hurtTakenByEnemy;
					}
					if (num4 < enumerator2.get_Current().numAssist)
					{
						num4 = enumerator2.get_Current().numAssist;
					}
					if (num5 < enumerator2.get_Current().TotalCoin)
					{
						num5 = enumerator2.get_Current().TotalCoin;
					}
					if (num6 < enumerator2.get_Current().numKillOrgan)
					{
						num6 = enumerator2.get_Current().numKillOrgan;
					}
					if (num7 < enumerator2.get_Current().hurtToHero)
					{
						num7 = enumerator2.get_Current().hurtToHero;
					}
				}
			}
			enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current2 = enumerator.get_Current();
				ListView<HeroKDA>.Enumerator enumerator3 = current2.get_Value().GetEnumerator();
				while (enumerator3.MoveNext())
				{
					if (num == enumerator3.get_Current().numKill)
					{
						enumerator3.get_Current().bKillMost = true;
					}
					if (num2 == enumerator3.get_Current().hurtToEnemy)
					{
						enumerator3.get_Current().bHurtMost = true;
					}
					if (num3 == enumerator3.get_Current().hurtTakenByEnemy)
					{
						enumerator3.get_Current().bHurtTakenMost = true;
					}
					if (num4 == enumerator3.get_Current().numAssist)
					{
						enumerator3.get_Current().bAsssistMost = true;
					}
					if (num5 == enumerator3.get_Current().TotalCoin)
					{
						enumerator3.get_Current().bGetCoinMost = true;
					}
					if (num6 == enumerator3.get_Current().numKillOrgan && enumerator3.get_Current().numKillOrgan > 0)
					{
						enumerator3.get_Current().bKillOrganMost = true;
					}
					if (num7 == enumerator3.get_Current().hurtToHero)
					{
						enumerator3.get_Current().bHurtToHeroMost = true;
					}
				}
			}
			if (this.m_CampKdaStat == null)
			{
				this.m_CampKdaStat = new CCampKDAStat();
				if (this.m_CampKdaStat != null)
				{
					this.m_CampKdaStat.Initialize(this.m_PlayerKDA);
				}
			}
		}

		public void reset()
		{
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				current.get_Value().clear();
			}
			this.m_PlayerKDA.Clear();
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<PoolObjHandle<ActorRoot>, sbyte, uint>("HeroLearnTalent", new Action<PoolObjHandle<ActorRoot>, sbyte, uint>(this.OnHeroLearnTalent));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<PoolObjHandle<ActorRoot>, int>("HeroSoulLevelChange", new Action<PoolObjHandle<ActorRoot>, int>(this.OnHeroSoulLvlChange));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<uint, stEquipInfo[], bool, int>("HeroEquipInBattleChange", new Action<uint, stEquipInfo[], bool, int>(this.OnHeroBattleEquipChange));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.OnActorDead));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_GameEnd, new RefAction<DefaultGameEventParam>(this.OnGameEnd));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<HurtEventResultInfo>(GameEventDef.Event_ActorDamage, new RefAction<HurtEventResultInfo>(this.OnActorDamage));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_DoubleKill, new RefAction<DefaultGameEventParam>(this.OnActorDoubleKill));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_TripleKill, new RefAction<DefaultGameEventParam>(this.OnActorTripleKill));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_QuataryKill, new RefAction<DefaultGameEventParam>(this.OnActorQuataryKill));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_PentaKill, new RefAction<DefaultGameEventParam>(this.OnActorPentaKill));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_Odyssey, new RefAction<DefaultGameEventParam>(this.OnActorOdyssey));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<SkillChooseTargetEventParam>(GameEventDef.Event_ActorBeChosenAsTarget, new RefAction<SkillChooseTargetEventParam>(this.OnActorBeChosen));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<SkillChooseTargetEventParam>(GameEventDef.Event_HitTrigger, new RefAction<SkillChooseTargetEventParam>(this.OnHitTrigger));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<PoolObjHandle<ActorRoot>, int, bool, PoolObjHandle<ActorRoot>>("HeroGoldCoinInBattleChange", new Action<PoolObjHandle<ActorRoot>, int, bool, PoolObjHandle<ActorRoot>>(this.OnActorBattleCoinChanged));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<HemophagiaEventResultInfo>(GameEventDef.Event_Hemophagia, new RefAction<HemophagiaEventResultInfo>(this.OnActorHemophagia));
		}

		private void initialize()
		{
			List<Player> allPlayers = Singleton<GamePlayerCenter>.get_instance().GetAllPlayers();
			List<Player>.Enumerator enumerator = allPlayers.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Player current = enumerator.get_Current();
				if (current != null)
				{
					PlayerKDA playerKDA = new PlayerKDA();
					playerKDA.initialize(current);
					this.m_PlayerKDA.Add(current.PlayerId, playerKDA);
				}
			}
			Singleton<EventRouter>.GetInstance().AddEventHandler<PoolObjHandle<ActorRoot>, sbyte, uint>("HeroLearnTalent", new Action<PoolObjHandle<ActorRoot>, sbyte, uint>(this.OnHeroLearnTalent));
			Singleton<EventRouter>.GetInstance().AddEventHandler<PoolObjHandle<ActorRoot>, int>("HeroSoulLevelChange", new Action<PoolObjHandle<ActorRoot>, int>(this.OnHeroSoulLvlChange));
			Singleton<EventRouter>.GetInstance().AddEventHandler<uint, stEquipInfo[], bool, int>("HeroEquipInBattleChange", new Action<uint, stEquipInfo[], bool, int>(this.OnHeroBattleEquipChange));
			Singleton<GameEventSys>.get_instance().AddEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.OnActorDead));
			Singleton<GameEventSys>.get_instance().AddEventHandler<DefaultGameEventParam>(GameEventDef.Event_GameEnd, new RefAction<DefaultGameEventParam>(this.OnGameEnd));
			Singleton<GameEventSys>.get_instance().AddEventHandler<HurtEventResultInfo>(GameEventDef.Event_ActorDamage, new RefAction<HurtEventResultInfo>(this.OnActorDamage));
			Singleton<GameEventSys>.get_instance().AddEventHandler<DefaultGameEventParam>(GameEventDef.Event_DoubleKill, new RefAction<DefaultGameEventParam>(this.OnActorDoubleKill));
			Singleton<GameEventSys>.get_instance().AddEventHandler<DefaultGameEventParam>(GameEventDef.Event_TripleKill, new RefAction<DefaultGameEventParam>(this.OnActorTripleKill));
			Singleton<GameEventSys>.get_instance().AddEventHandler<DefaultGameEventParam>(GameEventDef.Event_QuataryKill, new RefAction<DefaultGameEventParam>(this.OnActorQuataryKill));
			Singleton<GameEventSys>.get_instance().AddEventHandler<DefaultGameEventParam>(GameEventDef.Event_PentaKill, new RefAction<DefaultGameEventParam>(this.OnActorPentaKill));
			Singleton<GameEventSys>.get_instance().AddEventHandler<DefaultGameEventParam>(GameEventDef.Event_Odyssey, new RefAction<DefaultGameEventParam>(this.OnActorOdyssey));
			Singleton<GameEventSys>.get_instance().AddEventHandler<SkillChooseTargetEventParam>(GameEventDef.Event_ActorBeChosenAsTarget, new RefAction<SkillChooseTargetEventParam>(this.OnActorBeChosen));
			Singleton<GameEventSys>.get_instance().AddEventHandler<SkillChooseTargetEventParam>(GameEventDef.Event_HitTrigger, new RefAction<SkillChooseTargetEventParam>(this.OnHitTrigger));
			Singleton<EventRouter>.GetInstance().AddEventHandler<PoolObjHandle<ActorRoot>, int, bool, PoolObjHandle<ActorRoot>>("HeroGoldCoinInBattleChange", new Action<PoolObjHandle<ActorRoot>, int, bool, PoolObjHandle<ActorRoot>>(this.OnActorBattleCoinChanged));
			Singleton<GameEventSys>.get_instance().AddEventHandler<HemophagiaEventResultInfo>(GameEventDef.Event_Hemophagia, new RefAction<HemophagiaEventResultInfo>(this.OnActorHemophagia));
		}

		public void OnHeroLearnTalent(PoolObjHandle<ActorRoot> hero, sbyte talentLevel, uint talentID)
		{
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				ListView<HeroKDA>.Enumerator enumerator2 = current.get_Value().GetEnumerator();
				while (enumerator2.MoveNext())
				{
					if (hero == enumerator2.get_Current().actorHero)
					{
						enumerator2.get_Current().TalentArr[(int)talentLevel].dwTalentID = talentID;
						enumerator2.get_Current().TalentArr[(int)talentLevel].dwLearnLevel = (uint)hero.get_handle().ValueComponent.actorSoulLevel;
						return;
					}
				}
			}
		}

		public void OnHeroBattleEquipChange(uint actorId, stEquipInfo[] equips, bool bIsAdd, int iEquipSlotIndex)
		{
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				ListView<HeroKDA>.Enumerator enumerator2 = current.get_Value().GetEnumerator();
				while (enumerator2.MoveNext())
				{
					if (enumerator2.get_Current() != null && enumerator2.get_Current().actorHero.get_handle().ObjID == actorId)
					{
						equips.CopyTo(enumerator2.get_Current().Equips, 0);
						Singleton<EventRouter>.get_instance().BroadCastEvent(EventID.BATTLE_KDA_CHANGED);
						Singleton<EventRouter>.get_instance().BroadCastEvent(EventID.BATTLE_HERO_PROPERTY_CHANGED);
						return;
					}
				}
			}
		}

		public void OnHeroSoulLvlChange(PoolObjHandle<ActorRoot> hero, int level)
		{
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				ListView<HeroKDA>.Enumerator enumerator2 = current.get_Value().GetEnumerator();
				while (enumerator2.MoveNext())
				{
					if (hero == enumerator2.get_Current().actorHero)
					{
						enumerator2.get_Current().SoulLevel = Math.Max(level, 1);
						Singleton<EventRouter>.get_instance().BroadCastEvent(EventID.BATTLE_KDA_CHANGED);
						Singleton<EventRouter>.get_instance().BroadCastEvent(EventID.BATTLE_HERO_PROPERTY_CHANGED);
						return;
					}
				}
			}
		}

		public void DumpDebugInfo()
		{
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string arg_2A_0 = "PlayerKDA Id {0}";
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				Debug.Log(string.Format(arg_2A_0, current.get_Key()));
			}
		}

		public void OnActorBattleCoinChanged(PoolObjHandle<ActorRoot> actor, int changeValue, bool isIncome, PoolObjHandle<ActorRoot> target)
		{
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				ListView<HeroKDA>.Enumerator enumerator2 = current.get_Value().GetEnumerator();
				while (enumerator2.MoveNext())
				{
					if (actor == enumerator2.get_Current().actorHero)
					{
						enumerator2.get_Current().OnActorBattleCoinChanged(actor, changeValue, isIncome, target);
						Singleton<EventRouter>.get_instance().BroadCastEvent(EventID.BATTLE_KDA_CHANGED);
						return;
					}
				}
			}
		}

		public void OnActorDead(ref GameDeadEventParam prm)
		{
			if (prm.bImmediateRevive)
			{
				return;
			}
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				ListView<HeroKDA>.Enumerator enumerator2 = current.get_Value().GetEnumerator();
				while (enumerator2.MoveNext())
				{
					enumerator2.get_Current().OnActorDead(ref prm);
				}
				KeyValuePair<uint, PlayerKDA> current2 = enumerator.get_Current();
				if (current2.get_Value().IsHost)
				{
					KeyValuePair<uint, PlayerKDA> current3 = enumerator.get_Current();
					if (current3.get_Value().m_hostHeroDamage != null)
					{
						KeyValuePair<uint, PlayerKDA> current4 = enumerator.get_Current();
						ListView<CHostHeroDamage>.Enumerator enumerator3 = current4.get_Value().m_hostHeroDamage.GetEnumerator();
						while (enumerator3.MoveNext())
						{
							if (enumerator3.get_Current() != null)
							{
								enumerator3.get_Current().OnActorDead(ref prm);
							}
						}
					}
				}
			}
			if (prm.src && prm.src.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero)
			{
				Singleton<EventRouter>.get_instance().BroadCastEvent(EventID.BATTLE_KDA_CHANGED);
				Singleton<EventRouter>.get_instance().BroadCastEvent(EventID.BATTLE_KDA_CHANGED_BY_ACTOR_DEAD);
			}
		}

		public void OnActorDamage(ref HurtEventResultInfo prm)
		{
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				ListView<HeroKDA>.Enumerator enumerator2 = current.get_Value().GetEnumerator();
				while (enumerator2.MoveNext())
				{
					enumerator2.get_Current().OnActorDamage(ref prm);
				}
				KeyValuePair<uint, PlayerKDA> current2 = enumerator.get_Current();
				if (current2.get_Value().IsHost)
				{
					KeyValuePair<uint, PlayerKDA> current3 = enumerator.get_Current();
					if (current3.get_Value().m_hostHeroDamage != null)
					{
						KeyValuePair<uint, PlayerKDA> current4 = enumerator.get_Current();
						ListView<CHostHeroDamage>.Enumerator enumerator3 = current4.get_Value().m_hostHeroDamage.GetEnumerator();
						while (enumerator3.MoveNext())
						{
							if (enumerator3.get_Current() != null)
							{
								enumerator3.get_Current().OnActorDamage(ref prm);
							}
						}
					}
				}
			}
		}

		public void OnActorDoubleKill(ref DefaultGameEventParam prm)
		{
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				ListView<HeroKDA>.Enumerator enumerator2 = current.get_Value().GetEnumerator();
				while (enumerator2.MoveNext())
				{
					if (prm.logicAtker && prm.logicAtker == enumerator2.get_Current().actorHero)
					{
						enumerator2.get_Current().OnActorDoubleKill(ref prm);
						return;
					}
				}
			}
		}

		public void OnActorTripleKill(ref DefaultGameEventParam prm)
		{
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				ListView<HeroKDA>.Enumerator enumerator2 = current.get_Value().GetEnumerator();
				while (enumerator2.MoveNext())
				{
					if (prm.logicAtker && prm.logicAtker == enumerator2.get_Current().actorHero)
					{
						enumerator2.get_Current().OnActorTripleKill(ref prm);
						return;
					}
				}
			}
		}

		public void OnActorQuataryKill(ref DefaultGameEventParam prm)
		{
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				ListView<HeroKDA>.Enumerator enumerator2 = current.get_Value().GetEnumerator();
				while (enumerator2.MoveNext())
				{
					if (prm.logicAtker && prm.logicAtker == enumerator2.get_Current().actorHero)
					{
						enumerator2.get_Current().OnActorQuataryKill(ref prm);
						return;
					}
				}
			}
		}

		public void OnActorPentaKill(ref DefaultGameEventParam prm)
		{
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				ListView<HeroKDA>.Enumerator enumerator2 = current.get_Value().GetEnumerator();
				while (enumerator2.MoveNext())
				{
					if (prm.logicAtker && prm.logicAtker == enumerator2.get_Current().actorHero)
					{
						enumerator2.get_Current().OnActorPentaKill(ref prm);
						return;
					}
				}
			}
		}

		public void OnActorOdyssey(ref DefaultGameEventParam prm)
		{
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				ListView<HeroKDA>.Enumerator enumerator2 = current.get_Value().GetEnumerator();
				while (enumerator2.MoveNext())
				{
					if (prm.logicAtker && prm.logicAtker == enumerator2.get_Current().actorHero)
					{
						enumerator2.get_Current().OnActorOdyssey(ref prm);
						return;
					}
				}
			}
		}

		public void OnActorBeChosen(ref SkillChooseTargetEventParam prm)
		{
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				ListView<HeroKDA>.Enumerator enumerator2 = current.get_Value().GetEnumerator();
				while (enumerator2.MoveNext())
				{
					enumerator2.get_Current().OnActorBeChosen(ref prm);
				}
			}
		}

		public void OnHitTrigger(ref SkillChooseTargetEventParam prm)
		{
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				ListView<HeroKDA>.Enumerator enumerator2 = current.get_Value().GetEnumerator();
				while (enumerator2.MoveNext())
				{
					enumerator2.get_Current().OnHitTrigger(ref prm);
				}
			}
		}

		private void OnGameEnd(ref DefaultGameEventParam prm)
		{
			Singleton<GameEventSys>.get_instance().RmvEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.OnActorDead));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_GameEnd, new RefAction<DefaultGameEventParam>(this.OnGameEnd));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<HurtEventResultInfo>(GameEventDef.Event_ActorDamage, new RefAction<HurtEventResultInfo>(this.OnActorDamage));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_DoubleKill, new RefAction<DefaultGameEventParam>(this.OnActorDoubleKill));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_TripleKill, new RefAction<DefaultGameEventParam>(this.OnActorTripleKill));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_QuataryKill, new RefAction<DefaultGameEventParam>(this.OnActorQuataryKill));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_PentaKill, new RefAction<DefaultGameEventParam>(this.OnActorPentaKill));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_Odyssey, new RefAction<DefaultGameEventParam>(this.OnActorOdyssey));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<SkillChooseTargetEventParam>(GameEventDef.Event_ActorBeChosenAsTarget, new RefAction<SkillChooseTargetEventParam>(this.OnActorBeChosen));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<SkillChooseTargetEventParam>(GameEventDef.Event_HitTrigger, new RefAction<SkillChooseTargetEventParam>(this.OnHitTrigger));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<PoolObjHandle<ActorRoot>, int, bool, PoolObjHandle<ActorRoot>>("HeroGoldCoinInBattleChange", new Action<PoolObjHandle<ActorRoot>, int, bool, PoolObjHandle<ActorRoot>>(this.OnActorBattleCoinChanged));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<HemophagiaEventResultInfo>(GameEventDef.Event_Hemophagia, new RefAction<HemophagiaEventResultInfo>(this.OnActorHemophagia));
		}

		private void OnActorHemophagia(ref HemophagiaEventResultInfo hri)
		{
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = this.m_PlayerKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				ListView<HeroKDA>.Enumerator enumerator2 = current.get_Value().GetEnumerator();
				while (enumerator2.MoveNext())
				{
					enumerator2.get_Current().OnActorHemophagia(ref hri);
				}
			}
		}
	}
}
