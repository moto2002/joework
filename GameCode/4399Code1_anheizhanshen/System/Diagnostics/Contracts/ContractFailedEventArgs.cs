namespace System.Diagnostics.Contracts
{
    using System;
    using System.Runtime.CompilerServices;

    internal sealed class ContractFailedEventArgs : EventArgs
    {
        public void SetUnwind()
        {
            this.IsUnwined = true;
        }

        internal bool IsUnwined { get; private set; }
    }
}

