using Assets.Scripts.GameLogic.GameKernal;
using Assets.Scripts.GameSystem;
using Assets.Scripts.UI;
using CSProtocol;
using System;

namespace Assets.Scripts.GameLogic
{
	[MessageHandlerClass]
	public class BattleMsgHandler
	{
		[MessageHandler(1044)]
		public static void onIsAccept_Aiplayer(CSPkg msg)
		{
			stUIEventParams par = default(stUIEventParams);
			par.commonUInt32Param1 = msg.stPkgData.get_stIsAcceptAiPlayerReq().dwAiPlayerObjID;
			Singleton<CUIManager>.GetInstance().OpenSmallMessageBox(Singleton<CTextManager>.GetInstance().GetText("Trusteeship_Ask"), true, enUIEventID.Trusteeship_Accept, enUIEventID.Trusteeship_Cancel, par, 30, enUIEventID.Trusteeship_Cancel, Singleton<CTextManager>.GetInstance().GetText("FollowMe"), Singleton<CTextManager>.GetInstance().GetText("StayHome"), false);
		}

		[MessageHandler(1046)]
		public static void onHangup(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("Hangup_Warnning"), false);
		}

		[MessageHandler(1099)]
		public static void OnBroadHangup(CSPkg msg)
		{
			Singleton<EventRouter>.get_instance().BroadCastEvent<HANGUP_TYPE, uint>(EventID.HangupNtf, msg.stPkgData.get_stHangUpNtf().bType, msg.stPkgData.get_stHangUpNtf().dwObjID);
		}

		[MessageHandler(1085)]
		public static void OnGameOverEvent(CSPkg msg)
		{
			if (Singleton<BattleLogic>.get_instance().isWaitGameEnd)
			{
				return;
			}
			if (msg.stPkgData.get_stNtfCltGameOver().bWinCamp > 0)
			{
				COM_PLAYERCAMP playerCamp = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().PlayerCamp;
				Singleton<BattleLogic>.get_instance().battleStat.iBattleResult = ((playerCamp != msg.stPkgData.get_stNtfCltGameOver().bWinCamp) ? 2 : 1);
				COM_PLAYERCAMP camp;
				if (Singleton<BattleLogic>.get_instance().battleStat.iBattleResult == 1)
				{
					camp = BattleLogic.GetOppositeCmp(playerCamp);
				}
				else
				{
					camp = playerCamp;
				}
				BattleLogic.ForceKillCrystal(camp);
			}
			Singleton<CSurrenderSystem>.get_instance().DelayCloseSurrenderForm(5);
		}
	}
}
