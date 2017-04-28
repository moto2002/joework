using System;

namespace Assets.Scripts.GameLogic
{
	public struct SoldierWaveParam
	{
		public int WaveIndex;

		public int RepeatCount;

		public int NextDuration;

		public SoldierWaveParam(int inWaveIndex, int inRepeatCount, int inNextDuration)
		{
			this.WaveIndex = inWaveIndex;
			this.RepeatCount = inRepeatCount;
			this.NextDuration = inNextDuration;
		}
	}
}
