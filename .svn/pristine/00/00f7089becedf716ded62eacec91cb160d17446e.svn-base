namespace Mogo.RPC
{
    using System;

    public class VBoolean : VObject
    {
        private static VBoolean m_instance = new VBoolean();

        public VBoolean() : base(typeof(bool), VType.V_BLOB, 1)
        {
        }

        public VBoolean(object vValue) : base(typeof(bool), VType.V_BLOB, vValue)
        {
        }

        public override object Decode(byte[] data, ref int index)
        {
            byte[] dst = new byte[base.VTypeLength];
            Buffer.BlockCopy(data, index, dst, 0, base.VTypeLength);
            index += base.VTypeLength;
            return BitConverter.ToBoolean(dst, 0);
        }

        public override byte[] Encode(object vValue)
        {
            return BitConverter.GetBytes(Convert.ToBoolean(vValue));
        }

        public static VBoolean Instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}

