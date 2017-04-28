using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Framework
{
	public class MultiValueHashDictionary<TKey, TValue> : DictionaryView<TKey, HashSet<TValue>>
	{
		public void Add(TKey key, TValue value)
		{
			HashSet<TValue> hashSet = null;
			if (!base.TryGetValue(key, ref hashSet))
			{
				hashSet = new HashSet<TValue>();
				base.Add(key, hashSet);
			}
			hashSet.Add(value);
		}

		public HashSet<TValue> GetValues(TKey key, bool returnEmptySet = true)
		{
			HashSet<TValue> result = null;
			if (!base.TryGetValue(key, ref result) && returnEmptySet)
			{
				result = new HashSet<TValue>();
			}
			return result;
		}

		public TValue[] GetAllValueArray()
		{
			ListLinqView<TValue> listLinqView = new ListLinqView<TValue>();
			DictionaryView<TKey, HashSet<TValue>>.Enumerator enumerator = base.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<TKey, HashSet<TValue>> current = enumerator.get_Current();
				HashSet<TValue> value = current.get_Value();
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
