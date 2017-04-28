namespace MsgPack.Serialization.Metadata
{
    using MsgPack;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;

    internal static class _Unpacker
    {
        private static readonly Dictionary<Type, MethodInfo> _directReadMethods;
        public static readonly PropertyInfo Data = FromExpression.ToProperty<Unpacker, MessagePackObject?>(unpacker => unpacker.Data);
        public static readonly PropertyInfo IsArrayHeader = FromExpression.ToProperty<Unpacker, bool>(unpacker => unpacker.IsArrayHeader);
        public static readonly PropertyInfo IsMapHeader = FromExpression.ToProperty<Unpacker, bool>(unpacker => unpacker.IsMapHeader);
        public static readonly PropertyInfo ItemsCount = FromExpression.ToProperty<Unpacker, long>(unpacker => unpacker.ItemsCount);
        public static readonly MethodInfo Read = FromExpression.ToMethod<Unpacker, bool>(unpacker => unpacker.Read());
        public static readonly MethodInfo ReadBinary;
        public static readonly MethodInfo ReadBoolean;
        public static readonly MethodInfo ReadByte;
        public static readonly MethodInfo ReadDouble;
        public static readonly MethodInfo ReadInt16;
        public static readonly MethodInfo ReadInt32;
        public static readonly MethodInfo ReadInt64;
        public static readonly MethodInfo ReadNullableBoolean;
        public static readonly MethodInfo ReadNullableByte;
        public static readonly MethodInfo ReadNullableDouble;
        public static readonly MethodInfo ReadNullableInt16;
        public static readonly MethodInfo ReadNullableInt32;
        public static readonly MethodInfo ReadNullableInt64;
        public static readonly MethodInfo ReadNullableSByte;
        public static readonly MethodInfo ReadNullableSingle;
        public static readonly MethodInfo ReadNullableUInt16;
        public static readonly MethodInfo ReadNullableUInt32;
        public static readonly MethodInfo ReadNullableUInt64;
        public static readonly MethodInfo ReadObject;
        public static readonly MethodInfo ReadSByte;
        public static readonly MethodInfo ReadSingle;
        public static readonly MethodInfo ReadString;
        public static readonly MethodInfo ReadSubtree = FromExpression.ToMethod<Unpacker, Unpacker>(unpacker => unpacker.ReadSubtree());
        public static readonly MethodInfo ReadUInt16;
        public static readonly MethodInfo ReadUInt32;
        public static readonly MethodInfo ReadUInt64;
        public static readonly MethodInfo UnpackSubtree = FromExpression.ToMethod<Unpacker, MessagePackObject?>(unpacker => unpacker.UnpackSubtree());

        static _Unpacker()
        {
            Dictionary<Type, MethodInfo> dictionary = new Dictionary<Type, MethodInfo>(0x19);
            ReadSByte = typeof(Unpacker).GetMethod("ReadSByte");
            ReadNullableSByte = typeof(Unpacker).GetMethod("ReadNullableSByte");
            dictionary.Add(typeof(sbyte), ReadNullableSByte);
            dictionary.Add(typeof(sbyte?), ReadNullableSByte);
            ReadInt16 = typeof(Unpacker).GetMethod("ReadInt16");
            ReadNullableInt16 = typeof(Unpacker).GetMethod("ReadNullableInt16");
            dictionary.Add(typeof(short), ReadNullableInt16);
            dictionary.Add(typeof(short?), ReadNullableInt16);
            ReadInt32 = typeof(Unpacker).GetMethod("ReadInt32");
            ReadNullableInt32 = typeof(Unpacker).GetMethod("ReadNullableInt32");
            dictionary.Add(typeof(int), ReadNullableInt32);
            dictionary.Add(typeof(int?), ReadNullableInt32);
            ReadInt64 = typeof(Unpacker).GetMethod("ReadInt64");
            ReadNullableInt64 = typeof(Unpacker).GetMethod("ReadNullableInt64");
            dictionary.Add(typeof(long), ReadNullableInt64);
            dictionary.Add(typeof(long?), ReadNullableInt64);
            ReadByte = typeof(Unpacker).GetMethod("ReadByte");
            ReadNullableByte = typeof(Unpacker).GetMethod("ReadNullableByte");
            dictionary.Add(typeof(byte), ReadNullableByte);
            dictionary.Add(typeof(byte?), ReadNullableByte);
            ReadUInt16 = typeof(Unpacker).GetMethod("ReadUInt16");
            ReadNullableUInt16 = typeof(Unpacker).GetMethod("ReadNullableUInt16");
            dictionary.Add(typeof(ushort), ReadNullableUInt16);
            dictionary.Add(typeof(ushort?), ReadNullableUInt16);
            ReadUInt32 = typeof(Unpacker).GetMethod("ReadUInt32");
            ReadNullableUInt32 = typeof(Unpacker).GetMethod("ReadNullableUInt32");
            dictionary.Add(typeof(uint), ReadNullableUInt32);
            dictionary.Add(typeof(uint?), ReadNullableUInt32);
            ReadUInt64 = typeof(Unpacker).GetMethod("ReadUInt64");
            ReadNullableUInt64 = typeof(Unpacker).GetMethod("ReadNullableUInt64");
            dictionary.Add(typeof(ulong), ReadNullableUInt64);
            dictionary.Add(typeof(ulong?), ReadNullableUInt64);
            ReadSingle = typeof(Unpacker).GetMethod("ReadSingle");
            ReadNullableSingle = typeof(Unpacker).GetMethod("ReadNullableSingle");
            dictionary.Add(typeof(float), ReadNullableSingle);
            dictionary.Add(typeof(float?), ReadNullableSingle);
            ReadDouble = typeof(Unpacker).GetMethod("ReadDouble");
            ReadNullableDouble = typeof(Unpacker).GetMethod("ReadNullableDouble");
            dictionary.Add(typeof(double), ReadNullableDouble);
            dictionary.Add(typeof(double?), ReadNullableDouble);
            ReadBoolean = typeof(Unpacker).GetMethod("ReadBoolean");
            ReadNullableBoolean = typeof(Unpacker).GetMethod("ReadNullableBoolean");
            dictionary.Add(typeof(bool), ReadNullableBoolean);
            dictionary.Add(typeof(bool?), ReadNullableBoolean);
            ReadString = typeof(Unpacker).GetMethod("ReadString");
            dictionary.Add(typeof(string), ReadString);
            ReadBinary = typeof(Unpacker).GetMethod("ReadBinary");
            dictionary.Add(typeof(byte[]), ReadBinary);
            ReadObject = typeof(Unpacker).GetMethod("ReadObject");
            dictionary.Add(typeof(MessagePackObject), ReadObject);
            Interlocked.Exchange<Dictionary<Type, MethodInfo>>(ref _directReadMethods, dictionary);
        }

        public static MethodInfo GetDirectReadMethod(Type type)
        {
            MethodInfo info;
            _directReadMethods.TryGetValue(type, out info);
            return info;
        }
    }
}

