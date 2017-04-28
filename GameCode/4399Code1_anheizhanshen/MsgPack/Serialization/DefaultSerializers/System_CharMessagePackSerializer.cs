namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    internal sealed class System_CharMessagePackSerializer : MessagePackSerializer<char>
    {
        protected internal sealed override void PackToCore(Packer packer, char value)
        {
            packer.Pack((ushort) value);
        }

        protected internal sealed override char UnpackFromCore(Unpacker unpacker)
        {
            char ch;
            try
            {
                ch = (char) unpacker.Data.Value.AsUInt16();
            }
            catch (ArgumentException exception)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", new object[] { exception.Message }), exception);
            }
            catch (InvalidOperationException exception2)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", new object[] { exception2.Message }), exception2);
            }
            return ch;
        }
    }
}

