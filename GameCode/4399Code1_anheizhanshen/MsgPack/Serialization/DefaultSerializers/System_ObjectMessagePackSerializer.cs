namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;

    internal sealed class System_ObjectMessagePackSerializer : MessagePackSerializer<object>
    {
        protected internal sealed override void PackToCore(Packer packer, object value)
        {
            packer.PackObject(value);
        }

        protected internal sealed override object UnpackFromCore(Unpacker unpacker)
        {
            MessagePackObject obj2 = unpacker.Data.Value;
            return (obj2.IsNil ? null : ((object) obj2));
        }
    }
}

