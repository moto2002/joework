namespace MsgPack
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public abstract class Unpacker : IEnumerable<MessagePackObject>, IEnumerable, IDisposable
    {
        private bool _isSubtreeReading = false;
        private UnpackerMode _mode = UnpackerMode.Unknown;

        protected Unpacker()
        {
        }

        public static Unpacker Create(Stream stream)
        {
            return Create(stream, true);
        }

        public static Unpacker Create(Stream stream, bool ownsStream)
        {
            return new ItemsUnpacker(stream, ownsStream);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        protected internal virtual void EndReadSubtree()
        {
            this._isSubtreeReading = false;
            this.SetStable();
        }

        internal void EnsureNotInSubtreeMode()
        {
            this.VerifyMode(UnpackerMode.Streaming);
            if (this._isSubtreeReading)
            {
                throw new InvalidOperationException("Unpacker is in 'Subtree' mode.");
            }
        }

        public IEnumerator<MessagePackObject> GetEnumerator()
        {
            this.VerifyMode(UnpackerMode.Enumerating);
            while (this.ReadCore())
            {
                if (this.Data.HasValue)
                {
                    yield return this.Data.Value;
                }
                this.VerifyMode(UnpackerMode.Enumerating);
            }
            this.SetStable();
        }

        private Exception NewInvalidModeException()
        {
            return new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Reader is in '{0}' mode.", new object[] { this._mode }));
        }

        public bool Read()
        {
            this.EnsureNotInSubtreeMode();
            bool flag = this.ReadCore();
            if (!((!flag || this.IsArrayHeader) || this.IsMapHeader))
            {
                this.SetStable();
            }
            return flag;
        }

        public virtual bool ReadArrayLength(out long result)
        {
            if (!this.Read())
            {
                result = 0L;
                return false;
            }
            if (!this.IsArrayHeader)
            {
                throw new MessageTypeException("Not in map header.");
            }
            result = this.Data.Value.AsInt64();
            return true;
        }

        public virtual bool ReadBinary(out byte[] result)
        {
            if (!this.Read())
            {
                result = null;
                return false;
            }
            result = this.Data.Value.AsBinary();
            return true;
        }

        [CLSCompliant(false)]
        public virtual bool ReadBoolean(out bool result)
        {
            if (!this.Read())
            {
                result = false;
                return false;
            }
            result = this.Data.Value.AsBoolean();
            return true;
        }

        [CLSCompliant(false)]
        public virtual bool ReadByte(out byte result)
        {
            if (!this.Read())
            {
                result = 0;
                return false;
            }
            result = this.Data.Value.AsByte();
            return true;
        }

        protected abstract bool ReadCore();
        [CLSCompliant(false)]
        public virtual bool ReadDouble(out double result)
        {
            if (!this.Read())
            {
                result = 0.0;
                return false;
            }
            result = this.Data.Value.AsDouble();
            return true;
        }

        [CLSCompliant(false)]
        public virtual bool ReadInt16(out short result)
        {
            if (!this.Read())
            {
                result = 0;
                return false;
            }
            result = this.Data.Value.AsInt16();
            return true;
        }

        [CLSCompliant(false)]
        public virtual bool ReadInt32(out int result)
        {
            if (!this.Read())
            {
                result = 0;
                return false;
            }
            result = this.Data.Value.AsInt32();
            return true;
        }

        [CLSCompliant(false)]
        public virtual bool ReadInt64(out long result)
        {
            if (!this.Read())
            {
                result = 0L;
                return false;
            }
            result = this.Data.Value.AsInt64();
            return true;
        }

        public MessagePackObject? ReadItem()
        {
            if (!this.Read())
            {
                return null;
            }
            this.UnpackSubtree();
            return this.Data;
        }

        public virtual bool ReadMapLength(out long result)
        {
            if (!this.Read())
            {
                result = 0L;
                return false;
            }
            if (!this.IsMapHeader)
            {
                throw new MessageTypeException("Not in map header.");
            }
            result = this.Data.Value.AsInt64();
            return true;
        }

        [CLSCompliant(false)]
        public virtual bool ReadNullableBoolean(out bool? result)
        {
            if (!this.Read())
            {
                result = 0;
                return false;
            }
            result = this.Data.Value.IsNil ? ((bool?) false) : new bool?(this.Data.Value.AsBoolean());
            return true;
        }

        [CLSCompliant(false)]
        public virtual bool ReadNullableByte(out byte? result)
        {
            if (!this.Read())
            {
                result = 0;
                return false;
            }
            result = this.Data.Value.IsNil ? ((byte?) 0) : new byte?(this.Data.Value.AsByte());
            return true;
        }

        [CLSCompliant(false)]
        public virtual bool ReadNullableDouble(out double? result)
        {
            if (!this.Read())
            {
                result = 0;
                return false;
            }
            result = this.Data.Value.IsNil ? 0 : new double?(this.Data.Value.AsDouble());
            return true;
        }

        [CLSCompliant(false)]
        public virtual bool ReadNullableInt16(out short? result)
        {
            if (!this.Read())
            {
                result = 0;
                return false;
            }
            result = this.Data.Value.IsNil ? ((short?) 0) : new short?(this.Data.Value.AsInt16());
            return true;
        }

        [CLSCompliant(false)]
        public virtual bool ReadNullableInt32(out int? result)
        {
            if (!this.Read())
            {
                result = 0;
                return false;
            }
            result = this.Data.Value.IsNil ? 0 : new int?(this.Data.Value.AsInt32());
            return true;
        }

        [CLSCompliant(false)]
        public virtual bool ReadNullableInt64(out long? result)
        {
            if (!this.Read())
            {
                result = 0;
                return false;
            }
            result = this.Data.Value.IsNil ? 0 : new long?(this.Data.Value.AsInt64());
            return true;
        }

        [CLSCompliant(false)]
        public virtual bool ReadNullableSByte(out sbyte? result)
        {
            if (!this.Read())
            {
                result = 0;
                return false;
            }
            result = this.Data.Value.IsNil ? ((sbyte?) 0) : new sbyte?(this.Data.Value.AsSByte());
            return true;
        }

        [CLSCompliant(false)]
        public virtual bool ReadNullableSingle(out float? result)
        {
            if (!this.Read())
            {
                result = 0;
                return false;
            }
            result = this.Data.Value.IsNil ? 0 : new float?(this.Data.Value.AsSingle());
            return true;
        }

        [CLSCompliant(false)]
        public virtual bool ReadNullableUInt16(out ushort? result)
        {
            if (!this.Read())
            {
                result = 0;
                return false;
            }
            result = this.Data.Value.IsNil ? ((ushort?) 0) : new ushort?(this.Data.Value.AsUInt16());
            return true;
        }

        [CLSCompliant(false)]
        public virtual bool ReadNullableUInt32(out uint? result)
        {
            if (!this.Read())
            {
                result = 0;
                return false;
            }
            result = this.Data.Value.IsNil ? ((uint?) 0) : new uint?(this.Data.Value.AsUInt32());
            return true;
        }

        [CLSCompliant(false)]
        public virtual bool ReadNullableUInt64(out ulong? result)
        {
            if (!this.Read())
            {
                result = 0;
                return false;
            }
            result = this.Data.Value.IsNil ? ((ulong?) 0) : new ulong?(this.Data.Value.AsUInt64());
            return true;
        }

        public virtual bool ReadObject(out MessagePackObject result)
        {
            if (!this.Read())
            {
                result = new MessagePackObject();
                return false;
            }
            result = this.Data.Value;
            return true;
        }

        [CLSCompliant(false)]
        public virtual bool ReadSByte(out sbyte result)
        {
            if (!this.Read())
            {
                result = 0;
                return false;
            }
            result = this.Data.Value.AsSByte();
            return true;
        }

        [CLSCompliant(false)]
        public virtual bool ReadSingle(out float result)
        {
            if (!this.Read())
            {
                result = 0f;
                return false;
            }
            result = this.Data.Value.AsSingle();
            return true;
        }

        public virtual bool ReadString(out string result)
        {
            if (!this.Read())
            {
                result = null;
                return false;
            }
            result = this.Data.Value.AsString();
            return true;
        }

        public Unpacker ReadSubtree()
        {
            if (!(this.IsArrayHeader || this.IsMapHeader))
            {
                throw new InvalidOperationException("Unpacker does not locate on array nor map header.");
            }
            if (this._isSubtreeReading)
            {
                throw new InvalidOperationException("Unpacker is already in 'Subtree' mode.");
            }
            Unpacker objA = this.ReadSubtreeCore();
            this._isSubtreeReading = !object.ReferenceEquals(objA, this);
            return objA;
        }

        protected abstract Unpacker ReadSubtreeCore();
        [CLSCompliant(false)]
        public virtual bool ReadUInt16(out ushort result)
        {
            if (!this.Read())
            {
                result = 0;
                return false;
            }
            result = this.Data.Value.AsUInt16();
            return true;
        }

        [CLSCompliant(false)]
        public virtual bool ReadUInt32(out uint result)
        {
            if (!this.Read())
            {
                result = 0;
                return false;
            }
            result = this.Data.Value.AsUInt32();
            return true;
        }

        [CLSCompliant(false)]
        public virtual bool ReadUInt64(out ulong result)
        {
            if (!this.Read())
            {
                result = 0L;
                return false;
            }
            result = this.Data.Value.AsUInt64();
            return true;
        }

        private void SetStable()
        {
            this._mode = UnpackerMode.Unknown;
        }

        public long? Skip()
        {
            this.VerifyIsNotDisposed();
            switch (this._mode)
            {
                case UnpackerMode.Streaming:
                    if (!this.Data.HasValue)
                    {
                        throw this.NewInvalidModeException();
                    }
                    break;

                case UnpackerMode.Enumerating:
                    throw this.NewInvalidModeException();
            }
            this._mode = UnpackerMode.Skipping;
            if (this._isSubtreeReading)
            {
                throw new InvalidOperationException("Unpacker is in 'Subtree' mode.");
            }
            long? nullable = this.SkipCore();
            if (nullable.HasValue)
            {
                this.SetStable();
            }
            return nullable;
        }

        protected abstract long? SkipCore();
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public MessagePackObject? UnpackSubtree()
        {
            Unpacker unpacker;
            int num;
            if (this.IsArrayHeader)
            {
                MessagePackObject[] objArray = new MessagePackObject[this.Data.Value.AsUInt32()];
                using (unpacker = this.ReadSubtree())
                {
                    for (num = 0; num < objArray.Length; num++)
                    {
                        MessagePackObject? nullable = unpacker.ReadItem();
                        Contract.Assert(nullable.HasValue);
                        objArray[num] = nullable.Value;
                    }
                }
                this.Data = new MessagePackObject(objArray, true);
            }
            else if (this.IsMapHeader)
            {
                int initialCapacity = (int) this.Data.Value.AsUInt32();
                MessagePackObjectDictionary dictionary = new MessagePackObjectDictionary(initialCapacity);
                using (unpacker = this.ReadSubtree())
                {
                    for (num = 0; num < initialCapacity; num++)
                    {
                        MessagePackObject? nullable2 = unpacker.ReadItem();
                        MessagePackObject? nullable3 = unpacker.ReadItem();
                        Contract.Assert(nullable2.HasValue);
                        Contract.Assert(nullable3.HasValue);
                        dictionary.Add(nullable2.Value, nullable3.Value);
                    }
                }
                this.Data = new MessagePackObject(dictionary, true);
            }
            else
            {
                return null;
            }
            return this.Data;
        }

        private void VerifyIsNotDisposed()
        {
            if (this._mode == UnpackerMode.Disposed)
            {
                throw new ObjectDisposedException(base.GetType().FullName);
            }
        }

        private void VerifyMode(UnpackerMode mode)
        {
            this.VerifyIsNotDisposed();
            if (this._mode == UnpackerMode.Unknown)
            {
                this._mode = mode;
            }
            else if (this._mode != mode)
            {
                throw this.NewInvalidModeException();
            }
        }

        public abstract MessagePackObject? Data { get; protected set; }

        public abstract bool IsArrayHeader { get; }

        public abstract bool IsMapHeader { get; }

        public abstract long ItemsCount { get; }

        protected virtual Stream UnderlyingStream
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        internal virtual long? UnderlyingStreamPosition
        {
            get
            {
                return null;
            }
        }


        private enum UnpackerMode
        {
            Unknown,
            Skipping,
            Streaming,
            Enumerating,
            Disposed
        }
    }
}

