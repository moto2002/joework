namespace ICSharpCode.SharpZipLib.Encryption
{
    using System;
    using System.Security.Cryptography;

    public sealed class PkzipClassicManaged : PkzipClassic
    {
        private byte[] key_;

        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            this.key_ = rgbKey;
            return new PkzipClassicDecryptCryptoTransform(this.Key);
        }

        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            this.key_ = rgbKey;
            return new PkzipClassicEncryptCryptoTransform(this.Key);
        }

        public override void GenerateIV()
        {
        }

        public override void GenerateKey()
        {
            this.key_ = new byte[12];
            new Random().NextBytes(this.key_);
        }

        public override int BlockSize
        {
            get
            {
                return 8;
            }
            set
            {
                if (value != 8)
                {
                    throw new CryptographicException("Block size is invalid");
                }
            }
        }

        public override byte[] Key
        {
            get
            {
                if (this.key_ == null)
                {
                    this.GenerateKey();
                }
                return (byte[]) this.key_.Clone();
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (value.Length != 12)
                {
                    throw new CryptographicException("Key size is illegal");
                }
                this.key_ = (byte[]) value.Clone();
            }
        }

        public override KeySizes[] LegalBlockSizes
        {
            get
            {
                return new KeySizes[] { new KeySizes(8, 8, 0) };
            }
        }

        public override KeySizes[] LegalKeySizes
        {
            get
            {
                return new KeySizes[] { new KeySizes(0x60, 0x60, 0) };
            }
        }
    }
}

