using CSProtocol;
using System;

namespace Assets.Scripts.GameLogic
{
	public class GameIntimacyData
	{
		public COM_INTIMACY_STATE state;

		public ulong ulluid;

		public uint worldId;

		public string title;

		public GameIntimacyData(COM_INTIMACY_STATE state, ulong ulluid, uint worldId, string title)
		{
			this.state = state;
			this.ulluid = ulluid;
			this.worldId = worldId;
			this.title = title;
		}
	}
}
