namespace MsgPack
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    internal static class ReflectionAbstractions
    {
        public static readonly Type[] EmptyTypes = new Type[0];
        public static readonly char TypeDelimiter = '.';

        public static Assembly GetAssembly(this Type source)
        {
            return source.Assembly;
        }

        public static bool GetContainsGenericParameters(this Type source)
        {
            return source.ContainsGenericParameters;
        }

        public static T GetCustomAttribute<T>(this MemberInfo source) where T: Attribute
        {
            return (Attribute.GetCustomAttribute(source, typeof(T)) as T);
        }

        public static bool GetIsAbstract(this Type source)
        {
            return source.IsAbstract;
        }

        public static bool GetIsEnum(this Type source)
        {
            return source.IsEnum;
        }

        public static bool GetIsGenericType(this Type source)
        {
            return source.IsGenericType;
        }

        public static bool GetIsGenericTypeDefinition(this Type source)
        {
            return source.IsGenericTypeDefinition;
        }

        public static bool GetIsInterface(this Type source)
        {
            return source.IsInterface;
        }

        public static bool GetIsPublic(this Type source)
        {
            return source.IsPublic;
        }

        public static bool GetIsValueType(this Type source)
        {
            return source.IsValueType;
        }

        public static bool IsDefined(this MemberInfo source, Type attributeType)
        {
            return Attribute.IsDefined(source, attributeType);
        }
    }
}

