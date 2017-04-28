namespace MsgPack
{
    using System;
    using System.IO;

    internal class StreamPacker : Packer
    {
        private readonly bool _ownsStream;
        private readonly Stream _stream;

        public StreamPacker(Stream output, bool ownsStream)
        {
            this._stream = output;
            this._ownsStream = ownsStream;
        }

        protected sealed override void Dispose(bool disposing)
        {
            if (this._ownsStream)
            {
                this._stream.Dispose();
            }
            base.Dispose(disposing);
        }

        protected sealed override void SeekTo(long offset)
        {
            if (!this.CanSeek)
            {
                throw new NotSupportedException();
            }
            this._stream.Seek(offset, SeekOrigin.Current);
        }

        protected sealed override void WriteByte(byte value)
        {
            this._stream.WriteByte(value);
        }

        protected sealed override void WriteBytes(byte[] asArray, bool isImmutable)
        {
            this._stream.Write(asArray, 0, asArray.Length);
        }

        public sealed override bool CanSeek
        {
            get
            {
                return this._stream.CanSeek;
            }
        }

        public sealed override long Position
        {
            get
            {
                return this._stream.Position;
            }
        }
    }
}

