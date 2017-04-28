namespace MsgPack.Serialization
{
    using MsgPack;
    using MsgPack.Serialization.DefaultSerializers;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Runtime.InteropServices.ComTypes;
    using System.Text;

    public sealed class SerializerRepository : IDisposable
    {
        private static readonly SerializerRepository _default = new SerializerRepository(InitializeDefaultTable());
        private readonly TypeKeyRepository _repository;

        public SerializerRepository()
        {
            this._repository = new TypeKeyRepository();
        }

        public SerializerRepository(SerializerRepository copiedFrom)
        {
            if (copiedFrom == null)
            {
                throw new ArgumentNullException("copiedFrom");
            }
            this._repository = new TypeKeyRepository(copiedFrom._repository);
        }

        private SerializerRepository(Dictionary<RuntimeTypeHandle, object> table)
        {
            this._repository = new TypeKeyRepository(table);
            this._repository.Freeze();
        }

        public void Dispose()
        {
            this._repository.Dispose();
        }

        public MessagePackSerializer<T> Get<T>(SerializationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (typeof(T).GetIsEnum())
            {
                return new EnumMessagePackSerializer<T>();
            }
            if (typeof(T).GetIsGenericType() && (typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                return new NullableMessagePackSerializer<T>(context);
            }
            return this._repository.Get<T, MessagePackSerializer<T>>(context);
        }

        private static Dictionary<RuntimeTypeHandle, object> InitializeDefaultTable()
        {
            Dictionary<RuntimeTypeHandle, object> dictionary = new Dictionary<RuntimeTypeHandle, object>(0x1ac);
            dictionary.Add(typeof(MessagePackObject).TypeHandle, new MsgPack_MessagePackObjectMessagePackSerializer());
            dictionary.Add(typeof(object).TypeHandle, new System_ObjectMessagePackSerializer());
            dictionary.Add(typeof(string).TypeHandle, new System_StringMessagePackSerializer());
            dictionary.Add(typeof(StringBuilder).TypeHandle, new System_Text_StringBuilderMessagePackSerializer());
            dictionary.Add(typeof(char[]).TypeHandle, new System_CharArrayMessagePackSerializer());
            dictionary.Add(typeof(byte[]).TypeHandle, new System_ByteArrayMessagePackSerializer());
            dictionary.Add(typeof(DateTime).TypeHandle, new System_DateTimeMessagePackSerializer());
            dictionary.Add(typeof(DateTimeOffset).TypeHandle, new System_DateTimeOffsetMessagePackSerializer());
            dictionary.Add(typeof(bool).TypeHandle, new System_BooleanMessagePackSerializer());
            dictionary.Add(typeof(byte).TypeHandle, new System_ByteMessagePackSerializer());
            dictionary.Add(typeof(char).TypeHandle, new System_CharMessagePackSerializer());
            dictionary.Add(typeof(decimal).TypeHandle, new System_DecimalMessagePackSerializer());
            dictionary.Add(typeof(double).TypeHandle, new System_DoubleMessagePackSerializer());
            dictionary.Add(typeof(Guid).TypeHandle, new System_GuidMessagePackSerializer());
            dictionary.Add(typeof(short).TypeHandle, new System_Int16MessagePackSerializer());
            dictionary.Add(typeof(int).TypeHandle, new System_Int32MessagePackSerializer());
            dictionary.Add(typeof(long).TypeHandle, new System_Int64MessagePackSerializer());
            dictionary.Add(typeof(sbyte).TypeHandle, new System_SByteMessagePackSerializer());
            dictionary.Add(typeof(float).TypeHandle, new System_SingleMessagePackSerializer());
            dictionary.Add(typeof(TimeSpan).TypeHandle, new System_TimeSpanMessagePackSerializer());
            dictionary.Add(typeof(ushort).TypeHandle, new System_UInt16MessagePackSerializer());
            dictionary.Add(typeof(uint).TypeHandle, new System_UInt32MessagePackSerializer());
            dictionary.Add(typeof(ulong).TypeHandle, new System_UInt64MessagePackSerializer());
            dictionary.Add(typeof(FILETIME).TypeHandle, new System_Runtime_InteropServices_ComTypes_FILETIMEMessagePackSerializer());
            dictionary.Add(typeof(BitVector32).TypeHandle, new System_Collections_Specialized_BitVector32MessagePackSerializer());
            dictionary.Add(typeof(ArraySegment<>).TypeHandle, typeof(System_ArraySegment_1MessagePackSerializer<>));
            dictionary.Add(typeof(DictionaryEntry).TypeHandle, new System_Collections_DictionaryEntryMessagePackSerializer());
            dictionary.Add(typeof(KeyValuePair<,>).TypeHandle, typeof(System_Collections_Generic_KeyValuePair_2MessagePackSerializer<,>));
            dictionary.Add(typeof(Uri).TypeHandle, new System_UriMessagePackSerializer());
            dictionary.Add(typeof(Version).TypeHandle, new System_VersionMessagePackSerializer());
            dictionary.Add(typeof(NameValueCollection).TypeHandle, new System_Collections_Specialized_NameValueCollectionMessagePackSerializer());
            return dictionary;
        }

        public bool Register<T>(MessagePackSerializer<T> serializer)
        {
            if (serializer == null)
            {
                throw new ArgumentNullException("serializer");
            }
            return this._repository.Register<T>(serializer);
        }

        public static SerializerRepository Default
        {
            get
            {
                return _default;
            }
        }
    }
}

