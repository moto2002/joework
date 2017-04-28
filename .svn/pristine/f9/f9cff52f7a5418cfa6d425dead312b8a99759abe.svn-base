namespace MsgPack.Serialization
{
    using MsgPack;
    using MsgPack.Serialization.Reflection;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    internal static class ReflectionExtensions
    {
        private static readonly PropertyInfo _icollectionCount = FromExpression.ToProperty<ICollection, int>(value => value.Count);

        public static bool CanSetValue(this MemberInfo source)
        {
            PropertyInfo info = source as PropertyInfo;
            FieldInfo info2 = source as FieldInfo;
            return ((info != null) ? ((info.CanWrite && (info.GetSetMethod() != null)) && info.GetSetMethod().IsPublic) : !info2.IsInitOnly);
        }

        private static bool FilterCollectionType(Type type, object filterCriteria)
        {
            Contract.Assert(type.IsInterface);
            return ((type.Assembly == typeof(Array).Assembly) && ((type.Namespace == "System.Collections") || (type.Namespace == "System.Collections.Generic")));
        }

        private static MethodInfo FindInterfaceMethod(Type targetType, Type interfaceType, string name, Type[] parameterTypes)
        {
            TypeFilter filter = null;
            if (targetType.GetIsInterface())
            {
                if (filter == null)
                {
                    filter = (type, _) => type == interfaceType;
                }
                return targetType.FindInterfaces(filter, null).Single<Type>().GetMethod(name, parameterTypes);
            }
            InterfaceMapping interfaceMap = targetType.GetInterfaceMap(interfaceType);
            int index = Array.FindIndex<MethodInfo>(interfaceMap.InterfaceMethods, method => (method.Name == name) && (from p in method.GetParameters() select p.ParameterType).SequenceEqual<Type>(parameterTypes));
            if (index < 0)
            {
                Contract.Assert(false, string.Concat(new object[] { interfaceType, "::", name, "(", string.Join(", ", (from t in parameterTypes select t.ToString()).ToArray<string>()), ") is not found in ", targetType }));
                return null;
            }
            return interfaceMap.TargetMethods[index];
        }

        private static MethodInfo GetAddMethod(Type targetType, Type argumentType)
        {
            Type[] types = new Type[] { argumentType };
            MethodInfo method = targetType.GetMethod("Add", types);
            if (method != null)
            {
                return method;
            }
            Type target = typeof(ICollection<>).MakeGenericType(new Type[] { argumentType });
            if (targetType.IsAssignableTo(target))
            {
                return target.GetMethod("Add", types);
            }
            if (targetType.IsAssignableTo(typeof(IList)))
            {
                return typeof(IList).GetMethod("Add", new Type[] { typeof(object) });
            }
            return null;
        }

        private static MethodInfo GetAddMethod(Type targetType, Type keyType, Type valueType)
        {
            Type[] types = new Type[] { keyType, valueType };
            MethodInfo method = targetType.GetMethod("Add", types);
            if (method != null)
            {
                return method;
            }
            return typeof(IDictionary<,>).MakeGenericType(types).GetMethod("Add", types);
        }

        private static PropertyInfo GetCollectionTCountProperty(Type targetType, Type elementType)
        {
            if (!(targetType.GetIsValueType() || !targetType.Implements(typeof(ICollection<>))))
            {
                return typeof(ICollection<>).MakeGenericType(new Type[] { elementType }).GetProperty("Count");
            }
            PropertyInfo property = targetType.GetProperty("Count");
            if (((property != null) && (property.PropertyType == typeof(int))) && (property.GetIndexParameters().Length == 0))
            {
                return property;
            }
            return null;
        }

        public static CollectionTraits GetCollectionTraits(this Type source)
        {
            Contract.Assert(!source.GetContainsGenericParameters());
            if (source.IsAssignableTo(typeof(IEnumerable)))
            {
                Type type2;
                MethodInfo addMethod;
                if (source.IsArray)
                {
                    return new CollectionTraits(CollectionKind.Array, null, null, null, source.GetElementType());
                }
                MethodInfo method = source.GetMethod("GetEnumerator", ReflectionAbstractions.EmptyTypes);
                if ((method != null) && method.ReturnType.IsAssignableTo(typeof(IEnumerator)))
                {
                    Type type;
                    if (source.Implements(typeof(IDictionary<,>)))
                    {
                        type = method.ReturnType.GetInterfaces().FirstOrDefault<Type>(@interface => @interface.GetIsGenericType() && (@interface.GetGenericTypeDefinition() == typeof(IEnumerator<>)));
                        if (type != null)
                        {
                            type2 = type.GetGenericArguments()[0];
                            return new CollectionTraits(CollectionKind.Map, GetAddMethod(source, type2.GetGenericArguments()[0], type2.GetGenericArguments()[1]), method, GetDictionaryCountProperty(source, type2.GetGenericArguments()[0], type2.GetGenericArguments()[1]), type2);
                        }
                    }
                    if (source.IsAssignableTo(typeof(IDictionary)))
                    {
                        return new CollectionTraits(CollectionKind.Map, GetAddMethod(source, typeof(object), typeof(object)), method, _icollectionCount, typeof(DictionaryEntry));
                    }
                    type = method.ReturnType.GetInterfaces().FirstOrDefault<Type>(@interface => @interface.GetIsGenericType() && (@interface.GetGenericTypeDefinition() == typeof(IEnumerator<>)));
                    if (type != null)
                    {
                        type2 = type.GetGenericArguments()[0];
                        return new CollectionTraits(CollectionKind.Array, GetAddMethod(source, type2), method, null, type2);
                    }
                }
                Type interfaceType = null;
                Type type4 = null;
                Type type5 = null;
                Type type6 = null;
                foreach (Type type7 in source.FindInterfaces(new TypeFilter(MsgPack.Serialization.ReflectionExtensions.FilterCollectionType), null))
                {
                    if (type7 == typeof(IDictionary<MessagePackObject, MessagePackObject>))
                    {
                        return new CollectionTraits(CollectionKind.Map, GetAddMethod(source, typeof(MessagePackObject), typeof(MessagePackObject)), FindInterfaceMethod(source, typeof(IEnumerable<KeyValuePair<MessagePackObject, MessagePackObject>>), "GetEnumerator", ReflectionAbstractions.EmptyTypes), GetDictionaryCountProperty(source, typeof(MessagePackObject), typeof(MessagePackObject)), typeof(KeyValuePair<MessagePackObject, MessagePackObject>));
                    }
                    if (type7 == typeof(IEnumerable<MessagePackObject>))
                    {
                        addMethod = GetAddMethod(source, typeof(MessagePackObject));
                        if (addMethod != null)
                        {
                            return new CollectionTraits(CollectionKind.Array, addMethod, FindInterfaceMethod(source, typeof(IEnumerable<MessagePackObject>), "GetEnumerator", ReflectionAbstractions.EmptyTypes), GetCollectionTCountProperty(source, typeof(MessagePackObject)), typeof(MessagePackObject));
                        }
                    }
                    if (type7.GetIsGenericType())
                    {
                        Type genericTypeDefinition = type7.GetGenericTypeDefinition();
                        if (genericTypeDefinition == typeof(IDictionary<,>))
                        {
                            if (type4 != null)
                            {
                                return CollectionTraits.Unserializable;
                            }
                            type4 = type7;
                        }
                        else if (genericTypeDefinition == typeof(IEnumerable<>))
                        {
                            if (interfaceType != null)
                            {
                                return CollectionTraits.Unserializable;
                            }
                            interfaceType = type7;
                        }
                    }
                    else if (type7 == typeof(IDictionary))
                    {
                        type6 = type7;
                    }
                    else if (type7 == typeof(IEnumerable))
                    {
                        type5 = type7;
                    }
                }
                if (type4 != null)
                {
                    type2 = typeof(KeyValuePair<,>).MakeGenericType(type4.GetGenericArguments());
                    return new CollectionTraits(CollectionKind.Map, GetAddMethod(source, type4.GetGenericArguments()[0], type4.GetGenericArguments()[1]), FindInterfaceMethod(source, typeof(IEnumerable<>).MakeGenericType(new Type[] { type2 }), "GetEnumerator", ReflectionAbstractions.EmptyTypes), GetDictionaryCountProperty(source, type4.GetGenericArguments()[0], type4.GetGenericArguments()[1]), type2);
                }
                if (interfaceType != null)
                {
                    type2 = interfaceType.GetGenericArguments()[0];
                    return new CollectionTraits(CollectionKind.Array, GetAddMethod(source, type2), FindInterfaceMethod(source, interfaceType, "GetEnumerator", ReflectionAbstractions.EmptyTypes), GetCollectionTCountProperty(source, type2), type2);
                }
                if (type6 != null)
                {
                    return new CollectionTraits(CollectionKind.Map, GetAddMethod(source, typeof(object), typeof(object)), FindInterfaceMethod(source, type6, "GetEnumerator", ReflectionAbstractions.EmptyTypes), _icollectionCount, typeof(object));
                }
                if (type5 != null)
                {
                    addMethod = GetAddMethod(source, typeof(object));
                    if (addMethod != null)
                    {
                        return new CollectionTraits(CollectionKind.Array, addMethod, FindInterfaceMethod(source, type5, "GetEnumerator", ReflectionAbstractions.EmptyTypes), _icollectionCount, typeof(object));
                    }
                }
            }
            return CollectionTraits.NotCollection;
        }

        private static PropertyInfo GetDictionaryCountProperty(Type targetType, Type keyType, Type valueType)
        {
            PropertyInfo property = targetType.GetProperty("Count");
            if (((property != null) && (property.PropertyType == typeof(int))) && (property.GetIndexParameters().Length == 0))
            {
                return property;
            }
            return typeof(ICollection<>).MakeGenericType(new Type[] { typeof(KeyValuePair<,>).MakeGenericType(new Type[] { keyType, valueType }) }).GetProperty("Count");
        }

        public static Type GetMemberValueType(this MemberInfo source)
        {
            Contract.Requires(source != null);
            PropertyInfo info = source as PropertyInfo;
            FieldInfo info2 = source as FieldInfo;
            return ((info != null) ? info.PropertyType : info2.FieldType);
        }
    }
}

