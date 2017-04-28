namespace MsgPack
{
    using System;
    using System.IO;

    public abstract class UnpackingStream : Stream
    {
        internal long CurrentOffset;
        internal readonly long RawLength;
        internal readonly Stream Underlying;

        internal UnpackingStream(Stream underlying, long rawLength)
        {
            this.Underlying = underlying;
            this.RawLength = rawLength;
        }

        public sealed override void Flush()
        {
        }

        public sealed override int Read(byte[] buffer, int offset, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if (this.CurrentOffset == this.RawLength)
            {
                return 0;
            }
            int num = count;
            long num2 = (this.CurrentOffset + count) - this.RawLength;
            if (num2 > 0L)
            {
                num -= (int) num2;
            }
            int num3 = this.Underlying.Read(buffer, offset, num);
            this.CurrentOffset += num3;
            return num3;
        }

        public sealed override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public sealed override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public sealed override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public sealed override bool CanTimeout
        {
            get
            {
                return this.Underlying.CanTimeout;
            }
        }

        public sealed override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public sealed override long Length
        {
            get
            {
                return this.RawLength;
            }
        }
    }
}

