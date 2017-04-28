using Assets.Scripts.GameLogic.DataCenter;
using Assets.Scripts.GameLogic.GameKernal;
using CSProtocol;
using System;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
	public class TestGameContext : GameContextBase
	{
		public TestGameContext(ref SCPKG_STARTSINGLEGAMERSP InMessage)
		{
			Singleton<ActorDataCenter>.get_instance().ClearHeroServerData();
			Player player = Singleton<GamePlayerCenter>.GetInstance().AddPlayer(1u, 1, 0, 1u, false, "test", 0, 0, 0uL, 0u, null, 0u, 0u, 0, 0, null);
			Singleton<GamePlayerCenter>.GetInstance().SetHostPlayer(1u);
			player.AddHero(InMessage.stDetail.get_stSingleGameSucc().astFighter[0].astChoiceHero[0].stBaseInfo.stCommonInfo.dwHeroID);
			COMDT_CHOICEHERO[] array = new COMDT_CHOICEHERO[]
			{
				new COMDT_CHOICEHERO()
			};
			array[0].stBaseInfo.stCommonInfo.dwHeroID = InMessage.stDetail.get_stSingleGameSucc().astFighter[0].astChoiceHero[0].stBaseInfo.stCommonInfo.dwHeroID;
			array[0].stBaseInfo.stCommonInfo.wSkinID = InMessage.stDetail.get_stSingleGameSucc().astFighter[0].astChoiceHero[0].stBaseInfo.stCommonInfo.wSkinID;
			Singleton<ActorDataCenter>.get_instance().AddHeroesServerData(1u, array);
			Player player2 = Singleton<GamePlayerCenter>.GetInstance().AddPlayer(2u, 2, 0, 1u, true, string.Empty, 0, 0, 0uL, 0u, null, 0u, 0u, 0, 0, null);
			player2.AddHero(166u);
			array = new COMDT_CHOICEHERO[]
			{
				new COMDT_CHOICEHERO()
			};
			array[0].stBaseInfo.stCommonInfo.dwHeroID = 166u;
			Singleton<ActorDataCenter>.get_instance().AddHeroesServerData(2u, array);
			this.LevelContext = new SLevelContext();
			this.LevelContext.SetGameType(0);
			this.LevelContext.m_levelArtistFileName = PlayerPrefs.GetString("PrevewMapArt");
			base.levelContext.m_levelDesignFileName = PlayerPrefs.GetString("PrevewMapDesigner");
			base.levelContext.m_levelDifficulty = 1;
		}

		public override GameInfoBase CreateGameInfo()
		{
			SingleGameInfo singleGameInfo = new SingleGameInfo();
			singleGameInfo.Initialize(this);
			return singleGameInfo;
		}
	}
}
