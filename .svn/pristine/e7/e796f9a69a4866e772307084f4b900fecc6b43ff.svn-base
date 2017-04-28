namespace ICSharpCode.SharpZipLib.BZip2
{
    using ICSharpCode.SharpZipLib.Checksums;
    using System;
    using System.IO;

    public class BZip2InputStream : Stream
    {
        private int[][] baseArray = new int[6][];
        private Stream baseStream;
        private bool blockRandomised;
        private int blockSize100k;
        private int bsBuff;
        private int bsLive;
        private int ch2;
        private int chPrev;
        private int computedBlockCRC;
        private uint computedCombinedCRC;
        private int count;
        private int currentChar = -1;
        private int currentState = 1;
        private int i2;
        private bool[] inUse = new bool[0x100];
        private bool isStreamOwner = true;
        private int j2;
        private int last;
        private int[][] limit = new int[6][];
        private byte[] ll8;
        private IChecksum mCrc = new StrangeCRC();
        private int[] minLens = new int[6];
        private int nInUse;
        private const int NO_RAND_PART_A_STATE = 5;
        private const int NO_RAND_PART_B_STATE = 6;
        private const int NO_RAND_PART_C_STATE = 7;
        private int origPtr;
        private int[][] perm = new int[6][];
        private const int RAND_PART_A_STATE = 2;
        private const int RAND_PART_B_STATE = 3;
        private const int RAND_PART_C_STATE = 4;
        private int rNToGo;
        private int rTPos;
        private byte[] selector = new byte[0x4652];
        private byte[] selectorMtf = new byte[0x4652];
        private byte[] seqToUnseq = new byte[0x100];
        private const int START_BLOCK_STATE = 1;
        private int storedBlockCRC;
        private int storedCombinedCRC;
        private bool streamEnd;
        private int tPos;
        private int[] tt;
        private byte[] unseqToSeq = new byte[0x100];
        private int[] unzftab = new int[0x100];
        private byte z;

        public BZip2InputStream(Stream stream)
        {
            for (int i = 0; i < 6; i++)
            {
                this.limit[i] = new int[0x102];
                this.baseArray[i] = new int[0x102];
                this.perm[i] = new int[0x102];
            }
            this.BsSetStream(stream);
            this.Initialize();
            this.InitBlock();
            this.SetupBlock();
        }

        private static void BadBlockHeader()
        {
            throw new BZip2Exception("BZip2 input stream bad block header");
        }

        private static void BlockOverrun()
        {
            throw new BZip2Exception("BZip2 input stream block overrun");
        }

        private int BsGetInt32()
        {
            int num = (this.BsR(8) << 8) | this.BsR(8);
            num = (num << 8) | this.BsR(8);
            return ((num << 8) | this.BsR(8));
        }

        private int BsGetIntVS(int numBits)
        {
            return this.BsR(numBits);
        }

        private char BsGetUChar()
        {
            return (char) this.BsR(8);
        }

        private int BsR(int n)
        {
            while (this.bsLive < n)
            {
                this.FillBuffer();
            }
            int num = (this.bsBuff >> (this.bsLive - n)) & ((((int) 1) << n) - 1);
            this.bsLive -= n;
            return num;
        }

        private void BsSetStream(Stream stream)
        {
            this.baseStream = stream;
            this.bsLive = 0;
            this.bsBuff = 0;
        }

        public override void Close()
        {
            if (this.IsStreamOwner && (this.baseStream != null))
            {
                this.baseStream.Close();
            }
        }

        private void Complete()
        {
            this.storedCombinedCRC = this.BsGetInt32();
            if (this.storedCombinedCRC != this.computedCombinedCRC)
            {
                CrcError();
            }
            this.streamEnd = true;
        }

        private static void CompressedStreamEOF()
        {
            throw new EndOfStreamException("BZip2 input stream end of compressed stream");
        }

        private static void CrcError()
        {
            throw new BZip2Exception("BZip2 input stream crc error");
        }

        private void EndBlock()
        {
            this.computedBlockCRC = (int) this.mCrc.Value;
            if (this.storedBlockCRC != this.computedBlockCRC)
            {
                CrcError();
            }
            this.computedCombinedCRC = ((this.computedCombinedCRC << 1) & uint.MaxValue) | (this.computedCombinedCRC >> 0x1f);
            this.computedCombinedCRC ^= (uint) this.computedBlockCRC;
        }

        private void FillBuffer()
        {
            int num = 0;
            try
            {
                num = this.baseStream.ReadByte();
            }
            catch (Exception)
            {
                CompressedStreamEOF();
            }
            if (num == -1)
            {
                CompressedStreamEOF();
            }
            this.bsBuff = (this.bsBuff << 8) | (num & 0xff);
            this.bsLive += 8;
        }

        public override void Flush()
        {
            if (this.baseStream != null)
            {
                this.baseStream.Flush();
            }
        }

        private void GetAndMoveToFrontDecode()
        {
            int num6;
            int num10;
            bool flag;
            byte[] buffer = new byte[0x100];
            int num2 = 0x186a0 * this.blockSize100k;
            this.origPtr = this.BsGetIntVS(0x18);
            this.RecvDecodingTables();
            int num3 = this.nInUse + 1;
            int index = -1;
            int num5 = 0;
            for (num6 = 0; num6 <= 0xff; num6++)
            {
                this.unzftab[num6] = 0;
            }
            for (num6 = 0; num6 <= 0xff; num6++)
            {
                buffer[num6] = (byte) num6;
            }
            this.last = -1;
            if (num5 == 0)
            {
                index++;
                num5 = 50;
            }
            num5--;
            int num7 = this.selector[index];
            int n = this.minLens[num7];
            int num9 = this.BsR(n);
            while (num9 > this.limit[num7][n])
            {
                if (n > 20)
                {
                    throw new BZip2Exception("Bzip data error");
                }
                n++;
                while (this.bsLive < 1)
                {
                    this.FillBuffer();
                }
                num10 = (this.bsBuff >> (this.bsLive - 1)) & 1;
                this.bsLive--;
                num9 = (num9 << 1) | num10;
            }
            if (((num9 - this.baseArray[num7][n]) < 0) || ((num9 - this.baseArray[num7][n]) >= 0x102))
            {
                throw new BZip2Exception("Bzip data error");
            }
            int num = this.perm[num7][num9 - this.baseArray[num7][n]];
        Label_04BC:
            flag = true;
            if (num == num3)
            {
                return;
            }
            switch (num)
            {
                case 0:
                case 1:
                {
                    int num11 = -1;
                    int num12 = 1;
                    do
                    {
                        switch (num)
                        {
                            case 0:
                                num11 += num12;
                                break;

                            case 1:
                                num11 += 2 * num12;
                                break;
                        }
                        num12 = num12 << 1;
                        if (num5 == 0)
                        {
                            index++;
                            num5 = 50;
                        }
                        num5--;
                        num7 = this.selector[index];
                        n = this.minLens[num7];
                        num9 = this.BsR(n);
                        while (num9 > this.limit[num7][n])
                        {
                            n++;
                            while (this.bsLive < 1)
                            {
                                this.FillBuffer();
                            }
                            num10 = (this.bsBuff >> (this.bsLive - 1)) & 1;
                            this.bsLive--;
                            num9 = (num9 << 1) | num10;
                        }
                        num = this.perm[num7][num9 - this.baseArray[num7][n]];
                    }
                    while ((num == 0) || (num == 1));
                    num11++;
                    byte num13 = this.seqToUnseq[buffer[0]];
                    this.unzftab[num13] += num11;
                    while (num11 > 0)
                    {
                        this.last++;
                        this.ll8[this.last] = num13;
                        num11--;
                    }
                    if (this.last >= num2)
                    {
                        BlockOverrun();
                    }
                    goto Label_04BC;
                }
            }
            this.last++;
            if (this.last >= num2)
            {
                BlockOverrun();
            }
            byte num14 = buffer[num - 1];
            this.unzftab[this.seqToUnseq[num14]]++;
            this.ll8[this.last] = this.seqToUnseq[num14];
            for (int i = num - 1; i > 0; i--)
            {
                buffer[i] = buffer[i - 1];
            }
            buffer[0] = num14;
            if (num5 == 0)
            {
                index++;
                num5 = 50;
            }
            num5--;
            num7 = this.selector[index];
            n = this.minLens[num7];
            num9 = this.BsR(n);
            while (num9 > this.limit[num7][n])
            {
                n++;
                while (this.bsLive < 1)
                {
                    this.FillBuffer();
                }
                num10 = (this.bsBuff >> (this.bsLive - 1)) & 1;
                this.bsLive--;
                num9 = (num9 << 1) | num10;
            }
            num = this.perm[num7][num9 - this.baseArray[num7][n]];
            goto Label_04BC;
        }

        private static void HbCreateDecodeTables(int[] limit, int[] baseArray, int[] perm, char[] length, int minLen, int maxLen, int alphaSize)
        {
            int num2;
            int index = 0;
            for (num2 = minLen; num2 <= maxLen; num2++)
            {
                for (int i = 0; i < alphaSize; i++)
                {
                    if (length[i] == num2)
                    {
                        perm[index] = i;
                        index++;
                    }
                }
            }
            for (num2 = 0; num2 < 0x17; num2++)
            {
                baseArray[num2] = 0;
            }
            for (num2 = 0; num2 < alphaSize; num2++)
            {
                baseArray[length[num2] + '\x0001']++;
            }
            for (num2 = 1; num2 < 0x17; num2++)
            {
                baseArray[num2] += baseArray[num2 - 1];
            }
            for (num2 = 0; num2 < 0x17; num2++)
            {
                limit[num2] = 0;
            }
            int num4 = 0;
            for (num2 = minLen; num2 <= maxLen; num2++)
            {
                num4 += baseArray[num2 + 1] - baseArray[num2];
                limit[num2] = num4 - 1;
                num4 = num4 << 1;
            }
            for (num2 = minLen + 1; num2 <= maxLen; num2++)
            {
                baseArray[num2] = ((limit[num2 - 1] + 1) << 1) - baseArray[num2];
            }
        }

        private void InitBlock()
        {
            char ch = this.BsGetUChar();
            char ch2 = this.BsGetUChar();
            char ch3 = this.BsGetUChar();
            char ch4 = this.BsGetUChar();
            char ch5 = this.BsGetUChar();
            char ch6 = this.BsGetUChar();
            if (((((ch == '\x0017') && (ch2 == 'r')) && ((ch3 == 'E') && (ch4 == '8'))) && (ch5 == 'P')) && (ch6 == '\x0090'))
            {
                this.Complete();
            }
            else if (((((ch != '1') || (ch2 != 'A')) || ((ch3 != 'Y') || (ch4 != '&'))) || (ch5 != 'S')) || (ch6 != 'Y'))
            {
                BadBlockHeader();
                this.streamEnd = true;
            }
            else
            {
                this.storedBlockCRC = this.BsGetInt32();
                this.blockRandomised = this.BsR(1) == 1;
                this.GetAndMoveToFrontDecode();
                this.mCrc.Reset();
                this.currentState = 1;
            }
        }

        private void Initialize()
        {
            char ch = this.BsGetUChar();
            char ch2 = this.BsGetUChar();
            char ch3 = this.BsGetUChar();
            char ch4 = this.BsGetUChar();
            if ((((ch != 'B') || (ch2 != 'Z')) || ((ch3 != 'h') || (ch4 < '1'))) || (ch4 > '9'))
            {
                this.streamEnd = true;
            }
            else
            {
                this.SetDecompressStructureSizes(ch4 - '0');
                this.computedCombinedCRC = 0;
            }
        }

        private void MakeMaps()
        {
            this.nInUse = 0;
            for (int i = 0; i < 0x100; i++)
            {
                if (this.inUse[i])
                {
                    this.seqToUnseq[this.nInUse] = (byte) i;
                    this.unseqToSeq[i] = (byte) this.nInUse;
                    this.nInUse++;
                }
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            for (int i = 0; i < count; i++)
            {
                int num2 = this.ReadByte();
                if (num2 == -1)
                {
                    return i;
                }
                buffer[offset + i] = (byte) num2;
            }
            return count;
        }

        public override int ReadByte()
        {
            if (this.streamEnd)
            {
                return -1;
            }
            int currentChar = this.currentChar;
            switch (this.currentState)
            {
                case 1:
                case 2:
                case 5:
                    return currentChar;

                case 3:
                    this.SetupRandPartB();
                    return currentChar;

                case 4:
                    this.SetupRandPartC();
                    return currentChar;

                case 6:
                    this.SetupNoRandPartB();
                    return currentChar;

                case 7:
                    this.SetupNoRandPartC();
                    return currentChar;
            }
            return currentChar;
        }

        private void RecvDecodingTables()
        {
            int num;
            int num2;
            int num8;
            char[][] chArray = new char[6][];
            for (num = 0; num < 6; num++)
            {
                chArray[num] = new char[0x102];
            }
            bool[] flagArray = new bool[0x10];
            for (num = 0; num < 0x10; num++)
            {
                flagArray[num] = this.BsR(1) == 1;
            }
            for (num = 0; num < 0x10; num++)
            {
                if (flagArray[num])
                {
                    num2 = 0;
                    while (num2 < 0x10)
                    {
                        this.inUse[(num * 0x10) + num2] = this.BsR(1) == 1;
                        num2++;
                    }
                }
                else
                {
                    num2 = 0;
                    while (num2 < 0x10)
                    {
                        this.inUse[(num * 0x10) + num2] = false;
                        num2++;
                    }
                }
            }
            this.MakeMaps();
            int alphaSize = this.nInUse + 2;
            int num4 = this.BsR(3);
            int num5 = this.BsR(15);
            for (num = 0; num < num5; num++)
            {
                num2 = 0;
                while (this.BsR(1) == 1)
                {
                    num2++;
                }
                this.selectorMtf[num] = (byte) num2;
            }
            byte[] buffer = new byte[6];
            int index = 0;
            while (index < num4)
            {
                buffer[index] = (byte) index;
                index++;
            }
            num = 0;
            while (num < num5)
            {
                index = this.selectorMtf[num];
                byte num7 = buffer[index];
                while (index > 0)
                {
                    buffer[index] = buffer[index - 1];
                    index--;
                }
                buffer[0] = num7;
                this.selector[num] = num7;
                num++;
            }
            for (num8 = 0; num8 < num4; num8++)
            {
                int num9 = this.BsR(5);
                num = 0;
                while (num < alphaSize)
                {
                    while (this.BsR(1) == 1)
                    {
                        if (this.BsR(1) == 0)
                        {
                            num9++;
                        }
                        else
                        {
                            num9--;
                        }
                    }
                    chArray[num8][num] = (char) num9;
                    num++;
                }
            }
            for (num8 = 0; num8 < num4; num8++)
            {
                int num10 = 0x20;
                int num11 = 0;
                for (num = 0; num < alphaSize; num++)
                {
                    num11 = Math.Max(num11, chArray[num8][num]);
                    num10 = Math.Min(num10, chArray[num8][num]);
                }
                HbCreateDecodeTables(this.limit[num8], this.baseArray[num8], this.perm[num8], chArray[num8], num10, num11, alphaSize);
                this.minLens[num8] = num10;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException("BZip2InputStream Seek not supported");
        }

        private void SetDecompressStructureSizes(int newSize100k)
        {
            if ((((0 > newSize100k) || (newSize100k > 9)) || (0 > this.blockSize100k)) || (this.blockSize100k > 9))
            {
                throw new BZip2Exception("Invalid block size");
            }
            this.blockSize100k = newSize100k;
            if (newSize100k != 0)
            {
                int num = 0x186a0 * newSize100k;
                this.ll8 = new byte[num];
                this.tt = new int[num];
            }
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("BZip2InputStream SetLength not supported");
        }

        private void SetupBlock()
        {
            int num;
            int[] destinationArray = new int[0x101];
            destinationArray[0] = 0;
            Array.Copy(this.unzftab, 0, destinationArray, 1, 0x100);
            for (num = 1; num <= 0x100; num++)
            {
                destinationArray[num] += destinationArray[num - 1];
            }
            for (num = 0; num <= this.last; num++)
            {
                byte index = this.ll8[num];
                this.tt[destinationArray[index]] = num;
                destinationArray[index]++;
            }
            destinationArray = null;
            this.tPos = this.tt[this.origPtr];
            this.count = 0;
            this.i2 = 0;
            this.ch2 = 0x100;
            if (this.blockRandomised)
            {
                this.rNToGo = 0;
                this.rTPos = 0;
                this.SetupRandPartA();
            }
            else
            {
                this.SetupNoRandPartA();
            }
        }

        private void SetupNoRandPartA()
        {
            if (this.i2 <= this.last)
            {
                this.chPrev = this.ch2;
                this.ch2 = this.ll8[this.tPos];
                this.tPos = this.tt[this.tPos];
                this.i2++;
                this.currentChar = this.ch2;
                this.currentState = 6;
                this.mCrc.Update(this.ch2);
            }
            else
            {
                this.EndBlock();
                this.InitBlock();
                this.SetupBlock();
            }
        }

        private void SetupNoRandPartB()
        {
            if (this.ch2 != this.chPrev)
            {
                this.currentState = 5;
                this.count = 1;
                this.SetupNoRandPartA();
            }
            else
            {
                this.count++;
                if (this.count >= 4)
                {
                    this.z = this.ll8[this.tPos];
                    this.tPos = this.tt[this.tPos];
                    this.currentState = 7;
                    this.j2 = 0;
                    this.SetupNoRandPartC();
                }
                else
                {
                    this.currentState = 5;
                    this.SetupNoRandPartA();
                }
            }
        }

        private void SetupNoRandPartC()
        {
            if (this.j2 < this.z)
            {
                this.currentChar = this.ch2;
                this.mCrc.Update(this.ch2);
                this.j2++;
            }
            else
            {
                this.currentState = 5;
                this.i2++;
                this.count = 0;
                this.SetupNoRandPartA();
            }
        }

        private void SetupRandPartA()
        {
            if (this.i2 <= this.last)
            {
                this.chPrev = this.ch2;
                this.ch2 = this.ll8[this.tPos];
                this.tPos = this.tt[this.tPos];
                if (this.rNToGo == 0)
                {
                    this.rNToGo = BZip2Constants.RandomNumbers[this.rTPos];
                    this.rTPos++;
                    if (this.rTPos == 0x200)
                    {
                        this.rTPos = 0;
                    }
                }
                this.rNToGo--;
                this.ch2 ^= (this.rNToGo == 1) ? 1 : 0;
                this.i2++;
                this.currentChar = this.ch2;
                this.currentState = 3;
                this.mCrc.Update(this.ch2);
            }
            else
            {
                this.EndBlock();
                this.InitBlock();
                this.SetupBlock();
            }
        }

        private void SetupRandPartB()
        {
            if (this.ch2 != this.chPrev)
            {
                this.currentState = 2;
                this.count = 1;
                this.SetupRandPartA();
            }
            else
            {
                this.count++;
                if (this.count >= 4)
                {
                    this.z = this.ll8[this.tPos];
                    this.tPos = this.tt[this.tPos];
                    if (this.rNToGo == 0)
                    {
                        this.rNToGo = BZip2Constants.RandomNumbers[this.rTPos];
                        this.rTPos++;
                        if (this.rTPos == 0x200)
                        {
                            this.rTPos = 0;
                        }
                    }
                    this.rNToGo--;
                    this.z = (byte) (this.z ^ ((this.rNToGo == 1) ? ((byte) 1) : ((byte) 0)));
                    this.j2 = 0;
                    this.currentState = 4;
                    this.SetupRandPartC();
                }
                else
                {
                    this.currentState = 2;
                    this.SetupRandPartA();
                }
            }
        }

        private void SetupRandPartC()
        {
            if (this.j2 < this.z)
            {
                this.currentChar = this.ch2;
                this.mCrc.Update(this.ch2);
                this.j2++;
            }
            else
            {
                this.currentState = 2;
                this.i2++;
                this.count = 0;
                this.SetupRandPartA();
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("BZip2InputStream Write not supported");
        }

        public override void WriteByte(byte value)
        {
            throw new NotSupportedException("BZip2InputStream WriteByte not supported");
        }

        public override bool CanRead
        {
            get
            {
                return this.baseStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return this.baseStream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return false;
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
                throw new NotSupportedException("BZip2InputStream position cannot be set");
            }
        }
    }
}

