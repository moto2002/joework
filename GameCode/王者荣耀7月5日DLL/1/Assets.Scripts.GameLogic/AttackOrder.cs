using Assets.Scripts.Common;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameLogic
{
	public class AttackOrder
	{
		private DictionaryView<int, List<PoolObjHandle<ActorRoot>>> _orderDepends;

		private Dictionary<int, PoolObjHandle<ActorRoot>> _orderOwner;

		public void FightStart()
		{
			Singleton<GameEventSys>.get_instance().RmvEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.onActorDead));
			Singleton<GameEventSys>.get_instance().AddEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.onActorDead));
			this._orderDepends = new DictionaryView<int, List<PoolObjHandle<ActorRoot>>>();
			this._orderOwner = new Dictionary<int, PoolObjHandle<ActorRoot>>();
			List<PoolObjHandle<ActorRoot>> gameActors = Singleton<GameObjMgr>.GetInstance().GameActors;
			for (int i = 0; i < gameActors.get_Count(); i++)
			{
				PoolObjHandle<ActorRoot> poolObjHandle = gameActors.get_Item(i);
				int battleOrder = poolObjHandle.get_handle().ObjLinker.BattleOrder;
				if (battleOrder != 0 && !this._orderOwner.ContainsKey(battleOrder))
				{
					this._orderOwner.Add(battleOrder, poolObjHandle);
				}
				int[][] array = new int[][]
				{
					poolObjHandle.get_handle().ObjLinker.BattleOrderDepend
				};
				if (array != null)
				{
					for (int j = 0; j < array.Length; j++)
					{
						int[] array2 = array[j];
						if (array2 != null)
						{
							for (int k = 0; k < array2.Length; k++)
							{
								int num = array2[k];
								if (num != 0)
								{
									poolObjHandle.get_handle().AttackOrderReady = false;
									List<PoolObjHandle<ActorRoot>> list;
									if (this._orderDepends.ContainsKey(num))
									{
										list = this._orderDepends.get_Item(num);
									}
									else
									{
										list = new List<PoolObjHandle<ActorRoot>>();
										this._orderDepends.Add(num, list);
									}
									list.Add(poolObjHandle);
								}
							}
						}
					}
				}
			}
		}

		public void FightOver()
		{
			this._orderDepends = null;
			this._orderOwner = null;
			Singleton<GameEventSys>.get_instance().RmvEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.onActorDead));
		}

		private void onActorDead(ref GameDeadEventParam prm)
		{
			int battleOrder = prm.src.get_handle().ObjLinker.BattleOrder;
			if (battleOrder == 0)
			{
				return;
			}
			if (!this._orderDepends.ContainsKey(battleOrder))
			{
				return;
			}
			List<PoolObjHandle<ActorRoot>> list = this._orderDepends.get_Item(battleOrder);
			for (int i = 0; i < list.get_Count(); i++)
			{
				PoolObjHandle<ActorRoot> poolObjHandle = list.get_Item(i);
				if (poolObjHandle)
				{
					int[][] array = new int[][]
					{
						poolObjHandle.get_handle().ObjLinker.BattleOrderDepend
					};
					if (array == null)
					{
						poolObjHandle.get_handle().AttackOrderReady = true;
					}
					else
					{
						for (int j = 0; j < array.Length; j++)
						{
							int[] array2 = array[j];
							if (array2 != null)
							{
								bool flag = false;
								for (int k = 0; k < array2.Length; k++)
								{
									int num = array2[k];
									if (num != 0 && this._orderOwner.ContainsKey(num) && this._orderOwner.get_Item(num).get_handle().ActorControl.IsDeadState)
									{
										flag = true;
										break;
									}
								}
								if (flag || array2.Length == 0)
								{
									poolObjHandle.get_handle().AttackOrderReady = true;
									break;
								}
							}
						}
					}
				}
			}
		}
	}
}
