using Assets.Scripts.Common;
using Assets.Scripts.GameLogic.GameKernal;
using CSProtocol;
using ResData;
using System;

namespace Assets.Scripts.GameLogic
{
	[StarConditionAttrContext(16)]
	internal class StarConditionDataStatTimer : StarCondition
	{
		private bool bCheckResults;

		private PoolObjHandle<ActorRoot> CachedSource;

		private PoolObjHandle<ActorRoot> CachedAttacker;

		private int TimeSeq = -1;

		private bool m_bTimeUp;

		public override int[] values
		{
			get
			{
				return new int[1];
			}
		}

		public RES_STAR_CONDITION_DATA_SUB_TYPE DataSubType
		{
			get
			{
				return base.ConditionInfo.KeyDetail[1];
			}
		}

		private int TargetCamp
		{
			get
			{
				return base.ConditionInfo.KeyDetail[2];
			}
		}

		public int TimerDuration
		{
			get
			{
				return (int)Singleton<WinLoseByStarSys>.get_instance().CurLevelTimeDuration;
			}
		}

		public override StarEvaluationStatus status
		{
			get
			{
				return (!this.bCheckResults || !this.m_bTimeUp) ? StarEvaluationStatus.Failure : StarEvaluationStatus.Success;
			}
		}

		public override void Start()
		{
			base.Start();
			this.m_bTimeUp = false;
			this.TimeSeq = Singleton<CTimerManager>.get_instance().AddTimer(this.TimerDuration, 1, new CTimer.OnTimeUpHandler(this.OnTimeUp), true);
		}

		private void OnTimeUp(int inSeq)
		{
			if (this.TimeSeq >= 0)
			{
				Singleton<CTimerManager>.get_instance().RemoveTimer(this.TimeSeq);
				this.TimeSeq = -1;
				this.UpdateCheckResults();
				this.m_bTimeUp = true;
				this.TriggerChangedEvent();
			}
		}

		private void UpdateCheckResults()
		{
			COM_PLAYERCAMP hostPlayerCamp = Singleton<GamePlayerCenter>.get_instance().hostPlayerCamp;
			COM_PLAYERCAMP cOM_PLAYERCAMP = 0;
			if (hostPlayerCamp == 1)
			{
				cOM_PLAYERCAMP = 2;
			}
			else if (hostPlayerCamp == 2)
			{
				cOM_PLAYERCAMP = 1;
			}
			uint score = (uint)Singleton<BattleStatistic>.get_instance().GetCampStat().get_Item(hostPlayerCamp).GetScore(this.DataSubType);
			uint score2 = (uint)Singleton<BattleStatistic>.get_instance().GetCampStat().get_Item(cOM_PLAYERCAMP).GetScore(this.DataSubType);
			if (score != score2)
			{
				if (this.TargetCamp == 0)
				{
					this.bCheckResults = SmartCompare.Compare<uint>(score, score2, this.operation);
					Singleton<BattleStatistic>.get_instance().bSelfCampHaveWinningFlag = this.bCheckResults;
				}
				else if (this.TargetCamp == 1)
				{
					this.bCheckResults = SmartCompare.Compare<uint>(score2, score, this.operation);
					Singleton<BattleStatistic>.get_instance().bSelfCampHaveWinningFlag = !this.bCheckResults;
				}
			}
			else if (score == 0u && this.TargetCamp == 1)
			{
				this.bCheckResults = true;
				Singleton<BattleStatistic>.get_instance().bSelfCampHaveWinningFlag = false;
			}
		}

		public override void Dispose()
		{
			if (this.TimeSeq >= 0)
			{
				Singleton<CTimerManager>.get_instance().RemoveTimer(this.TimeSeq);
				this.TimeSeq = -1;
			}
			base.Dispose();
		}

		public override void OnCampScoreUpdated(ref SCampScoreUpdateParam prm)
		{
			this.UpdateCheckResults();
		}

		public override bool GetActorRef(out PoolObjHandle<ActorRoot> OutSource, out PoolObjHandle<ActorRoot> OutAttacker)
		{
			OutSource = this.CachedSource;
			OutAttacker = this.CachedAttacker;
			return true;
		}
	}
}
