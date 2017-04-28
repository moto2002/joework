namespace ICSharpCode.SharpZipLib.Core
{
    using System;

    public class ScanFailureEventArgs : EventArgs
    {
        private bool continueRunning_;
        private System.Exception exception_;
        private string name_;

        public ScanFailureEventArgs(string name, System.Exception e)
        {
            this.name_ = name;
            this.exception_ = e;
            this.continueRunning_ = true;
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

        public System.Exception Exception
        {
            get
            {
                return this.exception_;
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

