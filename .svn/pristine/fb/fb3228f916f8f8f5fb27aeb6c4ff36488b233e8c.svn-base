namespace ICSharpCode.SharpZipLib.Encryption
{
    using ICSharpCode.SharpZipLib.Checksums;
    using System;

    internal class PkzipClassicCryptoBase
    {
        private uint[] keys;

        protected void Reset()
        {
            this.keys[0] = 0;
            this.keys[1] = 0;
            this.keys[2] = 0;
        }

        protected void SetKeys(byte[] keyData)
        {
            if (keyData == null)
            {
                throw new ArgumentNullException("keyData");
            }
            if (keyData.Length != 12)
            {
                throw new InvalidOperationException("Key length is not valid");
            }
            this.keys = new uint[] { (((keyData[3] << 0x18) | (keyData[2] << 0x10)) | (keyData[1] << 8)) | keyData[0], (((keyData[7] << 0x18) | (keyData[6] << 0x10)) | (keyData[5] << 8)) | keyData[4], (((keyData[11] << 0x18) | (keyData[10] << 0x10)) | (keyData[9] << 8)) | keyData[8] };
        }

        protected byte TransformByte()
        {
            uint num = (this.keys[2] & 0xffff) | 2;
            return (byte) ((num * (num ^ 1)) >> 8);
        }

        protected void UpdateKeys(byte ch)
        {
            this.keys[0] = Crc32.ComputeCrc32(this.keys[0], ch);
            this.keys[1] += (byte) this.keys[0];
            this.keys[1] = (this.keys[1] * 0x8088405) + 1;
            this.keys[2] = Crc32.ComputeCrc32(this.keys[2], (byte) (this.keys[1] >> 0x18));
        }
    }
}

