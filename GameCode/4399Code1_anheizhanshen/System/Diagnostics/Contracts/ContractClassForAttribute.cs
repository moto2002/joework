namespace System.Diagnostics.Contracts
{
    using System;
    using System.Diagnostics;

    [Conditional("NEVER_COMPILED"), AttributeUsage(AttributeTargets.Class)]
    internal sealed class ContractClassForAttribute : Attribute
    {
        public ContractClassForAttribute(Type typeContractsAreFor)
        {
        }
    }
}

