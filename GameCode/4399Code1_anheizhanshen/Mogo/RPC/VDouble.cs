namespace Mogo.RPC
{
    using System;

    public class VDouble : VObject
    {
        private static VDouble m_instance = new VDouble();

        public VDouble() : base(typeof(double), VType.V_FLOAT64, Marshal.SizeOf(typeof(double)))
        {
        }

        public VDouble(object vValue) : base(typeof(double), VType.V_FLOAT64, vValue)
        {
        }

        public override object Decode(byte[] data, ref int index)
        {
            byte[] dst = new byte[base.VTypeLength];
            Buffer.BlockCopy(data, index, dst, 0, base.VTypeLength);
            index += base.VTypeLength;
            return BitConverter.ToDouble(dst, 0);
        }

        public override byte[] Encode(object vValue)
        {
            return BitConverter.GetBytes(Convert.ToDouble(vValue));
        }

        public static VDouble Instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}

