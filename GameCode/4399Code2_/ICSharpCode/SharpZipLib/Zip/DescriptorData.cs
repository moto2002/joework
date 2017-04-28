namespace ICSharpCode.SharpZipLib.Zip
{
    using System;

    public class DescriptorData
    {
        private long compressedSize;
        private long crc;
        private long size;

        public long CompressedSize
        {
            get
            {
                return this.compressedSize;
            }
            set
            {
                this.compressedSize = value;
            }
        }

        public long Crc
        {
            get
            {
                return this.crc;
            }
            set
            {
                this.crc = value & ((long) 0xffffffffL);
            }
        }

        public long Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;
            }
        }
    }
}

