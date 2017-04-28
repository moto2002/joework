using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.GameLogic.GameKernal;
using Assets.Scripts.UI;
using ResData;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	public class CRecordUseSDK : Singleton<CRecordUseSDK>
	{
		public enum RECORD_EVENT_PRIORITY
		{
			RECORD_EVENT_TYPE_INVALID,
			RECORD_EVENT_TYPE_ASSIST,
			RECORD_EVENT_TYPE_ONCEKILL,
			RECORD_EVENT_TYPE_DOUBLEKILL,
			RECORD_EVENT_TYPE_TRIPLEKILL,
			RECORD_EVENT_TYPE_QUATARYKILL,
			RECORD_EVENT_TYPE_PENTAKILL
		}

		public enum CHECK_PERMISSION_STUTAS
		{
			CHECK_PERMISSION_STUTAS_TYPE_WITHOUTRESULT = -1,
			CHECK_PERMISSION_STUTAS_TYPE_NOPERMISSION,
			CHECK_PERMISSION_STUTAS_TYPE_PERMISSIONOK
		}

		public enum CHECK_WHITELIST_STATUS
		{
			CHECK_WHITELIST_STATUS_TYPE_INVALID,
			CHECK_WHITELIST_STATUS_TYPE_AUTOCHECK,
			CHECK_WHITELIST_STATUS_TYPE_TIMEUP,
			CHECK_WHITELIST_STATUS_TYPE_RESULTOK,
			CHECK_WHITELIST_STATUS_TYPE_RESULTFAILED,
			CHECK_WHITELIST_STATUS_TYPE_VIDEOMGRCLICK
		}

		public struct RECORD_INFO
		{
			public CRecordUseSDK.RECORD_EVENT_PRIORITY eventPriority;

			public long lEndTime;

			public RECORD_INFO(CRecordUseSDK.RECORD_EVENT_PRIORITY _eventPriority, long _lEndTime)
			{
				this.eventPriority = _eventPriority;
				this.lEndTime = _lEndTime;
			}
		}

		private const string MODENAME = "MODE";

		private const string GRADENAME = "GRADE";

		private const string HERONAME = "HERO";

		private const string KILLNUM = "KILLNUM";

		private const string DEADNUM = "DEADNUM";

		private const string ASSISTNUM = "ASSISTNUM";

		private const string MULTIKILL = "MULTIKILL";

		private CRecordUseSDK.CHECK_PERMISSION_STUTAS m_enmCheckPermissionRes = CRecordUseSDK.CHECK_PERMISSION_STUTAS.CHECK_PERMISSION_STUTAS_TYPE_WITHOUTRESULT;

		private CRecordUseSDK.RECORD_EVENT_PRIORITY m_enLastEventPriority;

		private long m_lLastEventStartTime;

		private long m_lLastEventEndTime;

		private uint m_uiMinSpaceLimit = 200u;

		private uint m_uiWarningSpaceLimit = 500u;

		private bool m_bIsStartRecordOk;

		private Transform m_RecorderPanel;

		private uint m_uiEventStartTimeInterval = 5000u;

		private uint m_uiEventEndTimeInterval = 10000u;

		private uint m_uiEventNumMax = 5u;

		private uint m_ui543KillEventTotalTime = 90000u;

		private uint m_ui210KillEventTotalTime = 60000u;

		private long m_lGameEndTime;

		private long m_lGameStartTime;

		private int m_iContinuKillMaxNum = -1;

		private PoolObjHandle<ActorRoot> m_hostActor;

		private string m_strHostPlayerName;

		private bool m_bIsMvp;

		private bool m_bIsRecordMomentsEnable;

		private int m_iHostPlaterKillNum;

		private int m_iHostPlaterDeadNum;

		private int m_iHostPlaterAssistNum;

		private GameObject m_objKingBar;

		private long m_lVideoTimeLen;

		private uint m_uiOnceDoubleEventTimeIntervalReduce = 5000u;

		private Dictionary<CRecordUseSDK.RECORD_EVENT_PRIORITY, SortedList<long, long>> m_stRecordInfo = new Dictionary<CRecordUseSDK.RECORD_EVENT_PRIORITY, SortedList<long, long>>();

		private bool m_bIsCallGameJoyGenerate;

		private bool m_bIsCallStopGameJoyRecord;

		private CRecordUseSDK.CHECK_WHITELIST_STATUS m_enmCheckWhiteListStatus;

		private void Reset()
		{
			this.m_enLastEventPriority = CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_INVALID;
			this.m_lLastEventStartTime = 0L;
			this.m_lLastEventEndTime = 0L;
			this.m_lGameStartTime = 0L;
			this.m_lGameEndTime = 0L;
			this.m_iContinuKillMaxNum = -1;
			this.m_bIsStartRecordOk = false;
			this.m_bIsMvp = false;
			this.m_iHostPlaterKillNum = 0;
			this.m_iHostPlaterDeadNum = 0;
			this.m_iHostPlaterAssistNum = 0;
			this.m_bIsCallGameJoyGenerate = false;
			this.m_bIsCallStopGameJoyRecord = false;
			this.m_lVideoTimeLen = 0L;
			if (this.m_hostActor)
			{
				this.m_hostActor.Release();
			}
			if (this.m_stRecordInfo != null)
			{
				this.m_stRecordInfo.Clear();
			}
			this.m_uiEventStartTimeInterval = GameDataMgr.GetGlobeValue(219) * 1000u;
			this.m_uiEventEndTimeInterval = GameDataMgr.GetGlobeValue(249) * 1000u;
			this.m_uiEventNumMax = GameDataMgr.GetGlobeValue(220);
			this.m_ui543KillEventTotalTime = GameDataMgr.GetGlobeValue(221) * 1000u;
			this.m_ui210KillEventTotalTime = GameDataMgr.GetGlobeValue(256) * 1000u;
			this.m_uiMinSpaceLimit = GameDataMgr.GetGlobeValue(231);
			this.m_uiOnceDoubleEventTimeIntervalReduce = GameDataMgr.GetGlobeValue(248) * 1000u;
		}

		public override void Init()
		{
			base.Init();
			this.Reset();
			Singleton<GameEventSys>.get_instance().AddEventHandler<DefaultGameEventParam>(GameEventDef.Event_FightPrepare, new RefAction<DefaultGameEventParam>(this.OnFightPrepare));
			Singleton<GameEventSys>.get_instance().AddEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.OnActorDead));
			Singleton<GameEventSys>.get_instance().AddEventHandler<DefaultGameEventParam>(GameEventDef.Event_DoubleKill, new RefAction<DefaultGameEventParam>(this.OnActorDoubleKill));
			Singleton<GameEventSys>.get_instance().AddEventHandler<DefaultGameEventParam>(GameEventDef.Event_TripleKill, new RefAction<DefaultGameEventParam>(this.OnActorTripleKill));
			Singleton<GameEventSys>.get_instance().AddEventHandler<DefaultGameEventParam>(GameEventDef.Event_QuataryKill, new RefAction<DefaultGameEventParam>(this.OnActorQuataryKill));
			Singleton<GameEventSys>.get_instance().AddEventHandler<DefaultGameEventParam>(GameEventDef.Event_PentaKill, new RefAction<DefaultGameEventParam>(this.OnActorPentaKill));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Record_Save_Moment_Video, new CUIEventManager.OnUIEventHandler(this.OnSaveMomentVideo));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Record_Save_Moment_Video_Cancel, new CUIEventManager.OnUIEventHandler(this.OnSaveMomentVideoCancel));
			Singleton<EventRouter>.GetInstance().AddEventHandler<bool>(EventID.GAMEJOY_STARTRECORDING_RESULT, new Action<bool>(this.OnGameJoyStartRecordResult));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.OB_Video_Btn_VideoMgr_Click, new CUIEventManager.OnUIEventHandler(this.OnBtnVideoMgrClick));
			Singleton<EventRouter>.GetInstance().AddEventHandler<bool>(EventID.GAMEJOY_SDK_PERMISSION_CHECK_RESULT, new Action<bool>(this.OnGameJoyCheckPermissionResult));
			Singleton<EventRouter>.GetInstance().AddEventHandler<bool>(EventID.GAMEJOY_AVAILABILITY_CHECK_RESULT, new Action<bool>(this.OnGameJoyCheckAvailabilityResult));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Record_Check_WhiteList_TimeUp, new CUIEventManager.OnUIEventHandler(this.OnCheckWhiteListTimeUp));
			Singleton<EventRouter>.GetInstance().AddEventHandler<long>(EventID.GAMEJOY_STOPRECORDING_RESULT, new Action<long>(this.OnGameJoyStopRecordResult));
			if (GameSettings.EnableKingTimeMode)
			{
				this.m_enmCheckWhiteListStatus = CRecordUseSDK.CHECK_WHITELIST_STATUS.CHECK_WHITELIST_STATUS_TYPE_AUTOCHECK;
				this.CheckWhiteList();
			}
		}

		public override void UnInit()
		{
			base.UnInit();
			if (this.m_RecorderPanel)
			{
				this.m_stRecordInfo.Clear();
			}
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_FightPrepare, new RefAction<DefaultGameEventParam>(this.OnFightPrepare));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.OnActorDead));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_DoubleKill, new RefAction<DefaultGameEventParam>(this.OnActorDoubleKill));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_TripleKill, new RefAction<DefaultGameEventParam>(this.OnActorTripleKill));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_QuataryKill, new RefAction<DefaultGameEventParam>(this.OnActorQuataryKill));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_PentaKill, new RefAction<DefaultGameEventParam>(this.OnActorPentaKill));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Record_Save_Moment_Video, new CUIEventManager.OnUIEventHandler(this.OnSaveMomentVideo));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Record_Save_Moment_Video_Cancel, new CUIEventManager.OnUIEventHandler(this.OnSaveMomentVideoCancel));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<bool>(EventID.GAMEJOY_STARTRECORDING_RESULT, new Action<bool>(this.OnGameJoyStartRecordResult));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.OB_Video_Btn_VideoMgr_Click, new CUIEventManager.OnUIEventHandler(this.OnBtnVideoMgrClick));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<bool>(EventID.GAMEJOY_SDK_PERMISSION_CHECK_RESULT, new Action<bool>(this.OnGameJoyCheckPermissionResult));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<bool>(EventID.GAMEJOY_AVAILABILITY_CHECK_RESULT, new Action<bool>(this.OnGameJoyCheckAvailabilityResult));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Record_Check_WhiteList_TimeUp, new CUIEventManager.OnUIEventHandler(this.OnCheckWhiteListTimeUp));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<long>(EventID.GAMEJOY_STOPRECORDING_RESULT, new Action<long>(this.OnGameJoyStopRecordResult));
		}

		private void OnFightPrepare(ref DefaultGameEventParam prm)
		{
			SLevelContext curLvelContext = Singleton<BattleLogic>.get_instance().GetCurLvelContext();
			if (curLvelContext == null)
			{
				this.m_bIsRecordMomentsEnable = false;
				return;
			}
			this.m_bIsRecordMomentsEnable = (GameSettings.EnableKingTimeMode && this.GetRecorderGlobalCfgEnableFlag() && !Singleton<WatchController>.GetInstance().IsWatching && curLvelContext.IsMobaModeWithOutGuide());
			if (this.m_bIsRecordMomentsEnable)
			{
				if (Singleton<LobbyLogic>.get_instance().reconnGameInfo != null)
				{
					this.m_bIsRecordMomentsEnable = false;
					Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("RecordMomentSuspendRecord"), false, 1.5f, null, new object[0]);
					return;
				}
				this.Reset();
				this.m_hostActor = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().Captain;
				Singleton<GameJoy>.get_instance().StartMomentsRecording();
			}
		}

		private void AddRecordEvent(CRecordUseSDK.RECORD_EVENT_PRIORITY eventPriority, long lStartTime, long lEndTime)
		{
			if (this.m_stRecordInfo == null)
			{
				return;
			}
			SortedList<long, long> sortedList = null;
			if (!this.m_stRecordInfo.TryGetValue(eventPriority, ref sortedList))
			{
				sortedList = new SortedList<long, long>();
				this.m_stRecordInfo.Add(eventPriority, sortedList);
			}
			if (sortedList != null && !sortedList.ContainsKey(lStartTime))
			{
				sortedList.Add(lStartTime, lEndTime);
			}
			this.m_enLastEventPriority = CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_INVALID;
		}

		private void UpdateRecordEvent(PoolObjHandle<ActorRoot> eventActor, CRecordUseSDK.RECORD_EVENT_PRIORITY eventPriority)
		{
			if (!this.m_bIsRecordMomentsEnable || !this.m_bIsStartRecordOk)
			{
				return;
			}
			if (eventPriority != CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_INVALID && Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().Captain != eventActor)
			{
				return;
			}
			long num = GameJoy.get_getSystemCurrentTimeMillis() - this.m_lGameStartTime;
			bool flag = false;
			if (eventActor && eventActor.get_handle().ActorControl != null)
			{
				HeroWrapper heroWrapper = eventActor.get_handle().ActorControl as HeroWrapper;
				if (heroWrapper != null)
				{
					flag = heroWrapper.IsInMultiKill();
				}
			}
			if (!flag || eventPriority == CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_INVALID)
			{
				if (this.m_enLastEventPriority > CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_INVALID)
				{
					this.AddRecordEvent(this.m_enLastEventPriority, this.m_lLastEventStartTime, this.m_lLastEventEndTime);
				}
				this.m_enLastEventPriority = eventPriority;
				this.m_lLastEventStartTime = num;
				this.m_lLastEventEndTime = num;
			}
			else
			{
				if (this.m_enLastEventPriority != CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_INVALID && eventPriority <= this.m_enLastEventPriority)
				{
					return;
				}
				if (this.m_enLastEventPriority <= CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_ASSIST)
				{
					this.m_lLastEventStartTime = num;
				}
				this.m_enLastEventPriority = eventPriority;
				this.m_lLastEventEndTime = num;
			}
		}

		private void OnActorDead(ref GameDeadEventParam prm)
		{
			if (!this.m_bIsRecordMomentsEnable || !this.m_bIsStartRecordOk)
			{
				return;
			}
			if (!prm.src || prm.src.get_handle().TheActorMeta.ActorType != ActorTypeDef.Actor_Type_Hero)
			{
				return;
			}
			this.UpdateRecordEvent(prm.atker, CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_ONCEKILL);
			if (Singleton<GamePlayerCenter>.get_instance() != null && Singleton<GamePlayerCenter>.get_instance().GetHostPlayer() != null && prm.atker != Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().Captain)
			{
				if (prm.src && prm.src.get_handle().ActorControl != null)
				{
					List<KeyValuePair<uint, ulong>>.Enumerator enumerator = prm.src.get_handle().ActorControl.hurtSelfActorList.GetEnumerator();
					while (enumerator.MoveNext())
					{
						KeyValuePair<uint, ulong> current = enumerator.get_Current();
						if (current.get_Key() == Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().Captain.get_handle().ObjID)
						{
							this.UpdateRecordEvent(Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().Captain, CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_ASSIST);
							return;
						}
					}
				}
				if (prm.atker && prm.atker.get_handle().ActorControl != null)
				{
					List<KeyValuePair<uint, ulong>>.Enumerator enumerator2 = prm.atker.get_handle().ActorControl.helpSelfActorList.GetEnumerator();
					while (enumerator2.MoveNext())
					{
						KeyValuePair<uint, ulong> current2 = enumerator2.get_Current();
						if (current2.get_Key() == Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().Captain.get_handle().ObjID)
						{
							this.UpdateRecordEvent(Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().Captain, CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_ASSIST);
							return;
						}
					}
				}
			}
			else if (Singleton<GamePlayerCenter>.get_instance() != null && Singleton<GamePlayerCenter>.get_instance().GetHostPlayer() != null && prm.atker == Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().Captain)
			{
				HeroWrapper heroWrapper = prm.orignalAtker.get_handle().ActorControl as HeroWrapper;
				if (heroWrapper != null && heroWrapper.ContiKillNum > this.m_iContinuKillMaxNum)
				{
					this.m_iContinuKillMaxNum = heroWrapper.ContiKillNum;
				}
			}
		}

		private void OnActorDoubleKill(ref DefaultGameEventParam prm)
		{
			this.UpdateRecordEvent(prm.atker, CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_DOUBLEKILL);
		}

		private void OnActorTripleKill(ref DefaultGameEventParam prm)
		{
			this.UpdateRecordEvent(prm.atker, CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_TRIPLEKILL);
		}

		private void OnActorQuataryKill(ref DefaultGameEventParam prm)
		{
			this.UpdateRecordEvent(prm.atker, CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_QUATARYKILL);
		}

		private void OnActorPentaKill(ref DefaultGameEventParam prm)
		{
			this.UpdateRecordEvent(prm.atker, CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_PENTAKILL);
		}

		public void DoFightOver()
		{
			if (!this.m_bIsRecordMomentsEnable || !this.m_bIsStartRecordOk)
			{
				return;
			}
			this.UpdateRecordEvent(default(PoolObjHandle<ActorRoot>), CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_INVALID);
			this.m_lGameEndTime = GameJoy.get_getSystemCurrentTimeMillis() - this.m_lGameStartTime;
			if (this.m_hostActor)
			{
				this.m_strHostPlayerName = this.m_hostActor.get_handle().name;
				uint mvpPlayer = Singleton<BattleStatistic>.get_instance().GetMvpPlayer(this.m_hostActor.get_handle().TheActorMeta.ActorCamp, true);
				if (mvpPlayer != 0u && mvpPlayer == this.m_hostActor.get_handle().TheActorMeta.PlayerId)
				{
					this.m_bIsMvp = true;
				}
			}
			if (Singleton<BattleLogic>.GetInstance().battleStat != null && Singleton<BattleLogic>.GetInstance().battleStat.m_playerKDAStat != null)
			{
				PlayerKDA hostKDA = Singleton<BattleLogic>.GetInstance().battleStat.m_playerKDAStat.GetHostKDA();
				if (hostKDA != null)
				{
					this.m_iHostPlaterKillNum = hostKDA.numKill;
					this.m_iHostPlaterDeadNum = hostKDA.numDead;
					this.m_iHostPlaterAssistNum = hostKDA.numAssist;
				}
			}
			this.StopMomentsRecording();
		}

		private int GetAssistCountWithTime(float fStartTime, float fEndTime)
		{
			int num = 0;
			SortedList<long, long> sortedList = null;
			if (this.m_stRecordInfo != null && this.m_stRecordInfo.TryGetValue(CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_ASSIST, ref sortedList) && sortedList != null)
			{
				IEnumerator<KeyValuePair<long, long>> enumerator = sortedList.GetEnumerator();
				while (enumerator.MoveNext())
				{
					KeyValuePair<long, long> current = enumerator.get_Current();
					if (fStartTime <= (float)current.get_Key())
					{
						KeyValuePair<long, long> current2 = enumerator.get_Current();
						if ((float)current2.get_Value() <= fEndTime)
						{
							num++;
							continue;
						}
					}
					KeyValuePair<long, long> current3 = enumerator.get_Current();
					if ((float)current3.get_Value() > fEndTime)
					{
						break;
					}
				}
			}
			return num;
		}

		private void InsertAssistInfo(ref SortedList<int, SortedList<long, long>> assistInfo, int iAssistCount, long lStartTime, long lEndTime)
		{
			if (assistInfo == null)
			{
				assistInfo = new SortedList<int, SortedList<long, long>>();
			}
			SortedList<long, long> sortedList = null;
			if (!assistInfo.TryGetValue(iAssistCount, ref sortedList))
			{
				sortedList = new SortedList<long, long>();
				assistInfo.Add(iAssistCount, sortedList);
			}
			if (sortedList != null && !sortedList.ContainsKey(lStartTime))
			{
				sortedList.Add(lStartTime, lEndTime);
			}
		}

		private void ChooseTopEvent()
		{
			int num = 0;
			int num2 = 0;
			bool flag = false;
			SortedList<long, CRecordUseSDK.RECORD_INFO> sortedList = new SortedList<long, CRecordUseSDK.RECORD_INFO>();
			SortedList<long, long> sortedList2 = null;
			for (CRecordUseSDK.RECORD_EVENT_PRIORITY rECORD_EVENT_PRIORITY = CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_PENTAKILL; rECORD_EVENT_PRIORITY > CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_DOUBLEKILL; rECORD_EVENT_PRIORITY--)
			{
				if (this.m_stRecordInfo != null && this.m_stRecordInfo.TryGetValue(rECORD_EVENT_PRIORITY, ref sortedList2) && sortedList2 != null)
				{
					IEnumerator<KeyValuePair<long, long>> enumerator = sortedList2.GetEnumerator();
					while (enumerator.MoveNext())
					{
						num2++;
						if ((long)num2 > (long)((ulong)this.m_uiEventNumMax))
						{
							flag = true;
							break;
						}
						KeyValuePair<long, long> current = enumerator.get_Current();
						long arg_9F_0;
						if (current.get_Key() < (long)((ulong)this.m_uiEventStartTimeInterval))
						{
							arg_9F_0 = 0L;
						}
						else
						{
							KeyValuePair<long, long> current2 = enumerator.get_Current();
							arg_9F_0 = current2.get_Key() - (long)((ulong)this.m_uiEventStartTimeInterval);
						}
						long num3 = arg_9F_0;
						KeyValuePair<long, long> current3 = enumerator.get_Current();
						long num4 = current3.get_Value() + (long)((ulong)this.m_uiEventEndTimeInterval);
						num4 = ((num4 <= this.m_lGameEndTime) ? num4 : this.m_lGameEndTime);
						num += (int)(num4 - num3);
						if ((long)num > (long)((ulong)this.m_ui543KillEventTotalTime))
						{
							flag = true;
							break;
						}
						if (!sortedList.ContainsKey(num3))
						{
							sortedList.Add(num3, new CRecordUseSDK.RECORD_INFO(rECORD_EVENT_PRIORITY, num4));
						}
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (!flag && (long)num < (long)((ulong)this.m_ui210KillEventTotalTime))
			{
				bool flag2 = false;
				SortedList<int, SortedList<long, long>> sortedList3 = null;
				SortedList<long, long> sortedList4 = null;
				if (this.m_stRecordInfo != null && this.m_stRecordInfo.TryGetValue(CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_ASSIST, ref sortedList4))
				{
					flag2 = true;
				}
				for (CRecordUseSDK.RECORD_EVENT_PRIORITY rECORD_EVENT_PRIORITY2 = CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_DOUBLEKILL; rECORD_EVENT_PRIORITY2 > CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_ASSIST; rECORD_EVENT_PRIORITY2--)
				{
					if (this.m_stRecordInfo != null && this.m_stRecordInfo.TryGetValue(rECORD_EVENT_PRIORITY2, ref sortedList2) && sortedList2 != null)
					{
						IEnumerator<KeyValuePair<long, long>> enumerator2 = sortedList2.GetEnumerator();
						while (enumerator2.MoveNext())
						{
							KeyValuePair<long, long> current4 = enumerator2.get_Current();
							long arg_1ED_0;
							if (current4.get_Key() < (long)((ulong)this.m_uiEventStartTimeInterval))
							{
								arg_1ED_0 = 0L;
							}
							else
							{
								KeyValuePair<long, long> current5 = enumerator2.get_Current();
								arg_1ED_0 = current5.get_Key() - (long)((ulong)this.m_uiEventStartTimeInterval);
							}
							long num5 = arg_1ED_0;
							KeyValuePair<long, long> current6 = enumerator2.get_Current();
							long num6 = current6.get_Value() + (long)((ulong)this.m_uiEventEndTimeInterval) - (long)((ulong)this.m_uiOnceDoubleEventTimeIntervalReduce);
							num6 = ((num6 <= this.m_lGameEndTime) ? num6 : this.m_lGameEndTime);
							if (!flag2)
							{
								num2++;
								if ((long)num2 > (long)((ulong)this.m_uiEventNumMax))
								{
									flag = true;
									break;
								}
								num += (int)(num6 - num5);
								if ((long)num > (long)((ulong)this.m_ui210KillEventTotalTime))
								{
									flag = true;
									break;
								}
								if (!sortedList.ContainsKey(num5))
								{
									sortedList.Add(num5, new CRecordUseSDK.RECORD_INFO(rECORD_EVENT_PRIORITY2, num6));
								}
							}
							else
							{
								int assistCountWithTime = this.GetAssistCountWithTime((float)num5, (float)num6);
								this.InsertAssistInfo(ref sortedList3, assistCountWithTime, num5, num6);
							}
						}
						if (flag2 && sortedList3 != null && sortedList3.get_Count() > 0)
						{
							int count = sortedList3.get_Count();
							for (int i = count - 1; i >= 0; i--)
							{
								sortedList2 = sortedList3.get_Values().get_Item(i);
								if (sortedList2 != null)
								{
									IEnumerator<KeyValuePair<long, long>> enumerator3 = sortedList2.GetEnumerator();
									while (enumerator3.MoveNext())
									{
										KeyValuePair<long, long> current7 = enumerator3.get_Current();
										long key = current7.get_Key();
										KeyValuePair<long, long> current8 = enumerator3.get_Current();
										long value = current8.get_Value();
										num2++;
										if ((long)num2 > (long)((ulong)this.m_uiEventNumMax))
										{
											flag = true;
											break;
										}
										num += (int)(value - key);
										if ((long)num > (long)((ulong)this.m_ui210KillEventTotalTime))
										{
											flag = true;
											break;
										}
										if (!sortedList.ContainsKey(key))
										{
											sortedList.Add(key, new CRecordUseSDK.RECORD_INFO(rECORD_EVENT_PRIORITY2, value));
										}
									}
								}
							}
							sortedList3.Clear();
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (!flag && this.m_stRecordInfo != null && this.m_stRecordInfo.TryGetValue(CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_ASSIST, ref sortedList2) && sortedList2 != null)
				{
					IEnumerator<KeyValuePair<long, long>> enumerator4 = sortedList2.GetEnumerator();
					while (enumerator4.MoveNext())
					{
						KeyValuePair<long, long> current9 = enumerator4.get_Current();
						long arg_431_0;
						if (current9.get_Key() < (long)((ulong)this.m_uiEventStartTimeInterval))
						{
							arg_431_0 = 0L;
						}
						else
						{
							KeyValuePair<long, long> current10 = enumerator4.get_Current();
							arg_431_0 = current10.get_Key() - (long)((ulong)this.m_uiEventStartTimeInterval);
						}
						long num7 = arg_431_0;
						KeyValuePair<long, long> current11 = enumerator4.get_Current();
						long num8 = current11.get_Value() + (long)((ulong)this.m_uiEventEndTimeInterval) - (long)((ulong)this.m_uiOnceDoubleEventTimeIntervalReduce);
						num8 = ((num8 <= this.m_lGameEndTime) ? num8 : this.m_lGameEndTime);
						num2++;
						if ((long)num2 > (long)((ulong)this.m_uiEventNumMax))
						{
							break;
						}
						num += (int)(num8 - num7);
						if ((long)num > (long)((ulong)this.m_ui210KillEventTotalTime))
						{
							break;
						}
						if (!sortedList.ContainsKey(num7))
						{
							sortedList.Add(num7, new CRecordUseSDK.RECORD_INFO(CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_ASSIST, num8));
						}
					}
				}
			}
			List<TimeStamp> list = new List<TimeStamp>();
			long num9 = 0L;
			long num10 = 0L;
			IEnumerator<KeyValuePair<long, CRecordUseSDK.RECORD_INFO>> enumerator5 = sortedList.GetEnumerator();
			while (enumerator5.MoveNext())
			{
				KeyValuePair<long, CRecordUseSDK.RECORD_INFO> current12 = enumerator5.get_Current();
				long num11 = current12.get_Key();
				KeyValuePair<long, CRecordUseSDK.RECORD_INFO> current13 = enumerator5.get_Current();
				long lEndTime = current13.get_Value().lEndTime;
				if (list.get_Count() > 0 && num9 > num11)
				{
					list.RemoveAt(list.get_Count() - 1);
					num -= (int)(num9 - num10);
					num9 = (num9 + num11) / 2L;
					num11 = num9;
					num += (int)(num9 - num10);
					list.Add(new TimeStamp(num10, num9));
				}
				num10 = num11;
				num9 = lEndTime;
				list.Add(new TimeStamp(num11, lEndTime));
			}
			this.m_bIsCallGameJoyGenerate = true;
			Singleton<GameJoy>.get_instance().GenerateMomentsVideo(list, this.GetVideoName(), this.GetExtraInfos());
		}

		public bool OpenRecorderCheck(GameObject KingBar)
		{
			this.m_objKingBar = KingBar;
			this.m_enmCheckWhiteListStatus = CRecordUseSDK.CHECK_WHITELIST_STATUS.CHECK_WHITELIST_STATUS_TYPE_INVALID;
			Singleton<CUIManager>.GetInstance().OpenSendMsgAlert(null, 3, enUIEventID.Record_Check_WhiteList_TimeUp);
			this.CheckWhiteList();
			return false;
		}

		private void CheckPermission()
		{
			GameJoy.checkSDKPermission();
		}

		private void CheckWhiteList()
		{
			GameJoy.CheckRecorderAvailability();
		}

		private bool CheckStorage()
		{
			bool result = true;
			long num = 0L;
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.os.StatFs", new object[]
			{
				Application.persistentDataPath
			}))
			{
				num = (long)androidJavaObject.Call<int>("getBlockSize", new object[0]) * (long)androidJavaObject.Call<int>("getAvailableBlocks", new object[0]) / 1024L / 1024L;
			}
			if (num < (long)((ulong)this.m_uiMinSpaceLimit))
			{
				this.SetKingBarSliderState(false);
				Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("ReplayKit_Disk_Space_Limit"), false, 1.5f, null, new object[0]);
				result = false;
			}
			return result;
		}

		public void OpenMsgBoxForMomentRecorder(Transform container)
		{
			if (!this.m_bIsRecordMomentsEnable)
			{
				return;
			}
			if (container == null)
			{
				return;
			}
			if (this.m_stRecordInfo == null || this.m_stRecordInfo.get_Count() == 0)
			{
				return;
			}
			if (this.m_lVideoTimeLen <= 0L)
			{
				Singleton<CUIManager>.get_instance().OpenTips("RecordMoment_EndGame_Tips_NoRecorderExist", true, 1.5f, null, new object[0]);
				return;
			}
			this.m_RecorderPanel = container;
			if (GameSettings.EnableKingTimeMode && this.m_bIsStartRecordOk)
			{
				Transform transform = container.FindChild("Extra/Image/Image/Text");
				if (transform && transform.gameObject)
				{
					Text component = transform.gameObject.GetComponent<Text>();
					if (component)
					{
						component.text = Singleton<CTextManager>.GetInstance().GetText("RecordSaveMomentVideo");
					}
				}
				container.gameObject.CustomSetActive(true);
			}
		}

		private void CloseRecorderPanel()
		{
			if (this.m_RecorderPanel != null)
			{
				Transform transform = this.m_RecorderPanel.FindChild("Extra");
				if (transform)
				{
					transform.gameObject.CustomSetActive(false);
				}
			}
		}

		private void OnSaveMomentVideo(CUIEvent uiEvent)
		{
			Vector3 recorderPosition = this.GetRecorderPosition();
			Singleton<GameJoy>.get_instance().SetDefaultUploadShareDialogPosition(recorderPosition.x, recorderPosition.y);
			this.CloseRecorderPanel();
			this.ChooseTopEvent();
		}

		private void OnSaveMomentVideoCancel(CUIEvent uiEvent)
		{
			this.CloseRecorderPanel();
			this.CallGameJoyGenerateWithNothing();
		}

		public void CallGameJoyGenerateWithNothing()
		{
			if (this.m_bIsRecordMomentsEnable && this.m_bIsStartRecordOk && !this.m_bIsCallGameJoyGenerate)
			{
				this.m_bIsCallGameJoyGenerate = true;
				Singleton<GameJoy>.get_instance().GenerateMomentsVideo(null, null, null);
			}
		}

		public void StopMomentsRecording()
		{
			if (this.m_bIsRecordMomentsEnable && this.m_bIsStartRecordOk && !this.m_bIsCallStopGameJoyRecord)
			{
				this.m_bIsCallStopGameJoyRecord = true;
				Singleton<GameJoy>.get_instance().EndMomentsRecording();
			}
		}

		private int ConvertMaxMultiKillPriorityToResDef()
		{
			int result = -1;
			if (this.m_stRecordInfo.get_Count() > 0)
			{
				if (this.m_stRecordInfo.ContainsKey(CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_PENTAKILL))
				{
					result = 6;
				}
				else if (this.m_stRecordInfo.ContainsKey(CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_QUATARYKILL))
				{
					result = 5;
				}
				else if (this.m_stRecordInfo.ContainsKey(CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_TRIPLEKILL))
				{
					result = 4;
				}
				else if (this.m_stRecordInfo.ContainsKey(CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_DOUBLEKILL))
				{
					result = 3;
				}
				else if (this.m_stRecordInfo.ContainsKey(CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_ONCEKILL))
				{
					result = 2;
				}
				else if (this.m_stRecordInfo.ContainsKey(CRecordUseSDK.RECORD_EVENT_PRIORITY.RECORD_EVENT_TYPE_ASSIST))
				{
					result = 0;
				}
			}
			return result;
		}

		private Dictionary<string, string> GetExtraInfos()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			SLevelContext curLvelContext = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
			if (curLvelContext != null)
			{
				dictionary.Add("MODE", curLvelContext.m_gameMatchName);
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				ResRankGradeConf dataByKey = GameDataMgr.rankGradeDatabin.GetDataByKey((uint)masterRoleInfo.m_rankGrade);
				if (dataByKey != null)
				{
					dictionary.Add("GRADE", dataByKey.szGradeDesc);
				}
			}
			if (!string.IsNullOrEmpty(this.m_strHostPlayerName))
			{
				int num = this.m_strHostPlayerName.IndexOf('(');
				string text = this.m_strHostPlayerName.Substring(num + 1, this.m_strHostPlayerName.get_Length() - num - 2);
				dictionary.Add("HERO", text);
			}
			dictionary.Add("KILLNUM", this.m_iHostPlaterKillNum.ToString());
			dictionary.Add("DEADNUM", this.m_iHostPlaterDeadNum.ToString());
			dictionary.Add("ASSISTNUM", this.m_iHostPlaterAssistNum.ToString());
			int num2 = this.ConvertMaxMultiKillPriorityToResDef();
			if (num2 > 2)
			{
				ResMultiKill dataByKey2 = GameDataMgr.multiKillDatabin.GetDataByKey((long)num2);
				if (dataByKey2 != null)
				{
					dictionary.Add("MULTIKILL", dataByKey2.szAchievementName);
				}
			}
			return dictionary;
		}

		private string GetVideoName()
		{
			string text = Singleton<CTextManager>.GetInstance().GetText("RecordMomentVideoNameHeader");
			SLevelContext curLvelContext = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
			if (curLvelContext != null)
			{
				text += curLvelContext.m_levelName;
				if (curLvelContext.IsGameTypeLadder())
				{
					text += curLvelContext.m_gameMatchName;
				}
			}
			if (this.m_bIsMvp)
			{
				text += "MVP";
			}
			if (!string.IsNullOrEmpty(this.m_strHostPlayerName))
			{
				int num = this.m_strHostPlayerName.IndexOf('(');
				string text2 = this.m_strHostPlayerName.Substring(num + 1, this.m_strHostPlayerName.get_Length() - num - 2);
				text += text2;
			}
			int num2 = this.ConvertMaxMultiKillPriorityToResDef();
			if (num2 > 2)
			{
				ResMultiKill dataByKey = GameDataMgr.multiKillDatabin.GetDataByKey((long)num2);
				if (dataByKey != null)
				{
					text += dataByKey.szAchievementName;
				}
			}
			return text + Singleton<CTextManager>.GetInstance().GetText("RecordMomentVideoNameTail");
		}

		private Vector3 GetRecorderPosition()
		{
			Vector3 result = Vector3.zero;
			if (this.m_RecorderPanel)
			{
				Transform transform = this.m_RecorderPanel.FindChild("Record");
				if (transform)
				{
					Camera camera = Camera.current;
					if (camera == null)
					{
						camera = Camera.allCameras[0];
					}
					if (camera)
					{
						result = camera.WorldToViewportPoint(transform.transform.position);
					}
				}
			}
			return result;
		}

		private void OnGameJoyStartRecordResult(bool bRes)
		{
			this.m_bIsStartRecordOk = bRes;
			if (bRes)
			{
				this.m_lGameStartTime = GameJoy.get_getSystemCurrentTimeMillis();
			}
		}

		private void OnGameJoyStopRecordResult(long lDuration)
		{
			this.m_lVideoTimeLen = lDuration;
		}

		private void OnBtnVideoMgrClick(CUIEvent cuiEvent)
		{
			this.m_enmCheckWhiteListStatus = CRecordUseSDK.CHECK_WHITELIST_STATUS.CHECK_WHITELIST_STATUS_TYPE_VIDEOMGRCLICK;
			Singleton<CUIManager>.GetInstance().OpenSendMsgAlert(null, 3, enUIEventID.Record_Check_WhiteList_TimeUp);
			this.CheckWhiteList();
		}

		public bool GetRecorderGlobalCfgEnableFlag()
		{
			bool result = false;
			if (GameDataMgr.svr2CltCfgDict != null && GameDataMgr.svr2CltCfgDict.ContainsKey(12u))
			{
				ResGlobalInfo resGlobalInfo = new ResGlobalInfo();
				if (GameDataMgr.svr2CltCfgDict.TryGetValue(12u, ref resGlobalInfo))
				{
					result = (resGlobalInfo.dwConfValue > 0u);
				}
			}
			return result;
		}

		private void OnGameJoyCheckPermissionResult(bool bRes)
		{
			this.m_enmCheckPermissionRes = ((!bRes) ? CRecordUseSDK.CHECK_PERMISSION_STUTAS.CHECK_PERMISSION_STUTAS_TYPE_NOPERMISSION : CRecordUseSDK.CHECK_PERMISSION_STUTAS.CHECK_PERMISSION_STUTAS_TYPE_PERMISSIONOK);
			if (this.m_objKingBar != null && !bRes)
			{
				this.SetKingBarSliderState(false);
				Singleton<CUIManager>.get_instance().OpenTips("GameJoyCheckPermissionFailed", true, 1.5f, null, new object[0]);
			}
		}

		private bool CheckStorageAndPermission()
		{
			if (!this.CheckStorage())
			{
				return false;
			}
			this.CheckPermission();
			return true;
		}

		private void OnGameJoyCheckAvailabilityResult(bool bRes)
		{
			if (this.m_enmCheckWhiteListStatus != CRecordUseSDK.CHECK_WHITELIST_STATUS.CHECK_WHITELIST_STATUS_TYPE_TIMEUP && this.m_enmCheckWhiteListStatus != CRecordUseSDK.CHECK_WHITELIST_STATUS.CHECK_WHITELIST_STATUS_TYPE_AUTOCHECK)
			{
				Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
				if (!bRes)
				{
					Singleton<CUIManager>.get_instance().OpenTips("GamejoyCheckAvailabilityFailed", true, 1.5f, null, new object[0]);
					this.SetKingBarSliderState(false);
				}
				if (this.m_enmCheckWhiteListStatus == CRecordUseSDK.CHECK_WHITELIST_STATUS.CHECK_WHITELIST_STATUS_TYPE_VIDEOMGRCLICK)
				{
					if (bRes)
					{
						Singleton<GameJoy>.get_instance().ShowVideoListDialog();
					}
					this.m_enmCheckWhiteListStatus = CRecordUseSDK.CHECK_WHITELIST_STATUS.CHECK_WHITELIST_STATUS_TYPE_INVALID;
					return;
				}
				this.m_enmCheckWhiteListStatus = ((!bRes) ? CRecordUseSDK.CHECK_WHITELIST_STATUS.CHECK_WHITELIST_STATUS_TYPE_RESULTFAILED : CRecordUseSDK.CHECK_WHITELIST_STATUS.CHECK_WHITELIST_STATUS_TYPE_RESULTOK);
				if (this.m_enmCheckWhiteListStatus == CRecordUseSDK.CHECK_WHITELIST_STATUS.CHECK_WHITELIST_STATUS_TYPE_RESULTOK)
				{
					this.CheckStorageAndPermission();
				}
			}
			else if (this.m_enmCheckWhiteListStatus == CRecordUseSDK.CHECK_WHITELIST_STATUS.CHECK_WHITELIST_STATUS_TYPE_AUTOCHECK)
			{
				this.m_enmCheckWhiteListStatus = ((!bRes) ? CRecordUseSDK.CHECK_WHITELIST_STATUS.CHECK_WHITELIST_STATUS_TYPE_RESULTFAILED : CRecordUseSDK.CHECK_WHITELIST_STATUS.CHECK_WHITELIST_STATUS_TYPE_RESULTOK);
			}
		}

		public void SetKingBarSliderState(bool bIsOpen)
		{
			if (this.m_objKingBar != null)
			{
				Transform transform = this.m_objKingBar.transform.FindChild("Slider");
				if (transform)
				{
					CUISliderEventScript component = transform.GetComponent<CUISliderEventScript>();
					int num = (!bIsOpen) ? 0 : 1;
					if ((int)component.value != num)
					{
						component.value = (float)num;
					}
				}
				else if (GameSettings.EnableKingTimeMode != bIsOpen)
				{
					GameSettings.EnableKingTimeMode = bIsOpen;
				}
			}
			else if (GameSettings.EnableKingTimeMode != bIsOpen)
			{
				GameSettings.EnableKingTimeMode = bIsOpen;
			}
		}

		private void OnCheckWhiteListTimeUp(CUIEvent uiEvent)
		{
			if (this.m_enmCheckWhiteListStatus == CRecordUseSDK.CHECK_WHITELIST_STATUS.CHECK_WHITELIST_STATUS_TYPE_INVALID)
			{
				this.SetKingBarSliderState(false);
			}
			this.m_enmCheckWhiteListStatus = CRecordUseSDK.CHECK_WHITELIST_STATUS.CHECK_WHITELIST_STATUS_TYPE_TIMEUP;
		}

		public void OnBadGameEnd()
		{
			if (this.m_bIsStartRecordOk && this.m_bIsRecordMomentsEnable)
			{
				CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(SettlementSystem.SettlementFormName);
				if (form && form.gameObject.activeSelf)
				{
					return;
				}
				this.StopMomentsRecording();
				this.CallGameJoyGenerateWithNothing();
			}
		}
	}
}
