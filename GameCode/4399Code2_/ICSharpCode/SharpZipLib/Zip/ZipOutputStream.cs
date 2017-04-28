namespace ICSharpCode.SharpZipLib.Zip
{
    using ICSharpCode.SharpZipLib.Checksums;
    using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
    using System;
    using System.Collections;
    using System.IO;

    public class ZipOutputStream : DeflaterOutputStream
    {
        private Crc32 crc;
        private long crcPatchPos;
        private ZipEntry curEntry;
        private CompressionMethod curMethod;
        private int defaultCompressionLevel;
        private ArrayList entries;
        private long offset;
        private bool patchEntryHeader;
        private long size;
        private long sizePatchPos;
        private ICSharpCode.SharpZipLib.Zip.UseZip64 useZip64_;
        private byte[] zipComment;

        public ZipOutputStream(Stream baseOutputStream) : base(baseOutputStream, new Deflater(-1, true))
        {
            this.entries = new ArrayList();
            this.crc = new Crc32();
            this.defaultCompressionLevel = -1;
            this.curMethod = CompressionMethod.Deflated;
            this.zipComment = new byte[0];
            this.crcPatchPos = -1L;
            this.sizePatchPos = -1L;
            this.useZip64_ = ICSharpCode.SharpZipLib.Zip.UseZip64.Dynamic;
        }

        public ZipOutputStream(Stream baseOutputStream, int bufferSize) : base(baseOutputStream, new Deflater(-1, true), bufferSize)
        {
            this.entries = new ArrayList();
            this.crc = new Crc32();
            this.defaultCompressionLevel = -1;
            this.curMethod = CompressionMethod.Deflated;
            this.zipComment = new byte[0];
            this.crcPatchPos = -1L;
            this.sizePatchPos = -1L;
            this.useZip64_ = ICSharpCode.SharpZipLib.Zip.UseZip64.Dynamic;
        }

        private static void AddExtraDataAES(ZipEntry entry, ZipExtraData extraData)
        {
            extraData.StartNewEntry();
            extraData.AddLeShort(2);
            extraData.AddLeShort(0x4541);
            extraData.AddData(entry.AESEncryptionStrength);
            extraData.AddLeShort((int) entry.CompressionMethod);
            extraData.AddNewEntry(0x9901);
        }

        public void CloseEntry()
        {
            if (this.curEntry == null)
            {
                throw new InvalidOperationException("No open entry");
            }
            long size = this.size;
            if (this.curMethod == CompressionMethod.Deflated)
            {
                if (this.size >= 0L)
                {
                    base.Finish();
                    size = base.deflater_.TotalOut;
                }
                else
                {
                    base.deflater_.Reset();
                }
            }
            if (this.curEntry.AESKeySize > 0)
            {
                base.baseOutputStream_.Write(base.AESAuthCode, 0, 10);
            }
            if (this.curEntry.Size < 0L)
            {
                this.curEntry.Size = this.size;
            }
            else if (this.curEntry.Size != this.size)
            {
                throw new ZipException(string.Concat(new object[] { "size was ", this.size, ", but I expected ", this.curEntry.Size }));
            }
            if (this.curEntry.CompressedSize < 0L)
            {
                this.curEntry.CompressedSize = size;
            }
            else if (this.curEntry.CompressedSize != size)
            {
                throw new ZipException(string.Concat(new object[] { "compressed size was ", size, ", but I expected ", this.curEntry.CompressedSize }));
            }
            if (this.curEntry.Crc < 0L)
            {
                this.curEntry.Crc = this.crc.Value;
            }
            else if (this.curEntry.Crc != this.crc.Value)
            {
                throw new ZipException(string.Concat(new object[] { "crc was ", this.crc.Value, ", but I expected ", this.curEntry.Crc }));
            }
            this.offset += size;
            if (this.curEntry.IsCrypted)
            {
                if (this.curEntry.AESKeySize > 0)
                {
                    this.curEntry.CompressedSize += this.curEntry.AESOverheadSize;
                }
                else
                {
                    this.curEntry.CompressedSize += 12L;
                }
            }
            if (this.patchEntryHeader)
            {
                this.patchEntryHeader = false;
                long position = base.baseOutputStream_.Position;
                base.baseOutputStream_.Seek(this.crcPatchPos, SeekOrigin.Begin);
                this.WriteLeInt((int) this.curEntry.Crc);
                if (this.curEntry.LocalHeaderRequiresZip64)
                {
                    if (this.sizePatchPos == -1L)
                    {
                        throw new ZipException("Entry requires zip64 but this has been turned off");
                    }
                    base.baseOutputStream_.Seek(this.sizePatchPos, SeekOrigin.Begin);
                    this.WriteLeLong(this.curEntry.Size);
                    this.WriteLeLong(this.curEntry.CompressedSize);
                }
                else
                {
                    this.WriteLeInt((int) this.curEntry.CompressedSize);
                    this.WriteLeInt((int) this.curEntry.Size);
                }
                base.baseOutputStream_.Seek(position, SeekOrigin.Begin);
            }
            if ((this.curEntry.Flags & 8) != 0)
            {
                this.WriteLeInt(0x8074b50);
                this.WriteLeInt((int) this.curEntry.Crc);
                if (this.curEntry.LocalHeaderRequiresZip64)
                {
                    this.WriteLeLong(this.curEntry.CompressedSize);
                    this.WriteLeLong(this.curEntry.Size);
                    this.offset += 0x18L;
                }
                else
                {
                    this.WriteLeInt((int) this.curEntry.CompressedSize);
                    this.WriteLeInt((int) this.curEntry.Size);
                    this.offset += 0x10L;
                }
            }
            this.entries.Add(this.curEntry);
            this.curEntry = null;
        }

        private void CopyAndEncrypt(byte[] buffer, int offset, int count)
        {
            byte[] destinationArray = new byte[0x1000];
            while (count > 0)
            {
                int length = (count < 0x1000) ? count : 0x1000;
                Array.Copy(buffer, offset, destinationArray, 0, length);
                base.EncryptBlock(destinationArray, 0, length);
                base.baseOutputStream_.Write(destinationArray, 0, length);
                count -= length;
                offset += length;
            }
        }

        public override void Finish()
        {
            if (this.entries != null)
            {
                if (this.curEntry != null)
                {
                    this.CloseEntry();
                }
                long count = this.entries.Count;
                long sizeEntries = 0L;
                foreach (ZipEntry entry in this.entries)
                {
                    this.WriteLeInt(0x2014b50);
                    this.WriteLeShort(0x33);
                    this.WriteLeShort(entry.Version);
                    this.WriteLeShort(entry.Flags);
                    this.WriteLeShort((short) entry.CompressionMethodForHeader);
                    this.WriteLeInt((int) entry.DosTime);
                    this.WriteLeInt((int) entry.Crc);
                    if (entry.IsZip64Forced() || (entry.CompressedSize >= 0xffffffffL))
                    {
                        this.WriteLeInt(-1);
                    }
                    else
                    {
                        this.WriteLeInt((int) entry.CompressedSize);
                    }
                    if (entry.IsZip64Forced() || (entry.Size >= 0xffffffffL))
                    {
                        this.WriteLeInt(-1);
                    }
                    else
                    {
                        this.WriteLeInt((int) entry.Size);
                    }
                    byte[] buffer = ZipConstants.ConvertToArray(entry.Flags, entry.Name);
                    if (buffer.Length > 0xffff)
                    {
                        throw new ZipException("Name too long.");
                    }
                    ZipExtraData extraData = new ZipExtraData(entry.ExtraData);
                    if (entry.CentralHeaderRequiresZip64)
                    {
                        extraData.StartNewEntry();
                        if (entry.IsZip64Forced() || (entry.Size >= 0xffffffffL))
                        {
                            extraData.AddLeLong(entry.Size);
                        }
                        if (entry.IsZip64Forced() || (entry.CompressedSize >= 0xffffffffL))
                        {
                            extraData.AddLeLong(entry.CompressedSize);
                        }
                        if (entry.Offset >= 0xffffffffL)
                        {
                            extraData.AddLeLong(entry.Offset);
                        }
                        extraData.AddNewEntry(1);
                    }
                    else
                    {
                        extraData.Delete(1);
                    }
                    if (entry.AESKeySize > 0)
                    {
                        AddExtraDataAES(entry, extraData);
                    }
                    byte[] entryData = extraData.GetEntryData();
                    byte[] buffer3 = (entry.Comment != null) ? ZipConstants.ConvertToArray(entry.Flags, entry.Comment) : new byte[0];
                    if (buffer3.Length > 0xffff)
                    {
                        throw new ZipException("Comment too long.");
                    }
                    this.WriteLeShort(buffer.Length);
                    this.WriteLeShort(entryData.Length);
                    this.WriteLeShort(buffer3.Length);
                    this.WriteLeShort(0);
                    this.WriteLeShort(0);
                    if (entry.ExternalFileAttributes != -1)
                    {
                        this.WriteLeInt(entry.ExternalFileAttributes);
                    }
                    else if (entry.IsDirectory)
                    {
                        this.WriteLeInt(0x10);
                    }
                    else
                    {
                        this.WriteLeInt(0);
                    }
                    if (entry.Offset >= 0xffffffffL)
                    {
                        this.WriteLeInt(-1);
                    }
                    else
                    {
                        this.WriteLeInt((int) entry.Offset);
                    }
                    if (buffer.Length > 0)
                    {
                        base.baseOutputStream_.Write(buffer, 0, buffer.Length);
                    }
                    if (entryData.Length > 0)
                    {
                        base.baseOutputStream_.Write(entryData, 0, entryData.Length);
                    }
                    if (buffer3.Length > 0)
                    {
                        base.baseOutputStream_.Write(buffer3, 0, buffer3.Length);
                    }
                    sizeEntries += ((0x2e + buffer.Length) + entryData.Length) + buffer3.Length;
                }
                using (ZipHelperStream stream = new ZipHelperStream(base.baseOutputStream_))
                {
                    stream.WriteEndOfCentralDirectory(count, sizeEntries, this.offset, this.zipComment);
                }
                this.entries = null;
            }
        }

        public int GetLevel()
        {
            return base.deflater_.GetLevel();
        }

        public void PutNextEntry(ZipEntry entry)
        {
            bool hasCrc;
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }
            if (this.entries == null)
            {
                throw new InvalidOperationException("ZipOutputStream was finished");
            }
            if (this.curEntry != null)
            {
                this.CloseEntry();
            }
            if (this.entries.Count == 0x7fffffff)
            {
                throw new ZipException("Too many entries for Zip file");
            }
            CompressionMethod compressionMethod = entry.CompressionMethod;
            int defaultCompressionLevel = this.defaultCompressionLevel;
            entry.Flags &= 0x800;
            this.patchEntryHeader = false;
            if (entry.Size == 0L)
            {
                entry.CompressedSize = entry.Size;
                entry.Crc = 0L;
                compressionMethod = CompressionMethod.Stored;
                hasCrc = true;
            }
            else
            {
                hasCrc = (entry.Size >= 0L) && entry.HasCrc;
                if (compressionMethod == CompressionMethod.Stored)
                {
                    if (!hasCrc)
                    {
                        if (!base.CanPatchEntries)
                        {
                            compressionMethod = CompressionMethod.Deflated;
                            defaultCompressionLevel = 0;
                        }
                    }
                    else
                    {
                        entry.CompressedSize = entry.Size;
                        hasCrc = entry.HasCrc;
                    }
                }
            }
            if (!hasCrc)
            {
                if (!base.CanPatchEntries)
                {
                    entry.Flags |= 8;
                }
                else
                {
                    this.patchEntryHeader = true;
                }
            }
            if (base.Password != null)
            {
                entry.IsCrypted = true;
                if (entry.Crc < 0L)
                {
                    entry.Flags |= 8;
                }
            }
            entry.Offset = this.offset;
            entry.CompressionMethod = compressionMethod;
            this.curMethod = compressionMethod;
            this.sizePatchPos = -1L;
            if ((this.useZip64_ == ICSharpCode.SharpZipLib.Zip.UseZip64.On) || ((entry.Size < 0L) && (this.useZip64_ == ICSharpCode.SharpZipLib.Zip.UseZip64.Dynamic)))
            {
                entry.ForceZip64();
            }
            this.WriteLeInt(0x4034b50);
            this.WriteLeShort(entry.Version);
            this.WriteLeShort(entry.Flags);
            this.WriteLeShort((byte) entry.CompressionMethodForHeader);
            this.WriteLeInt((int) entry.DosTime);
            if (hasCrc)
            {
                this.WriteLeInt((int) entry.Crc);
                if (entry.LocalHeaderRequiresZip64)
                {
                    this.WriteLeInt(-1);
                    this.WriteLeInt(-1);
                }
                else
                {
                    this.WriteLeInt(entry.IsCrypted ? (((int) entry.CompressedSize) + 12) : ((int) entry.CompressedSize));
                    this.WriteLeInt((int) entry.Size);
                }
            }
            else
            {
                if (this.patchEntryHeader)
                {
                    this.crcPatchPos = base.baseOutputStream_.Position;
                }
                this.WriteLeInt(0);
                if (this.patchEntryHeader)
                {
                    this.sizePatchPos = base.baseOutputStream_.Position;
                }
                if (entry.LocalHeaderRequiresZip64 || this.patchEntryHeader)
                {
                    this.WriteLeInt(-1);
                    this.WriteLeInt(-1);
                }
                else
                {
                    this.WriteLeInt(0);
                    this.WriteLeInt(0);
                }
            }
            byte[] buffer = ZipConstants.ConvertToArray(entry.Flags, entry.Name);
            if (buffer.Length > 0xffff)
            {
                throw new ZipException("Entry name too long.");
            }
            ZipExtraData extraData = new ZipExtraData(entry.ExtraData);
            if (entry.LocalHeaderRequiresZip64)
            {
                extraData.StartNewEntry();
                if (hasCrc)
                {
                    extraData.AddLeLong(entry.Size);
                    extraData.AddLeLong(entry.CompressedSize);
                }
                else
                {
                    extraData.AddLeLong(-1L);
                    extraData.AddLeLong(-1L);
                }
                extraData.AddNewEntry(1);
                if (!extraData.Find(1))
                {
                    throw new ZipException("Internal error cant find extra data");
                }
                if (this.patchEntryHeader)
                {
                    this.sizePatchPos = extraData.CurrentReadIndex;
                }
            }
            else
            {
                extraData.Delete(1);
            }
            if (entry.AESKeySize > 0)
            {
                AddExtraDataAES(entry, extraData);
            }
            byte[] entryData = extraData.GetEntryData();
            this.WriteLeShort(buffer.Length);
            this.WriteLeShort(entryData.Length);
            if (buffer.Length > 0)
            {
                base.baseOutputStream_.Write(buffer, 0, buffer.Length);
            }
            if (entry.LocalHeaderRequiresZip64 && this.patchEntryHeader)
            {
                this.sizePatchPos += base.baseOutputStream_.Position;
            }
            if (entryData.Length > 0)
            {
                base.baseOutputStream_.Write(entryData, 0, entryData.Length);
            }
            this.offset += (30 + buffer.Length) + entryData.Length;
            if (entry.AESKeySize > 0)
            {
                this.offset += entry.AESOverheadSize;
            }
            this.curEntry = entry;
            this.crc.Reset();
            if (compressionMethod == CompressionMethod.Deflated)
            {
                base.deflater_.Reset();
                base.deflater_.SetLevel(defaultCompressionLevel);
            }
            this.size = 0L;
            if (entry.IsCrypted)
            {
                if (entry.AESKeySize > 0)
                {
                    this.WriteAESHeader(entry);
                }
                else if (entry.Crc < 0L)
                {
                    this.WriteEncryptionHeader(entry.DosTime << 0x10);
                }
                else
                {
                    this.WriteEncryptionHeader(entry.Crc);
                }
            }
        }

        public void SetComment(string comment)
        {
            byte[] buffer = ZipConstants.ConvertToArray(comment);
            if (buffer.Length > 0xffff)
            {
                throw new ArgumentOutOfRangeException("comment");
            }
            this.zipComment = buffer;
        }

        public void SetLevel(int level)
        {
            base.deflater_.SetLevel(level);
            this.defaultCompressionLevel = level;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this.curEntry == null)
            {
                throw new InvalidOperationException("No open entry.");
            }
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", "Cannot be negative");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", "Cannot be negative");
            }
            if ((buffer.Length - offset) < count)
            {
                throw new ArgumentException("Invalid offset/count combination");
            }
            this.crc.Update(buffer, offset, count);
            this.size += count;
            switch (this.curMethod)
            {
                case CompressionMethod.Stored:
                    if (base.Password != null)
                    {
                        this.CopyAndEncrypt(buffer, offset, count);
                    }
                    else
                    {
                        base.baseOutputStream_.Write(buffer, offset, count);
                    }
                    break;

                case CompressionMethod.Deflated:
                    base.Write(buffer, offset, count);
                    break;
            }
        }

        private void WriteAESHeader(ZipEntry entry)
        {
            byte[] buffer;
            byte[] buffer2;
            base.InitializeAESPassword(entry, base.Password, out buffer, out buffer2);
            base.baseOutputStream_.Write(buffer, 0, buffer.Length);
            base.baseOutputStream_.Write(buffer2, 0, buffer2.Length);
        }

        private void WriteEncryptionHeader(long crcValue)
        {
            this.offset += 12L;
            base.InitializePassword(base.Password);
            byte[] buffer = new byte[12];
            new Random().NextBytes(buffer);
            buffer[11] = (byte) (crcValue >> 0x18);
            base.EncryptBlock(buffer, 0, buffer.Length);
            base.baseOutputStream_.Write(buffer, 0, buffer.Length);
        }

        private void WriteLeInt(int value)
        {
            this.WriteLeShort(value);
            this.WriteLeShort(value >> 0x10);
        }

        private void WriteLeLong(long value)
        {
            this.WriteLeInt((int) value);
            this.WriteLeInt((int) (value >> 0x20));
        }

        private void WriteLeShort(int value)
        {
            base.baseOutputStream_.WriteByte((byte) (value & 0xff));
            base.baseOutputStream_.WriteByte((byte) ((value >> 8) & 0xff));
        }

        public bool IsFinished
        {
            get
            {
                return (this.entries == null);
            }
        }

        public ICSharpCode.SharpZipLib.Zip.UseZip64 UseZip64
        {
            get
            {
                return this.useZip64_;
            }
            set
            {
                this.useZip64_ = value;
            }
        }
    }
}

