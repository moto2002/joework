using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.Sound;
using Assets.Scripts.UI;
using CSProtocol;
using ResData;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	public class CHeroSelectNormalSystem : Singleton<CHeroSelectNormalSystem>
	{
		private const int c_maxSelectedHeroIDsBeforeGC = 3;

		private const float c_countDownCheckTime = 6.1f;

		private const int c_countDownTotalTime = 59;

		public static string s_heroSelectFormPath = "UGUI/Form/System/HeroSelect/Form_HeroSelectNormal.prefab";

		public static string s_symbolPropPanelPath = "Other/Panel_SymbolProp";

		public static string s_defaultHeroListName = "ListHostHeroInfo";

		public uint m_showHeroID;

		public uint m_nowShowHeroID;

		public string m_heroGameObjName = string.Empty;

		private List<uint> m_selectedHeroModelIDs = new List<uint>();

		private enHeroJobType m_heroSelectJobType;

		private ListView<IHeroData> m_canUseHeroListByJob = new ListView<IHeroData>();

		public override void Init()
		{
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_ReqCloseForm, new CUIEventManager.OnUIEventHandler(this.HeroSelect_ClickCloseBtn));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_FormClose, new CUIEventManager.OnUIEventHandler(this.HeroSelect_OnClose));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_SelectHero, new CUIEventManager.OnUIEventHandler(this.HeroSelect_SelectHero));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_LeftHeroItemEnable, new CUIEventManager.OnUIEventHandler(this.LeftHeroItemEnable));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_SelectTeamHero, new CUIEventManager.OnUIEventHandler(this.HeroSelect_SelectTeamHero));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_ConfirmHeroSelect, new CUIEventManager.OnUIEventHandler(this.HeroSelect_ConfirmHeroSelect));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_Del_Hero, new CUIEventManager.OnUIEventHandler(this.HeroSelect_Del_Hero));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_Skill_Down, new CUIEventManager.OnUIEventHandler(this.HeroSelect_Skill_Down));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_Skill_Up, new CUIEventManager.OnUIEventHandler(this.HeroSelect_Skill_Up));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_OpenFullHeroList, new CUIEventManager.OnUIEventHandler(this.HeroSelect_OpenFullHeroList));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_CloseFullHeroList, new CUIEventManager.OnUIEventHandler(this.HeroSelect_CloseFullHeroList));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_SymbolPageSelect, new CUIEventManager.OnUIEventHandler(this.OnHeroSymbolPageSelect));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_PageDownBtnClick, new CUIEventManager.OnUIEventHandler(this.OnSymbolPageDownBtnClick));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_ViewProp_Down, new CUIEventManager.OnUIEventHandler(this.OnOpenSymbolProp));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_ViewProp_Up, new CUIEventManager.OnUIEventHandler(this.OnCloseSymbolProp));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_MenuSelect, new CUIEventManager.OnUIEventHandler(this.HeroSelect_OnMenuSelect));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_RefreshSkinPanel, new CUIEventManager.OnUIEventHandler(this.RefreshSkinPanel));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_SkinSelect, new CUIEventManager.OnUIEventHandler(this.HeroSelect_OnSkinSelect));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_HeroJobMenuSelect, new CUIEventManager.OnUIEventHandler(this.OnHeroJobMenuSelect));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_AddedSkillSelected, new CUIEventManager.OnUIEventHandler(this.OnSelectedAddedSkill));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_AddedSkillOpenForm, new CUIEventManager.OnUIEventHandler(this.OnOpenAddedSkillPanel));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_AddedSkillCloseForm, new CUIEventManager.OnUIEventHandler(this.OnCloseAddedSkillPanel));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_AddedSkillConfirm, new CUIEventManager.OnUIEventHandler(this.OnConfirmAddedSkill));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_RandomHero, new CUIEventManager.OnUIEventHandler(this.OnReqRandHero));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroCount_Buy, new CUIEventManager.OnUIEventHandler(this.BuyHeroCount));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroCount_CancelBuy, new CUIEventManager.OnUIEventHandler(this.OnCancelBuy));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_OnTimerCountDown, new CUIEventManager.OnUIEventHandler(this.OnTimerCountDown));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_UseHeroExpCard, new CUIEventManager.OnUIEventHandler(this.OnUseHeroExpCard));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_UseHeroExpCardChanel, new CUIEventManager.OnUIEventHandler(this.OnUseHeroExpCardChanel));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_FullHeroTipsComplete, new CUIEventManager.OnUIEventHandler(this.OnFullHeroTipsComplete));
		}

		public void OpenForm()
		{
			OutlineFilter.EnableSurfaceShaderOutline(true);
			CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(CHeroSelectNormalSystem.s_heroSelectFormPath, false, true);
			if (cUIFormScript == null)
			{
				return;
			}
			Singleton<CUIManager>.GetInstance().LoadUIScenePrefab(CUIUtility.s_heroSelectBgPath, cUIFormScript);
			Transform targetTrans = cUIFormScript.transform.Find("CountDownMovie");
			CUICommonSystem.SetObjActive(targetTrans, false);
			CUITimerScript component = cUIFormScript.transform.Find("CountDown/Timer").gameObject.GetComponent<CUITimerScript>();
			Button component2 = cUIFormScript.transform.Find("btnClose").gameObject.GetComponent<Button>();
			component2.gameObject.CustomSetActive(false);
			component.gameObject.CustomSetActive(false);
			if (Singleton<CHeroSelectBaseSystem>.get_instance().IsMultilMode() || Singleton<CHeroSelectBaseSystem>.get_instance().IsSingleWarmBattle())
			{
				if (!component.gameObject.activeInHierarchy)
				{
					this.StartEndTimer(59);
				}
			}
			else
			{
				component2.gameObject.CustomSetActive(true);
			}
			if (cUIFormScript != null)
			{
				DynamicShadow.EnableDynamicShow(cUIFormScript.gameObject, true);
			}
			string[] titleList = new string[]
			{
				Singleton<CTextManager>.get_instance().GetText("Choose_Hero"),
				Singleton<CTextManager>.get_instance().GetText("Choose_Skin")
			};
			GameObject gameObject = cUIFormScript.gameObject.transform.Find("TabList").gameObject;
			int selectIndex = 0;
			this.InitHeroJobMenu(cUIFormScript);
			if (Singleton<CHeroSelectBaseSystem>.get_instance().selectType == enSelectType.enRandom)
			{
				selectIndex = 1;
				if (Singleton<CHeroSelectBaseSystem>.get_instance().gameType == enSelectGameType.enPVE_Computer)
				{
					this.RadomHeroBySelfWithSingleMode();
					this.RefreshHeroPanel(true, true);
				}
			}
			CUICommonSystem.InitMenuPanel(gameObject, titleList, selectIndex, true);
			this.RefreshLeftRandCountText();
			this.InitAddedSkillPanel(cUIFormScript);
			if (Singleton<CHeroSelectBaseSystem>.get_instance().IsSingleWarmBattle())
			{
				CFakePvPHelper.BeginFakeSelectHero();
			}
			Singleton<CReplayKitSys>.GetInstance().InitReplayKit(cUIFormScript.transform.Find("Other/ReplayKit"), true, true);
			MonoSingleton<NewbieGuideManager>.GetInstance().CheckTriggerTime(NewbieGuideTriggerTimeType.enterBattleHeroSel, new uint[0]);
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo.PvpLevel >= 5u)
			{
				CUICommonSystem.SetObjActive(cUIFormScript.transform.FindChild("PanelLeft/ListHostHeroInfo/btnOpenFullHeroPanel/Panel_Guide").gameObject, !masterRoleInfo.IsClientBitsSet(5));
			}
		}

		public void CloseForm()
		{
			Singleton<CUIManager>.GetInstance().CloseForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
		}

		private void HeroSelect_OnClose(CUIEvent uiEvent)
		{
			this.m_selectedHeroModelIDs.Clear();
			this.m_showHeroID = 0u;
			this.m_nowShowHeroID = 0u;
			this.m_heroSelectJobType = enHeroJobType.All;
			this.m_canUseHeroListByJob.Clear();
			if (uiEvent.m_srcWidget != null)
			{
				DynamicShadow.EnableDynamicShow(uiEvent.m_srcWidget, false);
			}
			OutlineFilter.EnableSurfaceShaderOutline(false);
			Singleton<CHeroSelectBaseSystem>.get_instance().Clear();
		}

		private void HeroSelect_ClickCloseBtn(CUIEvent uiEvent)
		{
			if (Singleton<CHeroSelectBaseSystem>.get_instance().gameType == enSelectGameType.enPVE_Computer)
			{
				CHeroSelectBaseSystem.SendQuitSingleGameReq();
			}
			else
			{
				this.CloseForm();
			}
		}

		public void HeroSelect_SelectHero(IHeroData selectHeroData)
		{
			if (Singleton<CHeroSelectBaseSystem>.get_instance().selectType == enSelectType.enRandom)
			{
				return;
			}
			if (selectHeroData == null || Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo() == null || Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Count() == 0)
			{
				return;
			}
			if (selectHeroData.cfgID > 0u)
			{
				if (this.m_selectedHeroModelIDs.get_Count() >= 3)
				{
					Singleton<CResourceManager>.GetInstance().UnloadAssetBundlesByTag("CharShow");
					Singleton<CResourceManager>.GetInstance().UnloadAssetBundlesByTag("CharSkillIcon");
					Singleton<CResourceManager>.GetInstance().UnloadAssetBundlesByTag("ActorInfo");
					Singleton<CResourceManager>.GetInstance().UnloadAssetBundlesByTag("CharLoading");
					Singleton<CResourceManager>.GetInstance().UnloadUnusedAssets();
					this.m_selectedHeroModelIDs.Clear();
				}
				if (!this.m_selectedHeroModelIDs.Contains(selectHeroData.cfgID))
				{
					this.m_selectedHeroModelIDs.Add(selectHeroData.cfgID);
				}
			}
			if (Singleton<CHeroSelectBaseSystem>.get_instance().IsMultilMode())
			{
				CHeroSelectBaseSystem.SendHeroSelectMsg(0, 0, selectHeroData.cfgID);
			}
			else if (Singleton<CHeroSelectBaseSystem>.get_instance().gameType == enSelectGameType.enPVE_Computer)
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
				masterMemberInfo.ChoiceHero[0].stBaseInfo.stCommonInfo.dwHeroID = selectHeroData.cfgID;
				this.m_showHeroID = selectHeroData.cfgID;
				Singleton<CHeroSelectBaseSystem>.get_instance().SetPvpHeroSelect(this.m_showHeroID);
			}
			else if (Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Count() == 1)
			{
				Singleton<CHeroSelectBaseSystem>.get_instance().SetPvpHeroSelect(selectHeroData.cfgID);
				this.m_showHeroID = selectHeroData.cfgID;
			}
			else if ((int)Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroCount < Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Count())
			{
				Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.set_Item((int)Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroCount, selectHeroData.cfgID);
				CHeroSelectBaseSystem expr_1DE = Singleton<CHeroSelectBaseSystem>.get_instance();
				expr_1DE.m_selectHeroCount += 1;
				this.m_showHeroID = selectHeroData.cfgID;
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips("hero is select over", false, 1.5f, null, new object[0]);
			}
			this.RefreshHeroPanel(true, true);
			if (Singleton<CHeroSelectBaseSystem>.get_instance().IsMobaMode() || Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Count() == 1)
			{
				this.HeroSelect_CloseFullHeroList(null);
			}
			this.HeroSelect_Skill_Up(null);
			if (CFakePvPHelper.bInFakeSelect)
			{
				CFakePvPHelper.OnSelfSelectHero(Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.selfInfo.ullUid, selectHeroData.cfgID);
			}
			MonoSingleton<NewbieGuideManager>.GetInstance().CheckTriggerTime(NewbieGuideTriggerTimeType.heroSelectedForBattle, new uint[]
			{
				(uint)Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroCount
			});
		}

		private void HeroSelect_SelectHero(CUIEvent uiEvent)
		{
			Singleton<CSoundManager>.GetInstance().PostEvent("UI_BanPick_Swicth", null);
			CUIListScript srcWidgetBelongedListScript = uiEvent.m_srcWidgetBelongedListScript;
			int srcWidgetIndexInBelongedList = uiEvent.m_srcWidgetIndexInBelongedList;
			if (srcWidgetBelongedListScript == null)
			{
				return;
			}
			int count = Singleton<CHeroSelectBaseSystem>.get_instance().m_canUseHeroList.get_Count();
			if (srcWidgetBelongedListScript.gameObject.name != CHeroSelectNormalSystem.s_defaultHeroListName)
			{
				count = this.m_canUseHeroListByJob.get_Count();
			}
			if (srcWidgetIndexInBelongedList < 0 || srcWidgetIndexInBelongedList >= count)
			{
				return;
			}
			IHeroData heroData;
			if (srcWidgetBelongedListScript.gameObject.name != CHeroSelectNormalSystem.s_defaultHeroListName)
			{
				heroData = this.m_canUseHeroListByJob.get_Item(srcWidgetIndexInBelongedList);
			}
			else
			{
				heroData = Singleton<CHeroSelectBaseSystem>.get_instance().m_canUseHeroList.get_Item(srcWidgetIndexInBelongedList);
			}
			if (CHeroDataFactory.IsHeroCanUse(heroData.cfgID) || Singleton<CHeroSelectBaseSystem>.get_instance().IsSpecTraingMode())
			{
				this.HeroSelect_SelectHero(heroData);
			}
			else
			{
				string text = Singleton<CTextManager>.get_instance().GetText("ExpCard_Use", new string[]
				{
					heroData.heroName
				});
				stUIEventParams par = default(stUIEventParams);
				par.tagUInt = heroData.cfgID;
				Singleton<CUIManager>.get_instance().OpenMessageBoxWithCancel(text, enUIEventID.HeroSelect_UseHeroExpCard, enUIEventID.HeroSelect_UseHeroExpCardChanel, par, false);
			}
		}

		private void OnUseHeroExpCard(CUIEvent uiEvent)
		{
			CBagSystem.UseHeroExpCard(uiEvent.m_eventParams.tagUInt);
		}

		private void OnUseHeroExpCardChanel(CUIEvent uiEvent)
		{
			this.RefreshHeroPanel(false, true);
		}

		private void HeroSelect_SelectTeamHero(CUIEvent uiEvent)
		{
			if (!Singleton<CHeroSelectBaseSystem>.get_instance().IsMobaMode())
			{
				int srcWidgetIndexInBelongedList = uiEvent.m_srcWidgetIndexInBelongedList;
				if (srcWidgetIndexInBelongedList >= Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Count() || srcWidgetIndexInBelongedList < 0)
				{
					return;
				}
				this.m_showHeroID = Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Item(srcWidgetIndexInBelongedList);
				this.RefreshHeroPanel(false, true);
				this.RefreshSkinPanel(null);
			}
		}

		private void HeroSelect_Del_Hero(CUIEvent uiEvent)
		{
			int srcWidgetIndexInBelongedList = uiEvent.m_srcWidgetIndexInBelongedList;
			if (srcWidgetIndexInBelongedList >= Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Count() || srcWidgetIndexInBelongedList < 0)
			{
				return;
			}
			if (Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Item(srcWidgetIndexInBelongedList) != 0u)
			{
				Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.RemoveAt(srcWidgetIndexInBelongedList);
				Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.Add(0u);
				CHeroSelectBaseSystem expr_5E = Singleton<CHeroSelectBaseSystem>.get_instance();
				expr_5E.m_selectHeroCount -= 1;
			}
			this.m_showHeroID = Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Item(0);
			this.RefreshHeroPanel(false, true);
			this.RefreshSkinPanel(null);
		}

		public void SwitchSkinMenuSelect()
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			CUIListScript component = form.gameObject.transform.Find("TabList").GetComponent<CUIListScript>();
			component.m_alwaysDispatchSelectedChangeEvent = true;
			component.SelectElement(1, true);
			component.m_alwaysDispatchSelectedChangeEvent = false;
		}

		private void HeroSelect_ConfirmHeroSelect(CUIEvent uiEvent)
		{
			if (Singleton<CHeroSelectBaseSystem>.get_instance().IsMultilMode())
			{
				CHeroSelectBaseSystem.SendMuliPrepareToBattleMsg();
			}
			else if (Singleton<CHeroSelectBaseSystem>.get_instance().gameType == enSelectGameType.enArenaDefTeamConfig)
			{
				stUIEventParams par = default(stUIEventParams);
				par.heroIdList = new List<uint>();
				for (int i = 0; i < (int)Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroCount; i++)
				{
					par.heroIdList.Add(Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Item(i));
				}
				Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Arena_ReciveDefTeamInfo, par);
				this.CloseForm();
			}
			else
			{
				Singleton<CHeroSelectBaseSystem>.get_instance().m_isSelectConfirm = true;
				if (Singleton<CHeroSelectBaseSystem>.get_instance().IsSingleWarmBattle())
				{
					CFakePvPHelper.OnSelfHeroConfirmed();
					this.SwitchSkinMenuSelect();
				}
				else
				{
					CHeroSelectBaseSystem.SendSinglePrepareToBattleMsg(Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo, Singleton<CHeroSelectBaseSystem>.get_instance().m_battleListID, Singleton<CHeroSelectBaseSystem>.get_instance().gameType, Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroCount, Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList);
				}
				this.RefreshHeroPanel(false, true);
			}
			if (Singleton<CHeroSelectBaseSystem>.get_instance().selectType == enSelectType.enRandom)
			{
				CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
				if (form != null)
				{
					Transform transform = form.transform.FindChild("Other/RandomHero");
					if (transform != null)
					{
						CUICommonSystem.SetButtonEnableWithShader(transform.GetComponent<Button>(), false, true);
					}
				}
			}
		}

		private void SetShadeBtnState(CUIFormScript form, bool show)
		{
			if (form != null)
			{
				GameObject widget = form.GetWidget(5);
				if (widget != null)
				{
					widget.CustomSetActive(show);
				}
			}
		}

		private void HeroSelect_Skill_Down(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.gameObject.transform.Find("Other/PanelSkillInfo");
			if (transform == null)
			{
				return;
			}
			this.SetShadeBtnState(form, false);
			GameObject widget = form.GetWidget(4);
			CUICommonSystem.RefreshSkillLevelUpProperty(widget, ref uiEvent.m_eventParams.skillPropertyDesc, uiEvent.m_eventParams.skillSlotId);
			Text component = transform.Find("lblName").gameObject.GetComponent<Text>();
			Text component2 = transform.Find("lblDesc").gameObject.GetComponent<Text>();
			Text component3 = transform.Find("SkillCDText").gameObject.GetComponent<Text>();
			Text component4 = transform.Find("SkillEnergyCostText").gameObject.GetComponent<Text>();
			component.text = uiEvent.m_eventParams.skillTipParam.strTipTitle;
			component2.text = uiEvent.m_eventParams.skillTipParam.strTipText;
			component3.text = uiEvent.m_eventParams.skillTipParam.skillCoolDown;
			component4.text = uiEvent.m_eventParams.skillTipParam.skillEnergyCost;
			ushort[] skillEffect = uiEvent.m_eventParams.skillTipParam.skillEffect;
			if (skillEffect == null)
			{
				return;
			}
			for (int i = 1; i <= 2; i++)
			{
				GameObject gameObject = transform.transform.Find(string.Format("EffectNode{0}", i)).gameObject;
				if (i <= skillEffect.Length && skillEffect[i - 1] != 0)
				{
					gameObject.CustomSetActive(true);
					gameObject.GetComponent<Image>().SetSprite(CSkillData.GetEffectSlotBg(skillEffect[i - 1]), uiEvent.m_srcFormScript, true, false, false, false);
					gameObject.transform.Find("Text").GetComponent<Text>().text = CSkillData.GetEffectDesc(skillEffect[i - 1]);
				}
				else
				{
					gameObject.CustomSetActive(false);
				}
			}
			transform.gameObject.CustomSetActive(true);
		}

		private void HeroSelect_Skill_Up(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
			if (form != null && form.gameObject != null)
			{
				Transform transform = form.gameObject.transform.Find("Other/PanelSkillInfo");
				GameObject gameObject = (!(transform != null)) ? null : transform.gameObject;
				if (gameObject != null)
				{
					gameObject.gameObject.CustomSetActive(false);
				}
				this.SetShadeBtnState(form, true);
			}
		}

		private void HeroSelect_OpenFullHeroList(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.gameObject.transform.Find("PanelLeft/ListHostHeroInfo");
			Transform transform2 = form.gameObject.transform.Find("PanelLeft/ListHostHeroInfoFull");
			Transform transform3 = form.gameObject.transform.Find("PanelLeft/MenuList");
			if (transform != null && transform2 != null)
			{
				transform.gameObject.CustomSetActive(false);
				transform2.gameObject.CustomSetActive(true);
				transform3.gameObject.CustomSetActive(true);
			}
			if (Singleton<CHeroSelectBaseSystem>.get_instance().IsMultilMode() || Singleton<CHeroSelectBaseSystem>.get_instance().IsSingleWarmBattle())
			{
				Singleton<CChatController>.get_instance().Hide_SelectChat_MidNode();
				Singleton<CChatController>.get_instance().Set_Show_Bottom(false);
				Singleton<CChatController>.get_instance().SetEntryNodeVoiceBtnShowable(false);
			}
			this.CompleteHeroTips(uiEvent.m_srcWidget.transform.FindChild("Panel_Guide").gameObject);
		}

		private void HeroSelect_CloseFullHeroList(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.gameObject.transform.Find("PanelLeft/ListHostHeroInfo");
			Transform transform2 = form.gameObject.transform.Find("PanelLeft/ListHostHeroInfoFull");
			Transform transform3 = form.gameObject.transform.Find("PanelLeft/MenuList");
			if (transform != null && transform2 != null)
			{
				transform.gameObject.CustomSetActive(true);
				transform2.gameObject.CustomSetActive(false);
				transform3.gameObject.CustomSetActive(false);
			}
			if (Singleton<CHeroSelectBaseSystem>.get_instance().IsMultilMode() || Singleton<CHeroSelectBaseSystem>.get_instance().IsSingleWarmBattle())
			{
				Singleton<CChatController>.get_instance().Set_Show_Bottom(true);
				Singleton<CChatController>.get_instance().SetEntryNodeVoiceBtnShowable(true);
			}
		}

		private void OnFullHeroTipsComplete(CUIEvent uiEvent)
		{
			this.CompleteHeroTips(uiEvent.m_srcWidget.transform.parent.gameObject);
		}

		private void CompleteHeroTips(GameObject tipsPanel)
		{
			if (tipsPanel != null)
			{
				tipsPanel.CustomSetActive(false);
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				masterRoleInfo.SetClientBits(5, true, true);
			}
		}

		private void HeroSelect_OnMenuSelect(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			int selectedIndex = uiEvent.m_srcWidget.GetComponent<CUIListScript>().GetSelectedIndex();
			if (Singleton<CHeroSelectBaseSystem>.get_instance().selectType == enSelectType.enRandom && selectedIndex == 0)
			{
				CUIListScript component = form.gameObject.transform.Find("TabList").GetComponent<CUIListScript>();
				component.SelectElement(1, true);
				Singleton<CUIManager>.get_instance().OpenTips(Singleton<CTextManager>.get_instance().GetText("Luandou_heroabandon_Tips_1"), false, 1.5f, null, new object[0]);
				return;
			}
			Transform transform = form.gameObject.transform.Find("PanelLeft");
			Transform transform2 = form.gameObject.transform.Find("PanelLeftSkin");
			if (selectedIndex == 0)
			{
				transform.gameObject.CustomSetActive(true);
				transform2.gameObject.CustomSetActive(false);
				this.HeroSelect_CloseFullHeroList(null);
				this.RefreshHeroPanel(false, true);
			}
			else
			{
				transform.gameObject.CustomSetActive(false);
				transform2.gameObject.CustomSetActive(true);
				this.RefreshSkinPanel(null);
			}
		}

		public void RefreshSkinPanel(CUIEvent uiEvent = null)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			uint showHeroID = this.m_showHeroID;
			ListView<ResHeroSkin> listView = new ListView<ResHeroSkin>();
			ListView<ResHeroSkin> listView2 = new ListView<ResHeroSkin>();
			int num = -1;
			if (showHeroID != 0u)
			{
				ListView<ResHeroSkin> availableSkinByHeroId = CSkinInfo.GetAvailableSkinByHeroId(showHeroID);
				for (int i = 0; i < availableSkinByHeroId.get_Count(); i++)
				{
					ResHeroSkin resHeroSkin = availableSkinByHeroId.get_Item(i);
					if (masterRoleInfo.IsCanUseSkin(showHeroID, resHeroSkin.dwSkinID) || CBagSystem.CanUseSkinExpCard(resHeroSkin.dwID))
					{
						listView.Add(resHeroSkin);
					}
					else
					{
						listView2.Add(resHeroSkin);
					}
					if (masterRoleInfo.GetHeroWearSkinId(showHeroID) == resHeroSkin.dwSkinID)
					{
						num = listView.get_Count() - 1;
					}
				}
				listView.AddRange(listView2);
			}
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.gameObject.transform.Find("PanelLeftSkin/ListHostSkinInfo");
			Transform transform2 = form.gameObject.transform.Find("PanelLeftSkin/ListHostSkinInfo/panelEffect");
			if (transform == null)
			{
				return;
			}
			CUIListScript[] array = new CUIListScript[]
			{
				transform.GetComponent<CUIListScript>()
			};
			for (int j = 0; j < array.Length; j++)
			{
				CUIListScript cUIListScript = array[j];
				cUIListScript.SetElementAmount(listView.get_Count());
				for (int k = 0; k < listView.get_Count(); k++)
				{
					CUIListElementScript elemenet = cUIListScript.GetElemenet(k);
					Transform transform3 = cUIListScript.GetElemenet(k).transform;
					Image component = transform3.Find("imageIcon").GetComponent<Image>();
					Image component2 = transform3.Find("imageIconGray").GetComponent<Image>();
					Text component3 = transform3.Find("lblName").GetComponent<Text>();
					GameObject gameObject = transform3.Find("imgExperienceMark").gameObject;
					Transform transform4 = transform3.Find("expCardPanel");
					ResHeroSkin resHeroSkin = listView.get_Item(k);
					bool flag = masterRoleInfo.IsValidExperienceSkin(showHeroID, resHeroSkin.dwSkinID);
					gameObject.CustomSetActive(flag);
					bool flag2 = !masterRoleInfo.IsCanUseSkin(showHeroID, resHeroSkin.dwSkinID) && CBagSystem.CanUseSkinExpCard(resHeroSkin.dwID);
					RectTransform rectTransform = (RectTransform)component3.transform;
					RectTransform rectTransform2 = (RectTransform)transform4;
					if (flag2)
					{
						transform4.gameObject.CustomSetActive(true);
						rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform2.anchoredPosition.y + rectTransform.rect.height);
					}
					else
					{
						transform4.gameObject.CustomSetActive(false);
						rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform2.anchoredPosition.y);
					}
					if (masterRoleInfo.IsCanUseSkin(showHeroID, resHeroSkin.dwSkinID) || CBagSystem.CanUseSkinExpCard(resHeroSkin.dwID))
					{
						component.gameObject.CustomSetActive(true);
						component2.gameObject.CustomSetActive(false);
						elemenet.enabled = true;
					}
					else
					{
						component.gameObject.CustomSetActive(false);
						component2.gameObject.CustomSetActive(true);
						elemenet.enabled = false;
					}
					GameObject spritePrefeb = CUIUtility.GetSpritePrefeb(CUIUtility.s_Sprite_Dynamic_Icon_Dir + StringHelper.UTF8BytesToString(ref resHeroSkin.szSkinPicID), true, true);
					component.SetSprite(spritePrefeb, false);
					component2.SetSprite(spritePrefeb, false);
					component3.text = StringHelper.UTF8BytesToString(ref resHeroSkin.szSkinName);
					CUIEventScript component4 = transform3.GetComponent<CUIEventScript>();
					component4.SetUIEvent(enUIEventType.Click, enUIEventID.HeroSelect_SkinSelect, new stUIEventParams
					{
						tagUInt = resHeroSkin.dwSkinID,
						commonBool = flag
					});
					if (k == num)
					{
						this.InitSkinEffect(cUIListScript.transform.Find("panelEffect/List").gameObject, showHeroID, resHeroSkin.dwSkinID);
					}
				}
				cUIListScript.SelectElement(num, true);
			}
			if (num == -1)
			{
				transform2.gameObject.CustomSetActive(false);
			}
			else
			{
				transform2.gameObject.CustomSetActive(true);
			}
		}

		private void HeroSelect_OnSkinSelect(CUIEvent uiEvent)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			uint showHeroID = this.m_showHeroID;
			uint tagUInt = uiEvent.m_eventParams.tagUInt;
			bool commonBool = uiEvent.m_eventParams.commonBool;
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.gameObject.transform.Find("PanelLeftSkin/ListHostSkinInfo");
			Transform transform2 = form.gameObject.transform.Find("PanelLeftSkin/ListHostSkinInfo/panelEffect/List");
			if (transform == null || transform2 == null)
			{
				return;
			}
			CUIListScript component = transform.GetComponent<CUIListScript>();
			if (masterRoleInfo.IsCanUseSkin(showHeroID, tagUInt))
			{
				this.InitSkinEffect(transform2.gameObject, showHeroID, tagUInt);
			}
			else
			{
				component.SelectElement(component.GetLastSelectedIndex(), true);
			}
			if (masterRoleInfo.IsCanUseSkin(showHeroID, tagUInt))
			{
				if (masterRoleInfo.GetHeroWearSkinId(showHeroID) != tagUInt)
				{
					CHeroInfoSystem2.ReqWearHeroSkin(showHeroID, tagUInt, true);
				}
			}
			else
			{
				CHeroSkinBuyManager.OpenBuyHeroSkinForm3D(showHeroID, tagUInt, false);
			}
		}

		private void InitSkinEffect(GameObject objList, uint heroID, uint skinID)
		{
			CSkinInfo.GetHeroSkinProp(heroID, skinID, ref CHeroSelectBaseSystem.s_propArr, ref CHeroSelectBaseSystem.s_propPctArr, ref CHeroSelectBaseSystem.s_propImgArr);
			CUICommonSystem.SetListProp(objList, ref CHeroSelectBaseSystem.s_propArr, ref CHeroSelectBaseSystem.s_propPctArr);
		}

		public void OnHeroSkinWearSuc(uint heroId, uint skinId)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			this.m_nowShowHeroID = 0u;
			this.RefreshHeroPanel(false, true);
			this.RefreshSkinPanel(null);
		}

		private void OnOpenSymbolProp(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			CHeroInfo cHeroInfo;
			if (masterRoleInfo.GetHeroInfo(this.m_showHeroID, out cHeroInfo, true))
			{
				this.OpenSymbolPropPanel(form, cHeroInfo.m_selectPageIndex);
			}
			else if (masterRoleInfo.IsFreeHero(this.m_showHeroID))
			{
				int freeHeroSymbolId = (int)masterRoleInfo.GetFreeHeroSymbolId(this.m_showHeroID);
				this.OpenSymbolPropPanel(form, freeHeroSymbolId);
			}
		}

		private void OpenSymbolPropPanel(CUIFormScript form, int pageIndex)
		{
			GameObject gameObject = form.transform.Find(CHeroSelectNormalSystem.s_symbolPropPanelPath).gameObject;
			GameObject gameObject2 = gameObject.gameObject.transform.Find("basePropPanel").gameObject;
			GameObject gameObject3 = gameObject.gameObject.transform.Find("enhancePropPanel").gameObject;
			GameObject gameObject4 = gameObject2.transform.Find("List").gameObject;
			CSymbolSystem.RefreshSymbolPageProp(gameObject4, pageIndex, true);
			gameObject3.CustomSetActive(!Singleton<CHeroSelectBaseSystem>.get_instance().IsMobaMode());
			if (!Singleton<CHeroSelectBaseSystem>.get_instance().IsMobaMode())
			{
				GameObject gameObject5 = gameObject3.transform.Find("List").gameObject;
				CSymbolSystem.RefreshSymbolPagePveEnhanceProp(gameObject5, pageIndex);
			}
			gameObject.gameObject.CustomSetActive(true);
		}

		private void OnCloseSymbolProp(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			GameObject gameObject = form.gameObject.transform.Find(CHeroSelectNormalSystem.s_symbolPropPanelPath).gameObject;
			gameObject.gameObject.CustomSetActive(false);
		}

		public void OnSymbolPageDownBtnClick(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.gameObject.transform.Find("Other/Panel_SymbolChange/DropList/List");
			transform.gameObject.CustomSetActive(!transform.gameObject.activeSelf);
		}

		public void OnHeroSymbolPageSelect(CUIEvent uiEvent)
		{
			int selectedIndex = uiEvent.m_srcWidget.GetComponent<CUIListScript>().GetSelectedIndex();
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			Transform transform = uiEvent.m_srcFormScript.gameObject.transform.Find("Other");
			Transform transform2 = transform.Find("Panel_SymbolChange/DropList/List");
			transform2.gameObject.CustomSetActive(false);
			CHeroInfo cHeroInfo;
			bool heroInfo = masterRoleInfo.GetHeroInfo(this.m_showHeroID, out cHeroInfo, true);
			if (heroInfo && selectedIndex != cHeroInfo.m_selectPageIndex)
			{
				CHeroSelectBaseSystem.SendHeroSelectSymbolPage(this.m_showHeroID, selectedIndex, false);
			}
			else if (!heroInfo && masterRoleInfo.IsFreeHero(this.m_showHeroID) && selectedIndex != (int)masterRoleInfo.GetFreeHeroSymbolId(this.m_showHeroID))
			{
				CHeroSelectBaseSystem.SendHeroSelectSymbolPage(this.m_showHeroID, selectedIndex, false);
			}
		}

		public void OnSymbolPageChange()
		{
			this.RefreshHeroInfo_DropList();
		}

		public void RefreshHeroInfo_DropList()
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.gameObject.transform.Find("Other/Panel_SymbolChange");
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (Singleton<CFunctionUnlockSys>.GetInstance().FucIsUnlock(8) && GameDataMgr.heroDatabin.GetDataByKey(this.m_showHeroID) != null)
			{
				int selectIndex = 0;
				CHeroInfo cHeroInfo;
				if (masterRoleInfo.GetHeroInfo(this.m_showHeroID, out cHeroInfo, true))
				{
					selectIndex = cHeroInfo.m_selectPageIndex;
				}
				else if (masterRoleInfo.IsFreeHero(this.m_showHeroID))
				{
					selectIndex = (int)masterRoleInfo.GetFreeHeroSymbolId(this.m_showHeroID);
				}
				transform.gameObject.CustomSetActive(true);
				CHeroSelectNormalSystem.SetPageDropListDataByHeroSelect(transform.gameObject, selectIndex);
			}
			else
			{
				transform.gameObject.CustomSetActive(false);
			}
		}

		public static void SetPageDropListDataByHeroSelect(GameObject panelObj, int selectIndex)
		{
			if (panelObj == null)
			{
				return;
			}
			Transform transform = panelObj.transform.Find("DropList/List");
			CUIListScript component = transform.GetComponent<CUIListScript>();
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			component.SetElementAmount(masterRoleInfo.m_symbolInfo.m_pageCount);
			for (int i = 0; i < masterRoleInfo.m_symbolInfo.m_pageCount; i++)
			{
				CUIListElementScript elemenet = component.GetElemenet(i);
				Text component2 = elemenet.gameObject.transform.Find("Text").GetComponent<Text>();
				component2.text = masterRoleInfo.m_symbolInfo.GetSymbolPageName(i);
				Text component3 = elemenet.gameObject.transform.Find("SymbolLevel/Text").GetComponent<Text>();
				component3.text = masterRoleInfo.m_symbolInfo.GetSymbolPageMaxLvl(i).ToString();
			}
			component.SelectElement(selectIndex, true);
			Text component4 = panelObj.transform.Find("DropList/Button_Down/Text").GetComponent<Text>();
			component4.text = masterRoleInfo.m_symbolInfo.GetSymbolPageName(selectIndex);
			Text component5 = panelObj.transform.Find("DropList/Button_Down/SymbolLevel/Text").GetComponent<Text>();
			component5.text = masterRoleInfo.m_symbolInfo.GetSymbolPageMaxLvl(selectIndex).ToString();
			Text component6 = panelObj.transform.Find("DropList/Button_Down/SymbolLevel/Text").GetComponent<Text>();
			component6.text = masterRoleInfo.m_symbolInfo.GetSymbolPageMaxLvl(selectIndex).ToString();
		}

		public void RefreshAddedSkillItem(CUIFormScript form, uint addedSkillID, bool bForceRefresh = false)
		{
			if (!CAddSkillSys.IsSelSkillAvailable())
			{
				return;
			}
			GameObject gameObject = form.transform.Find("Other/SkillList/AddedSkillItem").gameObject;
			CUIEventScript component = gameObject.GetComponent<CUIEventScript>();
			ResSkillCfgInfo dataByKey = GameDataMgr.skillDatabin.GetDataByKey(addedSkillID);
			if (dataByKey != null)
			{
				string prefabPath = string.Format("{0}{1}", CUIUtility.s_Sprite_Dynamic_Skill_Dir, Utility.UTF8Convert(dataByKey.szIconPath));
				Image component2 = gameObject.transform.Find("Icon").GetComponent<Image>();
				component2.SetSprite(prefabPath, form, true, false, false, false);
				gameObject.transform.Find("SkillNameTxt").GetComponent<Text>().text = Utility.UTF8Convert(dataByKey.szSkillName);
				string skillDescLobby = CUICommonSystem.GetSkillDescLobby(dataByKey.szSkillDesc, this.m_showHeroID);
				stUIEventParams eventParams = default(stUIEventParams);
				eventParams.skillTipParam = default(stSkillTipParams);
				eventParams.skillTipParam.strTipText = skillDescLobby;
				eventParams.skillTipParam.strTipTitle = StringHelper.UTF8BytesToString(ref dataByKey.szSkillName);
				eventParams.skillTipParam.skillCoolDown = Singleton<CTextManager>.get_instance().GetText("Skill_Cool_Down_Tips", new string[]
				{
					(dataByKey.iCoolDown / 1000).ToString()
				});
				eventParams.skillTipParam.skillEffect = dataByKey.SkillEffectType;
				eventParams.skillTipParam.skillEnergyCost = ((dataByKey.iEnergyCost != 0) ? Singleton<CTextManager>.get_instance().GetText(EnergyCommon.GetEnergyShowText((uint)dataByKey.bEnergyCostType, EnergyShowType.CostValue), new string[]
				{
					dataByKey.iEnergyCost.ToString()
				}) : string.Empty);
				component.SetUIEvent(enUIEventType.Down, enUIEventID.HeroSelect_Skill_Down, eventParams);
				if (bForceRefresh)
				{
					form.transform.Find("PanelAddSkill/AddSkillTitletxt").GetComponent<Text>().text = dataByKey.szSkillName;
					form.transform.Find("PanelAddSkill/AddSkilltxt").GetComponent<Text>().text = skillDescLobby;
					form.transform.Find("PanelAddSkill/btnConfirm").GetComponent<CUIEventScript>().m_onClickEventParams.tag = (int)addedSkillID;
					ListView<ResSkillUnlock> selSkillAvailable = CAddSkillSys.GetSelSkillAvailable(Singleton<CHeroSelectBaseSystem>.get_instance().m_mapUnUseSkill);
					for (int i = 0; i < selSkillAvailable.get_Count(); i++)
					{
						if (selSkillAvailable.get_Item(i).dwUnlockSkillID == addedSkillID)
						{
							CUIToggleListScript component3 = form.transform.Find("PanelAddSkill/ToggleList").GetComponent<CUIToggleListScript>();
							component3.SelectElement(i, true);
							break;
						}
					}
				}
			}
			else
			{
				DebugHelper.Assert(false, string.Format("ResSkillCfgInfo[{0}] can not be found!", addedSkillID));
			}
		}

		public void InitAddedSkillPanel(CUIFormScript form)
		{
			if (Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo() == null)
			{
				return;
			}
			if (CAddSkillSys.IsSelSkillAvailable())
			{
				CUIToggleListScript component = form.transform.Find("PanelAddSkill/ToggleList").GetComponent<CUIToggleListScript>();
				ListView<ResSkillUnlock> selSkillAvailable = CAddSkillSys.GetSelSkillAvailable(Singleton<CHeroSelectBaseSystem>.get_instance().m_mapUnUseSkill);
				component.SetElementAmount(selSkillAvailable.get_Count());
				int num = 0;
				ResSkillUnlock resSkillUnlock;
				for (int i = 0; i < selSkillAvailable.get_Count(); i++)
				{
					CUIListElementScript elemenet = component.GetElemenet(i);
					CUIEventScript component2 = elemenet.GetComponent<CUIEventScript>();
					resSkillUnlock = selSkillAvailable.get_Item(i);
					uint dwUnlockSkillID = resSkillUnlock.dwUnlockSkillID;
					ResSkillCfgInfo dataByKey = GameDataMgr.skillDatabin.GetDataByKey(dwUnlockSkillID);
					if (dataByKey != null)
					{
						component2.m_onClickEventID = enUIEventID.HeroSelect_AddedSkillSelected;
						component2.m_onClickEventParams.tag = (int)resSkillUnlock.dwUnlockSkillID;
						string prefabPath = string.Format("{0}{1}", CUIUtility.s_Sprite_Dynamic_Skill_Dir, Utility.UTF8Convert(dataByKey.szIconPath));
						Image component3 = elemenet.transform.Find("Icon").GetComponent<Image>();
						component3.SetSprite(prefabPath, form.GetComponent<CUIFormScript>(), true, false, false, false);
						elemenet.transform.Find("SkillNameTxt").GetComponent<Text>().text = Utility.UTF8Convert(dataByKey.szSkillName);
					}
					else
					{
						DebugHelper.Assert(false, string.Format("ResSkillCfgInfo[{0}] can not be found!", dwUnlockSkillID));
					}
				}
				component.SelectElement(num, true);
				resSkillUnlock = GameDataMgr.addedSkiilDatabin.GetDataByIndex(num);
				form.transform.Find("Other/SkillList/AddedSkillItem").gameObject.CustomSetActive(selSkillAvailable.get_Count() > 0);
			}
			else
			{
				form.transform.Find("Other/SkillList/AddedSkillItem").gameObject.CustomSetActive(false);
			}
			form.transform.Find("PanelAddSkill").gameObject.CustomSetActive(false);
		}

		public void OnSelectedAddedSkill(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			uint tag = (uint)uiEvent.m_eventParams.tag;
			form.transform.Find("PanelAddSkill/btnConfirm").GetComponent<CUIEventScript>().m_onClickEventParams.tag = (int)tag;
			ResSkillCfgInfo dataByKey = GameDataMgr.skillDatabin.GetDataByKey(tag);
			if (dataByKey == null)
			{
				return;
			}
			string skillDescLobby = CUICommonSystem.GetSkillDescLobby(dataByKey.szSkillDesc, this.m_showHeroID);
			form.transform.Find("PanelAddSkill/AddSkillTitletxt").GetComponent<Text>().text = dataByKey.szSkillName;
			form.transform.Find("PanelAddSkill/AddSkilltxt").GetComponent<Text>().text = skillDescLobby;
		}

		public void OnConfirmAddedSkill(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
			uint tag = (uint)uiEvent.m_eventParams.tag;
			if (form == null || Singleton<CHeroSelectBaseSystem>.get_instance().m_mapUnUseSkill == null || !CAddSkillSys.IsSelSkillAvailable(Singleton<CHeroSelectBaseSystem>.get_instance().m_mapUnUseSkill, tag))
			{
				DebugHelper.Assert(false, string.Format("CHeroSelectNormalSystem addedSkillID[{0}]", tag));
			}
			else
			{
				this.RefreshAddedSkillItem(form, tag, true);
				CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1166u);
				cSPkg.stPkgData.get_stUnlockSkillSelReq().dwHeroID = this.m_showHeroID;
				cSPkg.stPkgData.get_stUnlockSkillSelReq().dwSkillID = tag;
				Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
			}
			this.OnCloseAddedSkillPanel(null);
		}

		public void OnOpenAddedSkillPanel(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			form.transform.Find("PanelAddSkill").gameObject.CustomSetActive(true);
			if (Singleton<CHeroSelectBaseSystem>.get_instance().IsMultilMode() || Singleton<CHeroSelectBaseSystem>.get_instance().IsSingleWarmBattle())
			{
				Singleton<CChatController>.get_instance().Hide_SelectChat_MidNode();
				Singleton<CChatController>.get_instance().Set_Show_Bottom(false);
				Singleton<CChatController>.get_instance().SetEntryNodeVoiceBtnShowable(false);
			}
		}

		public void OnCloseAddedSkillPanel(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			form.transform.Find("PanelAddSkill").gameObject.CustomSetActive(false);
			if (Singleton<CHeroSelectBaseSystem>.get_instance().IsMultilMode() || Singleton<CHeroSelectBaseSystem>.get_instance().IsSingleWarmBattle())
			{
				Singleton<CChatController>.get_instance().Set_Show_Bottom(true);
				Singleton<CChatController>.get_instance().SetEntryNodeVoiceBtnShowable(true);
			}
		}

		private void OnCancelBuy(CUIEvent uiEvent)
		{
			if (uiEvent.m_srcFormScript.transform == null)
			{
				return;
			}
			if (Singleton<CHeroSelectBaseSystem>.get_instance().m_isSelectConfirm)
			{
				return;
			}
			Transform transform = uiEvent.m_srcFormScript.transform.FindChild("Other/RandomHero");
			CUICommonSystem.SetButtonEnableWithShader(transform.GetComponent<Button>(), true, true);
		}

		private void BuyHeroCount(CUIEvent uiEvent)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1113u);
			cSPkg.stPkgData.get_stShopBuyReq().iBuyType = 13;
			cSPkg.stPkgData.get_stShopBuyReq().iBuySubType = Singleton<CHeroSelectBaseSystem>.get_instance().m_UseRandSelCount + 1;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		public void OnReqRandHero(CUIEvent uiEvent)
		{
			if (Singleton<CHeroSelectBaseSystem>.get_instance().selectType == enSelectType.enRandom)
			{
				ResShopInfo cfgShopInfo = CPurchaseSys.GetCfgShopInfo(13, Singleton<CHeroSelectBaseSystem>.get_instance().m_UseRandSelCount + 1);
				if (cfgShopInfo != null)
				{
					Transform transform = uiEvent.m_srcFormScript.transform.FindChild("Other/RandomHero");
					CUICommonSystem.SetButtonEnableWithShader(transform.GetComponent<Button>(), false, true);
					transform.transform.Find("Timer").GetComponent<CUITimerScript>().ReStartTimer();
					uint dwCoinPrice = cfgShopInfo.dwCoinPrice;
					int dwValue = (int)cfgShopInfo.dwValue;
					enPayType payType = CMallSystem.ResBuyTypeToPayType((int)cfgShopInfo.bCoinType);
					stUIEventParams stUIEventParams = default(stUIEventParams);
					CMallSystem.TryToPay(enPayPurpose.Buy, string.Empty, payType, dwCoinPrice, enUIEventID.HeroCount_Buy, ref stUIEventParams, enUIEventID.None, false, false, false);
				}
			}
		}

		public void OnHeroCountBought()
		{
			if (Singleton<CHeroSelectBaseSystem>.get_instance().selectType == enSelectType.enRandom)
			{
				if (Singleton<CHeroSelectBaseSystem>.get_instance().gameType == enSelectGameType.enPVE_Computer)
				{
					this.RadomHeroBySelfWithSingleMode();
					this.RefreshHeroPanel(false, true);
					this.RefreshSkinPanel(null);
				}
				Singleton<CHeroSelectBaseSystem>.get_instance().m_UseRandSelCount++;
				this.RefreshLeftRandCountText();
			}
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
		}

		public void RadomHeroBySelfWithSingleMode()
		{
			bool flag = false;
			List<uint> list = new List<uint>();
			ResBanHeroConf dataByKey = GameDataMgr.banHeroBin.GetDataByKey(GameDataMgr.GetDoubleKey(4u, Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.roomAttrib.dwMapId));
			if (dataByKey != null)
			{
				for (int i = 0; i < dataByKey.BanHero.Length; i++)
				{
					if (dataByKey.BanHero[i] != 0u)
					{
						list.Add(dataByKey.BanHero[i]);
					}
				}
			}
			while (!flag)
			{
				IHeroData heroData = Singleton<CHeroSelectBaseSystem>.get_instance().m_canUseHeroList.get_Item(Random.Range(0, Singleton<CHeroSelectBaseSystem>.get_instance().m_canUseHeroList.get_Count()));
				if ((Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Item(0) != heroData.cfgID && !list.Contains(heroData.cfgID)) || Singleton<CHeroSelectBaseSystem>.get_instance().m_canUseHeroList.get_Count() == 1)
				{
					Singleton<CHeroSelectBaseSystem>.get_instance().SetPvpHeroSelect(heroData.cfgID);
					this.m_showHeroID = heroData.cfgID;
					MemberInfo masterMemberInfo = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetMasterMemberInfo();
					if (masterMemberInfo == null)
					{
						return;
					}
					masterMemberInfo.ChoiceHero[0].stBaseInfo.stCommonInfo.dwHeroID = heroData.cfgID;
					flag = true;
				}
			}
		}

		public void StartEndTimer(int totlaTimes)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.transform.Find("CountDown/Timer");
			if (transform == null)
			{
				return;
			}
			CUITimerScript component = transform.GetComponent<CUITimerScript>();
			if (component == null)
			{
				return;
			}
			component.SetTotalTime((float)totlaTimes);
			component.m_timerType = enTimerType.CountDown;
			component.ReStartTimer();
			component.gameObject.CustomSetActive(true);
		}

		private void OnTimerCountDown(CUIEvent uiEvent)
		{
			if (uiEvent.m_srcFormScript == null || uiEvent.m_srcWidget == null)
			{
				return;
			}
			CUIFormScript srcFormScript = uiEvent.m_srcFormScript;
			Transform transform = srcFormScript.transform.Find("CountDownMovie");
			CUITimerScript component = uiEvent.m_srcWidget.GetComponent<CUITimerScript>();
			if (component.GetCurrentTime() <= 6.1f && !transform.gameObject.activeSelf && (Singleton<CHeroSelectBaseSystem>.get_instance().selectType != enSelectType.enClone || Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enSwap))
			{
				transform.gameObject.CustomSetActive(true);
				component.gameObject.CustomSetActive(false);
				Singleton<CSoundManager>.GetInstance().PostEvent("UI_daojishi", null);
			}
		}

		public void RefreshHeroPanel(bool bForceRefreshAddSkillPanel = false, bool bRefreshSymbol = true)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (form == null || masterRoleInfo == null)
			{
				return;
			}
			CUIListScript component = form.transform.Find("Other/SkillList").gameObject.GetComponent<CUIListScript>();
			if (Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroCount <= 0)
			{
				CUICommonSystem.PlayAnimator(form.gameObject, "show");
			}
			else
			{
				CUICommonSystem.PlayAnimator(form.gameObject, "hide");
			}
			this.RefreshHeroInfo_LeftPanel(form, masterRoleInfo);
			this.RefreshHeroInfo_RightPanel(form, masterRoleInfo);
			this.RefreshHeroInfo_CenterPanel(form, masterRoleInfo, component);
			this.RefreshHeroInfo_SpecSkillPanel(component, masterRoleInfo, bForceRefreshAddSkillPanel, form);
			if (bRefreshSymbol)
			{
				this.RefreshHeroInfo_DropList();
			}
			this.RefreshHeroInfo_ExperiencePanel(form);
			this.RefreshHeroInfo_ConfirmButtonPanel(form, masterRoleInfo);
		}

		private void RefreshHeroInfo_LeftPanel(CUIFormScript form, CRoleInfo roleInfo)
		{
			CUIListScript component = form.transform.Find("PanelLeft/ListHostHeroInfo").gameObject.GetComponent<CUIListScript>();
			CUIListScript component2 = form.transform.Find("PanelLeft/ListHostHeroInfoFull").gameObject.GetComponent<CUIListScript>();
			component.m_alwaysDispatchSelectedChangeEvent = true;
			component2.m_alwaysDispatchSelectedChangeEvent = true;
			component.SetElementAmount(Singleton<CHeroSelectBaseSystem>.get_instance().m_canUseHeroList.get_Count());
			this.InitFullHeroListData();
			component2.SetElementAmount(this.m_canUseHeroListByJob.get_Count());
		}

		private void RefreshHeroInfo_RightPanel(CUIFormScript form, CRoleInfo roleInfo)
		{
			CUIListScript component = form.transform.Find("PanelRight/ListTeamHeroInfo").gameObject.GetComponent<CUIListScript>();
			component.m_alwaysDispatchSelectedChangeEvent = true;
			List<uint> teamHeroList;
			if (Singleton<CHeroSelectBaseSystem>.get_instance().IsMobaMode())
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
				teamHeroList = Singleton<CHeroSelectBaseSystem>.get_instance().GetTeamHeroList(masterMemberInfo.camp);
			}
			else
			{
				teamHeroList = Singleton<CHeroSelectBaseSystem>.get_instance().GetTeamHeroList(1);
			}
			component.SetElementAmount(teamHeroList.get_Count());
			for (int i = 0; i < teamHeroList.get_Count(); i++)
			{
				GameObject gameObject = component.GetElemenet(i).gameObject;
				GameObject gameObject2 = gameObject.transform.Find("heroItemCell").gameObject;
				GameObject gameObject3 = gameObject2.transform.Find("ItemBg1").gameObject;
				GameObject gameObject4 = gameObject2.transform.Find("ItemBg2").gameObject;
				GameObject gameObject5 = gameObject2.transform.Find("redReadyIcon").gameObject;
				GameObject gameObject6 = gameObject2.transform.Find("redReadyIcon").gameObject;
				GameObject gameObject7 = gameObject2.transform.Find("selfIcon").gameObject;
				GameObject gameObject8 = gameObject2.transform.Find("delBtn").gameObject;
				GameObject selSkillCell = gameObject.transform.Find("selSkillItemCell").gameObject;
				Image component2 = gameObject2.transform.Find("imageIcon").gameObject.GetComponent<Image>();
				Text component3 = gameObject2.transform.Find("lblName").gameObject.GetComponent<Text>();
				CUIEventScript component4 = gameObject2.GetComponent<CUIEventScript>();
				uint num = teamHeroList.get_Item(i);
				component3.text = string.Empty;
				gameObject3.CustomSetActive(false);
				gameObject4.CustomSetActive(false);
				gameObject5.CustomSetActive(false);
				gameObject6.CustomSetActive(false);
				gameObject7.CustomSetActive(false);
				gameObject8.CustomSetActive(false);
				selSkillCell.CustomSetActive(false);
				component4.enabled = false;
				if (Singleton<CHeroSelectBaseSystem>.get_instance().IsMobaMode())
				{
					MemberInfo masterMemberInfo2 = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetMasterMemberInfo();
					ListView<MemberInfo> listView = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo[masterMemberInfo2.camp];
					MemberInfo memberInfo = listView.get_Item(i);
					component3.text = memberInfo.MemberName;
					if (Singleton<CHeroSelectBaseSystem>.get_instance().gameType == enSelectGameType.enPVE_Computer && masterMemberInfo2.ChoiceHero[0].stBaseInfo.stCommonInfo.dwHeroID != 0u)
					{
						masterMemberInfo2.ChoiceHero[0].stBaseInfo.stCommonInfo.stSkill.dwSelSkillID = CAddSkillSys.GetSelfSelSkill(Singleton<CHeroSelectBaseSystem>.get_instance().m_mapUnUseSkill, masterMemberInfo2.ChoiceHero[0].stBaseInfo.stCommonInfo.dwHeroID);
					}
					uint selSkillID = memberInfo.ChoiceHero[0].stBaseInfo.stCommonInfo.stSkill.dwSelSkillID;
					if (Singleton<CHeroSelectBaseSystem>.get_instance().IsMobaMode() && selSkillID != 0u)
					{
						GameDataMgr.addedSkiilDatabin.Accept(delegate(ResSkillUnlock rule)
						{
							if (rule != null && rule.dwUnlockSkillID == selSkillID)
							{
								ResSkillCfgInfo dataByKey = GameDataMgr.skillDatabin.GetDataByKey(selSkillID);
								if (dataByKey != null)
								{
									selSkillCell.CustomSetActive(true);
									string prefabPath = string.Format("{0}{1}", CUIUtility.s_Sprite_Dynamic_Skill_Dir, Utility.UTF8Convert(dataByKey.szIconPath));
									selSkillCell.transform.Find("Icon").GetComponent<Image>().SetSprite(prefabPath, form, true, false, false, false);
								}
								else
								{
									DebugHelper.Assert(false, string.Format("SelSkill ResSkillCfgInfo[{0}] can not be find!!", selSkillID));
								}
							}
						});
					}
					if (memberInfo.dwObjId == Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.selfObjID && memberInfo.RoomMemberType != 2u)
					{
						gameObject3.CustomSetActive(true);
						if (memberInfo.isPrepare)
						{
							gameObject5.CustomSetActive(true);
						}
						component3.text = Singleton<CTextManager>.get_instance().GetText("Pvp_PlayerName", new string[]
						{
							memberInfo.MemberName
						});
					}
					else
					{
						gameObject4.CustomSetActive(true);
						if (memberInfo.isPrepare)
						{
							gameObject6.CustomSetActive(true);
						}
					}
				}
				else
				{
					if (num != 0u)
					{
						IHeroData heroData = CHeroDataFactory.CreateHeroData(num);
						if (heroData == null)
						{
							return;
						}
						component3.text = heroData.heroName;
					}
					gameObject3.CustomSetActive(true);
					if (i == 0)
					{
						gameObject7.CustomSetActive(true);
					}
					if (i < (int)Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroCount)
					{
						gameObject8.CustomSetActive(true);
					}
				}
				if (num != 0u)
				{
					IHeroData heroData2 = CHeroDataFactory.CreateHeroData(num);
					if (heroData2 == null)
					{
						return;
					}
					component2.SetSprite(CUIUtility.s_Sprite_Dynamic_Icon_Dir + CSkinInfo.GetHeroSkinPic(heroData2.cfgID, 0u), form, true, false, false, false);
				}
				else if (!Singleton<CHeroSelectBaseSystem>.get_instance().IsMobaMode())
				{
					component2.SetSprite(CUIUtility.s_Sprite_System_HeroSelect_Dir + "HeroChoose_unknownIcon", form, true, false, false, false);
				}
				else
				{
					MemberInfo masterMemberInfo3 = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetMasterMemberInfo();
					ListView<MemberInfo> listView2 = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo[masterMemberInfo3.camp];
					MemberInfo memberInfo2 = listView2.get_Item(i);
					if (memberInfo2.RoomMemberType == 2u && !Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.roomAttrib.bWarmBattle)
					{
						component2.SetSprite(CUIUtility.s_Sprite_System_HeroSelect_Dir + "Img_ComputerHead", form, true, false, false, false);
					}
					else
					{
						component2.SetSprite(CUIUtility.s_Sprite_System_HeroSelect_Dir + "HeroChoose_unknownIcon", form, true, false, false, false);
					}
				}
				if (Singleton<CHeroSelectBaseSystem>.get_instance().IsMobaMode())
				{
					if (Singleton<CHeroSelectBaseSystem>.get_instance().m_isSelectConfirm && num != 0u)
					{
						component4.enabled = true;
					}
				}
				else if (num != 0u)
				{
					component4.enabled = true;
				}
			}
		}

		private void RefreshHeroInfo_CenterPanel(CUIFormScript form, CRoleInfo roleInfo, CUIListScript skillList)
		{
			CUI3DImageScript component = form.transform.Find("PanelCenter/3DImage").gameObject.GetComponent<CUI3DImageScript>();
			Image component2 = form.transform.Find("Other/HeroInfo/imgJob").gameObject.GetComponent<Image>();
			Text component3 = form.transform.Find("Other/HeroInfo/HeroName/lblName").gameObject.GetComponent<Text>();
			Text component4 = form.transform.Find("Other/HeroInfo/HeroName/jobTitleText").gameObject.GetComponent<Text>();
			Text component5 = form.transform.Find("Other/HeroInfo/HeroJob/jobFeatureText").gameObject.GetComponent<Text>();
			if (this.m_showHeroID == 0u)
			{
				component3.gameObject.CustomSetActive(false);
				component2.gameObject.CustomSetActive(false);
				component4.gameObject.CustomSetActive(false);
				component5.gameObject.CustomSetActive(false);
			}
			if (this.m_nowShowHeroID != this.m_showHeroID)
			{
				component.RemoveGameObject(this.m_heroGameObjName);
				this.m_nowShowHeroID = this.m_showHeroID;
				if (this.m_nowShowHeroID != 0u)
				{
					int heroWearSkinId = (int)roleInfo.GetHeroWearSkinId(this.m_nowShowHeroID);
					ObjNameData heroPrefabPath = CUICommonSystem.GetHeroPrefabPath(this.m_nowShowHeroID, heroWearSkinId, true);
					this.m_heroGameObjName = heroPrefabPath.ObjectName;
					GameObject gameObject = component.AddGameObject(this.m_heroGameObjName, false, false);
					if (gameObject != null)
					{
						gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
						if (heroPrefabPath.ActorInfo != null)
						{
							gameObject.transform.localScale = new Vector3(heroPrefabPath.ActorInfo.LobbyScale, heroPrefabPath.ActorInfo.LobbyScale, heroPrefabPath.ActorInfo.LobbyScale);
						}
						CHeroAnimaSystem instance = Singleton<CHeroAnimaSystem>.GetInstance();
						instance.Set3DModel(gameObject);
						instance.InitAnimatList();
						instance.InitAnimatSoundList(this.m_nowShowHeroID, (uint)heroWearSkinId);
						instance.OnModePlayAnima("Come");
					}
					IHeroData heroData = CHeroDataFactory.CreateHeroData(this.m_nowShowHeroID);
					if (heroData == null)
					{
						return;
					}
					ResDT_SkillInfo[] skillArr = heroData.skillArr;
					skillList.SetElementAmount(skillArr.Length - 1);
					for (int i = 0; i < skillArr.Length - 1; i++)
					{
						GameObject gameObject2 = skillList.GetElemenet(i).gameObject.transform.Find("heroSkillItemCell").gameObject;
						ResSkillCfgInfo skillCfgInfo = CSkillData.GetSkillCfgInfo(skillArr[i].iSkillID);
						CUIEventScript component6 = gameObject2.GetComponent<CUIEventScript>();
						if (skillCfgInfo == null)
						{
							return;
						}
						if (i == 0)
						{
							gameObject2.transform.localScale = new Vector3(0.85f, 0.85f, 1f);
						}
						else
						{
							gameObject2.transform.localScale = Vector3.one;
						}
						GameObject gameObject3 = Utility.FindChild(gameObject2, "skillMask/skillIcon");
						if (gameObject3 == null)
						{
							return;
						}
						Image component7 = gameObject3.GetComponent<Image>();
						if (component7 == null)
						{
							return;
						}
						CUIUtility.SetImageSprite(component7, CUIUtility.s_Sprite_Dynamic_Skill_Dir + StringHelper.UTF8BytesToString(ref skillCfgInfo.szIconPath), form, true, false, false, false);
						gameObject3.CustomSetActive(true);
						stUIEventParams eventParams = default(stUIEventParams);
						eventParams.skillTipParam = default(stSkillTipParams);
						eventParams.skillPropertyDesc = CUICommonSystem.ParseSkillLevelUpProperty(ref skillCfgInfo.astSkillPropertyDescInfo, this.m_nowShowHeroID);
						eventParams.skillSlotId = i;
						eventParams.skillTipParam.strTipText = CUICommonSystem.GetSkillDescLobby(skillCfgInfo.szLobbySkillDesc, this.m_nowShowHeroID);
						eventParams.skillTipParam.strTipTitle = StringHelper.UTF8BytesToString(ref skillCfgInfo.szSkillName);
						eventParams.skillTipParam.skillCoolDown = ((i != 0) ? Singleton<CTextManager>.get_instance().GetText("Skill_Cool_Down_Tips", new string[]
						{
							(skillCfgInfo.iCoolDown / 1000).ToString()
						}) : Singleton<CTextManager>.get_instance().GetText("Skill_Common_Effect_Type_5"));
						eventParams.skillTipParam.skillEnergyCost = ((i != 0) ? Singleton<CTextManager>.get_instance().GetText(EnergyCommon.GetEnergyShowText((uint)skillCfgInfo.bEnergyCostType, EnergyShowType.CostValue), new string[]
						{
							skillCfgInfo.iEnergyCost.ToString()
						}) : string.Empty);
						eventParams.skillTipParam.skillEffect = skillCfgInfo.SkillEffectType;
						component6.SetUIEvent(enUIEventType.Down, enUIEventID.HeroSelect_Skill_Down, eventParams);
					}
					component3.text = heroData.heroName;
					CUICommonSystem.SetHeroJob(form, component2.gameObject, (enHeroJobType)heroData.heroType);
					component3.gameObject.CustomSetActive(true);
					component2.gameObject.CustomSetActive(true);
					component4.gameObject.CustomSetActive(true);
					component5.gameObject.CustomSetActive(true);
					component4.text = CHeroInfo.GetHeroJob(this.m_nowShowHeroID);
					component5.text = CHeroInfo.GetJobFeature(this.m_nowShowHeroID);
				}
			}
		}

		private void RefreshHeroInfo_SpecSkillPanel(CUIListScript skillList, CRoleInfo roleInfo, bool bForceRefreshAddSkillPanel, CUIFormScript form)
		{
			skillList.gameObject.CustomSetActive(false);
			if (this.m_nowShowHeroID != 0u)
			{
				skillList.gameObject.CustomSetActive(true);
				if (CAddSkillSys.IsSelSkillAvailable())
				{
					if (Singleton<CHeroSelectBaseSystem>.get_instance().IsMultilMode())
					{
						MemberInfo masterMemberInfo = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetMasterMemberInfo();
						if (masterMemberInfo != null)
						{
							this.RefreshAddedSkillItem(form, masterMemberInfo.ChoiceHero[0].stBaseInfo.stCommonInfo.stSkill.dwSelSkillID, bForceRefreshAddSkillPanel);
						}
					}
					else if (Singleton<CHeroSelectBaseSystem>.get_instance().gameType == enSelectGameType.enPVE_Computer)
					{
						uint selfSelSkill = CAddSkillSys.GetSelfSelSkill(Singleton<CHeroSelectBaseSystem>.get_instance().m_mapUnUseSkill, this.m_showHeroID);
						this.RefreshAddedSkillItem(form, selfSelSkill, bForceRefreshAddSkillPanel);
					}
				}
			}
		}

		private void RefreshHeroInfo_ConfirmButtonPanel(CUIFormScript form, CRoleInfo roleInfo)
		{
			Button component = form.transform.Find("PanelRight/btnConfirm").gameObject.GetComponent<Button>();
			bool flag = true;
			if (Singleton<CHeroSelectBaseSystem>.get_instance().IsMobaMode())
			{
				flag = Singleton<CHeroSelectBaseSystem>.get_instance().m_isSelectConfirm;
				MemberInfo masterMemberInfo = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetMasterMemberInfo();
				if (masterMemberInfo == null)
				{
					return;
				}
				if (masterMemberInfo.ChoiceHero[0].stBaseInfo.stCommonInfo.dwHeroID == 0u)
				{
					flag = true;
				}
			}
			else if (Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroCount == 0)
			{
				flag = true;
			}
			else if (!Singleton<CHeroSelectBaseSystem>.get_instance().m_isSelectConfirm)
			{
				flag = false;
			}
			if (flag)
			{
				CUICommonSystem.SetButtonEnableWithShader(component.GetComponent<Button>(), false, true);
			}
			else
			{
				CUICommonSystem.SetButtonEnableWithShader(component.GetComponent<Button>(), true, true);
			}
		}

		private void RefreshHeroInfo_ExperiencePanel(CUIFormScript form)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			GameObject widget = form.GetWidget(0);
			GameObject widget2 = form.GetWidget(1);
			GameObject widget3 = form.GetWidget(2);
			GameObject widget4 = form.GetWidget(3);
			widget.CustomSetActive(false);
			widget2.CustomSetActive(false);
			widget3.CustomSetActive(false);
			widget4.CustomSetActive(false);
			if (masterRoleInfo.IsValidExperienceHero(this.m_showHeroID))
			{
				CUICommonSystem.RefreshExperienceHeroLeftTime(widget, widget3, this.m_showHeroID);
			}
			uint heroWearSkinId = masterRoleInfo.GetHeroWearSkinId(this.m_showHeroID);
			if (masterRoleInfo.IsValidExperienceSkin(this.m_showHeroID, heroWearSkinId))
			{
				CUICommonSystem.RefreshExperienceSkinLeftTime(widget2, widget4, this.m_showHeroID, heroWearSkinId, null);
			}
		}

		private void RefreshLeftRandCountText()
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.transform.FindChild("Other/RandomHero");
			if (Singleton<CHeroSelectBaseSystem>.get_instance().selectType == enSelectType.enRandom)
			{
				transform.gameObject.CustomSetActive(true);
				ResShopInfo cfgShopInfo = CPurchaseSys.GetCfgShopInfo(13, Singleton<CHeroSelectBaseSystem>.get_instance().m_UseRandSelCount + 1);
				if (cfgShopInfo != null)
				{
					stPayInfo stPayInfo = default(stPayInfo);
					stPayInfo.m_payType = CMallSystem.ResBuyTypeToPayType((int)cfgShopInfo.bCoinType);
					stPayInfo.m_payValue = cfgShopInfo.dwCoinPrice;
					stUIEventParams stUIEventParams = default(stUIEventParams);
					CMallSystem.SetPayButton(form, transform.transform as RectTransform, stPayInfo.m_payType, stPayInfo.m_payValue, enUIEventID.HeroSelect_RandomHero, ref stUIEventParams);
				}
				else
				{
					transform.gameObject.CustomSetActive(false);
				}
			}
			else
			{
				transform.gameObject.CustomSetActive(false);
			}
		}

		public void ResetHero3DObj()
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectNormalSystem.s_heroSelectFormPath);
			if (form == null || form.gameObject == null || this.m_nowShowHeroID == 0u)
			{
				return;
			}
			GameObject gameObject = form.gameObject;
			CUI3DImageScript component = gameObject.transform.Find("PanelCenter/3DImage").gameObject.GetComponent<CUI3DImageScript>();
			GameObject gameObject2 = component.GetGameObject(this.m_heroGameObjName);
			if (gameObject2 != null)
			{
				CHeroAnimaSystem instance = Singleton<CHeroAnimaSystem>.GetInstance();
				instance.Set3DModel(gameObject2);
				instance.InitAnimatList();
				uint heroWearSkinId = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().GetHeroWearSkinId(this.m_nowShowHeroID);
				instance.InitAnimatSoundList(this.m_nowShowHeroID, heroWearSkinId);
			}
		}

		private void LeftHeroItemEnable(CUIEvent uiEvent)
		{
			CUIFormScript srcFormScript = uiEvent.m_srcFormScript;
			CUIListScript srcWidgetBelongedListScript = uiEvent.m_srcWidgetBelongedListScript;
			GameObject srcWidget = uiEvent.m_srcWidget;
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			int srcWidgetIndexInBelongedList = uiEvent.m_srcWidgetIndexInBelongedList;
			int count = Singleton<CHeroSelectBaseSystem>.get_instance().m_canUseHeroList.get_Count();
			if (srcWidgetBelongedListScript.gameObject.name != CHeroSelectNormalSystem.s_defaultHeroListName)
			{
				count = this.m_canUseHeroListByJob.get_Count();
			}
			if (srcFormScript == null || srcWidgetBelongedListScript == null || srcWidget == null || masterRoleInfo == null || srcWidgetIndexInBelongedList < 0 || srcWidgetIndexInBelongedList >= count)
			{
				return;
			}
			IHeroData heroData;
			if (srcWidgetBelongedListScript.gameObject.name != CHeroSelectNormalSystem.s_defaultHeroListName)
			{
				heroData = this.m_canUseHeroListByJob.get_Item(srcWidgetIndexInBelongedList);
			}
			else
			{
				heroData = Singleton<CHeroSelectBaseSystem>.get_instance().m_canUseHeroList.get_Item(srcWidgetIndexInBelongedList);
			}
			if (heroData == null)
			{
				return;
			}
			CUIListElementScript component = srcWidget.GetComponent<CUIListElementScript>();
			if (component == null)
			{
				return;
			}
			Image component2 = srcWidget.transform.Find("imgHP").gameObject.GetComponent<Image>();
			Image component3 = srcWidget.transform.Find("imgDead").gameObject.GetComponent<Image>();
			GameObject gameObject = srcWidget.transform.Find("heroItemCell").gameObject;
			GameObject gameObject2 = gameObject.transform.Find("TxtFree").gameObject;
			GameObject gameObject3 = gameObject.transform.Find("TxtCreditFree").gameObject;
			GameObject gameObject4 = gameObject.transform.Find("imgExperienceMark").gameObject;
			Transform targetTrans = gameObject.transform.Find("expCardPanel");
			CUIEventScript component4 = gameObject.GetComponent<CUIEventScript>();
			CUIEventScript component5 = srcWidget.GetComponent<CUIEventScript>();
			bool flag = masterRoleInfo.IsFreeHero(heroData.cfgID);
			bool flag2 = masterRoleInfo.IsCreditFreeHero(heroData.cfgID);
			gameObject2.CustomSetActive(flag && !flag2);
			gameObject3.CustomSetActive(flag2);
			if (masterRoleInfo.IsValidExperienceHero(heroData.cfgID) && !Singleton<CHeroSelectBaseSystem>.get_instance().IsSpecTraingMode())
			{
				gameObject4.CustomSetActive(true);
			}
			else
			{
				gameObject4.CustomSetActive(false);
			}
			if (!CHeroDataFactory.IsHeroCanUse(heroData.cfgID) && !Singleton<CHeroSelectBaseSystem>.get_instance().IsSpecTraingMode())
			{
				CUICommonSystem.SetObjActive(targetTrans, true);
			}
			else
			{
				CUICommonSystem.SetObjActive(targetTrans, false);
			}
			component2.gameObject.CustomSetActive(false);
			component3.gameObject.CustomSetActive(false);
			component4.enabled = false;
			component5.enabled = false;
			if (!Singleton<CHeroSelectBaseSystem>.get_instance().m_isSelectConfirm)
			{
				bool flag3 = Singleton<CHeroSelectBaseSystem>.get_instance().IsHeroExist(heroData.cfgID);
				if (Singleton<CHeroSelectBaseSystem>.get_instance().gameType == enSelectGameType.enBurning)
				{
					if (Singleton<BurnExpeditionController>.get_instance().model.IsHeroInRecord(heroData.cfgID))
					{
						int num = Singleton<BurnExpeditionController>.get_instance().model.Get_HeroHP(heroData.cfgID);
						int num2 = Singleton<BurnExpeditionController>.get_instance().model.Get_HeroMaxHP(heroData.cfgID);
						if (num <= 0)
						{
							flag3 = true;
							component3.gameObject.CustomSetActive(true);
						}
						else
						{
							component2.CustomFillAmount((float)num / ((float)num2 * 1f));
							component2.gameObject.CustomSetActive(true);
						}
					}
					else
					{
						component2.CustomFillAmount(1f);
						component2.gameObject.CustomSetActive(true);
					}
				}
				if (!flag3)
				{
					component4.enabled = true;
					component5.enabled = true;
					CUICommonSystem.SetHeroItemData(srcFormScript, gameObject, heroData, enHeroHeadType.enIcon, false, true);
				}
				else
				{
					CUICommonSystem.SetHeroItemData(srcFormScript, gameObject, heroData, enHeroHeadType.enIcon, true, true);
				}
				if (Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroCount > 0)
				{
					if (heroData.cfgID == Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Item((int)(Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroCount - 1)))
					{
						component.ChangeDisplay(true);
					}
					else
					{
						component.ChangeDisplay(false);
					}
				}
				else
				{
					component.ChangeDisplay(false);
				}
			}
			else
			{
				CUICommonSystem.SetHeroItemData(srcFormScript, gameObject, heroData, enHeroHeadType.enIcon, true, true);
			}
		}

		private void InitFullHeroListData()
		{
			this.m_canUseHeroListByJob.Clear();
			ListView<IHeroData> canUseHeroList = Singleton<CHeroSelectBaseSystem>.get_instance().m_canUseHeroList;
			for (int i = 0; i < canUseHeroList.get_Count(); i++)
			{
				if (this.m_heroSelectJobType == enHeroJobType.All || canUseHeroList.get_Item(i).heroCfgInfo.bMainJob == (byte)this.m_heroSelectJobType || canUseHeroList.get_Item(i).heroCfgInfo.bMinorJob == (byte)this.m_heroSelectJobType)
				{
					this.m_canUseHeroListByJob.Add(canUseHeroList.get_Item(i));
				}
			}
			CHeroOverviewSystem.SortHeroList(ref this.m_canUseHeroListByJob, Singleton<CHeroSelectBaseSystem>.get_instance().m_sortType, false);
		}

		private void InitHeroJobMenu(CUIFormScript form)
		{
			string text = Singleton<CTextManager>.GetInstance().GetText("Hero_Job_All");
			string text2 = Singleton<CTextManager>.GetInstance().GetText("Hero_Job_Tank");
			string text3 = Singleton<CTextManager>.GetInstance().GetText("Hero_Job_Soldier");
			string text4 = Singleton<CTextManager>.GetInstance().GetText("Hero_Job_Assassin");
			string text5 = Singleton<CTextManager>.GetInstance().GetText("Hero_Job_Master");
			string text6 = Singleton<CTextManager>.GetInstance().GetText("Hero_Job_Archer");
			string text7 = Singleton<CTextManager>.GetInstance().GetText("Hero_Job_Aid");
			string[] titleList = new string[]
			{
				text,
				text2,
				text3,
				text4,
				text5,
				text6,
				text7
			};
			GameObject gameObject = form.transform.Find("PanelLeft/MenuList").gameObject;
			CUICommonSystem.InitMenuPanel(gameObject, titleList, 0, true);
		}

		public void OnHeroJobMenuSelect(CUIEvent uiEvent)
		{
			CUIListScript component = uiEvent.m_srcWidget.GetComponent<CUIListScript>();
			int selectedIndex = component.GetSelectedIndex();
			if (this.m_heroSelectJobType != (enHeroJobType)selectedIndex)
			{
				this.m_heroSelectJobType = (enHeroJobType)selectedIndex;
				this.RefreshHeroInfo_LeftPanel(uiEvent.m_srcFormScript, Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo());
			}
		}
	}
}
