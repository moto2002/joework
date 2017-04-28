namespace MsgPack
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;

    internal sealed class EnumerableStream : Stream
    {
        private bool _isDisposed;
        private int _position;
        private readonly IEnumerator<byte> _underlyingEnumerator;
        private readonly IList<byte> _underlyingList;

        public EnumerableStream(IEnumerable<byte> source)
        {
            Contract.Assert(source != null);
            this._underlyingList = source as IList<byte>;
            if (this._underlyingList == null)
            {
                this._underlyingEnumerator = source.GetEnumerator();
            }
            else
            {
                this._underlyingEnumerator = null;
            }
        }

        protected sealed override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing && (this._underlyingEnumerator != null))
            {
                this._underlyingEnumerator.Dispose();
            }
            this._isDisposed = true;
        }

        public sealed override void Flush()
        {
        }

        public sealed override int Read(byte[] buffer, int offset, int count)
        {
            this.VerifyIsNotDisposed();
            if (this._underlyingList != null)
            {
                int num = this._underlyingList.Count - this._position;
                if (num == 0)
                {
                    return 0;
                }
                int num2 = Math.Min(count, num);
                byte[] src = this._underlyingList as byte[];
                if (src != null)
                {
                    Buffer.BlockCopy(src, this._position, buffer, offset, num2);
                    return num2;
                }
                List<byte> list = this._underlyingList as List<byte>;
                if (list != null)
                {
                    list.CopyTo(this._position, buffer, offset, num2);
                    return num2;
                }
                int num3 = 0;
                foreach (byte num4 in this._underlyingList.Skip<byte>(this._position).Take<byte>(num2))
                {
                    buffer[num3 + offset] = num4;
                    num3++;
                }
                return num2;
            }
            int num5 = 0;
            while ((num5 < count) && this._underlyingEnumerator.MoveNext())
            {
                buffer[num5 + offset] = this._underlyingEnumerator.Current;
                num5++;
            }
            return num5;
        }

        public sealed override int ReadByte()
        {
            this.VerifyIsNotDisposed();
            if (this._underlyingList != null)
            {
                if (this._position < (this._underlyingList.Count - 1))
                {
                    this._position++;
                    return this._underlyingList[this._position];
                }
            }
            else if (this._underlyingEnumerator.MoveNext())
            {
                return this._underlyingEnumerator.Current;
            }
            return -1;
        }

        public sealed override long Seek(long offset, SeekOrigin origin)
        {
            this.VerifyCanSeek();
            switch (origin)
            {
                case SeekOrigin.Begin:
                    return (this.Position = offset);

                case SeekOrigin.Current:
                    return (this.Position += offset);

                case SeekOrigin.End:
                    return (this.Position = this.Length + offset);
            }
            throw new ArgumentOutOfRangeException("origin");
        }

        public sealed override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        private void VerifyCanSeek()
        {
            this.VerifyIsNotDisposed();
            if (!this.CanSeek)
            {
                throw new NotSupportedException();
            }
        }

        private void VerifyIsNotDisposed()
        {
            if (this._isDisposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
        }

        public sealed override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public sealed override bool CanRead
        {
            get
            {
                return !this._isDisposed;
            }
        }

        public sealed override bool CanSeek
        {
            get
            {
                return (!this._isDisposed && (this._underlyingList != null));
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
                this.VerifyCanSeek();
                return (long) this._underlyingList.Count;
            }
        }

        public override long Position
        {
            get
            {
                this.VerifyCanSeek();
                return (long) this._position;
            }
            set
            {
                this.VerifyCanSeek();
                if ((value < 0L) || (0x7fffffffL < value))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                if (this.Length <= value)
                {
                    this.SetLength(value);
                }
                this._position = (int) value;
            }
        }
    }
}

