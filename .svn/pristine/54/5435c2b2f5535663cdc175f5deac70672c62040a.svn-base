namespace Mogo.RPC
{
    using System;
    using System.Text;

    public class VString : VObject
    {
        private static Encoding m_encoding = Encoding.UTF8;
        private static VString m_instance = new VString();

        public VString() : base(typeof(string), VType.V_STR, 0)
        {
        }

        public VString(object vValue) : base(typeof(string), VType.V_STR, vValue)
        {
        }

        public override object Decode(byte[] data, ref int index)
        {
            byte[] bytes = base.CutLengthHead(data, ref index);
            return m_encoding.GetString(bytes);
        }

        public override byte[] Encode(object vValue)
        {
            string str = (string) vValue;
            Encoder encoder = m_encoding.GetEncoder();
            char[] chars = str.ToCharArray();
            byte[] bytes = new byte[encoder.GetByteCount(chars, 0, chars.Length, false)];
            encoder.GetBytes(chars, 0, chars.Length, bytes, 0, true);
            return Utils.FillLengthHead(bytes);
        }

        public static VString Instance
        {
            get
            {
                return m_instance;
            }
        }
    }
}

