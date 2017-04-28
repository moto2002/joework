using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using CSProtocol;
using System;

namespace Assets.Scripts.GameLogic
{
	[FrameSCSYNCCommandClass]
	public struct AutoAIChgCommand : ICommandImplement
	{
		public byte m_autoType;

		public uint m_playerID;

		public bool TransProtocol(FRAME_CMD_PKG msg)
		{
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
			Player player = Singleton<GamePlayerCenter>.get_instance().GetPlayer(this.m_playerID);
			if (player != null && player.Captain)
			{
				if (this.m_autoType == 1)
				{
					player.Captain.get_handle().ActorControl.SetAutoAI(true);
				}
				else if (this.m_autoType == 2)
				{
					player.Captain.get_handle().ActorControl.SetAutoAI(false);
				}
				else if (this.m_autoType == 3)
				{
					player.Captain.get_handle().ActorControl.SetOffline(true);
				}
			}
		}

		public void AwakeCommand(IFrameCommand cmd)
		{
		}
	}
}
