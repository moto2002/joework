namespace MsgPack.Serialization.EmittingSerializers
{
    using MsgPack;
    using MsgPack.Serialization.Reflection;
    using System;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    internal sealed class LocalVariableHolder
    {
        private readonly Dictionary<Type, LocalBuilder> _catchedExceptions = new Dictionary<Type, LocalBuilder>();
        private readonly Dictionary<Type, LocalBuilder> _deserializedValues = new Dictionary<Type, LocalBuilder>();
        private readonly Dictionary<Type, LocalBuilder> _deserializingCollectios = new Dictionary<Type, LocalBuilder>();
        private readonly TracingILGenerator _il;
        private LocalBuilder _isDeserializationSucceeded;
        private LocalBuilder _itemsCount;
        private LocalBuilder _memberName;
        private LocalBuilder _packingCollectionCount;
        private LocalBuilder _rawItemsCount;
        private readonly Dictionary<Type, LocalBuilder> _serializingCollectionItems = new Dictionary<Type, LocalBuilder>();
        private readonly Dictionary<Type, LocalBuilder> _serializingCollections = new Dictionary<Type, LocalBuilder>();
        private readonly Dictionary<Type, LocalBuilder> _serializingValues = new Dictionary<Type, LocalBuilder>();
        private LocalBuilder _subtreeUnpacker;
        private LocalBuilder _unpackedData;
        private LocalBuilder _unpackedDataValue;

        public LocalVariableHolder(TracingILGenerator il)
        {
            this._il = il;
        }

        public LocalBuilder GetCatchedException(Type type)
        {
            LocalBuilder builder;
            if (!this._catchedExceptions.TryGetValue(type, out builder))
            {
                builder = this._il.DeclareLocal(type, "ex");
                this._catchedExceptions[type] = builder;
            }
            return builder;
        }

        public LocalBuilder GetDeserializedValue(Type type)
        {
            LocalBuilder builder;
            if (!this._deserializedValues.TryGetValue(type, out builder))
            {
                builder = this._il.DeclareLocal(type, "deserializedValue");
                this._deserializedValues[type] = builder;
            }
            return builder;
        }

        public LocalBuilder GetDeserializingCollection(Type type)
        {
            LocalBuilder builder;
            if (!this._deserializingCollectios.TryGetValue(type, out builder))
            {
                builder = this._il.DeclareLocal(type, type.IsArray ? "array" : "collection");
                this._deserializingCollectios[type] = builder;
            }
            return builder;
        }

        public LocalBuilder GetSerializingCollection(Type type)
        {
            LocalBuilder builder;
            if (!this._serializingCollections.TryGetValue(type, out builder))
            {
                builder = this._il.DeclareLocal(type, type.IsArray ? "array" : "collection");
                this._serializingCollections[type] = builder;
            }
            return builder;
        }

        public LocalBuilder GetSerializingCollectionItem(Type type)
        {
            LocalBuilder builder;
            if (!this._serializingCollectionItems.TryGetValue(type, out builder))
            {
                builder = this._il.DeclareLocal(type, "item");
                this._serializingCollectionItems[type] = builder;
            }
            return builder;
        }

        public LocalBuilder GetSerializingValue(Type type)
        {
            LocalBuilder builder;
            if (!this._serializingValues.TryGetValue(type, out builder))
            {
                builder = this._il.DeclareLocal(type, "serializingValue");
                this._serializingValues[type] = builder;
            }
            return builder;
        }

        public LocalBuilder IsDeserializationSucceeded
        {
            get
            {
                if (this._isDeserializationSucceeded == null)
                {
                    this._isDeserializationSucceeded = this._il.DeclareLocal(typeof(bool), "isDeserializationSucceeded");
                }
                return this._isDeserializationSucceeded;
            }
        }

        public LocalBuilder ItemsCount
        {
            get
            {
                if (this._itemsCount == null)
                {
                    this._itemsCount = this._il.DeclareLocal(typeof(int), "itemsCount");
                }
                return this._itemsCount;
            }
        }

        public LocalBuilder MemberName
        {
            get
            {
                if (this._memberName == null)
                {
                    this._memberName = this._il.DeclareLocal(typeof(string), "memberName");
                }
                return this._memberName;
            }
        }

        public LocalBuilder PackingCollectionCount
        {
            get
            {
                if (this._packingCollectionCount == null)
                {
                    this._packingCollectionCount = this._il.DeclareLocal(typeof(int), "packingCollectionCount");
                }
                return this._packingCollectionCount;
            }
        }

        public LocalBuilder RawItemsCount
        {
            get
            {
                if (this._rawItemsCount == null)
                {
                    this._rawItemsCount = this._il.DeclareLocal(typeof(long), "rawItemsCount");
                }
                return this._rawItemsCount;
            }
        }

        public LocalBuilder SubtreeUnpacker
        {
            get
            {
                if (this._subtreeUnpacker == null)
                {
                    this._subtreeUnpacker = this._il.DeclareLocal(typeof(Unpacker), "subtreeUnpacker");
                }
                return this._subtreeUnpacker;
            }
        }

        public LocalBuilder UnpackedData
        {
            get
            {
                if (this._unpackedData == null)
                {
                    this._unpackedData = this._il.DeclareLocal(typeof(MessagePackObject?), "unpackedData");
                }
                return this._unpackedData;
            }
        }

        public LocalBuilder UnpackedDataValue
        {
            get
            {
                if (this._unpackedDataValue == null)
                {
                    this._unpackedDataValue = this._il.DeclareLocal(typeof(MessagePackObject), "unpackedDataValue");
                }
                return this._unpackedDataValue;
            }
        }
    }
}

