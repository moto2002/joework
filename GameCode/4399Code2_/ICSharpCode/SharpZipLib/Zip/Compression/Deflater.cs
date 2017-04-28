namespace ICSharpCode.SharpZipLib.Zip.Compression
{
    using System;

    public class Deflater
    {
        public const int BEST_COMPRESSION = 9;
        public const int BEST_SPEED = 1;
        private const int BUSY_STATE = 0x10;
        private const int CLOSED_STATE = 0x7f;
        public const int DEFAULT_COMPRESSION = -1;
        public const int DEFLATED = 8;
        private DeflaterEngine engine;
        private const int FINISHED_STATE = 30;
        private const int FINISHING_STATE = 0x1c;
        private const int FLUSHING_STATE = 20;
        private const int INIT_STATE = 0;
        private const int IS_FINISHING = 8;
        private const int IS_FLUSHING = 4;
        private const int IS_SETDICT = 1;
        private int level;
        public const int NO_COMPRESSION = 0;
        private bool noZlibHeaderOrFooter;
        private DeflaterPending pending;
        private const int SETDICT_STATE = 1;
        private int state;
        private long totalOut;

        public Deflater() : this(-1, false)
        {
        }

        public Deflater(int level) : this(level, false)
        {
        }

        public Deflater(int level, bool noZlibHeaderOrFooter)
        {
            if (level == -1)
            {
                level = 6;
            }
            else if ((level < 0) || (level > 9))
            {
                throw new ArgumentOutOfRangeException("level");
            }
            this.pending = new DeflaterPending();
            this.engine = new DeflaterEngine(this.pending);
            this.noZlibHeaderOrFooter = noZlibHeaderOrFooter;
            this.SetStrategy(DeflateStrategy.Default);
            this.SetLevel(level);
            this.Reset();
        }

        public int Deflate(byte[] output)
        {
            return this.Deflate(output, 0, output.Length);
        }

        public int Deflate(byte[] output, int offset, int length)
        {
            int num = length;
            if (this.state == 0x7f)
            {
                throw new InvalidOperationException("Deflater closed");
            }
            if (this.state < 0x10)
            {
                int s = 0x7800;
                int num3 = (this.level - 1) >> 1;
                if ((num3 < 0) || (num3 > 3))
                {
                    num3 = 3;
                }
                s |= num3 << 6;
                if ((this.state & 1) != 0)
                {
                    s |= 0x20;
                }
                s += 0x1f - (s % 0x1f);
                this.pending.WriteShortMSB(s);
                if ((this.state & 1) != 0)
                {
                    int adler = this.engine.Adler;
                    this.engine.ResetAdler();
                    this.pending.WriteShortMSB(adler >> 0x10);
                    this.pending.WriteShortMSB(adler & 0xffff);
                }
                this.state = 0x10 | (this.state & 12);
            }
            while (true)
            {
                int num5 = this.pending.Flush(output, offset, length);
                offset += num5;
                this.totalOut += num5;
                length -= num5;
                if ((length == 0) || (this.state == 30))
                {
                    return (num - length);
                }
                if (!this.engine.Deflate((this.state & 4) != 0, (this.state & 8) != 0))
                {
                    if (this.state == 0x10)
                    {
                        return (num - length);
                    }
                    if (this.state == 20)
                    {
                        if (this.level != 0)
                        {
                            for (int i = 8 + (-this.pending.BitCount & 7); i > 0; i -= 10)
                            {
                                this.pending.WriteBits(2, 10);
                            }
                        }
                        this.state = 0x10;
                    }
                    else if (this.state == 0x1c)
                    {
                        this.pending.AlignToByte();
                        if (!this.noZlibHeaderOrFooter)
                        {
                            int num7 = this.engine.Adler;
                            this.pending.WriteShortMSB(num7 >> 0x10);
                            this.pending.WriteShortMSB(num7 & 0xffff);
                        }
                        this.state = 30;
                    }
                }
            }
        }

        public void Finish()
        {
            this.state |= 12;
        }

        public void Flush()
        {
            this.state |= 4;
        }

        public int GetLevel()
        {
            return this.level;
        }

        public void Reset()
        {
            this.state = this.noZlibHeaderOrFooter ? 0x10 : 0;
            this.totalOut = 0L;
            this.pending.Reset();
            this.engine.Reset();
        }

        public void SetDictionary(byte[] dictionary)
        {
            this.SetDictionary(dictionary, 0, dictionary.Length);
        }

        public void SetDictionary(byte[] dictionary, int index, int count)
        {
            if (this.state != 0)
            {
                throw new InvalidOperationException();
            }
            this.state = 1;
            this.engine.SetDictionary(dictionary, index, count);
        }

        public void SetInput(byte[] input)
        {
            this.SetInput(input, 0, input.Length);
        }

        public void SetInput(byte[] input, int offset, int count)
        {
            if ((this.state & 8) != 0)
            {
                throw new InvalidOperationException("Finish() already called");
            }
            this.engine.SetInput(input, offset, count);
        }

        public void SetLevel(int level)
        {
            if (level == -1)
            {
                level = 6;
            }
            else if ((level < 0) || (level > 9))
            {
                throw new ArgumentOutOfRangeException("level");
            }
            if (this.level != level)
            {
                this.level = level;
                this.engine.SetLevel(level);
            }
        }

        public void SetStrategy(DeflateStrategy strategy)
        {
            this.engine.Strategy = strategy;
        }

        public int Adler
        {
            get
            {
                return this.engine.Adler;
            }
        }

        public bool IsFinished
        {
            get
            {
                return ((this.state == 30) && this.pending.IsFlushed);
            }
        }

        public bool IsNeedingInput
        {
            get
            {
                return this.engine.NeedsInput();
            }
        }

        public long TotalIn
        {
            get
            {
                return this.engine.TotalIn;
            }
        }

        public long TotalOut
        {
            get
            {
                return this.totalOut;
            }
        }
    }
}

