namespace MsgPack.Serialization
{
    using MsgPack;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class UnpackHelpers
    {
        private static readonly MessagePackSerializer<MessagePackObject> _messagePackObjectSerializer = new MsgPack_MessagePackObjectMessagePackSerializer();

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static T ConvertWithEnsuringNotNull<T>(object boxed, string name, Type targetType)
        {
            if ((typeof(T).GetIsValueType() && (boxed == null)) && (Nullable.GetUnderlyingType(typeof(T)) == null))
            {
                throw SerializationExceptions.NewValueTypeCannotBeNull(name, typeof(T), targetType);
            }
            return (T) boxed;
        }

        internal static int GetItemsCount(Unpacker unpacker)
        {
            long itemsCount;
            try
            {
                itemsCount = unpacker.ItemsCount;
            }
            catch (InvalidOperationException exception)
            {
                throw SerializationExceptions.NewIsIncorrectStream(exception);
            }
            if (itemsCount > 0x7fffffffL)
            {
                throw SerializationExceptions.NewIsTooLargeCollection();
            }
            return (int) itemsCount;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static T InvokeUnpackFrom<T>(MessagePackSerializer<T> serializer, Unpacker unpacker)
        {
            return serializer.UnpackFromCore(unpacker);
        }

        internal static bool IsReadOnlyAppendableCollectionMember(MemberInfo memberInfo)
        {
            Contract.Requires(memberInfo != null);
            if (memberInfo.CanSetValue())
            {
                return false;
            }
            Type memberValueType = memberInfo.GetMemberValueType();
            if (memberValueType.IsArray)
            {
                return false;
            }
            CollectionTraits collectionTraits = memberValueType.GetCollectionTraits();
            return ((collectionTraits.CollectionType != CollectionKind.NotCollection) && (collectionTraits.AddMethod != null));
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void UnpackArrayTo<T>(Unpacker unpacker, MessagePackSerializer<T> serializer, T[] array)
        {
            if (unpacker == null)
            {
                throw new ArgumentNullException("unpacker");
            }
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (!unpacker.IsArrayHeader)
            {
                throw SerializationExceptions.NewIsNotArrayHeader();
            }
            int itemsCount = GetItemsCount(unpacker);
            for (int i = 0; i < itemsCount; i++)
            {
                T local;
                if (!unpacker.Read())
                {
                    throw SerializationExceptions.NewMissingItem(i);
                }
                if (!(unpacker.IsArrayHeader || unpacker.IsMapHeader))
                {
                    local = serializer.UnpackFrom(unpacker);
                }
                else
                {
                    using (Unpacker unpacker2 = unpacker.ReadSubtree())
                    {
                        local = serializer.UnpackFrom(unpacker2);
                    }
                }
                array[i] = local;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void UnpackCollectionTo(Unpacker unpacker, IEnumerable collection, Action<object> addition)
        {
            if (unpacker == null)
            {
                throw new ArgumentNullException("unpacker");
            }
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            if (!unpacker.IsArrayHeader)
            {
                throw SerializationExceptions.NewIsNotArrayHeader();
            }
            int itemsCount = GetItemsCount(unpacker);
            for (int i = 0; i < itemsCount; i++)
            {
                MessagePackObject obj2;
                if (!unpacker.Read())
                {
                    throw SerializationExceptions.NewMissingItem(i);
                }
                if (!(unpacker.IsArrayHeader || unpacker.IsMapHeader))
                {
                    obj2 = _messagePackObjectSerializer.UnpackFrom(unpacker);
                }
                else
                {
                    using (Unpacker unpacker2 = unpacker.ReadSubtree())
                    {
                        obj2 = _messagePackObjectSerializer.UnpackFrom(unpacker2);
                    }
                }
                addition(obj2);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void UnpackCollectionTo<TDiscarded>(Unpacker unpacker, IEnumerable collection, Func<object, TDiscarded> addition)
        {
            if (unpacker == null)
            {
                throw new ArgumentNullException("unpacker");
            }
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            if (!unpacker.IsArrayHeader)
            {
                throw SerializationExceptions.NewIsNotArrayHeader();
            }
            int itemsCount = GetItemsCount(unpacker);
            for (int i = 0; i < itemsCount; i++)
            {
                MessagePackObject obj2;
                if (!unpacker.Read())
                {
                    throw SerializationExceptions.NewMissingItem(i);
                }
                if (!(unpacker.IsArrayHeader || unpacker.IsMapHeader))
                {
                    obj2 = _messagePackObjectSerializer.UnpackFrom(unpacker);
                }
                else
                {
                    using (Unpacker unpacker2 = unpacker.ReadSubtree())
                    {
                        obj2 = _messagePackObjectSerializer.UnpackFrom(unpacker2);
                    }
                }
                addition(obj2);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void UnpackCollectionTo<T>(Unpacker unpacker, MessagePackSerializer<T> serializer, IEnumerable<T> collection, Action<T> addition)
        {
            if (unpacker == null)
            {
                throw new ArgumentNullException("unpacker");
            }
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            if (!unpacker.IsArrayHeader)
            {
                throw SerializationExceptions.NewIsNotArrayHeader();
            }
            int itemsCount = GetItemsCount(unpacker);
            for (int i = 0; i < itemsCount; i++)
            {
                T local;
                if (!unpacker.Read())
                {
                    throw SerializationExceptions.NewMissingItem(i);
                }
                if (!(unpacker.IsArrayHeader || unpacker.IsMapHeader))
                {
                    local = serializer.UnpackFrom(unpacker);
                }
                else
                {
                    using (Unpacker unpacker2 = unpacker.ReadSubtree())
                    {
                        local = serializer.UnpackFrom(unpacker2);
                    }
                }
                addition(local);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void UnpackCollectionTo<T, TDiscarded>(Unpacker unpacker, MessagePackSerializer<T> serializer, IEnumerable<T> collection, Func<T, TDiscarded> addition)
        {
            if (unpacker == null)
            {
                throw new ArgumentNullException("unpacker");
            }
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            if (!unpacker.IsArrayHeader)
            {
                throw SerializationExceptions.NewIsNotArrayHeader();
            }
            int itemsCount = GetItemsCount(unpacker);
            for (int i = 0; i < itemsCount; i++)
            {
                T local;
                if (!unpacker.Read())
                {
                    throw SerializationExceptions.NewMissingItem(i);
                }
                if (!(unpacker.IsArrayHeader || unpacker.IsMapHeader))
                {
                    local = serializer.UnpackFrom(unpacker);
                }
                else
                {
                    using (Unpacker unpacker2 = unpacker.ReadSubtree())
                    {
                        local = serializer.UnpackFrom(unpacker2);
                    }
                }
                addition(local);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void UnpackMapTo(Unpacker unpacker, IDictionary dictionary)
        {
            if (unpacker == null)
            {
                throw new ArgumentNullException("unpacker");
            }
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }
            if (!unpacker.IsMapHeader)
            {
                throw SerializationExceptions.NewIsNotMapHeader();
            }
            int itemsCount = GetItemsCount(unpacker);
            for (int i = 0; i < itemsCount; i++)
            {
                MessagePackObject obj2;
                Unpacker unpacker2;
                MessagePackObject obj3;
                if (!unpacker.Read())
                {
                    throw SerializationExceptions.NewMissingItem(i);
                }
                if (!(unpacker.IsArrayHeader || unpacker.IsMapHeader))
                {
                    obj2 = _messagePackObjectSerializer.UnpackFrom(unpacker);
                }
                else
                {
                    using (unpacker2 = unpacker.ReadSubtree())
                    {
                        obj2 = _messagePackObjectSerializer.UnpackFrom(unpacker2);
                    }
                }
                if (!unpacker.Read())
                {
                    throw SerializationExceptions.NewMissingItem(i);
                }
                if (!(unpacker.IsArrayHeader || unpacker.IsMapHeader))
                {
                    obj3 = _messagePackObjectSerializer.UnpackFrom(unpacker);
                }
                else
                {
                    using (unpacker2 = unpacker.ReadSubtree())
                    {
                        obj3 = _messagePackObjectSerializer.UnpackFrom(unpacker2);
                    }
                }
                dictionary.Add(obj2, obj3);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void UnpackMapTo<TKey, TValue>(Unpacker unpacker, MessagePackSerializer<TKey> keySerializer, MessagePackSerializer<TValue> valueSerializer, IDictionary<TKey, TValue> dictionary)
        {
            if (unpacker == null)
            {
                throw new ArgumentNullException("unpacker");
            }
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }
            if (!unpacker.IsMapHeader)
            {
                throw SerializationExceptions.NewIsNotMapHeader();
            }
            int itemsCount = GetItemsCount(unpacker);
            for (int i = 0; i < itemsCount; i++)
            {
                TKey local;
                Unpacker unpacker2;
                TValue local2;
                if (!unpacker.Read())
                {
                    throw SerializationExceptions.NewMissingItem(i);
                }
                if (!(unpacker.IsArrayHeader || unpacker.IsMapHeader))
                {
                    local = keySerializer.UnpackFrom(unpacker);
                }
                else
                {
                    using (unpacker2 = unpacker.ReadSubtree())
                    {
                        local = keySerializer.UnpackFrom(unpacker2);
                    }
                }
                if (!unpacker.Read())
                {
                    throw SerializationExceptions.NewMissingItem(i);
                }
                if (!(unpacker.IsArrayHeader || unpacker.IsMapHeader))
                {
                    local2 = valueSerializer.UnpackFrom(unpacker);
                }
                else
                {
                    using (unpacker2 = unpacker.ReadSubtree())
                    {
                        local2 = valueSerializer.UnpackFrom(unpacker2);
                    }
                }
                dictionary.Add(local, local2);
            }
        }
    }
}

