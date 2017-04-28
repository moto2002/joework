using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AGE
{
	[EventCategory("ActionControl")]
	public class StopConflictActions : TickEvent
	{
		private int[] gameObjectIds = new int[0];

		public override BaseEvent Clone()
		{
			StopConflictActions stopConflictActions = ClassObjPool<StopConflictActions>.Get();
			stopConflictActions.CopyData(this);
			return stopConflictActions;
		}

		protected override void CopyData(BaseEvent src)
		{
			base.CopyData(src);
			StopConflictActions stopConflictActions = src as StopConflictActions;
			this.gameObjectIds = stopConflictActions.gameObjectIds;
		}

		public override void OnUse()
		{
			base.OnUse();
			this.gameObjectIds = new int[0];
		}

		public override void Process(Action _action, Track _track)
		{
			List<GameObject> list = new List<GameObject>();
			int[] array = this.gameObjectIds;
			for (int i = 0; i < array.Length; i++)
			{
				int index = array[i];
				list.Add(_action.GetGameObject(index));
			}
			ListView<Action> listView = new ListView<Action>();
			using (List<GameObject>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject current = enumerator.get_Current();
					using (ListView<Action>.Enumerator enumerator2 = ActionManager.Instance.objectReferenceSet.get_Item(current).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Action current2 = enumerator2.get_Current();
							if (current2 != _action && !current2.unstoppable)
							{
								listView.Add(current2);
							}
						}
					}
				}
			}
			using (ListView<Action>.Enumerator enumerator3 = listView.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					Action current3 = enumerator3.get_Current();
					current3.Stop(false);
				}
			}
		}
	}
}
