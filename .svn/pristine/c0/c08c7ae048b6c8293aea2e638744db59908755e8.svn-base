namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    internal sealed class System_GuidMessagePackSerializer : MessagePackSerializer<Guid>
    {
        protected internal sealed override void PackToCore(Packer packer, Guid value)
        {
            packer.PackRaw(value.ToByteArray());
        }

        protected internal sealed override Guid UnpackFromCore(Unpacker unpacker)
        {
            Guid guid;
            try
            {
                guid = new Guid(unpacker.Data.Value.AsBinary());
            }
            catch (ArgumentException exception)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", new object[] { exception.Message }), exception);
            }
            catch (InvalidOperationException exception2)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", new object[] { exception2.Message }), exception2);
            }
            return guid;
        }
    }
}

