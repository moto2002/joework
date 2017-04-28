namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    internal sealed class System_DoubleMessagePackSerializer : MessagePackSerializer<double>
    {
        protected internal sealed override void PackToCore(Packer packer, double value)
        {
            packer.Pack(value);
        }

        protected internal sealed override double UnpackFromCore(Unpacker unpacker)
        {
            double num;
            try
            {
                num = unpacker.Data.Value.AsDouble();
            }
            catch (InvalidOperationException exception)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", new object[] { typeof(double), exception.Message }));
            }
            return num;
        }
    }
}

