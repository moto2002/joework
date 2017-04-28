using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using Assets.Scripts.GameSystem;
using CSProtocol;
using System;

namespace Assets.Scripts.GameLogic
{
	[FrameSCSYNCCommandClass]
	public struct SvrReconnectCommand : ICommandImplement
	{
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
			Player player = Singleton<GamePlayerCenter>.GetInstance().GetPlayer(cmd.playerID);
			if (player != null && player.Captain)
			{
				if (ActorHelper.IsHostCampActor(ref player.Captain))
				{
					KillDetailInfo killDetailInfo = new KillDetailInfo();
					killDetailInfo.Killer = player.Captain;
					killDetailInfo.bSelfCamp = true;
					killDetailInfo.Type = KillDetailInfoType.Info_Type_Reconnect;
					Singleton<EventRouter>.get_instance().BroadCastEvent<KillDetailInfo>(EventID.AchievementRecorderEvent, killDetailInfo);
					Singleton<EventRouter>.get_instance().BroadCastEvent<bool, uint>(EventID.DisConnectNtf, false, cmd.playerID);
				}
				Singleton<CBattleSystem>.GetInstance().m_battleEquipSystem.ExecuteInOutEquipShopFrameCommand(0, ref player.Captain);
			}
		}

		public void AwakeCommand(IFrameCommand cmd)
		{
		}
	}
}
