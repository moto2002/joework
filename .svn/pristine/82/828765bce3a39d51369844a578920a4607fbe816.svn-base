namespace MsgPack
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;

    [Serializable]
    public struct MessagePackObject : IEquatable<MessagePackObject>, IPackable
    {
        private static readonly ValueTypeCode _booleanTypeCode;
        private static readonly ValueTypeCode _byteTypeCode;
        private static readonly ValueTypeCode _doubleTypeCode;
        private object _handleOrTypeCode;
        private static readonly ValueTypeCode _int16TypeCode;
        private static readonly ValueTypeCode _int32TypeCode;
        private static readonly ValueTypeCode _int64TypeCode;
        private static readonly ValueTypeCode _sbyteTypeCode;
        private static readonly ValueTypeCode _singleTypeCode;
        private static readonly ValueTypeCode _uint16TypeCode;
        private static readonly ValueTypeCode _uint32TypeCode;
        private static readonly ValueTypeCode _uint64TypeCode;
        private ulong _value;
        public static readonly MessagePackObject Nil;

        static MessagePackObject()
        {
            _sbyteTypeCode = new ValueTypeCode(typeof(sbyte), MessagePackValueTypeCode.Int8);
            _byteTypeCode = new ValueTypeCode(typeof(byte), MessagePackValueTypeCode.UInt8);
            _int16TypeCode = new ValueTypeCode(typeof(short), MessagePackValueTypeCode.Int16);
            _uint16TypeCode = new ValueTypeCode(typeof(ushort), MessagePackValueTypeCode.UInt16);
            _int32TypeCode = new ValueTypeCode(typeof(int), MessagePackValueTypeCode.Int32);
            _uint32TypeCode = new ValueTypeCode(typeof(uint), MessagePackValueTypeCode.UInt32);
            _int64TypeCode = new ValueTypeCode(typeof(long), MessagePackValueTypeCode.Int64);
            _uint64TypeCode = new ValueTypeCode(typeof(ulong), MessagePackValueTypeCode.UInt64);
            _singleTypeCode = new ValueTypeCode(typeof(float), MessagePackValueTypeCode.Single);
            _doubleTypeCode = new ValueTypeCode(typeof(double), MessagePackValueTypeCode.Double);
            _booleanTypeCode = new ValueTypeCode(typeof(bool), MessagePackValueTypeCode.Boolean);
            Nil = new MessagePackObject();
        }

        public MessagePackObject(MessagePackObjectDictionary value) : this(value, false)
        {
        }

        internal MessagePackObject(MessagePackString messagePackString)
        {
            this = new MessagePackObject();
            this._handleOrTypeCode = messagePackString;
        }

        public MessagePackObject(bool value)
        {
            this = new MessagePackObject();
            this._value = value ? ((ulong) 1L) : ((ulong) 0L);
            this._handleOrTypeCode = _booleanTypeCode;
        }

        public MessagePackObject(byte value)
        {
            this = new MessagePackObject();
            this._value = value;
            this._handleOrTypeCode = _byteTypeCode;
        }

        public MessagePackObject(IList<MessagePackObject> value) : this(value, false)
        {
        }

        public MessagePackObject(double value)
        {
            this = new MessagePackObject();
            this._value = (ulong) BitConverter.DoubleToInt64Bits(value);
            this._handleOrTypeCode = _doubleTypeCode;
        }

        public MessagePackObject(short value)
        {
            this = new MessagePackObject();
            this._value = (ulong) value;
            this._handleOrTypeCode = _int16TypeCode;
        }

        public MessagePackObject(int value)
        {
            this = new MessagePackObject();
            this._value = (ulong) value;
            this._handleOrTypeCode = _int32TypeCode;
        }

        public MessagePackObject(long value)
        {
            this = new MessagePackObject();
            this._value = (ulong) value;
            this._handleOrTypeCode = _int64TypeCode;
        }

        [CLSCompliant(false)]
        public MessagePackObject(sbyte value)
        {
            this = new MessagePackObject();
            this._value = (ulong) value;
            this._handleOrTypeCode = _sbyteTypeCode;
        }

        public MessagePackObject(float value)
        {
            this = new MessagePackObject();
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                this._value |= bytes[3] << 0x18;
                this._value |= bytes[2] << 0x10;
                this._value |= bytes[1] << 8;
                this._value |= bytes[0];
            }
            else
            {
                this._value |= bytes[0] << 0x18;
                this._value |= bytes[1] << 0x10;
                this._value |= bytes[2] << 8;
                this._value |= bytes[3];
            }
            this._handleOrTypeCode = _singleTypeCode;
        }

        public MessagePackObject(string value)
        {
            this = new MessagePackObject();
            if (value == null)
            {
                this._handleOrTypeCode = null;
            }
            else
            {
                this._handleOrTypeCode = new MessagePackString(value);
            }
        }

        [CLSCompliant(false)]
        public MessagePackObject(ushort value)
        {
            this = new MessagePackObject();
            this._value = value;
            this._handleOrTypeCode = _uint16TypeCode;
        }

        [CLSCompliant(false)]
        public MessagePackObject(uint value)
        {
            this = new MessagePackObject();
            this._value = value;
            this._handleOrTypeCode = _uint32TypeCode;
        }

        [CLSCompliant(false)]
        public MessagePackObject(ulong value)
        {
            this = new MessagePackObject();
            this._value = value;
            this._handleOrTypeCode = _uint64TypeCode;
        }

        public MessagePackObject(byte[] value)
        {
            this = new MessagePackObject();
            if (value == null)
            {
                this._handleOrTypeCode = null;
            }
            else
            {
                this._handleOrTypeCode = new MessagePackString(value);
            }
        }

        public MessagePackObject(MessagePackObjectDictionary value, bool isImmutable)
        {
            this = new MessagePackObject();
            if (isImmutable)
            {
                this._handleOrTypeCode = value;
            }
            else if (value == null)
            {
                this._handleOrTypeCode = null;
            }
            else
            {
                this._handleOrTypeCode = new MessagePackObjectDictionary(value);
            }
        }

        public MessagePackObject(IList<MessagePackObject> value, bool isImmutable)
        {
            this = new MessagePackObject();
            if (isImmutable)
            {
                this._handleOrTypeCode = value;
            }
            else if (value == null)
            {
                this._handleOrTypeCode = null;
            }
            else
            {
                MessagePackObject[] array = new MessagePackObject[value.Count];
                value.CopyTo(array, 0);
                this._handleOrTypeCode = array;
            }
        }

        public byte[] AsBinary()
        {
            VerifyUnderlyingType<MessagePackString>(this, null);
            if (this._handleOrTypeCode == null)
            {
                return null;
            }
            MessagePackString str = this._handleOrTypeCode as MessagePackString;
            Contract.Assert(str != null);
            return str.GetBytes();
        }

        public bool AsBoolean()
        {
            if (this.IsNil)
            {
                ThrowCannotBeNilAs<bool>();
            }
            ValueTypeCode code = this._handleOrTypeCode as ValueTypeCode;
            if ((code == null) || (code.TypeCode != MessagePackValueTypeCode.Boolean))
            {
                ThrowInvalidTypeAs<bool>(this);
            }
            return (this._value != 0L);
        }

        public byte AsByte()
        {
            if (this.IsNil)
            {
                ThrowCannotBeNilAs<byte>();
            }
            ValueTypeCode code = this._handleOrTypeCode as ValueTypeCode;
            if (code == null)
            {
                ThrowInvalidTypeAs<byte>(this);
            }
            if (code.IsInteger)
            {
                if (code.IsSigned)
                {
                    long num = (long) this._value;
                    if ((num < 0L) || (0xffL < num))
                    {
                        ThrowInvalidTypeAs<byte>(this);
                    }
                    return (byte) num;
                }
                if (0xffL < this._value)
                {
                    ThrowInvalidTypeAs<byte>(this);
                }
                return (byte) this._value;
            }
            if (code.TypeCode == MessagePackValueTypeCode.Double)
            {
                return (byte) BitConverter.Int64BitsToDouble((long) this._value);
            }
            if (code.TypeCode == MessagePackValueTypeCode.Single)
            {
                return (byte) BitConverter.ToSingle(BitConverter.GetBytes(this._value), 0);
            }
            ThrowInvalidTypeAs<byte>(this);
            return 0;
        }

        public char[] AsCharArray()
        {
            return this.AsString().ToCharArray();
        }

        public MessagePackObjectDictionary AsDictionary()
        {
            VerifyUnderlyingType<MessagePackObjectDictionary>(this, null);
            return (this._handleOrTypeCode as MessagePackObjectDictionary);
        }

        public double AsDouble()
        {
            if (this.IsNil)
            {
                ThrowCannotBeNilAs<double>();
            }
            ValueTypeCode code = this._handleOrTypeCode as ValueTypeCode;
            if (code == null)
            {
                ThrowInvalidTypeAs<double>(this);
            }
            if (code.IsInteger)
            {
                if (code.IsSigned)
                {
                    return (double) this._value;
                }
                return (double) this._value;
            }
            if (code.TypeCode == MessagePackValueTypeCode.Double)
            {
                return BitConverter.Int64BitsToDouble((long) this._value);
            }
            if (code.TypeCode == MessagePackValueTypeCode.Single)
            {
                return (double) BitConverter.ToSingle(BitConverter.GetBytes(this._value), 0);
            }
            ThrowInvalidTypeAs<double>(this);
            return 0.0;
        }

        public IEnumerable<MessagePackObject> AsEnumerable()
        {
            if (this.IsNil)
            {
                return null;
            }
            VerifyUnderlyingType<IEnumerable<MessagePackObject>>(this, null);
            return (this._handleOrTypeCode as IEnumerable<MessagePackObject>);
        }

        public short AsInt16()
        {
            if (this.IsNil)
            {
                ThrowCannotBeNilAs<short>();
            }
            ValueTypeCode code = this._handleOrTypeCode as ValueTypeCode;
            if (code == null)
            {
                ThrowInvalidTypeAs<short>(this);
            }
            if (code.IsInteger)
            {
                if (code.IsSigned)
                {
                    long num = (long) this._value;
                    if ((num < -32768L) || (0x7fffL < num))
                    {
                        ThrowInvalidTypeAs<short>(this);
                    }
                    return (short) num;
                }
                if (0x7fffL < this._value)
                {
                    ThrowInvalidTypeAs<short>(this);
                }
                return (short) this._value;
            }
            if (code.TypeCode == MessagePackValueTypeCode.Double)
            {
                return (short) BitConverter.Int64BitsToDouble((long) this._value);
            }
            if (code.TypeCode == MessagePackValueTypeCode.Single)
            {
                return (short) BitConverter.ToSingle(BitConverter.GetBytes(this._value), 0);
            }
            ThrowInvalidTypeAs<short>(this);
            return 0;
        }

        public int AsInt32()
        {
            if (this.IsNil)
            {
                ThrowCannotBeNilAs<int>();
            }
            ValueTypeCode code = this._handleOrTypeCode as ValueTypeCode;
            if (code == null)
            {
                ThrowInvalidTypeAs<int>(this);
            }
            if (code.IsInteger)
            {
                if (code.IsSigned)
                {
                    long num = (long) this._value;
                    if ((num < -2147483648L) || (0x7fffffffL < num))
                    {
                        ThrowInvalidTypeAs<int>(this);
                    }
                    return (int) num;
                }
                if (0x7fffffffL < this._value)
                {
                    ThrowInvalidTypeAs<int>(this);
                }
                return (int) this._value;
            }
            if (code.TypeCode == MessagePackValueTypeCode.Double)
            {
                return (int) BitConverter.Int64BitsToDouble((long) this._value);
            }
            if (code.TypeCode == MessagePackValueTypeCode.Single)
            {
                return (int) BitConverter.ToSingle(BitConverter.GetBytes(this._value), 0);
            }
            ThrowInvalidTypeAs<int>(this);
            return 0;
        }

        public long AsInt64()
        {
            if (this.IsNil)
            {
                ThrowCannotBeNilAs<long>();
            }
            ValueTypeCode code = this._handleOrTypeCode as ValueTypeCode;
            if (code == null)
            {
                ThrowInvalidTypeAs<long>(this);
            }
            if (code.IsInteger)
            {
                if (!code.IsSigned && (0x7fffffffffffffffL < this._value))
                {
                    ThrowInvalidTypeAs<long>(this);
                }
                return (long) this._value;
            }
            if (code.TypeCode == MessagePackValueTypeCode.Double)
            {
                return (long) BitConverter.Int64BitsToDouble((long) this._value);
            }
            if (code.TypeCode == MessagePackValueTypeCode.Single)
            {
                return (long) BitConverter.ToSingle(BitConverter.GetBytes(this._value), 0);
            }
            ThrowInvalidTypeAs<long>(this);
            return 0L;
        }

        public IList<MessagePackObject> AsList()
        {
            if (this.IsNil)
            {
                return null;
            }
            VerifyUnderlyingType<IList<MessagePackObject>>(this, null);
            return (this._handleOrTypeCode as IList<MessagePackObject>);
        }

        [CLSCompliant(false)]
        public sbyte AsSByte()
        {
            if (this.IsNil)
            {
                ThrowCannotBeNilAs<sbyte>();
            }
            ValueTypeCode code = this._handleOrTypeCode as ValueTypeCode;
            if (code == null)
            {
                ThrowInvalidTypeAs<sbyte>(this);
            }
            if (code.IsInteger)
            {
                if (code.IsSigned)
                {
                    long num = (long) this._value;
                    if ((num < -128L) || (0x7fL < num))
                    {
                        ThrowInvalidTypeAs<sbyte>(this);
                    }
                    return (sbyte) num;
                }
                if (0x7fL < this._value)
                {
                    ThrowInvalidTypeAs<sbyte>(this);
                }
                return (sbyte) this._value;
            }
            if (code.TypeCode == MessagePackValueTypeCode.Double)
            {
                return (sbyte) BitConverter.Int64BitsToDouble((long) this._value);
            }
            if (code.TypeCode == MessagePackValueTypeCode.Single)
            {
                return (sbyte) BitConverter.ToSingle(BitConverter.GetBytes(this._value), 0);
            }
            ThrowInvalidTypeAs<sbyte>(this);
            return 0;
        }

        public float AsSingle()
        {
            if (this.IsNil)
            {
                ThrowCannotBeNilAs<float>();
            }
            ValueTypeCode code = this._handleOrTypeCode as ValueTypeCode;
            if (code == null)
            {
                ThrowInvalidTypeAs<float>(this);
            }
            if (code.IsInteger)
            {
                if (code.IsSigned)
                {
                    return (float) this._value;
                }
                return (float) this._value;
            }
            if (code.TypeCode == MessagePackValueTypeCode.Double)
            {
                return (float) BitConverter.Int64BitsToDouble((long) this._value);
            }
            if (code.TypeCode == MessagePackValueTypeCode.Single)
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(this._value), 0);
            }
            ThrowInvalidTypeAs<float>(this);
            return 0f;
        }

        public string AsString()
        {
            VerifyUnderlyingType<MessagePackString>(this, null);
            if (this._handleOrTypeCode == null)
            {
                return null;
            }
            MessagePackString str = this._handleOrTypeCode as MessagePackString;
            Contract.Assert(str != null);
            return str.GetString();
        }

        public string AsString(Encoding encoding)
        {
            string str2;
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            if (this.IsNil)
            {
                return null;
            }
            VerifyUnderlyingType<MessagePackString>(this, null);
            try
            {
                MessagePackString str = this._handleOrTypeCode as MessagePackString;
                Contract.Assert(str != null);
                if (encoding is UTF8Encoding)
                {
                    return str.GetString();
                }
                str2 = encoding.GetString(str.UnsafeGetBuffer(), 0, str.UnsafeGetBuffer().Length);
            }
            catch (ArgumentException exception)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Not '{0}' string.", new object[] { encoding.WebName }), exception);
            }
            return str2;
        }

        public string AsStringUtf16()
        {
            string str2;
            VerifyUnderlyingType<byte[]>(this, null);
            if (this.IsNil)
            {
                return null;
            }
            try
            {
                MessagePackString str = this._handleOrTypeCode as MessagePackString;
                Contract.Assert(str != null);
                if (str.UnsafeGetString() != null)
                {
                    return str.UnsafeGetString();
                }
                byte[] bytes = str.UnsafeGetBuffer();
                if (bytes.Length == 0)
                {
                    return string.Empty;
                }
                if ((bytes.Length % 2) != 0)
                {
                    throw new InvalidOperationException("Not UTF-16 string.");
                }
                if ((bytes[0] == 0xff) && (bytes[1] == 0xfe))
                {
                    return Encoding.Unicode.GetString(bytes, 2, bytes.Length - 2);
                }
                if ((bytes[0] == 0xfe) && (bytes[1] == 0xff))
                {
                    return Encoding.BigEndianUnicode.GetString(bytes, 2, bytes.Length - 2);
                }
                str2 = Encoding.BigEndianUnicode.GetString(bytes, 0, bytes.Length);
            }
            catch (ArgumentException exception)
            {
                throw new InvalidOperationException("Not UTF-16 string.", exception);
            }
            return str2;
        }

        public string AsStringUtf8()
        {
            return this.AsString(MessagePackConvert.Utf8NonBomStrict);
        }

        [CLSCompliant(false)]
        public ushort AsUInt16()
        {
            if (this.IsNil)
            {
                ThrowCannotBeNilAs<ushort>();
            }
            ValueTypeCode code = this._handleOrTypeCode as ValueTypeCode;
            if (code == null)
            {
                ThrowInvalidTypeAs<ushort>(this);
            }
            if (code.IsInteger)
            {
                if (0xffffL < this._value)
                {
                    ThrowInvalidTypeAs<ushort>(this);
                }
                return (ushort) this._value;
            }
            if (code.TypeCode == MessagePackValueTypeCode.Double)
            {
                return (ushort) BitConverter.Int64BitsToDouble((long) this._value);
            }
            if (code.TypeCode == MessagePackValueTypeCode.Single)
            {
                return (ushort) BitConverter.ToSingle(BitConverter.GetBytes(this._value), 0);
            }
            ThrowInvalidTypeAs<ushort>(this);
            return 0;
        }

        [CLSCompliant(false)]
        public uint AsUInt32()
        {
            if (this.IsNil)
            {
                ThrowCannotBeNilAs<uint>();
            }
            ValueTypeCode code = this._handleOrTypeCode as ValueTypeCode;
            if (code == null)
            {
                ThrowInvalidTypeAs<uint>(this);
            }
            if (code.IsInteger)
            {
                if (0xffffffffL < this._value)
                {
                    ThrowInvalidTypeAs<uint>(this);
                }
                return (uint) this._value;
            }
            if (code.TypeCode == MessagePackValueTypeCode.Double)
            {
                return (uint) BitConverter.Int64BitsToDouble((long) this._value);
            }
            if (code.TypeCode == MessagePackValueTypeCode.Single)
            {
                return (uint) BitConverter.ToSingle(BitConverter.GetBytes(this._value), 0);
            }
            ThrowInvalidTypeAs<uint>(this);
            return 0;
        }

        [CLSCompliant(false)]
        public ulong AsUInt64()
        {
            if (this.IsNil)
            {
                ThrowCannotBeNilAs<ulong>();
            }
            ValueTypeCode code = this._handleOrTypeCode as ValueTypeCode;
            if (code == null)
            {
                ThrowInvalidTypeAs<ulong>(this);
            }
            if (code.IsInteger)
            {
                if (code.IsSigned && (0x7fffffffffffffffL < this._value))
                {
                    ThrowInvalidTypeAs<ulong>(this);
                }
                return this._value;
            }
            if (code.TypeCode == MessagePackValueTypeCode.Double)
            {
                return (ulong) BitConverter.Int64BitsToDouble((long) this._value);
            }
            if (code.TypeCode == MessagePackValueTypeCode.Single)
            {
                return (ulong) BitConverter.ToSingle(BitConverter.GetBytes(this._value), 0);
            }
            ThrowInvalidTypeAs<ulong>(this);
            return 0L;
        }

        public bool Equals(MessagePackObject other)
        {
            if (this._handleOrTypeCode == null)
            {
                return (other._handleOrTypeCode == null);
            }
            if (other._handleOrTypeCode == null)
            {
                return false;
            }
            ValueTypeCode leftTypeCode = this._handleOrTypeCode as ValueTypeCode;
            if (leftTypeCode != null)
            {
                ValueTypeCode rightTypeCode = other._handleOrTypeCode as ValueTypeCode;
                if (rightTypeCode == null)
                {
                    return false;
                }
                if (leftTypeCode.TypeCode == MessagePackValueTypeCode.Boolean)
                {
                    if (rightTypeCode.TypeCode != MessagePackValueTypeCode.Boolean)
                    {
                        return false;
                    }
                    return (((bool) this) == ((bool) other));
                }
                if (rightTypeCode.TypeCode == MessagePackValueTypeCode.Boolean)
                {
                    return false;
                }
                if (leftTypeCode.IsInteger)
                {
                    if (rightTypeCode.IsInteger)
                    {
                        return IntegerIntegerEquals(this._value, leftTypeCode, other._value, rightTypeCode);
                    }
                    if (rightTypeCode.TypeCode == MessagePackValueTypeCode.Single)
                    {
                        return IntegerSingleEquals(this, other);
                    }
                    if (rightTypeCode.TypeCode == MessagePackValueTypeCode.Double)
                    {
                        return IntegerDoubleEquals(this, other);
                    }
                }
                else if (leftTypeCode.TypeCode == MessagePackValueTypeCode.Double)
                {
                    if (rightTypeCode.IsInteger)
                    {
                        return IntegerDoubleEquals(other, this);
                    }
                    if (rightTypeCode.TypeCode == MessagePackValueTypeCode.Single)
                    {
                        return (((double) this) == ((float) other));
                    }
                    if (rightTypeCode.TypeCode == MessagePackValueTypeCode.Double)
                    {
                        return (((double) this) == ((double) other));
                    }
                }
                else if (leftTypeCode.TypeCode == MessagePackValueTypeCode.Single)
                {
                    if (rightTypeCode.IsInteger)
                    {
                        return IntegerSingleEquals(other, this);
                    }
                    if (rightTypeCode.TypeCode == MessagePackValueTypeCode.Single)
                    {
                        return (((float) this) == ((float) other));
                    }
                    if (rightTypeCode.TypeCode == MessagePackValueTypeCode.Double)
                    {
                        return (((float) this) == ((double) other));
                    }
                }
            }
            MessagePackString str = this._handleOrTypeCode as MessagePackString;
            if (str != null)
            {
                return str.Equals(other._handleOrTypeCode as MessagePackString);
            }
            IList<MessagePackObject> first = this._handleOrTypeCode as IList<MessagePackObject>;
            if (first != null)
            {
                IList<MessagePackObject> second = other._handleOrTypeCode as IList<MessagePackObject>;
                if (second == null)
                {
                    return false;
                }
                return first.SequenceEqual<MessagePackObject>(second);
            }
            IDictionary<MessagePackObject, MessagePackObject> dictionary = this._handleOrTypeCode as IDictionary<MessagePackObject, MessagePackObject>;
            if (dictionary != null)
            {
                IDictionary<MessagePackObject, MessagePackObject> dictionary2 = other._handleOrTypeCode as IDictionary<MessagePackObject, MessagePackObject>;
                if (dictionary2 == null)
                {
                    return false;
                }
                if (dictionary.Count != dictionary2.Count)
                {
                    return false;
                }
                foreach (KeyValuePair<MessagePackObject, MessagePackObject> pair in dictionary)
                {
                    MessagePackObject obj2;
                    if (!dictionary2.TryGetValue(pair.Key, out obj2))
                    {
                        return false;
                    }
                    if (pair.Value != obj2)
                    {
                        return false;
                    }
                }
                return true;
            }
            Debug.Assert(false, string.Format("Unknown handle type this:'{0}'(value: '{1}'), other:'{2}'(value: '{3}')", new object[] { this._handleOrTypeCode.GetType(), this._handleOrTypeCode, other._handleOrTypeCode.GetType(), other._handleOrTypeCode }));
            return this._handleOrTypeCode.Equals(other._handleOrTypeCode);
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null))
            {
                return this.IsNil;
            }
            MessagePackObjectDictionary dictionary = obj as MessagePackObjectDictionary;
            if (dictionary != null)
            {
                return this.Equals(new MessagePackObject(dictionary));
            }
            return ((obj is MessagePackObject) && this.Equals((MessagePackObject) obj));
        }

        public static MessagePackObject FromObject(object boxedValue)
        {
            if (boxedValue == null)
            {
                return Nil;
            }
            if (boxedValue is MessagePackObject)
            {
                return (MessagePackObject) boxedValue;
            }
            if (boxedValue is sbyte)
            {
                return (sbyte) boxedValue;
            }
            if (boxedValue is byte)
            {
                return (byte) boxedValue;
            }
            if (boxedValue is short)
            {
                return (short) boxedValue;
            }
            if (boxedValue is ushort)
            {
                return (ushort) boxedValue;
            }
            if (boxedValue is int)
            {
                return (int) boxedValue;
            }
            if (boxedValue is uint)
            {
                return (uint) boxedValue;
            }
            if (boxedValue is long)
            {
                return (long) boxedValue;
            }
            if (boxedValue is ulong)
            {
                return (ulong) boxedValue;
            }
            if (boxedValue is float)
            {
                return (float) boxedValue;
            }
            if (boxedValue is double)
            {
                return (double) boxedValue;
            }
            if (boxedValue is bool)
            {
                return (bool) boxedValue;
            }
            byte[] buffer = boxedValue as byte[];
            if (buffer != null)
            {
                return new MessagePackObject(buffer);
            }
            string str = boxedValue as string;
            if (str != null)
            {
                return new MessagePackObject(str);
            }
            IEnumerable<byte> source = boxedValue as IEnumerable<byte>;
            if (source != null)
            {
                return new MessagePackObject(source.ToArray<byte>());
            }
            IEnumerable<char> enumerable2 = boxedValue as IEnumerable<char>;
            if (enumerable2 != null)
            {
                return new MessagePackObject(new string(enumerable2.ToArray<char>()));
            }
            IEnumerable<MessagePackObject> enumerable3 = boxedValue as IEnumerable<MessagePackObject>;
            if (enumerable3 != null)
            {
                IList<MessagePackObject> list = boxedValue as IList<MessagePackObject>;
                if (list != null)
                {
                    return new MessagePackObject(list, false);
                }
                return new MessagePackObject(enumerable3.ToArray<MessagePackObject>(), true);
            }
            MessagePackObjectDictionary dictionary = boxedValue as MessagePackObjectDictionary;
            if (dictionary == null)
            {
                throw new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Type '{0}' is not supported.", new object[] { boxedValue.GetType() }));
            }
            return new MessagePackObject(dictionary, false);
        }

        public override int GetHashCode()
        {
            if (this._handleOrTypeCode != null)
            {
                if (this._handleOrTypeCode is ValueTypeCode)
                {
                    return this._value.GetHashCode();
                }
                IList<MessagePackObject> source = this._handleOrTypeCode as IList<MessagePackObject>;
                if (source != null)
                {
                    return source.Aggregate<MessagePackObject, int>(0, (hash, item) => (hash ^ item.GetHashCode()));
                }
                IDictionary<MessagePackObject, MessagePackObject> dictionary = this._handleOrTypeCode as IDictionary<MessagePackObject, MessagePackObject>;
                if (dictionary != null)
                {
                    return dictionary.Aggregate<KeyValuePair<MessagePackObject, MessagePackObject>, int>(0, (hash, item) => (hash ^ item.GetHashCode()));
                }
                MessagePackString str = this._handleOrTypeCode as MessagePackString;
                if (str != null)
                {
                    return str.GetHashCode();
                }
                Contract.Assert(false, string.Format("(this._handleOrTypeCode is string) but {0}", this._handleOrTypeCode.GetType()));
            }
            return 0;
        }

        private static bool IntegerDoubleEquals(MessagePackObject integer, MessagePackObject real)
        {
            if ((integer._handleOrTypeCode as ValueTypeCode).IsSigned)
            {
                return (integer._value == ((double) real));
            }
            return (integer._value == ((double) real));
        }

        private static bool IntegerIntegerEquals(ulong left, ValueTypeCode leftTypeCode, ulong right, ValueTypeCode rightTypeCode)
        {
            if (leftTypeCode.IsSigned)
            {
                if (!rightTypeCode.IsSigned)
                {
                    long num = (long) left;
                    if (num < 0L)
                    {
                        return false;
                    }
                }
                return (left == right);
            }
            if (rightTypeCode.IsSigned)
            {
                long num2 = (long) right;
                if (num2 < 0L)
                {
                    return false;
                }
                return (left == right);
            }
            return (left == right);
        }

        private static bool IntegerSingleEquals(MessagePackObject integer, MessagePackObject real)
        {
            if ((integer._handleOrTypeCode as ValueTypeCode).IsSigned)
            {
                return (integer._value == ((float) real));
            }
            return (integer._value == ((float) real));
        }

        public bool? IsTypeOf<T>()
        {
            return this.IsTypeOf(typeof(T));
        }

        public bool? IsTypeOf(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (this._handleOrTypeCode == null)
            {
                return ((type.IsValueType && (Nullable.GetUnderlyingType(type) == null)) ? ((bool?) false) : null);
            }
            ValueTypeCode code = this._handleOrTypeCode as ValueTypeCode;
            if (code == null)
            {
                if (((type == typeof(string)) || (type == typeof(IList<char>))) || (type == typeof(IEnumerable<char>)))
                {
                    MessagePackString str = this._handleOrTypeCode as MessagePackString;
                    return new bool?((str != null) && (str.GetUnderlyingType() == typeof(string)));
                }
                if (((type == typeof(byte[])) || (type == typeof(IList<byte>))) || (type == typeof(IEnumerable<byte>)))
                {
                    return new bool?(this._handleOrTypeCode is MessagePackString);
                }
                if (typeof(IEnumerable<MessagePackObject>).IsAssignableFrom(type) && (this._handleOrTypeCode is MessagePackString))
                {
                    return false;
                }
                return new bool?(type.IsAssignableFrom(this._handleOrTypeCode.GetType()));
            }
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return new bool?(code.IsInteger && ((this._value < 0x80L) || ((18446744073709551488L <= this._value) && code.IsSigned)));

                case TypeCode.Byte:
                    return new bool?(code.IsInteger && (this._value < 0x100L));

                case TypeCode.Int16:
                    return new bool?(code.IsInteger && ((this._value < 0x8000L) || ((18446744073709518848L <= this._value) && code.IsSigned)));

                case TypeCode.UInt16:
                    return new bool?(code.IsInteger && (this._value < 0x10000L));

                case TypeCode.Int32:
                    return new bool?(code.IsInteger && ((this._value < 0x80000000L) || ((18446744071562067968L <= this._value) && code.IsSigned)));

                case TypeCode.UInt32:
                    return new bool?(code.IsInteger && (this._value < 0x100000000L));

                case TypeCode.Int64:
                    return new bool?(code.IsInteger && ((this._value < 9223372036854775808L) || code.IsSigned));

                case TypeCode.UInt64:
                    return new bool?(code.IsInteger && ((this._value < 9223372036854775808L) || !code.IsSigned));

                case TypeCode.Double:
                    return new bool?((code.Type == typeof(float)) || (code.Type == typeof(double)));
            }
            return new bool?(code.Type == type);
        }

        public static bool operator ==(MessagePackObject left, MessagePackObject right)
        {
            return left.Equals(right);
        }

        public static explicit operator bool(MessagePackObject value)
        {
            if (value.IsNil)
            {
                ThrowCannotBeNilAs<bool>();
            }
            ValueTypeCode code = value._handleOrTypeCode as ValueTypeCode;
            if ((code == null) || (code.TypeCode != MessagePackValueTypeCode.Boolean))
            {
                ThrowInvalidTypeAs<bool>(value);
            }
            return (value._value != 0L);
        }

        public static explicit operator byte(MessagePackObject value)
        {
            if (value.IsNil)
            {
                ThrowCannotBeNilAs<byte>();
            }
            ValueTypeCode code = value._handleOrTypeCode as ValueTypeCode;
            if (code == null)
            {
                ThrowInvalidTypeAs<byte>(value);
            }
            if (code.IsInteger)
            {
                if (code.IsSigned)
                {
                    long num = (long) value._value;
                    if ((num < 0L) || (0xffL < num))
                    {
                        ThrowInvalidTypeAs<byte>(value);
                    }
                    return (byte) num;
                }
                if (0xffL < value._value)
                {
                    ThrowInvalidTypeAs<byte>(value);
                }
                return (byte) value._value;
            }
            if (code.TypeCode == MessagePackValueTypeCode.Double)
            {
                return (byte) BitConverter.Int64BitsToDouble((long) value._value);
            }
            if (code.TypeCode == MessagePackValueTypeCode.Single)
            {
                return (byte) BitConverter.ToSingle(BitConverter.GetBytes(value._value), 0);
            }
            ThrowInvalidTypeAs<byte>(value);
            return 0;
        }

        public static explicit operator double(MessagePackObject value)
        {
            if (value.IsNil)
            {
                ThrowCannotBeNilAs<double>();
            }
            ValueTypeCode code = value._handleOrTypeCode as ValueTypeCode;
            if (code == null)
            {
                ThrowInvalidTypeAs<double>(value);
            }
            if (code.IsInteger)
            {
                if (code.IsSigned)
                {
                    return (double) value._value;
                }
                return (double) value._value;
            }
            if (code.TypeCode == MessagePackValueTypeCode.Double)
            {
                return BitConverter.Int64BitsToDouble((long) value._value);
            }
            if (code.TypeCode == MessagePackValueTypeCode.Single)
            {
                return (double) BitConverter.ToSingle(BitConverter.GetBytes(value._value), 0);
            }
            ThrowInvalidTypeAs<double>(value);
            return 0.0;
        }

        public static explicit operator short(MessagePackObject value)
        {
            if (value.IsNil)
            {
                ThrowCannotBeNilAs<short>();
            }
            ValueTypeCode code = value._handleOrTypeCode as ValueTypeCode;
            if (code == null)
            {
                ThrowInvalidTypeAs<short>(value);
            }
            if (code.IsInteger)
            {
                if (code.IsSigned)
                {
                    long num = (long) value._value;
                    if ((num < -32768L) || (0x7fffL < num))
                    {
                        ThrowInvalidTypeAs<short>(value);
                    }
                    return (short) num;
                }
                if (0x7fffL < value._value)
                {
                    ThrowInvalidTypeAs<short>(value);
                }
                return (short) value._value;
            }
            if (code.TypeCode == MessagePackValueTypeCode.Double)
            {
                return (short) BitConverter.Int64BitsToDouble((long) value._value);
            }
            if (code.TypeCode == MessagePackValueTypeCode.Single)
            {
                return (short) BitConverter.ToSingle(BitConverter.GetBytes(value._value), 0);
            }
            ThrowInvalidTypeAs<short>(value);
            return 0;
        }

        public static explicit operator int(MessagePackObject value)
        {
            if (value.IsNil)
            {
                ThrowCannotBeNilAs<int>();
            }
            ValueTypeCode code = value._handleOrTypeCode as ValueTypeCode;
            if (code == null)
            {
                ThrowInvalidTypeAs<int>(value);
            }
            if (code.IsInteger)
            {
                if (code.IsSigned)
                {
                    long num = (long) value._value;
                    if ((num < -2147483648L) || (0x7fffffffL < num))
                    {
                        ThrowInvalidTypeAs<int>(value);
                    }
                    return (int) num;
                }
                if (0x7fffffffL < value._value)
                {
                    ThrowInvalidTypeAs<int>(value);
                }
                return (int) value._value;
            }
            if (code.TypeCode == MessagePackValueTypeCode.Double)
            {
                return (int) BitConverter.Int64BitsToDouble((long) value._value);
            }
            if (code.TypeCode == MessagePackValueTypeCode.Single)
            {
                return (int) BitConverter.ToSingle(BitConverter.GetBytes(value._value), 0);
            }
            ThrowInvalidTypeAs<int>(value);
            return 0;
        }

        public static explicit operator long(MessagePackObject value)
        {
            if (value.IsNil)
            {
                ThrowCannotBeNilAs<long>();
            }
            ValueTypeCode code = value._handleOrTypeCode as ValueTypeCode;
            if (code == null)
            {
                ThrowInvalidTypeAs<long>(value);
            }
            if (code.IsInteger)
            {
                if (!code.IsSigned && (0x7fffffffffffffffL < value._value))
                {
                    ThrowInvalidTypeAs<long>(value);
                }
                return (long) value._value;
            }
            if (code.TypeCode == MessagePackValueTypeCode.Double)
            {
                return (long) BitConverter.Int64BitsToDouble((long) value._value);
            }
            if (code.TypeCode == MessagePackValueTypeCode.Single)
            {
                return (long) BitConverter.ToSingle(BitConverter.GetBytes(value._value), 0);
            }
            ThrowInvalidTypeAs<long>(value);
            return 0L;
        }

        [CLSCompliant(false)]
        public static explicit operator sbyte(MessagePackObject value)
        {
            if (value.IsNil)
            {
                ThrowCannotBeNilAs<sbyte>();
            }
            ValueTypeCode code = value._handleOrTypeCode as ValueTypeCode;
            if (code == null)
            {
                ThrowInvalidTypeAs<sbyte>(value);
            }
            if (code.IsInteger)
            {
                if (code.IsSigned)
                {
                    long num = (long) value._value;
                    if ((num < -128L) || (0x7fL < num))
                    {
                        ThrowInvalidTypeAs<sbyte>(value);
                    }
                    return (sbyte) num;
                }
                if (0x7fL < value._value)
                {
                    ThrowInvalidTypeAs<sbyte>(value);
                }
                return (sbyte) value._value;
            }
            if (code.TypeCode == MessagePackValueTypeCode.Double)
            {
                return (sbyte) BitConverter.Int64BitsToDouble((long) value._value);
            }
            if (code.TypeCode == MessagePackValueTypeCode.Single)
            {
                return (sbyte) BitConverter.ToSingle(BitConverter.GetBytes(value._value), 0);
            }
            ThrowInvalidTypeAs<sbyte>(value);
            return 0;
        }

        public static explicit operator float(MessagePackObject value)
        {
            if (value.IsNil)
            {
                ThrowCannotBeNilAs<float>();
            }
            ValueTypeCode code = value._handleOrTypeCode as ValueTypeCode;
            if (code == null)
            {
                ThrowInvalidTypeAs<float>(value);
            }
            if (code.IsInteger)
            {
                if (code.IsSigned)
                {
                    return (float) value._value;
                }
                return (float) value._value;
            }
            if (code.TypeCode == MessagePackValueTypeCode.Double)
            {
                return (float) BitConverter.Int64BitsToDouble((long) value._value);
            }
            if (code.TypeCode == MessagePackValueTypeCode.Single)
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(value._value), 0);
            }
            ThrowInvalidTypeAs<float>(value);
            return 0f;
        }

        public static explicit operator string(MessagePackObject value)
        {
            VerifyUnderlyingType<MessagePackString>(value, "value");
            if (value._handleOrTypeCode == null)
            {
                return null;
            }
            MessagePackString str = value._handleOrTypeCode as MessagePackString;
            Contract.Assert(str != null);
            return str.GetString();
        }

        [CLSCompliant(false)]
        public static explicit operator ushort(MessagePackObject value)
        {
            if (value.IsNil)
            {
                ThrowCannotBeNilAs<ushort>();
            }
            ValueTypeCode code = value._handleOrTypeCode as ValueTypeCode;
            if (code == null)
            {
                ThrowInvalidTypeAs<ushort>(value);
            }
            if (code.IsInteger)
            {
                if (0xffffL < value._value)
                {
                    ThrowInvalidTypeAs<ushort>(value);
                }
                return (ushort) value._value;
            }
            if (code.TypeCode == MessagePackValueTypeCode.Double)
            {
                return (ushort) BitConverter.Int64BitsToDouble((long) value._value);
            }
            if (code.TypeCode == MessagePackValueTypeCode.Single)
            {
                return (ushort) BitConverter.ToSingle(BitConverter.GetBytes(value._value), 0);
            }
            ThrowInvalidTypeAs<ushort>(value);
            return 0;
        }

        [CLSCompliant(false)]
        public static explicit operator uint(MessagePackObject value)
        {
            if (value.IsNil)
            {
                ThrowCannotBeNilAs<uint>();
            }
            ValueTypeCode code = value._handleOrTypeCode as ValueTypeCode;
            if (code == null)
            {
                ThrowInvalidTypeAs<uint>(value);
            }
            if (code.IsInteger)
            {
                if (0xffffffffL < value._value)
                {
                    ThrowInvalidTypeAs<uint>(value);
                }
                return (uint) value._value;
            }
            if (code.TypeCode == MessagePackValueTypeCode.Double)
            {
                return (uint) BitConverter.Int64BitsToDouble((long) value._value);
            }
            if (code.TypeCode == MessagePackValueTypeCode.Single)
            {
                return (uint) BitConverter.ToSingle(BitConverter.GetBytes(value._value), 0);
            }
            ThrowInvalidTypeAs<uint>(value);
            return 0;
        }

        [CLSCompliant(false)]
        public static explicit operator ulong(MessagePackObject value)
        {
            if (value.IsNil)
            {
                ThrowCannotBeNilAs<ulong>();
            }
            ValueTypeCode code = value._handleOrTypeCode as ValueTypeCode;
            if (code == null)
            {
                ThrowInvalidTypeAs<ulong>(value);
            }
            if (code.IsInteger)
            {
                if (code.IsSigned && (0x7fffffffffffffffL < value._value))
                {
                    ThrowInvalidTypeAs<ulong>(value);
                }
                return value._value;
            }
            if (code.TypeCode == MessagePackValueTypeCode.Double)
            {
                return (ulong) BitConverter.Int64BitsToDouble((long) value._value);
            }
            if (code.TypeCode == MessagePackValueTypeCode.Single)
            {
                return (ulong) BitConverter.ToSingle(BitConverter.GetBytes(value._value), 0);
            }
            ThrowInvalidTypeAs<ulong>(value);
            return 0L;
        }

        public static explicit operator byte[](MessagePackObject value)
        {
            VerifyUnderlyingType<MessagePackString>(value, "value");
            if (value._handleOrTypeCode == null)
            {
                return null;
            }
            MessagePackString str = value._handleOrTypeCode as MessagePackString;
            Contract.Assert(str != null);
            return str.GetBytes();
        }

        public static implicit operator MessagePackObject(bool value)
        {
            return new MessagePackObject { _value = value ? ((ulong) 1L) : ((ulong) 0L), _handleOrTypeCode = _booleanTypeCode };
        }

        public static implicit operator MessagePackObject(byte value)
        {
            return new MessagePackObject { _value = value, _handleOrTypeCode = _byteTypeCode };
        }

        public static implicit operator MessagePackObject(double value)
        {
            return new MessagePackObject { _value = (ulong) BitConverter.DoubleToInt64Bits(value), _handleOrTypeCode = _doubleTypeCode };
        }

        public static implicit operator MessagePackObject(short value)
        {
            return new MessagePackObject { _value = (ulong) value, _handleOrTypeCode = _int16TypeCode };
        }

        public static implicit operator MessagePackObject(int value)
        {
            return new MessagePackObject { _value = (ulong) value, _handleOrTypeCode = _int32TypeCode };
        }

        public static implicit operator MessagePackObject(long value)
        {
            return new MessagePackObject { _value = (ulong) value, _handleOrTypeCode = _int64TypeCode };
        }

        [CLSCompliant(false)]
        public static implicit operator MessagePackObject(sbyte value)
        {
            return new MessagePackObject { _value = (ulong) value, _handleOrTypeCode = _sbyteTypeCode };
        }

        public static implicit operator MessagePackObject(float value)
        {
            MessagePackObject obj2 = new MessagePackObject();
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                obj2._value |= bytes[3] << 0x18;
                obj2._value |= bytes[2] << 0x10;
                obj2._value |= bytes[1] << 8;
                obj2._value |= bytes[0];
            }
            else
            {
                obj2._value |= bytes[0] << 0x18;
                obj2._value |= bytes[1] << 0x10;
                obj2._value |= bytes[2] << 8;
                obj2._value |= bytes[3];
            }
            obj2._handleOrTypeCode = _singleTypeCode;
            return obj2;
        }

        public static implicit operator MessagePackObject(string value)
        {
            MessagePackObject obj2 = new MessagePackObject();
            if (value == null)
            {
                obj2._handleOrTypeCode = null;
                return obj2;
            }
            obj2._handleOrTypeCode = new MessagePackString(value);
            return obj2;
        }

        [CLSCompliant(false)]
        public static implicit operator MessagePackObject(ushort value)
        {
            return new MessagePackObject { _value = value, _handleOrTypeCode = _uint16TypeCode };
        }

        [CLSCompliant(false)]
        public static implicit operator MessagePackObject(uint value)
        {
            return new MessagePackObject { _value = value, _handleOrTypeCode = _uint32TypeCode };
        }

        [CLSCompliant(false)]
        public static implicit operator MessagePackObject(ulong value)
        {
            return new MessagePackObject { _value = value, _handleOrTypeCode = _uint64TypeCode };
        }

        public static implicit operator MessagePackObject(MessagePackObject[] value)
        {
            return new MessagePackObject(value, false);
        }

        public static implicit operator MessagePackObject(byte[] value)
        {
            MessagePackObject obj2 = new MessagePackObject();
            if (value == null)
            {
                obj2._handleOrTypeCode = null;
                return obj2;
            }
            obj2._handleOrTypeCode = new MessagePackString(value);
            return obj2;
        }

        public static bool operator !=(MessagePackObject left, MessagePackObject right)
        {
            return !left.Equals(right);
        }

        public void PackToMessage(Packer packer, PackingOptions options)
        {
            if (packer == null)
            {
                throw new ArgumentNullException("packer");
            }
            if (this._handleOrTypeCode == null)
            {
                packer.PackNull();
            }
            else
            {
                ValueTypeCode code = this._handleOrTypeCode as ValueTypeCode;
                if (code == null)
                {
                    MessagePackString str = this._handleOrTypeCode as MessagePackString;
                    if (str != null)
                    {
                        packer.PackRaw(str.GetBytes());
                    }
                    else
                    {
                        IList<MessagePackObject> list = this._handleOrTypeCode as IList<MessagePackObject>;
                        if (list != null)
                        {
                            packer.PackArrayHeader(list.Count);
                            foreach (MessagePackObject obj2 in list)
                            {
                                obj2.PackToMessage(packer, options);
                            }
                        }
                        else
                        {
                            IDictionary<MessagePackObject, MessagePackObject> dictionary = this._handleOrTypeCode as IDictionary<MessagePackObject, MessagePackObject>;
                            if (dictionary == null)
                            {
                                throw new SerializationException("Failed to pack this object.");
                            }
                            packer.PackMapHeader(dictionary.Count);
                            foreach (KeyValuePair<MessagePackObject, MessagePackObject> pair in dictionary)
                            {
                                pair.Key.PackToMessage(packer, options);
                                pair.Value.PackToMessage(packer, options);
                            }
                        }
                    }
                }
                else
                {
                    switch (code.TypeCode)
                    {
                        case MessagePackValueTypeCode.Int8:
                            packer.Pack((sbyte) this);
                            return;

                        case MessagePackValueTypeCode.UInt8:
                            packer.Pack((byte) this);
                            return;

                        case MessagePackValueTypeCode.Int16:
                            packer.Pack((short) this);
                            return;

                        case MessagePackValueTypeCode.UInt16:
                            packer.Pack((ushort) this);
                            return;

                        case MessagePackValueTypeCode.Int32:
                            packer.Pack((int) this);
                            return;

                        case MessagePackValueTypeCode.UInt32:
                            packer.Pack((uint) this);
                            return;

                        case MessagePackValueTypeCode.Int64:
                            packer.Pack((long) this);
                            return;

                        case MessagePackValueTypeCode.UInt64:
                            packer.Pack(this._value);
                            return;

                        case MessagePackValueTypeCode.Boolean:
                            packer.Pack(this._value != 0L);
                            return;

                        case MessagePackValueTypeCode.Single:
                            packer.Pack((float) this);
                            return;

                        case MessagePackValueTypeCode.Double:
                            packer.Pack((double) this);
                            return;
                    }
                    throw new SerializationException("Failed to pack this object.");
                }
            }
        }

        private static void ThrowCannotBeNilAs<T>()
        {
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Do not convert nil MessagePackObject to {0}.", new object[] { typeof(T) }));
        }

        private static void ThrowInvalidTypeAs<T>(MessagePackObject instance)
        {
            if (instance._handleOrTypeCode is ValueTypeCode)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Do not convert {0} (binary:0x{2:x}) MessagePackObject to {1}.", new object[] { instance.UnderlyingType, typeof(T), instance._value }));
            }
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Do not convert {0} MessagePackObject to {1}.", new object[] { instance.UnderlyingType, typeof(T) }));
        }

        public object ToObject()
        {
            if (this._handleOrTypeCode != null)
            {
                ValueTypeCode code = this._handleOrTypeCode as ValueTypeCode;
                if (code == null)
                {
                    MessagePackString str = this._handleOrTypeCode as MessagePackString;
                    if (str != null)
                    {
                        string str2 = str.TryGetString();
                        if (str2 != null)
                        {
                            return str2;
                        }
                        return str.UnsafeGetBuffer();
                    }
                    IDictionary<MessagePackObject, MessagePackObject> dictionary = this._handleOrTypeCode as IDictionary<MessagePackObject, MessagePackObject>;
                    if (dictionary != null)
                    {
                        return dictionary;
                    }
                    IList<MessagePackObject> list = this._handleOrTypeCode as IList<MessagePackObject>;
                    if (list != null)
                    {
                        return list;
                    }
                    Contract.Assert(false, "Unknwon type:" + this._handleOrTypeCode);
                    return null;
                }
                switch (code.TypeCode)
                {
                    case MessagePackValueTypeCode.Int8:
                        return this.AsSByte();

                    case MessagePackValueTypeCode.UInt8:
                        return this.AsByte();

                    case MessagePackValueTypeCode.Int16:
                        return this.AsInt16();

                    case MessagePackValueTypeCode.UInt16:
                        return this.AsUInt16();

                    case MessagePackValueTypeCode.Int32:
                        return this.AsInt32();

                    case MessagePackValueTypeCode.UInt32:
                        return this.AsUInt32();

                    case MessagePackValueTypeCode.Int64:
                        return this.AsInt64();

                    case MessagePackValueTypeCode.UInt64:
                        return this.AsUInt64();

                    case MessagePackValueTypeCode.Boolean:
                        return this.AsBoolean();

                    case MessagePackValueTypeCode.Single:
                        return this.AsSingle();

                    case MessagePackValueTypeCode.Double:
                        return this.AsDouble();
                }
                Contract.Assert(false, "Unknwon type code:" + code.TypeCode);
            }
            return null;
        }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            this.ToString(buffer, false);
            return buffer.ToString();
        }

        private void ToString(StringBuilder buffer, bool isJson)
        {
            if (this._handleOrTypeCode == null)
            {
                if (isJson)
                {
                    buffer.Append("null");
                }
            }
            else
            {
                ValueTypeCode code = this._handleOrTypeCode as ValueTypeCode;
                if (code == null)
                {
                    IList<MessagePackObject> list = this._handleOrTypeCode as IList<MessagePackObject>;
                    if (list != null)
                    {
                        buffer.Append('[');
                        if (list.Count > 0)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (i > 0)
                                {
                                    buffer.Append(',');
                                }
                                buffer.Append(' ');
                                list[i].ToString(buffer, true);
                            }
                            buffer.Append(' ');
                        }
                        buffer.Append(']');
                    }
                    else
                    {
                        IDictionary<MessagePackObject, MessagePackObject> dictionary = this._handleOrTypeCode as IDictionary<MessagePackObject, MessagePackObject>;
                        if (dictionary != null)
                        {
                            buffer.Append('{');
                            if (dictionary.Count > 0)
                            {
                                bool flag = true;
                                foreach (KeyValuePair<MessagePackObject, MessagePackObject> pair in dictionary)
                                {
                                    if (flag)
                                    {
                                        flag = false;
                                    }
                                    else
                                    {
                                        buffer.Append(',');
                                    }
                                    buffer.Append(' ');
                                    pair.Key.ToString(buffer, true);
                                    buffer.Append(' ').Append(':').Append(' ');
                                    pair.Value.ToString(buffer, true);
                                }
                                buffer.Append(' ');
                            }
                            buffer.Append('}');
                        }
                        else
                        {
                            MessagePackString str = this._handleOrTypeCode as MessagePackString;
                            if (str != null)
                            {
                                string str2 = str.TryGetString();
                                if (str2 != null)
                                {
                                    if (isJson)
                                    {
                                        buffer.Append('"');
                                        foreach (char ch in str2)
                                        {
                                            char ch2 = ch;
                                            if (ch2 <= '"')
                                            {
                                                switch (ch2)
                                                {
                                                    case '\b':
                                                    {
                                                        buffer.Append('\\').Append('b');
                                                        continue;
                                                    }
                                                    case '\t':
                                                    {
                                                        buffer.Append('\\').Append('t');
                                                        continue;
                                                    }
                                                    case '\n':
                                                    {
                                                        buffer.Append('\\').Append('n');
                                                        continue;
                                                    }
                                                    case '\f':
                                                    {
                                                        buffer.Append('\\').Append('f');
                                                        continue;
                                                    }
                                                    case '\r':
                                                    {
                                                        buffer.Append('\\').Append('r');
                                                        continue;
                                                    }
                                                    case ' ':
                                                    {
                                                        buffer.Append(' ');
                                                        continue;
                                                    }
                                                    case '"':
                                                    {
                                                        buffer.Append('\\').Append('"');
                                                        continue;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (ch2 != '/')
                                                {
                                                    if (ch2 != '\\')
                                                    {
                                                        goto Label_043E;
                                                    }
                                                    buffer.Append('\\').Append('\\');
                                                }
                                                else
                                                {
                                                    buffer.Append('\\').Append('/');
                                                }
                                                continue;
                                            }
                                        Label_043E:
                                            switch (CharUnicodeInfo.GetUnicodeCategory(ch))
                                            {
                                                case UnicodeCategory.SpaceSeparator:
                                                case UnicodeCategory.LineSeparator:
                                                case UnicodeCategory.ParagraphSeparator:
                                                case UnicodeCategory.Control:
                                                case UnicodeCategory.Format:
                                                case UnicodeCategory.Surrogate:
                                                case UnicodeCategory.PrivateUse:
                                                case UnicodeCategory.OtherNotAssigned:
                                                {
                                                    ushort num6 = ch;
                                                    buffer.Append('\\').Append('u').Append(num6.ToString("X", CultureInfo.InvariantCulture));
                                                    break;
                                                }
                                                default:
                                                    buffer.Append(ch);
                                                    break;
                                            }
                                        }
                                        buffer.Append('"');
                                    }
                                    else
                                    {
                                        buffer.Append(str2);
                                    }
                                    return;
                                }
                                byte[] blob = str.UnsafeGetBuffer();
                                if (blob != null)
                                {
                                    if (isJson)
                                    {
                                        buffer.Append('"');
                                        Binary.ToHexString(blob, buffer);
                                        buffer.Append('"');
                                    }
                                    else
                                    {
                                        Binary.ToHexString(blob, buffer);
                                    }
                                    return;
                                }
                            }
                            Contract.Assert(false, string.Format("(this._handleOrTypeCode is string) but {0}", this._handleOrTypeCode.GetType()));
                            if (isJson)
                            {
                                buffer.Append('"').Append(this._handleOrTypeCode).Append('"');
                            }
                            else
                            {
                                buffer.Append(this._handleOrTypeCode);
                            }
                        }
                    }
                }
                else
                {
                    switch (code.TypeCode)
                    {
                        case MessagePackValueTypeCode.Boolean:
                            if (!isJson)
                            {
                                buffer.Append(this.AsBoolean());
                                return;
                            }
                            buffer.Append(this.AsBoolean() ? "true" : "false");
                            return;

                        case MessagePackValueTypeCode.Single:
                            buffer.Append(this.AsSingle().ToString(CultureInfo.InvariantCulture));
                            return;

                        case MessagePackValueTypeCode.Double:
                            buffer.Append(this.AsDouble().ToString(CultureInfo.InvariantCulture));
                            return;
                    }
                    if (code.IsSigned)
                    {
                        buffer.Append(((long) this._value).ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        buffer.Append(this._value.ToString(CultureInfo.InvariantCulture));
                    }
                }
            }
        }

        private static void VerifyUnderlyingType<T>(MessagePackObject instance, string parameterName)
        {
            if (instance.IsNil)
            {
                if (!(typeof(T).IsValueType && (Nullable.GetUnderlyingType(typeof(T)) == null)))
                {
                    return;
                }
                if (parameterName != null)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Do not convert nil MessagePackObject to {0}.", new object[] { typeof(T) }), parameterName);
                }
                ThrowCannotBeNilAs<T>();
            }
            if (!instance.IsTypeOf<T>().GetValueOrDefault())
            {
                if (parameterName != null)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Do not convert {0} MessagePackObject to {1}.", new object[] { instance.UnderlyingType, typeof(T) }), parameterName);
                }
                ThrowInvalidTypeAs<T>(instance);
            }
        }

        public bool IsArray
        {
            get
            {
                return this.IsList;
            }
        }

        public bool IsDictionary
        {
            get
            {
                return (!this.IsNil && (this._handleOrTypeCode is IDictionary<MessagePackObject, MessagePackObject>));
            }
        }

        public bool IsList
        {
            get
            {
                return (!this.IsNil && (this._handleOrTypeCode is IList<MessagePackObject>));
            }
        }

        public bool IsMap
        {
            get
            {
                return this.IsDictionary;
            }
        }

        public bool IsNil
        {
            get
            {
                return (this._handleOrTypeCode == null);
            }
        }

        public bool IsRaw
        {
            get
            {
                return (!this.IsNil && (this._handleOrTypeCode is MessagePackString));
            }
        }

        public Type UnderlyingType
        {
            get
            {
                if (this._handleOrTypeCode == null)
                {
                    return null;
                }
                ValueTypeCode code = this._handleOrTypeCode as ValueTypeCode;
                if (code == null)
                {
                    MessagePackString str = this._handleOrTypeCode as MessagePackString;
                    if (str != null)
                    {
                        return str.GetUnderlyingType();
                    }
                    return this._handleOrTypeCode.GetType();
                }
                return code.Type;
            }
        }

        [Serializable]
        private enum MessagePackValueTypeCode
        {
            Boolean = 10,
            Double = 13,
            Int16 = 3,
            Int32 = 5,
            Int64 = 7,
            Int8 = 1,
            Nil = 0,
            Object = 0x10,
            Single = 11,
            UInt16 = 4,
            UInt32 = 6,
            UInt64 = 8,
            UInt8 = 2
        }

        [Serializable]
        private sealed class ValueTypeCode
        {
            private readonly System.Type _type;
            private readonly MessagePackObject.MessagePackValueTypeCode _typeCode;

            internal ValueTypeCode(System.Type type, MessagePackObject.MessagePackValueTypeCode typeCode)
            {
                this._type = type;
                this._typeCode = typeCode;
            }

            public override string ToString()
            {
                return ((this._typeCode == MessagePackObject.MessagePackValueTypeCode.Object) ? this._type.FullName : this._typeCode.ToString());
            }

            public bool IsInteger
            {
                get
                {
                    return (this._typeCode < MessagePackObject.MessagePackValueTypeCode.Boolean);
                }
            }

            public bool IsSigned
            {
                get
                {
                    return ((this._typeCode % MessagePackObject.MessagePackValueTypeCode.UInt8) != MessagePackObject.MessagePackValueTypeCode.Nil);
                }
            }

            public System.Type Type
            {
                get
                {
                    return this._type;
                }
            }

            public MessagePackObject.MessagePackValueTypeCode TypeCode
            {
                get
                {
                    return this._typeCode;
                }
            }
        }
    }
}

