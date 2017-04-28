namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    internal sealed class System_DateTimeOffsetMessagePackSerializer : MessagePackSerializer<DateTimeOffset>
    {
        protected internal sealed override void PackToCore(Packer packer, DateTimeOffset value)
        {
            packer.Pack(MessagePackConvert.FromDateTimeOffset(value));
        }

        protected internal sealed override DateTimeOffset UnpackFromCore(Unpacker unpacker)
        {
            DateTimeOffset offset;
            try
            {
                offset = MessagePackConvert.ToDateTimeOffset(unpacker.Data.Value.AsInt64());
            }
            catch (ArgumentException exception)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", new object[] { exception.Message }), exception);
            }
            catch (InvalidOperationException exception2)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", new object[] { exception2.Message }), exception2);
            }
            return offset;
        }
    }
}

