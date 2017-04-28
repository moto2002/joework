using Assets.Scripts.Framework;
using CSProtocol;
using System;

namespace Assets.Scripts.GameSystem
{
	public class CFriendReCallData
	{
		public class CDFriendReCallData
		{
			public COM_FRIEND_TYPE friendType;

			public ulong ullUid;

			public uint dwLogicWorldId;
		}

		private ListView<CFriendReCallData.CDFriendReCallData> _reCallList = new ListView<CFriendReCallData.CDFriendReCallData>();

		private static uint inviteLimitSec;

		private static uint InviteLimitSec
		{
			get
			{
				if (CFriendReCallData.inviteLimitSec == 0u)
				{
					CFriendReCallData.inviteLimitSec = 86400u * GameDataMgr.globalInfoDatabin.GetDataByKey(158u).dwConfValue;
				}
				return CFriendReCallData.inviteLimitSec;
			}
		}

		public void Clear()
		{
			this._reCallList.Clear();
		}

		public void Add(COMDT_ACNT_UNIQ uniq, COM_FRIEND_TYPE friendType)
		{
			CFriendReCallData.CDFriendReCallData friendData = this.GetFriendData(uniq, friendType);
			if (friendData != null)
			{
				this.RemoveFriendReCallData(uniq, friendType);
			}
			UT.Add2List<CFriendReCallData.CDFriendReCallData>(new CFriendReCallData.CDFriendReCallData
			{
				ullUid = uniq.ullUid,
				dwLogicWorldId = uniq.dwLogicWorldId,
				friendType = friendType
			}, this._reCallList);
		}

		public static bool BLose(COMDT_ACNT_UNIQ uniq, COM_FRIEND_TYPE friendType)
		{
			COMDT_FRIEND_INFO cOMDT_FRIEND_INFO = null;
			if (friendType == 1)
			{
				cOMDT_FRIEND_INFO = Singleton<CFriendContoller>.get_instance().model.getFriendByUid(uniq.ullUid, CFriendModel.FriendType.GameFriend, 0u);
			}
			else if (friendType == 2)
			{
				cOMDT_FRIEND_INFO = Singleton<CFriendContoller>.get_instance().model.getFriendByUid(uniq.ullUid, CFriendModel.FriendType.SNS, 0u);
			}
			return cOMDT_FRIEND_INFO != null && (long)CRoleInfo.GetCurrentUTCTime() - (long)((ulong)cOMDT_FRIEND_INFO.dwLastLoginTime) >= (long)((ulong)CFriendReCallData.InviteLimitSec);
		}

		public bool BInCd(COMDT_ACNT_UNIQ uniq, COM_FRIEND_TYPE friendType)
		{
			int friendReCallDataIndex = this.GetFriendReCallDataIndex(uniq, friendType);
			return friendReCallDataIndex != -1;
		}

		private CFriendReCallData.CDFriendReCallData GetFriendData(COMDT_ACNT_UNIQ uniq, COM_FRIEND_TYPE friendType)
		{
			int friendReCallDataIndex = this.GetFriendReCallDataIndex(uniq, friendType);
			if (friendReCallDataIndex == -1)
			{
				return null;
			}
			return this._reCallList.get_Item(friendReCallDataIndex);
		}

		private void RemoveFriendReCallData(COMDT_ACNT_UNIQ uniq, COM_FRIEND_TYPE friendType)
		{
			int friendReCallDataIndex = this.GetFriendReCallDataIndex(uniq, friendType);
			if (friendReCallDataIndex != -1)
			{
				this._reCallList.RemoveAt(friendReCallDataIndex);
			}
		}

		private int GetFriendReCallDataIndex(COMDT_ACNT_UNIQ uniq, COM_FRIEND_TYPE friendType)
		{
			if (uniq == null)
			{
				return -1;
			}
			for (int i = 0; i < this._reCallList.get_Count(); i++)
			{
				CFriendReCallData.CDFriendReCallData cDFriendReCallData = this._reCallList.get_Item(i);
				if (cDFriendReCallData.ullUid == uniq.ullUid && cDFriendReCallData.dwLogicWorldId == uniq.dwLogicWorldId && cDFriendReCallData.friendType == friendType)
				{
					return i;
				}
			}
			return -1;
		}
	}
}
