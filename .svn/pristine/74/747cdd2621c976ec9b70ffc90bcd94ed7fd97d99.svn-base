using Assets.Scripts.Framework;
using Assets.Scripts.UI;
using CSProtocol;
using ResData;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	[MessageHandlerClass]
	public class COBSystem : Singleton<COBSystem>
	{
		private enum enOBTab
		{
			Null,
			Expert,
			Friend,
			Guild,
			Local
		}

		private enum enStatus
		{
			Normal,
			Editor
		}

		private struct stOBFriend
		{
			public COMDT_ACNT_UNIQ uin;

			public string friendName;

			public string headUrl;

			public COMDT_GAMEINFO_DETAIL gameDetail;
		}

		private struct stOBExpert
		{
			public COMDT_OB_DESK desk;

			public uint startTime;

			public uint observeNum;

			public COMDT_HEROLABEL heroLabel;
		}

		public class stOBGuild
		{
			public ulong obUid;

			public ulong playerUid;

			public string playerName;

			public string teamName;

			public string headUrl;

			public uint dwHeroID;

			public uint dwStartTime;

			public byte bGrade;

			public uint dwClass;

			public uint dwObserveNum;
		}

		private struct stOBLocal
		{
			public string path;

			public uint heroId;
		}

		public static readonly string OB_FORM_PATH = "UGUI/Form/System/OB/Form_OB.prefab";

		private COBSystem.enStatus curStatus;

		private List<COBSystem.stOBFriend> OBFriendList = new List<COBSystem.stOBFriend>();

		private List<COBSystem.stOBExpert> OBExpertList = new List<COBSystem.stOBExpert>();

		private List<COBSystem.stOBGuild> OBGuildList = new List<COBSystem.stOBGuild>();

		private ListView<GameReplayModule.ReplayFileInfo> OBLocalList = new ListView<GameReplayModule.ReplayFileInfo>();

		private static int m_lastGetExpertListTime = 0;

		public static int EXPERT_DETAL_SEC = 60;

		private static int m_lastGetFriendStateTime = 0;

		public static int FRIEND_DETAL_SEC = 60;

		private static Vector2 s_content_pos = new Vector2(0f, -30f);

		private static Vector2 s_content_size = new Vector2(0f, -60f);

		private COBSystem.enOBTab CurTab
		{
			get
			{
				CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(COBSystem.OB_FORM_PATH);
				if (form == null)
				{
					return COBSystem.enOBTab.Null;
				}
				CUIListScript componetInChild = Utility.GetComponetInChild<CUIListScript>(form.gameObject, "Panel_Menu/List");
				if (componetInChild == null)
				{
					return COBSystem.enOBTab.Null;
				}
				CUIListElementScript selectedElement = componetInChild.GetSelectedElement();
				if (selectedElement == null)
				{
					return COBSystem.enOBTab.Null;
				}
				return selectedElement.m_onEnableEventParams.tag + COBSystem.enOBTab.Expert;
			}
		}

		public override void Init()
		{
			Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg>("Friend_SNS_STATE_NTF", new Action<CSPkg>(this.On_Friend_SNS_STATE_NTF));
			Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg>("Friend_List", new Action<CSPkg>(this.On_FriendSys_Friend_List));
			Singleton<EventRouter>.GetInstance().AddEventHandler<CSPkg>("Friend_GAME_STATE_NTF", new Action<CSPkg>(this.On_Friend_GAME_STATE_NTF));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.OB_Form_Open, new CUIEventManager.OnUIEventHandler(this.OnOBFormOpen));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.OB_Form_Close, new CUIEventManager.OnUIEventHandler(this.OnOBFormClose));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.OB_Video_Tab_Click, new CUIEventManager.OnUIEventHandler(this.OnOBVideoTabClick));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.OB_Video_Enter, new CUIEventManager.OnUIEventHandler(this.OnVideoEnter));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.OB_Video_Enter_Confirm, new CUIEventManager.OnUIEventHandler(this.OnVideoEnterConfirm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.OB_Element_Enable, new CUIEventManager.OnUIEventHandler(this.OnElementEnable));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.OB_Video_Editor_Click, new CUIEventManager.OnUIEventHandler(this.OnEditorClick));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.OB_Video_Delete, new CUIEventManager.OnUIEventHandler(this.OnVideoDelete));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.OB_Video_Delete_Confirm, new CUIEventManager.OnUIEventHandler(this.OnConfirmDelete));
			base.Init();
		}

		public override void UnInit()
		{
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<CSPkg>("Friend_SNS_STATE_NTF", new Action<CSPkg>(this.On_Friend_SNS_STATE_NTF));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<CSPkg>("Friend_List", new Action<CSPkg>(this.On_FriendSys_Friend_List));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<CSPkg>("Friend_GAME_STATE_NTF", new Action<CSPkg>(this.On_Friend_GAME_STATE_NTF));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.OB_Form_Open, new CUIEventManager.OnUIEventHandler(this.OnOBFormOpen));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.OB_Form_Close, new CUIEventManager.OnUIEventHandler(this.OnOBFormClose));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.OB_Video_Tab_Click, new CUIEventManager.OnUIEventHandler(this.OnOBVideoTabClick));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.OB_Video_Enter, new CUIEventManager.OnUIEventHandler(this.OnVideoEnter));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.OB_Video_Enter_Confirm, new CUIEventManager.OnUIEventHandler(this.OnVideoEnterConfirm));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.OB_Element_Enable, new CUIEventManager.OnUIEventHandler(this.OnElementEnable));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.OB_Video_Editor_Click, new CUIEventManager.OnUIEventHandler(this.OnEditorClick));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.OB_Video_Delete, new CUIEventManager.OnUIEventHandler(this.OnVideoDelete));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.OB_Video_Delete_Confirm, new CUIEventManager.OnUIEventHandler(this.OnConfirmDelete));
			base.UnInit();
		}

		public void Clear()
		{
			this.OBFriendList.Clear();
			this.OBExpertList.Clear();
			this.OBLocalList.Clear();
			this.OBGuildList.Clear();
			COBSystem.m_lastGetExpertListTime = 0;
			COBSystem.m_lastGetFriendStateTime = 0;
		}

		public bool IsInOBFriendList(ulong uid)
		{
			for (int i = 0; i < this.OBFriendList.get_Count(); i++)
			{
				if (this.OBFriendList.get_Item(i).uin.ullUid == uid)
				{
					return true;
				}
			}
			return false;
		}

		public void OnOBFormOpen(CUIEvent cuiEvent)
		{
			if (Singleton<CMatchingSystem>.GetInstance().IsInMatching)
			{
				Singleton<CUIManager>.GetInstance().OpenTips("PVP_Matching", true, 1.5f, null, new object[0]);
				return;
			}
			CUIFormScript cUIFormScript = Singleton<CUIManager>.get_instance().OpenForm(COBSystem.OB_FORM_PATH, false, true);
			if (cUIFormScript == null)
			{
				return;
			}
			CUIListScript componetInChild = Utility.GetComponetInChild<CUIListScript>(cUIFormScript.gameObject, "Panel_Menu/List");
			this.InitTab(componetInChild);
			Singleton<CUINewFlagSystem>.get_instance().SetNewFlagForOBBtn(false);
			Transform transform = cUIFormScript.gameObject.transform.FindChild("Panel_Menu/BtnVideoMgr");
			if (transform && transform.gameObject)
			{
				if (!Singleton<CRecordUseSDK>.get_instance().GetRecorderGlobalCfgEnableFlag())
				{
					transform.gameObject.CustomSetActive(false);
				}
				else
				{
					transform.gameObject.CustomSetActive(true);
				}
			}
		}

		private void InitTab(CUIListScript list)
		{
			if (list == null)
			{
				return;
			}
			bool flag = Singleton<CGuildSystem>.GetInstance().IsInNormalGuild();
			int num = Enum.GetValues(typeof(COBSystem.enOBTab)).get_Length() - 1;
			if (!flag)
			{
				num--;
			}
			list.SetElementAmount(num);
			string[] array = new string[]
			{
				"OB_Expert",
				"OB_Freind",
				"OB_Guild",
				"OB_Local"
			};
			for (int i = 0; i < num; i++)
			{
				CUIListElementScript elemenet = list.GetElemenet(i);
				int num2 = (i + 1 < 3 || flag) ? i : (i + 1);
				elemenet.m_onEnableEventParams.tag = num2;
				Utility.GetComponetInChild<Text>(elemenet.gameObject, "Text").text = Singleton<CTextManager>.get_instance().GetText(array[num2]);
			}
			list.SelectElement(0, true);
		}

		private void OnOBFormClose(CUIEvent cuiEvent)
		{
			Singleton<GameJoy>.get_instance().CloseVideoListDialog();
		}

		private void OnOBVideoTabClick(CUIEvent cuiEvent)
		{
			switch (this.CurTab)
			{
			case COBSystem.enOBTab.Expert:
				COBSystem.GetGreatMatch(false);
				break;
			case COBSystem.enOBTab.Friend:
				COBSystem.GetFriendsState();
				break;
			case COBSystem.enOBTab.Guild:
				this.OBGuildList = Singleton<CGuildMatchSystem>.GetInstance().GetGuidMatchObInfo();
				Singleton<CGuildMatchSystem>.GetInstance().RequestGuildOBCount();
				break;
			case COBSystem.enOBTab.Local:
				this.OBLocalList = Singleton<GameReplayModule>.get_instance().ListReplayFiles(true);
				break;
			}
			this.curStatus = COBSystem.enStatus.Normal;
			this.UpdateView();
		}

		private void OnEditorClick(CUIEvent cuiEvent)
		{
			if (this.CurTab == COBSystem.enOBTab.Local)
			{
				if (this.curStatus == COBSystem.enStatus.Normal)
				{
					this.curStatus = COBSystem.enStatus.Editor;
				}
				else
				{
					this.curStatus = COBSystem.enStatus.Normal;
				}
				this.UpdateView();
			}
		}

		private void OnVideoDelete(CUIEvent cuiEvent)
		{
			int srcWidgetIndexInBelongedList = cuiEvent.m_srcWidgetIndexInBelongedList;
			stUIEventParams par = default(stUIEventParams);
			par.tag = srcWidgetIndexInBelongedList;
			Singleton<CUIManager>.get_instance().OpenMessageBoxWithCancel(Singleton<CTextManager>.get_instance().GetText("OB_Desc_7"), enUIEventID.OB_Video_Delete_Confirm, enUIEventID.None, par, false);
		}

		private void OnConfirmDelete(CUIEvent cuiEvent)
		{
			int tag = cuiEvent.m_eventParams.tag;
			if (tag >= 0 && tag < this.OBLocalList.get_Count())
			{
				string path = this.OBLocalList.get_Item(tag).path;
				try
				{
					File.Delete(path);
					this.OBLocalList.RemoveAt(tag);
					Singleton<CUIManager>.get_instance().OpenTips("OB_Desc_8", true, 1.5f, null, new object[0]);
				}
				catch
				{
					Singleton<CUIManager>.get_instance().OpenTips("OB_Desc_9", true, 1.5f, null, new object[0]);
				}
			}
			this.UpdateView();
		}

		public void UpdateView()
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(COBSystem.OB_FORM_PATH);
			if (form == null)
			{
				return;
			}
			GameObject obj = Utility.FindChild(form.gameObject, "ContentList/BtnEditor");
			Text componetInChild = Utility.GetComponetInChild<Text>(form.gameObject, "ContentList/BtnEditor/Text");
			CUIListScript componetInChild2 = Utility.GetComponetInChild<CUIListScript>(form.gameObject, "ContentList");
			RectTransform component = Utility.FindChild(componetInChild2.gameObject, "ScrollRect").GetComponent<RectTransform>();
			if (componetInChild2 == null)
			{
				return;
			}
			COBSystem.enOBTab curTab = this.CurTab;
			if (curTab == COBSystem.enOBTab.Expert)
			{
				obj.CustomSetActive(false);
				component.anchoredPosition = Vector2.zero;
				component.sizeDelta = Vector2.zero;
				componetInChild2.SetElementAmount(this.OBExpertList.get_Count());
			}
			else if (curTab == COBSystem.enOBTab.Friend)
			{
				obj.CustomSetActive(false);
				component.anchoredPosition = Vector2.zero;
				component.sizeDelta = Vector2.zero;
				componetInChild2.SetElementAmount(this.OBFriendList.get_Count());
			}
			else if (curTab == COBSystem.enOBTab.Guild)
			{
				obj.CustomSetActive(false);
				component.anchoredPosition = Vector2.zero;
				component.sizeDelta = Vector2.zero;
				componetInChild2.SetElementAmount(this.OBGuildList.get_Count());
			}
			else if (curTab == COBSystem.enOBTab.Local)
			{
				obj.CustomSetActive(this.OBLocalList.get_Count() > 0);
				component.sizeDelta = COBSystem.s_content_size;
				component.anchoredPosition = COBSystem.s_content_pos;
				componetInChild.text = Singleton<CTextManager>.get_instance().GetText((this.curStatus != COBSystem.enStatus.Normal) ? "Common_Close" : "Common_Edit");
				componetInChild2.SetElementAmount(this.OBLocalList.get_Count());
			}
		}

		private void OnElementEnable(CUIEvent cuiEvent)
		{
			COBSystem.enOBTab curTab = this.CurTab;
			if (curTab == COBSystem.enOBTab.Expert)
			{
				this.UpdateElement(cuiEvent.m_srcWidget, this.OBExpertList.get_Item(cuiEvent.m_srcWidgetIndexInBelongedList));
			}
			else if (curTab == COBSystem.enOBTab.Friend)
			{
				this.UpdateElement(cuiEvent.m_srcWidget, this.OBFriendList.get_Item(cuiEvent.m_srcWidgetIndexInBelongedList));
			}
			else if (curTab == COBSystem.enOBTab.Guild)
			{
				this.UpdateElement(cuiEvent.m_srcWidget, this.OBGuildList.get_Item(cuiEvent.m_srcWidgetIndexInBelongedList));
			}
			else if (curTab == COBSystem.enOBTab.Local)
			{
				this.UpdateElement(cuiEvent.m_srcWidget, this.OBLocalList.get_Item(cuiEvent.m_srcWidgetIndexInBelongedList));
			}
		}

		private void UpdateElement(GameObject element, COBSystem.stOBFriend OBFriend)
		{
			if (CFriendModel.RemarkNames != null && CFriendModel.RemarkNames.ContainsKey(OBFriend.uin.ullUid))
			{
				string empty = string.Empty;
				if (CFriendModel.RemarkNames.TryGetValue(OBFriend.uin.ullUid, ref empty))
				{
					if (!string.IsNullOrEmpty(empty))
					{
						this.UpdateElement(element, empty, Singleton<ApolloHelper>.GetInstance().ToSnsHeadUrl(OBFriend.headUrl), OBFriend.gameDetail.bGrade, OBFriend.gameDetail.dwClass, OBFriend.gameDetail.dwHeroID, COBSystem.enOBTab.Friend, (int)OBFriend.gameDetail.dwObserveNum, this.curStatus, 0L, 0, 0u, string.Empty);
					}
					else
					{
						this.UpdateElement(element, OBFriend.friendName, Singleton<ApolloHelper>.GetInstance().ToSnsHeadUrl(OBFriend.headUrl), OBFriend.gameDetail.bGrade, OBFriend.gameDetail.dwClass, OBFriend.gameDetail.dwHeroID, COBSystem.enOBTab.Friend, (int)OBFriend.gameDetail.dwObserveNum, this.curStatus, 0L, 0, 0u, string.Empty);
					}
				}
			}
			else
			{
				this.UpdateElement(element, OBFriend.friendName, Singleton<ApolloHelper>.GetInstance().ToSnsHeadUrl(OBFriend.headUrl), OBFriend.gameDetail.bGrade, OBFriend.gameDetail.dwClass, OBFriend.gameDetail.dwHeroID, COBSystem.enOBTab.Friend, (int)OBFriend.gameDetail.dwObserveNum, this.curStatus, 0L, 0, 0u, string.Empty);
			}
		}

		private void UpdateElement(GameObject element, COBSystem.stOBGuild obGuild)
		{
			this.UpdateElement(element, obGuild.playerName, obGuild.headUrl, obGuild.bGrade, obGuild.dwClass, obGuild.dwHeroID, COBSystem.enOBTab.Guild, (int)obGuild.dwObserveNum, this.curStatus, 0L, 0, 0u, obGuild.teamName);
		}

		private void UpdateElement(GameObject element, COBSystem.stOBExpert OBExpert)
		{
			this.UpdateElement(element, Utility.UTF8Convert(OBExpert.heroLabel.szRoleName), Singleton<ApolloHelper>.GetInstance().ToSnsHeadUrl(Utility.UTF8Convert(OBExpert.heroLabel.szHealUrl)), (byte)OBExpert.heroLabel.dwGrade, OBExpert.heroLabel.dwClass, OBExpert.heroLabel.dwHeroID, COBSystem.enOBTab.Expert, (int)OBExpert.observeNum, this.curStatus, 0L, 0, 0u, string.Empty);
		}

		private void UpdateElement(GameObject element, GameReplayModule.ReplayFileInfo OBLocal)
		{
			this.UpdateElement(element, OBLocal.userName, OBLocal.userHead, OBLocal.userRankGrade, OBLocal.userRankClass, OBLocal.heroId, COBSystem.enOBTab.Local, 0, this.curStatus, OBLocal.startTime, OBLocal.mapType, OBLocal.mapId, string.Empty);
		}

		private void UpdateElement(GameObject element, string name, string headUrl, byte bGrade, uint subGrade, uint heroId, COBSystem.enOBTab curTab, int onlineNum, COBSystem.enStatus status = COBSystem.enStatus.Normal, long localTicks = 0L, byte mapType = 0, uint mapId = 0u, string localName = "")
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(COBSystem.OB_FORM_PATH);
			if (form == null)
			{
				return;
			}
			CUIHttpImageScript componetInChild = Utility.GetComponetInChild<CUIHttpImageScript>(element, "HeadImg");
			Image componetInChild2 = Utility.GetComponetInChild<Image>(element, "HeroImg");
			Image componetInChild3 = Utility.GetComponetInChild<Image>(element, "RankImg");
			Image componetInChild4 = Utility.GetComponetInChild<Image>(element, "RankImg/SubRankImg");
			Text componetInChild5 = Utility.GetComponetInChild<Text>(element, "PlayerName");
			Text componetInChild6 = Utility.GetComponetInChild<Text>(element, "HeroName");
			GameObject obj = Utility.FindChild(element, "WatchImg");
			Text componetInChild7 = Utility.GetComponetInChild<Text>(element, "LocalTime");
			Text componetInChild8 = Utility.GetComponetInChild<Text>(element, "LocalMap");
			Text componetInChild9 = Utility.GetComponetInChild<Text>(element, "WatchImg/OnlineCount");
			GameObject obj2 = Utility.FindChild(element, "DeleteBtn");
			componetInChild.SetImageUrl(headUrl);
			if (bGrade > 0)
			{
				componetInChild3.gameObject.CustomSetActive(true);
				componetInChild3.SetSprite(CLadderView.GetRankSmallIconPath(bGrade, subGrade), form, true, false, false, false);
				componetInChild4.SetSprite(CLadderView.GetSubRankSmallIconPath(bGrade, subGrade), form, true, false, false, false);
			}
			else
			{
				componetInChild3.gameObject.CustomSetActive(false);
			}
			componetInChild5.text = name;
			ResHeroCfgInfo dataByKey = GameDataMgr.heroDatabin.GetDataByKey(heroId);
			if (dataByKey != null)
			{
				string prefabPath = string.Format("{0}{1}", CUIUtility.s_Sprite_Dynamic_BustHero_Dir, CSkinInfo.GetHeroSkinPic(heroId, 0u));
				componetInChild2.SetSprite(prefabPath, form, false, true, true, false);
				componetInChild6.text = dataByKey.szName;
			}
			else
			{
				componetInChild6.text = string.Empty;
				DebugHelper.Assert(false, string.Format("COBSystem UpdateElement hero cfg[{0}] can not be found!", heroId));
			}
			if (curTab != COBSystem.enOBTab.Local)
			{
				obj.CustomSetActive(true);
				componetInChild9.text = Singleton<CTextManager>.get_instance().GetText("OB_Desc_3", new string[]
				{
					onlineNum.ToString()
				});
				componetInChild7.gameObject.SetActive(false);
				obj2.CustomSetActive(false);
				componetInChild8.gameObject.CustomSetActive(false);
			}
			else
			{
				obj.CustomSetActive(false);
				componetInChild7.gameObject.SetActive(true);
				DateTime dateTime = new DateTime(localTicks);
				componetInChild7.text = Singleton<CTextManager>.get_instance().GetText("OB_Desc_12", new string[]
				{
					dateTime.get_Month().ToString(),
					dateTime.get_Day().ToString(),
					dateTime.get_Hour().ToString("D2"),
					dateTime.get_Minute().ToString("D2")
				});
				obj2.CustomSetActive(status == COBSystem.enStatus.Editor);
				componetInChild8.gameObject.CustomSetActive(true);
				ResDT_LevelCommonInfo pvpMapCommonInfo = CLevelCfgLogicManager.GetPvpMapCommonInfo(mapType, mapId);
				if (pvpMapCommonInfo != null)
				{
					componetInChild8.text = pvpMapCommonInfo.szName;
				}
				else
				{
					componetInChild8.text = string.Empty;
				}
				componetInChild6.text = string.Empty;
			}
		}

		private void OnVideoEnter(CUIEvent cuiEvent)
		{
			if (this.curStatus == COBSystem.enStatus.Editor)
			{
				return;
			}
			if (Singleton<CMatchingSystem>.GetInstance().IsInMatching)
			{
				Singleton<CUIManager>.GetInstance().OpenTips("PVP_Matching", true, 1.5f, null, new object[0]);
				return;
			}
			Singleton<CUIManager>.get_instance().OpenMessageBoxWithCancel(Singleton<CTextManager>.get_instance().GetText("OB_Desc_11"), enUIEventID.OB_Video_Enter_Confirm, enUIEventID.None, false);
		}

		private void OnVideoEnterConfirm(CUIEvent cuiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(COBSystem.OB_FORM_PATH);
			if (form == null)
			{
				return;
			}
			CUIListScript componetInChild = Utility.GetComponetInChild<CUIListScript>(form.gameObject, "ContentList");
			if (componetInChild == null)
			{
				return;
			}
			int selectedIndex = componetInChild.GetSelectedIndex();
			COBSystem.enOBTab curTab = this.CurTab;
			if (curTab == COBSystem.enOBTab.Expert)
			{
				if (selectedIndex >= 0 && selectedIndex < this.OBExpertList.get_Count())
				{
					Singleton<WatchController>.GetInstance().TargetUID = this.OBExpertList.get_Item(selectedIndex).heroLabel.ullUid;
					COBSystem.SendOBServeGreat(this.OBExpertList.get_Item(selectedIndex).desk);
				}
				int count = this.OBExpertList.get_Count();
			}
			else if (curTab == COBSystem.enOBTab.Friend)
			{
				if (selectedIndex >= 0 && selectedIndex < this.OBFriendList.get_Count())
				{
					Singleton<WatchController>.GetInstance().TargetUID = this.OBFriendList.get_Item(selectedIndex).uin.ullUid;
					COBSystem.SendOBServeFriend(this.OBFriendList.get_Item(selectedIndex).uin);
				}
				int count = this.OBFriendList.get_Count();
			}
			else if (curTab == COBSystem.enOBTab.Guild)
			{
				if (selectedIndex >= 0 && selectedIndex < this.OBGuildList.get_Count())
				{
					Singleton<WatchController>.GetInstance().TargetUID = this.OBGuildList.get_Item(selectedIndex).playerUid;
					Singleton<CGuildMatchSystem>.GetInstance().RequestObGuildMatch(this.OBGuildList.get_Item(selectedIndex).obUid);
				}
				int count = this.OBGuildList.get_Count();
			}
			else if (curTab == COBSystem.enOBTab.Local)
			{
				Singleton<WatchController>.GetInstance().TargetUID = Singleton<CRoleInfoManager>.get_instance().masterUUID;
				if (selectedIndex >= 0 && selectedIndex < this.OBLocalList.get_Count())
				{
					Singleton<WatchController>.GetInstance().StartReplay(this.OBLocalList.get_Item(selectedIndex).path);
				}
				int count = this.OBLocalList.get_Count();
			}
		}

		public void OBFriend(COMDT_ACNT_UNIQ uin)
		{
			bool flag = false;
			for (int i = 0; i < this.OBFriendList.get_Count(); i++)
			{
				if (uin.ullUid == this.OBFriendList.get_Item(i).uin.ullUid)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				COBSystem.SendOBServeFriend(uin);
			}
			else
			{
				Singleton<CUIManager>.get_instance().OpenTips("OB_Error_1", false, 1.5f, null, new object[0]);
			}
		}

		private void On_Friend_SNS_STATE_NTF(CSPkg msg)
		{
			SCPKG_NTF_SNS_FRIEND stNtfSnsFriend = msg.stPkgData.get_stNtfSnsFriend();
			int num = 0;
			while ((long)num < (long)((ulong)stNtfSnsFriend.dwSnsFriendNum))
			{
				CSDT_SNS_FRIEND_INFO cSDT_SNS_FRIEND_INFO = stNtfSnsFriend.astSnsFriendList[num];
				if (cSDT_SNS_FRIEND_INFO != null)
				{
					if (cSDT_SNS_FRIEND_INFO.bVideoState != 0)
					{
						if (cSDT_SNS_FRIEND_INFO.stGameInfo.get_stDetail() == null)
						{
							DebugHelper.Assert(false, "SCPKG_NTF_SNS_FRIEND [bMultGameSubState == COM_ACNT_MULTIGAME_GAMING] and  [stGameInfo.stDetail == null] , this is sever' guo!");
						}
						else
						{
							bool flag = false;
							for (int i = 0; i < this.OBFriendList.get_Count(); i++)
							{
								if (stNtfSnsFriend.astSnsFriendList[num].stSnsFrindInfo.stUin.ullUid == this.OBFriendList.get_Item(i).uin.ullUid)
								{
									COBSystem.stOBFriend stOBFriend = this.OBFriendList.get_Item(i);
									stOBFriend.gameDetail = stNtfSnsFriend.astSnsFriendList[num].stGameInfo.get_stDetail();
									this.OBFriendList.set_Item(i, stOBFriend);
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								COBSystem.stOBFriend stOBFriend = default(COBSystem.stOBFriend);
								stOBFriend.uin = stNtfSnsFriend.astSnsFriendList[num].stSnsFrindInfo.stUin;
								stOBFriend.friendName = Utility.UTF8Convert(stNtfSnsFriend.astSnsFriendList[num].stSnsFrindInfo.szUserName);
								stOBFriend.headUrl = Utility.UTF8Convert(stNtfSnsFriend.astSnsFriendList[num].stSnsFrindInfo.szHeadUrl);
								stOBFriend.gameDetail = stNtfSnsFriend.astSnsFriendList[num].stGameInfo.get_stDetail();
								this.OBFriendList.Add(stOBFriend);
							}
						}
					}
				}
				num++;
			}
			this.UpdateView();
		}

		private void On_FriendSys_Friend_List(CSPkg msg)
		{
			SCPKG_CMD_LIST_FRIEND stFriendListRsp = msg.stPkgData.get_stFriendListRsp();
			int i = 0;
			while ((long)i < (long)((ulong)stFriendListRsp.dwFriendNum))
			{
				CSDT_FRIEND_INFO cSDT_FRIEND_INFO = stFriendListRsp.astFrindList[i];
				if (cSDT_FRIEND_INFO != null)
				{
					if (cSDT_FRIEND_INFO.bVideoState != 0)
					{
						if (cSDT_FRIEND_INFO.stGameInfo.get_stDetail() == null)
						{
							DebugHelper.Assert(false, "CSDT_FRIEND_INFO [bMultGameSubState == COM_ACNT_MULTIGAME_GAMING] and  [stGameInfo.stDetail == null] , this is sever' guo!");
						}
						else
						{
							bool flag = false;
							int num = 0;
							while (i < this.OBFriendList.get_Count())
							{
								if (stFriendListRsp.astFrindList[i].stFriendInfo.stUin.ullUid == this.OBFriendList.get_Item(num).uin.ullUid)
								{
									COBSystem.stOBFriend stOBFriend = this.OBFriendList.get_Item(num);
									stOBFriend.gameDetail = stFriendListRsp.astFrindList[i].stGameInfo.get_stDetail();
									this.OBFriendList.set_Item(num, stOBFriend);
									flag = true;
									break;
								}
								num++;
							}
							if (!flag)
							{
								COBSystem.stOBFriend stOBFriend = default(COBSystem.stOBFriend);
								stOBFriend.uin = stFriendListRsp.astFrindList[i].stFriendInfo.stUin;
								stOBFriend.friendName = Utility.UTF8Convert(stFriendListRsp.astFrindList[i].stFriendInfo.szUserName);
								stOBFriend.headUrl = Utility.UTF8Convert(stFriendListRsp.astFrindList[i].stFriendInfo.szHeadUrl);
								stOBFriend.gameDetail = stFriendListRsp.astFrindList[i].stGameInfo.get_stDetail();
								this.OBFriendList.Add(stOBFriend);
							}
						}
					}
				}
				i++;
			}
			this.UpdateView();
		}

		private void On_Friend_GAME_STATE_NTF(CSPkg msg)
		{
			SCPKG_CMD_NTF_FRIEND_GAME_STATE stNtfFriendGameState = msg.stPkgData.get_stNtfFriendGameState();
			COBSystem.stOBFriend stOBFriend;
			for (int i = 0; i < this.OBFriendList.get_Count(); i++)
			{
				if (this.OBFriendList.get_Item(i).uin.ullUid == stNtfFriendGameState.stAcntUniq.ullUid)
				{
					if (stNtfFriendGameState.bVideoState == 0)
					{
						this.OBFriendList.RemoveAt(i);
					}
					else if (stNtfFriendGameState.bVideoState == 1)
					{
						stOBFriend = this.OBFriendList.get_Item(i);
						stOBFriend.gameDetail = stNtfFriendGameState.stGameInfo.get_stDetail();
						this.OBFriendList.set_Item(i, stOBFriend);
					}
					this.UpdateView();
					return;
				}
			}
			if (stNtfFriendGameState.bVideoState == 0)
			{
				return;
			}
			if (stNtfFriendGameState.stGameInfo.get_stDetail() == null)
			{
				DebugHelper.Assert(false, "SCPKG_CMD_NTF_FRIEND_GAME_STATE [bMultGameSubState == COM_ACNT_MULTIGAME_GAMING] and  [stGameInfo.stDetail == null] , this is sever' guo!");
				return;
			}
			COMDT_FRIEND_INFO friendByUid = Singleton<CFriendContoller>.get_instance().model.getFriendByUid(stNtfFriendGameState.stAcntUniq.ullUid, CFriendModel.FriendType.GameFriend, 0u);
			if (friendByUid == null)
			{
				friendByUid = Singleton<CFriendContoller>.get_instance().model.getFriendByUid(stNtfFriendGameState.stAcntUniq.ullUid, CFriendModel.FriendType.SNS, 0u);
			}
			if (friendByUid == null)
			{
				return;
			}
			stOBFriend = default(COBSystem.stOBFriend);
			stOBFriend.uin = stNtfFriendGameState.stAcntUniq;
			stOBFriend.friendName = Utility.UTF8Convert(friendByUid.szUserName);
			stOBFriend.headUrl = Utility.UTF8Convert(friendByUid.szHeadUrl);
			stOBFriend.gameDetail = stNtfFriendGameState.stGameInfo.get_stDetail();
			this.OBFriendList.Add(stOBFriend);
			this.UpdateView();
		}

		private void OnGetGreatMatch(CSPkg msg)
		{
			this.OBExpertList.Clear();
			int num = 0;
			while ((long)num < (long)((ulong)msg.stPkgData.get_stGetGreatMatchRsp().dwCount))
			{
				int num2 = 0;
				while ((long)num2 < (long)((ulong)msg.stPkgData.get_stGetGreatMatchRsp().astList[num].dwLabelNum))
				{
					COBSystem.stOBExpert stOBExpert = default(COBSystem.stOBExpert);
					stOBExpert.desk = msg.stPkgData.get_stGetGreatMatchRsp().astList[num].stDesk;
					stOBExpert.startTime = msg.stPkgData.get_stGetGreatMatchRsp().astList[num].dwStartTime;
					stOBExpert.observeNum = msg.stPkgData.get_stGetGreatMatchRsp().astList[num].dwObserveNum;
					stOBExpert.heroLabel = msg.stPkgData.get_stGetGreatMatchRsp().astList[num].astLabel[num2];
					this.OBExpertList.Add(stOBExpert);
					num2++;
				}
				num++;
			}
			this.OBExpertList.Sort(new Comparison<COBSystem.stOBExpert>(this.SortByObserveNum));
			this.UpdateView();
		}

		private int SortByObserveNum(COBSystem.stOBExpert left, COBSystem.stOBExpert right)
		{
			return (int)(right.observeNum - left.observeNum);
		}

		public static void SendOBServeFriend(COMDT_ACNT_UNIQ uin)
		{
			if (Singleton<CMatchingSystem>.GetInstance().IsInMatching)
			{
				Singleton<CUIManager>.GetInstance().OpenTips("PVP_Matching", true, 1.5f, null, new object[0]);
				return;
			}
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5218u);
			cSPkg.stPkgData.get_stObserveFriendReq().bType = ((!Singleton<CFriendContoller>.GetInstance().model.IsGameFriend(uin.ullUid, uin.dwLogicWorldId)) ? 2 : 1);
			cSPkg.stPkgData.get_stObserveFriendReq().stFriendUniq = uin;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		[MessageHandler(5219)]
		public static void ON_OBSERVE_FRIEND_RSP(CSPkg msg)
		{
			Singleton<CUIManager>.get_instance().CloseSendMsgAlert();
			if (msg.stPkgData.get_stObserveFriendRsp().iResult == 0)
			{
				if (Singleton<WatchController>.GetInstance().StartObserve(msg.stPkgData.get_stObserveFriendRsp().stTgwinfo))
				{
					Singleton<CUIManager>.GetInstance().OpenSendMsgAlert(5, enUIEventID.None);
				}
			}
			else
			{
				Singleton<COBSystem>.get_instance().UpdateView();
				Singleton<CUIManager>.get_instance().OpenTips(string.Format("OB_Error_{0}", msg.stPkgData.get_stObserveFriendRsp().iResult), true, 1.5f, null, new object[0]);
			}
		}

		public static void SendOBServeGreat(COMDT_OB_DESK desk)
		{
			if (Singleton<CMatchingSystem>.GetInstance().IsInMatching)
			{
				Singleton<CUIManager>.GetInstance().OpenTips("PVP_Matching", true, 1.5f, null, new object[0]);
				return;
			}
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5220u);
			cSPkg.stPkgData.get_stObserveGreatReq().stDesk = desk;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		[MessageHandler(5221)]
		public static void ON_OBSERVE_GREAT_RSP(CSPkg msg)
		{
			Singleton<CUIManager>.get_instance().CloseSendMsgAlert();
			if (msg.stPkgData.get_stObserveGreatRsp().iResult == 0)
			{
				if (Singleton<WatchController>.GetInstance().StartObserve(null))
				{
					Singleton<CUIManager>.GetInstance().OpenSendMsgAlert(5, enUIEventID.None);
				}
			}
			else
			{
				COBSystem.GetGreatMatch(true);
				Singleton<CUIManager>.get_instance().OpenTips(string.Format("OB_Error_{0}", msg.stPkgData.get_stObserveGreatRsp().iResult), true, 1.5f, null, new object[0]);
			}
		}

		public static void GetGreatMatch(bool bForce = false)
		{
			if (bForce || CRoleInfo.GetCurrentUTCTime() - COBSystem.m_lastGetExpertListTime > COBSystem.EXPERT_DETAL_SEC)
			{
				CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5222u);
				Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
				COBSystem.m_lastGetExpertListTime = CRoleInfo.GetCurrentUTCTime();
			}
		}

		public static void GetFriendsState()
		{
			if (CRoleInfo.GetCurrentUTCTime() - COBSystem.m_lastGetFriendStateTime > COBSystem.FRIEND_DETAL_SEC)
			{
				CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5233u);
				Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
				COBSystem.m_lastGetFriendStateTime = CRoleInfo.GetCurrentUTCTime();
			}
		}

		public void SetGuildMatchOBCount(SCPKG_GET_GUILD_MATCH_OB_CNT_RSP msg)
		{
			bool flag = false;
			for (int i = 0; i < (int)msg.bMatchCnt; i++)
			{
				COMDT_GUILD_MATCH_OB_CNT cOMDT_GUILD_MATCH_OB_CNT = msg.astMatchOBCnt[i];
				for (int j = 0; j < this.OBGuildList.get_Count(); j++)
				{
					if (this.OBGuildList.get_Item(j).obUid == cOMDT_GUILD_MATCH_OB_CNT.ullUid && this.OBGuildList.get_Item(j).dwObserveNum != cOMDT_GUILD_MATCH_OB_CNT.dwOBCnt)
					{
						this.OBGuildList.get_Item(j).dwObserveNum = cOMDT_GUILD_MATCH_OB_CNT.dwOBCnt;
						flag = true;
					}
				}
			}
			if (flag && this.CurTab == COBSystem.enOBTab.Guild)
			{
				this.UpdateView();
			}
		}

		[MessageHandler(5223)]
		public static void ON_GET_GREATMATCH_RSP(CSPkg msg)
		{
			Singleton<COBSystem>.get_instance().OnGetGreatMatch(msg);
			Singleton<CUIManager>.get_instance().CloseSendMsgAlert();
		}
	}
}
