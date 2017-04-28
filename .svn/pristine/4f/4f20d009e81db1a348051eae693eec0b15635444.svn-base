namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Runtime.InteropServices.ComTypes;

    internal sealed class System_Runtime_InteropServices_ComTypes_FILETIMEMessagePackSerializer : MessagePackSerializer<FILETIME>
    {
        protected internal sealed override void PackToCore(Packer packer, FILETIME value)
        {
            packer.Pack(MessagePackConvert.FromDateTime(DateTime.FromFileTimeUtc((value.dwHighDateTime << 0x20) | (value.dwLowDateTime & ((long) 0xffffffffL)))));
        }

        protected internal sealed override FILETIME UnpackFromCore(Unpacker unpacker)
        {
            long num = MessagePackConvert.ToDateTime(unpacker.Data.Value.AsInt64()).ToFileTimeUtc();
            return new FILETIME { dwHighDateTime = (int) (num >> 0x20), dwLowDateTime = (int) (((ulong) num) & 0xffffffffL) };
        }
    }
}

