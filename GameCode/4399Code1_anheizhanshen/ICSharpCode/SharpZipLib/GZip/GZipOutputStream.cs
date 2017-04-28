namespace ICSharpCode.SharpZipLib.GZip
{
    using ICSharpCode.SharpZipLib.Checksums;
    using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
    using System;
    using System.IO;

    public class GZipOutputStream : DeflaterOutputStream
    {
        protected Crc32 crc;
        private OutputState state_;

        public GZipOutputStream(Stream baseOutputStream) : this(baseOutputStream, 0x1000)
        {
        }

        public GZipOutputStream(Stream baseOutputStream, int size) : base(baseOutputStream, new Deflater(-1, true), size)
        {
            this.crc = new Crc32();
            this.state_ = OutputState.Header;
        }

        public override void Close()
        {
            try
            {
                this.Finish();
            }
            finally
            {
                if (this.state_ != OutputState.Closed)
                {
                    this.state_ = OutputState.Closed;
                    if (base.IsStreamOwner)
                    {
                        base.baseOutputStream_.Close();
                    }
                }
            }
        }

        public override void Finish()
        {
            if (this.state_ == OutputState.Header)
            {
                this.WriteHeader();
            }
            if (this.state_ == OutputState.Footer)
            {
                this.state_ = OutputState.Finished;
                base.Finish();
                uint num = (uint) (((ulong) base.deflater_.TotalIn) & 0xffffffffL);
                uint num2 = (uint) (((ulong) this.crc.Value) & 0xffffffffL);
                byte[] buffer = new byte[] { (byte) num2, (byte) (num2 >> 8), (byte) (num2 >> 0x10), (byte) (num2 >> 0x18), (byte) num, (byte) (num >> 8), (byte) (num >> 0x10), (byte) (num >> 0x18) };
                base.baseOutputStream_.Write(buffer, 0, buffer.Length);
            }
        }

        public int GetLevel()
        {
            return base.deflater_.GetLevel();
        }

        public void SetLevel(int level)
        {
            if (level < 1)
            {
                throw new ArgumentOutOfRangeException("level");
            }
            base.deflater_.SetLevel(level);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this.state_ == OutputState.Header)
            {
                this.WriteHeader();
            }
            if (this.state_ != OutputState.Footer)
            {
                throw new InvalidOperationException("Write not permitted in current state");
            }
            this.crc.Update(buffer, offset, count);
            base.Write(buffer, offset, count);
        }

        private void WriteHeader()
        {
            if (this.state_ == OutputState.Header)
            {
                this.state_ = OutputState.Footer;
                DateTime time = new DateTime(0x7b2, 1, 1);
                int num = (int) ((DateTime.Now.Ticks - time.Ticks) / 0x989680L);
                byte[] buffer2 = new byte[] { 0x1f, 0x8b, 8, 0, 0, 0, 0, 0, 0, 0xff };
                buffer2[4] = (byte) num;
                buffer2[5] = (byte) (num >> 8);
                buffer2[6] = (byte) (num >> 0x10);
                buffer2[7] = (byte) (num >> 0x18);
                byte[] buffer = buffer2;
                base.baseOutputStream_.Write(buffer, 0, buffer.Length);
            }
        }

        private enum OutputState
        {
            Header,
            Footer,
            Finished,
            Closed
        }
    }
}

