namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    internal sealed class System_UInt32MessagePackSerializer : MessagePackSerializer<uint>
    {
        protected internal sealed override void PackToCore(Packer packer, uint value)
        {
            packer.Pack(value);
        }

        protected internal sealed override uint UnpackFromCore(Unpacker unpacker)
        {
            uint num;
            try
            {
                num = unpacker.Data.Value.AsUInt32();
            }
            catch (InvalidOperationException exception)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", new object[] { typeof(uint), exception.Message }));
            }
            return num;
        }
    }
}

