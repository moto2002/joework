namespace MsgPack
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Threading;

    [SecurityCritical, SuppressUnmanagedCodeSecurity]
    internal static class UnsafeNativeMethods
    {
        private static int _libCAvailability = 0;
        private const int _libCAvailability_LibC = 2;
        private const int _libCAvailability_MSVCRT = 1;
        private const int _libCAvailability_None = -1;
        private const int _libCAvailability_Unknown = 0;

        [DllImport("libc", EntryPoint="memcmp", CallingConvention=CallingConvention.Cdecl, ExactSpelling=true)]
        private static extern int memcmpLibC(byte[] s1, byte[] s2, UIntPtr size);
        [DllImport("msvcrt", EntryPoint="memcmp", CallingConvention=CallingConvention.Cdecl, ExactSpelling=true)]
        private static extern int memcmpVC(byte[] s1, byte[] s2, UIntPtr size);
        public static bool TryMemCmp(byte[] s1, byte[] s2, UIntPtr size, out int result)
        {
            if (_libCAvailability < 0)
            {
                result = 0;
                return false;
            }
            if (_libCAvailability <= 1)
            {
                try
                {
                    result = memcmpVC(s1, s2, size);
                    return true;
                }
                catch (DllNotFoundException)
                {
                    Interlocked.Exchange(ref _libCAvailability, 2);
                }
            }
            if (_libCAvailability <= 2)
            {
                try
                {
                    result = memcmpLibC(s1, s2, size);
                    return true;
                }
                catch (DllNotFoundException)
                {
                    Interlocked.Exchange(ref _libCAvailability, -1);
                }
            }
            result = 0;
            return false;
        }
    }
}

