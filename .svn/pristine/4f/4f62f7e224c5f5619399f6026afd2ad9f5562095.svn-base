using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using CSProtocol;
using System;

namespace Assets.Scripts.GameLogic
{
	[FrameCommandClass]
	public struct SetSkillLevelInBattleCommand : ICommandImplement
	{
		public byte SkillSlot;

		public byte SkillLevel;

		[FrameCommandCreator]
		public static IFrameCommand Creator(ref FRAME_CMD_PKG msg)
		{
			FrameCommand<SetSkillLevelInBattleCommand> frameCommand = FrameCommandFactory.CreateFrameCommand<SetSkillLevelInBattleCommand>();
			frameCommand.cmdData.SkillSlot = msg.stCmdInfo.get_stCmdSetSkillLevel().bSkillSlot;
			frameCommand.cmdData.SkillLevel = msg.stCmdInfo.get_stCmdSetSkillLevel().bSkillLevel;
			return frameCommand;
		}

		public bool TransProtocol(FRAME_CMD_PKG msg)
		{
			msg.stCmdInfo.get_stCmdSetSkillLevel().bSkillSlot = this.SkillSlot;
			msg.stCmdInfo.get_stCmdSetSkillLevel().bSkillLevel = this.SkillLevel;
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
			if (player != null && player.isGM && player.Captain && player.Captain.get_handle().SkillControl != null && this.SkillSlot < 10)
			{
				player.Captain.get_handle().SkillControl.SkillSlotArray[(int)this.SkillSlot].SetSkillLevel((int)this.SkillLevel);
			}
		}

		public void AwakeCommand(IFrameCommand cmd)
		{
		}
	}
}
