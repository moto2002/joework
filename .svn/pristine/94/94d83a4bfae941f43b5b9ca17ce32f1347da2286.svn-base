using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Pathfinding;
using Pathfinding.Serialization;
using Pathfinding.Util;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MTileHandlerHelper : MonoBehaviour
{
	public const int CampCount = 3;

	private SGameTileHandler[] handlers;

	private List<Bounds> forcedReloadBounds = new List<Bounds>();

	public static MTileHandlerHelper Instance;

	private ListView<RecastGraph> recastGraphs = new ListView<RecastGraph>();

	private void OnEnable()
	{
		NavmeshCut.add_OnDestroyCallback(new Action<NavmeshCut>(this.HandleOnDestroyCallback));
	}

	private void OnDisable()
	{
		NavmeshCut.remove_OnDestroyCallback(new Action<NavmeshCut>(this.HandleOnDestroyCallback));
	}

	public void DiscardPending()
	{
		ListView<NavmeshCut> all = NavmeshCut.GetAll();
		for (int i = 0; i < all.get_Count(); i++)
		{
			if (all.get_Item(i).RequiresUpdate())
			{
				all.get_Item(i).NotifyUpdated();
			}
		}
	}

	private void CreateHandlers(ListView<NavmeshCut> cuts)
	{
		if (this.handlers != null)
		{
			return;
		}
		AstarPath active = AstarPath.active;
		if (active == null || active.astarData == null || active.astarData.recastGraph == null)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < cuts.get_Count(); i++)
		{
			if (cuts.get_Item(i).campIndex != -1)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			if (active.astarDataArray == null)
			{
				active.astarDataArray = new AstarData[3];
				for (int j = 0; j < 3; j++)
				{
					AstarData astarData = new AstarData();
					astarData.DataGroupIndex = j + 1;
					astarData.recastGraph = active.astarData.recastGraph.Clone(astarData);
					astarData.userConnections = new UserConnection[0];
					astarData.graphs = new NavGraph[]
					{
						astarData.recastGraph
					};
					active.astarDataArray[j] = astarData;
				}
			}
			this.handlers = new SGameTileHandler[3];
			for (int k = 0; k < 3; k++)
			{
				AstarData astarData2 = active.astarDataArray[k];
				this.handlers[k] = new SGameTileHandler(astarData2.recastGraph);
				this.handlers[k].CreateTileTypesFromGraph();
			}
		}
		else
		{
			this.handlers = new SGameTileHandler[1];
			this.handlers[0] = new SGameTileHandler(active.astarData.recastGraph);
			this.handlers[0].CreateTileTypesFromGraph();
		}
	}

	private void Awake()
	{
		MTileHandlerHelper.Instance = this;
	}

	private void OnDestroy()
	{
		MTileHandlerHelper.Instance = null;
		this.handlers = null;
	}

	private void HandleOnDestroyCallback(NavmeshCut obj)
	{
		this.forcedReloadBounds.Add(obj.get_LastBounds());
	}

	public void UpdateLogic()
	{
		if (this.ShouldRebuildNav())
		{
			this.Rebuild2();
		}
	}

	private void Rebuild2()
	{
		ListView<NavmeshCut> all = NavmeshCut.GetAll();
		ListView<NavmeshCut> listView = new ListView<NavmeshCut>();
		this.CreateHandlers(all);
		if (this.handlers == null)
		{
			return;
		}
		AstarPath active = AstarPath.active;
		int num = active.astarData.graphs.Length + 1;
		for (int i = 0; i < all.get_Count(); i++)
		{
			all.get_Item(i).Check();
		}
		for (int j = 0; j < this.handlers.Length; j++)
		{
			listView.Clear();
			for (int k = 0; k < all.get_Count(); k++)
			{
				NavmeshCut navmeshCut = all.get_Item(k);
				if (navmeshCut.campIndex != j && navmeshCut.enabled)
				{
					listView.Add(navmeshCut);
				}
			}
			this.handlers[j].ReloadTiles(listView);
			AstarData astarData = this.handlers[j].get_graph().get_astarData();
			astarData.RasterizeGraphNodes();
		}
		for (int l = 0; l < all.get_Count(); l++)
		{
			if (all.get_Item(l).RequiresUpdate())
			{
				all.get_Item(l).NotifyUpdated();
			}
		}
		this.forcedReloadBounds.Clear();
	}

	private bool ShouldRebuildNav()
	{
		ListView<NavmeshCut> all = NavmeshCut.GetAll();
		if (this.forcedReloadBounds.get_Count() != 0)
		{
			return true;
		}
		for (int i = 0; i < all.get_Count(); i++)
		{
			if (all.get_Item(i).RequiresUpdate())
			{
				return true;
			}
		}
		return false;
	}

	private void SaveNavData(string file, AstarData astarData)
	{
		AstarPath active = AstarPath.active;
		if (astarData == null)
		{
			return;
		}
		byte[] array = astarData.SerializeGraphsExtra(new SerializeSettings());
		FileStream fileStream = new FileStream(file, 2, 2);
		fileStream.Write(array, 0, array.Length);
		fileStream.Close();
	}

	private void SaveNavData(int campIndex)
	{
		if (AstarPath.active == null || AstarPath.active.astarDataArray == null || campIndex < 0 || campIndex >= AstarPath.active.astarDataArray.Length)
		{
			return;
		}
		GameReplayModule instance = Singleton<GameReplayModule>.GetInstance();
		string text = instance.streamPath;
		if (instance.isReplay)
		{
			text = string.Format("{0}/{1}_sgame_debug.txt", DebugHelper.get_logRootPath(), DateTime.get_Now().ToString("yyyyMMdd_HHmmss"));
			int num = text.IndexOf("_sgame_debug");
			if (num != -1)
			{
				text = text.Substring(0, num);
			}
		}
		string text3;
		if (text == null)
		{
			string text2 = GameReplayModule.ReplayFolder;
			text2 += "/NavData";
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			text3 = text2 + "/NAV";
		}
		else
		{
			int num2 = text.LastIndexOf('.');
			if (num2 != -1)
			{
				text3 = text.Substring(0, num2);
			}
			else
			{
				text3 = text;
			}
			ulong num3 = (ulong)Singleton<FrameSynchr>.GetInstance().CurFrameNum;
			if (!Singleton<BattleLogic>.get_instance().isFighting)
			{
				num3 = 0uL;
			}
			text3 += "_";
			text3 += num3;
		}
		text3 += "_camp";
		text3 += campIndex;
		string text4 = text3;
		int num4 = 1;
		while (File.Exists(text4 + ".nav"))
		{
			text4 = text3;
			text4 += " (";
			text4 += num4++;
			text4 += ")";
		}
		this.SaveNavData(text4 + ".nav", AstarPath.active.astarDataArray[campIndex]);
	}
}
