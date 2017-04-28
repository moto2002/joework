namespace Mogo.RPC
{
    using System;

    public class VUInt16 : VObject
    {
        private static VUInt16 m_instance = new VUInt16();

        public VUInt16() : base(typeof(ushort), VType.V_UINT16, Marshal.SizeOf(typeof(ushort)))
        {
        }

        public VUInt16(object vValue) : base(typeof(ushort), VType.V_UINT16, vValue)
        {
        }

        public override object Decode(byte[] data, ref int index)
        {
            byte[] dst = new byte[base.VTypeLength];
            Buffer.BlockCopy(data, index, dst, 0, base.VTypeLength);
            index += base.VTypeLength;
            return BitConverter.ToUInt16(dst, 0);
        }

        public override byte[] Encode(object vValue)
        {
            return BitConverter.GetBytes(Convert.ToUInt16(vValue));
        }

        public static VUInt16 Instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}

