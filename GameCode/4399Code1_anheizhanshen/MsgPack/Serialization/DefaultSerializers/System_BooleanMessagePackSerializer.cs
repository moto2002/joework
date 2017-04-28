namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    internal sealed class System_BooleanMessagePackSerializer : MessagePackSerializer<bool>
    {
        protected internal sealed override void PackToCore(Packer packer, bool value)
        {
            packer.Pack(value);
        }

        protected internal sealed override bool UnpackFromCore(Unpacker unpacker)
        {
            bool flag;
            try
            {
                flag = unpacker.Data.Value.AsBoolean();
            }
            catch (InvalidOperationException exception)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", new object[] { typeof(bool), exception.Message }));
            }
            return flag;
        }
    }
}

