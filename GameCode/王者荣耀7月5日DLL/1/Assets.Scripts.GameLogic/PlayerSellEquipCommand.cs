using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using Assets.Scripts.GameSystem;
using CSProtocol;
using System;

namespace Assets.Scripts.GameLogic
{
	[FrameCommandClass]
	public struct PlayerSellEquipCommand : ICommandImplement
	{
		public int m_equipIndex;

		[FrameCommandCreator]
		public static IFrameCommand Creator(ref FRAME_CMD_PKG msg)
		{
			FrameCommand<PlayerSellEquipCommand> frameCommand = FrameCommandFactory.CreateFrameCommand<PlayerSellEquipCommand>();
			frameCommand.cmdData.m_equipIndex = (int)msg.stCmdInfo.get_stCmdPlayerSellEquip().bEquipIndex;
			return frameCommand;
		}

		public bool TransProtocol(FRAME_CMD_PKG msg)
		{
			msg.stCmdInfo.get_stCmdPlayerSellEquip().bEquipIndex = (byte)this.m_equipIndex;
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
				Singleton<CBattleSystem>.GetInstance().m_battleEquipSystem.ExecuteSellEquipFrameCommand(this.m_equipIndex, ref player.Captain);
			}
		}

		public void AwakeCommand(IFrameCommand cmd)
		{
		}
	}
}
