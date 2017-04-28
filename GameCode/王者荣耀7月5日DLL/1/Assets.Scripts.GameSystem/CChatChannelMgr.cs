using CSProtocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Assets.Scripts.GameSystem
{
	public class CChatChannelMgr
	{
		public enum EChatTab
		{
			Normal,
			Room,
			Team,
			Settle,
			GuildMatch
		}

		public ListView<CChatChannel> NormalChannelList = new ListView<CChatChannel>();

		public ListView<CChatChannel> FriendChannelList = new ListView<CChatChannel>();

		public List<uint> CurActiveChannels = new List<uint>();

		public CChatChannelMgr.EChatTab ChatTab;

		public CChatChannelMgr()
		{
			this.NormalChannelList.Add(new CChatChannel(EChatChannel.Lobby, 0u, 0uL, 0u));
			this.SetChatTab(CChatChannelMgr.EChatTab.Normal);
		}

		public void ClearAll()
		{
			for (int i = 0; i < this.NormalChannelList.get_Count(); i++)
			{
				if (this.NormalChannelList.get_Item(i) != null)
				{
					this.NormalChannelList.get_Item(i).Clear();
				}
			}
			for (int j = 0; j < this.FriendChannelList.get_Count(); j++)
			{
				if (this.FriendChannelList.get_Item(j) != null)
				{
					this.FriendChannelList.get_Item(j).Clear();
				}
			}
			this.SetChatTab(CChatChannelMgr.EChatTab.Normal);
		}

		public void SetChatTab(CChatChannelMgr.EChatTab type)
		{
			this.CurActiveChannels.Clear();
			if (type == CChatChannelMgr.EChatTab.Normal)
			{
				this.CurActiveChannels.Add(2u);
				this.CurActiveChannels.Add(4u);
				CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
				if (masterRoleInfo != null && masterRoleInfo.PvpLevel >= CGuildHelper.GetGuildMemberMinPvpLevel() && Singleton<CGuildSystem>.GetInstance() != null)
				{
					if (Singleton<CGuildSystem>.GetInstance().IsInNormalGuild())
					{
						this.CurActiveChannels.Add(5u);
					}
					else
					{
						this.CurActiveChannels.Add(6u);
					}
				}
				this.ChatTab = type;
			}
			else if (type == CChatChannelMgr.EChatTab.Room)
			{
				this.CurActiveChannels.Add(1u);
				this.CurActiveChannels.Add(4u);
				this.ChatTab = type;
			}
			else if (type == CChatChannelMgr.EChatTab.Team)
			{
				this.CurActiveChannels.Add(0u);
				this.CurActiveChannels.Add(4u);
				this.ChatTab = type;
			}
			else if (type == CChatChannelMgr.EChatTab.Settle)
			{
				this.CurActiveChannels.Add(10u);
				this.ChatTab = type;
			}
			else if (type == CChatChannelMgr.EChatTab.GuildMatch)
			{
				if (Singleton<CGuildMatchSystem>.GetInstance().IsSelfInAnyTeam())
				{
					this.CurActiveChannels.Add(3u);
				}
				this.CurActiveChannels.Add(5u);
				this.CurActiveChannels.Add(4u);
				this.ChatTab = type;
			}
		}

		public CChatChannel GetChannel(EChatChannel type)
		{
			CChatChannel cChatChannel = this._getChannel(type, 0uL, 0u);
			if (cChatChannel == null)
			{
				cChatChannel = this.CreateChannel(type, 0uL, 0u);
			}
			return cChatChannel;
		}

		public CChatChannel GetFriendChannel(ulong ullUid = 0uL, uint dwLogicWorldId = 0u)
		{
			CChatChannel cChatChannel = this._getChannel(EChatChannel.Friend, ullUid, dwLogicWorldId);
			if (cChatChannel == null)
			{
				cChatChannel = this.CreateChannel(EChatChannel.Friend, ullUid, dwLogicWorldId);
			}
			return cChatChannel;
		}

		public int GetAllFriendUnreadCount()
		{
			int num = 0;
			for (int i = 0; i < this.FriendChannelList.get_Count(); i++)
			{
				CChatChannel cChatChannel = this.FriendChannelList.get_Item(i);
				num += cChatChannel.GetUnreadCount();
			}
			return num;
		}

		public void Clear(EChatChannel type, ulong ullUid = 0uL, uint dwLogicWorldId = 0u)
		{
			CChatChannel cChatChannel = this._getChannel(type, ullUid, dwLogicWorldId);
			if (cChatChannel != null)
			{
				cChatChannel.Clear();
			}
		}

		public int GetUnreadCount(EChatChannel type, ulong ullUid = 0uL, uint dwLogicWorldId = 0u)
		{
			CChatChannel cChatChannel = this._getChannel(type, ullUid, dwLogicWorldId);
			if (cChatChannel == null)
			{
				cChatChannel = this.CreateChannel(type, ullUid, dwLogicWorldId);
			}
			return cChatChannel.GetUnreadCount();
		}

		public int GetFriendTotal_UnreadCount()
		{
			int num = 0;
			for (int i = 0; i < this.FriendChannelList.get_Count(); i++)
			{
				CChatChannel cChatChannel = this.FriendChannelList.get_Item(i);
				COMDT_FRIEND_INFO gameOrSnsFriend = Singleton<CFriendContoller>.get_instance().model.GetGameOrSnsFriend(cChatChannel.ullUid, cChatChannel.dwLogicWorldId);
				if (gameOrSnsFriend != null && gameOrSnsFriend.bIsOnline == 1)
				{
					num += cChatChannel.GetUnreadCount();
				}
			}
			return num;
		}

		public void Add_CurChatFriend(CChatEntity chatEnt)
		{
			DebugHelper.Assert(Singleton<CChatController>.get_instance().model.sysData.ullUid != 0uL && Singleton<CChatController>.get_instance().model.sysData.dwLogicWorldId != 0u);
			this.Add_ChatEntity(chatEnt, EChatChannel.Friend, Singleton<CChatController>.get_instance().model.sysData.ullUid, Singleton<CChatController>.get_instance().model.sysData.dwLogicWorldId);
		}

		public void Add_ChatEntity(CChatEntity chatEnt, EChatChannel type, ulong ullUid = 0uL, uint dwLogicWorldId = 0u)
		{
			CChatChannel cChatChannel = this._getChannel(type, ullUid, dwLogicWorldId);
			if (cChatChannel == null)
			{
				cChatChannel = this.CreateChannel(type, ullUid, dwLogicWorldId);
			}
			if (chatEnt.type != EChaterType.System || chatEnt.type != EChaterType.OfflineInfo)
			{
				if (Singleton<CChatController>.get_instance().view.ChatParser != null)
				{
					Singleton<CChatController>.get_instance().view.ChatParser.bProc_ChatEntry = false;
					Singleton<CChatController>.get_instance().view.ChatParser.maxWidth = CChatParser.chat_list_max_width;
					Singleton<CChatController>.get_instance().view.ChatParser.Parse(chatEnt.text, CChatParser.start_x, chatEnt);
				}
				else
				{
					DebugHelper.Assert(false, "CChatController.instance.view.ChatParser = null! StackTrace = " + new StackTrace().ToString());
				}
			}
			CChatEntity last = cChatChannel.GetLast();
			if (last != null && last.time != 0 && chatEnt.time - last.time > 60)
			{
				cChatChannel.Add(CChatUT.Build_4_Time());
			}
			cChatChannel.Add(chatEnt);
		}

		public CChatChannel _getChannel(EChatChannel type, ulong ullUid = 0uL, uint dwLogicWorldId = 0u)
		{
			if (type != EChatChannel.Friend)
			{
				for (int i = 0; i < this.NormalChannelList.get_Count(); i++)
				{
					if (this.NormalChannelList.get_Item(i) != null && this.NormalChannelList.get_Item(i).ChannelType == type)
					{
						return this.NormalChannelList.get_Item(i);
					}
				}
			}
			else
			{
				for (int j = 0; j < this.FriendChannelList.get_Count(); j++)
				{
					if (this.FriendChannelList.get_Item(j) != null && this.FriendChannelList.get_Item(j).ullUid == ullUid && this.FriendChannelList.get_Item(j).dwLogicWorldId == dwLogicWorldId)
					{
						return this.FriendChannelList.get_Item(j);
					}
				}
			}
			return null;
		}

		public CChatChannel CreateChannel(EChatChannel type, ulong ullUid = 0uL, uint dwLogicWorldId = 0u)
		{
			CChatChannel cChatChannel = this._getChannel(type, ullUid, dwLogicWorldId);
			if (cChatChannel != null)
			{
				return cChatChannel;
			}
			if (type != EChatChannel.Friend)
			{
				cChatChannel = new CChatChannel(type, 0u, 0uL, 0u);
				this.NormalChannelList.Add(cChatChannel);
			}
			else
			{
				cChatChannel = new CChatChannel(type, 7000u, ullUid, dwLogicWorldId);
				cChatChannel.list.Add(CChatUT.Build_4_System(Singleton<CTextManager>.get_instance().GetText("Chat_Common_Tips_4")));
				cChatChannel.ReadAll();
				this.FriendChannelList.Add(cChatChannel);
			}
			return cChatChannel;
		}
	}
}
