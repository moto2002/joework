namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Collections.Specialized;
    using System.Runtime.Serialization;

    internal sealed class System_Collections_Specialized_NameValueCollectionMessagePackSerializer : MessagePackSerializer<NameValueCollection>
    {
        protected internal sealed override void PackToCore(Packer packer, NameValueCollection objectTree)
        {
            if (objectTree == null)
            {
                packer.PackNull();
            }
            else
            {
                packer.PackMapHeader(objectTree.Count);
                foreach (string str in objectTree)
                {
                    if (str == null)
                    {
                        throw new NotSupportedException("null key is not supported.");
                    }
                    string[] values = objectTree.GetValues(str);
                    if (values != null)
                    {
                        packer.PackString(str);
                        packer.PackArrayHeader(values.Length);
                        foreach (string str2 in values)
                        {
                            packer.PackString(str2);
                        }
                    }
                }
            }
        }

        protected internal sealed override NameValueCollection UnpackFromCore(Unpacker unpacker)
        {
            NameValueCollection values = new NameValueCollection((int) unpacker.ItemsCount);
            while (unpacker.Read())
            {
                string name = unpacker.Data.Value.DeserializeAsString();
                if (!unpacker.Read())
                {
                    throw SerializationExceptions.NewUnexpectedEndOfStream();
                }
                if (!unpacker.IsArrayHeader)
                {
                    throw new SerializationException("Invalid NameValueCollection value.");
                }
                using (Unpacker unpacker2 = unpacker.ReadSubtree())
                {
                    while (unpacker2.Read())
                    {
                        values.Add(name, unpacker.Data.Value.DeserializeAsString());
                    }
                }
            }
            return values;
        }
    }
}

