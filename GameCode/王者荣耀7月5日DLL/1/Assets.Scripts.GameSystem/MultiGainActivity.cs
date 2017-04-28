using CSProtocol;
using ResData;
using System;

namespace Assets.Scripts.GameSystem
{
	public class MultiGainActivity : Activity
	{
		private ResWealMultiple _config;

		public override uint ID
		{
			get
			{
				return this._config.dwID;
			}
		}

		public override COM_WEAL_TYPE Type
		{
			get
			{
				return 2;
			}
		}

		public MultiGainActivity(ActivitySys mgr, ResWealMultiple config) : base(mgr, config.stCommon)
		{
			this._config = config;
			for (ushort num = 0; num < this._config.wPeriodNum; num += 1)
			{
				MultiGainPhase ap = new MultiGainPhase(this, (uint)num, this._config.astPeriod[(int)num]);
				base.AddPhase(ap);
			}
		}

		public override void UpdateInfo(ref COMDT_WEAL_UNION actvInfo)
		{
			for (int i = 0; i < base.PhaseList.get_Count(); i++)
			{
				if (i >= actvInfo.get_stMultiple().UsedCnt.Length)
				{
					break;
				}
				MultiGainPhase multiGainPhase = (MultiGainPhase)base.PhaseList.get_Item(i);
				multiGainPhase._usedTimes = actvInfo.get_stMultiple().UsedCnt[i];
				multiGainPhase._NotifyStateChanged();
			}
		}
	}
}
