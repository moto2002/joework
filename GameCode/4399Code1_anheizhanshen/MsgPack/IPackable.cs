namespace MsgPack
{
    using System;

    public interface IPackable
    {
        void PackToMessage(Packer packer, PackingOptions options);
    }
}

