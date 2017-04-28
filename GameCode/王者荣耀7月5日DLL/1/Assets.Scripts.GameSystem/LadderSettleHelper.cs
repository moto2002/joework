using System;
using UnityEngine;

namespace Assets.Scripts.GameSystem
{
	public class LadderSettleHelper : MonoBehaviour
	{
		public void OnXingUpAnimationOver()
		{
			Singleton<SettlementSystem>.get_instance().OnLadderXingUpOver();
		}

		public void OnXingDownAnimationOver()
		{
			Singleton<SettlementSystem>.get_instance().OnLadderXingDownOver();
		}

		public void OnLevelUpStartAnimationOver()
		{
			Singleton<SettlementSystem>.get_instance().OnLadderLevelUpStartOver();
		}

		public void OnLevelUpEndAnimationOver()
		{
			Singleton<SettlementSystem>.get_instance().OnLadderLevelUpEndOver();
		}

		public void OnLevelDownStartAnimationOver()
		{
			Singleton<SettlementSystem>.get_instance().OnLadderLevelDownStartOver();
		}

		public void OnLevelDownEndAnimationOver()
		{
			Singleton<SettlementSystem>.get_instance().OnLadderLevelDownEndOver();
		}

		public void OnShowInAnimationOver()
		{
			Singleton<SettlementSystem>.get_instance().OnLadderShowInOver();
		}

		public void OnWangZheXingAnimationStartOver()
		{
		}

		public void OnWangZheXingAnimationEndOver()
		{
			Singleton<SettlementSystem>.get_instance().OnLadderWangZheXingEndOver();
		}
	}
}
