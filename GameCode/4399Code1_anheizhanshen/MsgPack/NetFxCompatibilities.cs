namespace MsgPack
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;

    internal static class NetFxCompatibilities
    {
        public static void Clear(this StringBuilder source)
        {
            source.Length = 0;
        }

        public static void Restart(this Stopwatch source)
        {
            source.Stop();
            source.Reset();
            source.Start();
        }
    }
}

