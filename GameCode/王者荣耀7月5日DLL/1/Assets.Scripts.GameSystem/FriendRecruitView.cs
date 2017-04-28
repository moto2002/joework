using Assets.Scripts.Framework;
using Assets.Scripts.UI;
using CSProtocol;
using ResData;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	public class FriendRecruitView
	{
		public enum enRecruitWidget
		{
			None = -1,
			HttpImage,
			NobeIcon,
			Name,
			Gender
		}

		public enum Tab
		{
			None,
			Zhaomu_Reward,
			BeiZhaomu_Reward
		}

		public const string FRDataChange = "FRDataChange";

		public CUIListScript zhaomuzheList;

		public GameObject node;

		public GameObject zhaomu_content;

		public Text zm_benifit_exp;

		public Text zm_benifit_gold;

		public Text zm_totalProgress;

		public GameObject zm_progressNode;

		public Text zm_boxTxt;

		public GameObject supberRewardNode;

		public Text supberRewardNodeTxt;

		public GameObject beizhaomu_content;

		private static int zmzCount = 4;

		private static int zmzLevel = 20;

		private static int ruleId = 23;

		private int zhaomuzheRewardCount = 3;

		public void Show()
		{
			this.node.CustomSetActive(true);
			Singleton<CFriendContoller>.get_instance().model.friendRecruit.Check();
			this.On_FRDataChange();
		}

		public void Hide()
		{
			this.node.CustomSetActive(false);
		}

		public void Init(GameObject node)
		{
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_Recruit_zmzBtn, new CUIEventManager.OnUIEventHandler(this.On_Friend_Recruit_zmzBtn));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_Recruit_bzmzBtn, new CUIEventManager.OnUIEventHandler(this.On_Friend_Recruit_bzmzBtn));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_Recruit_bzmRoleBtn, new CUIEventManager.OnUIEventHandler(this.On_Friend_Recruit_bzmRoleBtn));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_Recruit_RecruitBtn, new CUIEventManager.OnUIEventHandler(this.On_Friend_Recruit_RecruitBtn));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_Recruit_zmzListEnable, new CUIEventManager.OnUIEventHandler(this.On_Friend_Recruit_zmzListEnable));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_Recruit_zmzItemClickDown, new CUIEventManager.OnUIEventHandler(this.On_Friend_Recruit_zmzItemClickDown));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Friend_Recruit_TabChange, new CUIEventManager.OnUIEventHandler(this.On_Friend_Recruit_TabChange));
			Singleton<EventRouter>.GetInstance().AddEventHandler("FRDataChange", new Action(this.On_FRDataChange));
			this.node = node;
			this.zhaomu_content = node.transform.FindChild("zhaomu_content").gameObject;
			this.zm_benifit_exp = node.transform.FindChild("zhaomu_content/benift/exp/icon/txt").GetComponent<Text>();
			this.zm_benifit_gold = node.transform.FindChild("zhaomu_content/benift/gold/icon/txt").GetComponent<Text>();
			this.zm_totalProgress = node.transform.FindChild("zhaomu_content/progress_node/progress_txt").GetComponent<Text>();
			this.zm_progressNode = node.transform.FindChild("zhaomu_content/progress_node/progress").gameObject;
			this.zm_boxTxt = node.transform.FindChild("zhaomu_content/progress_node/icon/txt").GetComponent<Text>();
			this.zhaomuzheList = node.transform.FindChild("zhaomu_content/list").GetComponent<CUIListScript>();
			this.supberRewardNode = node.transform.FindChild("zhaomu_content/progress_node/superReward").gameObject;
			this.supberRewardNodeTxt = node.transform.FindChild("zhaomu_content/progress_node/superReward/box/num").GetComponent<Text>();
			this.beizhaomu_content = node.transform.FindChild("beizhaomu_content").gameObject;
			CUIListScript component = node.transform.FindChild("tab").GetComponent<CUIListScript>();
			string[] array = new string[]
			{
				Singleton<CTextManager>.get_instance().GetText("Friend_Rec_ZMZTitle"),
				Singleton<CTextManager>.get_instance().GetText("Friend_Rec_BZMZTitle")
			};
			UT.SetTabList(Enumerable.ToList<string>(array), 0, component);
			this.ShowNode(true);
		}

		private void On_FRDataChange()
		{
			if (this.zhaomu_content != null)
			{
				this.ShowNode(this.zhaomu_content.activeSelf);
			}
		}

		public void Clear()
		{
			this.node = null;
			this.zhaomuzheList = null;
			this.zhaomu_content = null;
			this.zm_benifit_exp = null;
			this.zm_benifit_gold = null;
			this.zm_totalProgress = null;
			this.zm_progressNode = null;
			this.zm_boxTxt = null;
			this.supberRewardNode = null;
			this.supberRewardNodeTxt = null;
			this.beizhaomu_content = null;
			Singleton<EventRouter>.GetInstance().RemoveEventHandler("FRDataChange", new Action(this.On_FRDataChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Friend_Recruit_zmzBtn, new CUIEventManager.OnUIEventHandler(this.On_Friend_Recruit_zmzBtn));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Friend_Recruit_bzmzBtn, new CUIEventManager.OnUIEventHandler(this.On_Friend_Recruit_bzmzBtn));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Friend_Recruit_bzmRoleBtn, new CUIEventManager.OnUIEventHandler(this.On_Friend_Recruit_bzmRoleBtn));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Friend_Recruit_RecruitBtn, new CUIEventManager.OnUIEventHandler(this.On_Friend_Recruit_RecruitBtn));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Friend_Recruit_zmzListEnable, new CUIEventManager.OnUIEventHandler(this.On_Friend_Recruit_zmzListEnable));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Friend_Recruit_zmzItemClickDown, new CUIEventManager.OnUIEventHandler(this.On_Friend_Recruit_zmzItemClickDown));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Friend_Recruit_TabChange, new CUIEventManager.OnUIEventHandler(this.On_Friend_Recruit_TabChange));
		}

		public void ShowNode(bool bZhaomuzhe)
		{
			if (bZhaomuzhe)
			{
				this.Show_ZhouMoZhe_Reward();
			}
			else
			{
				this.Show_BeiZhouMoZhe_Reward();
			}
		}

		private void Show_ZhouMoZhe_Reward()
		{
			CFriendRecruit friendRecruit = Singleton<CFriendContoller>.get_instance().model.friendRecruit;
			if (this.zm_benifit_exp != null)
			{
				this.zm_benifit_exp.text = string.Format("+{0}%", friendRecruit.GetZhaoMuZhe_RewardExp());
			}
			if (this.zm_benifit_gold != null)
			{
				this.zm_benifit_gold.text = string.Format("+{0}%", friendRecruit.GetZhaoMuZhe_RewardGold());
			}
			int zhaoMuZhe_RewardProgress = friendRecruit.GetZhaoMuZhe_RewardProgress();
			int zhaoMuZhe_RewardTotalCount = friendRecruit.GetZhaoMuZhe_RewardTotalCount();
			string text = Singleton<CTextManager>.get_instance().GetText("Friend_Rec_zmz_whole_Progress", new string[]
			{
				zhaoMuZhe_RewardProgress.ToString(),
				zhaoMuZhe_RewardTotalCount.ToString()
			});
			if (this.zm_totalProgress != null)
			{
				this.zm_totalProgress.text = text;
			}
			for (int i = 0; i < zhaoMuZhe_RewardProgress; i++)
			{
				GameObject gameObject = this.zm_progressNode.transform.FindChild(string.Format("icon_{0}", i)).gameObject;
				this.ShowBar(gameObject, true);
			}
			for (int j = zhaoMuZhe_RewardProgress; j < zhaoMuZhe_RewardTotalCount; j++)
			{
				GameObject gameObject2 = this.zm_progressNode.transform.FindChild(string.Format("icon_{0}", j)).gameObject;
				this.ShowBar(gameObject2, false);
			}
			if (this.supberRewardNodeTxt != null)
			{
				this.supberRewardNodeTxt.text = Singleton<CTextManager>.get_instance().GetText("Friend_Rec_zmz_Progress", new string[]
				{
					FriendRecruitView.zmzCount.ToString(),
					FriendRecruitView.zmzLevel.ToString()
				});
			}
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CFriendContoller.FriendFormPath);
			this.Show_Award(this.supberRewardNode, 0uL, 0u, 1, friendRecruit.SuperReward.rewardID, friendRecruit.SuperReward.state, form, false);
			this.Refresh_ZhaomuZhe_List();
			this.zhaomu_content.CustomSetActive(true);
			this.beizhaomu_content.CustomSetActive(false);
		}

		private void Show_BeiZhouMoZhe_Reward()
		{
			if (this.zhaomu_content == null || this.beizhaomu_content == null)
			{
				return;
			}
			this.zhaomu_content.CustomSetActive(false);
			this.beizhaomu_content.CustomSetActive(true);
			CFriendRecruit friendRecruit = Singleton<CFriendContoller>.get_instance().model.friendRecruit;
			Text component = this.beizhaomu_content.transform.FindChild("info/text").GetComponent<Text>();
			Text component2 = this.beizhaomu_content.transform.FindChild("info/benift/exp/icon/txt").GetComponent<Text>();
			Text component3 = this.beizhaomu_content.transform.FindChild("info/benift/gold/icon/txt").GetComponent<Text>();
			if (component2 != null)
			{
				component2.text = string.Format("+{0}%", friendRecruit.GetBeiZhaoMuZhe_RewardExp());
			}
			if (component3 != null)
			{
				component3.text = string.Format("+{0}%", friendRecruit.GetBeiZhaoMuZhe_RewardGold());
			}
			GameObject gameObject = this.beizhaomu_content.transform.FindChild("info/user").gameObject;
			gameObject.CustomSetActive(true);
			GameObject gameObject2 = gameObject.transform.FindChild("default").gameObject;
			gameObject2.GetComponent<Text>().text = Singleton<CTextManager>.get_instance().GetText("Friend_Rec_Bei_User_DefalutTxt");
			this.beizhaomu_content.transform.FindChild("info/reward/title/Text").GetComponent<Text>().text = Singleton<CTextManager>.get_instance().GetText("Friend_Rec_Bei_RewardTip");
			GameObject gameObject3 = gameObject.transform.FindChild("NameGroup").gameObject;
			CFriendRecruit.RecruitData beiZhaoMuZhe = friendRecruit.GetBeiZhaoMuZhe();
			Text component4 = gameObject.transform.FindChild("Level").GetComponent<Text>();
			if (beiZhaoMuZhe.userInfo == null)
			{
				component.text = Singleton<CTextManager>.get_instance().GetText("Friend_Rec_Bei_Role_NoData");
				gameObject2.CustomSetActive(true);
				gameObject3.CustomSetActive(false);
				component4.gameObject.CustomSetActive(false);
			}
			else
			{
				component.text = Singleton<CTextManager>.get_instance().GetText("Friend_Rec_Bei_Role_HasData");
				gameObject2.CustomSetActive(false);
				gameObject3.CustomSetActive(true);
				component4.gameObject.CustomSetActive(true);
				CUIHttpImageScript component5 = gameObject.transform.FindChild("pnlSnsHead/HttpImage").GetComponent<CUIHttpImageScript>();
				UT.SetHttpImage(component5, beiZhaoMuZhe.userInfo.szHeadUrl);
				component4.text = string.Format("Lv.{0}", beiZhaoMuZhe.userInfo.dwPvpLvl);
				GameObject gameObject4 = gameObject.transform.FindChild("pnlSnsHead/HttpImage/NobeIcon").gameObject;
				if (gameObject4)
				{
					MonoSingleton<NobeSys>.GetInstance().SetNobeIcon(gameObject4.GetComponent<Image>(), (int)beiZhaoMuZhe.userInfo.stGameVip.dwCurLevel, false);
				}
				Text component6 = gameObject.transform.FindChild("NameGroup/Name").GetComponent<Text>();
				string text = UT.Bytes2String(beiZhaoMuZhe.userInfo.szUserName);
				if (component6 != null)
				{
					component6.text = text;
				}
				GameObject gameObject5 = gameObject.transform.FindChild("NameGroup/Gender").gameObject;
				FriendShower.ShowGender(gameObject5, beiZhaoMuZhe.userInfo.bGender);
			}
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CFriendContoller.FriendFormPath);
			GameObject gameObject6 = this.beizhaomu_content.transform.FindChild("info/reward").gameObject;
			CFriendRecruit.RecruitData beiZhaoMuZhe2 = friendRecruit.GetBeiZhaoMuZhe();
			ulong ullUid = beiZhaoMuZhe2.ullUid;
			uint dwLogicWorldId = beiZhaoMuZhe2.dwLogicWorldId;
			int num = Math.Min(4, beiZhaoMuZhe2.RewardList.get_Count());
			for (int i = 0; i < num; i++)
			{
				CFriendRecruit.RecruitReward recruitReward = beiZhaoMuZhe2.RewardList.get_Item(i);
				GameObject gameObject7 = gameObject6.transform.FindChild(string.Format("reward_{0}", i)).gameObject;
				this.Show_Award(gameObject7, ullUid, dwLogicWorldId, 2, recruitReward.rewardID, recruitReward.state, form, true);
			}
		}

		private void On_Friend_Recruit_zmzItemClickDown(CUIEvent uievent)
		{
			ushort num = (ushort)uievent.m_eventParams.tagUInt;
			ulong commonUInt64Param = uievent.m_eventParams.commonUInt64Param1;
			uint taskId = uievent.m_eventParams.taskId;
			COM_RECRUITMENT_TYPE weakGuideId = uievent.m_eventParams.weakGuideId;
			if (num == 0)
			{
				return;
			}
			CFriendRecruit friendRecruit = Singleton<CFriendContoller>.get_instance().model.friendRecruit;
			CFriendRecruit.RecruitReward recruitReward;
			if (friendRecruit.SuperReward.rewardID == num)
			{
				recruitReward = friendRecruit.SuperReward;
			}
			else
			{
				recruitReward = friendRecruit.GetRecruitReward(commonUInt64Param, taskId, num);
			}
			if (recruitReward != null && recruitReward.state == CFriendRecruit.RewardState.Getted)
			{
				Singleton<CUIManager>.get_instance().OpenTips(UT.GetText("CS_HUOYUEDUREWARD_GETED"), false, 1.5f, null, new object[0]);
			}
			else if (recruitReward != null && recruitReward.state == CFriendRecruit.RewardState.Keling)
			{
				CFriendRecruitNetCore.Send_INTIMACY_RELATION_REQUEST(commonUInt64Param, taskId, num);
			}
			else
			{
				CUseable usable = friendRecruit.GetUsable(num);
				Singleton<CUICommonSystem>.get_instance().OpenUseableTips(usable, uievent.m_pointerEventData.pressPosition.x, uievent.m_pointerEventData.pressPosition.y, enUseableTipsPos.enTop);
			}
		}

		private void On_Friend_Recruit_TabChange(CUIEvent uievent)
		{
			int selectedIndex = uievent.m_srcWidget.GetComponent<CUIListScript>().GetSelectedIndex();
			if (selectedIndex == 0)
			{
				this.ShowNode(true);
			}
			else if (selectedIndex == 1)
			{
				this.ShowNode(false);
			}
		}

		private void On_Friend_Recruit_RecruitBtn(CUIEvent uievent)
		{
			MonoSingleton<ShareSys>.GetInstance().ShareRecruitFriend(Singleton<CTextManager>.GetInstance().GetText("ShareRecruit_Title"), Singleton<CTextManager>.GetInstance().GetText("ShareRecruit_Desc"));
		}

		private void On_Friend_Recruit_bzmRoleBtn(CUIEvent uievent)
		{
			ResRuleText dataByKey = GameDataMgr.s_ruleTextDatabin.GetDataByKey((long)FriendRecruitView.ruleId);
			if (dataByKey != null)
			{
				string title = StringHelper.UTF8BytesToString(ref dataByKey.szTitle);
				string info = StringHelper.UTF8BytesToString(ref dataByKey.szContent);
				Singleton<CUIManager>.GetInstance().OpenInfoForm(title, info);
			}
		}

		private void On_Friend_Recruit_bzmzBtn(CUIEvent uievent)
		{
		}

		private void On_Friend_Recruit_zmzBtn(CUIEvent uievent)
		{
		}

		public void Refresh_ZhaomuZhe_List()
		{
			CFriendModel model = Singleton<CFriendContoller>.GetInstance().model;
			ListView<CFriendRecruit.RecruitData> zhaoMuZheRewardList = model.friendRecruit.GetZhaoMuZheRewardList();
			this.zhaomuzheList.SetElementAmount(zhaoMuZheRewardList.get_Count());
			for (int i = 0; i < zhaoMuZheRewardList.get_Count(); i++)
			{
				CUIListElementScript elemenet = this.zhaomuzheList.GetElemenet(i);
				if (elemenet != null)
				{
					this.ShowZhaomuZhe_Item(elemenet.gameObject, zhaoMuZheRewardList.get_Item(i));
				}
			}
		}

		public void On_Friend_Recruit_zmzListEnable(CUIEvent uievent)
		{
			if (uievent == null)
			{
				return;
			}
			int srcWidgetIndexInBelongedList = uievent.m_srcWidgetIndexInBelongedList;
			CFriendModel model = Singleton<CFriendContoller>.GetInstance().model;
			ListView<CFriendRecruit.RecruitData> zhaoMuZheRewardList = model.friendRecruit.GetZhaoMuZheRewardList();
			if (zhaoMuZheRewardList == null)
			{
				return;
			}
			CFriendRecruit.RecruitData recruitData = null;
			if (srcWidgetIndexInBelongedList >= 0 && srcWidgetIndexInBelongedList < zhaoMuZheRewardList.get_Count())
			{
				recruitData = zhaoMuZheRewardList.get_Item(srcWidgetIndexInBelongedList);
			}
			if (recruitData != null && uievent.m_srcWidget != null && uievent.m_srcWidget != null)
			{
				this.ShowZhaomuZhe_Item(uievent.m_srcWidget, recruitData);
			}
		}

		public void ShowZhaomuZhe_Item(GameObject com, CFriendRecruit.RecruitData info)
		{
			if (info.userInfo != null)
			{
				com.transform.FindChild("user/hasData").gameObject.CustomSetActive(true);
				com.transform.FindChild("user/null").gameObject.CustomSetActive(false);
				com.transform.FindChild("user/hasData/Level").gameObject.CustomSetActive(true);
				CUIHttpImageScript component = com.transform.FindChild("user/hasData/pnlSnsHead/HttpImage").GetComponent<CUIHttpImageScript>();
				UT.SetHttpImage(component, info.userInfo.szHeadUrl);
				Text component2 = com.transform.FindChild("user/hasData/Level").GetComponent<Text>();
				component2.text = string.Format("Lv.{0}", info.userInfo.dwPvpLvl);
				GameObject gameObject = com.transform.FindChild("user/hasData/pnlSnsHead/HttpImage/NobeIcon").gameObject;
				if (gameObject)
				{
					MonoSingleton<NobeSys>.GetInstance().SetNobeIcon(gameObject.GetComponent<Image>(), (int)info.userInfo.stGameVip.dwCurLevel, false);
				}
				Text component3 = com.transform.FindChild("user/hasData/NameGroup/Name").GetComponent<Text>();
				string text = UT.Bytes2String(info.userInfo.szUserName);
				if (component3 != null)
				{
					component3.text = text;
				}
				GameObject gameObject2 = com.transform.FindChild("user/hasData/NameGroup/Gender").gameObject;
				FriendShower.ShowGender(gameObject2, info.userInfo.bGender);
			}
			else
			{
				com.transform.FindChild("user/hasData").gameObject.CustomSetActive(false);
				com.transform.FindChild("user/null").gameObject.CustomSetActive(true);
			}
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CFriendContoller.FriendFormPath);
			int num = Math.Min(this.zhaomuzheRewardCount, info.RewardList.get_Count());
			for (int i = 0; i < num; i++)
			{
				Transform transform = com.transform.FindChild(string.Format("reward_{0}", i));
				DebugHelper.Assert(transform != null, "rewardNodeTS not null...");
				if (!(transform == null))
				{
					CFriendRecruit.RecruitReward recruitReward = info.RewardList.get_Item(i);
					this.Show_Award(transform.gameObject, info.ullUid, info.dwLogicWorldId, 1, recruitReward.rewardID, recruitReward.state, form, true);
				}
			}
		}

		public void Show_Award(GameObject node, ulong ullUid, uint dwLogicWorldId, COM_RECRUITMENT_TYPE type, ushort rewardID, CFriendRecruit.RewardState state, CUIFormScript formScript, bool bShowLevelNum = true)
		{
			Image component = node.transform.FindChild("box/icon").GetComponent<Image>();
			CUIEventScript component2 = component.GetComponent<CUIEventScript>();
			component2.m_onDownEventParams.tagUInt = (uint)rewardID;
			component2.m_onDownEventParams.commonUInt64Param1 = ullUid;
			component2.m_onDownEventParams.taskId = dwLogicWorldId;
			component2.m_onDownEventParams.weakGuideId = (uint)type;
			CFriendRecruit friendRecruit = Singleton<CFriendContoller>.get_instance().model.friendRecruit;
			ResRecruitmentReward cfgReward = friendRecruit.GetCfgReward(rewardID);
			if (cfgReward == null)
			{
				return;
			}
			component.SetSprite(CUIUtility.s_Sprite_Dynamic_Icon_Dir + cfgReward.szIcon, formScript, true, false, false, false);
			if (bShowLevelNum)
			{
				Text component3 = node.transform.FindChild("box/num").GetComponent<Text>();
				component3.text = cfgReward.dwLevel.ToString();
			}
			bool flag = state == CFriendRecruit.RewardState.Getted;
			Image component4 = node.transform.FindChild("box/mark").GetComponent<Image>();
			component4.gameObject.CustomSetActive(flag);
			Transform transform = node.transform.FindChild("icon");
			if (transform != null)
			{
				this.ShowBar(transform.gameObject, flag);
			}
			bool flag2 = state == CFriendRecruit.RewardState.Keling;
			GameObject gameObject = node.transform.FindChild("box/effect").gameObject;
			gameObject.CustomSetActive(flag2);
			node.transform.FindChild("box").GetComponent<Animation>().enabled = flag2;
		}

		private void ShowBar(GameObject node, bool bOutterShow)
		{
			if (node == null)
			{
				return;
			}
			node.CustomSetActive(true);
			Transform transform = node.transform.FindChild("outer");
			if (transform != null)
			{
				transform.gameObject.CustomSetActive(bOutterShow);
			}
		}
	}
}
