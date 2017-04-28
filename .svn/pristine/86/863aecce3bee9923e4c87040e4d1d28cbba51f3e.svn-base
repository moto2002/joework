using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using CSProtocol;
using System;

namespace Assets.Scripts.GameLogic
{
	[FrameCommandClass]
	public struct SwitchActorSwitchGodMode : ICommandImplement
	{
		public sbyte IsGodMode;

		[FrameCommandCreator]
		public static IFrameCommand Creator(ref FRAME_CMD_PKG msg)
		{
			FrameCommand<SwitchActorSwitchGodMode> frameCommand = FrameCommandFactory.CreateFrameCommand<SwitchActorSwitchGodMode>();
			frameCommand.cmdData.IsGodMode = msg.stCmdInfo.get_stCmdPlayerSwitchGodMode().chIsGodMode;
			return frameCommand;
		}

		public bool TransProtocol(FRAME_CMD_PKG msg)
		{
			msg.stCmdInfo.get_stCmdPlayerSwitchGodMode().chIsGodMode = this.IsGodMode;
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
			if (player != null && ((flag && player.isGM) || (!flag && LobbyMsgHandler.isHostGMAcnt)) && player.Captain && player.Captain.get_handle().ActorControl is HeroWrapper)
			{
				HeroWrapper heroWrapper = (HeroWrapper)player.Captain.get_handle().ActorControl;
				heroWrapper.bGodMode = ((int)this.IsGodMode != 0);
			}
		}

		public void AwakeCommand(IFrameCommand cmd)
		{
		}
	}
}
