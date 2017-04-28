namespace MsgPack.Serialization
{
    using MsgPack;
    using System;

    internal sealed class CallbackMessagePackSerializer<T> : MessagePackSerializer<T>
    {
        private readonly SerializationContext _context;
        private readonly Action<SerializationContext, Packer, T> _packToCore;
        private readonly Func<SerializationContext, Unpacker, T> _unpackFromCore;
        private readonly Action<SerializationContext, Unpacker, T> _unpackToCore;

        public CallbackMessagePackSerializer(SerializationContext context, Action<SerializationContext, Packer, T> packToCore, Func<SerializationContext, Unpacker, T> unpackFromCore, Action<SerializationContext, Unpacker, T> unpackToCore)
        {
            this._context = context;
            this._packToCore = packToCore;
            this._unpackFromCore = unpackFromCore;
            this._unpackToCore = unpackToCore;
        }

        protected internal override void PackToCore(Packer packer, T objectTree)
        {
            this._packToCore(this._context, packer, objectTree);
        }

        protected internal override T UnpackFromCore(Unpacker unpacker)
        {
            return this._unpackFromCore(this._context, unpacker);
        }

        protected internal sealed override void UnpackToCore(Unpacker unpacker, T collection)
        {
            if (this._unpackToCore != null)
            {
                this._unpackToCore(this._context, unpacker, collection);
            }
            else
            {
                base.UnpackToCore(unpacker, collection);
            }
        }
    }
}

