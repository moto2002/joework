namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    internal sealed class System_Int16MessagePackSerializer : MessagePackSerializer<short>
    {
        protected internal sealed override void PackToCore(Packer packer, short value)
        {
            packer.Pack(value);
        }

        protected internal sealed override short UnpackFromCore(Unpacker unpacker)
        {
            short num;
            try
            {
                num = unpacker.Data.Value.AsInt16();
            }
            catch (InvalidOperationException exception)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", new object[] { typeof(short), exception.Message }));
            }
            return num;
        }
    }
}

