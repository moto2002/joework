namespace ICSharpCode.SharpZipLib.Zip
{
    using ICSharpCode.SharpZipLib.Core;
    using System;
    using System.IO;

    public class ZipEntryFactory : IEntryFactory
    {
        private DateTime fixedDateTime_;
        private int getAttributes_;
        private bool isUnicodeText_;
        private INameTransform nameTransform_;
        private int setAttributes_;
        private TimeSetting timeSetting_;

        public ZipEntryFactory()
        {
            this.fixedDateTime_ = DateTime.Now;
            this.getAttributes_ = -1;
            this.nameTransform_ = new ZipNameTransform();
        }

        public ZipEntryFactory(TimeSetting timeSetting)
        {
            this.fixedDateTime_ = DateTime.Now;
            this.getAttributes_ = -1;
            this.timeSetting_ = timeSetting;
            this.nameTransform_ = new ZipNameTransform();
        }

        public ZipEntryFactory(DateTime time)
        {
            this.fixedDateTime_ = DateTime.Now;
            this.getAttributes_ = -1;
            this.timeSetting_ = TimeSetting.Fixed;
            this.FixedDateTime = time;
            this.nameTransform_ = new ZipNameTransform();
        }

        public ZipEntry MakeDirectoryEntry(string directoryName)
        {
            return this.MakeDirectoryEntry(directoryName, true);
        }

        public ZipEntry MakeDirectoryEntry(string directoryName, bool useFileSystem)
        {
            ZipEntry entry = new ZipEntry(this.nameTransform_.TransformDirectory(directoryName)) {
                IsUnicodeText = this.isUnicodeText_,
                Size = 0L
            };
            int num = 0;
            DirectoryInfo info = null;
            if (useFileSystem)
            {
                info = new DirectoryInfo(directoryName);
            }
            if ((info == null) || !info.Exists)
            {
                if (this.timeSetting_ == TimeSetting.Fixed)
                {
                    entry.DateTime = this.fixedDateTime_;
                }
            }
            else
            {
                switch (this.timeSetting_)
                {
                    case TimeSetting.LastWriteTime:
                        entry.DateTime = info.LastWriteTime;
                        break;

                    case TimeSetting.LastWriteTimeUtc:
                        entry.DateTime = info.LastWriteTimeUtc;
                        break;

                    case TimeSetting.CreateTime:
                        entry.DateTime = info.CreationTime;
                        break;

                    case TimeSetting.CreateTimeUtc:
                        entry.DateTime = info.CreationTimeUtc;
                        break;

                    case TimeSetting.LastAccessTime:
                        entry.DateTime = info.LastAccessTime;
                        break;

                    case TimeSetting.LastAccessTimeUtc:
                        entry.DateTime = info.LastAccessTimeUtc;
                        break;

                    case TimeSetting.Fixed:
                        entry.DateTime = this.fixedDateTime_;
                        break;

                    default:
                        throw new ZipException("Unhandled time setting in MakeDirectoryEntry");
                }
                num = ((int) info.Attributes) & this.getAttributes_;
            }
            num |= this.setAttributes_ | 0x10;
            entry.ExternalFileAttributes = num;
            return entry;
        }

        public ZipEntry MakeFileEntry(string fileName)
        {
            return this.MakeFileEntry(fileName, true);
        }

        public ZipEntry MakeFileEntry(string fileName, bool useFileSystem)
        {
            ZipEntry entry = new ZipEntry(this.nameTransform_.TransformFile(fileName)) {
                IsUnicodeText = this.isUnicodeText_
            };
            int num = 0;
            bool flag = this.setAttributes_ != 0;
            FileInfo info = null;
            if (useFileSystem)
            {
                info = new FileInfo(fileName);
            }
            if ((info == null) || !info.Exists)
            {
                if (this.timeSetting_ == TimeSetting.Fixed)
                {
                    entry.DateTime = this.fixedDateTime_;
                }
            }
            else
            {
                switch (this.timeSetting_)
                {
                    case TimeSetting.LastWriteTime:
                        entry.DateTime = info.LastWriteTime;
                        break;

                    case TimeSetting.LastWriteTimeUtc:
                        entry.DateTime = info.LastWriteTimeUtc;
                        break;

                    case TimeSetting.CreateTime:
                        entry.DateTime = info.CreationTime;
                        break;

                    case TimeSetting.CreateTimeUtc:
                        entry.DateTime = info.CreationTimeUtc;
                        break;

                    case TimeSetting.LastAccessTime:
                        entry.DateTime = info.LastAccessTime;
                        break;

                    case TimeSetting.LastAccessTimeUtc:
                        entry.DateTime = info.LastAccessTimeUtc;
                        break;

                    case TimeSetting.Fixed:
                        entry.DateTime = this.fixedDateTime_;
                        break;

                    default:
                        throw new ZipException("Unhandled time setting in MakeFileEntry");
                }
                entry.Size = info.Length;
                flag = true;
                num = ((int) info.Attributes) & this.getAttributes_;
            }
            if (flag)
            {
                num |= this.setAttributes_;
                entry.ExternalFileAttributes = num;
            }
            return entry;
        }

        public DateTime FixedDateTime
        {
            get
            {
                return this.fixedDateTime_;
            }
            set
            {
                if (value.Year < 0x7b2)
                {
                    throw new ArgumentException("Value is too old to be valid", "value");
                }
                this.fixedDateTime_ = value;
            }
        }

        public int GetAttributes
        {
            get
            {
                return this.getAttributes_;
            }
            set
            {
                this.getAttributes_ = value;
            }
        }

        public bool IsUnicodeText
        {
            get
            {
                return this.isUnicodeText_;
            }
            set
            {
                this.isUnicodeText_ = value;
            }
        }

        public INameTransform NameTransform
        {
            get
            {
                return this.nameTransform_;
            }
            set
            {
                if (value == null)
                {
                    this.nameTransform_ = new ZipNameTransform();
                }
                else
                {
                    this.nameTransform_ = value;
                }
            }
        }

        public int SetAttributes
        {
            get
            {
                return this.setAttributes_;
            }
            set
            {
                this.setAttributes_ = value;
            }
        }

        public TimeSetting Setting
        {
            get
            {
                return this.timeSetting_;
            }
            set
            {
                this.timeSetting_ = value;
            }
        }

        public enum TimeSetting
        {
            LastWriteTime,
            LastWriteTimeUtc,
            CreateTime,
            CreateTimeUtc,
            LastAccessTime,
            LastAccessTimeUtc,
            Fixed
        }
    }
}

