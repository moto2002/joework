namespace Mogo.Util
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class PackageInfo
    {
        private static int m_packageSize = -1;

        static PackageInfo()
        {
            m_packageSize = Marshal.SizeOf(typeof(uint));
        }

        public static int GetPackageSize()
        {
            return m_packageSize;
        }

        public uint IndexOffset { get; set; }
    }
}

