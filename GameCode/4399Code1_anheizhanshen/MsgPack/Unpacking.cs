namespace MsgPack
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    public static class Unpacking
    {
        internal static readonly MessagePackObject FalseValue = 0;
        internal static readonly MessagePackObject[] NegativeIntegers = (from i in Enumerable.Range(0, 0x20) select new MessagePackObject((sbyte) (-32 + i))).ToArray<MessagePackObject>();
        internal static readonly MessagePackObject? NullableFalseValue = 0;
        internal static readonly MessagePackObject?[] NullableNegativeIntegers = (from i in Enumerable.Range(0, 0x20) select new MessagePackObject?((sbyte) (-32 + i))).ToArray<MessagePackObject?>();
        internal static readonly MessagePackObject? NullableNilValue = new MessagePackObject?(MessagePackObject.Nil);
        internal static readonly MessagePackObject?[] NullablePositiveIntegers = (from i in Enumerable.Range(0, 0x80) select new MessagePackObject?((byte) i)).ToArray<MessagePackObject?>();
        internal static readonly MessagePackObject? NullableTrueValue = 1;
        internal static readonly MessagePackObject[] PositiveIntegers = (from i in Enumerable.Range(0, 0x80) select new MessagePackObject((byte) i)).ToArray<MessagePackObject>();
        internal static readonly MessagePackObject TrueValue = 1;

        [CompilerGenerated]
        private static MessagePackObject <.cctor>b__0(int i)
        {
            return new MessagePackObject((byte) i);
        }

        [CompilerGenerated]
        private static MessagePackObject <.cctor>b__1(int i)
        {
            return new MessagePackObject((sbyte) (-32 + i));
        }

        [CompilerGenerated]
        private static MessagePackObject? <.cctor>b__2(int i)
        {
            return new MessagePackObject?((byte) i);
        }

        [CompilerGenerated]
        private static MessagePackObject? <.cctor>b__3(int i)
        {
            return new MessagePackObject?((sbyte) (-32 + i));
        }

        private static bool IsNil(Unpacker unpacker)
        {
            return (unpacker.Data.HasValue && unpacker.Data.Value.IsNil);
        }

        private static Exception NewInvalidEncodingException(Encoding encoding, Exception innerException)
        {
            return new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "The stream cannot be decoded as {0} string.", new object[] { encoding.WebName }), innerException);
        }

        private static Exception NewTypeMismatchException(Type requestedType, InvalidOperationException innerException)
        {
            return new MessageTypeException(string.Format(CultureInfo.CurrentCulture, "Message type is not compatible to {0}.", new object[] { requestedType }), innerException);
        }

        private static byte[] ReadBytes(Stream source, int length)
        {
            byte[] buffer = new byte[length];
            if (source.Read(buffer, 0, length) < length)
            {
                throw new UnpackException("The underlying stream unexpectedly ends.");
            }
            return buffer;
        }

        public static UnpackingResult<IList<MessagePackObject>> UnpackArray(byte[] source)
        {
            return UnpackArray(source, 0);
        }

        public static IList<MessagePackObject> UnpackArray(Stream source)
        {
            ValidateStream(source);
            return UnpackArrayCore(source);
        }

        public static UnpackingResult<IList<MessagePackObject>> UnpackArray(byte[] source, int offset)
        {
            ValidateByteArray(source, offset);
            using (MemoryStream stream = new MemoryStream(source))
            {
                stream.Position = offset;
                return new UnpackingResult<IList<MessagePackObject>>(UnpackArrayCore(stream), ((int) stream.Position) - offset);
            }
        }

        private static IList<MessagePackObject> UnpackArrayCore(Unpacker unpacker)
        {
            if (IsNil(unpacker))
            {
                return null;
            }
            uint num = (uint) unpacker.Data.Value;
            if (num > 0x7fffffff)
            {
                throw new MessageNotSupportedException("The array which length is greater than Int32.MaxValue is not supported.");
            }
            MessagePackObject[] objArray = new MessagePackObject[num];
            for (int i = 0; i < objArray.Length; i++)
            {
                objArray[i] = UnpackObjectCore(unpacker);
            }
            return objArray;
        }

        private static IList<MessagePackObject> UnpackArrayCore(Stream source)
        {
            using (Unpacker unpacker = Unpacker.Create(source, false))
            {
                UnpackOne(unpacker);
                if (!(IsNil(unpacker) || unpacker.IsArrayHeader))
                {
                    throw new MessageTypeException("The underlying stream is not array type.");
                }
                return UnpackArrayCore(unpacker);
            }
        }

        public static UnpackingResult<long?> UnpackArrayLength(byte[] source)
        {
            return UnpackArrayLength(source, 0);
        }

        public static long? UnpackArrayLength(Stream source)
        {
            ValidateStream(source);
            uint? nullable2 = UnpackArrayLengthCore(source);
            return (nullable2.HasValue ? new long?((long) ((ulong) nullable2.GetValueOrDefault())) : null);
        }

        public static UnpackingResult<long?> UnpackArrayLength(byte[] source, int offset)
        {
            ValidateByteArray(source, offset);
            using (MemoryStream stream = new MemoryStream(source))
            {
                stream.Position = offset;
                uint? nullable2 = UnpackArrayLengthCore(stream);
                return new UnpackingResult<long?>(nullable2.HasValue ? new long?((long) ((ulong) nullable2.GetValueOrDefault())) : null, ((int) stream.Position) - offset);
            }
        }

        private static uint? UnpackArrayLengthCore(Stream source)
        {
            using (Unpacker unpacker = Unpacker.Create(source, false))
            {
                UnpackOne(unpacker);
                if (IsNil(unpacker))
                {
                    return null;
                }
                if (!unpacker.IsArrayHeader)
                {
                    throw new MessageTypeException("The underlying stream is not array type.");
                }
                return new uint?((uint) unpacker.Data.Value);
            }
        }

        public static UnpackingResult<byte[]> UnpackBinary(byte[] source)
        {
            return UnpackBinary(source, 0);
        }

        public static byte[] UnpackBinary(Stream source)
        {
            ValidateStream(source);
            return UnpackBinaryCore(source);
        }

        public static UnpackingResult<byte[]> UnpackBinary(byte[] source, int offset)
        {
            ValidateByteArray(source, offset);
            using (MemoryStream stream = new MemoryStream(source))
            {
                stream.Position = offset;
                return new UnpackingResult<byte[]>(UnpackBinaryCore(stream), ((int) stream.Position) - offset);
            }
        }

        private static byte[] UnpackBinaryCore(Stream source)
        {
            byte[] buffer;
            using (Unpacker unpacker = Unpacker.Create(source, false))
            {
                UnpackOne(unpacker);
                try
                {
                    buffer = unpacker.Data.Value.AsBinary();
                }
                catch (InvalidOperationException exception)
                {
                    throw NewTypeMismatchException(typeof(byte[]), exception);
                }
            }
            return buffer;
        }

        public static UnpackingResult<bool> UnpackBoolean(byte[] source)
        {
            return UnpackBoolean(source, 0);
        }

        public static bool UnpackBoolean(Stream source)
        {
            ValidateStream(source);
            return UnpackBooleanCore(source);
        }

        public static UnpackingResult<bool> UnpackBoolean(byte[] source, int offset)
        {
            ValidateByteArray(source, offset);
            using (MemoryStream stream = new MemoryStream(source))
            {
                stream.Position = offset;
                return new UnpackingResult<bool>(UnpackBooleanCore(stream), ((int) stream.Position) - offset);
            }
        }

        private static bool UnpackBooleanCore(Stream source)
        {
            bool flag;
            using (Unpacker unpacker = Unpacker.Create(source, false))
            {
                UnpackOne(unpacker);
                VerifyIsScalar(unpacker);
                try
                {
                    flag = (bool) unpacker.Data.Value;
                }
                catch (InvalidOperationException exception)
                {
                    throw NewTypeMismatchException(typeof(bool), exception);
                }
            }
            return flag;
        }

        public static UnpackingResult<byte> UnpackByte(byte[] source)
        {
            return UnpackByte(source, 0);
        }

        public static byte UnpackByte(Stream source)
        {
            ValidateStream(source);
            return UnpackByteCore(source);
        }

        public static UnpackingResult<byte> UnpackByte(byte[] source, int offset)
        {
            ValidateByteArray(source, offset);
            using (MemoryStream stream = new MemoryStream(source))
            {
                stream.Position = offset;
                return new UnpackingResult<byte>(UnpackByteCore(stream), ((int) stream.Position) - offset);
            }
        }

        private static byte UnpackByteCore(Stream source)
        {
            byte num;
            using (Unpacker unpacker = Unpacker.Create(source, false))
            {
                UnpackOne(unpacker);
                VerifyIsScalar(unpacker);
                try
                {
                    num = (byte) unpacker.Data.Value;
                }
                catch (InvalidOperationException exception)
                {
                    throw NewTypeMismatchException(typeof(byte), exception);
                }
            }
            return num;
        }

        public static UnpackingStream UnpackByteStream(Stream source)
        {
            ValidateStream(source);
            return UnpackByteStreamCore(source);
        }

        private static UnpackingStream UnpackByteStreamCore(Stream source)
        {
            uint num = UnpackRawLengthCore(source);
            if (source.CanSeek)
            {
                return new SeekableUnpackingStream(source, (long) num);
            }
            return new UnseekableUnpackingStream(source, (long) num);
        }

        public static UnpackingStreamReader UnpackCharStream(Stream source)
        {
            return UnpackCharStream(source, MessagePackConvert.Utf8NonBomStrict);
        }

        public static UnpackingStreamReader UnpackCharStream(Stream source, Encoding encoding)
        {
            ValidateStream(source);
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            UnpackingStream stream = UnpackByteStreamCore(source);
            return new DefaultUnpackingStreamReader(stream, encoding, stream.RawLength);
        }

        public static MessagePackObjectDictionary UnpackDictionary(Stream source)
        {
            ValidateStream(source);
            return UnpackDictionaryCore(source);
        }

        public static UnpackingResult<MessagePackObjectDictionary> UnpackDictionary(byte[] source)
        {
            return UnpackDictionary(source, 0);
        }

        public static UnpackingResult<MessagePackObjectDictionary> UnpackDictionary(byte[] source, int offset)
        {
            ValidateByteArray(source, offset);
            using (MemoryStream stream = new MemoryStream(source))
            {
                stream.Position = offset;
                return new UnpackingResult<MessagePackObjectDictionary>(UnpackDictionaryCore(stream), ((int) stream.Position) - offset);
            }
        }

        private static MessagePackObjectDictionary UnpackDictionaryCore(Unpacker unpacker)
        {
            if (IsNil(unpacker))
            {
                return null;
            }
            uint num = (uint) unpacker.Data.Value;
            if (num > 0x7fffffff)
            {
                throw new MessageNotSupportedException("The map which count is greater than Int32.MaxValue is not supported.");
            }
            MessagePackObjectDictionary dictionary = new MessagePackObjectDictionary((int) num);
            for (int i = 0; i < num; i++)
            {
                MessagePackObject key = UnpackObjectCore(unpacker);
                MessagePackObject obj3 = UnpackObjectCore(unpacker);
                try
                {
                    dictionary.Add(key, obj3);
                }
                catch (ArgumentException exception)
                {
                    throw new InvalidMessagePackStreamException("The dicationry key is duplicated in the stream.", exception);
                }
            }
            return dictionary;
        }

        private static MessagePackObjectDictionary UnpackDictionaryCore(Stream source)
        {
            using (Unpacker unpacker = Unpacker.Create(source, false))
            {
                UnpackOne(unpacker);
                if (!(IsNil(unpacker) || unpacker.IsMapHeader))
                {
                    throw new MessageTypeException("The underlying stream is not map type.");
                }
                return UnpackDictionaryCore(unpacker);
            }
        }

        public static UnpackingResult<long?> UnpackDictionaryCount(byte[] source)
        {
            return UnpackDictionaryCount(source, 0);
        }

        public static long? UnpackDictionaryCount(Stream source)
        {
            ValidateStream(source);
            uint? nullable2 = UnpackDictionaryCountCore(source);
            return (nullable2.HasValue ? new long?((long) ((ulong) nullable2.GetValueOrDefault())) : null);
        }

        public static UnpackingResult<long?> UnpackDictionaryCount(byte[] source, int offset)
        {
            ValidateByteArray(source, offset);
            using (MemoryStream stream = new MemoryStream(source))
            {
                stream.Position = offset;
                uint? nullable2 = UnpackDictionaryCountCore(stream);
                return new UnpackingResult<long?>(nullable2.HasValue ? new long?((long) ((ulong) nullable2.GetValueOrDefault())) : null, ((int) stream.Position) - offset);
            }
        }

        private static uint? UnpackDictionaryCountCore(Stream source)
        {
            using (Unpacker unpacker = Unpacker.Create(source, false))
            {
                UnpackOne(unpacker);
                if (IsNil(unpacker))
                {
                    return null;
                }
                if (!unpacker.IsMapHeader)
                {
                    throw new MessageTypeException("The underlying stream is not map type.");
                }
                return new uint?((uint) unpacker.Data.Value);
            }
        }

        public static UnpackingResult<double> UnpackDouble(byte[] source)
        {
            return UnpackDouble(source, 0);
        }

        public static double UnpackDouble(Stream source)
        {
            ValidateStream(source);
            return UnpackDoubleCore(source);
        }

        public static UnpackingResult<double> UnpackDouble(byte[] source, int offset)
        {
            ValidateByteArray(source, offset);
            using (MemoryStream stream = new MemoryStream(source))
            {
                stream.Position = offset;
                return new UnpackingResult<double>(UnpackDoubleCore(stream), ((int) stream.Position) - offset);
            }
        }

        private static double UnpackDoubleCore(Stream source)
        {
            double num;
            using (Unpacker unpacker = Unpacker.Create(source, false))
            {
                UnpackOne(unpacker);
                VerifyIsScalar(unpacker);
                try
                {
                    num = (double) unpacker.Data.Value;
                }
                catch (InvalidOperationException exception)
                {
                    throw NewTypeMismatchException(typeof(double), exception);
                }
            }
            return num;
        }

        public static UnpackingResult<short> UnpackInt16(byte[] source)
        {
            return UnpackInt16(source, 0);
        }

        public static short UnpackInt16(Stream source)
        {
            ValidateStream(source);
            return UnpackInt16Core(source);
        }

        public static UnpackingResult<short> UnpackInt16(byte[] source, int offset)
        {
            ValidateByteArray(source, offset);
            using (MemoryStream stream = new MemoryStream(source))
            {
                stream.Position = offset;
                return new UnpackingResult<short>(UnpackInt16Core(stream), ((int) stream.Position) - offset);
            }
        }

        private static short UnpackInt16Core(Stream source)
        {
            short num;
            using (Unpacker unpacker = Unpacker.Create(source, false))
            {
                UnpackOne(unpacker);
                VerifyIsScalar(unpacker);
                try
                {
                    num = (short) unpacker.Data.Value;
                }
                catch (InvalidOperationException exception)
                {
                    throw NewTypeMismatchException(typeof(short), exception);
                }
            }
            return num;
        }

        public static UnpackingResult<int> UnpackInt32(byte[] source)
        {
            return UnpackInt32(source, 0);
        }

        public static int UnpackInt32(Stream source)
        {
            ValidateStream(source);
            return UnpackInt32Core(source);
        }

        public static UnpackingResult<int> UnpackInt32(byte[] source, int offset)
        {
            ValidateByteArray(source, offset);
            using (MemoryStream stream = new MemoryStream(source))
            {
                stream.Position = offset;
                return new UnpackingResult<int>(UnpackInt32Core(stream), ((int) stream.Position) - offset);
            }
        }

        private static int UnpackInt32Core(Stream source)
        {
            int num;
            using (Unpacker unpacker = Unpacker.Create(source, false))
            {
                UnpackOne(unpacker);
                VerifyIsScalar(unpacker);
                try
                {
                    num = (int) unpacker.Data.Value;
                }
                catch (InvalidOperationException exception)
                {
                    throw NewTypeMismatchException(typeof(int), exception);
                }
            }
            return num;
        }

        public static UnpackingResult<long> UnpackInt64(byte[] source)
        {
            return UnpackInt64(source, 0);
        }

        public static long UnpackInt64(Stream source)
        {
            ValidateStream(source);
            return UnpackInt64Core(source);
        }

        public static UnpackingResult<long> UnpackInt64(byte[] source, int offset)
        {
            ValidateByteArray(source, offset);
            using (MemoryStream stream = new MemoryStream(source))
            {
                stream.Position = offset;
                return new UnpackingResult<long>(UnpackInt64Core(stream), ((int) stream.Position) - offset);
            }
        }

        private static long UnpackInt64Core(Stream source)
        {
            long num;
            using (Unpacker unpacker = Unpacker.Create(source, false))
            {
                UnpackOne(unpacker);
                VerifyIsScalar(unpacker);
                try
                {
                    num = (long) unpacker.Data.Value;
                }
                catch (InvalidOperationException exception)
                {
                    throw NewTypeMismatchException(typeof(long), exception);
                }
            }
            return num;
        }

        public static UnpackingResult<object> UnpackNull(byte[] source)
        {
            return UnpackNull(source, 0);
        }

        public static object UnpackNull(Stream source)
        {
            ValidateStream(source);
            return UnpackNullCore(source);
        }

        public static UnpackingResult<object> UnpackNull(byte[] source, int offset)
        {
            ValidateByteArray(source, offset);
            using (MemoryStream stream = new MemoryStream(source))
            {
                stream.Position = offset;
                return new UnpackingResult<object>(UnpackNullCore(stream), ((int) stream.Position) - offset);
            }
        }

        private static object UnpackNullCore(Stream source)
        {
            using (Unpacker unpacker = Unpacker.Create(source, false))
            {
                UnpackOne(unpacker);
                VerifyIsScalar(unpacker);
                if (!unpacker.Data.Value.IsNil)
                {
                    throw new MessageTypeException("The underlying stream is not nil.");
                }
                return null;
            }
        }

        public static MessagePackObject UnpackObject(Stream source)
        {
            ValidateStream(source);
            return UnpackObjectCore(source);
        }

        public static UnpackingResult<MessagePackObject> UnpackObject(byte[] source)
        {
            return UnpackObject(source, 0);
        }

        public static UnpackingResult<MessagePackObject> UnpackObject(byte[] source, int offset)
        {
            ValidateByteArray(source, offset);
            using (MemoryStream stream = new MemoryStream(source))
            {
                stream.Position = offset;
                return new UnpackingResult<MessagePackObject>(UnpackObjectCore(stream), ((int) stream.Position) - offset);
            }
        }

        private static MessagePackObject UnpackObjectCore(Unpacker unpacker)
        {
            UnpackOne(unpacker);
            if (unpacker.IsArrayHeader)
            {
                return new MessagePackObject(UnpackArrayCore(unpacker), true);
            }
            if (unpacker.IsMapHeader)
            {
                return new MessagePackObject(UnpackDictionaryCore(unpacker), true);
            }
            return unpacker.Data.Value;
        }

        private static MessagePackObject UnpackObjectCore(Stream source)
        {
            using (Unpacker unpacker = Unpacker.Create(source, false))
            {
                return UnpackObjectCore(unpacker);
            }
        }

        private static void UnpackOne(Unpacker unpacker)
        {
            if (!(unpacker.Read() && unpacker.Data.HasValue))
            {
                throw new UnpackException("Cannot unpack MesssagePack object from the stream.");
            }
        }

        private static uint UnpackRawLengthCore(Stream source)
        {
            byte[] buffer;
            int num = source.ReadByte();
            if (num < 0)
            {
                throw new UnpackException("Stream is end.");
            }
            if (num == 0xc0)
            {
                return 0;
            }
            if ((160 <= num) && (num <= 0xbf))
            {
                return (uint) (num - 160);
            }
            if (num == 0xda)
            {
                buffer = ReadBytes(source, 2);
                ushort num2 = buffer[1];
                return (ushort) (num2 | ((ushort) (buffer[0] << 8)));
            }
            if (num != 0xdb)
            {
                throw new MessageTypeException("The underlying stream is not raw type.");
            }
            buffer = ReadBytes(source, 4);
            uint num3 = buffer[3];
            num3 |= (uint) (buffer[2] << 8);
            num3 |= (uint) (buffer[1] << 0x10);
            return (num3 | ((uint) (buffer[0] << 0x18)));
        }

        [CLSCompliant(false)]
        public static UnpackingResult<sbyte> UnpackSByte(byte[] source)
        {
            return UnpackSByte(source, 0);
        }

        [CLSCompliant(false)]
        public static sbyte UnpackSByte(Stream source)
        {
            ValidateStream(source);
            return UnpackSByteCore(source);
        }

        [CLSCompliant(false)]
        public static UnpackingResult<sbyte> UnpackSByte(byte[] source, int offset)
        {
            ValidateByteArray(source, offset);
            using (MemoryStream stream = new MemoryStream(source))
            {
                stream.Position = offset;
                return new UnpackingResult<sbyte>(UnpackSByteCore(stream), ((int) stream.Position) - offset);
            }
        }

        private static sbyte UnpackSByteCore(Stream source)
        {
            sbyte num;
            using (Unpacker unpacker = Unpacker.Create(source, false))
            {
                UnpackOne(unpacker);
                VerifyIsScalar(unpacker);
                try
                {
                    num = (sbyte) unpacker.Data.Value;
                }
                catch (InvalidOperationException exception)
                {
                    throw NewTypeMismatchException(typeof(sbyte), exception);
                }
            }
            return num;
        }

        public static UnpackingResult<float> UnpackSingle(byte[] source)
        {
            return UnpackSingle(source, 0);
        }

        public static float UnpackSingle(Stream source)
        {
            ValidateStream(source);
            return UnpackSingleCore(source);
        }

        public static UnpackingResult<float> UnpackSingle(byte[] source, int offset)
        {
            ValidateByteArray(source, offset);
            using (MemoryStream stream = new MemoryStream(source))
            {
                stream.Position = offset;
                return new UnpackingResult<float>(UnpackSingleCore(stream), ((int) stream.Position) - offset);
            }
        }

        private static float UnpackSingleCore(Stream source)
        {
            float num;
            using (Unpacker unpacker = Unpacker.Create(source, false))
            {
                UnpackOne(unpacker);
                VerifyIsScalar(unpacker);
                try
                {
                    num = (float) unpacker.Data.Value;
                }
                catch (InvalidOperationException exception)
                {
                    throw NewTypeMismatchException(typeof(float), exception);
                }
            }
            return num;
        }

        public static UnpackingResult<string> UnpackString(byte[] source)
        {
            return UnpackString(source, 0);
        }

        public static string UnpackString(Stream source)
        {
            return UnpackString(source, MessagePackConvert.Utf8NonBomStrict);
        }

        public static UnpackingResult<string> UnpackString(byte[] source, int offset)
        {
            return UnpackString(source, offset, MessagePackConvert.Utf8NonBomStrict);
        }

        public static UnpackingResult<string> UnpackString(byte[] source, Encoding encoding)
        {
            return UnpackString(source, 0, encoding);
        }

        public static string UnpackString(Stream source, Encoding encoding)
        {
            string str;
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            try
            {
                byte[] bytes = UnpackBinary(source);
                str = encoding.GetString(bytes, 0, bytes.Length);
            }
            catch (DecoderFallbackException exception)
            {
                throw NewInvalidEncodingException(encoding, exception);
            }
            return str;
        }

        public static UnpackingResult<string> UnpackString(byte[] source, int offset, Encoding encoding)
        {
            UnpackingResult<string> result2;
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            try
            {
                UnpackingResult<byte[]> result = UnpackBinary(source, offset);
                string introduced4 = encoding.GetString(result.Value, 0, result.Value.Length);
                result2 = new UnpackingResult<string>(introduced4, result.ReadCount);
            }
            catch (DecoderFallbackException exception)
            {
                throw NewInvalidEncodingException(encoding, exception);
            }
            return result2;
        }

        [CLSCompliant(false)]
        public static UnpackingResult<ushort> UnpackUInt16(byte[] source)
        {
            return UnpackUInt16(source, 0);
        }

        [CLSCompliant(false)]
        public static ushort UnpackUInt16(Stream source)
        {
            ValidateStream(source);
            return UnpackUInt16Core(source);
        }

        [CLSCompliant(false)]
        public static UnpackingResult<ushort> UnpackUInt16(byte[] source, int offset)
        {
            ValidateByteArray(source, offset);
            using (MemoryStream stream = new MemoryStream(source))
            {
                stream.Position = offset;
                return new UnpackingResult<ushort>(UnpackUInt16Core(stream), ((int) stream.Position) - offset);
            }
        }

        private static ushort UnpackUInt16Core(Stream source)
        {
            ushort num;
            using (Unpacker unpacker = Unpacker.Create(source, false))
            {
                UnpackOne(unpacker);
                VerifyIsScalar(unpacker);
                try
                {
                    num = (ushort) unpacker.Data.Value;
                }
                catch (InvalidOperationException exception)
                {
                    throw NewTypeMismatchException(typeof(ushort), exception);
                }
            }
            return num;
        }

        [CLSCompliant(false)]
        public static UnpackingResult<uint> UnpackUInt32(byte[] source)
        {
            return UnpackUInt32(source, 0);
        }

        [CLSCompliant(false)]
        public static uint UnpackUInt32(Stream source)
        {
            ValidateStream(source);
            return UnpackUInt32Core(source);
        }

        [CLSCompliant(false)]
        public static UnpackingResult<uint> UnpackUInt32(byte[] source, int offset)
        {
            ValidateByteArray(source, offset);
            using (MemoryStream stream = new MemoryStream(source))
            {
                stream.Position = offset;
                return new UnpackingResult<uint>(UnpackUInt32Core(stream), ((int) stream.Position) - offset);
            }
        }

        private static uint UnpackUInt32Core(Stream source)
        {
            uint num;
            using (Unpacker unpacker = Unpacker.Create(source, false))
            {
                UnpackOne(unpacker);
                VerifyIsScalar(unpacker);
                try
                {
                    num = (uint) unpacker.Data.Value;
                }
                catch (InvalidOperationException exception)
                {
                    throw NewTypeMismatchException(typeof(uint), exception);
                }
            }
            return num;
        }

        [CLSCompliant(false)]
        public static UnpackingResult<ulong> UnpackUInt64(byte[] source)
        {
            return UnpackUInt64(source, 0);
        }

        [CLSCompliant(false)]
        public static ulong UnpackUInt64(Stream source)
        {
            ValidateStream(source);
            return UnpackUInt64Core(source);
        }

        [CLSCompliant(false)]
        public static UnpackingResult<ulong> UnpackUInt64(byte[] source, int offset)
        {
            ValidateByteArray(source, offset);
            using (MemoryStream stream = new MemoryStream(source))
            {
                stream.Position = offset;
                return new UnpackingResult<ulong>(UnpackUInt64Core(stream), ((int) stream.Position) - offset);
            }
        }

        private static ulong UnpackUInt64Core(Stream source)
        {
            ulong num;
            using (Unpacker unpacker = Unpacker.Create(source, false))
            {
                UnpackOne(unpacker);
                VerifyIsScalar(unpacker);
                try
                {
                    num = (ulong) unpacker.Data.Value;
                }
                catch (InvalidOperationException exception)
                {
                    throw NewTypeMismatchException(typeof(ulong), exception);
                }
            }
            return num;
        }

        private static void ValidateByteArray(byte[] source, int offset)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (source.Length == 0)
            {
                throw new ArgumentException("Source array is empty.", "source");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", "The offset cannot be negative.");
            }
            if (source.Length <= offset)
            {
                throw new ArgumentException("Source array is too small to the offset.", "source");
            }
        }

        private static void ValidateStream(Stream source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (!source.CanRead)
            {
                throw new ArgumentException("Stream is not readable.", "source");
            }
        }

        private static void VerifyIsScalar(Unpacker unpacker)
        {
            if (unpacker.IsArrayHeader || unpacker.IsMapHeader)
            {
                throw new MessageTypeException("The underlying stream is not scalar type.");
            }
        }

        private sealed class DefaultUnpackingStreamReader : UnpackingStreamReader
        {
            public DefaultUnpackingStreamReader(Stream stream, Encoding encoding, long byteLength) : base(stream, encoding, byteLength)
            {
            }
        }

        private sealed class SeekableUnpackingStream : UnpackingStream
        {
            private readonly long _initialPosition;

            public SeekableUnpackingStream(Stream underlying, long rawLength) : base(underlying, rawLength)
            {
                Contract.Assert(underlying.CanSeek);
                this._initialPosition = underlying.Position;
            }

            public sealed override long Seek(long offset, SeekOrigin origin)
            {
                switch (origin)
                {
                    case SeekOrigin.Begin:
                        this.SeekTo(offset);
                        break;

                    case SeekOrigin.Current:
                        this.SeekTo(this.Position + offset);
                        break;

                    case SeekOrigin.End:
                        this.SeekTo(base.RawLength + offset);
                        break;
                }
                return this.Position;
            }

            private void SeekTo(long position)
            {
                if (position < 0L)
                {
                    throw new IOException("An attempt was made to move the position before the beginning of the stream.");
                }
                if (position > base.RawLength)
                {
                    this.SetLength(position);
                }
                base.CurrentOffset = position;
                base.Underlying.Position = position + this._initialPosition;
            }

            public sealed override bool CanSeek
            {
                get
                {
                    return true;
                }
            }

            public sealed override long Position
            {
                get
                {
                    return base.CurrentOffset;
                }
                set
                {
                    this.SeekTo(value);
                }
            }
        }

        private sealed class UnseekableUnpackingStream : UnpackingStream
        {
            public UnseekableUnpackingStream(Stream underlying, long rawLength) : base(underlying, rawLength)
            {
            }

            public sealed override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotSupportedException();
            }

            public sealed override bool CanSeek
            {
                get
                {
                    return false;
                }
            }

            public sealed override long Position
            {
                get
                {
                    throw new NotSupportedException();
                }
                set
                {
                    throw new NotSupportedException();
                }
            }
        }
    }
}

