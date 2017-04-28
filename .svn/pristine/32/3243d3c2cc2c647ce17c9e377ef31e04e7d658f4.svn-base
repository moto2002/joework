using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DataBase
{
    public class DBBase
    {
        protected static BinaryReader s_reader;

        /// <summary>
        /// 读取二进制文件
        /// </summary>
        protected static void openFile(string fileName)
        {
            s_reader = new BinaryReader(new FileStream(fileName, FileMode.Open));
        }

        /// <summary>
        /// 关闭二进制文件
        /// </summary>
        protected static void closeFile()
        {
            s_reader.Close();
            s_reader = null;
        }

        /// <summary>
        /// 读取字节
        /// </summary>
        protected static byte readByte()
        {
            return s_reader.ReadByte();
        }

        /// <summary>
        /// 读取短整型
        /// </summary>
        protected static short readShort()
        {
            return s_reader.ReadInt16();
        }

        /// <summary>
        /// 读取整型
        /// </summary>
        protected static int readInt()
        {
            return s_reader.ReadInt32();
        }

        /// <summary>
        /// 读取布尔值
        /// </summary>
        protected static bool readBool()
        {
            return s_reader.ReadBoolean();
        }

        /// <summary>
        /// 读取浮点数
        /// </summary>
        protected static float readFloat()
        {
            return float.Parse(readString());
        }

        /// <summary>
        /// 读取字符串
        /// </summary>
        protected static string readString()
	    {
            byte length = s_reader.ReadByte();
            char[] chars = s_reader.ReadChars(length);
            return (new string(chars));
	    }

        /// <summary>
        /// 读取int数组
        /// </summary>
        protected static int[] readIntList()
        {
            short length = readShort();
            int[] list = new int[length];
            for (int i = 0; i < length; i++)
            {
                list[i] = readInt();
            }
            return list;
        }

        /// <summary>
        /// 读取bool数组
        /// </summary>
        protected static bool[] readBoolList()
        {
            short length = readShort();
            bool[] list = new bool[length];
            for (int i = 0; i < length; i++)
            {
                list[i] = readBool();
            }
            return list;
        }

        /// <summary>
        /// 读取float数组
        /// </summary>
        protected static float[] readFloatList()
        {
            short length = readShort();
            float[] list = new float[length];
            for (int i = 0; i < length; i++)
            {
                list[i] = readFloat();
            }
            return list;
        }

        /// <summary>
        /// 读取string数组
        /// </summary>
        protected static string[] readStringList()
        {
            short length = readShort();
            string[] list = new string[length];
            for (int i = 0; i < length; i++)
            {
                list[i] = readString();
            }
            return list;
        }

        /// <summary>
        /// 读取int字典
        /// </summary>
        protected static Dictionary<string, int> readIntDict()
        {
            short length = readShort();
            Dictionary<string, int> dict = new Dictionary<string, int>(length);
            for (int i = 0; i < length; i++)
            {
                string key = readString();
                dict[key] = readInt();
            }
            return dict;
        }

        /// <summary>
        /// 读取bool字典
        /// </summary>
        protected static Dictionary<string, bool> readBoolDict()
        {
            short length = readShort();
            Dictionary<string, bool> dict = new Dictionary<string, bool>(length);
            for (int i = 0; i < length; i++)
            {
                string key = readString();
                dict[key] = readBool();
            }
            return dict;
        }

        /// <summary>
        /// 读取float字典
        /// </summary>
        protected static Dictionary<string, float> readFloatDict()
        {
            short length = readShort();
            Dictionary<string, float> dict = new Dictionary<string, float>(length);
            for (int i = 0; i < length; i++)
            {
                string key = readString();
                dict[key] = readFloat();
            }
            return dict;
        }

        /// <summary>
        /// 读取string字典
        /// </summary>
        protected static Dictionary<string, string> readStringDict()
        {
            short length = readShort();
            Dictionary<string, string> dict = new Dictionary<string, string>(length);
            for (int i = 0; i < length; i++)
            {
                string key = readString();
                dict[key] = readString();
            }
            return dict;
        }

        /// <summary>
        /// 数组描述(int)
        /// </summary>
        protected string listDesc(int[] list)
        {
            if (list == null)
                return "";
            string desc = "[";
            for (int i = 0; i < list.Length; i++)
            {
                desc += list[i];
                if (i < list.Length - 1)
                    desc += ", ";
            }
            desc += "]";
            return desc;
        }

        /// <summary>
        /// 数组描述(bool)
        /// </summary>
        protected string listDesc(bool[] list)
        {
            if (list == null)
                return "";
            string desc = "[";
            for (int i = 0; i < list.Length; i++)
            {
                desc += list[i];
                if (i < list.Length - 1)
                    desc += ", ";
            }
            desc += "]";
            return desc;
        }

        /// <summary>
        /// 数组描述(float)
        /// </summary>
        protected string listDesc(float[] list)
        {
            if (list == null)
                return "";
            string desc = "[";
            for (int i = 0; i < list.Length; i++)
            {
                desc += list[i];
                if (i < list.Length - 1)
                    desc += ", ";
            }
            desc += "]";
            return desc;
        }

        /// <summary>
        /// 数组描述(string)
        /// </summary>
        protected string listDesc(string[] list)
        {
            if (list == null)
                return "";
            string desc = "[";
            for (int i = 0; i < list.Length; i++)
            {
                desc += list[i];
                if (i < list.Length - 1)
                    desc += ", ";
            }
            desc += "]";
            return desc;
        }

        /// <summary>
        /// 字典描述(int)
        /// </summary>
        protected string dictDesc(Dictionary<string, int> dict)
        {
            if (dict == null)
                return "";
            string desc = "{";
            string[] keys = dict.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                desc += (keys[i] + "=" + dict[keys[i]]);
                if (i < keys.Length - 1)
                    desc += ", ";
            }
            desc += "}";
            return desc;
        }

        /// <summary>
        /// 字典描述(bool)
        /// </summary>
        protected string dictDesc(Dictionary<string, bool> dict)
        {
            if (dict == null)
                return "";
            string desc = "{";
            string[] keys = dict.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                desc += (keys[i] + "=" + dict[keys[i]]);
                if (i < keys.Length - 1)
                    desc += ", ";
            }
            desc += "}";
            return desc;
        }

        /// <summary>
        /// 字典描述(float)
        /// </summary>
        protected string dictDesc(Dictionary<string, float> dict)
        {
            if (dict == null)
                return "";
            string desc = "{";
            string[] keys = dict.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                desc += (keys[i] + "=" + dict[keys[i]]);
                if (i < keys.Length - 1)
                    desc += ", ";
            }
            desc += "}";
            return desc;
        }

        /// <summary>
        /// 字典描述(string)
        /// </summary>
        protected string dictDesc(Dictionary<string, string> dict)
        {
            if (dict == null)
                return "";
            string desc = "{";
            string[] keys = dict.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                desc += (keys[i] + "=" + dict[keys[i]]);
                if (i < keys.Length - 1)
                    desc += ", ";
            }
            desc += "}";
            return desc;
        }
    }
}
