namespace ICSharpCode.SharpZipLib.Zip.Compression
{
    using ICSharpCode.SharpZipLib;
    using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
    using System;

    internal class InflaterDynHeader
    {
        private static readonly int[] BL_ORDER = new int[] { 
            0x10, 0x11, 0x12, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 
            14, 1, 15
         };
        private byte[] blLens;
        private const int BLLENS = 3;
        private int blnum;
        private const int BLNUM = 2;
        private InflaterHuffmanTree blTree;
        private int dnum;
        private const int DNUM = 1;
        private byte lastLen;
        private const int LENS = 4;
        private byte[] litdistLens;
        private int lnum;
        private const int LNUM = 0;
        private int mode;
        private int num;
        private int ptr;
        private static readonly int[] repBits = new int[] { 2, 3, 7 };
        private static readonly int[] repMin = new int[] { 3, 3, 11 };
        private const int REPS = 5;
        private int repSymbol;

        public InflaterHuffmanTree BuildDistTree()
        {
            byte[] destinationArray = new byte[this.dnum];
            Array.Copy(this.litdistLens, this.lnum, destinationArray, 0, this.dnum);
            return new InflaterHuffmanTree(destinationArray);
        }

        public InflaterHuffmanTree BuildLitLenTree()
        {
            byte[] destinationArray = new byte[this.lnum];
            Array.Copy(this.litdistLens, 0, destinationArray, 0, this.lnum);
            return new InflaterHuffmanTree(destinationArray);
        }

        public bool Decode(StreamManipulator input)
        {
            int num2;
            int num3;
            bool flag2;
            goto Label_034E;
        Label_007E:
            this.dnum = input.PeekBits(5);
            if (this.dnum < 0)
            {
                return false;
            }
            this.dnum++;
            input.DropBits(5);
            this.num = this.lnum + this.dnum;
            this.litdistLens = new byte[this.num];
            this.mode = 2;
        Label_00E9:
            this.blnum = input.PeekBits(4);
            if (this.blnum < 0)
            {
                return false;
            }
            this.blnum += 4;
            input.DropBits(4);
            this.blLens = new byte[0x13];
            this.ptr = 0;
            this.mode = 3;
        Label_0191:
            while (this.ptr < this.blnum)
            {
                int num = input.PeekBits(3);
                if (num < 0)
                {
                    return false;
                }
                input.DropBits(3);
                this.blLens[BL_ORDER[this.ptr]] = (byte) num;
                this.ptr++;
            }
            this.blTree = new InflaterHuffmanTree(this.blLens);
            this.blLens = null;
            this.ptr = 0;
            this.mode = 4;
        Label_0219:
            while (((num2 = this.blTree.GetSymbol(input)) & -16) == 0)
            {
                this.litdistLens[this.ptr++] = this.lastLen = (byte) num2;
                if (this.ptr == this.num)
                {
                    return true;
                }
            }
            if (num2 < 0)
            {
                return false;
            }
            if (num2 >= 0x11)
            {
                this.lastLen = 0;
            }
            else if (this.ptr == 0)
            {
                throw new SharpZipBaseException();
            }
            this.repSymbol = num2 - 0x10;
            this.mode = 5;
        Label_028E:
            num3 = repBits[this.repSymbol];
            int num4 = input.PeekBits(num3);
            if (num4 < 0)
            {
                return false;
            }
            input.DropBits(num3);
            num4 += repMin[this.repSymbol];
            if ((this.ptr + num4) > this.num)
            {
                throw new SharpZipBaseException();
            }
            while (num4-- > 0)
            {
                this.litdistLens[this.ptr++] = this.lastLen;
            }
            if (this.ptr == this.num)
            {
                return true;
            }
            this.mode = 4;
        Label_034E:
            flag2 = true;
            switch (this.mode)
            {
                case 0:
                    this.lnum = input.PeekBits(5);
                    if (this.lnum >= 0)
                    {
                        this.lnum += 0x101;
                        input.DropBits(5);
                        this.mode = 1;
                        goto Label_007E;
                    }
                    return false;

                case 1:
                    goto Label_007E;

                case 2:
                    goto Label_00E9;

                case 3:
                    goto Label_0191;

                case 4:
                    goto Label_0219;

                case 5:
                    goto Label_028E;
            }
            goto Label_034E;
        }
    }
}

