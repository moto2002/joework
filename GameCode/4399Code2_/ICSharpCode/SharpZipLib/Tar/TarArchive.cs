namespace ICSharpCode.SharpZipLib.Tar
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;

    public class TarArchive : IDisposable
    {
        private bool applyUserInfoOverrides;
        private bool asciiTranslate;
        private int groupId;
        private string groupName;
        private bool isDisposed;
        private bool keepOldFiles;
        private string pathPrefix;
        private string rootPath;
        private TarInputStream tarIn;
        private TarOutputStream tarOut;
        private int userId;
        private string userName;

        public event ProgressMessageHandler ProgressMessageEvent;

        protected TarArchive()
        {
            this.userName = string.Empty;
            this.groupName = string.Empty;
        }

        protected TarArchive(TarInputStream stream)
        {
            this.userName = string.Empty;
            this.groupName = string.Empty;
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            this.tarIn = stream;
        }

        protected TarArchive(TarOutputStream stream)
        {
            this.userName = string.Empty;
            this.groupName = string.Empty;
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            this.tarOut = stream;
        }

        public virtual void Close()
        {
            this.Dispose(true);
        }

        [Obsolete("Use Close instead")]
        public void CloseArchive()
        {
            this.Close();
        }

        public static TarArchive CreateInputTarArchive(Stream inputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException("inputStream");
            }
            TarInputStream stream = inputStream as TarInputStream;
            if (stream != null)
            {
                return new TarArchive(stream);
            }
            return CreateInputTarArchive(inputStream, 20);
        }

        public static TarArchive CreateInputTarArchive(Stream inputStream, int blockFactor)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException("inputStream");
            }
            if (inputStream is TarInputStream)
            {
                throw new ArgumentException("TarInputStream not valid");
            }
            return new TarArchive(new TarInputStream(inputStream, blockFactor));
        }

        public static TarArchive CreateOutputTarArchive(Stream outputStream)
        {
            if (outputStream == null)
            {
                throw new ArgumentNullException("outputStream");
            }
            TarOutputStream stream = outputStream as TarOutputStream;
            if (stream != null)
            {
                return new TarArchive(stream);
            }
            return CreateOutputTarArchive(outputStream, 20);
        }

        public static TarArchive CreateOutputTarArchive(Stream outputStream, int blockFactor)
        {
            if (outputStream == null)
            {
                throw new ArgumentNullException("outputStream");
            }
            if (outputStream is TarOutputStream)
            {
                throw new ArgumentException("TarOutputStream is not valid");
            }
            return new TarArchive(new TarOutputStream(outputStream, blockFactor));
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                this.isDisposed = true;
                if (disposing)
                {
                    if (this.tarOut != null)
                    {
                        this.tarOut.Flush();
                        this.tarOut.Close();
                    }
                    if (this.tarIn != null)
                    {
                        this.tarIn.Close();
                    }
                }
            }
        }

        private static void EnsureDirectoryExists(string directoryName)
        {
            if (!Directory.Exists(directoryName))
            {
                try
                {
                    Directory.CreateDirectory(directoryName);
                }
                catch (Exception exception)
                {
                    throw new TarException("Exception creating directory '" + directoryName + "', " + exception.Message, exception);
                }
            }
        }

        public void ExtractContents(string destinationDirectory)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException("TarArchive");
            }
            while (true)
            {
                TarEntry nextEntry = this.tarIn.GetNextEntry();
                if (nextEntry == null)
                {
                    return;
                }
                this.ExtractEntry(destinationDirectory, nextEntry);
            }
        }

        private void ExtractEntry(string destDir, TarEntry entry)
        {
            this.OnProgressMessageEvent(entry, null);
            string name = entry.Name;
            if (Path.IsPathRooted(name))
            {
                name = name.Substring(Path.GetPathRoot(name).Length);
            }
            name = name.Replace('/', Path.DirectorySeparatorChar);
            string directoryName = Path.Combine(destDir, name);
            if (entry.IsDirectory)
            {
                EnsureDirectoryExists(directoryName);
            }
            else
            {
                EnsureDirectoryExists(Path.GetDirectoryName(directoryName));
                bool flag = true;
                FileInfo info = new FileInfo(directoryName);
                if (info.Exists)
                {
                    if (this.keepOldFiles)
                    {
                        this.OnProgressMessageEvent(entry, "Destination file already exists");
                        flag = false;
                    }
                    else if ((info.Attributes & FileAttributes.ReadOnly) != 0)
                    {
                        this.OnProgressMessageEvent(entry, "Destination file already exists, and is read-only");
                        flag = false;
                    }
                }
                if (flag)
                {
                    bool flag2 = false;
                    Stream stream = File.Create(directoryName);
                    if (this.asciiTranslate)
                    {
                        flag2 = !IsBinary(directoryName);
                    }
                    StreamWriter writer = null;
                    if (flag2)
                    {
                        writer = new StreamWriter(stream);
                    }
                    byte[] buffer = new byte[0x8000];
                    while (true)
                    {
                        int count = this.tarIn.Read(buffer, 0, buffer.Length);
                        if (count <= 0)
                        {
                            if (flag2)
                            {
                                writer.Close();
                            }
                            else
                            {
                                stream.Close();
                            }
                            return;
                        }
                        if (flag2)
                        {
                            int index = 0;
                            for (int i = 0; i < count; i++)
                            {
                                if (buffer[i] == 10)
                                {
                                    string str4 = Encoding.ASCII.GetString(buffer, index, i - index);
                                    writer.WriteLine(str4);
                                    index = i + 1;
                                }
                            }
                        }
                        else
                        {
                            stream.Write(buffer, 0, count);
                        }
                    }
                }
            }
        }

        ~TarArchive()
        {
            this.Dispose(false);
        }

        private static bool IsBinary(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                int count = Math.Min(0x1000, (int) stream.Length);
                byte[] buffer = new byte[count];
                int num2 = stream.Read(buffer, 0, count);
                for (int i = 0; i < num2; i++)
                {
                    byte num4 = buffer[i];
                    if (((num4 < 8) || ((num4 > 13) && (num4 < 0x20))) || (num4 == 0xff))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void ListContents()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException("TarArchive");
            }
            while (true)
            {
                TarEntry nextEntry = this.tarIn.GetNextEntry();
                if (nextEntry == null)
                {
                    return;
                }
                this.OnProgressMessageEvent(nextEntry, null);
            }
        }

        protected virtual void OnProgressMessageEvent(TarEntry entry, string message)
        {
            ProgressMessageHandler progressMessageEvent = this.ProgressMessageEvent;
            if (progressMessageEvent != null)
            {
                progressMessageEvent(this, entry, message);
            }
        }

        [Obsolete("Use the AsciiTranslate property")]
        public void SetAsciiTranslation(bool translateAsciiFiles)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException("TarArchive");
            }
            this.asciiTranslate = translateAsciiFiles;
        }

        public void SetKeepOldFiles(bool keepExistingFiles)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException("TarArchive");
            }
            this.keepOldFiles = keepExistingFiles;
        }

        public void SetUserInfo(int userId, string userName, int groupId, string groupName)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException("TarArchive");
            }
            this.userId = userId;
            this.userName = userName;
            this.groupId = groupId;
            this.groupName = groupName;
            this.applyUserInfoOverrides = true;
        }

        public void WriteEntry(TarEntry sourceEntry, bool recurse)
        {
            if (sourceEntry == null)
            {
                throw new ArgumentNullException("sourceEntry");
            }
            if (this.isDisposed)
            {
                throw new ObjectDisposedException("TarArchive");
            }
            try
            {
                if (recurse)
                {
                    TarHeader.SetValueDefaults(sourceEntry.UserId, sourceEntry.UserName, sourceEntry.GroupId, sourceEntry.GroupName);
                }
                this.WriteEntryCore(sourceEntry, recurse);
            }
            finally
            {
                if (recurse)
                {
                    TarHeader.RestoreSetValues();
                }
            }
        }

        private void WriteEntryCore(TarEntry sourceEntry, bool recurse)
        {
            string path = null;
            bool flag;
            string file = sourceEntry.File;
            TarEntry entry = (TarEntry) sourceEntry.Clone();
            if (this.applyUserInfoOverrides)
            {
                entry.GroupId = this.groupId;
                entry.GroupName = this.groupName;
                entry.UserId = this.userId;
                entry.UserName = this.userName;
            }
            this.OnProgressMessageEvent(entry, null);
            if ((this.asciiTranslate && !entry.IsDirectory) && !IsBinary(file))
            {
                path = Path.GetTempFileName();
                using (StreamReader reader = File.OpenText(file))
                {
                    using (Stream stream = File.Create(path))
                    {
                        string str3;
                        goto Label_00EB;
                    Label_00A9:
                        str3 = reader.ReadLine();
                        if (str3 == null)
                        {
                            goto Label_00F0;
                        }
                        byte[] bytes = Encoding.ASCII.GetBytes(str3);
                        stream.Write(bytes, 0, bytes.Length);
                        stream.WriteByte(10);
                    Label_00EB:
                        flag = true;
                        goto Label_00A9;
                    Label_00F0:
                        stream.Flush();
                    }
                }
                entry.Size = new FileInfo(path).Length;
                file = path;
            }
            string str4 = null;
            if ((this.rootPath != null) && entry.Name.StartsWith(this.rootPath))
            {
                str4 = entry.Name.Substring(this.rootPath.Length + 1);
            }
            if (this.pathPrefix != null)
            {
                str4 = (str4 == null) ? (this.pathPrefix + "/" + entry.Name) : (this.pathPrefix + "/" + str4);
            }
            if (str4 != null)
            {
                entry.Name = str4;
            }
            this.tarOut.PutNextEntry(entry);
            if (entry.IsDirectory)
            {
                if (recurse)
                {
                    TarEntry[] directoryEntries = entry.GetDirectoryEntries();
                    for (int i = 0; i < directoryEntries.Length; i++)
                    {
                        this.WriteEntryCore(directoryEntries[i], recurse);
                    }
                }
                return;
            }
            using (Stream stream2 = File.OpenRead(file))
            {
                int num2;
                byte[] buffer = new byte[0x8000];
                goto Label_0286;
            Label_0255:
                num2 = stream2.Read(buffer, 0, buffer.Length);
                if (num2 <= 0)
                {
                    goto Label_02A2;
                }
                this.tarOut.Write(buffer, 0, num2);
            Label_0286:
                flag = true;
                goto Label_0255;
            }
        Label_02A2:
            if ((path != null) && (path.Length > 0))
            {
                File.Delete(path);
            }
            this.tarOut.CloseEntry();
        }

        public bool ApplyUserInfoOverrides
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException("TarArchive");
                }
                return this.applyUserInfoOverrides;
            }
            set
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException("TarArchive");
                }
                this.applyUserInfoOverrides = value;
            }
        }

        public bool AsciiTranslate
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException("TarArchive");
                }
                return this.asciiTranslate;
            }
            set
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException("TarArchive");
                }
                this.asciiTranslate = value;
            }
        }

        public int GroupId
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException("TarArchive");
                }
                return this.groupId;
            }
        }

        public string GroupName
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException("TarArchive");
                }
                return this.groupName;
            }
        }

        public bool IsStreamOwner
        {
            set
            {
                if (this.tarIn != null)
                {
                    this.tarIn.IsStreamOwner = value;
                }
                else
                {
                    this.tarOut.IsStreamOwner = value;
                }
            }
        }

        public string PathPrefix
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException("TarArchive");
                }
                return this.pathPrefix;
            }
            set
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException("TarArchive");
                }
                this.pathPrefix = value;
            }
        }

        public int RecordSize
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException("TarArchive");
                }
                if (this.tarIn != null)
                {
                    return this.tarIn.RecordSize;
                }
                if (this.tarOut != null)
                {
                    return this.tarOut.RecordSize;
                }
                return 0x2800;
            }
        }

        public string RootPath
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException("TarArchive");
                }
                return this.rootPath;
            }
            set
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException("TarArchive");
                }
                this.rootPath = value;
            }
        }

        public int UserId
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException("TarArchive");
                }
                return this.userId;
            }
        }

        public string UserName
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException("TarArchive");
                }
                return this.userName;
            }
        }
    }
}

