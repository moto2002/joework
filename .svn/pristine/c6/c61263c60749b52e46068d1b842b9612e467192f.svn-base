using Assets.Scripts.Framework;
using Assets.Scripts.GameSystem;
using CSProtocol;
using System;

[CheatCommandEntry("工具"), MessageHandlerClass]
internal class CheatCommandCommonEntryCommon
{
	[CheatCommandEntryMethod("一键毕业", true, false)]
	public static string FinishSchool()
	{
		Singleton<CheatCommandsRepository>.get_instance().ExecuteCommand("FinishLevel", new string[]
		{
			string.Empty
		});
		Singleton<CheatCommandsRepository>.get_instance().ExecuteCommand("UnlockAllLevel", new string[]
		{
			string.Empty
		});
		Singleton<CheatCommandsRepository>.get_instance().ExecuteCommand("ClearStoreLimit", new string[]
		{
			string.Empty
		});
		Singleton<CheatCommandsRepository>.get_instance().ExecuteCommand("AddHero", new string[]
		{
			"0"
		});
		Singleton<CheatCommandsRepository>.get_instance().ExecuteCommand("UnlockPvPHero", new string[]
		{
			"0"
		});
		Singleton<CheatCommandsRepository>.get_instance().ExecuteCommand("SetNewbieGuideState", new string[]
		{
			string.Empty
		});
		return CheatCommandBase.Done;
	}

	[CheatCommandEntryMethod("服务器/获取服务器时间", true, false)]
	public static string GetServerTime()
	{
		CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(4001u);
		Singleton<NetworkModule>.get_instance().SendLobbyMsg(ref cSPkg, false);
		return "Wait server rsp.";
	}

	[MessageHandler(4002)]
	public static void OnServerTimeRSP(CSPkg msg)
	{
		CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
		if (masterRoleInfo != null)
		{
			CRoleInfo.SetServerTime((int)msg.stPkgData.get_stServerTimeRsp().dwUTCSec);
			masterRoleInfo.SetGlobalRefreshTimer(msg.stPkgData.get_stServerTimeRsp().dwUTCSec, true);
		}
		MonoSingleton<ConsoleWindow>.get_instance().AddMessage(string.Format("{0}.{1}.{2} {3}:{4}:{5}", new object[]
		{
			msg.stPkgData.get_stServerTimeRsp().iYear,
			msg.stPkgData.get_stServerTimeRsp().iMonth,
			msg.stPkgData.get_stServerTimeRsp().iDay,
			msg.stPkgData.get_stServerTimeRsp().iHour,
			msg.stPkgData.get_stServerTimeRsp().iMin,
			msg.stPkgData.get_stServerTimeRsp().iSec
		}));
		Singleton<EventRouter>.get_instance().BroadCastEvent(EventID.EDITOR_REFRESH_GM_PANEL);
	}
}
