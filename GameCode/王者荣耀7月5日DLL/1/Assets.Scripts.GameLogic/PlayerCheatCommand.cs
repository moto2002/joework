using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using CSProtocol;
using System;

namespace Assets.Scripts.GameLogic
{
	[FrameCommandClass]
	public struct PlayerCheatCommand : ICommandImplement
	{
		public byte CheatType;

		[FrameCommandCreator]
		public static IFrameCommand Creator(ref FRAME_CMD_PKG msg)
		{
			FrameCommand<PlayerCheatCommand> frameCommand = FrameCommandFactory.CreateFrameCommand<PlayerCheatCommand>();
			frameCommand.cmdData.CheatType = msg.stCmdInfo.get_stCmdPlayerCheat().bCheatType;
			return frameCommand;
		}

		public bool TransProtocol(FRAME_CMD_PKG msg)
		{
			msg.stCmdInfo.get_stCmdPlayerCheat().bCheatType = this.CheatType;
			return true;
		}

		public bool TransProtocol(CSDT_GAMING_CSSYNCINFO msg)
		{
			return true;
		}

		public void OnReceive(IFrameCommand cmd)
		{
		}

		public void Preprocess(IFrameCommand cmd)
		{
		}

		public void ExecCommand(IFrameCommand cmd)
		{
			SLevelContext curLvelContext = Singleton<BattleLogic>.get_instance().GetCurLvelContext();
			bool flag = curLvelContext != null && curLvelContext.IsMobaMode();
			Player player = Singleton<GamePlayerCenter>.GetInstance().GetPlayer(cmd.playerID);
			if (player != null && ((flag && player.isGM) || (!flag && LobbyMsgHandler.isHostGMAcnt)))
			{
				CheatCommandBattleEntry.ProcessCheat(this.CheatType, ref player.Captain);
			}
		}

		public void AwakeCommand(IFrameCommand cmd)
		{
		}
	}
}
