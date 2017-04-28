namespace MsgPack
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    public abstract class Packer : IDisposable
    {
        private bool _isDisposed;

        protected Packer()
        {
        }

        public static Packer Create(Stream stream)
        {
            return Create(stream, true);
        }

        public static Packer Create(Stream stream, bool ownsStream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            return new StreamPacker(stream, ownsStream);
        }

        public void Dispose()
        {
            if (!this._isDisposed)
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
                this._isDisposed = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public Packer Pack(bool value)
        {
            this.VerifyNotDisposed();
            this.PrivatePackCore(value);
            return this;
        }

        public Packer Pack(byte value)
        {
            this.VerifyNotDisposed();
            this.PrivatePackCore(value);
            return this;
        }

        public Packer Pack(double value)
        {
            this.VerifyNotDisposed();
            this.PrivatePackCore(value);
            return this;
        }

        public Packer Pack(short value)
        {
            this.VerifyNotDisposed();
            this.PrivatePackCore(value);
            return this;
        }

        public Packer Pack(int value)
        {
            this.VerifyNotDisposed();
            this.PrivatePackCore(value);
            return this;
        }

        public Packer Pack(long value)
        {
            this.VerifyNotDisposed();
            this.PrivatePackCore(value);
            return this;
        }

        public Packer Pack(bool? value)
        {
            return (value.HasValue ? this.Pack(value.Value) : this.PackNull());
        }

        public Packer Pack(byte? value)
        {
            return (value.HasValue ? this.Pack(value.Value) : this.PackNull());
        }

        public Packer Pack(double? value)
        {
            return (value.HasValue ? this.Pack(value.Value) : this.PackNull());
        }

        public Packer Pack(short? value)
        {
            return (value.HasValue ? this.Pack(value.Value) : this.PackNull());
        }

        public Packer Pack(int? value)
        {
            return (value.HasValue ? this.Pack(value.Value) : this.PackNull());
        }

        public Packer Pack(long? value)
        {
            return (value.HasValue ? this.Pack(value.Value) : this.PackNull());
        }

        [CLSCompliant(false)]
        public Packer Pack(sbyte? value)
        {
            return (value.HasValue ? this.Pack(value.Value) : this.PackNull());
        }

        public Packer Pack(float? value)
        {
            return (value.HasValue ? this.Pack(value.Value) : this.PackNull());
        }

        [CLSCompliant(false)]
        public Packer Pack(ushort? value)
        {
            return (value.HasValue ? this.Pack(value.Value) : this.PackNull());
        }

        [CLSCompliant(false)]
        public Packer Pack(uint? value)
        {
            return (value.HasValue ? this.Pack(value.Value) : this.PackNull());
        }

        [CLSCompliant(false)]
        public Packer Pack(ulong? value)
        {
            return (value.HasValue ? this.Pack(value.Value) : this.PackNull());
        }

        [CLSCompliant(false)]
        public Packer Pack(sbyte value)
        {
            this.VerifyNotDisposed();
            this.PrivatePackCore(value);
            return this;
        }

        public Packer Pack(float value)
        {
            this.VerifyNotDisposed();
            this.PrivatePackCore(value);
            return this;
        }

        [CLSCompliant(false)]
        public Packer Pack(ushort value)
        {
            this.VerifyNotDisposed();
            this.PrivatePackCore(value);
            return this;
        }

        [CLSCompliant(false)]
        public Packer Pack(uint value)
        {
            this.VerifyNotDisposed();
            this.PrivatePackCore(value);
            return this;
        }

        [CLSCompliant(false)]
        public Packer Pack(ulong value)
        {
            this.VerifyNotDisposed();
            this.PrivatePackCore(value);
            return this;
        }

        public Packer PackArrayHeader<TItem>(IList<TItem> array)
        {
            if (array == null)
            {
                return this.PackNull();
            }
            return this.PackArrayHeader(array.Count);
        }

        public Packer PackArrayHeader(int count)
        {
            this.PackArrayHeaderCore(count);
            return this;
        }

        protected void PackArrayHeaderCore(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", string.Format(CultureInfo.CurrentCulture, "'{0}' is negative.", new object[] { "count" }));
            }
            this.VerifyNotDisposed();
            this.PrivatePackArrayHeaderCore(count);
        }

        public Packer PackMapHeader<TKey, TValue>(IDictionary<TKey, TValue> map)
        {
            if (map == null)
            {
                return this.PackNull();
            }
            return this.PackMapHeader(map.Count);
        }

        public Packer PackMapHeader(int count)
        {
            this.PackMapHeaderCore(count);
            return this;
        }

        protected void PackMapHeaderCore(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", string.Format(CultureInfo.CurrentCulture, "'{0}' is negative.", new object[] { "count" }));
            }
            this.VerifyNotDisposed();
            this.PrivatePackMapHeaderCore(count);
        }

        public Packer PackNull()
        {
            this.VerifyNotDisposed();
            this.PrivatePackNullCore();
            return this;
        }

        public Packer PackRaw(IEnumerable<byte> value)
        {
            this.VerifyNotDisposed();
            this.PrivatePackRaw(value);
            return this;
        }

        public Packer PackRaw(IList<byte> value)
        {
            this.VerifyNotDisposed();
            byte[] buffer = value as byte[];
            if (buffer == null)
            {
                this.PrivatePackRaw(value);
            }
            else
            {
                this.PrivatePackRaw(buffer);
            }
            return this;
        }

        public Packer PackRaw(byte[] value)
        {
            this.VerifyNotDisposed();
            this.PrivatePackRaw(value);
            return this;
        }

        public Packer PackRawBody(IEnumerable<byte> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            this.VerifyNotDisposed();
            this.PrivatePackRawBodyCore(value);
            return this;
        }

        public Packer PackRawBody(byte[] value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            this.VerifyNotDisposed();
            this.WriteBytes(value, false);
            return this;
        }

        public Packer PackRawHeader(int length)
        {
            this.PackRawHeaderCore(length);
            return this;
        }

        protected void PackRawHeaderCore(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", string.Format(CultureInfo.CurrentCulture, "'{0}' is negative.", new object[] { "length" }));
            }
            this.VerifyNotDisposed();
            this.PrivatePackRawHeaderCore(length);
        }

        public Packer PackString(IEnumerable<char> value)
        {
            this.PackStringCore(value, Encoding.UTF8);
            return this;
        }

        public Packer PackString(string value)
        {
            this.PackStringCore(value, Encoding.UTF8);
            return this;
        }

        public Packer PackString(IEnumerable<char> value, Encoding encoding)
        {
            this.PackStringCore(value, encoding);
            return this;
        }

        public Packer PackString(string value, Encoding encoding)
        {
            this.PackStringCore(value, encoding);
            return this;
        }

        protected virtual void PackStringCore(IEnumerable<char> value, Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            this.VerifyNotDisposed();
            this.PrivatePackString(value, encoding);
        }

        protected virtual void PackStringCore(string value, Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            this.VerifyNotDisposed();
            this.PrivatePackString(value, encoding);
        }

        private void PrivatePackArrayHeaderCore(int count)
        {
            Contract.Assert(0 <= count);
            if (count < 0x10)
            {
                this.WriteByte((byte) (0x90 | count));
            }
            else if (count <= 0xffff)
            {
                this.WriteByte(220);
                this.WriteByte((byte) ((count >> 8) & 0xff));
                this.WriteByte((byte) (count & 0xff));
            }
            else
            {
                this.WriteByte(0xdd);
                this.WriteByte((byte) ((count >> 0x18) & 0xff));
                this.WriteByte((byte) ((count >> 0x10) & 0xff));
                this.WriteByte((byte) ((count >> 8) & 0xff));
                this.WriteByte((byte) (count & 0xff));
            }
        }

        private void PrivatePackCore(bool value)
        {
            this.WriteByte(value ? ((byte) 0xc3) : ((byte) 0xc2));
        }

        private void PrivatePackCore(byte value)
        {
            if (!this.TryPackTinyUnsignedInteger((ulong) value))
            {
                Contract.Assume(this.TryPackUInt8((ulong) value));
            }
        }

        private void PrivatePackCore(double value)
        {
            this.WriteByte(0xcb);
            long num = BitConverter.DoubleToInt64Bits(value);
            this.WriteByte((byte) ((num >> 0x38) & 0xffL));
            this.WriteByte((byte) ((num >> 0x30) & 0xffL));
            this.WriteByte((byte) ((num >> 40) & 0xffL));
            this.WriteByte((byte) ((num >> 0x20) & 0xffL));
            this.WriteByte((byte) ((num >> 0x18) & 0xffL));
            this.WriteByte((byte) ((num >> 0x10) & 0xffL));
            this.WriteByte((byte) ((num >> 8) & 0xffL));
            this.WriteByte((byte) (num & 0xffL));
        }

        private void PrivatePackCore(short value)
        {
            if (!this.TryPackTinySignedInteger((long) value) && !this.TryPackInt8((long) value))
            {
                Contract.Assume(this.TryPackInt16((long) value));
            }
        }

        private void PrivatePackCore(int value)
        {
            if ((!this.TryPackTinySignedInteger((long) value) && !this.TryPackInt8((long) value)) && !this.TryPackInt16((long) value))
            {
                Contract.Assume(this.TryPackInt32((long) value));
            }
        }

        private void PrivatePackCore(long value)
        {
            if ((!this.TryPackTinySignedInteger(value) && !this.TryPackInt8(value)) && (!this.TryPackInt16(value) && !this.TryPackInt32(value)))
            {
                Contract.Assume(this.TryPackInt64(value));
            }
        }

        private void PrivatePackCore(sbyte value)
        {
            if (!this.TryPackTinySignedInteger((long) value))
            {
                Contract.Assume(this.TryPackInt8((long) value));
            }
        }

        private void PrivatePackCore(float value)
        {
            this.WriteByte(0xca);
            Float32Bits bits = new Float32Bits(value);
            if (BitConverter.IsLittleEndian)
            {
                this.WriteByte(bits.Byte3);
                this.WriteByte(bits.Byte2);
                this.WriteByte(bits.Byte1);
                this.WriteByte(bits.Byte0);
            }
            else
            {
                this.WriteByte(bits.Byte0);
                this.WriteByte(bits.Byte1);
                this.WriteByte(bits.Byte2);
                this.WriteByte(bits.Byte3);
            }
        }

        private void PrivatePackCore(ushort value)
        {
            if (!this.TryPackTinyUnsignedInteger((ulong) value) && !this.TryPackUInt8((ulong) value))
            {
                Contract.Assume(this.TryPackUInt16((ulong) value));
            }
        }

        private void PrivatePackCore(uint value)
        {
            if ((!this.TryPackTinyUnsignedInteger((ulong) value) && !this.TryPackUInt8((ulong) value)) && !this.TryPackUInt16((ulong) value))
            {
                Contract.Assume(this.TryPackUInt32((ulong) value));
            }
        }

        private void PrivatePackCore(ulong value)
        {
            if ((!this.TryPackTinyUnsignedInteger(value) && !this.TryPackUInt8(value)) && (!this.TryPackUInt16(value) && !this.TryPackUInt32(value)))
            {
                Contract.Assume(this.TryPackUInt64(value));
            }
        }

        private void PrivatePackMapHeaderCore(int count)
        {
            Contract.Assert(0 <= count);
            if (count < 0x10)
            {
                this.WriteByte((byte) (0x80 | count));
            }
            else if (count <= 0xffff)
            {
                this.WriteByte(0xde);
                this.WriteByte((byte) ((count >> 8) & 0xff));
                this.WriteByte((byte) (count & 0xff));
            }
            else
            {
                this.WriteByte(0xdf);
                this.WriteByte((byte) ((count >> 0x18) & 0xff));
                this.WriteByte((byte) ((count >> 0x10) & 0xff));
                this.WriteByte((byte) ((count >> 8) & 0xff));
                this.WriteByte((byte) (count & 0xff));
            }
        }

        private void PrivatePackNullCore()
        {
            this.WriteByte(0xc0);
        }

        private void PrivatePackRaw(IEnumerable<byte> value)
        {
            if (value == null)
            {
                this.PrivatePackNullCore();
            }
            else
            {
                this.PrivatePackRawCore(value);
            }
        }

        private void PrivatePackRaw(IList<byte> value)
        {
            if (value == null)
            {
                this.PrivatePackNullCore();
            }
            else
            {
                this.PrivatePackRawHeaderCore(value.Count);
                this.WriteBytes(value);
            }
        }

        private void PrivatePackRaw(byte[] value)
        {
            if (value == null)
            {
                this.PrivatePackNullCore();
            }
            else
            {
                this.PrivatePackRawCore(value, false);
            }
        }

        private int PrivatePackRawBodyCore(IEnumerable<byte> value)
        {
            Contract.Assert(value != null);
            ICollection<byte> is2 = value as ICollection<byte>;
            if (is2 != null)
            {
                return this.PrivatePackRawBodyCore(is2, is2.IsReadOnly);
            }
            int num = 0;
            foreach (byte num2 in value)
            {
                this.WriteByte(num2);
                num++;
            }
            return num;
        }

        private int PrivatePackRawBodyCore(ICollection<byte> value, bool isImmutable)
        {
            Contract.Assert(value != null);
            byte[] buffer = value as byte[];
            if (buffer != null)
            {
                this.WriteBytes(buffer, isImmutable);
            }
            else
            {
                this.WriteBytes(value);
            }
            return value.Count;
        }

        private void PrivatePackRawCore(IEnumerable<byte> value)
        {
            Action<IEnumerable<byte>, PackingOptions> writeBody = null;
            if (!this.CanSeek)
            {
                this.PrivatePackRawCore(value.ToArray<byte>(), true);
            }
            else
            {
                this.WriteByte(0xdb);
                if (writeBody == null)
                {
                    writeBody = (Action<IEnumerable<byte>, PackingOptions>) ((items, _) => this.PrivatePackRawBodyCore(items));
                }
                this.StreamWrite<byte>(value, writeBody, null);
            }
        }

        private void PrivatePackRawCore(byte[] value, bool isImmutable)
        {
            this.PrivatePackRawHeaderCore(value.Length);
            this.WriteBytes(value, isImmutable);
        }

        private void PrivatePackRawHeaderCore(int length)
        {
            Contract.Assert(0 <= length);
            if (length < 0x20)
            {
                this.WriteByte((byte) (160 | length));
            }
            else if (length <= 0xffff)
            {
                this.WriteByte(0xda);
                this.WriteByte((byte) ((length >> 8) & 0xff));
                this.WriteByte((byte) (length & 0xff));
            }
            else
            {
                this.WriteByte(0xdb);
                this.WriteByte((byte) ((length >> 0x18) & 0xff));
                this.WriteByte((byte) ((length >> 0x10) & 0xff));
                this.WriteByte((byte) ((length >> 8) & 0xff));
                this.WriteByte((byte) (length & 0xff));
            }
        }

        private void PrivatePackString(IEnumerable<char> value, Encoding encoding)
        {
            Contract.Assert(encoding != null);
            if (value == null)
            {
                this.PrivatePackNullCore();
            }
            else
            {
                this.PrivatePackStringCore(value, encoding);
            }
        }

        private void PrivatePackString(string value, Encoding encoding)
        {
            Contract.Assert(encoding != null);
            if (value == null)
            {
                this.PrivatePackNullCore();
            }
            else
            {
                this.PrivatePackStringCore(value, encoding);
            }
        }

        private void PrivatePackStringCore(IEnumerable<char> value, Encoding encoding)
        {
            Contract.Assert(value != null);
            Contract.Assert(encoding != null);
            byte[] bytes = encoding.GetBytes(value.ToArray<char>());
            this.PrivatePackRawHeaderCore(bytes.Length);
            this.WriteBytes(bytes, true);
        }

        private void PrivatePackStringCore(string value, Encoding encoding)
        {
            Contract.Assert(value != null);
            Contract.Assert(encoding != null);
            byte[] bytes = encoding.GetBytes(value);
            this.PrivatePackRawHeaderCore(bytes.Length);
            this.WriteBytes(bytes, true);
        }

        protected virtual void SeekTo(long offset)
        {
            throw new NotSupportedException();
        }

        private void StreamWrite<TItem>(IEnumerable<TItem> value, Action<IEnumerable<TItem>, PackingOptions> writeBody, PackingOptions options)
        {
            if (this.CanSeek)
            {
                this.SeekTo(4L);
                long position = this.Position;
                writeBody(value, options);
                long offset = this.Position - position;
                this.SeekTo(-offset);
                this.SeekTo(-4L);
                this.WriteByte((byte) ((offset >> 0x18) & 0xffL));
                this.WriteByte((byte) ((offset >> 0x10) & 0xffL));
                this.WriteByte((byte) ((offset >> 8) & 0xffL));
                this.WriteByte((byte) (offset & 0xffL));
                this.SeekTo(offset);
            }
            else
            {
                ICollection<TItem> is2 = value as ICollection<TItem>;
                if (is2 == null)
                {
                    is2 = value.ToArray<TItem>();
                }
                int count = is2.Count;
                this.WriteByte((byte) ((count >> 0x18) & 0xff));
                this.WriteByte((byte) ((count >> 0x10) & 0xff));
                this.WriteByte((byte) ((count >> 8) & 0xff));
                this.WriteByte((byte) (count & 0xff));
                writeBody(is2, options);
            }
        }

        protected bool TryPackInt16(long value)
        {
            if ((value < -32768L) || (value > 0x7fffL))
            {
                return false;
            }
            this.WriteByte(0xd1);
            this.WriteByte((byte) ((value >> 8) & 0xffL));
            this.WriteByte((byte) (value & 0xffL));
            return true;
        }

        protected bool TryPackInt32(long value)
        {
            if ((value > 0x7fffffffL) || (value < -2147483648L))
            {
                return false;
            }
            this.WriteByte(210);
            this.WriteByte((byte) ((value >> 0x18) & 0xffL));
            this.WriteByte((byte) ((value >> 0x10) & 0xffL));
            this.WriteByte((byte) ((value >> 8) & 0xffL));
            this.WriteByte((byte) (value & 0xffL));
            return true;
        }

        protected bool TryPackInt64(long value)
        {
            this.WriteByte(0xd3);
            this.WriteByte((byte) ((value >> 0x38) & 0xffL));
            this.WriteByte((byte) ((value >> 0x30) & 0xffL));
            this.WriteByte((byte) ((value >> 40) & 0xffL));
            this.WriteByte((byte) ((value >> 0x20) & 0xffL));
            this.WriteByte((byte) ((value >> 0x18) & 0xffL));
            this.WriteByte((byte) ((value >> 0x10) & 0xffL));
            this.WriteByte((byte) ((value >> 8) & 0xffL));
            this.WriteByte((byte) (value & 0xffL));
            return true;
        }

        protected bool TryPackInt8(long value)
        {
            if ((value > 0x7fL) || (value < -128L))
            {
                return false;
            }
            this.WriteByte(0xd0);
            this.WriteByte((byte) value);
            return true;
        }

        protected bool TryPackTinySignedInteger(long value)
        {
            if ((value >= 0L) && (value < 0x80L))
            {
                this.WriteByte((byte) value);
                return true;
            }
            if ((value >= -32L) && (value <= -1L))
            {
                this.WriteByte((byte) value);
                return true;
            }
            return false;
        }

        [CLSCompliant(false)]
        protected bool TryPackTinyUnsignedInteger(ulong value)
        {
            if (value < 0x80L)
            {
                this.WriteByte((byte) value);
                return true;
            }
            return false;
        }

        [CLSCompliant(false)]
        protected bool TryPackUInt16(ulong value)
        {
            if (value > 0xffffL)
            {
                return false;
            }
            this.WriteByte(0xcd);
            this.WriteByte((byte) ((value >> 8) & ((ulong) 0xffL)));
            this.WriteByte((byte) (value & ((ulong) 0xffL)));
            return true;
        }

        [CLSCompliant(false)]
        protected bool TryPackUInt32(ulong value)
        {
            if (value > 0xffffffffL)
            {
                return false;
            }
            this.WriteByte(0xce);
            this.WriteByte((byte) ((value >> 0x18) & ((ulong) 0xffL)));
            this.WriteByte((byte) ((value >> 0x10) & ((ulong) 0xffL)));
            this.WriteByte((byte) ((value >> 8) & ((ulong) 0xffL)));
            this.WriteByte((byte) (value & ((ulong) 0xffL)));
            return true;
        }

        [CLSCompliant(false)]
        protected bool TryPackUInt64(ulong value)
        {
            this.WriteByte(0xcf);
            this.WriteByte((byte) ((value >> 0x38) & ((ulong) 0xffL)));
            this.WriteByte((byte) ((value >> 0x30) & ((ulong) 0xffL)));
            this.WriteByte((byte) ((value >> 40) & ((ulong) 0xffL)));
            this.WriteByte((byte) ((value >> 0x20) & ((ulong) 0xffL)));
            this.WriteByte((byte) ((value >> 0x18) & ((ulong) 0xffL)));
            this.WriteByte((byte) ((value >> 0x10) & ((ulong) 0xffL)));
            this.WriteByte((byte) ((value >> 8) & ((ulong) 0xffL)));
            this.WriteByte((byte) (value & ((ulong) 0xffL)));
            return true;
        }

        private bool TryPackUInt8(ulong value)
        {
            if (value > 0xffL)
            {
                return false;
            }
            this.WriteByte(0xcc);
            this.WriteByte((byte) value);
            return true;
        }

        private void VerifyNotDisposed()
        {
            if (this._isDisposed)
            {
                throw new ObjectDisposedException(this.ToString());
            }
        }

        protected abstract void WriteByte(byte value);
        protected virtual void WriteBytes(ICollection<byte> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            foreach (byte num in value)
            {
                this.WriteByte(num);
            }
        }

        protected virtual void WriteBytes(byte[] value, bool isImmutable)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            foreach (byte num in value)
            {
                this.WriteByte(num);
            }
        }

        public virtual bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public virtual long Position
        {
            get
            {
                throw new NotSupportedException();
            }
        }
    }
}

