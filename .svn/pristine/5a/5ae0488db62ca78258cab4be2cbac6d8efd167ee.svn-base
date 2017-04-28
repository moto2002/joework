using Assets.Scripts.Framework;
using CSProtocol;
using System;

namespace Assets.Scripts.GameSystem
{
	[MessageHandlerClass]
	public class VoiceStateNetCore
	{
		public static void Send_Acnt_VoiceState(CS_VOICESTATE_TYPE type)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(4850u);
			cSPkg.stPkgData.get_stAcntVoiceState().bVoiceState = type;
			Singleton<NetworkModule>.GetInstance().SendGameMsg(ref cSPkg, 0u);
		}

		[MessageHandler(4851)]
		public static void On_NTF_VOICESTATE(CSPkg msg)
		{
			SCPKG_NTF_VOICESTATE stNtfVoiceState = msg.stPkgData.get_stNtfVoiceState();
			MonoSingleton<VoiceSys>.get_instance().SetVoiceState(stNtfVoiceState.ullAcntUid, stNtfVoiceState.bVoiceState);
			if (Singleton<CBattleSystem>.get_instance().BattleStatView != null)
			{
				Singleton<CBattleSystem>.get_instance().BattleStatView.RefreshVoiceStateIfNess();
			}
		}
	}
}
