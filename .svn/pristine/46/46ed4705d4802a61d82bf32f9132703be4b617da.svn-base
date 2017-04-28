using Assets.Scripts.GameLogic.GameKernal;
using Assets.Scripts.UI;
using CSProtocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	[MessageHandlerClass]
	public class VoiceSys : MonoSingleton<VoiceSys>
	{
		public struct ROOMINFO
		{
			public ulong roomid;

			public ulong ullRoomKey;

			public uint memberid;

			public string openid;

			public ulong uuid;
		}

		public struct ROOMMate
		{
			public uint memID;

			public string openID;

			public ulong uuid;
		}

		public class VoiceState
		{
			public CS_VOICESTATE_TYPE state;

			public ulong uid;

			public VoiceState(ulong uid, CS_VOICESTATE_TYPE state)
			{
				this.uid = uid;
				this.state = state;
			}
		}

		private CApolloVoiceSys m_ApolloVoiceMgr;

		private bool m_isOpenMic;

		private bool m_isOpenSpeaker;

		private bool m_bInRoom;

		private VoiceSys.ROOMINFO m_MyRoomInfo = default(VoiceSys.ROOMINFO);

		private float m_UpdateTime;

		private bool m_bUpdateEnterRoomState;

		private List<VoiceSys.ROOMMate> m_RoomMateList = new List<VoiceSys.ROOMMate>();

		private bool m_bGetIsBattleSupportVoice;

		private bool m_IsBattleSupportVoice;

		private List<ulong> forbidMemberVoice = new List<ulong>();

		private bool m_bGobalUseVoice;

		private bool m_bUseMicOnUser;

		private bool m_bUseVoiceSysSetting;

		private float m_VoiceLevel = 100f;

		private int m_TotalVoiceTime = 10;

		private Transform m_SoundLevel_HeroSelect;

		private Transform m_VoiceBtn;

		private bool m_bInHeroSelectUI;

		public string m_Voice_Server_Not_Open_Tips = string.Empty;

		public string m_Voice_Cannot_JoinVoiceRoom = string.Empty;

		public string m_Voice_Cannot_OpenSetting = string.Empty;

		public string m_Voice_Battle_FirstTips = string.Empty;

		public string m_Voice_Battle_OpenSpeaker = string.Empty;

		public string m_Voice_Battle_CloseSpeaker = string.Empty;

		public string m_Voice_Battle_OpenMic = string.Empty;

		public string m_Voice_Battle_CloseMic = string.Empty;

		public string m_Voice_Battle_FIrstOPenSpeak = string.Empty;

		private float m_fStartTimeHeroSelect;

		private float m_fCurTimeHeroSelect;

		private float m_VoiceUpdateDelta = 0.1f;

		private bool m_bClickSound;

		private int m_nSoundBattleTimerID;

		private bool m_bSoundInBattle;

		public static int maxDeltaTime = 3000;

		private int m_timer = -1;

		private List<VoiceSys.VoiceState> m_voiceStateList = new List<VoiceSys.VoiceState>();

		private CApolloVoiceSys ApolloVoiceMgr
		{
			get
			{
				if (this.m_ApolloVoiceMgr == null)
				{
					return null;
				}
				if (this.m_ApolloVoiceMgr.CallApolloVoiceSDK == null)
				{
					return null;
				}
				return this.m_ApolloVoiceMgr;
			}
		}

		public bool UseMic
		{
			get
			{
				return this.m_isOpenMic;
			}
		}

		public bool UseSpeak
		{
			get
			{
				return this.m_isOpenSpeaker;
			}
		}

		public bool GlobalVoiceSetting
		{
			get
			{
				return this.m_bGobalUseVoice;
			}
		}

		public bool UseMicOnUser
		{
			get
			{
				return this.m_bUseMicOnUser;
			}
			set
			{
				this.m_bUseMicOnUser = value;
			}
		}

		public bool IsUseVoiceSysSetting
		{
			get
			{
				return this.m_bUseVoiceSysSetting;
			}
			set
			{
				this.m_bUseVoiceSysSetting = value;
				if (this.m_bUseVoiceSysSetting)
				{
					this.OpenSpeakers();
				}
				else
				{
					this.ClosenSpeakers();
				}
			}
		}

		public float VoiceLevel
		{
			get
			{
				return this.m_VoiceLevel;
			}
			set
			{
				this.m_VoiceLevel = value;
				this.SetSpeakerVolume((int)this.m_VoiceLevel);
			}
		}

		public int TotalVoiceTime
		{
			get
			{
				return this.m_TotalVoiceTime;
			}
		}

		public CS_VOICESTATE_TYPE lastSendVoiceState
		{
			get;
			set;
		}

		public CS_VOICESTATE_TYPE curVoiceState
		{
			get
			{
				if (this.IsOpenMic())
				{
					return 2;
				}
				if (this.IsOpenSpeak())
				{
					return 1;
				}
				return 0;
			}
		}

		[MessageHandler(3100)]
		public static void On_CreateVoiceRoom(CSPkg msg)
		{
			try
			{
				MonoSingleton<VoiceSys>.GetInstance().EnterRoom(msg.stPkgData.get_stCreateTvoipRoomNtf());
			}
			catch (Exception ex)
			{
				DebugHelper.Assert(false, "Exception in CreateVoiceRoom, {0}\n {1}", new object[]
				{
					ex.get_Message(),
					ex.get_StackTrace()
				});
			}
		}

		[MessageHandler(3101)]
		public static void On_UpdateRoomMateInfo(CSPkg msg)
		{
			try
			{
				MonoSingleton<VoiceSys>.GetInstance().UpdateRoomMateInfo(msg.stPkgData.get_stJoinTvoipRoomNtf());
			}
			catch (Exception ex)
			{
				DebugHelper.Assert(false, "Exception in On_UpdateRoomMateInfo, {0}\n {1}", new object[]
				{
					ex.get_Message(),
					ex.get_StackTrace()
				});
			}
		}

		[DebuggerHidden]
		private IEnumerator UpdateEnterRoomState()
		{
			VoiceSys.<UpdateEnterRoomState>c__Iterator39 <UpdateEnterRoomState>c__Iterator = new VoiceSys.<UpdateEnterRoomState>c__Iterator39();
			<UpdateEnterRoomState>c__Iterator.<>f__this = this;
			return <UpdateEnterRoomState>c__Iterator;
		}

		private void UpdateRoomMateInfo(SCPKG_JOIN_TVOIP_ROOM_NTF updateInfo)
		{
			uint dwMemberID = updateInfo.stUserInfo.dwMemberID;
			ulong ullUid = updateInfo.stUserInfo.ullUid;
			string text = Utility.UTF8Convert(updateInfo.stUserInfo.szOpenID);
			this.PrintLog(string.Concat(new object[]
			{
				"***recv update msg***memid ",
				dwMemberID,
				" uid ",
				ullUid,
				" id ",
				text
			}), null, false);
			bool flag = false;
			for (int i = 0; i < this.m_RoomMateList.get_Count(); i++)
			{
				VoiceSys.ROOMMate rOOMMate = this.m_RoomMateList.get_Item(i);
				if (rOOMMate.openID == text && rOOMMate.uuid == ullUid)
				{
					rOOMMate.memID = dwMemberID;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				VoiceSys.ROOMMate rOOMMate2 = default(VoiceSys.ROOMMate);
				rOOMMate2.memID = dwMemberID;
				rOOMMate2.openID = text;
				rOOMMate2.uuid = ullUid;
				this.m_RoomMateList.Add(rOOMMate2);
			}
		}

		private void EnterRoom(SCPKG_CREATE_TVOIP_ROOM_NTF roomInfoSvr)
		{
			this.PrintLog("***recv enterroom msg***", null, false);
			this.m_bGobalUseVoice = true;
			this.EnterRoomReal(roomInfoSvr);
		}

		private void EnterRoomReal(SCPKG_CREATE_TVOIP_ROOM_NTF roomInfoSvr)
		{
			if (this.m_bInRoom)
			{
				this.PrintLog("***reconnect enter room***", null, false);
			}
			this.PrintLog("EnterRoom", null, false);
			this.m_bInRoom = false;
			this.LeaveRoom();
			this.UpdateHeroSelectVoiceBtnState(false);
			this.m_RoomMateList.Clear();
			string openId = Singleton<ApolloHelper>.GetInstance().GetAccountInfo(false).get_OpenId();
			this.m_MyRoomInfo = default(VoiceSys.ROOMINFO);
			if (this.m_ApolloVoiceMgr == null)
			{
				this.m_ApolloVoiceMgr = new CApolloVoiceSys();
				this.m_ApolloVoiceMgr.SysInitial();
				if (this.m_ApolloVoiceMgr != null)
				{
					this.PrintLog("ApolloVoiceMgr Created", null, false);
				}
				this.CreateEngine();
			}
			if (this.ApolloVoiceMgr != null)
			{
				string[] array = new string[3];
				for (int i = 0; i < 3; i++)
				{
					array[i] = string.Empty;
				}
				if (MonoSingleton<CTongCaiSys>.get_instance().IsCanUseTongCai() && MonoSingleton<CTongCaiSys>.get_instance().IsLianTongIp())
				{
					string text = "16285";
					for (int j = 0; j < (int)roomInfoSvr.wAccessIPCount; j++)
					{
						array[j] = "udp://" + MonoSingleton<CTongCaiSys>.get_instance().TongcaiIps[2] + ":" + text;
					}
				}
				else
				{
					for (int k = 0; k < (int)roomInfoSvr.wAccessIPCount; k++)
					{
						uint dwIP = roomInfoSvr.astAccessIPList[k].dwIP;
						array[k] = "udp://" + IPAddress.Parse(((uint)IPAddress.NetworkToHostOrder((int)dwIP)).ToString()).ToString() + ":" + roomInfoSvr.astAccessIPList[k].wPort.ToString();
						this.PrintLog("host_ip : " + array[k], null, false);
					}
				}
				this.m_MyRoomInfo.roomid = roomInfoSvr.ullRoomID;
				this.m_MyRoomInfo.ullRoomKey = roomInfoSvr.ullRoomKey;
				this.m_MyRoomInfo.openid = openId;
				bool flag = false;
				int num = 0;
				while ((long)num < (long)((ulong)roomInfoSvr.dwRoomUserCnt))
				{
					if (Utility.UTF8Convert(roomInfoSvr.astRoomUserList[num].szOpenID) == openId)
					{
						flag = true;
						this.m_MyRoomInfo.memberid = roomInfoSvr.astRoomUserList[num].dwMemberID;
						this.m_MyRoomInfo.uuid = roomInfoSvr.astRoomUserList[num].ullUid;
					}
					else
					{
						VoiceSys.ROOMMate rOOMMate = default(VoiceSys.ROOMMate);
						rOOMMate.openID = Utility.UTF8Convert(roomInfoSvr.astRoomUserList[num].szOpenID);
						rOOMMate.memID = roomInfoSvr.astRoomUserList[num].dwMemberID;
						rOOMMate.uuid = roomInfoSvr.astRoomUserList[num].ullUid;
						this.m_RoomMateList.Add(rOOMMate);
						this.PrintLog(string.Format("memList idx = {0}, id = {1} , openid = {2}", num.ToString(), rOOMMate.memID, rOOMMate.openID), null, false);
					}
					num++;
				}
				if (!flag)
				{
					this.PrintLog("matelist error", null, false);
				}
				string message = string.Format("roomid is {0}, roomkey is {1}, openid is {2}, memberid is {3}, accIP is {4}, {5}, {6}\n", new object[]
				{
					this.m_MyRoomInfo.roomid,
					this.m_MyRoomInfo.ullRoomKey,
					this.m_MyRoomInfo.openid,
					this.m_MyRoomInfo.memberid,
					array[0],
					array[1],
					array[2]
				});
				this.PrintLog(message, null, false);
				if (this.JoinVoiceRoom(array[0], array[1], array[2], (long)this.m_MyRoomInfo.roomid, (long)this.m_MyRoomInfo.ullRoomKey, (short)this.m_MyRoomInfo.memberid, this.m_MyRoomInfo.openid, 6000) == 0 && !this.m_bUpdateEnterRoomState)
				{
					this.m_bUpdateEnterRoomState = true;
					this.PrintLog("UpdateEnterRoomState", null, false);
					base.StartCoroutine(this.UpdateEnterRoomState());
				}
			}
		}

		public bool IsOpenSpeak()
		{
			return this.m_isOpenSpeaker;
		}

		public bool IsOpenMic()
		{
			return this.m_isOpenMic;
		}

		public void LeaveRoom()
		{
			try
			{
				this.m_bInRoom = false;
				this.m_isOpenMic = false;
				this.m_isOpenSpeaker = false;
				this.QuitRoom();
			}
			catch (Exception ex)
			{
				DebugHelper.Assert(false, "Exception In VoiceSys.LeaveRoom, {0}\n {1}", new object[]
				{
					ex.get_Message(),
					ex.get_StackTrace()
				});
			}
		}

		private int JoinVoiceRoom(string url1, string url2, string url3, long roomId, long roomKey, short memberId, string OpenId, int nTimeOut)
		{
			if (this.ApolloVoiceMgr != null && this.ApolloVoiceMgr.CallApolloVoiceSDK != null)
			{
				return this.ApolloVoiceMgr.CallApolloVoiceSDK._JoinRoom(url1, url2, url3, roomId, roomKey, memberId, OpenId, nTimeOut);
			}
			this.PrintLog("JoinVoiceRoom fatal error", null, false);
			return 3;
		}

		private void QuitRoom()
		{
			if (this.ApolloVoiceMgr != null)
			{
				int num = this.ApolloVoiceMgr.CallApolloVoiceSDK._QuitRoom((long)this.m_MyRoomInfo.roomid, (short)this.m_MyRoomInfo.memberid, this.m_MyRoomInfo.openid);
				if (num == 0)
				{
					this.m_bInRoom = false;
					this.PrintLog("QuitRoom Succ", null, false);
				}
				else
				{
					string message = string.Format("QuitRoom Err is {0}", num);
					this.PrintLog(message, null, false);
				}
			}
		}

		private void CreateEngine()
		{
			if (this.ApolloVoiceMgr != null)
			{
				ApolloVoiceErr apolloVoiceErr = (ApolloVoiceErr)this.ApolloVoiceMgr.CallApolloVoiceSDK._CreateApolloVoiceEngine(ApolloConfig.appID);
				if (apolloVoiceErr == ApolloVoiceErr.APOLLO_VOICE_SUCC)
				{
					this.PrintLog("CreateApolloVoiceEngine Succ", null, false);
				}
				else
				{
					string message = string.Format("CreateApolloVoiceEngine Err is {0}", apolloVoiceErr);
					this.PrintLog(message, null, false);
				}
			}
			this.PrintLog("CreateEngine", null, false);
		}

		private void DestoyEngine()
		{
			if (this.ApolloVoiceMgr != null)
			{
				ApolloVoiceErr apolloVoiceErr = (ApolloVoiceErr)this.ApolloVoiceMgr.CallApolloVoiceSDK._DestoryApolloVoiceEngine();
				if (apolloVoiceErr == ApolloVoiceErr.APOLLO_VOICE_SUCC)
				{
					this.PrintLog("_DestoryApolloVoiceEngine Succ", null, false);
				}
				else
				{
					string message = string.Format("_DestoryApolloVoiceEngine Err is {0}", apolloVoiceErr);
					this.PrintLog(message, null, false);
				}
			}
			this.PrintLog("_DestoryApolloVoiceEngine", null, false);
		}

		public void OpenMic()
		{
			if (this.m_bInRoom && this.ApolloVoiceMgr != null && this.ApolloVoiceMgr.CallApolloVoiceSDK != null)
			{
				ApolloVoiceErr apolloVoiceErr = (ApolloVoiceErr)this.ApolloVoiceMgr.CallApolloVoiceSDK._OpenMic();
				if (apolloVoiceErr == ApolloVoiceErr.APOLLO_VOICE_SUCC)
				{
					this.UpdateMyVoiceIcon(1);
					this.m_isOpenMic = true;
					this.PrintLog("OpenMic Succ", null, false);
				}
				else
				{
					string message = string.Format("OpenMic Err is {0}", apolloVoiceErr);
					this.PrintLog(message, null, false);
				}
			}
			else
			{
				this.m_isOpenMic = true;
			}
			this.PrintLog("onOpenMicButtonClick", null, false);
		}

		public void CloseMic()
		{
			if (this.m_bInRoom && this.ApolloVoiceMgr != null && this.ApolloVoiceMgr.CallApolloVoiceSDK != null)
			{
				ApolloVoiceErr apolloVoiceErr = (ApolloVoiceErr)this.ApolloVoiceMgr.CallApolloVoiceSDK._CloseMic();
				if (apolloVoiceErr == ApolloVoiceErr.APOLLO_VOICE_SUCC)
				{
					this.UpdateMyVoiceIcon(0);
					this.m_isOpenMic = false;
					this.PrintLog("CloseMic Succ", null, false);
				}
				else
				{
					string message = string.Format("CloseMic Err is {0}", apolloVoiceErr);
					this.PrintLog(message, null, false);
				}
			}
			else
			{
				this.m_isOpenMic = false;
			}
			this.PrintLog("onCloseMicButtonClick", null, false);
		}

		public void OpenSpeakers()
		{
			if (this.m_bInRoom && this.ApolloVoiceMgr != null && this.ApolloVoiceMgr.CallApolloVoiceSDK != null)
			{
				if (!this.m_isOpenSpeaker)
				{
					ApolloVoiceErr apolloVoiceErr = (ApolloVoiceErr)this.ApolloVoiceMgr.CallApolloVoiceSDK._OpenSpeaker();
					if (apolloVoiceErr == ApolloVoiceErr.APOLLO_VOICE_SUCC)
					{
						this.m_isOpenSpeaker = true;
						this.PrintLog("OpenSpeaker Succ", null, false);
					}
					else
					{
						string message = string.Format("OpenSpeaker Err is {0}", apolloVoiceErr);
						this.PrintLog(message, null, false);
					}
				}
			}
			else
			{
				this.m_isOpenSpeaker = true;
			}
			this.PrintLog("onOpenSpeakersButtonClick", null, false);
		}

		public void ClosenSpeakers()
		{
			if (this.m_bInRoom && this.ApolloVoiceMgr != null && this.ApolloVoiceMgr.CallApolloVoiceSDK != null)
			{
				if (this.m_isOpenSpeaker)
				{
					ApolloVoiceErr apolloVoiceErr = (ApolloVoiceErr)this.ApolloVoiceMgr.CallApolloVoiceSDK._CloseSpeaker();
					if (apolloVoiceErr == ApolloVoiceErr.APOLLO_VOICE_SUCC)
					{
						this.m_isOpenSpeaker = false;
						this.PrintLog("CloseSpeaker Succ", null, false);
					}
					else
					{
						string message = string.Format("CloseSpeaker Err is {0}", apolloVoiceErr);
						this.PrintLog(message, null, false);
					}
				}
			}
			else
			{
				this.m_isOpenSpeaker = false;
			}
			this.PrintLog("onClosenSpeakersButtonClick", null, false);
		}

		private void onGetMemberState()
		{
			if (this.m_bInRoom && this.ApolloVoiceMgr != null && this.ApolloVoiceMgr.CallApolloVoiceSDK != null)
			{
				int[] array = new int[12];
				int num = this.ApolloVoiceMgr.CallApolloVoiceSDK._GetMemberState(array);
				if (num >= 0)
				{
					for (int i = 0; i < num; i++)
					{
						int memID = array[2 * i];
						int iMemberState = array[2 * i + 1];
						this.UpdateVoiceIcon(memID, iMemberState);
					}
				}
			}
		}

		public bool IsBattleSupportVoice()
		{
			if (this.m_bGetIsBattleSupportVoice)
			{
				return this.m_IsBattleSupportVoice;
			}
			this.m_bGetIsBattleSupportVoice = true;
			this.m_IsBattleSupportVoice = false;
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			if (hostPlayer == null)
			{
				return false;
			}
			int num = 0;
			COM_PLAYERCAMP playerCamp = hostPlayer.PlayerCamp;
			List<Player> allCampPlayers = Singleton<GamePlayerCenter>.GetInstance().GetAllCampPlayers(playerCamp);
			for (int i = 0; i < allCampPlayers.get_Count(); i++)
			{
				Player player = allCampPlayers.get_Item(i);
				if (player != null && !player.Computer)
				{
					num++;
				}
			}
			if (num >= 2)
			{
				this.m_IsBattleSupportVoice = true;
				return true;
			}
			this.m_IsBattleSupportVoice = false;
			return false;
		}

		private ulong FindUUIDByMemId(int memID)
		{
			if ((ulong)this.m_MyRoomInfo.memberid == (ulong)((long)memID))
			{
				return this.m_MyRoomInfo.uuid;
			}
			for (int i = 0; i < this.m_RoomMateList.get_Count(); i++)
			{
				VoiceSys.ROOMMate rOOMMate = this.m_RoomMateList.get_Item(i);
				if ((ulong)rOOMMate.memID == (ulong)((long)memID))
				{
					return rOOMMate.uuid;
				}
			}
			return 0uL;
		}

		private uint FindMemIdByUUID(ulong uuid)
		{
			if (this.m_MyRoomInfo.uuid == uuid)
			{
				return this.m_MyRoomInfo.memberid;
			}
			for (int i = 0; i < this.m_RoomMateList.get_Count(); i++)
			{
				VoiceSys.ROOMMate rOOMMate = this.m_RoomMateList.get_Item(i);
				if (rOOMMate.uuid == uuid)
				{
					return rOOMMate.memID;
				}
			}
			return 0u;
		}

		public void UpdateMyVoiceIcon(int iMemberState)
		{
			if (Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo == null)
			{
				return;
			}
			MemberInfo masterMemberInfo = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetMasterMemberInfo();
			if (masterMemberInfo == null)
			{
				return;
			}
			ulong num = 0uL;
			if (Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo() != null)
			{
				num = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID;
			}
			if (this.m_bInHeroSelectUI)
			{
				Transform teamPlayerElement = Singleton<CHeroSelectBaseSystem>.get_instance().GetTeamPlayerElement(num, masterMemberInfo.camp);
				if (teamPlayerElement != null)
				{
					Transform transform = teamPlayerElement.FindChild("heroItemCell/VoiceIcon");
					if (this.m_bInRoom && transform)
					{
						transform.gameObject.CustomSetActive(iMemberState >= 1);
					}
				}
			}
			else if (num > 0uL && Singleton<CBattleSystem>.GetInstance().FightForm != null)
			{
				Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
				if (hostPlayer != null && this.m_bInRoom && hostPlayer.Captain && hostPlayer.Captain.get_handle() != null && hostPlayer.Captain.get_handle().HudControl != null)
				{
					hostPlayer.Captain.get_handle().HudControl.ShowVoiceIcon(iMemberState >= 1);
				}
			}
		}

		private void UpdateVoiceIcon(int memID, int iMemberState)
		{
			if (Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo == null)
			{
				return;
			}
			MemberInfo masterMemberInfo = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetMasterMemberInfo();
			if (masterMemberInfo == null)
			{
				return;
			}
			ulong num = this.FindUUIDByMemId(memID);
			if (this.m_bInHeroSelectUI)
			{
				Transform teamPlayerElement = Singleton<CHeroSelectBaseSystem>.get_instance().GetTeamPlayerElement(num, masterMemberInfo.camp);
				if (teamPlayerElement != null)
				{
					Transform transform = teamPlayerElement.FindChild("heroItemCell/VoiceIcon");
					if (this.m_bInRoom && transform)
					{
						transform.gameObject.CustomSetActive(iMemberState >= 1);
					}
				}
			}
			else if (num > 0uL && Singleton<CBattleSystem>.GetInstance().FightForm != null)
			{
				Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
				if (hostPlayer != null && this.m_bInRoom)
				{
					COM_PLAYERCAMP playerCamp = hostPlayer.PlayerCamp;
					List<Player> allCampPlayers = Singleton<GamePlayerCenter>.GetInstance().GetAllCampPlayers(playerCamp);
					for (int i = 0; i < allCampPlayers.get_Count(); i++)
					{
						Player player = allCampPlayers.get_Item(i);
						if (this.m_bInRoom && player != null && player.PlayerUId == num)
						{
							if (player.Captain && player.Captain.get_handle() != null && player.Captain.get_handle().HudControl != null)
							{
								player.Captain.get_handle().HudControl.ShowVoiceIcon(iMemberState >= 1);
							}
							break;
						}
					}
				}
			}
		}

		public void ClearInBattleForbidMember()
		{
			for (int i = 0; i < this.forbidMemberVoice.get_Count(); i++)
			{
				ulong uid = this.forbidMemberVoice.get_Item(i);
				this.ForbidMemberVoiceByUID(uid, false);
			}
			this.forbidMemberVoice.Clear();
		}

		public void SwitchForbidden(ulong uid)
		{
			if (!this.forbidMemberVoice.Contains(uid))
			{
				this.forbidMemberVoice.Add(uid);
				MonoSingleton<VoiceSys>.get_instance().ForbidMemberVoiceByUID(uid, true);
			}
			else
			{
				this.forbidMemberVoice.Remove(uid);
				MonoSingleton<VoiceSys>.get_instance().ForbidMemberVoiceByUID(uid, false);
			}
		}

		public bool IsForbid(ulong uid)
		{
			return this.forbidMemberVoice.Contains(uid);
		}

		public void ForbidMemberVoiceByUID(ulong uid, bool bForbidden)
		{
			this.ForbidMemberVoice((int)this.FindMemIdByUUID(uid), !bForbidden);
		}

		public void ForbidMemberVoice(int nMemberId, bool bForbidden)
		{
			if (this.m_bInRoom && this.ApolloVoiceMgr != null && this.ApolloVoiceMgr.CallApolloVoiceSDK != null && this.ApolloVoiceMgr.CallApolloVoiceSDK._EnableMemberVoice(nMemberId, bForbidden) == 0)
			{
				this.PrintLog(string.Format("ForbidMemberVoice id = {0}", nMemberId), null, false);
			}
		}

		private void SetSpeakerVolume(int level)
		{
			if (this.m_bInRoom && this.ApolloVoiceMgr != null && this.ApolloVoiceMgr.CallApolloVoiceSDK != null)
			{
				if (level <= 2)
				{
					level = 2;
				}
				this.ApolloVoiceMgr.CallApolloVoiceSDK._SetSpeakerVolume(level);
			}
		}

		public void OnApplicationPause(bool pauseStatus)
		{
			if (this.ApolloVoiceMgr == null)
			{
				return;
			}
			this.PrintLog("Voice OnApplicationPause: " + pauseStatus, null, false);
			if (pauseStatus)
			{
				if (this.ApolloVoiceMgr.CallApolloVoiceSDK._Pause() == 0)
				{
					this.PrintLog("_Pause succ", null, false);
				}
				if (this.ApolloVoiceMgr.CallApolloVoiceSDK._CloseMic() == 0)
				{
					this.PrintLog("onPause_CloseMic succ", null, false);
				}
				if (this.ApolloVoiceMgr.CallApolloVoiceSDK._CloseSpeaker() == 0)
				{
					this.PrintLog("onPause_CloseSpeaker succ", null, false);
				}
			}
			else
			{
				if (this.ApolloVoiceMgr.CallApolloVoiceSDK._Resume() == 0)
				{
					this.PrintLog("_Resume succ", null, false);
				}
				if (this.m_isOpenMic && this.ApolloVoiceMgr.CallApolloVoiceSDK._OpenMic() == 0)
				{
					this.PrintLog("onResume_OpenMic succ", null, false);
				}
				if (this.m_isOpenSpeaker && this.ApolloVoiceMgr.CallApolloVoiceSDK._OpenSpeaker() == 0)
				{
					this.PrintLog("onResume_OpenSpeaker succ", null, false);
				}
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			this.PrintLog("Ondestoy LeaveRoom", null, false);
			this.LeaveRoom();
		}

		public void ShowVoiceBtn_HeroSelect(CUIFormScript formScript)
		{
			this.PrintLog("ShowVoiceBtn_HeroSelect", null, false);
			this.m_bInHeroSelectUI = true;
			if (formScript != null)
			{
				this.m_SoundLevel_HeroSelect = formScript.transform.Find("chatTools/volume_PL");
			}
		}

		private void UpdateSoundLevel_HeroSelect(float soudLevel, float leftSecond)
		{
			if (this.m_SoundLevel_HeroSelect)
			{
				this.m_SoundLevel_HeroSelect.Find("Volume").GetComponent<Image>().CustomFillAmount(soudLevel);
				this.m_SoundLevel_HeroSelect.Find("CountDown").GetComponent<Text>().text = string.Format("{0:0.00}", leftSecond);
			}
		}

		protected override void Init()
		{
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.VOICE_HoldStart_VOCEBtn, new CUIEventManager.OnUIEventHandler(this.OnHoldStart_VOCEBtn_HeroSelect));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.VOICE_Hold_VOCEBtn, new CUIEventManager.OnUIEventHandler(this.OnHold_VOCEBtn_HeroSelect));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.VOICE_HoldEnd_VOCEBtn, new CUIEventManager.OnUIEventHandler(this.OnHoldEnd_VOCEBtn_HeroSelect));
			this.m_Voice_Server_Not_Open_Tips = Singleton<CTextManager>.GetInstance().GetText("Voice_Server_Not_Open_Tips");
			this.m_Voice_Cannot_JoinVoiceRoom = Singleton<CTextManager>.GetInstance().GetText("Voice_Cannot_JoinVoiceRoom");
			this.m_Voice_Cannot_OpenSetting = Singleton<CTextManager>.GetInstance().GetText("Voice_Cannot_OpenSetting");
			this.m_Voice_Battle_FirstTips = Singleton<CTextManager>.GetInstance().GetText("Voice_Battle_FirstTips");
			this.m_Voice_Battle_OpenSpeaker = Singleton<CTextManager>.GetInstance().GetText("Voice_Battle_OpenSpeaker");
			this.m_Voice_Battle_CloseSpeaker = Singleton<CTextManager>.GetInstance().GetText("Voice_Battle_CloseSpeaker");
			this.m_Voice_Battle_OpenMic = Singleton<CTextManager>.GetInstance().GetText("Voice_Battle_OpenMic");
			this.m_Voice_Battle_CloseMic = Singleton<CTextManager>.GetInstance().GetText("Voice_Battle_CloseMic");
			this.m_Voice_Battle_FIrstOPenSpeak = Singleton<CTextManager>.GetInstance().GetText("Voice_Battle_FIrstOPenSpeak");
		}

		private void CloseHeroSelctVoice()
		{
			if (this.m_bClickSound)
			{
				this.m_bClickSound = false;
				this.CloseMic();
				if (this.m_SoundLevel_HeroSelect)
				{
					this.m_SoundLevel_HeroSelect.gameObject.CustomSetActive(false);
				}
			}
		}

		private void OnHoldStart_VOCEBtn_HeroSelect(CUIEvent uiEvent)
		{
			if (CFakePvPHelper.bInFakeSelect)
			{
				if (!this.m_bUseVoiceSysSetting)
				{
					Singleton<CUIManager>.GetInstance().OpenTips(this.m_Voice_Cannot_OpenSetting, false, 1.5f, null, new object[0]);
					return;
				}
			}
			else
			{
				if (!this.m_bGobalUseVoice)
				{
					Singleton<CUIManager>.GetInstance().OpenTips(this.m_Voice_Server_Not_Open_Tips, false, 1.5f, null, new object[0]);
					return;
				}
				if (!this.m_bInRoom)
				{
					Singleton<CUIManager>.GetInstance().OpenTips(this.m_Voice_Cannot_JoinVoiceRoom, false, 1.5f, null, new object[0]);
					return;
				}
			}
			if (!this.m_bClickSound)
			{
				this.m_bClickSound = true;
				this.PrintLog("holdStart ", null, false);
				if (this.m_SoundLevel_HeroSelect)
				{
					this.m_SoundLevel_HeroSelect.gameObject.CustomSetActive(true);
				}
				this.m_fStartTimeHeroSelect = Time.time;
				this.m_fCurTimeHeroSelect = Time.time;
				this.UpdateSoundLevel_HeroSelect(Random.Range(0f, 1f), 10f);
				this.OpenMic();
			}
		}

		private void OnHold_VOCEBtn_HeroSelect(CUIEvent uiEvent)
		{
			this.PrintLog("holding ", null, false);
			if (Time.time - this.m_fStartTimeHeroSelect >= (float)this.m_TotalVoiceTime)
			{
				this.CloseHeroSelctVoice();
			}
			else if (Time.time - this.m_fCurTimeHeroSelect >= this.m_VoiceUpdateDelta)
			{
				this.PrintLog("holding " + this.m_fCurTimeHeroSelect, null, false);
				this.m_fCurTimeHeroSelect = Time.time;
				this.UpdateSoundLevel_HeroSelect(Random.Range(0f, 1f), (float)this.m_TotalVoiceTime - (Time.time - this.m_fStartTimeHeroSelect));
			}
		}

		private void OnHoldEnd_VOCEBtn_HeroSelect(CUIEvent uiEvent)
		{
			this.PrintLog("holdend ", null, false);
			this.CloseHeroSelctVoice();
		}

		private void UpdateHeroSelectVoiceBtnState(bool bEnable)
		{
			if (this.m_VoiceBtn)
			{
				if (!bEnable)
				{
					CUIEventScript component = this.m_VoiceBtn.GetComponent<CUIEventScript>();
					component.enabled = false;
					this.m_VoiceBtn.GetComponent<Image>().color = new Color(this.m_VoiceBtn.GetComponent<Image>().color.r, this.m_VoiceBtn.GetComponent<Image>().color.g, this.m_VoiceBtn.GetComponent<Image>().color.b, 0.37f);
					Text componentInChildren = this.m_VoiceBtn.GetComponentInChildren<Text>();
					componentInChildren.color = new Color(componentInChildren.color.r, componentInChildren.color.g, componentInChildren.color.b, 0.37f);
				}
				else
				{
					CUIEventScript component2 = this.m_VoiceBtn.GetComponent<CUIEventScript>();
					component2.enabled = true;
					this.m_VoiceBtn.GetComponent<Image>().color = new Color(this.m_VoiceBtn.GetComponent<Image>().color.r, this.m_VoiceBtn.GetComponent<Image>().color.g, this.m_VoiceBtn.GetComponent<Image>().color.b, 1f);
					Text componentInChildren2 = this.m_VoiceBtn.GetComponentInChildren<Text>();
					componentInChildren2.color = new Color(componentInChildren2.color.r, componentInChildren2.color.g, componentInChildren2.color.b, 1f);
				}
			}
		}

		public void HeroSelectTobattle()
		{
			try
			{
				this.m_bGetIsBattleSupportVoice = false;
				this.m_IsBattleSupportVoice = false;
				this.m_bInHeroSelectUI = false;
				this.m_SoundLevel_HeroSelect = null;
				this.m_VoiceBtn = null;
				MonoSingleton<VoiceSys>.GetInstance().ClosenSpeakers();
				this.CloseHeroSelctVoice();
			}
			catch (Exception ex)
			{
				DebugHelper.Assert(false, "Exception in HeroSelectTobattle {0} {1}", new object[]
				{
					ex.get_Message(),
					ex.get_StackTrace()
				});
			}
		}

		private void OnTimerBattle(int timeSeq)
		{
			this.CloseSoundInBattle();
		}

		public bool IsInVoiceRoom()
		{
			return this.m_bInRoom;
		}

		public void OpenSoundInBattle()
		{
			if (this.m_bInRoom && !this.m_bSoundInBattle)
			{
				this.m_bSoundInBattle = true;
				this.OpenMic();
			}
		}

		public void CloseSoundInBattle()
		{
			if (this.m_bSoundInBattle)
			{
				this.m_bSoundInBattle = false;
				this.CloseMic();
				this.m_nSoundBattleTimerID = 0;
			}
		}

		protected void PrintLog(string message, string filename = null, bool append = false)
		{
		}

		private static void WriteLogtoFile(string filename, string strinfo, bool append = false)
		{
			Debug.Log("commonForTest.WriteLogtoFile");
			string text = "sdcard/" + filename + ".txt";
			using (StreamWriter streamWriter = new StreamWriter(text, append))
			{
				streamWriter.WriteLine(strinfo);
			}
		}

		private void Update()
		{
			if (this.m_bInRoom && Time.time - this.m_UpdateTime >= 1f)
			{
				this.m_UpdateTime = Time.time;
				this.onGetMemberState();
			}
		}

		public void ClearVoiceStateData()
		{
			this.m_voiceStateList.Clear();
			this.lastSendVoiceState = 0;
			Singleton<CTimerManager>.get_instance().RemoveTimer(this.m_timer);
			this.m_timer = -1;
		}

		public void StartSyncVoiceStateTimer(int ms = 4000)
		{
			if (this.m_timer == -1)
			{
				this.m_timer = Singleton<CTimerManager>.get_instance().AddTimer(ms, 0, new CTimer.OnTimeUpHandler(this.UpdateSyncVoiceState));
			}
		}

		public void UpdateSyncVoiceState(int index)
		{
			if (this.lastSendVoiceState != this.curVoiceState)
			{
				VoiceStateNetCore.Send_Acnt_VoiceState(this.curVoiceState);
				this.lastSendVoiceState = this.curVoiceState;
			}
		}

		public void SetVoiceState(ulong uid, CS_VOICESTATE_TYPE state)
		{
			for (int i = 0; i < this.m_voiceStateList.get_Count(); i++)
			{
				VoiceSys.VoiceState voiceState = this.m_voiceStateList.get_Item(i);
				if (voiceState != null)
				{
					if (voiceState.uid == uid)
					{
						voiceState.state = state;
						return;
					}
				}
			}
			this.m_voiceStateList.Add(new VoiceSys.VoiceState(uid, state));
		}

		public void SyncReconnectData(CSDT_RECONN_GAMEINGINFO info)
		{
			if (info != null && info.stCampVoiceState != null)
			{
				for (int i = 0; i < (int)info.stCampVoiceState.bPlayerNum; i++)
				{
					CSDT_RECONN_PLAYERVOICEINFO cSDT_RECONN_PLAYERVOICEINFO = info.stCampVoiceState.astPlayerVoiceState[i];
					if (cSDT_RECONN_PLAYERVOICEINFO != null)
					{
						this.SetVoiceState(cSDT_RECONN_PLAYERVOICEINFO.ullUid, cSDT_RECONN_PLAYERVOICEINFO.bVoiceState);
					}
				}
			}
		}

		public CS_VOICESTATE_TYPE TryGetVoiceState(ulong uid)
		{
			for (int i = 0; i < this.m_voiceStateList.get_Count(); i++)
			{
				VoiceSys.VoiceState voiceState = this.m_voiceStateList.get_Item(i);
				if (voiceState != null)
				{
					if (voiceState.uid == uid)
					{
						return voiceState.state;
					}
				}
			}
			return 0;
		}
	}
}
