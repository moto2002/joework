using Assets.Scripts.Framework;
using ResData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameSystem
{
	public class CChatChannel
	{
		public static int MaxDeltaTime_Seconds = 60;

		public EChatChannel ChannelType;

		public bool bOffline;

		public ulong ullUid;

		public uint dwLogicWorldId;

		public ListView<CChatEntity> list = new ListView<CChatEntity>();

		private int unread_time_entity_count;

		private int unreadIndex;

		private int unread_count;

		public uint lastTimeStamp;

		public uint cd_time;

		private int lastSendTime;

		public int addTimeSplice_timer = -1;

		public List<Vector2> sizeVec = new List<Vector2>();

		private static uint clt_pendding_time = 2000u;

		public CChatChannel(EChatChannel channelType, uint cdTime = 0u, ulong ullUid = 0uL, uint dwLogicWorldId = 0u)
		{
			this.ChannelType = channelType;
			this.cd_time = 0u;
			this.ullUid = ullUid;
			this.dwLogicWorldId = dwLogicWorldId;
		}

		public void Clear()
		{
			this.list.Clear();
			this.sizeVec.Clear();
			this.ullUid = 0uL;
			this.dwLogicWorldId = 0u;
			for (int i = 0; i < this.list.get_Count(); i++)
			{
				this.list.get_Item(i).Clear();
			}
			this.list.Clear();
			this.unread_time_entity_count = (this.unreadIndex = 0);
			this.lastTimeStamp = 0u;
			this.lastSendTime = 0;
			this.unread_count = 0;
		}

		public void Init_Timer()
		{
			if (this.ChannelType == EChatChannel.Lobby)
			{
				ResAcntExpInfo dataByKey = GameDataMgr.acntExpDatabin.GetDataByKey(Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo().PvpLevel);
				this.cd_time = dataByKey.dwChatCD;
				if (this.cd_time > 0u)
				{
					this.cd_time += CChatChannel.clt_pendding_time;
				}
				this.cd_time /= 1000u;
			}
		}

		public CChatEntity GetLast()
		{
			if (this.list.get_Count() == 0)
			{
				return null;
			}
			if (this.ChannelType == EChatChannel.Friend)
			{
				return (this.list.get_Item(this.list.get_Count() - 1).type != EChaterType.System && this.list.get_Item(this.list.get_Count() - 1).type != EChaterType.OfflineInfo) ? this.list.get_Item(this.list.get_Count() - 1) : null;
			}
			return this.list.get_Item(this.list.get_Count() - 1);
		}

		public bool HasAnyValidChatEntity()
		{
			for (int i = 0; i < this.list.get_Count(); i++)
			{
				CChatEntity cChatEntity = this.list.get_Item(i);
				if (cChatEntity != null)
				{
					if (cChatEntity.type == EChaterType.Self || cChatEntity.type == EChaterType.Friend || cChatEntity.type == EChaterType.Strenger)
					{
						return true;
					}
				}
			}
			return false;
		}

		public int GetUnreadCount()
		{
			return this.unread_count;
		}

		public void Add(CChatEntity ent)
		{
			if (ent == null)
			{
				return;
			}
			if (ent.type == EChaterType.Time)
			{
				this.unread_time_entity_count++;
			}
			this.list.Add(ent);
			if (this.list.get_Count() > CChatController.MaxCount)
			{
				this.list.RemoveRange(0, this.list.get_Count() - CChatController.MaxCount);
			}
			if (this.IsMeanbleChatEnt(ent))
			{
				this.unread_count++;
				ent.bHasReaded = false;
			}
		}

		public int GetUnreadMeanbleChatEntCount(int start_index = 0)
		{
			int num = 0;
			for (int i = start_index; i < this.list.get_Count(); i++)
			{
				CChatEntity cChatEntity = this.list.get_Item(i);
				if (cChatEntity != null)
				{
					if (this.IsMeanbleChatEnt(cChatEntity) && !cChatEntity.bHasReaded)
					{
						num++;
					}
				}
			}
			return num;
		}

		public bool IsMeanbleChatEnt(CChatEntity ent)
		{
			return ent.type != EChaterType.System && ent.type != EChaterType.Time && ent.type != EChaterType.OfflineInfo && ent.type != EChaterType.LeaveRoom;
		}

		public void ReadAll()
		{
			this.unread_count = 0;
		}

		public void InitChat_InputTimer(int time)
		{
			this.lastSendTime = 0;
			if (time <= 0)
			{
				return;
			}
			this.cd_time = (uint)(time + (int)CChatChannel.clt_pendding_time);
			this.cd_time /= 1000u;
		}

		public void Start_InputCD()
		{
			if (this.cd_time == 0u)
			{
				return;
			}
			this.lastSendTime = CRoleInfo.GetCurrentUTCTime();
		}

		public bool IsInputValid()
		{
			return (long)this.lastSendTime + (long)((ulong)this.cd_time) <= (long)CRoleInfo.GetCurrentUTCTime();
		}

		public void ClearCd()
		{
			this.lastSendTime = 0;
		}

		public int Get_Left_CDTime()
		{
			return (int)Mathf.Max(0f, (float)((long)this.lastSendTime + (long)((ulong)this.cd_time) - (long)CRoleInfo.GetCurrentUTCTime()));
		}
	}
}
