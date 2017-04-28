namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    internal sealed class System_DecimalMessagePackSerializer : MessagePackSerializer<decimal>
    {
        protected internal sealed override void PackToCore(Packer packer, decimal value)
        {
            packer.PackString(value.ToString("G", CultureInfo.InvariantCulture));
        }

        protected internal sealed override decimal UnpackFromCore(Unpacker unpacker)
        {
            decimal num;
            try
            {
                num = decimal.Parse(unpacker.Data.Value.AsString(), CultureInfo.InvariantCulture);
            }
            catch (ArgumentException exception)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", new object[] { exception.Message }), exception);
            }
            catch (InvalidOperationException exception2)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", new object[] { exception2.Message }), exception2);
            }
            return num;
        }
    }
}

