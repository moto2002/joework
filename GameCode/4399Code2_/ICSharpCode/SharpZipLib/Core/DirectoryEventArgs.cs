namespace ICSharpCode.SharpZipLib.Core
{
    using System;

    public class DirectoryEventArgs : ScanEventArgs
    {
        private bool hasMatchingFiles_;

        public DirectoryEventArgs(string name, bool hasMatchingFiles) : base(name)
        {
            this.hasMatchingFiles_ = hasMatchingFiles;
        }

        public bool HasMatchingFiles
        {
            get
            {
                return this.hasMatchingFiles_;
            }
        }
    }
}

