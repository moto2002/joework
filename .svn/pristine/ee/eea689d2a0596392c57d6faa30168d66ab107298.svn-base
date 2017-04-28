using System.IO;
using System.Text;
using System;

public class ByteBuffer
{
    MemoryStream stream = null;
    BinaryWriter writer = null;
    BinaryReader reader = null;

    public ByteBuffer()
    {
        stream = new MemoryStream();
        writer = new BinaryWriter(stream);
    }

    public ByteBuffer(byte[] data)
    {
        if (data != null)
        {
            stream = new MemoryStream(data);
            reader = new BinaryReader(stream);
        }
        else
        {
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
        }
    }

    public void Close()
    {
        if (writer != null)
            writer.Close();
        if (reader != null)
            reader.Close();

        stream.Close();
        writer = null;
        reader = null;
        stream = null;
    }

    public void WriteByte(byte v)
    {
        writer.Write(v);
    }

    public void WriteUInt(uint v)
    {
        writer.Write(v);
    }
    public void WriteInt(int v)
    {
        writer.Write(v);
    }

    public void WriteUShort(ushort v)
    {
        writer.Write(v);
    }
    public void WriteShort(short v)
    {
        writer.Write(v);
    }

    public void WriteULong(ulong v)
    {
        writer.Write(v);
    }

    public void WriteLong(long v)
    {
        writer.Write(v);
    }

    public void WriteFloat(float v)
    {
        byte[] temp = BitConverter.GetBytes(v);
        Array.Reverse(temp);
        writer.Write(BitConverter.ToSingle(temp, 0));
    }

    public void WriteDouble(double v)
    {
        byte[] temp = BitConverter.GetBytes(v);
        Array.Reverse(temp);
        writer.Write(BitConverter.ToDouble(temp, 0));
    }

    public void WriteString(string v)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(v);
        writer.Write((ushort)bytes.Length);
        writer.Write(bytes);
    }

    public void WriteBytes(byte[] v)
    {
        writer.Write(v);
    }

    public byte ReadByte()
    {
        return reader.ReadByte();
    }

    public int ReadInt()
    {
        return reader.ReadInt32();
    }
    public uint ReadUInt()
    {
        return reader.ReadUInt32();
    }

    public ushort ReadUShort()
    {
        return reader.ReadUInt16();
    }

    public long ReadLong()
    {
        return reader.ReadInt64();
    }

    public float ReadFloat()
    {
        byte[] temp = BitConverter.GetBytes(reader.ReadSingle());
        Array.Reverse(temp);
        return BitConverter.ToSingle(temp, 0);
    }

    public double ReadDouble()
    {
        byte[] temp = BitConverter.GetBytes(reader.ReadDouble());
        Array.Reverse(temp);
        return BitConverter.ToDouble(temp, 0);
    }

    public string ReadString()
    {
        ushort len = reader.ReadUInt16();
        byte[] buffer = new byte[len];
        buffer = reader.ReadBytes(len);
        return Encoding.UTF8.GetString(buffer);
    }

    public byte[] ReadBytes(ushort len)
    {
        return reader.ReadBytes(len);
    }

    public byte[] ToBytes()
    {
        writer.Flush();
        return stream.ToArray();
    }

    public void Flush()
    {
        writer.Flush();
    }

    public long Reaiming()
    {
        return this.stream.Length - this.stream.Position;
    }
}



