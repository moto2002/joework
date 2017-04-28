namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Collections.Generic;

    internal sealed class System_Collections_Generic_KeyValuePair_2MessagePackSerializer<TKey, TValue> : MessagePackSerializer<KeyValuePair<TKey, TValue>>
    {
        private readonly MessagePackSerializer<TKey> _keySerializer;
        private readonly MessagePackSerializer<TValue> _valueSerializer;

        public System_Collections_Generic_KeyValuePair_2MessagePackSerializer(SerializationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this._keySerializer = context.GetSerializer<TKey>();
            this._valueSerializer = context.GetSerializer<TValue>();
        }

        private static Action<Packer, KeyValuePair<TKey, TValue>> CreatePackToCore()
        {
            throw new NotImplementedException();
        }

        private static Func<Unpacker, KeyValuePair<TKey, TValue>> CreateUnpackFromCore()
        {
            throw new NotImplementedException();
        }

        protected internal sealed override void PackToCore(Packer packer, KeyValuePair<TKey, TValue> objectTree)
        {
            packer.PackArrayHeader(2);
            this._keySerializer.PackTo(packer, objectTree.Key);
            this._valueSerializer.PackTo(packer, objectTree.Value);
        }

        protected internal sealed override KeyValuePair<TKey, TValue> UnpackFromCore(Unpacker unpacker)
        {
            if (!unpacker.Read())
            {
                throw SerializationExceptions.NewUnexpectedEndOfStream();
            }
            TKey key = unpacker.Data.Value.IsNil ? default(TKey) : this._keySerializer.UnpackFrom(unpacker);
            if (!unpacker.Read())
            {
                throw SerializationExceptions.NewUnexpectedEndOfStream();
            }
            return new KeyValuePair<TKey, TValue>(key, unpacker.Data.Value.IsNil ? default(TValue) : this._valueSerializer.UnpackFrom(unpacker));
        }
    }
}

