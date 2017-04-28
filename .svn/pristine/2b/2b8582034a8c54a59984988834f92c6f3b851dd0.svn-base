using Assets.Scripts.Framework;
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
	public class CHeroSelectBanPickSystem : Singleton<CHeroSelectBanPickSystem>
	{
		private const float c_countDownCheckTime = 6.1f;

		public const int c_banHeroCountMax = 3;

		public static string s_heroSelectFormPath = "UGUI/Form/System/HeroSelect/Form_HeroSelectBanPick.prefab";

		public static string s_symbolPropPanelPath = "Bottom/Panel_SymbolProp";

		private ListView<IHeroData> m_banHeroList;

		private IHeroData m_selectBanHeroData;

		private enHeroJobType m_heroSelectJobType;

		private ListView<IHeroData> m_canUseHeroListByJob = new ListView<IHeroData>();

		public override void Init()
		{
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_BanPick_FormClose, new CUIEventManager.OnUIEventHandler(this.HeroSelect_OnClose));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_BanPick_HeroJobMenuSelect, new CUIEventManager.OnUIEventHandler(this.OnHeroJobMenuSelect));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_BanPick_SkinSelect, new CUIEventManager.OnUIEventHandler(this.HeroSelect_OnSkinSelect));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_BanPick_SelectHero, new CUIEventManager.OnUIEventHandler(this.HeroSelect_SelectHero));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_BanPick_CenterHeroItemEnable, new CUIEventManager.OnUIEventHandler(this.CenterHeroItemEnable));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_BanPick_ConfirmHeroSelect, new CUIEventManager.OnUIEventHandler(this.HeroSelect_ConfirmHeroSelect));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_BanPick_SwapHeroReq, new CUIEventManager.OnUIEventHandler(this.HeroSelect_SwapHeroReq));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_BanPick_SwapHeroAllow, new CUIEventManager.OnUIEventHandler(this.HeroSelect_SwapHeroAllow));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_BanPick_SwapHeroCanel, new CUIEventManager.OnUIEventHandler(this.HeroSelect_SwapHeroCanel));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_BanPick_Symbol_PageDownBtnClick, new CUIEventManager.OnUIEventHandler(this.OnSymbolPageDownBtnClick));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_BanPick_SymbolPageSelect, new CUIEventManager.OnUIEventHandler(this.OnHeroSymbolPageSelect));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_BanPick_Symbol_ViewProp_Down, new CUIEventManager.OnUIEventHandler(this.OnOpenSymbolProp));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_BanPick_Symbol_ViewProp_Up, new CUIEventManager.OnUIEventHandler(this.OnCloseSymbolProp));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_BanPick_AddedSkillOpenForm, new CUIEventManager.OnUIEventHandler(this.OnOpenAddedSkillPanel));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_BanPick_AddedSkillSelected, new CUIEventManager.OnUIEventHandler(this.OnSelectedAddedSkill));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_BanPick_AddedSkillConfirm, new CUIEventManager.OnUIEventHandler(this.OnConfirmAddedSkill));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_BanPick_AddedSkillCloseForm, new CUIEventManager.OnUIEventHandler(this.OnCloseAddedSkillPanel));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.HeroSelect_BanPick_OnTimerCountDown, new CUIEventManager.OnUIEventHandler(this.OnTimerCountDown));
		}

		public void CloseForm()
		{
			Singleton<CUIManager>.GetInstance().CloseForm(CHeroSelectBanPickSystem.s_heroSelectFormPath);
		}

		private void HeroSelect_OnClose(CUIEvent uiEvent)
		{
			this.m_banHeroList = null;
			this.m_selectBanHeroData = null;
			this.m_heroSelectJobType = enHeroJobType.All;
			this.m_canUseHeroListByJob.Clear();
			Singleton<CHeroSelectBaseSystem>.get_instance().Clear();
			Singleton<CSoundManager>.GetInstance().UnLoadBank("Music_BanPick", CSoundManager.BankType.Lobby);
			Singleton<CSoundManager>.GetInstance().UnLoadBank("Newguide_Voice_BanPick", CSoundManager.BankType.Lobby);
		}

		public void OpenForm()
		{
			CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(CHeroSelectBanPickSystem.s_heroSelectFormPath, false, true);
			if (cUIFormScript == null || Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo == null)
			{
				return;
			}
			this.m_banHeroList = CHeroDataFactory.GetBanHeroList();
			this.InitSystem(cUIFormScript);
			this.RefreshAll();
			Singleton<CSoundManager>.GetInstance().LoadBank("Music_BanPick", CSoundManager.BankType.Lobby);
			Singleton<CSoundManager>.GetInstance().LoadBank("Newguide_Voice_BanPick", CSoundManager.BankType.Lobby);
		}

		public void InitSystem(CUIFormScript form)
		{
			CUICommonSystem.SetObjActive(form.transform.Find("Top/Timer/CountDownMovie"), false);
			this.InitAddedSkillPanel();
			this.InitMenu(false);
			Singleton<CReplayKitSys>.GetInstance().InitReplayKit(form.transform.Find("ReplayKit"), true, true);
		}

		public void InitMenu(bool isResetListSelect = false)
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CHeroSelectBanPickSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			GameObject gameObject = form.gameObject.transform.Find("PanelCenter/TabListHero").gameObject;
			GameObject gameObject2 = form.gameObject.transform.Find("PanelCenter/TabListSkin").gameObject;
			string[] strTitleList = new string[]
			{
				Singleton<CTextManager>.get_instance().GetText("Choose_Skin")
			};
			string[] strTitleList2 = new string[]
			{
				Singleton<CTextManager>.GetInstance().GetText("Hero_Job_All"),
				Singleton<CTextManager>.GetInstance().GetText("Hero_Job_Tank"),
				Singleton<CTextManager>.GetInstance().GetText("Hero_Job_Soldier"),
				Singleton<CTextManager>.GetInstance().GetText("Hero_Job_Assassin"),
				Singleton<CTextManager>.GetInstance().GetText("Hero_Job_Master"),
				Singleton<CTextManager>.GetInstance().GetText("Hero_Job_Archer"),
				Singleton<CTextManager>.GetInstance().GetText("Hero_Job_Aid")
			};
			Transform targetTrans = form.transform.Find("PanelCenter/ListHostHeroInfo");
			Transform targetTrans2 = form.gameObject.transform.Find("PanelCenter/ListHostSkinInfo");
			if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enBan)
			{
				this.InitSubMenu(gameObject, strTitleList2, true);
				this.InitSubMenu(gameObject2, strTitleList, false);
				CUICommonSystem.SetObjActive(targetTrans, true);
				CUICommonSystem.SetObjActive(targetTrans2, false);
			}
			else if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enPick)
			{
				if (Singleton<CHeroSelectBaseSystem>.get_instance().m_isSelectConfirm)
				{
					this.InitSubMenu(gameObject, strTitleList2, false);
					this.InitSubMenu(gameObject2, strTitleList, true);
					CUICommonSystem.SetObjActive(targetTrans, false);
					CUICommonSystem.SetObjActive(targetTrans2, true);
				}
				else
				{
					this.InitSubMenu(gameObject, strTitleList2, true);
					this.InitSubMenu(gameObject2, strTitleList, false);
					CUICommonSystem.SetObjActive(targetTrans, true);
					CUICommonSystem.SetObjActive(targetTrans2, false);
				}
			}
			else if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enSwap || Singleton<CHeroSelectBaseSystem>.get_instance().m_isSelectConfirm)
			{
				this.InitSubMenu(gameObject, strTitleList2, false);
				this.InitSubMenu(gameObject2, strTitleList, true);
				CUICommonSystem.SetObjActive(targetTrans, false);
				CUICommonSystem.SetObjActive(targetTrans2, true);
			}
			this.ResetHeroSelectJobType();
			if (isResetListSelect)
			{
				this.InitHeroList(form, true);
			}
		}

		private void InitSubMenu(GameObject menuObj, string[] strTitleList, bool isShow)
		{
			if (isShow)
			{
				CUICommonSystem.InitMenuPanel(menuObj, strTitleList, 0, false);
				CUICommonSystem.SetObjActive(menuObj, true);
			}
			else
			{
				CUICommonSystem.SetObjActive(menuObj, false);
			}
		}

		public void RefreshAll()
		{
			this.RefreshTop();
			this.RefreshBottom();
			this.RefreshLeft();
			this.RefreshRight();
			this.RefreshCenter();
			this.RefreshSwapPanel();
		}

		public void RefreshTop()
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CHeroSelectBanPickSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.transform.Find("Top/Timer");
			Transform transform2 = form.transform.Find("Top/Tips");
			Text component = form.transform.Find("Top/Tips/lblTitle").GetComponent<Text>();
			if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enBan)
			{
				component.gameObject.CustomSetActive(true);
				transform2.gameObject.CustomSetActive(true);
				transform.gameObject.CustomSetActive(false);
				component.text = Singleton<CTextManager>.get_instance().GetText("BP_Title_1");
			}
			else if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enPick)
			{
				component.gameObject.CustomSetActive(true);
				transform2.gameObject.CustomSetActive(true);
				transform.gameObject.CustomSetActive(false);
				component.text = Singleton<CTextManager>.get_instance().GetText("BP_Title_2");
			}
			else
			{
				transform2.gameObject.CustomSetActive(false);
				transform.gameObject.CustomSetActive(true);
			}
			CUIListScript component2 = form.transform.Find("Top/LeftListBan").GetComponent<CUIListScript>();
			CUIListScript component3 = form.transform.Find("Top/RightListBan").GetComponent<CUIListScript>();
			MemberInfo masterMemberInfo = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetMasterMemberInfo();
			if (masterMemberInfo == null)
			{
				return;
			}
			COM_PLAYERCAMP camp;
			COM_PLAYERCAMP camp2;
			if (masterMemberInfo.camp == null)
			{
				camp = 1;
				camp2 = 2;
			}
			else
			{
				camp = masterMemberInfo.camp;
				camp2 = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetEnemyCamp(masterMemberInfo.camp);
			}
			this.InitBanHeroList(component2, camp);
			this.InitBanHeroList(component3, camp2);
		}

		public void RefreshBottom()
		{
			this.RefreshSymbolPage();
			this.RefreshAddedSkillItem();
		}

		public void RefreshLeft()
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CHeroSelectBanPickSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			CUIListScript component = form.transform.Find("PanelLeft/TeamHeroInfo").GetComponent<CUIListScript>();
			MemberInfo masterMemberInfo = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetMasterMemberInfo();
			if (masterMemberInfo == null)
			{
				return;
			}
			COM_PLAYERCAMP cOM_PLAYERCAMP = masterMemberInfo.camp;
			if (cOM_PLAYERCAMP == null)
			{
				cOM_PLAYERCAMP = 1;
			}
			this.InitTeamHeroList(component, cOM_PLAYERCAMP);
		}

		public void RefreshRight()
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CHeroSelectBanPickSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			MemberInfo masterMemberInfo = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetMasterMemberInfo();
			if (masterMemberInfo == null)
			{
				return;
			}
			CUIListScript component = form.transform.Find("PanelRight/TeamHeroInfo").GetComponent<CUIListScript>();
			COM_PLAYERCAMP camp;
			if (masterMemberInfo.camp == null)
			{
				camp = 2;
			}
			else
			{
				camp = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetEnemyCamp(masterMemberInfo.camp);
			}
			this.InitTeamHeroList(component, camp);
		}

		public void RefreshCenter()
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CHeroSelectBanPickSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			this.InitHeroList(form, false);
			this.InitSkinList(form, 0u);
		}

		public void RefreshSwapPanel()
		{
			if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep != enBanPickStep.enSwap)
			{
				return;
			}
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CHeroSelectBanPickSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.transform.Find("PanelSwap/PanelSwapHero");
			transform.gameObject.CustomSetActive(false);
			if (Singleton<CHeroSelectBaseSystem>.get_instance().m_swapState == enSwapHeroState.enIdle)
			{
				return;
			}
			if (Singleton<CHeroSelectBaseSystem>.get_instance().m_swapInfo == null)
			{
				return;
			}
			MemberInfo masterMemberInfo = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetMasterMemberInfo();
			MemberInfo memberInfo = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetMemberInfo(Singleton<CHeroSelectBaseSystem>.get_instance().m_swapInfo.dwActiveObjID);
			MemberInfo memberInfo2 = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetMemberInfo(Singleton<CHeroSelectBaseSystem>.get_instance().m_swapInfo.dwPassiveObjID);
			if (masterMemberInfo == null || memberInfo == null || memberInfo2 == null)
			{
				return;
			}
			IHeroData heroData = CHeroDataFactory.CreateHeroData(masterMemberInfo.ChoiceHero[0].stBaseInfo.stCommonInfo.dwHeroID);
			IHeroData heroData2 = CHeroDataFactory.CreateHeroData(memberInfo.ChoiceHero[0].stBaseInfo.stCommonInfo.dwHeroID);
			IHeroData heroData3 = CHeroDataFactory.CreateHeroData(memberInfo2.ChoiceHero[0].stBaseInfo.stCommonInfo.dwHeroID);
			if (heroData == null || heroData2 == null || heroData3 == null)
			{
				return;
			}
			GameObject gameObject = transform.Find("heroItemCell1").gameObject;
			GameObject gameObject2 = transform.Find("heroItemCell2").gameObject;
			GameObject gameObject3 = transform.Find("btnConfirmSwap").gameObject;
			GameObject gameObject4 = transform.Find("btnConfirmSwapCanel").gameObject;
			if (Singleton<CHeroSelectBaseSystem>.get_instance().m_swapState == enSwapHeroState.enSwapAllow)
			{
				CUICommonSystem.SetHeroItemData(form, gameObject, heroData2, enHeroHeadType.enIcon, false, true);
				CUICommonSystem.SetHeroItemData(form, gameObject2, heroData, enHeroHeadType.enIcon, false, true);
				gameObject3.CustomSetActive(true);
				gameObject4.CustomSetActive(true);
			}
			else
			{
				CUICommonSystem.SetHeroItemData(form, gameObject, heroData3, enHeroHeadType.enIcon, false, true);
				CUICommonSystem.SetHeroItemData(form, gameObject2, heroData, enHeroHeadType.enIcon, false, true);
				gameObject3.CustomSetActive(false);
				gameObject4.CustomSetActive(true);
			}
			RectTransform rectTransform = Singleton<CHeroSelectBaseSystem>.get_instance().GetTeamPlayerElement(masterMemberInfo.ullUid, masterMemberInfo.camp) as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			RectTransform rectTransform2 = transform.transform as RectTransform;
			rectTransform2.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + rectTransform.rect.width, rectTransform.anchoredPosition.y);
			transform.gameObject.CustomSetActive(true);
		}

		public void InitBanHeroList(CUIListScript listScript, COM_PLAYERCAMP camp)
		{
			List<uint> banHeroList = Singleton<CHeroSelectBaseSystem>.get_instance().GetBanHeroList(camp);
			listScript.SetElementAmount(Singleton<CHeroSelectBaseSystem>.get_instance().m_banHeroTeamMaxCount);
			for (int i = 0; i < banHeroList.get_Count(); i++)
			{
				Transform transform = listScript.GetElemenet(i).transform;
				IHeroData heroData = CHeroDataFactory.CreateHeroData(banHeroList.get_Item(i));
				if (heroData != null)
				{
					CUICommonSystem.SetObjActive(transform.transform.Find("imageIcon"), true);
					CUICommonSystem.SetHeroItemData(listScript.m_belongedFormScript, transform.gameObject, heroData, enHeroHeadType.enBustCircle, false, true);
				}
			}
		}

		public void InitTeamHeroList(CUIListScript listScript, COM_PLAYERCAMP camp)
		{
			List<uint> teamHeroList = Singleton<CHeroSelectBaseSystem>.get_instance().GetTeamHeroList(camp);
			listScript.SetElementAmount(teamHeroList.get_Count());
			MemberInfo masterMemberInfo = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetMasterMemberInfo();
			if (masterMemberInfo == null)
			{
				return;
			}
			for (int i = 0; i < teamHeroList.get_Count(); i++)
			{
				ListView<MemberInfo> listView = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo[camp];
				MemberInfo memberInfo = listView.get_Item(i);
				uint num = teamHeroList.get_Item(i);
				if (listView == null || memberInfo == null)
				{
					return;
				}
				Transform transform = listScript.GetElemenet(i).transform;
				GameObject gameObject = transform.Find("BgState/NormalBg").gameObject;
				GameObject gameObject2 = transform.Find("BgState/NextBg").gameObject;
				GameObject gameObject3 = transform.Find("BgState/CurrentBg").gameObject;
				CUITimerScript component = transform.Find("BgState/CurrentBg/Timer").GetComponent<CUITimerScript>();
				gameObject.CustomSetActive(false);
				gameObject2.CustomSetActive(false);
				gameObject3.CustomSetActive(false);
				component.gameObject.CustomSetActive(false);
				if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enSwap)
				{
					gameObject.CustomSetActive(true);
				}
				else
				{
					if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep != enBanPickStep.enBan && Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep != enBanPickStep.enPick)
					{
						return;
					}
					if (Singleton<CHeroSelectBaseSystem>.get_instance().IsCurBanOrPickMember(memberInfo))
					{
						gameObject3.CustomSetActive(true);
						component.gameObject.CustomSetActive(true);
						if (!component.IsRunning())
						{
							component.SetTotalTime(Singleton<CHeroSelectBaseSystem>.get_instance().m_curBanPickInfo.stCurState.dwTimeout / 1000u);
							component.ReStartTimer();
						}
					}
					else if (Singleton<CHeroSelectBaseSystem>.get_instance().IsNextBanOrPickMember(memberInfo))
					{
						gameObject2.CustomSetActive(true);
						component.EndTimer();
					}
					else
					{
						gameObject.CustomSetActive(true);
						component.EndTimer();
					}
				}
				GameObject gameObject4 = transform.Find("heroItemCell").gameObject;
				Text component2 = gameObject4.transform.Find("lblName").gameObject.GetComponent<Text>();
				GameObject gameObject5 = transform.Find("heroItemCell/readyIcon").gameObject;
				Image component3 = gameObject4.transform.Find("imageIcon").gameObject.GetComponent<Image>();
				if (num != 0u)
				{
					IHeroData heroData = CHeroDataFactory.CreateHeroData(num);
					if (heroData != null)
					{
						CUICommonSystem.SetHeroItemData(listScript.m_belongedFormScript, gameObject4, heroData, enHeroHeadType.enIcon, false, true);
					}
					component3.gameObject.CustomSetActive(true);
				}
				if (memberInfo.camp == masterMemberInfo.camp)
				{
					if (memberInfo == masterMemberInfo)
					{
						component2.text = Singleton<CTextManager>.get_instance().GetText("Pvp_PlayerName", new string[]
						{
							memberInfo.MemberName
						});
					}
					else
					{
						component2.text = memberInfo.MemberName;
					}
				}
				else
				{
					component2.text = Singleton<CTextManager>.get_instance().GetText("Matching_Tip_9", new string[]
					{
						(memberInfo.dwPosOfCamp + 1u).ToString()
					});
				}
				gameObject5.CustomSetActive(memberInfo.isPrepare);
				CUICommonSystem.SetObjActive(gameObject4.transform.Find("VoiceIcon"), false);
				Button component4 = transform.Find("ExchangeBtn").GetComponent<Button>();
				if (masterMemberInfo.camp != camp)
				{
					component4.gameObject.CustomSetActive(false);
				}
				else if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep != enBanPickStep.enSwap)
				{
					component4.gameObject.CustomSetActive(false);
				}
				else if (Singleton<CHeroSelectBaseSystem>.get_instance().m_swapState != enSwapHeroState.enReqing && memberInfo != masterMemberInfo)
				{
					if (Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.IsHaveHeroByID(masterMemberInfo, num) && Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.IsHaveHeroByID(memberInfo, masterMemberInfo.ChoiceHero[0].stBaseInfo.stCommonInfo.dwHeroID))
					{
						component4.gameObject.CustomSetActive(true);
						CUIEventScript component5 = component4.GetComponent<CUIEventScript>();
						if (component5 != null)
						{
							component5.m_onClickEventParams.tagUInt = memberInfo.dwObjId;
						}
					}
					else
					{
						component4.gameObject.CustomSetActive(false);
					}
				}
				else
				{
					component4.gameObject.CustomSetActive(false);
				}
				GameObject selSkillCell = transform.Find("selSkillItemCell").gameObject;
				uint selSkillID = memberInfo.ChoiceHero[0].stBaseInfo.stCommonInfo.stSkill.dwSelSkillID;
				if (selSkillID != 0u && camp == masterMemberInfo.camp)
				{
					GameDataMgr.addedSkiilDatabin.Accept(delegate(ResSkillUnlock rule)
					{
						if (rule != null && rule.dwUnlockSkillID == selSkillID)
						{
							ResSkillCfgInfo dataByKey = GameDataMgr.skillDatabin.GetDataByKey(selSkillID);
							if (dataByKey != null)
							{
								string prefabPath = string.Format("{0}{1}", CUIUtility.s_Sprite_Dynamic_Skill_Dir, Utility.UTF8Convert(dataByKey.szIconPath));
								selSkillCell.transform.Find("Icon").GetComponent<Image>().SetSprite(prefabPath, listScript.m_belongedFormScript, true, false, false, false);
								selSkillCell.CustomSetActive(true);
							}
							else
							{
								DebugHelper.Assert(false, string.Format("SelSkill ResSkillCfgInfo[{0}] can not be find!!", selSkillID));
							}
						}
					});
				}
				else
				{
					selSkillCell.gameObject.CustomSetActive(false);
				}
				if (memberInfo.camp == masterMemberInfo.camp)
				{
					Transform transform2 = transform.Find("RecentUseHeroPanel");
					if (transform2 != null)
					{
						if (Singleton<CHeroSelectBaseSystem>.get_instance().IsCurBanOrPickMember(memberInfo) || Singleton<CHeroSelectBaseSystem>.get_instance().IsPickedMember(memberInfo) || Singleton<CHeroSelectBaseSystem>.get_instance().gameType != enSelectGameType.enLadder)
						{
							transform2.gameObject.CustomSetActive(false);
							selSkillCell.CustomSetActive(selSkillCell.activeSelf);
						}
						else
						{
							selSkillCell.CustomSetActive(false);
							transform2.gameObject.CustomSetActive(true);
							int num2 = 0;
							while (num2 < 3 && num2 < memberInfo.recentUsedHero.astHeroInfo.Length)
							{
								Transform transform3 = transform2.transform.FindChild(string.Format("Element{0}", num2));
								if (transform3 != null && !CLadderSystem.IsRecentUsedHeroMaskSet(ref memberInfo.recentUsedHero.dwCtrlMask, 1) && (long)num2 < (long)((ulong)memberInfo.recentUsedHero.dwHeroNum) && memberInfo.recentUsedHero.astHeroInfo[num2].dwHeroID != 0u)
								{
									CUICommonSystem.SetObjActive(transform3.transform.Find("imageIcon"), true);
									IHeroData data = CHeroDataFactory.CreateHeroData(memberInfo.recentUsedHero.astHeroInfo[num2].dwHeroID);
									CUICommonSystem.SetHeroItemData(listScript.m_belongedFormScript, transform3.gameObject, data, enHeroHeadType.enBustCircle, false, true);
								}
								else
								{
									CUICommonSystem.SetObjActive(transform3.transform.Find("imageIcon"), false);
								}
								num2++;
							}
						}
					}
				}
				else
				{
					Transform transform4 = transform.Find("RecentUseHeroPanel");
					if (transform4 != null)
					{
						transform4.gameObject.CustomSetActive(false);
					}
				}
			}
		}

		private void InitFullHeroListData(ListView<IHeroData> sourceList)
		{
			this.m_canUseHeroListByJob.Clear();
			for (int i = 0; i < sourceList.get_Count(); i++)
			{
				if (this.m_heroSelectJobType == enHeroJobType.All || sourceList.get_Item(i).heroCfgInfo.bMainJob == (byte)this.m_heroSelectJobType || sourceList.get_Item(i).heroCfgInfo.bMinorJob == (byte)this.m_heroSelectJobType)
				{
					this.m_canUseHeroListByJob.Add(sourceList.get_Item(i));
				}
			}
			CHeroOverviewSystem.SortHeroList(ref this.m_canUseHeroListByJob, Singleton<CHeroSelectBaseSystem>.get_instance().m_sortType, false);
		}

		public void InitHeroList(CUIFormScript form, bool isResetSelect = false)
		{
			CUIListScript component = form.transform.Find("PanelCenter/ListHostHeroInfo").GetComponent<CUIListScript>();
			if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enBan)
			{
				this.InitFullHeroListData(this.m_banHeroList);
				component.SetElementAmount(this.m_canUseHeroListByJob.get_Count());
			}
			else if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enPick)
			{
				this.InitFullHeroListData(Singleton<CHeroSelectBaseSystem>.get_instance().m_canUseHeroList);
				component.SetElementAmount(this.m_canUseHeroListByJob.get_Count());
			}
			else
			{
				component.gameObject.CustomSetActive(false);
			}
			Button component2 = form.transform.Find("PanelCenter/ListHostHeroInfo/btnConfirmSelectHero").GetComponent<Button>();
			MemberInfo masterMemberInfo = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetMasterMemberInfo();
			if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep != enBanPickStep.enSwap && masterMemberInfo.camp != null)
			{
				if (masterMemberInfo == null)
				{
					return;
				}
				if (Singleton<CHeroSelectBaseSystem>.get_instance().IsCurBanOrPickMember(masterMemberInfo) && Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enBan && this.m_selectBanHeroData != null)
				{
					CUICommonSystem.SetButtonEnableWithShader(component2, true, true);
				}
				else if (Singleton<CHeroSelectBaseSystem>.get_instance().IsCurBanOrPickMember(masterMemberInfo) && Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enPick && Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Item(0) != 0u)
				{
					CUICommonSystem.SetButtonEnableWithShader(component2, true, true);
				}
				else
				{
					CUICommonSystem.SetButtonEnableWithShader(component2, false, true);
				}
				if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enBan)
				{
					CUICommonSystem.SetButtonName(component2.gameObject, Singleton<CTextManager>.get_instance().GetText("BP_SureButton_1"));
				}
				else if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enPick)
				{
					CUICommonSystem.SetButtonName(component2.gameObject, Singleton<CTextManager>.get_instance().GetText("BP_SureButton_2"));
				}
			}
			else
			{
				CUICommonSystem.SetButtonEnableWithShader(component2, false, true);
			}
			if (isResetSelect)
			{
				component.SelectElement(-1, true);
			}
		}

		public void InitSkinList(CUIFormScript form, uint customHeroID = 0u)
		{
			if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enBan)
			{
				return;
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetMasterMemberInfo() == null)
			{
				return;
			}
			uint num = Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Item(0);
			if (customHeroID != 0u)
			{
				num = customHeroID;
			}
			ListView<ResHeroSkin> listView = new ListView<ResHeroSkin>();
			ListView<ResHeroSkin> listView2 = new ListView<ResHeroSkin>();
			int index = -1;
			if (num != 0u)
			{
				ListView<ResHeroSkin> availableSkinByHeroId = CSkinInfo.GetAvailableSkinByHeroId(num);
				for (int i = 0; i < availableSkinByHeroId.get_Count(); i++)
				{
					ResHeroSkin resHeroSkin = availableSkinByHeroId.get_Item(i);
					if (masterRoleInfo.IsCanUseSkin(num, resHeroSkin.dwSkinID) || CBagSystem.CanUseSkinExpCard(resHeroSkin.dwID))
					{
						listView.Add(resHeroSkin);
					}
					else
					{
						listView2.Add(resHeroSkin);
					}
					if (masterRoleInfo.GetHeroWearSkinId(num) == resHeroSkin.dwSkinID)
					{
						index = listView.get_Count() - 1;
					}
				}
				listView.AddRange(listView2);
			}
			Transform transform = form.gameObject.transform.Find("PanelCenter/ListHostSkinInfo");
			Transform transform2 = form.gameObject.transform.Find("PanelCenter/ListHostSkinInfo/panelEffect");
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
					bool flag = masterRoleInfo.IsValidExperienceSkin(num, resHeroSkin.dwSkinID);
					gameObject.CustomSetActive(flag);
					bool flag2 = !masterRoleInfo.IsCanUseSkin(num, resHeroSkin.dwSkinID) && CBagSystem.CanUseSkinExpCard(resHeroSkin.dwID);
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
					if (masterRoleInfo.IsCanUseSkin(num, resHeroSkin.dwSkinID) || CBagSystem.CanUseSkinExpCard(resHeroSkin.dwID))
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
					component4.SetUIEvent(enUIEventType.Click, enUIEventID.HeroSelect_BanPick_SkinSelect, new stUIEventParams
					{
						tagUInt = resHeroSkin.dwSkinID,
						commonBool = flag
					});
				}
				cUIListScript.SelectElement(index, true);
			}
			transform2.gameObject.CustomSetActive(false);
		}

		private void CenterHeroItemEnable(CUIEvent uiEvent)
		{
			CUIFormScript srcFormScript = uiEvent.m_srcFormScript;
			CUIListScript srcWidgetBelongedListScript = uiEvent.m_srcWidgetBelongedListScript;
			GameObject srcWidget = uiEvent.m_srcWidget;
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			int srcWidgetIndexInBelongedList = uiEvent.m_srcWidgetIndexInBelongedList;
			if (srcFormScript == null || srcWidgetBelongedListScript == null || srcWidget == null || masterRoleInfo == null || srcWidgetIndexInBelongedList < 0)
			{
				return;
			}
			IHeroData heroData = this.m_canUseHeroListByJob.get_Item(srcWidgetIndexInBelongedList);
			if (heroData == null)
			{
				return;
			}
			CUIListElementScript component = srcWidget.GetComponent<CUIListElementScript>();
			if (component == null)
			{
				return;
			}
			GameObject gameObject = srcWidget.transform.Find("heroItemCell").gameObject;
			GameObject gameObject2 = gameObject.transform.Find("TxtFree").gameObject;
			GameObject gameObject3 = gameObject.transform.Find("TxtCreditFree").gameObject;
			GameObject gameObject4 = gameObject.transform.Find("imgExperienceMark").gameObject;
			Transform transform = gameObject.transform.Find("expCardPanel");
			CUIEventScript component2 = gameObject.GetComponent<CUIEventScript>();
			CUIEventScript component3 = srcWidget.GetComponent<CUIEventScript>();
			gameObject2.CustomSetActive(false);
			gameObject3.CustomSetActive(false);
			gameObject4.CustomSetActive(false);
			transform.gameObject.CustomSetActive(false);
			component2.enabled = false;
			component3.enabled = false;
			if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enPick)
			{
				bool flag = masterRoleInfo.IsFreeHero(heroData.cfgID);
				bool flag2 = masterRoleInfo.IsCreditFreeHero(heroData.cfgID);
				gameObject2.CustomSetActive(flag && !flag2);
				gameObject3.CustomSetActive(flag2);
				if (masterRoleInfo.IsValidExperienceHero(heroData.cfgID))
				{
					gameObject4.CustomSetActive(true);
				}
				else
				{
					gameObject4.CustomSetActive(false);
				}
			}
			MemberInfo masterMemberInfo = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetMasterMemberInfo();
			if (masterMemberInfo == null)
			{
				return;
			}
			if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enBan || Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enPick)
			{
				if (Singleton<CHeroSelectBaseSystem>.get_instance().IsCurBanOrPickMember(masterMemberInfo))
				{
					if (Singleton<CHeroSelectBaseSystem>.get_instance().IsBanByHeroID(heroData.cfgID) || Singleton<CHeroSelectBaseSystem>.get_instance().IsHeroExist(heroData.cfgID))
					{
						CUICommonSystem.SetHeroItemData(srcFormScript, gameObject, heroData, enHeroHeadType.enIcon, true, true);
					}
					else
					{
						component2.enabled = true;
						component3.enabled = true;
						CUICommonSystem.SetHeroItemData(srcFormScript, gameObject, heroData, enHeroHeadType.enIcon, false, true);
					}
				}
				else
				{
					CUICommonSystem.SetHeroItemData(srcFormScript, gameObject, heroData, enHeroHeadType.enIcon, true, true);
				}
				if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enBan)
				{
					if (this.m_selectBanHeroData != null)
					{
						if (heroData.cfgID == this.m_selectBanHeroData.heroCfgInfo.dwCfgID)
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
				else if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enPick)
				{
					if (Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroCount > 0)
					{
						if (heroData.cfgID == Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Item(0))
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
				return;
			}
		}

		public void StartEndTimer(int totlaTimes)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectBanPickSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.transform.Find("Top/Timer/CountDown");
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
			Transform transform = srcFormScript.transform.Find("Top/Timer/CountDownMovie");
			CUITimerScript component = uiEvent.m_srcWidget.GetComponent<CUITimerScript>();
			if (component.GetCurrentTime() <= 6.1f && !transform.gameObject.activeSelf)
			{
				transform.gameObject.CustomSetActive(true);
				component.gameObject.CustomSetActive(false);
				Singleton<CSoundManager>.GetInstance().PostEvent("UI_daojishi", null);
				Singleton<CSoundManager>.GetInstance().PostEvent("Play_sys_ban_5", null);
			}
		}

		private void OnHeroJobMenuSelect(CUIEvent uiEvent)
		{
			CUIListScript component = uiEvent.m_srcWidget.GetComponent<CUIListScript>();
			int selectedIndex = component.GetSelectedIndex();
			this.m_heroSelectJobType = (enHeroJobType)selectedIndex;
			this.InitHeroList(uiEvent.m_srcFormScript, false);
		}

		private void ResetHeroSelectJobType()
		{
			this.m_heroSelectJobType = enHeroJobType.All;
		}

		private void HeroSelect_SelectHero(CUIEvent uiEvent)
		{
			Singleton<CSoundManager>.GetInstance().PostEvent("UI_BanPick_Swicth", null);
			MemberInfo masterMemberInfo = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetMasterMemberInfo();
			if (masterMemberInfo == null)
			{
				return;
			}
			int srcWidgetIndexInBelongedList = uiEvent.m_srcWidgetIndexInBelongedList;
			if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enSwap)
			{
				return;
			}
			if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enBan)
			{
				if (srcWidgetIndexInBelongedList >= 0 && srcWidgetIndexInBelongedList < this.m_canUseHeroListByJob.get_Count() && Singleton<CHeroSelectBaseSystem>.get_instance().IsCurBanOrPickMember(masterMemberInfo))
				{
					this.m_selectBanHeroData = this.m_canUseHeroListByJob.get_Item(srcWidgetIndexInBelongedList);
					this.RefreshCenter();
				}
				return;
			}
			if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enPick)
			{
				if (srcWidgetIndexInBelongedList < 0 || srcWidgetIndexInBelongedList >= this.m_canUseHeroListByJob.get_Count())
				{
					return;
				}
				IHeroData heroData = this.m_canUseHeroListByJob.get_Item(srcWidgetIndexInBelongedList);
				if (heroData == null)
				{
					return;
				}
				CHeroSelectBaseSystem.SendHeroSelectMsg(0, 0, heroData.cfgID);
			}
		}

		private void HeroSelect_ConfirmHeroSelect(CUIEvent uiEvent)
		{
			if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enBan)
			{
				if (this.m_selectBanHeroData != null)
				{
					CHeroSelectBaseSystem.SendBanHeroMsg(this.m_selectBanHeroData.cfgID);
				}
			}
			else if (Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enPick)
			{
				CHeroSelectBaseSystem.SendMuliPrepareToBattleMsg();
			}
		}

		private void HeroSelect_SwapHeroReq(CUIEvent uiEvent)
		{
			uint tagUInt = uiEvent.m_eventParams.tagUInt;
			CHeroSelectBaseSystem.SendSwapHeroMsg(tagUInt);
		}

		private void HeroSelect_SwapHeroAllow(CUIEvent uiEvent)
		{
			CHeroSelectBaseSystem.SendSwapAcceptHeroMsg(1);
		}

		private void HeroSelect_SwapHeroCanel(CUIEvent uiEvent)
		{
			if (Singleton<CHeroSelectBaseSystem>.get_instance().m_swapState == enSwapHeroState.enSwapAllow)
			{
				CHeroSelectBaseSystem.SendSwapAcceptHeroMsg(0);
			}
			else if (Singleton<CHeroSelectBaseSystem>.get_instance().m_swapState == enSwapHeroState.enReqing)
			{
				CHeroSelectBaseSystem.SendCanelSwapHeroMsg();
			}
		}

		private void HeroSelect_OnSkinSelect(CUIEvent uiEvent)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			uint num = Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Item(0);
			uint tagUInt = uiEvent.m_eventParams.tagUInt;
			bool commonBool = uiEvent.m_eventParams.commonBool;
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectBanPickSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.gameObject.transform.Find("PanelCenter/ListHostSkinInfo");
			Transform transform2 = form.gameObject.transform.Find("PanelCenter/ListHostSkinInfo/panelEffect/List");
			if (transform == null || transform2 == null)
			{
				return;
			}
			CUIListScript component = transform.GetComponent<CUIListScript>();
			if (masterRoleInfo.IsCanUseSkin(num, tagUInt))
			{
				this.InitSkinEffect(transform2.gameObject, num, tagUInt);
			}
			else
			{
				component.SelectElement(component.GetLastSelectedIndex(), true);
			}
			if (masterRoleInfo.IsCanUseSkin(num, tagUInt))
			{
				if (masterRoleInfo.GetHeroWearSkinId(num) != tagUInt)
				{
					CHeroInfoSystem2.ReqWearHeroSkin(num, tagUInt, true);
				}
			}
			else
			{
				CHeroSkinBuyManager.OpenBuyHeroSkinForm3D(num, tagUInt, false);
			}
		}

		private void InitSkinEffect(GameObject objList, uint heroID, uint skinID)
		{
			CSkinInfo.GetHeroSkinProp(heroID, skinID, ref CHeroSelectBaseSystem.s_propArr, ref CHeroSelectBaseSystem.s_propPctArr, ref CHeroSelectBaseSystem.s_propImgArr);
			CUICommonSystem.SetListProp(objList, ref CHeroSelectBaseSystem.s_propArr, ref CHeroSelectBaseSystem.s_propPctArr);
		}

		public void OnHeroSkinWearSuc(uint heroId, uint skinId)
		{
			this.RefreshCenter();
		}

		private void OnOpenSymbolProp(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectBanPickSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			uint num = Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Item(0);
			if (num == 0u)
			{
				return;
			}
			CHeroInfo cHeroInfo;
			if (masterRoleInfo.GetHeroInfo(num, out cHeroInfo, true))
			{
				this.OpenSymbolPropPanel(form, cHeroInfo.m_selectPageIndex);
			}
			else if (masterRoleInfo.IsFreeHero(num))
			{
				int freeHeroSymbolId = (int)masterRoleInfo.GetFreeHeroSymbolId(num);
				this.OpenSymbolPropPanel(form, freeHeroSymbolId);
			}
		}

		private void OpenSymbolPropPanel(CUIFormScript form, int pageIndex)
		{
			this.SetEffectNoteVisiable(false);
			GameObject gameObject = form.transform.Find(CHeroSelectBanPickSystem.s_symbolPropPanelPath).gameObject;
			GameObject gameObject2 = gameObject.gameObject.transform.Find("basePropPanel").gameObject;
			GameObject gameObject3 = gameObject2.transform.Find("List").gameObject;
			CSymbolSystem.RefreshSymbolPageProp(gameObject3, pageIndex, true);
			gameObject.gameObject.CustomSetActive(true);
		}

		private void OnCloseSymbolProp(CUIEvent uiEvent)
		{
			this.SetEffectNoteVisiable(true);
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectBanPickSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			GameObject gameObject = form.gameObject.transform.Find(CHeroSelectBanPickSystem.s_symbolPropPanelPath).gameObject;
			gameObject.gameObject.CustomSetActive(false);
		}

		public void OnSymbolPageDownBtnClick(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectBanPickSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.gameObject.transform.Find("Bottom/Panel_SymbolChange/DropList/List");
			this.SetEffectNoteVisiable(transform.gameObject.activeSelf);
			transform.gameObject.CustomSetActive(!transform.gameObject.activeSelf);
		}

		public void OnHeroSymbolPageSelect(CUIEvent uiEvent)
		{
			int selectedIndex = uiEvent.m_srcWidget.GetComponent<CUIListScript>().GetSelectedIndex();
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			uint num = Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Item(0);
			if (num == 0u)
			{
				return;
			}
			Transform transform = uiEvent.m_srcFormScript.gameObject.transform.Find("Bottom");
			Transform transform2 = transform.Find("Panel_SymbolChange/DropList/List");
			transform2.gameObject.CustomSetActive(false);
			CHeroInfo cHeroInfo;
			bool heroInfo = masterRoleInfo.GetHeroInfo(num, out cHeroInfo, true);
			if (heroInfo && selectedIndex != cHeroInfo.m_selectPageIndex)
			{
				CHeroSelectBaseSystem.SendHeroSelectSymbolPage(num, selectedIndex, false);
			}
			else if (!heroInfo && masterRoleInfo.IsFreeHero(num) && selectedIndex != (int)masterRoleInfo.GetFreeHeroSymbolId(num))
			{
				CHeroSelectBaseSystem.SendHeroSelectSymbolPage(num, selectedIndex, false);
			}
		}

		public void OnSymbolPageChange()
		{
			this.RefreshSymbolPage();
		}

		public void RefreshSymbolPage()
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CHeroSelectBanPickSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			uint num = Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Item(0);
			Transform transform = form.gameObject.transform.Find("Bottom/Panel_SymbolChange");
			transform.gameObject.CustomSetActive(false);
			if (Singleton<CFunctionUnlockSys>.GetInstance().FucIsUnlock(8) && Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep != enBanPickStep.enBan && GameDataMgr.heroDatabin.GetDataByKey(num) != null)
			{
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				int selectIndex = 0;
				CHeroInfo cHeroInfo;
				if (masterRoleInfo.GetHeroInfo(num, out cHeroInfo, true))
				{
					selectIndex = cHeroInfo.m_selectPageIndex;
				}
				else if (masterRoleInfo.IsFreeHero(num))
				{
					selectIndex = (int)masterRoleInfo.GetFreeHeroSymbolId(num);
				}
				transform.gameObject.CustomSetActive(true);
				CHeroSelectBanPickSystem.SetPageDropListDataByHeroSelect(transform.gameObject, selectIndex);
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

		public void RefreshAddedSkillItem()
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CHeroSelectBanPickSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			GameObject gameObject = form.transform.Find("Bottom/AddedSkillItem").gameObject;
			gameObject.CustomSetActive(false);
			uint num = Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Item(0);
			MemberInfo masterMemberInfo = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetMasterMemberInfo();
			if (!CAddSkillSys.IsSelSkillAvailable() || Singleton<CHeroSelectBaseSystem>.get_instance().m_banPickStep == enBanPickStep.enBan || num == 0u || masterMemberInfo == null)
			{
				return;
			}
			uint dwSelSkillID = masterMemberInfo.ChoiceHero[0].stBaseInfo.stCommonInfo.stSkill.dwSelSkillID;
			ResSkillCfgInfo dataByKey = GameDataMgr.skillDatabin.GetDataByKey(dwSelSkillID);
			bool flag = true;
			if (dataByKey == null)
			{
				DebugHelper.Assert(false, string.Format("ResSkillCfgInfo[{0}] can not be found!", dwSelSkillID));
				return;
			}
			gameObject.CustomSetActive(true);
			string prefabPath = string.Format("{0}{1}", CUIUtility.s_Sprite_Dynamic_Skill_Dir, Utility.UTF8Convert(dataByKey.szIconPath));
			Image component = gameObject.transform.Find("Icon").GetComponent<Image>();
			component.SetSprite(prefabPath, form, true, false, false, false);
			string skillDescLobby = CUICommonSystem.GetSkillDescLobby(dataByKey.szSkillDesc, num);
			if (flag)
			{
				form.transform.Find("PanelAddSkill/AddSkillTitletxt").GetComponent<Text>().text = dataByKey.szSkillName;
				form.transform.Find("PanelAddSkill/AddSkilltxt").GetComponent<Text>().text = skillDescLobby;
				form.transform.Find("PanelAddSkill/btnConfirm").GetComponent<CUIEventScript>().m_onClickEventParams.tag = (int)dwSelSkillID;
				ListView<ResSkillUnlock> selSkillAvailable = CAddSkillSys.GetSelSkillAvailable(Singleton<CHeroSelectBaseSystem>.get_instance().m_mapUnUseSkill);
				for (int i = 0; i < selSkillAvailable.get_Count(); i++)
				{
					if (selSkillAvailable.get_Item(i).dwUnlockSkillID == dwSelSkillID)
					{
						CUIToggleListScript component2 = form.transform.Find("PanelAddSkill/ToggleList").GetComponent<CUIToggleListScript>();
						component2.SelectElement(i, true);
						break;
					}
				}
			}
		}

		public void InitAddedSkillPanel()
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CHeroSelectBanPickSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
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
						component2.m_onClickEventID = enUIEventID.HeroSelect_BanPick_AddedSkillSelected;
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
			}
			form.transform.Find("Bottom/AddedSkillItem").gameObject.CustomSetActive(false);
			form.transform.Find("PanelAddSkill").gameObject.CustomSetActive(false);
		}

		public void OnSelectedAddedSkill(CUIEvent uiEvent)
		{
			uint num = Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Item(0);
			if (num == 0u)
			{
				return;
			}
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CHeroSelectBanPickSystem.s_heroSelectFormPath);
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
			string skillDescLobby = CUICommonSystem.GetSkillDescLobby(dataByKey.szSkillDesc, num);
			form.transform.Find("PanelAddSkill/AddSkillTitletxt").GetComponent<Text>().text = dataByKey.szSkillName;
			form.transform.Find("PanelAddSkill/AddSkilltxt").GetComponent<Text>().text = skillDescLobby;
		}

		public void OnConfirmAddedSkill(CUIEvent uiEvent)
		{
			uint num = Singleton<CHeroSelectBaseSystem>.get_instance().m_selectHeroIDList.get_Item(0);
			uint tag = (uint)uiEvent.m_eventParams.tag;
			if (num == 0u || Singleton<CHeroSelectBaseSystem>.get_instance().m_mapUnUseSkill == null || !CAddSkillSys.IsSelSkillAvailable(Singleton<CHeroSelectBaseSystem>.get_instance().m_mapUnUseSkill, tag))
			{
				DebugHelper.Assert(false, string.Format("CHeroSelectBanPickSystem heroID[{0}] addedSkillID[{1}]", num, tag));
			}
			else
			{
				CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1166u);
				cSPkg.stPkgData.get_stUnlockSkillSelReq().dwHeroID = num;
				cSPkg.stPkgData.get_stUnlockSkillSelReq().dwSkillID = tag;
				Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
			}
			this.OnCloseAddedSkillPanel(null);
		}

		public void OnOpenAddedSkillPanel(CUIEvent uiEvent)
		{
			this.SetEffectNoteVisiable(false);
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CHeroSelectBanPickSystem.s_heroSelectFormPath);
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
			this.SetEffectNoteVisiable(true);
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CHeroSelectBanPickSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			form.transform.Find("PanelAddSkill").gameObject.CustomSetActive(false);
			if (Singleton<CHeroSelectBaseSystem>.get_instance().IsMultilMode())
			{
				Singleton<CChatController>.get_instance().Set_Show_Bottom(true);
				Singleton<CChatController>.get_instance().SetEntryNodeVoiceBtnShowable(true);
			}
		}

		private void SetEffectNoteVisiable(bool isShow)
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CHeroSelectBanPickSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			CUIListScript component = form.transform.Find("PanelLeft/TeamHeroInfo").GetComponent<CUIListScript>();
			CUIListScript component2 = form.transform.Find("PanelRight/TeamHeroInfo").GetComponent<CUIListScript>();
			CUIListScript[] array = new CUIListScript[]
			{
				component,
				component2
			};
			for (int i = 0; i < array.Length; i++)
			{
				CUIListScript cUIListScript = array[i];
				int elementAmount = cUIListScript.GetElementAmount();
				for (int j = 0; j < elementAmount; j++)
				{
					Transform transform = cUIListScript.GetElemenet(j).transform;
					CUICommonSystem.SetObjActive(transform.Find("BgState/CurrentBg/UI_BR_effect"), isShow);
				}
			}
		}

		public void PlayStepTitleAnimation()
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CHeroSelectBanPickSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			CUICommonSystem.PlayAnimation(form.transform.Find("Top/Tips"), null);
		}

		public void PlayCurrentBgAnimation()
		{
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CHeroSelectBanPickSystem.s_heroSelectFormPath);
			if (form == null)
			{
				return;
			}
			CSDT_BAN_PICK_STATE_INFO stCurState = Singleton<CHeroSelectBaseSystem>.get_instance().m_curBanPickInfo.stCurState;
			for (int i = 0; i < (int)stCurState.bPosNum; i++)
			{
				MemberInfo memberInfo = Singleton<CHeroSelectBaseSystem>.get_instance().roomInfo.GetMemberInfo(stCurState.bCamp, (int)stCurState.szPosList[i]);
				if (memberInfo != null)
				{
					Transform teamPlayerElement = Singleton<CHeroSelectBaseSystem>.get_instance().GetTeamPlayerElement(memberInfo.ullUid, memberInfo.camp);
					if (teamPlayerElement == null)
					{
						return;
					}
					CUICommonSystem.PlayAnimation(teamPlayerElement.Find("BgState/CurrentBg"), null);
				}
			}
		}
	}
}
