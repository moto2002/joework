namespace MsgPack
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [Serializable, DebuggerTypeProxy(typeof(DictionaryDebuggerProxy<,>))]
    public class MessagePackObjectDictionary : IDictionary<MessagePackObject, MessagePackObject>, ICollection<KeyValuePair<MessagePackObject, MessagePackObject>>, IEnumerable<KeyValuePair<MessagePackObject, MessagePackObject>>, IDictionary, ICollection, IEnumerable
    {
        private Dictionary<MessagePackObject, MessagePackObject> _dictionary;
        private const int _dictionaryInitialCapacity = 20;
        private bool _isFrozen;
        private List<MessagePackObject> _keys;
        private const int _listInitialCapacity = 10;
        private const int _threashold = 10;
        private List<MessagePackObject> _values;
        private int _version;

        public MessagePackObjectDictionary()
        {
            this._keys = new List<MessagePackObject>(10);
            this._values = new List<MessagePackObject>(10);
        }

        public MessagePackObjectDictionary(IDictionary<MessagePackObject, MessagePackObject> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }
            if (dictionary.Count <= 10)
            {
                this._keys = new List<MessagePackObject>(dictionary.Count);
                this._values = new List<MessagePackObject>(dictionary.Count);
            }
            else
            {
                this._dictionary = new Dictionary<MessagePackObject, MessagePackObject>(dictionary.Count, MessagePackObjectEqualityComparer.Instance);
            }
            try
            {
                foreach (KeyValuePair<MessagePackObject, MessagePackObject> pair in dictionary)
                {
                    this.AddCore(pair.Key, pair.Value, false);
                }
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException("Failed to copy specified dictionary.", "dictionary", exception);
            }
        }

        public MessagePackObjectDictionary(int initialCapacity)
        {
            if (initialCapacity < 0)
            {
                throw new ArgumentOutOfRangeException("initialCapacity");
            }
            if (initialCapacity <= 10)
            {
                this._keys = new List<MessagePackObject>(initialCapacity);
                this._values = new List<MessagePackObject>(initialCapacity);
            }
            else
            {
                this._dictionary = new Dictionary<MessagePackObject, MessagePackObject>(initialCapacity, MessagePackObjectEqualityComparer.Instance);
            }
        }

        public void Add(MessagePackObject key, MessagePackObject value)
        {
            if (key.IsNil)
            {
                ThrowKeyNotNilException("key");
            }
            this.VerifyIsNotFrozen();
            this.AddCore(key, value, false);
        }

        private void AddCore(MessagePackObject key, MessagePackObject value, bool allowOverwrite)
        {
            Predicate<MessagePackObject> match = null;
            Predicate<MessagePackObject> predicate2 = null;
            Contract.Assert(!key.IsNil);
            if (this._dictionary == null)
            {
                int num;
                if (this._keys.Count < 10)
                {
                    if (match == null)
                    {
                        match = item => item == key;
                    }
                    num = this._keys.FindIndex(match);
                    if (num < 0)
                    {
                        this._keys.Add(key);
                        this._values.Add(value);
                    }
                    else
                    {
                        if (!allowOverwrite)
                        {
                            ThrowDuplicatedKeyException(key, "key");
                        }
                        this._values[num] = value;
                    }
                    this._version++;
                    return;
                }
                if ((this._keys.Count == 10) && allowOverwrite)
                {
                    if (predicate2 == null)
                    {
                        predicate2 = item => item == key;
                    }
                    num = this._keys.FindIndex(predicate2);
                    if (0 <= num)
                    {
                        this._values[num] = value;
                        this._version++;
                        return;
                    }
                }
                this._dictionary = new Dictionary<MessagePackObject, MessagePackObject>(20, MessagePackObjectEqualityComparer.Instance);
                for (int i = 0; i < this._keys.Count; i++)
                {
                    this._dictionary.Add(this._keys[i], this._values[i]);
                }
                this._keys = null;
                this._values = null;
            }
            if (allowOverwrite)
            {
                this._dictionary[key] = value;
            }
            else
            {
                try
                {
                    this._dictionary.Add(key, value);
                }
                catch (ArgumentException)
                {
                    ThrowDuplicatedKeyException(key, "key");
                }
            }
            this._version++;
        }

        public MessagePackObjectDictionary AsFrozen()
        {
            return new MessagePackObjectDictionary(this).Freeze();
        }

        [Conditional("DEBUG")]
        private void AssertInvariant()
        {
            if (this._dictionary == null)
            {
                Contract.Assert(this._keys != null);
                Contract.Assert(this._values != null);
                Contract.Assert(this._keys.Count == this._values.Count);
                Contract.Assert(this._keys.Distinct<MessagePackObject>(MessagePackObjectEqualityComparer.Instance).Count<MessagePackObject>() == this._keys.Count);
            }
            else
            {
                Contract.Assert(this._keys == null);
                Contract.Assert(this._values == null);
            }
        }

        public void Clear()
        {
            this.VerifyIsNotFrozen();
            this.AssertInvariant();
            if (this._dictionary == null)
            {
                this._keys.Clear();
                this._values.Clear();
            }
            else
            {
                this._dictionary.Clear();
            }
            this._version++;
        }

        public bool ContainsKey(MessagePackObject key)
        {
            if (key.IsNil)
            {
                ThrowKeyNotNilException("key");
            }
            this.AssertInvariant();
            if (this._dictionary == null)
            {
                return this._keys.Contains<MessagePackObject>(key, MessagePackObjectEqualityComparer.Instance);
            }
            return this._dictionary.ContainsKey(key);
        }

        public bool ContainsValue(MessagePackObject value)
        {
            this.AssertInvariant();
            if (this._dictionary == null)
            {
                return this._values.Contains<MessagePackObject>(value, MessagePackObjectEqualityComparer.Instance);
            }
            return this._dictionary.ContainsValue(value);
        }

        public MessagePackObjectDictionary Freeze()
        {
            this._isFrozen = true;
            return this;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public bool Remove(MessagePackObject key)
        {
            if (key.IsNil)
            {
                ThrowKeyNotNilException("key");
            }
            this.VerifyIsNotFrozen();
            return this.RemoveCore(key, new MessagePackObject(), false);
        }

        private bool RemoveCore(MessagePackObject key, MessagePackObject value, bool checkValue)
        {
            Predicate<MessagePackObject> match = null;
            Contract.Assert(!key.IsNil);
            this.AssertInvariant();
            if (this._dictionary == null)
            {
                if (match == null)
                {
                    match = item => item == key;
                }
                int index = this._keys.FindIndex(match);
                if (index < 0)
                {
                    return false;
                }
                if (checkValue && (this._values[index] != value))
                {
                    return false;
                }
                this._keys.RemoveAt(index);
                this._values.RemoveAt(index);
            }
            else if (checkValue)
            {
                if (!this._dictionary.Remove(new KeyValuePair<MessagePackObject, MessagePackObject>(key, value)))
                {
                    return false;
                }
            }
            else if (!this._dictionary.Remove(key))
            {
                return false;
            }
            this._version++;
            return true;
        }

        void ICollection<KeyValuePair<MessagePackObject, MessagePackObject>>.Add(KeyValuePair<MessagePackObject, MessagePackObject> item)
        {
            if (item.Key.IsNil)
            {
                ThrowKeyNotNilException("key");
            }
            this.VerifyIsNotFrozen();
            this.AddCore(item.Key, item.Value, false);
        }

        bool ICollection<KeyValuePair<MessagePackObject, MessagePackObject>>.Contains(KeyValuePair<MessagePackObject, MessagePackObject> item)
        {
            MessagePackObject obj2;
            if (!this.TryGetValue(item.Key, out obj2))
            {
                return false;
            }
            return (item.Value == obj2);
        }

        void ICollection<KeyValuePair<MessagePackObject, MessagePackObject>>.CopyTo(KeyValuePair<MessagePackObject, MessagePackObject>[] array, int arrayIndex)
        {
            CollectionOperation.CopyTo<KeyValuePair<MessagePackObject, MessagePackObject>>(this, this.Count, 0, array, arrayIndex, this.Count);
        }

        bool ICollection<KeyValuePair<MessagePackObject, MessagePackObject>>.Remove(KeyValuePair<MessagePackObject, MessagePackObject> item)
        {
            if (item.Key.IsNil)
            {
                ThrowKeyNotNilException("key");
            }
            this.VerifyIsNotFrozen();
            return this.RemoveCore(item.Key, item.Value, true);
        }

        IEnumerator<KeyValuePair<MessagePackObject, MessagePackObject>> IEnumerable<KeyValuePair<MessagePackObject, MessagePackObject>>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            DictionaryEntry[] entryArray = array as DictionaryEntry[];
            if (entryArray != null)
            {
                CollectionOperation.CopyTo<KeyValuePair<MessagePackObject, MessagePackObject>, DictionaryEntry>(this, this.Count, 0, entryArray, index, array.Length, kv => new DictionaryEntry(kv.Key, kv.Value));
            }
            else
            {
                CollectionOperation.CopyTo<KeyValuePair<MessagePackObject, MessagePackObject>>(this, this.Count, array, index);
            }
        }

        void IDictionary.Add(object key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            this.VerifyIsNotFrozen();
            MessagePackObject obj2 = ValidateObjectArgument(key, "key");
            if (obj2.IsNil)
            {
                ThrowKeyNotNilException("key");
            }
            this.AddCore(obj2, ValidateObjectArgument(value, "value"), false);
        }

        bool IDictionary.Contains(object key)
        {
            if (key == null)
            {
                return false;
            }
            MessagePackObject? nullable = TryValidateObjectArgument(key);
            if (nullable.GetValueOrDefault().IsNil)
            {
                return false;
            }
            return this.ContainsKey(nullable.Value);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new DictionaryEnumerator(this);
        }

        void IDictionary.Remove(object key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            this.VerifyIsNotFrozen();
            MessagePackObject obj2 = ValidateObjectArgument(key, "key");
            if (obj2.IsNil)
            {
                ThrowKeyNotNilException("key");
            }
            this.RemoveCore(obj2, new MessagePackObject(), false);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private static void ThrowDuplicatedKeyException(MessagePackObject key, string parameterName)
        {
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Key '{0}'({1} type) already exists in this dictionary.", new object[] { key, key.UnderlyingType }), parameterName);
        }

        private static void ThrowKeyNotNilException(string parameterName)
        {
            throw new ArgumentNullException(parameterName, "Key cannot be nil.");
        }

        public bool TryGetValue(MessagePackObject key, out MessagePackObject value)
        {
            Predicate<MessagePackObject> match = null;
            if (key.IsNil)
            {
                ThrowKeyNotNilException("key");
            }
            this.AssertInvariant();
            if (this._dictionary == null)
            {
                if (match == null)
                {
                    match = item => item == key;
                }
                int num = this._keys.FindIndex(match);
                if (num < 0)
                {
                    value = MessagePackObject.Nil;
                    return false;
                }
                value = this._values[num];
                return true;
            }
            return this._dictionary.TryGetValue(key, out value);
        }

        private static MessagePackObject? TryValidateObjectArgument(object value)
        {
            if (value == null)
            {
                return new MessagePackObject?(MessagePackObject.Nil);
            }
            if (value is MessagePackObject)
            {
                return new MessagePackObject?((MessagePackObject) value);
            }
            if (value is MessagePackObject?)
            {
                MessagePackObject? nullable2 = (MessagePackObject?) value;
                return new MessagePackObject?(nullable2.HasValue ? nullable2.GetValueOrDefault() : MessagePackObject.Nil);
            }
            byte[] buffer = value as byte[];
            if (buffer != null)
            {
                return new MessagePackObject?(buffer);
            }
            string str = value as string;
            if (str != null)
            {
                return new MessagePackObject?(str);
            }
            MessagePackString messagePackString = value as MessagePackString;
            if (messagePackString != null)
            {
                return new MessagePackObject(messagePackString);
            }
            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Empty:
                case TypeCode.DBNull:
                    return new MessagePackObject?(MessagePackObject.Nil);

                case TypeCode.Boolean:
                    return new MessagePackObject?((bool) value);

                case TypeCode.SByte:
                    return new MessagePackObject?((sbyte) value);

                case TypeCode.Byte:
                    return new MessagePackObject?((byte) value);

                case TypeCode.Int16:
                    return new MessagePackObject?((short) value);

                case TypeCode.UInt16:
                    return new MessagePackObject?((ushort) value);

                case TypeCode.Int32:
                    return new MessagePackObject?((int) value);

                case TypeCode.UInt32:
                    return new MessagePackObject?((uint) value);

                case TypeCode.Int64:
                    return new MessagePackObject?((long) value);

                case TypeCode.UInt64:
                    return new MessagePackObject?((ulong) value);

                case TypeCode.Single:
                    return new MessagePackObject?((float) value);

                case TypeCode.Double:
                    return new MessagePackObject?((double) value);

                case TypeCode.DateTime:
                    return new MessagePackObject?(MessagePackConvert.FromDateTime((DateTime) value));

                case TypeCode.String:
                    return new MessagePackObject?(value.ToString());
            }
            return null;
        }

        private static MessagePackObject ValidateObjectArgument(object obj, string parameterName)
        {
            MessagePackObject? nullable = TryValidateObjectArgument(obj);
            if (!nullable.HasValue)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{1}' to {0}.", new object[] { typeof(MessagePackObject).Name, obj.GetType() }), parameterName);
            }
            return nullable.Value;
        }

        private void VerifyIsNotFrozen()
        {
            if (this._isFrozen)
            {
                throw new InvalidOperationException("This dictionary is frozen.");
            }
        }

        public int Count
        {
            get
            {
                this.AssertInvariant();
                if (this._dictionary == null)
                {
                    return this._keys.Count;
                }
                return this._dictionary.Count;
            }
        }

        public bool IsFrozen
        {
            get
            {
                return this._isFrozen;
            }
        }

        public MessagePackObject this[MessagePackObject key]
        {
            get
            {
                MessagePackObject obj2;
                if (key.IsNil)
                {
                    ThrowKeyNotNilException("key");
                }
                if (!this.TryGetValue(key, out obj2))
                {
                    throw new KeyNotFoundException(string.Format(CultureInfo.CurrentCulture, "Key '{0}'({1} type) does not exist in this dictionary.", new object[] { key, key.UnderlyingType }));
                }
                return obj2;
            }
            set
            {
                if (key.IsNil)
                {
                    ThrowKeyNotNilException("key");
                }
                this.VerifyIsNotFrozen();
                this.AssertInvariant();
                this.AddCore(key, value, true);
            }
        }

        public KeySet Keys
        {
            get
            {
                this.AssertInvariant();
                return new KeySet(this);
            }
        }

        bool ICollection<KeyValuePair<MessagePackObject, MessagePackObject>>.IsReadOnly
        {
            get
            {
                return this.IsFrozen;
            }
        }

        ICollection<MessagePackObject> IDictionary<MessagePackObject, MessagePackObject>.Keys
        {
            get
            {
                return this.Keys;
            }
        }

        ICollection<MessagePackObject> IDictionary<MessagePackObject, MessagePackObject>.Values
        {
            get
            {
                return this.Values;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return this;
            }
        }

        bool IDictionary.IsFixedSize
        {
            get
            {
                return this.IsFrozen;
            }
        }

        bool IDictionary.IsReadOnly
        {
            get
            {
                return this.IsFrozen;
            }
        }

        object IDictionary.this[object key]
        {
            get
            {
                MessagePackObject obj3;
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }
                MessagePackObject obj2 = ValidateObjectArgument(key, "key");
                if (obj2.IsNil)
                {
                    ThrowKeyNotNilException("key");
                }
                if (!this.TryGetValue(obj2, out obj3))
                {
                    return null;
                }
                return obj3;
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }
                this.VerifyIsNotFrozen();
                MessagePackObject obj2 = ValidateObjectArgument(key, "key");
                if (obj2.IsNil)
                {
                    ThrowKeyNotNilException("key");
                }
                this[obj2] = ValidateObjectArgument(value, "value");
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                return this._keys;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                return this.Values;
            }
        }

        public ValueCollection Values
        {
            get
            {
                this.AssertInvariant();
                return new ValueCollection(this);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct DictionaryEnumerator : IDictionaryEnumerator, IEnumerator
        {
            private IDictionaryEnumerator _underlying;
            public object Current
            {
                get
                {
                    return this._underlying.Entry;
                }
            }
            public DictionaryEntry Entry
            {
                get
                {
                    return this._underlying.Entry;
                }
            }
            public object Key
            {
                get
                {
                    return this.Entry.Key;
                }
            }
            public object Value
            {
                get
                {
                    return this.Entry.Value;
                }
            }
            internal DictionaryEnumerator(MessagePackObjectDictionary dictionary)
            {
                this = new MessagePackObjectDictionary.DictionaryEnumerator();
                Contract.Assert(dictionary != null);
                this._underlying = new MessagePackObjectDictionary.Enumerator(dictionary);
            }

            public bool MoveNext()
            {
                return this._underlying.MoveNext();
            }

            void IEnumerator.Reset()
            {
                this._underlying.Reset();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Enumerator : IEnumerator<KeyValuePair<MessagePackObject, MessagePackObject>>, IDisposable, IDictionaryEnumerator, IEnumerator
        {
            private const int _beforeHead = -1;
            private const int _isDictionary = -2;
            private const int _end = -3;
            private readonly MessagePackObjectDictionary _underlying;
            private Dictionary<MessagePackObject, MessagePackObject>.Enumerator _enumerator;
            private KeyValuePair<MessagePackObject, MessagePackObject> _current;
            private int _position;
            private int _initialVersion;
            public KeyValuePair<MessagePackObject, MessagePackObject> Current
            {
                get
                {
                    this.VerifyVersion();
                    return this._current;
                }
            }
            object IEnumerator.Current
            {
                get
                {
                    return this.GetCurrentStrict();
                }
            }
            DictionaryEntry IDictionaryEnumerator.Entry
            {
                get
                {
                    KeyValuePair<MessagePackObject, MessagePackObject> currentStrict = this.GetCurrentStrict();
                    return new DictionaryEntry(currentStrict.Key, currentStrict.Value);
                }
            }
            object IDictionaryEnumerator.Key
            {
                get
                {
                    return this.GetCurrentStrict().Key;
                }
            }
            object IDictionaryEnumerator.Value
            {
                get
                {
                    return this.GetCurrentStrict().Value;
                }
            }
            internal KeyValuePair<MessagePackObject, MessagePackObject> GetCurrentStrict()
            {
                this.VerifyVersion();
                if ((this._position == -1) || (this._position == -3))
                {
                    throw new InvalidOperationException("The enumerator is positioned before the first element of the collection or after the last element.");
                }
                return this._current;
            }

            internal Enumerator(MessagePackObjectDictionary dictionary)
            {
                this = new MessagePackObjectDictionary.Enumerator();
                Contract.Assert(dictionary != null);
                this = new MessagePackObjectDictionary.Enumerator();
                this._underlying = dictionary;
                this.ResetCore();
            }

            internal void VerifyVersion()
            {
                if ((this._underlying != null) && (this._underlying._version != this._initialVersion))
                {
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");
                }
            }

            public void Dispose()
            {
                this._enumerator.Dispose();
            }

            public bool MoveNext()
            {
                if (this._position == -3)
                {
                    return false;
                }
                if (this._position == -2)
                {
                    if (!this._enumerator.MoveNext())
                    {
                        return false;
                    }
                    this._current = this._enumerator.Current;
                    return true;
                }
                if (this._position == -1)
                {
                    if (this._underlying._keys.Count == 0)
                    {
                        this._position = -3;
                        return false;
                    }
                    this._position = 0;
                }
                else
                {
                    this._position++;
                }
                if (this._position == this._underlying._keys.Count)
                {
                    this._position = -3;
                    return false;
                }
                this._current = new KeyValuePair<MessagePackObject, MessagePackObject>(this._underlying._keys[this._position], this._underlying._values[this._position]);
                return true;
            }

            void IEnumerator.Reset()
            {
                this.ResetCore();
            }

            internal void ResetCore()
            {
                this._initialVersion = this._underlying._version;
                this._current = new KeyValuePair<MessagePackObject, MessagePackObject>();
                this._initialVersion = this._underlying._version;
                if (this._underlying._dictionary != null)
                {
                    this._enumerator = this._underlying._dictionary.GetEnumerator();
                    this._position = -2;
                }
                else
                {
                    this._position = -1;
                }
            }
        }

        [Serializable, DebuggerTypeProxy(typeof(CollectionDebuggerProxy<>)), DebuggerDisplay("Count={Count}")]
        public sealed class KeySet : ICollection<MessagePackObject>, IEnumerable<MessagePackObject>, ICollection, IEnumerable
        {
            private readonly MessagePackObjectDictionary _dictionary;

            internal KeySet(MessagePackObjectDictionary dictionary)
            {
                Contract.Assert(dictionary != null);
                this._dictionary = dictionary;
            }

            public void CopyTo(MessagePackObject[] array)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }
                CollectionOperation.CopyTo<MessagePackObject>(this, this.Count, 0, array, 0, this.Count);
            }

            public void CopyTo(MessagePackObject[] array, int arrayIndex)
            {
                CollectionOperation.CopyTo<MessagePackObject>(this, this.Count, 0, array, arrayIndex, this.Count);
            }

            public void CopyTo(int index, MessagePackObject[] array, int arrayIndex, int count)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                if ((0 < this.Count) && (this.Count <= index))
                {
                    throw new ArgumentException("Specified array is too small to complete copy operation.", "array");
                }
                if (arrayIndex < 0)
                {
                    throw new ArgumentOutOfRangeException("arrayIndex");
                }
                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException("count");
                }
                if ((array.Length - count) <= arrayIndex)
                {
                    throw new ArgumentException("Specified array is too small to complete copy operation.", "array");
                }
                CollectionOperation.CopyTo<MessagePackObject>(this, this.Count, index, array, arrayIndex, count);
            }

            public Enumerator GetEnumerator()
            {
                return new Enumerator(this._dictionary);
            }

            public bool IsProperSubsetOf(IEnumerable<MessagePackObject> other)
            {
                return SetOperation.IsProperSubsetOf<MessagePackObject>(this, other);
            }

            public bool IsProperSupersetOf(IEnumerable<MessagePackObject> other)
            {
                return SetOperation.IsProperSupersetOf<MessagePackObject>(this, other);
            }

            public bool IsSubsetOf(IEnumerable<MessagePackObject> other)
            {
                return SetOperation.IsSubsetOf<MessagePackObject>(this, other);
            }

            public bool IsSupersetOf(IEnumerable<MessagePackObject> other)
            {
                return SetOperation.IsSupersetOf<MessagePackObject>(this, other);
            }

            public bool Overlaps(IEnumerable<MessagePackObject> other)
            {
                return SetOperation.Overlaps<MessagePackObject>(this, other);
            }

            public bool SetEquals(IEnumerable<MessagePackObject> other)
            {
                return SetOperation.SetEquals<MessagePackObject>(this, other);
            }

            void ICollection<MessagePackObject>.Add(MessagePackObject item)
            {
                throw new NotSupportedException();
            }

            void ICollection<MessagePackObject>.Clear()
            {
                throw new NotSupportedException();
            }

            bool ICollection<MessagePackObject>.Contains(MessagePackObject item)
            {
                return this._dictionary.ContainsKey(item);
            }

            bool ICollection<MessagePackObject>.Remove(MessagePackObject item)
            {
                throw new NotSupportedException();
            }

            IEnumerator<MessagePackObject> IEnumerable<MessagePackObject>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            void ICollection.CopyTo(Array array, int arrayIndex)
            {
                CollectionOperation.CopyTo<MessagePackObject>(this, this.Count, array, arrayIndex);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public int Count
            {
                get
                {
                    return this._dictionary.Count;
                }
            }

            bool ICollection<MessagePackObject>.IsReadOnly
            {
                get
                {
                    return true;
                }
            }

            bool ICollection.IsSynchronized
            {
                get
                {
                    return false;
                }
            }

            object ICollection.SyncRoot
            {
                get
                {
                    return this;
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct Enumerator : IEnumerator<MessagePackObject>, IDisposable, IEnumerator
            {
                private MessagePackObjectDictionary.Enumerator _underlying;
                public MessagePackObject Current
                {
                    get
                    {
                        return this._underlying.Current.Key;
                    }
                }
                object IEnumerator.Current
                {
                    get
                    {
                        return this._underlying.GetCurrentStrict().Key;
                    }
                }
                internal Enumerator(MessagePackObjectDictionary dictionary)
                {
                    Contract.Assert(dictionary != null);
                    this._underlying = dictionary.GetEnumerator();
                }

                public void Dispose()
                {
                    this._underlying.Dispose();
                }

                public bool MoveNext()
                {
                    this._underlying.VerifyVersion();
                    return this._underlying.MoveNext();
                }

                void IEnumerator.Reset()
                {
                    this._underlying.ResetCore();
                }
            }
        }

        [Serializable, DebuggerTypeProxy(typeof(CollectionDebuggerProxy<>)), DebuggerDisplay("Count={Count}")]
        public sealed class ValueCollection : ICollection<MessagePackObject>, IEnumerable<MessagePackObject>, ICollection, IEnumerable
        {
            private readonly MessagePackObjectDictionary _dictionary;

            internal ValueCollection(MessagePackObjectDictionary dictionary)
            {
                Contract.Assert(dictionary != null);
                this._dictionary = dictionary;
            }

            public void CopyTo(MessagePackObject[] array)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }
                CollectionOperation.CopyTo<MessagePackObject>(this, this.Count, 0, array, 0, this.Count);
            }

            public void CopyTo(MessagePackObject[] array, int arrayIndex)
            {
                CollectionOperation.CopyTo<MessagePackObject>(this, this.Count, 0, array, arrayIndex, this.Count);
            }

            public void CopyTo(int index, MessagePackObject[] array, int arrayIndex, int count)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                if ((0 < this.Count) && (this.Count <= index))
                {
                    throw new ArgumentException("Specified array is too small to complete copy operation.", "array");
                }
                if (arrayIndex < 0)
                {
                    throw new ArgumentOutOfRangeException("arrayIndex");
                }
                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException("count");
                }
                if ((array.Length - count) <= arrayIndex)
                {
                    throw new ArgumentException("Specified array is too small to complete copy operation.", "array");
                }
                CollectionOperation.CopyTo<MessagePackObject>(this, this.Count, index, array, arrayIndex, count);
            }

            public Enumerator GetEnumerator()
            {
                return new Enumerator(this._dictionary);
            }

            void ICollection<MessagePackObject>.Add(MessagePackObject item)
            {
                throw new NotSupportedException();
            }

            void ICollection<MessagePackObject>.Clear()
            {
                throw new NotSupportedException();
            }

            bool ICollection<MessagePackObject>.Contains(MessagePackObject item)
            {
                return this._dictionary.ContainsValue(item);
            }

            bool ICollection<MessagePackObject>.Remove(MessagePackObject item)
            {
                throw new NotSupportedException();
            }

            IEnumerator<MessagePackObject> IEnumerable<MessagePackObject>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            void ICollection.CopyTo(Array array, int arrayIndex)
            {
                CollectionOperation.CopyTo<MessagePackObject>(this, this.Count, array, arrayIndex);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public int Count
            {
                get
                {
                    return this._dictionary.Count;
                }
            }

            bool ICollection<MessagePackObject>.IsReadOnly
            {
                get
                {
                    return true;
                }
            }

            bool ICollection.IsSynchronized
            {
                get
                {
                    return false;
                }
            }

            object ICollection.SyncRoot
            {
                get
                {
                    return this;
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct Enumerator : IEnumerator<MessagePackObject>, IDisposable, IEnumerator
            {
                private MessagePackObjectDictionary.Enumerator _underlying;
                public MessagePackObject Current
                {
                    get
                    {
                        return this._underlying.Current.Value;
                    }
                }
                object IEnumerator.Current
                {
                    get
                    {
                        return this._underlying.GetCurrentStrict().Value;
                    }
                }
                internal Enumerator(MessagePackObjectDictionary dictionary)
                {
                    Contract.Assert(dictionary != null);
                    this._underlying = dictionary.GetEnumerator();
                }

                public void Dispose()
                {
                    this._underlying.Dispose();
                }

                public bool MoveNext()
                {
                    this._underlying.VerifyVersion();
                    return this._underlying.MoveNext();
                }

                void IEnumerator.Reset()
                {
                    this._underlying.ResetCore();
                }
            }
        }
    }
}

