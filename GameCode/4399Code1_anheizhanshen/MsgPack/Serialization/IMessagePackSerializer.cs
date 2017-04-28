namespace MsgPack.Serialization
{
    using MsgPack;
    using System;

    public interface IMessagePackSerializer
    {
        void PackTo(Packer packer, object objectTree);
        object UnpackFrom(Unpacker unpacker);
        void UnpackTo(Unpacker unpacker, object collection);
    }
}

