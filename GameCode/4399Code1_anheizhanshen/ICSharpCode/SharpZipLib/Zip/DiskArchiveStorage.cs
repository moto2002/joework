namespace ICSharpCode.SharpZipLib.Zip
{
    using System;
    using System.IO;

    public class DiskArchiveStorage : BaseArchiveStorage
    {
        private string fileName_;
        private string temporaryName_;
        private Stream temporaryStream_;

        public DiskArchiveStorage(ZipFile file) : this(file, FileUpdateMode.Safe)
        {
        }

        public DiskArchiveStorage(ZipFile file, FileUpdateMode updateMode) : base(updateMode)
        {
            if (file.Name == null)
            {
                throw new ZipException("Cant handle non file archives");
            }
            this.fileName_ = file.Name;
        }

        public override Stream ConvertTemporaryToFinal()
        {
            if (this.temporaryStream_ == null)
            {
                throw new ZipException("No temporary stream has been created");
            }
            Stream stream = null;
            string tempFileName = GetTempFileName(this.fileName_, false);
            bool flag = false;
            try
            {
                this.temporaryStream_.Close();
                File.Move(this.fileName_, tempFileName);
                File.Move(this.temporaryName_, this.fileName_);
                flag = true;
                File.Delete(tempFileName);
                stream = File.Open(this.fileName_, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (Exception)
            {
                stream = null;
                if (!flag)
                {
                    File.Move(tempFileName, this.fileName_);
                    File.Delete(this.temporaryName_);
                }
                throw;
            }
            return stream;
        }

        public override void Dispose()
        {
            if (this.temporaryStream_ != null)
            {
                this.temporaryStream_.Close();
            }
        }

        private static string GetTempFileName(string original, bool makeTempFile)
        {
            string str = null;
            if (original == null)
            {
                return Path.GetTempFileName();
            }
            int num = 0;
            int second = DateTime.Now.Second;
            while (str == null)
            {
                num++;
                string path = string.Format("{0}.{1}{2}.tmp", original, second, num);
                if (!File.Exists(path))
                {
                    if (makeTempFile)
                    {
                        try
                        {
                            using (File.Create(path))
                            {
                            }
                            str = path;
                        }
                        catch
                        {
                            second = DateTime.Now.Second;
                        }
                    }
                    else
                    {
                        str = path;
                    }
                }
            }
            return str;
        }

        public override Stream GetTemporaryOutput()
        {
            if (this.temporaryName_ != null)
            {
                this.temporaryName_ = GetTempFileName(this.temporaryName_, true);
                this.temporaryStream_ = File.Open(this.temporaryName_, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            }
            else
            {
                this.temporaryName_ = Path.GetTempFileName();
                this.temporaryStream_ = File.Open(this.temporaryName_, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            }
            return this.temporaryStream_;
        }

        public override Stream MakeTemporaryCopy(Stream stream)
        {
            stream.Close();
            this.temporaryName_ = GetTempFileName(this.fileName_, true);
            File.Copy(this.fileName_, this.temporaryName_, true);
            this.temporaryStream_ = new FileStream(this.temporaryName_, FileMode.Open, FileAccess.ReadWrite);
            return this.temporaryStream_;
        }

        public override Stream OpenForDirectUpdate(Stream stream)
        {
            if ((stream == null) || !stream.CanWrite)
            {
                if (stream != null)
                {
                    stream.Close();
                }
                return new FileStream(this.fileName_, FileMode.Open, FileAccess.ReadWrite);
            }
            return stream;
        }
    }
}

