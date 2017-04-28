namespace System.Diagnostics.Contracts
{
    using System;
    using System.Diagnostics;

    [AttributeUsage(AttributeTargets.Delegate | AttributeTargets.Interface | AttributeTargets.Class), Conditional("NEVER_COMPILED")]
    internal sealed class ContractClassAttribute : Attribute
    {
        public ContractClassAttribute(Type typeContainingContracts)
        {
        }
    }
}

