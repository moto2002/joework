namespace ICSharpCode.SharpZipLib.Zip
{
    using ICSharpCode.SharpZipLib.Core;
    using System;
    using System.Collections;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class FastZip
    {
        private byte[] buffer_;
        private ConfirmOverwriteDelegate confirmDelegate_;
        private bool continueRunning_;
        private bool createEmptyDirectories_;
        private NameFilter directoryFilter_;
        private IEntryFactory entryFactory_;
        private FastZipEvents events_;
        private INameTransform extractNameTransform_;
        private NameFilter fileFilter_;
        private ZipOutputStream outputStream_;
        private Overwrite overwrite_;
        private string password_;
        private bool restoreAttributesOnExtract_;
        private bool restoreDateTimeOnExtract_;
        private string sourceDirectory_;
        private ICSharpCode.SharpZipLib.Zip.UseZip64 useZip64_;
        private ZipFile zipFile_;

        public FastZip()
        {
            this.entryFactory_ = new ZipEntryFactory();
            this.useZip64_ = ICSharpCode.SharpZipLib.Zip.UseZip64.Dynamic;
        }

        public FastZip(FastZipEvents events)
        {
            this.entryFactory_ = new ZipEntryFactory();
            this.useZip64_ = ICSharpCode.SharpZipLib.Zip.UseZip64.Dynamic;
            this.events_ = events;
        }

        private void AddFileContents(string name, Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            if (this.buffer_ == null)
            {
                this.buffer_ = new byte[0x1000];
            }
            if ((this.events_ != null) && (this.events_.Progress != null))
            {
                StreamUtils.Copy(stream, this.outputStream_, this.buffer_, this.events_.Progress, this.events_.ProgressInterval, this, name);
            }
            else
            {
                StreamUtils.Copy(stream, this.outputStream_, this.buffer_);
            }
            if (this.events_ != null)
            {
                this.continueRunning_ = this.events_.OnCompletedFile(name);
            }
        }

        public void CreateZip(string zipFileName, string sourceDirectory, bool recurse, string fileFilter)
        {
            this.CreateZip(File.Create(zipFileName), sourceDirectory, recurse, fileFilter, null);
        }

        public void CreateZip(Stream outputStream, string sourceDirectory, bool recurse, string fileFilter, string directoryFilter)
        {
            this.NameTransform = new ZipNameTransform(sourceDirectory);
            this.sourceDirectory_ = sourceDirectory;
            using (this.outputStream_ = new ZipOutputStream(outputStream))
            {
                FileSystemScanner scanner;
                if (this.password_ != null)
                {
                    this.outputStream_.Password = this.password_;
                }
                this.outputStream_.UseZip64 = this.UseZip64;
                scanner = new FileSystemScanner(fileFilter, directoryFilter) {
                    ProcessFile = (ProcessFileHandler) Delegate.Combine(scanner.ProcessFile, new ProcessFileHandler(this.ProcessFile))
                };
                if (this.CreateEmptyDirectories)
                {
                    scanner.ProcessDirectory = (ProcessDirectoryHandler) Delegate.Combine(scanner.ProcessDirectory, new ProcessDirectoryHandler(this.ProcessDirectory));
                }
                if (this.events_ != null)
                {
                    if (this.events_.FileFailure != null)
                    {
                        scanner.FileFailure = (FileFailureHandler) Delegate.Combine(scanner.FileFailure, this.events_.FileFailure);
                    }
                    if (this.events_.DirectoryFailure != null)
                    {
                        scanner.DirectoryFailure = (DirectoryFailureHandler) Delegate.Combine(scanner.DirectoryFailure, this.events_.DirectoryFailure);
                    }
                }
                scanner.Scan(sourceDirectory, recurse);
            }
        }

        public void CreateZip(string zipFileName, string sourceDirectory, bool recurse, string fileFilter, string directoryFilter)
        {
            this.CreateZip(File.Create(zipFileName), sourceDirectory, recurse, fileFilter, directoryFilter);
        }

        private void ExtractEntry(ZipEntry entry)
        {
            bool flag = entry.IsCompressionMethodSupported();
            string name = entry.Name;
            if (flag)
            {
                if (entry.IsFile)
                {
                    name = this.extractNameTransform_.TransformFile(name);
                }
                else if (entry.IsDirectory)
                {
                    name = this.extractNameTransform_.TransformDirectory(name);
                }
                flag = (name != null) && (name.Length != 0);
            }
            string path = null;
            if (flag)
            {
                if (entry.IsDirectory)
                {
                    path = name;
                }
                else
                {
                    path = Path.GetDirectoryName(Path.GetFullPath(name));
                }
            }
            if ((flag && !Directory.Exists(path)) && (!entry.IsDirectory || this.CreateEmptyDirectories))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception exception)
                {
                    flag = false;
                    if (this.events_ != null)
                    {
                        if (entry.IsDirectory)
                        {
                            this.continueRunning_ = this.events_.OnDirectoryFailure(name, exception);
                        }
                        else
                        {
                            this.continueRunning_ = this.events_.OnFileFailure(name, exception);
                        }
                    }
                    else
                    {
                        this.continueRunning_ = false;
                        throw;
                    }
                }
            }
            if (flag && entry.IsFile)
            {
                this.ExtractFileEntry(entry, name);
            }
        }

        private void ExtractFileEntry(ZipEntry entry, string targetName)
        {
            bool flag = true;
            if ((this.overwrite_ != Overwrite.Always) && File.Exists(targetName))
            {
                if ((this.overwrite_ == Overwrite.Prompt) && (this.confirmDelegate_ != null))
                {
                    flag = this.confirmDelegate_(targetName);
                }
                else
                {
                    flag = false;
                }
            }
            if (flag)
            {
                if (this.events_ != null)
                {
                    this.continueRunning_ = this.events_.OnProcessFile(entry.Name);
                }
                if (this.continueRunning_)
                {
                    try
                    {
                        using (FileStream stream = File.Create(targetName))
                        {
                            if (this.buffer_ == null)
                            {
                                this.buffer_ = new byte[0x1000];
                            }
                            if ((this.events_ != null) && (this.events_.Progress != null))
                            {
                                StreamUtils.Copy(this.zipFile_.GetInputStream(entry), stream, this.buffer_, this.events_.Progress, this.events_.ProgressInterval, this, entry.Name, entry.Size);
                            }
                            else
                            {
                                StreamUtils.Copy(this.zipFile_.GetInputStream(entry), stream, this.buffer_);
                            }
                            if (this.events_ != null)
                            {
                                this.continueRunning_ = this.events_.OnCompletedFile(entry.Name);
                            }
                        }
                        if (this.restoreDateTimeOnExtract_)
                        {
                            File.SetLastWriteTime(targetName, entry.DateTime);
                        }
                        if ((this.RestoreAttributesOnExtract && entry.IsDOSEntry) && (entry.ExternalFileAttributes != -1))
                        {
                            FileAttributes fileAttributes = ((FileAttributes) entry.ExternalFileAttributes) & (FileAttributes.Normal | FileAttributes.Archive | FileAttributes.Hidden | FileAttributes.ReadOnly);
                            File.SetAttributes(targetName, fileAttributes);
                        }
                    }
                    catch (Exception exception)
                    {
                        if (this.events_ == null)
                        {
                            this.continueRunning_ = false;
                            throw;
                        }
                        this.continueRunning_ = this.events_.OnFileFailure(targetName, exception);
                    }
                }
            }
        }

        public void ExtractZip(string zipFileName, string targetDirectory, string fileFilter)
        {
            this.ExtractZip(zipFileName, targetDirectory, Overwrite.Always, null, fileFilter, null, this.restoreDateTimeOnExtract_);
        }

        public void ExtractZip(string zipFileName, string targetDirectory, Overwrite overwrite, ConfirmOverwriteDelegate confirmDelegate, string fileFilter, string directoryFilter, bool restoreDateTime)
        {
            Stream inputStream = File.Open(zipFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            this.ExtractZip(inputStream, targetDirectory, overwrite, confirmDelegate, fileFilter, directoryFilter, restoreDateTime, true);
        }

        public void ExtractZip(Stream inputStream, string targetDirectory, Overwrite overwrite, ConfirmOverwriteDelegate confirmDelegate, string fileFilter, string directoryFilter, bool restoreDateTime, bool isStreamOwner)
        {
            if ((overwrite == Overwrite.Prompt) && (confirmDelegate == null))
            {
                throw new ArgumentNullException("confirmDelegate");
            }
            this.continueRunning_ = true;
            this.overwrite_ = overwrite;
            this.confirmDelegate_ = confirmDelegate;
            this.extractNameTransform_ = new WindowsNameTransform(targetDirectory);
            this.fileFilter_ = new NameFilter(fileFilter);
            this.directoryFilter_ = new NameFilter(directoryFilter);
            this.restoreDateTimeOnExtract_ = restoreDateTime;
            using (this.zipFile_ = new ZipFile(inputStream))
            {
                if (this.password_ != null)
                {
                    this.zipFile_.Password = this.password_;
                }
                this.zipFile_.IsStreamOwner = isStreamOwner;
                IEnumerator enumerator = this.zipFile_.GetEnumerator();
                while (this.continueRunning_ && enumerator.MoveNext())
                {
                    ZipEntry current = (ZipEntry) enumerator.Current;
                    if (current.IsFile)
                    {
                        if (this.directoryFilter_.IsMatch(Path.GetDirectoryName(current.Name)) && this.fileFilter_.IsMatch(current.Name))
                        {
                            this.ExtractEntry(current);
                        }
                    }
                    else if (current.IsDirectory && (this.directoryFilter_.IsMatch(current.Name) && this.CreateEmptyDirectories))
                    {
                        this.ExtractEntry(current);
                    }
                }
            }
        }

        private static int MakeExternalAttributes(FileInfo info)
        {
            return (int) info.Attributes;
        }

        private static bool NameIsValid(string name)
        {
            return (((name != null) && (name.Length > 0)) && (name.IndexOfAny(Path.GetInvalidPathChars()) < 0));
        }

        private void ProcessDirectory(object sender, DirectoryEventArgs e)
        {
            if (!e.HasMatchingFiles && this.CreateEmptyDirectories)
            {
                if (this.events_ != null)
                {
                    this.events_.OnProcessDirectory(e.Name, e.HasMatchingFiles);
                }
                if (e.ContinueRunning && (e.Name != this.sourceDirectory_))
                {
                    ZipEntry entry = this.entryFactory_.MakeDirectoryEntry(e.Name);
                    this.outputStream_.PutNextEntry(entry);
                }
            }
        }

        private void ProcessFile(object sender, ScanEventArgs e)
        {
            if ((this.events_ != null) && (this.events_.ProcessFile != null))
            {
                this.events_.ProcessFile(sender, e);
            }
            if (e.ContinueRunning)
            {
                try
                {
                    using (FileStream stream = File.Open(e.Name, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        ZipEntry entry = this.entryFactory_.MakeFileEntry(e.Name);
                        this.outputStream_.PutNextEntry(entry);
                        this.AddFileContents(e.Name, stream);
                    }
                }
                catch (Exception exception)
                {
                    if (this.events_ == null)
                    {
                        this.continueRunning_ = false;
                        throw;
                    }
                    this.continueRunning_ = this.events_.OnFileFailure(e.Name, exception);
                }
            }
        }

        public bool CreateEmptyDirectories
        {
            get
            {
                return this.createEmptyDirectories_;
            }
            set
            {
                this.createEmptyDirectories_ = value;
            }
        }

        public IEntryFactory EntryFactory
        {
            get
            {
                return this.entryFactory_;
            }
            set
            {
                if (value == null)
                {
                    this.entryFactory_ = new ZipEntryFactory();
                }
                else
                {
                    this.entryFactory_ = value;
                }
            }
        }

        public INameTransform NameTransform
        {
            get
            {
                return this.entryFactory_.NameTransform;
            }
            set
            {
                this.entryFactory_.NameTransform = value;
            }
        }

        public string Password
        {
            get
            {
                return this.password_;
            }
            set
            {
                this.password_ = value;
            }
        }

        public bool RestoreAttributesOnExtract
        {
            get
            {
                return this.restoreAttributesOnExtract_;
            }
            set
            {
                this.restoreAttributesOnExtract_ = value;
            }
        }

        public bool RestoreDateTimeOnExtract
        {
            get
            {
                return this.restoreDateTimeOnExtract_;
            }
            set
            {
                this.restoreDateTimeOnExtract_ = value;
            }
        }

        public ICSharpCode.SharpZipLib.Zip.UseZip64 UseZip64
        {
            get
            {
                return this.useZip64_;
            }
            set
            {
                this.useZip64_ = value;
            }
        }

        public delegate bool ConfirmOverwriteDelegate(string fileName);

        public enum Overwrite
        {
            Prompt,
            Never,
            Always
        }
    }
}

