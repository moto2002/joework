namespace ICSharpCode.SharpZipLib.Zip.Compression
{
    using ICSharpCode.SharpZipLib.Checksums;
    using System;

    public class DeflaterEngine : DeflaterConstants
    {
        private Adler32 adler;
        private int blockStart;
        private int compressionFunction;
        private int goodLength;
        private short[] head;
        private DeflaterHuffman huffman;
        private byte[] inputBuf;
        private int inputEnd;
        private int inputOff;
        private int ins_h;
        private int lookahead;
        private int matchLen;
        private int matchStart;
        private int max_chain;
        private int max_lazy;
        private int niceLength;
        private DeflaterPending pending;
        private short[] prev;
        private bool prevAvailable;
        private DeflateStrategy strategy;
        private int strstart;
        private const int TooFar = 0x1000;
        private long totalIn;
        private byte[] window;

        public DeflaterEngine(DeflaterPending pending)
        {
            this.pending = pending;
            this.huffman = new DeflaterHuffman(pending);
            this.adler = new Adler32();
            this.window = new byte[0x10000];
            this.head = new short[0x8000];
            this.prev = new short[0x8000];
            this.blockStart = this.strstart = 1;
        }

        public bool Deflate(bool flush, bool finish)
        {
            bool flag;
            do
            {
                this.FillWindow();
                bool flag2 = flush && (this.inputOff == this.inputEnd);
                switch (this.compressionFunction)
                {
                    case 0:
                        flag = this.DeflateStored(flag2, finish);
                        break;

                    case 1:
                        flag = this.DeflateFast(flag2, finish);
                        break;

                    case 2:
                        flag = this.DeflateSlow(flag2, finish);
                        break;

                    default:
                        throw new InvalidOperationException("unknown compressionFunction");
                }
            }
            while (this.pending.IsFlushed && flag);
            return flag;
        }

        private bool DeflateFast(bool flush, bool finish)
        {
            if (!((this.lookahead >= 0x106) || flush))
            {
                return false;
            }
            while ((this.lookahead >= 0x106) || flush)
            {
                int num;
                if (this.lookahead == 0)
                {
                    this.huffman.FlushBlock(this.window, this.blockStart, this.strstart - this.blockStart, finish);
                    this.blockStart = this.strstart;
                    return false;
                }
                if (this.strstart > 0xfefa)
                {
                    this.SlideWindow();
                }
                if ((((this.lookahead >= 3) && ((num = this.InsertString()) != 0)) && ((this.strategy != DeflateStrategy.HuffmanOnly) && ((this.strstart - num) <= 0x7efa))) && this.FindLongestMatch(num))
                {
                    bool flag = this.huffman.TallyDist(this.strstart - this.matchStart, this.matchLen);
                    this.lookahead -= this.matchLen;
                    if ((this.matchLen <= this.max_lazy) && (this.lookahead >= 3))
                    {
                        while (--this.matchLen > 0)
                        {
                            this.strstart++;
                            this.InsertString();
                        }
                        this.strstart++;
                    }
                    else
                    {
                        this.strstart += this.matchLen;
                        if (this.lookahead >= 2)
                        {
                            this.UpdateHash();
                        }
                    }
                    this.matchLen = 2;
                    if (!flag)
                    {
                        continue;
                    }
                }
                else
                {
                    this.huffman.TallyLit(this.window[this.strstart] & 0xff);
                    this.strstart++;
                    this.lookahead--;
                }
                if (this.huffman.IsFull())
                {
                    bool lastBlock = finish && (this.lookahead == 0);
                    this.huffman.FlushBlock(this.window, this.blockStart, this.strstart - this.blockStart, lastBlock);
                    this.blockStart = this.strstart;
                    return !lastBlock;
                }
            }
            return true;
        }

        private bool DeflateSlow(bool flush, bool finish)
        {
            if (!((this.lookahead >= 0x106) || flush))
            {
                return false;
            }
            while ((this.lookahead >= 0x106) || flush)
            {
                if (this.lookahead == 0)
                {
                    if (this.prevAvailable)
                    {
                        this.huffman.TallyLit(this.window[this.strstart - 1] & 0xff);
                    }
                    this.prevAvailable = false;
                    this.huffman.FlushBlock(this.window, this.blockStart, this.strstart - this.blockStart, finish);
                    this.blockStart = this.strstart;
                    return false;
                }
                if (this.strstart >= 0xfefa)
                {
                    this.SlideWindow();
                }
                int matchStart = this.matchStart;
                int matchLen = this.matchLen;
                if (this.lookahead >= 3)
                {
                    int curMatch = this.InsertString();
                    if (((((this.strategy != DeflateStrategy.HuffmanOnly) && (curMatch != 0)) && ((this.strstart - curMatch) <= 0x7efa)) && this.FindLongestMatch(curMatch)) && ((this.matchLen <= 5) && ((this.strategy == DeflateStrategy.Filtered) || ((this.matchLen == 3) && ((this.strstart - this.matchStart) > 0x1000)))))
                    {
                        this.matchLen = 2;
                    }
                }
                if ((matchLen >= 3) && (this.matchLen <= matchLen))
                {
                    this.huffman.TallyDist((this.strstart - 1) - matchStart, matchLen);
                    matchLen -= 2;
                    do
                    {
                        this.strstart++;
                        this.lookahead--;
                        if (this.lookahead >= 3)
                        {
                            this.InsertString();
                        }
                    }
                    while (--matchLen > 0);
                    this.strstart++;
                    this.lookahead--;
                    this.prevAvailable = false;
                    this.matchLen = 2;
                }
                else
                {
                    if (this.prevAvailable)
                    {
                        this.huffman.TallyLit(this.window[this.strstart - 1] & 0xff);
                    }
                    this.prevAvailable = true;
                    this.strstart++;
                    this.lookahead--;
                }
                if (this.huffman.IsFull())
                {
                    int storedLength = this.strstart - this.blockStart;
                    if (this.prevAvailable)
                    {
                        storedLength--;
                    }
                    bool lastBlock = (finish && (this.lookahead == 0)) && !this.prevAvailable;
                    this.huffman.FlushBlock(this.window, this.blockStart, storedLength, lastBlock);
                    this.blockStart += storedLength;
                    return !lastBlock;
                }
            }
            return true;
        }

        private bool DeflateStored(bool flush, bool finish)
        {
            if (!(flush || (this.lookahead != 0)))
            {
                return false;
            }
            this.strstart += this.lookahead;
            this.lookahead = 0;
            int storedLength = this.strstart - this.blockStart;
            if (((storedLength >= DeflaterConstants.MAX_BLOCK_SIZE) || ((this.blockStart < 0x8000) && (storedLength >= 0x7efa))) || flush)
            {
                bool lastBlock = finish;
                if (storedLength > DeflaterConstants.MAX_BLOCK_SIZE)
                {
                    storedLength = DeflaterConstants.MAX_BLOCK_SIZE;
                    lastBlock = false;
                }
                this.huffman.FlushStoredBlock(this.window, this.blockStart, storedLength, lastBlock);
                this.blockStart += storedLength;
                return !lastBlock;
            }
            return true;
        }

        public void FillWindow()
        {
            if (this.strstart >= 0xfefa)
            {
                this.SlideWindow();
            }
            while ((this.lookahead < 0x106) && (this.inputOff < this.inputEnd))
            {
                int length = (0x10000 - this.lookahead) - this.strstart;
                if (length > (this.inputEnd - this.inputOff))
                {
                    length = this.inputEnd - this.inputOff;
                }
                Array.Copy(this.inputBuf, this.inputOff, this.window, this.strstart + this.lookahead, length);
                this.adler.Update(this.inputBuf, this.inputOff, length);
                this.inputOff += length;
                this.totalIn += length;
                this.lookahead += length;
            }
            if (this.lookahead >= 3)
            {
                this.UpdateHash();
            }
        }

        private bool FindLongestMatch(int curMatch)
        {
            int num = this.max_chain;
            int niceLength = this.niceLength;
            short[] prev = this.prev;
            int strstart = this.strstart;
            int index = this.strstart + this.matchLen;
            int num6 = Math.Max(this.matchLen, 2);
            int num7 = Math.Max(this.strstart - 0x7efa, 0);
            int num8 = (this.strstart + 0x102) - 1;
            byte num9 = this.window[index - 1];
            byte num10 = this.window[index];
            if (num6 >= this.goodLength)
            {
                num = num >> 2;
            }
            if (niceLength > this.lookahead)
            {
                niceLength = this.lookahead;
            }
            do
            {
                if ((((this.window[curMatch + num6] == num10) && (this.window[(curMatch + num6) - 1] == num9)) && (this.window[curMatch] == this.window[strstart])) && (this.window[curMatch + 1] == this.window[strstart + 1]))
                {
                    int num4 = curMatch + 2;
                    strstart += 2;
                    while (((((this.window[++strstart] == this.window[++num4]) && (this.window[++strstart] == this.window[++num4])) && ((this.window[++strstart] == this.window[++num4]) && (this.window[++strstart] == this.window[++num4]))) && (((this.window[++strstart] == this.window[++num4]) && (this.window[++strstart] == this.window[++num4])) && ((this.window[++strstart] == this.window[++num4]) && (this.window[++strstart] == this.window[++num4])))) && (strstart < num8))
                    {
                    }
                    if (strstart > index)
                    {
                        this.matchStart = curMatch;
                        index = strstart;
                        num6 = strstart - this.strstart;
                        if (num6 >= niceLength)
                        {
                            break;
                        }
                        num9 = this.window[index - 1];
                        num10 = this.window[index];
                    }
                    strstart = this.strstart;
                }
            }
            while (((curMatch = prev[curMatch & 0x7fff] & 0xffff) > num7) && (--num != 0));
            this.matchLen = Math.Min(num6, this.lookahead);
            return (this.matchLen >= 3);
        }

        private int InsertString()
        {
            short num;
            int index = ((this.ins_h << 5) ^ this.window[this.strstart + 2]) & 0x7fff;
            this.prev[this.strstart & 0x7fff] = num = this.head[index];
            this.head[index] = (short) this.strstart;
            this.ins_h = index;
            return (num & 0xffff);
        }

        public bool NeedsInput()
        {
            return (this.inputEnd == this.inputOff);
        }

        public void Reset()
        {
            int num;
            this.huffman.Reset();
            this.adler.Reset();
            this.blockStart = this.strstart = 1;
            this.lookahead = 0;
            this.totalIn = 0L;
            this.prevAvailable = false;
            this.matchLen = 2;
            for (num = 0; num < 0x8000; num++)
            {
                this.head[num] = 0;
            }
            for (num = 0; num < 0x8000; num++)
            {
                this.prev[num] = 0;
            }
        }

        public void ResetAdler()
        {
            this.adler.Reset();
        }

        public void SetDictionary(byte[] buffer, int offset, int length)
        {
            this.adler.Update(buffer, offset, length);
            if (length >= 3)
            {
                if (length > 0x7efa)
                {
                    offset += length - 0x7efa;
                    length = 0x7efa;
                }
                Array.Copy(buffer, offset, this.window, this.strstart, length);
                this.UpdateHash();
                length--;
                while (--length > 0)
                {
                    this.InsertString();
                    this.strstart++;
                }
                this.strstart += 2;
                this.blockStart = this.strstart;
            }
        }

        public void SetInput(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if (this.inputOff < this.inputEnd)
            {
                throw new InvalidOperationException("Old input was not completely processed");
            }
            int num = offset + count;
            if ((offset > num) || (num > buffer.Length))
            {
                throw new ArgumentOutOfRangeException("count");
            }
            this.inputBuf = buffer;
            this.inputOff = offset;
            this.inputEnd = num;
        }

        public void SetLevel(int level)
        {
            if ((level < 0) || (level > 9))
            {
                throw new ArgumentOutOfRangeException("level");
            }
            this.goodLength = DeflaterConstants.GOOD_LENGTH[level];
            this.max_lazy = DeflaterConstants.MAX_LAZY[level];
            this.niceLength = DeflaterConstants.NICE_LENGTH[level];
            this.max_chain = DeflaterConstants.MAX_CHAIN[level];
            if (DeflaterConstants.COMPR_FUNC[level] != this.compressionFunction)
            {
                switch (this.compressionFunction)
                {
                    case 0:
                        if (this.strstart > this.blockStart)
                        {
                            this.huffman.FlushStoredBlock(this.window, this.blockStart, this.strstart - this.blockStart, false);
                            this.blockStart = this.strstart;
                        }
                        this.UpdateHash();
                        break;

                    case 1:
                        if (this.strstart > this.blockStart)
                        {
                            this.huffman.FlushBlock(this.window, this.blockStart, this.strstart - this.blockStart, false);
                            this.blockStart = this.strstart;
                        }
                        break;

                    case 2:
                        if (this.prevAvailable)
                        {
                            this.huffman.TallyLit(this.window[this.strstart - 1] & 0xff);
                        }
                        if (this.strstart > this.blockStart)
                        {
                            this.huffman.FlushBlock(this.window, this.blockStart, this.strstart - this.blockStart, false);
                            this.blockStart = this.strstart;
                        }
                        this.prevAvailable = false;
                        this.matchLen = 2;
                        break;
                }
                this.compressionFunction = DeflaterConstants.COMPR_FUNC[level];
            }
        }

        private void SlideWindow()
        {
            int num;
            int num2;
            Array.Copy(this.window, 0x8000, this.window, 0, 0x8000);
            this.matchStart -= 0x8000;
            this.strstart -= 0x8000;
            this.blockStart -= 0x8000;
            for (num = 0; num < 0x8000; num++)
            {
                num2 = this.head[num] & 0xffff;
                this.head[num] = (num2 >= 0x8000) ? ((short) (num2 - 0x8000)) : ((short) 0);
            }
            for (num = 0; num < 0x8000; num++)
            {
                num2 = this.prev[num] & 0xffff;
                this.prev[num] = (num2 >= 0x8000) ? ((short) (num2 - 0x8000)) : ((short) 0);
            }
        }

        private void UpdateHash()
        {
            this.ins_h = (this.window[this.strstart] << 5) ^ this.window[this.strstart + 1];
        }

        public int Adler
        {
            get
            {
                return (int) this.adler.Value;
            }
        }

        public DeflateStrategy Strategy
        {
            get
            {
                return this.strategy;
            }
            set
            {
                this.strategy = value;
            }
        }

        public long TotalIn
        {
            get
            {
                return this.totalIn;
            }
        }
    }
}

