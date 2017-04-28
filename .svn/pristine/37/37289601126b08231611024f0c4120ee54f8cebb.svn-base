namespace MsgPack.Serialization.Metadata
{
    using MsgPack.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    internal static class _KeyValuePair<TKey, TValue>
    {
        public static readonly ConstructorInfo Ctor;
        public static readonly PropertyInfo Key;
        public static readonly PropertyInfo Value;

        static _KeyValuePair()
        {
            _KeyValuePair<TKey, TValue>.Key = FromExpression.ToProperty<KeyValuePair<TKey, TValue>, TKey>(entry => entry.Key);
            _KeyValuePair<TKey, TValue>.Value = FromExpression.ToProperty<KeyValuePair<TKey, TValue>, TValue>(entry => entry.Value);
            _KeyValuePair<TKey, TValue>.Ctor = FromExpression.ToConstructor<TKey, TValue, KeyValuePair<TKey, TValue>>((key, value) => new KeyValuePair<TKey, TValue>(key, value));
        }
    }
}

