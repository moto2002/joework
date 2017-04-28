namespace Mogo.Util
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class IndexInfo
    {
        public static IndexInfo Decode(byte[] data, ref int offset)
        {
            return new IndexInfo { Id = EncodeDecoder.DecodeString(data, ref offset), FileName = EncodeDecoder.DecodeString(data, ref offset), Path = EncodeDecoder.DecodeString(data, ref offset), Offset = EncodeDecoder.DecodeUInt32(data, ref offset), Length = EncodeDecoder.DecodeUInt32(data, ref offset), PageLength = EncodeDecoder.DecodeUInt32(data, ref offset), Deleted = EncodeDecoder.DecodeBoolean(data, ref offset) };
        }

        public static byte[] Encode(IndexInfo info)
        {
            List<byte> list = new List<byte>();
            list.AddRange(EncodeDecoder.EncodeString(info.Id));
            list.AddRange(EncodeDecoder.EncodeString(info.FileName));
            list.AddRange(EncodeDecoder.EncodeString(info.Path));
            list.AddRange(EncodeDecoder.EncodeUInt32(info.Offset));
            list.AddRange(EncodeDecoder.EncodeUInt32(info.Length));
            list.AddRange(EncodeDecoder.EncodeUInt32(info.PageLength));
            list.AddRange(EncodeDecoder.EncodeBoolean(info.Deleted));
            return list.ToArray();
        }

        public byte[] GetEncodeData()
        {
            return Encode(this);
        }

        public bool Deleted { get; set; }

        public string FileName { get; set; }

        public Mogo.Util.FileState FileState { get; set; }

        public string Id { get; set; }

        public uint Length { get; set; }

        public uint Offset { get; set; }

        public uint PageLength { get; set; }

        public string Path { get; set; }
    }
}

