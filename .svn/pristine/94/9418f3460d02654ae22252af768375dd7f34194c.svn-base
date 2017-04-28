namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
    using System;

    public class StreamManipulator
    {
        private int bitsInBuffer_;
        private uint buffer_;
        private byte[] window_;
        private int windowEnd_;
        private int windowStart_;

        public int CopyBytes(byte[] output, int offset, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            if ((this.bitsInBuffer_ & 7) != 0)
            {
                throw new InvalidOperationException("Bit buffer is not byte aligned!");
            }
            int num = 0;
            while ((this.bitsInBuffer_ > 0) && (length > 0))
            {
                output[offset++] = (byte) this.buffer_;
                this.buffer_ = this.buffer_ >> 8;
                this.bitsInBuffer_ -= 8;
                length--;
                num++;
            }
            if (length == 0)
            {
                return num;
            }
            int num2 = this.windowEnd_ - this.windowStart_;
            if (length > num2)
            {
                length = num2;
            }
            Array.Copy(this.window_, this.windowStart_, output, offset, length);
            this.windowStart_ += length;
            if (((this.windowStart_ - this.windowEnd_) & 1) != 0)
            {
                this.buffer_ = (uint) (this.window_[this.windowStart_++] & 0xff);
                this.bitsInBuffer_ = 8;
            }
            return (num + length);
        }

        public void DropBits(int bitCount)
        {
            this.buffer_ = this.buffer_ >> bitCount;
            this.bitsInBuffer_ -= bitCount;
        }

        public int GetBits(int bitCount)
        {
            int num = this.PeekBits(bitCount);
            if (num >= 0)
            {
                this.DropBits(bitCount);
            }
            return num;
        }

        public int PeekBits(int bitCount)
        {
            if (this.bitsInBuffer_ < bitCount)
            {
                if (this.windowStart_ == this.windowEnd_)
                {
                    return -1;
                }
                this.buffer_ |= (uint) (((this.window_[this.windowStart_++] & 0xff) | ((this.window_[this.windowStart_++] & 0xff) << 8)) << this.bitsInBuffer_);
                this.bitsInBuffer_ += 0x10;
            }
            return (((int) this.buffer_) & ((((int) 1) << bitCount) - 1));
        }

        public void Reset()
        {
            this.buffer_ = 0;
            this.windowStart_ = this.windowEnd_ = this.bitsInBuffer_ = 0;
        }

        public void SetInput(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", "Cannot be negative");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", "Cannot be negative");
            }
            if (this.windowStart_ < this.windowEnd_)
            {
                throw new InvalidOperationException("Old input was not completely processed");
            }
            int num = offset + count;
            if ((offset > num) || (num > buffer.Length))
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if ((count & 1) != 0)
            {
                this.buffer_ |= (uint) ((buffer[offset++] & 0xff) << this.bitsInBuffer_);
                this.bitsInBuffer_ += 8;
            }
            this.window_ = buffer;
            this.windowStart_ = offset;
            this.windowEnd_ = num;
        }

        public void SkipToByteBoundary()
        {
            this.buffer_ = this.buffer_ >> (this.bitsInBuffer_ & 7);
            this.bitsInBuffer_ &= -8;
        }

        public int AvailableBits
        {
            get
            {
                return this.bitsInBuffer_;
            }
        }

        public int AvailableBytes
        {
            get
            {
                return ((this.windowEnd_ - this.windowStart_) + (this.bitsInBuffer_ >> 3));
            }
        }

        public bool IsNeedingInput
        {
            get
            {
                return (this.windowStart_ == this.windowEnd_);
            }
        }
    }
}

