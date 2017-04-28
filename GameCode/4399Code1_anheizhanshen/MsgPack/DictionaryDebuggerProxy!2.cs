namespace MsgPack
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal sealed class DictionaryDebuggerProxy<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _dictionary;

        public DictionaryDebuggerProxy(IDictionary<TKey, TValue> target)
        {
            this._dictionary = target;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public KeyValuePair<TKey, TValue>[] Items
        {
            get
            {
                if (this._dictionary == null)
                {
                    return null;
                }
                KeyValuePair<TKey, TValue>[] array = new KeyValuePair<TKey, TValue>[this._dictionary.Count];
                this._dictionary.CopyTo(array, 0);
                return array;
            }
        }
    }
}

