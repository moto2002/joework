using Assets.Scripts.Common;
using System;

namespace Assets.Scripts.GameLogic
{
	public class CSkillStat
	{
		public uint m_uiStunTime;

		public uint m_uiBeStunnedTime;

		public PoolObjHandle<ActorRoot> actorHero;

		public SKILLSTATISTICTINFO[] SkillStatistictInfo = new SKILLSTATISTICTINFO[10];

		public uint m_uiSpawnEyeTimes;

		public VInt3[] stEyePostion = new VInt3[15];

		public uint m_uiRealSpawnEyeTimes;

		public uint m_uiEyeSwitchTimes;

		public uint StunTime
		{
			get
			{
				return this.m_uiStunTime;
			}
		}

		public uint BeStunTime
		{
			get
			{
				return this.m_uiBeStunnedTime;
			}
		}

		public void Initialize(PoolObjHandle<ActorRoot> _actorHero)
		{
			this.m_uiStunTime = 0u;
			this.m_uiBeStunnedTime = 0u;
			this.actorHero = _actorHero;
			for (int i = 0; i < 10; i++)
			{
				this.SkillStatistictInfo[i] = new SKILLSTATISTICTINFO(0);
			}
			this.m_uiSpawnEyeTimes = 0u;
			this.m_uiRealSpawnEyeTimes = 0u;
			this.m_uiEyeSwitchTimes = 0u;
		}

		public void UnInit()
		{
		}

		public int GetStunSkillNum()
		{
			int num = 0;
			if (!this.actorHero)
			{
				return 0;
			}
			SkillSlot[] skillSlotArray = this.actorHero.get_handle().SkillControl.SkillSlotArray;
			if (skillSlotArray == null)
			{
				return 0;
			}
			for (int i = 0; i < 10; i++)
			{
				if (skillSlotArray[i] != null && skillSlotArray[i].SkillObj != null && skillSlotArray[i].SkillObj.cfgData != null)
				{
					if (skillSlotArray[i].SkillObj.cfgData.bIsStunSkill == 1)
					{
						num++;
					}
				}
			}
			return num;
		}
	}
}
