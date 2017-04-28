using Assets.Scripts.UI;
using CSProtocol;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	public class IntimacyRelationViewUT
	{
		public static void Show_Item(CUIComponent com, CFR frData)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo();
			if (masterRoleInfo != null && masterRoleInfo.playerUllUID == frData.ulluid && (long)masterRoleInfo.logicWorldID == (long)((ulong)frData.worldID))
			{
				return;
			}
			IntimacyRelationViewUT.Show_Item_Top(com, frData);
			IntimacyRelationViewUT.Show_Item_Middle(com, frData);
			IntimacyRelationViewUT.Show_Item_Bottom(com, frData);
			frData.bRedDot = false;
		}

		public static void Show_Item_Top(CUIComponent com, CFR frData)
		{
			IntimacyRelationViewUT.SetButtonParam(com.m_widgets[19], frData);
			if (frData.state == 1 || frData.state == 2)
			{
				com.m_widgets[19].CustomSetActive(true);
			}
			else
			{
				com.m_widgets[19].CustomSetActive(false);
			}
			COMDT_FRIEND_INFO friendInfo = frData.friendInfo;
			if (friendInfo == null)
			{
				return;
			}
			CUIHttpImageScript component = com.m_widgets[1].GetComponent<CUIHttpImageScript>();
			UT.SetHttpImage(component, friendInfo.szHeadUrl);
			GameObject gameObject = com.m_widgets[2];
			if (gameObject)
			{
				MonoSingleton<NobeSys>.GetInstance().SetNobeIcon(gameObject.GetComponent<Image>(), (int)friendInfo.stGameVip.dwCurLevel, false);
			}
			Text component2 = com.m_widgets[3].GetComponent<Text>();
			string text = UT.Bytes2String(friendInfo.szUserName);
			if (component2 != null)
			{
				component2.text = text;
			}
			GameObject genderImage = com.m_widgets[4];
			FriendShower.ShowGender(genderImage, friendInfo.bGender);
		}

		public static void Show_Item_Middle(CUIComponent com, CFR frData)
		{
			IntimacyRelationViewUT.SetButtonParam(com.m_widgets[16], frData);
			IntimacyRelationViewUT.SetButtonParam(com.m_widgets[14], frData);
			IntimacyRelationViewUT.SetButtonParam(com.m_widgets[12], frData);
			IntimacyRelationViewUT.SetButtonParam(com.m_widgets[7], frData);
			int cDDays = frData.CDDays;
			if (cDDays != -1)
			{
				com.m_widgets[5].CustomSetActive(true);
				com.m_widgets[6].CustomSetActive(false);
				IntimacyRelationViewUT.Set_Middle_Text(com, true, string.Format(UT.FRData().IntimRela_CD_CountDown, cDDays));
				return;
			}
			if (frData.state == 24 && cDDays == -1)
			{
				com.m_widgets[5].CustomSetActive(true);
				com.m_widgets[6].CustomSetActive(true);
				com.m_widgets[10].CustomSetActive(false);
				IntimacyRelationViewUT.Set_Drop_Text(com, !frData.bInShowChoiseRelaList, frData);
				IntimacyRelationViewUT.Set_Drop_List(com, frData.bInShowChoiseRelaList);
				IntimacyRelationViewUT.Set_Middle_Text(com, false, string.Empty);
			}
			if (frData.state == 1 || frData.state == 2)
			{
				com.m_widgets[5].CustomSetActive(true);
				com.m_widgets[6].CustomSetActive(false);
				IntimacyRelationViewUT.Set_Middle_Text(com, true, IntimacyRelationViewUT.GetMiddleTextStr(frData.state));
			}
			if (frData.state == 20 || frData.state == 22)
			{
				if (frData.bReciveOthersRequest)
				{
					com.m_widgets[5].CustomSetActive(true);
					com.m_widgets[6].CustomSetActive(false);
					string text = string.Empty;
					if (frData.state == 20)
					{
						text = UT.FRData().IntimRela_Type_Gay;
					}
					if (frData.state == 22)
					{
						text = UT.FRData().IntimRela_Type_Lover;
					}
					string txt = string.Format(UT.FRData().IntimRela_Tips_ReceiveOtherReqRela, UT.Bytes2String(frData.friendInfo.szUserName), text);
					IntimacyRelationViewUT.Set_Middle_Text(com, true, txt);
				}
				else
				{
					com.m_widgets[5].CustomSetActive(true);
					com.m_widgets[6].CustomSetActive(false);
					IntimacyRelationViewUT.Set_Middle_Text(com, true, UT.FRData().IntimRela_Tips_Wait4TargetRspReqRela);
				}
			}
			if (frData.state == 21 || frData.state == 23)
			{
				if (frData.bReciveOthersRequest)
				{
					com.m_widgets[5].CustomSetActive(true);
					com.m_widgets[6].CustomSetActive(false);
					string text2 = string.Empty;
					if (frData.state == 21)
					{
						text2 = UT.FRData().IntimRela_Type_Gay;
					}
					if (frData.state == 23)
					{
						text2 = UT.FRData().IntimRela_Type_Lover;
					}
					IntimacyRelationViewUT.Set_Middle_Text(com, true, string.Format(UT.FRData().IntimRela_Tips_ReceiveOtherDelRela, UT.Bytes2String(frData.friendInfo.szUserName), text2));
				}
				else
				{
					com.m_widgets[5].CustomSetActive(true);
					com.m_widgets[6].CustomSetActive(false);
					IntimacyRelationViewUT.Set_Middle_Text(com, true, UT.FRData().IntimRela_Tips_Wait4TargetRspDelRela);
				}
			}
		}

		public static void Set_Drop_Text(CUIComponent com, bool bShow, CFR frData)
		{
			com.m_widgets[9].CustomSetActive(bShow);
			string text = string.Empty;
			CFriendRelationship.FRConfig cFGByIndex = Singleton<CFriendContoller>.get_instance().model.FRData.GetCFGByIndex(frData.choiseRelation);
			if (cFGByIndex != null)
			{
				text = cFGByIndex.cfgRelaStr;
			}
			else
			{
				text = UT.FRData().IntimRela_Tips_SelectRelation;
			}
			com.m_widgets[9].GetComponent<Text>().text = text;
		}

		public static void Set_Drop_List(CUIComponent com, bool bShow)
		{
			com.m_widgets[8].CustomSetActive(bShow);
		}

		public static void Set_Middle_Text(CUIComponent com, bool bShow, string txt = "")
		{
			com.m_widgets[10].CustomSetActive(bShow);
			com.m_widgets[10].GetComponent<Text>().text = txt;
		}

		public static string GetMiddleTextStr(COM_INTIMACY_STATE state)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			string text = string.Empty;
			if (state == 1)
			{
				text = UT.FRData().IntimRela_Type_Gay;
				return string.Format(UT.FRData().IntimRela_Tips_MidText, masterRoleInfo.Name, text);
			}
			if (state == 2)
			{
				text = UT.FRData().IntimRela_Type_Lover;
				return string.Format(UT.FRData().IntimRela_Tips_MidText, masterRoleInfo.Name, text);
			}
			return string.Empty;
		}

		public static void Show_Item_Bottom(CUIComponent com, CFR frData)
		{
			IntimacyRelationViewUT.SetButtonParam(com.m_widgets[12], frData);
			IntimacyRelationViewUT.SetButtonParam(com.m_widgets[14], frData);
			IntimacyRelationViewUT.SetButtonParam(com.m_widgets[16], frData);
			IntimacyRelationViewUT.SetButtonParam(com.m_widgets[20], frData);
			IntimacyRelationViewUT.SetButtonParam(com.m_widgets[22], frData);
			int cDDays = frData.CDDays;
			if (cDDays != -1)
			{
				com.m_widgets[11].CustomSetActive(true);
				IntimacyRelationViewUT.Set_2_Button(com, false, false, string.Empty, string.Empty);
				IntimacyRelationViewUT.Set_1_Button(com, false, string.Empty);
				IntimacyRelationViewUT.Set_ReSelect_Button(com, false, string.Empty);
				IntimacyRelationViewUT.Set_DoSelect_Button(com, false, string.Empty);
				IntimacyRelationViewUT.Set_Bottom_Text(com, true, UT.FRData().IntimRela_Tips_RelaHasDel);
				return;
			}
			if (frData.state == 24 && cDDays == -1)
			{
				com.m_widgets[11].CustomSetActive(true);
				string intimRela_Tips_OK = UT.FRData().IntimRela_Tips_OK;
				string intimRela_Tips_Cancle = UT.FRData().IntimRela_Tips_Cancle;
				IntimacyRelationViewUT.Set_DoSelect_Button(com, frData.choiseRelation != -1, intimRela_Tips_OK);
				IntimacyRelationViewUT.Set_2_Button(com, false, false, intimRela_Tips_Cancle, intimRela_Tips_OK);
				IntimacyRelationViewUT.Set_1_Button(com, false, string.Empty);
				IntimacyRelationViewUT.Set_ReSelect_Button(com, false, string.Empty);
				IntimacyRelationViewUT.Set_Bottom_Text(com, false, string.Empty);
			}
			if (frData.state == 1 || frData.state == 2)
			{
				com.m_widgets[11].CustomSetActive(true);
				IntimacyRelationViewUT.Set_2_Button(com, false, false, string.Empty, string.Empty);
				IntimacyRelationViewUT.Set_1_Button(com, false, string.Empty);
				IntimacyRelationViewUT.Set_ReSelect_Button(com, false, string.Empty);
				IntimacyRelationViewUT.Set_DoSelect_Button(com, false, string.Empty);
				COM_INTIMACY_STATE firstChoiseState = Singleton<CFriendContoller>.get_instance().model.FRData.GetFirstChoiseState();
				if (firstChoiseState == frData.state)
				{
					IntimacyRelationViewUT.Set_Bottom_Text(com, true, UT.FRData().IntimRela_AleadyFristChoise);
				}
				else
				{
					IntimacyRelationViewUT.Set_Bottom_Text(com, false, string.Empty);
					IntimacyRelationViewUT.Set_1_Button(com, true, UT.FRData().IntimRela_DoFristChoise);
				}
			}
			if (frData.state == 20 || frData.state == 22)
			{
				if (frData.bReciveOthersRequest)
				{
					com.m_widgets[11].CustomSetActive(true);
					IntimacyRelationViewUT.Set_2_Button(com, true, true, UT.FRData().IntimRela_Tips_Cancle, UT.FRData().IntimRela_Tips_OK);
					IntimacyRelationViewUT.Set_1_Button(com, false, string.Empty);
					IntimacyRelationViewUT.Set_Bottom_Text(com, false, string.Empty);
					IntimacyRelationViewUT.Set_ReSelect_Button(com, false, string.Empty);
					IntimacyRelationViewUT.Set_DoSelect_Button(com, false, string.Empty);
				}
				else
				{
					com.m_widgets[11].CustomSetActive(true);
					IntimacyRelationViewUT.Set_2_Button(com, false, false, string.Empty, string.Empty);
					IntimacyRelationViewUT.Set_1_Button(com, false, string.Empty);
					IntimacyRelationViewUT.Set_Bottom_Text(com, false, UT.FRData().IntimRela_Tips_Wait4TargetRspReqRela);
					IntimacyRelationViewUT.Set_ReSelect_Button(com, true, UT.FRData().IntimRela_ReselectRelation);
					IntimacyRelationViewUT.Set_DoSelect_Button(com, false, string.Empty);
				}
			}
			if (frData.state == 21 || frData.state == 23)
			{
				if (frData.bReciveOthersRequest)
				{
					com.m_widgets[11].CustomSetActive(true);
					IntimacyRelationViewUT.Set_2_Button(com, true, true, UT.FRData().IntimRela_Tips_Cancle, UT.FRData().IntimRela_Tips_OK);
					IntimacyRelationViewUT.Set_1_Button(com, false, string.Empty);
					IntimacyRelationViewUT.Set_Bottom_Text(com, false, string.Empty);
					IntimacyRelationViewUT.Set_ReSelect_Button(com, false, string.Empty);
					IntimacyRelationViewUT.Set_DoSelect_Button(com, false, string.Empty);
				}
				else
				{
					com.m_widgets[11].CustomSetActive(true);
					IntimacyRelationViewUT.Set_2_Button(com, false, false, string.Empty, string.Empty);
					IntimacyRelationViewUT.Set_1_Button(com, false, string.Empty);
					IntimacyRelationViewUT.Set_Bottom_Text(com, false, UT.FRData().IntimRela_Tips_Wait4TargetRspDelRela);
					IntimacyRelationViewUT.Set_ReSelect_Button(com, true, UT.FRData().IntimRela_ReDelRelation);
					IntimacyRelationViewUT.Set_DoSelect_Button(com, false, string.Empty);
				}
			}
		}

		public static void Set_2_Button(CUIComponent com, bool bLeftShow, bool bRightShow, string left = "", string right = "")
		{
			com.m_widgets[16].CustomSetActive(bLeftShow);
			com.m_widgets[14].CustomSetActive(bRightShow);
			com.m_widgets[17].GetComponent<Text>().text = left;
			com.m_widgets[15].GetComponent<Text>().text = right;
		}

		public static void Set_1_Button(CUIComponent com, bool bShow, string txt = "")
		{
			com.m_widgets[12].CustomSetActive(bShow);
			com.m_widgets[13].GetComponent<Text>().text = txt;
		}

		public static void Set_ReSelect_Button(CUIComponent com, bool bShow, string txt = "")
		{
			com.m_widgets[20].CustomSetActive(bShow);
			com.m_widgets[21].GetComponent<Text>().text = txt;
		}

		public static void Set_DoSelect_Button(CUIComponent com, bool bShow, string txt = "")
		{
			com.m_widgets[22].CustomSetActive(bShow);
			com.m_widgets[23].GetComponent<Text>().text = txt;
		}

		public static void Set_Bottom_Text(CUIComponent com, bool bShow, string txt = "")
		{
			com.m_widgets[18].CustomSetActive(bShow);
			com.m_widgets[18].GetComponent<Text>().text = txt;
		}

		private static void SetButtonParam(GameObject obj, CFR frData)
		{
			CUIEventScript component = obj.GetComponent<CUIEventScript>();
			if (component != null)
			{
				component.m_onClickEventParams.commonUInt64Param1 = frData.ulluid;
				component.m_onClickEventParams.tagUInt = frData.worldID;
				component.m_onClickEventParams.tag = frData.state;
				component.m_onClickEventParams.tag2 = frData.choiseRelation;
			}
		}

		private static void Process_Bottm_Btns(bool bShow, GameObject node, ulong ullUid, uint dwLogicWorldId)
		{
			GameObject gameObject = node.transform.FindChild("Button_Send").gameObject;
			GameObject gameObject2 = node.transform.FindChild("Button_Cancel").gameObject;
			IntimacyRelationViewUT.SetEvtParam(gameObject, ullUid, dwLogicWorldId);
			IntimacyRelationViewUT.SetEvtParam(gameObject2, ullUid, dwLogicWorldId);
			gameObject.CustomSetActive(bShow);
			gameObject.CustomSetActive(bShow);
		}

		private static void SetEvtParam(GameObject obj, ulong ullUid, uint dwLogicWorldId)
		{
			CUIEventScript component = obj.GetComponent<CUIEventScript>();
			component.m_onClickEventParams.commonUInt64Param2 = ullUid;
			component.m_onClickEventParams.taskId = dwLogicWorldId;
		}
	}
}
