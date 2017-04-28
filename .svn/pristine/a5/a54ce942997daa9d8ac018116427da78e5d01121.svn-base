using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using CSProtocol;
using System;

namespace Assets.Scripts.GameLogic
{
	[FrameCommandClass]
	public struct AttackPositionCommand : ICommandImplement
	{
		public VInt3 WorldPos;

		[FrameCommandCreator]
		public static IFrameCommand Creator(ref FRAME_CMD_PKG msg)
		{
			FrameCommand<AttackPositionCommand> frameCommand = FrameCommandFactory.CreateFrameCommand<AttackPositionCommand>();
			frameCommand.cmdData.WorldPos = CommonTools.ToVector3(msg.stCmdInfo.get_stCmdPlayerAttackPosition().stWorldPos);
			return frameCommand;
		}

		public bool TransProtocol(FRAME_CMD_PKG msg)
		{
			CommonTools.FromVector3(this.WorldPos, ref msg.stCmdInfo.get_stCmdPlayerAttackPosition().stWorldPos);
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
			if (player != null && player.Captain)
			{
				player.Captain.get_handle().ActorControl.CmdAttackMoveToDest(cmd, this.WorldPos);
			}
		}

		public void AwakeCommand(IFrameCommand cmd)
		{
		}
	}
}
