using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameSystem;
using ResData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
	public class MonsterWrapper : ObjWrapper
	{
		private const int MON_BATTLE_COOL_TICKS = 30;

		private int nOutCombatHpRecoveryTick;

		protected int lifeTime;

		protected int BornTime;

		private bool isBoss;

		protected VInt3 originalPos = VInt3.zero;

		private Vector3 originalMeshScale = Vector3.one;

		protected PoolObjHandle<ActorRoot> HostActor;

		protected SkillSlotType SpawnSkillSlot = SkillSlotType.SLOT_SKILL_VALID;

		protected bool bCopyedHeroInfo;

		protected int DistanceTestCount;

		private static readonly long CheckDistance = 225000000L;

		private readonly long BaronPretectDistance = 36000000L;

		private int endurance;

		private bool isEnduranceDown;

		public PursuitInfo Pursuit;

		public ResMonsterCfgInfo cfgInfo
		{
			get;
			protected set;
		}

		public int Endurance
		{
			get
			{
				return this.endurance;
			}
			set
			{
				this.endurance = value;
			}
		}

		public bool IsEnduranceDown
		{
			get
			{
				return this.isEnduranceDown;
			}
			set
			{
				if (value != this.isEnduranceDown)
				{
				}
				this.isEnduranceDown = value;
			}
		}

		public SkillSlotType spawnSkillSlot
		{
			get
			{
				return this.SpawnSkillSlot;
			}
			protected set
			{
				this.SpawnSkillSlot = value;
			}
		}

		public PoolObjHandle<ActorRoot> hostActor
		{
			get
			{
				return this.HostActor;
			}
		}

		public bool isCalledMonster
		{
			get
			{
				return this.HostActor;
			}
		}

		public bool isDisplayHeroInfo
		{
			get
			{
				return this.bCopyedHeroInfo;
			}
			protected set
			{
				this.bCopyedHeroInfo = value;
			}
		}

		public int LifeTime
		{
			get
			{
				return this.lifeTime;
			}
			set
			{
				this.lifeTime = value;
			}
		}

		public override int CfgReviveCD
		{
			get
			{
				return 2147483647;
			}
		}

		public override PoolObjHandle<ActorRoot> GetOrignalActor()
		{
			if (this.isCalledMonster)
			{
				return this.HostActor;
			}
			return base.GetOrignalActor();
		}

		public void SetHostActorInfo(ref PoolObjHandle<ActorRoot> InHostActor, SkillSlotType InSpawnSkillSlot, bool bInCopyedHeroInfo)
		{
			this.HostActor = InHostActor;
			this.SpawnSkillSlot = InSpawnSkillSlot;
			this.isDisplayHeroInfo = bInCopyedHeroInfo;
			if (this.HostActor)
			{
				this.HostActor.get_handle().ActorControl.eventActorDead += new ActorDeadEventHandler(this.OnActorDead);
			}
		}

		private void OnCheckDistance(int seq)
		{
			if (++this.DistanceTestCount >= 15 && this.actor != null)
			{
				this.DistanceTestCount = 0;
				if ((this.HostActor.get_handle().location - this.actor.location).get_sqrMagnitudeLong2D() >= MonsterWrapper.CheckDistance)
				{
					this.actor.location = this.HostActor.get_handle().location;
				}
			}
		}

		private void RemoveCheckDistanceTimer()
		{
		}

		private void OnActorDead(ref GameDeadEventParam prm)
		{
			if (prm.src == this.hostActor)
			{
				this.actor.Suicide();
				this.RemoveCheckDistanceTimer();
			}
		}

		protected override void InitDefaultState()
		{
			DebugHelper.Assert(this.actor != null && this.cfgInfo != null);
			this.BornTime = this.cfgInfo.iBornTime;
			if (this.BornTime > 0)
			{
				base.SetObjBehaviMode(ObjBehaviMode.State_Born);
				this.nextBehavior = ObjBehaviMode.State_Null;
			}
			else
			{
				base.InitDefaultState();
			}
		}

		public override string GetTypeName()
		{
			return "MonsterWrapper";
		}

		public override void Init()
		{
			base.Init();
			this.SpawnSkillSlot = SkillSlotType.SLOT_SKILL_VALID;
		}

		public override void OnUse()
		{
			base.OnUse();
			this.isBoss = false;
			this.nOutCombatHpRecoveryTick = 0;
			this.lifeTime = 0;
			this.BornTime = 0;
			this.cfgInfo = null;
			this.originalPos = VInt3.zero;
			this.originalMeshScale = Vector3.one;
			this.HostActor = default(PoolObjHandle<ActorRoot>);
			this.spawnSkillSlot = SkillSlotType.SLOT_SKILL_VALID;
			this.bCopyedHeroInfo = false;
			this.DistanceTestCount = 0;
			this.endurance = 0;
			this.isEnduranceDown = false;
			this.Pursuit = null;
		}

		public override void Born(ActorRoot owner)
		{
			base.Born(owner);
			this.originalPos = this.actor.location;
			if (this.actor.ActorMesh != null)
			{
				this.originalMeshScale = this.actor.ActorMesh.transform.localScale;
			}
			this.actor.isMovable = this.actor.ObjLinker.CanMovable;
			this.actor.MovementComponent = this.actor.CreateLogicComponent<PlayerMovement>(this.actor);
			this.actor.MatHurtEffect = this.actor.CreateActorComponent<MaterialHurtEffect>(this.actor);
			this.actor.ShadowEffect = this.actor.CreateActorComponent<UpdateShadowPlane>(this.actor);
			VCollisionShape.InitActorCollision(this.actor);
			this.cfgInfo = MonsterDataHelper.GetDataCfgInfo(this.actor.TheActorMeta.ConfigId, (int)this.actor.TheActorMeta.Difficuty);
			if (this.cfgInfo != null)
			{
				this.endurance = this.cfgInfo.iPursuitE;
			}
			DebugHelper.Assert(this.cfgInfo != null, "Failed find monster cfg by id {0}", new object[]
			{
				this.actor.TheActorMeta.ConfigId
			});
			if (this.cfgInfo != null && this.cfgInfo.bIsBoss > 0)
			{
				this.isBoss = true;
			}
			else
			{
				this.isBoss = false;
			}
			this.actorSubType = this.cfgInfo.bMonsterType;
			this.actorSubSoliderType = this.cfgInfo.bSoldierType;
		}

		protected void UpdateBornLogic(int delta)
		{
			this.BornTime -= delta;
			if (this.BornTime <= 0)
			{
				base.InitDefaultState();
			}
		}

		public override void UpdateLogic(int delta)
		{
			this.actor.ActorAgent.UpdateLogic(delta);
			base.UpdateLogic(delta);
			if (base.IsBornState)
			{
				this.UpdateBornLogic(delta);
			}
			else
			{
				if (this.isEnduranceDown)
				{
					this.endurance -= delta;
					if (this.endurance < 0)
					{
						this.endurance = 0;
					}
				}
				if (this.lifeTime > 0)
				{
					this.lifeTime -= delta;
					if (this.lifeTime <= 0)
					{
						base.SetObjBehaviMode(ObjBehaviMode.State_Dead);
					}
				}
				if (this.actorSubSoliderType == 8 && this.myBehavior == ObjBehaviMode.State_AutoAI && !this.actor.SkillControl.isUsing)
				{
					List<PoolObjHandle<ActorRoot>> heroActors = Singleton<GameObjMgr>.get_instance().HeroActors;
					Vector3 vec = this.actor.forward.get_vec3();
					long num = (long)this.cfgInfo.iPursuitR;
					num *= num;
					bool flag = false;
					int count = heroActors.get_Count();
					for (int i = 0; i < count; i++)
					{
						VInt3 location = heroActors.get_Item(i).get_handle().location;
						VInt3 vInt = location - this.actor.location;
						Vector3 vec2 = vInt.get_vec3();
						long sqrMagnitudeLong2D = vInt.get_sqrMagnitudeLong2D();
						if (sqrMagnitudeLong2D < this.BaronPretectDistance || (sqrMagnitudeLong2D < num && Vector3.Dot(vec, vec2) > 0f))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						base.SetObjBehaviMode(ObjBehaviMode.State_Idle);
					}
				}
				if (this.actor.ActorControl.IsDeadState || this.myBehavior != ObjBehaviMode.State_Idle)
				{
					this.nOutCombatHpRecoveryTick = 0;
				}
				else
				{
					this.nOutCombatHpRecoveryTick += delta;
					if (this.nOutCombatHpRecoveryTick >= 1000)
					{
						int nAddHp = this.cfgInfo.iOutCombatHPAdd * this.actor.ValueComponent.mActorValue[5].totalValue / 10000;
						base.ReviveHp(nAddHp);
						this.nOutCombatHpRecoveryTick -= 1000;
					}
				}
				if (this.isCalledMonster)
				{
					this.OnCheckDistance(0);
				}
			}
		}

		public override void Deactive()
		{
			if (this.actor.ActorMesh != null)
			{
				this.actor.ActorMesh.transform.localScale = this.originalMeshScale;
			}
			this.nOutCombatHpRecoveryTick = 0;
			this.HostActor = default(PoolObjHandle<ActorRoot>);
			this.spawnSkillSlot = SkillSlotType.SLOT_SKILL_VALID;
			this.bCopyedHeroInfo = false;
			this.DistanceTestCount = 0;
			base.Deactive();
		}

		public override void Fight()
		{
			base.Fight();
			if (this.cfgInfo.iLeftBattleFrame > 0)
			{
				this.m_battle_cool_ticks = this.cfgInfo.iLeftBattleFrame;
			}
			else
			{
				this.m_battle_cool_ticks = 30;
			}
			if (!Singleton<FrameSynchr>.GetInstance().bActive)
			{
				this.m_battle_cool_ticks *= 2;
			}
			this._inBattleCoolTick = this.m_battle_cool_ticks;
			if (this.cfgInfo.iDropProbability == 101)
			{
				BattleMisc.BossRoot = this.actorPtr;
			}
			if (FogOfWar.enable && this.actor.HorizonMarker != null && this.GetActorSubType() == 1)
			{
				this.actor.HorizonMarker.SightRadius = Horizon.QuerySoldierSightRadius();
			}
		}

		protected override void OnDead()
		{
			MonsterDropItemCreator monsterDropItemCreator = default(MonsterDropItemCreator);
			if (this.myLastAtker)
			{
				monsterDropItemCreator.MakeDropItemIfNeed(this, this.myLastAtker.get_handle().ActorControl);
			}
			else
			{
				monsterDropItemCreator.MakeDropItemIfNeed(this, null);
			}
			if (this.isCalledMonster)
			{
				this.RemoveCheckDistanceTimer();
			}
			base.OnDead();
			bool flag = true;
			if (flag)
			{
				Singleton<GameObjMgr>.get_instance().RecycleActor(this.actorPtr, this.cfgInfo.iRecyleTime);
			}
			if (this.isCalledMonster)
			{
				this.HostActor.get_handle().ActorControl.eventActorDead -= new ActorDeadEventHandler(this.OnActorDead);
			}
			if (this.actor.HorizonMarker != null)
			{
				this.actor.HorizonMarker.SetTranslucentMark(HorizonConfig.HideMark.Skill, false, false);
				this.actor.HorizonMarker.SetTranslucentMark(HorizonConfig.HideMark.Jungle, false, false);
			}
		}

		protected override void OnRevive()
		{
			base.OnRevive();
			base.EnableRVO(true);
		}

		public override int TakeDamage(ref HurtDataInfo hurt)
		{
			if (base.IsBornState)
			{
				base.SetObjBehaviMode(ObjBehaviMode.State_Idle);
				this.nextBehavior = ObjBehaviMode.State_Null;
				base.PlayAnimation("Idle", 0f, 0, true);
			}
			return base.TakeDamage(ref hurt);
		}

		protected override void OnBehaviModeChange(ObjBehaviMode oldState, ObjBehaviMode curState)
		{
			base.OnBehaviModeChange(oldState, curState);
			if (curState == ObjBehaviMode.State_Idle)
			{
				base.EnableRVO(false);
			}
		}

		public override bool DoesApplyExposingRule()
		{
			return this.actorSubType == 2;
		}

		public override bool DoesIgnoreAlreadyLit()
		{
			return false;
		}

		public override void BeAttackHit(PoolObjHandle<ActorRoot> atker, bool bExposeAttacker)
		{
			if (!this.actor.IsSelfCamp(atker.get_handle()))
			{
				if (this.actorSubType == 2)
				{
					long num = (long)this.cfgInfo.iPursuitR;
					if (this.actor.ValueComponent.actorHp * 10000 / this.actor.ValueComponent.actorHpTotal > 500 || (atker.get_handle().location - this.originalPos).get_sqrMagnitudeLong2D() < num * num)
					{
						if (this.actorSubSoliderType == 8)
						{
							Vector3 vec = this.actor.forward.get_vec3();
							Vector3 rhs = atker.get_handle().location.get_vec3() - this.actor.location.get_vec3();
							if (Vector3.Dot(vec, rhs) < 0f)
							{
								if ((atker.get_handle().location - this.actor.location).get_sqrMagnitudeLong2D() < this.BaronPretectDistance)
								{
									base.SetInBattle();
									this.m_isAttacked = true;
								}
								else
								{
									for (int i = 0; i < Singleton<GameObjMgr>.get_instance().HeroActors.get_Count(); i++)
									{
										Vector3 rhs2 = Singleton<GameObjMgr>.get_instance().HeroActors.get_Item(i).get_handle().location.get_vec3() - this.actor.location.get_vec3();
										VInt3 vInt = Singleton<GameObjMgr>.get_instance().HeroActors.get_Item(i).get_handle().location - this.actor.location;
										if (vInt.get_sqrMagnitudeLong2D() < this.BaronPretectDistance || (vInt.get_sqrMagnitudeLong2D() < num * num && Vector3.Dot(vec, rhs2) > 0f))
										{
											base.SetInBattle();
											this.m_isAttacked = true;
											break;
										}
									}
								}
							}
							else
							{
								base.SetInBattle();
								this.m_isAttacked = true;
							}
							Singleton<SoundCookieSys>.get_instance().OnBaronAttacked(this, atker.get_handle());
						}
						else
						{
							base.SetInBattle();
							this.m_isAttacked = true;
						}
					}
				}
				else
				{
					base.SetInBattle();
					this.m_isAttacked = true;
				}
				atker.get_handle().ActorControl.SetInBattle();
				atker.get_handle().ActorControl.SetInAttack(this.actorPtr, bExposeAttacker);
				DefaultGameEventParam defaultGameEventParam = new DefaultGameEventParam(base.GetActor(), atker);
				Singleton<GameEventSys>.get_instance().SendEvent<DefaultGameEventParam>(GameEventDef.Event_ActorBeAttack, ref defaultGameEventParam);
			}
		}

		public override bool IsBossOrHeroAutoAI()
		{
			return this.isBoss;
		}
	}
}
