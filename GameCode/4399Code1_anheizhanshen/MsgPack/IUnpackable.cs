namespace MsgPack
{
    using System;

    public interface IUnpackable
    {
        void UnpackFromMessage(Unpacker unpacker);
    }
}

