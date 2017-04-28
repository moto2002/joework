namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;

    internal sealed class System_CharArrayMessagePackSerializer : MessagePackSerializer<char[]>
    {
        protected internal sealed override void PackToCore(Packer packer, char[] value)
        {
            if (value == null)
            {
                packer.PackNull();
            }
            else
            {
                packer.PackString(value);
            }
        }

        protected internal sealed override char[] UnpackFromCore(Unpacker unpacker)
        {
            MessagePackObject obj2 = unpacker.Data.Value;
            return (obj2.IsNil ? null : obj2.AsCharArray());
        }
    }
}

