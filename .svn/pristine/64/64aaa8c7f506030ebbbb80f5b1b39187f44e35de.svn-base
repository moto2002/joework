namespace HMF
{
    using System;
    using System.IO;
    using System.Text;

    internal class Util
    {
        public static int FromZigzag32(uint v)
        {
            return (int) ((v >> 1) ^ (v << 0x1f));
        }

        public static long FromZigzag64(ulong v)
        {
            return (long) ((v >> 1) ^ (v << 0x3f));
        }

        public static double ReadDouble(Stream stream)
        {
            byte[] buffer = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                buffer[i] = (byte) stream.ReadByte();
            }
            return BitConverter.ToDouble(buffer, 0);
        }

        public static string ReadStr(int len, Stream stream)
        {
            byte[] buffer = new byte[len];
            stream.Read(buffer, 0, len);
            return Encoding.UTF8.GetString(buffer);
        }

        public static int ReadVarint(Stream stream)
        {
            int num = 0;
            int num3 = stream.ReadByte() & 0xff;
            num = num3;
            if ((num & 0x80) != 0x80)
            {
                return num;
            }
            num3 = stream.ReadByte() & 0xff;
            num = (num & 0x7f) | (num3 << 7);
            if ((num & 0x4000) != 0x4000)
            {
                return num;
            }
            num3 = stream.ReadByte() & 0xff;
            num = (num & 0x3fff) | (num3 << 14);
            if ((num & 0x200000) != 0x200000)
            {
                return num;
            }
            num3 = stream.ReadByte() & 0xff;
            num = (num & 0x1fffff) | (num3 << 0x15);
            if ((num & 0x10000000) != 0x10000000)
            {
                return num;
            }
            num3 = stream.ReadByte() & 0xff;
            return ((num & 0xfffffff) | (num3 << 0x1c));
        }

        public static uint ToZigzag32(int v)
        {
            return (uint) ((v << 1) ^ (v >> 0x1f));
        }

        public static ulong ToZigzag64(long v)
        {
            return (ulong) ((v << 1) ^ (v >> 0x3f));
        }

        public static void WriteDobule(double v, Stream stream)
        {
            byte[] bytes = BitConverter.GetBytes(v);
            for (int i = 0; i < 8; i++)
            {
                stream.WriteByte(bytes[i]);
            }
        }

        public static void WriteStr(string v, Stream stream)
        {
            char[] chArray = v.ToCharArray();
            int length = chArray.Length;
            for (int i = 0; i < length; i++)
            {
                stream.WriteByte((byte) chArray[i]);
            }
        }

        public static void WriteVarint(int v, Stream stream)
        {
            int num = v & 0x7f;
            if ((v >> 7) == 0)
            {
                stream.WriteByte((byte) (num & 0x7f));
            }
            else
            {
                stream.WriteByte((byte) (num | 0x80));
                int num2 = v << 0x12;
                num2 = num2 >> 0x19;
                if ((v >> 14) == 0)
                {
                    stream.WriteByte((byte) (num2 & 0x7f));
                }
                else
                {
                    stream.WriteByte((byte) (num2 | 0x80));
                    int num3 = v << 11;
                    num3 = num3 >> 0x19;
                    if ((v >> 0x15) == 0)
                    {
                        stream.WriteByte((byte) (num3 & 0x7f));
                    }
                    else
                    {
                        stream.WriteByte((byte) (num3 | 0x80));
                        int num4 = v << 4;
                        num4 = num4 >> 0x19;
                        if ((v >> 0x1c) == 0)
                        {
                            stream.WriteByte((byte) (num4 & 0x7f));
                        }
                        else
                        {
                            stream.WriteByte((byte) (num4 | 0x80));
                            int num5 = (v >> 0x1c) & 15;
                            stream.WriteByte((byte) num5);
                        }
                    }
                }
            }
        }
    }
}

