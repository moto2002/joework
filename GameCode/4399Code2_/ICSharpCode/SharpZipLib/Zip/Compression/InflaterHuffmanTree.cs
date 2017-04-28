namespace ICSharpCode.SharpZipLib.Zip.Compression
{
    using ICSharpCode.SharpZipLib;
    using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
    using System;

    public class InflaterHuffmanTree
    {
        public static InflaterHuffmanTree defDistTree;
        public static InflaterHuffmanTree defLitLenTree;
        private const int MAX_BITLEN = 15;
        private short[] tree;

        static InflaterHuffmanTree()
        {
            try
            {
                byte[] codeLengths = new byte[0x120];
                int num = 0;
                while (num < 0x90)
                {
                    codeLengths[num++] = 8;
                }
                while (num < 0x100)
                {
                    codeLengths[num++] = 9;
                }
                while (num < 280)
                {
                    codeLengths[num++] = 7;
                }
                while (num < 0x120)
                {
                    codeLengths[num++] = 8;
                }
                defLitLenTree = new InflaterHuffmanTree(codeLengths);
                codeLengths = new byte[0x20];
                num = 0;
                while (num < 0x20)
                {
                    codeLengths[num++] = 5;
                }
                defDistTree = new InflaterHuffmanTree(codeLengths);
            }
            catch (Exception)
            {
                throw new SharpZipBaseException("InflaterHuffmanTree: static tree length illegal");
            }
        }

        public InflaterHuffmanTree(byte[] codeLengths)
        {
            this.BuildTree(codeLengths);
        }

        private void BuildTree(byte[] codeLengths)
        {
            int num2;
            int num5;
            int num6;
            int[] numArray = new int[0x10];
            int[] numArray2 = new int[0x10];
            int index = 0;
            while (index < codeLengths.Length)
            {
                num2 = codeLengths[index];
                if (num2 > 0)
                {
                    numArray[num2]++;
                }
                index++;
            }
            int toReverse = 0;
            int num4 = 0x200;
            for (num2 = 1; num2 <= 15; num2++)
            {
                numArray2[num2] = toReverse;
                toReverse += numArray[num2] << (0x10 - num2);
                if (num2 >= 10)
                {
                    num5 = numArray2[num2] & 0x1ff80;
                    num6 = toReverse & 0x1ff80;
                    num4 += (num6 - num5) >> (0x10 - num2);
                }
            }
            this.tree = new short[num4];
            int num7 = 0x200;
            num2 = 15;
            while (num2 >= 10)
            {
                num6 = toReverse & 0x1ff80;
                toReverse -= numArray[num2] << (0x10 - num2);
                num5 = toReverse & 0x1ff80;
                index = num5;
                while (index < num6)
                {
                    this.tree[DeflaterHuffman.BitReverse(index)] = (short) ((-num7 << 4) | num2);
                    num7 += ((int) 1) << (num2 - 9);
                    index += 0x80;
                }
                num2--;
            }
            for (index = 0; index < codeLengths.Length; index++)
            {
                num2 = codeLengths[index];
                if (num2 != 0)
                {
                    toReverse = numArray2[num2];
                    int num8 = DeflaterHuffman.BitReverse(toReverse);
                    if (num2 <= 9)
                    {
                        do
                        {
                            this.tree[num8] = (short) ((index << 4) | num2);
                            num8 += ((int) 1) << num2;
                        }
                        while (num8 < 0x200);
                    }
                    else
                    {
                        int num9 = this.tree[num8 & 0x1ff];
                        int num10 = ((int) 1) << (num9 & 15);
                        num9 = -(num9 >> 4);
                        do
                        {
                            this.tree[num9 | (num8 >> 9)] = (short) ((index << 4) | num2);
                            num8 += ((int) 1) << num2;
                        }
                        while (num8 < num10);
                    }
                    numArray2[num2] = toReverse + (((int) 1) << (0x10 - num2));
                }
            }
        }

        public int GetSymbol(StreamManipulator input)
        {
            int num2;
            int availableBits;
            int index = input.PeekBits(9);
            if (index >= 0)
            {
                num2 = this.tree[index];
                if (num2 >= 0)
                {
                    input.DropBits(num2 & 15);
                    return (num2 >> 4);
                }
                int num3 = -(num2 >> 4);
                int bitCount = num2 & 15;
                index = input.PeekBits(bitCount);
                if (index >= 0)
                {
                    num2 = this.tree[num3 | (index >> 9)];
                    input.DropBits(num2 & 15);
                    return (num2 >> 4);
                }
                availableBits = input.AvailableBits;
                index = input.PeekBits(availableBits);
                num2 = this.tree[num3 | (index >> 9)];
                if ((num2 & 15) <= availableBits)
                {
                    input.DropBits(num2 & 15);
                    return (num2 >> 4);
                }
                return -1;
            }
            availableBits = input.AvailableBits;
            index = input.PeekBits(availableBits);
            num2 = this.tree[index];
            if ((num2 >= 0) && ((num2 & 15) <= availableBits))
            {
                input.DropBits(num2 & 15);
                return (num2 >> 4);
            }
            return -1;
        }
    }
}

