using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.GameLogic.GameKernal;
using Assets.Scripts.GameSystem;
using Assets.Scripts.Sound;
using Assets.Scripts.UI;
using CSProtocol;
using ResData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InBattleShortcut
{
	public static readonly string InBattleMsgView_FORM_PATH = "UGUI/Form/System/Chat/Form_InBattleChat.prefab";

	public static int InBat_Bubble_CDTime = 3000;

	private CUIFormScript m_CUIForm;

	private CUIFormScript m_battleForm;

	private CUIListScript contentList;

	public CDButton m_cdButton;

	private DictionaryView<ulong, BubbleTimerEntity> player_bubbleTime_map = new DictionaryView<ulong, BubbleTimerEntity>();

	public void OpenForm(CUIFormScript battleForm, CUIEvent uiEvent, bool bShow = true)
	{
		if (this.m_CUIForm != null)
		{
			this.m_CUIForm.gameObject.CustomSetActive(true);
			if (this.contentList != null)
			{
				this.contentList.SelectElement(-1, false);
			}
			GameObject gameObject = Utility.FindChild(this.m_CUIForm.gameObject, "chatTools/node/InputChat_Buttons");
			if (gameObject != null)
			{
				gameObject.CustomSetActive(GameSettings.InBattleInputChatEnable == 1);
			}
			return;
		}
		this.m_battleForm = battleForm;
		this.m_CUIForm = Singleton<CUIManager>.GetInstance().OpenForm(InBattleShortcut.InBattleMsgView_FORM_PATH, true, true);
		DebugHelper.Assert(this.m_CUIForm != null, "InbattleMsgView m_CUIForm == null");
		if (this.m_CUIForm == null)
		{
			return;
		}
		Singleton<CSoundManager>.GetInstance().LoadBank("System_Call", CSoundManager.BankType.Battle);
		this.RegInBattleEvent();
		Singleton<InBattleMsgMgr>.get_instance().BuildInBatEnt();
		this.contentList = this.m_CUIForm.transform.Find("chatTools/node/ListView/List").GetComponent<CUIListScript>();
		DebugHelper.Assert(this.contentList != null, "InbattleMsgView contentList == null");
		if (this.contentList == null)
		{
			return;
		}
		GameObject gameObject2 = battleForm.transform.Find("panelTopRight/SignalPanel/Button_Chat").gameObject;
		DebugHelper.Assert(gameObject2 != null, "InbattleMsgView btnObj == null");
		if (gameObject2 != null)
		{
			gameObject2.CustomSetActive(true);
		}
		this.m_cdButton = new CDButton(gameObject2);
		GameObject gameObject3 = Utility.FindChild(this.m_CUIForm.gameObject, "chatTools/node/InputChat_Buttons");
		if (gameObject3 != null)
		{
			gameObject3.CustomSetActive(GameSettings.InBattleInputChatEnable == 1);
		}
		if (!bShow)
		{
			this.m_CUIForm.gameObject.CustomSetActive(false);
		}
		this.Refresh_List();
	}

	public void Clear()
	{
		Singleton<CSoundManager>.GetInstance().UnLoadBank("System_Call", CSoundManager.BankType.Battle);
		this.UnRegInBattleEvent();
		Singleton<InBattleMsgMgr>.get_instance().inbatEntList.Clear();
		DictionaryView<ulong, BubbleTimerEntity>.Enumerator enumerator = this.player_bubbleTime_map.GetEnumerator();
		while (enumerator.MoveNext())
		{
			KeyValuePair<ulong, BubbleTimerEntity> current = enumerator.get_Current();
			BubbleTimerEntity value = current.get_Value();
			if (value != null)
			{
				value.Clear();
			}
		}
		this.player_bubbleTime_map.Clear();
		this.contentList = null;
		if (this.m_cdButton != null)
		{
			this.m_cdButton.Clear();
			this.m_cdButton = null;
		}
		this.m_CUIForm = null;
		this.m_battleForm = null;
		Singleton<CUIManager>.GetInstance().CloseForm(InBattleShortcut.InBattleMsgView_FORM_PATH);
	}

	public void UpdatePlayerBubbleTimer(ulong playerid, uint heroid)
	{
		BubbleTimerEntity bubbleTimerEntity = null;
		this.player_bubbleTime_map.TryGetValue(playerid, ref bubbleTimerEntity);
		if (bubbleTimerEntity == null)
		{
			bubbleTimerEntity = new BubbleTimerEntity(playerid, heroid, InBattleShortcut.InBat_Bubble_CDTime);
			this.player_bubbleTime_map.Add(playerid, bubbleTimerEntity);
		}
		bubbleTimerEntity.Start();
	}

	public void Show(bool bShow)
	{
		if (this.m_CUIForm != null)
		{
			this.m_CUIForm.gameObject.CustomSetActive(bShow);
		}
	}

	public void Update()
	{
		if (this.m_cdButton != null)
		{
			this.m_cdButton.Update();
		}
	}

	public void RegInBattleEvent()
	{
		Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.InBattleMsg_CloseForm, new CUIEventManager.OnUIEventHandler(this.On_InBattleMsg_CloseForm));
		Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.InBattleMsg_ListElement_Enable, new CUIEventManager.OnUIEventHandler(this.On_InBattleMsg_ListElement_Enable));
		Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.InBattleMsg_ListElement_Click, new CUIEventManager.OnUIEventHandler(this.On_InBattleMsg_ListElement_Click));
	}

	public void UnRegInBattleEvent()
	{
		Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.InBattleMsg_CloseForm, new CUIEventManager.OnUIEventHandler(this.On_InBattleMsg_CloseForm));
		Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.InBattleMsg_ListElement_Enable, new CUIEventManager.OnUIEventHandler(this.On_InBattleMsg_ListElement_Enable));
		Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.InBattleMsg_ListElement_Click, new CUIEventManager.OnUIEventHandler(this.On_InBattleMsg_ListElement_Click));
	}

	public void On_InBattleMsg_TabChange(int index)
	{
	}

	public void Refresh_List()
	{
		ListView<TabElement> inbatEntList = Singleton<InBattleMsgMgr>.get_instance().inbatEntList;
		if (inbatEntList != null)
		{
			this._refresh_list(this.contentList, inbatEntList);
		}
	}

	public void InnerHandle_InBat_PreConfigMsg(COM_INBATTLE_CHAT_TYPE chatType, uint herocfgID, uint cfg_id, ulong ullUid)
	{
		ResInBatMsgHeroActCfg heroActCfg = Singleton<InBattleMsgMgr>.get_instance().GetHeroActCfg(herocfgID, cfg_id);
		ResInBatMsgCfg cfgData = Singleton<InBattleMsgMgr>.get_instance().GetCfgData(cfg_id);
		if (cfgData == null)
		{
			return;
		}
		if (heroActCfg != null)
		{
			InBattleMsgUT.ShowInBattleMsg(chatType, ullUid, herocfgID, heroActCfg.szContent, heroActCfg.szSound);
		}
		else
		{
			InBattleMsgUT.ShowInBattleMsg(chatType, ullUid, herocfgID, cfgData.szContent, cfgData.szSound);
		}
		if (chatType == 1 && Singleton<CBattleSystem>.get_instance().TheMinimapSys.CurMapType() == MinimapSys.EMapType.Mini)
		{
			Player playerByUid = Singleton<GamePlayerCenter>.get_instance().GetPlayerByUid(ullUid);
			ReadonlyContext<PoolObjHandle<ActorRoot>> allHeroes = playerByUid.GetAllHeroes();
			for (int i = 0; i < allHeroes.get_Count(); i++)
			{
				ActorRoot handle = allHeroes.get_Item(i).get_handle();
				if (handle != null && (long)handle.TheActorMeta.ConfigId == (long)((ulong)herocfgID))
				{
					Camera currentCamera = Singleton<Camera_UI3D>.GetInstance().GetCurrentCamera();
					if (currentCamera != null)
					{
						Vector2 sreenLoc = currentCamera.WorldToScreenPoint(handle.HudControl.GetSmallMapPointer_WorldPosition());
						Singleton<CUIParticleSystem>.get_instance().AddParticle(cfgData.szMiniMapEffect, 2f, sreenLoc);
					}
				}
			}
		}
	}

	public void On_InBattleMsg_ListElement_Click(CUIEvent uiEvent)
	{
		this.Show(false);
		int srcWidgetIndexInBelongedList = uiEvent.m_srcWidgetIndexInBelongedList;
		this.Send_Config_Chat(srcWidgetIndexInBelongedList);
	}

	public void Send_Config_Chat(int index)
	{
		if (this.m_cdButton == null || this.m_cdButton.startCooldownTimestamp != 0uL)
		{
			return;
		}
		if (index < 0 || index >= Singleton<InBattleMsgMgr>.get_instance().inbatEntList.get_Count())
		{
			return;
		}
		if (Singleton<InBattleMsgMgr>.get_instance().inbatEntList.get_Count() == 0)
		{
			Singleton<InBattleMsgMgr>.get_instance().BuildInBatEnt();
		}
		TabElement tabElement = Singleton<InBattleMsgMgr>.get_instance().inbatEntList.get_Item(index);
		if (tabElement == null)
		{
			return;
		}
		Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
		if (hostPlayer == null)
		{
			return;
		}
		ResInBatMsgCfg cfgData = Singleton<InBattleMsgMgr>.get_instance().GetCfgData(tabElement.cfgId);
		DebugHelper.Assert(cfgData != null, "InbattleMsgView cfg_data == null");
		if (cfgData == null)
		{
			return;
		}
		SLevelContext curLvelContext = Singleton<BattleLogic>.get_instance().GetCurLvelContext();
		if (curLvelContext == null)
		{
			return;
		}
		if (!Singleton<InBattleMsgMgr>.get_instance().ShouldBeThroughNet(curLvelContext))
		{
			if (tabElement.cfgId >= 1u && tabElement.cfgId <= 15u)
			{
				CPlayerBehaviorStat.Plus((CPlayerBehaviorStat.BehaviorType)tabElement.cfgId);
			}
			this.InnerHandle_InBat_PreConfigMsg(cfgData.bShowType, hostPlayer.CaptainId, tabElement.cfgId, hostPlayer.PlayerUId);
		}
		else
		{
			if (tabElement.cfgId >= 1u && tabElement.cfgId <= 15u)
			{
				CPlayerBehaviorStat.Plus((CPlayerBehaviorStat.BehaviorType)tabElement.cfgId);
			}
			InBattleMsgNetCore.SendInBattleMsg_PreConfig(tabElement.cfgId, cfgData.bShowType, hostPlayer.CaptainId);
		}
		if (this.m_cdButton != null)
		{
			ResInBatChannelCfg dataByKey = GameDataMgr.inBattleChannelDatabin.GetDataByKey((uint)cfgData.bInBatChannelID);
			if (dataByKey != null)
			{
				this.m_cdButton.StartCooldown(dataByKey.dwCdTime, null);
			}
			else
			{
				this.m_cdButton.StartCooldown(4000u, null);
			}
		}
	}

	private void _refresh_list(CUIListScript listScript, ListView<TabElement> data_list)
	{
		if (listScript == null || data_list == null || data_list.get_Count() == 0)
		{
			return;
		}
		int count = data_list.get_Count();
		listScript.SetElementAmount(count);
	}

	public void On_InBattleMsg_CloseForm(CUIEvent uiEvent)
	{
		this.Show(false);
	}

	public void On_InBattleMsg_ListElement_Enable(CUIEvent uievent)
	{
		int srcWidgetIndexInBelongedList = uievent.m_srcWidgetIndexInBelongedList;
		if (srcWidgetIndexInBelongedList < 0 || srcWidgetIndexInBelongedList >= Singleton<InBattleMsgMgr>.get_instance().inbatEntList.get_Count())
		{
			return;
		}
		TabElement tabElement = Singleton<InBattleMsgMgr>.get_instance().inbatEntList.get_Item(srcWidgetIndexInBelongedList);
		if (tabElement == null)
		{
			return;
		}
		InBattleMsgShower component = uievent.m_srcWidget.GetComponent<InBattleMsgShower>();
		if (component != null && tabElement != null)
		{
			component.Set(tabElement.cfgId, tabElement.configContent);
		}
	}
}
