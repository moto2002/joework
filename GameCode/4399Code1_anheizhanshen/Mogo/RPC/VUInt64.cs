namespace Mogo.RPC
{
    using System;

    public class VUInt64 : VObject
    {
        private static VUInt64 m_instance = new VUInt64();

        public VUInt64() : base(typeof(ulong), VType.V_UINT64, Marshal.SizeOf(typeof(ulong)))
        {
        }

        public VUInt64(object vValue) : base(typeof(ulong), VType.V_UINT64, vValue)
        {
        }

        public override object Decode(byte[] data, ref int index)
        {
            byte[] dst = new byte[base.VTypeLength];
            Buffer.BlockCopy(data, index, dst, 0, base.VTypeLength);
            index += base.VTypeLength;
            return BitConverter.ToUInt64(dst, 0);
        }

        public override byte[] Encode(object vValue)
        {
            return BitConverter.GetBytes(Convert.ToUInt64(vValue));
        }

        public static VUInt64 Instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}

