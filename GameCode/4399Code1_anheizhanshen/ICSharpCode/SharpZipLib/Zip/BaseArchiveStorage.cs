namespace ICSharpCode.SharpZipLib.Zip
{
    using System;
    using System.IO;

    public abstract class BaseArchiveStorage : IArchiveStorage
    {
        private FileUpdateMode updateMode_;

        protected BaseArchiveStorage(FileUpdateMode updateMode)
        {
            this.updateMode_ = updateMode;
        }

        public abstract Stream ConvertTemporaryToFinal();
        public abstract void Dispose();
        public abstract Stream GetTemporaryOutput();
        public abstract Stream MakeTemporaryCopy(Stream stream);
        public abstract Stream OpenForDirectUpdate(Stream stream);

        public FileUpdateMode UpdateMode
        {
            get
            {
                return this.updateMode_;
            }
        }
    }
}

