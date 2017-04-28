using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using CSProtocol;
using System;

namespace Assets.Scripts.GameLogic
{
	[FrameCSSYNCCommandClass]
	public struct UseObjectiveSkillCommand : ICommandImplement
	{
		public uint ObjectID;

		public SkillSlotType SlotType;

		[FrameCommandCreator]
		public static IFrameCommand Creator(ref CSDT_GAMING_CSSYNCINFO msg)
		{
			FrameCommand<UseObjectiveSkillCommand> frameCommand = FrameCommandFactory.CreateCSSyncFrameCommand<UseObjectiveSkillCommand>();
			frameCommand.cmdData.ObjectID = (uint)msg.stCSSyncDt.get_stObjectiveSkill().iObjectID;
			frameCommand.cmdData.SlotType = (SkillSlotType)msg.stCSSyncDt.get_stObjectiveSkill().chSlotType;
			return frameCommand;
		}

		public bool TransProtocol(FRAME_CMD_PKG msg)
		{
			return true;
		}

		public bool TransProtocol(CSDT_GAMING_CSSYNCINFO msg)
		{
			msg.stCSSyncDt.get_stObjectiveSkill().iObjectID = (int)this.ObjectID;
			msg.stCSSyncDt.get_stObjectiveSkill().chSlotType = (sbyte)this.SlotType;
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
				SkillUseParam skillUseParam = default(SkillUseParam);
				skillUseParam.Init(this.SlotType, this.ObjectID);
				player.Captain.get_handle().ActorControl.CmdUseSkill(cmd, ref skillUseParam);
			}
		}

		public void AwakeCommand(IFrameCommand cmd)
		{
		}
	}
}
