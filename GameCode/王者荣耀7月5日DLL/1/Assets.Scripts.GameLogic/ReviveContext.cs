using System;

namespace Assets.Scripts.GameLogic
{
	public struct ReviveContext
	{
		public bool bEnable;

		public int ReviveTime;

		public int ReviveLife;

		public int ReviveEnergy;

		public bool AutoReset;

		public bool bBaseRevive;

		public bool bCDReset;

		public void Reset()
		{
			this.ReviveTime = 0;
			this.ReviveLife = 10000;
			this.ReviveEnergy = 10000;
			this.AutoReset = false;
			this.bBaseRevive = true;
			this.bCDReset = false;
			this.bEnable = false;
		}
	}
}
