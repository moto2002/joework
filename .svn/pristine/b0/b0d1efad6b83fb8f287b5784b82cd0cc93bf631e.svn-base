using System;
using System.IO;
using System.Text;
public class ByteArray : MemoryStream
{
    public ByteArray()
    {
    }

    public ByteArray(byte[] bytes)
        : base(bytes)
    {
    }

    public byte[] ToBytes()
    {
        int length = (int)this.Length;
        byte[] bytes = new byte[length];
        Array.Copy(this.GetBuffer(), 0, bytes, 0, length);
        return bytes;
    }

    public void Write(bool value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        this.Write(bytes, 0, bytes.Length);
    }

    public void Write(byte value)
    {
        this.WriteByte(value);
    }

    public void Write(byte[] bytes)
    {
        int length = bytes.Length;
        for (int i = 0; i < length; i++)
        {
            Write(bytes[i]);
        }
    }

    public void Write(int[] bytes)
    {
        int length = bytes.Length;
        for (int i = 0; i < length; i++)
        {
            Write(bytes[i]);
        }
    }

    public void Write(double value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        this.Write(bytes, 0, bytes.Length);
    }
    public void Write(long value)
    {
        //Converter.GetBigEndian((long)value)
        byte[] bytes = BitConverter.GetBytes(value);
        this.Write(bytes, 0, bytes.Length);
    }
    public void Write(float value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        this.Write(bytes, 0, bytes.Length);
    }

    public void Write(int value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        this.Write(bytes, 0, bytes.Length);
    }

    public void Write(ushort value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        this.Write(bytes, 0, bytes.Length);
    }
    public void Write(short value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        this.Write(bytes, 0, bytes.Length);
    }

    public void Write(uint value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        this.Write(bytes, 0, bytes.Length);
    }

    public void Write(string value)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(value);
        this.Write(bytes.Length); //先写入 字节长度 modify by wukun 2015/11/3
        this.Write(bytes, 0, bytes.Length);
    }

    public byte[] ReadBytes(int size)
    {
        byte[] bytes = new byte[size];
        this.Read(bytes, 0, size);
        return bytes;
    }

    public void ReadBytes(byte[] bytes, int offset, int length)
    {
        this.Read(bytes, offset, length);
    }

    public bool ReadBoolean()
    {
        byte[] bytes = this.ReadBytes(sizeof(bool));
        return BitConverter.ToBoolean(bytes, 0);
    }

    public new byte ReadByte()
    {
        return this.ReadBytes(sizeof(byte))[0];
    }

    public double ReadDouble()
    {
        byte[] bytes = this.ReadBytes(sizeof(double));
        return BitConverter.ToDouble(bytes, 0);
    }

    public float ReadFloat()
    {
        byte[] bytes = this.ReadBytes(sizeof(float));
        return BitConverter.ToSingle(bytes, 0);
    }

    public int ReadInt()
    {
        byte[] bytes = this.ReadBytes(sizeof(int));
        return BitConverter.ToInt32(bytes, 0);
    }

    public short ReadShort()
    {
        byte[] bytes = this.ReadBytes(sizeof(short));
        return BitConverter.ToInt16(bytes, 0);
    }

    public byte ReadUnsignedByte()
    {
        return this.ReadBytes(sizeof(byte))[0];
    }

    public uint ReadUnsignedInt32()
    {
        byte[] bytes = this.ReadBytes(sizeof(uint));
        return BitConverter.ToUInt32(bytes, 0);
    }
    public ulong ReadUnsignedInt64()
    {
        byte[] bytes = this.ReadBytes(sizeof(ulong));
        return BitConverter.ToUInt64(bytes, 0);
    }
    public long ReadInt64()
    {
        byte[] bytes = this.ReadBytes(sizeof(ulong));
        return BitConverter.ToInt64(bytes, 0);
    }

    public ushort ReadUnsignedShort()
    {
        byte[] bytes = this.ReadBytes(sizeof(ushort));
        return BitConverter.ToUInt16(bytes, 0);
    }

    public string ReadUTFBytes(int length)
    {
        byte[] bytes = this.ReadBytes(length);
        return Encoding.UTF8.GetString(bytes, 0, length);
    }
    //默认UTF-8编码
    public string ReadString()
    {
        int length = this.ReadInt();
        byte[] bytes = this.ReadBytes(length);
        return Encoding.UTF8.GetString(bytes, 0, length);
    }

}