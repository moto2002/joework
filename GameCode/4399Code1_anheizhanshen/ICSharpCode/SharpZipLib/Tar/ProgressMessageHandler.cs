namespace ICSharpCode.SharpZipLib.Tar
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void ProgressMessageHandler(TarArchive archive, TarEntry entry, string message);
}

