using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.Sound;
using Assets.Scripts.UI;
using CSProtocol;
using ResData;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	[MessageHandlerClass]
	public class CSymbolWearController
	{
		public static string s_symbolEquipPanel = "SymbolEquip/Panel_SymbolEquip";

		public static string s_symbolBagPanel = "SymbolEquip/Panel_SymbolEquip/Panel_SymbolBag";

		public static string s_symbolPagePanel = "SymbolEquip/Panel_SymbolEquip/Panel_SymbolPageRect/Panel_SymbolPage";

		private int m_symbolPageIndex;

		private int m_selectSymbolPos = -1;

		public static int s_maxSameIDSymbolEquipNum = 10;

		private CSymbolItem m_curViewSymbol;

		private ListView<CSymbolItem> m_pageSymbolBagList = new ListView<CSymbolItem>();

		private CSymbolSystem m_symbolSys;

		public static readonly Vector2 s_symbolPos1 = new Vector2(25f, -1f);

		public static readonly Vector2 s_symbolPos2 = new Vector2(0f, -1f);

		public void Init(CSymbolSystem symbolSys)
		{
			this.m_symbolSys = symbolSys;
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_Off, new CUIEventManager.OnUIEventHandler(this.OnOffSymbol));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_View, new CUIEventManager.OnUIEventHandler(this.OnSymbolView));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_BagItemEnable, new CUIEventManager.OnUIEventHandler(this.OnSymbolBagElementEnable));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_BagItemClick, new CUIEventManager.OnUIEventHandler(this.OnSymbolBagItemClick));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_BagViewHide, new CUIEventManager.OnUIEventHandler(this.OnSymbolBagSymbolViewHide));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_BagShow, new CUIEventManager.OnUIEventHandler(this.OnSymbolBagShow));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_View_NotWear_Item, new CUIEventManager.OnUIEventHandler(this.OnViewNotWearItem));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_ChangeSymbolClick, new CUIEventManager.OnUIEventHandler(this.OnChangeSymbolClick));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_ChangeConfirm, new CUIEventManager.OnUIEventHandler(this.OnSymbolChangeConfirm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_PageClear, new CUIEventManager.OnUIEventHandler(this.OnSymbolPageClear));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_PageClearConfirm, new CUIEventManager.OnUIEventHandler(this.OnSymbolPageClearConfirm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_OpenUnlockLvlTip, new CUIEventManager.OnUIEventHandler(this.OnOpenUnlockLvlTip));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_PromptBuyGrid, new CUIEventManager.OnUIEventHandler(this.OnPromptBuyGrid));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_ConfirmBuyGrid, new CUIEventManager.OnUIEventHandler(this.OnConfirmBuyGrid));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_ConfirmWhenMoneyNotEnough, new CUIEventManager.OnUIEventHandler(this.ConfirmWhenMoneyNotEnough));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_SymbolPageDownBtnClick, new CUIEventManager.OnUIEventHandler(this.OnSymbolPageDownBtnClick));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_PageItemSelect, new CUIEventManager.OnUIEventHandler(this.OnSymbolPageItemSelect));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_ChangePageName, new CUIEventManager.OnUIEventHandler(this.OnChangeSymbolPageName));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_PageAddClick, new CUIEventManager.OnUIEventHandler(this.OnSymbol_PageAddClick));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Symbol_ConfirmChgPageName, new CUIEventManager.OnUIEventHandler(this.OnConfirmChgPageName));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Purchase_BuySymbolPage, new CUIEventManager.OnUIEventHandler(this.OnBuySymbolPage));
			Singleton<EventRouter>.get_instance().AddEventHandler("MasterPvpLevelChanged", new Action(this.RefreshPageDropList));
		}

		public void UnInit()
		{
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Symbol_Off, new CUIEventManager.OnUIEventHandler(this.OnOffSymbol));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Symbol_View, new CUIEventManager.OnUIEventHandler(this.OnSymbolView));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Symbol_BagItemEnable, new CUIEventManager.OnUIEventHandler(this.OnSymbolBagElementEnable));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Symbol_BagItemClick, new CUIEventManager.OnUIEventHandler(this.OnSymbolBagItemClick));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Symbol_BagViewHide, new CUIEventManager.OnUIEventHandler(this.OnSymbolBagSymbolViewHide));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Symbol_BagShow, new CUIEventManager.OnUIEventHandler(this.OnSymbolBagShow));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Symbol_View_NotWear_Item, new CUIEventManager.OnUIEventHandler(this.OnViewNotWearItem));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Symbol_ChangeSymbolClick, new CUIEventManager.OnUIEventHandler(this.OnChangeSymbolClick));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Symbol_ChangeConfirm, new CUIEventManager.OnUIEventHandler(this.OnSymbolChangeConfirm));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Symbol_PageClear, new CUIEventManager.OnUIEventHandler(this.OnSymbolPageClear));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Symbol_PageClearConfirm, new CUIEventManager.OnUIEventHandler(this.OnSymbolPageClearConfirm));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Symbol_OpenUnlockLvlTip, new CUIEventManager.OnUIEventHandler(this.OnOpenUnlockLvlTip));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Symbol_PromptBuyGrid, new CUIEventManager.OnUIEventHandler(this.OnPromptBuyGrid));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Symbol_ConfirmBuyGrid, new CUIEventManager.OnUIEventHandler(this.OnConfirmBuyGrid));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Symbol_ConfirmWhenMoneyNotEnough, new CUIEventManager.OnUIEventHandler(this.ConfirmWhenMoneyNotEnough));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Symbol_SymbolPageDownBtnClick, new CUIEventManager.OnUIEventHandler(this.OnSymbolPageDownBtnClick));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Symbol_PageItemSelect, new CUIEventManager.OnUIEventHandler(this.OnSymbolPageItemSelect));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Symbol_ChangePageName, new CUIEventManager.OnUIEventHandler(this.OnChangeSymbolPageName));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Symbol_PageAddClick, new CUIEventManager.OnUIEventHandler(this.OnSymbol_PageAddClick));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Symbol_ConfirmChgPageName, new CUIEventManager.OnUIEventHandler(this.OnConfirmChgPageName));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Purchase_BuySymbolPage, new CUIEventManager.OnUIEventHandler(this.OnBuySymbolPage));
			Singleton<EventRouter>.get_instance().RemoveEventHandler("MasterPvpLevelChanged", new Action(this.RefreshPageDropList));
		}

		public void Clear()
		{
			this.ClearSymbolEquipData();
		}

		private void ClearSymbolEquipData()
		{
			this.m_selectSymbolPos = -1;
		}

		public void Load(CUIFormScript form)
		{
			CUICommonSystem.LoadUIPrefab(CSymbolSystem.s_symbolEquipModulePath, CSymbolSystem.s_symbolEquipPanel, form.GetWidget(5), form);
		}

		public bool Loaded(CUIFormScript form)
		{
			GameObject x = Utility.FindChild(form.GetWidget(5), CSymbolSystem.s_symbolEquipPanel);
			return !(x == null);
		}

		public void SwitchToSymbolWearPanel(CUIFormScript form)
		{
			if (form == null || form.IsClosed())
			{
				return;
			}
			Transform transform = form.transform.Find(CSymbolWearController.s_symbolEquipPanel);
			if (transform == null)
			{
				return;
			}
			CUIComponent component = transform.GetComponent<CUIComponent>();
			if (component == null)
			{
				return;
			}
			this.SetSymbolItemSelect(this.m_selectSymbolPos, false);
			this.ClearSymbolEquipData();
			this.RefreshSymbolEquipPanel();
			this.RefreshPageDropList();
			CTextManager instance = Singleton<CTextManager>.GetInstance();
			GameObject gameObject = component.GetWidget(2).transform.Find("titlePanel").gameObject;
			CSymbolWearController.AddTip(gameObject, instance.GetText("Symbol_PvpProp_Desc"), enUseableTipsPos.enLeft);
			GameObject gameObject2 = component.GetWidget(3).transform.Find("titlePanel").gameObject;
			CSymbolWearController.AddTip(gameObject2, instance.GetText("Symbol_EnhancePveProp_Desc"), enUseableTipsPos.enLeft);
		}

		public static void AddTip(GameObject target, string tip, enUseableTipsPos pos)
		{
			if (null == target)
			{
				return;
			}
			stUIEventParams eventParams = default(stUIEventParams);
			eventParams.tagStr = tip;
			eventParams.tag = (int)pos;
			CUIEventScript component = target.GetComponent<CUIEventScript>();
			if (component != null)
			{
				component.SetUIEvent(enUIEventType.Down, enUIEventID.Tips_CommonInfoOpen, eventParams);
				component.SetUIEvent(enUIEventType.HoldEnd, enUIEventID.Tips_CommonInfoClose, eventParams);
				component.SetUIEvent(enUIEventType.Click, enUIEventID.Tips_CommonInfoClose, eventParams);
				component.SetUIEvent(enUIEventType.DragEnd, enUIEventID.Tips_CommonInfoClose, eventParams);
			}
		}

		public void RefreshSymbolEquipPanel()
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CSymbolSystem.s_symbolFormPath);
			if (form == null)
			{
				return;
			}
			this.m_symbolSys.SetSymbolData();
			this.RefreshSymbolPage();
			this.RefreshPageDropList();
			this.RefreshSymbolProp();
			this.OnSymbolBagSymbolViewHide(null);
		}

		private void RefreshSymbolProp()
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CSymbolSystem.s_symbolFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.transform.Find(CSymbolWearController.s_symbolEquipPanel);
			if (transform != null)
			{
				CUIComponent component = transform.GetComponent<CUIComponent>();
				if (component != null)
				{
					GameObject widget = component.GetWidget(2);
					GameObject gameObject = widget.transform.Find("propList").gameObject;
					CSymbolSystem.RefreshSymbolPageProp(gameObject, this.m_symbolPageIndex, true);
					GameObject widget2 = component.GetWidget(3);
					GameObject gameObject2 = widget2.transform.Find("propList").gameObject;
					CSymbolSystem.RefreshSymbolPagePveEnhanceProp(gameObject2, this.m_symbolPageIndex);
				}
			}
		}

		private void RefreshPageDropList()
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CSymbolSystem.s_symbolFormPath);
			if (form == null)
			{
				return;
			}
			this.SetPageDropListData(form, this.m_symbolPageIndex);
		}

		public void SetPageDropListData(CUIFormScript form, int selectIndex)
		{
			GameObject widget = form.GetWidget(1);
			Transform transform = widget.transform.Find("DropList/List");
			transform.gameObject.CustomSetActive(false);
			CUIListScript component = transform.GetComponent<CUIListScript>();
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			bool flag = masterRoleInfo.m_symbolInfo.IsPageFull();
			int nextFreePageLevel = masterRoleInfo.m_symbolInfo.GetNextFreePageLevel();
			component.SetElementAmount(((long)nextFreePageLevel <= (long)((ulong)masterRoleInfo.PvpLevel)) ? masterRoleInfo.m_symbolInfo.m_pageCount : (masterRoleInfo.m_symbolInfo.m_pageCount + 1));
			for (int i = 0; i < masterRoleInfo.m_symbolInfo.m_pageCount; i++)
			{
				CUIListElementScript elemenet = component.GetElemenet(i);
				if (elemenet != null)
				{
					Text component2 = elemenet.gameObject.transform.Find("Text").GetComponent<Text>();
					component2.text = masterRoleInfo.m_symbolInfo.GetSymbolPageName(i);
					RectTransform component3 = component2.GetComponent<RectTransform>();
					component3.anchoredPosition = CSymbolWearController.s_symbolPos1;
					Text component4 = elemenet.gameObject.transform.Find("SymbolLevel/Text").GetComponent<Text>();
					component4.text = masterRoleInfo.m_symbolInfo.GetSymbolPageMaxLvl(i).ToString();
					elemenet.gameObject.transform.Find("SymbolLevel").gameObject.CustomSetActive(true);
				}
			}
			if ((long)nextFreePageLevel > (long)((ulong)masterRoleInfo.PvpLevel))
			{
				CUIListElementScript elemenet2 = component.GetElemenet(masterRoleInfo.m_symbolInfo.m_pageCount);
				if (elemenet2 != null)
				{
					Text component5 = elemenet2.gameObject.transform.Find("Text").GetComponent<Text>();
					component5.text = string.Format(Singleton<CTextManager>.GetInstance().GetText("Symbol_Free_GetPage"), nextFreePageLevel);
					RectTransform component6 = component5.GetComponent<RectTransform>();
					component6.anchoredPosition = CSymbolWearController.s_symbolPos2;
					elemenet2.gameObject.transform.Find("SymbolLevel").gameObject.CustomSetActive(false);
				}
			}
			component.SelectElement(selectIndex, true);
			if (flag)
			{
				form.GetWidget(2).CustomSetActive(false);
			}
			Text component7 = widget.transform.Find("DropList/Button_Down/Text").GetComponent<Text>();
			component7.text = masterRoleInfo.m_symbolInfo.GetSymbolPageName(this.m_symbolPageIndex);
			Text component8 = widget.transform.Find("DropList/Button_Down/SymbolLevel/Text").GetComponent<Text>();
			component8.text = masterRoleInfo.m_symbolInfo.GetSymbolPageMaxLvl(selectIndex).ToString();
		}

		private void SetSymbolItem(CUIFormScript formScript, GameObject item, int i, ulong objId)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CSymbolSystem.s_symbolFormPath);
			if (form == null || item == null)
			{
				return;
			}
			CUseable symbolByObjID = Singleton<CSymbolSystem>.GetInstance().GetSymbolByObjID(objId);
			CUIEventScript component = item.GetComponent<CUIEventScript>();
			Transform transform = item.transform;
			GameObject gameObject = transform.Find("bg").gameObject;
			GameObject gameObject2 = transform.Find("imgLock").gameObject;
			GameObject gameObject3 = transform.Find("lblOpenLevel").gameObject;
			GameObject gameObject4 = transform.Find("imgIcon").gameObject;
			GameObject gameObject5 = transform.Find("imgLevel").gameObject;
			GameObject gameObject6 = transform.Find("imgCanBuy").gameObject;
			Text component2 = gameObject3.transform.Find("Level").GetComponent<Text>();
			gameObject.CustomSetActive(true);
			gameObject2.CustomSetActive(false);
			gameObject3.CustomSetActive(false);
			gameObject4.CustomSetActive(false);
			gameObject5.CustomSetActive(false);
			gameObject6.CustomSetActive(false);
			int tag = 0;
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			enSymbolWearState symbolPosWearState = masterRoleInfo.m_symbolInfo.GetSymbolPosWearState(this.m_symbolPageIndex, i, out tag);
			stUIEventParams eventParams = default(stUIEventParams);
			switch (symbolPosWearState)
			{
			case enSymbolWearState.WearSuccess:
				if (symbolByObjID != null)
				{
					gameObject4.GetComponent<Image>().SetSprite(symbolByObjID.GetIconPath(), formScript, false, false, false, false);
					gameObject4.CustomSetActive(true);
					eventParams.symbolParam.symbol = (CSymbolItem)symbolByObjID;
					eventParams.symbolParam.page = this.m_symbolPageIndex;
					eventParams.symbolParam.pos = i;
					component.SetUIEvent(enUIEventType.Click, enUIEventID.Symbol_View, eventParams);
				}
				break;
			case enSymbolWearState.OpenToWear:
				eventParams.symbolParam.symbol = (CSymbolItem)symbolByObjID;
				eventParams.symbolParam.page = this.m_symbolPageIndex;
				eventParams.symbolParam.pos = i;
				component.SetUIEvent(enUIEventType.Click, enUIEventID.Symbol_BagShow, eventParams);
				break;
			case enSymbolWearState.WillOpen:
				component2.text = "Lv." + tag.ToString();
				gameObject3.CustomSetActive(true);
				eventParams.tag = tag;
				component.SetUIEvent(enUIEventType.Click, enUIEventID.Symbol_OpenUnlockLvlTip, eventParams);
				break;
			case enSymbolWearState.UnOpen:
				gameObject2.CustomSetActive(true);
				gameObject.CustomSetActive(false);
				eventParams.tag = tag;
				component.SetUIEvent(enUIEventType.Click, enUIEventID.Symbol_OpenUnlockLvlTip, eventParams);
				break;
			case enSymbolWearState.CanBuy:
				gameObject.CustomSetActive(false);
				gameObject6.CustomSetActive(true);
				eventParams.tag = i + 1;
				component.SetUIEvent(enUIEventType.Click, enUIEventID.Symbol_PromptBuyGrid, eventParams);
				break;
			case enSymbolWearState.CanBuyAndWillOpen:
				gameObject.CustomSetActive(false);
				component2.text = "Lv." + tag.ToString();
				gameObject3.CustomSetActive(true);
				eventParams.tag = i + 1;
				component.SetUIEvent(enUIEventType.Click, enUIEventID.Symbol_PromptBuyGrid, eventParams);
				break;
			}
		}

		private void RefreshSymbolPage()
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CSymbolSystem.s_symbolFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.gameObject.transform.Find(CSymbolWearController.s_symbolPagePanel);
			if (transform == null)
			{
				return;
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo == null)
			{
				return;
			}
			ulong[] pageSymbolData = masterRoleInfo.m_symbolInfo.GetPageSymbolData(this.m_symbolPageIndex);
			if (pageSymbolData == null)
			{
				return;
			}
			for (int i = 0; i < 30; i++)
			{
				Transform transform2 = transform.Find(string.Format("itemCell{0}", i));
				if (transform2 != null)
				{
					this.SetSymbolItem(form, transform2.gameObject, i, pageSymbolData[i]);
				}
			}
			Transform transform3 = form.transform.Find(CSymbolWearController.s_symbolEquipPanel);
			if (transform3 != null)
			{
				CUIComponent component = transform3.GetComponent<CUIComponent>();
				if (component != null)
				{
					Text component2 = component.GetWidget(10).GetComponent<Text>();
					component2.text = masterRoleInfo.m_symbolInfo.GetSymbolPageMaxLvl(this.m_symbolPageIndex).ToString();
				}
			}
		}

		private void OnSymbolPageItemSelect(CUIEvent uiEvent)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo == null)
			{
				DebugHelper.Assert(false, "OnSymbolPageItemSelect role is null");
				return;
			}
			CUIListScript component = uiEvent.m_srcWidget.GetComponent<CUIListScript>();
			int elementAmount = component.GetElementAmount();
			int selectedIndex = component.GetSelectedIndex();
			GameObject widget = uiEvent.m_srcFormScript.GetWidget(1);
			Transform transform = widget.transform.Find("DropList/List");
			transform.gameObject.CustomSetActive(false);
			if (elementAmount == masterRoleInfo.m_symbolInfo.m_pageCount || (elementAmount > masterRoleInfo.m_symbolInfo.m_pageCount && selectedIndex != elementAmount - 1))
			{
				this.m_symbolPageIndex = selectedIndex;
				this.OnSymbolPageChange();
				Text component2 = widget.transform.Find("DropList/Button_Down/Text").GetComponent<Text>();
				component2.text = masterRoleInfo.m_symbolInfo.GetSymbolPageName(this.m_symbolPageIndex);
				Text component3 = widget.transform.Find("DropList/Button_Down/SymbolLevel/Text").GetComponent<Text>();
				component3.text = masterRoleInfo.m_symbolInfo.GetSymbolPageMaxLvl(this.m_symbolPageIndex).ToString();
			}
		}

		public void OnSymbolPageChange()
		{
			this.RefreshSymbolPage();
			this.RefreshSymbolProp();
			this.OnSymbolBagSymbolViewHide(null);
		}

		public void OnSymbolEquip(bool bMoveNext)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CSymbolSystem.s_symbolFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.transform.Find(CSymbolWearController.s_symbolEquipPanel);
			if (transform != null)
			{
				CUIComponent component = transform.GetComponent<CUIComponent>();
				if (component != null)
				{
					component.GetWidget(5).CustomSetActive(false);
					component.GetWidget(6).CustomSetActive(true);
					this.RefreshSymbolPage();
					this.RefreshSymbolProp();
					this.RefreshPageDropList();
					if (this.m_selectSymbolPos >= 0)
					{
						if (!bMoveNext || !this.MovePosToNextCanEquipPos())
						{
							this.OnSymbolBagSymbolViewHide(null);
						}
					}
					else
					{
						Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Symbol_View_NotWear_Item);
					}
				}
			}
		}

		public void OnSymbolEquipOff(bool bClear)
		{
			if (Singleton<CSymbolSystem>.GetInstance().m_selectMenuType == enSymbolMenuType.SymbolEquip)
			{
				this.RefreshSymbolPage();
				this.RefreshSymbolProp();
				this.RefreshPageDropList();
				if (bClear)
				{
					this.m_selectSymbolPos = -1;
				}
				else
				{
					stUIEventParams par = default(stUIEventParams);
					par.symbolParam.symbol = null;
					par.symbolParam.page = this.m_symbolPageIndex;
					par.symbolParam.pos = this.m_selectSymbolPos;
					Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Symbol_BagShow, par);
				}
			}
		}

		private bool MovePosToNextCanEquipPos()
		{
			bool result = false;
			ListView<CSymbolItem> allSymbolList = this.m_symbolSys.GetAllSymbolList();
			int nextCanEquipPos = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo().m_symbolInfo.GetNextCanEquipPos(this.m_symbolPageIndex, this.m_selectSymbolPos, ref allSymbolList);
			if (nextCanEquipPos != -1)
			{
				stUIEventParams par = default(stUIEventParams);
				par.symbolParam.symbol = null;
				par.symbolParam.page = this.m_symbolPageIndex;
				par.symbolParam.pos = nextCanEquipPos;
				Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Symbol_BagShow, par);
				result = true;
			}
			return result;
		}

		private void OnSymbol_PageAddClick(CUIEvent uiEvent)
		{
			CSymbolWearController.OnBuyNewSymbolPage();
		}

		private void OnConfirmChgPageName(CUIEvent uiEvent)
		{
			string text = uiEvent.m_srcFormScript.gameObject.transform.Find("Panel/inputText/Text").GetComponent<Text>().text;
			if (string.IsNullOrEmpty(text) || text.get_Length() > 6)
			{
				string text2 = Singleton<CTextManager>.GetInstance().GetText("Symbol_Name_LenError");
				Singleton<CUIManager>.GetInstance().OpenTips(text2, false, 1.5f, null, new object[0]);
				return;
			}
			CSymbolWearController.SendChgSymbolPageName(this.m_symbolPageIndex, text);
		}

		public static void OnBuyNewSymbolPage()
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo.m_symbolInfo.IsPageFull())
			{
				string text = Singleton<CTextManager>.GetInstance().GetText("Symbol_Page_Buy_PageFull");
				Singleton<CUIManager>.GetInstance().OpenTips(text, false, 1.5f, null, new object[0]);
				return;
			}
			RES_SHOPBUY_COINTYPE resShopBuyType = 4;
			uint payValue = 0u;
			masterRoleInfo.m_symbolInfo.GetNewPageCost(out resShopBuyType, out payValue);
			string goodName = "新的铭文页";
			stUIEventParams stUIEventParams = default(stUIEventParams);
			CMallSystem.TryToPay(enPayPurpose.Buy, goodName, CMallSystem.ResBuyTypeToPayType(resShopBuyType), payValue, enUIEventID.Purchase_BuySymbolPage, ref stUIEventParams, enUIEventID.None, true, true, false);
		}

		public void OnSymbolPageDownBtnClick(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CSymbolSystem.s_symbolFormPath);
			if (form == null)
			{
				return;
			}
			GameObject widget = form.GetWidget(1);
			Transform transform = widget.transform.Find("DropList/List");
			transform.gameObject.CustomSetActive(!transform.gameObject.activeSelf);
		}

		public void OnBuySymbolPage(CUIEvent uiEvent)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			CSymbolWearController.SendBuySymbol(masterRoleInfo.m_symbolInfo.m_pageBuyCnt + 1);
		}

		private void OnChangeSymbolPageName(CUIEvent uiEvent)
		{
			string text = Singleton<CTextManager>.GetInstance().GetText("Symbol_ChangeName");
			Singleton<CUIManager>.GetInstance().OpenInputBox(text, string.Empty, enUIEventID.Symbol_ConfirmChgPageName);
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm("UGUI/Form/Common/Form_InputBox.prefab");
			if (form != null)
			{
				Transform transform = form.transform.Find("Panel/inputText");
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				if (transform != null && masterRoleInfo != null)
				{
					string symbolPageName = masterRoleInfo.m_symbolInfo.GetSymbolPageName(this.m_symbolPageIndex);
					transform.GetComponent<InputField>().text = symbolPageName;
				}
			}
		}

		private void OnOffSymbol(CUIEvent uiEvent)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1135u);
			cSPkg.stPkgData.get_stSymbolOff().bPage = (byte)this.m_symbolPageIndex;
			cSPkg.stPkgData.get_stSymbolOff().bPos = (byte)this.m_selectSymbolPos;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		private void OnSymbolView(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CSymbolSystem.s_symbolFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.transform.Find(CSymbolWearController.s_symbolEquipPanel);
			if (transform != null)
			{
				CUIComponent component = transform.GetComponent<CUIComponent>();
				if (component != null)
				{
					component.GetWidget(1).CustomSetActive(true);
					component.GetWidget(0).CustomSetActive(false);
					this.SetSeletSymbolPos(uiEvent.m_eventParams.symbolParam.pos);
					this.ShowSymbolView(uiEvent);
				}
			}
		}

		private void SetSeletSymbolPos(int newPos)
		{
			this.SetSymbolItemSelect(this.m_selectSymbolPos, false);
			this.m_selectSymbolPos = newPos;
			this.SetSymbolItemSelect(this.m_selectSymbolPos, true);
		}

		private void ShowSymbolView(CUIEvent uiEvent)
		{
			CUIFormScript srcFormScript = uiEvent.m_srcFormScript;
			if (srcFormScript == null)
			{
				return;
			}
			Transform transform = srcFormScript.transform.Find(CSymbolWearController.s_symbolEquipPanel);
			if (transform == null)
			{
				return;
			}
			CUIComponent component = transform.GetComponent<CUIComponent>();
			if (component != null)
			{
				GameObject widget = component.GetWidget(1);
				Text component2 = widget.transform.Find("root/lblName").GetComponent<Text>();
				Image component3 = widget.transform.Find("root/itemCell0/imgIcon").GetComponent<Image>();
				CSymbolItem symbol = uiEvent.m_eventParams.symbolParam.symbol;
				this.m_curViewSymbol = symbol;
				component2.text = symbol.m_name;
				component3.SetSprite(symbol.GetIconPath(), uiEvent.m_srcFormScript, true, false, false, false);
				GameObject gameObject = widget.transform.Find("root/symbolPropPanel").gameObject;
				CSymbolSystem.RefreshSymbolPropContent(gameObject, symbol.m_baseID);
			}
		}

		private void SetSymbolItemSelect(int pos, bool isSelect = true)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CSymbolSystem.s_symbolFormPath);
			if (form == null || pos < 0 || pos >= 30)
			{
				return;
			}
			Transform transform = form.gameObject.transform.Find(CSymbolWearController.s_symbolPagePanel);
			Transform transform2 = transform.Find(string.Format("itemCell{0}/imgSelect", pos));
			if (transform2 != null)
			{
				transform2.gameObject.CustomSetActive(isSelect);
			}
		}

		public void SetSymbolListItem(CUIFormScript formScript, GameObject itemObj, CSymbolItem item)
		{
			if (itemObj == null || item == null)
			{
				return;
			}
			Image component = itemObj.transform.Find("imgIcon").GetComponent<Image>();
			component.SetSprite(item.GetIconPath(), formScript, true, false, false, false);
			Text component2 = itemObj.transform.Find("SymbolName").GetComponent<Text>();
			component2.text = item.m_name;
			Text component3 = itemObj.transform.Find("SymbolDesc").GetComponent<Text>();
			component3.text = CSymbolSystem.GetSymbolAttString(item, true);
			Text component4 = itemObj.transform.Find("lblIconCount").GetComponent<Text>();
			int num = item.m_stackCount - item.GetPageWearCnt(this.m_symbolPageIndex);
			component4.text = string.Format("x{0}", num);
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if ((int)item.m_SymbolData.wLevel <= masterRoleInfo.m_symbolInfo.m_pageMaxLvlLimit && item.GetPageWearCnt(this.m_symbolPageIndex) < CSymbolWearController.s_maxSameIDSymbolEquipNum)
			{
				component.color = CUIUtility.s_Color_White;
			}
			else
			{
				component.color = CUIUtility.s_Color_GrayShader;
			}
		}

		public int GetSymbolListIndex(uint symbolCfgId)
		{
			if (this.m_pageSymbolBagList.get_Count() > 0)
			{
				for (int i = 0; i < this.m_pageSymbolBagList.get_Count(); i++)
				{
					if (this.m_pageSymbolBagList.get_Item(i).m_baseID == symbolCfgId)
					{
						return i;
					}
				}
			}
			return -1;
		}

		private void SetPageSymbolData(bool bChange)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			ResSymbolPos symbolPos = CSymbolInfo.GetSymbolPos(this.m_selectSymbolPos);
			ListView<CSymbolItem> listView = new ListView<CSymbolItem>();
			this.m_pageSymbolBagList.Clear();
			ListView<CSymbolItem> allSymbolList = this.m_symbolSys.GetAllSymbolList();
			int count = allSymbolList.get_Count();
			for (int i = 0; i < count; i++)
			{
				if (!bChange || this.m_curViewSymbol == null || allSymbolList.get_Item(i).m_SymbolData.dwID != this.m_curViewSymbol.m_SymbolData.dwID)
				{
					if (CSymbolInfo.CheckSymbolColor(symbolPos, allSymbolList.get_Item(i).m_SymbolData.bColor) && allSymbolList.get_Item(i).m_stackCount > allSymbolList.get_Item(i).GetPageWearCnt(this.m_symbolPageIndex))
					{
						if ((int)allSymbolList.get_Item(i).m_SymbolData.wLevel <= masterRoleInfo.m_symbolInfo.m_pageMaxLvlLimit && allSymbolList.get_Item(i).GetPageWearCnt(this.m_symbolPageIndex) < CSymbolWearController.s_maxSameIDSymbolEquipNum)
						{
							this.m_pageSymbolBagList.Add(allSymbolList.get_Item(i));
						}
						else
						{
							listView.Add(allSymbolList.get_Item(i));
						}
					}
				}
			}
			CSymbolWearController.SortSymbolList(ref this.m_pageSymbolBagList);
			CSymbolWearController.SortSymbolList(ref listView);
			this.m_pageSymbolBagList.AddRange(listView);
		}

		public static void SortSymbolList(ref ListView<CSymbolItem> symbolList)
		{
			int count = symbolList.get_Count();
			for (int i = 0; i < count - 1; i++)
			{
				for (int j = 0; j < count - 1 - i; j++)
				{
					if (symbolList.get_Item(j).m_SymbolData.wLevel < symbolList.get_Item(j + 1).m_SymbolData.wLevel || (symbolList.get_Item(j).m_SymbolData.wLevel == symbolList.get_Item(j + 1).m_SymbolData.wLevel && symbolList.get_Item(j).m_SymbolData.bColor > symbolList.get_Item(j + 1).m_SymbolData.bColor))
					{
						CSymbolItem cSymbolItem = symbolList.get_Item(j);
						symbolList.set_Item(j, symbolList.get_Item(j + 1));
						symbolList.set_Item(j + 1, cSymbolItem);
					}
				}
			}
		}

		private void OnSymbolBagElementEnable(CUIEvent uiEvent)
		{
			int srcWidgetIndexInBelongedList = uiEvent.m_srcWidgetIndexInBelongedList;
			GameObject srcWidget = uiEvent.m_srcWidget;
			GameObject gameObject = srcWidget.transform.Find("itemCell").gameObject;
			if (this.m_symbolSys.m_selectMenuType == enSymbolMenuType.SymbolEquip && srcWidgetIndexInBelongedList >= 0 && srcWidgetIndexInBelongedList < this.m_pageSymbolBagList.get_Count())
			{
				this.SetSymbolListItem(uiEvent.m_srcFormScript, gameObject, this.m_pageSymbolBagList.get_Item(srcWidgetIndexInBelongedList));
			}
		}

		private void OnSymbolBagItemClick(CUIEvent uiEvent)
		{
			if (uiEvent.m_srcWidgetIndexInBelongedList < 0 || uiEvent.m_srcWidgetIndexInBelongedList >= this.m_pageSymbolBagList.get_Count())
			{
				DebugHelper.Assert(false, "OnSymbolBagItemClick index is out of array index");
				return;
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			CSymbolItem cSymbolItem = this.m_pageSymbolBagList.get_Item(uiEvent.m_srcWidgetIndexInBelongedList);
			if (cSymbolItem == null)
			{
				return;
			}
			if ((int)cSymbolItem.m_SymbolData.wLevel > masterRoleInfo.m_symbolInfo.m_pageMaxLvlLimit)
			{
				int minWearPvpLvl = CSymbolInfo.GetMinWearPvpLvl((int)cSymbolItem.m_SymbolData.wLevel);
				Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("Symbol_LvLimit", new string[]
				{
					minWearPvpLvl.ToString()
				}), false, 1.5f, null, new object[0]);
			}
			else if (cSymbolItem.GetPageWearCnt(this.m_symbolPageIndex) >= CSymbolWearController.s_maxSameIDSymbolEquipNum)
			{
				Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("Symbol_CountLimit"), false, 1.5f, null, new object[0]);
			}
			else
			{
				bool flag = this.IsSymbolChangePanelShow(uiEvent.m_srcFormScript);
				if (flag)
				{
					this.RefreshSymbolChangePanel(this.m_curViewSymbol, cSymbolItem);
					CUIFormScript srcFormScript = uiEvent.m_srcFormScript;
					if (srcFormScript == null)
					{
						return;
					}
					Transform transform = srcFormScript.transform.Find(CSymbolWearController.s_symbolEquipPanel);
					if (transform != null)
					{
						CUIComponent component = transform.GetComponent<CUIComponent>();
						if (component != null)
						{
							GameObject widget = component.GetWidget(7);
							GameObject widget2 = component.GetWidget(9);
							stUIEventParams eventParams = default(stUIEventParams);
							eventParams.symbolParam.symbol = cSymbolItem;
							widget.GetComponent<CUIEventScript>().SetUIEvent(enUIEventType.Click, enUIEventID.Symbol_ChangeConfirm, eventParams);
							widget.CustomSetActive(true);
							widget2.CustomSetActive(false);
						}
					}
				}
				else if (this.m_selectSymbolPos >= 0)
				{
					CSymbolWearController.SendWearSymbol((byte)this.m_symbolPageIndex, (byte)this.m_selectSymbolPos, cSymbolItem.m_objID);
				}
				else
				{
					int num = -1;
					enFindSymbolWearPosCode enFindSymbolWearPosCode;
					if (masterRoleInfo.m_symbolInfo.GetWearPos(cSymbolItem, this.m_symbolPageIndex, out num, out enFindSymbolWearPosCode))
					{
						CSymbolWearController.SendWearSymbol((byte)this.m_symbolPageIndex, (byte)num, cSymbolItem.m_objID);
					}
					else if (enFindSymbolWearPosCode == 1)
					{
						Singleton<CUIManager>.GetInstance().OpenTips("Symbol_FindSymbolPosFull_Tip", true, 1.5f, null, new object[0]);
					}
					else if (enFindSymbolWearPosCode == 2)
					{
						Singleton<CUIManager>.GetInstance().OpenTips("Symbol_FindSymbolNotOpen_Tip", true, 1.5f, null, new object[0]);
					}
					else if (enFindSymbolWearPosCode == 3)
					{
						Singleton<CUIManager>.GetInstance().OpenTips("Symbol_FindSymbolLevelLimit_Tip", true, 1.5f, null, new object[0]);
					}
				}
			}
		}

		private void OnSymbolBagSymbolViewHide(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CSymbolSystem.s_symbolFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.transform.Find(CSymbolWearController.s_symbolEquipPanel);
			if (transform != null)
			{
				CUIComponent component = transform.GetComponent<CUIComponent>();
				if (component != null)
				{
					component.GetWidget(0).CustomSetActive(false);
					component.GetWidget(1).CustomSetActive(false);
					component.GetWidget(5).CustomSetActive(false);
					component.GetWidget(6).CustomSetActive(true);
					this.SetSymbolItemSelect(this.m_selectSymbolPos, false);
				}
			}
		}

		private void OnSymbolBagShow(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CSymbolSystem.s_symbolFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.transform.Find(CSymbolWearController.s_symbolEquipPanel);
			if (transform != null)
			{
				CUIComponent component = transform.GetComponent<CUIComponent>();
				if (component != null)
				{
					component.GetWidget(0).CustomSetActive(true);
					component.GetWidget(1).CustomSetActive(false);
					this.SetSeletSymbolPos(uiEvent.m_eventParams.symbolParam.pos);
					this.ShowSymbolBag(false);
				}
			}
		}

		private void OnViewNotWearItem(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CSymbolSystem.s_symbolFormPath);
			if (form == null)
			{
				return;
			}
			this.SetSymbolItemSelect(this.m_selectSymbolPos, false);
			this.m_selectSymbolPos = -1;
			Transform transform = form.transform.Find(CSymbolWearController.s_symbolEquipPanel);
			if (transform != null)
			{
				CUIComponent component = transform.GetComponent<CUIComponent>();
				if (component != null)
				{
					component.GetWidget(0).CustomSetActive(true);
					component.GetWidget(1).CustomSetActive(false);
					this.SetNotWearSymbolListData();
					this.RefreshSymbolBag(true);
				}
			}
		}

		private void OnChangeSymbolClick(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CSymbolSystem.s_symbolFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.transform.Find(CSymbolWearController.s_symbolEquipPanel);
			if (transform != null)
			{
				CUIComponent component = transform.GetComponent<CUIComponent>();
				if (component != null)
				{
					component.GetWidget(0).CustomSetActive(true);
					component.GetWidget(1).CustomSetActive(false);
					component.GetWidget(6).CustomSetActive(false);
					component.GetWidget(5).CustomSetActive(true);
					this.RefreshSymbolChangePanel(this.m_curViewSymbol, null);
					this.ShowSymbolBag(true);
				}
			}
		}

		private void OnSymbolChangeConfirm(CUIEvent uiEvent)
		{
			CSymbolItem symbol = uiEvent.m_eventParams.symbolParam.symbol;
			if (symbol != null)
			{
				CSymbolWearController.SendWearSymbol((byte)this.m_symbolPageIndex, (byte)this.m_selectSymbolPos, symbol.m_objID);
			}
		}

		private void OnSymbolPageClear(CUIEvent uiEvent)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null && masterRoleInfo.m_symbolInfo.IsPageEmpty(this.m_symbolPageIndex))
			{
				Singleton<CUIManager>.GetInstance().OpenTips("Symbol_Clear_NoWear_Tip", true, 1.5f, null, new object[0]);
			}
			else
			{
				string text = Singleton<CTextManager>.GetInstance().GetText("Symbol_Page_Clear_Tip");
				Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(text, enUIEventID.Symbol_PageClearConfirm, enUIEventID.None, false);
			}
		}

		private void OnSymbolPageClearConfirm(CUIEvent uiEvent)
		{
			CSymbolWearController.SendReqClearSymbolPage(this.m_symbolPageIndex);
		}

		private void OnOpenUnlockLvlTip(CUIEvent uiEvent)
		{
			int tag = uiEvent.m_eventParams.tag;
			Singleton<CUIManager>.GetInstance().OpenTips("Symbol_PosUnlock_lvl_Tip", true, 1f, null, new object[]
			{
				tag
			});
		}

		private void SetNotWearSymbolListData()
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo == null)
			{
				DebugHelper.Assert(false, "SetNotWearSymbolListData role is null");
				return;
			}
			this.m_pageSymbolBagList.Clear();
			ListView<CSymbolItem> allSymbolList = this.m_symbolSys.GetAllSymbolList();
			int count = allSymbolList.get_Count();
			for (int i = 0; i < count; i++)
			{
				if (allSymbolList.get_Item(i).m_stackCount > allSymbolList.get_Item(i).GetPageWearCnt(this.m_symbolPageIndex) && (int)allSymbolList.get_Item(i).m_SymbolData.wLevel <= masterRoleInfo.m_symbolInfo.m_pageMaxLvlLimit && allSymbolList.get_Item(i).GetPageWearCnt(this.m_symbolPageIndex) < CSymbolWearController.s_maxSameIDSymbolEquipNum)
				{
					this.m_pageSymbolBagList.Add(allSymbolList.get_Item(i));
				}
			}
			CSymbolWearController.SortSymbolList(ref this.m_pageSymbolBagList);
		}

		private void ShowSymbolBag(bool bChange = false)
		{
			this.SetPageSymbolData(bChange);
			this.RefreshSymbolBag(false);
		}

		private void RefreshSymbolBag(bool bNoWeatItem)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CSymbolSystem.s_symbolFormPath);
			if (form == null)
			{
				return;
			}
			Transform transform = form.gameObject.transform.Find(CSymbolWearController.s_symbolBagPanel);
			CUIListScript component = transform.Find("Panel_BagList/List").GetComponent<CUIListScript>();
			component.SetElementAmount(this.m_pageSymbolBagList.get_Count());
			Transform transform2 = transform.Find("Panel_BagList/lblTitle");
			if (transform2 != null)
			{
				transform2.GetComponent<Text>().text = ((!bNoWeatItem) ? Singleton<CTextManager>.GetInstance().GetText("Symbol_Title_SymbolBagList") : Singleton<CTextManager>.GetInstance().GetText("Symbol_Title_NoWear"));
			}
			GameObject gameObject = transform.Find("Panel_BagList/lblTips").gameObject;
			bool flag = this.IsSymbolChangePanelShow(form);
			if (this.m_pageSymbolBagList.get_Count() == 0)
			{
				gameObject.CustomSetActive(true);
			}
			else
			{
				gameObject.CustomSetActive(false);
			}
			Transform transform3 = form.transform.Find(CSymbolWearController.s_symbolEquipPanel);
			if (transform3 != null)
			{
				CUIComponent component2 = transform3.GetComponent<CUIComponent>();
				if (component2 != null)
				{
					GameObject widget = component2.GetWidget(8);
					widget.CustomSetActive(!flag || this.m_pageSymbolBagList.get_Count() == 0);
					if (CSysDynamicBlock.bLobbyEntryBlocked)
					{
						widget.CustomSetActive(false);
					}
					GameObject widget2 = component2.GetWidget(7);
					widget2.CustomSetActive(false);
					GameObject widget3 = component2.GetWidget(9);
					widget3.CustomSetActive(flag && this.m_pageSymbolBagList.get_Count() > 0);
				}
			}
		}

		private void RefreshSymbolChangePanel(CSymbolItem fromSymbol, CSymbolItem toSymbol)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CSymbolSystem.s_symbolFormPath);
			if (null == form || fromSymbol == null)
			{
				return;
			}
			Transform transform = form.transform.Find(CSymbolWearController.s_symbolEquipPanel);
			if (transform != null)
			{
				CUIComponent component = transform.GetComponent<CUIComponent>();
				if (component != null)
				{
					GameObject widget = component.GetWidget(5);
					if (null == widget)
					{
						return;
					}
					GameObject gameObject = widget.transform.Find("symbolFrom").gameObject;
					this.RefreshSymbolInfo(form, gameObject, fromSymbol);
					GameObject gameObject2 = widget.transform.Find("symbolTo").gameObject;
					gameObject2.CustomSetActive(toSymbol != null);
					if (toSymbol != null)
					{
						this.RefreshSymbolInfo(form, gameObject2, toSymbol);
					}
				}
			}
		}

		private void RefreshSymbolInfo(CUIFormScript form, GameObject symbolPanel, CSymbolItem symbol)
		{
			if (null == symbolPanel || symbol == null)
			{
				return;
			}
			Image component = symbolPanel.transform.Find("itemCell/imgIcon").GetComponent<Image>();
			component.SetSprite(symbol.GetIconPath(), form, true, false, false, false);
			Text component2 = symbolPanel.transform.Find("lblName").GetComponent<Text>();
			component2.text = symbol.m_name;
			GameObject gameObject = symbolPanel.transform.Find("symbolPropPanel").gameObject;
			CSymbolSystem.RefreshSymbolPropContent(gameObject, symbol.m_baseID);
		}

		private bool IsSymbolChangePanelShow(CUIFormScript form)
		{
			if (null == form)
			{
				return false;
			}
			bool result = false;
			Transform transform = form.transform.Find(CSymbolWearController.s_symbolEquipPanel);
			if (transform != null)
			{
				CUIComponent component = transform.GetComponent<CUIComponent>();
				if (component != null)
				{
					GameObject widget = component.GetWidget(5);
					result = (widget != null && widget.activeSelf);
				}
			}
			return result;
		}

		private void OnPromptBuyGrid(CUIEvent uiEvent)
		{
			int tag = uiEvent.m_eventParams.tag;
			DebugHelper.Assert(tag > 0, "gridPos should be above 0!!!");
			ResShopInfo gridShopInfo = this.GetGridShopInfo(tag);
			DebugHelper.Assert(gridShopInfo != null, "shopCfg is NULL!!!");
			string text = StringHelper.UTF8BytesToString(ref gridShopInfo.szDesc);
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo == null)
			{
				return;
			}
			enPayType enPayType = CMallSystem.ResBuyTypeToPayType((int)gridShopInfo.bCoinType);
			uint dwCoinPrice = gridShopInfo.dwCoinPrice;
			uint currencyValueFromRoleInfo = CMallSystem.GetCurrencyValueFromRoleInfo(masterRoleInfo, enPayType);
			if (currencyValueFromRoleInfo < dwCoinPrice)
			{
				string text2 = string.Empty;
				if (enPayType == enPayType.DiamondAndDianQuan)
				{
					uint diamond = masterRoleInfo.Diamond;
					if (diamond < dwCoinPrice)
					{
						text2 = string.Format(Singleton<CTextManager>.GetInstance().GetText("MixPayNotice"), new object[]
						{
							(dwCoinPrice - diamond).ToString(),
							Singleton<CTextManager>.GetInstance().GetText(CMallSystem.s_payTypeNameKeys[3]),
							(dwCoinPrice - diamond).ToString(),
							Singleton<CTextManager>.GetInstance().GetText(CMallSystem.s_payTypeNameKeys[2])
						});
					}
				}
				string strContent = string.Format(Singleton<CTextManager>.GetInstance().GetText("ConfirmPay", new string[]
				{
					gridShopInfo.dwCoinPrice.ToString(),
					Singleton<CTextManager>.GetInstance().GetText(CMallSystem.s_payTypeNameKeys[(int)enPayType]),
					Singleton<CTextManager>.GetInstance().GetText(CMallSystem.s_payPurposeNameKeys[4]),
					text,
					text2
				}), new object[0]);
				Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(strContent, enUIEventID.Symbol_ConfirmWhenMoneyNotEnough, enUIEventID.None, uiEvent.m_eventParams, false);
			}
			else
			{
				CMallSystem.TryToPay(enPayPurpose.Open, text, CMallSystem.ResBuyTypeToPayType((int)gridShopInfo.bCoinType), gridShopInfo.dwCoinPrice, enUIEventID.Symbol_ConfirmBuyGrid, ref uiEvent.m_eventParams, enUIEventID.None, true, true, false);
			}
		}

		private void OnConfirmBuyGrid(CUIEvent uiEvent)
		{
			DebugHelper.Assert(uiEvent.m_eventParams.tag > 0, "gridPos should be above 0!!!");
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1113u);
			cSPkg.stPkgData.get_stShopBuyReq().iBuyType = 12;
			cSPkg.stPkgData.get_stShopBuyReq().iBuySubType = uiEvent.m_eventParams.tag;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		private void ConfirmWhenMoneyNotEnough(CUIEvent uiEvent)
		{
			int tag = uiEvent.m_eventParams.tag;
			DebugHelper.Assert(tag > 0, "gridPos should be above 0!!!");
			ResShopInfo gridShopInfo = this.GetGridShopInfo(tag);
			DebugHelper.Assert(gridShopInfo != null, "shopCfg is NULL!!!");
			string goodName = StringHelper.UTF8BytesToString(ref gridShopInfo.szDesc);
			CMallSystem.TryToPay(enPayPurpose.Open, goodName, CMallSystem.ResBuyTypeToPayType((int)gridShopInfo.bCoinType), gridShopInfo.dwCoinPrice, enUIEventID.Symbol_ConfirmBuyGrid, ref uiEvent.m_eventParams, enUIEventID.None, true, true, false);
		}

		private ResShopInfo GetGridShopInfo(int pos)
		{
			return CPurchaseSys.GetCfgShopInfo(12, pos);
		}

		public static string GetSymbolWearTip(uint cfgId, bool bWear)
		{
			string text = (!bWear) ? Singleton<CTextManager>.GetInstance().GetText("Symbol_TakeOffTip") : Singleton<CTextManager>.GetInstance().GetText("Symbol_WearTip");
			ResSymbolInfo dataByKey = GameDataMgr.symbolInfoDatabin.GetDataByKey(cfgId);
			if (dataByKey == null)
			{
				return string.Empty;
			}
			text = string.Format(text, StringHelper.UTF8BytesToString(ref dataByKey.szName));
			text += "\n";
			for (int i = 0; i < dataByKey.astFuncEftList.Length; i++)
			{
				ushort wType = dataByKey.astFuncEftList[i].wType;
				byte bValType = dataByKey.astFuncEftList[i].bValType;
				int iValue = dataByKey.astFuncEftList[i].iValue;
				if (wType != 0)
				{
					if (bValType == 0)
					{
						if (CUICommonSystem.s_pctFuncEftList.IndexOf((uint)wType) != -1)
						{
							text += ((!bWear) ? string.Format("{0}-{1}", CUICommonSystem.s_attNameList[(int)wType], CUICommonSystem.GetValuePercent(iValue / 100)) : string.Format("{0}+{1}", CUICommonSystem.s_attNameList[(int)wType], CUICommonSystem.GetValuePercent(iValue / 100)));
						}
						else
						{
							text += ((!bWear) ? string.Format("{0}-{1}", CUICommonSystem.s_attNameList[(int)wType], (float)iValue / 100f) : string.Format("{0}+{1}", CUICommonSystem.s_attNameList[(int)wType], (float)iValue / 100f));
						}
					}
					else if (bValType == 1)
					{
						text += ((!bWear) ? string.Format("{0}-{1}", CUICommonSystem.s_attNameList[(int)wType], CUICommonSystem.GetValuePercent(iValue)) : string.Format("{0}+{1}", CUICommonSystem.s_attNameList[(int)wType], CUICommonSystem.GetValuePercent(iValue)));
					}
				}
			}
			return text;
		}

		public static void SendWearSymbol(byte page, byte pos, ulong objId)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1134u);
			cSPkg.stPkgData.get_stSymbolWear().bPage = page;
			cSPkg.stPkgData.get_stSymbolWear().bPos = pos;
			cSPkg.stPkgData.get_stSymbolWear().ullUniqueID = objId;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		public static void SendReqClearSymbolPage(int pageIndex)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1162u);
			cSPkg.stPkgData.get_stSymbolPageClrReq().bSymbolPageIdx = (byte)pageIndex;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		public static void SendBuySymbol(int symbolIndex)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1113u);
			cSPkg.stPkgData.set_stShopBuyReq(new CSPKG_CMD_SHOPBUY());
			cSPkg.stPkgData.get_stShopBuyReq().iBuyType = 7;
			cSPkg.stPkgData.get_stShopBuyReq().iBuySubType = symbolIndex;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		public static void SendChgSymbolPageName(int pageIndex, string pageName)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1121u);
			cSPkg.stPkgData.get_stSymbolNameChgReq().bPageIdx = (byte)pageIndex;
			StringHelper.StringToUTF8Bytes(pageName, ref cSPkg.stPkgData.get_stSymbolNameChgReq().szPageName);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		public static void OnSymbolGridBuySuccess(int gridPos)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				masterRoleInfo.m_symbolInfo.UpdateBuyGridInfo(gridPos);
				Singleton<CSymbolSystem>.GetInstance().RefreshSymbolForm();
			}
		}

		[MessageHandler(1136)]
		public static void ReciveSymbolChange(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			SCPKG_CMD_SYMBOLCHG stSymbolChg = msg.stPkgData.get_stSymbolChg();
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			uint cfgId = 0u;
			CSymbolInfo.enSymbolOperationType enSymbolOperationType;
			masterRoleInfo.m_symbolInfo.OnSymbolChange((int)stSymbolChg.bPageIdx, (int)stSymbolChg.bPosIdx, stSymbolChg.ullUniqueID, out cfgId, out enSymbolOperationType);
			if (enSymbolOperationType == CSymbolInfo.enSymbolOperationType.Wear || enSymbolOperationType == CSymbolInfo.enSymbolOperationType.Replace)
			{
				string symbolWearTip = CSymbolWearController.GetSymbolWearTip(cfgId, true);
				Singleton<CUIManager>.GetInstance().OpenTips(symbolWearTip, false, 1.5f, null, new object[0]);
				if (enSymbolOperationType == CSymbolInfo.enSymbolOperationType.Wear)
				{
					Singleton<CSoundManager>.GetInstance().PostEvent("UI_fuwen_zhuangbei", null);
				}
				Singleton<CSymbolSystem>.GetInstance().m_symbolWearCtrl.OnSymbolEquip(enSymbolOperationType == CSymbolInfo.enSymbolOperationType.Wear);
			}
			else if (enSymbolOperationType == CSymbolInfo.enSymbolOperationType.TakeOff)
			{
				string symbolWearTip2 = CSymbolWearController.GetSymbolWearTip(cfgId, false);
				Singleton<CUIManager>.GetInstance().OpenTips(symbolWearTip2, false, 1.5f, null, new object[0]);
				Singleton<CSoundManager>.GetInstance().PostEvent("UI_rune_dblclick", null);
				Singleton<CSymbolSystem>.GetInstance().m_symbolWearCtrl.OnSymbolEquipOff(false);
			}
			Singleton<EventRouter>.GetInstance().BroadCastEvent(EventID.SymbolEquipSuc);
			Singleton<CBagSystem>.GetInstance().RefreshBagForm();
		}

		[MessageHandler(1163)]
		public static void ReciveSymbolPageClear(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			SCPKG_CMD_SYMBOLPAGE_CLR stSymbolPageClrRsp = msg.stPkgData.get_stSymbolPageClrRsp();
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				masterRoleInfo.m_symbolInfo.OnSymbolPageClear((int)stSymbolPageClrRsp.bSymbolPageIdx);
			}
			Singleton<CSymbolSystem>.GetInstance().m_symbolWearCtrl.OnSymbolEquipOff(true);
			Singleton<CBagSystem>.GetInstance().RefreshBagForm();
		}

		[MessageHandler(1122)]
		public static void ReciveSymbolNameChange(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			SCPKG_CMD_SYMBOLNAMECHG stSymbolNameChgRsp = msg.stPkgData.get_stSymbolNameChgRsp();
			if (stSymbolNameChgRsp.iResult == 0)
			{
				int bPageIdx = (int)stSymbolNameChgRsp.bPageIdx;
				string pageName = StringHelper.UTF8BytesToString(ref stSymbolNameChgRsp.szPageName);
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				masterRoleInfo.m_symbolInfo.SetSymbolPageName(bPageIdx, pageName);
				Singleton<CSymbolSystem>.GetInstance().m_symbolWearCtrl.RefreshPageDropList();
			}
			else if (stSymbolNameChgRsp.iResult == 125)
			{
				Singleton<CUIManager>.GetInstance().OpenTips("Symbol_Name_illegal", true, 1.5f, null, new object[0]);
			}
		}

		public static void OnSymbolBuySuccess(int pageCount, int buyCnt)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				masterRoleInfo.m_symbolInfo.SetSymbolPageCount(pageCount);
				masterRoleInfo.m_symbolInfo.SetSymbolPageBuyCnt(buyCnt);
			}
			Singleton<CSymbolSystem>.GetInstance().m_symbolWearCtrl.RefreshPageDropList();
			Singleton<CUIManager>.GetInstance().OpenTips("购买成功", false, 1.5f, null, new object[0]);
		}
	}
}
