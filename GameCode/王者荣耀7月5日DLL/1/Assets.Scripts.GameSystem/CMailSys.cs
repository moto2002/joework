using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.UI;
using CSProtocol;
using ResData;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	[MessageHandlerClass]
	public class CMailSys : Singleton<CMailSys>
	{
		[Serializable]
		private struct stInvite
		{
			public ulong uid;

			public uint dwLogicWorldID;

			public uint time;

			public byte relationType;

			public byte inviteType;

			public byte processType;

			public string title;

			public byte bMapType;

			public uint dwMapId;

			public uint dwGameSvrEntity;

			public stInvite(string title, ulong uid, uint dwLogicWorldID, uint time, byte relationType, byte inviteType, byte processType, byte bMapType, uint dwMapId, uint dwGameSvrEntity)
			{
				this.title = title;
				this.uid = uid;
				this.dwLogicWorldID = dwLogicWorldID;
				this.time = time;
				this.relationType = relationType;
				this.inviteType = inviteType;
				this.processType = processType;
				this.bMapType = bMapType;
				this.dwMapId = dwMapId;
				this.dwGameSvrEntity = dwGameSvrEntity;
			}
		}

		public enum enMailSysFormWidget
		{
			GetAccessBtn,
			DeleteBtn,
			TitleText
		}

		public enum enMailWriteFormWidget
		{
			Mail_Title_Input,
			Mail_Content_Input,
			Mail_Send_Num_Text
		}

		public enum enProcessInviteType
		{
			Refuse,
			Accept,
			NoProcess
		}

		public const int c_driveingRefreshTimeDelta = 20;

		public const int c_refreshTimeDelta = 300;

		public const string FILE_FRIEND_INVITE_ARRAY = "SGAME_FILE_FRIEND_INVITE_ARRAY";

		public const int FILE_FRIEND_INVITE_ARRAY_LEN = 30;

		public static readonly string MAIL_FORM_PATH = "UGUI/Form/System/Mail/Form_Mail.prefab";

		public static readonly string MAIL_SYSTEM_FORM_PATH = "UGUI/Form/System/Mail/Form_SysMail.prefab";

		public static readonly string MAIL_WRITE_FORM_PATH = "UGUI/Form/System/Mail/Form_Mail_Write.prefab";

		private ListView<CMail> m_friMailList = new ListView<CMail>();

		private ListView<CMail> m_sysMailList = new ListView<CMail>();

		private ListView<CMail> m_msgMailList = new ListView<CMail>();

		private int m_friMailUnReadNum;

		private int m_sysMailUnReadNum;

		private int m_msgMailUnReadNum;

		private bool m_friAccessAll;

		private bool m_sysAccessAll;

		private ListView<CUseable> m_accessList = new ListView<CUseable>();

		private CMailView m_mailView = new CMailView();

		private CSysMailView m_sysMailView = new CSysMailView();

		private CMail m_mail;

		private uint m_mailFriVersion;

		private uint m_mailSysVersion;

		public int m_lastOpenTime;

		private List<CMailSys.stInvite> m_InviteList = new List<CMailSys.stInvite>();

		private bool bReadFile;

		public string offlineStr = "offline";

		public string onlineStr = "online";

		public string gamingStr = "gaming";

		public string inviteNoProcessStr = "gaming";

		public string inviteAcceptStr = "gaming";

		public string inviteRefuseStr = "gaming";

		public ListView<GuildMemInfo> m_mailGuildMemInfos = new ListLinqView<GuildMemInfo>();

		private int m_refreshGuildTimer = -1;

		public void LoadTxtStr()
		{
			this.offlineStr = Singleton<CTextManager>.get_instance().GetText("OfflineStr");
			this.onlineStr = Singleton<CTextManager>.get_instance().GetText("OnlineStr");
			this.gamingStr = Singleton<CTextManager>.get_instance().GetText("GamingStr");
			this.inviteNoProcessStr = Singleton<CTextManager>.get_instance().GetText("Invite_NoProcess");
			this.inviteAcceptStr = Singleton<CTextManager>.get_instance().GetText("Invite_Accept");
			this.inviteRefuseStr = Singleton<CTextManager>.get_instance().GetText("Invite_Refuse");
		}

		public void Clear()
		{
			this.m_mailView.SetUnReadNum(2, 0);
			this.m_mailView.SetUnReadNum(1, 0);
			this.m_mailView.SetUnReadNum(3, 0);
			this.m_friMailUnReadNum = 0;
			this.m_sysMailUnReadNum = 0;
			this.m_msgMailUnReadNum = 0;
			this.m_mailFriVersion = 0u;
			this.m_mailSysVersion = 0u;
			this.m_lastOpenTime = 0;
			this.m_friAccessAll = false;
			this.m_sysAccessAll = false;
			this.m_friMailList.Clear();
			this.m_sysMailList.Clear();
			this.m_msgMailList.Clear();
			this.m_InviteList.Clear();
			this.bReadFile = false;
			Singleton<CTimerManager>.get_instance().RemoveTimer(new CTimer.OnTimeUpHandler(this.RepeatReqMailList));
		}

		public override void Init()
		{
			base.Init();
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_OpenForm, new CUIEventManager.OnUIEventHandler(this.OnOpenMailForm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_CloseForm, new CUIEventManager.OnUIEventHandler(this.OnCloseMailForm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_FriendRead, new CUIEventManager.OnUIEventHandler(this.OnFriMailRead));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_SysRead, new CUIEventManager.OnUIEventHandler(this.OnSysMailRead));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_FriendCloseForm, new CUIEventManager.OnUIEventHandler(this.OnCloseFriMailFrom));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_SysCloseForm, new CUIEventManager.OnUIEventHandler(this.OnCloseSysMailFrom));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_FriendAccessAll, new CUIEventManager.OnUIEventHandler(this.OnFriAccessAll));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_SysAccess, new CUIEventManager.OnUIEventHandler(this.OnSysAccess));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_FriendAccess, new CUIEventManager.OnUIEventHandler(this.OnFriendAccess));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_TabFriend, new CUIEventManager.OnUIEventHandler(this.OnTabFriend));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_TabSystem, new CUIEventManager.OnUIEventHandler(this.OnTabSysem));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_TabMsgCenter, new CUIEventManager.OnUIEventHandler(this.OnTabMsgCenter));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_SysDelete, new CUIEventManager.OnUIEventHandler(this.OnSystemMailDelete));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_SysAccessAll, new CUIEventManager.OnUIEventHandler(this.OnSysAccessAll));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_ListElementEnable, new CUIEventManager.OnUIEventHandler(this.OnMailListElementEnable));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Invite_AddToMsgCenter, new CUIEventManager.OnUIEventHandler(this.OnAddToMsgCenter));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_MsgCenterDeleteAll, new CUIEventManager.OnUIEventHandler(this.OnMsgCenterDeleteAll));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_JumpForm, new CUIEventManager.OnUIEventHandler(this.OnJumpForm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_JumpUrl, new CUIEventManager.OnUIEventHandler(this.OnJumpUrl));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_Form_OnClose, new CUIEventManager.OnUIEventHandler(this.OnMailFormClose));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_Open_Mail_Write_Form, new CUIEventManager.OnUIEventHandler(this.OnOpenMailWriteForm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_Send_Guild_Mail, new CUIEventManager.OnUIEventHandler(this.OnSendGuildMail));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_FriendDelete, new CUIEventManager.OnUIEventHandler(this.OnFriendMailDelete));
			Singleton<EventRouter>.GetInstance().AddEventHandler("MAIL_GUILD_MEM_UPDATE", new Action(this.OnGUILD_MEM_UPDATE));
			Singleton<EventRouter>.GetInstance().AddEventHandler("Friend_LBS_User_Refresh", new Action(this.OnLBS_User_Refresh));
			Singleton<EventRouter>.GetInstance().AddEventHandler(EventID.Friend_Game_State_Change, new Action(this.OnFriendChg));
			Singleton<EventRouter>.GetInstance().AddEventHandler<CFriendModel.FriendType, ulong, uint, bool>("Chat_Friend_Online_Change", new Action<CFriendModel.FriendType, ulong, uint, bool>(this.OnFriendOnlineChg));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mail_Invite, new CUIEventManager.OnUIEventHandler(this.OnMail_Invite));
		}

		private void OnMail_Invite(CUIEvent uievent)
		{
			byte mapType = (byte)uievent.m_eventParams.heroId;
			uint weakGuideId = uievent.m_eventParams.weakGuideId;
			byte objSrc = (byte)uievent.m_eventParams.tag2;
			byte joinType = (byte)uievent.m_eventParams.tag3;
			uint tagUInt = uievent.m_eventParams.tagUInt;
			ulong commonUInt64Param = uievent.m_eventParams.commonUInt64Param1;
			uint taskId = uievent.m_eventParams.taskId;
			CInviteSystem.stInviteInfo inviteInfo = default(CInviteSystem.stInviteInfo);
			inviteInfo.playerUid = commonUInt64Param;
			inviteInfo.playerLogicWorldId = taskId;
			inviteInfo.joinType = joinType;
			inviteInfo.objSrc = (CInviteView.enInviteListTab)objSrc;
			inviteInfo.gameEntity = (int)tagUInt;
			ResDT_LevelCommonInfo pvpMapCommonInfo = CLevelCfgLogicManager.GetPvpMapCommonInfo(mapType, weakGuideId);
			if (pvpMapCommonInfo != null)
			{
				inviteInfo.maxTeamNum = (int)(pvpMapCommonInfo.bMaxAcntNum / 2);
			}
			DebugHelper.Assert(pvpMapCommonInfo != null, "----levelInfo is null... ");
			if (inviteInfo.joinType == 1)
			{
				CRoomSystem.ReqCreateRoomAndInvite(weakGuideId, mapType, inviteInfo);
			}
			else
			{
				CMatchingSystem.ReqCreateTeamAndInvite(weakGuideId, mapType, inviteInfo);
			}
		}

		private void OnFriendOnlineChg(CFriendModel.FriendType type, ulong ullUid, uint dwLogicWorldId, bool bOffline)
		{
			this.m_mailView.UpdateMailList(3, this.m_msgMailList);
		}

		private void OnFriendChg()
		{
			this.m_mailView.UpdateMailList(3, this.m_msgMailList);
		}

		private void OnLBS_User_Refresh()
		{
			this.m_mailView.UpdateMailList(3, this.m_msgMailList);
		}

		private void OnGUILD_MEM_UPDATE()
		{
			this.m_mailView.UpdateMailList(3, this.m_msgMailList);
		}

		public override void UnInit()
		{
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Mail_OpenForm, new CUIEventManager.OnUIEventHandler(this.OnOpenMailForm));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Mail_CloseForm, new CUIEventManager.OnUIEventHandler(this.OnCloseMailForm));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Mail_FriendRead, new CUIEventManager.OnUIEventHandler(this.OnFriMailRead));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Mail_SysRead, new CUIEventManager.OnUIEventHandler(this.OnSysMailRead));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Mail_FriendCloseForm, new CUIEventManager.OnUIEventHandler(this.OnCloseFriMailFrom));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Mail_SysCloseForm, new CUIEventManager.OnUIEventHandler(this.OnCloseSysMailFrom));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Mail_FriendAccessAll, new CUIEventManager.OnUIEventHandler(this.OnFriAccessAll));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Mail_SysAccess, new CUIEventManager.OnUIEventHandler(this.OnSysAccess));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Mail_FriendAccess, new CUIEventManager.OnUIEventHandler(this.OnFriendAccess));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Mail_TabFriend, new CUIEventManager.OnUIEventHandler(this.OnTabFriend));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Mail_TabSystem, new CUIEventManager.OnUIEventHandler(this.OnTabSysem));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Mail_TabMsgCenter, new CUIEventManager.OnUIEventHandler(this.OnTabMsgCenter));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Mail_SysDelete, new CUIEventManager.OnUIEventHandler(this.OnSystemMailDelete));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Mail_SysAccessAll, new CUIEventManager.OnUIEventHandler(this.OnSysAccessAll));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Mail_ListElementEnable, new CUIEventManager.OnUIEventHandler(this.OnMailListElementEnable));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Invite_AddToMsgCenter, new CUIEventManager.OnUIEventHandler(this.OnAddToMsgCenter));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Mail_MsgCenterDeleteAll, new CUIEventManager.OnUIEventHandler(this.OnMsgCenterDeleteAll));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Mail_JumpForm, new CUIEventManager.OnUIEventHandler(this.OnJumpForm));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Mail_JumpUrl, new CUIEventManager.OnUIEventHandler(this.OnJumpUrl));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Mail_Form_OnClose, new CUIEventManager.OnUIEventHandler(this.OnMailFormClose));
			base.UnInit();
		}

		public void InitLoginRsp(SCPKG_CMD_GAMELOGINRSP rsp)
		{
			Singleton<CTimerManager>.get_instance().AddTimer(300000, -1, new CTimer.OnTimeUpHandler(this.RepeatReqMailList));
		}

		private ListView<CMail> GetMailList(COM_MAIL_TYPE mailType)
		{
			if (mailType == 2)
			{
				return this.m_friMailList;
			}
			if (mailType == 1)
			{
				return this.m_sysMailList;
			}
			return this.m_msgMailList;
		}

		private void SortMailList(COM_MAIL_TYPE mailType)
		{
			ListView<CMail> listView = null;
			if (mailType == 2)
			{
				listView = this.m_friMailList;
			}
			else if (mailType == 1)
			{
				listView = this.m_sysMailList;
			}
			else if (mailType == 3)
			{
				listView = this.m_msgMailList;
			}
			if (listView != null)
			{
				listView.Sort(new Comparison<CMail>(this.Comparison));
			}
		}

		private int Comparison(CMail mailA, CMail mailB)
		{
			if (mailA.mailState != mailB.mailState)
			{
				return (mailA.mailState - mailB.mailState) * 10000;
			}
			return (int)(mailB.sendTime - mailA.sendTime);
		}

		private CMail MailPop(COM_MAIL_TYPE mailType)
		{
			ListView<CMail> listView = null;
			if (mailType == 2)
			{
				listView = this.m_friMailList;
			}
			else if (mailType == 1)
			{
				listView = this.m_sysMailList;
			}
			else if (mailType == 3)
			{
				listView = this.m_msgMailList;
			}
			CMail cMail = null;
			for (int i = 0; i < listView.get_Count(); i++)
			{
				if (cMail == null || cMail.sendTime > listView.get_Item(i).sendTime)
				{
					cMail = listView.get_Item(i);
				}
			}
			if (cMail != null)
			{
				listView.Remove(cMail);
			}
			return cMail;
		}

		private void OnOpenMailForm(CUIEvent uiEvent)
		{
			this.m_friAccessAll = false;
			this.m_sysAccessAll = false;
			this.m_mailView.Open(2);
			this.m_mailView.UpdateMailList(2, this.m_friMailList);
			this.m_mailView.UpdateMailList(1, this.m_sysMailList);
			this.m_mailView.UpdateMailList(3, this.m_msgMailList);
			int currentUTCTime = CRoleInfo.GetCurrentUTCTime();
			if (currentUTCTime - this.m_lastOpenTime > 20)
			{
				this.ReqMailList(1, 1, true, 0);
				this.m_lastOpenTime = currentUTCTime;
			}
			if (!this.bReadFile)
			{
				this.ReadFriendInviteFile();
				this.bReadFile = true;
			}
			if (Singleton<CGuildSystem>.GetInstance().IsInNormalGuild())
			{
				this.m_refreshGuildTimer = Singleton<CTimerManager>.get_instance().AddTimer(10000, 0, new CTimer.OnTimeUpHandler(this.OnRefreshGuildTimer));
			}
		}

		private void OnRefreshGuildTimer(int timersequence)
		{
			Debug.Log("---cmailsys OnRefreshGuildTimer...");
			Singleton<CInviteSystem>.get_instance().SendSendGetGuildMemberGameStateReqRaw(this.m_mailGuildMemInfos);
		}

		public void AddGuildMemInfo(GuildMemInfo info)
		{
			if (info == null)
			{
				return;
			}
			if (this.GetGuildMemInfoIndex(info.stBriefInfo.uulUid) == -1)
			{
				this.m_mailGuildMemInfos.Add(info);
			}
		}

		private int GetGuildMemInfoIndex(ulong uid)
		{
			for (int i = 0; i < this.m_mailGuildMemInfos.get_Count(); i++)
			{
				GuildMemInfo guildMemInfo = this.m_mailGuildMemInfos.get_Item(i);
				if (guildMemInfo.stBriefInfo.uulUid == uid)
				{
					return i;
				}
			}
			return -1;
		}

		private void OnCloseMailForm(CUIEvent uiEvent)
		{
			Singleton<CTimerManager>.get_instance().RemoveTimer(this.m_refreshGuildTimer);
			this.m_refreshGuildTimer = -1;
			this.m_friAccessAll = false;
			this.m_sysAccessAll = false;
			this.m_mailView.Close();
			this.m_mailGuildMemInfos.Clear();
		}

		private void OnOpenSysMailForm(CMail mail, COM_MAIL_TYPE mailType)
		{
			this.m_friAccessAll = false;
			this.m_sysAccessAll = false;
			CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(CMailSys.MAIL_SYSTEM_FORM_PATH, false, true);
			GameObject widget = cUIFormScript.GetWidget(0);
			GameObject widget2 = cUIFormScript.GetWidget(1);
			bool flag = mailType == 2 && mail.subType == 3;
			if (flag)
			{
				Text component = cUIFormScript.GetWidget(2).GetComponent<Text>();
				component.text = Singleton<CTextManager>.GetInstance().GetText("Mail_Guild_Mail");
				widget.CustomSetActive(false);
				widget2.CustomSetActive(true);
			}
			else
			{
				widget2.CustomSetActive(false);
			}
			this.m_sysMailView.Form = cUIFormScript;
			this.m_sysMailView.mailType = mailType;
			this.m_sysMailView.Mail = mail;
			this.m_mail = mail;
		}

		private void OnCloseFriMailFrom(CUIEvent uiEvent)
		{
		}

		private void OnCloseSysMailFrom(CUIEvent uiEvent)
		{
			Singleton<CUIManager>.GetInstance().CloseForm(CMailSys.MAIL_SYSTEM_FORM_PATH);
		}

		private void OnFriAccessAll(CUIEvent uiEvent)
		{
			this.m_accessList.Clear();
			int nearAccessIndex = this.GetNearAccessIndex(2);
			if (nearAccessIndex >= 0)
			{
				this.m_friAccessAll = true;
				this.ReqMailGetAccess(2, this.m_friMailList.get_Item(nearAccessIndex).mailIndex, 0);
			}
			else
			{
				this.m_friAccessAll = false;
			}
		}

		private void OnSysAccessAll(CUIEvent uiEvent)
		{
			int nearAccessIndex = this.GetNearAccessIndex(1);
			if (nearAccessIndex >= 0)
			{
				this.m_sysAccessAll = true;
				this.ReqMailGetAccess(1, this.m_sysMailList.get_Item(nearAccessIndex).mailIndex, 0);
			}
			else
			{
				this.m_sysAccessAll = false;
			}
		}

		private void OnSysAccess(CUIEvent uiEvent)
		{
			int mailIndex = this.m_mail.mailIndex;
			this.ReqMailGetAccess(1, mailIndex, 0);
		}

		private void OnFriendAccess(CUIEvent uiEvent)
		{
			int mailIndex = this.m_mail.mailIndex;
			this.ReqMailGetAccess(2, mailIndex, 0);
		}

		private void OnTabFriend(CUIEvent uiEvent)
		{
			this.m_mailView.CurMailType = 2;
			this.m_mailView.UpdateMailList(2, this.m_friMailList);
			this.m_mailView.SetActiveTab(0);
		}

		private void OnTabSysem(CUIEvent uiEvent)
		{
			this.m_mailView.CurMailType = 1;
			this.m_mailView.UpdateMailList(1, this.m_sysMailList);
			this.m_mailView.SetActiveTab(1);
		}

		private void OnTabMsgCenter(CUIEvent uiEvent)
		{
			this.m_mailView.CurMailType = 3;
			this.m_mailView.UpdateMailList(3, this.m_msgMailList);
			this.m_mailView.SetActiveTab(2);
			this.m_msgMailUnReadNum = 0;
			this.UpdateUnReadNum();
		}

		private void OnSysMailRead(CUIEvent uiEvent)
		{
			int srcWidgetIndexInBelongedList = uiEvent.m_srcWidgetIndexInBelongedList;
			if (srcWidgetIndexInBelongedList < 0 || srcWidgetIndexInBelongedList >= this.m_sysMailList.get_Count())
			{
				return;
			}
			this.OnMailRead(1, this.m_sysMailList.get_Item(srcWidgetIndexInBelongedList).mailIndex);
		}

		private void OnFriMailRead(CUIEvent uiEvent)
		{
			int srcWidgetIndexInBelongedList = uiEvent.m_srcWidgetIndexInBelongedList;
			this.OnMailRead(2, this.m_friMailList.get_Item(srcWidgetIndexInBelongedList).mailIndex);
		}

		private void OnSystemMailDelete(CUIEvent uiEvent)
		{
			if (this.m_sysMailList != null && this.m_sysMailList.get_Count() > 0)
			{
				int canBeDeleted = this.m_sysMailList.get_Item(0).CanBeDeleted;
				if (canBeDeleted == 0)
				{
					this.ReqDeleteMail(1, this.m_sysMailList.get_Item(0).mailIndex);
				}
				else
				{
					Singleton<CUIManager>.get_instance().OpenTips(Singleton<CTextManager>.GetInstance().GetText(canBeDeleted.ToString()), false, 1.5f, null, new object[0]);
				}
			}
		}

		private void OnFriendMailDelete(CUIEvent uiEvent)
		{
			Singleton<CUIManager>.GetInstance().CloseForm(CMailSys.MAIL_SYSTEM_FORM_PATH);
			if (this.m_friMailList != null && this.m_friMailList.get_Count() > 0)
			{
				CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CMailSys.MAIL_FORM_PATH);
				if (form == null)
				{
					return;
				}
				int friendMailListSelectedIndex = this.m_mailView.GetFriendMailListSelectedIndex();
				if (friendMailListSelectedIndex < 0 || friendMailListSelectedIndex >= this.m_friMailList.get_Count())
				{
					return;
				}
				this.ReqDeleteMail(2, this.m_friMailList.get_Item(friendMailListSelectedIndex).mailIndex);
			}
		}

		private void OnMailListElementEnable(CUIEvent uiEvent)
		{
			COM_MAIL_TYPE curMailType = this.m_mailView.CurMailType;
			int srcWidgetIndexInBelongedList = uiEvent.m_srcWidgetIndexInBelongedList;
			ListView<CMail> mailList = this.GetMailList(curMailType);
			if (srcWidgetIndexInBelongedList >= 0 && srcWidgetIndexInBelongedList < mailList.get_Count())
			{
				this.m_mailView.UpdateListElenment(uiEvent.m_srcWidget, mailList.get_Item(srcWidgetIndexInBelongedList));
			}
		}

		private int GetNearAccessIndex(COM_MAIL_TYPE mailType)
		{
			int result = -1;
			if (mailType == 2)
			{
				if (this.m_friMailList != null)
				{
					for (int i = 0; i < this.m_friMailList.get_Count(); i++)
					{
						if (!this.m_friMailList.get_Item(i).isAccess && this.m_friMailList.get_Item(i).subType == 1)
						{
							result = i;
							break;
						}
					}
				}
			}
			else if (mailType == 1 && this.m_sysMailList != null)
			{
				for (int j = 0; j < this.m_sysMailList.get_Count(); j++)
				{
					if (!this.m_sysMailList.get_Item(j).isAccess && this.m_sysMailList.get_Item(j).subType == 2)
					{
						result = j;
						break;
					}
				}
			}
			return result;
		}

		private void OnMailRead(COM_MAIL_TYPE mailType, int index)
		{
			CMail mail = this.GetMail(mailType, index);
			DebugHelper.Assert(mail != null, "mail cannot be find. mailType[{0}] , index[{1}]", new object[]
			{
				mailType,
				index
			});
			if (mail == null)
			{
				return;
			}
			if (mail.isReceive)
			{
				this.OnOpenSysMailForm(mail, mailType);
			}
			else
			{
				this.ReqReadMail(mailType, index);
				this.OnOpenSysMailForm(mail, mailType);
			}
		}

		private void OnAddToMsgCenter(CUIEvent uiEvent)
		{
			COMDT_FRIEND_INFO friendByName = Singleton<CFriendContoller>.get_instance().model.getFriendByName(uiEvent.m_eventParams.tagStr, CFriendModel.FriendType.GameFriend);
			if (friendByName == null)
			{
				friendByName = Singleton<CFriendContoller>.get_instance().model.getFriendByName(uiEvent.m_eventParams.tagStr, CFriendModel.FriendType.SNS);
			}
			if (friendByName != null)
			{
			}
		}

		private void OnMsgCenterDeleteAll(CUIEvent uiEvent)
		{
			this.m_InviteList.Clear();
			this.WriteFriendInviteFile();
			this.m_msgMailList = this.ConvertToMailList();
			this.m_mailView.UpdateMailList(3, this.m_msgMailList);
			this.m_msgMailUnReadNum = 0;
			this.UpdateUnReadNum();
		}

		private void OnJumpForm(CUIEvent uiEvent)
		{
			CUICommonSystem.JumpForm(uiEvent.m_eventParams.tag, 0, 0);
		}

		private void OnJumpUrl(CUIEvent uiEvent)
		{
			CUICommonSystem.OpenUrl(uiEvent.m_eventParams.tagStr, true, 0);
		}

		private void OnMailFormClose(CUIEvent uiEvent)
		{
			this.m_mailView.OnClose();
		}

		private void OnOpenMailWriteForm(CUIEvent uiEvent)
		{
			CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(CMailSys.MAIL_WRITE_FORM_PATH, false, true);
			if (cUIFormScript == null)
			{
				return;
			}
			Text component = cUIFormScript.GetWidget(2).GetComponent<Text>();
			uint sendGuildMailCnt = (uint)CGuildHelper.GetSendGuildMailCnt();
			uint sendGuildMailLimit = CGuildHelper.GetSendGuildMailLimit();
			component.text = Singleton<CTextManager>.GetInstance().GetText("Mail_Today_Send_Num", new string[]
			{
				sendGuildMailCnt.ToString(),
				sendGuildMailLimit.ToString()
			});
		}

		private void OnSendGuildMail(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CMailSys.MAIL_WRITE_FORM_PATH);
			if (form == null)
			{
				return;
			}
			InputField component = form.GetWidget(1).GetComponent<InputField>();
			string text = CUIUtility.RemoveEmoji(component.text).Trim();
			if (string.IsNullOrEmpty(text))
			{
				Singleton<CUIManager>.GetInstance().OpenTips("Mail_Content_Cannot_Be_Empty", true, 1.5f, null, new object[0]);
				return;
			}
			InputField component2 = form.GetWidget(0).GetComponent<InputField>();
			string text2 = CUIUtility.RemoveEmoji(component2.text).Trim();
			if (string.IsNullOrEmpty(text2) && Singleton<CGuildSystem>.GetInstance().IsInNormalGuild())
			{
				text2 = Singleton<CTextManager>.GetInstance().GetText("Mail_Default_Guild_Mail_Title", new string[]
				{
					CGuildHelper.GetGuildName()
				});
			}
			Singleton<CGuildInfoController>.GetInstance().ReqSendGuildMail(text2, text);
		}

		public void AddFriendInviteMail(CUIEvent uiEvent, CMailSys.enProcessInviteType processType)
		{
			this.AddFriendInviteMail(uiEvent.m_eventParams.tagStr, uiEvent.m_eventParams.commonUInt64Param1, uiEvent.m_eventParams.taskId, (uint)CRoleInfo.GetCurrentUTCTime(), (byte)uiEvent.m_eventParams.tag2, (byte)uiEvent.m_eventParams.tag3, (byte)processType, (byte)uiEvent.m_eventParams.heroId, uiEvent.m_eventParams.weakGuideId, uiEvent.m_eventParams.tagUInt);
		}

		private void AddFriendInviteMail(string title, ulong uid, uint dwLogicWorldID, uint time, byte relationType, byte bInviteType, byte processType, byte bMapType, uint dwMapId, uint dwGameSvrEntity)
		{
			this.m_InviteList.Add(new CMailSys.stInvite(title, uid, dwLogicWorldID, time, relationType, bInviteType, processType, bMapType, dwMapId, dwGameSvrEntity));
			if (this.m_InviteList.get_Count() > 30)
			{
				this.m_InviteList.RemoveAt(0);
			}
			this.WriteFriendInviteFile();
			this.m_msgMailList = this.ConvertToMailList();
			this.m_mailView.UpdateMailList(3, this.m_msgMailList);
			this.m_msgMailUnReadNum = 1;
			this.UpdateUnReadNum();
		}

		private void WriteFriendInviteFile()
		{
			string cachePath = CFileManager.GetCachePath(string.Format("{0}_{1}", "SGAME_FILE_FRIEND_INVITE_ARRAY", Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID));
			byte[] bytes = Utility.ObjectToBytes(this.m_InviteList);
			Utility.WriteFile(cachePath, bytes);
		}

		private ListView<CMail> ConvertToMailList()
		{
			ListView<CMail> listView = new ListView<CMail>();
			for (int i = 0; i < this.m_InviteList.get_Count(); i++)
			{
				CMailSys.stInvite stInvite = this.m_InviteList.get_Item(i);
				listView.Add(new CMail
				{
					uid = stInvite.uid,
					dwLogicWorldID = stInvite.dwLogicWorldID,
					mailType = 3,
					sendTime = stInvite.time,
					subject = stInvite.title,
					processType = stInvite.processType,
					relationType = stInvite.relationType,
					inviteType = stInvite.inviteType,
					bMapType = stInvite.bMapType,
					dwMapId = stInvite.dwMapId,
					dwGameSvrEntity = stInvite.dwGameSvrEntity
				});
			}
			return listView;
		}

		private void ReadFriendInviteFile()
		{
			string cachePath = CFileManager.GetCachePath(string.Format("{0}_{1}", "SGAME_FILE_FRIEND_INVITE_ARRAY", Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID));
			byte[] array = Utility.ReadFile(cachePath);
			List<CMailSys.stInvite> list = null;
			try
			{
				if (array != null)
				{
					list = (Utility.BytesToObject(array) as List<CMailSys.stInvite>);
				}
			}
			catch (Exception var_3_49)
			{
			}
			if (list != null)
			{
				this.m_InviteList = list;
			}
			this.m_msgMailList = this.ConvertToMailList();
			this.m_mailView.UpdateMailList(3, this.m_msgMailList);
		}

		private void RepeatReqMailList(int timerSequence)
		{
			if (!Singleton<BattleLogic>.get_instance().isRuning && Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo() != null)
			{
				this.ReqMailList(1, 1, false, 0);
			}
		}

		public CMail GetMail(COM_MAIL_TYPE mailType, int index)
		{
			ListView<CMail> listView = null;
			if (mailType == 2)
			{
				listView = this.m_friMailList;
			}
			else if (mailType == 1)
			{
				listView = this.m_sysMailList;
			}
			else if (mailType == 3)
			{
				listView = this.m_msgMailList;
			}
			if (listView != null)
			{
				for (int i = 0; i < listView.get_Count(); i++)
				{
					if (listView.get_Item(i).mailIndex == index)
					{
						return listView.get_Item(i);
					}
				}
			}
			return null;
		}

		public void AddMail(COM_MAIL_TYPE mailType, CMail mail)
		{
			ListView<CMail> listView = null;
			if (mailType == 2)
			{
				listView = this.m_friMailList;
			}
			else if (mailType == 1)
			{
				listView = this.m_sysMailList;
			}
			else if (mailType == 3)
			{
				listView = this.m_msgMailList;
			}
			for (int i = 0; i < listView.get_Count(); i++)
			{
				if (mail.mailIndex == listView.get_Item(i).mailIndex)
				{
					listView.RemoveAt(i);
					break;
				}
			}
			listView.Add(mail);
		}

		public bool DeleteMail(COM_MAIL_TYPE mailType, int index)
		{
			ListView<CMail> listView = null;
			if (mailType == 2)
			{
				listView = this.m_friMailList;
			}
			else if (mailType == 1)
			{
				listView = this.m_sysMailList;
			}
			else if (mailType == 3)
			{
				listView = this.m_msgMailList;
			}
			for (int i = 0; i < listView.get_Count(); i++)
			{
				if (listView.get_Item(i).mailIndex == index)
				{
					listView.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		public void ReqMailList(COM_MAIL_TYPE mailType, MAIL_OPT_MAILLISTTYPE optType, bool isDiff = false, int startPos = 0)
		{
			uint dwVersion = 0u;
			if (mailType == 2)
			{
				dwVersion = this.m_mailFriVersion;
			}
			else if (mailType == 1)
			{
				dwVersion = this.m_mailSysVersion;
			}
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1400u);
			cSPkg.stPkgData.get_stMailOptReq().bOptType = 1;
			cSPkg.stPkgData.get_stMailOptReq().stOptInfo.set_stGetMailList(new CSDT_MAILOPTREQ_GETMAILLIST());
			cSPkg.stPkgData.get_stMailOptReq().stOptInfo.get_stGetMailList().bReqType = optType;
			cSPkg.stPkgData.get_stMailOptReq().stOptInfo.get_stGetMailList().dwVersion = dwVersion;
			cSPkg.stPkgData.get_stMailOptReq().stOptInfo.get_stGetMailList().bIsDiff = ((!isDiff) ? 0 : 1);
			cSPkg.stPkgData.get_stMailOptReq().stOptInfo.get_stGetMailList().bStartPos = (byte)startPos;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		public void ReqSendMail(COM_MAIL_TYPE mailType)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1400u);
			cSPkg.stPkgData.get_stMailOptReq().bOptType = 2;
			cSPkg.stPkgData.get_stMailOptReq().stOptInfo.set_stSendMail(new CSDT_MAILOPTREQ_SENDMAIL());
			cSPkg.stPkgData.get_stMailOptReq().stOptInfo.get_stSendMail().bMailType = mailType;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		public void ReqReadMail(COM_MAIL_TYPE mailType, int index)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1400u);
			cSPkg.stPkgData.get_stMailOptReq().bOptType = 3;
			cSPkg.stPkgData.get_stMailOptReq().stOptInfo.set_stReadMail(new CSDT_MAILOPTREQ_READMAIL());
			cSPkg.stPkgData.get_stMailOptReq().stOptInfo.get_stReadMail().bMailType = mailType;
			cSPkg.stPkgData.get_stMailOptReq().stOptInfo.get_stReadMail().iMailIndex = index;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		public void ReqDeleteMail(COM_MAIL_TYPE mailType, int index)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1400u);
			cSPkg.stPkgData.get_stMailOptReq().bOptType = 4;
			cSPkg.stPkgData.get_stMailOptReq().stOptInfo.set_stDelMail(new CSDT_MAILOPTREQ_DELMAIL());
			cSPkg.stPkgData.get_stMailOptReq().stOptInfo.get_stDelMail().bMailType = mailType;
			cSPkg.stPkgData.get_stMailOptReq().stOptInfo.get_stDelMail().iMailIndex = index;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		public void ReqMailGetAccess(COM_MAIL_TYPE mailType, int index, int getAll = 0)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1400u);
			cSPkg.stPkgData.get_stMailOptReq().bOptType = 5;
			cSPkg.stPkgData.get_stMailOptReq().stOptInfo.set_stGetAccess(new CSDT_MAILOPTREQ_GETACCESS());
			cSPkg.stPkgData.get_stMailOptReq().stOptInfo.get_stGetAccess().bMailType = mailType;
			cSPkg.stPkgData.get_stMailOptReq().stOptInfo.get_stGetAccess().iMailIndex = index;
			cSPkg.stPkgData.get_stMailOptReq().stOptInfo.get_stGetAccess().bGetAll = (byte)getAll;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		public void OnMailGetListRes(CSPkg msg)
		{
			ListView<CMail> listView = null;
			COM_MAIL_TYPE bMailType = msg.stPkgData.get_stMailOptRes().stOptInfo.get_stGetMailList().bMailType;
			int num = 0;
			if (bMailType == 2)
			{
				listView = this.m_friMailList;
				num = 30;
			}
			else if (bMailType == 1)
			{
				listView = this.m_sysMailList;
				num = 100;
			}
			if (msg.stPkgData.get_stMailOptRes().stOptInfo.get_stGetMailList().bIsDiff == 0)
			{
				listView.Clear();
			}
			for (int i = 0; i < (int)msg.stPkgData.get_stMailOptRes().stOptInfo.get_stGetMailList().bCnt; i++)
			{
				this.AddMail(bMailType, new CMail(bMailType, ref msg.stPkgData.get_stMailOptRes().stOptInfo.get_stGetMailList().astMailInfo[i]));
			}
			while (listView.get_Count() > num)
			{
				this.MailPop(bMailType);
			}
			if (bMailType == 2)
			{
				this.m_mailFriVersion = msg.stPkgData.get_stMailOptRes().stOptInfo.get_stGetMailList().dwVersion;
			}
			else if (bMailType == 1)
			{
				this.m_mailSysVersion = msg.stPkgData.get_stMailOptRes().stOptInfo.get_stGetMailList().dwVersion;
			}
			this.SortMailList(bMailType);
			this.UpdateUnReadNum();
			this.m_mailView.UpdateMailList(bMailType, listView);
		}

		public void OnMailSendMailRes(CSPkg msg)
		{
			COM_MAIL_TYPE bMailType = msg.stPkgData.get_stMailOptRes().stOptInfo.get_stSendMail().bMailType;
			switch (msg.stPkgData.get_stMailOptRes().stOptInfo.get_stSendMail().bResult)
			{
			case 3:
			{
				MAIL_OPT_MAILLISTTYPE optType = 5;
				if (bMailType == 2)
				{
					optType = 5;
				}
				else if (bMailType == 1)
				{
					optType = 4;
				}
				this.ReqMailList(bMailType, optType, false, 0);
				break;
			}
			case 4:
				Singleton<CUIManager>.get_instance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("MailOp_Packagefull"), false, 1.5f, null, new object[0]);
				this.m_sysAccessAll = false;
				break;
			case 5:
				Singleton<CUIManager>.get_instance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("MailOp_Packagefull"), false, 1.5f, null, new object[0]);
				this.m_sysAccessAll = false;
				break;
			case 6:
				Singleton<CUIManager>.get_instance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("MailOp_ListGeting"), false, 1.5f, null, new object[0]);
				break;
			case 8:
				if (this.m_friAccessAll)
				{
					Singleton<CUIManager>.get_instance().OpenAwardTip(LinqS.ToArray<CUseable>(this.m_accessList), null, false, enUIEventID.None, false, false, "Form_Award");
					this.m_accessList.Clear();
					this.m_friAccessAll = false;
					Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
					Singleton<CUIManager>.get_instance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("Mailop_GetHeartLimit"), false, 1.5f, null, new object[0]);
				}
				else
				{
					Singleton<CUIManager>.get_instance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("Mailop_GetHeartLimit"), false, 1.5f, null, new object[0]);
				}
				break;
			}
		}

		public void OnMailReadMailRes(CSPkg msg)
		{
			COM_MAIL_TYPE bMailType = msg.stPkgData.get_stMailOptRes().stOptInfo.get_stReadMail().bMailType;
			int iMailIndex = msg.stPkgData.get_stMailOptRes().stOptInfo.get_stReadMail().iMailIndex;
			CMail mail = this.GetMail(bMailType, iMailIndex);
			DebugHelper.Assert(mail != null, "mail cannot be find. mailType[{0}] , index[{1}]", new object[]
			{
				bMailType,
				iMailIndex
			});
			if (mail == null)
			{
				return;
			}
			mail.Read(msg.stPkgData.get_stMailOptRes().stOptInfo.get_stReadMail());
			this.m_sysMailView.Mail = mail;
			this.m_mail = mail;
			this.SortMailList(bMailType);
			if (bMailType == 2)
			{
				this.m_mailView.UpdateMailList(bMailType, this.m_friMailList);
			}
			else if (bMailType == 1)
			{
				this.m_mailView.UpdateMailList(bMailType, this.m_sysMailList);
			}
			this.UpdateUnReadNum();
		}

		public void OnMailDeleteRes(CSPkg msg)
		{
			COM_MAIL_TYPE bMailType = msg.stPkgData.get_stMailOptRes().stOptInfo.get_stDelMail().bMailType;
			this.DeleteMail(bMailType, msg.stPkgData.get_stMailOptRes().stOptInfo.get_stDelMail().iMailIndex);
			ListView<CMail> mailList = null;
			if (bMailType == 2)
			{
				mailList = this.m_friMailList;
			}
			else if (bMailType == 1)
			{
				mailList = this.m_sysMailList;
			}
			this.UpdateUnReadNum();
			this.m_mailView.UpdateMailList(bMailType, mailList);
		}

		public void OnMailGetAccess(CSPkg msg)
		{
			switch (msg.stPkgData.get_stMailOptRes().stOptInfo.get_stGetAccess().bResult)
			{
			case 1:
			{
				CSDT_MAILOPTRES_GETACCESS stGetAccess = msg.stPkgData.get_stMailOptRes().stOptInfo.get_stGetAccess();
				COM_MAIL_TYPE bMailType = stGetAccess.bMailType;
				int iMailIndex = stGetAccess.iMailIndex;
				CMail mail = this.GetMail(bMailType, iMailIndex);
				if (mail != null)
				{
					if (bMailType == 2)
					{
						CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo();
						if (masterRoleInfo != null)
						{
							masterRoleInfo.getFriendCoinCnt++;
						}
					}
					mail.isAccess = true;
					CUseable[] array = LinqS.ToArray<CUseable>(CMailSys.StAccessToUseable(stGetAccess.astAccess, stGetAccess.astAccessFrom, (int)stGetAccess.bAccessCnt));
					this.OnCloseSysMailFrom(null);
					enUIEventID eventID = enUIEventID.None;
					if (this.m_friAccessAll)
					{
						CMailSys.ConnectVirtualList(ref this.m_accessList, array);
						int nearAccessIndex = this.GetNearAccessIndex(2);
						if (nearAccessIndex >= 0)
						{
							this.ReqMailGetAccess(2, this.m_friMailList.get_Item(nearAccessIndex).mailIndex, 0);
						}
						else
						{
							Singleton<CUIManager>.get_instance().OpenAwardTip(LinqS.ToArray<CUseable>(this.m_accessList), null, false, enUIEventID.None, false, false, "Form_Award");
							this.m_accessList.Clear();
							this.m_friAccessAll = false;
							Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
						}
						this.UpdateUnReadNum();
						return;
					}
					if (this.m_sysAccessAll)
					{
						eventID = enUIEventID.Mail_SysAccessAll;
					}
					Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
					Singleton<CUIManager>.get_instance().OpenAwardTip(array, null, false, eventID, true, false, "Form_Award");
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] != null)
						{
							if (array[i].m_type == 4)
							{
								CHeroItem cHeroItem = array[i] as CHeroItem;
								if (cHeroItem != null && !(cHeroItem is CExpHeroItem))
								{
									CUICommonSystem.ShowNewHeroOrSkin(cHeroItem.m_baseID, 0u, enUIEventID.None, true, 5, false, null, enFormPriority.Priority1, 0u, 0);
								}
							}
							else if (array[i].m_type == 7)
							{
								CHeroSkin cHeroSkin = array[i] as CHeroSkin;
								if (cHeroSkin != null && !(cHeroSkin is CExpHeroSkin))
								{
									CUICommonSystem.ShowNewHeroOrSkin(cHeroSkin.m_heroId, cHeroSkin.m_skinId, enUIEventID.None, true, 10, false, null, enFormPriority.Priority1, 0u, 0);
								}
							}
							if (array[i].m_type == 2)
							{
								if (array[i].ExtraFromType == 1)
								{
									int extraFromData = array[i].ExtraFromData;
									CUICommonSystem.ShowNewHeroOrSkin((uint)extraFromData, 0u, enUIEventID.None, true, 5, true, null, enFormPriority.Priority1, (uint)array[i].m_stackCount, 0);
								}
								else if (array[i].ExtraFromType == 2)
								{
									int extraFromData2 = array[i].ExtraFromData;
									CUICommonSystem.ShowNewHeroOrSkin(0u, (uint)extraFromData2, enUIEventID.None, true, 10, true, null, enFormPriority.Priority1, (uint)array[i].m_stackCount, 0);
								}
							}
						}
					}
				}
				break;
			}
			case 4:
				Singleton<CUIManager>.get_instance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("MailOp_PackageClean"), false, 1.5f, null, new object[0]);
				this.m_sysAccessAll = false;
				break;
			case 5:
				Singleton<CUIManager>.get_instance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("MailOp_Packagefull"), false, 1.5f, null, new object[0]);
				this.m_sysAccessAll = false;
				break;
			case 8:
				if (this.m_friAccessAll)
				{
					if (this.m_accessList.get_Count() > 0)
					{
						Singleton<CUIManager>.get_instance().OpenAwardTip(LinqS.ToArray<CUseable>(this.m_accessList), null, false, enUIEventID.None, false, false, "Form_Award");
					}
					this.m_accessList.Clear();
					this.m_friAccessAll = false;
					Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
					Singleton<CUIManager>.get_instance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("Mailop_GetHeartLimit"), false, 1.5f, null, new object[0]);
				}
				else
				{
					Singleton<CUIManager>.get_instance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("Mailop_GetHeartLimit"), false, 1.5f, null, new object[0]);
					Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
				}
				break;
			}
			this.UpdateUnReadNum();
		}

		public void OnMailUnReadRes(CSPkg msg)
		{
			this.ReqMailList(2, 5, false, 0);
			this.ReqMailList(1, 4, false, 0);
		}

		public void UpdateUnReadNum()
		{
			this.m_friMailUnReadNum = 0;
			if (this.m_friMailList != null)
			{
				for (int i = 0; i < this.m_friMailList.get_Count(); i++)
				{
					if (this.m_friMailList.get_Item(i).mailState == 1)
					{
						this.m_friMailUnReadNum++;
					}
				}
			}
			this.m_sysMailUnReadNum = 0;
			if (this.m_sysMailList != null)
			{
				for (int j = 0; j < this.m_sysMailList.get_Count(); j++)
				{
					if (this.m_sysMailList.get_Item(j).mailState == 1)
					{
						this.m_sysMailUnReadNum++;
					}
				}
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo();
			if (masterRoleInfo != null && !masterRoleInfo.bCanRecvCoin)
			{
				this.m_mailView.SetUnReadNum(2, 0);
			}
			else
			{
				this.m_mailView.SetUnReadNum(2, this.m_friMailUnReadNum);
			}
			this.m_mailView.SetUnReadNum(1, this.m_sysMailUnReadNum);
			this.m_mailView.SetUnReadNum(3, this.m_msgMailUnReadNum);
			Singleton<EventRouter>.get_instance().BroadCastEvent("MailUnReadNumUpdate");
		}

		public int GetUnReadMailCount(bool bIgnoreFriend = false)
		{
			if (!bIgnoreFriend)
			{
				return this.m_friMailUnReadNum + this.m_sysMailUnReadNum + this.m_msgMailUnReadNum;
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo();
			if (masterRoleInfo.bCanRecvCoin)
			{
				return this.m_friMailUnReadNum + this.m_sysMailUnReadNum + this.m_msgMailUnReadNum;
			}
			return this.m_sysMailUnReadNum + this.m_msgMailUnReadNum;
		}

		[MessageHandler(1401)]
		public static void OnMailRes(CSPkg msg)
		{
			switch (msg.stPkgData.get_stMailOptRes().bOptType)
			{
			case 1:
				Singleton<CMailSys>.get_instance().OnMailGetListRes(msg);
				break;
			case 2:
				Singleton<CMailSys>.get_instance().OnMailSendMailRes(msg);
				break;
			case 3:
				Singleton<CMailSys>.get_instance().OnMailReadMailRes(msg);
				break;
			case 4:
				Singleton<CMailSys>.get_instance().OnMailDeleteRes(msg);
				break;
			case 5:
				Singleton<CMailSys>.get_instance().OnMailGetAccess(msg);
				break;
			case 6:
				Singleton<CMailSys>.get_instance().OnMailUnReadRes(msg);
				break;
			}
		}

		public static ListView<CUseable> StAccessToUseable(COMDT_MAILACCESS[] stAccess, CSDT_MAILACCESS_FROM[] stAccessFrom, int count)
		{
			ListView<CUseable> listView = new ListView<CUseable>();
			for (int i = 0; i < count; i++)
			{
				COM_MAILACCESS_TYPE bAccessType = stAccess[i].bAccessType;
				CUseable cUseable = null;
				if (bAccessType == 3)
				{
					cUseable = CUseableManager.CreateVirtualUseable(enVirtualItemType.enHeart, (int)stAccess[i].stAccessInfo.get_stHeart().dwHeart);
				}
				else if (bAccessType == 4)
				{
					cUseable = CUseableManager.CreateVirtualUseable(enVirtualItemType.enGoldCoin, (int)stAccess[i].stAccessInfo.get_stRongYu().dwRongYuPoint);
				}
				else if (bAccessType == 11)
				{
					cUseable = CUseableManager.CreateVirtualUseable(enVirtualItemType.enMentorPoint, (int)stAccess[i].stAccessInfo.get_stMasterPoint().dwPoint);
				}
				else if (bAccessType == 2)
				{
					if (stAccess[i].stAccessInfo.get_stMoney().bType == 1)
					{
						cUseable = CUseableManager.CreateVirtualUseable(enVirtualItemType.enNoUsed, (int)stAccess[i].stAccessInfo.get_stMoney().dwMoney);
					}
					else if (stAccess[i].stAccessInfo.get_stMoney().bType == 7)
					{
						cUseable = CUseableManager.CreateVirtualUseable(enVirtualItemType.enDiamond, (int)stAccess[i].stAccessInfo.get_stMoney().dwMoney);
					}
					else if (stAccess[i].stAccessInfo.get_stMoney().bType == 3)
					{
						cUseable = CUseableManager.CreateVirtualUseable(enVirtualItemType.enArenaCoin, (int)stAccess[i].stAccessInfo.get_stMoney().dwMoney);
					}
					else if (stAccess[i].stAccessInfo.get_stMoney().bType == 4)
					{
						cUseable = CUseableManager.CreateVirtualUseable(enVirtualItemType.enBurningCoin, (int)stAccess[i].stAccessInfo.get_stMoney().dwMoney);
					}
					else if (stAccess[i].stAccessInfo.get_stMoney().bType == 5)
					{
						cUseable = CUseableManager.CreateVirtualUseable(enVirtualItemType.enGuildConstruct, (int)stAccess[i].stAccessInfo.get_stMoney().dwMoney);
					}
					else if (stAccess[i].stAccessInfo.get_stMoney().bType == 6)
					{
						cUseable = CUseableManager.CreateVirtualUseable(enVirtualItemType.enSymbolCoin, (int)stAccess[i].stAccessInfo.get_stMoney().dwMoney);
					}
					else if (stAccess[i].stAccessInfo.get_stMoney().bType == 2)
					{
						cUseable = CUseableManager.CreateVirtualUseable(enVirtualItemType.enDianQuan, (int)stAccess[i].stAccessInfo.get_stMoney().dwMoney);
					}
				}
				else if (bAccessType == 1)
				{
					COM_ITEM_TYPE wPropType = stAccess[i].stAccessInfo.get_stProp().wPropType;
					cUseable = CUseableManager.CreateUseable(wPropType, 0uL, stAccess[i].stAccessInfo.get_stProp().dwPropID, stAccess[i].stAccessInfo.get_stProp().iPropNum, 0);
				}
				else if (bAccessType == 5)
				{
					cUseable = CUseableManager.CreateVirtualUseable(enVirtualItemType.enExp, (int)stAccess[i].stAccessInfo.get_stExp().dwExp);
				}
				else if (bAccessType == 6)
				{
					cUseable = CUseableManager.CreateUseable(4, 0uL, stAccess[i].stAccessInfo.get_stHero().dwHeroID, 1, 0);
				}
				else if (bAccessType == 7)
				{
					cUseable = CUseableManager.CreateUseable(7, 0uL, stAccess[i].stAccessInfo.get_stPiFu().dwSkinID, 1, 0);
				}
				else if (bAccessType == 8)
				{
					cUseable = CUseableManager.CreateExpUseable(4, 0uL, stAccess[i].stAccessInfo.get_stExpHero().dwHeroID, stAccess[i].stAccessInfo.get_stExpHero().dwExpDays, 1, 0);
				}
				else if (bAccessType == 9)
				{
					cUseable = CUseableManager.CreateExpUseable(7, 0uL, stAccess[i].stAccessInfo.get_stExpSkin().dwSkinID, stAccess[i].stAccessInfo.get_stExpSkin().dwExpDays, 1, 0);
				}
				else if (bAccessType == 10)
				{
					cUseable = CUseableManager.CreateUseable(8, 0uL, stAccess[i].stAccessInfo.get_stHeadImg().dwHeadImgID, 0, 0);
				}
				if (cUseable != null)
				{
					if (stAccessFrom != null && i < stAccessFrom.Length)
					{
						CSDT_MAILACCESS_FROM cSDT_MAILACCESS_FROM = stAccessFrom[i];
						if (cSDT_MAILACCESS_FROM != null)
						{
							if (cSDT_MAILACCESS_FROM.bFromType == 1)
							{
								cUseable.ExtraFromType = (int)cSDT_MAILACCESS_FROM.bFromType;
								cUseable.ExtraFromData = (int)cSDT_MAILACCESS_FROM.stFromInfo.get_stHeroInfo().dwHeroID;
							}
							else if (cSDT_MAILACCESS_FROM.bFromType == 2)
							{
								cUseable.ExtraFromType = (int)cSDT_MAILACCESS_FROM.bFromType;
								cUseable.ExtraFromData = (int)cSDT_MAILACCESS_FROM.stFromInfo.get_stSkinInfo().dwSkinID;
							}
						}
					}
					listView.Add(cUseable);
				}
			}
			return listView;
		}

		public static void ConnectVirtualList(ref ListView<CUseable> srcList1, CUseable[] srcList2)
		{
			for (int i = 0; i < srcList2.Length; i++)
			{
				bool flag = false;
				CVirtualItem cVirtualItem = srcList2[i] as CVirtualItem;
				if (cVirtualItem != null)
				{
					for (int j = 0; j < srcList1.get_Count(); j++)
					{
						CVirtualItem cVirtualItem2 = srcList1.get_Item(j) as CVirtualItem;
						if (cVirtualItem2 != null && cVirtualItem2.m_virtualType == cVirtualItem.m_virtualType)
						{
							cVirtualItem2.m_stackCount += cVirtualItem.m_stackCount;
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					srcList1.Add(srcList2[i]);
				}
			}
		}
	}
}
