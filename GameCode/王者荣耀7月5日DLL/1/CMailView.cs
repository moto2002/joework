using Assets.Scripts.GameSystem;
using Assets.Scripts.UI;
using CSProtocol;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CMailView
{
	private CUIFormScript m_CUIForm;

	private CUIListScript m_CUIListScriptTab;

	private GameObject m_panelFri;

	private GameObject m_panelSys;

	private GameObject m_panelMsg;

	private int m_friUnReadNum;

	private int m_sysUnReadNum;

	private int m_msgUnReadNum;

	private GameObject m_SysDeleteBtn;

	private GameObject m_allReceiveSysButton;

	private GameObject m_allReceiveFriButton;

	private GameObject m_allDeleteMsgCenterButton;

	private COM_MAIL_TYPE m_curMailtype = 1;

	public COM_MAIL_TYPE CurMailType
	{
		get
		{
			return this.m_curMailtype;
		}
		set
		{
			if (this.m_CUIListScriptTab == null)
			{
				return;
			}
			this.m_curMailtype = value;
			if (this.m_curMailtype == 2)
			{
				this.SetActiveTab(0);
				this.m_CUIListScriptTab.SelectElement(0, true);
			}
			else if (this.m_curMailtype == 1)
			{
				this.SetActiveTab(1);
				this.m_CUIListScriptTab.SelectElement(1, true);
			}
			else if (this.m_curMailtype == 3)
			{
				this.SetActiveTab(2);
				this.m_CUIListScriptTab.SelectElement(2, true);
			}
		}
	}

	public void Open(COM_MAIL_TYPE mailType)
	{
		this.m_CUIForm = Singleton<CUIManager>.GetInstance().OpenForm(CMailSys.MAIL_FORM_PATH, false, true);
		if (this.m_CUIForm == null)
		{
			return;
		}
		this.m_CUIListScriptTab = this.m_CUIForm.transform.FindChild("TopCommon/Panel_Menu/ListMenu").GetComponent<CUIListScript>();
		this.m_CUIListScriptTab.SetElementAmount(3);
		this.m_CUIListScriptTab.GetElemenet(0).transform.FindChild("Text").GetComponent<Text>().text = Singleton<CTextManager>.GetInstance().GetText("Mail_Friend");
		this.m_CUIListScriptTab.GetElemenet(1).transform.FindChild("Text").GetComponent<Text>().text = Singleton<CTextManager>.GetInstance().GetText("Mail_System");
		this.m_CUIListScriptTab.GetElemenet(2).transform.FindChild("Text").GetComponent<Text>().text = Singleton<CTextManager>.GetInstance().GetText("Mail_MsgCenter");
		this.m_CUIListScriptTab.GetElemenet(0).GetComponent<CUIEventScript>().SetUIEvent(enUIEventType.Click, enUIEventID.Mail_TabFriend);
		this.m_CUIListScriptTab.GetElemenet(1).GetComponent<CUIEventScript>().SetUIEvent(enUIEventType.Click, enUIEventID.Mail_TabSystem);
		this.m_CUIListScriptTab.GetElemenet(2).GetComponent<CUIEventScript>().SetUIEvent(enUIEventType.Click, enUIEventID.Mail_TabMsgCenter);
		this.m_panelFri = this.m_CUIForm.transform.FindChild("PanelFriMail").gameObject;
		this.m_panelSys = this.m_CUIForm.transform.FindChild("PanelSysMail").gameObject;
		this.m_panelMsg = this.m_CUIForm.transform.FindChild("PanelMsgMail").gameObject;
		this.m_SysDeleteBtn = this.m_panelSys.transform.FindChild("ButtonGrid/DeleteButton").gameObject;
		this.m_allReceiveSysButton = this.m_panelSys.transform.FindChild("ButtonGrid/AllReceiveButton").gameObject;
		this.m_allReceiveFriButton = this.m_panelFri.transform.FindChild("AllReceiveButton").gameObject;
		this.m_allDeleteMsgCenterButton = this.m_panelMsg.transform.FindChild("AllDeleteButton").gameObject;
		this.SetUnReadNum(2, this.m_friUnReadNum);
		this.SetUnReadNum(1, this.m_sysUnReadNum);
		this.SetUnReadNum(3, this.m_msgUnReadNum);
		this.CurMailType = mailType;
	}

	public void Close()
	{
		Singleton<CUIManager>.GetInstance().CloseForm(CMailSys.MAIL_FORM_PATH);
		this.OnClose();
	}

	public void OnClose()
	{
		this.m_CUIForm = null;
		this.m_CUIListScriptTab = null;
		this.m_panelFri = null;
		this.m_panelSys = null;
		this.m_panelMsg = null;
		this.m_SysDeleteBtn = null;
		this.m_allReceiveSysButton = null;
		this.m_allReceiveFriButton = null;
		this.m_allDeleteMsgCenterButton = null;
	}

	public void SetActiveTab(int index)
	{
		if (index == 0)
		{
			this.m_panelFri.CustomSetActive(true);
			this.m_panelSys.CustomSetActive(false);
			this.m_panelMsg.CustomSetActive(false);
		}
		else if (index == 1)
		{
			this.m_panelFri.CustomSetActive(false);
			this.m_panelSys.CustomSetActive(true);
			this.m_panelMsg.CustomSetActive(false);
		}
		else if (index == 2)
		{
			this.m_panelFri.CustomSetActive(false);
			this.m_panelSys.CustomSetActive(false);
			this.m_panelMsg.CustomSetActive(true);
		}
	}

	public void SetUnReadNum(COM_MAIL_TYPE mailtype, int unReadNum)
	{
		if (mailtype == 2)
		{
			this.m_friUnReadNum = unReadNum;
		}
		else if (mailtype == 1)
		{
			this.m_sysUnReadNum = unReadNum;
		}
		else if (mailtype == 3)
		{
			this.m_msgUnReadNum = unReadNum;
		}
		if (this.m_CUIListScriptTab == null)
		{
			return;
		}
		if (mailtype == 2 && this.m_CUIListScriptTab.GetElemenet(0) != null)
		{
			if (unReadNum > 9)
			{
				CUICommonSystem.AddRedDot(this.m_CUIListScriptTab.GetElemenet(0).gameObject, enRedDotPos.enTopRight, 0);
			}
			else if (unReadNum > 0)
			{
				CUICommonSystem.AddRedDot(this.m_CUIListScriptTab.GetElemenet(0).gameObject, enRedDotPos.enTopRight, unReadNum);
			}
			else
			{
				CUICommonSystem.DelRedDot(this.m_CUIListScriptTab.GetElemenet(0).gameObject);
			}
		}
		else if (mailtype == 1 && this.m_CUIListScriptTab.GetElemenet(1) != null)
		{
			if (unReadNum > 9)
			{
				CUICommonSystem.AddRedDot(this.m_CUIListScriptTab.GetElemenet(1).gameObject, enRedDotPos.enTopRight, 0);
			}
			else if (unReadNum > 0)
			{
				CUICommonSystem.AddRedDot(this.m_CUIListScriptTab.GetElemenet(1).gameObject, enRedDotPos.enTopRight, unReadNum);
			}
			else
			{
				CUICommonSystem.DelRedDot(this.m_CUIListScriptTab.GetElemenet(1).gameObject);
			}
		}
		else if (mailtype == 3 && this.m_CUIListScriptTab.GetElemenet(2) != null)
		{
			if (unReadNum > 0)
			{
				CUICommonSystem.AddRedDot(this.m_CUIListScriptTab.GetElemenet(2).gameObject, enRedDotPos.enTopRight, 0);
			}
			else
			{
				CUICommonSystem.DelRedDot(this.m_CUIListScriptTab.GetElemenet(2).gameObject);
			}
		}
	}

	public void UpdateMailList(COM_MAIL_TYPE mailtype, ListView<CMail> mailList)
	{
		if (this.m_CUIForm == null || mailList == null)
		{
			return;
		}
		CUIListElementScript cUIListElementScript = null;
		int currentUTCTime = CRoleInfo.GetCurrentUTCTime();
		int num = -1;
		if (mailtype == 2)
		{
			CUIListScript component = this.m_CUIForm.transform.FindChild("PanelFriMail/List").GetComponent<CUIListScript>();
			component.SetElementAmount(mailList.get_Count());
			for (int i = 0; i < mailList.get_Count(); i++)
			{
				cUIListElementScript = component.GetElemenet(i);
				if (cUIListElementScript != null && cUIListElementScript.gameObject)
				{
					this.UpdateListElenment(cUIListElementScript.gameObject, mailList.get_Item(i));
				}
				if (num == -1 && mailList.get_Item(i).subType == 1)
				{
					num = i;
				}
			}
			this.m_allReceiveFriButton.CustomSetActive(num >= 0);
		}
		else if (mailtype == 1)
		{
			CUIListScript component = this.m_CUIForm.transform.FindChild("PanelSysMail/List").GetComponent<CUIListScript>();
			component.SetElementAmount(mailList.get_Count());
			for (int j = 0; j < mailList.get_Count(); j++)
			{
				if (cUIListElementScript != null && cUIListElementScript.gameObject)
				{
					this.UpdateListElenment(cUIListElementScript.gameObject, mailList.get_Item(j));
				}
				if (num == -1 && mailList.get_Item(j).subType == 2)
				{
					num = j;
				}
			}
			this.m_allReceiveSysButton.CustomSetActive(num >= 0);
			this.m_SysDeleteBtn.CustomSetActive(mailList.get_Count() > 0);
		}
		else if (mailtype == 3)
		{
			CUIListScript component = this.m_CUIForm.transform.FindChild("PanelMsgMail/List").GetComponent<CUIListScript>();
			component.SetElementAmount(mailList.get_Count());
			for (int k = 0; k < mailList.get_Count(); k++)
			{
				if (cUIListElementScript != null && cUIListElementScript.gameObject)
				{
					this.UpdateListElenment(cUIListElementScript.gameObject, mailList.get_Item(k));
				}
			}
			this.m_allDeleteMsgCenterButton.CustomSetActive(mailList.get_Count() > 0);
		}
	}

	public void UpdateListElenment(GameObject element, CMail mail)
	{
		int currentUTCTime = CRoleInfo.GetCurrentUTCTime();
		Text component = element.transform.FindChild("Title").GetComponent<Text>();
		Text component2 = element.transform.FindChild("MailTime").GetComponent<Text>();
		GameObject gameObject = element.transform.FindChild("New").gameObject;
		GameObject gameObject2 = element.transform.FindChild("ReadMailIcon").gameObject;
		GameObject gameObject3 = element.transform.FindChild("UnReadMailIcon").gameObject;
		GameObject gameObject4 = element.transform.FindChild("CoinImg").gameObject;
		Text component3 = element.transform.FindChild("From").GetComponent<Text>();
		CUIHttpImageScript component4 = element.transform.FindChild("HeadBg/imgHead").GetComponent<CUIHttpImageScript>();
		GameObject obj = null;
		Text text = null;
		Transform transform = element.transform.FindChild("OnlineBg");
		if (transform != null)
		{
			obj = transform.gameObject;
		}
		Transform transform2 = element.transform.FindChild("Online");
		if (transform2 != null)
		{
			text = transform2.GetComponent<Text>();
		}
		component.text = mail.subject;
		component2.text = Utility.GetTimeBeforString((long)((ulong)mail.sendTime), (long)currentUTCTime);
		bool flag = mail.mailState == 1;
		gameObject.CustomSetActive(flag);
		if (mail.mailType == 1)
		{
			gameObject2.CustomSetActive(!flag);
			gameObject3.CustomSetActive(flag);
			component3.text = string.Empty;
			component4.gameObject.CustomSetActive(false);
			gameObject4.SetActive(false);
			obj.CustomSetActive(false);
			if (text != null)
			{
				text.gameObject.CustomSetActive(false);
			}
		}
		else if (mail.mailType == 2)
		{
			obj.CustomSetActive(false);
			if (text != null)
			{
				text.gameObject.CustomSetActive(false);
			}
			gameObject2.CustomSetActive(false);
			gameObject3.CustomSetActive(false);
			component3.text = mail.from;
			component4.gameObject.CustomSetActive(true);
			if (mail.subType == 3)
			{
				gameObject4.CustomSetActive(false);
				component4.SetImageSprite(CGuildHelper.GetGuildHeadPath(), this.m_CUIForm);
			}
			else
			{
				gameObject4.CustomSetActive(true);
				if (!CSysDynamicBlock.bFriendBlocked)
				{
					COMDT_FRIEND_INFO friendByName = Singleton<CFriendContoller>.get_instance().model.getFriendByName(mail.from, CFriendModel.FriendType.GameFriend);
					if (friendByName == null)
					{
						friendByName = Singleton<CFriendContoller>.get_instance().model.getFriendByName(mail.from, CFriendModel.FriendType.SNS);
					}
					if (friendByName != null)
					{
						string url = Utility.UTF8Convert(friendByName.szHeadUrl);
						component4.SetImageUrl(Singleton<ApolloHelper>.GetInstance().ToSnsHeadUrl(url));
					}
				}
			}
		}
		else if (mail.mailType == 3)
		{
			obj.CustomSetActive(true);
			if (text != null)
			{
				text.gameObject.CustomSetActive(true);
			}
			gameObject2.CustomSetActive(false);
			gameObject3.CustomSetActive(false);
			component3.text = string.Empty;
			component4.gameObject.CustomSetActive(true);
			gameObject4.SetActive(false);
			Transform transform3 = element.transform.FindChild("invite_btn");
			GameObject obj2 = null;
			if (transform3 != null)
			{
				obj2 = transform3.gameObject;
			}
			if (mail.relationType == 1)
			{
				GuildMemInfo guildMemberInfoByUid = CGuildHelper.GetGuildMemberInfoByUid(mail.uid);
				Singleton<CMailSys>.get_instance().AddGuildMemInfo(guildMemberInfoByUid);
			}
			this.SetEventParams(element, mail);
			string text2;
			string url2;
			bool flag2 = !this.GetOtherPlayerState(mail.relationType, mail.uid, mail.dwLogicWorldID, out text2, out url2);
			string processTypeString = this.GetProcessTypeString((CMailSys.enProcessInviteType)mail.processType);
			component.text = string.Format("{0} {1}", mail.subject, processTypeString);
			if (text != null)
			{
				text.text = text2;
			}
			component4.SetImageUrl(Singleton<ApolloHelper>.GetInstance().ToSnsHeadUrl(url2));
			if (flag2)
			{
				CUIUtility.GetComponentInChildren<Image>(component4.gameObject).color = CUIUtility.s_Color_GrayShader;
			}
			else
			{
				CUIUtility.GetComponentInChildren<Image>(component4.gameObject).color = CUIUtility.s_Color_Full;
			}
			obj2.CustomSetActive(!flag2);
		}
	}

	private void SetEventParams(GameObject node, CMail mail)
	{
		if (node == null || mail == null)
		{
			return;
		}
		Transform transform = node.transform.FindChild("invite_btn");
		if (transform == null)
		{
			return;
		}
		CUIEventScript component = transform.GetComponent<CUIEventScript>();
		if (component == null)
		{
			return;
		}
		this.SetEventParams(component, mail);
	}

	private void SetEventParams(CUIEventScript com, CMail mail)
	{
		com.m_onClickEventParams.heroId = (uint)mail.bMapType;
		com.m_onClickEventParams.weakGuideId = mail.dwMapId;
		com.m_onClickEventParams.tag2 = (int)mail.relationType;
		com.m_onClickEventParams.tagUInt = mail.dwGameSvrEntity;
		com.m_onClickEventParams.commonUInt64Param1 = mail.uid;
		com.m_onClickEventParams.taskId = mail.dwLogicWorldID;
		com.m_onClickEventParams.tag3 = (int)mail.inviteType;
	}

	private string GetProcessTypeString(CMailSys.enProcessInviteType type)
	{
		switch (type)
		{
		case CMailSys.enProcessInviteType.Refuse:
			return Singleton<CMailSys>.get_instance().inviteRefuseStr;
		case CMailSys.enProcessInviteType.Accept:
			return Singleton<CMailSys>.get_instance().inviteAcceptStr;
		case CMailSys.enProcessInviteType.NoProcess:
			return Singleton<CMailSys>.get_instance().inviteNoProcessStr;
		default:
			return "error";
		}
	}

	private bool GetOtherPlayerState(COM_INVITE_RELATION_TYPE type, ulong uid, uint dwLogicWorldID, out string stateStr, out string headURL)
	{
		headURL = string.Empty;
		if (type == null)
		{
			CFriendModel.FriendInGame friendInGaming = Singleton<CFriendContoller>.get_instance().model.GetFriendInGaming(uid, dwLogicWorldID);
			COMDT_FRIEND_INFO gameOrSnsFriend = Singleton<CFriendContoller>.get_instance().model.GetGameOrSnsFriend(uid, dwLogicWorldID);
			if (gameOrSnsFriend == null)
			{
				stateStr = Singleton<CMailSys>.get_instance().offlineStr;
				return false;
			}
			headURL = Utility.UTF8Convert(gameOrSnsFriend.szHeadUrl);
			if (gameOrSnsFriend.bIsOnline != 1)
			{
				stateStr = Singleton<CMailSys>.get_instance().offlineStr;
				return false;
			}
			stateStr = Singleton<CMailSys>.get_instance().onlineStr;
			if (friendInGaming == null)
			{
				return gameOrSnsFriend.bIsOnline == 1;
			}
			if (friendInGaming.State == 1 || friendInGaming.State == 2)
			{
				if (friendInGaming.startTime > 0u)
				{
					stateStr = Singleton<CMailSys>.get_instance().gamingStr;
				}
				else
				{
					stateStr = string.Format("<color=#ffff00>{0}</color>", Singleton<CTextManager>.get_instance().GetText("Common_Gaming_NoTime"));
				}
			}
			else if (friendInGaming.State == 3)
			{
				stateStr = string.Format("<color=#ffff00>{0}</color>", Singleton<CTextManager>.get_instance().GetText("Common_Teaming"));
			}
			return gameOrSnsFriend.bIsOnline == 1;
		}
		else if (type == 1)
		{
			GuildMemInfo guildMemberInfoByUid = CGuildHelper.GetGuildMemberInfoByUid(uid);
			if (guildMemberInfoByUid == null)
			{
				stateStr = Singleton<CMailSys>.get_instance().offlineStr;
				return false;
			}
			headURL = guildMemberInfoByUid.stBriefInfo.szHeadUrl;
			if (!CGuildHelper.IsMemberOnline(guildMemberInfoByUid))
			{
				stateStr = Singleton<CMailSys>.get_instance().offlineStr;
				return false;
			}
			stateStr = Singleton<CMailSys>.get_instance().onlineStr;
			if (guildMemberInfoByUid.GameState == 1 || guildMemberInfoByUid.GameState == 2)
			{
				if (guildMemberInfoByUid.dwGameStartTime > 0u)
				{
					stateStr = Singleton<CMailSys>.get_instance().gamingStr;
				}
				else
				{
					stateStr = string.Format("<color=#ffff00>{0}</color>", Singleton<CTextManager>.get_instance().GetText("Common_Gaming_NoTime"));
				}
			}
			return true;
		}
		else
		{
			if (type != 2)
			{
				stateStr = Singleton<CMailSys>.get_instance().offlineStr;
				return false;
			}
			CSDT_LBS_USER_INFO lBSUserInfo = Singleton<CFriendContoller>.get_instance().model.GetLBSUserInfo(uid, dwLogicWorldID, CFriendModel.LBSGenderType.Both);
			if (lBSUserInfo == null)
			{
				stateStr = Singleton<CMailSys>.get_instance().offlineStr;
				return false;
			}
			headURL = Utility.UTF8Convert(lBSUserInfo.stLbsUserInfo.szHeadUrl);
			if (lBSUserInfo.stLbsUserInfo.bIsOnline == 1)
			{
				stateStr = Singleton<CMailSys>.get_instance().onlineStr;
			}
			else
			{
				stateStr = Singleton<CMailSys>.get_instance().offlineStr;
			}
			return lBSUserInfo.stLbsUserInfo.bIsOnline == 1;
		}
	}

	public int GetFriendMailListSelectedIndex()
	{
		if (this.m_panelFri != null)
		{
			CUIListScript component = this.m_panelFri.transform.Find("List").GetComponent<CUIListScript>();
			if (component != null)
			{
				return component.GetSelectedIndex();
			}
		}
		return -1;
	}
}
