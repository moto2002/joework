namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    internal sealed class System_Int32MessagePackSerializer : MessagePackSerializer<int>
    {
        protected internal sealed override void PackToCore(Packer packer, int value)
        {
            packer.Pack(value);
        }

        protected internal sealed override int UnpackFromCore(Unpacker unpacker)
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
            return num;
        }
    }
}

