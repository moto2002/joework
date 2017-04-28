namespace MsgPack.Serialization.Metadata
{
    using MsgPack.Serialization;
    using System;
    using System.Reflection;

    internal static class _Nullable<T> where T: struct
    {
        public static readonly PropertyInfo HasValue;
        public static readonly PropertyInfo Value;

        static _Nullable()
        {
            _Nullable<T>.HasValue = FromExpression.ToProperty<T?, bool>(nullable => nullable.HasValue);
            _Nullable<T>.Value = FromExpression.ToProperty<T?, T>(nullable => nullable.Value);
        }
    }
}

