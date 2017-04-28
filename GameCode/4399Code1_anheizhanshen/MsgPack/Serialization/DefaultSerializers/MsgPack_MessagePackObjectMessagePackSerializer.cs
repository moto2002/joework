namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;

    internal sealed class MsgPack_MessagePackObjectMessagePackSerializer : MessagePackSerializer<MessagePackObject>
    {
        protected internal sealed override void PackToCore(Packer packer, MessagePackObject value)
        {
            value.PackToMessage(packer, new PackingOptions());
        }

        protected internal sealed override MessagePackObject UnpackFromCore(Unpacker unpacker)
        {
            return unpacker.Data.Value;
        }
    }
}

