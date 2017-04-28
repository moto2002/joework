namespace Mogo.RPC
{
    using System;

    public class VEmpty : VObject
    {
        private static VEmpty m_instance = new VEmpty();

        public VEmpty() : base(typeof(object), VType.V_TYPE_ERR, 1)
        {
        }

        public VEmpty(object vValue) : base(typeof(object), VType.V_TYPE_ERR, vValue)
        {
        }

        public override object Decode(byte[] data, ref int index)
        {
            return new object();
        }

        public override byte[] Encode(object vValue)
        {
            return new byte[0];
        }

        public static VEmpty Instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}

