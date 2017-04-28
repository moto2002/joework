using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.DataCenter;
using Assets.Scripts.GameLogic.GameKernal;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameLogic
{
	public class GameInfoBase
	{
		protected GameContextBase GameContext;

		public GameContextBase gameContext
		{
			get
			{
				return this.GameContext;
			}
		}

		public virtual bool Initialize(GameContextBase InGameContext)
		{
			DebugHelper.Assert(InGameContext != null);
			this.GameContext = InGameContext;
			return this.GameContext != null;
		}

		public virtual void PreBeginPlay()
		{
		}

		public virtual void PostBeginPlay()
		{
			Singleton<BattleLogic>.GetInstance().PrepareFight();
			if (!Singleton<LobbyLogic>.get_instance().inMultiGame)
			{
				Singleton<FrameSynchr>.GetInstance().ResetSynchr();
				bool flag = false;
				SLevelContext curLvelContext = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
				Player hostPlayer = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer();
				if (curLvelContext != null && curLvelContext.m_preDialogId > 0 && hostPlayer != null && hostPlayer.Captain)
				{
					flag = true;
					MonoSingleton<DialogueProcessor>.get_instance().PlayDrama(curLvelContext.m_preDialogId, hostPlayer.Captain.get_handle().gameObject, hostPlayer.Captain.get_handle().gameObject, true);
				}
				if (!flag)
				{
					Singleton<BattleLogic>.GetInstance().DoBattleStart();
				}
				else
				{
					Singleton<BattleLogic>.GetInstance().BindFightPrepareFinListener();
				}
			}
			else if (!Singleton<GameReplayModule>.GetInstance().isReplay)
			{
				Singleton<LobbyLogic>.GetInstance().ReqStartMultiGame();
			}
			SoldierRegion.bFirstSpawnEvent = true;
		}

		public virtual void StartFight()
		{
		}

		public virtual void ReduceDamage(ref HurtDataInfo HurtInfo)
		{
		}

		public virtual void EndGame()
		{
		}

		public virtual void OnLoadingProgress(float Progress)
		{
		}

		protected virtual void LoadAllTeamActors()
		{
			List<Player>.Enumerator enumerator = Singleton<GamePlayerCenter>.get_instance().GetAllPlayers().GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.get_Current() != null)
				{
					ReadonlyContext<uint> allHeroIds = enumerator.get_Current().GetAllHeroIds();
					for (int i = 0; i < allHeroIds.get_Count(); i++)
					{
						ActorMeta actorMeta = default(ActorMeta);
						ActorMeta actorMeta2 = actorMeta;
						actorMeta2.ActorCamp = enumerator.get_Current().PlayerCamp;
						actorMeta2.ConfigId = (int)allHeroIds.get_Item(i);
						actorMeta2.PlayerId = enumerator.get_Current().PlayerId;
						actorMeta = actorMeta2;
						MonoSingleton<GameLoader>.get_instance().AddActor(ref actorMeta);
					}
				}
			}
		}
	}
}
