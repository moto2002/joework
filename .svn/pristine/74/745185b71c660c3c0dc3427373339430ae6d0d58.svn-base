using Assets.Scripts.UI;
using System;
using UnityEngine;

namespace Assets.Scripts.GameSystem
{
	public class CUINewFlagSystem : Singleton<CUINewFlagSystem>
	{
		private static string NewFlagSetStr = "1";

		public bool IsHaveNewFlagKey(enNewFlagKey newFlagKey)
		{
			return PlayerPrefs.HasKey(newFlagKey.ToString());
		}

		public void SetAllNewFlagKey()
		{
			for (int i = 0; i < 34; i++)
			{
				PlayerPrefs.SetString(((enNewFlagKey)i).ToString(), CUINewFlagSystem.NewFlagSetStr);
			}
		}

		public void AddNewFlag(GameObject obj, enNewFlagKey flagKey, enNewFlagPos newFlagPos = enNewFlagPos.enTopRight, float scale = 1f, float offsetX = 0f, float offsetY = 0f, enNewFlagType newFlagType = enNewFlagType.enNewFlag)
		{
			if (obj == null)
			{
				return;
			}
			Transform x = obj.transform.Find("redDotNew");
			string key = flagKey.ToString();
			if (x != null)
			{
				if (flagKey > enNewFlagKey.New_None && flagKey < enNewFlagKey.New_Count && !PlayerPrefs.HasKey(key))
				{
					this.DelNewFlag(obj, flagKey, true);
					return;
				}
			}
			else if (flagKey > enNewFlagKey.New_None && flagKey < enNewFlagKey.New_Count && !PlayerPrefs.HasKey(key))
			{
				string text = string.Empty;
				if (newFlagType == enNewFlagType.enNewFlag)
				{
					text = "redDotNew";
				}
				else
				{
					text = "redDot";
				}
				GameObject gameObject = Object.Instantiate(CUIUtility.GetSpritePrefeb("UGUI/Form/Common/" + text, false, false)) as GameObject;
				if (gameObject == null)
				{
					return;
				}
				gameObject.transform.SetParent(obj.transform, false);
				gameObject.transform.SetAsLastSibling();
				RectTransform rectTransform = gameObject.transform as RectTransform;
				Vector2 anchorMin = default(Vector2);
				Vector2 anchorMax = default(Vector2);
				Vector2 pivot = default(Vector2);
				switch (newFlagPos)
				{
				case enNewFlagPos.enTopLeft:
					anchorMin.x = 0f;
					anchorMin.y = 1f;
					anchorMax.x = 0f;
					anchorMax.y = 1f;
					pivot.x = 0f;
					pivot.y = 1f;
					break;
				case enNewFlagPos.enTopCenter:
					anchorMin.x = 0.5f;
					anchorMin.y = 1f;
					anchorMax.x = 0.5f;
					anchorMax.y = 1f;
					pivot.x = 0.5f;
					pivot.y = 1f;
					break;
				case enNewFlagPos.enTopRight:
					anchorMin.x = 1f;
					anchorMin.y = 1f;
					anchorMax.x = 1f;
					anchorMax.y = 1f;
					pivot.x = 1f;
					pivot.y = 1f;
					break;
				case enNewFlagPos.enMiddleLeft:
					anchorMin.x = 0f;
					anchorMin.y = 0.5f;
					anchorMax.x = 0f;
					anchorMax.y = 0.5f;
					pivot.x = 0f;
					pivot.y = 0.5f;
					break;
				case enNewFlagPos.enMiddleCenter:
					anchorMin.x = 0.5f;
					anchorMin.y = 0.5f;
					anchorMax.x = 0.5f;
					anchorMax.y = 0.5f;
					pivot.x = 0.5f;
					pivot.y = 0.5f;
					break;
				case enNewFlagPos.enMiddleRight:
					anchorMin.x = 1f;
					anchorMin.y = 0.5f;
					anchorMax.x = 1f;
					anchorMax.y = 0.5f;
					pivot.x = 1f;
					pivot.y = 0.5f;
					break;
				case enNewFlagPos.enBottomLeft:
					anchorMin.x = 0f;
					anchorMin.y = 0f;
					anchorMax.x = 0f;
					anchorMax.y = 0f;
					pivot.x = 0f;
					pivot.y = 0f;
					break;
				case enNewFlagPos.enBottomCenter:
					anchorMin.x = 0.5f;
					anchorMin.y = 0f;
					anchorMax.x = 0.5f;
					anchorMax.y = 0f;
					pivot.x = 0.5f;
					pivot.y = 0f;
					break;
				case enNewFlagPos.enBottomRight:
					anchorMin.x = 1f;
					anchorMin.y = 0f;
					anchorMax.x = 1f;
					anchorMax.y = 0f;
					pivot.x = 1f;
					pivot.y = 0f;
					break;
				}
				rectTransform.pivot = pivot;
				rectTransform.anchorMin = anchorMin;
				rectTransform.anchorMax = anchorMax;
				if (scale != 1f)
				{
					rectTransform.localScale = new Vector3(scale, scale, scale);
				}
				rectTransform.anchoredPosition = new Vector2(offsetX, offsetY);
			}
		}

		public void DelNewFlag(GameObject obj, enNewFlagKey flagKey, bool immediately = true)
		{
			if (obj)
			{
				Transform transform = obj.transform.Find("redDotNew(Clone)");
				if (transform == null)
				{
					transform = obj.transform.Find("redDot(Clone)");
				}
				if (transform)
				{
					string key = flagKey.ToString();
					if (!PlayerPrefs.HasKey(key))
					{
						PlayerPrefs.SetString(key, CUINewFlagSystem.NewFlagSetStr);
						PlayerPrefs.Save();
						if (immediately)
						{
							Object.Destroy(transform.gameObject);
						}
					}
				}
			}
		}

		public void ShowNewFlagForBeizhanEntry()
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CLobbySystem.LOBBY_FORM_PATH);
			if (form == null)
			{
				return;
			}
			GameObject gameObject = form.transform.Find("LobbyBottom/SysEntry/ChatBtn").gameObject;
			if (gameObject != null)
			{
				this.AddNewFlag(gameObject, enNewFlagKey.New_BeizhanEntryBtn_V3, enNewFlagPos.enTopRight, 1f, 0f, 0f, enNewFlagType.enNewFlag);
			}
		}

		public void HideNewFlagForBeizhanEntry()
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CLobbySystem.LOBBY_FORM_PATH);
			if (form == null)
			{
				return;
			}
			GameObject gameObject = form.transform.Find("LobbyBottom/SysEntry/ChatBtn").gameObject;
			if (gameObject != null)
			{
				this.DelNewFlag(gameObject, enNewFlagKey.New_BeizhanEntryBtn_V3, true);
			}
		}

		public void SetNewFlagFormCustomEquipShow(bool bShow)
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CLobbySystem.LOBBY_FORM_PATH);
			if (form == null)
			{
				return;
			}
			GameObject gameObject = form.transform.Find("LobbyBottom/SysEntry/ChatBtn_sub/Menu/PropBtn").gameObject;
			if (gameObject != null)
			{
				if (bShow)
				{
					this.AddNewFlag(gameObject, enNewFlagKey.New_CustomEqiup_V1, enNewFlagPos.enTopRight, 1f, 0f, 0f, enNewFlagType.enNewFlag);
				}
				else
				{
					this.DelNewFlag(gameObject, enNewFlagKey.New_CustomEqiup_V1, true);
				}
			}
		}

		public void ShowNewFlagForMishuEntry()
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CLobbySystem.LOBBY_FORM_PATH);
			if (form == null)
			{
				return;
			}
			GameObject gameObject = form.transform.Find("LobbyBottom/Newbie").gameObject;
			if (gameObject != null)
			{
				this.AddNewFlag(gameObject, enNewFlagKey.New_MishuEntryBtn_V1, enNewFlagPos.enBottomRight, 1f, 0f, 0f, enNewFlagType.enNewFlag);
			}
		}

		public void HideNewFlagForMishuEntry()
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CLobbySystem.LOBBY_FORM_PATH);
			if (form == null)
			{
				return;
			}
			GameObject gameObject = form.transform.Find("LobbyBottom/Newbie").gameObject;
			if (gameObject != null)
			{
				this.DelNewFlag(gameObject, enNewFlagKey.New_MishuEntryBtn_V1, true);
			}
		}

		public void ShowNewFlagForAchievementEntry()
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CLobbySystem.LOBBY_FORM_PATH);
			if (form == null)
			{
				return;
			}
			GameObject gameObject = form.transform.Find("LobbyBottom/SysEntry/AchievementBtn").gameObject;
			if (gameObject != null)
			{
				this.AddNewFlag(gameObject, enNewFlagKey.New_Achievement_V1, enNewFlagPos.enTopLeft, 1f, 0f, 0f, enNewFlagType.enNewFlag);
			}
		}

		public void HideNewFlagForAchievementEntry()
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CLobbySystem.LOBBY_FORM_PATH);
			if (form == null)
			{
				return;
			}
			GameObject gameObject = form.transform.Find("LobbyBottom/SysEntry/AchievementBtn").gameObject;
			if (gameObject != null)
			{
				this.DelNewFlag(gameObject, enNewFlagKey.New_Achievement_V1, true);
			}
		}

		public void SetNewFlagForSettingEntry(bool bShow)
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CLobbySystem.SYSENTRY_FORM_PATH);
			if (form == null)
			{
				return;
			}
			GameObject gameObject = form.transform.Find("PlayerBtn/SettingBtn").gameObject;
			if (gameObject != null)
			{
				if (bShow)
				{
					this.AddNewFlag(gameObject, enNewFlagKey.New_LobbySettingEntry_V2, enNewFlagPos.enTopRight, 0.8f, 0f, -2f, enNewFlagType.enNewFlag);
				}
				else
				{
					this.DelNewFlag(gameObject, enNewFlagKey.New_LobbySettingEntry_V2, true);
				}
			}
		}

		public void SetNewFlagForMessageBtnEntry(bool bShow)
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CLobbySystem.LOBBY_FORM_PATH);
			if (form == null)
			{
				return;
			}
			GameObject gameObject = form.transform.Find("LobbyBottom/SysEntry/ChatBtn_sub/Menu/MessageBtn").gameObject;
			if (gameObject != null)
			{
				if (bShow)
				{
					this.AddNewFlag(gameObject, enNewFlagKey.New_MessageEntry_V1, enNewFlagPos.enTopRight, 1f, 0f, 0f, enNewFlagType.enNewFlag);
				}
				else
				{
					this.DelNewFlag(gameObject, enNewFlagKey.New_MessageEntry_V1, true);
				}
			}
		}

		public void SetNewFlagForOBBtn(bool bShow)
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CLobbySystem.LOBBY_FORM_PATH);
			if (form == null)
			{
				return;
			}
			GameObject gameObject = form.transform.Find("Popup/OBBtn").gameObject;
			if (gameObject != null)
			{
				if (bShow)
				{
					this.AddNewFlag(gameObject, enNewFlagKey.New_OBBtn_V1, enNewFlagPos.enTopLeft, 0.8f, 0f, 0f, enNewFlagType.enNewFlag);
				}
				else
				{
					this.DelNewFlag(gameObject, enNewFlagKey.New_OBBtn_V1, true);
				}
			}
		}

		public void SetNewFlagForMatch(bool bShow)
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CLobbySystem.LOBBY_FORM_PATH);
			if (form == null)
			{
				return;
			}
			GameObject gameObject = form.transform.Find("Popup/BattleWebHome").gameObject;
			if (gameObject != null)
			{
				if (bShow)
				{
					this.AddNewFlag(gameObject, enNewFlagKey.New_Match_V1, enNewFlagPos.enTopLeft, 0.8f, 0f, 0f, enNewFlagType.enNewFlag);
				}
				else
				{
					this.DelNewFlag(gameObject, enNewFlagKey.New_Match_V1, true);
				}
			}
		}

		public void SetNewFlagForArenaRankBtn(bool bShow)
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(RankingSystem.s_rankingForm);
			if (form == null)
			{
				return;
			}
			GameObject gameObject = form.transform.Find("bg/AllRankType/AllRankSelectMenu/ListElement8").gameObject;
			if (gameObject != null)
			{
				if (bShow)
				{
					this.AddNewFlag(gameObject, enNewFlagKey.New_ArenaRank_V1, enNewFlagPos.enTopRight, 1f, 0f, 0f, enNewFlagType.enNewFlag);
				}
				else
				{
					this.DelNewFlag(gameObject, enNewFlagKey.New_ArenaRank_V1, true);
				}
			}
		}

		public void SetNewFlagForGodRankBtn(bool bShow)
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(RankingSystem.s_rankingForm);
			if (form == null)
			{
				return;
			}
			GameObject gameObject = form.transform.Find("bg/AllRankType/AllRankSelectMenu/ListElement7").gameObject;
			if (gameObject != null)
			{
				if (bShow)
				{
					this.AddNewFlag(gameObject, enNewFlagKey.New_GodRank_V1, enNewFlagPos.enTopRight, 1f, 0f, 0f, enNewFlagType.enNewFlag);
				}
				else
				{
					this.DelNewFlag(gameObject, enNewFlagKey.New_GodRank_V1, true);
				}
			}
		}

		public void SetNewFlagForFriendEntry(bool bShow)
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CLobbySystem.SYSENTRY_FORM_PATH);
			if (form == null)
			{
				return;
			}
			GameObject gameObject = form.transform.Find("PlayerBtn/FriendBtn").gameObject;
			if (gameObject != null)
			{
				if (bShow)
				{
					this.AddNewFlag(gameObject, enNewFlagKey.New_Lobby_Friend_V1, enNewFlagPos.enTopLeft, 0.8f, 0f, -2f, enNewFlagType.enNewFlag);
				}
				else
				{
					this.DelNewFlag(gameObject, enNewFlagKey.New_Lobby_Friend_V1, true);
				}
			}
		}
	}
}
