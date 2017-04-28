using Assets.Scripts.Framework;
using CSProtocol;
using System;

namespace Assets.Scripts.GameSystem
{
	public class CFriendRelationship
	{
		public class FRConfig
		{
			public int piority = -1;

			public COM_INTIMACY_STATE state;

			public string cfgRelaStr;

			public FRConfig(int piority, COM_INTIMACY_STATE state, string cfgRelaStr)
			{
				this.piority = piority;
				this.state = state;
				this.cfgRelaStr = cfgRelaStr;
			}
		}

		public ListView<CFriendRelationship.FRConfig> frConfig_list = new ListView<CFriendRelationship.FRConfig>();

		private uint _initmacyLimitTime;

		private ListView<CFR> m_cfrList = new ListView<CFR>();

		private COM_INTIMACY_STATE _state;

		public static CFriendRelationship FRData
		{
			get
			{
				return Singleton<CFriendContoller>.get_instance().model.FRData;
			}
		}

		public uint InitmacyLimitTime
		{
			get
			{
				if (this._initmacyLimitTime == 0u)
				{
					this._initmacyLimitTime = GameDataMgr.GetGlobeValue(295);
				}
				return this._initmacyLimitTime;
			}
		}

		public string IntimRela_Tips_AlreadyHasGay
		{
			get;
			set;
		}

		public string IntimRela_Tips_AlreadyHasLover
		{
			get;
			set;
		}

		public string IntimRela_Type_Gay
		{
			get;
			set;
		}

		public string IntimRela_Type_Lover
		{
			get;
			set;
		}

		public string IntimRela_Tips_SendRequestGaySuccess
		{
			get;
			set;
		}

		public string IntimRela_Tips_SendRequestLoverSuccess
		{
			get;
			set;
		}

		public string IntimRela_Tips_SendDelGaySuccess
		{
			get;
			set;
		}

		public string IntimRela_Tips_SendDelLoverSuccess
		{
			get;
			set;
		}

		public string IntimRela_Tips_DenyYourRequestGay
		{
			get;
			set;
		}

		public string IntimRela_Tips_DenyYourRequestLover
		{
			get;
			set;
		}

		public string IntimRela_Tips_DenyYourDelGay
		{
			get;
			set;
		}

		public string IntimRela_Tips_DenyYourDelLover
		{
			get;
			set;
		}

		public string IntimRela_CD_CountDown
		{
			get;
			set;
		}

		public string IntimRela_Tips_ReceiveOtherReqRela
		{
			get;
			set;
		}

		public string IntimRela_Tips_Wait4TargetRspReqRela
		{
			get;
			set;
		}

		public string IntimRela_Tips_ReceiveOtherDelRela
		{
			get;
			set;
		}

		public string IntimRela_Tips_Wait4TargetRspDelRela
		{
			get;
			set;
		}

		public string IntimRela_Tips_SelectRelation
		{
			get;
			set;
		}

		public string IntimRela_Tips_MidText
		{
			get;
			set;
		}

		public string IntimRela_Tips_RelaHasDel
		{
			get;
			set;
		}

		public string IntimRela_Tips_OK
		{
			get;
			set;
		}

		public string IntimRela_Tips_Cancle
		{
			get;
			set;
		}

		public string IntimRela_DoFristChoise
		{
			get;
			set;
		}

		public string IntimRela_AleadyFristChoise
		{
			get;
			set;
		}

		public string IntimRela_ReselectRelation
		{
			get;
			set;
		}

		public string IntimRela_ReDelRelation
		{
			get;
			set;
		}

		public string IntimRela_EmptyDataText
		{
			get;
			set;
		}

		public string IntimRela_TypeColor_Gay
		{
			get;
			set;
		}

		public string IntimRela_TypeColor_Lover
		{
			get;
			set;
		}

		public COM_INTIMACY_STATE GetFirstChoiseRelation()
		{
			return 1;
		}

		public void Clear()
		{
			for (int i = 0; i < this.m_cfrList.get_Count(); i++)
			{
				CFR cFR = this.m_cfrList.get_Item(i);
				if (cFR != null)
				{
					cFR.Clear();
				}
			}
			this.m_cfrList.Clear();
			this._initmacyLimitTime = 0u;
		}

		public ListView<CFR> GetList()
		{
			return this.m_cfrList;
		}

		public bool HasRedDot()
		{
			for (int i = 0; i < this.m_cfrList.get_Count(); i++)
			{
				CFR cFR = this.m_cfrList.get_Item(i);
				if (cFR != null && cFR.bRedDot)
				{
					return true;
				}
			}
			return false;
		}

		public void FindSetState(COM_INTIMACY_STATE target1, COM_INTIMACY_STATE target2, COM_INTIMACY_STATE newState)
		{
			for (int i = 0; i < this.m_cfrList.get_Count(); i++)
			{
				CFR cFR = this.m_cfrList.get_Item(i);
				if (cFR != null && (cFR.state == target1 || cFR.state == target2))
				{
					cFR.state = newState;
				}
			}
		}

		public void FindSetState(COM_INTIMACY_STATE target1, COM_INTIMACY_STATE newState)
		{
			for (int i = 0; i < this.m_cfrList.get_Count(); i++)
			{
				CFR cFR = this.m_cfrList.get_Item(i);
				if (cFR != null && cFR.state == target1)
				{
					cFR.state = newState;
				}
			}
		}

		public CFR FindFrist(COM_INTIMACY_STATE state)
		{
			for (int i = 0; i < this.m_cfrList.get_Count(); i++)
			{
				CFR cFR = this.m_cfrList.get_Item(i);
				if (cFR != null && cFR.state == state)
				{
					return cFR;
				}
			}
			return null;
		}

		public void ProcessFriendList(COMDT_ACNT_UNIQ uniq, COMDT_INTIMACY_DATA data)
		{
			if (uniq == null || data == null)
			{
				return;
			}
			byte bIntimacyState = data.bIntimacyState;
			CFR cfr = this.GetCfr(uniq.ullUid, uniq.dwLogicWorldId);
			if (cfr != null && cfr.state != null)
			{
				return;
			}
			if (CFR.GetCDDays(data.dwTerminateTime) != -1)
			{
				this.Add(uniq.ullUid, uniq.dwLogicWorldId, bIntimacyState, 0, data.dwTerminateTime, false);
				return;
			}
			if (data.bIntimacyState == 0 && (uint)data.wIntimacyValue == Singleton<CFriendContoller>.get_instance().model.GetMaxIntimacyNum())
			{
				this.Add(uniq.ullUid, uniq.dwLogicWorldId, 24, 0, data.dwTerminateTime, false);
				return;
			}
			if (bIntimacyState != 0)
			{
				this.Add(uniq.ullUid, uniq.dwLogicWorldId, bIntimacyState, 0, data.dwTerminateTime, false);
			}
		}

		public int GetCount()
		{
			return this.m_cfrList.get_Count();
		}

		public void ProcessOtherRequest(CSDT_VERIFICATION_INFO data, bool bReceiveNtf = false)
		{
			this.Add(data.stFriendInfo.stUin.ullUid, data.stFriendInfo.stUin.dwLogicWorldId, data.bIntimacyState, 1, 0u, bReceiveNtf);
		}

		public void Add(ulong ulluid, uint worldId, byte state, byte op, uint timeStamp, bool bReceiveNtf = false)
		{
			this.Add(ulluid, worldId, state, op, timeStamp, bReceiveNtf);
		}

		public void Add(ulong ulluid, uint worldId, COM_INTIMACY_STATE state, COM_INTIMACY_RELATION_CHG_TYPE op, uint timeStamp, bool bReceiveNtf = false)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo();
			if (masterRoleInfo != null && masterRoleInfo.playerUllUID == ulluid && (long)masterRoleInfo.logicWorldID == (long)((ulong)worldId))
			{
				return;
			}
			CFR cfr = this.GetCfr(ulluid, worldId);
			if (cfr == null)
			{
				this.m_cfrList.Add(new CFR(ulluid, worldId, state, op, timeStamp, bReceiveNtf));
			}
			else
			{
				cfr.SetData(ulluid, worldId, state, op, timeStamp, bReceiveNtf);
			}
			if (state == 1)
			{
				this.FindSetState(20, 24);
			}
			if (state == 2)
			{
				this.FindSetState(22, 24);
			}
			Singleton<EventRouter>.GetInstance().BroadCastEvent("FRDataChange");
		}

		public void Remove(ulong ulluid, uint worldID)
		{
			int num = -1;
			for (int i = 0; i < this.m_cfrList.get_Count(); i++)
			{
				CFR cFR = this.m_cfrList.get_Item(i);
				if (cFR != null && cFR.ulluid == ulluid && cFR.worldID == worldID)
				{
					num = i;
				}
			}
			if (num != -1)
			{
				this.m_cfrList.RemoveAt(num);
				Singleton<EventRouter>.GetInstance().BroadCastEvent("FRDataChange");
			}
		}

		public CFR GetCfr(ulong ulluid, uint worldID)
		{
			for (int i = 0; i < this.m_cfrList.get_Count(); i++)
			{
				CFR cFR = this.m_cfrList.get_Item(i);
				if (cFR != null && cFR.ulluid == ulluid && cFR.worldID == worldID)
				{
					return cFR;
				}
			}
			return null;
		}

		public void ResetChoiseRelaState(ulong ulluid, uint worldID)
		{
			CFR cfr = this.GetCfr(ulluid, worldID);
			if (cfr != null)
			{
				cfr.choiseRelation = -1;
				cfr.bInShowChoiseRelaList = false;
			}
		}

		public void Sort()
		{
		}

		public int GetCandiRelationCount()
		{
			return 2;
		}

		public void LoadConfig()
		{
			this.frConfig_list.Clear();
			this.frConfig_list.Add(new CFriendRelationship.FRConfig(0, 1, Singleton<CTextManager>.get_instance().GetText("IntimRela_Type_Gay")));
			this.frConfig_list.Add(new CFriendRelationship.FRConfig(1, 2, Singleton<CTextManager>.get_instance().GetText("IntimRela_Type_Lover")));
			this.IntimRela_Tips_AlreadyHasGay = Singleton<CTextManager>.get_instance().GetText("IntimRela_Tips_AlreadyHasGay");
			this.IntimRela_Tips_AlreadyHasLover = Singleton<CTextManager>.get_instance().GetText("IntimRela_Tips_AlreadyHasLover");
			this.IntimRela_Type_Gay = Singleton<CTextManager>.get_instance().GetText("IntimRela_Type_Gay");
			this.IntimRela_Type_Lover = Singleton<CTextManager>.get_instance().GetText("IntimRela_Type_Lover");
			this.IntimRela_Tips_SendRequestGaySuccess = Singleton<CTextManager>.get_instance().GetText("IntimRela_Tips_SendRequestGaySuccess");
			this.IntimRela_Tips_SendRequestLoverSuccess = Singleton<CTextManager>.get_instance().GetText("IntimRela_Tips_SendRequestLoverSuccess");
			this.IntimRela_Tips_SendDelGaySuccess = Singleton<CTextManager>.get_instance().GetText("IntimRela_Tips_SendDelGaySuccess");
			this.IntimRela_Tips_SendDelLoverSuccess = Singleton<CTextManager>.get_instance().GetText("IntimRela_Tips_SendDelLoverSuccess");
			this.IntimRela_Tips_DenyYourRequestGay = Singleton<CTextManager>.get_instance().GetText("IntimRela_Tips_DenyYourRequestGay");
			this.IntimRela_Tips_DenyYourRequestLover = Singleton<CTextManager>.get_instance().GetText("IntimRela_Tips_DenyYourRequestLover");
			this.IntimRela_Tips_DenyYourDelGay = Singleton<CTextManager>.get_instance().GetText("IntimRela_Tips_DenyYourDelGay");
			this.IntimRela_Tips_DenyYourDelLover = Singleton<CTextManager>.get_instance().GetText("IntimRela_Tips_DenyYourDelLover");
			this.IntimRela_CD_CountDown = Singleton<CTextManager>.get_instance().GetText("IntimRela_CD_CountDown");
			this.IntimRela_Tips_ReceiveOtherReqRela = Singleton<CTextManager>.get_instance().GetText("IntimRela_Tips_ReceiveOtherReqRela");
			this.IntimRela_Tips_Wait4TargetRspReqRela = Singleton<CTextManager>.get_instance().GetText("IntimRela_Tips_Wait4TargetRspReqRela");
			this.IntimRela_Tips_ReceiveOtherDelRela = Singleton<CTextManager>.get_instance().GetText("IntimRela_Tips_ReceiveOtherDelRela");
			this.IntimRela_Tips_Wait4TargetRspDelRela = Singleton<CTextManager>.get_instance().GetText("IntimRela_Tips_Wait4TargetRspDelRela");
			this.IntimRela_Tips_SelectRelation = Singleton<CTextManager>.get_instance().GetText("IntimRela_Tips_SelectRelation");
			this.IntimRela_Tips_MidText = Singleton<CTextManager>.get_instance().GetText("IntimRela_Tips_MidText");
			this.IntimRela_Tips_RelaHasDel = Singleton<CTextManager>.get_instance().GetText("IntimRela_Tips_RelaHasDel");
			this.IntimRela_Tips_OK = Singleton<CTextManager>.get_instance().GetText("IntimRela_Tips_OK");
			this.IntimRela_Tips_Cancle = Singleton<CTextManager>.get_instance().GetText("IntimRela_Tips_Cancle");
			this.IntimRela_DoFristChoise = Singleton<CTextManager>.get_instance().GetText("IntimRela_DoFristChoise");
			this.IntimRela_AleadyFristChoise = Singleton<CTextManager>.get_instance().GetText("IntimRela_AleadyFristChoise");
			this.IntimRela_ReselectRelation = Singleton<CTextManager>.get_instance().GetText("IntimRela_ReselectRelation");
			this.IntimRela_ReDelRelation = Singleton<CTextManager>.get_instance().GetText("IntimRela_ReDelRelation");
			this.IntimRela_EmptyDataText = Singleton<CTextManager>.get_instance().GetText("IntimRela_EmptyDataText");
			this.IntimRela_TypeColor_Gay = Singleton<CTextManager>.get_instance().GetText("IntimRela_TypeColor_Gay");
			this.IntimRela_TypeColor_Lover = Singleton<CTextManager>.get_instance().GetText("IntimRela_TypeColor_Lover");
		}

		public CFriendRelationship.FRConfig GetCFGByIndex(int index)
		{
			for (int i = 0; i < this.frConfig_list.get_Count(); i++)
			{
				CFriendRelationship.FRConfig fRConfig = this.frConfig_list.get_Item(i);
				if (fRConfig != null && fRConfig.piority == index)
				{
					return fRConfig;
				}
			}
			return null;
		}

		public COM_INTIMACY_STATE GetFirstChoiseState()
		{
			return this._state;
		}

		public void SetFirstChoiseState(byte state)
		{
			this.SetFirstChoiseState(state);
		}

		public void SetFirstChoiseState(COM_INTIMACY_STATE state)
		{
			this._state = state;
			Singleton<EventRouter>.GetInstance().BroadCastEvent("FRDataChange");
		}
	}
}
