using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DataBase
{
    enum DBBinMODE { Array, IntDict, StrDict };
    enum DBBinCOMPLEX { None, List, Dict };
    enum DBBinData { Null, Int, Bool, Float, String };

    class DBBinary
    {
        protected static Dictionary<string, object> s_unionTable;

        protected static string readString(BinaryReader reader)
        {
            byte length = reader.ReadByte();
            char[] chars = reader.ReadChars(length);
            return (new string(chars));
        }

        protected static float readFloat(BinaryReader reader)
        {
            return float.Parse(readString(reader));
        }

        protected static object readAttribute(BinaryReader reader, DBBinData type)
	    {
		    object value = null;
		    switch (type)
		    {
		    case DBBinData.Int:
			    value = reader.ReadInt32();
			    break;
		    case DBBinData.Bool:
			    value = reader.ReadBoolean();
			    break;
		    case DBBinData.Float:
			    value = readFloat(reader);
			    break;
		    case DBBinData.String:
			    value = readString(reader);
			    break;
		    }
		    return value;
	    }
	
	    protected static Object readList(BinaryReader reader, DBBinData type)
	    {
		    short count = reader.ReadInt16();
		    object[] list = new object[count];
		    for (int i = 0; i < count; i++)
		    {
			    list[i] = readAttribute(reader, type);
		    }
		    return list;
	    }

        protected static Object readDict(BinaryReader reader, DBBinData type)
	    {
            short count = reader.ReadInt16();
            Dictionary<string, object> dict = new Dictionary<string, object>(count);
		    for (int i = 0; i < count; i++)
		    {
                String key = readString(reader);
                Object value = readAttribute(reader, type);
			    dict.Add(key, value);
		    }
		    return dict;
	    }

        static string[] readUnionKeys(BinaryReader reader)
	    {
            byte unionCount = reader.ReadByte();
		    string[] keys = new string[unionCount];
		    for (int i = 0; i < unionCount; i++)
		    {
                keys[i] = readString(reader);
		    }
		    return keys;
	    }

        public static Object loadData(string fileName)
        {
            BinaryReader reader = new BinaryReader(new FileStream(fileName, FileMode.Open));
            Object dataBase = parseDB(reader);
            return dataBase;
        }

        public static Object parseDB(BinaryReader reader)
        {
            Object dataBase = null;

            DBBinMODE mode = (DBBinMODE)reader.ReadByte();
            short dataCount = reader.ReadInt16();
            short valueCount = reader.ReadInt16();
            string[] unionKeys = readUnionKeys(reader);
            Dictionary<string, object> unionTable = new Dictionary<string, object>(dataCount);

            if (mode == DBBinMODE.Array)
            {
                List<object> temp = new List<object>(dataCount);
                for (int i = 0; i < dataCount; i++)
                {
                    Dictionary<string, object> data = readData(reader, valueCount);
                    temp.Add(data);
                    setUnionKeys(unionTable, data, unionKeys);
                }
                dataBase = temp;
            }
            else if (mode == DBBinMODE.IntDict)
            {
                Dictionary<int, object> temp = new Dictionary<int, object>(dataCount);
                for (int i = 0; i < dataCount; i++)
                {
                    int index = reader.ReadInt32();
                    Dictionary<string, object> data = readData(reader, valueCount);
                    temp.Add(index, data);
                    setUnionKeys(unionTable, data, unionKeys);
                }
                dataBase = temp;
            }
            else if (mode == DBBinMODE.StrDict)
            {
                Dictionary<string, object> temp = new Dictionary<string, object>(dataCount);
                for (int i = 0; i < dataCount; i++)
                {
                    string key = readString(reader);
                    Dictionary<string, object> data = readData(reader, valueCount);
                    temp.Add(key, data);
                    setUnionKeys(unionTable, data, unionKeys);
                }
                dataBase = temp;
            }

            s_unionTable = unionTable;

            return dataBase;
        }

        protected static Dictionary<string, object> readData(BinaryReader reader, int count)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();

            for (int i = 0; i < count; i++)
            {
                string name = readString(reader);
                DBBinData type = (DBBinData)reader.ReadByte();
                DBBinCOMPLEX complex = (DBBinCOMPLEX)reader.ReadByte();
                Object value = null;
                switch (complex)
                {
                    case DBBinCOMPLEX.None:
                        value = readAttribute(reader, type);
                        break;
                    case DBBinCOMPLEX.List:
                        value = readList(reader, type);
                        break;
                    case DBBinCOMPLEX.Dict:
                        value = readDict(reader, type);
                        break;
                }
                data.Add(name, value);
            }

            return data;
        }

        public static void setUnionKeys(Dictionary<string, object> unionTable, Dictionary<string, object> data, string[] unionKeys)
        {
            int count = unionKeys.Length;
            if (count > 0)
            {
                string unionKey = "";
                for (int i = 0; i < count; i++)
                {
                    unionKey += data[unionKeys[i]] + (i < count - 1 ? "_" : "");
                }
                unionTable.Add(unionKey, data);
            }
        }

        public static Dictionary<string, object> UnionTable
        {
            get { return s_unionTable; }
        }
    }
}
