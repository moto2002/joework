using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.GameLogic.GameKernal;
using Assets.Scripts.UI;
using CSProtocol;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameSystem
{
	public class WatchForm : IBattleForm
	{
		public const int SAMPLE_FRAME_STEP = 150;

		public static string s_watchUIForm = "UGUI/Form/Battle/Form_Watch.prefab";

		private CUIFormScript _form;

		private PoolObjHandle<ActorRoot> _pickedHero;

		private DictionaryView<uint, HeroInfoItem> _heroWrapDict;

		private WatchScoreHud _scoreHud;

		private GameObject _camp1List;

		private GameObject _camp2List;

		private CUIListScript _camp1BaseList;

		private CUIListScript _camp1EquipList;

		private CUIListScript _camp2BaseList;

		private CUIListScript _camp2EquipList;

		private HeroInfoHud _heroInfoHud;

		private ReplayControl _replayControl;

		private GameObject _hideBtn;

		private GameObject _showBtn;

		private bool _isCampFold_1;

		private bool _isCampFold_2;

		private bool _isBottomFold;

		private bool _kdaChanged;

		private bool _isViewHide;

		private Plane _pickPlane;

		private float _lastSampleTime;

		private uint _lastUpdateFrame;

		private uint _clickPickHeroID;

		public uint TargetHeroId
		{
			get;
			private set;
		}

		public SampleData moneySample
		{
			get;
			private set;
		}

		public SampleData expSample
		{
			get;
			private set;
		}

		public CUIFormScript FormScript
		{
			get
			{
				return this._form;
			}
		}

		public bool IsFormShow
		{
			get
			{
				return null != this._form;
			}
		}

		public WatchForm()
		{
			this._pickPlane = new Plane(new Vector3(0f, 1f, 0f), 0f);
		}

		public bool OpenForm()
		{
			this._form = Singleton<CUIManager>.GetInstance().OpenForm(WatchForm.s_watchUIForm, false, true);
			if (null == this._form)
			{
				return false;
			}
			this._scoreHud = new WatchScoreHud(Utility.FindChild(this._form.gameObject, "ScoreBoard"));
			this._camp1List = Utility.FindChild(this._form.gameObject, "CampInfoList_1");
			this._camp1BaseList = Utility.GetComponetInChild<CUIListScript>(this._camp1List, "BaseInfoList");
			this._camp1EquipList = Utility.GetComponetInChild<CUIListScript>(this._camp1List, "EquipInfoList");
			this._camp2List = Utility.FindChild(this._form.gameObject, "CampInfoList_2");
			this._camp2BaseList = Utility.GetComponetInChild<CUIListScript>(this._camp2List, "BaseInfoList");
			this._camp2EquipList = Utility.GetComponetInChild<CUIListScript>(this._camp2List, "EquipInfoList");
			this._hideBtn = Utility.FindChild(this._form.gameObject, "PanelBtn/HideBtn");
			this._showBtn = Utility.FindChild(this._form.gameObject, "PanelBtn/ShowBtn");
			this._heroInfoHud = new HeroInfoHud(Utility.FindChild(this._form.gameObject, "HeroInfoHud"));
			this._replayControl = new ReplayControl(Utility.FindChild(this._form.gameObject, "ReplayControl"));
			return true;
		}

		public void CloseForm()
		{
			if (null == this._form)
			{
				return;
			}
			Singleton<CUIManager>.GetInstance().CloseForm(this._form);
			this._form = null;
		}

		private void OnFormClosed(CUIEvent uiEvt)
		{
			Singleton<CBattleSystem>.GetInstance().OnFormClosed();
			this.UnRegisterEvents();
			if (this._heroWrapDict != null)
			{
				this._heroWrapDict.Clear();
				this._heroWrapDict = null;
			}
			if (this._scoreHud != null)
			{
				this._scoreHud.Clear();
				this._scoreHud = null;
			}
			if (this._heroInfoHud != null)
			{
				this._heroInfoHud.Clear();
				this._heroInfoHud = null;
			}
			if (this._replayControl != null)
			{
				this._replayControl.Clear();
				this._replayControl = null;
			}
			this._camp1List = null;
			this._camp2List = null;
			this._camp1BaseList = null;
			this._camp1EquipList = null;
			this._camp2BaseList = null;
			this._camp2EquipList = null;
			this._hideBtn = null;
			this._showBtn = null;
			this.moneySample = null;
			this.expSample = null;
			this._form = null;
		}

		private void RegisterEvents()
		{
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Watch_CameraDraging, new CUIEventManager.OnUIEventHandler(this.OnCameraDraging));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Watch_PickCampList_1, new CUIEventManager.OnUIEventHandler(this.OnPickCampList_1));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Watch_PickCampList_2, new CUIEventManager.OnUIEventHandler(this.OnPickCampList_2));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Watch_Quit, new CUIEventManager.OnUIEventHandler(this.OnQuitWatch));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Watch_ClickCampFold_1, new CUIEventManager.OnUIEventHandler(this.OnClickCampFold_1));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Watch_ClickCampFold_1_End, new CUIEventManager.OnUIEventHandler(this.OnClickCampFold_1_End));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Watch_ClickCampFold_2, new CUIEventManager.OnUIEventHandler(this.OnClickCampFold_2));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Watch_ClickCampFold_2_End, new CUIEventManager.OnUIEventHandler(this.OnClickCampFold_2_End));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Watch_ClickBottomFold, new CUIEventManager.OnUIEventHandler(this.OnClickBottomFold));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Watch_ClickReplayTalk, new CUIEventManager.OnUIEventHandler(this.OnClickReplayTalk));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Watch_HideView, new CUIEventManager.OnUIEventHandler(this.OnToggleHide));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Watch_QuitConfirm, new CUIEventManager.OnUIEventHandler(this.OnQuitConfirm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Watch_FormClosed, new CUIEventManager.OnUIEventHandler(this.OnFormClosed));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Battle_Click_Scene, new CUIEventManager.OnUIEventHandler(this.OnClickBattleScene));
			Singleton<EventRouter>.GetInstance().AddEventHandler<PoolObjHandle<ActorRoot>, int>("HeroSoulLevelChange", new Action<PoolObjHandle<ActorRoot>, int>(this.OnHeroLevelChange));
			Singleton<EventRouter>.GetInstance().AddEventHandler<PoolObjHandle<ActorRoot>, int, bool, PoolObjHandle<ActorRoot>>("HeroGoldCoinInBattleChange", new Action<PoolObjHandle<ActorRoot>, int, bool, PoolObjHandle<ActorRoot>>(this.OnBattleMoneyChange));
			Singleton<EventRouter>.GetInstance().AddEventHandler<uint, stEquipInfo[], bool, int>("HeroEquipInBattleChange", new Action<uint, stEquipInfo[], bool, int>(this.OnBattleEquipChange));
			Singleton<EventRouter>.GetInstance().AddEventHandler(EventID.BATTLE_KDA_CHANGED_BY_ACTOR_DEAD, new Action(this.OnBattleKDAChange));
			Singleton<EventRouter>.GetInstance().AddEventHandler<PoolObjHandle<ActorRoot>, int, int>("HeroEnergyChange", new Action<PoolObjHandle<ActorRoot>, int, int>(this.OnHeroEpChange));
			Singleton<EventRouter>.GetInstance().AddEventHandler<PoolObjHandle<ActorRoot>, int, int>("HeroHpChange", new Action<PoolObjHandle<ActorRoot>, int, int>(this.OnHeroHpChange));
			Singleton<GameSkillEventSys>.GetInstance().AddEventHandler<DefaultSkillEventParam>(GameSkillEventDef.AllEvent_ChangeSkillCD, new GameSkillEvent<DefaultSkillEventParam>(this.OnSkillCDChanged));
			Singleton<EventRouter>.GetInstance().AddEventHandler<PoolObjHandle<ActorRoot>, byte, byte>("HeroSkillLevelUp", new Action<PoolObjHandle<ActorRoot>, byte, byte>(this.OnSkillLevelUp));
		}

		private void UnRegisterEvents()
		{
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Watch_CameraDraging, new CUIEventManager.OnUIEventHandler(this.OnCameraDraging));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Watch_PickCampList_1, new CUIEventManager.OnUIEventHandler(this.OnPickCampList_1));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Watch_PickCampList_2, new CUIEventManager.OnUIEventHandler(this.OnPickCampList_2));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Watch_Quit, new CUIEventManager.OnUIEventHandler(this.OnQuitWatch));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Watch_ClickCampFold_1, new CUIEventManager.OnUIEventHandler(this.OnClickCampFold_1));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Watch_ClickCampFold_1_End, new CUIEventManager.OnUIEventHandler(this.OnClickCampFold_1_End));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Watch_ClickCampFold_2, new CUIEventManager.OnUIEventHandler(this.OnClickCampFold_2));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Watch_ClickCampFold_2_End, new CUIEventManager.OnUIEventHandler(this.OnClickCampFold_2_End));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Watch_ClickBottomFold, new CUIEventManager.OnUIEventHandler(this.OnClickBottomFold));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Watch_ClickReplayTalk, new CUIEventManager.OnUIEventHandler(this.OnClickReplayTalk));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Watch_HideView, new CUIEventManager.OnUIEventHandler(this.OnToggleHide));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Watch_QuitConfirm, new CUIEventManager.OnUIEventHandler(this.OnQuitConfirm));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Watch_FormClosed, new CUIEventManager.OnUIEventHandler(this.OnFormClosed));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Battle_Click_Scene, new CUIEventManager.OnUIEventHandler(this.OnClickBattleScene));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<PoolObjHandle<ActorRoot>, int>("HeroSoulLevelChange", new Action<PoolObjHandle<ActorRoot>, int>(this.OnHeroLevelChange));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<PoolObjHandle<ActorRoot>, int, bool, PoolObjHandle<ActorRoot>>("HeroGoldCoinInBattleChange", new Action<PoolObjHandle<ActorRoot>, int, bool, PoolObjHandle<ActorRoot>>(this.OnBattleMoneyChange));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<uint, stEquipInfo[], bool, int>("HeroEquipInBattleChange", new Action<uint, stEquipInfo[], bool, int>(this.OnBattleEquipChange));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler(EventID.BATTLE_KDA_CHANGED_BY_ACTOR_DEAD, new Action(this.OnBattleKDAChange));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<PoolObjHandle<ActorRoot>, int, int>("HeroEnergyChange", new Action<PoolObjHandle<ActorRoot>, int, int>(this.OnHeroEpChange));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<PoolObjHandle<ActorRoot>, int, int>("HeroHpChange", new Action<PoolObjHandle<ActorRoot>, int, int>(this.OnHeroHpChange));
			Singleton<GameSkillEventSys>.GetInstance().RmvEventHandler<DefaultSkillEventParam>(GameSkillEventDef.AllEvent_ChangeSkillCD, new GameSkillEvent<DefaultSkillEventParam>(this.OnSkillCDChanged));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<PoolObjHandle<ActorRoot>, byte, byte>("HeroSkillLevelUp", new Action<PoolObjHandle<ActorRoot>, byte, byte>(this.OnSkillLevelUp));
		}

		public void BattleStart()
		{
			if (this._heroWrapDict != null)
			{
				return;
			}
			this._lastUpdateFrame = 0u;
			Player playerByUid = Singleton<GamePlayerCenter>.GetInstance().GetPlayerByUid(Singleton<WatchController>.GetInstance().TargetUID);
			this.TargetHeroId = ((playerByUid == null) ? Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer().Captain.get_handle().ObjID : playerByUid.Captain.get_handle().ObjID);
			this._heroWrapDict = new DictionaryView<uint, HeroInfoItem>();
			List<HeroKDA> list = new List<HeroKDA>();
			List<HeroKDA> list2 = new List<HeroKDA>();
			CPlayerKDAStat playerKDAStat = Singleton<BattleLogic>.GetInstance().battleStat.m_playerKDAStat;
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = playerKDAStat.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				PlayerKDA value = current.get_Value();
				if (value.PlayerCamp == 1)
				{
					ListView<HeroKDA>.Enumerator enumerator2 = value.GetEnumerator();
					while (enumerator2.MoveNext())
					{
						list.Add(enumerator2.get_Current());
					}
				}
				else if (value.PlayerCamp == 2)
				{
					ListView<HeroKDA>.Enumerator enumerator3 = value.GetEnumerator();
					while (enumerator3.MoveNext())
					{
						list2.Add(enumerator3.get_Current());
					}
				}
			}
			this.InitCampInfoUIList(1, list, this._camp1BaseList, this._camp1EquipList);
			this.InitCampInfoUIList(2, list2, this._camp2BaseList, this._camp2EquipList);
			this.PickHero(this.TargetHeroId);
			this.ValidateCampMoney();
			this.RegisterEvents();
			this._isBottomFold = false;
			this._isCampFold_1 = false;
			this.OnClickCampFold_1(null);
			this._isCampFold_2 = false;
			this.OnClickCampFold_2(null);
			this._isViewHide = false;
			float step = 150u * Singleton<WatchController>.GetInstance().FrameDelta * 0.001f;
			this.moneySample = new SampleData(step);
			this.expSample = new SampleData(step);
			this._lastSampleTime = 0f;
		}

		private void InitCampInfoUIList(COM_PLAYERCAMP listCamp, List<HeroKDA> heroList, CUIListScript baseInfoUIList, CUIListScript equipInfoUIList)
		{
			if (null == baseInfoUIList || heroList == null || heroList.get_Count() == 0)
			{
				return;
			}
			baseInfoUIList.SetElementAmount(5);
			equipInfoUIList.SetElementAmount(5);
			for (int i = 0; i < 5; i++)
			{
				GameObject gameObject = baseInfoUIList.GetElemenet(i).gameObject;
				GameObject gameObject2 = equipInfoUIList.GetElemenet(i).gameObject;
				if (i < heroList.get_Count())
				{
					HeroKDA heroKDA = heroList.get_Item(i);
					HeroInfoItem heroInfoItem = new HeroInfoItem(listCamp, i, heroKDA, gameObject, gameObject2);
					this._heroWrapDict.Add(heroKDA.actorHero.get_handle().ObjID, heroInfoItem);
				}
				else
				{
					HeroInfoItem.MakeEmpty(gameObject, gameObject2);
				}
			}
		}

		public void UpdateLogic(int delta)
		{
			if (this.moneySample != null && this.expSample != null && Singleton<FrameSynchr>.GetInstance().CurFrameNum % 150u == 1u)
			{
				CampInfo campInfoByCamp = Singleton<BattleStatistic>.GetInstance().GetCampInfoByCamp(1);
				CampInfo campInfoByCamp2 = Singleton<BattleStatistic>.GetInstance().GetCampInfoByCamp(2);
				if (campInfoByCamp != null && campInfoByCamp2 != null)
				{
					this.moneySample.SetCurData(campInfoByCamp.coinTotal, campInfoByCamp2.coinTotal);
					this.expSample.SetCurData(campInfoByCamp.soulExpTotal, campInfoByCamp2.soulExpTotal);
					this._lastSampleTime = Time.time;
				}
			}
			if (this._heroInfoHud != null)
			{
				this._heroInfoHud.UpdateLogic(delta);
			}
		}

		public void Update()
		{
		}

		public void LateUpdate()
		{
			if (this._heroWrapDict == null)
			{
				return;
			}
			if (!Singleton<WatchController>.GetInstance().IsWatching)
			{
				this.CloseForm();
				bool flag = true;
				if (flag)
				{
					Singleton<SettlementSystem>.GetInstance().ShowSettlementPanel(true);
				}
				Singleton<GameBuilder>.GetInstance().EndGame();
				return;
			}
			uint curFrameNum = Singleton<FrameSynchr>.GetInstance().CurFrameNum;
			if (curFrameNum < this._lastUpdateFrame + 3u)
			{
				return;
			}
			this._lastUpdateFrame = curFrameNum;
			DictionaryView<uint, HeroInfoItem>.Enumerator enumerator = this._heroWrapDict.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, HeroInfoItem> current = enumerator.get_Current();
				current.get_Value().LateUpdate();
				if (this._kdaChanged)
				{
					KeyValuePair<uint, HeroInfoItem> current2 = enumerator.get_Current();
					current2.get_Value().ValidateKDA();
				}
			}
			if (this._heroInfoHud != null)
			{
				this._heroInfoHud.LateUpdate();
				if (this._kdaChanged)
				{
					this._heroInfoHud.ValidateKDA();
				}
			}
			if (this._scoreHud != null)
			{
				this._scoreHud.LateUpdate();
			}
			if (this._replayControl != null)
			{
				this._replayControl.LateUpdate();
			}
			this._kdaChanged = false;
		}

		private void OnQuitWatch(CUIEvent uiEvent)
		{
			Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(Singleton<CTextManager>.GetInstance().GetText("confirmQuitWatch"), enUIEventID.Watch_QuitConfirm, enUIEventID.Watch_QuitCancel, false);
		}

		private void OnQuitConfirm(CUIEvent uiEvent)
		{
			Singleton<WatchController>.GetInstance().Quit();
		}

		private void OnToggleHide(CUIEvent uiEvent)
		{
			if (this._form && this._form.gameObject)
			{
				this._isViewHide = !this._isViewHide;
				this._hideBtn.CustomSetActive(!this._isViewHide);
				this._showBtn.CustomSetActive(this._isViewHide);
				this.DisableAnimator(Utility.FindChild(this._camp1List, "EquipInfoList"));
				this._camp1List.CustomSetActive(!this._isViewHide);
				this.DisableAnimator(Utility.FindChild(this._camp2List, "EquipInfoList"));
				this._camp2List.CustomSetActive(!this._isViewHide);
				this.DisableAnimator(this._heroInfoHud.Root);
				this._heroInfoHud.Root.CustomSetActive(!this._isViewHide);
				this.DisableAnimator(this._replayControl.Root);
				this._replayControl.Root.CustomSetActive(!this._isViewHide);
			}
		}

		private void DisableAnimator(GameObject node)
		{
			if (node)
			{
				Animator component = node.GetComponent<Animator>();
				if (component)
				{
					component.enabled = false;
				}
			}
		}

		public void OnCameraDraging(CUIEvent uiEvent)
		{
			if (MonoSingleton<CameraSystem>.GetInstance().enableLockedCamera)
			{
				MonoSingleton<CameraSystem>.get_instance().ToggleFreeDragCamera(true);
				if (this._pickedHero)
				{
					MonoSingleton<CameraSystem>.get_instance().MobaCamera.SetAbsoluteLockLocation((Vector3)this._pickedHero.get_handle().ActorControl.actorLocation);
				}
				this.OnCamerFreed();
			}
			MonoSingleton<CameraSystem>.GetInstance().MoveCamera(-uiEvent.m_pointerEventData.delta.x, -uiEvent.m_pointerEventData.delta.y);
		}

		public void OnCamerFreed()
		{
			this._camp1BaseList.SelectElement(-1, false);
			this._camp2BaseList.SelectElement(-1, false);
		}

		public void OnPickCampList_1(CUIEvent uiEvent)
		{
			this.FocusHeroPicked(1, uiEvent.m_srcWidgetIndexInBelongedList);
		}

		public void OnPickCampList_2(CUIEvent uiEvent)
		{
			this.FocusHeroPicked(2, uiEvent.m_srcWidgetIndexInBelongedList);
		}

		private void PickHero(uint heroObjId)
		{
			PoolObjHandle<ActorRoot> actor = Singleton<GameObjMgr>.GetInstance().GetActor(heroObjId);
			if (actor)
			{
				this.FocusHeroPicked(actor.get_handle().TheActorMeta.ActorCamp, this._heroWrapDict.get_Item(heroObjId).listIndex);
			}
		}

		private void FocusHeroPicked(COM_PLAYERCAMP listCamp, int listIndex)
		{
			if (this._heroWrapDict == null)
			{
				return;
			}
			HeroInfoItem heroInfoItem = null;
			DictionaryView<uint, HeroInfoItem>.Enumerator enumerator = this._heroWrapDict.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, HeroInfoItem> current = enumerator.get_Current();
				HeroInfoItem value = current.get_Value();
				if (value.listCamp == listCamp && value.listIndex == listIndex)
				{
					heroInfoItem = value;
					break;
				}
			}
			if (heroInfoItem != null)
			{
				MonoSingleton<CameraSystem>.get_instance().ToggleFreeDragCamera(false);
				MonoSingleton<CameraSystem>.get_instance().SetFocusActor(heroInfoItem.HeroInfo.actorHero);
				this._pickedHero = heroInfoItem.HeroInfo.actorHero;
				this._heroInfoHud.SetPickHero(heroInfoItem.HeroInfo);
				if (listCamp == 1)
				{
					this._camp1BaseList.SelectElement(listIndex, false);
					this._camp2BaseList.SelectElement(-1, false);
				}
				else if (listCamp == 2)
				{
					this._camp2BaseList.SelectElement(listIndex, false);
					this._camp1BaseList.SelectElement(-1, false);
				}
				else
				{
					this._camp1BaseList.SelectElement(-1, false);
					this._camp2BaseList.SelectElement(-1, false);
				}
			}
			else
			{
				this._pickedHero.Release();
				this._camp1BaseList.SelectElement(-1, false);
				this._camp2BaseList.SelectElement(-1, false);
			}
		}

		private void OnHeroLevelChange(PoolObjHandle<ActorRoot> hero, int level)
		{
			if (this._heroWrapDict.ContainsKey(hero.get_handle().ObjID))
			{
				this._heroWrapDict.get_Item(hero.get_handle().ObjID).ValidateLevel();
			}
			if (hero.get_handle().ObjID == this._heroInfoHud.PickedHeroID)
			{
				this._heroInfoHud.ValidateLevel();
			}
		}

		private void OnBattleKDAChange()
		{
			this._kdaChanged = true;
		}

		private void OnBattleMoneyChange(PoolObjHandle<ActorRoot> actor, int changeValue, bool isIncome, PoolObjHandle<ActorRoot> target)
		{
			this.ValidateCampMoney();
			if (this._heroWrapDict.ContainsKey(actor.get_handle().ObjID))
			{
				this._heroWrapDict.get_Item(actor.get_handle().ObjID).ValidateMoney();
			}
			if (actor.get_handle().ObjID == this._heroInfoHud.PickedHeroID)
			{
				this._heroInfoHud.ValidateMoney();
			}
		}

		private void OnBattleEquipChange(uint actorObjectID, stEquipInfo[] equips, bool bIsAdd, int iEquipSlotIndex)
		{
			if (this._heroWrapDict.ContainsKey(actorObjectID))
			{
				this._heroWrapDict.get_Item(actorObjectID).ValidateEquip();
			}
			if (actorObjectID == this._heroInfoHud.PickedHeroID)
			{
				this._heroInfoHud.ValidateEquip();
			}
		}

		private void OnHeroHpChange(PoolObjHandle<ActorRoot> hero, int iCurVal, int iMaxVal)
		{
			if (hero.get_handle().ObjID == this._heroInfoHud.PickedHeroID)
			{
				this._heroInfoHud.ValidateHp();
			}
		}

		private void OnHeroEpChange(PoolObjHandle<ActorRoot> hero, int iCurVal, int iMaxVal)
		{
			if (hero.get_handle().ObjID == this._heroInfoHud.PickedHeroID)
			{
				this._heroInfoHud.ValidateEp();
			}
		}

		private void ValidateCampMoney()
		{
			this._scoreHud.ValidateMoney(Singleton<BattleStatistic>.GetInstance().GetCampInfoByCamp(1).coinTotal, Singleton<BattleStatistic>.GetInstance().GetCampInfoByCamp(2).coinTotal);
		}

		private void OnSkillCDChanged(ref DefaultSkillEventParam _param)
		{
			if (this._heroInfoHud != null && _param.actor.get_handle().ObjID == this._heroInfoHud.PickedHeroID)
			{
				this._heroInfoHud.TheSkillHud.ValidateCD(_param.slot);
			}
		}

		private void OnSkillLevelUp(PoolObjHandle<ActorRoot> hero, byte bSlotType, byte bSkillLevel)
		{
			if (this._heroInfoHud != null && hero.get_handle().ObjID == this._heroInfoHud.PickedHeroID)
			{
				this._heroInfoHud.TheSkillHud.ValidateLevel((SkillSlotType)bSlotType);
			}
		}

		private void OnClickCampFold_1(CUIEvent evt)
		{
			CUIAnimatorScript componetInChild = Utility.GetComponetInChild<CUIAnimatorScript>(this._form.gameObject, "CampInfoList_1/EquipInfoList");
			if (componetInChild)
			{
				this._isCampFold_1 = !this._isCampFold_1;
				componetInChild.PlayAnimator((!this._isCampFold_1) ? "Open" : "Close");
			}
		}

		private void OnClickCampFold_1_End(CUIEvent evt)
		{
			this._form.m_sgameGraphicRaycaster.RefreshGameObject(Utility.FindChild(this._form.gameObject, "CampInfoList_1/EquipInfoList/FoldButton"));
		}

		private void OnClickCampFold_2(CUIEvent evt)
		{
			CUIAnimatorScript componetInChild = Utility.GetComponetInChild<CUIAnimatorScript>(this._form.gameObject, "CampInfoList_2/EquipInfoList");
			if (componetInChild)
			{
				this._isCampFold_2 = !this._isCampFold_2;
				componetInChild.PlayAnimator((!this._isCampFold_2) ? "Open" : "Close");
			}
		}

		private void OnClickCampFold_2_End(CUIEvent evt)
		{
			this._form.m_sgameGraphicRaycaster.RefreshGameObject(Utility.FindChild(this._form.gameObject, "CampInfoList_2/EquipInfoList/FoldButton"));
		}

		private void OnClickBottomFold(CUIEvent evt)
		{
			CUIAnimatorScript componetInChild = Utility.GetComponetInChild<CUIAnimatorScript>(this._form.gameObject, "HeroInfoHud");
			CUIAnimatorScript componetInChild2 = Utility.GetComponetInChild<CUIAnimatorScript>(this._form.gameObject, "ReplayControl");
			if (componetInChild && componetInChild2)
			{
				this._isBottomFold = !this._isBottomFold;
				componetInChild.PlayAnimator((!this._isBottomFold) ? "Open" : "Close");
				componetInChild2.PlayAnimator((!this._isBottomFold) ? "Open" : "Close");
			}
		}

		private void OnClickReplayTalk(CUIEvent evt)
		{
			Singleton<CUIManager>.GetInstance().OpenTips("barrageNotReady", true, 1.5f, null, new object[0]);
		}

		private void OnClickBattleScene(CUIEvent uievent)
		{
			Ray ray = Camera.main.ScreenPointToRay(uievent.m_pointerEventData.position);
			this._clickPickHeroID = 0u;
			float distance;
			if (this._pickPlane.Raycast(ray, out distance))
			{
				Vector3 point = ray.GetPoint(distance);
				SceneManagement instance = Singleton<SceneManagement>.GetInstance();
				SceneManagement.Coordinate coord = default(SceneManagement.Coordinate);
				instance.GetCoord_Center(ref coord, ((VInt3)point).get_xz(), 3000);
				instance.UpdateDirtyNodes();
				instance.ForeachActorsBreak(coord, new SceneManagement.Process_Bool(this.TrySearchHero));
				if (this._clickPickHeroID > 0u)
				{
					this.PickHero(this._clickPickHeroID);
				}
			}
		}

		private bool TrySearchHero(ref PoolObjHandle<ActorRoot> actor)
		{
			if (actor.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero)
			{
				this._clickPickHeroID = actor.get_handle().ObjID;
				return false;
			}
			return true;
		}
	}
}
