using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using CSProtocol;
using System;

namespace Assets.Scripts.GameLogic
{
	[FrameCommandClass]
	public struct SwitchCaptainCommand : ICommandImplement
	{
		public uint ObjectID;

		[FrameCommandCreator]
		public static IFrameCommand Creator(ref FRAME_CMD_PKG msg)
		{
			FrameCommand<SwitchCaptainCommand> frameCommand = FrameCommandFactory.CreateFrameCommand<SwitchCaptainCommand>();
			frameCommand.cmdData.ObjectID = (uint)msg.stCmdInfo.get_stCmdPlayerSwitchCaptain().iObjectID;
			return frameCommand;
		}

		public bool TransProtocol(FRAME_CMD_PKG msg)
		{
			msg.stCmdInfo.get_stCmdPlayerSwitchCaptain().iObjectID = (int)this.ObjectID;
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
				PoolObjHandle<ActorRoot> actor = Singleton<GameObjMgr>.get_instance().GetActor(this.ObjectID);
				if (actor && actor.get_handle().TheActorMeta.PlayerId == cmd.playerID)
				{
					player.Captain.get_handle().HudControl.SetSelected(false);
					player.Captain.get_handle().ActorControl.SetSelected(false);
					bool isAutoAI = player.Captain.get_handle().ActorControl.m_isAutoAI;
					player.Captain.get_handle().ActorControl.SetAutoAI(true);
					player.SetCaptain((uint)actor.get_handle().TheActorMeta.ConfigId);
					player.Captain.get_handle().HudControl.SetSelected(true);
					player.Captain.get_handle().ActorControl.SetSelected(true);
					player.Captain.get_handle().ActorControl.SetAutoAI(isAutoAI);
					DefaultGameEventParam defaultGameEventParam = new DefaultGameEventParam(actor, actor);
					Singleton<GameEventSys>.get_instance().SendEvent<DefaultGameEventParam>(GameEventDef.Event_CaptainSwitch, ref defaultGameEventParam);
				}
			}
		}

		public void AwakeCommand(IFrameCommand cmd)
		{
		}
	}
}
