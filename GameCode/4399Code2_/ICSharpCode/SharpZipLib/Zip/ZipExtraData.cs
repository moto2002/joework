namespace ICSharpCode.SharpZipLib.Zip
{
    using System;
    using System.IO;

    public sealed class ZipExtraData : IDisposable
    {
        private byte[] _data;
        private int _index;
        private MemoryStream _newEntry;
        private int _readValueLength;
        private int _readValueStart;

        public ZipExtraData()
        {
            this.Clear();
        }

        public ZipExtraData(byte[] data)
        {
            if (data == null)
            {
                this._data = new byte[0];
            }
            else
            {
                this._data = data;
            }
        }

        public void AddData(byte data)
        {
            this._newEntry.WriteByte(data);
        }

        public void AddData(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            this._newEntry.Write(data, 0, data.Length);
        }

        public void AddEntry(ITaggedData taggedData)
        {
            if (taggedData == null)
            {
                throw new ArgumentNullException("taggedData");
            }
            this.AddEntry(taggedData.TagID, taggedData.GetData());
        }

        public void AddEntry(int headerID, byte[] fieldData)
        {
            if ((headerID > 0xffff) || (headerID < 0))
            {
                throw new ArgumentOutOfRangeException("headerID");
            }
            int source = (fieldData == null) ? 0 : fieldData.Length;
            if (source > 0xffff)
            {
                throw new ArgumentOutOfRangeException("fieldData", "exceeds maximum length");
            }
            int num2 = (this._data.Length + source) + 4;
            if (this.Find(headerID))
            {
                num2 -= this.ValueLength + 4;
            }
            if (num2 > 0xffff)
            {
                throw new ZipException("Data exceeds maximum length");
            }
            this.Delete(headerID);
            byte[] array = new byte[num2];
            this._data.CopyTo(array, 0);
            int length = this._data.Length;
            this._data = array;
            this.SetShort(ref length, headerID);
            this.SetShort(ref length, source);
            if (fieldData != null)
            {
                fieldData.CopyTo(array, length);
            }
        }

        public void AddLeInt(int toAdd)
        {
            this.AddLeShort((short) toAdd);
            this.AddLeShort((short) (toAdd >> 0x10));
        }

        public void AddLeLong(long toAdd)
        {
            this.AddLeInt((int) (((ulong) toAdd) & 0xffffffffL));
            this.AddLeInt((int) (toAdd >> 0x20));
        }

        public void AddLeShort(int toAdd)
        {
            this._newEntry.WriteByte((byte) toAdd);
            this._newEntry.WriteByte((byte) (toAdd >> 8));
        }

        public void AddNewEntry(int headerID)
        {
            byte[] fieldData = this._newEntry.ToArray();
            this._newEntry = null;
            this.AddEntry(headerID, fieldData);
        }

        public void Clear()
        {
            if ((this._data == null) || (this._data.Length != 0))
            {
                this._data = new byte[0];
            }
        }

        private static ITaggedData Create(short tag, byte[] data, int offset, int count)
        {
            ITaggedData data2 = null;
            switch (tag)
            {
                case 10:
                    data2 = new NTTaggedData();
                    break;

                case 0x5455:
                    data2 = new ExtendedUnixData();
                    break;

                default:
                    data2 = new RawTaggedData(tag);
                    break;
            }
            data2.SetData(data, offset, count);
            return data2;
        }

        public bool Delete(int headerID)
        {
            bool flag = false;
            if (this.Find(headerID))
            {
                flag = true;
                int length = this._readValueStart - 4;
                byte[] destinationArray = new byte[this._data.Length - (this.ValueLength + 4)];
                Array.Copy(this._data, 0, destinationArray, 0, length);
                int sourceIndex = (length + this.ValueLength) + 4;
                Array.Copy(this._data, sourceIndex, destinationArray, length, this._data.Length - sourceIndex);
                this._data = destinationArray;
            }
            return flag;
        }

        public void Dispose()
        {
            if (this._newEntry != null)
            {
                this._newEntry.Close();
            }
        }

        public bool Find(int headerID)
        {
            this._readValueStart = this._data.Length;
            this._readValueLength = 0;
            this._index = 0;
            int num = this._readValueStart;
            int num2 = headerID - 1;
            while ((num2 != headerID) && (this._index < (this._data.Length - 3)))
            {
                num2 = this.ReadShortInternal();
                num = this.ReadShortInternal();
                if (num2 != headerID)
                {
                    this._index += num;
                }
            }
            bool flag = (num2 == headerID) && ((this._index + num) <= this._data.Length);
            if (flag)
            {
                this._readValueStart = this._index;
                this._readValueLength = num;
            }
            return flag;
        }

        private ITaggedData GetData(short tag)
        {
            ITaggedData data = null;
            if (this.Find(tag))
            {
                data = Create(tag, this._data, this._readValueStart, this._readValueLength);
            }
            return data;
        }

        public byte[] GetEntryData()
        {
            if (this.Length > 0xffff)
            {
                throw new ZipException("Data exceeds maximum length");
            }
            return (byte[]) this._data.Clone();
        }

        public Stream GetStreamForTag(int tag)
        {
            Stream stream = null;
            if (this.Find(tag))
            {
                stream = new MemoryStream(this._data, this._index, this._readValueLength, false);
            }
            return stream;
        }

        public int ReadByte()
        {
            int num = -1;
            if ((this._index < this._data.Length) && ((this._readValueStart + this._readValueLength) > this._index))
            {
                num = this._data[this._index];
                this._index++;
            }
            return num;
        }

        private void ReadCheck(int length)
        {
            if ((this._readValueStart > this._data.Length) || (this._readValueStart < 4))
            {
                throw new ZipException("Find must be called before calling a Read method");
            }
            if (this._index > ((this._readValueStart + this._readValueLength) - length))
            {
                throw new ZipException("End of extra data");
            }
            if ((this._index + length) < 4)
            {
                throw new ZipException("Cannot read before start of tag");
            }
        }

        public int ReadInt()
        {
            this.ReadCheck(4);
            int num = ((this._data[this._index] + (this._data[this._index + 1] << 8)) + (this._data[this._index + 2] << 0x10)) + (this._data[this._index + 3] << 0x18);
            this._index += 4;
            return num;
        }

        public long ReadLong()
        {
            this.ReadCheck(8);
            return ((this.ReadInt() & ((long) 0xffffffffL)) | (this.ReadInt() << 0x20));
        }

        public int ReadShort()
        {
            this.ReadCheck(2);
            int num = this._data[this._index] + (this._data[this._index + 1] << 8);
            this._index += 2;
            return num;
        }

        private int ReadShortInternal()
        {
            if (this._index > (this._data.Length - 2))
            {
                throw new ZipException("End of extra data");
            }
            int num = this._data[this._index] + (this._data[this._index + 1] << 8);
            this._index += 2;
            return num;
        }

        private void SetShort(ref int index, int source)
        {
            this._data[index] = (byte) source;
            this._data[index + 1] = (byte) (source >> 8);
            index += 2;
        }

        public void Skip(int amount)
        {
            this.ReadCheck(amount);
            this._index += amount;
        }

        public void StartNewEntry()
        {
            this._newEntry = new MemoryStream();
        }

        public int CurrentReadIndex
        {
            get
            {
                return this._index;
            }
        }

        public int Length
        {
            get
            {
                return this._data.Length;
            }
        }

        public int UnreadCount
        {
            get
            {
                if ((this._readValueStart > this._data.Length) || (this._readValueStart < 4))
                {
                    throw new ZipException("Find must be called before calling a Read method");
                }
                return ((this._readValueStart + this._readValueLength) - this._index);
            }
        }

        public int ValueLength
        {
            get
            {
                return this._readValueLength;
            }
        }
    }
}

