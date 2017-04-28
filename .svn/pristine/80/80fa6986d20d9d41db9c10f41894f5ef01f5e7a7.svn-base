namespace ICSharpCode.SharpZipLib.Zip
{
    using System;
    using System.IO;

    public interface IArchiveStorage
    {
        Stream ConvertTemporaryToFinal();
        void Dispose();
        Stream GetTemporaryOutput();
        Stream MakeTemporaryCopy(Stream stream);
        Stream OpenForDirectUpdate(Stream stream);

        FileUpdateMode UpdateMode { get; }
    }
}

