namespace Mogo.RPC
{
    using System;

    public class VUInt8 : VObject
    {
        private static VUInt8 m_instance = new VUInt8();

        public VUInt8() : base(typeof(byte), VType.V_UINT8, Marshal.SizeOf(typeof(byte)))
        {
        }

        public VUInt8(object vValue) : base(typeof(byte), VType.V_UINT8, vValue)
        {
        }

        public override object Decode(byte[] data, ref int index)
        {
            return data[index++];
        }

        public override byte[] Encode(object vValue)
        {
            return new byte[] { Convert.ToByte(vValue) };
        }

        public static VUInt8 Instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}

