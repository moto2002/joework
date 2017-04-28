using Mogo.Util;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

public static class DictionaryExtend
{
    public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
        TValue local2;
        if (dictionary == null)
        {
            LoggerHelper.Critical("Dictionary is null.", true);
            local2 = default(TValue);
            return ((local2 == null) ? GetDefaultValue<TValue>() : default(TValue));
        }
        if (!dictionary.ContainsKey(key))
        {
            LoggerHelper.Critical(string.Format("Key '{0}' is not exist.", key), true);
            local2 = default(TValue);
            return ((local2 == null) ? GetDefaultValue<TValue>() : default(TValue));
        }
        return dictionary[key];
    }

    public static T Get<T>(this List<T> list, int index)
    {
        T local2;
        if (list == null)
        {
            LoggerHelper.Critical("List is null.", true);
            local2 = default(T);
            return ((local2 == null) ? GetDefaultValue<T>() : default(T));
        }
        if (list.Count <= index)
        {
            LoggerHelper.Critical(string.Format("Index '{0}' is out of range.", index), true);
            local2 = default(T);
            return ((local2 == null) ? GetDefaultValue<T>() : default(T));
        }
        return list[index];
    }

    public static T Get<T>(this T[] array, int index)
    {
        T local2;
        if (array == null)
        {
            LoggerHelper.Critical("Array is null.", true);
            local2 = default(T);
            return ((local2 == null) ? GetDefaultValue<T>() : default(T));
        }
        if (array.Length <= index)
        {
            LoggerHelper.Critical(string.Format("Index '{0}' is out of range.", index), true);
            local2 = default(T);
            return ((local2 == null) ? GetDefaultValue<T>() : default(T));
        }
        return array[index];
    }

    public static T GetDefaultValue<T>()
    {
        ConstructorInfo constructor = typeof(T).GetConstructor(Type.EmptyTypes);
        if (constructor == null)
        {
            return default(T);
        }
        return (T) constructor.Invoke(null);
    }

    public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
    {
        TValue local;
        return (dictionary.TryGetValue(key, out local) ? local : defaultValue);
    }

    public static TValue GetValueOrDefaultValueProvider<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> defaultValueProvider)
    {
        TValue local;
        return (dictionary.TryGetValue(key, out local) ? local : defaultValueProvider());
    }
}

