namespace MsgPack.Serialization.DefaultSerializers
{
    using MsgPack;
    using MsgPack.Serialization;
    using System;
    using System.Linq;
    using System.Reflection;

    internal static class ArraySegmentMessageSerializer
    {
        public static readonly MethodInfo PackByteArraySegmentToMethod = FromExpression.ToMethod<Packer, ArraySegment<byte>, MessagePackSerializer<byte>>((packer, objectTree, itemSerializer) => PackByteArraySegmentTo(packer, objectTree, itemSerializer));
        public static readonly MethodInfo PackCharArraySegmentToMethod = FromExpression.ToMethod<Packer, ArraySegment<char>, MessagePackSerializer<char>>((packer, objectTree, itemSerializer) => PackCharArraySegmentTo(packer, objectTree, itemSerializer));
        public static readonly MethodInfo PackGenericArraySegmentTo1Method = typeof(ArraySegmentMessageSerializer).GetMethod("PackGenericArraySegmentTo");
        public static readonly MethodInfo UnpackByteArraySegmentFromMethod = FromExpression.ToMethod<Unpacker, MessagePackSerializer<byte>, ArraySegment<byte>>((unpacker, itemSerializer) => UnpackByteArraySegmentFrom(unpacker, itemSerializer));
        public static readonly MethodInfo UnpackCharArraySegmentFromMethod = FromExpression.ToMethod<Unpacker, MessagePackSerializer<char>, ArraySegment<char>>((unpacker, itemSerializer) => UnpackCharArraySegmentFrom(unpacker, itemSerializer));
        public static readonly MethodInfo UnpackGenericArraySegmentFrom1Method = typeof(ArraySegmentMessageSerializer).GetMethod("UnpackGenericArraySegmentFrom");

        public static void PackByteArraySegmentTo(Packer packer, ArraySegment<byte> objectTree, MessagePackSerializer<byte> itemSerializer)
        {
            if (objectTree.Array == null)
            {
                packer.PackRawHeader(0);
            }
            else
            {
                packer.PackRawHeader(objectTree.Count);
                packer.PackRawBody(objectTree.Array.Skip<byte>(objectTree.Offset).Take<byte>(objectTree.Count));
            }
        }

        public static void PackCharArraySegmentTo(Packer packer, ArraySegment<char> objectTree, MessagePackSerializer<char> itemSerializer)
        {
            packer.PackRawHeader(objectTree.Count);
            packer.PackRawBody(MessagePackConvert.EncodeString(new string(objectTree.Array.Skip<char>(objectTree.Offset).Take<char>(objectTree.Count).ToArray<char>())));
        }

        public static void PackGenericArraySegmentTo<T>(Packer packer, ArraySegment<T> objectTree, MessagePackSerializer<T> itemSerializer)
        {
            packer.PackArrayHeader(objectTree.Count);
            for (int i = 0; i < objectTree.Count; i++)
            {
                itemSerializer.PackTo(packer, objectTree.Array[i + objectTree.Offset]);
            }
        }

        public static ArraySegment<byte> UnpackByteArraySegmentFrom(Unpacker unpacker, MessagePackSerializer<byte> itemSerializer)
        {
            return new ArraySegment<byte>(unpacker.Data.Value.AsBinary());
        }

        public static ArraySegment<char> UnpackCharArraySegmentFrom(Unpacker unpacker, MessagePackSerializer<char> itemSerializer)
        {
            return new ArraySegment<char>(unpacker.Data.Value.AsCharArray());
        }

        public static ArraySegment<T> UnpackGenericArraySegmentFrom<T>(Unpacker unpacker, MessagePackSerializer<T> itemSerializer)
        {
            T[] array = new T[unpacker.ItemsCount];
            for (int i = 0; i < array.Length; i++)
            {
                if (!unpacker.Read())
                {
                    throw SerializationExceptions.NewMissingItem(i);
                }
                array[i] = itemSerializer.UnpackFrom(unpacker);
            }
            return new ArraySegment<T>(array);
        }
    }
}

