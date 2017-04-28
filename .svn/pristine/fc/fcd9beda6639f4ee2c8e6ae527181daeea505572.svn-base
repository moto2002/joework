namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
    using ICSharpCode.SharpZipLib;
    using ICSharpCode.SharpZipLib.Encryption;
    using ICSharpCode.SharpZipLib.Zip;
    using ICSharpCode.SharpZipLib.Zip.Compression;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;

    public class DeflaterOutputStream : Stream
    {
        private static RNGCryptoServiceProvider _aesRnd;
        protected byte[] AESAuthCode;
        protected Stream baseOutputStream_;
        private byte[] buffer_;
        private ICryptoTransform cryptoTransform_;
        protected Deflater deflater_;
        private bool isClosed_;
        private bool isStreamOwner_;
        private string password;

        public DeflaterOutputStream(Stream baseOutputStream) : this(baseOutputStream, new Deflater(), 0x200)
        {
        }

        public DeflaterOutputStream(Stream baseOutputStream, Deflater deflater) : this(baseOutputStream, deflater, 0x200)
        {
        }

        public DeflaterOutputStream(Stream baseOutputStream, Deflater deflater, int bufferSize)
        {
            this.isStreamOwner_ = true;
            if (baseOutputStream == null)
            {
                throw new ArgumentNullException("baseOutputStream");
            }
            if (!baseOutputStream.CanWrite)
            {
                throw new ArgumentException("Must support writing", "baseOutputStream");
            }
            if (deflater == null)
            {
                throw new ArgumentNullException("deflater");
            }
            if (bufferSize < 0x200)
            {
                throw new ArgumentOutOfRangeException("bufferSize");
            }
            this.baseOutputStream_ = baseOutputStream;
            this.buffer_ = new byte[bufferSize];
            this.deflater_ = deflater;
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException("DeflaterOutputStream BeginRead not currently supported");
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException("BeginWrite is not supported");
        }

        public override void Close()
        {
            if (!this.isClosed_)
            {
                this.isClosed_ = true;
                try
                {
                    this.Finish();
                    if (this.cryptoTransform_ != null)
                    {
                        this.GetAuthCodeIfAES();
                        this.cryptoTransform_.Dispose();
                        this.cryptoTransform_ = null;
                    }
                }
                finally
                {
                    if (this.isStreamOwner_)
                    {
                        this.baseOutputStream_.Close();
                    }
                }
            }
        }

        protected void Deflate()
        {
            while (!this.deflater_.IsNeedingInput)
            {
                int length = this.deflater_.Deflate(this.buffer_, 0, this.buffer_.Length);
                if (length <= 0)
                {
                    break;
                }
                if (this.cryptoTransform_ != null)
                {
                    this.EncryptBlock(this.buffer_, 0, length);
                }
                this.baseOutputStream_.Write(this.buffer_, 0, length);
            }
            if (!this.deflater_.IsNeedingInput)
            {
                throw new SharpZipBaseException("DeflaterOutputStream can't deflate all input?");
            }
        }

        protected void EncryptBlock(byte[] buffer, int offset, int length)
        {
            this.cryptoTransform_.TransformBlock(buffer, 0, length, buffer, 0);
        }

        public virtual void Finish()
        {
            this.deflater_.Finish();
            while (!this.deflater_.IsFinished)
            {
                int length = this.deflater_.Deflate(this.buffer_, 0, this.buffer_.Length);
                if (length <= 0)
                {
                    break;
                }
                if (this.cryptoTransform_ != null)
                {
                    this.EncryptBlock(this.buffer_, 0, length);
                }
                this.baseOutputStream_.Write(this.buffer_, 0, length);
            }
            if (!this.deflater_.IsFinished)
            {
                throw new SharpZipBaseException("Can't deflate all input?");
            }
            this.baseOutputStream_.Flush();
            if (this.cryptoTransform_ != null)
            {
                if (this.cryptoTransform_ is ZipAESTransform)
                {
                    this.AESAuthCode = ((ZipAESTransform) this.cryptoTransform_).GetAuthCode();
                }
                this.cryptoTransform_.Dispose();
                this.cryptoTransform_ = null;
            }
        }

        public override void Flush()
        {
            this.deflater_.Flush();
            this.Deflate();
            this.baseOutputStream_.Flush();
        }

        private void GetAuthCodeIfAES()
        {
            if (this.cryptoTransform_ is ZipAESTransform)
            {
                this.AESAuthCode = ((ZipAESTransform) this.cryptoTransform_).GetAuthCode();
            }
        }

        protected void InitializeAESPassword(ZipEntry entry, string rawPassword, out byte[] salt, out byte[] pwdVerifier)
        {
            salt = new byte[entry.AESSaltLen];
            if (_aesRnd == null)
            {
                _aesRnd = new RNGCryptoServiceProvider();
            }
            _aesRnd.GetBytes(salt);
            int blockSize = entry.AESKeySize / 8;
            this.cryptoTransform_ = new ZipAESTransform(rawPassword, salt, blockSize, true);
            pwdVerifier = ((ZipAESTransform) this.cryptoTransform_).PwdVerifier;
        }

        protected void InitializePassword(string password)
        {
            PkzipClassicManaged managed = new PkzipClassicManaged();
            byte[] rgbKey = PkzipClassic.GenerateKeys(ZipConstants.ConvertToArray(password));
            this.cryptoTransform_ = managed.CreateEncryptor(rgbKey, null);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("DeflaterOutputStream Read not supported");
        }

        public override int ReadByte()
        {
            throw new NotSupportedException("DeflaterOutputStream ReadByte not supported");
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException("DeflaterOutputStream Seek not supported");
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("DeflaterOutputStream SetLength not supported");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.deflater_.SetInput(buffer, offset, count);
            this.Deflate();
        }

        public override void WriteByte(byte value)
        {
            byte[] buffer = new byte[] { value };
            this.Write(buffer, 0, 1);
        }

        public bool CanPatchEntries
        {
            get
            {
                return this.baseOutputStream_.CanSeek;
            }
        }

        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return this.baseOutputStream_.CanWrite;
            }
        }

        public bool IsStreamOwner
        {
            get
            {
                return this.isStreamOwner_;
            }
            set
            {
                this.isStreamOwner_ = value;
            }
        }

        public override long Length
        {
            get
            {
                return this.baseOutputStream_.Length;
            }
        }

        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                if ((value != null) && (value.Length == 0))
                {
                    this.password = null;
                }
                else
                {
                    this.password = value;
                }
            }
        }

        public override long Position
        {
            get
            {
                return this.baseOutputStream_.Position;
            }
            set
            {
                throw new NotSupportedException("Position property not supported");
            }
        }
    }
}

