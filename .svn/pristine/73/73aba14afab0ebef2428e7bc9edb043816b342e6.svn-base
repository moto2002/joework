namespace MsgPack.Serialization
{
    using MsgPack;
    using System;
    using System.Globalization;

    internal sealed class LazyDelegatingMessagePackSerializer<T> : MessagePackSerializer<T>
    {
        private readonly SerializationContext _context;
        private MessagePackSerializer<T> _delegated;

        public LazyDelegatingMessagePackSerializer(SerializationContext context)
        {
            this._context = context;
        }

        private MessagePackSerializer<T> GetDelegatedSerializer()
        {
            MessagePackSerializer<T> serializer = this._delegated;
            if (serializer == null)
            {
                serializer = this._context.GetSerializer<T>();
                if (serializer is LazyDelegatingMessagePackSerializer<T>)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "MessagePack serializer for the type '{0}' is not constructed yet.", new object[] { typeof(T) }));
                }
                this._delegated = serializer;
            }
            return serializer;
        }

        protected internal sealed override void PackToCore(Packer packer, T objectTree)
        {
            this.GetDelegatedSerializer().PackToCore(packer, objectTree);
        }

        public override string ToString()
        {
            return this.GetDelegatedSerializer().ToString();
        }

        protected internal sealed override T UnpackFromCore(Unpacker unpacker)
        {
            return this.GetDelegatedSerializer().UnpackFromCore(unpacker);
        }

        protected internal sealed override void UnpackToCore(Unpacker unpacker, T collection)
        {
            this.GetDelegatedSerializer().UnpackToCore(unpacker, collection);
        }
    }
}

