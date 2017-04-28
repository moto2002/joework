namespace MsgPack
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal sealed class CollectionDebuggerProxy<T>
    {
        private readonly ICollection<T> _collection;

        public CollectionDebuggerProxy(ICollection<T> target)
        {
            this._collection = target;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                if (this._collection == null)
                {
                    return null;
                }
                T[] array = new T[this._collection.Count];
                this._collection.CopyTo(array, 0);
                return array;
            }
        }
    }
}

