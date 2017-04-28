namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;

    internal class System_ArraySegment_1MessagePackSerializer<T> : MessagePackSerializer<ArraySegment<T>>
    {
        private readonly MessagePackSerializer<T> _itemSerializer;
        private static readonly Action<Packer, ArraySegment<T>, MessagePackSerializer<T>> _packing;
        private static readonly Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>> _unpacking;

        static System_ArraySegment_1MessagePackSerializer()
        {
            System_ArraySegment_1MessagePackSerializer<T>._packing = System_ArraySegment_1MessagePackSerializer<T>.InitializePacking();
            System_ArraySegment_1MessagePackSerializer<T>._unpacking = System_ArraySegment_1MessagePackSerializer<T>.InitializeUnacking();
        }

        public System_ArraySegment_1MessagePackSerializer(SerializationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this._itemSerializer = context.GetSerializer<T>();
        }

        private static Action<Packer, ArraySegment<T>, MessagePackSerializer<T>> InitializePacking()
        {
            if (typeof(T) == typeof(byte))
            {
                return (Delegate.CreateDelegate(typeof(Action<Packer, ArraySegment<T>, MessagePackSerializer<T>>), ArraySegmentMessageSerializer.PackByteArraySegmentToMethod) as Action<Packer, ArraySegment<T>, MessagePackSerializer<T>>);
            }
            if (typeof(T) == typeof(char))
            {
                return (Delegate.CreateDelegate(typeof(Action<Packer, ArraySegment<T>, MessagePackSerializer<T>>), ArraySegmentMessageSerializer.PackCharArraySegmentToMethod) as Action<Packer, ArraySegment<T>, MessagePackSerializer<T>>);
            }
            return (Delegate.CreateDelegate(typeof(Action<Packer, ArraySegment<T>, MessagePackSerializer<T>>), ArraySegmentMessageSerializer.PackGenericArraySegmentTo1Method.MakeGenericMethod(new Type[] { typeof(T) })) as Action<Packer, ArraySegment<T>, MessagePackSerializer<T>>);
        }

        private static Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>> InitializeUnacking()
        {
            if (typeof(T) == typeof(byte))
            {
                return (Delegate.CreateDelegate(typeof(Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>>), ArraySegmentMessageSerializer.UnpackByteArraySegmentFromMethod) as Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>>);
            }
            if (typeof(T) == typeof(char))
            {
                return (Delegate.CreateDelegate(typeof(Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>>), ArraySegmentMessageSerializer.UnpackCharArraySegmentFromMethod) as Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>>);
            }
            return (Delegate.CreateDelegate(typeof(Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>>), ArraySegmentMessageSerializer.UnpackGenericArraySegmentFrom1Method.MakeGenericMethod(new Type[] { typeof(T) })) as Func<Unpacker, MessagePackSerializer<T>, ArraySegment<T>>);
        }

        protected internal sealed override void PackToCore(Packer packer, ArraySegment<T> objectTree)
        {
            System_ArraySegment_1MessagePackSerializer<T>._packing(packer, objectTree, this._itemSerializer);
        }

        protected internal sealed override ArraySegment<T> UnpackFromCore(Unpacker unpacker)
        {
            return System_ArraySegment_1MessagePackSerializer<T>._unpacking(unpacker, this._itemSerializer);
        }
    }
}

