using System;

namespace Assets.Scripts.GameSystem
{
	public class GuildSelfRecommendInfo
	{
		public ulong uid;

		public uint time;

		public GuildSelfRecommendInfo()
		{
			this.Reset();
		}

		public void Reset()
		{
			this.uid = 0uL;
			this.time = 0u;
		}
	}
}
