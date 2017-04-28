using CSProtocol;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameSystem
{
	[Serializable]
	public class GuildInfo
	{
		public GuildMemInfo chairman;

		public stGuildBriefInfo briefInfo;

		public ulong uulCreatedTime;

		public uint dwActive;

		public uint dwCoinPool;

		public uint dwGuildMoney;

		public byte bBuildingCount;

		[NonSerialized]
		public ListView<GuildMemInfo> listMemInfo;

		public List<GuildBuildingInfo> listBuildingInfo;

		public List<GuildSelfRecommendInfo> listSelfRecommendInfo;

		public GuildRankInfo RankInfo;

		public uint star;

		public uint groupGuildId;

		public string groupKey;

		public string groupOpenId;

		public COMDT_GUILD_MATCH_INFO GuildMatchInfo;

		public ListView<COMDT_GUILD_MATCH_OB_INFO> GuildMatchObInfos;

		public GuildInfo()
		{
			this.chairman = new GuildMemInfo();
			this.briefInfo = default(stGuildBriefInfo);
			this.listMemInfo = new ListView<GuildMemInfo>();
			this.listBuildingInfo = new List<GuildBuildingInfo>();
			this.listSelfRecommendInfo = new List<GuildSelfRecommendInfo>();
			this.RankInfo = new GuildRankInfo();
			this.GuildMatchInfo = new COMDT_GUILD_MATCH_INFO();
			this.GuildMatchObInfos = new ListView<COMDT_GUILD_MATCH_OB_INFO>();
		}

		public void Reset()
		{
			this.uulCreatedTime = 0uL;
			this.dwActive = 0u;
			this.dwCoinPool = 0u;
			this.dwGuildMoney = 0u;
			this.chairman.Reset();
			this.briefInfo.Reset();
			this.listMemInfo.Clear();
			this.listBuildingInfo.Clear();
			this.listSelfRecommendInfo.Clear();
			this.RankInfo.Reset();
			this.star = 0u;
			this.groupKey = null;
			this.groupOpenId = null;
			this.GuildMatchInfo.dwScore = 0u;
			this.GuildMatchInfo.dwWeekScore = 0u;
			this.GuildMatchInfo.dwLastRankNo = 0u;
			this.GuildMatchInfo.dwUpdRankNoTime = 0u;
			this.GuildMatchInfo.dwLastSeasonRankNo = 0u;
			this.GuildMatchObInfos.Clear();
		}
	}
}
