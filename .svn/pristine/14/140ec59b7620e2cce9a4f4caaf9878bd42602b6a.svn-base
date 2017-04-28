using Assets.Scripts.Framework;
using ResData;
using System;
using System.Runtime.CompilerServices;

namespace Assets.Scripts.GameLogic
{
	public class StarSystem : Singleton<StarSystem>
	{
		protected ListView<IStarEvaluation> StarEvaluations = new ListView<IStarEvaluation>(3);

		protected IStarEvaluation FailureEvaluation;

		public event OnEvaluationChangedDelegate OnEvaluationChanged
		{
			[MethodImpl(32)]
			add
			{
				this.OnEvaluationChanged = (OnEvaluationChangedDelegate)Delegate.Combine(this.OnEvaluationChanged, value);
			}
			[MethodImpl(32)]
			remove
			{
				this.OnEvaluationChanged = (OnEvaluationChangedDelegate)Delegate.Remove(this.OnEvaluationChanged, value);
			}
		}

		public event OnEvaluationChangedDelegate OnFailureEvaluationChanged
		{
			[MethodImpl(32)]
			add
			{
				this.OnFailureEvaluationChanged = (OnEvaluationChangedDelegate)Delegate.Combine(this.OnFailureEvaluationChanged, value);
			}
			[MethodImpl(32)]
			remove
			{
				this.OnFailureEvaluationChanged = (OnEvaluationChangedDelegate)Delegate.Remove(this.OnFailureEvaluationChanged, value);
			}
		}

		public IStarEvaluation winEvaluation
		{
			get
			{
				IStarEvaluation arg_25_0;
				if (this.StarEvaluations.get_Count() > 0)
				{
					IStarEvaluation starEvaluation = this.StarEvaluations.get_Item(0);
					arg_25_0 = starEvaluation;
				}
				else
				{
					arg_25_0 = null;
				}
				return arg_25_0;
			}
		}

		public IStarEvaluation failureEvaluation
		{
			get
			{
				return this.FailureEvaluation;
			}
		}

		public bool isFailure
		{
			get
			{
				return this.FailureEvaluation != null && this.FailureEvaluation.status == StarEvaluationStatus.Success;
			}
		}

		public int starCount
		{
			get
			{
				int num = 0;
				ListView<IStarEvaluation>.Enumerator enumerator = this.GetEnumerator();
				while (enumerator.MoveNext())
				{
					if (enumerator.get_Current().isSuccess)
					{
						num++;
					}
				}
				return num;
			}
		}

		public bool isFirstStarCompleted
		{
			get
			{
				return this.StarEvaluations.get_Count() > 0 && this.StarEvaluations.get_Item(0).isSuccess;
			}
		}

		protected IStarEvaluation CreateStar(ResEvaluateStarInfo ConditionDetail, int InIndex)
		{
			StarEvaluation starEvaluation = new StarEvaluation();
			starEvaluation.OnChanged += new OnEvaluationChangedDelegate(this.OnEvaluationChangedInner);
			starEvaluation.Index = InIndex;
			starEvaluation.Initialize(ConditionDetail);
			return starEvaluation;
		}

		private void OnEvaluationChangedInner(IStarEvaluation InStarEvaluation, IStarCondition InStarCondition)
		{
			if (InStarEvaluation == this.FailureEvaluation)
			{
				if (this.OnFailureEvaluationChanged != null)
				{
					this.OnFailureEvaluationChanged(InStarEvaluation, InStarCondition);
				}
			}
			else if (this.OnEvaluationChanged != null)
			{
				this.OnEvaluationChanged(InStarEvaluation, InStarCondition);
			}
		}

		public void StartFight()
		{
			SLevelContext curLvelContext = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
			if (curLvelContext != null && !Singleton<LobbyLogic>.get_instance().inMultiGame && !curLvelContext.IsMobaMode())
			{
				Singleton<StarSystem>.get_instance().Reset(curLvelContext.m_mapID);
				StarSystem expr_41 = Singleton<StarSystem>.get_instance();
				expr_41.OnEvaluationChanged = (OnEvaluationChangedDelegate)Delegate.Combine(expr_41.OnEvaluationChanged, new OnEvaluationChangedDelegate(BattleLogic.OnStarSystemChanged));
				StarSystem expr_67 = Singleton<StarSystem>.get_instance();
				expr_67.OnFailureEvaluationChanged = (OnEvaluationChangedDelegate)Delegate.Combine(expr_67.OnFailureEvaluationChanged, new OnEvaluationChangedDelegate(BattleLogic.OnFailureEvaluationChanged));
				Singleton<StarSystem>.get_instance().Start();
			}
		}

		public void EndGame()
		{
			StarSystem expr_05 = Singleton<StarSystem>.get_instance();
			expr_05.OnFailureEvaluationChanged = (OnEvaluationChangedDelegate)Delegate.Remove(expr_05.OnFailureEvaluationChanged, new OnEvaluationChangedDelegate(BattleLogic.OnFailureEvaluationChanged));
			StarSystem expr_2B = Singleton<StarSystem>.get_instance();
			expr_2B.OnEvaluationChanged = (OnEvaluationChangedDelegate)Delegate.Remove(expr_2B.OnEvaluationChanged, new OnEvaluationChangedDelegate(BattleLogic.OnStarSystemChanged));
		}

		public byte GetStarBits()
		{
			byte b = 0;
			byte b2 = 1;
			for (int i = 0; i < this.StarEvaluations.get_Count(); i++)
			{
				if (this.StarEvaluations.get_Item(i) != null && this.StarEvaluations.get_Item(i).status == StarEvaluationStatus.Success)
				{
					b |= b2;
				}
				b2 = (byte)(b2 << 1);
			}
			return b;
		}

		public ListView<IStarEvaluation>.Enumerator GetEnumerator()
		{
			return this.StarEvaluations.GetEnumerator();
		}

		public void Start()
		{
			ListView<IStarEvaluation>.Enumerator enumerator = this.StarEvaluations.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.get_Current() != null)
				{
					enumerator.get_Current().Start();
				}
			}
			if (this.FailureEvaluation != null)
			{
				this.FailureEvaluation.Start();
			}
			Singleton<GameEventSys>.get_instance().AddEventHandler<GameDeadEventParam>(GameEventDef.Event_PostActorDead, new RefAction<GameDeadEventParam>(this.OnActorDeath));
		}

		public void Clear()
		{
			ListView<IStarEvaluation>.Enumerator enumerator = this.StarEvaluations.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.get_Current() != null)
				{
					enumerator.get_Current().Dispose();
				}
			}
			this.StarEvaluations.Clear();
			if (this.FailureEvaluation != null)
			{
				this.FailureEvaluation.Dispose();
				this.FailureEvaluation = null;
			}
			Singleton<GameEventSys>.get_instance().RmvEventHandler<GameDeadEventParam>(GameEventDef.Event_PostActorDead, new RefAction<GameDeadEventParam>(this.OnActorDeath));
		}

		private void OnActorDeath(ref GameDeadEventParam prm)
		{
			int num = this.StarEvaluations.get_Count() - 1;
			while (num >= 0 && num < this.StarEvaluations.get_Count())
			{
				IStarEvaluation starEvaluation = this.StarEvaluations.get_Item(num);
				if (starEvaluation != null)
				{
					starEvaluation.OnActorDeath(ref prm);
				}
				num--;
			}
			if (this.FailureEvaluation != null)
			{
				this.FailureEvaluation.OnActorDeath(ref prm);
			}
		}

		public bool Reset(int LevelID)
		{
			this.Clear();
			SLevelContext curLvelContext = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
			DebugHelper.Assert(curLvelContext != null);
			if (curLvelContext == null)
			{
				return false;
			}
			for (int i = 0; i < curLvelContext.m_starDetail.Length; i++)
			{
				ResDT_IntParamArrayNode resDT_IntParamArrayNode = curLvelContext.m_starDetail[i];
				if (resDT_IntParamArrayNode.iParam == 0)
				{
					break;
				}
				this.AddStarEvaluation(resDT_IntParamArrayNode.iParam);
			}
			if (curLvelContext.m_loseCondition != 0)
			{
				ResEvaluateStarInfo dataByKey = GameDataMgr.evaluateCondInfoDatabin.GetDataByKey((uint)curLvelContext.m_loseCondition);
				DebugHelper.Assert(dataByKey != null);
				if (dataByKey == null)
				{
					return false;
				}
				this.FailureEvaluation = this.CreateStar(dataByKey, 0);
				DebugHelper.Assert(this.FailureEvaluation != null, "我擦，怎会没有？");
			}
			Singleton<EventRouter>.get_instance().BroadCastEvent(EventID.StarSystemInitialized);
			return true;
		}

		protected void AddStarEvaluation(int CondID)
		{
			ResEvaluateStarInfo dataByKey = GameDataMgr.evaluateCondInfoDatabin.GetDataByKey((uint)CondID);
			DebugHelper.Assert(dataByKey != null);
			if (dataByKey == null)
			{
				return;
			}
			IStarEvaluation starEvaluation = this.CreateStar(dataByKey, this.StarEvaluations.get_Count());
			this.StarEvaluations.Add(starEvaluation);
		}

		public IStarEvaluation GetEvaluationAt(int Index)
		{
			if (Index >= 0 && this.StarEvaluations != null && Index < this.StarEvaluations.get_Count())
			{
				return this.StarEvaluations.get_Item(Index);
			}
			return null;
		}
	}
}
