using Assets.Scripts.Common;
using Assets.Scripts.GameLogic.GameKernal;
using Assets.Scripts.GameSystem;
using ResData;
using System;

namespace Assets.Scripts.GameLogic
{
	public class EyeWrapper : ObjWrapper
	{
		private const string DeadAnimName = "Eye_Dead";

		public bool bLifeTimeOver;

		private int lifeTime;

		public int lifeTimeTotal
		{
			get;
			private set;
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
				this.lifeTimeTotal = value;
				this.UpdateTimerBar();
			}
		}

		public ResMonsterCfgInfo cfgInfo
		{
			get;
			private set;
		}

		private int RecycleTime
		{
			get
			{
				if (this.cfgInfo != null)
				{
					return this.cfgInfo.iRecyleTime;
				}
				return 0;
			}
		}

		public override void OnUse()
		{
			base.OnUse();
			this.lifeTime = 0;
			this.lifeTimeTotal = 0;
			this.cfgInfo = null;
			this.bLifeTimeOver = false;
		}

		public override void Born(ActorRoot owner)
		{
			this.actor = owner;
			this.actorPtr = new PoolObjHandle<ActorRoot>(this.actor);
			base.ClearNeedToHelpOther();
			base.ClearNeedSwitchTarget();
			this.m_curWaypointsHolder = null;
			this.m_curWaypointTarget = null;
			this.m_isCurWaypointEndPoint = false;
			this.m_isStartPoint = false;
			this.m_isControledByMan = true;
			this.m_isAutoAI = false;
			this.m_offline = false;
			this.m_followOther = false;
			this.m_leaderID = 0u;
			this.m_isAttackedByEnemyHero = false;
			this.m_isAttacked = false;
			this.bForceNotRevive = false;
			this.actor.SkillControl = this.actor.CreateLogicComponent<SkillComponent>(this.actor);
			this.actor.ValueComponent = this.actor.CreateLogicComponent<ValueProperty>(this.actor);
			this.actor.HurtControl = this.actor.CreateLogicComponent<HurtComponent>(this.actor);
			this.actor.BuffHolderComp = this.actor.CreateLogicComponent<BuffHolderComponent>(this.actor);
			this.actor.AnimControl = this.actor.CreateLogicComponent<AnimPlayComponent>(this.actor);
			this.actor.HudControl = this.actor.CreateLogicComponent<HudComponent3D>(this.actor);
			if (FogOfWar.enable)
			{
				this.actor.HorizonMarker = this.actor.CreateLogicComponent<HorizonMarkerByFow>(this.actor);
			}
			else
			{
				this.actor.HorizonMarker = this.actor.CreateLogicComponent<HorizonMarker>(this.actor);
			}
			this.actor.MatHurtEffect = this.actor.CreateActorComponent<MaterialHurtEffect>(this.actor);
			if (this.actor.MatHurtEffect != null && this.actor.MatHurtEffect.mats != null)
			{
				this.actor.MatHurtEffect.mats.Clear();
				this.actor.MatHurtEffect.mats = null;
			}
			this.cfgInfo = MonsterDataHelper.GetDataCfgInfo(this.actor.TheActorMeta.ConfigId, 1);
			this.bLifeTimeOver = false;
		}

		protected override void OnDead()
		{
			base.OnDead();
			bool flag = true;
			if (flag)
			{
				Singleton<GameObjMgr>.get_instance().RecycleActor(this.actorPtr, this.RecycleTime);
			}
			if (!string.IsNullOrEmpty("Eye_Dead"))
			{
				AnimPlayComponent animControl = this.actor.AnimControl;
				if (animControl != null)
				{
					animControl.Play(new PlayAnimParam
					{
						animName = "Eye_Dead",
						blendTime = 0f,
						loop = false,
						layer = 1,
						speed = 1f,
						cancelCurrent = true,
						cancelAll = true
					});
				}
			}
			if (this.actor.HorizonMarker != null)
			{
				this.actor.HorizonMarker.SetTranslucentMark(HorizonConfig.HideMark.Skill, false, false);
				this.actor.HorizonMarker.SetTranslucentMark(HorizonConfig.HideMark.Jungle, false, false);
			}
		}

		public override string GetTypeName()
		{
			return "EyeWrapper";
		}

		public override void Init()
		{
			base.Init();
			if (FogOfWar.enable)
			{
				Player hostPlayer = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer();
				if (hostPlayer != null)
				{
					if (this.actor.TheActorMeta.ActorCamp == hostPlayer.PlayerCamp)
					{
						this.actor.Visible = true;
					}
					else
					{
						VInt3 worldLoc = new VInt3(this.actor.location.x, this.actor.location.z, 0);
						this.actor.Visible = Singleton<GameFowManager>.get_instance().IsSurfaceCellVisible(worldLoc, hostPlayer.PlayerCamp);
					}
				}
			}
		}

		public override void Uninit()
		{
			base.Uninit();
		}

		public override void Prepare()
		{
			base.Prepare();
		}

		public override void Deactive()
		{
			base.Deactive();
		}

		public override void Reactive()
		{
			base.Reactive();
			this.LifeTime = 0;
		}

		public override void Fight()
		{
			base.Fight();
		}

		public override void FightOver()
		{
			base.FightOver();
		}

		private void UpdateTimerBar()
		{
			if (this.actor.HudControl != null)
			{
				this.actor.HudControl.UpdateTimerBar(this.lifeTime, this.lifeTimeTotal);
			}
		}

		public override void UpdateLogic(int delta)
		{
			base.updateAffectActors();
			if (this.lifeTime > 0)
			{
				this.lifeTime -= delta;
				if (this.lifeTime <= 0)
				{
					this.lifeTime = 0;
					this.bLifeTimeOver = true;
					base.SetObjBehaviMode(ObjBehaviMode.State_Dead);
				}
				this.UpdateTimerBar();
			}
		}
	}
}
