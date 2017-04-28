namespace ICSharpCode.SharpZipLib.Zip
{
    using ICSharpCode.SharpZipLib.Core;
    using System;

    public class FastZipEvents
    {
        public CompletedFileHandler CompletedFile;
        public DirectoryFailureHandler DirectoryFailure;
        public FileFailureHandler FileFailure;
        public ProcessDirectoryHandler ProcessDirectory;
        public ProcessFileHandler ProcessFile;
        public ProgressHandler Progress;
        private TimeSpan progressInterval_ = TimeSpan.FromSeconds(3.0);

        public bool OnCompletedFile(string file)
        {
            bool continueRunning = true;
            CompletedFileHandler completedFile = this.CompletedFile;
            if (completedFile != null)
            {
                ScanEventArgs e = new ScanEventArgs(file);
                completedFile(this, e);
                continueRunning = e.ContinueRunning;
            }
            return continueRunning;
        }

        public bool OnDirectoryFailure(string directory, Exception e)
        {
            bool continueRunning = false;
            DirectoryFailureHandler directoryFailure = this.DirectoryFailure;
            if (directoryFailure != null)
            {
                ScanFailureEventArgs args = new ScanFailureEventArgs(directory, e);
                directoryFailure(this, args);
                continueRunning = args.ContinueRunning;
            }
            return continueRunning;
        }

        public bool OnFileFailure(string file, Exception e)
        {
            FileFailureHandler fileFailure = this.FileFailure;
            bool continueRunning = fileFailure != null;
            if (continueRunning)
            {
                ScanFailureEventArgs args = new ScanFailureEventArgs(file, e);
                fileFailure(this, args);
                continueRunning = args.ContinueRunning;
            }
            return continueRunning;
        }

        public bool OnProcessDirectory(string directory, bool hasMatchingFiles)
        {
            bool continueRunning = true;
            ProcessDirectoryHandler processDirectory = this.ProcessDirectory;
            if (processDirectory != null)
            {
                DirectoryEventArgs e = new DirectoryEventArgs(directory, hasMatchingFiles);
                processDirectory(this, e);
                continueRunning = e.ContinueRunning;
            }
            return continueRunning;
        }

        public bool OnProcessFile(string file)
        {
            bool continueRunning = true;
            ProcessFileHandler processFile = this.ProcessFile;
            if (processFile != null)
            {
                ScanEventArgs e = new ScanEventArgs(file);
                processFile(this, e);
                continueRunning = e.ContinueRunning;
            }
            return continueRunning;
        }

        public TimeSpan ProgressInterval
        {
            get
            {
                return this.progressInterval_;
            }
            set
            {
                this.progressInterval_ = value;
            }
        }
    }
}

