namespace System.Diagnostics.Contracts
{
    using System;
    using System.Diagnostics;

    [Conditional("NEVER_COMPILED"), AttributeUsage(AttributeTargets.Delegate | AttributeTargets.Parameter | AttributeTargets.Event | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Class, Inherited=true)]
    internal sealed class PureAttribute : Attribute
    {
    }
}

