namespace ICSharpCode.SharpZipLib.Encryption
{
    using System;
    using System.Security.Cryptography;

    internal class ZipAESTransform : ICryptoTransform, IDisposable
    {
        private int _blockSize;
        private readonly byte[] _counterNonce;
        private int _encrPos;
        private byte[] _encryptBuffer;
        private ICryptoTransform _encryptor;
        private bool _finalised;
        private HMACSHA1 _hmacsha1;
        private byte[] _pwdVerifier;
        private bool _writeMode;
        private const int ENCRYPT_BLOCK = 0x10;
        private const int KEY_ROUNDS = 0x3e8;
        private const int PWD_VER_LENGTH = 2;

        public ZipAESTransform(string key, byte[] saltBytes, int blockSize, bool writeMode)
        {
            if ((blockSize != 0x10) && (blockSize != 0x20))
            {
                throw new Exception("Invalid blocksize " + blockSize + ". Must be 16 or 32.");
            }
            if (saltBytes.Length != (blockSize / 2))
            {
                throw new Exception(string.Concat(new object[] { "Invalid salt len. Must be ", blockSize / 2, " for blocksize ", blockSize }));
            }
            this._blockSize = blockSize;
            this._encryptBuffer = new byte[this._blockSize];
            this._encrPos = 0x10;
            Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(key, saltBytes, 0x3e8);
            RijndaelManaged managed = new RijndaelManaged {
                Mode = CipherMode.ECB
            };
            this._counterNonce = new byte[this._blockSize];
            byte[] rgbKey = bytes.GetBytes(this._blockSize);
            byte[] rgbIV = bytes.GetBytes(this._blockSize);
            this._encryptor = managed.CreateEncryptor(rgbKey, rgbIV);
            this._pwdVerifier = bytes.GetBytes(2);
            this._hmacsha1 = new HMACSHA1(rgbIV);
            this._writeMode = writeMode;
        }

        public void Dispose()
        {
            this._encryptor.Dispose();
        }

        public byte[] GetAuthCode()
        {
            if (!this._finalised)
            {
                byte[] inputBuffer = new byte[0];
                this._hmacsha1.TransformFinalBlock(inputBuffer, 0, 0);
                this._finalised = true;
            }
            return this._hmacsha1.Hash;
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            if (!this._writeMode)
            {
                this._hmacsha1.TransformBlock(inputBuffer, inputOffset, inputCount, inputBuffer, inputOffset);
            }
            for (int i = 0; i < inputCount; i++)
            {
                if (this._encrPos != 0x10)
                {
                    goto Label_0090;
                }
                int index = 0;
                goto Label_0042;
            Label_003C:
                index++;
            Label_0042:
                if ((this._counterNonce[index] = (byte) (this._counterNonce[index] + 1)) == 0)
                {
                    goto Label_003C;
                }
                this._encryptor.TransformBlock(this._counterNonce, 0, this._blockSize, this._encryptBuffer, 0);
                this._encrPos = 0;
            Label_0090:
                outputBuffer[i + outputOffset] = (byte) (inputBuffer[i + inputOffset] ^ this._encryptBuffer[this._encrPos++]);
            }
            if (this._writeMode)
            {
                this._hmacsha1.TransformBlock(outputBuffer, outputOffset, inputCount, outputBuffer, outputOffset);
            }
            return inputCount;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            throw new NotImplementedException("ZipAESTransform.TransformFinalBlock");
        }

        public bool CanReuseTransform
        {
            get
            {
                return true;
            }
        }

        public bool CanTransformMultipleBlocks
        {
            get
            {
                return true;
            }
        }

        public int InputBlockSize
        {
            get
            {
                return this._blockSize;
            }
        }

        public int OutputBlockSize
        {
            get
            {
                return this._blockSize;
            }
        }

        public byte[] PwdVerifier
        {
            get
            {
                return this._pwdVerifier;
            }
        }
    }
}

