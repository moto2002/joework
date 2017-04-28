namespace MsgPack
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit)]
    internal struct Float64Bits
    {
        [FieldOffset(0)]
        public readonly byte Byte0;
        [FieldOffset(1)]
        public readonly byte Byte1;
        [FieldOffset(2)]
        public readonly byte Byte2;
        [FieldOffset(3)]
        public readonly byte Byte3;
        [FieldOffset(4)]
        public readonly byte Byte4;
        [FieldOffset(5)]
        public readonly byte Byte5;
        [FieldOffset(6)]
        public readonly byte Byte6;
        [FieldOffset(7)]
        public readonly byte Byte7;
        [FieldOffset(0)]
        public readonly double Value;

        public Float64Bits(double value)
        {
            this = new Float64Bits();
            this.Value = value;
        }

        public Float64Bits(byte[] bigEndianBytes, int offset)
        {
            Contract.Assume(bigEndianBytes != null);
            Contract.Assume((bigEndianBytes.Length - offset) >= 8, bigEndianBytes.Length.ToString() + "-" + offset.ToString() + ">= 4");
            this = new Float64Bits();
            if (BitConverter.IsLittleEndian)
            {
                this.Byte0 = bigEndianBytes[offset + 7];
                this.Byte1 = bigEndianBytes[offset + 6];
                this.Byte2 = bigEndianBytes[offset + 5];
                this.Byte3 = bigEndianBytes[offset + 4];
                this.Byte4 = bigEndianBytes[offset + 3];
                this.Byte5 = bigEndianBytes[offset + 2];
                this.Byte6 = bigEndianBytes[offset + 1];
                this.Byte7 = bigEndianBytes[offset];
            }
            else
            {
                this.Byte0 = bigEndianBytes[offset];
                this.Byte1 = bigEndianBytes[offset + 1];
                this.Byte2 = bigEndianBytes[offset + 2];
                this.Byte3 = bigEndianBytes[offset + 3];
                this.Byte4 = bigEndianBytes[offset + 4];
                this.Byte5 = bigEndianBytes[offset + 5];
                this.Byte6 = bigEndianBytes[offset + 6];
                this.Byte7 = bigEndianBytes[offset + 7];
            }
        }
    }
}

