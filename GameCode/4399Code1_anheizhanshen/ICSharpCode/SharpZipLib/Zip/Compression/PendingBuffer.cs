namespace ICSharpCode.SharpZipLib.Zip.Compression
{
    using System;

    public class PendingBuffer
    {
        private int bitCount;
        private uint bits;
        private byte[] buffer_;
        private int end;
        private int start;

        public PendingBuffer() : this(0x1000)
        {
        }

        public PendingBuffer(int bufferSize)
        {
            this.buffer_ = new byte[bufferSize];
        }

        public void AlignToByte()
        {
            if (this.bitCount > 0)
            {
                this.buffer_[this.end++] = (byte) this.bits;
                if (this.bitCount > 8)
                {
                    this.buffer_[this.end++] = (byte) (this.bits >> 8);
                }
            }
            this.bits = 0;
            this.bitCount = 0;
        }

        public int Flush(byte[] output, int offset, int length)
        {
            if (this.bitCount >= 8)
            {
                this.buffer_[this.end++] = (byte) this.bits;
                this.bits = this.bits >> 8;
                this.bitCount -= 8;
            }
            if (length > (this.end - this.start))
            {
                length = this.end - this.start;
                Array.Copy(this.buffer_, this.start, output, offset, length);
                this.start = 0;
                this.end = 0;
                return length;
            }
            Array.Copy(this.buffer_, this.start, output, offset, length);
            this.start += length;
            return length;
        }

        public void Reset()
        {
            this.start = this.end = this.bitCount = 0;
        }

        public byte[] ToByteArray()
        {
            byte[] destinationArray = new byte[this.end - this.start];
            Array.Copy(this.buffer_, this.start, destinationArray, 0, destinationArray.Length);
            this.start = 0;
            this.end = 0;
            return destinationArray;
        }

        public void WriteBits(int b, int count)
        {
            this.bits |= (uint) (b << this.bitCount);
            this.bitCount += count;
            if (this.bitCount >= 0x10)
            {
                this.buffer_[this.end++] = (byte) this.bits;
                this.buffer_[this.end++] = (byte) (this.bits >> 8);
                this.bits = this.bits >> 0x10;
                this.bitCount -= 0x10;
            }
        }

        public void WriteBlock(byte[] block, int offset, int length)
        {
            Array.Copy(block, offset, this.buffer_, this.end, length);
            this.end += length;
        }

        public void WriteByte(int value)
        {
            this.buffer_[this.end++] = (byte) value;
        }

        public void WriteInt(int value)
        {
            this.buffer_[this.end++] = (byte) value;
            this.buffer_[this.end++] = (byte) (value >> 8);
            this.buffer_[this.end++] = (byte) (value >> 0x10);
            this.buffer_[this.end++] = (byte) (value >> 0x18);
        }

        public void WriteShort(int value)
        {
            this.buffer_[this.end++] = (byte) value;
            this.buffer_[this.end++] = (byte) (value >> 8);
        }

        public void WriteShortMSB(int s)
        {
            this.buffer_[this.end++] = (byte) (s >> 8);
            this.buffer_[this.end++] = (byte) s;
        }

        public int BitCount
        {
            get
            {
                return this.bitCount;
            }
        }

        public bool IsFlushed
        {
            get
            {
                return (this.end == 0);
            }
        }
    }
}

