namespace Mogo.RPC
{
    using System;

    public class VInt32 : VObject
    {
        private static VInt32 m_instance = new VInt32();

        public VInt32() : base(typeof(int), VType.V_INT32, Marshal.SizeOf(typeof(int)))
        {
        }

        public VInt32(object vValue) : base(typeof(int), VType.V_INT32, vValue)
        {
        }

        public override object Decode(byte[] data, ref int index)
        {
            byte[] dst = new byte[base.VTypeLength];
            Buffer.BlockCopy(data, index, dst, 0, base.VTypeLength);
            index += base.VTypeLength;
            return BitConverter.ToInt32(dst, 0);
        }

        public override byte[] Encode(object vValue)
        {
            return BitConverter.GetBytes(Convert.ToInt32(vValue));
        }

        public static VInt32 Instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}

