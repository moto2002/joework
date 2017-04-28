namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;

    internal sealed class System_ByteArrayMessagePackSerializer : MessagePackSerializer<byte[]>
    {
        protected internal sealed override void PackToCore(Packer packer, byte[] value)
        {
            packer.PackRaw(value);
        }

        protected internal sealed override byte[] UnpackFromCore(Unpacker unpacker)
        {
            MessagePackObject obj2 = unpacker.Data.Value;
            return (obj2.IsNil ? null : obj2.AsBinary());
        }
    }
}

