namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    internal static class MessagePackObjectExtensions
    {
        public static string DeserializeAsString(this MessagePackObject source)
        {
            string str;
            try
            {
                str = source.AsString();
            }
            catch (InvalidOperationException exception)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", new object[] { exception.Message }), exception);
            }
            return str;
        }
    }
}

