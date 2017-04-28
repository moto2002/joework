namespace MsgPack.Serialization
{
    using MsgPack.Serialization.Metadata;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Threading;

    public sealed class SerializationContext
    {
        private readonly SerializationCompatibilityOptions _compatibilityOptions;
        private static SerializationContext _default = new SerializationContext();
        private MsgPack.Serialization.EmitterFlavor _emitterFlavor;
        private SerializationMethodGeneratorOption _generatorOption;
        private MsgPack.Serialization.SerializationMethod _serializationMethod;
        private readonly SerializerRepository _serializers;
        private readonly HashSet<Type> _typeLock;

        public SerializationContext() : this(new SerializerRepository(SerializerRepository.Default))
        {
        }

        internal SerializationContext(SerializerRepository serializers)
        {
            this._emitterFlavor = MsgPack.Serialization.EmitterFlavor.FieldBased;
            Contract.Requires(serializers != null);
            this._compatibilityOptions = new SerializationCompatibilityOptions();
            this._serializers = serializers;
            this._typeLock = new HashSet<Type>();
        }

        public MessagePackSerializer<T> GetSerializer<T>()
        {
            MessagePackSerializer<T> serializer = this._serializers.Get<T>(this);
            if (serializer == null)
            {
                HashSet<Type> set;
                bool flag = false;
                try
                {
                    try
                    {
                    }
                    finally
                    {
                        lock ((set = this._typeLock))
                        {
                            flag = this._typeLock.Add(typeof(T));
                        }
                    }
                    if (!flag)
                    {
                        return new LazyDelegatingMessagePackSerializer<T>(this);
                    }
                    serializer = MessagePackSerializer.Create<T>(this);
                }
                finally
                {
                    if (flag)
                    {
                        lock ((set = this._typeLock))
                        {
                            this._typeLock.Remove(typeof(T));
                        }
                    }
                }
                if (!this._serializers.Register<T>(serializer))
                {
                    serializer = this._serializers.Get<T>(this);
                }
            }
            return serializer;
        }

        public IMessagePackSingleObjectSerializer GetSerializer(Type targetType)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }
            return SerializerGetter.Instance.Get(this, targetType);
        }

        public SerializationCompatibilityOptions CompatibilityOptions
        {
            get
            {
                return this._compatibilityOptions;
            }
        }

        public static SerializationContext Default
        {
            get
            {
                return Interlocked.CompareExchange<SerializationContext>(ref _default, null, null);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                Interlocked.Exchange<SerializationContext>(ref _default, value);
            }
        }

        internal MsgPack.Serialization.EmitterFlavor EmitterFlavor
        {
            get
            {
                return this._emitterFlavor;
            }
            set
            {
                this._emitterFlavor = value;
            }
        }

        public SerializationMethodGeneratorOption GeneratorOption
        {
            get
            {
                return this._generatorOption;
            }
            set
            {
                switch (value)
                {
                    case SerializationMethodGeneratorOption.CanDump:
                    case SerializationMethodGeneratorOption.CanCollect:
                    case SerializationMethodGeneratorOption.Fast:
                        this._generatorOption = value;
                        return;
                }
                throw new ArgumentOutOfRangeException("value");
            }
        }

        public MsgPack.Serialization.SerializationMethod SerializationMethod
        {
            get
            {
                return this._serializationMethod;
            }
            set
            {
                switch (value)
                {
                    case MsgPack.Serialization.SerializationMethod.Array:
                    case MsgPack.Serialization.SerializationMethod.Map:
                        this._serializationMethod = value;
                        return;
                }
                throw new ArgumentOutOfRangeException("value");
            }
        }

        public SerializerRepository Serializers
        {
            get
            {
                return this._serializers;
            }
        }

        private sealed class SerializerGetter
        {
            private readonly Dictionary<RuntimeTypeHandle, Func<SerializationContext, IMessagePackSingleObjectSerializer>> _cache = new Dictionary<RuntimeTypeHandle, Func<SerializationContext, IMessagePackSingleObjectSerializer>>();
            public static readonly SerializationContext.SerializerGetter Instance = new SerializationContext.SerializerGetter();

            private SerializerGetter()
            {
            }

            public IMessagePackSingleObjectSerializer Get(SerializationContext context, Type targetType)
            {
                Func<SerializationContext, IMessagePackSingleObjectSerializer> func;
                if (!(this._cache.TryGetValue(targetType.TypeHandle, out func) && (func != null)))
                {
                    func = Delegate.CreateDelegate(typeof(Func<SerializationContext, IMessagePackSingleObjectSerializer>), typeof(SerializationContext.SerializerGetter<>).MakeGenericType(new Type[] { targetType }).GetMethod("Get")) as Func<SerializationContext, IMessagePackSingleObjectSerializer>;
                    this._cache[targetType.TypeHandle] = func;
                }
                return func(context);
            }
        }

        private static class SerializerGetter<T>
        {
            private static readonly Func<SerializationContext, MessagePackSerializer<T>> _func;

            static SerializerGetter()
            {
                SerializationContext.SerializerGetter<T>._func = Delegate.CreateDelegate(typeof(Func<SerializationContext, MessagePackSerializer<T>>), _SerializationContext.GetSerializer1_Method.MakeGenericMethod(new Type[] { typeof(T) })) as Func<SerializationContext, MessagePackSerializer<T>>;
            }

            public static IMessagePackSingleObjectSerializer Get(SerializationContext context)
            {
                return SerializationContext.SerializerGetter<T>._func(context);
            }
        }
    }
}

