using CSProtocol;
using System;

namespace Assets.Scripts.GameSystem
{
	[Serializable]
	public class GuildMemInfo
	{
		public stGuildMemBriefInfo stBriefInfo;

		public COM_PLAYER_GUILD_STATE enPosition;

		public uint dwConstruct;

		public uint TotalContruct;

		public uint CurrActive;

		public uint WeekActive;

		public byte DonateCnt;

		public uint DonateNum;

		public uint WeekDividend;

		public MemberRankInfo RankInfo;

		public COM_ACNT_GAME_STATE GameState;

		public uint dwGameStartTime;

		public uint LastLoginTime;

		public uint JoinTime;

		public COMDT_MEMBER_GUILD_MATCH_INFO GuildMatchInfo;

		public GuildMemInfo()
		{
			this.RankInfo = new MemberRankInfo();
			this.GuildMatchInfo = new COMDT_MEMBER_GUILD_MATCH_INFO();
		}

		public void Reset()
		{
			this.stBriefInfo.Reset();
			this.enPosition = 6;
			this.dwConstruct = 0u;
			this.TotalContruct = 0u;
			this.CurrActive = 0u;
			this.WeekActive = 0u;
			this.DonateCnt = 0;
			this.DonateNum = 0u;
			this.WeekDividend = 0u;
			this.RankInfo.Reset();
			this.GameState = 0;
			this.LastLoginTime = 0u;
			this.JoinTime = 0u;
			this.GuildMatchInfo.bIsLeader = 0;
			this.GuildMatchInfo.ullTeamLeaderUid = 0uL;
			this.GuildMatchInfo.dwScore = 0u;
		}

		public override bool Equals(object obj)
		{
			GuildMemInfo guildMemInfo = obj as GuildMemInfo;
			return guildMemInfo != null && this.stBriefInfo.uulUid == guildMemInfo.stBriefInfo.uulUid;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
