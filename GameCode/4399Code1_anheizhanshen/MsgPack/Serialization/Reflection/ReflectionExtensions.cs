namespace MsgPack.Serialization.Reflection
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    internal static class ReflectionExtensions
    {
        public static bool IsAssignableTo(this Type source, Type target)
        {
            Contract.Assert(source != null);
            if (target == null)
            {
                return false;
            }
            return target.IsAssignableFrom(source);
        }
    }
}

