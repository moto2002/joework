namespace MsgPack
{
    using System;
    using System.Text;

    internal static class Binary
    {
        public static readonly byte[] Empty = new byte[0];

        private static char ToHexChar(int b)
        {
            if (b < 10)
            {
                return (char) (0x30 + b);
            }
            return (char) (0x41 + (b - 10));
        }

        public static string ToHexString(byte[] blob)
        {
            if ((blob == null) || (blob.Length == 0))
            {
                return string.Empty;
            }
            StringBuilder buffer = new StringBuilder((blob.Length * 2) + 2);
            ToHexStringCore(blob, buffer);
            return buffer.ToString();
        }

        public static void ToHexString(byte[] blob, StringBuilder buffer)
        {
            if ((blob != null) && (blob.Length != 0))
            {
                ToHexStringCore(blob, buffer);
            }
        }

        private static void ToHexStringCore(byte[] blob, StringBuilder buffer)
        {
            buffer.Append("0x");
            foreach (byte num in blob)
            {
                buffer.Append(ToHexChar(num >> 4));
                buffer.Append(ToHexChar(num & 15));
            }
        }
    }
}

