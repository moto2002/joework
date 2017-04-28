using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Framework
{
	public class MultiValueListDictionary<TKey, TValue> : DictionaryView<TKey, ListView<TValue>>
	{
		public void Add(TKey key, TValue value)
		{
			ListView<TValue> listView = null;
			if (!base.TryGetValue(key, ref listView))
			{
				listView = new ListView<TValue>();
				base.Add(key, listView);
			}
			listView.Add(value);
		}

		public ListView<TValue> GetValues(TKey key, bool returnEmptySet = true)
		{
			ListView<TValue> result = null;
			if (!base.TryGetValue(key, ref result) && returnEmptySet)
			{
				result = new ListView<TValue>();
			}
			return result;
		}

		public TValue[] GetValuesAll()
		{
			ListLinqView<TValue> listLinqView = new ListLinqView<TValue>();
			DictionaryView<TKey, ListView<TValue>>.Enumerator enumerator = base.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<TKey, ListView<TValue>> current = enumerator.get_Current();
				ListView<TValue> value = current.get_Value();
				if (value != null)
				{
					IEnumerator enumerator2 = value.GetEnumerator();
					while (enumerator2.MoveNext())
					{
						TValue tValue = (TValue)((object)enumerator2.get_Current());
						listLinqView.Add(tValue);
					}
				}
			}
			return listLinqView.ToArray();
		}
	}
}
