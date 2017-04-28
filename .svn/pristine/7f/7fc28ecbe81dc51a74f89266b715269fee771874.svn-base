using Assets.Scripts.Framework;
using Assets.Scripts.GameSystem;
using CSProtocol;
using ResData;
using System;
using UnityEngine;

[CheatCommand("英雄/历史战绩/增加历史战绩记录：", "增加一条历史战绩记录", 76)]
internal class SetPvpHisotryCommand : CheatCommandNetworking
{
	protected override string Execute(string[] InArguments, ref CSDT_CHEATCMD_DETAIL CheatCmdRef)
	{
		CheatCmdRef.set_stAddFightHistory(new CSDT_CHEAT_ADD_FIGHTHISTORY());
		CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
		CheatCmdRef.get_stAddFightHistory().stFightRecord.bGameType = 6;
		CheatCmdRef.get_stAddFightHistory().stFightRecord.bPlayerCnt = 4;
		CheatCmdRef.get_stAddFightHistory().stFightRecord.bWinCamp = 1;
		CheatCmdRef.get_stAddFightHistory().stFightRecord.dwGameStartTime = (uint)CRoleInfo.GetCurrentUTCTime();
		CheatCmdRef.get_stAddFightHistory().stFightRecord.dwGameTime = 1000u;
		for (int i = 0; i < 4; i++)
		{
			CheatCmdRef.get_stAddFightHistory().stFightRecord.astPlayerFightData[i].bHeroLv = 1;
			CheatCmdRef.get_stAddFightHistory().stFightRecord.astPlayerFightData[i].bPlayerLv = 1;
			CheatCmdRef.get_stAddFightHistory().stFightRecord.astPlayerFightData[i].bPlayerCamp = ((i >= 2) ? 2 : 1);
			int id = Random.Range(0, GameDataMgr.robotRookieHeroSkinDatabin.Count());
			ResFakeAcntHero dataByIndex = GameDataMgr.robotRookieHeroSkinDatabin.GetDataByIndex(id);
			CheatCmdRef.get_stAddFightHistory().stFightRecord.astPlayerFightData[i].dwHeroID = dataByIndex.dwHeroID;
			StringHelper.StringToUTF8Bytes(i.ToString(), ref CheatCmdRef.get_stAddFightHistory().stFightRecord.astPlayerFightData[i].szPlayerName);
			if (i == 0)
			{
				CheatCmdRef.get_stAddFightHistory().stFightRecord.astPlayerFightData[i].ullPlayerUid = masterRoleInfo.playerUllUID;
				CheatCmdRef.get_stAddFightHistory().stFightRecord.astPlayerFightData[i].iPlayerLogicWorldID = MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID;
			}
			else
			{
				CheatCmdRef.get_stAddFightHistory().stFightRecord.astPlayerFightData[i].ullPlayerUid = (ulong)((long)i);
				CheatCmdRef.get_stAddFightHistory().stFightRecord.astPlayerFightData[i].iPlayerLogicWorldID = i;
			}
		}
		Singleton<CPlayerPvpHistoryController>.GetInstance().AddSelfRecordData(CheatCmdRef.get_stAddFightHistory().stFightRecord);
		return CheatCommandBase.Done;
	}
}
