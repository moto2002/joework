namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    internal sealed class System_SingleMessagePackSerializer : MessagePackSerializer<float>
    {
        protected internal sealed override void PackToCore(Packer packer, float value)
        {
            packer.Pack(value);
        }

        protected internal sealed override float UnpackFromCore(Unpacker unpacker)
        {
            float num;
            try
            {
                num = unpacker.Data.Value.AsSingle();
            }
            catch (InvalidOperationException exception)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", new object[] { typeof(float), exception.Message }));
            }
            return num;
        }
    }
}

