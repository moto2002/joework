namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Runtime.Serialization;

    internal sealed class System_Collections_Specialized_BitVector32MessagePackSerializer : MessagePackSerializer<BitVector32>
    {
        protected internal sealed override void PackToCore(Packer packer, BitVector32 value)
        {
            packer.Pack(value.Data);
        }

        protected internal sealed override BitVector32 UnpackFromCore(Unpacker unpacker)
        {
            int num;
            try
            {
                num = unpacker.Data.Value.AsInt32();
            }
            catch (InvalidOperationException exception)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", new object[] { typeof(int), exception.Message }));
            }
            return new BitVector32(num);
        }
    }
}

