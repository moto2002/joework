namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    internal sealed class System_TimeSpanMessagePackSerializer : MessagePackSerializer<TimeSpan>
    {
        protected internal sealed override void PackToCore(Packer packer, TimeSpan value)
        {
            packer.Pack(value.Ticks);
        }

        protected internal sealed override TimeSpan UnpackFromCore(Unpacker unpacker)
        {
            long num;
            try
            {
                num = unpacker.Data.Value.AsInt64();
            }
            catch (InvalidOperationException exception)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", new object[] { typeof(long), exception.Message }));
            }
            return new TimeSpan(num);
        }
    }
}

