namespace MsgPack.Serialization.Reflection
{
    using MsgPack;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.CompilerServices;

    internal static class GenericTypeExtensions
    {
        private static readonly Type[] _emptyTypes = new Type[0];

        private static IEnumerable<Type> EnumerateGenericIntefaces(Type source, Type genericType, bool includesOwn)
        {
            return (from @interface in includesOwn ? new Type[] { source }.Concat<Type>(source.GetInterfaces()) : ((IEnumerable<Type>) source.GetInterfaces())
                where @interface.GetIsGenericType() && (genericType.GetIsGenericTypeDefinition() ? (@interface.GetGenericTypeDefinition() == genericType) : (@interface == genericType))
                select source.GetIsGenericTypeDefinition() ? ((IEnumerable<Type>) @interface.GetGenericTypeDefinition()) : ((IEnumerable<Type>) @interface));
        }

        public static string GetFullName(this Type source)
        {
            Contract.Assert(source != null);
            if (source.IsArray)
            {
                Type elementType = source.GetElementType();
                if (!elementType.GetIsGenericType())
                {
                    return source.FullName;
                }
                if (1 < source.GetArrayRank())
                {
                    return (elementType.GetFullName() + "[*]");
                }
                return (elementType.GetFullName() + "[]");
            }
            if (!source.GetIsGenericType())
            {
                return source.FullName;
            }
            return string.Concat(new object[] { source.Namespace, ReflectionAbstractions.TypeDelimiter, source.Name, '[', string.Join(", ", (from t in source.GetGenericArguments() select t.GetFullName()).ToArray<string>()), ']' });
        }

        public static string GetName(this Type source)
        {
            Contract.Assert(source != null);
            if (!source.GetIsGenericType())
            {
                return source.Name;
            }
            return string.Concat(new object[] { source.Name, '[', string.Join(", ", (from t in source.GetGenericArguments() select t.GetName()).ToArray<string>()), ']' });
        }

        public static bool Implements(this Type source, Type genericType)
        {
            Contract.Assert(source != null);
            Contract.Assert(genericType != null);
            Contract.Assert(genericType.GetIsInterface());
            return EnumerateGenericIntefaces(source, genericType, false).Any<Type>();
        }
    }
}

