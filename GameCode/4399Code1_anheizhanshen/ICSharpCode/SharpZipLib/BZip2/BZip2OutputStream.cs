namespace ICSharpCode.SharpZipLib.BZip2
{
    using ICSharpCode.SharpZipLib.Checksums;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    public class BZip2OutputStream : Stream
    {
        private int allowableBlockSize;
        private Stream baseStream;
        private byte[] block;
        private uint blockCRC;
        private bool blockRandomised;
        private int blockSize100k;
        private int bsBuff;
        private int bsLive;
        private int bytesOut;
        private const int CLEARMASK = -2097153;
        private uint combinedCRC;
        private int currentChar;
        private const int DEPTH_THRESH = 10;
        private bool disposed_;
        private bool firstAttempt;
        private int[] ftab;
        private const int GREATER_ICOST = 15;
        private readonly int[] increments;
        private bool[] inUse;
        private bool isStreamOwner;
        private int last;
        private const int LESSER_ICOST = 0;
        private IChecksum mCrc;
        private int[] mtfFreq;
        private int nBlocksRandomised;
        private int nInUse;
        private int nMTF;
        private int origPtr;
        private const int QSORT_STACK_SIZE = 0x3e8;
        private int[] quadrant;
        private int runLength;
        private char[] selector;
        private char[] selectorMtf;
        private char[] seqToUnseq;
        private const int SETMASK = 0x200000;
        private const int SMALL_THRESH = 20;
        private short[] szptr;
        private char[] unseqToSeq;
        private int workDone;
        private int workFactor;
        private int workLimit;
        private int[] zptr;

        public BZip2OutputStream(Stream stream) : this(stream, 9)
        {
        }

        public BZip2OutputStream(Stream stream, int blockSize)
        {
            this.increments = new int[] { 1, 4, 13, 40, 0x79, 0x16c, 0x445, 0xcd0, 0x2671, 0x7354, 0x159fd, 0x40df8, 0xc29e9, 0x247dbc };
            this.isStreamOwner = true;
            this.mCrc = new StrangeCRC();
            this.inUse = new bool[0x100];
            this.seqToUnseq = new char[0x100];
            this.unseqToSeq = new char[0x100];
            this.selector = new char[0x4652];
            this.selectorMtf = new char[0x4652];
            this.mtfFreq = new int[0x102];
            this.currentChar = -1;
            this.BsSetStream(stream);
            this.workFactor = 50;
            if (blockSize > 9)
            {
                blockSize = 9;
            }
            if (blockSize < 1)
            {
                blockSize = 1;
            }
            this.blockSize100k = blockSize;
            this.AllocateCompressStructures();
            this.Initialize();
            this.InitBlock();
        }

        private void AllocateCompressStructures()
        {
            int num = 0x186a0 * this.blockSize100k;
            this.block = new byte[(num + 1) + 20];
            this.quadrant = new int[num + 20];
            this.zptr = new int[num];
            this.ftab = new int[0x10001];
            if ((((this.block == null) || (this.quadrant == null)) || (this.zptr == null)) || (this.ftab == null))
            {
            }
            this.szptr = new short[2 * num];
        }

        private void BsFinishedWithStream()
        {
            while (this.bsLive > 0)
            {
                int num = this.bsBuff >> 0x18;
                this.baseStream.WriteByte((byte) num);
                this.bsBuff = this.bsBuff << 8;
                this.bsLive -= 8;
                this.bytesOut++;
            }
        }

        private void BsPutint(int u)
        {
            this.BsW(8, (u >> 0x18) & 0xff);
            this.BsW(8, (u >> 0x10) & 0xff);
            this.BsW(8, (u >> 8) & 0xff);
            this.BsW(8, u & 0xff);
        }

        private void BsPutIntVS(int numBits, int c)
        {
            this.BsW(numBits, c);
        }

        private void BsPutUChar(int c)
        {
            this.BsW(8, c);
        }

        private void BsSetStream(Stream stream)
        {
            this.baseStream = stream;
            this.bsLive = 0;
            this.bsBuff = 0;
            this.bytesOut = 0;
        }

        private void BsW(int n, int v)
        {
            while (this.bsLive >= 8)
            {
                int num = this.bsBuff >> 0x18;
                this.baseStream.WriteByte((byte) num);
                this.bsBuff = this.bsBuff << 8;
                this.bsLive -= 8;
                this.bytesOut++;
            }
            this.bsBuff |= v << ((0x20 - this.bsLive) - n);
            this.bsLive += n;
        }

        public override void Close()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(disposing);
                if (!this.disposed_)
                {
                    this.disposed_ = true;
                    if (this.runLength > 0)
                    {
                        this.WriteRun();
                    }
                    this.currentChar = -1;
                    this.EndBlock();
                    this.EndCompression();
                    this.Flush();
                }
            }
            finally
            {
                if (disposing && this.IsStreamOwner)
                {
                    this.baseStream.Close();
                }
            }
        }

        private void DoReversibleTransformation()
        {
            this.workLimit = this.workFactor * this.last;
            this.workDone = 0;
            this.blockRandomised = false;
            this.firstAttempt = true;
            this.MainSort();
            if ((this.workDone > this.workLimit) && this.firstAttempt)
            {
                this.RandomiseBlock();
                this.workLimit = this.workDone = 0;
                this.blockRandomised = true;
                this.firstAttempt = false;
                this.MainSort();
            }
            this.origPtr = -1;
            for (int i = 0; i <= this.last; i++)
            {
                if (this.zptr[i] == 0)
                {
                    this.origPtr = i;
                    break;
                }
            }
            if (this.origPtr == -1)
            {
                Panic();
            }
        }

        private void EndBlock()
        {
            if (this.last >= 0)
            {
                this.blockCRC = (uint) this.mCrc.Value;
                this.combinedCRC = (this.combinedCRC << 1) | (this.combinedCRC >> 0x1f);
                this.combinedCRC ^= this.blockCRC;
                this.DoReversibleTransformation();
                this.BsPutUChar(0x31);
                this.BsPutUChar(0x41);
                this.BsPutUChar(0x59);
                this.BsPutUChar(0x26);
                this.BsPutUChar(0x53);
                this.BsPutUChar(0x59);
                this.BsPutint((int) this.blockCRC);
                if (this.blockRandomised)
                {
                    this.BsW(1, 1);
                    this.nBlocksRandomised++;
                }
                else
                {
                    this.BsW(1, 0);
                }
                this.MoveToFrontCodeAndSend();
            }
        }

        private void EndCompression()
        {
            this.BsPutUChar(0x17);
            this.BsPutUChar(0x72);
            this.BsPutUChar(0x45);
            this.BsPutUChar(0x38);
            this.BsPutUChar(80);
            this.BsPutUChar(0x90);
            this.BsPutint((int) this.combinedCRC);
            this.BsFinishedWithStream();
        }

        ~BZip2OutputStream()
        {
            this.Dispose(false);
        }

        public override void Flush()
        {
            this.baseStream.Flush();
        }

        private bool FullGtU(int i1, int i2)
        {
            byte num2 = this.block[i1 + 1];
            byte num3 = this.block[i2 + 1];
            if (num2 != num3)
            {
                return (num2 > num3);
            }
            i1++;
            i2++;
            num2 = this.block[i1 + 1];
            num3 = this.block[i2 + 1];
            if (num2 != num3)
            {
                return (num2 > num3);
            }
            i1++;
            i2++;
            num2 = this.block[i1 + 1];
            num3 = this.block[i2 + 1];
            if (num2 != num3)
            {
                return (num2 > num3);
            }
            i1++;
            i2++;
            num2 = this.block[i1 + 1];
            num3 = this.block[i2 + 1];
            if (num2 != num3)
            {
                return (num2 > num3);
            }
            i1++;
            i2++;
            num2 = this.block[i1 + 1];
            num3 = this.block[i2 + 1];
            if (num2 != num3)
            {
                return (num2 > num3);
            }
            i1++;
            i2++;
            num2 = this.block[i1 + 1];
            num3 = this.block[i2 + 1];
            if (num2 != num3)
            {
                return (num2 > num3);
            }
            i1++;
            i2++;
            int num = this.last + 1;
            do
            {
                num2 = this.block[i1 + 1];
                num3 = this.block[i2 + 1];
                if (num2 != num3)
                {
                    return (num2 > num3);
                }
                int num4 = this.quadrant[i1];
                int num5 = this.quadrant[i2];
                if (num4 != num5)
                {
                    return (num4 > num5);
                }
                i1++;
                i2++;
                num2 = this.block[i1 + 1];
                num3 = this.block[i2 + 1];
                if (num2 != num3)
                {
                    return (num2 > num3);
                }
                num4 = this.quadrant[i1];
                num5 = this.quadrant[i2];
                if (num4 != num5)
                {
                    return (num4 > num5);
                }
                i1++;
                i2++;
                num2 = this.block[i1 + 1];
                num3 = this.block[i2 + 1];
                if (num2 != num3)
                {
                    return (num2 > num3);
                }
                num4 = this.quadrant[i1];
                num5 = this.quadrant[i2];
                if (num4 != num5)
                {
                    return (num4 > num5);
                }
                i1++;
                i2++;
                num2 = this.block[i1 + 1];
                num3 = this.block[i2 + 1];
                if (num2 != num3)
                {
                    return (num2 > num3);
                }
                num4 = this.quadrant[i1];
                num5 = this.quadrant[i2];
                if (num4 != num5)
                {
                    return (num4 > num5);
                }
                i1++;
                i2++;
                if (i1 > this.last)
                {
                    i1 -= this.last;
                    i1--;
                }
                if (i2 > this.last)
                {
                    i2 -= this.last;
                    i2--;
                }
                num -= 4;
                this.workDone++;
            }
            while (num >= 0);
            return false;
        }

        private void GenerateMTFValues()
        {
            int num;
            bool flag;
            char[] chArray = new char[0x100];
            this.MakeMaps();
            int index = this.nInUse + 1;
            for (num = 0; num <= index; num++)
            {
                this.mtfFreq[num] = 0;
            }
            int num4 = 0;
            int num3 = 0;
            for (num = 0; num < this.nInUse; num++)
            {
                chArray[num] = (char) num;
            }
            for (num = 0; num <= this.last; num++)
            {
                char ch3 = this.unseqToSeq[this.block[this.zptr[num]]];
                int num2 = 0;
                char ch = chArray[num2];
                while (ch3 != ch)
                {
                    num2++;
                    char ch2 = ch;
                    ch = chArray[num2];
                    chArray[num2] = ch2;
                }
                chArray[0] = ch;
                if (num2 == 0)
                {
                    num3++;
                    continue;
                }
                if (num3 <= 0)
                {
                    goto Label_017A;
                }
                num3--;
                goto Label_016E;
            Label_00E6:
                switch ((num3 % 2))
                {
                    case 0:
                        this.szptr[num4] = 0;
                        num4++;
                        this.mtfFreq[0]++;
                        break;

                    case 1:
                        this.szptr[num4] = 1;
                        num4++;
                        this.mtfFreq[1]++;
                        break;
                }
                if (num3 < 2)
                {
                    goto Label_0176;
                }
                num3 = (num3 - 2) / 2;
            Label_016E:
                flag = true;
                goto Label_00E6;
            Label_0176:
                num3 = 0;
            Label_017A:
                this.szptr[num4] = (short) (num2 + 1);
                num4++;
                this.mtfFreq[num2 + 1]++;
            }
            if (num3 <= 0)
            {
                goto Label_0271;
            }
            num3--;
            goto Label_0268;
        Label_024E:
            if (num3 < 2)
            {
                goto Label_0271;
            }
            num3 = (num3 - 2) / 2;
        Label_0268:
            flag = true;
            switch ((num3 % 2))
            {
                case 0:
                    this.szptr[num4] = 0;
                    num4++;
                    this.mtfFreq[0]++;
                    goto Label_024E;

                case 1:
                    this.szptr[num4] = 1;
                    num4++;
                    this.mtfFreq[1]++;
                    goto Label_024E;

                default:
                    goto Label_024E;
            }
        Label_0271:
            this.szptr[num4] = (short) index;
            num4++;
            this.mtfFreq[index]++;
            this.nMTF = num4;
        }

        private static void HbAssignCodes(int[] code, char[] length, int minLen, int maxLen, int alphaSize)
        {
            int num = 0;
            for (int i = minLen; i <= maxLen; i++)
            {
                for (int j = 0; j < alphaSize; j++)
                {
                    if (length[j] == i)
                    {
                        code[j] = num;
                        num++;
                    }
                }
                num = num << 1;
            }
        }

        private static void HbMakeCodeLengths(char[] len, int[] freq, int alphaSize, int maxLen)
        {
            int num7;
            int[] numArray = new int[260];
            int[] numArray2 = new int[0x204];
            int[] numArray3 = new int[0x204];
            for (num7 = 0; num7 < alphaSize; num7++)
            {
                numArray2[num7 + 1] = ((freq[num7] == null) ? 1 : freq[num7]) << 8;
            }
            while (true)
            {
                int num5;
                int num8;
                int num9;
                bool flag2 = true;
                int index = alphaSize;
                int num2 = 0;
                numArray[0] = 0;
                numArray2[0] = 0;
                numArray3[0] = -2;
                for (num7 = 1; num7 <= alphaSize; num7++)
                {
                    numArray3[num7] = -1;
                    num2++;
                    numArray[num2] = num7;
                    num8 = num2;
                    num9 = numArray[num8];
                    while (numArray2[num9] < numArray2[numArray[num8 >> 1]])
                    {
                        numArray[num8] = numArray[num8 >> 1];
                        num8 = num8 >> 1;
                    }
                    numArray[num8] = num9;
                }
                if (num2 >= 260)
                {
                    Panic();
                }
                while (num2 > 1)
                {
                    int num3 = numArray[1];
                    numArray[1] = numArray[num2];
                    num2--;
                    num8 = 1;
                    int num10 = 0;
                    num9 = numArray[num8];
                    goto Label_0183;
                Label_0113:
                    num10 = num8 << 1;
                    if (num10 > num2)
                    {
                        goto Label_0188;
                    }
                    if ((num10 < num2) && (numArray2[numArray[num10 + 1]] < numArray2[numArray[num10]]))
                    {
                        num10++;
                    }
                    if (numArray2[num9] < numArray2[numArray[num10]])
                    {
                        goto Label_0188;
                    }
                    numArray[num8] = numArray[num10];
                    num8 = num10;
                Label_0183:
                    flag2 = true;
                    goto Label_0113;
                Label_0188:
                    numArray[num8] = num9;
                    int num4 = numArray[1];
                    numArray[1] = numArray[num2];
                    num2--;
                    num8 = 1;
                    num10 = 0;
                    num9 = numArray[num8];
                    goto Label_021F;
                Label_01AF:
                    num10 = num8 << 1;
                    if (num10 > num2)
                    {
                        goto Label_0224;
                    }
                    if ((num10 < num2) && (numArray2[numArray[num10 + 1]] < numArray2[numArray[num10]]))
                    {
                        num10++;
                    }
                    if (numArray2[num9] < numArray2[numArray[num10]])
                    {
                        goto Label_0224;
                    }
                    numArray[num8] = numArray[num10];
                    num8 = num10;
                Label_021F:
                    flag2 = true;
                    goto Label_01AF;
                Label_0224:
                    numArray[num8] = num9;
                    index++;
                    numArray3[num3] = numArray3[num4] = index;
                    numArray2[index] = ((int) ((numArray2[num3] & 0xffffff00L) + (numArray2[num4] & 0xffffff00L))) | (1 + (((numArray2[num3] & 0xff) > (numArray2[num4] & 0xff)) ? (numArray2[num3] & 0xff) : (numArray2[num4] & 0xff)));
                    numArray3[index] = -1;
                    num2++;
                    numArray[num2] = index;
                    num8 = num2;
                    num9 = numArray[num8];
                    while (numArray2[num9] < numArray2[numArray[num8 >> 1]])
                    {
                        numArray[num8] = numArray[num8 >> 1];
                        num8 = num8 >> 1;
                    }
                    numArray[num8] = num9;
                }
                if (index >= 0x204)
                {
                    Panic();
                }
                bool flag = false;
                for (num7 = 1; num7 <= alphaSize; num7++)
                {
                    num5 = 0;
                    int num6 = num7;
                    while (numArray3[num6] >= 0)
                    {
                        num6 = numArray3[num6];
                        num5++;
                    }
                    len[num7 - 1] = (char) num5;
                    if (num5 > maxLen)
                    {
                        flag = true;
                    }
                }
                if (!flag)
                {
                    return;
                }
                for (num7 = 1; num7 < alphaSize; num7++)
                {
                    num5 = numArray2[num7] >> 8;
                    num5 = 1 + (num5 / 2);
                    numArray2[num7] = num5 << 8;
                }
            }
        }

        private void InitBlock()
        {
            this.mCrc.Reset();
            this.last = -1;
            for (int i = 0; i < 0x100; i++)
            {
                this.inUse[i] = false;
            }
            this.allowableBlockSize = (0x186a0 * this.blockSize100k) - 20;
        }

        private void Initialize()
        {
            this.bytesOut = 0;
            this.nBlocksRandomised = 0;
            this.BsPutUChar(0x42);
            this.BsPutUChar(90);
            this.BsPutUChar(0x68);
            this.BsPutUChar(0x30 + this.blockSize100k);
            this.combinedCRC = 0;
        }

        private void MainSort()
        {
            int num;
            int[] numArray = new int[0x100];
            int[] numArray2 = new int[0x100];
            bool[] flagArray = new bool[0x100];
            for (num = 0; num < 20; num++)
            {
                this.block[(this.last + num) + 2] = this.block[(num % (this.last + 1)) + 1];
            }
            for (num = 0; num <= (this.last + 20); num++)
            {
                this.quadrant[num] = 0;
            }
            this.block[0] = this.block[this.last + 1];
            if (this.last < 0xfa0)
            {
                for (num = 0; num <= this.last; num++)
                {
                    this.zptr[num] = num;
                }
                this.firstAttempt = false;
                this.workDone = this.workLimit = 0;
                this.SimpleSort(0, this.last, 0);
            }
            else
            {
                int num2;
                int num6;
                int num7 = 0;
                for (num = 0; num <= 0xff; num++)
                {
                    flagArray[num] = false;
                }
                for (num = 0; num <= 0x10000; num++)
                {
                    this.ftab[num] = 0;
                }
                int index = this.block[0];
                for (num = 0; num <= this.last; num++)
                {
                    num6 = this.block[num + 1];
                    this.ftab[(index << 8) + num6]++;
                    index = num6;
                }
                for (num = 1; num <= 0x10000; num++)
                {
                    this.ftab[num] += this.ftab[num - 1];
                }
                index = this.block[1];
                for (num = 0; num < this.last; num++)
                {
                    num6 = this.block[num + 2];
                    num2 = (index << 8) + num6;
                    index = num6;
                    this.ftab[num2]--;
                    this.zptr[this.ftab[num2]] = num;
                }
                num2 = (this.block[this.last + 1] << 8) + this.block[1];
                this.ftab[num2]--;
                this.zptr[this.ftab[num2]] = this.last;
                num = 0;
                while (num <= 0xff)
                {
                    numArray[num] = num;
                    num++;
                }
                int num9 = 1;
                do
                {
                    num9 = (3 * num9) + 1;
                }
                while (num9 <= 0x100);
                do
                {
                    num9 /= 3;
                    num = num9;
                    while (num <= 0xff)
                    {
                        int num8 = numArray[num];
                        num2 = num;
                        while ((this.ftab[(numArray[num2 - num9] + 1) << 8] - this.ftab[numArray[num2 - num9] << 8]) > (this.ftab[(num8 + 1) << 8] - this.ftab[num8 << 8]))
                        {
                            numArray[num2] = numArray[num2 - num9];
                            num2 -= num9;
                            if (num2 <= (num9 - 1))
                            {
                                break;
                            }
                        }
                        numArray[num2] = num8;
                        num++;
                    }
                }
                while (num9 != 1);
                for (num = 0; num <= 0xff; num++)
                {
                    int num3 = numArray[num];
                    num2 = 0;
                    while (num2 <= 0xff)
                    {
                        int num4 = (num3 << 8) + num2;
                        if ((this.ftab[num4] & 0x200000) != 0x200000)
                        {
                            int loSt = this.ftab[num4] & -2097153;
                            int hiSt = (this.ftab[num4 + 1] & -2097153) - 1;
                            if (hiSt > loSt)
                            {
                                this.QSort3(loSt, hiSt, 2);
                                num7 += (hiSt - loSt) + 1;
                                if ((this.workDone > this.workLimit) && this.firstAttempt)
                                {
                                    break;
                                }
                            }
                            this.ftab[num4] |= 0x200000;
                        }
                        num2++;
                    }
                    flagArray[num3] = true;
                    if (num < 0xff)
                    {
                        int num12 = this.ftab[num3 << 8] & -2097153;
                        int num13 = (this.ftab[(num3 + 1) << 8] & -2097153) - num12;
                        int num14 = 0;
                        while ((num13 >> num14) > 0xfffe)
                        {
                            num14++;
                        }
                        num2 = 0;
                        while (num2 < num13)
                        {
                            int num15 = this.zptr[num12 + num2];
                            int num16 = num2 >> num14;
                            this.quadrant[num15] = num16;
                            if (num15 < 20)
                            {
                                this.quadrant[(num15 + this.last) + 1] = num16;
                            }
                            num2++;
                        }
                        if (((num13 - 1) >> num14) > 0xffff)
                        {
                            Panic();
                        }
                    }
                    num2 = 0;
                    while (num2 <= 0xff)
                    {
                        numArray2[num2] = this.ftab[(num2 << 8) + num3] & -2097153;
                        num2++;
                    }
                    num2 = this.ftab[num3 << 8] & -2097153;
                    while (num2 < (this.ftab[(num3 + 1) << 8] & -2097153))
                    {
                        index = this.block[this.zptr[num2]];
                        if (!flagArray[index])
                        {
                            this.zptr[numArray2[index]] = (this.zptr[num2] == 0) ? this.last : (this.zptr[num2] - 1);
                            numArray2[index]++;
                        }
                        num2++;
                    }
                    for (num2 = 0; num2 <= 0xff; num2++)
                    {
                        this.ftab[(num2 << 8) + num3] |= 0x200000;
                    }
                }
            }
        }

        private void MakeMaps()
        {
            this.nInUse = 0;
            for (int i = 0; i < 0x100; i++)
            {
                if (this.inUse[i])
                {
                    this.seqToUnseq[this.nInUse] = (char) i;
                    this.unseqToSeq[i] = (char) this.nInUse;
                    this.nInUse++;
                }
            }
        }

        private static byte Med3(byte a, byte b, byte c)
        {
            byte num;
            if (a > b)
            {
                num = a;
                a = b;
                b = num;
            }
            if (b > c)
            {
                num = b;
                b = c;
                c = num;
            }
            if (a > b)
            {
                b = a;
            }
            return b;
        }

        private void MoveToFrontCodeAndSend()
        {
            this.BsPutIntVS(0x18, this.origPtr);
            this.GenerateMTFValues();
            this.SendMTFValues();
        }

        private static void Panic()
        {
            throw new BZip2Exception("BZip2 output stream panic");
        }

        private void QSort3(int loSt, int hiSt, int dSt)
        {
            StackElement[] elementArray = new StackElement[0x3e8];
            int index = 0;
            elementArray[index].ll = loSt;
            elementArray[index].hh = hiSt;
            elementArray[index].dd = dSt;
            index++;
            while (index > 0)
            {
                int num3;
                int num4;
                int num12;
                bool flag;
                if (index >= 0x3e8)
                {
                    Panic();
                }
                index--;
                int ll = elementArray[index].ll;
                int hh = elementArray[index].hh;
                int dd = elementArray[index].dd;
                if (((hh - ll) < 20) || (dd > 10))
                {
                    this.SimpleSort(ll, hh, dd);
                    if ((this.workDone <= this.workLimit) || !this.firstAttempt)
                    {
                        continue;
                    }
                    break;
                }
                int num5 = Med3(this.block[(this.zptr[ll] + dd) + 1], this.block[(this.zptr[hh] + dd) + 1], this.block[(this.zptr[(ll + hh) >> 1] + dd) + 1]);
                int num = num3 = ll;
                int num2 = num4 = hh;
                goto Label_0294;
            Label_0149:
                if (num > num2)
                {
                    goto Label_024D;
                }
                int n = this.block[(this.zptr[num] + dd) + 1] - num5;
                if (n == 0)
                {
                    num12 = this.zptr[num];
                    this.zptr[num] = this.zptr[num3];
                    this.zptr[num3] = num12;
                    num3++;
                    num++;
                }
                else
                {
                    if (n > 0)
                    {
                        goto Label_024D;
                    }
                    num++;
                }
            Label_01C6:
                flag = true;
                goto Label_0149;
            Label_01D0:
                if (num > num2)
                {
                    goto Label_0255;
                }
                n = this.block[(this.zptr[num2] + dd) + 1] - num5;
                if (n == 0)
                {
                    num12 = this.zptr[num2];
                    this.zptr[num2] = this.zptr[num4];
                    this.zptr[num4] = num12;
                    num4--;
                    num2--;
                }
                else
                {
                    if (n < 0)
                    {
                        goto Label_0255;
                    }
                    num2--;
                }
            Label_024D:
                flag = true;
                goto Label_01D0;
            Label_0255:
                if (num > num2)
                {
                    goto Label_029C;
                }
                num12 = this.zptr[num];
                this.zptr[num] = this.zptr[num2];
                this.zptr[num2] = num12;
                num++;
                num2--;
            Label_0294:
                flag = true;
                goto Label_01C6;
            Label_029C:
                if (num4 < num3)
                {
                    elementArray[index].ll = ll;
                    elementArray[index].hh = hh;
                    elementArray[index].dd = dd + 1;
                    index++;
                }
                else
                {
                    n = ((num3 - ll) < (num - num3)) ? (num3 - ll) : (num - num3);
                    this.Vswap(ll, num - n, n);
                    int num7 = ((hh - num4) < (num4 - num2)) ? (hh - num4) : (num4 - num2);
                    this.Vswap(num, (hh - num7) + 1, num7);
                    n = ((ll + num) - num3) - 1;
                    num7 = (hh - (num4 - num2)) + 1;
                    elementArray[index].ll = ll;
                    elementArray[index].hh = n;
                    elementArray[index].dd = dd;
                    index++;
                    elementArray[index].ll = n + 1;
                    elementArray[index].hh = num7 - 1;
                    elementArray[index].dd = dd + 1;
                    index++;
                    elementArray[index].ll = num7;
                    elementArray[index].hh = hh;
                    elementArray[index].dd = dd;
                    index++;
                }
            }
        }

        private void RandomiseBlock()
        {
            int num;
            int num2 = 0;
            int index = 0;
            for (num = 0; num < 0x100; num++)
            {
                this.inUse[num] = false;
            }
            for (num = 0; num <= this.last; num++)
            {
                if (num2 == 0)
                {
                    num2 = BZip2Constants.RandomNumbers[index];
                    index++;
                    if (index == 0x200)
                    {
                        index = 0;
                    }
                }
                num2--;
                this.block[num + 1] = (byte) (this.block[num + 1] ^ ((num2 == 1) ? ((byte) 1) : ((byte) 0)));
                this.block[num + 1] = (byte) (this.block[num + 1] & 0xff);
                this.inUse[this.block[num + 1]] = true;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("BZip2OutputStream Read not supported");
        }

        public override int ReadByte()
        {
            throw new NotSupportedException("BZip2OutputStream ReadByte not supported");
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException("BZip2OutputStream Seek not supported");
        }

        private void SendMTFValues()
        {
            int num;
            int num3;
            int num13;
            int num15;
            int num27;
            bool flag;
            char[][] chArray = new char[6][];
            for (num = 0; num < 6; num++)
            {
                chArray[num] = new char[0x102];
            }
            int index = 0;
            int alphaSize = this.nInUse + 2;
            int num14 = 0;
            while (num14 < 6)
            {
                num15 = 0;
                while (num15 < alphaSize)
                {
                    chArray[num14][num15] = '\x000f';
                    num15++;
                }
                num14++;
            }
            if (this.nMTF <= 0)
            {
                Panic();
            }
            if (this.nMTF < 200)
            {
                num13 = 2;
            }
            else if (this.nMTF < 600)
            {
                num13 = 3;
            }
            else if (this.nMTF < 0x4b0)
            {
                num13 = 4;
            }
            else if (this.nMTF < 0x960)
            {
                num13 = 5;
            }
            else
            {
                num13 = 6;
            }
            int num16 = num13;
            int nMTF = this.nMTF;
            int num2 = 0;
            while (num16 > 0)
            {
                int num18 = nMTF / num16;
                int num19 = 0;
                num3 = num2 - 1;
                while ((num19 < num18) && (num3 < (alphaSize - 1)))
                {
                    num3++;
                    num19 += this.mtfFreq[num3];
                }
                if ((((num3 > num2) && (num16 != num13)) && (num16 != 1)) && (((num13 - num16) % 2) == 1))
                {
                    num19 -= this.mtfFreq[num3];
                    num3--;
                }
                num15 = 0;
                while (num15 < alphaSize)
                {
                    if ((num15 >= num2) && (num15 <= num3))
                    {
                        chArray[num16 - 1][num15] = '\0';
                    }
                    else
                    {
                        chArray[num16 - 1][num15] = '\x000f';
                    }
                    num15++;
                }
                num16--;
                num2 = num3 + 1;
                nMTF -= num19;
            }
            int[][] numArray = new int[6][];
            num = 0;
            while (num < 6)
            {
                numArray[num] = new int[0x102];
                num++;
            }
            int[] numArray2 = new int[6];
            short[] numArray3 = new short[6];
            for (int i = 0; i < 4; i++)
            {
                short num26;
                num14 = 0;
                while (num14 < num13)
                {
                    numArray2[num14] = 0;
                    num14++;
                }
                num14 = 0;
                while (num14 < num13)
                {
                    for (num15 = 0; num15 < alphaSize; num15++)
                    {
                        numArray[num14][num15] = 0;
                    }
                    num14++;
                }
                index = 0;
                int num4 = 0;
                num2 = 0;
                goto Label_04AF;
            Label_0299:
                if (num2 >= this.nMTF)
                {
                    goto Label_04B7;
                }
                num3 = (num2 + 50) - 1;
                if (num3 >= this.nMTF)
                {
                    num3 = this.nMTF - 1;
                }
                num14 = 0;
                while (num14 < num13)
                {
                    numArray3[num14] = 0;
                    num14++;
                }
                if (num13 == 6)
                {
                    short num21;
                    short num22;
                    short num23;
                    short num24;
                    short num25;
                    short num20 = num21 = num22 = num23 = num24 = (short) (num25 = 0);
                    num = num2;
                    while (num <= num3)
                    {
                        num26 = this.szptr[num];
                        num20 = (short) (num20 + ((short) chArray[0][num26]));
                        num21 = (short) (num21 + ((short) chArray[1][num26]));
                        num22 = (short) (num22 + ((short) chArray[2][num26]));
                        num23 = (short) (num23 + ((short) chArray[3][num26]));
                        num24 = (short) (num24 + ((short) chArray[4][num26]));
                        num25 = (short) (num25 + ((short) chArray[5][num26]));
                        num++;
                    }
                    numArray3[0] = num20;
                    numArray3[1] = num21;
                    numArray3[2] = num22;
                    numArray3[3] = num23;
                    numArray3[4] = num24;
                    numArray3[5] = num25;
                }
                else
                {
                    num = num2;
                    while (num <= num3)
                    {
                        num26 = this.szptr[num];
                        num14 = 0;
                        while (num14 < num13)
                        {
                            numArray3[num14] = (short) (numArray3[num14] + ((short) chArray[num14][num26]));
                            num14++;
                        }
                        num++;
                    }
                }
                int num6 = 0x3b9ac9ff;
                int num5 = -1;
                num14 = 0;
                while (num14 < num13)
                {
                    if (numArray3[num14] < num6)
                    {
                        num6 = numArray3[num14];
                        num5 = num14;
                    }
                    num14++;
                }
                num4 += num6;
                numArray2[num5]++;
                this.selector[index] = (char) num5;
                index++;
                num = num2;
                while (num <= num3)
                {
                    numArray[num5][this.szptr[num]]++;
                    num++;
                }
                num2 = num3 + 1;
            Label_04AF:
                flag = true;
                goto Label_0299;
            Label_04B7:
                num14 = 0;
                while (num14 < num13)
                {
                    HbMakeCodeLengths(chArray[num14], numArray[num14], alphaSize, 20);
                    num14++;
                }
            }
            numArray = null;
            numArray2 = null;
            numArray3 = null;
            if (num13 >= 8)
            {
                Panic();
            }
            if ((index >= 0x8000) || (index > 0x4652))
            {
                Panic();
            }
            char[] chArray2 = new char[6];
            for (num = 0; num < num13; num++)
            {
                chArray2[num] = (char) num;
            }
            for (num = 0; num < index; num++)
            {
                char ch = this.selector[num];
                num27 = 0;
                char ch3 = chArray2[num27];
                while (ch != ch3)
                {
                    num27++;
                    char ch2 = ch3;
                    ch3 = chArray2[num27];
                    chArray2[num27] = ch2;
                }
                chArray2[0] = ch3;
                this.selectorMtf[num] = (char) num27;
            }
            int[][] numArray4 = new int[6][];
            num = 0;
            while (num < 6)
            {
                numArray4[num] = new int[0x102];
                num++;
            }
            for (num14 = 0; num14 < num13; num14++)
            {
                int minLen = 0x20;
                int maxLen = 0;
                num = 0;
                while (num < alphaSize)
                {
                    if (chArray[num14][num] > maxLen)
                    {
                        maxLen = chArray[num14][num];
                    }
                    if (chArray[num14][num] < minLen)
                    {
                        minLen = chArray[num14][num];
                    }
                    num++;
                }
                if (maxLen > 20)
                {
                    Panic();
                }
                if (minLen < 1)
                {
                    Panic();
                }
                HbAssignCodes(numArray4[num14], chArray[num14], minLen, maxLen, alphaSize);
            }
            bool[] flagArray = new bool[0x10];
            for (num = 0; num < 0x10; num++)
            {
                flagArray[num] = false;
                num27 = 0;
                while (num27 < 0x10)
                {
                    if (this.inUse[(num * 0x10) + num27])
                    {
                        flagArray[num] = true;
                    }
                    num27++;
                }
            }
            for (num = 0; num < 0x10; num++)
            {
                if (flagArray[num])
                {
                    this.BsW(1, 1);
                }
                else
                {
                    this.BsW(1, 0);
                }
            }
            for (num = 0; num < 0x10; num++)
            {
                if (flagArray[num])
                {
                    num27 = 0;
                    while (num27 < 0x10)
                    {
                        if (this.inUse[(num * 0x10) + num27])
                        {
                            this.BsW(1, 1);
                        }
                        else
                        {
                            this.BsW(1, 0);
                        }
                        num27++;
                    }
                }
            }
            this.BsW(3, num13);
            this.BsW(15, index);
            num = 0;
            while (num < index)
            {
                for (num27 = 0; num27 < this.selectorMtf[num]; num27++)
                {
                    this.BsW(1, 1);
                }
                this.BsW(1, 0);
                num++;
            }
            for (num14 = 0; num14 < num13; num14++)
            {
                int v = chArray[num14][0];
                this.BsW(5, v);
                num = 0;
                while (num < alphaSize)
                {
                    while (v < chArray[num14][num])
                    {
                        this.BsW(2, 2);
                        v++;
                    }
                    while (v > chArray[num14][num])
                    {
                        this.BsW(2, 3);
                        v--;
                    }
                    this.BsW(1, 0);
                    num++;
                }
            }
            int num12 = 0;
            num2 = 0;
            while (true)
            {
                flag = true;
                if (num2 >= this.nMTF)
                {
                    if (num12 != index)
                    {
                        Panic();
                    }
                    return;
                }
                num3 = (num2 + 50) - 1;
                if (num3 >= this.nMTF)
                {
                    num3 = this.nMTF - 1;
                }
                for (num = num2; num <= num3; num++)
                {
                    this.BsW(chArray[this.selector[num12]][this.szptr[num]], numArray4[this.selector[num12]][this.szptr[num]]);
                }
                num2 = num3 + 1;
                num12++;
            }
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("BZip2OutputStream SetLength not supported");
        }

        private void SimpleSort(int lo, int hi, int d)
        {
            int num4 = (hi - lo) + 1;
            if (num4 >= 2)
            {
                int index = 0;
                while (this.increments[index] < num4)
                {
                    index++;
                }
                index--;
                while (index >= 0)
                {
                    bool flag;
                    int num3 = this.increments[index];
                    int num = lo + num3;
                    goto Label_01D8;
                Label_0058:
                    if (num > hi)
                    {
                        goto Label_01E0;
                    }
                    int num6 = this.zptr[num];
                    int num2 = num;
                    while (this.FullGtU(this.zptr[num2 - num3] + d, num6 + d))
                    {
                        this.zptr[num2] = this.zptr[num2 - num3];
                        num2 -= num3;
                        if (num2 <= ((lo + num3) - 1))
                        {
                            break;
                        }
                    }
                    this.zptr[num2] = num6;
                    num++;
                    if (num > hi)
                    {
                        goto Label_01E0;
                    }
                    num6 = this.zptr[num];
                    num2 = num;
                    while (this.FullGtU(this.zptr[num2 - num3] + d, num6 + d))
                    {
                        this.zptr[num2] = this.zptr[num2 - num3];
                        num2 -= num3;
                        if (num2 <= ((lo + num3) - 1))
                        {
                            break;
                        }
                    }
                    this.zptr[num2] = num6;
                    num++;
                    if (num > hi)
                    {
                        goto Label_01E0;
                    }
                    num6 = this.zptr[num];
                    num2 = num;
                    while (this.FullGtU(this.zptr[num2 - num3] + d, num6 + d))
                    {
                        this.zptr[num2] = this.zptr[num2 - num3];
                        num2 -= num3;
                        if (num2 <= ((lo + num3) - 1))
                        {
                            break;
                        }
                    }
                    this.zptr[num2] = num6;
                    num++;
                    if ((this.workDone > this.workLimit) && this.firstAttempt)
                    {
                        break;
                    }
                Label_01D8:
                    flag = true;
                    goto Label_0058;
                Label_01E0:
                    index--;
                }
            }
        }

        private void Vswap(int p1, int p2, int n)
        {
            int num = 0;
            while (n > 0)
            {
                num = this.zptr[p1];
                this.zptr[p1] = this.zptr[p2];
                this.zptr[p2] = num;
                p1++;
                p2++;
                n--;
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
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
            if ((buffer.Length - offset) < count)
            {
                throw new ArgumentException("Offset/count out of range");
            }
            for (int i = 0; i < count; i++)
            {
                this.WriteByte(buffer[offset + i]);
            }
        }

        public override void WriteByte(byte value)
        {
            int num = (0x100 + value) % 0x100;
            if (this.currentChar != -1)
            {
                if (this.currentChar == num)
                {
                    this.runLength++;
                    if (this.runLength > 0xfe)
                    {
                        this.WriteRun();
                        this.currentChar = -1;
                        this.runLength = 0;
                    }
                }
                else
                {
                    this.WriteRun();
                    this.runLength = 1;
                    this.currentChar = num;
                }
            }
            else
            {
                this.currentChar = num;
                this.runLength++;
            }
        }

        private void WriteRun()
        {
            if (this.last < this.allowableBlockSize)
            {
                this.inUse[this.currentChar] = true;
                for (int i = 0; i < this.runLength; i++)
                {
                    this.mCrc.Update(this.currentChar);
                }
                switch (this.runLength)
                {
                    case 1:
                        this.last++;
                        this.block[this.last + 1] = (byte) this.currentChar;
                        return;

                    case 2:
                        this.last++;
                        this.block[this.last + 1] = (byte) this.currentChar;
                        this.last++;
                        this.block[this.last + 1] = (byte) this.currentChar;
                        return;

                    case 3:
                        this.last++;
                        this.block[this.last + 1] = (byte) this.currentChar;
                        this.last++;
                        this.block[this.last + 1] = (byte) this.currentChar;
                        this.last++;
                        this.block[this.last + 1] = (byte) this.currentChar;
                        return;
                }
                this.inUse[this.runLength - 4] = true;
                this.last++;
                this.block[this.last + 1] = (byte) this.currentChar;
                this.last++;
                this.block[this.last + 1] = (byte) this.currentChar;
                this.last++;
                this.block[this.last + 1] = (byte) this.currentChar;
                this.last++;
                this.block[this.last + 1] = (byte) this.currentChar;
                this.last++;
                this.block[this.last + 1] = (byte) (this.runLength - 4);
            }
            else
            {
                this.EndBlock();
                this.InitBlock();
                this.WriteRun();
            }
        }

        public int BytesWritten
        {
            get
            {
                return this.bytesOut;
            }
        }

        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return this.baseStream.CanWrite;
            }
        }

        public bool IsStreamOwner
        {
            get
            {
                return this.isStreamOwner;
            }
            set
            {
                this.isStreamOwner = value;
            }
        }

        public override long Length
        {
            get
            {
                return this.baseStream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return this.baseStream.Position;
            }
            set
            {
                throw new NotSupportedException("BZip2OutputStream position cannot be set");
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct StackElement
        {
            public int ll;
            public int hh;
            public int dd;
        }
    }
}

