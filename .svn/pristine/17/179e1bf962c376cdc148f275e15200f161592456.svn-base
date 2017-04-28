using Assets.Scripts.Framework;
using Assets.Scripts.GameSystem;
using CSProtocol;
using System;

namespace Assets.Scripts.GameLogic
{
	[FrameCommandClass]
	public struct SignalMiniMapTarget : ICommandImplement
	{
		public byte m_type;

		public byte m_signalID;

		public uint m_targetObjID;

		[FrameCommandCreator]
		public static IFrameCommand Creator(ref FRAME_CMD_PKG msg)
		{
			FrameCommand<SignalMiniMapTarget> frameCommand = FrameCommandFactory.CreateFrameCommand<SignalMiniMapTarget>();
			frameCommand.cmdData.m_type = msg.stCmdInfo.get_stCmdSignalMiniMapTarget().bType;
			frameCommand.cmdData.m_signalID = msg.stCmdInfo.get_stCmdSignalMiniMapTarget().bSignalID;
			frameCommand.cmdData.m_targetObjID = msg.stCmdInfo.get_stCmdSignalMiniMapTarget().dwTargetObjID;
			return frameCommand;
		}

		public bool TransProtocol(FRAME_CMD_PKG msg)
		{
			msg.stCmdInfo.get_stCmdSignalMiniMapTarget().bType = this.m_type;
			msg.stCmdInfo.get_stCmdSignalMiniMapTarget().bSignalID = this.m_signalID;
			msg.stCmdInfo.get_stCmdSignalMiniMapTarget().dwTargetObjID = this.m_targetObjID;
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
			SignalPanel signalPanel = (Singleton<CBattleSystem>.GetInstance().FightForm == null) ? null : Singleton<CBattleSystem>.GetInstance().FightForm.GetSignalPanel();
			if (signalPanel != null)
			{
				signalPanel.ExecCommand_SignalMiniMap_Target(cmd.playerID, this.m_signalID, this.m_type, this.m_targetObjID);
			}
		}

		public void AwakeCommand(IFrameCommand cmd)
		{
		}
	}
}
