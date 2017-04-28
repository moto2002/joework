namespace MsgPack
{
    using System;
    using System.IO;
    using System.Text;

    public abstract class UnpackingStreamReader : StreamReader
    {
        private readonly long _byteLength;

        internal UnpackingStreamReader(Stream stream, Encoding encoding, long byteLength) : base(stream, encoding, true)
        {
            this._byteLength = byteLength;
        }

        public long ByteLength
        {
            get
            {
                return this._byteLength;
            }
        }
    }
}

