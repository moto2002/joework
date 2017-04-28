using Apollo;
using Assets.Scripts.Framework;
using Assets.Scripts.GameSystem;
using CSProtocol;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameLogic.GameKernal
{
	public class GameReportor
	{
		private readonly List<KeyValuePair<string, string>> _eventsLoadingTime = new List<KeyValuePair<string, string>>();

		public void PrepareReport()
		{
			this._eventsLoadingTime.Clear();
			ApolloAccountInfo accountInfo = Singleton<ApolloHelper>.GetInstance().GetAccountInfo(false);
			DebugHelper.Assert(accountInfo != null, "account info is null");
			this._eventsLoadingTime.Add(new KeyValuePair<string, string>("OpenID", (accountInfo == null) ? "0" : accountInfo.get_OpenId()));
			this._eventsLoadingTime.Add(new KeyValuePair<string, string>("LevelID", Singleton<GameContextEx>.GetInstance().GameContextCommonInfo.MapId.ToString()));
			this._eventsLoadingTime.Add(new KeyValuePair<string, string>("isPVPLevel", Singleton<GameContextEx>.GetInstance().IsMobaMode().ToString()));
			this._eventsLoadingTime.Add(new KeyValuePair<string, string>("isPVPMode", Singleton<GameContextEx>.GetInstance().IsMobaMode().ToString()));
			this._eventsLoadingTime.Add(new KeyValuePair<string, string>("bLevelNo", Singleton<GameContextEx>.GetInstance().GameContextSoloInfo.LevelNo.ToString()));
		}

		public void DoApolloReport()
		{
			string openID = Singleton<ApolloHelper>.GetInstance().GetOpenID();
			int mapId = Singleton<GameContextEx>.GetInstance().GameContextCommonInfo.MapId;
			COM_GAME_TYPE gameType = Singleton<GameContextEx>.GetInstance().GameContextCommonInfo.GameType;
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			list.Add(new KeyValuePair<string, string>("g_version", CVersion.GetAppVersion()));
			list.Add(new KeyValuePair<string, string>("WorldID", MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID.ToString()));
			list.Add(new KeyValuePair<string, string>("platform", Singleton<ApolloHelper>.GetInstance().CurPlatform.ToString()));
			list.Add(new KeyValuePair<string, string>("openid", openID));
			list.Add(new KeyValuePair<string, string>("GameType", gameType.ToString()));
			list.Add(new KeyValuePair<string, string>("MapID", mapId.ToString()));
			list.Add(new KeyValuePair<string, string>("LoadingTime", Singleton<GameBuilderEx>.GetInstance().LastLoadingTime.ToString()));
			Singleton<ApolloHelper>.GetInstance().ApolloRepoertEvent("Service_LoadingBattle", list, true);
			List<KeyValuePair<string, string>> list2 = new List<KeyValuePair<string, string>>();
			list2.Add(new KeyValuePair<string, string>("g_version", CVersion.GetAppVersion()));
			list2.Add(new KeyValuePair<string, string>("WorldID", MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID.ToString()));
			list2.Add(new KeyValuePair<string, string>("platform", Singleton<ApolloHelper>.GetInstance().CurPlatform.ToString()));
			list2.Add(new KeyValuePair<string, string>("openid", openID));
			list2.Add(new KeyValuePair<string, string>("totaltime", Singleton<CHeroSelectBaseSystem>.get_instance().m_fOpenHeroSelectForm.ToString()));
			list2.Add(new KeyValuePair<string, string>("gameType", gameType.ToString()));
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
			list3.Add(new KeyValuePair<string, string>("GameType", gameType.ToString()));
			list3.Add(new KeyValuePair<string, string>("MapID", mapId.ToString()));
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
			this._eventsLoadingTime.Clear();
			try
			{
				float num4 = (float)Singleton<BattleLogic>.GetInstance().m_fpsCunt10 / (float)Singleton<BattleLogic>.GetInstance().m_fpsCount;
				int iFps10PercentNum = Mathf.CeilToInt(num4 * 100f / 10f) * 10;
				float num5 = (float)(Singleton<BattleLogic>.GetInstance().m_fpsCunt18 + Singleton<BattleLogic>.GetInstance().m_fpsCunt10) / (float)Singleton<BattleLogic>.GetInstance().m_fpsCount;
				int iFps18PercentNum = Mathf.CeilToInt(num5 * 100f / 10f) * 10;
				CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5000u);
				cSPkg.stPkgData.get_stCltPerformance().iMapID = mapId;
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
				Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
			}
			catch (Exception ex)
			{
				Debug.Log(ex.get_Message());
			}
		}
	}
}
