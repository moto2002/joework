namespace HMF
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class Hmf
    {
        private List<bool> bools = new List<bool>();
        private List<double> doubles = new List<double>();
        private List<int> ints = new List<int>();
        private Stream stream = null;
        private List<string> strs = new List<string>();

        private void InitPool()
        {
            int num2;
            int num = Util.ReadVarint(this.stream);
            for (num2 = 0; num2 < num; num2++)
            {
                this.ints.Add(Util.ReadVarint(this.stream));
            }
            int num3 = Util.ReadVarint(this.stream);
            for (num2 = 0; num2 < num3; num2++)
            {
                this.doubles.Add(Util.ReadDouble(this.stream));
            }
            int num4 = Util.ReadVarint(this.stream);
            for (num2 = 0; num2 < num4; num2++)
            {
                int len = Util.ReadVarint(this.stream);
                this.strs.Add(Util.ReadStr(len, this.stream));
            }
        }

        private void MergeAll()
        {
            int num;
            MemoryStream stream = new MemoryStream();
            Util.WriteVarint(this.ints.Count, stream);
            for (num = 0; num < this.ints.Count; num++)
            {
                Util.WriteVarint(this.ints[num], stream);
            }
            Util.WriteVarint(this.doubles.Count, stream);
            for (num = 0; num < this.doubles.Count; num++)
            {
                Util.WriteDobule(this.doubles[num], stream);
            }
            Util.WriteVarint(this.strs.Count, stream);
            for (num = 0; num < this.strs.Count; num++)
            {
                Util.WriteVarint(this.strs[num].Length, stream);
                Util.WriteStr(this.strs[num], stream);
            }
            this.stream.Position = 0L;
            stream.Position = 0L;
            int length = (int) stream.Length;
            int count = (int) stream.Length;
            byte[] buffer = new byte[length];
            byte[] buffer2 = new byte[count];
            this.stream.Read(buffer2, 0, count);
            stream.Read(buffer, 0, length);
            this.stream.Position = 0L;
            this.stream.Write(buffer, 0, length);
            this.stream.Write(buffer2, 0, count);
        }

        private List<object> ReadArray()
        {
            List<object> list = new List<object>();
            int num = Util.ReadVarint(this.stream);
            for (int i = 0; i < num; i++)
            {
                byte num3 = (byte) Util.ReadVarint(this.stream);
                if (num3 == Tag.ARRAY_TAG)
                {
                    list.Add(this.ReadArray());
                }
                else if (num3 == Tag.OBJECT_TAG)
                {
                    list.Add(this.ReadDict());
                }
                else if (num3 == Tag.INT_TAG)
                {
                    list.Add(this.ints[Util.ReadVarint(this.stream)]);
                }
                else if (num3 == Tag.STRING_TAG)
                {
                    list.Add(this.strs[Util.ReadVarint(this.stream)]);
                }
                else if (num3 == Tag.DOUBLE_TAG)
                {
                    list.Add(this.strs[Util.ReadVarint(this.stream)]);
                }
            }
            return list;
        }

        private Dictionary<object, object> ReadDict()
        {
            Dictionary<object, object> dictionary = new Dictionary<object, object>();
            int num = Util.ReadVarint(this.stream);
            for (int i = 0; i < num; i++)
            {
                byte num3 = (byte) Util.ReadVarint(this.stream);
                object key = null;
                if (num3 == Tag.ARRAY_TAG)
                {
                    key = this.ReadArray();
                }
                else if (num3 == Tag.OBJECT_TAG)
                {
                    key = this.ReadDict();
                }
                else if (num3 == Tag.INT_TAG)
                {
                    key = this.ints[Util.ReadVarint(this.stream)];
                }
                else if (num3 == Tag.STRING_TAG)
                {
                    key = this.strs[Util.ReadVarint(this.stream)];
                }
                else if (num3 == Tag.DOUBLE_TAG)
                {
                    key = this.doubles[Util.ReadVarint(this.stream)];
                }
                num3 = (byte) Util.ReadVarint(this.stream);
                object obj3 = null;
                if (num3 == Tag.ARRAY_TAG)
                {
                    obj3 = this.ReadArray();
                }
                else if (num3 == Tag.OBJECT_TAG)
                {
                    obj3 = this.ReadDict();
                }
                else if (num3 == Tag.INT_TAG)
                {
                    obj3 = this.ints[Util.ReadVarint(this.stream)];
                }
                else if (num3 == Tag.STRING_TAG)
                {
                    obj3 = this.strs[Util.ReadVarint(this.stream)];
                }
                else if (num3 == Tag.DOUBLE_TAG)
                {
                    obj3 = this.doubles[Util.ReadVarint(this.stream)];
                }
                dictionary.Add(key, obj3);
            }
            return dictionary;
        }

        public object ReadObject(Stream stream)
        {
            this.SetStream(stream);
            this.InitPool();
            Console.WriteLine("p " + stream.Position);
            byte num = (byte) Util.ReadVarint(stream);
            object obj2 = null;
            if (num == Tag.ARRAY_TAG)
            {
                obj2 = this.ReadArray();
                this.Reset();
                return obj2;
            }
            if (num == Tag.OBJECT_TAG)
            {
                obj2 = this.ReadDict();
                this.Reset();
                return obj2;
            }
            Console.WriteLine("fuck " + num);
            this.Reset();
            return null;
        }

        private void RealWrite(object obj)
        {
            if (obj.GetType() == typeof(int))
            {
                this.WriteInt((int) obj);
            }
            else if (obj.GetType() == typeof(List<object>))
            {
                this.WriteArray((List<object>) obj);
            }
            else if (obj.GetType() == typeof(Dictionary<object, object>))
            {
                this.WriteDict((Dictionary<object, object>) obj);
            }
            else if (obj.GetType() == typeof(double))
            {
                this.WriteDouble((double) obj);
            }
            else if (obj.GetType() == typeof(string))
            {
                this.WriteString((string) obj);
            }
        }

        private void Reset()
        {
            this.ints = new List<int>();
            this.doubles = new List<double>();
            this.strs = new List<string>();
            this.bools = new List<bool>();
            this.bools.Add(false);
            this.bools.Add(true);
            this.stream = null;
        }

        private void SetStream(Stream stream)
        {
            this.stream = stream;
        }

        private void WriteArray(List<object> v)
        {
            Util.WriteVarint(Tag.ARRAY_TAG, this.stream);
            int count = v.Count;
            Util.WriteVarint(count, this.stream);
            for (int i = 0; i < count; i++)
            {
                this.RealWrite(v[i]);
            }
        }

        private void WriteDict(Dictionary<object, object> v)
        {
            Util.WriteVarint(Tag.OBJECT_TAG, this.stream);
            Util.WriteVarint(v.Count, this.stream);
            foreach (KeyValuePair<object, object> pair in v)
            {
                this.RealWrite(pair.Key);
                this.RealWrite(pair.Value);
            }
        }

        private void WriteDouble(double v)
        {
            Util.WriteVarint(Tag.DOUBLE_TAG, this.stream);
            int index = this.doubles.IndexOf(v);
            if (index != -1)
            {
                Util.WriteVarint(index, this.stream);
            }
            else
            {
                this.doubles.Add(v);
                index = this.doubles.Count - 1;
                Util.WriteVarint(index, this.stream);
            }
        }

        private void WriteInt(int v)
        {
            Util.WriteVarint(Tag.INT_TAG, this.stream);
            int index = this.ints.IndexOf(v);
            if (index != -1)
            {
                Util.WriteVarint(index, this.stream);
            }
            else
            {
                this.ints.Add(v);
                index = this.ints.Count - 1;
                Util.WriteVarint(index, this.stream);
            }
        }

        public void WriteObject(object obj, Stream stream)
        {
            this.SetStream(stream);
            this.RealWrite(obj);
            this.MergeAll();
        }

        private void WriteString(string v)
        {
            Util.WriteVarint(Tag.STRING_TAG, this.stream);
            int index = this.strs.IndexOf(v);
            if (index != -1)
            {
                Util.WriteVarint(index, this.stream);
            }
            else
            {
                this.strs.Add(v);
                index = this.strs.Count - 1;
                Util.WriteVarint(index, this.stream);
            }
        }
    }
}

