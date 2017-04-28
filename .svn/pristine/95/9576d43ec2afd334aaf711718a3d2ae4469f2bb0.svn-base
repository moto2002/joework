namespace ICSharpCode.SharpZipLib.Zip
{
    using ICSharpCode.SharpZipLib.Core;
    using System;
    using System.IO;

    public class MemoryArchiveStorage : BaseArchiveStorage
    {
        private MemoryStream finalStream_;
        private MemoryStream temporaryStream_;

        public MemoryArchiveStorage() : base(FileUpdateMode.Direct)
        {
        }

        public MemoryArchiveStorage(FileUpdateMode updateMode) : base(updateMode)
        {
        }

        public override Stream ConvertTemporaryToFinal()
        {
            if (this.temporaryStream_ == null)
            {
                throw new ZipException("No temporary stream has been created");
            }
            this.finalStream_ = new MemoryStream(this.temporaryStream_.ToArray());
            return this.finalStream_;
        }

        public override void Dispose()
        {
            if (this.temporaryStream_ != null)
            {
                this.temporaryStream_.Close();
            }
        }

        public override Stream GetTemporaryOutput()
        {
            this.temporaryStream_ = new MemoryStream();
            return this.temporaryStream_;
        }

        public override Stream MakeTemporaryCopy(Stream stream)
        {
            this.temporaryStream_ = new MemoryStream();
            stream.Position = 0L;
            StreamUtils.Copy(stream, this.temporaryStream_, new byte[0x1000]);
            return this.temporaryStream_;
        }

        public override Stream OpenForDirectUpdate(Stream stream)
        {
            if ((stream == null) || !stream.CanWrite)
            {
                Stream destination = new MemoryStream();
                if (stream != null)
                {
                    stream.Position = 0L;
                    StreamUtils.Copy(stream, destination, new byte[0x1000]);
                    stream.Close();
                }
                return destination;
            }
            return stream;
        }

        public MemoryStream FinalStream
        {
            get
            {
                return this.finalStream_;
            }
        }
    }
}

