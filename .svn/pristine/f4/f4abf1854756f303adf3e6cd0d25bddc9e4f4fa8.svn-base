namespace ICSharpCode.SharpZipLib.Zip
{
    using System;
    using System.IO;

    public class ExtendedUnixData : ITaggedData
    {
        private DateTime _createTime = new DateTime(0x7b2, 1, 1);
        private Flags _flags;
        private DateTime _lastAccessTime = new DateTime(0x7b2, 1, 1);
        private DateTime _modificationTime = new DateTime(0x7b2, 1, 1);

        public byte[] GetData()
        {
            byte[] buffer;
            using (MemoryStream stream = new MemoryStream())
            {
                using (ZipHelperStream stream2 = new ZipHelperStream(stream))
                {
                    TimeSpan span;
                    int totalSeconds;
                    DateTime time;
                    stream2.IsStreamOwner = false;
                    stream2.WriteByte((byte) this._flags);
                    if (((byte) (this._flags & Flags.ModificationTime)) != 0)
                    {
                        time = new DateTime(0x7b2, 1, 1, 0, 0, 0);
                        span = (TimeSpan) (this._modificationTime.ToUniversalTime() - time.ToUniversalTime());
                        totalSeconds = (int) span.TotalSeconds;
                        stream2.WriteLEInt(totalSeconds);
                    }
                    if (((byte) (this._flags & Flags.AccessTime)) != 0)
                    {
                        time = new DateTime(0x7b2, 1, 1, 0, 0, 0);
                        span = (TimeSpan) (this._lastAccessTime.ToUniversalTime() - time.ToUniversalTime());
                        totalSeconds = (int) span.TotalSeconds;
                        stream2.WriteLEInt(totalSeconds);
                    }
                    if (((byte) (this._flags & Flags.CreateTime)) != 0)
                    {
                        time = new DateTime(0x7b2, 1, 1, 0, 0, 0);
                        span = (TimeSpan) (this._createTime.ToUniversalTime() - time.ToUniversalTime());
                        totalSeconds = (int) span.TotalSeconds;
                        stream2.WriteLEInt(totalSeconds);
                    }
                    buffer = stream.ToArray();
                }
            }
            return buffer;
        }

        public static bool IsValidValue(DateTime value)
        {
            return ((value >= new DateTime(0x76d, 12, 13, 20, 0x2d, 0x34)) || (value <= new DateTime(0x7f6, 1, 0x13, 3, 14, 7)));
        }

        public void SetData(byte[] data, int index, int count)
        {
            using (MemoryStream stream = new MemoryStream(data, index, count, false))
            {
                using (ZipHelperStream stream2 = new ZipHelperStream(stream))
                {
                    int num;
                    DateTime time;
                    this._flags = (Flags) ((byte) stream2.ReadByte());
                    if ((((byte) (this._flags & Flags.ModificationTime)) != 0) && (count >= 5))
                    {
                        num = stream2.ReadLEInt();
                        time = new DateTime(0x7b2, 1, 1, 0, 0, 0);
                        time = time.ToUniversalTime() + new TimeSpan(0, 0, 0, num, 0);
                        this._modificationTime = time.ToLocalTime();
                    }
                    if (((byte) (this._flags & Flags.AccessTime)) != 0)
                    {
                        num = stream2.ReadLEInt();
                        time = new DateTime(0x7b2, 1, 1, 0, 0, 0);
                        time = time.ToUniversalTime() + new TimeSpan(0, 0, 0, num, 0);
                        this._lastAccessTime = time.ToLocalTime();
                    }
                    if (((byte) (this._flags & Flags.CreateTime)) != 0)
                    {
                        num = stream2.ReadLEInt();
                        time = new DateTime(0x7b2, 1, 1, 0, 0, 0);
                        this._createTime = (time.ToUniversalTime() + new TimeSpan(0, 0, 0, num, 0)).ToLocalTime();
                    }
                }
            }
        }

        public DateTime AccessTime
        {
            get
            {
                return this._lastAccessTime;
            }
            set
            {
                if (!IsValidValue(value))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this._flags = (Flags) ((byte) (this._flags | Flags.AccessTime));
                this._lastAccessTime = value;
            }
        }

        public DateTime CreateTime
        {
            get
            {
                return this._createTime;
            }
            set
            {
                if (!IsValidValue(value))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this._flags = (Flags) ((byte) (this._flags | Flags.CreateTime));
                this._createTime = value;
            }
        }

        private Flags Include
        {
            get
            {
                return this._flags;
            }
            set
            {
                this._flags = value;
            }
        }

        public DateTime ModificationTime
        {
            get
            {
                return this._modificationTime;
            }
            set
            {
                if (!IsValidValue(value))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this._flags = (Flags) ((byte) (this._flags | Flags.ModificationTime));
                this._modificationTime = value;
            }
        }

        public short TagID
        {
            get
            {
                return 0x5455;
            }
        }

        [Flags]
        public enum Flags : byte
        {
            AccessTime = 2,
            CreateTime = 4,
            ModificationTime = 1
        }
    }
}

