namespace MsgPack.Serialization.EmittingSerializers
{
    using MsgPack.Serialization;
    using MsgPack.Serialization.Reflection;
    using System;
    using System.Diagnostics.Contracts;

    internal abstract class SerializerEmitterContract : SerializerEmitter
    {
        protected SerializerEmitterContract()
        {
        }

        public override MessagePackSerializer<T> CreateInstance<T>(SerializationContext context)
        {
            Contract.Requires(context != null);
            return null;
        }

        public override TracingILGenerator GetPackToMethodILGenerator()
        {
            return null;
        }

        public override TracingILGenerator GetUnpackFromMethodILGenerator()
        {
            return null;
        }

        public override TracingILGenerator GetUnpackToMethodILGenerator()
        {
            return null;
        }

        public override Action<TracingILGenerator, int> RegisterSerializer(Type targetType)
        {
            Contract.Requires(targetType != null);
            throw new NotImplementedException();
        }
    }
}

