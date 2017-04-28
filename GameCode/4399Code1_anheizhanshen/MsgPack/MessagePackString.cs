namespace MsgPack
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Security;
    using System.Text;
    using System.Threading;

    [Serializable, DebuggerDisplay("{DebuggerDisplayString}"), DebuggerTypeProxy(typeof(MessagePackString.MessagePackStringDebuggerProxy))]
    internal sealed class MessagePackString
    {
        private string _decoded;
        private DecoderFallbackException _decodingError;
        private byte[] _encoded;
        private static int _isFastEqualsDisabled = 0;
        private BinaryType _type;

        private MessagePackString(MessagePackString other)
        {
            this._encoded = other._encoded;
            this._decoded = other._decoded;
            this._decodingError = other._decodingError;
            this._type = other._type;
        }

        public MessagePackString(string decoded)
        {
            Contract.Assert(decoded != null);
            this._decoded = decoded;
            this._type = BinaryType.String;
        }

        public MessagePackString(byte[] encoded)
        {
            Contract.Assert(encoded != null);
            this._encoded = encoded;
        }

        private void DecodeIfNeeded()
        {
            if (((this._decoded == null) && (this._encoded != null)) && (this._type == BinaryType.Unknwon))
            {
                try
                {
                    this._decoded = MessagePackConvert.DecodeStringStrict(this._encoded);
                    this._type = BinaryType.String;
                }
                catch (DecoderFallbackException exception)
                {
                    this._decodingError = exception;
                    this._type = BinaryType.Blob;
                }
            }
        }

        private void EncodeIfNeeded()
        {
            if ((this._encoded == null) && (this._decoded != null))
            {
                this._encoded = MessagePackConvert.Utf8NonBom.GetBytes(this._decoded);
            }
        }

        public sealed override bool Equals(object obj)
        {
            MessagePackString right = obj as MessagePackString;
            if (object.ReferenceEquals(obj, null))
            {
                return false;
            }
            if ((this._decoded == null) || (right._decoded == null))
            {
                if ((this._decoded == null) && (right._decoded == null))
                {
                    return EqualsEncoded(this, right);
                }
                this.DecodeIfNeeded();
                right.DecodeIfNeeded();
            }
            return (this._decoded == right._decoded);
        }

        private static bool EqualsEncoded(MessagePackString left, MessagePackString right)
        {
            if (left._encoded == null)
            {
                return (right._encoded == null);
            }
            if (left._encoded.Length == 0)
            {
                return (right._encoded.Length == 0);
            }
            if (left._encoded.Length != right._encoded.Length)
            {
                return false;
            }
            if (_isFastEqualsDisabled == 0)
            {
                try
                {
                    return UnsafeFastEquals(left._encoded, right._encoded);
                }
                catch (SecurityException)
                {
                    Interlocked.Exchange(ref _isFastEqualsDisabled, 1);
                }
                catch (MemberAccessException)
                {
                    Interlocked.Exchange(ref _isFastEqualsDisabled, 1);
                }
            }
            return SlowEquals(left._encoded, right._encoded);
        }

        public byte[] GetBytes()
        {
            this.EncodeIfNeeded();
            return this._encoded;
        }

        public sealed override int GetHashCode()
        {
            if (this._decoded != null)
            {
                return this._decoded.GetHashCode();
            }
            this.DecodeIfNeeded();
            if (this._decoded != null)
            {
                return this._decoded.GetHashCode();
            }
            if (this._encoded != null)
            {
                int num = 0;
                for (int i = 0; i < this._encoded.Length; i++)
                {
                    int num3 = this._encoded[i] << ((i % 4) * 8);
                    num ^= num3;
                }
                return num;
            }
            return 0;
        }

        public string GetString()
        {
            this.DecodeIfNeeded();
            if (this._decodingError != null)
            {
                throw new InvalidOperationException("This bytes is not UTF-8 string.", this._decodingError);
            }
            return this._decoded;
        }

        public Type GetUnderlyingType()
        {
            this.DecodeIfNeeded();
            return ((this._type == BinaryType.String) ? typeof(string) : typeof(byte[]));
        }

        private static bool SlowEquals(byte[] x, byte[] y)
        {
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }
            return true;
        }

        public sealed override string ToString()
        {
            if (this._decoded != null)
            {
                return this._decoded;
            }
            if (this._encoded != null)
            {
                return Binary.ToHexString(this._encoded);
            }
            return string.Empty;
        }

        public string TryGetString()
        {
            this.DecodeIfNeeded();
            return this._decoded;
        }

        [SecuritySafeCritical]
        private static bool UnsafeFastEquals(byte[] x, byte[] y)
        {
            int num;
            Contract.Assert(x != null);
            Contract.Assert(y != null);
            Contract.Assert(0 < x.Length);
            Contract.Assert(x.Length == y.Length);
            if (!UnsafeNativeMethods.TryMemCmp(x, y, new UIntPtr((uint) x.Length), out num))
            {
                Interlocked.Exchange(ref _isFastEqualsDisabled, 1);
                return SlowEquals(x, y);
            }
            return (num == 0);
        }

        public byte[] UnsafeGetBuffer()
        {
            return this._encoded;
        }

        public string UnsafeGetString()
        {
            return this._decoded;
        }

        private string DebuggerDisplayString
        {
            get
            {
                return new MessagePackStringDebuggerProxy(this).Value;
            }
        }

        internal static bool IsFastEqualsDisabled
        {
            get
            {
                return (_isFastEqualsDisabled != 0);
            }
        }

        [Serializable]
        private enum BinaryType
        {
            Unknwon,
            String,
            Blob
        }

        internal sealed class MessagePackStringDebuggerProxy
        {
            private string _asByteArray;
            private static readonly string[] _elipse = new string[] { "..." };
            private readonly MessagePackString _target;

            public MessagePackStringDebuggerProxy(MessagePackString target)
            {
                this._target = new MessagePackString(target);
            }

            private string CreateByteArrayString()
            {
                byte[] bytes = this._target.GetBytes();
                StringBuilder builder = new StringBuilder(((bytes.Length <= 0x80) ? (bytes.Length * 3) : 0x183) + 4);
                builder.Append('[');
                foreach (byte num in bytes.Take<byte>(0x80))
                {
                    builder.Append(' ');
                    builder.Append(num.ToString("X2", CultureInfo.InvariantCulture));
                }
                builder.Append(" ]");
                string str = builder.ToString();
                Interlocked.Exchange<string>(ref this._asByteArray, str);
                return str;
            }

            private static bool MustBeString(string value)
            {
                for (int i = 0; (i < 0x80) && (i < value.Length); i++)
                {
                    char ch = value[i];
                    if ((ch < ' ') && (((ch != '\t') && (ch != '\n')) && (ch != '\r')))
                    {
                        return false;
                    }
                    if (('~' < ch) && (ch < '\x00a0'))
                    {
                        return false;
                    }
                }
                return true;
            }

            public string AsByteArray
            {
                get
                {
                    string str = Interlocked.CompareExchange<string>(ref this._asByteArray, null, null);
                    if (str == null)
                    {
                        str = this.CreateByteArrayString();
                    }
                    return str;
                }
            }

            public string AsString
            {
                get
                {
                    string str = this._target.TryGetString();
                    if ((str != null) && !MustBeString(str))
                    {
                        this.CreateByteArrayString();
                        return null;
                    }
                    return str;
                }
            }

            public string Value
            {
                get
                {
                    string str = Interlocked.CompareExchange<string>(ref this._asByteArray, null, null);
                    if (str != null)
                    {
                        return str;
                    }
                    switch (this._target._type)
                    {
                        case MessagePackString.BinaryType.String:
                            break;

                        case MessagePackString.BinaryType.Blob:
                            return this.AsByteArray;

                        default:
                            this._target.DecodeIfNeeded();
                            break;
                    }
                    string asString = this.AsString;
                    if (asString == null)
                    {
                        return this.AsByteArray;
                    }
                    return asString;
                }
            }
        }
    }
}

