using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using CSProtocol;
using System;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
	[FrameCommandClass]
	public struct PlayerAddGoldCoinInBattleCommand : ICommandImplement
	{
		public uint m_addValue;

		[FrameCommandCreator]
		public static IFrameCommand Creator(ref FRAME_CMD_PKG msg)
		{
			FrameCommand<PlayerAddGoldCoinInBattleCommand> frameCommand = FrameCommandFactory.CreateFrameCommand<PlayerAddGoldCoinInBattleCommand>();
			frameCommand.cmdData.m_addValue = msg.stCmdInfo.get_stCmdPlayerAddGoldCoinInBattle().dwAddValue;
			return frameCommand;
		}

		public bool TransProtocol(FRAME_CMD_PKG msg)
		{
			msg.stCmdInfo.get_stCmdPlayerAddGoldCoinInBattle().dwAddValue = this.m_addValue;
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
			if (player != null && ((flag && player.isGM) || (!flag && LobbyMsgHandler.isHostGMAcnt)) && player.Captain && player.Captain.get_handle().ValueComponent != null)
			{
				player.Captain.get_handle().ValueComponent.ChangeGoldCoinInBattle((int)this.m_addValue, true, true, default(Vector3), false, default(PoolObjHandle<ActorRoot>));
			}
		}

		public void AwakeCommand(IFrameCommand cmd)
		{
		}
	}
}
