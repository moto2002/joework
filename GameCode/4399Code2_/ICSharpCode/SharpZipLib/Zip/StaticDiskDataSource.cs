namespace ICSharpCode.SharpZipLib.Zip
{
    using System;
    using System.IO;

    public class StaticDiskDataSource : IStaticDataSource
    {
        private string fileName_;

        public StaticDiskDataSource(string fileName)
        {
            this.fileName_ = fileName;
        }

        public Stream GetSource()
        {
            return File.Open(this.fileName_, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
    }
}

