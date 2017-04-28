namespace MsgPack.Serialization.EmittingSerializers
{
    using MsgPack.Serialization;
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection.Emit;

    internal abstract class SerializationMethodGeneratorManager
    {
        internal static SerializationMethodGeneratorOption DefaultSerializationMethodGeneratorOption = SerializationMethodGeneratorOption.Fast;

        protected SerializationMethodGeneratorManager()
        {
        }

        public SerializerEmitter CreateEmitter(Type targetType, EmitterFlavor emitterFlavor)
        {
            Contract.Requires(targetType != null);
            Contract.Requires((emitterFlavor == EmitterFlavor.FieldBased) || (emitterFlavor == EmitterFlavor.ContextBased));
            return this.CreateEmitterCore(targetType, emitterFlavor);
        }

        protected abstract SerializerEmitter CreateEmitterCore(Type targetType, EmitterFlavor emitterFlavor);
        public static SerializationMethodGeneratorManager Get()
        {
            return Get(DefaultSerializationMethodGeneratorOption);
        }

        public static SerializationMethodGeneratorManager Get(SerializationMethodGeneratorOption option)
        {
            switch (option)
            {
                case SerializationMethodGeneratorOption.CanDump:
                    return DefaultSerializationMethodGeneratorManager.CanDump;

                case SerializationMethodGeneratorOption.CanCollect:
                    return DefaultSerializationMethodGeneratorManager.CanCollect;
            }
            return DefaultSerializationMethodGeneratorManager.Fast;
        }

        public static SerializationMethodGeneratorManager Get(AssemblyBuilder assemblyBuilder)
        {
            return DefaultSerializationMethodGeneratorManager.Create(assemblyBuilder);
        }
    }
}

