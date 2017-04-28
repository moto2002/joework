namespace ICSharpCode.SharpZipLib.Zip
{
    using System;
    using System.IO;

    public interface IDynamicDataSource
    {
        Stream GetSource(ZipEntry entry, string name);
    }
}

