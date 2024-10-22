using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using CSProtocol;
using System;

namespace Assets.Scripts.GameLogic
{
	[FrameCommandClass]
	public struct AttackActorCommand : ICommandImplement
	{
		public uint ObjectID;

		[FrameCommandCreator]
		public static IFrameCommand Creator(ref FRAME_CMD_PKG msg)
		{
			FrameCommand<AttackActorCommand> frameCommand = FrameCommandFactory.CreateFrameCommand<AttackActorCommand>();
			frameCommand.cmdData.ObjectID = (uint)msg.stCmdInfo.get_stCmdPlayerAttackPlayer().iObjectID;
			return frameCommand;
		}

		public bool TransProtocol(FRAME_CMD_PKG msg)
		{
			msg.stCmdInfo.get_stCmdPlayerAttackPlayer().iObjectID = (int)this.ObjectID;
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
				player.Captain.get_handle().ActorControl.AttackSelectActor(this.ObjectID);
			}
		}

		public void AwakeCommand(IFrameCommand cmd)
		{
		}
	}
}
