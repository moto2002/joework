using ResData;
using System;

namespace Assets.Scripts.GameSystem
{
	[Serializable]
	public class GuildBuildingInfo
	{
		public RES_GUILD_BUILDING_TYPE type;

		public byte level;

		public GuildBuildingInfo()
		{
			this.Reset();
		}

		public void Reset()
		{
			this.type = 0;
			this.level = 0;
		}
	}
}
