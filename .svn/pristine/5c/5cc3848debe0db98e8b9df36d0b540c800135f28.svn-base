using ResData;
using System;

namespace Assets.Scripts.GameLogic
{
	public class CRecommendEquipInfo : IComparable
	{
		public ushort m_equipID;

		public ResEquipInBattle m_resEquipInBattle;

		public bool m_hasBeenBought;

		public CRecommendEquipInfo(ushort equipID, ResEquipInBattle resEquipInBattle)
		{
			this.m_equipID = equipID;
			this.m_resEquipInBattle = resEquipInBattle;
			this.m_hasBeenBought = false;
		}

		public int CompareTo(object obj)
		{
			CRecommendEquipInfo cRecommendEquipInfo = obj as CRecommendEquipInfo;
			if (this.m_resEquipInBattle.dwBuyPrice > cRecommendEquipInfo.m_resEquipInBattle.dwBuyPrice)
			{
				return -1;
			}
			if (this.m_resEquipInBattle.dwBuyPrice != cRecommendEquipInfo.m_resEquipInBattle.dwBuyPrice)
			{
				return 1;
			}
			if (this.m_equipID > cRecommendEquipInfo.m_equipID)
			{
				return -1;
			}
			if (this.m_equipID == cRecommendEquipInfo.m_equipID)
			{
				return 0;
			}
			return 1;
		}
	}
}
