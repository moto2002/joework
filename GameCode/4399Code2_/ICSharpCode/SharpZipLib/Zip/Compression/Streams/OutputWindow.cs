namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
    using System;

    public class OutputWindow
    {
        private byte[] window = new byte[0x8000];
        private int windowEnd;
        private int windowFilled;
        private const int WindowMask = 0x7fff;
        private const int WindowSize = 0x8000;

        public void CopyDict(byte[] dictionary, int offset, int length)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }
            if (this.windowFilled > 0)
            {
                throw new InvalidOperationException();
            }
            if (length > 0x8000)
            {
                offset += length - 0x8000;
                length = 0x8000;
            }
            Array.Copy(dictionary, offset, this.window, 0, length);
            this.windowEnd = length & 0x7fff;
        }

        public int CopyOutput(byte[] output, int offset, int len)
        {
            int windowEnd = this.windowEnd;
            if (len > this.windowFilled)
            {
                len = this.windowFilled;
            }
            else
            {
                windowEnd = ((this.windowEnd - this.windowFilled) + len) & 0x7fff;
            }
            int num2 = len;
            int length = len - windowEnd;
            if (length > 0)
            {
                Array.Copy(this.window, 0x8000 - length, output, offset, length);
                offset += length;
                len = windowEnd;
            }
            Array.Copy(this.window, windowEnd - len, output, offset, len);
            this.windowFilled -= num2;
            if (this.windowFilled < 0)
            {
                throw new InvalidOperationException();
            }
            return num2;
        }

        public int CopyStored(StreamManipulator input, int length)
        {
            int num;
            length = Math.Min(Math.Min(length, 0x8000 - this.windowFilled), input.AvailableBytes);
            int num2 = 0x8000 - this.windowEnd;
            if (length > num2)
            {
                num = input.CopyBytes(this.window, this.windowEnd, num2);
                if (num == num2)
                {
                    num += input.CopyBytes(this.window, 0, length - num2);
                }
            }
            else
            {
                num = input.CopyBytes(this.window, this.windowEnd, length);
            }
            this.windowEnd = (this.windowEnd + num) & 0x7fff;
            this.windowFilled += num;
            return num;
        }

        public int GetAvailable()
        {
            return this.windowFilled;
        }

        public int GetFreeSpace()
        {
            return (0x8000 - this.windowFilled);
        }

        public void Repeat(int length, int distance)
        {
            this.windowFilled += length;
            if (this.windowFilled > 0x8000)
            {
                throw new InvalidOperationException("Window full");
            }
            int sourceIndex = (this.windowEnd - distance) & 0x7fff;
            int num2 = 0x8000 - length;
            if ((sourceIndex <= num2) && (this.windowEnd < num2))
            {
                if (length <= distance)
                {
                    Array.Copy(this.window, sourceIndex, this.window, this.windowEnd, length);
                    this.windowEnd += length;
                }
                else
                {
                    while (length-- > 0)
                    {
                        this.window[this.windowEnd++] = this.window[sourceIndex++];
                    }
                }
            }
            else
            {
                this.SlowRepeat(sourceIndex, length, distance);
            }
        }

        public void Reset()
        {
            this.windowFilled = this.windowEnd = 0;
        }

        private void SlowRepeat(int repStart, int length, int distance)
        {
            while (length-- > 0)
            {
                this.window[this.windowEnd++] = this.window[repStart++];
                this.windowEnd &= 0x7fff;
                repStart &= 0x7fff;
            }
        }

        public void Write(int value)
        {
            if (this.windowFilled++ == 0x8000)
            {
                throw new InvalidOperationException("Window full");
            }
            this.window[this.windowEnd++] = (byte) value;
            this.windowEnd &= 0x7fff;
        }
    }
}

