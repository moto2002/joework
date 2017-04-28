using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
	public class DropItemMgr : Singleton<DropItemMgr>, IUpdateLogic
	{
		protected HashSet<object> ActiveItems = new HashSet<object>();

		protected ListView<DropItem> DeprecatedItem = new ListView<DropItem>();

		public override void Init()
		{
		}

		public DropItem CreateItem(string InPrefab, IDropDownEffect InDropdownEffect, IPickupEffect InPickupEffect)
		{
			DropItem dropItem = new DropItem(InPrefab, InDropdownEffect, InPickupEffect);
			this.ActiveItems.Add(dropItem);
			return dropItem;
		}

		public void UpdateLogic(int delta)
		{
			HashSet<object>.Enumerator enumerator = this.ActiveItems.GetEnumerator();
			while (enumerator.MoveNext())
			{
				DropItem dropItem = (DropItem)enumerator.get_Current();
				if (dropItem != null)
				{
					dropItem.UpdateLogic(delta);
				}
			}
			for (int i = 0; i < this.DeprecatedItem.get_Count(); i++)
			{
				this.ActiveItems.Remove(this.DeprecatedItem.get_Item(i));
			}
			this.DeprecatedItem.Clear();
		}

		public void RemoveItem(DropItem item)
		{
			this.DeprecatedItem.Add(item);
		}

		public void RemoveItemImmediate(DropItem item)
		{
			this.ActiveItems.Remove(item);
		}

		public GameObject FindPrefabObject(string Prefab)
		{
			return (GameObject)Singleton<CResourceManager>.get_instance().GetResource(Prefab, typeof(GameObject), 0, false, false).m_content;
		}
	}
}
