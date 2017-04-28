using Assets.Scripts.UI;
using CSProtocol;
using System;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	public class IntimacyRelationView
	{
		public const string FRDataChange = "FRDataChange";

		public CUIListScript listScript;

		public void Open()
		{
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.IntimacyRela_Menu_Close, new CUIEventManager.OnUIEventHandler(this.On_IntimacyRela_Menu_Close));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.IntimacyRela_Show_Drop_List, new CUIEventManager.OnUIEventHandler(this.On_IntimacyRela_Show_Drop_List));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.IntimacyRela_Drop_ListElement_Click, new CUIEventManager.OnUIEventHandler(this.On_IntimacyRela_Drop_ListElement_Click));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.IntimacyRela_Drop_ListElement_Enable, new CUIEventManager.OnUIEventHandler(this.On_Drop_ListElement_Enable));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.IntimacyRela_OK, new CUIEventManager.OnUIEventHandler(this.On_IntimacyRela_OK));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.IntimacyRela_Cancle, new CUIEventManager.OnUIEventHandler(this.On_IntimacyRela_Cancle));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.IntimacyRela_Item_Enable, new CUIEventManager.OnUIEventHandler(this.On_IntimacyRelation_Item_Enable));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.IntimacyRela_FristChoise, new CUIEventManager.OnUIEventHandler(this.On_IntimacyRela_FristChoise));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.IntimacyRela_Delete_Relation, new CUIEventManager.OnUIEventHandler(this.On_IntimacyRela_Delete_Relation));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.IntimacyRela_PrevState, new CUIEventManager.OnUIEventHandler(this.On_IntimacyRela_PrevState));
			Singleton<EventRouter>.GetInstance().AddEventHandler("FRDataChange", new Action(this.On_FRDataChange));
			CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(CFriendContoller.IntimacyRelaFormPath, false, true);
			this.listScript = cUIFormScript.transform.FindChild("GameObject/list").GetComponent<CUIListScript>();
			cUIFormScript.transform.FindChild("GameObject/info/txt").GetComponent<Text>().text = UT.FRData().IntimRela_EmptyDataText;
			this.Refresh();
		}

		public void Clear()
		{
			this.listScript = null;
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.IntimacyRela_Menu_Close, new CUIEventManager.OnUIEventHandler(this.On_IntimacyRela_Menu_Close));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.IntimacyRela_Show_Drop_List, new CUIEventManager.OnUIEventHandler(this.On_IntimacyRela_Show_Drop_List));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.IntimacyRela_Drop_ListElement_Click, new CUIEventManager.OnUIEventHandler(this.On_IntimacyRela_Drop_ListElement_Click));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.IntimacyRela_Drop_ListElement_Enable, new CUIEventManager.OnUIEventHandler(this.On_Drop_ListElement_Enable));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.IntimacyRela_OK, new CUIEventManager.OnUIEventHandler(this.On_IntimacyRela_OK));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.IntimacyRela_Cancle, new CUIEventManager.OnUIEventHandler(this.On_IntimacyRela_Cancle));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.IntimacyRela_Item_Enable, new CUIEventManager.OnUIEventHandler(this.On_IntimacyRelation_Item_Enable));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.IntimacyRela_FristChoise, new CUIEventManager.OnUIEventHandler(this.On_IntimacyRela_FristChoise));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.IntimacyRela_PrevState, new CUIEventManager.OnUIEventHandler(this.On_IntimacyRela_PrevState));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler("FRDataChange", new Action(this.On_FRDataChange));
			CFriendModel model = Singleton<CFriendContoller>.GetInstance().model;
			ListView<CFR> list = model.FRData.GetList();
			for (int i = 0; i < list.get_Count(); i++)
			{
				CFR cFR = list.get_Item(i);
				cFR.choiseRelation = -1;
				cFR.bInShowChoiseRelaList = false;
				cFR.bRedDot = false;
			}
			if (Singleton<CFriendContoller>.get_instance().view != null)
			{
				Singleton<CFriendContoller>.get_instance().view.Refresh();
			}
			Singleton<EventRouter>.get_instance().BroadCastEvent("Friend_LobbyIconRedDot_Refresh");
		}

		private void On_FRDataChange()
		{
			this.Refresh();
		}

		public void Refresh()
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CFriendContoller.IntimacyRelaFormPath);
			if (form == null)
			{
				return;
			}
			bool flag = Singleton<CFriendContoller>.GetInstance().model.FRData.GetList().get_Count() == 0;
			if (flag)
			{
				form.transform.FindChild("GameObject/info").gameObject.CustomSetActive(true);
				form.transform.FindChild("GameObject/list").gameObject.CustomSetActive(false);
			}
			else
			{
				form.transform.FindChild("GameObject/info").gameObject.CustomSetActive(false);
				form.transform.FindChild("GameObject/list").gameObject.CustomSetActive(true);
				this.Refresh_IntimacyRelation_List();
			}
		}

		public void Refresh_IntimacyRelation_List()
		{
			CFriendModel model = Singleton<CFriendContoller>.GetInstance().model;
			ListView<CFR> list = model.FRData.GetList();
			this.listScript.SetElementAmount(list.get_Count());
			for (int i = 0; i < list.get_Count(); i++)
			{
				CUIListElementScript elemenet = this.listScript.GetElemenet(i);
				this.ShowIntimacyRelation_Item(elemenet, list.get_Item(i));
			}
		}

		public void On_IntimacyRelation_Item_Enable(CUIEvent uievent)
		{
			if (uievent == null)
			{
				return;
			}
			int srcWidgetIndexInBelongedList = uievent.m_srcWidgetIndexInBelongedList;
			CFriendModel model = Singleton<CFriendContoller>.GetInstance().model;
			ListView<CFR> list = model.FRData.GetList();
			if (list == null)
			{
				return;
			}
			CFR cFR = null;
			if (srcWidgetIndexInBelongedList >= 0 && srcWidgetIndexInBelongedList < list.get_Count())
			{
				cFR = list.get_Item(srcWidgetIndexInBelongedList);
			}
			if (cFR != null && uievent.m_srcWidget != null)
			{
				CUIComponent component = uievent.m_srcWidget.GetComponent<CUIComponent>();
				if (component != null)
				{
					this.ShowIntimacyRelation_Item(component, cFR);
				}
			}
		}

		public void ShowIntimacyRelation_Item(CUIComponent com, CFR frData)
		{
			if (com == null || frData == null)
			{
				return;
			}
			IntimacyRelationViewUT.Show_Item(com, frData);
		}

		private void On_IntimacyRela_Menu_Close(CUIEvent uievent)
		{
			this.Clear();
		}

		private void On_IntimacyRela_Cancle(CUIEvent uievent)
		{
			ulong commonUInt64Param = uievent.m_eventParams.commonUInt64Param1;
			uint tagUInt = uievent.m_eventParams.tagUInt;
			COM_INTIMACY_STATE tag = uievent.m_eventParams.tag;
			int tag2 = uievent.m_eventParams.tag2;
			Singleton<CFriendContoller>.get_instance().model.FRData.ResetChoiseRelaState(commonUInt64Param, tagUInt);
			if (tag == 20)
			{
				FriendRelationNetCore.Send_CHG_INTIMACY_DENY(commonUInt64Param, tagUInt, 1, 1);
				CFriendRelationship.FRData.Add(commonUInt64Param, tagUInt, 24, 0, 0u, false);
			}
			if (tag == 22)
			{
				FriendRelationNetCore.Send_CHG_INTIMACY_DENY(commonUInt64Param, tagUInt, 2, 1);
				CFriendRelationship.FRData.Add(commonUInt64Param, tagUInt, 24, 0, 0u, false);
			}
			if (tag == 21)
			{
				FriendRelationNetCore.Send_CHG_INTIMACY_DENY(commonUInt64Param, tagUInt, 1, 2);
				CFriendRelationship.FRData.Add(commonUInt64Param, tagUInt, 1, 0, 0u, false);
			}
			if (tag == 23)
			{
				FriendRelationNetCore.Send_CHG_INTIMACY_DENY(commonUInt64Param, tagUInt, 2, 2);
				CFriendRelationship.FRData.Add(commonUInt64Param, tagUInt, 2, 0, 0u, false);
			}
		}

		private void On_IntimacyRela_OK(CUIEvent uievent)
		{
			ulong commonUInt64Param = uievent.m_eventParams.commonUInt64Param1;
			uint tagUInt = uievent.m_eventParams.tagUInt;
			COM_INTIMACY_STATE tag = uievent.m_eventParams.tag;
			int tag2 = uievent.m_eventParams.tag2;
			if (tag == 24 && tag2 != -1)
			{
				CFriendRelationship fRData = Singleton<CFriendContoller>.get_instance().model.FRData;
				CFriendRelationship.FRConfig cFGByIndex = fRData.GetCFGByIndex(tag2);
				if (cFGByIndex.state == 1)
				{
					if (fRData.FindFrist(1) == null)
					{
						FriendRelationNetCore.Send_INTIMACY_RELATION_REQUEST(commonUInt64Param, tagUInt, 1, 1);
						Singleton<CFriendContoller>.get_instance().model.FRData.ResetChoiseRelaState(commonUInt64Param, tagUInt);
					}
					else
					{
						Singleton<CUIManager>.get_instance().OpenTips(UT.FRData().IntimRela_Tips_AlreadyHasGay, true, 1.5f, null, new object[0]);
					}
				}
				if (cFGByIndex.state == 2)
				{
					if (fRData.FindFrist(2) == null)
					{
						FriendRelationNetCore.Send_INTIMACY_RELATION_REQUEST(commonUInt64Param, tagUInt, 2, 1);
						Singleton<CFriendContoller>.get_instance().model.FRData.ResetChoiseRelaState(commonUInt64Param, tagUInt);
					}
					else
					{
						Singleton<CUIManager>.get_instance().OpenTips(UT.FRData().IntimRela_Tips_AlreadyHasLover, true, 1.5f, null, new object[0]);
					}
				}
			}
			else if (tag == 20)
			{
				FriendRelationNetCore.Send_CHG_INTIMACY_CONFIRM(commonUInt64Param, tagUInt, 1, 1);
			}
			else if (tag == 22)
			{
				FriendRelationNetCore.Send_CHG_INTIMACY_CONFIRM(commonUInt64Param, tagUInt, 2, 1);
			}
			else if (tag == 21)
			{
				FriendRelationNetCore.Send_CHG_INTIMACY_CONFIRM(commonUInt64Param, tagUInt, 1, 2);
			}
			else if (tag == 23)
			{
				FriendRelationNetCore.Send_CHG_INTIMACY_CONFIRM(commonUInt64Param, tagUInt, 2, 2);
			}
		}

		private void On_IntimacyRela_FristChoise(CUIEvent uievent)
		{
			ulong commonUInt64Param = uievent.m_eventParams.commonUInt64Param1;
			uint tagUInt = uievent.m_eventParams.tagUInt;
			CFR cfr = Singleton<CFriendContoller>.get_instance().model.FRData.GetCfr(commonUInt64Param, tagUInt);
			if (cfr != null)
			{
				FriendRelationNetCore.Send_CHG_INTIMACYPRIORITY(cfr.state);
			}
		}

		private void On_IntimacyRela_PrevState(CUIEvent uievent)
		{
			ulong commonUInt64Param = uievent.m_eventParams.commonUInt64Param1;
			uint tagUInt = uievent.m_eventParams.tagUInt;
			CFR cfr = Singleton<CFriendContoller>.get_instance().model.FRData.GetCfr(commonUInt64Param, tagUInt);
			if (cfr.state == 20 && !cfr.bReciveOthersRequest)
			{
				cfr.state = 24;
			}
			if (cfr.state == 22 && !cfr.bReciveOthersRequest)
			{
				cfr.state = 24;
			}
			if (cfr.state == 23 && !cfr.bReciveOthersRequest)
			{
				cfr.state = 2;
			}
			if (cfr.state == 21 && !cfr.bReciveOthersRequest)
			{
				cfr.state = 1;
			}
			Singleton<EventRouter>.GetInstance().BroadCastEvent("FRDataChange");
		}

		private void On_IntimacyRela_Delete_Relation(CUIEvent uievent)
		{
			ulong commonUInt64Param = uievent.m_eventParams.commonUInt64Param1;
			uint tagUInt = uievent.m_eventParams.tagUInt;
			CFR cfr = Singleton<CFriendContoller>.get_instance().model.FRData.GetCfr(commonUInt64Param, tagUInt);
			if (cfr != null)
			{
				if (cfr.state == 1)
				{
					FriendRelationNetCore.Send_INTIMACY_RELATION_REQUEST(commonUInt64Param, tagUInt, 1, 2);
				}
				if (cfr.state == 2)
				{
					FriendRelationNetCore.Send_INTIMACY_RELATION_REQUEST(commonUInt64Param, tagUInt, 2, 2);
				}
			}
		}

		private void On_IntimacyRela_Show_Drop_List(CUIEvent uievent)
		{
			ulong commonUInt64Param = uievent.m_eventParams.commonUInt64Param1;
			uint tagUInt = uievent.m_eventParams.tagUInt;
			CFR cfr = Singleton<CFriendContoller>.get_instance().model.FRData.GetCfr(commonUInt64Param, tagUInt);
			if (cfr != null)
			{
				cfr.bInShowChoiseRelaList = !cfr.bInShowChoiseRelaList;
				CUIComponent component = uievent.m_srcWidgetScript.m_widgets[0].GetComponent<CUIComponent>();
				CUIListScript component2 = uievent.m_srcWidgetScript.m_widgets[1].GetComponent<CUIListScript>();
				if (component2 != null)
				{
					component2.SetElementAmount(2);
				}
				IntimacyRelationViewUT.Show_Item(component, cfr);
			}
		}

		public void On_Drop_ListElement_Enable(CUIEvent uievent)
		{
			int srcWidgetIndexInBelongedList = uievent.m_srcWidgetIndexInBelongedList;
			CUIEventScript component = uievent.m_srcWidgetScript.GetComponent<CUIEventScript>();
			CUIComponent component2 = component.m_widgets[0].GetComponent<CUIComponent>();
			CUIEventScript component3 = component2.m_widgets[7].GetComponent<CUIEventScript>();
			ulong commonUInt64Param = component3.m_onClickEventParams.commonUInt64Param1;
			uint tagUInt = component3.m_onClickEventParams.tagUInt;
			if (Singleton<CFriendContoller>.get_instance().model.FRData.GetCfr(commonUInt64Param, tagUInt) == null)
			{
				return;
			}
			Text component4 = uievent.m_srcWidget.transform.Find("Text").GetComponent<Text>();
			if (component4 != null)
			{
				CFriendRelationship.FRConfig cFGByIndex = Singleton<CFriendContoller>.get_instance().model.FRData.GetCFGByIndex(srcWidgetIndexInBelongedList);
				if (cFGByIndex != null)
				{
					component4.text = cFGByIndex.cfgRelaStr;
				}
			}
		}

		private void On_IntimacyRela_Drop_ListElement_Click(CUIEvent uievent)
		{
			CUIComponent component = uievent.m_srcWidgetScript.m_widgets[0].GetComponent<CUIComponent>();
			CUIEventScript component2 = component.m_widgets[7].GetComponent<CUIEventScript>();
			ulong commonUInt64Param = component2.m_onClickEventParams.commonUInt64Param1;
			uint tagUInt = component2.m_onClickEventParams.tagUInt;
			CFR cfr = Singleton<CFriendContoller>.get_instance().model.FRData.GetCfr(commonUInt64Param, tagUInt);
			if (cfr == null)
			{
				return;
			}
			cfr.bInShowChoiseRelaList = false;
			cfr.choiseRelation = uievent.m_srcWidgetIndexInBelongedList;
			IntimacyRelationViewUT.Show_Item(component, cfr);
		}
	}
}
