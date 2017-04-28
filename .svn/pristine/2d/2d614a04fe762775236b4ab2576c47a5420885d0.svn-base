using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using CSProtocol;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameLogic
{
	public class CBattleBuffStat
	{
		private List<int> m_buffRecordCamp1 = new List<int>();

		private List<int> m_buffRecordCamp2 = new List<int>();

		private ulong m_lastRecordFrameTick;

		private int m_timerSeq = -1;

		public void StartRecord()
		{
			this.m_buffRecordCamp1.Clear();
			this.m_buffRecordCamp2.Clear();
			this.m_lastRecordFrameTick = 0uL;
			this.RemoveTimerEvent();
			this.AddTimerEvent();
		}

		public void AddTimerEvent()
		{
			if (this.m_timerSeq != -1)
			{
				return;
			}
			this.m_timerSeq = Singleton<CTimerManager>.get_instance().AddTimer(1000, 0, new CTimer.OnTimeUpHandler(this.OnCheckPlayerBuff));
		}

		public void RemoveTimerEvent()
		{
			if (this.m_timerSeq != -1)
			{
				Singleton<CTimerManager>.get_instance().RemoveTimer(this.m_timerSeq);
				this.m_timerSeq = -1;
			}
		}

		private void OnCheckPlayerBuff(int timerSequence)
		{
			if (Singleton<FrameSynchr>.get_instance().LogicFrameTick - this.m_lastRecordFrameTick < 30000uL)
			{
				return;
			}
			this.m_lastRecordFrameTick = Singleton<FrameSynchr>.get_instance().LogicFrameTick;
			int dragonBuffId = Singleton<BattleLogic>.get_instance().GetDragonBuffId(4);
			int num = 0;
			int num2 = 0;
			List<Player> allPlayers = Singleton<GamePlayerCenter>.get_instance().GetAllPlayers();
			List<Player>.Enumerator enumerator = allPlayers.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Player current = enumerator.get_Current();
				ReadonlyContext<PoolObjHandle<ActorRoot>>.Enumerator enumerator2 = current.GetAllHeroes().GetEnumerator();
				while (enumerator2.MoveNext())
				{
					if (enumerator2.get_Current())
					{
						PoolObjHandle<ActorRoot> current2 = enumerator2.get_Current();
						if (current2.get_handle().BuffHolderComp.FindBuff(dragonBuffId) != null)
						{
							if (current.PlayerCamp == 1)
							{
								num++;
							}
							else if (current.PlayerCamp == 2)
							{
								num2++;
							}
						}
						break;
					}
				}
			}
			this.m_buffRecordCamp1.Add(num);
			this.m_buffRecordCamp2.Add(num2);
		}

		public int GetRecordCnt()
		{
			return Math.Min(this.m_buffRecordCamp1.get_Count(), this.m_buffRecordCamp2.get_Count());
		}

		public int GetDataByIndex(COM_PLAYERCAMP camp, int index)
		{
			if (index >= this.GetRecordCnt())
			{
				return 0;
			}
			if (camp == 1)
			{
				return this.m_buffRecordCamp1.get_Item(index);
			}
			if (camp == 2)
			{
				return this.m_buffRecordCamp2.get_Item(index);
			}
			return 0;
		}
	}
}
