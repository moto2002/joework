namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    internal sealed class System_UInt64MessagePackSerializer : MessagePackSerializer<ulong>
    {
        protected internal sealed override void PackToCore(Packer packer, ulong value)
        {
            packer.Pack(value);
        }

        protected internal sealed override ulong UnpackFromCore(Unpacker unpacker)
        {
            ulong num;
            try
            {
                num = unpacker.Data.Value.AsUInt64();
            }
            catch (InvalidOperationException exception)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", new object[] { typeof(ulong), exception.Message }));
            }
            return num;
        }
    }
}

