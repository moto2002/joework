namespace MsgPack.Serialization
{
    using MsgPack.Serialization.EmittingSerializers;
    using MsgPack.Serialization.Metadata;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public static class MessagePackSerializer
    {
        private static readonly Dictionary<Type, Func<SerializationContext, IMessagePackSingleObjectSerializer>> _creatorCache = new Dictionary<Type, Func<SerializationContext, IMessagePackSingleObjectSerializer>>();
        private static readonly object _syncRoot = new object();

        public static MessagePackSerializer<T> Create<T>()
        {
            return Create<T>(SerializationContext.Default);
        }

        public static MessagePackSerializer<T> Create<T>(SerializationContext context)
        {
            Func<SerializationContext, SerializerBuilder<T>> func;
            Func<SerializationContext, SerializerBuilder<T>> func2 = null;
            Func<SerializationContext, SerializerBuilder<T>> func3 = null;
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (context.SerializationMethod == SerializationMethod.Map)
            {
                if (func2 == null)
                {
                    func2 = c => new MapEmittingSerializerBuilder<T>(c);
                }
                func = func2;
            }
            else
            {
                if (func3 == null)
                {
                    func3 = c => new ArrayEmittingSerializerBuilder<T>(c);
                }
                func = func3;
            }
            return new AutoMessagePackSerializer<T>(context, func);
        }

        public static IMessagePackSingleObjectSerializer Create(Type targetType)
        {
            return Create(targetType, SerializationContext.Default);
        }

        public static IMessagePackSingleObjectSerializer Create(Type targetType, SerializationContext context)
        {
            Func<SerializationContext, IMessagePackSingleObjectSerializer> func;
            object obj2;
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            lock ((obj2 = _syncRoot))
            {
                _creatorCache.TryGetValue(targetType, out func);
            }
            if (func == null)
            {
                func = Delegate.CreateDelegate(typeof(Func<SerializationContext, IMessagePackSingleObjectSerializer>), _MessagePackSerializer.Create1_Method.MakeGenericMethod(new Type[] { targetType })) as Func<SerializationContext, IMessagePackSingleObjectSerializer>;
                Contract.Assert(func != null);
                lock ((obj2 = _syncRoot))
                {
                    _creatorCache[targetType] = func;
                }
            }
            return func(context);
        }
    }
}

