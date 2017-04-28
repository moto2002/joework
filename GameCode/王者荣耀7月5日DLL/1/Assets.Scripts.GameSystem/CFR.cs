using CSProtocol;
using System;

namespace Assets.Scripts.GameSystem
{
	public class CFR
	{
		public bool bRedDot;

		public ulong ulluid;

		public uint worldID;

		public COM_INTIMACY_STATE state;

		public COM_INTIMACY_RELATION_CHG_TYPE op;

		public bool bReciveOthersRequest;

		private uint timeStamp;

		public int choiseRelation = -1;

		public bool bInShowChoiseRelaList;

		private COMDT_FRIEND_INFO _friendInfo;

		public int CDDays
		{
			get;
			protected set;
		}

		public COMDT_FRIEND_INFO friendInfo
		{
			get
			{
				if (this._friendInfo == null)
				{
					this._friendInfo = Singleton<CFriendContoller>.get_instance().model.getFriendByUid(this.ulluid, CFriendModel.FriendType.GameFriend, 0u);
				}
				return this._friendInfo;
			}
		}

		public CFR(ulong ulluid, uint worldId, COM_INTIMACY_STATE state, COM_INTIMACY_RELATION_CHG_TYPE op, uint timeStamp, bool bReciveOthersRequest)
		{
			this.SetData(ulluid, worldId, state, op, timeStamp, bReciveOthersRequest);
		}

		public void SetData(ulong ulluid, uint worldId, COM_INTIMACY_STATE state, COM_INTIMACY_RELATION_CHG_TYPE op, uint timeStamp, bool bReciveOthersRequest)
		{
			this.ulluid = ulluid;
			this.worldID = worldId;
			this.state = state;
			this.op = op;
			this.SetTimeStamp(timeStamp);
			this.bReciveOthersRequest = bReciveOthersRequest;
			this._friendInfo = null;
			COMDT_FRIEND_INFO friendInfo = this.friendInfo;
			if (bReciveOthersRequest && (state == 20 || state == 21 || state == 22 || state == 23))
			{
				this.bRedDot = true;
				Singleton<EventRouter>.get_instance().BroadCastEvent("Friend_LobbyIconRedDot_Refresh");
			}
		}

		private void SetTimeStamp(uint ts)
		{
			this.timeStamp = ts;
			this.CDDays = CFR.GetCDDays(ts);
		}

		public static int GetCDDays(uint ts)
		{
			if (ts == 0u)
			{
				return -1;
			}
			DateTime dateTime = Utility.ToUtcTime2Local((long)((ulong)ts)).AddDays(Singleton<CFriendContoller>.get_instance().model.FRData.InitmacyLimitTime);
			DateTime dateTime2 = Utility.ToUtcTime2Local((long)CRoleInfo.GetCurrentUTCTime());
			if (DateTime.Compare(dateTime, dateTime2) <= 0)
			{
				return -1;
			}
			TimeSpan timeSpan = dateTime - dateTime2;
			if (timeSpan.get_Days() < 1)
			{
				return 1;
			}
			return timeSpan.get_Days();
		}

		public void Clear()
		{
			this._friendInfo = null;
		}
	}
}
