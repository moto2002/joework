using CSProtocol;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameSystem
{
	public class RoomInfo
	{
		public int iRoomEntity;

		public uint dwRoomID;

		public uint dwRoomSeq;

		public PlayerUniqueID selfInfo;

		public PlayerUniqueID roomOwner;

		public RoomAttrib roomAttrib;

		private ListView<MemberInfo>[] campMemberList;

		public uint selfObjID;

		public ListView<MemberInfo> this[COM_PLAYERCAMP camp]
		{
			get
			{
				if (camp >= 0 && camp < this.campMemberList.Length)
				{
					return this.campMemberList[camp];
				}
				return null;
			}
		}

		public int CampListCount
		{
			get
			{
				return this.campMemberList.Length;
			}
		}

		public ListView<MemberInfo> this[int campIndex]
		{
			get
			{
				return this.campMemberList[campIndex];
			}
		}

		public RoomInfo()
		{
			this.selfInfo = new PlayerUniqueID();
			this.roomOwner = new PlayerUniqueID();
			this.roomAttrib = new RoomAttrib();
			this.campMemberList = new ListView<MemberInfo>[3];
			for (int i = 0; i < 3; i++)
			{
				this.campMemberList[i] = new ListView<MemberInfo>();
			}
		}

		public void SortCampMemList(COM_PLAYERCAMP camp)
		{
			ListView<MemberInfo> listView = this.campMemberList[camp];
			SortedList<uint, MemberInfo> sortedList = new SortedList<uint, MemberInfo>();
			ListView<MemberInfo>.Enumerator enumerator = listView.GetEnumerator();
			while (enumerator.MoveNext())
			{
				MemberInfo current = enumerator.get_Current();
				uint dwPosOfCamp = current.dwPosOfCamp;
				sortedList.Add(dwPosOfCamp, current);
			}
			this.campMemberList[camp] = new ListView<MemberInfo>(sortedList.get_Values());
		}

		public MemberInfo GetMemberInfo(COM_PLAYERCAMP camp, int posOfCamp)
		{
			MemberInfo result = null;
			ListView<MemberInfo> listView = this[camp];
			if (listView == null)
			{
				return result;
			}
			for (int i = 0; i < listView.get_Count(); i++)
			{
				if ((ulong)listView.get_Item(i).dwPosOfCamp == (ulong)((long)posOfCamp))
				{
					result = listView.get_Item(i);
					break;
				}
			}
			return result;
		}

		public MemberInfo GetMemberInfo(uint objID)
		{
			for (int i = 0; i < this.campMemberList.Length; i++)
			{
				ListView<MemberInfo> listView = this.campMemberList[i];
				for (int j = 0; j < listView.get_Count(); j++)
				{
					if (listView.get_Item(j).dwObjId == objID)
					{
						return listView.get_Item(j);
					}
				}
			}
			return null;
		}

		public MemberInfo GetMemberInfo(ulong playerUid)
		{
			for (int i = 0; i < this.campMemberList.Length; i++)
			{
				ListView<MemberInfo> listView = this.campMemberList[i];
				for (int j = 0; j < listView.get_Count(); j++)
				{
					if (listView.get_Item(j).ullUid == playerUid)
					{
						return listView.get_Item(j);
					}
				}
			}
			return null;
		}

		public MemberInfo GetMasterMemberInfo()
		{
			if (this.selfObjID != 0u)
			{
				return this.GetMemberInfo(this.selfObjID);
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				return this.GetMemberInfo(masterRoleInfo.playerUllUID);
			}
			return null;
		}

		public COM_PLAYERCAMP GetEnemyCamp(COM_PLAYERCAMP inCamp)
		{
			if (inCamp == 1)
			{
				return 2;
			}
			if (inCamp == 2)
			{
				return 1;
			}
			return 0;
		}

		public COM_PLAYERCAMP GetSelfCamp()
		{
			for (int i = 0; i < this.campMemberList.Length; i++)
			{
				for (int j = 0; j < this.campMemberList[i].get_Count(); j++)
				{
					if (this.campMemberList[i].get_Item(j).ullUid == this.selfInfo.ullUid)
					{
						return i;
					}
				}
			}
			return 0;
		}

		public int GetFreePos(COM_PLAYERCAMP camp, int maxPlayerNum)
		{
			int result = -1;
			ListView<MemberInfo> listView = this[camp];
			if (listView == null)
			{
				return result;
			}
			for (int i = 0; i < maxPlayerNum / 2; i++)
			{
				bool flag = false;
				for (int j = 0; j < listView.get_Count(); j++)
				{
					MemberInfo memberInfo = listView.get_Item(j);
					if (memberInfo != null && (ulong)memberInfo.dwPosOfCamp == (ulong)((long)i))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		public bool IsHeroExistWithCamp(COM_PLAYERCAMP camp, uint heroID)
		{
			bool result = false;
			ListView<MemberInfo> listView = this[camp];
			if (listView == null)
			{
				return result;
			}
			for (int i = 0; i < listView.get_Count(); i++)
			{
				if (listView.get_Item(i) == null || listView.get_Item(i).ChoiceHero == null)
				{
				}
				for (int j = 0; j < listView.get_Item(i).ChoiceHero.Length; j++)
				{
					if (listView.get_Item(i).ChoiceHero[j].stBaseInfo.stCommonInfo.dwHeroID == heroID)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		public bool IsHeroExist(uint heroID)
		{
			return this.IsHeroExistWithCamp(1, heroID) || this.IsHeroExistWithCamp(2, heroID);
		}

		public bool IsHaveHeroByID(MemberInfo mInfo, uint heroID)
		{
			bool result = false;
			if (mInfo.canUseHero != null)
			{
				int num = mInfo.canUseHero.Length;
				for (int i = 0; i < num; i++)
				{
					if (mInfo.canUseHero[i] == heroID)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		public bool IsAllConfirmHeroByTeam(COM_PLAYERCAMP camp)
		{
			bool result = true;
			ListView<MemberInfo> listView = this[camp];
			if (listView == null)
			{
				return false;
			}
			for (int i = 0; i < listView.get_Count(); i++)
			{
				if (!listView.get_Item(i).isPrepare)
				{
					result = false;
					break;
				}
			}
			return result;
		}

		public static bool IsSameMemeber(MemberInfo member, COM_PLAYERCAMP camp, int pos)
		{
			return member != null && member.camp == camp && (ulong)member.dwPosOfCamp == (ulong)((long)pos);
		}
	}
}
