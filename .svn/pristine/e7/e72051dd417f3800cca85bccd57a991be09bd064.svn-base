namespace ICSharpCode.SharpZipLib.Core
{
    using System;
    using System.IO;

    [Obsolete("Use ExtendedPathFilter instead")]
    public class NameAndSizeFilter : PathFilter
    {
        private long maxSize_;
        private long minSize_;

        public NameAndSizeFilter(string filter, long minSize, long maxSize) : base(filter)
        {
            this.maxSize_ = 0x7fffffffffffffffL;
            this.MinSize = minSize;
            this.MaxSize = maxSize;
        }

        public override bool IsMatch(string name)
        {
            bool flag = base.IsMatch(name);
            if (flag)
            {
                FileInfo info = new FileInfo(name);
                long length = info.Length;
                flag = (this.MinSize <= length) && (this.MaxSize >= length);
            }
            return flag;
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

