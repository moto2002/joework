using System.IO;
using System ;
using System.Text;

public class ByteArray
{
    private MemoryStream m_Stream = new MemoryStream();
    private BinaryReader m_Reader = null;
    private BinaryWriter m_Writer = null;
 
    public ByteArray()
    {
        Init();
    }
 
    public ByteArray(MemoryStream ms)
    {
        m_Stream = ms;
        Init();
    }
 
    public ByteArray(byte[] buffer)
    {
        m_Stream = new MemoryStream(buffer);
        Init();
    }
 
    private void Init()
    {
        m_Writer = new BinaryWriter(m_Stream);
        m_Reader = new BinaryReader(m_Stream);
    }
 
    public int Length
    {
        get { return (int)m_Stream.Length; }
    }
 
    public int Postion
    {
        get { return (int)m_Stream.Position; }
        set { m_Stream.Position = value; }
    }
 
    public byte[] Buffer
    {
        get { return m_Stream.GetBuffer(); }
    }
 
    internal MemoryStream MemoryStream { get { return m_Stream; } }

    public bool ReadBoolean()
    {
        return m_Reader.ReadBoolean();
    }

    public byte ReadByte()
    {
        return m_Reader.ReadByte();
    }

    public void ReadBytes(byte[] bytes, uint offset, uint length)
    {
        byte[] tmp = m_Reader.ReadBytes((int)length);
        for (int i = 0; i < tmp.Length; i++)
            bytes[i + offset] = tmp[i];
    }

    public double ReadDouble()
    {
        return m_Reader.ReadDouble();
    }
	
    public float ReadFloat()
    {
        byte[] bytes = m_Reader.ReadBytes(4);
        byte[] invertedBytes = new byte[4];
        for (int i = 3, j = 0; i >= 0; i--, j++)
        {
            invertedBytes[j] = bytes[j];
        }
        float value = BitConverter.ToSingle(invertedBytes, 0);
        return value;
    }
	
    public int ReadInt()
    {
        return m_Reader.ReadInt32();
    }
	
    public short ReadShort()
    {
        return m_Reader.ReadInt16();
    }

    public byte ReadUnsignedByte()
    {
        return m_Reader.ReadByte();
    }

    public uint ReadUnsignedInt()
    {
        return (uint)m_Reader.ReadInt32();
    }
	
    public ushort ReadUnsignedShort()
    {
        return m_Reader.ReadUInt16();
    }
	
    public string ReadUTF()
    {
        return ReadUTFBytes(ReadUnsignedShort()) ;
    }
	
    public string ReadUTFBytes(uint length)
    {
        if (length == 0)
            return string.Empty;
        UTF8Encoding utf8 = new UTF8Encoding(false, true);
        byte[] encodedBytes = m_Reader.ReadBytes((int)length);
        string decodedString = utf8.GetString(encodedBytes, 0, encodedBytes.Length);
        return decodedString;
    }
	
    public void WriteBoolean(bool value)
    {
        m_Writer.BaseStream.WriteByte(value ? ((byte)1) : ((byte)0));
    }
	
    public void WriteByte(byte value)
    {
        m_Writer.BaseStream.WriteByte(value);
    }
	
    public void WriteBytes(byte[] buffer)
    {
        m_Writer.BaseStream.Write(buffer,0,buffer.Length);
    }
	
    public void WriteBytes(byte[] bytes, int offset, int length)
    {
        m_Writer.BaseStream.Write(bytes,offset,length);
    }
	
    public void WriteDouble(double value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        WriteBigEndian(bytes);
    }
	
    public void WriteFloat(float value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        WriteBigEndian(bytes);
    }
	
    private void WriteBigEndian(byte[] bytes)
    {
        if (bytes == null)
            return;
		m_Writer.BaseStream.Write(bytes,0,bytes.Length);
    }
	
    public void WriteInt32(int value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        WriteBigEndian(bytes);
    }
	
    public void WriteInt(int value)
    {
        WriteInt32(value);
    }
	
    public void WriteShort(int value)
    {
        byte[] bytes = BitConverter.GetBytes((ushort)value);
        WriteBigEndian(bytes);
    }
	
    public void WriteUnsignedInt(uint value)
    {
        WriteInt32((int)value);
    }

    public void WriteUTF(string value)
    {
        UTF8Encoding utf8Encoding = new UTF8Encoding();
        int byteCount = utf8Encoding.GetByteCount(value);
        byte[] buffer = utf8Encoding.GetBytes(value);
        WriteShort(byteCount);
        if (buffer.Length > 0)
            m_Writer.Write(buffer);
    }
	
    public void WriteUTFBytes(string value)
    {
        UTF8Encoding utf8Encoding = new UTF8Encoding();
        byte[] buffer = utf8Encoding.GetBytes(value);
        if (buffer.Length > 0)
            m_Writer.Write(buffer);
    }

    public void WriteStringBytes(string value)
    {
        UTF8Encoding utf8Encoding = new UTF8Encoding();
        byte[] buffer = utf8Encoding.GetBytes(value);
        if (buffer.Length > 0)
        {
            m_Writer.Write(buffer.Length);
            m_Writer.Write(buffer);
        }
    }
 
}