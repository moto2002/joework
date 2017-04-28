namespace ICSharpCode.SharpZipLib.Zip
{
    using System;

    internal interface ITaggedDataFactory
    {
        ITaggedData Create(short tag, byte[] data, int offset, int count);
    }
}

