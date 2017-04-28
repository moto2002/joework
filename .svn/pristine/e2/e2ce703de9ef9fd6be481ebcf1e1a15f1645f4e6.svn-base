namespace System.Diagnostics.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Threading;

    internal static class Contract
    {
        private static EventHandler<ContractFailedEventArgs> _contractFailed;
        private static readonly object _contractFailedLock = new object();

        public static  event EventHandler<ContractFailedEventArgs> ContractFailed
        {
            add
            {
                lock (_contractFailedLock)
                {
                    _contractFailed = (EventHandler<ContractFailedEventArgs>) Delegate.Combine(_contractFailed, value);
                }
            }
            remove
            {
                lock (_contractFailedLock)
                {
                    _contractFailed = (EventHandler<ContractFailedEventArgs>) Delegate.Remove(_contractFailed, value);
                }
            }
        }

        [Conditional("DEBUG")]
        public static void Assert(bool condition)
        {
            AssertCore(condition, "Assert failed.");
        }

        [Conditional("DEBUG")]
        public static void Assert(bool condition, string message)
        {
            AssertCore(condition, message);
        }

        private static void AssertCore(bool condition, string message)
        {
            if (!condition)
            {
                EventHandler<ContractFailedEventArgs> handler = Interlocked.CompareExchange<EventHandler<ContractFailedEventArgs>>(ref _contractFailed, null, null);
                if (handler != null)
                {
                    ContractFailedEventArgs e = new ContractFailedEventArgs();
                    handler(null, e);
                    if (e.IsUnwined)
                    {
                        throw new Exception(message);
                    }
                }
                Debug.Assert(condition, message);
            }
        }

        [Conditional("DEBUG")]
        public static void Assume(bool condition)
        {
            AssertCore(condition, "Assume failed.");
        }

        [Conditional("DEBUG")]
        public static void Assume(bool condition, string message)
        {
            AssertCore(condition, message);
        }

        [Conditional("NEVER_COMPILED")]
        public static void EndContractBlock()
        {
        }

        [Conditional("NEVER_COMPILED")]
        public static void Ensures(bool condition)
        {
        }

        public static bool ForAll<T>(IEnumerable<T> collection, Predicate<T> predicate)
        {
            return true;
        }

        [Conditional("DEBUG")]
        public static void Requires(bool condition)
        {
            AssertCore(condition, "Precondition failed.");
        }

        public static T Result<T>()
        {
            return default(T);
        }

        public static T ValueAtReturn<T>(out T value)
        {
            value = default(T);
            return default(T);
        }
    }
}

