namespace MsgPack.Serialization
{
    using MsgPack;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Threading;

    internal sealed class TypeKeyRepository : IDisposable
    {
        private volatile int _isFrozen;
        private readonly ReaderWriterLockSlim _lock;
        private readonly Dictionary<RuntimeTypeHandle, object> _table;

        public TypeKeyRepository() : this((TypeKeyRepository) null)
        {
        }

        public TypeKeyRepository(TypeKeyRepository copiedFrom)
        {
            this._lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            if (copiedFrom == null)
            {
                this._table = new Dictionary<RuntimeTypeHandle, object>();
            }
            else
            {
                this._table = copiedFrom.GetClonedTable();
            }
        }

        public TypeKeyRepository(Dictionary<RuntimeTypeHandle, object> table)
        {
            this._lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            this._table = table;
        }

        public void Dispose()
        {
            this._lock.Dispose();
        }

        public void Freeze()
        {
            this._isFrozen = 1;
        }

        public TEntry Get<T, TEntry>(SerializationContext context) where TEntry: class
        {
            object obj2;
            object obj3;
            if (!this.Get<T>(out obj2, out obj3))
            {
                return default(TEntry);
            }
            if (obj2 != null)
            {
                return (obj2 as TEntry);
            }
            Contract.Assert(typeof(T).GetIsGenericType());
            Contract.Assert(!typeof(T).GetIsGenericTypeDefinition());
            Type source = obj3 as Type;
            Contract.Assert(source != null);
            Contract.Assert(source.GetIsGenericTypeDefinition());
            TEntry local = (TEntry) Activator.CreateInstance(source.MakeGenericType(typeof(T).GetGenericArguments()), new object[] { context });
            Contract.Assert(local != null);
            return local;
        }

        [SecuritySafeCritical]
        private bool Get<T>(out object matched, out object genericDefinitionMatched)
        {
            bool flag2;
            bool flag = false;
            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
            }
            finally
            {
                this._lock.EnterReadLock();
                flag = true;
            }
            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                object obj2;
                if (this._table.TryGetValue(typeof(T).TypeHandle, out obj2))
                {
                    matched = obj2;
                    genericDefinitionMatched = null;
                    return true;
                }
                if (typeof(T).GetIsGenericType() && this._table.TryGetValue(typeof(T).GetGenericTypeDefinition().TypeHandle, out obj2))
                {
                    matched = null;
                    genericDefinitionMatched = obj2;
                    return true;
                }
                matched = null;
                genericDefinitionMatched = null;
                flag2 = false;
            }
            finally
            {
                if (flag)
                {
                    this._lock.ExitReadLock();
                }
            }
            return flag2;
        }

        [SecuritySafeCritical]
        private Dictionary<RuntimeTypeHandle, object> GetClonedTable()
        {
            Dictionary<RuntimeTypeHandle, object> dictionary;
            bool flag = false;
            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
            }
            finally
            {
                this._lock.EnterReadLock();
                flag = true;
            }
            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                dictionary = new Dictionary<RuntimeTypeHandle, object>(this._table);
            }
            finally
            {
                if (flag)
                {
                    this._lock.ExitReadLock();
                }
            }
            return dictionary;
        }

        public bool Register<T>(object entry)
        {
            Contract.Assert(entry != null);
            if (this.IsFrozen)
            {
                throw new InvalidOperationException("This repository is frozen.");
            }
            return this.Register(typeof(T), entry);
        }

        [SecuritySafeCritical]
        private bool Register(Type key, object value)
        {
            if (!this._table.ContainsKey(key.TypeHandle))
            {
                bool flag = false;
                RuntimeHelpers.PrepareConstrainedRegions();
                try
                {
                }
                finally
                {
                    this._lock.EnterWriteLock();
                    flag = true;
                }
                RuntimeHelpers.PrepareConstrainedRegions();
                try
                {
                    if (!this._table.ContainsKey(key.TypeHandle))
                    {
                        this._table.Add(key.TypeHandle, value);
                        return true;
                    }
                }
                finally
                {
                    if (flag)
                    {
                        this._lock.ExitWriteLock();
                    }
                }
            }
            return false;
        }

        public bool IsFrozen
        {
            get
            {
                return (this._isFrozen != 0);
            }
        }
    }
}

