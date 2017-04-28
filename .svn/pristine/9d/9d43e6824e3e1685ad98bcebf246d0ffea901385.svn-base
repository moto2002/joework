namespace MsgPack.Serialization.Metadata
{
    using System;
    using System.Reflection;

    internal static class _IEnumerator
    {
        public static readonly PropertyInfo Current = FromExpression.ToProperty<IEnumerator, object>(enumerator => enumerator.Current);
        public static readonly MethodInfo MoveNext = FromExpression.ToMethod<IEnumerator, bool>(enumerator => enumerator.MoveNext());
    }
}

