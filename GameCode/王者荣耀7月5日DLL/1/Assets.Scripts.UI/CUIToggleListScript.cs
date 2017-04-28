using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class CUIToggleListScript : CUIListScript
	{
		public bool m_isMultiSelected;

		private int m_selected;

		private bool[] m_multiSelected;

		public override void Initialize(CUIFormScript formScript)
		{
			if (this.m_isInitialized)
			{
				return;
			}
			if (this.m_isMultiSelected)
			{
				this.m_multiSelected = new bool[this.m_elementAmount];
				for (int i = 0; i < this.m_elementAmount; i++)
				{
					this.m_multiSelected[i] = false;
				}
			}
			else
			{
				this.m_selected = -1;
			}
			base.Initialize(formScript);
		}

		public override void SetElementAmount(int amount, List<Vector2> elementsSize)
		{
			if (this.m_isMultiSelected && (this.m_multiSelected == null || this.m_multiSelected.Length < amount))
			{
				bool[] array = new bool[amount];
				for (int i = 0; i < amount; i++)
				{
					if (this.m_multiSelected != null && i < this.m_multiSelected.Length)
					{
						array[i] = this.m_multiSelected[i];
					}
					else
					{
						array[i] = false;
					}
				}
				this.m_multiSelected = array;
			}
			base.SetElementAmount(amount, elementsSize);
		}

		public override void SelectElement(int index, bool isDispatchSelectedChangeEvent = true)
		{
			if (this.m_isMultiSelected)
			{
				bool flag = this.m_multiSelected[index];
				flag = !flag;
				this.m_multiSelected[index] = flag;
				CUIListElementScript elemenet = base.GetElemenet(index);
				if (elemenet != null)
				{
					elemenet.ChangeDisplay(flag);
				}
				base.DispatchElementSelectChangedEvent();
			}
			else
			{
				if (index == this.m_selected)
				{
					if (this.m_alwaysDispatchSelectedChangeEvent)
					{
						base.DispatchElementSelectChangedEvent();
					}
					return;
				}
				if (this.m_selected >= 0)
				{
					CUIListElementScript elemenet2 = base.GetElemenet(this.m_selected);
					if (elemenet2 != null)
					{
						elemenet2.ChangeDisplay(false);
					}
				}
				this.m_selected = index;
				if (this.m_selected >= 0)
				{
					CUIListElementScript elemenet3 = base.GetElemenet(this.m_selected);
					if (elemenet3 != null)
					{
						elemenet3.ChangeDisplay(true);
					}
				}
				base.DispatchElementSelectChangedEvent();
			}
		}

		public int GetSelected()
		{
			return this.m_selected;
		}

		public bool[] GetMultiSelected()
		{
			return this.m_multiSelected;
		}

		public void SetSelected(int selected)
		{
			this.m_selected = selected;
			for (int i = 0; i < this.m_elementScripts.get_Count(); i++)
			{
				this.m_elementScripts.get_Item(i).ChangeDisplay(this.IsSelectedIndex(this.m_elementScripts.get_Item(i).m_index));
			}
		}

		public void SetMultiSelected(int index, bool selected)
		{
			if (index < 0 || index >= this.m_elementAmount)
			{
				return;
			}
			this.m_multiSelected[index] = selected;
			for (int i = 0; i < this.m_elementScripts.get_Count(); i++)
			{
				this.m_elementScripts.get_Item(i).ChangeDisplay(this.IsSelectedIndex(this.m_elementScripts.get_Item(i).m_index));
			}
		}

		public override bool IsSelectedIndex(int index)
		{
			return (!this.m_isMultiSelected) ? (index == this.m_selected) : this.m_multiSelected[index];
		}
	}
}
