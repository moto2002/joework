using Assets.Scripts.Common;
using Assets.Scripts.GameLogic.GameKernal;
using ResData;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameLogic
{
	public class DynamicDifficulty
	{
		public void FightStart()
		{
			SLevelContext curLvelContext = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
			if (curLvelContext != null && curLvelContext.m_warmHeroAiDiffInfo != null)
			{
				ResBattleDynamicDifficulty warmHeroAiDiffInfo = curLvelContext.m_warmHeroAiDiffInfo;
				List<PoolObjHandle<ActorRoot>> campActors = Singleton<GameObjMgr>.GetInstance().GetCampActors(Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().PlayerCamp);
				for (int i = 0; i < campActors.get_Count(); i++)
				{
					ActorRoot handle = campActors.get_Item(i).get_handle();
					if (handle.TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero)
					{
						PropertyHelper mActorValue = handle.ValueComponent.mActorValue;
						mActorValue[31].baseValue += warmHeroAiDiffInfo.iSelfAttackAdd;
						mActorValue[30].baseValue += warmHeroAiDiffInfo.iSelfDefenceAdd;
					}
				}
			}
		}
	}
}
