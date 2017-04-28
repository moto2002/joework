namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    internal sealed class System_ByteMessagePackSerializer : MessagePackSerializer<byte>
    {
        protected internal sealed override void PackToCore(Packer packer, byte value)
        {
            packer.Pack(value);
        }

        protected internal sealed override byte UnpackFromCore(Unpacker unpacker)
        {
            byte num;
            try
            {
                num = unpacker.Data.Value.AsByte();
            }
            catch (InvalidOperationException exception)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", new object[] { typeof(byte), exception.Message }));
            }
            return num;
        }
    }
}

