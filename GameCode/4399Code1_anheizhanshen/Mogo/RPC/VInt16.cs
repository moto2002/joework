namespace Mogo.RPC
{
    using System;

    public class VInt16 : VObject
    {
        private static VInt16 m_instance = new VInt16();

        public VInt16() : base(typeof(short), VType.V_INT16, Marshal.SizeOf(typeof(short)))
        {
        }

        public VInt16(object vValue) : base(typeof(short), VType.V_INT16, vValue)
        {
        }

        public override object Decode(byte[] data, ref int index)
        {
            byte[] dst = new byte[base.VTypeLength];
            Buffer.BlockCopy(data, index, dst, 0, base.VTypeLength);
            index += base.VTypeLength;
            return BitConverter.ToInt16(dst, 0);
        }

        public override byte[] Encode(object vValue)
        {
            return BitConverter.GetBytes(Convert.ToInt16(vValue));
        }

        public static VInt16 Instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}

