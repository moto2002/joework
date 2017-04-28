using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.DataCenter;
using Assets.Scripts.GameLogic.GameKernal;
using Assets.Scripts.GameSystem;
using CSProtocol;
using ResData;
using System;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
	public class SoldierWave
	{
		private int curTick;

		private int preSpawnTick;

		private int firstTick = -1;

		public int repeatCount = 1;

		public SoldierSelector Selector = new SoldierSelector();

		private bool bInIdleState;

		private int idleTick;

		private bool isCannonNotified;

		public static uint ms_updatedFrameNum;

		public SoldierRegion Region
		{
			get;
			protected set;
		}

		public ResSoldierWaveInfo WaveInfo
		{
			get;
			protected set;
		}

		public int Index
		{
			get;
			protected set;
		}

		public bool IsInIdle
		{
			get
			{
				return this.bInIdleState;
			}
		}

		public SoldierWave(SoldierRegion InRegion, ResSoldierWaveInfo InWaveInfo, int InIndex)
		{
			this.Region = InRegion;
			this.WaveInfo = InWaveInfo;
			this.Index = InIndex;
			DebugHelper.Assert(this.Region != null && InWaveInfo != null);
			this.isCannonNotified = false;
			this.Reset();
		}

		public void Reset()
		{
			this.curTick = (this.preSpawnTick = 0);
			this.firstTick = -1;
			this.repeatCount = 1;
			this.bInIdleState = false;
			this.idleTick = 0;
			this.Selector.Reset(this.WaveInfo);
		}

		public void CloneState(SoldierWave sw)
		{
			this.curTick = sw.curTick;
			this.preSpawnTick = sw.preSpawnTick;
			this.firstTick = sw.firstTick;
			this.bInIdleState = sw.bInIdleState;
			this.idleTick = sw.idleTick;
			this.bInIdleState = sw.bInIdleState;
			this.isCannonNotified = sw.isCannonNotified;
		}

		public SoldierSpawnResult Update(int delta)
		{
			this.firstTick = ((this.firstTick != -1) ? this.firstTick : this.curTick);
			this.curTick += delta;
			if (this.bInIdleState)
			{
				if ((long)(this.curTick - this.idleTick) < (long)((ulong)this.WaveInfo.dwIntervalTick))
				{
					return SoldierSpawnResult.ShouldWaitInterval;
				}
				this.bInIdleState = false;
				this.Selector.Reset(this.WaveInfo);
				this.repeatCount++;
			}
			if ((long)this.curTick < (long)((ulong)this.WaveInfo.dwStartWatiTick))
			{
				return SoldierSpawnResult.ShouldWaitStart;
			}
			if ((long)(this.curTick - this.firstTick) >= (long)((ulong)this.WaveInfo.dwRepeatTimeTick) && this.WaveInfo.dwRepeatTimeTick > 0u && (!this.Region.bForceCompleteSpawn || (this.Region.bForceCompleteSpawn && this.Selector.isFinished)))
			{
				return SoldierSpawnResult.Finish;
			}
			if (this.curTick - this.preSpawnTick < MonoSingleton<GlobalConfig>.get_instance().SoldierWaveInterval)
			{
				return SoldierSpawnResult.ShouldWaitSoldierInterval;
			}
			if (!this.Selector.isFinished)
			{
				if (SoldierWave.ms_updatedFrameNum < Singleton<FrameSynchr>.get_instance().CurFrameNum)
				{
					uint num = this.Selector.NextSoldierID();
					DebugHelper.Assert(num != 0u);
					this.SpawnSoldier(num);
					this.preSpawnTick = this.curTick;
					SoldierWave.ms_updatedFrameNum = Singleton<FrameSynchr>.get_instance().CurFrameNum;
				}
				return SoldierSpawnResult.ShouldWaitSoldierInterval;
			}
			if (this.WaveInfo.dwRepeatNum == 0u || (long)this.repeatCount < (long)((ulong)this.WaveInfo.dwRepeatNum))
			{
				this.bInIdleState = true;
				this.idleTick = this.curTick;
				return SoldierSpawnResult.ShouldWaitInterval;
			}
			return SoldierSpawnResult.Finish;
		}

		private void SpawnSoldier(uint SoldierID)
		{
			ResMonsterCfgInfo dataCfgInfoByCurLevelDiff = MonsterDataHelper.GetDataCfgInfoByCurLevelDiff((int)SoldierID);
			if (dataCfgInfoByCurLevelDiff == null)
			{
				return;
			}
			string path = StringHelper.UTF8BytesToString(ref dataCfgInfoByCurLevelDiff.szCharacterInfo);
			CActorInfo actorInfo = CActorInfo.GetActorInfo(path, 0);
			if (actorInfo)
			{
				Transform transform = this.Region.transform;
				COM_PLAYERCAMP campType = this.Region.CampType;
				ActorMeta actorMeta = default(ActorMeta);
				ActorMeta actorMeta2 = actorMeta;
				actorMeta2.ConfigId = (int)SoldierID;
				actorMeta2.ActorType = ActorTypeDef.Actor_Type_Monster;
				actorMeta2.ActorCamp = campType;
				actorMeta = actorMeta2;
				VInt3 vInt = (VInt3)transform.position;
				VInt3 vInt2 = (VInt3)transform.forward;
				PoolObjHandle<ActorRoot> poolObjHandle = default(PoolObjHandle<ActorRoot>);
				if (!Singleton<GameObjMgr>.GetInstance().TryGetFromCache(ref poolObjHandle, ref actorMeta))
				{
					poolObjHandle = Singleton<GameObjMgr>.GetInstance().SpawnActorEx(null, ref actorMeta, vInt, vInt2, false, true);
					if (poolObjHandle)
					{
						poolObjHandle.get_handle().InitActor();
						poolObjHandle.get_handle().PrepareFight();
						Singleton<GameObjMgr>.get_instance().AddActor(poolObjHandle);
						poolObjHandle.get_handle().StartFight();
					}
				}
				else
				{
					ActorRoot handle = poolObjHandle.get_handle();
					handle.TheActorMeta.ActorCamp = actorMeta.ActorCamp;
					handle.ReactiveActor(vInt, vInt2);
				}
				if (poolObjHandle)
				{
					if (this.Region.AttackRoute != null)
					{
						poolObjHandle.get_handle().ActorControl.AttackAlongRoute(this.Region.AttackRoute.GetComponent<WaypointsHolder>());
					}
					else if (this.Region.finalTarget != null)
					{
						FrameCommand<AttackPositionCommand> frameCommand = FrameCommandFactory.CreateFrameCommand<AttackPositionCommand>();
						frameCommand.cmdId = 1u;
						frameCommand.cmdData.WorldPos = new VInt3(this.Region.finalTarget.transform.position);
						poolObjHandle.get_handle().ActorControl.CmdAttackMoveToDest(frameCommand, frameCommand.cmdData.WorldPos);
					}
					if (!this.isCannonNotified && this.WaveInfo.bType == 1)
					{
						KillNotify theKillNotify = Singleton<CBattleSystem>.GetInstance().TheKillNotify;
						Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
						if (theKillNotify != null && hostPlayer != null)
						{
							bool flag = hostPlayer.PlayerCamp == poolObjHandle.get_handle().TheActorMeta.ActorCamp;
							if (flag)
							{
								KillInfo killInfo = new KillInfo((hostPlayer.PlayerCamp != 1) ? KillNotify.red_cannon_icon : KillNotify.blue_cannon_icon, null, KillDetailInfoType.Info_Type_Cannon_Spawned, flag, false, ActorTypeDef.Invalid);
								theKillNotify.AddKillInfo(ref killInfo);
								this.isCannonNotified = true;
							}
						}
					}
				}
			}
			if (this.Region.bTriggerEvent)
			{
				SoldierWaveParam soldierWaveParam = new SoldierWaveParam(this.Index, this.repeatCount, this.Region.GetNextRepeatTime(false));
				Singleton<GameEventSys>.get_instance().SendEvent<SoldierWaveParam>(GameEventDef.Event_SoldierWaveNextRepeat, ref soldierWaveParam);
			}
		}
	}
}
