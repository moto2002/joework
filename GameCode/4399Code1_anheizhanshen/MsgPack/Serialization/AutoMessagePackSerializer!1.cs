namespace MsgPack.Serialization
{
    using MsgPack;
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;

    internal class AutoMessagePackSerializer<T> : MessagePackSerializer<T>
    {
        private readonly MessagePackSerializer<T> _underlying;

        public AutoMessagePackSerializer(SerializationContext context, Func<SerializationContext, SerializerBuilder<T>> builderProvider)
        {
            Contract.Assert(context != null);
            MessagePackSerializer<T> serializer = context.Serializers.Get<T>(context);
            if (serializer != null)
            {
                this._underlying = serializer;
            }
            else
            {
                switch (typeof(T).GetCollectionTraits().CollectionType)
                {
                    case CollectionKind.NotCollection:
                        if (((!typeof(T).GetAssembly().Equals(typeof(object).GetAssembly()) && !typeof(T).GetAssembly().Equals(typeof(Enumerable).GetAssembly())) || !typeof(T).GetIsPublic()) || !typeof(T).Name.StartsWith("Tuple`", StringComparison.Ordinal))
                        {
                            serializer = builderProvider(context).CreateSerializer();
                            break;
                        }
                        serializer = builderProvider(context).CreateTupleSerializer();
                        break;

                    case CollectionKind.Array:
                        serializer = builderProvider(context).CreateArraySerializer();
                        break;

                    case CollectionKind.Map:
                        serializer = builderProvider(context).CreateMapSerializer();
                        break;
                }
                if (serializer == null)
                {
                    throw SerializationExceptions.NewTypeCannotSerialize(typeof(T));
                }
                if (!context.Serializers.Register<T>(serializer))
                {
                    serializer = context.Serializers.Get<T>(context);
                    Contract.Assert(serializer != null);
                }
                this._underlying = serializer;
            }
        }

        protected internal sealed override void PackToCore(Packer packer, T objectTree)
        {
            this._underlying.PackTo(packer, objectTree);
        }

        public override string ToString()
        {
            return this._underlying.ToString();
        }

        protected internal sealed override T UnpackFromCore(Unpacker unpacker)
        {
            return this._underlying.UnpackFrom(unpacker);
        }

        protected internal sealed override void UnpackToCore(Unpacker unpacker, T collection)
        {
            this._underlying.UnpackTo(unpacker, collection);
        }
    }
}

