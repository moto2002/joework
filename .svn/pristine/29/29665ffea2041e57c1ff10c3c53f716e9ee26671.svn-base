namespace MsgPack
{
    using System;
    using System.Diagnostics.Contracts;

    internal static class BigEndianBinary
    {
        public static byte ToByte(byte[] buffer, int offset)
        {
            Contract.Assume(buffer.Length >= (offset + 1));
            return buffer[offset];
        }

        public static double ToDouble(byte[] buffer, int offset)
        {
            return new Float64Bits(buffer, offset).Value;
        }

        public static short ToInt16(byte[] buffer, int offset)
        {
            Contract.Assume(buffer.Length >= (offset + 2));
            return (short) ((buffer[offset] << 8) | buffer[1 + offset]);
        }

        public static int ToInt32(byte[] buffer, int offset)
        {
            Contract.Assume(buffer.Length >= (offset + 4));
            return ((((buffer[offset] << 0x18) | (buffer[1 + offset] << 0x10)) | (buffer[2 + offset] << 8)) | buffer[3 + offset]);
        }

        public static long ToInt64(byte[] buffer, int offset)
        {
            Contract.Assume(buffer.Length >= (offset + 8));
            return (long) ((((((((buffer[offset] << 0x38) | (buffer[1 + offset] << 0x30)) | (buffer[2 + offset] << 40)) | (buffer[3 + offset] << 0x20)) | (buffer[4 + offset] << 0x18)) | (buffer[5 + offset] << 0x10)) | (buffer[6 + offset] << 8)) | buffer[7 + offset]);
        }

        public static sbyte ToSByte(byte[] buffer, int offset)
        {
            Contract.Assume(buffer.Length >= (offset + 1));
            return (sbyte) buffer[offset];
        }

        public static float ToSingle(byte[] buffer, int offset)
        {
            return new Float32Bits(buffer, offset).Value;
        }

        public static ushort ToUInt16(byte[] buffer, int offset)
        {
            Contract.Assume(buffer.Length >= (offset + 2));
            return (ushort) ((buffer[offset] << 8) | buffer[1 + offset]);
        }

        public static uint ToUInt32(byte[] buffer, int offset)
        {
            Contract.Assume(buffer.Length >= (offset + 4));
            return (uint) ((((buffer[offset] << 0x18) | (buffer[1 + offset] << 0x10)) | (buffer[2 + offset] << 8)) | buffer[3 + offset]);
        }

        public static ulong ToUInt64(byte[] buffer, int offset)
        {
            Contract.Assume(buffer.Length >= (offset + 8));
            return (ulong) ((((((((buffer[offset] << 0x38) | (buffer[1 + offset] << 0x30)) | (buffer[2 + offset] << 40)) | (buffer[3 + offset] << 0x20)) | (buffer[4 + offset] << 0x18)) | (buffer[5 + offset] << 0x10)) | (buffer[6 + offset] << 8)) | buffer[7 + offset]);
        }
    }
}

