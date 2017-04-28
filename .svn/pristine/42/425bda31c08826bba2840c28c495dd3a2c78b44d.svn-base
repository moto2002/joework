namespace ICSharpCode.SharpZipLib.Encryption
{
    using System;
    using System.Security.Cryptography;

    internal class PkzipClassicEncryptCryptoTransform : PkzipClassicCryptoBase, ICryptoTransform, IDisposable
    {
        internal PkzipClassicEncryptCryptoTransform(byte[] keyBlock)
        {
            base.SetKeys(keyBlock);
        }

        public void Dispose()
        {
            base.Reset();
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            for (int i = inputOffset; i < (inputOffset + inputCount); i++)
            {
                byte ch = inputBuffer[i];
                outputBuffer[outputOffset++] = (byte) (inputBuffer[i] ^ base.TransformByte());
                base.UpdateKeys(ch);
            }
            return inputCount;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            byte[] outputBuffer = new byte[inputCount];
            this.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, 0);
            return outputBuffer;
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
                return 1;
            }
        }

        public int OutputBlockSize
        {
            get
            {
                return 1;
            }
        }
    }
}

