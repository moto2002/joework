using Assets.Scripts.Framework;
using ResData;
using System;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
	public class SoldierRegion : FuncRegion
	{
		public int WaveID;

		public int RouteID;

		public GameObject finalTarget;

		private int _toSwitchWaveID;

		public GameObject AttackRoute;

		public bool bForceCompleteSpawn = true;

		[HideInInspector]
		[NonSerialized]
		public ListView<SoldierWave> Waves = new ListView<SoldierWave>();

		[HideInInspector]
		[NonSerialized]
		public SoldierWave CurrentWave;

		private SoldierWave _lastWave;

		private int curTick;

		private bool bShouldWait;

		private int waitTick;

		[HideInInspector]
		[NonSerialized]
		public bool bTriggerEvent;

		private bool bInited;

		private bool bShouldReset;

		public static bool bFirstSpawnEvent;

		public void Awake()
		{
			this.LoadWave(this.WaveID);
			this._toSwitchWaveID = 0;
		}

		private void LoadWave(int theWaveID)
		{
			DebugHelper.Assert(GameDataMgr.soldierWaveDatabin != null);
			ResSoldierWaveInfo dataByKey = GameDataMgr.soldierWaveDatabin.GetDataByKey((uint)theWaveID);
			int num = 0;
			while (dataByKey != null)
			{
				this.Waves.Add(new SoldierWave(this, dataByKey, num++));
				dataByKey = GameDataMgr.soldierWaveDatabin.GetDataByKey(dataByKey.dwNextSoldierWaveID);
			}
		}

		public void SwitchWave(int newWaveID)
		{
			if (this.CurrentWave == null || this.CurrentWave.IsInIdle)
			{
				this._lastWave = this.CurrentWave;
				this.CurrentWave = null;
				this.bShouldReset = true;
				this.waitTick = 0;
				this.curTick = 0;
				this.Waves.Clear();
				this.LoadWave(newWaveID);
				this._toSwitchWaveID = 0;
			}
			else
			{
				this._toSwitchWaveID = newWaveID;
			}
		}

		private SoldierWave FindNextValidWave()
		{
			if (this.CurrentWave == null)
			{
				this.CurrentWave = ((this.Waves.get_Count() <= 0) ? null : this.Waves.get_Item(0));
			}
			else
			{
				this.CurrentWave = ((this.CurrentWave.Index >= this.Waves.get_Count() - 1) ? null : this.Waves.get_Item(this.CurrentWave.Index + 1));
			}
			if (this.CurrentWave != null && this._lastWave != null)
			{
				this.CurrentWave.CloneState(this._lastWave);
				this._lastWave = null;
			}
			if (this.bTriggerEvent)
			{
				int inWaveIndex = (this.CurrentWave == null) ? (this.Waves.get_Count() - 1) : this.CurrentWave.Index;
				SoldierWaveParam soldierWaveParam = new SoldierWaveParam(inWaveIndex, 0, this.GetNextRepeatTime(true));
				Singleton<GameEventSys>.get_instance().SendEvent<SoldierWaveParam>(GameEventDef.Event_SoldierWaveNext, ref soldierWaveParam);
			}
			return this.CurrentWave;
		}

		public int GetTotalCount()
		{
			int num = 0;
			ListView<SoldierWave>.Enumerator enumerator = this.Waves.GetEnumerator();
			while (enumerator.MoveNext())
			{
				SoldierWave current = enumerator.get_Current();
				if (current.WaveInfo.dwRepeatNum == 0u)
				{
					return 0;
				}
				num += (int)current.WaveInfo.dwRepeatNum;
			}
			return num;
		}

		public int GetRepeatCountTill(int inWaveIndex)
		{
			int num = 0;
			int num2 = 0;
			ListView<SoldierWave>.Enumerator enumerator = this.Waves.GetEnumerator();
			while (enumerator.MoveNext() && num < inWaveIndex)
			{
				SoldierWave current = enumerator.get_Current();
				if (current.WaveInfo.dwRepeatNum == 0u)
				{
					return 0;
				}
				num2 += (int)current.WaveInfo.dwRepeatNum;
				num++;
			}
			int repeatCount = this.Waves.get_Item(inWaveIndex).repeatCount;
			return num2 + repeatCount;
		}

		public int GetTotalTime()
		{
			int num = 0;
			ListView<SoldierWave>.Enumerator enumerator = this.Waves.GetEnumerator();
			while (enumerator.MoveNext())
			{
				SoldierWave current = enumerator.get_Current();
				if (current.WaveInfo.dwRepeatNum == 0u)
				{
					return 0;
				}
				num += (int)current.WaveInfo.dwStartWatiTick;
				num += (int)(current.WaveInfo.dwIntervalTick * (current.WaveInfo.dwRepeatNum - 1u));
			}
			return num;
		}

		public int GetNextRepeatTime(bool bWaveOrRepeat)
		{
			if (this.CurrentWave == null)
			{
				return -1;
			}
			if (bWaveOrRepeat)
			{
				return (int)this.CurrentWave.WaveInfo.dwStartWatiTick;
			}
			bool flag = (long)this.CurrentWave.repeatCount >= (long)((ulong)this.CurrentWave.WaveInfo.dwRepeatNum);
			if (!flag)
			{
				return (int)(this.CurrentWave.WaveInfo.dwIntervalTick + this.CurrentWave.Selector.StatTotalCount * (uint)MonoSingleton<GlobalConfig>.get_instance().SoldierWaveInterval);
			}
			bool flag2 = this.CurrentWave.Index + 1 < this.Waves.get_Count();
			if (flag2)
			{
				SoldierWave soldierWave = this.Waves.get_Item(this.CurrentWave.Index + 1);
				return (int)(this.CurrentWave.Selector.StatTotalCount * (uint)MonoSingleton<GlobalConfig>.get_instance().SoldierWaveInterval + this.CurrentWave.WaveInfo.dwIntervalTick + soldierWave.WaveInfo.dwStartWatiTick);
			}
			return -1;
		}

		public SoldierSpawnResult UpdateLogicSpec(int delta)
		{
			if (!this.isStartup)
			{
				return SoldierSpawnResult.UnStarted;
			}
			if (this.CurrentWave == null && !this.bShouldReset)
			{
				return SoldierSpawnResult.Completed;
			}
			if (this._toSwitchWaveID > 0 && this.CurrentWave != null && this.CurrentWave.IsInIdle)
			{
				this.SwitchWave(this._toSwitchWaveID);
			}
			if (this.bShouldWait || this.bShouldReset)
			{
				this.bShouldReset = false;
				this.curTick += delta;
				if (this.curTick <= this.waitTick)
				{
					return SoldierSpawnResult.ShouldWaitInterval;
				}
				this.FindNextValidWave();
				this.bShouldWait = false;
				this.curTick = 0;
				this.waitTick = 0;
				if (this.CurrentWave == null)
				{
					return SoldierSpawnResult.Completed;
				}
			}
			SoldierSpawnResult soldierSpawnResult = this.CurrentWave.Update(delta);
			if (soldierSpawnResult > SoldierSpawnResult.ThresholdShouldWait)
			{
				this.bShouldWait = true;
				this.curTick = 0;
				this.waitTick = (int)this.CurrentWave.WaveInfo.dwIntervalTick;
			}
			return soldierSpawnResult;
		}

		public override void Startup()
		{
			base.Startup();
			if (!this.bInited)
			{
				this.FindNextValidWave();
				this.bInited = true;
			}
		}

		public void ResetRegion()
		{
			this.CurrentWave = null;
			this.bShouldReset = true;
			this.waitTick = 0;
			this.curTick = 0;
			if (this.Waves != null)
			{
				for (int i = 0; i < this.Waves.get_Count(); i++)
				{
					if (this.Waves.get_Item(i) != null)
					{
						this.Waves.get_Item(i).Reset();
					}
				}
			}
		}
	}
}
