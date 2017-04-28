namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    internal sealed class System_Int64MessagePackSerializer : MessagePackSerializer<long>
    {
        protected internal sealed override void PackToCore(Packer packer, long value)
        {
            packer.Pack(value);
        }

        protected internal sealed override long UnpackFromCore(Unpacker unpacker)
        {
            long num;
            try
            {
                num = unpacker.Data.Value.AsInt64();
            }
            catch (InvalidOperationException exception)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", new object[] { typeof(long), exception.Message }));
            }
            return num;
        }
    }
}

