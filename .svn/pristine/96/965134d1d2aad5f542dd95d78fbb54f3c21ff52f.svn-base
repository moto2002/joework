using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using CSProtocol;
using System;

namespace Assets.Scripts.GameLogic
{
	[FrameCommandClass]
	public struct PlayCommonAttackModeCommand : ICommandImplement
	{
		public byte CommonAttackMode;

		[FrameCommandCreator]
		public static IFrameCommand Creator(ref FRAME_CMD_PKG msg)
		{
			FrameCommand<PlayCommonAttackModeCommand> frameCommand = FrameCommandFactory.CreateFrameCommand<PlayCommonAttackModeCommand>();
			frameCommand.cmdData.CommonAttackMode = msg.stCmdInfo.get_stCmdPlayCommonAttackMode().bCommonAttackMode;
			return frameCommand;
		}

		public bool TransProtocol(FRAME_CMD_PKG msg)
		{
			msg.stCmdInfo.get_stCmdPlayCommonAttackMode().bCommonAttackMode = this.CommonAttackMode;
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
			Player player = Singleton<GamePlayerCenter>.GetInstance().GetPlayer(cmd.playerID);
			if (player != null)
			{
				player.SetOperateMode((OperateMode)this.CommonAttackMode);
			}
		}

		public void AwakeCommand(IFrameCommand cmd)
		{
		}
	}
}
