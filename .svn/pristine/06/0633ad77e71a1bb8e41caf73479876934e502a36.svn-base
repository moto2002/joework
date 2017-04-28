namespace ICSharpCode.SharpZipLib.Core
{
    using System;
    using System.IO;

    public class FileSystemScanner
    {
        private bool alive_;
        public CompletedFileHandler CompletedFile;
        public DirectoryFailureHandler DirectoryFailure;
        private IScanFilter directoryFilter_;
        public FileFailureHandler FileFailure;
        private IScanFilter fileFilter_;
        public ProcessDirectoryHandler ProcessDirectory;
        public ProcessFileHandler ProcessFile;

        public FileSystemScanner(IScanFilter fileFilter)
        {
            this.fileFilter_ = fileFilter;
        }

        public FileSystemScanner(string filter)
        {
            this.fileFilter_ = new PathFilter(filter);
        }

        public FileSystemScanner(IScanFilter fileFilter, IScanFilter directoryFilter)
        {
            this.fileFilter_ = fileFilter;
            this.directoryFilter_ = directoryFilter;
        }

        public FileSystemScanner(string fileFilter, string directoryFilter)
        {
            this.fileFilter_ = new PathFilter(fileFilter);
            this.directoryFilter_ = new PathFilter(directoryFilter);
        }

        private void OnCompleteFile(string file)
        {
            CompletedFileHandler completedFile = this.CompletedFile;
            if (completedFile != null)
            {
                ScanEventArgs e = new ScanEventArgs(file);
                completedFile(this, e);
                this.alive_ = e.ContinueRunning;
            }
        }

        private bool OnDirectoryFailure(string directory, Exception e)
        {
            DirectoryFailureHandler directoryFailure = this.DirectoryFailure;
            bool flag = directoryFailure != null;
            if (flag)
            {
                ScanFailureEventArgs args = new ScanFailureEventArgs(directory, e);
                directoryFailure(this, args);
                this.alive_ = args.ContinueRunning;
            }
            return flag;
        }

        private bool OnFileFailure(string file, Exception e)
        {
            bool flag = this.FileFailure != null;
            if (flag)
            {
                ScanFailureEventArgs args = new ScanFailureEventArgs(file, e);
                this.FileFailure(this, args);
                this.alive_ = args.ContinueRunning;
            }
            return flag;
        }

        private void OnProcessDirectory(string directory, bool hasMatchingFiles)
        {
            ProcessDirectoryHandler processDirectory = this.ProcessDirectory;
            if (processDirectory != null)
            {
                DirectoryEventArgs e = new DirectoryEventArgs(directory, hasMatchingFiles);
                processDirectory(this, e);
                this.alive_ = e.ContinueRunning;
            }
        }

        private void OnProcessFile(string file)
        {
            ProcessFileHandler processFile = this.ProcessFile;
            if (processFile != null)
            {
                ScanEventArgs e = new ScanEventArgs(file);
                processFile(this, e);
                this.alive_ = e.ContinueRunning;
            }
        }

        public void Scan(string directory, bool recurse)
        {
            this.alive_ = true;
            this.ScanDir(directory, recurse);
        }

        private void ScanDir(string directory, bool recurse)
        {
            string[] files;
            Exception exception;
            try
            {
                files = Directory.GetFiles(directory);
                bool hasMatchingFiles = false;
                for (int i = 0; i < files.Length; i++)
                {
                    if (!this.fileFilter_.IsMatch(files[i]))
                    {
                        files[i] = null;
                    }
                    else
                    {
                        hasMatchingFiles = true;
                    }
                }
                this.OnProcessDirectory(directory, hasMatchingFiles);
                if (this.alive_ && hasMatchingFiles)
                {
                    foreach (string str in files)
                    {
                        try
                        {
                            if (str != null)
                            {
                                this.OnProcessFile(str);
                                if (!this.alive_)
                                {
                                    goto Label_00E3;
                                }
                            }
                        }
                        catch (Exception exception1)
                        {
                            exception = exception1;
                            if (!this.OnFileFailure(str, exception))
                            {
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception exception2)
            {
                exception = exception2;
                if (!this.OnDirectoryFailure(directory, exception))
                {
                    throw;
                }
            }
        Label_00E3:
            if (this.alive_ && recurse)
            {
                try
                {
                    files = Directory.GetDirectories(directory);
                    foreach (string str2 in files)
                    {
                        if ((this.directoryFilter_ == null) || this.directoryFilter_.IsMatch(str2))
                        {
                            this.ScanDir(str2, true);
                            if (!this.alive_)
                            {
                                return;
                            }
                        }
                    }
                }
                catch (Exception exception3)
                {
                    exception = exception3;
                    if (!this.OnDirectoryFailure(directory, exception))
                    {
                        throw;
                    }
                }
            }
        }
    }
}

