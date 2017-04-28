namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    internal sealed class System_UInt16MessagePackSerializer : MessagePackSerializer<ushort>
    {
        protected internal sealed override void PackToCore(Packer packer, ushort value)
        {
            packer.Pack(value);
        }

        protected internal sealed override ushort UnpackFromCore(Unpacker unpacker)
        {
            ushort num;
            try
            {
                num = unpacker.Data.Value.AsUInt16();
            }
            catch (InvalidOperationException exception)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", new object[] { typeof(ushort), exception.Message }));
            }
            return num;
        }
    }
}

