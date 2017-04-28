using System;
using System.Text;


public class ByteBufferCustom
{
    //增加的容量
    public const short CAPACITY_INCREASEMENT = 128;
    public const ushort USHORT_8 = (ushort)8;
    public const short SHORT_8 = (short)8;

    //字节数组
    private byte[] buffers;

    //读取索引
    private int readerIndex;

    //写的索引
    private int writerIndex;

    //上次备份的读取索引
    private int readerIndexBak;

    //字符数组 空字符串
    public static byte[] NULL_STRING = new byte[] { (byte)0, (byte)0 };

    //会调用带参数的构造函数
    public ByteBufferCustom()
        : this(8)
    {

    }

    /// <summary>
    /// 带参构造函数 初始化字节数组
    /// </summary>
    /// <param name="initCapacity">初始容量</param>
    public ByteBufferCustom(int initCapacity)
    {
        buffers = new byte[initCapacity];
    }

    /// <summary>
    /// 带参构造函数 向字节数组中 写字节
    /// </summary>
    /// <param name="buffers">字节数组</param>
    public ByteBufferCustom(byte[] buffers)
        : this(buffers.Length)
    {
        writeBytes(buffers);
    }

    public void writeBytes(byte[] data, int dataOffset, int dataSize)
    {
        ensureWritable(dataSize);
        Array.Copy(data, dataOffset, buffers, writerIndex, dataSize);
        writerIndex += dataSize;
    }

    public void writeBytes(byte[] data)
    {
        writeBytes(data, 0, data.Length);
    }
    public void writeByte(byte data)
    {
        writeBytes(new byte[] { data });
    }

    public void writeByte(int data)
    {
        writeBytes(new byte[] { (byte)data });
    }

    public void writeShort(int data)
    {
        writeBytes(new byte[] { (byte)(data >> 8), (byte)data });
    }

    public void writeInt(int data)
    {
        writeBytes(new byte[]
                   {
                (byte) (data >> 24),
                (byte) (data >> 16),
                (byte) (data >> 8),
                (byte) data
        });
    }

    public void writeString(string data)
    {
        writeString(data, Encoding.UTF8);
    }

    public void writeString(string data, Encoding encoding)
    {
        if (data == null)
        {
            writeBytes(NULL_STRING);
        }
        else
        {
            byte[] b = encoding.GetBytes(data);
            byte[] strBytes = new byte[b.Length + 2];
            strBytes[0] = (byte)((b.Length & 0xff00) >> 8);
            strBytes[1] = (byte)(b.Length & 0xff);
            b.CopyTo(strBytes, 2);
            writeBytes(strBytes);
        }
    }

    public byte readByte()
    {
        byte b = buffers[readerIndex];
        readerIndex++;
        return b;
    }

    public ushort readUnsignShort()
    {
        ushort u = (ushort)(buffers[readerIndex] << USHORT_8 | buffers[readerIndex + 1]);
        readerIndex += 2;
        return u;
    }

    public short readShort()
    {
        short i = (short)(buffers[readerIndex] << SHORT_8 | buffers[readerIndex + 1]);
        readerIndex += 2;
        return i;
    }

    public int readInt()
    {
        int i = buffers[readerIndex] << 24 | buffers[readerIndex + 1] << 16 | buffers[readerIndex + 2] << 8 |
            buffers[readerIndex + 3];
        readerIndex += 4;
        return i;
    }

    public uint readUnsignInt()
    {
        return (uint)readInt();
    }

    /// <summary>
    /// 从缓冲区中读取大小为length的byte[]
    /// 读取指针向后移length个长度
    /// </summary>
    /// <returns>读取到的byte[]</returns>
    /// <param name="length">多少个字节</param>
    public byte[] readBytes(int length)
    {
        byte[] b = new byte[length];
        Array.Copy(buffers, readerIndex, b, 0, length);
        readerIndex += length;
        return b;
    }

    public string readString()
    {
        return readString(Encoding.UTF8);
    }

    public string readString(Encoding encoding)
    {
        ushort charLength = readUnsignShort();
        byte[] strBytes = readBytes(charLength);
        return encoding.GetString(strBytes);
    }

    public void writeBuffer(ByteBufferCustom buff)
    {
        byte[] bytes = buff.readBytes(buff.readableBytes());
        writeBytes(bytes);
    }

    public ByteBufferCustom readBuffer(int length)
    {
        byte[] bytes = readBytes(length);
        return new ByteBufferCustom(bytes);
    }

    public byte[] toArray()
    {
        return readBytes(readableBytes());
    }

    public byte[] getBytes()
    {
        return buffers;
    }

    /// <summary>
    /// 可读取的容量长度.
    /// </summary>
    /// <returns>The bytes.</returns>
    public int readableBytes()
    {
        return writerIndex - readerIndex;
    }

    /// <summary>
    /// 备份读取索引.
    /// </summary>
    public void saveReaderIndex()
    {
        readerIndexBak = readerIndex;
    }

    /// <summary>
    ///重置读取索引为上一次备份的读取索引 
    /// </summary>
    public void loadReaderIndex()
    {
        readerIndex = readerIndexBak;
    }

    /// <summary>
    ///判断当前有没有足够的空间来写入数据
    /// </summary>
    /// <param name="dataSize">要写入数据的长度</param>
    private void ensureWritable(int dataSize)
    {
        //剩余的可写入空间
        int leftCapacity = buffers.Length - writerIndex;
        if (leftCapacity < dataSize)
        {
            int oldReaderIndex = readerIndex;
            int oldWriterIndex = writerIndex;
            writerIndex = readableBytes();
            readerIndex = 0;
            int needSize = dataSize - (buffers.Length - writerIndex);
            if (needSize <= 0)
            {
                //回收一些已经读取过的空间来使用，把可读取的字符串移到起始位置，就是读取索引 =0的位置开始
                Array.Copy(buffers, oldReaderIndex, buffers, 0, oldWriterIndex - oldReaderIndex);
            }
            else
            {
                //重建一个更大的数组
                int newCapacity = ((needSize + buffers.Length) / CAPACITY_INCREASEMENT + 1) * CAPACITY_INCREASEMENT;
                byte[] newBuffers = new byte[newCapacity];
                Array.Copy(buffers, oldReaderIndex, newBuffers, 0, oldWriterIndex - oldReaderIndex);
                buffers = newBuffers;
            }
        }
    }


    public int getReaderIndex()
    {
        return readerIndex;
    }

    public int getWriterIndex()
    {
        return writerIndex;
    }

    /// <summary>
    /// 得到buffer的容量大小
    /// </summary>
    /// <returns>The capacity.</returns>
    public int getCapacity()
    {
        return buffers.Length;
    }

    public string remainBufferString()
    {
        string s = "";
        for (int i = readerIndex; i < writerIndex; i++)
        {
            s += buffers;
            if (i < writerIndex - 1)
            {
                s += ", ";
            }
        }
        return s;
    }
}
