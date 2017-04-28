namespace MsgPack.Serialization.Metadata
{
    using System;
    using System.Reflection;

    internal static class _String
    {
        public static readonly MethodInfo op_Equality = FromExpression.ToOperator<string, string, bool>((left, right) => left == right);
        public static readonly MethodInfo op_Inequality = FromExpression.ToOperator<string, string, bool>((left, right) => left != right);
    }
}

