namespace MsgPack
{
    using System;
    using System.Text;

    public static class MessagePackConvert
    {
        private static readonly DateTime _unixEpocUtc = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly Encoding _utf8NonBom = new UTF8Encoding(false, false);
        private static readonly Encoding _utf8NonBomStrict = new UTF8Encoding(false, true);

        public static string DecodeStringStrict(byte[] value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            return _utf8NonBomStrict.GetString(value, 0, value.Length);
        }

        public static byte[] EncodeString(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            return _utf8NonBom.GetBytes(value);
        }

        public static long FromDateTime(DateTime value)
        {
            return (long) value.ToUniversalTime().Subtract(_unixEpocUtc).TotalMilliseconds;
        }

        public static long FromDateTimeOffset(DateTimeOffset value)
        {
            return (long) value.ToUniversalTime().Subtract(_unixEpocUtc).TotalMilliseconds;
        }

        public static DateTime ToDateTime(long value)
        {
            return _unixEpocUtc.AddMilliseconds((double) value);
        }

        public static DateTimeOffset ToDateTimeOffset(long value)
        {
            return _unixEpocUtc.AddMilliseconds((double) value);
        }

        internal static Encoding Utf8NonBom
        {
            get
            {
                return _utf8NonBom;
            }
        }

        internal static Encoding Utf8NonBomStrict
        {
            get
            {
                return _utf8NonBomStrict;
            }
        }
    }
}

