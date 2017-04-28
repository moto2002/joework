namespace Mogo.RPC
{
    using System;

    public class VUInt32 : VObject
    {
        private static VUInt32 m_instance = new VUInt32();

        public VUInt32() : base(typeof(uint), VType.V_UINT32, Marshal.SizeOf(typeof(uint)))
        {
        }

        public VUInt32(object vValue) : base(typeof(uint), VType.V_UINT32, vValue)
        {
        }

        public override object Decode(byte[] data, ref int index)
        {
            byte[] dst = new byte[base.VTypeLength];
            Buffer.BlockCopy(data, index, dst, 0, base.VTypeLength);
            index += base.VTypeLength;
            return BitConverter.ToUInt32(dst, 0);
        }

        public override byte[] Encode(object vValue)
        {
            return BitConverter.GetBytes(Convert.ToUInt32(vValue));
        }

        public static VUInt32 Instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}

