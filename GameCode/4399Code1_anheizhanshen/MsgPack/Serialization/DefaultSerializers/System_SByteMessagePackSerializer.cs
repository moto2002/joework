namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    internal sealed class System_SByteMessagePackSerializer : MessagePackSerializer<sbyte>
    {
        protected internal sealed override void PackToCore(Packer packer, sbyte value)
        {
            packer.Pack(value);
        }

        protected internal sealed override sbyte UnpackFromCore(Unpacker unpacker)
        {
            sbyte num;
            try
            {
                num = unpacker.Data.Value.AsSByte();
            }
            catch (InvalidOperationException exception)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", new object[] { typeof(sbyte), exception.Message }));
            }
            return num;
        }
    }
}

