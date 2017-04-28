namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    internal sealed class System_DateTimeMessagePackSerializer : MessagePackSerializer<DateTime>
    {
        protected internal sealed override void PackToCore(Packer packer, DateTime value)
        {
            packer.Pack(MessagePackConvert.FromDateTime(value));
        }

        protected internal sealed override DateTime UnpackFromCore(Unpacker unpacker)
        {
            DateTime time;
            try
            {
                time = MessagePackConvert.ToDateTime(unpacker.Data.Value.AsInt64());
            }
            catch (ArgumentException exception)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", new object[] { exception.Message }), exception);
            }
            catch (InvalidOperationException exception2)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", new object[] { exception2.Message }), exception2);
            }
            return time;
        }
    }
}

