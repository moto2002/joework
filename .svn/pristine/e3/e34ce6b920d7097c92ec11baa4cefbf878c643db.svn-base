namespace MsgPack.Serialization
{
    using MsgPack;
    using System;
    using System.Globalization;
    using System.IO;

    public abstract class MessagePackSerializer<T> : IMessagePackSingleObjectSerializer, IMessagePackSerializer
    {
        private static readonly bool _isNullable;
        private static readonly string _memoryStreamExceptionSourceName;

        static MessagePackSerializer()
        {
            MessagePackSerializer<T>._isNullable = MessagePackSerializer<T>.JudgeNullable();
            MessagePackSerializer<T>._memoryStreamExceptionSourceName = typeof(MemoryStream).Assembly.GetName().Name;
        }

        protected MessagePackSerializer()
        {
        }

        private static bool JudgeNullable()
        {
            return (!typeof(T).GetIsValueType() || ((typeof(T) == typeof(MessagePackObject)) || (typeof(T).GetIsGenericType() && (typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>)))));
        }

        void IMessagePackSerializer.PackTo(Packer packer, object objectTree)
        {
            if (packer == null)
            {
                throw new ArgumentNullException("packer");
            }
            if (objectTree == null)
            {
                if (typeof(T).GetIsValueType() && !(typeof(T).GetIsGenericType() && (typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>))))
                {
                    throw SerializationExceptions.NewValueTypeCannotBeNull(typeof(T));
                }
                packer.PackNull();
            }
            else
            {
                if (!(objectTree is T))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "'{0}' is not compatible for '{1}'.", new object[] { objectTree.GetType(), typeof(T) }), "objectTree");
                }
                this.PackToCore(packer, (T) objectTree);
            }
        }

        object IMessagePackSerializer.UnpackFrom(Unpacker unpacker)
        {
            return this.UnpackFrom(unpacker);
        }

        void IMessagePackSerializer.UnpackTo(Unpacker unpacker, object collection)
        {
            if (unpacker == null)
            {
                throw new ArgumentNullException("unpacker");
            }
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            if (!(collection is T))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "'{0}' is not compatible for '{1}'.", new object[] { collection.GetType(), typeof(T) }), "collection");
            }
            if (!unpacker.Data.HasValue)
            {
                throw SerializationExceptions.NewEmptyOrUnstartedUnpacker();
            }
            this.UnpackToCore(unpacker, (T) collection);
        }

        byte[] IMessagePackSingleObjectSerializer.PackSingleObject(object objectTree)
        {
            if ((typeof(T).GetIsValueType() && !(objectTree is T)) || ((objectTree != null) && !(objectTree is T)))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "'{0}' is not compatible for '{1}'.", new object[] { (objectTree == null) ? "(null)" : objectTree.GetType().FullName, typeof(T) }), "objectTree");
            }
            return this.PackSingleObject((T) objectTree);
        }

        object IMessagePackSingleObjectSerializer.UnpackSingleObject(byte[] buffer)
        {
            return this.UnpackSingleObject(buffer);
        }

        public void Pack(Stream stream, T objectTree)
        {
            this.PackTo(Packer.Create(stream), objectTree);
        }

        public byte[] PackSingleObject(T objectTree)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                this.Pack(stream, objectTree);
                return stream.ToArray();
            }
        }

        public void PackTo(Packer packer, T objectTree)
        {
            if (packer == null)
            {
                throw new ArgumentNullException("packer");
            }
            if (objectTree == null)
            {
                packer.PackNull();
            }
            else
            {
                this.PackToCore(packer, objectTree);
            }
        }

        protected internal abstract void PackToCore(Packer packer, T objectTree);
        public T Unpack(Stream stream)
        {
            Unpacker unpacker = Unpacker.Create(stream);
            unpacker.Read();
            return this.UnpackFrom(unpacker);
        }

        public T UnpackFrom(Unpacker unpacker)
        {
            if (unpacker == null)
            {
                throw new ArgumentNullException("unpacker");
            }
            if (!unpacker.Data.HasValue)
            {
                throw SerializationExceptions.NewEmptyOrUnstartedUnpacker();
            }
            if (unpacker.Data.GetValueOrDefault().IsNil)
            {
                if (!MessagePackSerializer<T>._isNullable)
                {
                    throw SerializationExceptions.NewValueTypeCannotBeNull(typeof(T));
                }
                return default(T);
            }
            return this.UnpackFromCore(unpacker);
        }

        protected internal abstract T UnpackFromCore(Unpacker unpacker);
        public T UnpackSingleObject(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                return this.Unpack(stream);
            }
        }

        public void UnpackTo(Unpacker unpacker, T collection)
        {
            if (unpacker == null)
            {
                throw new ArgumentNullException("unpacker");
            }
            if (collection == null)
            {
                throw new ArgumentNullException("unpacker");
            }
            if (!unpacker.Data.HasValue)
            {
                throw SerializationExceptions.NewEmptyOrUnstartedUnpacker();
            }
            if (!unpacker.Data.Value.IsNil)
            {
                this.UnpackToCore(unpacker, collection);
            }
        }

        protected internal virtual void UnpackToCore(Unpacker unpacker, T collection)
        {
            throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "This operation is not supported by '{0}'.", new object[] { base.GetType() }));
        }
    }
}

