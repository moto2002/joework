using System;
using System.Collections;
using System.Collections.Generic;

public class DictionaryView<TKey, TValue> : IEnumerable, IEnumerable<KeyValuePair<TKey, TValue>>
{
	public struct Enumerator : IDisposable, IEnumerator, IEnumerator<KeyValuePair<TKey, TValue>>
	{
		private Dictionary<TKey, object> Reference;

		private Dictionary<TKey, object>.Enumerator Iter;

		object IEnumerator.Current
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public KeyValuePair<TKey, TValue> Current
		{
			get
			{
				KeyValuePair<TKey, object> current = this.Iter.get_Current();
				TKey arg_51_0 = current.get_Key();
				KeyValuePair<TKey, object> current2 = this.Iter.get_Current();
				TValue arg_51_1;
				if (current2.get_Value() != null)
				{
					KeyValuePair<TKey, object> current3 = this.Iter.get_Current();
					arg_51_1 = (TValue)((object)current3.get_Value());
				}
				else
				{
					arg_51_1 = default(TValue);
				}
				return new KeyValuePair<TKey, TValue>(arg_51_0, arg_51_1);
			}
		}

		public Enumerator(Dictionary<TKey, object> InReference)
		{
			this.Reference = InReference;
			this.Iter = this.Reference.GetEnumerator();
		}

		public void Reset()
		{
			this.Iter = this.Reference.GetEnumerator();
		}

		public void Dispose()
		{
			this.Iter.Dispose();
			this.Reference = null;
		}

		public bool MoveNext()
		{
			return this.Iter.MoveNext();
		}
	}

	protected Dictionary<TKey, object> Context;

	public int Count
	{
		get
		{
			return this.Context.get_Count();
		}
	}

	public TValue this[TKey key]
	{
		get
		{
			object obj = this.Context.get_Item(key);
			return (obj == null) ? default(TValue) : ((TValue)((object)obj));
		}
		set
		{
			this.Context.set_Item(key, value);
		}
	}

	public Dictionary<TKey, object>.KeyCollection Keys
	{
		get
		{
			return this.Context.get_Keys();
		}
	}

	public DictionaryView()
	{
		this.Context = new Dictionary<TKey, object>();
	}

	public DictionaryView(int capacity)
	{
		this.Context = new Dictionary<TKey, object>(capacity);
	}

	IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw new NotImplementedException();
	}

	public void Add(TKey key, TValue value)
	{
		this.Context.Add(key, value);
	}

	public void Clear()
	{
		this.Context.Clear();
	}

	public bool ContainsKey(TKey key)
	{
		return this.Context.ContainsKey(key);
	}

	public bool Remove(TKey key)
	{
		return this.Context.Remove(key);
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		object obj = null;
		bool result = this.Context.TryGetValue(key, ref obj);
		value = ((obj == null) ? default(TValue) : ((TValue)((object)obj)));
		return result;
	}

	public DictionaryView<TKey, TValue>.Enumerator GetEnumerator()
	{
		return new DictionaryView<TKey, TValue>.Enumerator(this.Context);
	}
}
