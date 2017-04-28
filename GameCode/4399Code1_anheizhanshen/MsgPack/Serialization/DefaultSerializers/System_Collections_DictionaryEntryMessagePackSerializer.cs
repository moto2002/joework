namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Collections;

    internal sealed class System_Collections_DictionaryEntryMessagePackSerializer : MessagePackSerializer<DictionaryEntry>
    {
        private static MessagePackObject EnsureMessagePackObject(object obj)
        {
            if (obj == null)
            {
                return MessagePackObject.Nil;
            }
            if (!(obj is MessagePackObject))
            {
                throw new NotSupportedException("Only MessagePackObject Key/Value is supported.");
            }
            return (MessagePackObject) obj;
        }

        protected internal sealed override void PackToCore(Packer packer, DictionaryEntry objectTree)
        {
            packer.PackMapHeader(2);
            packer.PackString("Key");
            packer.Pack<MessagePackObject>(EnsureMessagePackObject(objectTree.Key));
            packer.PackString("Value");
            packer.Pack<MessagePackObject>(EnsureMessagePackObject(objectTree.Value));
        }

        protected internal sealed override DictionaryEntry UnpackFromCore(Unpacker unpacker)
        {
            object key = null;
            object obj3 = null;
            bool flag = false;
            bool flag2 = false;
            while (unpacker.Read())
            {
                if (!unpacker.Data.HasValue)
                {
                    throw SerializationExceptions.NewUnexpectedEndOfStream();
                }
                string str = unpacker.Data.Value.DeserializeAsString();
                if (str != null)
                {
                    if (!(str == "Key"))
                    {
                        if (str == "Value")
                        {
                            goto Label_0090;
                        }
                    }
                    else
                    {
                        if (!unpacker.Read())
                        {
                            throw SerializationExceptions.NewUnexpectedEndOfStream();
                        }
                        flag = true;
                        key = unpacker.Data.Value;
                    }
                }
                continue;
            Label_0090:
                if (!unpacker.Read())
                {
                    throw SerializationExceptions.NewUnexpectedEndOfStream();
                }
                flag2 = true;
                obj3 = unpacker.Data.Value;
            }
            if (!flag)
            {
                throw SerializationExceptions.NewMissingProperty("Key");
            }
            if (!flag2)
            {
                throw SerializationExceptions.NewMissingProperty("Value");
            }
            return new DictionaryEntry(key, obj3);
        }
    }
}

