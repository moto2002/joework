using Assets.Scripts.Framework;
using CSProtocol;
using System;

namespace Assets.Scripts.GameLogic
{
	[FrameCommandClass]
	public struct SvrNtfChgFrameLaterCommand : ICommandImplement
	{
		public byte LaterNum;

		[FrameCommandCreator]
		public static IFrameCommand Creator(ref FRAME_CMD_PKG msg)
		{
			FrameCommand<SvrNtfChgFrameLaterCommand> frameCommand = FrameCommandFactory.CreateFrameCommand<SvrNtfChgFrameLaterCommand>();
			frameCommand.cmdData.LaterNum = msg.stCmdInfo.get_stCmdSvrNtfChgFrameLater().bKFrameLaterNum;
			return frameCommand;
		}

		public bool TransProtocol(FRAME_CMD_PKG msg)
		{
			msg.stCmdInfo.get_stCmdSvrNtfChgFrameLater().bKFrameLaterNum = this.LaterNum;
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
		}

		public void AwakeCommand(IFrameCommand cmd)
		{
		}
	}
}
