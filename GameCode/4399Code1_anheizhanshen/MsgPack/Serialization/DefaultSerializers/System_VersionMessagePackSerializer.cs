namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;

    internal sealed class System_VersionMessagePackSerializer : MessagePackSerializer<Version>
    {
        protected internal sealed override void PackToCore(Packer packer, Version objectTree)
        {
            packer.PackArrayHeader(4);
            packer.Pack(objectTree.Major);
            packer.Pack(objectTree.Minor);
            packer.Pack(objectTree.Build);
            packer.Pack(objectTree.Revision);
        }

        protected internal sealed override Version UnpackFromCore(Unpacker unpacker)
        {
            long num = unpacker.Data.Value.AsInt64();
            int[] numArray = new int[4];
            for (int i = 0; (i < num) && (i < 4); i++)
            {
                if (!unpacker.Read())
                {
                    throw SerializationExceptions.NewMissingItem(i);
                }
                numArray[i] = unpacker.Data.Value.AsInt32();
            }
            return new Version(numArray[0], numArray[1], numArray[2], numArray[3]);
        }
    }
}

