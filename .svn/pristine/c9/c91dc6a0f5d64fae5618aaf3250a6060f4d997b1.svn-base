namespace ICSharpCode.SharpZipLib.Zip
{
    using System;

    public class TestStatus
    {
        private long bytesTested_;
        private ZipEntry entry_;
        private bool entryValid_;
        private int errorCount_;
        private ZipFile file_;
        private TestOperation operation_;

        public TestStatus(ZipFile file)
        {
            this.file_ = file;
        }

        internal void AddError()
        {
            this.errorCount_++;
            this.entryValid_ = false;
        }

        internal void SetBytesTested(long value)
        {
            this.bytesTested_ = value;
        }

        internal void SetEntry(ZipEntry entry)
        {
            this.entry_ = entry;
            this.entryValid_ = true;
            this.bytesTested_ = 0L;
        }

        internal void SetOperation(TestOperation operation)
        {
            this.operation_ = operation;
        }

        public long BytesTested
        {
            get
            {
                return this.bytesTested_;
            }
        }

        public ZipEntry Entry
        {
            get
            {
                return this.entry_;
            }
        }

        public bool EntryValid
        {
            get
            {
                return this.entryValid_;
            }
        }

        public int ErrorCount
        {
            get
            {
                return this.errorCount_;
            }
        }

        public ZipFile File
        {
            get
            {
                return this.file_;
            }
        }

        public TestOperation Operation
        {
            get
            {
                return this.operation_;
            }
        }
    }
}

