namespace MsgPack.Serialization.Metadata
{
    using System;
    using System.Reflection;

    internal static class _IDictionaryEnumerator
    {
        public static readonly PropertyInfo Current = FromExpression.ToProperty<IDictionaryEnumerator, DictionaryEntry>(enumerator => enumerator.Entry);
    }
}

