using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using CSProtocol;
using System;

namespace Assets.Scripts.GameLogic
{
	[FrameCommandClass]
	public struct LearnSkillCommand : ICommandImplement
	{
		public uint dwHeroID;

		public byte bSlotType;

		public byte bSkillLevel;

		[FrameCommandCreator]
		public static IFrameCommand Creator(ref FRAME_CMD_PKG msg)
		{
			FrameCommand<LearnSkillCommand> frameCommand = FrameCommandFactory.CreateFrameCommand<LearnSkillCommand>();
			frameCommand.cmdData.dwHeroID = msg.stCmdInfo.get_stCmdPlayerLearnSkill().dwHeroObjID;
			frameCommand.cmdData.bSlotType = msg.stCmdInfo.get_stCmdPlayerLearnSkill().bSlotType;
			frameCommand.cmdData.bSkillLevel = msg.stCmdInfo.get_stCmdPlayerLearnSkill().bSkillLevel;
			return frameCommand;
		}

		public bool TransProtocol(FRAME_CMD_PKG msg)
		{
			msg.stCmdInfo.get_stCmdPlayerLearnSkill().dwHeroObjID = this.dwHeroID;
			msg.stCmdInfo.get_stCmdPlayerLearnSkill().bSlotType = this.bSlotType;
			msg.stCmdInfo.get_stCmdPlayerLearnSkill().bSkillLevel = this.bSkillLevel;
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
				ReadonlyContext<PoolObjHandle<ActorRoot>> allHeroes = player.GetAllHeroes();
				for (int i = 0; i < allHeroes.get_Count(); i++)
				{
					if (allHeroes.get_Item(i).get_handle().ObjID == this.dwHeroID)
					{
						allHeroes.get_Item(i).get_handle().ActorControl.CmdCommonLearnSkill(cmd);
						break;
					}
				}
			}
		}

		public void AwakeCommand(IFrameCommand cmd)
		{
		}
	}
}
