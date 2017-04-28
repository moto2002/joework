namespace Mogo.RPC
{
    using System;

    public abstract class VObject
    {
        private Mogo.RPC.VType m_vType;
        private int m_vTypeLength;
        private object m_vValue;
        private Type m_vValueType;

        private VObject(Type vValueType)
        {
            this.m_vValueType = vValueType;
        }

        private VObject(Type vValueType, Mogo.RPC.VType vType) : this(vValueType)
        {
            this.m_vType = vType;
        }

        protected VObject(Type vValueType, Mogo.RPC.VType vType, int vTypeLength) : this(vValueType, vType)
        {
            this.m_vTypeLength = vTypeLength;
        }

        protected VObject(Type vValueType, Mogo.RPC.VType vType, object vValue) : this(vValueType, vType)
        {
            this.m_vValue = vValue;
        }

        protected byte[] CutLengthHead(byte[] srcData, ref int index)
        {
            int count = (ushort) VUInt16.Instance.Decode(srcData, ref index);
            byte[] dst = new byte[count];
            Buffer.BlockCopy(srcData, index, dst, 0, count);
            index += count;
            return dst;
        }

        public abstract object Decode(byte[] data, ref int index);
        public abstract byte[] Encode(object vValue);

        public Mogo.RPC.VType VType
        {
            get
            {
                return this.m_vType;
            }
        }

        public int VTypeLength
        {
            get
            {
                return this.m_vTypeLength;
            }
        }

        public object VValue
        {
            get
            {
                return this.m_vValue;
            }
            set
            {
                this.m_vValue = value;
            }
        }

        public Type VValueType
        {
            get
            {
                return this.m_vValueType;
            }
        }
    }
}

