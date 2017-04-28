namespace MsgPack.Serialization.Metadata
{
    using System;
    using System.Reflection;

    internal static class _MessagePackObject
    {
        public static readonly MethodInfo AsString = FromExpression.ToMethod<MessagePackObject, string>(mpo => mpo.AsString());
        public static readonly PropertyInfo IsNil = FromExpression.ToProperty<MessagePackObject, bool>(mpo => mpo.IsNil);
    }
}

