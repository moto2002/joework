namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Text;

    internal sealed class System_Text_StringBuilderMessagePackSerializer : MessagePackSerializer<StringBuilder>
    {
        protected internal sealed override void PackToCore(Packer packer, StringBuilder value)
        {
            packer.PackString(value.ToString());
        }

        protected internal sealed override StringBuilder UnpackFromCore(Unpacker unpacker)
        {
            MessagePackObject source = unpacker.Data.Value;
            return (source.IsNil ? null : new StringBuilder(source.DeserializeAsString()));
        }
    }
}

