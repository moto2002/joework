namespace MsgPack
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct UnpackingResult<T> : IEquatable<UnpackingResult<T>>
    {
        private readonly int _readCount;
        private readonly T _value;
        public int ReadCount
        {
            get
            {
                return this._readCount;
            }
        }
        public T Value
        {
            get
            {
                return this._value;
            }
        }
        internal UnpackingResult(T value, int readCount)
        {
            this._value = value;
            this._readCount = readCount;
        }

        public override bool Equals(object obj)
        {
            return ((obj is UnpackingResult<T>) && this.Equals((UnpackingResult<T>) obj));
        }

        public bool Equals(UnpackingResult<T> other)
        {
            return ((this._readCount == other._readCount) && EqualityComparer<T>.Default.Equals(this._value, other._value));
        }

        public override int GetHashCode()
        {
            return (this._readCount.GetHashCode() ^ ((this._value == null) ? 0 : this._value.GetHashCode()));
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}({1} bytes)", new object[] { this._value, this._readCount });
        }

        public static bool operator ==(UnpackingResult<T> left, UnpackingResult<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(UnpackingResult<T> left, UnpackingResult<T> right)
        {
            return !left.Equals(right);
        }
    }
}

