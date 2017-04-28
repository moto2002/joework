namespace MsgPack.Serialization
{
    using System;
    using System.Reflection;

    internal sealed class CollectionTraits
    {
        public readonly MethodInfo AddMethod;
        public readonly CollectionKind CollectionType;
        public readonly PropertyInfo CountProperty;
        public readonly Type ElementType;
        public readonly MethodInfo GetEnumeratorMethod;
        public static readonly CollectionTraits NotCollection = new CollectionTraits(CollectionKind.NotCollection, null, null, null, null);
        public static readonly CollectionTraits Unserializable = new CollectionTraits(CollectionKind.Unserializable, null, null, null, null);

        public CollectionTraits(CollectionKind type, MethodInfo addMethod, MethodInfo getEnumeratorMethod, PropertyInfo countProperty, Type elementType)
        {
            this.CollectionType = type;
            this.GetEnumeratorMethod = getEnumeratorMethod;
            this.AddMethod = addMethod;
            this.CountProperty = countProperty;
            this.ElementType = elementType;
        }
    }
}

