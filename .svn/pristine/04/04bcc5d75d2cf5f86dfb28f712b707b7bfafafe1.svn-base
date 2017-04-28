namespace ICSharpCode.SharpZipLib.Core
{
    using System;

    public class ScanEventArgs : EventArgs
    {
        private bool continueRunning_ = true;
        private string name_;

        public ScanEventArgs(string name)
        {
            this.name_ = name;
        }

        public bool ContinueRunning
        {
            get
            {
                return this.continueRunning_;
            }
            set
            {
                this.continueRunning_ = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name_;
            }
        }
    }
}

