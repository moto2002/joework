namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;

    internal sealed class System_StringMessagePackSerializer : MessagePackSerializer<string>
    {
        protected internal sealed override void PackToCore(Packer packer, string value)
        {
            packer.PackString(value);
        }

        protected internal sealed override string UnpackFromCore(Unpacker unpacker)
        {
            MessagePackObject source = unpacker.Data.Value;
            return (source.IsNil ? null : source.DeserializeAsString());
        }
    }
}

