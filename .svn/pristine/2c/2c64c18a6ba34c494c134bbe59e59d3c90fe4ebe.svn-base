namespace Mogo.RPC
{
    using System;

    public class VInt8 : VObject
    {
        private static VInt8 m_instance = new VInt8();

        public VInt8() : base(typeof(sbyte), VType.V_INT8, Marshal.SizeOf(typeof(sbyte)))
        {
        }

        public VInt8(object vValue) : base(typeof(sbyte), VType.V_INT8, vValue)
        {
        }

        public override object Decode(byte[] data, ref int index)
        {
            byte[] dst = new byte[1];
            Buffer.BlockCopy(data, index, dst, 0, 1);
            index++;
            return Convert.ToSByte(dst[0]);
        }

        public override byte[] Encode(object vValue)
        {
            byte num = (byte) Convert.ToSByte(vValue);
            return new byte[] { num };
        }

        public static VInt8 Instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}

