namespace ICSharpCode.SharpZipLib.Core
{
    using System;
    using System.IO;

    public class ExtendedPathFilter : PathFilter
    {
        private DateTime maxDate_;
        private long maxSize_;
        private DateTime minDate_;
        private long minSize_;

        public ExtendedPathFilter(string filter, DateTime minDate, DateTime maxDate) : base(filter)
        {
            this.maxSize_ = 0x7fffffffffffffffL;
            this.minDate_ = DateTime.MinValue;
            this.maxDate_ = DateTime.MaxValue;
            this.MinDate = minDate;
            this.MaxDate = maxDate;
        }

        public ExtendedPathFilter(string filter, long minSize, long maxSize) : base(filter)
        {
            this.maxSize_ = 0x7fffffffffffffffL;
            this.minDate_ = DateTime.MinValue;
            this.maxDate_ = DateTime.MaxValue;
            this.MinSize = minSize;
            this.MaxSize = maxSize;
        }

        public ExtendedPathFilter(string filter, long minSize, long maxSize, DateTime minDate, DateTime maxDate) : base(filter)
        {
            this.maxSize_ = 0x7fffffffffffffffL;
            this.minDate_ = DateTime.MinValue;
            this.maxDate_ = DateTime.MaxValue;
            this.MinSize = minSize;
            this.MaxSize = maxSize;
            this.MinDate = minDate;
            this.MaxDate = maxDate;
        }

        public override bool IsMatch(string name)
        {
            bool flag = base.IsMatch(name);
            if (flag)
            {
                FileInfo info = new FileInfo(name);
                flag = (((this.MinSize <= info.Length) && (this.MaxSize >= info.Length)) && (this.MinDate <= info.LastWriteTime)) && (this.MaxDate >= info.LastWriteTime);
            }
            return flag;
        }

        public DateTime MaxDate
        {
            get
            {
                return this.maxDate_;
            }
            set
            {
                if (this.minDate_ > value)
                {
                    throw new ArgumentOutOfRangeException("value", "Exceeds MinDate");
                }
                this.maxDate_ = value;
            }
        }

        public long MaxSize
        {
            get
            {
                return this.maxSize_;
            }
            set
            {
                if ((value < 0L) || (this.minSize_ > value))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this.maxSize_ = value;
            }
        }

        public DateTime MinDate
        {
            get
            {
                return this.minDate_;
            }
            set
            {
                if (value > this.maxDate_)
                {
                    throw new ArgumentOutOfRangeException("value", "Exceeds MaxDate");
                }
                this.minDate_ = value;
            }
        }

        public long MinSize
        {
            get
            {
                return this.minSize_;
            }
            set
            {
                if ((value < 0L) || (this.maxSize_ < value))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this.minSize_ = value;
            }
        }
    }
}

