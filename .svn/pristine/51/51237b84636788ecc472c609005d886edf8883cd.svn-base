namespace Mogo.Util
{
    using Mogo.RPC;
    using System;
    using System.Text;

    public class EncodeDecoder
    {
        private static int m_boolLength = 1;
        private static Encoding m_encoding = Encoding.UTF8;
        private static int m_uint16Length = Marshal.SizeOf(typeof(ushort));
        private static int m_uintLength = Marshal.SizeOf(typeof(uint));

        protected static byte[] CutLengthHead(byte[] srcData, ref int index)
        {
            int count = DecodeUInt16(srcData, ref index);
            byte[] dst = new byte[count];
            Buffer.BlockCopy(srcData, index, dst, 0, count);
            index += count;
            return dst;
        }

        public static bool DecodeBoolean(byte[] data, ref int index)
        {
            byte[] dst = new byte[m_boolLength];
            Buffer.BlockCopy(data, index, dst, 0, m_boolLength);
            index += m_boolLength;
            return BitConverter.ToBoolean(dst, 0);
        }

        public static string DecodeString(byte[] data, ref int index)
        {
            byte[] bytes = CutLengthHead(data, ref index);
            return m_encoding.GetString(bytes);
        }

        public static ushort DecodeUInt16(byte[] data, ref int index)
        {
            byte[] dst = new byte[m_uint16Length];
            Buffer.BlockCopy(data, index, dst, 0, m_uint16Length);
            index += m_uint16Length;
            return BitConverter.ToUInt16(dst, 0);
        }

        public static uint DecodeUInt32(byte[] data, ref int index)
        {
            byte[] dst = new byte[m_uintLength];
            Buffer.BlockCopy(data, index, dst, 0, m_uintLength);
            index += m_uintLength;
            return BitConverter.ToUInt32(dst, 0);
        }

        public static byte[] EncodeBoolean(bool vValue)
        {
            return BitConverter.GetBytes(vValue);
        }

        public static byte[] EncodeString(string vValue)
        {
            string str = vValue;
            System.Text.Encoder encoder = m_encoding.GetEncoder();
            char[] chars = str.ToCharArray();
            byte[] bytes = new byte[encoder.GetByteCount(chars, 0, chars.Length, false)];
            encoder.GetBytes(chars, 0, chars.Length, bytes, 0, true);
            return Mogo.RPC.Utils.FillLengthHead(bytes);
        }

        public static byte[] EncodeUInt32(uint vValue)
        {
            return BitConverter.GetBytes(vValue);
        }
    }
}

