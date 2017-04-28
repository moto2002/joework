namespace Mogo.RPC
{
    using System;

    public class VInt64 : VObject
    {
        private static VInt64 m_instance = new VInt64();

        public VInt64() : base(typeof(long), VType.V_INT64, Marshal.SizeOf(typeof(long)))
        {
        }

        public VInt64(object vValue) : base(typeof(long), VType.V_INT64, vValue)
        {
        }

        public override object Decode(byte[] data, ref int index)
        {
            byte[] dst = new byte[base.VTypeLength];
            Buffer.BlockCopy(data, index, dst, 0, base.VTypeLength);
            index += base.VTypeLength;
            return BitConverter.ToInt64(dst, 0);
        }

        public override byte[] Encode(object vValue)
        {
            return BitConverter.GetBytes(Convert.ToInt64(vValue));
        }

        public static VInt64 Instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}

