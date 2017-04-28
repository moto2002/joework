namespace MsgPack
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit)]
    internal struct Float32Bits
    {
        [FieldOffset(0)]
        public readonly byte Byte0;
        [FieldOffset(1)]
        public readonly byte Byte1;
        [FieldOffset(2)]
        public readonly byte Byte2;
        [FieldOffset(3)]
        public readonly byte Byte3;
        [FieldOffset(0)]
        public readonly float Value;

        public Float32Bits(float value)
        {
            this = new Float32Bits();
            this.Value = value;
        }

        public Float32Bits(byte[] bigEndianBytes, int offset)
        {
            Contract.Assume(bigEndianBytes != null);
            Contract.Assume((bigEndianBytes.Length - offset) >= 4, bigEndianBytes.Length.ToString() + "-" + offset.ToString() + ">= 4");
            this = new Float32Bits();
            if (BitConverter.IsLittleEndian)
            {
                this.Byte0 = bigEndianBytes[offset + 3];
                this.Byte1 = bigEndianBytes[offset + 2];
                this.Byte2 = bigEndianBytes[offset + 1];
                this.Byte3 = bigEndianBytes[offset];
            }
            else
            {
                this.Byte0 = bigEndianBytes[offset];
                this.Byte1 = bigEndianBytes[offset + 1];
                this.Byte2 = bigEndianBytes[offset + 2];
                this.Byte3 = bigEndianBytes[offset + 3];
            }
        }
    }
}

