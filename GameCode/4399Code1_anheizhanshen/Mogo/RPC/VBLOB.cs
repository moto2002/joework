namespace Mogo.RPC
{
    using System;

    public class VBLOB : VObject
    {
        private static VBLOB m_instance = new VBLOB();

        public VBLOB() : base(typeof(char[]), VType.V_BLOB, 1)
        {
        }

        public VBLOB(object vValue) : base(typeof(char[]), VType.V_BLOB, vValue)
        {
        }

        public override object Decode(byte[] data, ref int index)
        {
            byte[] dst = new byte[2];
            Buffer.BlockCopy(data, index, dst, 0, 2);
            index += 2;
            int count = BitConverter.ToUInt16(dst, 0);
            byte[] buffer2 = new byte[count];
            Buffer.BlockCopy(data, index, buffer2, 0, count);
            index += count;
            return buffer2;
        }

        public override byte[] Encode(object vValue)
        {
            return BitConverter.GetBytes((bool) vValue);
        }

        public static VBLOB Instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}

