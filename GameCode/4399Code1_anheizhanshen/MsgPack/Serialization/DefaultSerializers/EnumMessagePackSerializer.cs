namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.Serialization;

    internal static class EnumMessagePackSerializer
    {
        public static readonly MethodInfo Unmarshal1Method = typeof(EnumMessagePackSerializer).GetMethod("Unmarshal");

        public static T Unmarshal<T>(Unpacker unpacker) where T: struct
        {
            T local;
            try
            {
                local = (T) Enum.Parse(typeof(T), unpacker.Data.Value.AsString(), false);
            }
            catch (ArgumentException)
            {
                throw new SerializationException(string.Format(CultureInfo.CurrentCulture, "'{0}' is not valid for enum type '{1}'.", new object[] { unpacker.Data.Value.AsString(), typeof(T) }));
            }
            return local;
        }
    }
}

