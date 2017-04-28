namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;

    internal sealed class System_UriMessagePackSerializer : MessagePackSerializer<Uri>
    {
        protected internal sealed override void PackToCore(Packer packer, Uri objectTree)
        {
            packer.PackString(objectTree.ToString());
        }

        protected internal sealed override Uri UnpackFromCore(Unpacker unpacker)
        {
            return new Uri(unpacker.Data.Value.DeserializeAsString());
        }
    }
}

