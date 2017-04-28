namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
    using ICSharpCode.SharpZipLib.Zip;
    using ICSharpCode.SharpZipLib.Zip.Compression;
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public class InflaterInputBuffer
    {
        private int available;
        private byte[] clearText;
        private int clearTextLength;
        private ICryptoTransform cryptoTransform;
        private Stream inputStream;
        private byte[] internalClearText;
        private byte[] rawData;
        private int rawLength;

        public InflaterInputBuffer(Stream stream) : this(stream, 0x1000)
        {
        }

        public InflaterInputBuffer(Stream stream, int bufferSize)
        {
            this.inputStream = stream;
            if (bufferSize < 0x400)
            {
                bufferSize = 0x400;
            }
            this.rawData = new byte[bufferSize];
            this.clearText = this.rawData;
        }

        public void Fill()
        {
            int num2;
            this.rawLength = 0;
            for (int i = this.rawData.Length; i > 0; i -= num2)
            {
                num2 = this.inputStream.Read(this.rawData, this.rawLength, i);
                if (num2 <= 0)
                {
                    break;
                }
                this.rawLength += num2;
            }
            if (this.cryptoTransform != null)
            {
                this.clearTextLength = this.cryptoTransform.TransformBlock(this.rawData, 0, this.rawLength, this.clearText, 0);
            }
            else
            {
                this.clearTextLength = this.rawLength;
            }
            this.available = this.clearTextLength;
        }

        public int ReadClearTextBuffer(byte[] outBuffer, int offset, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            int destinationIndex = offset;
            int num2 = length;
            while (num2 > 0)
            {
                if (this.available <= 0)
                {
                    this.Fill();
                    if (this.available <= 0)
                    {
                        return 0;
                    }
                }
                int num3 = Math.Min(num2, this.available);
                Array.Copy(this.clearText, this.clearTextLength - this.available, outBuffer, destinationIndex, num3);
                destinationIndex += num3;
                num2 -= num3;
                this.available -= num3;
            }
            return length;
        }

        public int ReadLeByte()
        {
            if (this.available <= 0)
            {
                this.Fill();
                if (this.available <= 0)
                {
                    throw new ZipException("EOF in header");
                }
            }
            byte num = this.rawData[this.rawLength - this.available];
            this.available--;
            return num;
        }

        public int ReadLeInt()
        {
            return (this.ReadLeShort() | (this.ReadLeShort() << 0x10));
        }

        public long ReadLeLong()
        {
            return (((long) ((ulong) this.ReadLeInt())) | (this.ReadLeInt() << 0x20));
        }

        public int ReadLeShort()
        {
            return (this.ReadLeByte() | (this.ReadLeByte() << 8));
        }

        public int ReadRawBuffer(byte[] buffer)
        {
            return this.ReadRawBuffer(buffer, 0, buffer.Length);
        }

        public int ReadRawBuffer(byte[] outBuffer, int offset, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            int destinationIndex = offset;
            int num2 = length;
            while (num2 > 0)
            {
                if (this.available <= 0)
                {
                    this.Fill();
                    if (this.available <= 0)
                    {
                        return 0;
                    }
                }
                int num3 = Math.Min(num2, this.available);
                Array.Copy(this.rawData, this.rawLength - this.available, outBuffer, destinationIndex, num3);
                destinationIndex += num3;
                num2 -= num3;
                this.available -= num3;
            }
            return length;
        }

        public void SetInflaterInput(Inflater inflater)
        {
            if (this.available > 0)
            {
                inflater.SetInput(this.clearText, this.clearTextLength - this.available, this.available);
                this.available = 0;
            }
        }

        public int Available
        {
            get
            {
                return this.available;
            }
            set
            {
                this.available = value;
            }
        }

        public byte[] ClearText
        {
            get
            {
                return this.clearText;
            }
        }

        public int ClearTextLength
        {
            get
            {
                return this.clearTextLength;
            }
        }

        public ICryptoTransform CryptoTransform
        {
            set
            {
                this.cryptoTransform = value;
                if (this.cryptoTransform != null)
                {
                    if (this.rawData == this.clearText)
                    {
                        if (this.internalClearText == null)
                        {
                            this.internalClearText = new byte[this.rawData.Length];
                        }
                        this.clearText = this.internalClearText;
                    }
                    this.clearTextLength = this.rawLength;
                    if (this.available > 0)
                    {
                        this.cryptoTransform.TransformBlock(this.rawData, this.rawLength - this.available, this.available, this.clearText, this.rawLength - this.available);
                    }
                }
                else
                {
                    this.clearText = this.rawData;
                    this.clearTextLength = this.rawLength;
                }
            }
        }

        public byte[] RawData
        {
            get
            {
                return this.rawData;
            }
        }

        public int RawLength
        {
            get
            {
                return this.rawLength;
            }
        }
    }
}

