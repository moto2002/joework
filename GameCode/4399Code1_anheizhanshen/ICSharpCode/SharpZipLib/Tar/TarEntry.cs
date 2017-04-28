namespace ICSharpCode.SharpZipLib.Tar
{
    using System;
    using System.IO;

    public class TarEntry : ICloneable
    {
        private string file;
        private ICSharpCode.SharpZipLib.Tar.TarHeader header;

        private TarEntry()
        {
            this.header = new ICSharpCode.SharpZipLib.Tar.TarHeader();
        }

        public TarEntry(byte[] headerBuffer)
        {
            this.header = new ICSharpCode.SharpZipLib.Tar.TarHeader();
            this.header.ParseBuffer(headerBuffer);
        }

        public TarEntry(ICSharpCode.SharpZipLib.Tar.TarHeader header)
        {
            if (header == null)
            {
                throw new ArgumentNullException("header");
            }
            this.header = (ICSharpCode.SharpZipLib.Tar.TarHeader) header.Clone();
        }

        public static void AdjustEntryName(byte[] buffer, string newName)
        {
            ICSharpCode.SharpZipLib.Tar.TarHeader.GetNameBytes(newName, buffer, 0, 100);
        }

        public object Clone()
        {
            return new TarEntry { file = this.file, header = (ICSharpCode.SharpZipLib.Tar.TarHeader) this.header.Clone(), Name = this.Name };
        }

        public static TarEntry CreateEntryFromFile(string fileName)
        {
            TarEntry entry = new TarEntry();
            entry.GetFileTarHeader(entry.header, fileName);
            return entry;
        }

        public static TarEntry CreateTarEntry(string name)
        {
            TarEntry entry = new TarEntry();
            NameTarHeader(entry.header, name);
            return entry;
        }

        public override bool Equals(object obj)
        {
            TarEntry entry = obj as TarEntry;
            return ((entry != null) && this.Name.Equals(entry.Name));
        }

        public TarEntry[] GetDirectoryEntries()
        {
            if (!((this.file != null) && Directory.Exists(this.file)))
            {
                return new TarEntry[0];
            }
            string[] fileSystemEntries = Directory.GetFileSystemEntries(this.file);
            TarEntry[] entryArray = new TarEntry[fileSystemEntries.Length];
            for (int i = 0; i < fileSystemEntries.Length; i++)
            {
                entryArray[i] = CreateEntryFromFile(fileSystemEntries[i]);
            }
            return entryArray;
        }

        public void GetFileTarHeader(ICSharpCode.SharpZipLib.Tar.TarHeader header, string file)
        {
            if (header == null)
            {
                throw new ArgumentNullException("header");
            }
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            this.file = file;
            string str = file;
            if (str.IndexOf(Environment.CurrentDirectory) == 0)
            {
                str = str.Substring(Environment.CurrentDirectory.Length);
            }
            str = str.Replace(Path.DirectorySeparatorChar, '/');
            while (str.StartsWith("/"))
            {
                str = str.Substring(1);
            }
            header.LinkName = string.Empty;
            header.Name = str;
            if (Directory.Exists(file))
            {
                header.Mode = 0x3eb;
                header.TypeFlag = 0x35;
                if ((header.Name.Length == 0) || (header.Name[header.Name.Length - 1] != '/'))
                {
                    header.Name = header.Name + "/";
                }
                header.Size = 0L;
            }
            else
            {
                header.Mode = 0x81c0;
                header.TypeFlag = 0x30;
                header.Size = new FileInfo(file.Replace('/', Path.DirectorySeparatorChar)).Length;
            }
            header.ModTime = System.IO.File.GetLastWriteTime(file.Replace('/', Path.DirectorySeparatorChar)).ToUniversalTime();
            header.DevMajor = 0;
            header.DevMinor = 0;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public bool IsDescendent(TarEntry toTest)
        {
            if (toTest == null)
            {
                throw new ArgumentNullException("toTest");
            }
            return toTest.Name.StartsWith(this.Name);
        }

        public static void NameTarHeader(ICSharpCode.SharpZipLib.Tar.TarHeader header, string name)
        {
            if (header == null)
            {
                throw new ArgumentNullException("header");
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            bool flag = name.EndsWith("/");
            header.Name = name;
            header.Mode = flag ? 0x3eb : 0x81c0;
            header.UserId = 0;
            header.GroupId = 0;
            header.Size = 0L;
            header.ModTime = DateTime.UtcNow;
            header.TypeFlag = flag ? ((byte) 0x35) : ((byte) 0x30);
            header.LinkName = string.Empty;
            header.UserName = string.Empty;
            header.GroupName = string.Empty;
            header.DevMajor = 0;
            header.DevMinor = 0;
        }

        public void SetIds(int userId, int groupId)
        {
            this.UserId = userId;
            this.GroupId = groupId;
        }

        public void SetNames(string userName, string groupName)
        {
            this.UserName = userName;
            this.GroupName = groupName;
        }

        public void WriteEntryHeader(byte[] outBuffer)
        {
            this.header.WriteHeader(outBuffer);
        }

        public string File
        {
            get
            {
                return this.file;
            }
        }

        public int GroupId
        {
            get
            {
                return this.header.GroupId;
            }
            set
            {
                this.header.GroupId = value;
            }
        }

        public string GroupName
        {
            get
            {
                return this.header.GroupName;
            }
            set
            {
                this.header.GroupName = value;
            }
        }

        public bool IsDirectory
        {
            get
            {
                if (this.file != null)
                {
                    return Directory.Exists(this.file);
                }
                return ((this.header != null) && ((this.header.TypeFlag == 0x35) || this.Name.EndsWith("/")));
            }
        }

        public DateTime ModTime
        {
            get
            {
                return this.header.ModTime;
            }
            set
            {
                this.header.ModTime = value;
            }
        }

        public string Name
        {
            get
            {
                return this.header.Name;
            }
            set
            {
                this.header.Name = value;
            }
        }

        public long Size
        {
            get
            {
                return this.header.Size;
            }
            set
            {
                this.header.Size = value;
            }
        }

        public ICSharpCode.SharpZipLib.Tar.TarHeader TarHeader
        {
            get
            {
                return this.header;
            }
        }

        public int UserId
        {
            get
            {
                return this.header.UserId;
            }
            set
            {
                this.header.UserId = value;
            }
        }

        public string UserName
        {
            get
            {
                return this.header.UserName;
            }
            set
            {
                this.header.UserName = value;
            }
        }
    }
}

