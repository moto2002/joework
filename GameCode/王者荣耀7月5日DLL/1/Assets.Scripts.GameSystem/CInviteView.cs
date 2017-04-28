using Assets.Scripts.UI;
using CSProtocol;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	internal class CInviteView
	{
		public enum enInviteFormWidget
		{
			Friend_Panel,
			GuildMember_Panel,
			Friend_List,
			FriendEmpty_Panel,
			FriendTotalNum_Text,
			GuildMember_List,
			GuildMemberTotalNum_Text,
			InviteTab_List,
			RefreshGuildMemberGameState_Timer,
			Bottom_Widget,
			LBS_Panel,
			LBS_List,
			LBSTotalNum_Text
		}

		public enum enInviteListTab
		{
			Friend,
			GuildMember,
			LBS,
			Count
		}

		public static string[] TabName = new string[]
		{
			Singleton<CTextManager>.GetInstance().GetText("Invite_Tab_Title_Friend"),
			Singleton<CTextManager>.GetInstance().GetText("Invite_Tab_Title_Guild"),
			Singleton<CTextManager>.GetInstance().GetText("Invite_Tab_Title_LBS")
		};

		public static CInviteView.enInviteListTab GetInviteListTab(int index)
		{
			if (Singleton<CGuildSystem>.GetInstance().IsInNormalGuild())
			{
				return (CInviteView.enInviteListTab)index;
			}
			return (index <= 0) ? CInviteView.enInviteListTab.Friend : (index + CInviteView.enInviteListTab.GuildMember);
		}

		public static string GetTabName(int index)
		{
			if (Singleton<CGuildSystem>.GetInstance().IsInNormalGuild())
			{
				return CInviteView.TabName[index];
			}
			return (index <= 0) ? CInviteView.TabName[0] : CInviteView.TabName[index + 1];
		}

		public static int GetTabCount()
		{
			return (!Singleton<CGuildSystem>.GetInstance().IsInNormalGuild()) ? 2 : 3;
		}

		public static void InitListTab(CUIFormScript form)
		{
			CUIListScript component = form.GetWidget(7).GetComponent<CUIListScript>();
			int tabCount = CInviteView.GetTabCount();
			component.SetElementAmount(tabCount);
			for (int i = 0; i < component.GetElementAmount(); i++)
			{
				CUIListElementScript elemenet = component.GetElemenet(i);
				elemenet.transform.Find("txtName").GetComponent<Text>().text = CInviteView.GetTabName(i);
			}
			component.SelectElement(0, true);
		}

		public static void SetInviteFriendData(CUIFormScript form, COM_INVITE_JOIN_TYPE joinType)
		{
			ListView<COMDT_FRIEND_INFO> allFriendList = Singleton<CInviteSystem>.get_instance().GetAllFriendList();
			int count = allFriendList.get_Count();
			int num = 0;
			CUIListScript component = form.GetWidget(2).GetComponent<CUIListScript>();
			component.SetElementAmount(count);
			form.GetWidget(3).gameObject.CustomSetActive(allFriendList.get_Count() == 0);
			for (int i = 0; i < count; i++)
			{
				if (allFriendList.get_Item(i).bIsOnline == 1)
				{
					num++;
				}
			}
			Text component2 = form.GetWidget(4).GetComponent<Text>();
			component2.text = Singleton<CTextManager>.GetInstance().GetText("Common_Online_Member", new string[]
			{
				num.ToString(),
				count.ToString()
			});
			GameObject widget = form.GetWidget(9);
			if (CSysDynamicBlock.bLobbyEntryBlocked || ApolloConfig.IsUseCEPackage() >= 1)
			{
				widget.CustomSetActive(false);
			}
			else
			{
				Text component3 = Utility.FindChild(widget, "ShareInviteButton/Text").GetComponent<Text>();
				GameObject gameObject = widget.transform.FindChild("ShareInviteButton/IconQQ").gameObject;
				GameObject gameObject2 = widget.transform.FindChild("ShareInviteButton/IconWeixin").gameObject;
				bool flag = Singleton<CRoomSystem>.GetInstance().IsInRoom || Singleton<CMatchingSystem>.GetInstance().IsInMatchingTeam;
				if (flag)
				{
					widget.CustomSetActive(true);
					if (Singleton<ApolloHelper>.GetInstance().CurPlatform == 2)
					{
						component3.text = Singleton<CTextManager>.GetInstance().GetText("Share_Room_Info_QQ");
						gameObject.CustomSetActive(true);
						gameObject2.CustomSetActive(false);
					}
					else if (Singleton<ApolloHelper>.GetInstance().CurPlatform == 1)
					{
						component3.text = Singleton<CTextManager>.GetInstance().GetText("Share_Room_Info_WX");
						gameObject.CustomSetActive(false);
						gameObject2.CustomSetActive(true);
					}
				}
				else
				{
					widget.CustomSetActive(false);
				}
			}
		}

		public static void SetInviteGuildMemberData(CUIFormScript form)
		{
			ListView<GuildMemInfo> allGuildMemberList = Singleton<CInviteSystem>.GetInstance().GetAllGuildMemberList();
			int count = allGuildMemberList.get_Count();
			int num = 0;
			CInviteView.RefreshInviteGuildMemberList(form, count);
			for (int i = 0; i < count; i++)
			{
				if (CGuildHelper.IsMemberOnline(allGuildMemberList.get_Item(i)))
				{
					num++;
				}
			}
			Text component = form.GetWidget(6).GetComponent<Text>();
			component.text = Singleton<CTextManager>.GetInstance().GetText("Common_Online_Member", new string[]
			{
				num.ToString(),
				count.ToString()
			});
		}

		public static void SetLBSData(CUIFormScript form, COM_INVITE_JOIN_TYPE joinType)
		{
			CUIListScript component = form.GetWidget(11).GetComponent<CUIListScript>();
			Text component2 = form.GetWidget(12).GetComponent<Text>();
			int elementAmount = 0;
			if (Singleton<CFriendContoller>.get_instance().model.EnableShareLocation)
			{
				ListView<CSDT_LBS_USER_INFO> lBSList = Singleton<CFriendContoller>.get_instance().model.GetLBSList(CFriendModel.LBSGenderType.Both);
				elementAmount = ((lBSList == null) ? 0 : lBSList.get_Count());
				component.SetElementAmount(elementAmount);
				Utility.FindChild(form.GetWidget(10), "Empty/Normal").CustomSetActive(true);
				Utility.FindChild(form.GetWidget(10), "Empty/GotoBtn").CustomSetActive(false);
			}
			else
			{
				component.SetElementAmount(0);
				Utility.FindChild(form.GetWidget(10), "Empty/Normal").CustomSetActive(false);
				Utility.FindChild(form.GetWidget(10), "Empty/GotoBtn").CustomSetActive(true);
			}
			component2.text = Singleton<CTextManager>.GetInstance().GetText("Common_Online_Member", new string[]
			{
				elementAmount.ToString(),
				elementAmount.ToString()
			});
		}

		public static void RefreshInviteGuildMemberList(CUIFormScript form, int allGuildMemberLen)
		{
			CUIListScript component = form.GetWidget(5).GetComponent<CUIListScript>();
			component.SetElementAmount(allGuildMemberLen);
		}

		public static void UpdateFriendListElement(GameObject element, COMDT_FRIEND_INFO friend)
		{
			CInviteView.UpdateFriendListElementBase(element, ref friend);
			CInviteView.SetFriendState(element, ref friend);
		}

		public static void UpdateLBSListElement(GameObject element, CSDT_LBS_USER_INFO LBSInfo)
		{
			CInviteView.UpdateFriendListElementBase(element, ref LBSInfo.stLbsUserInfo);
			CInviteView.SetLBSState(element, ref LBSInfo);
		}

		public static string ConnectPlayerNameAndNickName(byte[] szUserName, string nickName)
		{
			if (szUserName == null)
			{
				return string.Empty;
			}
			if (!string.IsNullOrEmpty(nickName))
			{
				return string.Format("{0}({1})", Utility.UTF8Convert(szUserName), nickName);
			}
			return Utility.UTF8Convert(szUserName);
		}

		public static void UpdateFriendListElementBase(GameObject element, ref COMDT_FRIEND_INFO friend)
		{
			GameObject gameObject = element.transform.FindChild("HeadBg").gameObject;
			Text component = element.transform.FindChild("PlayerName").GetComponent<Text>();
			Image component2 = element.transform.FindChild("NobeIcon").GetComponent<Image>();
			Image component3 = element.transform.FindChild("HeadBg/NobeImag").GetComponent<Image>();
			if (component2)
			{
				MonoSingleton<NobeSys>.GetInstance().SetNobeIcon(component2, (int)friend.stGameVip.dwCurLevel, false);
			}
			if (component3)
			{
				MonoSingleton<NobeSys>.GetInstance().SetHeadIconBk(component3, (int)friend.stGameVip.dwHeadIconId);
			}
			CFriendModel.FriendInGame friendInGaming = Singleton<CFriendContoller>.get_instance().model.GetFriendInGaming(friend.stUin.ullUid, friend.stUin.dwLogicWorldId);
			if (friendInGaming == null)
			{
				component.text = CInviteView.ConnectPlayerNameAndNickName(friend.szUserName, string.Empty);
			}
			else
			{
				component.text = CInviteView.ConnectPlayerNameAndNickName(friend.szUserName, friendInGaming.NickName);
			}
			string url = Utility.UTF8Convert(friend.szHeadUrl);
			if (!CSysDynamicBlock.bFriendBlocked)
			{
				CUIUtility.GetComponentInChildren<CUIHttpImageScript>(gameObject).SetImageUrl(Singleton<ApolloHelper>.GetInstance().ToSnsHeadUrl(url));
			}
		}

		private static void SetFriendState(GameObject element, ref COMDT_FRIEND_INFO friend)
		{
			GameObject gameObject = element.transform.FindChild("HeadBg").gameObject;
			Text component = element.transform.FindChild("Online").GetComponent<Text>();
			GameObject gameObject2 = element.transform.FindChild("InviteButton").gameObject;
			Text component2 = element.transform.FindChild("PlayerName").GetComponent<Text>();
			Text component3 = element.transform.FindChild("Time").GetComponent<Text>();
			if (component3 != null)
			{
				component3.gameObject.CustomSetActive(false);
			}
			if (component != null)
			{
				component.gameObject.CustomSetActive(true);
			}
			CInviteView.SetListElementLadderInfo(element, friend);
			COM_ACNT_GAME_STATE cOM_ACNT_GAME_STATE = 0;
			if (friend.bIsOnline == 1)
			{
				CFriendModel.FriendInGame friendInGaming = Singleton<CFriendContoller>.get_instance().model.GetFriendInGaming(friend.stUin.ullUid, friend.stUin.dwLogicWorldId);
				if (friendInGaming == null)
				{
					cOM_ACNT_GAME_STATE = 0;
				}
				else
				{
					cOM_ACNT_GAME_STATE = friendInGaming.State;
				}
				if (cOM_ACNT_GAME_STATE == null)
				{
					component.text = Singleton<CInviteSystem>.get_instance().GetInviteStateStr(friend.stUin.ullUid, false);
					CUIEventScript component4 = gameObject2.GetComponent<CUIEventScript>();
					component4.m_onClickEventParams.tag = Singleton<CInviteSystem>.get_instance().InviteType;
					component4.m_onClickEventParams.tag2 = (int)friend.stUin.dwLogicWorldId;
					component4.m_onClickEventParams.commonUInt64Param1 = friend.stUin.ullUid;
				}
				else if (cOM_ACNT_GAME_STATE == 1 || cOM_ACNT_GAME_STATE == 2 || cOM_ACNT_GAME_STATE == 4)
				{
					if (friendInGaming == null)
					{
						component.gameObject.CustomSetActive(true);
						component.text = "friendInGame is null";
						return;
					}
					if (friendInGaming.startTime > 0u)
					{
						component.gameObject.CustomSetActive(false);
						component3.gameObject.CustomSetActive(true);
						component3.text = string.Format(Singleton<CTextManager>.get_instance().GetText("Common_Gaming_Time"), CInviteView.GetStartMinute(friendInGaming.startTime));
						Singleton<CInviteSystem>.get_instance().CheckInviteListGameTimer();
					}
					else
					{
						component.gameObject.CustomSetActive(true);
						component.text = string.Format("<color=#ffff00>{0}</color>", Singleton<CTextManager>.get_instance().GetText("Common_Gaming_NoTime"));
					}
				}
				else if (cOM_ACNT_GAME_STATE == 3)
				{
					component.text = string.Format("<color=#ffff00>{0}</color>", Singleton<CTextManager>.get_instance().GetText("Common_Teaming"));
				}
				component2.color = CUIUtility.s_Color_White;
				CUIUtility.GetComponentInChildren<Image>(gameObject).color = CUIUtility.s_Color_White;
			}
			else
			{
				component.text = string.Format(Singleton<CTextManager>.get_instance().GetText("Common_Offline"), new object[0]);
				component2.color = CUIUtility.s_Color_Grey;
				CUIUtility.GetComponentInChildren<Image>(gameObject).color = CUIUtility.s_Color_GrayShader;
			}
			gameObject2.CustomSetActive(friend.bIsOnline == 1 && cOM_ACNT_GAME_STATE == 0);
		}

		private static int GetStartMinute(uint startTime)
		{
			DateTime dateTime = Utility.ToUtcTime2Local((long)CRoleInfo.GetCurrentUTCTime());
			DateTime dateTime2 = Utility.ToUtcTime2Local((long)((ulong)startTime));
			if (dateTime < dateTime2)
			{
				return 1;
			}
			int value = (int)(dateTime - dateTime2).get_TotalMinutes();
			return Mathf.Clamp(value, 1, 99);
		}

		private static void SetLBSState(GameObject element, ref CSDT_LBS_USER_INFO LBSInfo)
		{
			COMDT_FRIEND_INFO stLbsUserInfo = LBSInfo.stLbsUserInfo;
			GameObject gameObject = element.transform.FindChild("HeadBg").gameObject;
			Text component = element.transform.FindChild("Online").GetComponent<Text>();
			GameObject gameObject2 = element.transform.FindChild("InviteButton").gameObject;
			Text component2 = element.transform.FindChild("PlayerName").GetComponent<Text>();
			CInviteView.SetListElementLadderInfo(element, stLbsUserInfo);
			COM_ACNT_GAME_STATE cOM_ACNT_GAME_STATE = 0;
			if (stLbsUserInfo.bIsOnline == 1)
			{
				cOM_ACNT_GAME_STATE = Singleton<CFriendContoller>.get_instance().model.GetFriendInGamingState(stLbsUserInfo.stUin.ullUid, stLbsUserInfo.stUin.dwLogicWorldId);
				if (cOM_ACNT_GAME_STATE == null)
				{
					component.text = Singleton<CInviteSystem>.get_instance().GetInviteStateStr(stLbsUserInfo.stUin.ullUid, false);
					CUIEventScript component3 = gameObject2.GetComponent<CUIEventScript>();
					component3.m_onClickEventParams.tag = Singleton<CInviteSystem>.get_instance().InviteType;
					component3.m_onClickEventParams.tag2 = (int)stLbsUserInfo.stUin.dwLogicWorldId;
					component3.m_onClickEventParams.tag3 = (int)LBSInfo.dwGameSvrEntity;
					component3.m_onClickEventParams.commonUInt64Param1 = stLbsUserInfo.stUin.ullUid;
				}
				else if (cOM_ACNT_GAME_STATE == 1 || cOM_ACNT_GAME_STATE == 2 || cOM_ACNT_GAME_STATE == 4)
				{
					component.text = string.Format("<color=#ffff00>{0}</color>", Singleton<CTextManager>.get_instance().GetText("Common_Gaming"));
				}
				else if (cOM_ACNT_GAME_STATE == 3)
				{
					component.text = string.Format("<color=#ffff00>{0}</color>", Singleton<CTextManager>.get_instance().GetText("Common_Teaming"));
				}
				component2.color = CUIUtility.s_Color_White;
				CUIUtility.GetComponentInChildren<Image>(gameObject).color = CUIUtility.s_Color_White;
			}
			else
			{
				component.text = string.Format(Singleton<CTextManager>.get_instance().GetText("Common_Offline"), new object[0]);
				component2.color = CUIUtility.s_Color_Grey;
				CUIUtility.GetComponentInChildren<Image>(gameObject).color = CUIUtility.s_Color_GrayShader;
			}
			gameObject2.CustomSetActive(stLbsUserInfo.bIsOnline == 1 && cOM_ACNT_GAME_STATE == 0);
		}

		public static void UpdateGuildMemberListElement(GameObject element, GuildMemInfo guildMember, bool isGuildMatchInvite)
		{
			GameObject gameObject = element.transform.FindChild("HeadBg").gameObject;
			GameObject gameObject2 = element.transform.FindChild("InviteButton").gameObject;
			Text component = element.transform.FindChild("PlayerName").GetComponent<Text>();
			Text component2 = element.transform.FindChild("Online").GetComponent<Text>();
			Image component3 = element.transform.FindChild("NobeIcon").GetComponent<Image>();
			Image component4 = element.transform.FindChild("HeadBg/NobeImag").GetComponent<Image>();
			Text component5 = element.transform.FindChild("Time").GetComponent<Text>();
			if (component5 != null)
			{
				component5.gameObject.CustomSetActive(false);
			}
			if (component2 != null)
			{
				component2.gameObject.CustomSetActive(true);
			}
			Transform transform = element.transform.FindChild("RemindButton");
			GameObject gameObject3 = null;
			if (transform != null)
			{
				gameObject3 = transform.gameObject;
				gameObject3.CustomSetActive(false);
			}
			CInviteView.SetListElementLadderInfo(element, guildMember);
			if (component3)
			{
				MonoSingleton<NobeSys>.GetInstance().SetNobeIcon(component3, (int)guildMember.stBriefInfo.stVip.level, false);
			}
			if (component4)
			{
				MonoSingleton<NobeSys>.GetInstance().SetHeadIconBk(component4, (int)guildMember.stBriefInfo.stVip.headIconId);
			}
			component.text = Utility.UTF8Convert(guildMember.stBriefInfo.sName);
			if (CGuildHelper.IsMemberOnline(guildMember))
			{
				if (guildMember.GameState == null)
				{
					component2.text = Singleton<CInviteSystem>.get_instance().GetInviteStateStr(guildMember.stBriefInfo.uulUid, isGuildMatchInvite);
					CUIEventScript component6 = gameObject2.GetComponent<CUIEventScript>();
					component6.m_onClickEventParams.tag = Singleton<CInviteSystem>.get_instance().InviteType;
					component6.m_onClickEventParams.tag2 = guildMember.stBriefInfo.dwLogicWorldId;
					component6.m_onClickEventParams.commonUInt64Param1 = guildMember.stBriefInfo.uulUid;
				}
				else if (guildMember.GameState == 1 || guildMember.GameState == 2 || guildMember.GameState == 4)
				{
					if (guildMember.dwGameStartTime > 0u)
					{
						if (component2 != null)
						{
							component2.gameObject.CustomSetActive(false);
						}
						if (component5 != null)
						{
							component5.gameObject.CustomSetActive(true);
						}
						if (component5 != null)
						{
							component5.text = string.Format(Singleton<CTextManager>.get_instance().GetText("Common_Gaming_Time"), CInviteView.GetStartMinute(guildMember.dwGameStartTime));
						}
						Singleton<CInviteSystem>.get_instance().CheckInviteListGameTimer();
					}
					else
					{
						if (component2 != null)
						{
							component2.gameObject.CustomSetActive(true);
						}
						if (component2 != null)
						{
							component2.text = string.Format("<color=#ffff00>{0}</color>", Singleton<CTextManager>.get_instance().GetText("Common_Gaming_NoTime"));
						}
					}
				}
				else if (guildMember.GameState == 3)
				{
					component2.text = string.Format("<color=#ffff00>{0}</color>", Singleton<CTextManager>.get_instance().GetText("Common_Teaming"));
				}
				component.color = CUIUtility.s_Color_White;
				CUIUtility.GetComponentInChildren<Image>(gameObject).color = CUIUtility.s_Color_White;
			}
			else
			{
				component2.text = string.Format(Singleton<CTextManager>.get_instance().GetText("Common_Offline"), new object[0]);
				component.color = CUIUtility.s_Color_Grey;
				CUIUtility.GetComponentInChildren<Image>(gameObject).color = CUIUtility.s_Color_GrayShader;
			}
			gameObject2.CustomSetActive(CGuildHelper.IsMemberOnline(guildMember) && guildMember.GameState == 0);
			string szHeadUrl = guildMember.stBriefInfo.szHeadUrl;
			CUIUtility.GetComponentInChildren<CUIHttpImageScript>(gameObject).SetImageUrl(Singleton<ApolloHelper>.GetInstance().ToSnsHeadUrl(szHeadUrl));
			if (isGuildMatchInvite && Singleton<CGuildMatchSystem>.GetInstance().IsInTeam(guildMember.GuildMatchInfo.ullTeamLeaderUid, Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().playerUllUID) && CGuildHelper.IsMemberOnline(guildMember))
			{
				CInviteView.SetInvitedRelatedWidgets(gameObject2, component2);
				if (guildMember.GameState == null && !Convert.ToBoolean(guildMember.GuildMatchInfo.bIsReady) && gameObject3 != null)
				{
					gameObject3.CustomSetActive(true);
					CUIEventScript component7 = gameObject3.GetComponent<CUIEventScript>();
					component7.m_onClickEventParams.commonUInt64Param1 = guildMember.stBriefInfo.uulUid;
				}
			}
		}

		public static void SetInvitedRelatedWidgets(GameObject inviteButtonGo, Text gameStateText)
		{
			inviteButtonGo.CustomSetActive(false);
			gameStateText.text = Singleton<CTextManager>.GetInstance().GetText("Guild_Has_Invited");
		}

		private static void SetListElementLadderInfo(GameObject element, COMDT_FRIEND_INFO friendInfo)
		{
			GameObject gameObject = element.transform.Find("RankCon").gameObject;
			if (gameObject != null)
			{
				gameObject.CustomSetActive(false);
			}
			int num;
			int rankStar;
			CInviteView.GetFriendRankGradeAndStar(friendInfo, out num, out rankStar);
			bool flag = Singleton<CLadderSystem>.GetInstance().IsHaveFightRecord(false, num, rankStar);
			if (flag)
			{
				gameObject.CustomSetActive(true);
				CLadderView.ShowRankDetail(gameObject, (byte)num, (uint)((byte)friendInfo.dwRankClass), friendInfo.bIsOnline != 1, true);
			}
		}

		private static void SetListElementLadderInfo(GameObject element, GuildMemInfo guildMemInfo)
		{
			if (guildMemInfo == null)
			{
				return;
			}
			GameObject gameObject = element.transform.Find("RankCon").gameObject;
			if (gameObject != null)
			{
				gameObject.CustomSetActive(false);
			}
			int num;
			int rankStar;
			CInviteView.GetGuildMemberGradeAndStar(guildMemInfo, out num, out rankStar);
			bool flag = Singleton<CLadderSystem>.GetInstance().IsHaveFightRecord(false, num, rankStar);
			if (flag)
			{
				gameObject.CustomSetActive(true);
				CLadderView.ShowRankDetail(gameObject, (byte)num, (uint)((byte)guildMemInfo.stBriefInfo.dwClassOfRank), !CGuildHelper.IsMemberOnline(guildMemInfo), true);
			}
		}

		private static bool IsLadderInvite()
		{
			if (Singleton<CInviteSystem>.GetInstance().InviteType == 2)
			{
				CMatchingSystem instance = Singleton<CMatchingSystem>.GetInstance();
				if (instance != null && instance.teamInfo.stTeamInfo.bMapType == 3)
				{
					return true;
				}
			}
			return false;
		}

		private static void GetFriendRankGradeAndStar(COMDT_FRIEND_INFO friendInfo, out int rankGrade, out int rankStar)
		{
			if (friendInfo != null && friendInfo.RankVal != null)
			{
				uint elo = friendInfo.RankVal[7];
				rankGrade = CLadderSystem.ConvertEloToRank(elo);
				rankStar = CLadderSystem.GetCurXingByEloAndRankLv(elo, (uint)rankGrade);
			}
			else
			{
				rankGrade = 0;
				rankStar = 0;
			}
		}

		private static void GetGuildMemberGradeAndStar(GuildMemInfo guildMemInfo, out int rankGrade, out int rankStar)
		{
			if (guildMemInfo != null)
			{
				rankGrade = CLadderSystem.ConvertEloToRank(guildMemInfo.stBriefInfo.dwScoreOfRank);
				rankStar = CLadderSystem.GetCurXingByEloAndRankLv(guildMemInfo.stBriefInfo.dwScoreOfRank, (uint)rankGrade);
			}
			else
			{
				rankGrade = 0;
				rankStar = 0;
			}
		}
	}
}
