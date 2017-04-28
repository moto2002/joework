using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using CSProtocol;
using System;

namespace Assets.Scripts.GameLogic
{
	[FrameCSSYNCCommandClass]
	public struct UsePositionSkillCommand : ICommandImplement
	{
		public VInt2 Position;

		public SkillSlotType SlotType;

		[FrameCommandCreator]
		public static IFrameCommand Creator(ref CSDT_GAMING_CSSYNCINFO msg)
		{
			FrameCommand<UsePositionSkillCommand> frameCommand = FrameCommandFactory.CreateCSSyncFrameCommand<UsePositionSkillCommand>();
			frameCommand.cmdData.SlotType = (SkillSlotType)msg.stCSSyncDt.get_stPositionSkill().chSlotType;
			frameCommand.cmdData.Position = CommonTools.ToVector2(msg.stCSSyncDt.get_stPositionSkill().stPosition);
			return frameCommand;
		}

		public bool TransProtocol(FRAME_CMD_PKG msg)
		{
			return true;
		}

		public bool TransProtocol(CSDT_GAMING_CSSYNCINFO msg)
		{
			msg.stCSSyncDt.get_stPositionSkill().chSlotType = (sbyte)this.SlotType;
			CommonTools.CSDTFromVector2(this.Position, ref msg.stCSSyncDt.get_stPositionSkill().stPosition);
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
				VInt3 vInt = new VInt3(this.Position.x, 0, this.Position.y);
				VInt vInt2 = 0;
				if (PathfindingUtility.GetGroundY(vInt, out vInt2))
				{
					vInt.y = vInt2.i;
				}
				skillUseParam.Init(this.SlotType, vInt);
				player.Captain.get_handle().ActorControl.CmdUseSkill(cmd, ref skillUseParam);
			}
		}

		public void AwakeCommand(IFrameCommand cmd)
		{
		}
	}
}
