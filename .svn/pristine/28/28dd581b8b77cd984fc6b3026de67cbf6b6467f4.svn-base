namespace MsgPack.Serialization
{
    using System;

    public interface IMessagePackSingleObjectSerializer : IMessagePackSerializer
    {
        byte[] PackSingleObject(object objectTree);
        object UnpackSingleObject(byte[] buffer);
    }
}

