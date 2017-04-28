using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.GameLogic.GameKernal;
using CSProtocol;
using ResData;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameSystem
{
	public class ShenFuSystem : Singleton<ShenFuSystem>
	{
		public Dictionary<int, ShenFuObjects> _shenFuTriggerPool = new Dictionary<int, ShenFuObjects>();

		public List<PoolObjHandle<CTailsman>> m_charmList = new List<PoolObjHandle<CTailsman>>();

		public override void Init()
		{
		}

		public override void UnInit()
		{
			this.ClearAll();
		}

		public void UpdateLogic(int inDelta)
		{
			if (this.m_charmList.get_Count() > 0)
			{
				int count = this.m_charmList.get_Count();
				for (int i = count - 1; i >= 0; i--)
				{
					this.m_charmList.get_Item(i).get_handle().UpdateLogic(inDelta);
				}
			}
		}

		public void AddCharm(PoolObjHandle<CTailsman> inCharm)
		{
			if (inCharm)
			{
				this.m_charmList.Add(inCharm);
			}
		}

		public void RemoveCharm(PoolObjHandle<CTailsman> inCharm)
		{
			if (inCharm)
			{
				this.m_charmList.Remove(inCharm);
			}
		}

		public void PreLoadResource(TriggerActionWrapper triggerActionWrapper, ref ActorPreloadTab loadInfo, LoaderHelper loadHelper)
		{
			if (triggerActionWrapper == null)
			{
				return;
			}
			ShenFuInfo dataByKey = GameDataMgr.shenfuBin.GetDataByKey((long)triggerActionWrapper.UpdateUniqueId);
			if (dataByKey == null)
			{
				return;
			}
			AssetLoadBase assetLoadBase = new AssetLoadBase
			{
				assetPath = StringHelper.UTF8BytesToString(ref dataByKey.szShenFuResPath)
			};
			loadInfo.mesPrefabs.Add(assetLoadBase);
			loadHelper.AnalyseSkillCombine(ref loadInfo, dataByKey.iBufId);
		}

		public void ClearAll()
		{
			Dictionary<int, ShenFuObjects>.Enumerator enumerator = this._shenFuTriggerPool.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<int, ShenFuObjects> current = enumerator.get_Current();
				ShenFuObjects value = current.get_Value();
				if (value.ShenFu != null)
				{
					Singleton<CGameObjectPool>.GetInstance().RecycleGameObject(value.ShenFu);
				}
			}
			this._shenFuTriggerPool.Clear();
			while (this.m_charmList.get_Count() > 0)
			{
				this.m_charmList.get_Item(0).get_handle().DoClearing();
			}
		}

		public void OnShenfuStart(uint shenFuId, AreaEventTrigger trigger, TriggerActionShenFu shenFu)
		{
			if (trigger == null || shenFu == null)
			{
				return;
			}
			ShenFuObjects shenFuObjects = default(ShenFuObjects);
			ShenFuInfo dataByKey = GameDataMgr.shenfuBin.GetDataByKey(shenFuId);
			if (dataByKey == null)
			{
				return;
			}
			trigger.Radius = (int)dataByKey.dwGetRadius;
			string prefabName = StringHelper.UTF8BytesToString(ref dataByKey.szShenFuResPath);
			shenFuObjects.ShenFu = MonoSingleton<SceneMgr>.get_instance().InstantiateLOD(prefabName, false, SceneObjType.ActionRes, trigger.gameObject.transform.position);
			this._shenFuTriggerPool.Add(trigger.ID, shenFuObjects);
			if (FogOfWar.enable)
			{
				COM_PLAYERCAMP playerCamp = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().PlayerCamp;
				GameFowCollector.SetObjVisibleByFow(shenFuObjects.ShenFu, Singleton<GameFowManager>.get_instance(), playerCamp);
			}
		}

		public void OnShenfuHalt(uint shenFuId, AreaEventTrigger trigger, TriggerActionShenFu shenFu)
		{
		}

		public void OnShenFuEffect(PoolObjHandle<ActorRoot> actor, uint shenFuId, AreaEventTrigger trigger, TriggerActionShenFu shenFu)
		{
			ShenFuObjects shenFuObjects;
			if (this._shenFuTriggerPool.TryGetValue(trigger.ID, ref shenFuObjects))
			{
				if (shenFuObjects.ShenFu != null)
				{
					Singleton<CGameObjectPool>.GetInstance().RecycleGameObject(shenFuObjects.ShenFu);
				}
				this._shenFuTriggerPool.Remove(trigger.ID);
			}
			ShenFuInfo dataByKey = GameDataMgr.shenfuBin.GetDataByKey(shenFuId);
			if (dataByKey == null)
			{
				return;
			}
			BufConsumer bufConsumer = new BufConsumer(dataByKey.iBufId, actor, actor);
			if (bufConsumer.Use())
			{
			}
		}

		public void OnShenFuStopped(TriggerActionShenFu inAction)
		{
			if (inAction == null)
			{
				return;
			}
			ShenFuObjects shenFuObjects;
			if (this._shenFuTriggerPool.TryGetValue(inAction.TriggerId, ref shenFuObjects))
			{
				if (shenFuObjects.ShenFu != null)
				{
					Singleton<CGameObjectPool>.GetInstance().RecycleGameObject(shenFuObjects.ShenFu);
				}
				this._shenFuTriggerPool.Remove(inAction.TriggerId);
			}
		}
	}
}
