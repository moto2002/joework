using AGE;
using Apollo;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.DataCenter;
using Assets.Scripts.GameLogic.GameKernal;
using Assets.Scripts.GameSystem;
using CSProtocol;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
	public sealed class GameBuilder : Singleton<GameBuilder>
	{
		public int m_iMapId;

		public COM_GAME_TYPE m_kGameType = 12;

		private float m_fLoadingTime;

		private float m_fLoadProgress;

		private List<KeyValuePair<string, string>> m_eventsLoadingTime = new List<KeyValuePair<string, string>>();

		public GameInfoBase gameInfo
		{
			get;
			private set;
		}

		public GameInfoBase StartGame(GameContextBase InGameContext)
		{
			DebugHelper.Assert(InGameContext != null);
			if (InGameContext == null)
			{
				return null;
			}
			if (Singleton<BattleLogic>.get_instance().isRuning)
			{
				return null;
			}
			SynchrReport.Reset();
			GameSettings.DecideDynamicParticleLOD();
			Singleton<CHeroSelectBaseSystem>.get_instance().m_fOpenHeroSelectForm = Time.time - Singleton<CHeroSelectBaseSystem>.get_instance().m_fOpenHeroSelectForm;
			this.m_fLoadingTime = Time.time;
			this.m_eventsLoadingTime.Clear();
			ApolloAccountInfo accountInfo = Singleton<ApolloHelper>.GetInstance().GetAccountInfo(false);
			DebugHelper.Assert(accountInfo != null, "account info is null");
			this.m_iMapId = InGameContext.levelContext.m_mapID;
			this.m_kGameType = InGameContext.levelContext.GetGameType();
			this.m_eventsLoadingTime.Add(new KeyValuePair<string, string>("OpenID", (accountInfo == null) ? "0" : accountInfo.get_OpenId()));
			this.m_eventsLoadingTime.Add(new KeyValuePair<string, string>("LevelID", InGameContext.levelContext.m_mapID.ToString()));
			this.m_eventsLoadingTime.Add(new KeyValuePair<string, string>("isPVPLevel", InGameContext.levelContext.IsMobaMode().ToString()));
			this.m_eventsLoadingTime.Add(new KeyValuePair<string, string>("isPVPMode", InGameContext.levelContext.IsMobaMode().ToString()));
			this.m_eventsLoadingTime.Add(new KeyValuePair<string, string>("bLevelNo", InGameContext.levelContext.m_levelNo.ToString()));
			Singleton<BattleLogic>.GetInstance().isRuning = true;
			Singleton<BattleLogic>.GetInstance().isFighting = false;
			Singleton<BattleLogic>.GetInstance().isGameOver = false;
			Singleton<BattleLogic>.GetInstance().isWaitMultiStart = false;
			ActionManager.Instance.frameMode = true;
			MonoSingleton<ActionManager>.GetInstance().ForceStop();
			Singleton<GameObjMgr>.GetInstance().ClearActor();
			Singleton<SceneManagement>.GetInstance().Clear();
			MonoSingleton<SceneMgr>.GetInstance().ClearAll();
			MonoSingleton<GameLoader>.GetInstance().ResetLoader();
			InGameContext.PrepareStartup();
			if (!MonoSingleton<GameFramework>.get_instance().EditorPreviewMode)
			{
				DebugHelper.Assert(InGameContext.levelContext != null);
				DebugHelper.Assert(!string.IsNullOrEmpty(InGameContext.levelContext.m_levelDesignFileName));
				if (string.IsNullOrEmpty(InGameContext.levelContext.m_levelArtistFileName))
				{
					MonoSingleton<GameLoader>.get_instance().AddLevel(InGameContext.levelContext.m_levelDesignFileName);
				}
				else
				{
					MonoSingleton<GameLoader>.get_instance().AddDesignSerializedLevel(InGameContext.levelContext.m_levelDesignFileName);
					MonoSingleton<GameLoader>.get_instance().AddArtistSerializedLevel(InGameContext.levelContext.m_levelArtistFileName);
				}
				MonoSingleton<GameLoader>.get_instance().AddSoundBank("Effect_Common");
				MonoSingleton<GameLoader>.get_instance().AddSoundBank("System_Voice");
			}
			GameInfoBase gameInfoBase = InGameContext.CreateGameInfo();
			DebugHelper.Assert(gameInfoBase != null, "can't create game logic object!");
			this.gameInfo = gameInfoBase;
			gameInfoBase.PreBeginPlay();
			Singleton<BattleLogic>.get_instance().m_LevelContext = this.gameInfo.gameContext.levelContext;
			try
			{
				DebugHelper.CustomLog("GameBuilder Start Game: ispvplevel={0} ispvpmode={4} levelid={1} leveltype={6} levelname={3} Gametype={2} pick={5}", new object[]
				{
					InGameContext.levelContext.IsMobaMode(),
					InGameContext.levelContext.m_mapID,
					InGameContext.levelContext.GetGameType(),
					InGameContext.levelContext.m_levelName,
					InGameContext.levelContext.IsMobaMode(),
					InGameContext.levelContext.GetSelectHeroType(),
					InGameContext.levelContext.m_pveLevelType
				});
				Player hostPlayer = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer();
				if (hostPlayer != null)
				{
					DebugHelper.CustomLog("HostPlayer player id={1} name={2} ", new object[]
					{
						hostPlayer.PlayerId,
						hostPlayer.Name
					});
				}
			}
			catch (Exception)
			{
			}
			if (!MonoSingleton<GameFramework>.get_instance().EditorPreviewMode)
			{
				this.m_fLoadProgress = 0f;
				MonoSingleton<GameLoader>.GetInstance().Load(new GameLoader.LoadProgressDelegate(this.onGameLoadProgress), new GameLoader.LoadCompleteDelegate(this.OnGameLoadComplete));
				MonoSingleton<VoiceSys>.GetInstance().HeroSelectTobattle();
				Singleton<GameStateCtrl>.GetInstance().GotoState("LoadingState");
			}
			return gameInfoBase;
		}

		public void EndGame()
		{
			if (!Singleton<BattleLogic>.get_instance().isRuning)
			{
				return;
			}
			try
			{
				DebugHelper.CustomLog("Prepare GameBuilder EndGame");
			}
			catch (Exception)
			{
			}
			MonoSingleton<GSDKsys>.GetInstance().EndSpeed();
			Singleton<GameLogic>.GetInstance().HashCheckFreq = 500u;
			Singleton<GameLogic>.GetInstance().SnakeTraceMasks = 0u;
			Singleton<GameLogic>.GetInstance().SnakeTraceSize = 1024000u;
			Singleton<LobbyLogic>.GetInstance().StopGameEndTimer();
			Singleton<LobbyLogic>.GetInstance().StopSettleMsgTimer();
			Singleton<LobbyLogic>.GetInstance().StopSettlePanelTimer();
			MonoSingleton<GameLoader>.get_instance().AdvanceStopLoad();
			Singleton<WatchController>.GetInstance().Stop();
			Singleton<FrameWindow>.GetInstance().ResetSendCmdSeq();
			Singleton<CBattleGuideManager>.GetInstance().resetPause();
			MonoSingleton<ShareSys>.get_instance().SendQQGameTeamStateChgMsg(ShareSys.QQGameTeamEventType.end, 0, 0, 0u, string.Empty);
			Singleton<StarSystem>.GetInstance().EndGame();
			Singleton<WinLoseByStarSys>.GetInstance().EndGame();
			Singleton<CMatchingSystem>.GetInstance().EndGame();
			string openID = Singleton<ApolloHelper>.GetInstance().GetOpenID();
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			list.Add(new KeyValuePair<string, string>("g_version", CVersion.GetAppVersion()));
			list.Add(new KeyValuePair<string, string>("WorldID", MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID.ToString()));
			list.Add(new KeyValuePair<string, string>("platform", Singleton<ApolloHelper>.GetInstance().CurPlatform.ToString()));
			list.Add(new KeyValuePair<string, string>("openid", openID));
			list.Add(new KeyValuePair<string, string>("GameType", this.m_kGameType.ToString()));
			list.Add(new KeyValuePair<string, string>("MapID", this.m_iMapId.ToString()));
			list.Add(new KeyValuePair<string, string>("LoadingTime", this.m_fLoadingTime.ToString()));
			Singleton<ApolloHelper>.GetInstance().ApolloRepoertEvent("Service_LoadingBattle", list, true);
			List<KeyValuePair<string, string>> list2 = new List<KeyValuePair<string, string>>();
			list2.Add(new KeyValuePair<string, string>("g_version", CVersion.GetAppVersion()));
			list2.Add(new KeyValuePair<string, string>("WorldID", MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID.ToString()));
			list2.Add(new KeyValuePair<string, string>("platform", Singleton<ApolloHelper>.GetInstance().CurPlatform.ToString()));
			list2.Add(new KeyValuePair<string, string>("openid", openID));
			list2.Add(new KeyValuePair<string, string>("totaltime", Singleton<CHeroSelectBaseSystem>.get_instance().m_fOpenHeroSelectForm.ToString()));
			list2.Add(new KeyValuePair<string, string>("gameType", this.m_kGameType.ToString()));
			list2.Add(new KeyValuePair<string, string>("role_list", string.Empty));
			list2.Add(new KeyValuePair<string, string>("errorCode", string.Empty));
			list2.Add(new KeyValuePair<string, string>("error_msg", string.Empty));
			Singleton<ApolloHelper>.GetInstance().ApolloRepoertEvent("Service_Login_EnterGame", list2, true);
			float num = Singleton<FrameSynchr>.GetInstance().LogicFrameTick * 0.001f;
			Singleton<FrameSynchr>.GetInstance().PingVariance();
			List<KeyValuePair<string, string>> list3 = new List<KeyValuePair<string, string>>();
			list3.Add(new KeyValuePair<string, string>("g_version", CVersion.GetAppVersion()));
			list3.Add(new KeyValuePair<string, string>("WorldID", MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID.ToString()));
			list3.Add(new KeyValuePair<string, string>("platform", Singleton<ApolloHelper>.GetInstance().CurPlatform.ToString()));
			list3.Add(new KeyValuePair<string, string>("openid", openID));
			list3.Add(new KeyValuePair<string, string>("GameType", this.m_kGameType.ToString()));
			list3.Add(new KeyValuePair<string, string>("MapID", this.m_iMapId.ToString()));
			list3.Add(new KeyValuePair<string, string>("Max_FPS", Singleton<CBattleSystem>.GetInstance().m_MaxBattleFPS.ToString()));
			list3.Add(new KeyValuePair<string, string>("Min_FPS", Singleton<CBattleSystem>.GetInstance().m_MinBattleFPS.ToString()));
			float num2 = -1f;
			if (Singleton<CBattleSystem>.GetInstance().m_BattleFPSCount > 0f)
			{
				num2 = Singleton<CBattleSystem>.GetInstance().m_AveBattleFPS / Singleton<CBattleSystem>.GetInstance().m_BattleFPSCount;
			}
			list3.Add(new KeyValuePair<string, string>("Avg_FPS", num2.ToString()));
			list3.Add(new KeyValuePair<string, string>("Ab_FPS_time", Singleton<BattleLogic>.GetInstance().m_Ab_FPS_time.ToString()));
			list3.Add(new KeyValuePair<string, string>("Abnormal_FPS", Singleton<BattleLogic>.GetInstance().m_Abnormal_FPS_Count.ToString()));
			list3.Add(new KeyValuePair<string, string>("Less10FPSCount", Singleton<BattleLogic>.GetInstance().m_fpsCunt10.ToString()));
			list3.Add(new KeyValuePair<string, string>("Less18FPSCount", Singleton<BattleLogic>.GetInstance().m_fpsCunt18.ToString()));
			list3.Add(new KeyValuePair<string, string>("Ab_4FPS_time", Singleton<BattleLogic>.GetInstance().m_Ab_4FPS_time.ToString()));
			list3.Add(new KeyValuePair<string, string>("Abnormal_4FPS", Singleton<BattleLogic>.GetInstance().m_Abnormal_4FPS_Count.ToString()));
			list3.Add(new KeyValuePair<string, string>("Min_Ping", Singleton<FrameSynchr>.get_instance().m_MinPing.ToString()));
			list3.Add(new KeyValuePair<string, string>("Max_Ping", Singleton<FrameSynchr>.get_instance().m_MaxPing.ToString()));
			list3.Add(new KeyValuePair<string, string>("Avg_Ping", Singleton<FrameSynchr>.get_instance().m_AvePing.ToString()));
			list3.Add(new KeyValuePair<string, string>("Abnormal_Ping", Singleton<FrameSynchr>.get_instance().m_Abnormal_PingCount.ToString()));
			list3.Add(new KeyValuePair<string, string>("Ping300", Singleton<FrameSynchr>.get_instance().m_ping300Count.ToString()));
			list3.Add(new KeyValuePair<string, string>("Ping150to300", Singleton<FrameSynchr>.get_instance().m_ping150to300.ToString()));
			list3.Add(new KeyValuePair<string, string>("Ping150", Singleton<FrameSynchr>.get_instance().m_ping150.ToString()));
			list3.Add(new KeyValuePair<string, string>("LostpingCount", Singleton<FrameSynchr>.get_instance().m_pingLost.ToString()));
			list3.Add(new KeyValuePair<string, string>("PingSeqCount", Singleton<FrameSynchr>.get_instance().m_LastReceiveHeartSeq.ToString()));
			list3.Add(new KeyValuePair<string, string>("PingVariance", Singleton<FrameSynchr>.get_instance().m_PingVariance.ToString()));
			list3.Add(new KeyValuePair<string, string>("Battle_Time", num.ToString()));
			list3.Add(new KeyValuePair<string, string>("BattleSvr_Reconnect", Singleton<NetworkModule>.GetInstance().m_GameReconnetCount.ToString()));
			list3.Add(new KeyValuePair<string, string>("GameSvr_Reconnect", Singleton<NetworkModule>.GetInstance().m_lobbyReconnetCount.ToString()));
			list3.Add(new KeyValuePair<string, string>("music", GameSettings.EnableMusic.ToString()));
			list3.Add(new KeyValuePair<string, string>("quality", GameSettings.RenderQuality.ToString()));
			list3.Add(new KeyValuePair<string, string>("status", "1"));
			list3.Add(new KeyValuePair<string, string>("Quality_Mode", GameSettings.ModelLOD.ToString()));
			list3.Add(new KeyValuePair<string, string>("Quality_Particle", GameSettings.ParticleLOD.ToString()));
			list3.Add(new KeyValuePair<string, string>("receiveMoveCmdAverage", Singleton<FrameSynchr>.get_instance().m_receiveMoveCmdAverage.ToString()));
			list3.Add(new KeyValuePair<string, string>("receiveMoveCmdMax", Singleton<FrameSynchr>.get_instance().m_receiveMoveCmdMax.ToString()));
			list3.Add(new KeyValuePair<string, string>("execMoveCmdAverage", Singleton<FrameSynchr>.get_instance().m_execMoveCmdAverage.ToString()));
			list3.Add(new KeyValuePair<string, string>("execMoveCmdMax", Singleton<FrameSynchr>.get_instance().m_execMoveCmdMax.ToString()));
			list3.Add(new KeyValuePair<string, string>("LOD_Down", Singleton<BattleLogic>.GetInstance().m_iAutoLODState.ToString()));
			if (NetworkAccelerator.started)
			{
				if (NetworkAccelerator.isAccerating())
				{
					list3.Add(new KeyValuePair<string, string>("AccState", "Acc"));
				}
				else
				{
					list3.Add(new KeyValuePair<string, string>("AccState", "Direct"));
				}
			}
			else
			{
				list3.Add(new KeyValuePair<string, string>("AccState", "Off"));
			}
			int num3 = 0;
			if (MonoSingleton<VoiceSys>.GetInstance().UseSpeak && MonoSingleton<VoiceSys>.GetInstance().UseMic)
			{
				num3 = 2;
			}
			else if (MonoSingleton<VoiceSys>.GetInstance().UseSpeak)
			{
				num3 = 1;
			}
			list3.Add(new KeyValuePair<string, string>("Mic", num3.ToString()));
			Singleton<ApolloHelper>.GetInstance().ApolloRepoertEvent("Service_PVPBattle_Summary", list3, true);
			this.m_eventsLoadingTime.Clear();
			try
			{
				float num4 = (float)Singleton<BattleLogic>.GetInstance().m_fpsCunt10 / (float)Singleton<BattleLogic>.GetInstance().m_fpsCount;
				int iFps10PercentNum = Mathf.CeilToInt(num4 * 100f / 10f) * 10;
				float num5 = (float)(Singleton<BattleLogic>.GetInstance().m_fpsCunt18 + Singleton<BattleLogic>.GetInstance().m_fpsCunt10) / (float)Singleton<BattleLogic>.GetInstance().m_fpsCount;
				int iFps18PercentNum = Mathf.CeilToInt(num5 * 100f / 10f) * 10;
				CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5000u);
				cSPkg.stPkgData.get_stCltPerformance().iMapID = this.m_iMapId;
				cSPkg.stPkgData.get_stCltPerformance().iPlayerCnt = Singleton<GamePlayerCenter>.get_instance().GetAllPlayers().get_Count();
				cSPkg.stPkgData.get_stCltPerformance().chModelLOD = (sbyte)GameSettings.ModelLOD;
				cSPkg.stPkgData.get_stCltPerformance().chParticleLOD = (sbyte)GameSettings.ParticleLOD;
				cSPkg.stPkgData.get_stCltPerformance().chCameraHeight = (sbyte)GameSettings.CameraHeight;
				cSPkg.stPkgData.get_stCltPerformance().chEnableOutline = ((!GameSettings.EnableOutline) ? 0 : 1);
				cSPkg.stPkgData.get_stCltPerformance().iFps10PercentNum = iFps10PercentNum;
				cSPkg.stPkgData.get_stCltPerformance().iFps18PercentNum = iFps18PercentNum;
				cSPkg.stPkgData.get_stCltPerformance().iAveFps = (int)Singleton<CBattleSystem>.GetInstance().m_AveBattleFPS;
				cSPkg.stPkgData.get_stCltPerformance().iPingAverage = Singleton<FrameSynchr>.get_instance().m_PingAverage;
				cSPkg.stPkgData.get_stCltPerformance().iPingVariance = Singleton<FrameSynchr>.get_instance().m_PingVariance;
				Utility.StringToByteArray(SystemInfo.deviceModel, ref cSPkg.stPkgData.get_stCltPerformance().szDeviceModel);
				Utility.StringToByteArray(SystemInfo.graphicsDeviceName, ref cSPkg.stPkgData.get_stCltPerformance().szGPUName);
				cSPkg.stPkgData.get_stCltPerformance().iCpuCoreNum = SystemInfo.processorCount;
				cSPkg.stPkgData.get_stCltPerformance().iSysMemorySize = SystemInfo.systemMemorySize;
				cSPkg.stPkgData.get_stCltPerformance().iAvailMemory = DeviceCheckSys.GetAvailMemory();
				cSPkg.stPkgData.get_stCltPerformance().iIsTongCai = ((!MonoSingleton<CTongCaiSys>.GetInstance().IsCanUseTongCai()) ? 0 : 1);
				int iIsSpeedUp;
				if (NetworkAccelerator.started)
				{
					if (NetworkAccelerator.isAccerating())
					{
						iIsSpeedUp = 1;
					}
					else
					{
						iIsSpeedUp = 2;
					}
				}
				else
				{
					iIsSpeedUp = 0;
				}
				if (MonoSingleton<GSDKsys>.GetInstance().enabled)
				{
					iIsSpeedUp = 4;
				}
				cSPkg.stPkgData.get_stCltPerformance().iIsSpeedUp = iIsSpeedUp;
				Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
			}
			catch (Exception ex)
			{
				Debug.Log(ex.get_Message());
			}
			MonoSingleton<DialogueProcessor>.GetInstance().Uninit();
			Singleton<TipProcessor>.GetInstance().Uninit();
			Singleton<LobbyLogic>.get_instance().inMultiRoom = false;
			Singleton<LobbyLogic>.get_instance().inMultiGame = false;
			Singleton<LobbyLogic>.GetInstance().reconnGameInfo = null;
			Singleton<BattleLogic>.GetInstance().isRuning = false;
			Singleton<BattleLogic>.GetInstance().isFighting = false;
			Singleton<BattleLogic>.GetInstance().isGameOver = false;
			Singleton<BattleLogic>.GetInstance().isWaitMultiStart = false;
			Singleton<NetworkModule>.GetInstance().CloseGameServerConnect(true);
			Singleton<ShenFuSystem>.get_instance().ClearAll();
			MonoSingleton<ActionManager>.GetInstance().ForceStop();
			Singleton<GameObjMgr>.GetInstance().ClearActor();
			Singleton<SceneManagement>.GetInstance().Clear();
			MonoSingleton<SceneMgr>.GetInstance().ClearAll();
			Singleton<GamePlayerCenter>.GetInstance().ClearAllPlayers();
			Singleton<ActorDataCenter>.get_instance().ClearHeroServerData();
			Singleton<FrameSynchr>.GetInstance().ResetSynchr();
			Singleton<GameReplayModule>.GetInstance().OnGameEnd();
			Singleton<BattleLogic>.GetInstance().ResetBattleSystem();
			ActionManager.Instance.frameMode = false;
			MonoSingleton<VoiceInteractionSys>.get_instance().OnEndGame();
			if (!Singleton<GameStateCtrl>.get_instance().isLobbyState)
			{
				DebugHelper.CustomLog("GotoLobbyState by EndGame");
				Singleton<GameStateCtrl>.GetInstance().GotoState("LobbyState");
			}
			Singleton<BattleSkillHudControl>.DestroyInstance();
			this.m_kGameType = 12;
			this.m_iMapId = 0;
			try
			{
				FogOfWar.EndLevel();
			}
			catch (DllNotFoundException ex2)
			{
				DebugHelper.Assert(false, "FOW Exception {0} {1}", new object[]
				{
					ex2.get_Message(),
					ex2.get_StackTrace()
				});
			}
			Singleton<BattleStatistic>.get_instance().PostEndGame();
			try
			{
				DebugHelper.CustomLog("Finish GameBuilder EndGame");
			}
			catch (Exception)
			{
			}
		}

		private void onGameLoadProgress(float progress)
		{
			if (this.gameInfo != null && progress >= this.m_fLoadProgress + 0.01f)
			{
				this.m_fLoadProgress = progress;
				this.gameInfo.OnLoadingProgress(progress);
			}
		}

		private void OnGameLoadComplete()
		{
			if (!Singleton<BattleLogic>.get_instance().isRuning)
			{
				DebugHelper.Assert(false, "都没有在游戏局内，何来的游戏加载完成");
				return;
			}
			if (Singleton<WatchController>.get_instance().workMode != WatchController.WorkMode.None)
			{
				DebugHelper.CustomLog("观战模式:{0}", new object[]
				{
					Singleton<WatchController>.get_instance().workMode.ToString()
				});
			}
			try
			{
				this.gameInfo.PostBeginPlay();
			}
			catch (Exception ex)
			{
				DebugHelper.Assert(false, "Exception In PostBeginPlay {0} {1}", new object[]
				{
					ex.get_Message(),
					ex.get_StackTrace()
				});
				throw ex;
			}
			this.m_fLoadingTime = Time.time - this.m_fLoadingTime;
			if (MonoSingleton<Reconnection>.GetInstance().g_fBeginReconnectTime > 0f)
			{
				List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
				list.Add(new KeyValuePair<string, string>("ReconnectTime", (Time.time - MonoSingleton<Reconnection>.GetInstance().g_fBeginReconnectTime).ToString()));
				MonoSingleton<Reconnection>.GetInstance().g_fBeginReconnectTime = -1f;
				Singleton<ApolloHelper>.GetInstance().ApolloRepoertEvent("Service_Reconnet_IntoGame", list, true);
			}
		}

		public void StoreGame()
		{
		}

		public void RestoreGame()
		{
		}
	}
}
