using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.DataCenter;
using Assets.Scripts.GameLogic.GameKernal;
using CSProtocol;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
	public static class ActorHelper
	{
		public static PoolObjHandle<ActorRoot> GetActorRoot(GameObject go)
		{
			if (go == null)
			{
				return new PoolObjHandle<ActorRoot>(null);
			}
			ActorConfig component = go.GetComponent<ActorConfig>();
			if (component != null)
			{
				return component.GetActorHandle();
			}
			return new PoolObjHandle<ActorRoot>(null);
		}

		public static PoolObjHandle<ActorRoot> AttachActorRoot(GameObject go)
		{
			DebugHelper.Assert(!Singleton<BattleLogic>.get_instance().isFighting || Singleton<GameLogic>.get_instance().bInLogicTick || Singleton<FrameSynchr>.get_instance().isCmdExecuting);
			ActorConfig actorConfig = go.GetComponent<ActorConfig>();
			if (null == actorConfig)
			{
				actorConfig = go.AddComponent<ActorConfig>();
			}
			ActorMeta actorMeta = default(ActorMeta);
			ActorMeta actorMeta2 = actorMeta;
			actorMeta2.ActorType = ActorTypeDef.Invalid;
			actorMeta = actorMeta2;
			PoolObjHandle<ActorRoot> result = actorConfig.AttachActorRoot(go, ref actorMeta, null);
			result.get_handle().Spawned();
			return result;
		}

		public static PoolObjHandle<ActorRoot> AttachActorRoot(GameObject go, ActorTypeDef actorType, COM_PLAYERCAMP camp, CActorInfo actorInfo)
		{
			DebugHelper.Assert(!Singleton<BattleLogic>.get_instance().isFighting || Singleton<GameLogic>.get_instance().bInLogicTick || Singleton<FrameSynchr>.get_instance().isCmdExecuting);
			ActorConfig actorConfig = go.GetComponent<ActorConfig>();
			if (null == actorConfig)
			{
				actorConfig = go.AddComponent<ActorConfig>();
			}
			ActorMeta actorMeta = default(ActorMeta);
			ActorMeta actorMeta2 = actorMeta;
			actorMeta2.ActorType = actorType;
			actorMeta2.ActorCamp = camp;
			actorMeta = actorMeta2;
			PoolObjHandle<ActorRoot> result = actorConfig.AttachActorRoot(go, ref actorMeta, actorInfo);
			result.get_handle().Spawned();
			return result;
		}

		public static void DetachActorRoot(GameObject go)
		{
			ActorConfig component = go.GetComponent<ActorConfig>();
			if (component != null)
			{
				DebugHelper.Assert(!component.GetActorHandle() || component.GetActorHandle().get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Bullet);
				component.DetachActorRoot();
			}
		}

		public static bool IsHostActor(ref PoolObjHandle<ActorRoot> actor)
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer();
			return hostPlayer != null && hostPlayer.IsAtMyTeam(ref actor.get_handle().TheActorMeta);
		}

		public static bool IsHostCtrlActor(ref PoolObjHandle<ActorRoot> actor)
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer();
			return hostPlayer != null && hostPlayer.Captain == actor;
		}

		public static bool IsHostCampActor(ref PoolObjHandle<ActorRoot> actor)
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer();
			return actor.get_handle().TheActorMeta.ActorCamp == hostPlayer.PlayerCamp;
		}

		public static bool IsHostEnemyActor(ref PoolObjHandle<ActorRoot> actor)
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer();
			return hostPlayer != null && actor.get_handle().TheActorMeta.ActorCamp != hostPlayer.PlayerCamp;
		}

		public static bool IsCaptainActor(ref PoolObjHandle<ActorRoot> actor)
		{
			Player player = Singleton<GamePlayerCenter>.get_instance().GetPlayer(actor.get_handle().TheActorMeta.PlayerId);
			return player != null && player.Captain == actor;
		}

		public static Player GetOwnerPlayer(ref PoolObjHandle<ActorRoot> actor)
		{
			return Singleton<GamePlayerCenter>.get_instance().GetPlayer(actor.get_handle().TheActorMeta.PlayerId);
		}

		public static OperateMode GetPlayerOperateMode(ref PoolObjHandle<ActorRoot> actor)
		{
			Player player = Singleton<GamePlayerCenter>.get_instance().GetPlayer(actor.get_handle().TheActorMeta.PlayerId);
			return player.GetOperateMode();
		}

		public static List<PoolObjHandle<ActorRoot>> FilterActors(List<PoolObjHandle<ActorRoot>> actorList, ActorFilterDelegate filter)
		{
			List<PoolObjHandle<ActorRoot>> list = new List<PoolObjHandle<ActorRoot>>();
			int count = actorList.get_Count();
			for (int i = 0; i < count; i++)
			{
				PoolObjHandle<ActorRoot> poolObjHandle = actorList.get_Item(i);
				if (filter == null || filter(ref poolObjHandle))
				{
					list.Add(poolObjHandle);
				}
			}
			return list;
		}
	}
}
