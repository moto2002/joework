using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.UI;
using CSProtocol;
using ResData;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	public class CFriendContoller : Singleton<CFriendContoller>
	{
		public enum enMentorTab
		{
			MentorAndClassmate,
			Apprentice,
			Count
		}

		public const string MentorSearchResultNodeName = "MentorResult";

		public const string MentorSearchResultPath = "UGUI/Form/System/Friend/MentorResult.prefab";

		public const string MentorRecommendNodeName = "MentorRecommendList";

		public const string MentorSearchRecommendPath = "UGUI/Form/System/Friend/MentorRecommendList.prefab";

		public const string Evt_Mentor_OnGetRecommend = "Evt_Mentor_GetRecommend";

		public const string Evt_Mentor_OnGetResult = "Evt_Mentor_GetResult";

		public static string FriendFormPath = "UGUI/Form/System/Friend/FriendForm.prefab";

		public static string AddFriendFormPath = "UGUI/Form/System/Friend/AddFriend.prefab";

		public static string VerifyFriendFormPath = "UGUI/Form/System/Friend/Form_FriendVerification.prefab";

		public static string IntimacyRelaFormPath = "UGUI/Form/System/Friend/Form_IntimacyRela.prefab";

		public static string MentorRequestListFormPath = "UGUI/Form/System/Friend/Form_MentorRequestList.prefab";

		public static string MentorPrivilegeFormPath = "UGUI/Form/System/Friend/Form_MentorPrivilege.prefab";

		public static string MentorTaskFormPath = "UGUI/Form/System/Friend/Form_MentorTask.prefab";

		public static string AddMentorFormPath = "UGUI/Form/System/Friend/Form_AddMentor.prefab";

		public int m_currMentorPrivilegeLv = 1;

		public CMentorListOffset[] m_mentorListOff;

		public CFriendModel model;

		public CFriendView view;

		public COMDT_FRIEND_INFO search_info;

		private GameObject com;

		public static int MentorTabMask = 0;

		public static string[] s_mentorTabStr = null;

		public static string[] s_mentorTabName = new string[]
		{
			"Mentor_TabMentorNClassmate",
			"Mentor_TabApprentice"
		};

		public static SCPKG_MASTERSTUDENT_INFO m_mentorInfo = null;

		public static int s_addViewtype = 3;

		public string IntimacyUpInfo = string.Empty;

		public int IntimacyUpValue;

		public COMDT_ACNT_UNIQ m_mentorSelectedUin;

		private int m_mentorPrivilegePage = 1;

		public ulong startCooldownTimestamp
		{
			get;
			set;
		}

		public CFriendContoller()
		{
			this.model = new CFriendModel();
			this.view = new CFriendView();
			this.m_mentorListOff = new CMentorListOffset[3];
			this.m_mentorListOff[1] = new CMentorListOffset();
			this.m_mentorListOff[2] = new CMentorListOffset();
		}

		public void ClearAll()
		{
			this.model.ClearAll();
			this.IntimacyUpInfo = string.Empty;
			this.IntimacyUpValue = 0;
			this.search_info = null;
			this.com = null;
			this.SetFilter(3u);
		}

		public override void Init()
		{
			base.Init();
			this.InitEvent();
		}

		public void Update()
		{
			this.view.Update();
		}

		private void InitEvent()
		{
			Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg>("Friend_List", new Action<CSPkg>(this.On_FriendSys_Friend_List));
			Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg>("Friend_Request_List", new Action<CSPkg>(this.On_FriendSys_Friend_Request_List));
			Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg>("Friend_Recommand_List", new Action<CSPkg>(this.On_FriendSys_Friend_Recommand_List));
			Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg>("Friend_Search", new Action<CSPkg>(this.On_FriendSys_Friend_Search));
			Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg>("Friend_RequestBeFriend", new Action<CSPkg>(this.On_FriendSys_Friend_RequestBeFriend));
			Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg, CFriendModel.FriendType>("Friend_Confrim", new Action<CSPkg, CFriendModel.FriendType>(this.On_FriendSys_Friend_Confrim));
			Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg>("Friend_Deny", new Action<CSPkg>(this.On_FriendSys_Friend_Deny));
			Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg>("Friend_Delete", new Action<CSPkg>(this.On_FriendSys_Friend_Delete));
			Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg>("Friend_ADD_NTF", new Action<CSPkg>(this.On_FriendSys_Friend_ADD_NTF));
			Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg>("Friend_Delete_NTF", new Action<CSPkg>(this.On_FriendSys_Friend_Delete_NTF));
			Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg>("Friend_Request_NTF", new Action<CSPkg>(this.On_FriendSys_Friend_Request_NTF));
			Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg>("Friend_Login_NTF", new Action<CSPkg>(this.On_Friend_Login_NTF));
			Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg>("Friend_GAME_STATE_NTF", new Action<CSPkg>(this.On_Friend_GAME_STATE_NTF));
			Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg>("Friend_SNS_STATE_NTF", new Action<CSPkg>(this.On_Friend_SNS_STATE_NTF));
			Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg>("Friend_SNS_CHG_PROFILE", new Action<CSPkg>(this.On_Friend_SNS_CHG_PROFILE));
			Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg>("Friend_SNS_NICNAME_NTF", new Action<CSPkg>(this.On_Friend_SNS_NICKNAME_NTF));
			Singleton<EventRouter>.GetInstance().AddEventHandler<float, float>("Friend_LBS_Location_Calced", new Action<float, float>(this.On_Friend_LBS_Location_Calced));
			Singleton<EventRouter>.GetInstance().AddEventHandler("Friend_RecommandFriend_Refresh", new Action(this.On_Friend_RecommandFriend_Refresh));
			Singleton<EventRouter>.GetInstance().AddEventHandler("Friend_FriendList_Refresh", new Action(this.On_Friend_FriendList_Refresh));
			Singleton<EventRouter>.GetInstance().AddEventHandler("Friend_SNSFriendList_Refresh", new Action(this.On_Friend_SNSFriendList_Refresh));
			Singleton<EventRouter>.GetInstance().AddEventHandler("Friend_LBS_User_Refresh", new Action(this.On_Friend_LBS_User_Refresh));
			Singleton<EventRouter>.GetInstance().AddEventHandler("Guild_Invite_Success", new Action(this.On_GuildSys_Guild_Invite_Success));
			Singleton<EventRouter>.GetInstance().AddEventHandler("Guild_Recommend_Success", new Action(this.On_GuildSys_Guild_Recommend_Success));
			Singleton<EventRouter>.GetInstance().AddEventHandler(EventID.NEWDAY_NTF, new Action(this.OnNewDayNtf));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_OpenForm, new CUIEventManager.OnUIEventHandler(this.On_OpenForm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_CloseForm, new CUIEventManager.OnUIEventHandler(this.On_CloseForm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_Tab_Change, new CUIEventManager.OnUIEventHandler(this.On_TabChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_OpenAddFriendForm, new CUIEventManager.OnUIEventHandler(this.On_OpenAddFriendForm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_RequestBeFriend, new CUIEventManager.OnUIEventHandler(this.On_AddFriend));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_Accept_RequestFriend, new CUIEventManager.OnUIEventHandler(this.On_Accept_RequestFriend));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_Refuse_RequestFriend, new CUIEventManager.OnUIEventHandler(this.On_Refuse_RequestFriend));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_DelFriend, new CUIEventManager.OnUIEventHandler(this.On_DelFriend));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_Tab_Friend, new CUIEventManager.OnUIEventHandler(this.On_Friend_Tab_Friend));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_Tab_FriendRequest, new CUIEventManager.OnUIEventHandler(this.On_Friend_Tab_FriendRequest));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_DelFriend_OK, new CUIEventManager.OnUIEventHandler(this.On_DelFriend_OK));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_DelFriend_Cancle, new CUIEventManager.OnUIEventHandler(this.On_Friend_DelFriend_Cancle));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_SendCoin, new CUIEventManager.OnUIEventHandler(this.On_Friend_SendCoin));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_SNS_SendCoin, new CUIEventManager.OnUIEventHandler(this.On_SNSFriend_SendCoin));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_InviteGuild, new CUIEventManager.OnUIEventHandler(this.On_Friend_InviteGuild));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_RecommendGuild, new CUIEventManager.OnUIEventHandler(this.On_Friend_RecommendGuild));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_CheckInfo, new CUIEventManager.OnUIEventHandler(this.On_Friend_CheckInfo));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_Recommend_CheckInfo, new CUIEventManager.OnUIEventHandler(this.On_FriendRecommend_CheckInfo));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_List_ElementEnable, new CUIEventManager.OnUIEventHandler(this.On_Friend_List_ElementEnable));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_Invite_SNS_Friend, new CUIEventManager.OnUIEventHandler(this.On_Friend_Invite_SNS_Friend));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_Share_SendCoin, new CUIEventManager.OnUIEventHandler(this.On_Friend_Share_SendCoin));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_SNS_ReCall, new CUIEventManager.OnUIEventHandler(this.On_Friend_SNS_ReCall));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_OB_Click, new CUIEventManager.OnUIEventHandler(this.On_Friend_OB_Click));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.QQBOX_OnClick, new CUIEventManager.OnUIEventHandler(this.QQBox_OnClick));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_SNS_Share_Switch_Click, new CUIEventManager.OnUIEventHandler(this.OnShareToggle));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_SNS_Add_Switch_Click, new CUIEventManager.OnUIEventHandler(this.OnAddToggle));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_Chat_Button, new CUIEventManager.OnUIEventHandler(this.OnFriend_Chat_Button));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_Show_Rule, new CUIEventManager.OnUIEventHandler(this.OnFriend_Show_Rule));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_Gift, new CUIEventManager.OnUIEventHandler(this.OnFriend_Gift_Button));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_CancleBlock, new CUIEventManager.OnUIEventHandler(this.OnCancleBlockBtn));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_CancleBlock_Ok, new CUIEventManager.OnUIEventHandler(this.OnCancleBlockBtnOK));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_LBS_NoShare, new CUIEventManager.OnUIEventHandler(this.OnFriend_LBS_NoShare));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_LBS_Nan, new CUIEventManager.OnUIEventHandler(this.OnFriend_LBS_Nan));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_LBS_Nv, new CUIEventManager.OnUIEventHandler(this.OnFriend_LBS_Nv));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_LBS_Refresh, new CUIEventManager.OnUIEventHandler(this.OnFriend_LBS_Refresh));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_LBS_CheckInfo, new CUIEventManager.OnUIEventHandler(this.OnFriend_LBS_CheckInfo));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_Room_AddFriend, new CUIEventManager.OnUIEventHandler(this.OnFriend_Room_AddFriend));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.IntimacyRela_Menu_Btn_Click, new CUIEventManager.OnUIEventHandler(this.OnIntimacyRela_Menu_Btn_Click));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mentor_MentorQuest, new CUIEventManager.OnUIEventHandler(this.OnMentorTask_Btn_Click));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mentor_IWant, new CUIEventManager.OnUIEventHandler(this.OnMentor_IWant));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mentor_FriendFormTabChange, new CUIEventManager.OnUIEventHandler(this.OnMentorTabChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mentor_RequestListOnEnable, new CUIEventManager.OnUIEventHandler(this.On_MentorRequestListEnable));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mentor_AcceptRequest, new CUIEventManager.OnUIEventHandler(this.OnMentor_AcceptRequest));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mentor_RefuseRequest, new CUIEventManager.OnUIEventHandler(this.OnMentor_RefuseRequest));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mentor_AddMentor, new CUIEventManager.OnUIEventHandler(this.On_AddMentor));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mentor_OpenRequestList, new CUIEventManager.OnUIEventHandler(this.OnMentor_RequestListClick));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mentor_CloseRequestList, new CUIEventManager.OnUIEventHandler(this.OnMentorRequestListClose));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mentor_OpenPrivilegePage, new CUIEventManager.OnUIEventHandler(this.OnMentor_OpenPrivilegePage));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mentor_PrivilegePageLeft, new CUIEventManager.OnUIEventHandler(this.OnMentor_PrivilegeLeftClick));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mentor_PrivilegePageRight, new CUIEventManager.OnUIEventHandler(this.OnMentor_PrivilegeRightClick));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mentor_PrivilegeListEnable, new CUIEventManager.OnUIEventHandler(this.OnMentor_PrivilegeListEnable));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Mentor_GetMoreMentor, new CUIEventManager.OnUIEventHandler(this.OnMentorGetMoreMentorList));
		}

		private void On_Friend_FriendList_Refresh()
		{
			if (this.view != null && this.view.IsActive())
			{
				this.view.Refresh();
			}
		}

		private void On_Friend_SNSFriendList_Refresh()
		{
			if (this.view != null && this.view.addFriendView != null && this.view.addFriendView.bShow)
			{
				this.view.addFriendView.Refresh(3);
			}
		}

		private void On_Friend_LBS_User_Refresh()
		{
			if (this.view != null && this.view.IsActive())
			{
				this.view.Refresh();
			}
		}

		private void On_Friend_RecommandFriend_Refresh()
		{
		}

		private void On_Friend_Login_NTF(CSPkg msg)
		{
			SCPKG_CMD_NTF_FRIEND_LOGIN_STATUS stNtfFriendLoginStatus = msg.stPkgData.get_stNtfFriendLoginStatus();
			CFriendModel.FriendType type = (stNtfFriendLoginStatus.bFriendType != 1) ? CFriendModel.FriendType.SNS : CFriendModel.FriendType.GameFriend;
			COMDT_FRIEND_INFO info = Singleton<CFriendContoller>.GetInstance().model.GetInfo(type, stNtfFriendLoginStatus.stUin);
			if (info == null)
			{
				return;
			}
			info.bIsOnline = stNtfFriendLoginStatus.bLoginStatus;
			info.dwLastLoginTime = (uint)CRoleInfo.GetCurrentUTCTime();
			Singleton<CFriendContoller>.GetInstance().model.SortGameFriend();
			if (this.view != null && this.view.IsActive())
			{
				this.view.Refresh();
			}
		}

		public void On_Mentor_Login_NTF(CSPkg msg)
		{
			SCPKG_CMD_NTF_MASTERSTUDENT_LOGIN_STATUS stMasterStudentLoginNtf = msg.stPkgData.get_stMasterStudentLoginNtf();
			byte bType = stMasterStudentLoginNtf.bType;
			CFriendModel.FriendType type;
			if (bType != 2)
			{
				if (bType != 3)
				{
					return;
				}
				type = CFriendModel.FriendType.Apprentice;
			}
			else
			{
				type = CFriendModel.FriendType.Mentor;
			}
			COMDT_FRIEND_INFO info = Singleton<CFriendContoller>.GetInstance().model.GetInfo(type, stMasterStudentLoginNtf.stUin);
			if (info == null)
			{
				return;
			}
			info.bIsOnline = stMasterStudentLoginNtf.bLoginStatus;
			info.dwLastLoginTime = (uint)CRoleInfo.GetCurrentUTCTime();
			Singleton<CFriendContoller>.GetInstance().model.SortGameFriend();
			if (this.view != null && this.view.IsActive())
			{
				this.view.Refresh();
			}
		}

		private void On_Friend_GAME_STATE_NTF(CSPkg msg)
		{
			SCPKG_CMD_NTF_FRIEND_GAME_STATE stNtfFriendGameState = msg.stPkgData.get_stNtfFriendGameState();
			this.model.SetFriendGameState(stNtfFriendGameState.stAcntUniq.ullUid, stNtfFriendGameState.stAcntUniq.dwLogicWorldId, stNtfFriendGameState.bGameState, stNtfFriendGameState.dwGameStartTime, string.Empty, false);
			if (this.view != null && this.view.IsActive())
			{
				this.view.Refresh();
			}
			Singleton<EventRouter>.get_instance().BroadCastEvent(EventID.Friend_Game_State_Change);
		}

		private void On_Friend_Tab_FriendRequest(CUIEvent uievent)
		{
			if (this.view != null && this.view.IsActive())
			{
				this.view.On_Tab_Change(1);
			}
		}

		private void On_Friend_Tab_Friend(CUIEvent uievent)
		{
			if (this.view != null && this.view.IsActive())
			{
				this.view.On_Tab_Change(0);
			}
		}

		private void On_TabChange(CUIEvent uievent)
		{
			if (this.view != null && this.view.IsActive())
			{
				int selectedIndex = uievent.m_srcWidget.GetComponent<CUIListScript>().GetSelectedIndex();
				this.view.On_Tab_Change(selectedIndex);
			}
		}

		private void On_Friend_SNS_STATE_NTF(CSPkg msg)
		{
			SCPKG_NTF_SNS_FRIEND stNtfSnsFriend = msg.stPkgData.get_stNtfSnsFriend();
			uint currentUTCTime = (uint)CRoleInfo.GetCurrentUTCTime();
			uint num = 86400u * GameDataMgr.globalInfoDatabin.GetDataByKey(158u).dwConfValue;
			ListView<COMDT_FRIEND_INFO> list = this.model.GetList(CFriendModel.FriendType.SNS);
			int num2 = 0;
			while ((long)num2 < (long)((ulong)stNtfSnsFriend.dwSnsFriendNum))
			{
				CSDT_SNS_FRIEND_INFO cSDT_SNS_FRIEND_INFO = stNtfSnsFriend.astSnsFriendList[num2];
				if (cSDT_SNS_FRIEND_INFO != null)
				{
					if (!this.FilterSameFriend(cSDT_SNS_FRIEND_INFO.stSnsFrindInfo, list))
					{
						this.model.Add(CFriendModel.FriendType.SNS, cSDT_SNS_FRIEND_INFO.stSnsFrindInfo, false);
						this.model.SetFriendGameState(cSDT_SNS_FRIEND_INFO.stSnsFrindInfo.stUin.ullUid, cSDT_SNS_FRIEND_INFO.stSnsFrindInfo.stUin.dwLogicWorldId, cSDT_SNS_FRIEND_INFO.bGameState, cSDT_SNS_FRIEND_INFO.dwGameStartTime, UT.Bytes2String(cSDT_SNS_FRIEND_INFO.szNickName), false);
						this.Handle_CoinSend_Data(cSDT_SNS_FRIEND_INFO);
					}
				}
				num2++;
			}
			this.model.SortSNSFriend();
			if (this.view != null && this.view.IsActive())
			{
				this.view.Refresh();
			}
			Singleton<EventRouter>.get_instance().BroadCastEvent(EventID.Friend_Game_State_Change);
		}

		public static int GetSrvFriendTypeFromFriendType(CFriendModel.FriendType friendType)
		{
			switch (friendType)
			{
			case CFriendModel.FriendType.GameFriend:
				return 1;
			case CFriendModel.FriendType.SNS:
				return 2;
			case CFriendModel.FriendType.Mentor:
				return 6;
			case CFriendModel.FriendType.Apprentice:
				return 5;
			}
			return -1;
		}

		public static int GetFriendTypeFromSrvFriendType(COM_FRIEND_TYPE srvFriendType)
		{
			switch (srvFriendType)
			{
			case 1:
				return 1;
			case 2:
				return 4;
			case 5:
				return 7;
			case 6:
				return 6;
			}
			return -1;
		}

		private void On_Friend_SNS_NICKNAME_NTF(CSPkg msg)
		{
			SCPKG_NTF_SNS_NICKNAME stNtfSnsNickName = msg.stPkgData.get_stNtfSnsNickName();
			uint num = Math.Min(stNtfSnsNickName.dwSnsFriendNum, 100u);
			int num2 = 0;
			while ((long)num2 < (long)((ulong)num))
			{
				if (!CFriendModel.RemarkNames.ContainsKey(stNtfSnsNickName.astSnsNameList[num2].ullUid))
				{
					CFriendModel.RemarkNames.Add(stNtfSnsNickName.astSnsNameList[num2].ullUid, CUIUtility.RemoveEmoji(UT.Bytes2String(stNtfSnsNickName.astSnsNameList[num2].szNickName)));
				}
				num2++;
			}
			if (this.view != null && this.view.IsActive())
			{
				this.view.Refresh();
			}
		}

		public bool FilterSameFriend(COMDT_FRIEND_INFO info, ListView<COMDT_FRIEND_INFO> friendList)
		{
			if (friendList != null)
			{
				for (int i = 0; i < friendList.get_Count(); i++)
				{
					if (friendList.get_Item(i).stUin.ullUid == info.stUin.ullUid)
					{
						if (friendList.get_Item(i).dwLastLoginTime < info.dwLastLoginTime)
						{
							friendList.set_Item(i, info);
						}
						return true;
					}
				}
			}
			return false;
		}

		public void OnReCallFriendNtf(CSPkg msg)
		{
			uint currentUTCTime = (uint)CRoleInfo.GetCurrentUTCTime();
			uint num = 86400u * GameDataMgr.globalInfoDatabin.GetDataByKey(158u).dwConfValue;
			for (int i = 0; i < (int)msg.stPkgData.get_stNtfRecallFirend().wRecallNum; i++)
			{
				COMDT_RECALL_FRIEND cOMDT_RECALL_FRIEND = msg.stPkgData.get_stNtfRecallFirend().astRecallAcntList[i];
				if (cOMDT_RECALL_FRIEND != null)
				{
					this.Handle_Invite_Data(cOMDT_RECALL_FRIEND.stAcntUniq);
				}
			}
			Singleton<CFriendContoller>.get_instance().model.friendRecruit.SetRecruiterRewardBits(msg.stPkgData.get_stNtfRecallFirend().dwRecruiterRewardBits);
			Singleton<CFriendContoller>.GetInstance().model.FRData.SetFirstChoiseState(msg.stPkgData.get_stNtfRecallFirend().bIntimacyRelationPrior);
		}

		public void OnSnsSwitchRsp(CSPkg msg)
		{
			if (msg.stPkgData.get_stRefuseRecallRsp().dwResult == 0u)
			{
			}
		}

		public void OnSnsSwitchNtf(CSPkg msg)
		{
			COMDT_ACNT_UNIQ stUin = msg.stPkgData.get_stNtfRefuseRecall().stUin;
			COMDT_FRIEND_INFO friendByUid = this.model.getFriendByUid(stUin.ullUid, CFriendModel.FriendType.SNS, 0u);
			if (friendByUid == null)
			{
				return;
			}
			friendByUid.dwRefuseFriendBits = msg.stPkgData.get_stNtfRefuseRecall().dwRefuseFriendBits;
		}

		public void OnChangeIntimacy(CSPkg msg)
		{
			if (msg.stPkgData.get_stNtfChgIntimacy().dwResult == 0u)
			{
				SCPKG_CMD_NTF_CHG_INTIMACY stNtfChgIntimacy = msg.stPkgData.get_stNtfChgIntimacy();
				CFriendModel cFriendModel = Singleton<CFriendContoller>.get_instance().model;
				ushort num;
				CFriendModel.EIntimacyType eIntimacyType;
				bool flag;
				if (cFriendModel.GetFriendIntimacy(stNtfChgIntimacy.stUin.ullUid, stNtfChgIntimacy.stUin.dwLogicWorldId, out num, out eIntimacyType, out flag))
				{
					int num2 = (int)(stNtfChgIntimacy.stIntimacData.wIntimacyValue - num);
					if (num2 > 0)
					{
						COMDT_FRIEND_INFO gameOrSnsFriend = this.model.GetGameOrSnsFriend(stNtfChgIntimacy.stUin.ullUid, stNtfChgIntimacy.stUin.dwLogicWorldId);
						if (gameOrSnsFriend != null)
						{
							if (!Singleton<BattleLogic>.get_instance().isRuning)
							{
								this.IntimacyUpInfo = string.Format(UT.GetText("Intimacy_UpInfo"), UT.Bytes2String(gameOrSnsFriend.szUserName), num2);
								Singleton<CUIManager>.GetInstance().OpenTips(this.IntimacyUpInfo, false, 1.5f, null, new object[0]);
								this.IntimacyUpInfo = string.Empty;
								this.IntimacyUpValue = 0;
							}
							else
							{
								if (string.IsNullOrEmpty(this.IntimacyUpInfo))
								{
									this.IntimacyUpInfo = string.Format(" {0}", UT.Bytes2String(gameOrSnsFriend.szUserName));
								}
								else
								{
									this.IntimacyUpInfo += string.Format(", {0}", UT.Bytes2String(gameOrSnsFriend.szUserName));
								}
								this.IntimacyUpValue = num2;
							}
						}
					}
				}
				this.model.AddFriendIntimacy(stNtfChgIntimacy.stUin, stNtfChgIntimacy.stIntimacData);
				Singleton<CFriendContoller>.GetInstance().model.SortGameFriend();
				if (this.view != null && this.view.IsActive())
				{
					this.view.Refresh();
				}
			}
			else if (msg.stPkgData.get_stNtfChgIntimacy().dwResult == 170u)
			{
				this.IntimacyUpInfo = "跟朋友的亲密度已满";
				if (!Singleton<BattleLogic>.get_instance().isRuning)
				{
					Singleton<CUIManager>.GetInstance().OpenTips(this.IntimacyUpInfo, false, 1.5f, null, new object[0]);
					this.IntimacyUpInfo = string.Empty;
				}
				SCPKG_CMD_NTF_CHG_INTIMACY stNtfChgIntimacy2 = msg.stPkgData.get_stNtfChgIntimacy();
				this.model.AddFriendIntimacy(stNtfChgIntimacy2.stUin, stNtfChgIntimacy2.stIntimacData);
				Singleton<CFriendContoller>.GetInstance().model.SortGameFriend();
				if (this.view != null && this.view.IsActive())
				{
					this.view.Refresh();
				}
			}
		}

		private void On_Friend_SNS_CHG_PROFILE(CSPkg msg)
		{
			SCPKG_CHG_SNS_FRIEND_PROFILE stSnsFriendChgProfile = msg.stPkgData.get_stSnsFriendChgProfile();
			this.model.Add(CFriendModel.FriendType.SNS, stSnsFriendChgProfile.stSnsFrindInfo, true);
			this.model.SetFriendGameState(stSnsFriendChgProfile.stSnsFrindInfo.stUin.ullUid, stSnsFriendChgProfile.stSnsFrindInfo.stUin.dwLogicWorldId, stSnsFriendChgProfile.bGameState, 0u, string.Empty, true);
			this.model.SortSNSFriend();
			if (this.view != null && this.view.IsActive())
			{
				this.view.Refresh();
			}
			Singleton<EventRouter>.get_instance().BroadCastEvent(EventID.Friend_Game_State_Change);
		}

		private void On_Friend_LBS_Location_Calced(float n, float e)
		{
		}

		private void On_FriendSys_Friend_List(CSPkg msg)
		{
			SCPKG_CMD_LIST_FRIEND stFriendListRsp = msg.stPkgData.get_stFriendListRsp();
			if (stFriendListRsp == null)
			{
				return;
			}
			int num = Mathf.Min(stFriendListRsp.astFrindList.Length, (int)stFriendListRsp.dwFriendNum);
			for (int i = 0; i < num; i++)
			{
				CSDT_FRIEND_INFO cSDT_FRIEND_INFO = stFriendListRsp.astFrindList[i];
				this.model.Add(CFriendModel.FriendType.GameFriend, cSDT_FRIEND_INFO.stFriendInfo, false);
				this.Handle_CoinSend_Data(cSDT_FRIEND_INFO);
				this.model.SetFriendGameState(cSDT_FRIEND_INFO.stFriendInfo.stUin.ullUid, cSDT_FRIEND_INFO.stFriendInfo.stUin.dwLogicWorldId, cSDT_FRIEND_INFO.bGameState, cSDT_FRIEND_INFO.dwGameStartTime, string.Empty, false);
				this.model.AddFriendIntimacy(cSDT_FRIEND_INFO.stFriendInfo.stUin, cSDT_FRIEND_INFO.stIntimacyData);
				if (cSDT_FRIEND_INFO.stRecruitmentInfo != null)
				{
					this.model.friendRecruit.ParseFriend(cSDT_FRIEND_INFO.stFriendInfo, cSDT_FRIEND_INFO.stRecruitmentInfo);
				}
			}
			Singleton<CFriendContoller>.GetInstance().model.SortGameFriend();
			if (this.view != null && this.view.IsActive())
			{
				this.view.Refresh();
			}
			Singleton<EventRouter>.GetInstance().BroadCastEvent("Rank_Friend_List");
		}

		private void Handle_CoinSend_Data(CSDT_FRIEND_INFO info)
		{
			this.Update_Send_Coin_Data(info.stFriendInfo.stUin, info.ullDonateAPSec);
		}

		private void Handle_CoinSend_Data(CSDT_SNS_FRIEND_INFO info)
		{
			this.Update_Send_Coin_Data(info.stSnsFrindInfo.stUin, (ulong)info.dwDonateTime);
		}

		private void Handle_CoinSend_Data_Mentor(CSDT_FRIEND_INFO info)
		{
			this.Update_Send_Coin_Data(info.stFriendInfo.stUin, info.ullDonateAPSec);
		}

		private void Handle_CoinSend_Data_Apprentice(CSDT_FRIEND_INFO info)
		{
			this.Update_Send_Coin_Data(info.stFriendInfo.stUin, info.ullDonateAPSec);
		}

		private void Handle_Invite_Data(COMDT_ACNT_UNIQ uin)
		{
			this.model.SnsReCallData.Add(uin, 2);
		}

		private void Update_Send_Coin_Data(COMDT_ACNT_UNIQ uin, ulong donateAPSec)
		{
			DateTime dateTime = Utility.ToUtcTime2Local((long)donateAPSec);
			DateTime dateTime2 = Utility.ToUtcTime2Local((long)CRoleInfo.GetCurrentUTCTime());
			if (dateTime2.get_Year() == dateTime.get_Year() && dateTime2.get_Month() == dateTime.get_Month() && dateTime2.get_Day() == dateTime.get_Day())
			{
				this.model.HeartData.Add(uin);
			}
			if (dateTime2.get_Year() < dateTime.get_Year() || (dateTime2.get_Year() == dateTime.get_Year() && dateTime2.get_Month() < dateTime.get_Month()) || dateTime2.get_Year() != dateTime.get_Year() || dateTime2.get_Month() != dateTime.get_Month() || dateTime2.get_Day() < dateTime.get_Day())
			{
			}
		}

		private void On_FriendSys_Friend_Request_List(CSPkg msg)
		{
			SCPKG_CMD_LIST_FRIENDREQ stFriendReqListRsp = msg.stPkgData.get_stFriendReqListRsp();
			if (stFriendReqListRsp == null)
			{
				return;
			}
			int num = Mathf.Min(stFriendReqListRsp.astVerificationList.Length, (int)stFriendReqListRsp.dwFriendReqNum);
			for (int i = 0; i < num; i++)
			{
				CSDT_VERIFICATION_INFO cSDT_VERIFICATION_INFO = stFriendReqListRsp.astVerificationList[i];
				if (cSDT_VERIFICATION_INFO.bIntimacyState == 0)
				{
					this.model.Add(CFriendModel.FriendType.RequestFriend, cSDT_VERIFICATION_INFO.stFriendInfo, false);
					this.model.AddFriendVerifyContent(cSDT_VERIFICATION_INFO.stFriendInfo.stUin.ullUid, cSDT_VERIFICATION_INFO.stFriendInfo.stUin.dwLogicWorldId, StringHelper.BytesToString(cSDT_VERIFICATION_INFO.szVerificationInfo), CFriendModel.enVerifyDataSet.Friend, cSDT_VERIFICATION_INFO.stUserSource, 0);
				}
				else
				{
					Debug.Log("---FR On_FriendSys_Friend_Request_List");
					this.model.FRData.ProcessOtherRequest(cSDT_VERIFICATION_INFO, true);
				}
			}
			if (this.view != null && this.view.IsActive())
			{
				this.view.Refresh();
			}
		}

		public void OnMentor_RequestList(CSPkg msg)
		{
			SCPKG_MASTERREQ_LIST stMasterReqList = msg.stPkgData.get_stMasterReqList();
			if (stMasterReqList == null)
			{
				return;
			}
			int num = 0;
			while ((long)num < (long)((ulong)stMasterReqList.dwNum))
			{
				this.model.Add(CFriendModel.FriendType.MentorRequestList, stMasterReqList.astMasterReqList[num].stReqUserInfo, false);
				this.model.AddFriendVerifyContent(stMasterReqList.astMasterReqList[num].stReqUserInfo.stUin.ullUid, stMasterReqList.astMasterReqList[num].stReqUserInfo.stUin.dwLogicWorldId, StringHelper.BytesToString(stMasterReqList.astMasterReqList[num].szVerificationInfo), CFriendModel.enVerifyDataSet.Mentor, null, (int)stMasterReqList.astMasterReqList[num].bType);
				num++;
			}
			if (this.view != null && this.view.IsActive())
			{
				this.view.Refresh();
			}
		}

		public static CFriendModel.FriendType GetFriendType(int searchType)
		{
			if (searchType != 2 && searchType != 3)
			{
				return CFriendModel.FriendType.Recommend;
			}
			return CFriendModel.FriendType.MentorRecommend;
		}

		private void On_FriendSys_Friend_Recommand_List(CSPkg msg)
		{
			SCPKG_CMD_LIST_FREC stFRecRsp = msg.stPkgData.get_stFRecRsp();
			CFriendModel.FriendType friendType = CFriendContoller.GetFriendType((int)stFRecRsp.bType);
			this.model.Clear(friendType);
			if (stFRecRsp.dwResult == 0u)
			{
				int num = 0;
				while ((long)num < (long)((ulong)stFRecRsp.dwAcntNum))
				{
					COMDT_FRIEND_INFO cOMDT_FRIEND_INFO = stFRecRsp.astAcnts[num];
					if (friendType != CFriendModel.FriendType.GameFriend || this.model.getFriendByUid(cOMDT_FRIEND_INFO.stUin.ullUid, CFriendModel.FriendType.GameFriend, 0u) == null)
					{
						this.model.Add(friendType, cOMDT_FRIEND_INFO, false);
					}
					num++;
				}
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips(UT.ErrorCode_String(stFRecRsp.dwResult), false, 1.5f, null, new object[0]);
			}
			if (this.view != null)
			{
				this.view.addFriendView.Refresh_Friend_Recommand_List(friendType);
			}
		}

		private void On_FriendSys_Friend_Search(CSPkg msg)
		{
			SCPKG_CMD_SEARCH_PLAYER stFriendSearchPlayerRsp = msg.stPkgData.get_stFriendSearchPlayerRsp();
			if (stFriendSearchPlayerRsp.dwResult == 0u)
			{
				this.search_info = stFriendSearchPlayerRsp.stUserInfo;
				if (this.view != null)
				{
					this.view.Show_Search_Result(stFriendSearchPlayerRsp.stUserInfo);
				}
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips(UT.ErrorCode_String(stFriendSearchPlayerRsp.dwResult), false, 1.5f, null, new object[0]);
			}
			this.view.addFriendView.Refresh_Friend_Recommand_List_Pos();
		}

		private void On_FriendSys_Friend_RequestBeFriend(CSPkg msg)
		{
			SCPKG_CMD_ADD_FRIEND stFriendAddRsp = msg.stPkgData.get_stFriendAddRsp();
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			Singleton<EventRouter>.get_instance().BroadCastEvent("Friend_SNSFriendList_Refresh");
			UT.ShowFriendNetResult(stFriendAddRsp.dwResult, UT.FriendResultType.RequestBeFriend);
		}

		private void On_FriendSys_Friend_Confrim(CSPkg msg, CFriendModel.FriendType friendType)
		{
			if (friendType != CFriendModel.FriendType.RequestFriend)
			{
				if (friendType != CFriendModel.FriendType.MentorRequestList)
				{
					return;
				}
				SCPKG_CONFIRM_MASTER_RSP stConfirmMasterRsp = msg.stPkgData.get_stConfirmMasterRsp();
				Singleton<CFriendContoller>.GetInstance().model.Remove(CFriendModel.FriendType.MentorRequestList, stConfirmMasterRsp.stUin);
			}
			else
			{
				SCPKG_CMD_ADD_FRIEND_CONFIRM stFriendAddConfirmRsp = msg.stPkgData.get_stFriendAddConfirmRsp();
				COMDT_FRIEND_INFO stUserInfo = stFriendAddConfirmRsp.stUserInfo;
				if (stFriendAddConfirmRsp.dwResult == 0u)
				{
					Singleton<CUIManager>.GetInstance().OpenTips(string.Format(UT.GetText("Friend_Tips_BeFriend_Ok"), UT.Bytes2String(stUserInfo.szUserName)), false, 1.5f, null, new object[0]);
					Singleton<CFriendContoller>.GetInstance().model.Remove(CFriendModel.FriendType.RequestFriend, stUserInfo.stUin);
					Singleton<CFriendContoller>.GetInstance().model.Add(CFriendModel.FriendType.GameFriend, stUserInfo, false);
					if (this.model.IsBlack(stUserInfo.stUin.ullUid, stUserInfo.stUin.dwLogicWorldId))
					{
						this.model.RemoveFriendBlack(stUserInfo.stUin.ullUid, stUserInfo.stUin.dwLogicWorldId);
					}
					this.model.AddFriendIntimacy(stUserInfo.stUin, stFriendAddConfirmRsp.stIntimacData);
					Singleton<CFriendContoller>.GetInstance().model.SortGameFriend();
					if (stFriendAddConfirmRsp.bFriendSrcType == 2)
					{
						this.model.friendRecruit.SetBeiZhaoMuZheRewardData(stUserInfo);
					}
				}
				else
				{
					Singleton<CUIManager>.GetInstance().OpenTips(UT.ErrorCode_String(stFriendAddConfirmRsp.dwResult), false, 1.5f, null, new object[0]);
					this.Remove_And_Refresh(CFriendModel.FriendType.RequestFriend, stUserInfo.stUin);
				}
			}
			CFriendView cFriendView = Singleton<CFriendContoller>.GetInstance().view;
			if (cFriendView != null && cFriendView.IsActive())
			{
				cFriendView.Refresh();
			}
		}

		private void On_FriendSys_Friend_Deny(CSPkg msg)
		{
			SCPKG_CMD_ADD_FRIEND_DENY stFriendAddDenyRsp = msg.stPkgData.get_stFriendAddDenyRsp();
			if (stFriendAddDenyRsp.dwResult == 0u)
			{
				this.Remove_And_Refresh(CFriendModel.FriendType.RequestFriend, stFriendAddDenyRsp.stUin);
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips(UT.ErrorCode_String(stFriendAddDenyRsp.dwResult), false, 1.5f, null, new object[0]);
				this.Remove_And_Refresh(CFriendModel.FriendType.RequestFriend, stFriendAddDenyRsp.stUin);
			}
		}

		private void On_FriendSys_Friend_Delete(CSPkg msg)
		{
			SCPKG_CMD_DEL_FRIEND stFriendDelRsp = msg.stPkgData.get_stFriendDelRsp();
			if (stFriendDelRsp.dwResult == 0u)
			{
				this.Remove_And_Refresh(CFriendModel.FriendType.GameFriend, stFriendDelRsp.stUin);
			}
		}

		private void On_FriendSys_Friend_ADD_NTF(CSPkg msg)
		{
			SCPKG_CMD_NTF_FRIEND_ADD stNtfFriendAdd = msg.stPkgData.get_stNtfFriendAdd();
			if (this.model.IsBlack(stNtfFriendAdd.stUserInfo.stUin.ullUid, stNtfFriendAdd.stUserInfo.stUin.dwLogicWorldId))
			{
				this.model.RemoveFriendBlack(stNtfFriendAdd.stUserInfo.stUin.ullUid, stNtfFriendAdd.stUserInfo.stUin.dwLogicWorldId);
			}
			this.model.RemoveFriendVerifyContent(stNtfFriendAdd.stUserInfo.stUin.ullUid, stNtfFriendAdd.stUserInfo.stUin.dwLogicWorldId, CFriendModel.enVerifyDataSet.Friend);
			this.model.AddFriendIntimacy(stNtfFriendAdd.stUserInfo.stUin, stNtfFriendAdd.stIntimacData);
			if (stNtfFriendAdd.bFriendSrcType == 2)
			{
				this.model.friendRecruit.SetZhaoMuZhe(stNtfFriendAdd.stUserInfo);
			}
			this.Add_And_Refresh(CFriendModel.FriendType.GameFriend, stNtfFriendAdd.stUserInfo);
		}

		private void On_FriendSys_Friend_Delete_NTF(CSPkg msg)
		{
			SCPKG_CMD_NTF_FRIEND_DEL stNtfFriendDel = msg.stPkgData.get_stNtfFriendDel();
			this.Remove_And_Refresh(CFriendModel.FriendType.GameFriend, stNtfFriendDel.stUin);
		}

		private void On_FriendSys_Friend_Request_NTF(CSPkg msg)
		{
			SCPKG_CMD_NTF_FRIEND_REQUEST stNtfFriendRequest = msg.stPkgData.get_stNtfFriendRequest();
			if (stNtfFriendRequest == null)
			{
				return;
			}
			this.model.AddFriendVerifyContent(stNtfFriendRequest.stUserInfo.stUin.ullUid, stNtfFriendRequest.stUserInfo.stUin.dwLogicWorldId, StringHelper.BytesToString(stNtfFriendRequest.szVerificationInfo), CFriendModel.enVerifyDataSet.Friend, stNtfFriendRequest.stUserSource, 0);
			this.Add_And_Refresh(CFriendModel.FriendType.RequestFriend, stNtfFriendRequest.stUserInfo);
		}

		private void On_GuildSys_Guild_Invite_Success()
		{
			if (this.view != null && this.view.IsActive())
			{
				this.view.Refresh();
			}
		}

		private void On_GuildSys_Guild_Recommend_Success()
		{
			if (this.view != null && this.view.IsActive())
			{
				this.view.Refresh();
			}
		}

		private void Add_And_Refresh(CFriendModel.FriendType type, COMDT_FRIEND_INFO data)
		{
			this.model.Add(type, data, false);
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo();
			switch (type)
			{
			case CFriendModel.FriendType.Mentor:
				if (CFriendContoller.m_mentorInfo != null)
				{
					CFriendContoller.m_mentorInfo.stMaster = new CSDT_FRIEND_INFO();
					CFriendContoller.m_mentorInfo.stMaster.stFriendInfo = Utility.DeepCopyByReflection<COMDT_FRIEND_INFO>(data);
					masterRoleInfo.m_mentorInfo.szRoleName = CFriendContoller.m_mentorInfo.stMaster.stFriendInfo.szUserName;
					CPlayerProfile profile = Singleton<CPlayerInfoSystem>.get_instance().GetProfile();
					if (profile != null && profile._mentorInfo != null)
					{
						profile._mentorInfo.szRoleName = data.szUserName;
					}
					Singleton<CPlayerMentorInfoController>.GetInstance().UpdateUI();
				}
				goto IL_1E3;
			case CFriendModel.FriendType.Apprentice:
				if (CFriendContoller.m_mentorInfo != null)
				{
					bool flag = true;
					for (int i = 0; i < (int)CFriendContoller.m_mentorInfo.bStudentNum; i++)
					{
						if (CFriendContoller.m_mentorInfo.astStudentList[i].stFriendInfo.stUin.dwLogicWorldId == data.stUin.dwLogicWorldId && CFriendContoller.m_mentorInfo.astStudentList[i].stFriendInfo.stUin.ullUid == data.stUin.ullUid)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						SCPKG_MASTERSTUDENT_INFO expr_17A = CFriendContoller.m_mentorInfo;
						expr_17A.bStudentNum += 1;
						CFriendContoller.m_mentorInfo.astStudentList[(int)(CFriendContoller.m_mentorInfo.bStudentNum - 1)] = new CSDT_FRIEND_INFO();
						CFriendContoller.m_mentorInfo.astStudentList[(int)(CFriendContoller.m_mentorInfo.bStudentNum - 1)].stFriendInfo = Utility.DeepCopyByReflection<COMDT_FRIEND_INFO>(data);
						masterRoleInfo.m_mentorInfo.dwStudentNum += 1u;
					}
				}
				goto IL_1E3;
			case CFriendModel.FriendType.MentorRecommend:
				IL_35:
				if (type != CFriendModel.FriendType.GameFriend)
				{
					goto IL_1E3;
				}
				Singleton<CFriendContoller>.GetInstance().model.SortGameFriend();
				goto IL_1E3;
			case CFriendModel.FriendType.MentorRequestList:
				this.view.RefreshMentorReqList();
				return;
			}
			goto IL_35;
			IL_1E3:
			if (this.view.IsActive())
			{
				this.view.Refresh();
			}
		}

		public void Remove_And_Refresh(CFriendModel.FriendType type, COMDT_ACNT_UNIQ uniq)
		{
			this.model.Remove(type, uniq);
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo();
			if (type != CFriendModel.FriendType.Mentor)
			{
				if (type == CFriendModel.FriendType.Apprentice)
				{
					if (CFriendContoller.m_mentorInfo != null)
					{
						int num = -1;
						for (int i = 0; i < (int)CFriendContoller.m_mentorInfo.bStudentNum; i++)
						{
							if (CFriendContoller.m_mentorInfo.astStudentList[i].stFriendInfo.stUin.dwLogicWorldId == uniq.dwLogicWorldId && CFriendContoller.m_mentorInfo.astStudentList[i].stFriendInfo.stUin.ullUid == uniq.ullUid)
							{
								num = i;
								break;
							}
						}
						if (num != -1)
						{
							for (int j = num; j < (int)(CFriendContoller.m_mentorInfo.bStudentNum - 1); j++)
							{
								CFriendContoller.m_mentorInfo.astStudentList[j] = CFriendContoller.m_mentorInfo.astStudentList[j + 1];
							}
							SCPKG_MASTERSTUDENT_INFO expr_15D = CFriendContoller.m_mentorInfo;
							expr_15D.bStudentNum -= 1;
						}
					}
				}
			}
			else if (CFriendContoller.m_mentorInfo != null)
			{
				CFriendContoller.m_mentorInfo.stMaster = new CSDT_FRIEND_INFO();
				masterRoleInfo.m_mentorInfo.ullMasterUid = 0uL;
				masterRoleInfo.m_mentorInfo.dwMasterLogicWorldID = 0u;
				CPlayerProfile profile = Singleton<CPlayerInfoSystem>.get_instance().GetProfile();
				if (profile != null && profile._mentorInfo != null)
				{
					profile._mentorInfo.szRoleName = null;
				}
				Singleton<CPlayerMentorInfoController>.GetInstance().UpdateUI();
			}
			if (this.view.IsActive())
			{
				this.view.Refresh();
			}
		}

		private void OnShareToggle(CUIEvent uievent)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo();
			if (masterRoleInfo == null)
			{
				return;
			}
			bool flag = CFriendModel.IsOnSnsSwitch(masterRoleInfo.snsSwitchBits, 0);
			int num = 1;
			masterRoleInfo.snsSwitchBits = (uint)((ulong)masterRoleInfo.snsSwitchBits ^ (ulong)((long)num));
			FriendSysNetCore.Send_Request_Sns_Switch(0, flag ? 0 : 1);
		}

		private void OnAddToggle(CUIEvent uievent)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo();
			if (masterRoleInfo == null)
			{
				return;
			}
			bool flag = CFriendModel.IsOnSnsSwitch(masterRoleInfo.snsSwitchBits, 1);
			int num = 2;
			masterRoleInfo.snsSwitchBits = (uint)((ulong)masterRoleInfo.snsSwitchBits ^ (ulong)((long)num));
			FriendSysNetCore.Send_Request_Sns_Switch(1, flag ? 0 : 1);
		}

		private void OnMentorRequestListClose(CUIEvent uievent)
		{
			GameObject widget = uievent.m_srcFormScript.GetWidget(1);
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo();
			if (masterRoleInfo == null)
			{
				return;
			}
			bool flag = CFriendModel.IsOnSnsSwitch(masterRoleInfo.snsSwitchBits, 3);
			uint num = 8u;
			bool isOn = widget.GetComponent<Toggle>().isOn;
			if (flag != isOn)
			{
				if (isOn)
				{
					masterRoleInfo.snsSwitchBits |= num;
				}
				else
				{
					masterRoleInfo.snsSwitchBits &= ~num;
				}
			}
			FriendSysNetCore.Send_Request_Sns_Switch(3, (!flag) ? 0 : 1);
		}

		private void OnFriend_Chat_Button(CUIEvent uievent)
		{
			FriendShower component = uievent.m_srcWidget.transform.parent.parent.parent.GetComponent<FriendShower>();
			if (component == null)
			{
				return;
			}
			COMDT_FRIEND_INFO info = this.model.GetInfo(CFriendContoller.GetFriendTypeFromItemType((FriendShower.ItemType)uievent.m_eventParams.tag), component.ullUid, component.dwLogicWorldID);
			if (info == null)
			{
				info = this.model.GetInfo(CFriendModel.FriendType.SNS, component.ullUid, component.dwLogicWorldID);
				if (info == null)
				{
					return;
				}
			}
			Singleton<CChatController>.get_instance().Jump2FriendChat(info);
		}

		private void OnFriend_Gift_Button(CUIEvent uievent)
		{
			FriendShower component = uievent.m_srcWidget.transform.parent.parent.parent.GetComponent<FriendShower>();
			if (component == null)
			{
				return;
			}
			if (!Utility.IsSamePlatformWithSelf(component.dwLogicWorldID))
			{
				Singleton<CUIManager>.GetInstance().OpenTips("CS_PRESENTHEROSKIN_INVALID_PLAT", true, 1.5f, null, new object[0]);
				return;
			}
			bool isSns = false;
			if (this.model.GetInfo(CFriendContoller.GetFriendTypeFromItemType((FriendShower.ItemType)uievent.m_eventParams.tag), component.ullUid, component.dwLogicWorldID) == null)
			{
				isSns = true;
				if (this.model.GetInfo(CFriendModel.FriendType.SNS, component.ullUid, component.dwLogicWorldID) == null)
				{
					return;
				}
			}
			Singleton<CMallSystem>.GetInstance().OpenGiftCenter(component.ullUid, component.dwLogicWorldID, isSns);
		}

		private void OnFriend_Show_Rule(CUIEvent uievent)
		{
			ResRuleText dataByKey = GameDataMgr.s_ruleTextDatabin.GetDataByKey(12u);
			if (this.view != null && this.view.CurTab == CFriendView.Tab.Mentor)
			{
				dataByKey = GameDataMgr.s_ruleTextDatabin.GetDataByKey(21u);
			}
			if (dataByKey != null)
			{
				string title = StringHelper.UTF8BytesToString(ref dataByKey.szTitle);
				string info = StringHelper.UTF8BytesToString(ref dataByKey.szContent);
				Singleton<CUIManager>.GetInstance().OpenInfoForm(title, info);
			}
		}

		private void OnCancleBlockBtn(CUIEvent uiEvent)
		{
			this.com = uiEvent.m_srcWidget.transform.parent.parent.parent.gameObject;
			if (this.com == null)
			{
				return;
			}
			FriendShower component = this.com.GetComponent<FriendShower>();
			if (component == null)
			{
				return;
			}
			string blackName = this.model.GetBlackName(component.ullUid, component.dwLogicWorldID);
			string strContent = string.Format(Singleton<CTextManager>.get_instance().GetText("Black_CancleBlockTip", new string[]
			{
				blackName
			}), new object[0]);
			Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(strContent, enUIEventID.Friend_CancleBlock_Ok, enUIEventID.Friend_CancleBlock_Cancle, false);
		}

		private void OnCancleBlockBtnOK(CUIEvent evt)
		{
			FriendShower component = this.com.GetComponent<FriendShower>();
			if (component == null)
			{
				return;
			}
			FriendSysNetCore.Send_Cancle_Block(component.ullUid, component.dwLogicWorldID);
		}

		private void QQBox_OnClick(CUIEvent uievent)
		{
			if (ApolloConfig.platform == 2)
			{
				MonoSingleton<IDIPSys>.GetInstance().RequestQQBox();
			}
		}

		private void OnFriend_LBS_NoShare(CUIEvent evt)
		{
			Singleton<CFriendContoller>.get_instance().model.EnableShareLocation = !Singleton<CFriendContoller>.get_instance().model.EnableShareLocation;
			bool enableShareLocation = Singleton<CFriendContoller>.get_instance().model.EnableShareLocation;
			FriendSysNetCore.Send_Request_Sns_Switch(2, (!enableShareLocation) ? 0 : 1);
			if (this.view != null)
			{
				this.view.SyncLBSShareBtnState();
				this.view.Refresh_List(this.view.CurTab);
			}
			if (!enableShareLocation)
			{
				FriendSysNetCore.Send_Clear_Location();
			}
			else if (!MonoSingleton<GPSSys>.get_instance().bGetGPSData)
			{
				MonoSingleton<GPSSys>.get_instance().StartGPS();
				Singleton<CUIManager>.get_instance().OpenTips("正在定位，请稍后...", false, 1.5f, null, new object[0]);
			}
			else
			{
				FriendSysNetCore.Send_Report_Clt_Location(MonoSingleton<GPSSys>.get_instance().iLongitude, MonoSingleton<GPSSys>.get_instance().iLatitude);
			}
		}

		public void SetFilter(uint value)
		{
			this.model.fileter = value;
			if (this.view != null)
			{
				this.view.SyncGenderToggleState();
				this.view.Refresh_List(this.view.CurTab);
			}
		}

		private void OnFriend_LBS_Nan(CUIEvent evt)
		{
			CFriendModel cFriendModel = Singleton<CFriendContoller>.get_instance().model;
			if (cFriendModel.fileter != 1u)
			{
				this.SetFilter(cFriendModel.NegFlag(cFriendModel.fileter, 0));
			}
			else
			{
				this.SetFilter(cFriendModel.fileter);
			}
		}

		private void OnFriend_LBS_Nv(CUIEvent evt)
		{
			CFriendModel cFriendModel = Singleton<CFriendContoller>.get_instance().model;
			if (cFriendModel.fileter != 2u)
			{
				this.SetFilter(cFriendModel.NegFlag(cFriendModel.fileter, 1));
			}
			else
			{
				this.SetFilter(cFriendModel.fileter);
			}
		}

		private void OnFriend_LBS_Refresh(CUIEvent evt)
		{
			int iLongitude = MonoSingleton<GPSSys>.get_instance().iLongitude;
			int iLatitude = MonoSingleton<GPSSys>.get_instance().iLatitude;
			bool isShowAlert = evt.m_eventParams.tag == 0;
			if (!CSysDynamicBlock.bFriendBlocked && (iLongitude == 0 || iLatitude == 0))
			{
				string text = Singleton<CTextManager>.get_instance().GetText("LBS_Location_Error");
				Singleton<CUIManager>.get_instance().OpenTips(text, false, 1.5f, null, new object[0]);
				Singleton<CFriendContoller>.get_instance().model.searchLBSZero = text;
				if (this.view != null && this.view.ifnoText != null)
				{
					this.view.ifnoText.text = text;
				}
			}
			FriendSysNetCore.Send_Search_LBS_Req(this.model.GetLBSGenterFilter(), iLongitude, iLatitude, isShowAlert);
			this.startCooldownTimestamp = Singleton<FrameSynchr>.GetInstance().LogicFrameTick;
		}

		private void OnFriend_LBS_CheckInfo(CUIEvent evt)
		{
			FriendShower component = evt.m_srcWidget.transform.parent.parent.parent.GetComponent<FriendShower>();
			if (component == null)
			{
				return;
			}
			Singleton<CPlayerInfoSystem>.get_instance().ShowPlayerDetailInfo(component.ullUid, (int)component.dwLogicWorldID, CPlayerInfoSystem.DetailPlayerInfoSource.DefaultOthers, true);
		}

		private void OnFriend_Room_AddFriend(CUIEvent evt)
		{
			if (evt.m_eventParams.commonUInt64Param1 == 0uL || evt.m_eventParams.commonUInt32Param1 == 0u)
			{
				return;
			}
			this.Open_Friend_Verify(evt.m_eventParams.commonUInt64Param1, evt.m_eventParams.commonUInt32Param1, false, 0, -1, true);
		}

		private void OnIntimacyRela_Menu_Btn_Click(CUIEvent evt)
		{
			if (this.view != null && this.view.intimacyRelationView != null)
			{
				this.view.intimacyRelationView.Open();
			}
			Singleton<CUINewFlagSystem>.GetInstance().DelNewFlag(evt.m_srcWidget, enNewFlagKey.New_Friend_Relation_V1, true);
		}

		private void OnMentorTask_Btn_Click(CUIEvent evt)
		{
			if (this.view != null && this.view.mentorTaskView != null)
			{
				this.view.mentorTaskView.Open();
			}
		}

		private void On_OpenForm(CUIEvent uiEvent)
		{
			this.view.OpenForm(uiEvent);
			CMiShuSystem.SendUIClickToServer(enUIClickReprotID.rp_FrientBtn);
			Singleton<CBattleGuideManager>.GetInstance().OpenBannerDlgByBannerGuideId(5u, null, false);
			Singleton<CUINewFlagSystem>.get_instance().SetNewFlagForFriendEntry(false);
		}

		private void On_CloseForm(CUIEvent uiEvent)
		{
			this.view.CloseForm();
		}

		private void On_OpenAddFriendForm(CUIEvent uiEvent)
		{
			this.view.OpenSearchForm(1);
		}

		public static uint GetMentorGradeLimit()
		{
			return GameDataMgr.globalInfoDatabin.GetDataByKey(271u).dwConfValue;
		}

		private FriendShower GetParentFriendShower(Transform node)
		{
			FriendShower component = node.GetComponent<FriendShower>();
			if (component != null)
			{
				return component;
			}
			if (node.parent != null)
			{
				return this.GetParentFriendShower(node.parent);
			}
			return null;
		}

		public void On_AddSomthing(CUIEvent evt, CFriendModel.FriendType friendType, bool ignoreSearchReasult = false)
		{
			COMDT_FRIEND_INFO cOMDT_FRIEND_INFO = Singleton<CFriendContoller>.GetInstance().search_info;
			enMentorState mentorState = CFriendContoller.GetMentorState(0u, null);
			if (this.model == null)
			{
				return;
			}
			if (evt.m_srcWidgetBelongedListScript == null && cOMDT_FRIEND_INFO != null && !ignoreSearchReasult)
			{
				int num = CLadderSystem.ConvertEloToRank(cOMDT_FRIEND_INFO.RankVal[7]);
				if ((mentorState == enMentorState.IWantMentor || mentorState == enMentorState.IHasMentor) && cOMDT_FRIEND_INFO != null && (long)num < (long)((ulong)CFriendContoller.GetMentorGradeLimit()) && (friendType == CFriendModel.FriendType.MentorRequestList || friendType == CFriendModel.FriendType.MentorRecommend))
				{
					Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_CannotGetApprentice"), false, 1.5f, null, new object[0]);
					return;
				}
				this.Open_Friend_Verify(cOMDT_FRIEND_INFO.stUin.ullUid, cOMDT_FRIEND_INFO.stUin.dwLogicWorldId, true, 0, -1, false);
			}
			else
			{
				FriendShower parentFriendShower = this.GetParentFriendShower(evt.m_srcWidget.transform);
				if (parentFriendShower == null)
				{
					return;
				}
				cOMDT_FRIEND_INFO = this.model.GetInfo(friendType, parentFriendShower.ullUid, parentFriendShower.dwLogicWorldID);
				if (cOMDT_FRIEND_INFO != null)
				{
					int num = CLadderSystem.ConvertEloToRank(cOMDT_FRIEND_INFO.RankVal[7]);
					if ((mentorState == enMentorState.IWantMentor || mentorState == enMentorState.IHasMentor) && cOMDT_FRIEND_INFO != null && (long)num < (long)((ulong)CFriendContoller.GetMentorGradeLimit()) && (friendType == CFriendModel.FriendType.MentorRequestList || friendType == CFriendModel.FriendType.MentorRecommend))
					{
						Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_CannotGetApprentice"), false, 1.5f, null, new object[0]);
						return;
					}
				}
				else
				{
					CSDT_LBS_USER_INFO lBSUserInfo = this.model.GetLBSUserInfo(parentFriendShower.ullUid, parentFriendShower.dwLogicWorldID, CFriendModel.LBSGenderType.Both);
					if (lBSUserInfo != null)
					{
						cOMDT_FRIEND_INFO = lBSUserInfo.stLbsUserInfo;
					}
				}
				if (cOMDT_FRIEND_INFO != null)
				{
					if (this.view != null && this.view.CurTab == CFriendView.Tab.Friend_LBS)
					{
						this.Open_Friend_Verify(cOMDT_FRIEND_INFO.stUin.ullUid, cOMDT_FRIEND_INFO.stUin.dwLogicWorldId, false, 2, -1, false);
					}
					else
					{
						this.Open_Friend_Verify(cOMDT_FRIEND_INFO.stUin.ullUid, cOMDT_FRIEND_INFO.stUin.dwLogicWorldId, false, 0, -1, false);
					}
				}
			}
		}

		private void On_AddFriend(CUIEvent evt)
		{
			if (evt.m_srcFormScript.m_formPath.Equals(CFriendContoller.AddFriendFormPath))
			{
				this.On_AddSomthing(evt, CFriendModel.FriendType.Recommend, false);
			}
			else if (evt.m_srcFormScript.m_formPath.Equals(CFriendContoller.FriendFormPath))
			{
				CFriendContoller.s_addViewtype = 1;
				this.On_AddSomthing(evt, (CFriendModel.FriendType)evt.m_eventParams.tag, true);
			}
		}

		private void On_AddMentor(CUIEvent evt)
		{
			this.On_AddSomthing(evt, CFriendModel.FriendType.MentorRecommend, false);
		}

		public void Open_Friend_Verify(ulong ullUid, uint dwLogicWorldId, bool bAddSearchFirend, COM_ADD_FRIEND_TYPE addFriendType = 0, int useHeroId = -1, bool onlyAddFriend = true)
		{
			if (this.view != null && this.view.verficationView != null)
			{
				this.view.verficationView.Open(ullUid, dwLogicWorldId, bAddSearchFirend, addFriendType, useHeroId, onlyAddFriend);
			}
		}

		private void On_Accept_RequestFriend(CUIEvent uievent)
		{
			FriendShower component = uievent.m_srcWidget.transform.parent.parent.parent.GetComponent<FriendShower>();
			if (component == null)
			{
				return;
			}
			COMDT_FRIEND_INFO info = this.model.GetInfo(CFriendModel.FriendType.RequestFriend, component.ullUid, component.dwLogicWorldID);
			if (info == null)
			{
				return;
			}
			FriendSysNetCore.Send_Confrim_BeFriend(info.stUin);
		}

		private void On_Refuse_RequestFriend(CUIEvent uievent)
		{
			FriendShower component = uievent.m_srcWidget.transform.parent.parent.parent.GetComponent<FriendShower>();
			if (component == null)
			{
				return;
			}
			COMDT_FRIEND_INFO info = this.model.GetInfo(CFriendModel.FriendType.RequestFriend, component.ullUid, component.dwLogicWorldID);
			if (info == null)
			{
				return;
			}
			FriendSysNetCore.Send_DENY_BeFriend(info.stUin);
		}

		private void OnMentor_AcceptRequest(CUIEvent uievent)
		{
			FriendShower component = uievent.m_srcWidget.transform.parent.parent.parent.GetComponent<FriendShower>();
			this.SendMentorRequestConfirmRefuse(component, 1);
		}

		private void OnMentor_RefuseRequest(CUIEvent uievent)
		{
			FriendShower component = uievent.m_srcWidget.transform.parent.parent.parent.GetComponent<FriendShower>();
			this.SendMentorRequestConfirmRefuse(component, 2);
		}

		public static int GetMentorServerState()
		{
			int result = -1;
			switch (CFriendContoller.GetMentorState(0u, null))
			{
			case enMentorState.IWantMentor:
			case enMentorState.IHasMentor:
				result = 3;
				break;
			case enMentorState.IWantApprentice:
			case enMentorState.IHasApprentice:
				result = 2;
				break;
			}
			return result;
		}

		public static int GetSrvMentorTypeByItemType(FriendShower.ItemType itemType)
		{
			switch (itemType)
			{
			case FriendShower.ItemType.AddMentor:
			case FriendShower.ItemType.Mentor:
				return 2;
			case FriendShower.ItemType.AddApprentice:
			case FriendShower.ItemType.Apprentice:
				return 3;
			default:
				return 0;
			}
		}

		private void SendMentorRequestConfirmRefuse(FriendShower shower, COM_MASTERCOMFIRM_TYPE confType)
		{
			if (shower == null)
			{
				return;
			}
			COMDT_FRIEND_INFO info = this.model.GetInfo(CFriendModel.FriendType.MentorRequestList, shower.ullUid, shower.dwLogicWorldID);
			if (info == null)
			{
				return;
			}
			stFriendVerifyContent friendVerifyData = this.model.GetFriendVerifyData(info.stUin.ullUid, info.stUin.dwLogicWorldId, CFriendModel.enVerifyDataSet.Mentor);
			if (friendVerifyData == null)
			{
				return;
			}
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5404u);
			cSPkg.stPkgData.get_stConfirmMasterReq().bConfirmType = confType;
			cSPkg.stPkgData.get_stConfirmMasterReq().stUin.ullUid = info.stUin.ullUid;
			cSPkg.stPkgData.get_stConfirmMasterReq().stUin.dwLogicWorldId = info.stUin.dwLogicWorldId;
			cSPkg.stPkgData.get_stConfirmMasterReq().bReqType = (byte)friendVerifyData.mentorType;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		public void OnMentorInfoAdd(CSPkg msg)
		{
			SCPKG_MASTERSTUDENT_ADD stMasterStudentAdd = msg.stPkgData.get_stMasterStudentAdd();
			if (Utility.IsCanShowPrompt())
			{
				Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("Mentor_RelationshipMade", new string[]
				{
					UT.Bytes2String(stMasterStudentAdd.stDetailInfo.szUserName)
				}), false, 1.5f, null, new object[0]);
			}
			byte bType = stMasterStudentAdd.bType;
			if (bType != 2)
			{
				if (bType == 3)
				{
					this.Add_And_Refresh(CFriendModel.FriendType.Apprentice, stMasterStudentAdd.stDetailInfo);
				}
			}
			else
			{
				this.Add_And_Refresh(CFriendModel.FriendType.Mentor, stMasterStudentAdd.stDetailInfo);
			}
		}

		public void OnMentorInfoRemove(CSPkg msg)
		{
			SCPKG_MASTERSTUDENT_DEL stMasterStudentDel = msg.stPkgData.get_stMasterStudentDel();
			byte bType = stMasterStudentDel.bType;
			if (bType != 2)
			{
				if (bType == 3)
				{
					this.Remove_And_Refresh(CFriendModel.FriendType.Apprentice, stMasterStudentDel.stUin);
				}
			}
			else
			{
				this.Remove_And_Refresh(CFriendModel.FriendType.Mentor, stMasterStudentDel.stUin);
			}
		}

		public void OnMentor_Reqest_NTF(CSPkg msg)
		{
			SCPKG_MASTERREQ_NTF stMasterReqNtf = msg.stPkgData.get_stMasterReqNtf();
			if (stMasterReqNtf == null)
			{
				return;
			}
			this.model.AddFriendVerifyContent(stMasterReqNtf.stInfo.stReqUserInfo.stUin.ullUid, stMasterReqNtf.stInfo.stReqUserInfo.stUin.dwLogicWorldId, StringHelper.BytesToString(stMasterReqNtf.stInfo.szVerificationInfo), CFriendModel.enVerifyDataSet.Mentor, null, (int)stMasterReqNtf.stInfo.bType);
			this.Add_And_Refresh(CFriendModel.FriendType.MentorRequestList, stMasterReqNtf.stInfo.stReqUserInfo);
		}

		private void On_DelFriend(CUIEvent evt)
		{
			this.com = evt.m_srcWidget;
			string text = UT.GetText("Friend_Tips_DelFriend");
			int tag = evt.m_eventParams.tag;
			if (tag == 8 || tag == 9)
			{
				text = Singleton<CTextManager>.GetInstance().GetText("Mentor_DelConfirm");
			}
			Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(text, enUIEventID.Friend_DelFriend_OK, enUIEventID.Friend_DelFriend_Cancle, evt.m_eventParams, false);
		}

		private void On_DelFriend_OK(CUIEvent evt)
		{
			FriendShower component = this.com.transform.parent.parent.parent.GetComponent<FriendShower>();
			if (component == null)
			{
				return;
			}
			COMDT_FRIEND_INFO info = this.model.GetInfo(CFriendContoller.GetFriendTypeFromItemType((FriendShower.ItemType)evt.m_eventParams.tag), component.ullUid, component.dwLogicWorldID);
			if (info == null)
			{
				return;
			}
			FriendSysNetCore.Send_Del_Friend(info, (FriendShower.ItemType)evt.m_eventParams.tag);
		}

		public static CFriendModel.FriendType GetFriendTypeFromItemType(FriendShower.ItemType itemType)
		{
			if (itemType == FriendShower.ItemType.Mentor)
			{
				return CFriendModel.FriendType.Mentor;
			}
			if (itemType != FriendShower.ItemType.Apprentice)
			{
				return CFriendModel.FriendType.GameFriend;
			}
			return CFriendModel.FriendType.Apprentice;
		}

		private void On_Friend_DelFriend_Cancle(CUIEvent evt)
		{
		}

		private void OnMentor_IWant(CUIEvent evt)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			uint dwConfValue = GameDataMgr.globalInfoDatabin.GetDataByKey(271u).dwConfValue;
			switch (CFriendContoller.GetMentorState(0u, null))
			{
			case enMentorState.IWantMentor:
				this.view.OpenSearchForm(2);
				break;
			case enMentorState.IWantApprentice:
			case enMentorState.IHasApprentice:
				if ((uint)masterRoleInfo.m_rankHistoryHighestGrade < dwConfValue)
				{
					Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_Grade2LowAsMentor"), false, 1.5f, null, new object[0]);
				}
				else
				{
					this.view.OpenSearchForm(3);
				}
				break;
			case enMentorState.IHasMentor:
				Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("Mentor_Err_HasMentor"), false, 1.5f, null, new object[0]);
				break;
			}
		}

		private void OnMentor_RequestListClick(CUIEvent evt)
		{
			this.view.OpenMentorRequestForm();
			this.view.Refresh();
		}

		private void OnMentorTabChange(CUIEvent evt)
		{
			this.view.OnMentorTabChange(evt);
		}

		public void OnMasterStudentInfo(CSPkg msg)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			SCPKG_MASTERSTUDENT_INFO stMasterStudentInfo = msg.stPkgData.get_stMasterStudentInfo();
			CFriendContoller.m_mentorInfo = stMasterStudentInfo;
			if (stMasterStudentInfo.stMaster.stFriendInfo.stUin.ullUid != 0uL)
			{
				COMDT_FRIEND_INFO expr_42 = stMasterStudentInfo.stMaster.stFriendInfo;
				expr_42.bStudentType |= 0;
				this.model.Add(CFriendModel.FriendType.Mentor, stMasterStudentInfo.stMaster.stFriendInfo, false);
				this.Handle_CoinSend_Data_Mentor(stMasterStudentInfo.stMaster);
				this.model.SetFriendGameState(stMasterStudentInfo.stMaster.stFriendInfo.stUin.ullUid, stMasterStudentInfo.stMaster.stFriendInfo.stUin.dwLogicWorldId, stMasterStudentInfo.stMaster.bGameState, stMasterStudentInfo.stMaster.dwGameStartTime, string.Empty, false);
				this.model.AddFriendIntimacy(stMasterStudentInfo.stMaster.stFriendInfo.stUin, stMasterStudentInfo.stMaster.stIntimacyData);
			}
			for (int i = 0; i < (int)stMasterStudentInfo.bStudentNum; i++)
			{
				COMDT_FRIEND_INFO expr_FF = stMasterStudentInfo.astStudentList[i].stFriendInfo;
				expr_FF.bStudentType |= 16;
				this.model.Add(CFriendModel.FriendType.Apprentice, stMasterStudentInfo.astStudentList[i].stFriendInfo, false);
				this.Handle_CoinSend_Data_Apprentice(stMasterStudentInfo.astStudentList[i]);
				this.model.SetFriendGameState(stMasterStudentInfo.astStudentList[i].stFriendInfo.stUin.ullUid, stMasterStudentInfo.astStudentList[i].stFriendInfo.stUin.dwLogicWorldId, stMasterStudentInfo.astStudentList[i].bGameState, stMasterStudentInfo.astStudentList[i].dwGameStartTime, string.Empty, false);
				this.model.AddFriendIntimacy(stMasterStudentInfo.astStudentList[i].stFriendInfo.stUin, stMasterStudentInfo.astStudentList[i].stIntimacyData);
			}
			enMentorState mentorState = CFriendContoller.GetMentorState(0u, null);
			if (this.view != null && this.view.IsActive())
			{
				this.view.Refresh();
			}
		}

		public void OnMentor_GetAccountData(CSPkg msg)
		{
			CPlayerProfile profile = Singleton<CPlayerInfoSystem>.get_instance().GetProfile();
			if (profile._mentorInfo != null)
			{
				profile._mentorInfo.dwMasterPoint = msg.stPkgData.get_stMasterAcntDataNtf().dwMasterPoint;
				profile._mentorInfo.dwFinishStudentNum = msg.stPkgData.get_stMasterAcntDataNtf().dwFinishStudentNum;
				profile._mentorInfo.dwStudentNum = msg.stPkgData.get_stMasterAcntDataNtf().dwProcessStudentNum;
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null && masterRoleInfo.m_mentorInfo != null)
			{
				masterRoleInfo.m_mentorInfo.dwMasterPoint = msg.stPkgData.get_stMasterAcntDataNtf().dwMasterPoint;
				masterRoleInfo.m_mentorInfo.dwFinishStudentNum = msg.stPkgData.get_stMasterAcntDataNtf().dwFinishStudentNum;
				masterRoleInfo.m_mentorInfo.dwStudentNum = msg.stPkgData.get_stMasterAcntDataNtf().dwProcessStudentNum;
			}
			Singleton<CPlayerMentorInfoController>.GetInstance().UpdateUI();
		}

		public void OnMentor_GraduateNtf(CSPkg msg)
		{
			byte bType = msg.stPkgData.get_stGraduateNtf().bType;
			if (bType != 1)
			{
				if (bType == 2)
				{
					COMDT_FRIEND_INFO friendByUid = this.model.getFriendByUid(msg.stPkgData.get_stGraduateNtf().stStudentUin.ullUid, CFriendModel.FriendType.Apprentice, 0u);
					if (friendByUid != null)
					{
						friendByUid.bStudentType = ((friendByUid.bStudentType & 240) | 2);
					}
					CPlayerProfile profile = Singleton<CPlayerInfoSystem>.get_instance().GetProfile();
					if (profile != null)
					{
						profile._mentorInfo.dwFinishStudentNum += 1u;
					}
					CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
					if (masterRoleInfo != null && masterRoleInfo.m_mentorInfo != null)
					{
						masterRoleInfo.m_mentorInfo.dwFinishStudentNum += 1u;
					}
				}
			}
			else if (CFriendContoller.m_mentorInfo != null)
			{
				CFriendContoller.m_mentorInfo.bStudentType = 2;
			}
		}

		public void OnMentor_GetStudentList(CSPkg msg)
		{
			CFriendModel.FriendType type = CFriendModel.FriendType.Apprentice;
			int bListType = (int)msg.stPkgData.get_stGetStudentListRsp().bListType;
			enMentorRelationType enMentorRelationType = enMentorRelationType.schoolmate;
			CS_STUDENTLIST_TYPE cS_STUDENTLIST_TYPE = bListType;
			if (cS_STUDENTLIST_TYPE != 1)
			{
				if (cS_STUDENTLIST_TYPE == 2)
				{
					type = CFriendModel.FriendType.Mentor;
					enMentorRelationType = enMentorRelationType.schoolmate;
				}
			}
			else
			{
				type = CFriendModel.FriendType.Apprentice;
				enMentorRelationType = enMentorRelationType.apprentice;
			}
			this.m_mentorListOff[bListType].m_type = bListType;
			this.m_mentorListOff[bListType].m_isEnd = (msg.stPkgData.get_stGetStudentListRsp().dwOver != 0u);
			int num = 0;
			while ((long)num < (long)((ulong)msg.stPkgData.get_stGetStudentListRsp().dwStudentNum))
			{
				COMDT_FRIEND_INFO cOMDT_FRIEND_INFO = msg.stPkgData.get_stGetStudentListRsp().astStudentList[num];
				COMDT_FRIEND_INFO expr_92 = cOMDT_FRIEND_INFO;
				expr_92.bStudentType |= (byte)(enMentorRelationType << 4);
				this.model.Add(type, cOMDT_FRIEND_INFO, false);
				num++;
			}
			if (this.view != null && this.view.IsActive())
			{
				this.view.Refresh();
			}
		}

		private void OnMentorGetMoreMentorList(CUIEvent uievt)
		{
			CS_STUDENTLIST_TYPE tag = uievt.m_eventParams.tag;
			FriendSysNetCore.SendGetStudentListReq(tag);
		}

		public bool HasMentor(SCPKG_MASTERSTUDENT_INFO info = null)
		{
			if (info != null && info != CFriendContoller.m_mentorInfo)
			{
				return info.stMaster.stFriendInfo.stUin.ullUid != 0uL;
			}
			ListView<COMDT_FRIEND_INFO> list = Singleton<CFriendContoller>.get_instance().model.GetList(CFriendModel.FriendType.Mentor);
			if (list == null)
			{
				return false;
			}
			for (int i = 0; i < list.get_Count(); i++)
			{
				if ((list.get_Item(i).bStudentType & 240) >> 4 != 2)
				{
					return true;
				}
			}
			return false;
		}

		public bool HasApprentice(SCPKG_MASTERSTUDENT_INFO info = null)
		{
			if (info == null || info == CFriendContoller.m_mentorInfo)
			{
				ListView<COMDT_FRIEND_INFO> list = Singleton<CFriendContoller>.get_instance().model.GetList(CFriendModel.FriendType.Apprentice);
				return list != null && list.get_Count() != 0;
			}
			return info.bStudentNum != 0;
		}

		public static enMentorState GetMentorState(uint iLevel = 0u, SCPKG_MASTERSTUDENT_INFO info = null)
		{
			if (iLevel == 0u)
			{
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				if (CFriendContoller.m_mentorInfo == null)
				{
					return enMentorState.None;
				}
				if (masterRoleInfo != null)
				{
					iLevel = masterRoleInfo.Level;
				}
			}
			if (info == null)
			{
				info = CFriendContoller.m_mentorInfo;
			}
			if (iLevel != 0u)
			{
				uint dwConfValue = GameDataMgr.globalInfoDatabin.GetDataByKey(270u).dwConfValue;
				if (iLevel <= dwConfValue)
				{
					if (!Singleton<CFriendContoller>.GetInstance().HasMentor(info))
					{
						return enMentorState.IWantMentor;
					}
					return enMentorState.IHasMentor;
				}
				else
				{
					uint dwConfValue2 = GameDataMgr.globalInfoDatabin.GetDataByKey(269u).dwConfValue;
					if (iLevel >= dwConfValue2)
					{
						if (!Singleton<CFriendContoller>.GetInstance().HasApprentice(info))
						{
							return enMentorState.IWantApprentice;
						}
						return enMentorState.IHasApprentice;
					}
				}
			}
			return enMentorState.None;
		}

		public int RefreshMentorTabData()
		{
			int num = 0;
			CFriendContoller.MentorTabMask = 0;
			if (CFriendContoller.s_mentorTabStr == null)
			{
				CFriendContoller.s_mentorTabStr = new string[2];
				for (int i = 0; i < 2; i++)
				{
					CFriendContoller.s_mentorTabStr[i] = Singleton<CTextManager>.GetInstance().GetText(CFriendContoller.s_mentorTabName[i]);
				}
			}
			if (CFriendContoller.m_mentorInfo == null)
			{
				return num;
			}
			if (Singleton<CFriendContoller>.GetInstance().HasMentor(null))
			{
				CFriendContoller.MentorTabMask |= 1;
				num++;
			}
			if (Singleton<CFriendContoller>.GetInstance().HasApprentice(null))
			{
				CFriendContoller.MentorTabMask |= 2;
				num++;
			}
			return num;
		}

		public void OnMentorApplyVerifyBoxRetrun(string str)
		{
			if (this.m_mentorSelectedUin == null)
			{
				return;
			}
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5402u);
			cSPkg.stPkgData.get_stApplyMasterReq().bType = (byte)CFriendContoller.s_addViewtype;
			cSPkg.stPkgData.get_stApplyMasterReq().stUin.dwLogicWorldId = this.m_mentorSelectedUin.dwLogicWorldId;
			cSPkg.stPkgData.get_stApplyMasterReq().stUin.ullUid = this.m_mentorSelectedUin.ullUid;
			StringHelper.StringToUTF8Bytes(str, ref cSPkg.stPkgData.get_stApplyMasterReq().szVerificationInfo);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		public string GetMentorStateString()
		{
			switch (CFriendContoller.GetMentorState(0u, null))
			{
			case enMentorState.IWantMentor:
			case enMentorState.IHasMentor:
				return Singleton<CTextManager>.GetInstance().GetText("Mentor_GetMentor");
			case enMentorState.IWantApprentice:
			case enMentorState.IHasApprentice:
				return Singleton<CTextManager>.GetInstance().GetText("Mentor_GetApprentice");
			default:
				return null;
			}
		}

		private void On_Friend_SendCoin(CUIEvent uievent)
		{
			if (uievent.m_eventParams.tag == -1)
			{
				Debug.LogError("Error server friend type");
			}
			FriendSysNetCore.Send_FriendCoin(new COMDT_ACNT_UNIQ
			{
				ullUid = uievent.m_eventParams.commonUInt64Param1,
				dwLogicWorldId = (uint)uievent.m_eventParams.commonUInt64Param2
			}, uievent.m_eventParams.tag);
		}

		private void On_SNSFriend_SendCoin(CUIEvent uievent)
		{
			ulong commonUInt64Param = uievent.m_eventParams.commonUInt64Param1;
			uint dwLogicWorldID = (uint)uievent.m_eventParams.commonUInt64Param2;
			COMDT_FRIEND_INFO info = this.model.GetInfo(CFriendModel.FriendType.SNS, commonUInt64Param, dwLogicWorldID);
			if (info == null)
			{
				return;
			}
			FriendSysNetCore.Send_FriendCoin(info.stUin, 2);
		}

		private ulong GetFriendUid(CUIEvent uiEvent)
		{
			Transform parent = uiEvent.m_srcWidget.transform.parent.parent.parent;
			FriendShower component = parent.GetComponent<FriendShower>();
			if (component == null)
			{
				return 0uL;
			}
			CFriendView.Tab selectedTab = this.view.GetSelectedTab();
			COMDT_FRIEND_INFO info;
			if (selectedTab != CFriendView.Tab.Mentor)
			{
				CFriendModel.FriendType type = (this.view.GetSelectedTab() != CFriendView.Tab.Friend_SNS) ? CFriendModel.FriendType.GameFriend : CFriendModel.FriendType.SNS;
				info = this.model.GetInfo(type, component.ullUid, component.dwLogicWorldID);
			}
			else
			{
				info = this.model.GetInfo(CFriendModel.FriendType.Mentor, component.ullUid, component.dwLogicWorldID);
				if (info == null)
				{
					info = this.model.GetInfo(CFriendModel.FriendType.Apprentice, component.ullUid, component.dwLogicWorldID);
				}
			}
			if (info == null)
			{
				return 0uL;
			}
			return info.stUin.ullUid;
		}

		private void On_Friend_InviteGuild(CUIEvent uiEvent)
		{
			Singleton<EventRouter>.GetInstance().BroadCastEvent<ulong>("Guild_Invite", this.GetFriendUid(uiEvent));
		}

		private void On_Friend_RecommendGuild(CUIEvent uiEvent)
		{
			Singleton<EventRouter>.GetInstance().BroadCastEvent<ulong>("Guild_Recommend", this.GetFriendUid(uiEvent));
		}

		private void On_Friend_CheckInfo(CUIEvent uievent)
		{
			FriendShower component = uievent.m_srcWidget.transform.parent.parent.parent.GetComponent<FriendShower>();
			if (component == null)
			{
				return;
			}
			Singleton<CPlayerInfoSystem>.get_instance().ShowPlayerDetailInfo(component.ullUid, (int)component.dwLogicWorldID, CPlayerInfoSystem.DetailPlayerInfoSource.DefaultOthers, true);
		}

		private void On_FriendRecommend_CheckInfo(CUIEvent uievent)
		{
			FriendShower component = uievent.m_srcWidget.transform.parent.parent.GetComponent<FriendShower>();
			if (component == null)
			{
				return;
			}
			Singleton<CPlayerInfoSystem>.get_instance().ShowPlayerDetailInfo(component.ullUid, (int)component.dwLogicWorldID, CPlayerInfoSystem.DetailPlayerInfoSource.DefaultOthers, true);
		}

		private void OnMentor_OpenPrivilegePage(CUIEvent uievt)
		{
			CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(CFriendContoller.MentorPrivilegeFormPath, false, true);
			CFriendView.MentorPrivilege_SetMainList(cUIFormScript);
			CFriendView.MentorPrivilege_Refresh(cUIFormScript, 1);
			this.m_mentorPrivilegePage = 1;
			GameObject widget = cUIFormScript.GetWidget(5);
			widget.CustomSetActive(false);
		}

		private void OnMentor_PrivilegeLeftClick(CUIEvent uievt)
		{
			CUIFormScript srcFormScript = uievt.m_srcFormScript;
			CUIListScript component = srcFormScript.GetWidget(8).GetComponent<CUIListScript>();
			GameObject widget = srcFormScript.GetWidget(5);
			GameObject widget2 = srcFormScript.GetWidget(6);
			this.m_mentorPrivilegePage--;
			if (this.m_mentorPrivilegePage <= 1)
			{
				this.m_mentorPrivilegePage = 1;
				widget.CustomSetActive(false);
			}
			else
			{
				widget.CustomSetActive(true);
			}
			widget2.CustomSetActive(true);
			component.MoveElementInScrollArea(this.m_mentorPrivilegePage - 1, false);
			CFriendView.MentorPrivilege_Refresh(srcFormScript, this.m_mentorPrivilegePage);
		}

		private void OnMentor_PrivilegeRightClick(CUIEvent uievt)
		{
			CUIFormScript srcFormScript = uievt.m_srcFormScript;
			CUIListScript component = srcFormScript.GetWidget(8).GetComponent<CUIListScript>();
			int count = GameDataMgr.famousMentorDatabin.count;
			GameObject widget = srcFormScript.GetWidget(5);
			GameObject widget2 = srcFormScript.GetWidget(6);
			this.m_mentorPrivilegePage++;
			if (this.m_mentorPrivilegePage >= count)
			{
				this.m_mentorPrivilegePage = count;
				widget2.CustomSetActive(false);
			}
			else
			{
				widget2.CustomSetActive(true);
			}
			widget.CustomSetActive(true);
			component.MoveElementInScrollArea(this.m_mentorPrivilegePage - 1, false);
			CFriendView.MentorPrivilege_Refresh(srcFormScript, this.m_mentorPrivilegePage);
		}

		private void OnMentor_PrivilegeListEnable(CUIEvent uievt)
		{
			CUIFormScript srcFormScript = uievt.m_srcFormScript;
			CUIListScript component = srcFormScript.GetWidget(8).GetComponent<CUIListScript>();
			CFriendView.MentorPrivilegeMainList_OnEnable(uievt);
		}

		private void On_Friend_List_ElementEnable(CUIEvent uievent)
		{
			if (this.view != null && this.view.IsActive())
			{
				this.view.On_List_ElementEnable(uievent);
			}
		}

		private void On_MentorRequestListEnable(CUIEvent uievt)
		{
			COMDT_FRIEND_INFO infoAtIndex = Singleton<CFriendContoller>.GetInstance().model.GetInfoAtIndex(CFriendModel.FriendType.MentorRequestList, uievt.m_srcWidgetIndexInBelongedList);
			FriendShower component = uievt.m_srcWidget.GetComponent<FriendShower>();
			UT.ShowFriendData(infoAtIndex, component, FriendShower.ItemType.MentorRequest, false, CFriendModel.FriendType.MentorRequestList);
		}

		private void On_Friend_Invite_SNS_Friend(CUIEvent uievent)
		{
			if (this.view != null && this.view.IsActive())
			{
				this.view.On_Friend_Invite_SNS_Friend(uievent);
			}
		}

		private void On_Friend_Share_SendCoin(CUIEvent uievent)
		{
			try
			{
				if (MonoSingleton<ShareSys>.get_instance().IsInstallPlatform())
				{
					string openId = uievent.m_eventParams.snsFriendEventParams.openId;
					Singleton<ApolloHelper>.get_instance().ShareSendHeart(openId, Singleton<CTextManager>.get_instance().GetText("Common_Sns_Tips_1"), Singleton<CTextManager>.get_instance().GetText("Common_Sns_Tips_2"), ShareSys.SNS_SHARE_SEND_HEART);
					Singleton<CUIManager>.get_instance().OpenTips("Common_Sns_Tips_7", true, 1.5f, null, new object[0]);
				}
			}
			catch (Exception ex)
			{
				DebugHelper.Assert(false, "Exception in On_Friend_Share_SendCoin, {0}", new object[]
				{
					ex.get_Message()
				});
			}
		}

		public void ShareTo_SNSFriend_ReCall(string openId)
		{
			if (!MonoSingleton<ShareSys>.get_instance().IsInstallPlatform())
			{
				return;
			}
			string text = Singleton<CTextManager>.get_instance().GetText("Common_Sns_Tips_11");
			string text2 = Singleton<CTextManager>.get_instance().GetText("Common_Sns_Tips_12");
			Singleton<ApolloHelper>.get_instance().ShareInviteFriend(openId, text, text2, ShareSys.SNS_SHARE_RECALL_FRIEND);
			Singleton<CUIManager>.get_instance().OpenTips("Common_Sns_Tips_13", true, 1.5f, null, new object[0]);
		}

		private void On_Friend_SNS_ReCall(CUIEvent uievent)
		{
			FriendShower component = uievent.m_srcWidget.transform.parent.parent.parent.GetComponent<FriendShower>();
			if (component == null)
			{
				return;
			}
			COMDT_FRIEND_INFO info = this.model.GetInfo(CFriendModel.FriendType.SNS, component.ullUid, component.dwLogicWorldID);
			if (info == null)
			{
				return;
			}
			FriendSysNetCore.ReCallSnsFriend(info.stUin, 2);
			if (!CFriendModel.IsOnSnsSwitch(info.dwRefuseFriendBits, 0))
			{
				this.ShareTo_SNSFriend_ReCall(Utility.UTF8Convert(info.szOpenId));
			}
		}

		private void On_Friend_OB_Click(CUIEvent uievent)
		{
			FriendShower component = uievent.m_srcWidget.transform.parent.parent.parent.GetComponent<FriendShower>();
			if (component == null)
			{
				return;
			}
			COMDT_FRIEND_INFO info = this.model.GetInfo(CFriendModel.FriendType.SNS, component.ullUid, component.dwLogicWorldID);
			if (info == null)
			{
				info = this.model.GetInfo(CFriendModel.FriendType.GameFriend, component.ullUid, component.dwLogicWorldID);
			}
			if (info == null)
			{
				return;
			}
			Singleton<COBSystem>.get_instance().OBFriend(info.stUin);
		}

		private void OnNewDayNtf()
		{
			this.model.SnsReCallData.Clear();
			this.model.HeartData.Clear();
			if (this.view != null && this.view.IsActive())
			{
				this.view.Refresh();
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				masterRoleInfo.getFriendCoinCnt = 0;
				Singleton<CMailSys>.get_instance().UpdateUnReadNum();
			}
		}

		public void On_SCID_CMD_BLACKLIST(CSPkg msg)
		{
			SCPKG_CMD_BLACKLIST stBlackListProfile = msg.stPkgData.get_stBlackListProfile();
			for (int i = 0; i < (int)stBlackListProfile.wBlackListNum; i++)
			{
				COMDT_FRIEND_INFO cOMDT_FRIEND_INFO = stBlackListProfile.astBlackList[i];
				if (cOMDT_FRIEND_INFO != null)
				{
					this.model.AddFriendBlack(cOMDT_FRIEND_INFO);
				}
			}
		}

		public void OnSCPKG_CMD_DEFRIEND(CSPkg msg)
		{
			SCPKG_CMD_DEFRIEND stDeFriendRsp = msg.stPkgData.get_stDeFriendRsp();
			if (stDeFriendRsp.dwResult == 0u)
			{
				COMDT_FRIEND_INFO cOMDT_FRIEND_INFO;
				COMDT_CHAT_PLAYER_INFO cOMDT_CHAT_PLAYER_INFO;
				this.model.GetUser(stDeFriendRsp.stUin.ullUid, stDeFriendRsp.stUin.dwLogicWorldId, out cOMDT_FRIEND_INFO, out cOMDT_CHAT_PLAYER_INFO);
				if (cOMDT_FRIEND_INFO == null)
				{
					if (cOMDT_CHAT_PLAYER_INFO != null)
					{
						this.model.AddFriendBlack(cOMDT_CHAT_PLAYER_INFO, stDeFriendRsp.bGender, stDeFriendRsp.dwLastLoginTime);
						if (this.view != null && this.view.IsActive())
						{
							this.view.Refresh();
						}
					}
					else
					{
						DebugHelper.Assert(false, string.Concat(new object[]
						{
							"---black 找到不到 ulluid:",
							stDeFriendRsp.stUin.ullUid,
							",worldID:",
							stDeFriendRsp.stUin.dwLogicWorldId,
							",对应的玩家数据"
						}));
					}
				}
				else
				{
					this.model.AddFriendBlack(cOMDT_FRIEND_INFO);
					if (this.view != null && this.view.IsActive())
					{
						this.view.Refresh();
					}
				}
				Singleton<EventRouter>.GetInstance().BroadCastEvent(EventID.PlayerBlock_Success);
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips(UT.ErrorCode_String(stDeFriendRsp.dwResult), false, 1.5f, null, new object[0]);
			}
		}

		public void OnSCPKG_CMD_CANCEL_DEFRIEND(CSPkg msg)
		{
			SCPKG_CMD_CANCEL_DEFRIEND stCancelDeFriendRsp = msg.stPkgData.get_stCancelDeFriendRsp();
			if (stCancelDeFriendRsp.dwResult == 0u)
			{
				this.model.RemoveFriendBlack(stCancelDeFriendRsp.stUin.ullUid, stCancelDeFriendRsp.stUin.dwLogicWorldId);
				if (this.view != null && this.view.IsActive())
				{
					this.view.Refresh();
				}
			}
			else
			{
				Singleton<CUIManager>.get_instance().OpenTips("---black OnSCPKG_CMD_CANCEL_DEFRIEND error code:" + stCancelDeFriendRsp.dwResult, false, 1.5f, null, new object[0]);
			}
		}

		public void OnSCPKG_CMD_BLACKFORFRIENDREQ(CSPkg msg)
		{
		}
	}
}
