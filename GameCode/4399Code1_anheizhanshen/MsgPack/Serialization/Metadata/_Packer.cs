namespace MsgPack.Serialization.Metadata
{
    using System;
    using System.Reflection;

    internal static class _Packer
    {
        public static readonly MethodInfo PackArrayHeader = FromExpression.ToMethod<Packer, int, Packer>((packer, count) => packer.PackArrayHeader(count));
        public static readonly MethodInfo PackMapHeader = FromExpression.ToMethod<Packer, int, Packer>((packer, count) => packer.PackMapHeader(count));
        public static readonly MethodInfo PackNull = FromExpression.ToMethod<Packer, Packer>(packer => packer.PackNull());
        public static readonly MethodInfo PackString = FromExpression.ToMethod<Packer, string, Packer>((packer, value) => packer.PackString(value));
    }
}

