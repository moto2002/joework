namespace ICSharpCode.SharpZipLib.GZip
{
    using ICSharpCode.SharpZipLib.Checksums;
    using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
    using System;
    using System.IO;

    public class GZipInputStream : InflaterInputStream
    {
        protected Crc32 crc;
        private bool readGZIPHeader;

        public GZipInputStream(Stream baseInputStream) : this(baseInputStream, 0x1000)
        {
        }

        public GZipInputStream(Stream baseInputStream, int size) : base(baseInputStream, new Inflater(true), size)
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            while (true)
            {
                if (!this.readGZIPHeader && !this.ReadHeader())
                {
                    return 0;
                }
                int num = base.Read(buffer, offset, count);
                if (num > 0)
                {
                    this.crc.Update(buffer, offset, num);
                }
                if (base.inf.IsFinished)
                {
                    this.ReadFooter();
                }
                if (num > 0)
                {
                    return num;
                }
            }
        }

        private void ReadFooter()
        {
            int num3;
            byte[] outBuffer = new byte[8];
            long num = base.inf.TotalOut & ((long) 0xffffffffL);
            base.inputBuffer.Available += base.inf.RemainingInput;
            base.inf.Reset();
            for (int i = 8; i > 0; i -= num3)
            {
                num3 = base.inputBuffer.ReadClearTextBuffer(outBuffer, 8 - i, i);
                if (num3 <= 0)
                {
                    throw new EndOfStreamException("EOS reading GZIP footer");
                }
            }
            int num4 = (((outBuffer[0] & 0xff) | ((outBuffer[1] & 0xff) << 8)) | ((outBuffer[2] & 0xff) << 0x10)) | (outBuffer[3] << 0x18);
            if (num4 != ((int) this.crc.Value))
            {
                throw new GZipException(string.Concat(new object[] { "GZIP crc sum mismatch, theirs \"", num4, "\" and ours \"", (int) this.crc.Value }));
            }
            uint num5 = (uint) ((((outBuffer[4] & 0xff) | ((outBuffer[5] & 0xff) << 8)) | ((outBuffer[6] & 0xff) << 0x10)) | (outBuffer[7] << 0x18));
            if (num != num5)
            {
                throw new GZipException("Number of bytes mismatch in footer");
            }
            this.readGZIPHeader = false;
        }

        private bool ReadHeader()
        {
            int num4;
            int num5;
            this.crc = new Crc32();
            if (base.inputBuffer.Available <= 0)
            {
                base.inputBuffer.Fill();
                if (base.inputBuffer.Available <= 0)
                {
                    return false;
                }
            }
            Crc32 crc = new Crc32();
            int num = base.inputBuffer.ReadLeByte();
            if (num < 0)
            {
                throw new EndOfStreamException("EOS reading GZIP header");
            }
            crc.Update(num);
            if (num != 0x1f)
            {
                throw new GZipException("Error GZIP header, first magic byte doesn't match");
            }
            num = base.inputBuffer.ReadLeByte();
            if (num < 0)
            {
                throw new EndOfStreamException("EOS reading GZIP header");
            }
            if (num != 0x8b)
            {
                throw new GZipException("Error GZIP header,  second magic byte doesn't match");
            }
            crc.Update(num);
            int num2 = base.inputBuffer.ReadLeByte();
            if (num2 < 0)
            {
                throw new EndOfStreamException("EOS reading GZIP header");
            }
            if (num2 != 8)
            {
                throw new GZipException("Error GZIP header, data not in deflate format");
            }
            crc.Update(num2);
            int num3 = base.inputBuffer.ReadLeByte();
            if (num3 < 0)
            {
                throw new EndOfStreamException("EOS reading GZIP header");
            }
            crc.Update(num3);
            if ((num3 & 0xe0) != 0)
            {
                throw new GZipException("Reserved flag bits in GZIP header != 0");
            }
            for (num4 = 0; num4 < 6; num4++)
            {
                num5 = base.inputBuffer.ReadLeByte();
                if (num5 < 0)
                {
                    throw new EndOfStreamException("EOS reading GZIP header");
                }
                crc.Update(num5);
            }
            if ((num3 & 4) != 0)
            {
                for (num4 = 0; num4 < 2; num4++)
                {
                    num5 = base.inputBuffer.ReadLeByte();
                    if (num5 < 0)
                    {
                        throw new EndOfStreamException("EOS reading GZIP header");
                    }
                    crc.Update(num5);
                }
                if ((base.inputBuffer.ReadLeByte() < 0) || (base.inputBuffer.ReadLeByte() < 0))
                {
                    throw new EndOfStreamException("EOS reading GZIP header");
                }
                int num6 = base.inputBuffer.ReadLeByte();
                int num7 = base.inputBuffer.ReadLeByte();
                if ((num6 < 0) || (num7 < 0))
                {
                    throw new EndOfStreamException("EOS reading GZIP header");
                }
                crc.Update(num6);
                crc.Update(num7);
                int num8 = (num6 << 8) | num7;
                for (num4 = 0; num4 < num8; num4++)
                {
                    num5 = base.inputBuffer.ReadLeByte();
                    if (num5 < 0)
                    {
                        throw new EndOfStreamException("EOS reading GZIP header");
                    }
                    crc.Update(num5);
                }
            }
            if ((num3 & 8) != 0)
            {
                while ((num5 = base.inputBuffer.ReadLeByte()) > 0)
                {
                    crc.Update(num5);
                }
                if (num5 < 0)
                {
                    throw new EndOfStreamException("EOS reading GZIP header");
                }
                crc.Update(num5);
            }
            if ((num3 & 0x10) != 0)
            {
                while ((num5 = base.inputBuffer.ReadLeByte()) > 0)
                {
                    crc.Update(num5);
                }
                if (num5 < 0)
                {
                    throw new EndOfStreamException("EOS reading GZIP header");
                }
                crc.Update(num5);
            }
            if ((num3 & 2) != 0)
            {
                int num10 = base.inputBuffer.ReadLeByte();
                if (num10 < 0)
                {
                    throw new EndOfStreamException("EOS reading GZIP header");
                }
                int num9 = base.inputBuffer.ReadLeByte();
                if (num9 < 0)
                {
                    throw new EndOfStreamException("EOS reading GZIP header");
                }
                num10 = (num10 << 8) | num9;
                if (num10 != (((int) crc.Value) & 0xffff))
                {
                    throw new GZipException("Header CRC value mismatch");
                }
            }
            this.readGZIPHeader = true;
            return true;
        }
    }
}

