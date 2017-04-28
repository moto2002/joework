namespace MsgPack
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    internal sealed class ItemsUnpacker : Unpacker
    {
        private CollectionType _collectionType;
        private MessagePackObject? _data;
        private long _itemsCount;
        private readonly bool _ownsStream;
        private readonly Stack<uint> _remainingCollections = new Stack<uint>(4);
        private readonly byte[] _scalarBuffer = new byte[8];
        private readonly Stream _stream;
        private static readonly byte[] DummyBufferForSkipping = new byte[0x10000];

        public ItemsUnpacker(Stream stream, bool ownsStream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            this._stream = stream;
            this._ownsStream = ownsStream;
        }

        protected sealed override void Dispose(bool disposing)
        {
            if (disposing && this._ownsStream)
            {
                this._stream.Dispose();
            }
            base.Dispose(disposing);
        }

        internal void InternalSetData(MessagePackObject? value)
        {
            this._data = value;
        }

        public override bool ReadArrayLength(out long result)
        {
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
            }
            switch (code)
            {
                case 220:
                    if (stream.Read(buffer, 0, 2) < 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    result = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.Array;
                    this._itemsCount = result;
                    this._data = new MessagePackObject?(result);
                    return true;

                case 0xdd:
                {
                    if (stream.Read(buffer, 0, 4) < 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    uint num2 = BigEndianBinary.ToUInt32(buffer, 0);
                    if (num2 > 0x7fffffff)
                    {
                        throw new MessageNotSupportedException("MessagePack for CLI cannot handle large binary which has more than Int32.MaxValue bytes.");
                    }
                    result = num2;
                    this._collectionType = CollectionType.Array;
                    this._itemsCount = result;
                    this._data = new MessagePackObject?(result);
                    return true;
                }
            }
            if ((0x90 > code) || (code > 0x9f))
            {
                throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { "array header", code, MessagePackCode.ToString(code) }));
            }
            result = code & 15;
            this._collectionType = CollectionType.Array;
            this._itemsCount = result;
            this._data = new MessagePackObject?(result);
            return true;
        }

        public override bool ReadBinary(out byte[] result)
        {
            int num2;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = null;
                return false;
            }
            switch (code)
            {
                case 0xc0:
                    result = null;
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(result);
                    return true;

                case 0xc4:
                case 0xd9:
                    num2 = stream.ReadByte();
                    if (num2 < 0)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    break;

                case 0xc5:
                case 0xda:
                    if (stream.Read(buffer, 0, 2) < 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num2 = BigEndianBinary.ToUInt16(buffer, 0);
                    break;

                case 0xc6:
                case 0xdb:
                {
                    if (stream.Read(buffer, 0, 4) < 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    uint num3 = BigEndianBinary.ToUInt32(buffer, 0);
                    if (num3 > 0x7fffffff)
                    {
                        throw new MessageNotSupportedException("MessagePack for CLI cannot handle large binary which has more than Int32.MaxValue bytes.");
                    }
                    num2 = (int) num3;
                    break;
                }
                default:
                    if ((160 > code) || (code > 0xbf))
                    {
                        throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { "System.Byte[]", code, MessagePackCode.ToString(code) }));
                    }
                    num2 = code & 0x1f;
                    break;
            }
            if (num2 == 0)
            {
                result = Binary.Empty;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                this._data = new MessagePackObject?(result);
                return true;
            }
            int offset = 0;
            byte[] buffer2 = new byte[num2];
            if (stream.Read(buffer2, offset, num2) < num2)
            {
                throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
            }
            this._collectionType = CollectionType.None;
            this._itemsCount = 0L;
            result = buffer2;
            return true;
        }

        public override bool ReadBoolean(out bool result)
        {
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = false;
                return false;
            }
            switch (code)
            {
                case 0xc2:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = false;
                    return true;

                case 0xc3:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = true;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(bool), code, MessagePackCode.ToString(code) }));
        }

        public override bool ReadByte(out byte result)
        {
            byte num2;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            switch (code)
            {
                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num2 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num2;
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num2 = (byte) BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num2;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(byte), code, MessagePackCode.ToString(code) }));
        }

        protected override bool ReadCore()
        {
            MessagePackObject obj2;
            if (this.ReadSubtreeObject(out obj2))
            {
                this._data = new MessagePackObject?(obj2);
                return true;
            }
            return false;
        }

        public override bool ReadDouble(out double result)
        {
            double num5;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0.0;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            if (code >= 0xe0)
            {
                sbyte num3 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num3;
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcb:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToDouble(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcf:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd3:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(double), code, MessagePackCode.ToString(code) }));
        }

        public override bool ReadInt16(out short result)
        {
            short num5;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            if (code >= 0xe0)
            {
                sbyte num3 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num3;
                return true;
            }
            switch (code)
            {
                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (short) BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(short), code, MessagePackCode.ToString(code) }));
        }

        public override bool ReadInt32(out int result)
        {
            int num5;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            if (code >= 0xe0)
            {
                sbyte num3 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num3;
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (int) BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (int) BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(int), code, MessagePackCode.ToString(code) }));
        }

        public override bool ReadInt64(out long result)
        {
            long num5;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0L;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            if (code >= 0xe0)
            {
                sbyte num3 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num3;
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (long) BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcb:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (long) BigEndianBinary.ToDouble(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcf:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (long) BigEndianBinary.ToUInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd3:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(long), code, MessagePackCode.ToString(code) }));
        }

        public override bool ReadMapLength(out long result)
        {
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
            }
            switch (code)
            {
                case 0xde:
                    if (stream.Read(buffer, 0, 2) < 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    result = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.Map;
                    this._itemsCount = result;
                    this._data = new MessagePackObject?(result);
                    return true;

                case 0xdf:
                {
                    if (stream.Read(buffer, 0, 4) < 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    uint num2 = BigEndianBinary.ToUInt32(buffer, 0);
                    if (num2 > 0x7fffffff)
                    {
                        throw new MessageNotSupportedException("MessagePack for CLI cannot handle large binary which has more than Int32.MaxValue bytes.");
                    }
                    result = num2;
                    this._collectionType = CollectionType.Map;
                    this._itemsCount = result;
                    this._data = new MessagePackObject?(result);
                    return true;
                }
            }
            if ((0x80 > code) || (code > 0x8f))
            {
                throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { "map header", code, MessagePackCode.ToString(code) }));
            }
            result = code & 15;
            this._collectionType = CollectionType.Map;
            this._itemsCount = result;
            this._data = new MessagePackObject?(result);
            return true;
        }

        public override bool ReadNullableBoolean(out bool? result)
        {
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            switch (code)
            {
                case 0xc2:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;

                case 0xc3:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 1;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(bool), code, MessagePackCode.ToString(code) }));
        }

        public override bool ReadNullableByte(out byte? result)
        {
            byte num2;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new byte?(num2);
                return true;
            }
            switch (code)
            {
                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num2 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new byte?(num2);
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num2 = (byte) BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new byte?(num2);
                    return true;

                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(byte), code, MessagePackCode.ToString(code) }));
        }

        public override bool ReadNullableDouble(out double? result)
        {
            double num5;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0.0;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new double?((double) num2);
                return true;
            }
            if (code >= 0xe0)
            {
                sbyte num3 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new double?((double) num3);
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new double?(num5);
                    return true;

                case 0xcb:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToDouble(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new double?(num5);
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new double?(num5);
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new double?(num5);
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new double?(num5);
                    return true;

                case 0xcf:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new double?(num5);
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new double?(num5);
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new double?(num5);
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new double?(num5);
                    return true;

                case 0xd3:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new double?(num5);
                    return true;

                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(double), code, MessagePackCode.ToString(code) }));
        }

        public override bool ReadNullableInt16(out short? result)
        {
            short num5;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new short?(num2);
                return true;
            }
            if (code >= 0xe0)
            {
                sbyte num3 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new short?(num3);
                return true;
            }
            switch (code)
            {
                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new short?(num5);
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (short) BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new short?(num5);
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new short?(num5);
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new short?(num5);
                    return true;

                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(short), code, MessagePackCode.ToString(code) }));
        }

        public override bool ReadNullableInt32(out int? result)
        {
            int num5;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new int?(num2);
                return true;
            }
            if (code >= 0xe0)
            {
                sbyte num3 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new int?(num3);
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (int) BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new int?(num5);
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new int?(num5);
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new int?(num5);
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (int) BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new int?(num5);
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new int?(num5);
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new int?(num5);
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new int?(num5);
                    return true;

                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(int), code, MessagePackCode.ToString(code) }));
        }

        public override bool ReadNullableInt64(out long? result)
        {
            long num5;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0L;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new long?((long) num2);
                return true;
            }
            if (code >= 0xe0)
            {
                sbyte num3 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new long?((long) num3);
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (long) BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new long?(num5);
                    return true;

                case 0xcb:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (long) BigEndianBinary.ToDouble(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new long?(num5);
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new long?(num5);
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new long?(num5);
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new long?(num5);
                    return true;

                case 0xcf:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (long) BigEndianBinary.ToUInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new long?(num5);
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new long?(num5);
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new long?(num5);
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new long?(num5);
                    return true;

                case 0xd3:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new long?(num5);
                    return true;

                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(long), code, MessagePackCode.ToString(code) }));
        }

        public override bool ReadNullableSByte(out sbyte? result)
        {
            sbyte num2;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                num2 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new sbyte?(num2);
                return true;
            }
            if (code >= 0xe0)
            {
                num2 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new sbyte?(num2);
                return true;
            }
            switch (code)
            {
                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num2 = (sbyte) BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new sbyte?(num2);
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num2 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new sbyte?(num2);
                    return true;

                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(sbyte), code, MessagePackCode.ToString(code) }));
        }

        public override bool ReadNullableSingle(out float? result)
        {
            float num5;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0f;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new float?((float) num2);
                return true;
            }
            if (code >= 0xe0)
            {
                sbyte num3 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new float?((float) num3);
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new float?(num5);
                    return true;

                case 0xcb:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (float) BigEndianBinary.ToDouble(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new float?(num5);
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new float?(num5);
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new float?(num5);
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new float?(num5);
                    return true;

                case 0xcf:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new float?(num5);
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new float?(num5);
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new float?(num5);
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new float?(num5);
                    return true;

                case 0xd3:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new float?(num5);
                    return true;

                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(float), code, MessagePackCode.ToString(code) }));
        }

        public override bool ReadNullableUInt16(out ushort? result)
        {
            ushort num4;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new ushort?(num2);
                return true;
            }
            switch (code)
            {
                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ushort?(num4);
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ushort?(num4);
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ushort) BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ushort?(num4);
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ushort) BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ushort?(num4);
                    return true;

                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(ushort), code, MessagePackCode.ToString(code) }));
        }

        public override bool ReadNullableUInt32(out uint? result)
        {
            uint num4;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new uint?(num2);
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (uint) BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new uint?(num4);
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new uint?(num4);
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new uint?(num4);
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new uint?(num4);
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (uint) BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new uint?(num4);
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (uint) BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new uint?(num4);
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (uint) BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new uint?(num4);
                    return true;

                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(uint), code, MessagePackCode.ToString(code) }));
        }

        public override bool ReadNullableUInt64(out ulong? result)
        {
            ulong num4;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0L;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new ulong?((ulong) num2);
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ulong?(num4);
                    return true;

                case 0xcb:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToDouble(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ulong?(num4);
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ulong?(num4);
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ulong?(num4);
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ulong?(num4);
                    return true;

                case 0xcf:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ulong?(num4);
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ulong?(num4);
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ulong?(num4);
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ulong?(num4);
                    return true;

                case 0xd3:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ulong?(num4);
                    return true;

                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(ulong), code, MessagePackCode.ToString(code) }));
        }

        public override bool ReadObject(out MessagePackObject result)
        {
            int num2;
            byte[] buffer2;
            MessagePackObject obj2;
            ushort num16;
            uint num17;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int index = stream.ReadByte();
            if (index < 0)
            {
                result = new MessagePackObject();
                return false;
            }
            switch (index)
            {
                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(MessagePackObject.Nil);
                    result = MessagePackObject.Nil;
                    return true;

                case 0xc2:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(Unpacking.FalseValue);
                    result = Unpacking.FalseValue;
                    return true;

                case 0xc3:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(Unpacking.TrueValue);
                    result = Unpacking.TrueValue;
                    return true;
            }
            if (index < 0x80)
            {
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                this._data = new MessagePackObject?(Unpacking.PositiveIntegers[index]);
                result = Unpacking.PositiveIntegers[index];
                return true;
            }
            if (index >= 0xe0)
            {
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                this._data = new MessagePackObject?(Unpacking.NegativeIntegers[index - 0xe0]);
                result = Unpacking.NegativeIntegers[index - 0xe0];
                return true;
            }
            switch ((index & 240))
            {
                case 160:
                case 0xb0:
                    num2 = index & 0x1f;
                    buffer2 = new byte[num2];
                    if (stream.Read(buffer2, 0, num2) < num2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    obj2 = new MessagePackObject(new MessagePackString(buffer2));
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(obj2);
                    result = obj2;
                    return true;

                case 0x80:
                    num2 = index & 15;
                    this._collectionType = CollectionType.Map;
                    this._itemsCount = num2;
                    this._data = new MessagePackObject?(num2);
                    result = Unpacking.PositiveIntegers[num2];
                    return true;

                case 0x90:
                    num2 = index & 15;
                    this._collectionType = CollectionType.Array;
                    this._itemsCount = num2;
                    this._data = new MessagePackObject?(num2);
                    result = Unpacking.PositiveIntegers[num2];
                    return true;
            }
            switch (index)
            {
                case 0xc4:
                case 0xd9:
                {
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    byte count = BigEndianBinary.ToByte(buffer, 0);
                    buffer2 = new byte[count];
                    if (stream.Read(buffer2, 0, count) < count)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    obj2 = new MessagePackObject(new MessagePackString(buffer2));
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(obj2);
                    result = obj2;
                    return true;
                }
                case 0xc5:
                case 0xda:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num16 = BigEndianBinary.ToUInt16(buffer, 0);
                    buffer2 = new byte[num16];
                    if (stream.Read(buffer2, 0, num16) < num16)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    obj2 = new MessagePackObject(new MessagePackString(buffer2));
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(obj2);
                    result = obj2;
                    return true;

                case 0xc6:
                case 0xdb:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num17 = BigEndianBinary.ToUInt32(buffer, 0);
                    if (num17 > 0x7fffffff)
                    {
                        throw new MessageNotSupportedException("MessagePack for CLI cannot handle large binary which has more than Int32.MaxValue bytes.");
                    }
                    num2 = (int) num17;
                    buffer2 = new byte[num2];
                    if (stream.Read(buffer2, 0, num2) < num2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    obj2 = new MessagePackObject(new MessagePackString(buffer2));
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(obj2);
                    result = obj2;
                    return true;

                case 0xc7:
                case 200:
                case 0xc9:
                case 0xd4:
                case 0xd5:
                case 0xd6:
                case 0xd7:
                case 0xd8:
                    throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Extension type 0x{0:X} is not supported yet.", new object[] { index }));

                case 0xca:
                {
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    float num13 = BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(num13);
                    result = num13;
                    return true;
                }
                case 0xcb:
                {
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    double num14 = BigEndianBinary.ToDouble(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(num14);
                    result = num14;
                    return true;
                }
                case 0xcc:
                {
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    byte num9 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(num9);
                    result = num9;
                    return true;
                }
                case 0xcd:
                {
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    ushort num10 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(num10);
                    result = num10;
                    return true;
                }
                case 0xce:
                {
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    uint num11 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(num11);
                    result = num11;
                    return true;
                }
                case 0xcf:
                {
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    ulong num12 = BigEndianBinary.ToUInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(num12);
                    result = num12;
                    return true;
                }
                case 0xd0:
                {
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    sbyte num4 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(num4);
                    result = num4;
                    return true;
                }
                case 0xd1:
                {
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    short num6 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(num6);
                    result = num6;
                    return true;
                }
                case 210:
                {
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    int num7 = BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(num7);
                    result = num7;
                    return true;
                }
                case 0xd3:
                {
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    long num8 = BigEndianBinary.ToInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(num8);
                    result = num8;
                    return true;
                }
                case 220:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num16 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.Array;
                    this._itemsCount = num16;
                    this._data = new MessagePackObject?(num16);
                    result = num16;
                    return true;

                case 0xdd:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num17 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.Array;
                    this._itemsCount = num17;
                    this._data = new MessagePackObject?(num17);
                    result = num17;
                    return true;

                case 0xde:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num16 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.Map;
                    this._itemsCount = num16;
                    this._data = new MessagePackObject?(num16);
                    result = num16;
                    return true;

                case 0xdf:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num17 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.Map;
                    this._itemsCount = num17;
                    this._data = new MessagePackObject?(num17);
                    result = num17;
                    return true;
            }
            Contract.Assert(index == 0xc1, "Unhandled header:" + index.ToString("X2"));
            throw new UnassignedMessageTypeException(string.Format(CultureInfo.CurrentCulture, "Unknown header value 0x{0:X}", new object[] { index }));
        }

        public override bool ReadSByte(out sbyte result)
        {
            sbyte num2;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                num2 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            if (code >= 0xe0)
            {
                num2 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            switch (code)
            {
                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num2 = (sbyte) BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num2;
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num2 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num2;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(sbyte), code, MessagePackCode.ToString(code) }));
        }

        public override bool ReadSingle(out float result)
        {
            float num5;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0f;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            if (code >= 0xe0)
            {
                sbyte num3 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num3;
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcb:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (float) BigEndianBinary.ToDouble(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcf:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd3:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(float), code, MessagePackCode.ToString(code) }));
        }

        public override bool ReadString(out string result)
        {
            int num2;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Encoding encoding = Encoding.UTF8;
            Contract.Assert(encoding != null);
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = null;
                return false;
            }
            switch (code)
            {
                case 0xc0:
                    result = null;
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(result);
                    return true;

                case 0xc4:
                case 0xd9:
                    num2 = stream.ReadByte();
                    if (num2 < 0)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    break;

                case 0xc5:
                case 0xda:
                    if (stream.Read(buffer, 0, 2) < 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num2 = BigEndianBinary.ToUInt16(buffer, 0);
                    break;

                case 0xc6:
                case 0xdb:
                {
                    if (stream.Read(buffer, 0, 4) < 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    uint num3 = BigEndianBinary.ToUInt32(buffer, 0);
                    if (num3 > 0x7fffffff)
                    {
                        throw new MessageNotSupportedException("MessagePack for CLI cannot handle large binary which has more than Int32.MaxValue bytes.");
                    }
                    num2 = (int) num3;
                    break;
                }
                default:
                    if ((160 > code) || (code > 0xbf))
                    {
                        throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { "System.String", code, MessagePackCode.ToString(code) }));
                    }
                    num2 = code & 0x1f;
                    break;
            }
            if (num2 == 0)
            {
                result = string.Empty;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                this._data = new MessagePackObject?(result);
                return true;
            }
            System.Text.Decoder decoder = encoding.GetDecoder();
            int num4 = (num2 > 0x4000) ? 0x4000 : num2;
            byte[] buffer2 = new byte[num4];
            char[] chars = new char[num4];
            StringBuilder builder = new StringBuilder(num2);
            int num5 = num2;
            do
            {
                int num9;
                int count = (num5 > buffer2.Length) ? buffer2.Length : num5;
                int num7 = stream.Read(buffer2, 0, count);
                if (num7 == 0)
                {
                    throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                }
                num5 -= num7;
                bool completed = false;
                for (int i = 0; !completed; i += num9)
                {
                    int num10;
                    decoder.Convert(buffer2, i, num7 - i, chars, 0, chars.Length, num7 == 0, out num9, out num10, out completed);
                    builder.Append(chars, 0, num10);
                }
            }
            while (num5 > 0);
            string str = builder.ToString();
            this._collectionType = CollectionType.None;
            this._itemsCount = 0L;
            result = str;
            return true;
        }

        internal bool ReadSubtreeArrayLength(out long result)
        {
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
            }
            switch (code)
            {
                case 220:
                    if (stream.Read(buffer, 0, 2) < 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    result = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.Array;
                    this._itemsCount = result;
                    this._data = new MessagePackObject?(result);
                    return true;

                case 0xdd:
                {
                    if (stream.Read(buffer, 0, 4) < 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    uint num2 = BigEndianBinary.ToUInt32(buffer, 0);
                    if (num2 > 0x7fffffff)
                    {
                        throw new MessageNotSupportedException("MessagePack for CLI cannot handle large binary which has more than Int32.MaxValue bytes.");
                    }
                    result = num2;
                    this._collectionType = CollectionType.Array;
                    this._itemsCount = result;
                    this._data = new MessagePackObject?(result);
                    return true;
                }
            }
            if ((0x90 > code) || (code > 0x9f))
            {
                throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { "array header", code, MessagePackCode.ToString(code) }));
            }
            result = code & 15;
            this._collectionType = CollectionType.Array;
            this._itemsCount = result;
            this._data = new MessagePackObject?(result);
            return true;
        }

        internal bool ReadSubtreeBinary(out byte[] result)
        {
            int num2;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = null;
                return false;
            }
            switch (code)
            {
                case 0xc0:
                    result = null;
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(result);
                    return true;

                case 0xc4:
                case 0xd9:
                    num2 = stream.ReadByte();
                    if (num2 < 0)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    break;

                case 0xc5:
                case 0xda:
                    if (stream.Read(buffer, 0, 2) < 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num2 = BigEndianBinary.ToUInt16(buffer, 0);
                    break;

                case 0xc6:
                case 0xdb:
                {
                    if (stream.Read(buffer, 0, 4) < 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    uint num3 = BigEndianBinary.ToUInt32(buffer, 0);
                    if (num3 > 0x7fffffff)
                    {
                        throw new MessageNotSupportedException("MessagePack for CLI cannot handle large binary which has more than Int32.MaxValue bytes.");
                    }
                    num2 = (int) num3;
                    break;
                }
                default:
                    if ((160 > code) || (code > 0xbf))
                    {
                        throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { "System.Byte[]", code, MessagePackCode.ToString(code) }));
                    }
                    num2 = code & 0x1f;
                    break;
            }
            if (num2 == 0)
            {
                result = Binary.Empty;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                this._data = new MessagePackObject?(result);
                return true;
            }
            int offset = 0;
            byte[] buffer2 = new byte[num2];
            if (stream.Read(buffer2, offset, num2) < num2)
            {
                throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
            }
            this._collectionType = CollectionType.None;
            this._itemsCount = 0L;
            result = buffer2;
            return true;
        }

        internal bool ReadSubtreeBoolean(out bool result)
        {
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = false;
                return false;
            }
            switch (code)
            {
                case 0xc2:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = false;
                    return true;

                case 0xc3:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = true;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(bool), code, MessagePackCode.ToString(code) }));
        }

        internal bool ReadSubtreeByte(out byte result)
        {
            byte num2;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            switch (code)
            {
                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num2 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num2;
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num2 = (byte) BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num2;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(byte), code, MessagePackCode.ToString(code) }));
        }

        protected sealed override Unpacker ReadSubtreeCore()
        {
            return new SubtreeUnpacker(this);
        }

        internal bool ReadSubtreeDouble(out double result)
        {
            double num5;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0.0;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            if (code >= 0xe0)
            {
                sbyte num3 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num3;
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcb:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToDouble(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcf:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd3:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(double), code, MessagePackCode.ToString(code) }));
        }

        internal bool ReadSubtreeInt16(out short result)
        {
            short num5;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            if (code >= 0xe0)
            {
                sbyte num3 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num3;
                return true;
            }
            switch (code)
            {
                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (short) BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(short), code, MessagePackCode.ToString(code) }));
        }

        internal bool ReadSubtreeInt32(out int result)
        {
            int num5;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            if (code >= 0xe0)
            {
                sbyte num3 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num3;
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (int) BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (int) BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(int), code, MessagePackCode.ToString(code) }));
        }

        internal bool ReadSubtreeInt64(out long result)
        {
            long num5;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0L;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            if (code >= 0xe0)
            {
                sbyte num3 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num3;
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (long) BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcb:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (long) BigEndianBinary.ToDouble(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcf:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (long) BigEndianBinary.ToUInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd3:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(long), code, MessagePackCode.ToString(code) }));
        }

        internal bool ReadSubtreeItem()
        {
            return this.ReadCore();
        }

        internal bool ReadSubtreeMapLength(out long result)
        {
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
            }
            switch (code)
            {
                case 0xde:
                    if (stream.Read(buffer, 0, 2) < 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    result = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.Map;
                    this._itemsCount = result;
                    this._data = new MessagePackObject?(result);
                    return true;

                case 0xdf:
                {
                    if (stream.Read(buffer, 0, 4) < 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    uint num2 = BigEndianBinary.ToUInt32(buffer, 0);
                    if (num2 > 0x7fffffff)
                    {
                        throw new MessageNotSupportedException("MessagePack for CLI cannot handle large binary which has more than Int32.MaxValue bytes.");
                    }
                    result = num2;
                    this._collectionType = CollectionType.Map;
                    this._itemsCount = result;
                    this._data = new MessagePackObject?(result);
                    return true;
                }
            }
            if ((0x80 > code) || (code > 0x8f))
            {
                throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { "map header", code, MessagePackCode.ToString(code) }));
            }
            result = code & 15;
            this._collectionType = CollectionType.Map;
            this._itemsCount = result;
            this._data = new MessagePackObject?(result);
            return true;
        }

        internal bool ReadSubtreeNullableBoolean(out bool? result)
        {
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            switch (code)
            {
                case 0xc2:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;

                case 0xc3:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 1;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(bool), code, MessagePackCode.ToString(code) }));
        }

        internal bool ReadSubtreeNullableByte(out byte? result)
        {
            byte num2;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new byte?(num2);
                return true;
            }
            switch (code)
            {
                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num2 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new byte?(num2);
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num2 = (byte) BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new byte?(num2);
                    return true;

                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(byte), code, MessagePackCode.ToString(code) }));
        }

        internal bool ReadSubtreeNullableDouble(out double? result)
        {
            double num5;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0.0;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new double?((double) num2);
                return true;
            }
            if (code >= 0xe0)
            {
                sbyte num3 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new double?((double) num3);
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new double?(num5);
                    return true;

                case 0xcb:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToDouble(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new double?(num5);
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new double?(num5);
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new double?(num5);
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new double?(num5);
                    return true;

                case 0xcf:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new double?(num5);
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new double?(num5);
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new double?(num5);
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new double?(num5);
                    return true;

                case 0xd3:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new double?(num5);
                    return true;

                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(double), code, MessagePackCode.ToString(code) }));
        }

        internal bool ReadSubtreeNullableInt16(out short? result)
        {
            short num5;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new short?(num2);
                return true;
            }
            if (code >= 0xe0)
            {
                sbyte num3 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new short?(num3);
                return true;
            }
            switch (code)
            {
                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new short?(num5);
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (short) BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new short?(num5);
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new short?(num5);
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new short?(num5);
                    return true;

                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(short), code, MessagePackCode.ToString(code) }));
        }

        internal bool ReadSubtreeNullableInt32(out int? result)
        {
            int num5;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new int?(num2);
                return true;
            }
            if (code >= 0xe0)
            {
                sbyte num3 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new int?(num3);
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (int) BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new int?(num5);
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new int?(num5);
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new int?(num5);
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (int) BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new int?(num5);
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new int?(num5);
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new int?(num5);
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new int?(num5);
                    return true;

                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(int), code, MessagePackCode.ToString(code) }));
        }

        internal bool ReadSubtreeNullableInt64(out long? result)
        {
            long num5;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0L;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new long?((long) num2);
                return true;
            }
            if (code >= 0xe0)
            {
                sbyte num3 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new long?((long) num3);
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (long) BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new long?(num5);
                    return true;

                case 0xcb:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (long) BigEndianBinary.ToDouble(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new long?(num5);
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new long?(num5);
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new long?(num5);
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new long?(num5);
                    return true;

                case 0xcf:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (long) BigEndianBinary.ToUInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new long?(num5);
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new long?(num5);
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new long?(num5);
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new long?(num5);
                    return true;

                case 0xd3:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new long?(num5);
                    return true;

                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(long), code, MessagePackCode.ToString(code) }));
        }

        internal bool ReadSubtreeNullableSByte(out sbyte? result)
        {
            sbyte num2;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                num2 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new sbyte?(num2);
                return true;
            }
            if (code >= 0xe0)
            {
                num2 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new sbyte?(num2);
                return true;
            }
            switch (code)
            {
                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num2 = (sbyte) BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new sbyte?(num2);
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num2 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new sbyte?(num2);
                    return true;

                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(sbyte), code, MessagePackCode.ToString(code) }));
        }

        internal bool ReadSubtreeNullableSingle(out float? result)
        {
            float num5;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0f;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new float?((float) num2);
                return true;
            }
            if (code >= 0xe0)
            {
                sbyte num3 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new float?((float) num3);
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new float?(num5);
                    return true;

                case 0xcb:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (float) BigEndianBinary.ToDouble(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new float?(num5);
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new float?(num5);
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new float?(num5);
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new float?(num5);
                    return true;

                case 0xcf:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new float?(num5);
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new float?(num5);
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new float?(num5);
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new float?(num5);
                    return true;

                case 0xd3:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new float?(num5);
                    return true;

                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(float), code, MessagePackCode.ToString(code) }));
        }

        internal bool ReadSubtreeNullableUInt16(out ushort? result)
        {
            ushort num4;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new ushort?(num2);
                return true;
            }
            switch (code)
            {
                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ushort?(num4);
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ushort?(num4);
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ushort) BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ushort?(num4);
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ushort) BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ushort?(num4);
                    return true;

                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(ushort), code, MessagePackCode.ToString(code) }));
        }

        internal bool ReadSubtreeNullableUInt32(out uint? result)
        {
            uint num4;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new uint?(num2);
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (uint) BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new uint?(num4);
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new uint?(num4);
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new uint?(num4);
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new uint?(num4);
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (uint) BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new uint?(num4);
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (uint) BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new uint?(num4);
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (uint) BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new uint?(num4);
                    return true;

                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(uint), code, MessagePackCode.ToString(code) }));
        }

        internal bool ReadSubtreeNullableUInt64(out ulong? result)
        {
            ulong num4;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0L;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = new ulong?((ulong) num2);
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ulong?(num4);
                    return true;

                case 0xcb:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToDouble(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ulong?(num4);
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ulong?(num4);
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ulong?(num4);
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ulong?(num4);
                    return true;

                case 0xcf:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ulong?(num4);
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ulong?(num4);
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ulong?(num4);
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ulong?(num4);
                    return true;

                case 0xd3:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = new ulong?(num4);
                    return true;

                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = 0;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(ulong), code, MessagePackCode.ToString(code) }));
        }

        internal bool ReadSubtreeObject(out MessagePackObject result)
        {
            int num2;
            byte[] buffer2;
            MessagePackObject obj2;
            ushort num16;
            uint num17;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int index = stream.ReadByte();
            if (index < 0)
            {
                result = new MessagePackObject();
                return false;
            }
            switch (index)
            {
                case 0xc0:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(MessagePackObject.Nil);
                    result = MessagePackObject.Nil;
                    return true;

                case 0xc2:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(Unpacking.FalseValue);
                    result = Unpacking.FalseValue;
                    return true;

                case 0xc3:
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(Unpacking.TrueValue);
                    result = Unpacking.TrueValue;
                    return true;
            }
            if (index < 0x80)
            {
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                this._data = new MessagePackObject?(Unpacking.PositiveIntegers[index]);
                result = Unpacking.PositiveIntegers[index];
                return true;
            }
            if (index >= 0xe0)
            {
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                this._data = new MessagePackObject?(Unpacking.NegativeIntegers[index - 0xe0]);
                result = Unpacking.NegativeIntegers[index - 0xe0];
                return true;
            }
            switch ((index & 240))
            {
                case 160:
                case 0xb0:
                    num2 = index & 0x1f;
                    buffer2 = new byte[num2];
                    if (stream.Read(buffer2, 0, num2) < num2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    obj2 = new MessagePackObject(new MessagePackString(buffer2));
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(obj2);
                    result = obj2;
                    return true;

                case 0x80:
                    num2 = index & 15;
                    this._collectionType = CollectionType.Map;
                    this._itemsCount = num2;
                    this._data = new MessagePackObject?(num2);
                    result = Unpacking.PositiveIntegers[num2];
                    return true;

                case 0x90:
                    num2 = index & 15;
                    this._collectionType = CollectionType.Array;
                    this._itemsCount = num2;
                    this._data = new MessagePackObject?(num2);
                    result = Unpacking.PositiveIntegers[num2];
                    return true;
            }
            switch (index)
            {
                case 0xc4:
                case 0xd9:
                {
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    byte count = BigEndianBinary.ToByte(buffer, 0);
                    buffer2 = new byte[count];
                    if (stream.Read(buffer2, 0, count) < count)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    obj2 = new MessagePackObject(new MessagePackString(buffer2));
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(obj2);
                    result = obj2;
                    return true;
                }
                case 0xc5:
                case 0xda:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num16 = BigEndianBinary.ToUInt16(buffer, 0);
                    buffer2 = new byte[num16];
                    if (stream.Read(buffer2, 0, num16) < num16)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    obj2 = new MessagePackObject(new MessagePackString(buffer2));
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(obj2);
                    result = obj2;
                    return true;

                case 0xc6:
                case 0xdb:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num17 = BigEndianBinary.ToUInt32(buffer, 0);
                    if (num17 > 0x7fffffff)
                    {
                        throw new MessageNotSupportedException("MessagePack for CLI cannot handle large binary which has more than Int32.MaxValue bytes.");
                    }
                    num2 = (int) num17;
                    buffer2 = new byte[num2];
                    if (stream.Read(buffer2, 0, num2) < num2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    obj2 = new MessagePackObject(new MessagePackString(buffer2));
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(obj2);
                    result = obj2;
                    return true;

                case 0xc7:
                case 200:
                case 0xc9:
                case 0xd4:
                case 0xd5:
                case 0xd6:
                case 0xd7:
                case 0xd8:
                    throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Extension type 0x{0:X} is not supported yet.", new object[] { index }));

                case 0xca:
                {
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    float num13 = BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(num13);
                    result = num13;
                    return true;
                }
                case 0xcb:
                {
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    double num14 = BigEndianBinary.ToDouble(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(num14);
                    result = num14;
                    return true;
                }
                case 0xcc:
                {
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    byte num9 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(num9);
                    result = num9;
                    return true;
                }
                case 0xcd:
                {
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    ushort num10 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(num10);
                    result = num10;
                    return true;
                }
                case 0xce:
                {
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    uint num11 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(num11);
                    result = num11;
                    return true;
                }
                case 0xcf:
                {
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    ulong num12 = BigEndianBinary.ToUInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(num12);
                    result = num12;
                    return true;
                }
                case 0xd0:
                {
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    sbyte num4 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(num4);
                    result = num4;
                    return true;
                }
                case 0xd1:
                {
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    short num6 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(num6);
                    result = num6;
                    return true;
                }
                case 210:
                {
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    int num7 = BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(num7);
                    result = num7;
                    return true;
                }
                case 0xd3:
                {
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    long num8 = BigEndianBinary.ToInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(num8);
                    result = num8;
                    return true;
                }
                case 220:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num16 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.Array;
                    this._itemsCount = num16;
                    this._data = new MessagePackObject?(num16);
                    result = num16;
                    return true;

                case 0xdd:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num17 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.Array;
                    this._itemsCount = num17;
                    this._data = new MessagePackObject?(num17);
                    result = num17;
                    return true;

                case 0xde:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num16 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.Map;
                    this._itemsCount = num16;
                    this._data = new MessagePackObject?(num16);
                    result = num16;
                    return true;

                case 0xdf:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num17 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.Map;
                    this._itemsCount = num17;
                    this._data = new MessagePackObject?(num17);
                    result = num17;
                    return true;
            }
            Contract.Assert(index == 0xc1, "Unhandled header:" + index.ToString("X2"));
            throw new UnassignedMessageTypeException(string.Format(CultureInfo.CurrentCulture, "Unknown header value 0x{0:X}", new object[] { index }));
        }

        internal bool ReadSubtreeSByte(out sbyte result)
        {
            sbyte num2;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                num2 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            if (code >= 0xe0)
            {
                num2 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            switch (code)
            {
                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num2 = (sbyte) BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num2;
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num2 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num2;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(sbyte), code, MessagePackCode.ToString(code) }));
        }

        internal bool ReadSubtreeSingle(out float result)
        {
            float num5;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0f;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            if (code >= 0xe0)
            {
                sbyte num3 = (sbyte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num3;
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcb:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = (float) BigEndianBinary.ToDouble(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xcf:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToUInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;

                case 0xd3:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num5 = BigEndianBinary.ToInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num5;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(float), code, MessagePackCode.ToString(code) }));
        }

        internal bool ReadSubtreeString(out string result)
        {
            int num2;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Encoding encoding = Encoding.UTF8;
            Contract.Assert(encoding != null);
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = null;
                return false;
            }
            switch (code)
            {
                case 0xc0:
                    result = null;
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    this._data = new MessagePackObject?(result);
                    return true;

                case 0xc4:
                case 0xd9:
                    num2 = stream.ReadByte();
                    if (num2 < 0)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    break;

                case 0xc5:
                case 0xda:
                    if (stream.Read(buffer, 0, 2) < 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num2 = BigEndianBinary.ToUInt16(buffer, 0);
                    break;

                case 0xc6:
                case 0xdb:
                {
                    if (stream.Read(buffer, 0, 4) < 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    uint num3 = BigEndianBinary.ToUInt32(buffer, 0);
                    if (num3 > 0x7fffffff)
                    {
                        throw new MessageNotSupportedException("MessagePack for CLI cannot handle large binary which has more than Int32.MaxValue bytes.");
                    }
                    num2 = (int) num3;
                    break;
                }
                default:
                    if ((160 > code) || (code > 0xbf))
                    {
                        throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { "System.String", code, MessagePackCode.ToString(code) }));
                    }
                    num2 = code & 0x1f;
                    break;
            }
            if (num2 == 0)
            {
                result = string.Empty;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                this._data = new MessagePackObject?(result);
                return true;
            }
            System.Text.Decoder decoder = encoding.GetDecoder();
            int num4 = (num2 > 0x4000) ? 0x4000 : num2;
            byte[] buffer2 = new byte[num4];
            char[] chars = new char[num4];
            StringBuilder builder = new StringBuilder(num2);
            int num5 = num2;
            do
            {
                int num9;
                int count = (num5 > buffer2.Length) ? buffer2.Length : num5;
                int num7 = stream.Read(buffer2, 0, count);
                if (num7 == 0)
                {
                    throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                }
                num5 -= num7;
                bool completed = false;
                for (int i = 0; !completed; i += num9)
                {
                    int num10;
                    decoder.Convert(buffer2, i, num7 - i, chars, 0, chars.Length, num7 == 0, out num9, out num10, out completed);
                    builder.Append(chars, 0, num10);
                }
            }
            while (num5 > 0);
            string str = builder.ToString();
            this._collectionType = CollectionType.None;
            this._itemsCount = 0L;
            result = str;
            return true;
        }

        internal bool ReadSubtreeUInt16(out ushort result)
        {
            ushort num4;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            switch (code)
            {
                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ushort) BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ushort) BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(ushort), code, MessagePackCode.ToString(code) }));
        }

        internal bool ReadSubtreeUInt32(out uint result)
        {
            uint num4;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (uint) BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (uint) BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (uint) BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (uint) BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(uint), code, MessagePackCode.ToString(code) }));
        }

        internal bool ReadSubtreeUInt64(out ulong result)
        {
            ulong num4;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0L;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xcb:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToDouble(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xcf:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xd3:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(ulong), code, MessagePackCode.ToString(code) }));
        }

        public override bool ReadUInt16(out ushort result)
        {
            ushort num4;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            switch (code)
            {
                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ushort) BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ushort) BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(ushort), code, MessagePackCode.ToString(code) }));
        }

        public override bool ReadUInt32(out uint result)
        {
            uint num4;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (uint) BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (uint) BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (uint) BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (uint) BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(uint), code, MessagePackCode.ToString(code) }));
        }

        public override bool ReadUInt64(out ulong result)
        {
            ulong num4;
            base.EnsureNotInSubtreeMode();
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            int code = stream.ReadByte();
            if (code < 0)
            {
                result = 0L;
                return false;
            }
            if (code < 0x80)
            {
                byte num2 = (byte) code;
                this._collectionType = CollectionType.None;
                this._itemsCount = 0L;
                result = num2;
                return true;
            }
            switch (code)
            {
                case 0xca:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToSingle(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xcb:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToDouble(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xcc:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xcd:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xce:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xcf:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = BigEndianBinary.ToUInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xd0:
                    if (stream.Read(buffer, 0, 1) != 1)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToSByte(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xd1:
                    if (stream.Read(buffer, 0, 2) != 2)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToInt16(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 210:
                    if (stream.Read(buffer, 0, 4) != 4)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToInt32(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;

                case 0xd3:
                    if (stream.Read(buffer, 0, 8) != 8)
                    {
                        throw new InvalidMessagePackStreamException("Stream unexpectedly ends.");
                    }
                    num4 = (ulong) BigEndianBinary.ToInt64(buffer, 0);
                    this._collectionType = CollectionType.None;
                    this._itemsCount = 0L;
                    result = num4;
                    return true;
            }
            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Cannot convert '{0}' type value from type '{2}'(0x{1:X}).", new object[] { typeof(ulong), code, MessagePackCode.ToString(code) }));
        }

        protected sealed override long? SkipCore()
        {
            int num3;
            long num5;
            long num6;
            int num7;
            Stream stream = this._stream;
            byte[] buffer = this._scalarBuffer;
            Contract.Assert(stream != null);
            Contract.Assert(buffer != null);
            long item = -1L;
            long num2 = 0L;
            Stack<long> stack = null;
        Label_0032:
            num3 = stream.ReadByte();
            if (num3 < 0)
            {
                return null;
            }
            switch (num3)
            {
                case 0xc0:
                case 0xc2:
                case 0xc3:
                    num2 += 1L;
                    item -= 1L;
                    if (stack != null)
                    {
                        while ((item == 0L) && (stack.Count > 0))
                        {
                            if (stack.Count == 0)
                            {
                                break;
                            }
                            item = stack.Pop() - 1L;
                        }
                    }
                    goto Label_0D99;

                default:
                    int num4;
                    if (num3 < 0x80)
                    {
                        num2 += 1L;
                        item -= 1L;
                        if (stack != null)
                        {
                            while ((item == 0L) && (stack.Count > 0))
                            {
                                if (stack.Count == 0)
                                {
                                    break;
                                }
                                item = stack.Pop() - 1L;
                            }
                        }
                        goto Label_0D99;
                    }
                    if (num3 >= 0xe0)
                    {
                        num2 += 1L;
                        item -= 1L;
                        if (stack != null)
                        {
                            while ((item == 0L) && (stack.Count > 0))
                            {
                                if (stack.Count == 0)
                                {
                                    break;
                                }
                                item = stack.Pop() - 1L;
                            }
                        }
                        goto Label_0D99;
                    }
                    switch ((num3 & 240))
                    {
                        case 160:
                        case 0xb0:
                            num4 = num3 & 0x1f;
                            num2 += 1L;
                            num5 = 0L;
                            while (num4 > num5)
                            {
                                num6 = num4 - num5;
                                num7 = (num6 > DummyBufferForSkipping.Length) ? DummyBufferForSkipping.Length : ((int) num6);
                                num5 += stream.Read(DummyBufferForSkipping, 0, num7);
                                if (num5 < num7)
                                {
                                    return null;
                                }
                            }
                            num2 += num5;
                            item -= 1L;
                            if (stack != null)
                            {
                                while ((item == 0L) && (stack.Count > 0))
                                {
                                    if (stack.Count == 0)
                                    {
                                        break;
                                    }
                                    item = stack.Pop() - 1L;
                                }
                            }
                            goto Label_0D99;

                        case 0x80:
                            num4 = num3 & 15;
                            num2 += 1L;
                            if (num4 == 0)
                            {
                                item -= 1L;
                                if (stack != null)
                                {
                                    while ((item == 0L) && (stack.Count > 0))
                                    {
                                        if (stack.Count == 0)
                                        {
                                            break;
                                        }
                                        item = stack.Pop() - 1L;
                                    }
                                }
                            }
                            else
                            {
                                if (item >= 0L)
                                {
                                    if (stack == null)
                                    {
                                        stack = new Stack<long>(4);
                                    }
                                    stack.Push(item);
                                }
                                item = num4 * 2;
                            }
                            goto Label_0D99;

                        case 0x90:
                            num4 = num3 & 15;
                            num2 += 1L;
                            if (num4 == 0)
                            {
                                item -= 1L;
                                if (stack != null)
                                {
                                    while ((item == 0L) && (stack.Count > 0))
                                    {
                                        if (stack.Count == 0)
                                        {
                                            break;
                                        }
                                        item = stack.Pop() - 1L;
                                    }
                                }
                            }
                            else
                            {
                                if (item >= 0L)
                                {
                                    if (stack == null)
                                    {
                                        stack = new Stack<long>(4);
                                    }
                                    stack.Push(item);
                                }
                                item = num4;
                            }
                            goto Label_0D99;

                        default:
                            ushort num8;
                            uint num10;
                            switch (num3)
                            {
                                case 0xca:
                                case 0xce:
                                case 210:
                                    num2 += 1L;
                                    item -= 1L;
                                    if (stack != null)
                                    {
                                        while ((item == 0L) && (stack.Count > 0))
                                        {
                                            if (stack.Count == 0)
                                            {
                                                break;
                                            }
                                            item = stack.Pop() - 1L;
                                        }
                                    }
                                    num5 = 0L;
                                    while (4L > num5)
                                    {
                                        num6 = 4L - num5;
                                        num7 = (num6 > DummyBufferForSkipping.Length) ? DummyBufferForSkipping.Length : ((int) num6);
                                        num5 += stream.Read(DummyBufferForSkipping, 0, num7);
                                        if (num5 < num7)
                                        {
                                            return null;
                                        }
                                    }
                                    num2 += num5;
                                    goto Label_0D99;

                                case 0xcb:
                                case 0xcf:
                                case 0xd3:
                                    num2 += 1L;
                                    item -= 1L;
                                    if (stack != null)
                                    {
                                        while ((item == 0L) && (stack.Count > 0))
                                        {
                                            if (stack.Count == 0)
                                            {
                                                break;
                                            }
                                            item = stack.Pop() - 1L;
                                        }
                                    }
                                    num5 = 0L;
                                    while (8L > num5)
                                    {
                                        num6 = 8L - num5;
                                        num7 = (num6 > DummyBufferForSkipping.Length) ? DummyBufferForSkipping.Length : ((int) num6);
                                        num5 += stream.Read(DummyBufferForSkipping, 0, num7);
                                        if (num5 < num7)
                                        {
                                            return null;
                                        }
                                    }
                                    num2 += num5;
                                    goto Label_0D99;

                                case 0xcc:
                                case 0xd0:
                                    num2 += 1L;
                                    item -= 1L;
                                    if (stack != null)
                                    {
                                        while ((item == 0L) && (stack.Count > 0))
                                        {
                                            if (stack.Count == 0)
                                            {
                                                break;
                                            }
                                            item = stack.Pop() - 1L;
                                        }
                                    }
                                    goto Label_04F3;

                                case 0xcd:
                                case 0xd1:
                                    num2 += 1L;
                                    item -= 1L;
                                    if (stack != null)
                                    {
                                        while ((item == 0L) && (stack.Count > 0))
                                        {
                                            if (stack.Count == 0)
                                            {
                                                break;
                                            }
                                            item = stack.Pop() - 1L;
                                        }
                                    }
                                    num5 = 0L;
                                    while (2L > num5)
                                    {
                                        num6 = 2L - num5;
                                        num7 = (num6 > DummyBufferForSkipping.Length) ? DummyBufferForSkipping.Length : ((int) num6);
                                        num5 += stream.Read(DummyBufferForSkipping, 0, num7);
                                        if (num5 < num7)
                                        {
                                            return null;
                                        }
                                    }
                                    num2 += num5;
                                    goto Label_0D99;

                                case 0xda:
                                    num2 += 1L;
                                    if (stream.Read(buffer, 0, 2) != 2)
                                    {
                                        return null;
                                    }
                                    num8 = BigEndianBinary.ToUInt16(buffer, 0);
                                    num2 += 2L;
                                    num5 = 0L;
                                    while (num8 > num5)
                                    {
                                        num6 = num8 - num5;
                                        num7 = (num6 > DummyBufferForSkipping.Length) ? DummyBufferForSkipping.Length : ((int) num6);
                                        num5 += stream.Read(DummyBufferForSkipping, 0, num7);
                                        if (num5 < num7)
                                        {
                                            return null;
                                        }
                                    }
                                    num2 += num5;
                                    item -= 1L;
                                    if (stack != null)
                                    {
                                        while ((item == 0L) && (stack.Count > 0))
                                        {
                                            if (stack.Count == 0)
                                            {
                                                break;
                                            }
                                            item = stack.Pop() - 1L;
                                        }
                                    }
                                    goto Label_0D99;

                                case 0xdb:
                                    num2 += 1L;
                                    if (stream.Read(buffer, 0, 4) != 4)
                                    {
                                        return null;
                                    }
                                    num10 = BigEndianBinary.ToUInt32(buffer, 0);
                                    num2 += 4L;
                                    num5 = 0L;
                                    while (num10 > num5)
                                    {
                                        num6 = num10 - num5;
                                        num7 = (num6 > DummyBufferForSkipping.Length) ? DummyBufferForSkipping.Length : ((int) num6);
                                        num5 += stream.Read(DummyBufferForSkipping, 0, num7);
                                        if (num5 < num7)
                                        {
                                            return null;
                                        }
                                    }
                                    num2 += num5;
                                    item -= 1L;
                                    if (stack != null)
                                    {
                                        while ((item == 0L) && (stack.Count > 0))
                                        {
                                            if (stack.Count == 0)
                                            {
                                                break;
                                            }
                                            item = stack.Pop() - 1L;
                                        }
                                    }
                                    goto Label_0D99;

                                case 220:
                                    num2 += 1L;
                                    if (stream.Read(buffer, 0, 2) != 2)
                                    {
                                        return null;
                                    }
                                    num8 = BigEndianBinary.ToUInt16(buffer, 0);
                                    num2 += 2L;
                                    if (num8 == 0)
                                    {
                                        item -= 1L;
                                        if (stack != null)
                                        {
                                            while ((item == 0L) && (stack.Count > 0))
                                            {
                                                if (stack.Count == 0)
                                                {
                                                    break;
                                                }
                                                item = stack.Pop() - 1L;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item >= 0L)
                                        {
                                            if (stack == null)
                                            {
                                                stack = new Stack<long>(4);
                                            }
                                            stack.Push(item);
                                        }
                                        item = num8;
                                    }
                                    goto Label_0D99;

                                case 0xdd:
                                    num2 += 1L;
                                    if (stream.Read(buffer, 0, 4) != 4)
                                    {
                                        return null;
                                    }
                                    num10 = BigEndianBinary.ToUInt32(buffer, 0);
                                    num2 += 4L;
                                    if (num10 == 0)
                                    {
                                        item -= 1L;
                                        if (stack != null)
                                        {
                                            while ((item == 0L) && (stack.Count > 0))
                                            {
                                                if (stack.Count == 0)
                                                {
                                                    break;
                                                }
                                                item = stack.Pop() - 1L;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item >= 0L)
                                        {
                                            if (stack == null)
                                            {
                                                stack = new Stack<long>(4);
                                            }
                                            stack.Push(item);
                                        }
                                        item = num10;
                                    }
                                    goto Label_0D99;

                                case 0xde:
                                    num2 += 1L;
                                    if (stream.Read(buffer, 0, 2) != 2)
                                    {
                                        return null;
                                    }
                                    num8 = BigEndianBinary.ToUInt16(buffer, 0);
                                    num2 += 2L;
                                    if (num8 == 0)
                                    {
                                        item -= 1L;
                                        if (stack != null)
                                        {
                                            while ((item == 0L) && (stack.Count > 0))
                                            {
                                                if (stack.Count == 0)
                                                {
                                                    break;
                                                }
                                                item = stack.Pop() - 1L;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item >= 0L)
                                        {
                                            if (stack == null)
                                            {
                                                stack = new Stack<long>(4);
                                            }
                                            stack.Push(item);
                                        }
                                        item = num8 * 2;
                                    }
                                    goto Label_0D99;

                                case 0xdf:
                                    num2 += 1L;
                                    if (stream.Read(buffer, 0, 4) != 4)
                                    {
                                        return null;
                                    }
                                    num10 = BigEndianBinary.ToUInt32(buffer, 0);
                                    num2 += 4L;
                                    if (num10 == 0)
                                    {
                                        item -= 1L;
                                        if (stack != null)
                                        {
                                            while ((item == 0L) && (stack.Count > 0))
                                            {
                                                if (stack.Count == 0)
                                                {
                                                    break;
                                                }
                                                item = stack.Pop() - 1L;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (item >= 0L)
                                        {
                                            if (stack == null)
                                            {
                                                stack = new Stack<long>(4);
                                            }
                                            stack.Push(item);
                                        }
                                        item = num10 * 2;
                                    }
                                    goto Label_0D99;
                            }
                            throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Unknown header value 0x{0:X}", new object[] { num3 }));
                    }
                    break;
            }
        Label_04F3:
            num5 = 0L;
            while (1L > num5)
            {
                num6 = 1L - num5;
                num7 = (num6 > DummyBufferForSkipping.Length) ? DummyBufferForSkipping.Length : ((int) num6);
                num5 += stream.Read(DummyBufferForSkipping, 0, num7);
                if (num5 < num7)
                {
                    return null;
                }
            }
            num2 += num5;
        Label_0D99:
            if (item > 0L)
            {
                goto Label_0032;
            }
            return new long?(num2);
        }

        internal long? SkipSubtreeItem()
        {
            return this.SkipCore();
        }

        public override MessagePackObject? Data
        {
            get
            {
                return this._data;
            }
            protected set
            {
                this._data = value;
            }
        }

        public override bool IsArrayHeader
        {
            get
            {
                return (this._collectionType == CollectionType.Array);
            }
        }

        public override bool IsMapHeader
        {
            get
            {
                return (this._collectionType == CollectionType.Map);
            }
        }

        public override long ItemsCount
        {
            get
            {
                return this._itemsCount;
            }
        }

        protected sealed override Stream UnderlyingStream
        {
            get
            {
                return this._stream;
            }
        }

        internal override long? UnderlyingStreamPosition
        {
            get
            {
                return new long?(this._stream.Position);
            }
        }

        private enum CollectionType
        {
            None,
            Array,
            Map
        }
    }
}

