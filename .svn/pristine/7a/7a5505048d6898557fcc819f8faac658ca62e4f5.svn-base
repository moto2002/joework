namespace MsgPack.Serialization
{
    using System;
    using System.Diagnostics.Contracts;

    internal abstract class SerializerBuilderContract<T> : SerializerBuilder<T>
    {
        protected SerializerBuilderContract() : base(null)
        {
        }

        public override MessagePackSerializer<T> CreateArraySerializer()
        {
            return null;
        }

        public override MessagePackSerializer<T> CreateMapSerializer()
        {
            return null;
        }

        protected override MessagePackSerializer<T> CreateSerializer(SerializingMember[] entries)
        {
            Contract.Requires(entries != null);
            return null;
        }

        public override MessagePackSerializer<T> CreateTupleSerializer()
        {
            return null;
        }
    }
}

