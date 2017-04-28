using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using CSProtocol;
using System;

namespace Assets.Scripts.GameLogic
{
	[FrameCommandClass]
	public struct LearnTalentCommand : ICommandImplement
	{
		public uint HeroObjID;

		public sbyte TalentLevelIndex;

		public uint TalentID;

		[FrameCommandCreator]
		public static IFrameCommand Creator(ref FRAME_CMD_PKG msg)
		{
			FrameCommand<LearnTalentCommand> frameCommand = FrameCommandFactory.CreateFrameCommand<LearnTalentCommand>();
			frameCommand.cmdData.HeroObjID = msg.stCmdInfo.get_stCmdPlayerLearnTalent().dwHeroObjID;
			frameCommand.cmdData.TalentLevelIndex = msg.stCmdInfo.get_stCmdPlayerLearnTalent().chTalentLevel;
			frameCommand.cmdData.TalentID = msg.stCmdInfo.get_stCmdPlayerLearnTalent().dwTalentID;
			return frameCommand;
		}

		public bool TransProtocol(FRAME_CMD_PKG msg)
		{
			msg.stCmdInfo.get_stCmdPlayerLearnTalent().dwHeroObjID = this.HeroObjID;
			msg.stCmdInfo.get_stCmdPlayerLearnTalent().chTalentLevel = this.TalentLevelIndex;
			msg.stCmdInfo.get_stCmdPlayerLearnTalent().dwTalentID = this.TalentID;
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
					if (allHeroes.get_Item(i).get_handle().ObjID == this.HeroObjID)
					{
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
