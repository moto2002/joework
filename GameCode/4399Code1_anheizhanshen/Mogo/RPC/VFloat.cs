namespace Mogo.RPC
{
    using System;

    public class VFloat : VObject
    {
        private static VFloat m_instance = new VFloat();

        public VFloat() : base(typeof(float), VType.V_FLOAT32, Marshal.SizeOf(typeof(float)))
        {
        }

        public VFloat(object vValue) : base(typeof(float), VType.V_FLOAT32, vValue)
        {
        }

        public override object Decode(byte[] data, ref int index)
        {
            byte[] dst = new byte[base.VTypeLength];
            Buffer.BlockCopy(data, index, dst, 0, base.VTypeLength);
            index += base.VTypeLength;
            return BitConverter.ToSingle(dst, 0);
        }

        public override byte[] Encode(object vValue)
        {
            return BitConverter.GetBytes(Convert.ToSingle(vValue));
        }

        public static VFloat Instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}

