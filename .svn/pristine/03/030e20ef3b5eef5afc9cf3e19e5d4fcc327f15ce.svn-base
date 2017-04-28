namespace Mogo.Util
{
    using ICSharpCode.SharpZipLib.Zip;
    using Mogo.RPC;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;
    using UnityEngine;

    public static class Utils
    {
        private const char KEY_VALUE_SPRITER = ':';
        private const char LIST_SPRITER = ',';
        private const char MAP_SPRITER = ',';
        public const string RPC_HEAD = "RPC_";
        public const string SVR_RPC_HEAD = "SVR_RPC_";

        public static ulong BitReset(ulong data, int nBit)
        {
            if ((nBit >= 0) && (nBit < 0x40))
            {
                data &= ~(((int) 1) << nBit);
            }
            return data;
        }

        public static ulong BitSet(ulong data, int nBit)
        {
            if ((nBit >= 0) && (nBit < 0x40))
            {
                data |= ((int) 1) << nBit;
            }
            return data;
        }

        public static int BitTest(ulong data, int nBit)
        {
            int num = 0;
            if ((nBit >= 0) && (nBit < 0x40))
            {
                data &= ((int) 1) << nBit;
                if (data != 0L)
                {
                    num = 1;
                }
            }
            return num;
        }

        public static string BuildFileMd5(string filename)
        {
            string str = null;
            try
            {
                using (FileStream stream = File.OpenRead(filename))
                {
                    str = FormatMD5(MD5.Create().ComputeHash(stream));
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Except(exception, null);
            }
            return str;
        }

        public static T Choice<T>(List<T> list)
        {
            if (list.Count == 0)
            {
                return default(T);
            }
            int num = Random.Range(0, list.Count);
            return list[num];
        }

        public static void CircleXYByAngle(float angle, Vector3 O, Vector3 A, out Vector3 rnt)
        {
            float num = Vector3.Distance(O, A);
            rnt.y = A.y;
            rnt.x = (num * ((float) Math.Cos((double) angle))) + O.x;
            rnt.z = (num * ((float) Math.Sin((double) angle))) + O.z;
        }

        public static void CompressDirectory(this Stream target, string sourcePath, int zipLevel)
        {
            sourcePath = Path.GetFullPath(sourcePath);
            int startIndex = string.IsNullOrEmpty(sourcePath) ? Path.GetPathRoot(sourcePath).Length : sourcePath.Length;
            List<string> list = new List<string>();
            list.AddRange(from d in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories) select d + @"\");
            list.AddRange(Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories));
            using (ZipOutputStream stream = new ZipOutputStream(target))
            {
                stream.SetLevel(zipLevel);
                foreach (string str in list)
                {
                    string input = str.Substring(startIndex);
                    string name = input.StartsWith(@"\") ? input.ReplaceFirst(@"\", "", 0) : input;
                    name = name.Replace(@"\", "/");
                    stream.PutNextEntry(new ZipEntry(name));
                    if (!str.EndsWith(@"\"))
                    {
                        byte[] buffer = new byte[0x800];
                        using (FileStream stream2 = File.OpenRead(str))
                        {
                            int num2;
                            while ((num2 = stream2.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                stream.Write(buffer, 0, num2);
                            }
                        }
                    }
                }
                stream.Finish();
            }
        }

        public static void CompressDirectory(string sourcePath, string outputFilePath, int zipLevel = 0)
        {
            new FileStream(outputFilePath, FileMode.OpenOrCreate).CompressDirectory(sourcePath, zipLevel);
        }

        public static byte[] CreateMD5(byte[] data)
        {
            using (MD5 md = MD5.Create())
            {
                return md.ComputeHash(data);
            }
        }

        public static Random CreateRandom()
        {
            long ticks = DateTime.Now.Ticks;
            return new Random(((int) (((ulong) ticks) & 0xffffffffL)) | ((int) (ticks >> 0x20)));
        }

        private static bool DecodeDic(LuaTable luaTable, Type type, out object result)
        {
            result = type.GetConstructor(Type.EmptyTypes).Invoke(null);
            Type type2 = type.GetGenericArguments()[1];
            foreach (KeyValuePair<string, object> pair in luaTable)
            {
                object obj2 = null;
                if (luaTable.IsLuaTable(pair.Key))
                {
                    ParseLuaTable(pair.Value as LuaTable, type2, out obj2);
                }
                else
                {
                    obj2 = GetValue(pair.Value.ToString(), type2);
                }
                type.GetMethod("Add").Invoke(result, new object[] { luaTable.IsKeyString(pair.Key) ? ((object) pair.Key) : ((object) int.Parse(pair.Key)), obj2 });
            }
            return true;
        }

        private static bool DecodeDic(string inputString, Type type, ref int index, out object result)
        {
            if (!(type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))))
            {
                result = null;
                LoggerHelper.Error("Parse LuaTable error: type is not Dictionary: " + type, true);
                return false;
            }
            result = type.GetConstructor(Type.EmptyTypes).Invoke(null);
            if (!Utils.WaitChar(inputString, '{', ref index))
            {
                return false;
            }
            try
            {
                while (index < inputString.Length)
                {
                    string str;
                    bool flag;
                    object obj2;
                    Utils.DecodeKey(inputString, ref index, out str, out flag);
                    Utils.WaitChar(inputString, '=', ref index);
                    if (DecodeEntity(inputString, type.GetGenericArguments()[1], ref index, out obj2))
                    {
                        type.GetMethod("Add").Invoke(result, new object[] { flag ? ((object) str) : ((object) int.Parse(str)), obj2 });
                    }
                    if (!Utils.WaitChar(inputString, ',', ref index))
                    {
                        break;
                    }
                }
                Utils.WaitChar(inputString, '}', ref index);
                return true;
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Parse LuaTable error: " + inputString + exception.ToString(), true);
                return false;
            }
        }

        private static bool DecodeEntity(LuaTable luaTable, Type type, out object result)
        {
            if (type.GetConstructor(Type.EmptyTypes) == null)
            {
                LoggerHelper.Error(string.Format("type {0} can not create an entity. luatable: {1}", type.Name, luaTable), true);
                result = null;
                return false;
            }
            result = type.GetConstructor(Type.EmptyTypes).Invoke(null);
            PropertyInfo[] properties = type.GetProperties();
            foreach (KeyValuePair<string, object> pair in luaTable)
            {
                PropertyInfo property = null;
                if (luaTable.IsKeyString(pair.Key))
                {
                    property = type.GetProperty(pair.Key);
                }
                else
                {
                    int index = int.Parse(pair.Key) - 1;
                    if (index < properties.Length)
                    {
                        property = properties[index];
                    }
                }
                if (property != null)
                {
                    object obj2 = null;
                    if (luaTable.IsLuaTable(pair.Key))
                    {
                        ParseLuaTable(pair.Value as LuaTable, property.PropertyType, out obj2);
                    }
                    else
                    {
                        obj2 = GetValue(pair.Value.ToString(), property.PropertyType);
                    }
                    property.SetValue(result, obj2, null);
                }
            }
            return true;
        }

        private static bool DecodeEntity(string inputString, Type type, ref int index, out object result)
        {
            result = type.GetConstructor(Type.EmptyTypes).Invoke(null);
            if (!Utils.WaitChar(inputString, '{', ref index))
            {
                return false;
            }
            try
            {
                PropertyInfo[] properties = type.GetProperties();
                while (index < inputString.Length)
                {
                    string str;
                    bool flag;
                    object obj2;
                    PropertyInfo property = null;
                    Utils.DecodeKey(inputString, ref index, out str, out flag);
                    if (flag)
                    {
                        property = type.GetProperty(str);
                    }
                    else
                    {
                        int num = int.Parse(str) - 1;
                        if (num < properties.Length)
                        {
                            property = properties[num];
                        }
                    }
                    Utils.WaitChar(inputString, '=', ref index);
                    if ((property != null) && DecodeValue(inputString, property.PropertyType, ref index, out obj2))
                    {
                        property.SetValue(result, obj2, null);
                    }
                    if (!Utils.WaitChar(inputString, ',', ref index))
                    {
                        break;
                    }
                }
                Utils.WaitChar(inputString, '}', ref index);
                return true;
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Parse LuaTable error: " + inputString + exception.ToString(), true);
                return false;
            }
        }

        private static bool DecodeList(LuaTable luaTable, Type type, out object result)
        {
            result = type.GetConstructor(Type.EmptyTypes).Invoke(null);
            Type type2 = type.GetGenericArguments()[0];
            foreach (KeyValuePair<string, object> pair in luaTable)
            {
                object obj2 = null;
                if (luaTable.IsLuaTable(pair.Key))
                {
                    ParseLuaTable(pair.Value as LuaTable, type2, out obj2);
                }
                else
                {
                    obj2 = GetValue(pair.Value.ToString(), type2);
                }
                type.GetMethod("Add").Invoke(result, new object[] { obj2 });
            }
            return true;
        }

        private static bool DecodeValue(string inputString, Type type, ref int index, out object value)
        {
            int num2;
            switch (inputString[index])
            {
                case 's':
                {
                    int length = int.Parse(inputString.Substring(index + 1, 3));
                    if (length > 0)
                    {
                        index += 4;
                        value = inputString.Substring(index, length);
                        index += length;
                        return true;
                    }
                    value = "";
                    LoggerHelper.Error(string.Concat(new object[] { "Decode Lua Table Value Error: ", (int) index, " ", inputString }), true);
                    return false;
                }
                case '{':
                    if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Dictionary<,>)))
                    {
                        return DecodeDic(inputString, type, ref index, out value);
                    }
                    return DecodeEntity(inputString, type, ref index, out value);

                default:
                    num2 = index;
                    break;
            }
        Label_0154:
            if ((index += 1) < inputString.Length)
            {
                if (((inputString[index] == ',') || (inputString[index] == '}')) && (index > num2))
                {
                    string str2 = inputString.Substring(num2, index - num2);
                    value = GetValue(str2, type);
                    return true;
                }
                goto Label_0154;
            }
            value = GetValue("0", type);
            LoggerHelper.Error(string.Concat(new object[] { "Decode Lua Table Value Error: ", (int) index, " ", inputString }), true);
            return false;
        }

        public static void DecompressToDirectory(this Stream source, string targetPath)
        {
            targetPath = Path.GetFullPath(targetPath);
            using (ZipInputStream stream = new ZipInputStream(source))
            {
                ZipEntry entry;
                while ((entry = stream.GetNextEntry()) != null)
                {
                    string name = entry.Name;
                    if (entry.IsDirectory && entry.Name.StartsWith(@"\"))
                    {
                        name = entry.Name.ReplaceFirst(@"\", "", 0);
                    }
                    string path = Path.Combine(targetPath, name);
                    string directoryName = Path.GetDirectoryName(path);
                    if (!(string.IsNullOrEmpty(directoryName) || Directory.Exists(directoryName)))
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                    if (!entry.IsDirectory)
                    {
                        byte[] buffer = new byte[0x800];
                        using (FileStream stream2 = File.Create(path))
                        {
                            int num;
                            while ((num = stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                stream2.Write(buffer, 0, num);
                            }
                        }
                    }
                }
            }
        }

        public static void DecompressToDirectory(string targetPath, string zipFilePath)
        {
            if (File.Exists(zipFilePath))
            {
                File.OpenRead(zipFilePath).DecompressToDirectory(targetPath);
            }
            else
            {
                LoggerHelper.Error("Zip不存在: " + zipFilePath, true);
            }
        }

        public static void DecompressToMogoFile(string zipFilePath)
        {
            if (File.Exists(zipFilePath))
            {
                FileStream baseInputStream = File.OpenRead(zipFilePath);
                MogoFileSystem.Instance.Open();
                MogoFileSystem.Instance.GetAndBackUpIndexInfo();
                using (ZipInputStream stream2 = new ZipInputStream(baseInputStream))
                {
                    ZipEntry entry;
                    while ((entry = stream2.GetNextEntry()) != null)
                    {
                        if (!entry.IsDirectory)
                        {
                            string name = entry.Name;
                            if (!string.IsNullOrEmpty(name))
                            {
                                int num;
                                IndexInfo info = MogoFileSystem.Instance.BeginSaveFile(name, entry.Size);
                                byte[] buffer = new byte[0x800];
                                while ((num = stream2.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    MogoFileSystem.Instance.WriteFile(info, buffer, 0, num);
                                }
                                MogoFileSystem.Instance.EndSaveFile(info);
                            }
                        }
                    }
                }
                MogoFileSystem.Instance.SaveIndexInfo();
                MogoFileSystem.Instance.CleanBackUpIndex();
                MogoFileSystem.Instance.Close();
            }
            else
            {
                LoggerHelper.Error("Zip file not exist: " + zipFilePath, true);
            }
        }

        public static void DecompressToMogoFileAndDirectory(string targetPath, string zipFilePath)
        {
            if (File.Exists(zipFilePath))
            {
                FileStream baseInputStream = File.OpenRead(zipFilePath);
                MogoFileSystem.Instance.Open();
                MogoFileSystem.Instance.GetAndBackUpIndexInfo();
                using (ZipInputStream stream2 = new ZipInputStream(baseInputStream))
                {
                    ZipEntry entry;
                    while ((entry = stream2.GetNextEntry()) != null)
                    {
                        if (!entry.IsDirectory)
                        {
                            string name = entry.Name;
                            if (!string.IsNullOrEmpty(name))
                            {
                                int num;
                                byte[] buffer = new byte[0x800];
                                if (name.EndsWith(".u"))
                                {
                                    name = Path.Combine(targetPath, name);
                                    string directoryName = Path.GetDirectoryName(name);
                                    if (!(string.IsNullOrEmpty(directoryName) || Directory.Exists(directoryName)))
                                    {
                                        Directory.CreateDirectory(directoryName);
                                    }
                                    using (FileStream stream3 = File.Create(name))
                                    {
                                        while ((num = stream2.Read(buffer, 0, buffer.Length)) > 0)
                                        {
                                            stream3.Write(buffer, 0, num);
                                        }
                                    }
                                }
                                else
                                {
                                    IndexInfo info = MogoFileSystem.Instance.BeginSaveFile(name, entry.Size);
                                    while ((num = stream2.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        MogoFileSystem.Instance.WriteFile(info, buffer, 0, num);
                                    }
                                    MogoFileSystem.Instance.EndSaveFile(info);
                                }
                            }
                        }
                    }
                }
                MogoFileSystem.Instance.SaveIndexInfo();
                MogoFileSystem.Instance.CleanBackUpIndex();
                MogoFileSystem.Instance.Close();
            }
            else
            {
                LoggerHelper.Error("Zip file not exist: " + zipFilePath, true);
            }
        }

        public static string FormatMD5(byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", "").ToLower();
        }

        public static string FormatTime(this DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string FormatTime(this long datetime)
        {
            DateTime.FromBinary(datetime);
            return datetime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string GetDirectoryName(string fileName)
        {
            return fileName.Substring(0, fileName.LastIndexOf('/'));
        }

        public static string GetFileName(string path, char separator = '/')
        {
            return path.Substring(path.LastIndexOf(separator) + 1);
        }

        public static string GetFileNameWithoutExtention(string fileName, char separator = '/')
        {
            return GetFilePathWithoutExtention(GetFileName(fileName, separator));
        }

        public static string GetFilePathWithoutExtention(string fileName)
        {
            return fileName.Substring(0, fileName.LastIndexOf('.'));
        }

        public static string GetFullName(Transform rootTransform, Transform currentTransform)
        {
            string str = string.Empty;
            while (currentTransform != rootTransform)
            {
                str = currentTransform.get_name() + str;
                if (currentTransform.get_parent() != rootTransform)
                {
                    str = '/' + str;
                }
                currentTransform = currentTransform.get_parent();
            }
            return str;
        }

        public static void GetHttp(string Url, Action<string> onDone, Action<HttpStatusCode> onFail)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = (HttpWebRequest) WebRequest.Create(Url);
            }
            catch (Exception)
            {
                onFail(HttpStatusCode.NotAcceptable);
                return;
            }
            stopwatch.Stop();
            uint timerId = TimerHeap.AddTimer(0x3a98, 0, () => onFail(HttpStatusCode.RequestTimeout));
            stopwatch.Start();
            response = (HttpWebResponse) request.GetResponse();
            stopwatch.Stop();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                stopwatch.Start();
                TimerHeap.DelTimer(timerId);
                onFail(response.StatusCode);
                stopwatch.Stop();
            }
            else
            {
                TimerHeap.DelTimer(timerId);
                stopwatch.Start();
                Stream responseStream = response.GetResponseStream();
                stopwatch.Stop();
                stopwatch.Start();
                Encoding encoding = Encoding.UTF8;
                StreamReader reader = new StreamReader(responseStream, encoding);
                char[] buffer = new char[0x100];
                int length = reader.Read(buffer, 0, 0x100);
                StringBuilder builder = new StringBuilder("");
                while (length > 0)
                {
                    string str = new string(buffer, 0, length);
                    builder.Append(str);
                    length = reader.Read(buffer, 0, 0x100);
                }
                stopwatch.Stop();
                stopwatch.Start();
                response.Close();
                reader.Close();
                stopwatch.Stop();
                stopwatch.Start();
                onDone(builder.ToString());
                stopwatch.Stop();
            }
        }

        [DllImport("key", CallingConvention=CallingConvention.Cdecl)]
        public static extern int GetIndexKey(int i);
        public static byte[] GetIndexNumber()
        {
            List<byte> list = new List<byte>();
            for (int i = 0; i < 8; i++)
            {
                list.Add((byte) GetIndexKey(i));
            }
            return list.ToArray();
        }

        [DllImport("key", CallingConvention=CallingConvention.Cdecl)]
        public static extern int GetResKey(int i);
        public static byte[] GetResNumber()
        {
            List<byte> list = new List<byte>();
            for (int i = 0; i < 8; i++)
            {
                list.Add((byte) GetResKey(i));
            }
            return list.ToArray();
        }

        public static string GetStreamPath(string fileName)
        {
            string str = Application.get_streamingAssetsPath() + "/" + fileName;
            if (Application.get_platform() != 11)
            {
                str = "file://" + str;
            }
            return str;
        }

        public static DateTime GetTime(this int timeStamp)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(0x7b2, 1, 1)).AddSeconds((double) timeStamp);
        }

        public static object GetValue(string value, Type type)
        {
            if (type != null)
            {
                object obj2;
                object obj4;
                if (type == typeof(string))
                {
                    return value;
                }
                if (type == typeof(int))
                {
                    return Convert.ToInt32(Convert.ToDouble(value));
                }
                if (type == typeof(float))
                {
                    return float.Parse(value);
                }
                if (type == typeof(byte))
                {
                    return Convert.ToByte(Convert.ToDouble(value));
                }
                if (type == typeof(sbyte))
                {
                    return Convert.ToSByte(Convert.ToDouble(value));
                }
                if (type == typeof(uint))
                {
                    return Convert.ToUInt32(Convert.ToDouble(value));
                }
                if (type == typeof(short))
                {
                    return Convert.ToInt16(Convert.ToDouble(value));
                }
                if (type == typeof(long))
                {
                    return Convert.ToInt64(Convert.ToDouble(value));
                }
                if (type == typeof(ushort))
                {
                    return Convert.ToUInt16(Convert.ToDouble(value));
                }
                if (type == typeof(ulong))
                {
                    return Convert.ToUInt64(Convert.ToDouble(value));
                }
                if (type == typeof(double))
                {
                    return double.Parse(value);
                }
                if (type == typeof(bool))
                {
                    if (value == "0")
                    {
                        return false;
                    }
                    return ((value == "1") || bool.Parse(value));
                }
                if (type.BaseType == typeof(Enum))
                {
                    return GetValue(value, Enum.GetUnderlyingType(type));
                }
                if (type == typeof(Vector3))
                {
                    Vector3 vector;
                    ParseVector3(value, out vector);
                    return vector;
                }
                if (type == typeof(Quaternion))
                {
                    Quaternion quaternion;
                    ParseQuaternion(value, out quaternion);
                    return quaternion;
                }
                if (type == typeof(Color))
                {
                    Color color;
                    ParseColor(value, out color);
                    return color;
                }
                if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Dictionary<,>)))
                {
                    Type[] genericArguments = type.GetGenericArguments();
                    Dictionary<string, string> dictionary = value.ParseMap(':', ',');
                    obj2 = type.GetConstructor(Type.EmptyTypes).Invoke(null);
                    foreach (KeyValuePair<string, string> pair in dictionary)
                    {
                        object obj3 = GetValue(pair.Key, genericArguments[0]);
                        obj4 = GetValue(pair.Value, genericArguments[1]);
                        type.GetMethod("Add").Invoke(obj2, new object[] { obj3, obj4 });
                    }
                    return obj2;
                }
                if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>)))
                {
                    Type type2 = type.GetGenericArguments()[0];
                    List<string> list = value.ParseList(',');
                    obj2 = type.GetConstructor(Type.EmptyTypes).Invoke(null);
                    foreach (string str in list)
                    {
                        obj4 = GetValue(str, type2);
                        type.GetMethod("Add").Invoke(obj2, new object[] { obj4 });
                    }
                    return obj2;
                }
            }
            return null;
        }

        public static byte[] LoadByteFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                return File.ReadAllBytes(fileName);
            }
            return null;
        }

        public static byte[] LoadByteResource(string fileName)
        {
            TextAsset asset = Resources.Load(fileName, typeof(TextAsset)) as TextAsset;
            byte[] buffer = asset.get_bytes();
            Resources.UnloadAsset(asset);
            return buffer;
        }

        public static string LoadFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (StreamReader reader = File.OpenText(fileName))
                {
                    return reader.ReadToEnd();
                }
            }
            return string.Empty;
        }

        public static string LoadResource(string fileName)
        {
            Object obj2 = Resources.Load(fileName);
            if (obj2 != null)
            {
                string str = obj2.ToString();
                Resources.UnloadAsset(obj2);
                return str;
            }
            return string.Empty;
        }

        public static void MountToSomeObjWithoutPosChange(Transform child, Transform parent)
        {
            Vector3 vector = child.get_localScale();
            Vector3 vector2 = child.get_localPosition();
            Vector3 vector3 = child.get_localEulerAngles();
            child.set_parent(parent);
            child.set_localScale(vector);
            child.set_localEulerAngles(vector3);
            child.set_localPosition(vector2);
        }

        public static string PackArray<T>(this T[] array, char listSpriter = ',')
        {
            List<T> list = new List<T>();
            list.AddRange(array);
            return list.PackList<T>(listSpriter);
        }

        public static void PackFiles(string filename, string directory, string fileFilter)
        {
            try
            {
                new FastZip { CreateEmptyDirectories = true }.CreateZip(filename, directory, false, fileFilter);
            }
            catch (Exception exception)
            {
                LoggerHelper.Except(exception, null);
            }
        }

        public static string PackList<T>(this List<T> list, char listSpriter = ',')
        {
            if (list.Count == 0)
            {
                return "";
            }
            StringBuilder builder = new StringBuilder();
            foreach (T local in list)
            {
                builder.AppendFormat("{0}{1}", local, listSpriter);
            }
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        public static string PackMap<T, U>(this IEnumerable<KeyValuePair<T, U>> map, char keyValueSpriter = ':', char mapSpriter = ',')
        {
            if (map.Count<KeyValuePair<T, U>>() == 0)
            {
                return "";
            }
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<T, U> pair in map)
            {
                builder.AppendFormat("{0}{1}{2}{3}", new object[] { pair.Key, keyValueSpriter, pair.Value, mapSpriter });
            }
            return builder.ToString().Remove(builder.Length - 1, 1);
        }

        public static bool ParseColor(string _inputString, out Color result)
        {
            string str = _inputString.Trim();
            result = Color.get_clear();
            if (str.Length < 9)
            {
                return false;
            }
            try
            {
                string[] strArray = str.Split(new char[] { ',' });
                if (strArray.Length != 4)
                {
                    return false;
                }
                result = new Color(float.Parse(strArray[0]) / 255f, float.Parse(strArray[1]) / 255f, float.Parse(strArray[2]) / 255f, float.Parse(strArray[3]) / 255f);
                return true;
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Parse Color error: " + str + exception.ToString(), true);
                return false;
            }
        }

        public static List<string> ParseList(this string strList, char listSpriter = ',')
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(strList))
            {
                string str = strList.Trim();
                if (string.IsNullOrEmpty(strList))
                {
                    return list;
                }
                string[] strArray = str.Split(new char[] { listSpriter });
                foreach (string str2 in strArray)
                {
                    if (!string.IsNullOrEmpty(str2))
                    {
                        list.Add(str2.Trim());
                    }
                }
            }
            return list;
        }

        public static List<T> ParseListAny<T>(this string strList, char listSpriter = ',')
        {
            Type type = typeof(T);
            List<string> list = strList.ParseList(listSpriter);
            List<T> list2 = new List<T>();
            foreach (string str in list)
            {
                list2.Add((T) GetValue(str, type));
            }
            return list2;
        }

        public static bool ParseLuaTable<T>(LuaTable luaTable, out T result)
        {
            object obj2;
            if (ParseLuaTable(luaTable, typeof(T), out obj2))
            {
                result = (T) obj2;
                return true;
            }
            result = default(T);
            return false;
        }

        public static bool ParseLuaTable(LuaTable luaTable, Type type, out object result)
        {
            result = null;
            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    DecodeDic(luaTable, type, out result);
                }
                else if (type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    DecodeList(luaTable, type, out result);
                }
            }
            else
            {
                DecodeEntity(luaTable, type, out result);
            }
            return true;
        }

        public static bool ParseLuaTable(string inputString, Type type, out object result)
        {
            string str = inputString.Trim();
            if ((str[0] != '{') || (str[str.Length - 1] != '}'))
            {
                result = null;
                return false;
            }
            if (str.Length == 2)
            {
                result = type.GetConstructor(Type.EmptyTypes).Invoke(null);
                return true;
            }
            int index = 0;
            return DecodeDic(str, type, ref index, out result);
        }

        public static Dictionary<string, string> ParseMap(this string strMap, char keyValueSpriter = ':', char mapSpriter = ',')
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(strMap))
            {
                string[] strArray = strMap.Split(new char[] { mapSpriter });
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (!string.IsNullOrEmpty(strArray[i]))
                    {
                        string[] strArray2 = strArray[i].Split(new char[] { keyValueSpriter });
                        if (strArray2.Length == 2)
                        {
                            if (!dictionary.ContainsKey(strArray2[0]))
                            {
                                dictionary.Add(strArray2[0], strArray2[1]);
                            }
                            else
                            {
                                LoggerHelper.Warning(string.Format("Key {0} already exist, index {1} of {2}.", strArray2[0], i, strMap), true);
                            }
                        }
                        else
                        {
                            LoggerHelper.Warning(string.Format("KeyValuePair are not match: {0}, index {1} of {2}.", strArray[i], i, strMap), true);
                        }
                    }
                }
            }
            return dictionary;
        }

        public static Dictionary<T, U> ParseMapAny<T, U>(this string strMap, char keyValueSpriter = ':', char mapSpriter = ',')
        {
            Type type = typeof(T);
            Type type2 = typeof(U);
            Dictionary<T, U> dictionary = new Dictionary<T, U>();
            Dictionary<string, string> dictionary2 = strMap.ParseMap(keyValueSpriter, mapSpriter);
            foreach (KeyValuePair<string, string> pair in dictionary2)
            {
                try
                {
                    T key = (T) GetValue(pair.Key, type);
                    U local2 = (U) GetValue(pair.Value, type2);
                    dictionary.Add(key, local2);
                }
                catch (Exception)
                {
                    LoggerHelper.Warning(string.Format("Parse failure: {0}, {1}", pair.Key, pair.Value), true);
                }
            }
            return dictionary;
        }

        public static Dictionary<int, float> ParseMapIntFloat(this string strMap, char keyValueSpriter = ':', char mapSpriter = ',')
        {
            Dictionary<int, float> dictionary = new Dictionary<int, float>();
            Dictionary<string, string> dictionary2 = strMap.ParseMap(keyValueSpriter, mapSpriter);
            foreach (KeyValuePair<string, string> pair in dictionary2)
            {
                int num;
                float num2;
                if (int.TryParse(pair.Key, out num) && float.TryParse(pair.Value, out num2))
                {
                    dictionary.Add(num, num2);
                }
                else
                {
                    LoggerHelper.Warning(string.Format("Parse failure: {0}, {1}", pair.Key, pair.Value), true);
                }
            }
            return dictionary;
        }

        public static Dictionary<int, int> ParseMapIntInt(this string strMap, char keyValueSpriter = ':', char mapSpriter = ',')
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            Dictionary<string, string> dictionary2 = strMap.ParseMap(keyValueSpriter, mapSpriter);
            foreach (KeyValuePair<string, string> pair in dictionary2)
            {
                int num;
                int num2;
                if (int.TryParse(pair.Key, out num) && int.TryParse(pair.Value, out num2))
                {
                    dictionary.Add(num, num2);
                }
                else
                {
                    LoggerHelper.Warning(string.Format("Parse failure: {0}, {1}", pair.Key, pair.Value), true);
                }
            }
            return dictionary;
        }

        public static Dictionary<int, string> ParseMapIntString(this string strMap, char keyValueSpriter = ':', char mapSpriter = ',')
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            Dictionary<string, string> dictionary2 = strMap.ParseMap(keyValueSpriter, mapSpriter);
            foreach (KeyValuePair<string, string> pair in dictionary2)
            {
                int num;
                if (int.TryParse(pair.Key, out num))
                {
                    dictionary.Add(num, pair.Value);
                }
                else
                {
                    LoggerHelper.Warning(string.Format("Parse failure: {0}", pair.Key), true);
                }
            }
            return dictionary;
        }

        public static Dictionary<string, float> ParseMapStringFloat(this string strMap, char keyValueSpriter = ':', char mapSpriter = ',')
        {
            Dictionary<string, float> dictionary = new Dictionary<string, float>();
            Dictionary<string, string> dictionary2 = strMap.ParseMap(keyValueSpriter, mapSpriter);
            foreach (KeyValuePair<string, string> pair in dictionary2)
            {
                float num;
                if (float.TryParse(pair.Value, out num))
                {
                    dictionary.Add(pair.Key, num);
                }
                else
                {
                    LoggerHelper.Warning(string.Format("Parse failure: {0}", pair.Value), true);
                }
            }
            return dictionary;
        }

        public static Dictionary<string, int> ParseMapStringInt(this string strMap, char keyValueSpriter = ':', char mapSpriter = ',')
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            Dictionary<string, string> dictionary2 = strMap.ParseMap(keyValueSpriter, mapSpriter);
            foreach (KeyValuePair<string, string> pair in dictionary2)
            {
                int num;
                if (int.TryParse(pair.Value, out num))
                {
                    dictionary.Add(pair.Key, num);
                }
                else
                {
                    LoggerHelper.Warning(string.Format("Parse failure: {0}", pair.Value), true);
                }
            }
            return dictionary;
        }

        public static bool ParseQuaternion(string _inputString, out Quaternion result)
        {
            string str = _inputString.Trim();
            result = new Quaternion();
            if (str.Length < 9)
            {
                return false;
            }
            try
            {
                string[] strArray = str.Split(new char[] { ',' });
                if (strArray.Length != 4)
                {
                    return false;
                }
                result.x = float.Parse(strArray[0]);
                result.y = float.Parse(strArray[1]);
                result.z = float.Parse(strArray[2]);
                result.w = float.Parse(strArray[3]);
                return true;
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Parse Quaternion error: " + str + exception.ToString(), true);
                return false;
            }
        }

        public static bool ParseVector3(string _inputString, out Vector3 result)
        {
            string str = _inputString.Trim();
            result = new Vector3();
            if (str.Length < 7)
            {
                return false;
            }
            try
            {
                string[] strArray = str.Split(new char[] { ',' });
                if (strArray.Length != 3)
                {
                    return false;
                }
                result.x = float.Parse(strArray[0]);
                result.y = float.Parse(strArray[1]);
                result.z = float.Parse(strArray[2]);
                return true;
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Parse Vector3 error: " + str + exception.ToString(), true);
                return false;
            }
        }

        public static string PathNormalize(this string str)
        {
            return str.Replace(@"\", "/").ToLower();
        }

        public static string ReplaceFirst(this string input, string oldValue, string newValue, int startAt = 0)
        {
            int index = input.IndexOf(oldValue, startAt);
            if (index < 0)
            {
                return input;
            }
            return (input.Substring(0, index) + newValue + input.Substring(index + oldValue.Length));
        }

        public static void SendPostHttp(string Url, string datastr, Action<string> onDone, Action<HttpStatusCode> onFail)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(datastr);
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(Url);
            request.Method = "Post";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bytes.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            uint timerId = TimerHeap.AddTimer(0x3a98, 0, () => onFail(HttpStatusCode.RequestTimeout));
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                TimerHeap.DelTimer(timerId);
                onFail(response.StatusCode);
            }
            else
            {
                TimerHeap.DelTimer(timerId);
                Stream responseStream = response.GetResponseStream();
                Encoding encoding = Encoding.UTF8;
                StreamReader reader = new StreamReader(responseStream, encoding);
                char[] buffer = new char[0x100];
                int length = reader.Read(buffer, 0, 0x100);
                StringBuilder builder = new StringBuilder("");
                while (length > 0)
                {
                    string str = new string(buffer, 0, length);
                    builder.Append(str);
                    length = reader.Read(buffer, 0, 0x100);
                }
                response.Close();
                reader.Close();
                onDone(builder.ToString());
            }
        }

        public static bool UnpackFiles(string file, string dir)
        {
            try
            {
                ZipEntry entry;
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                ZipInputStream stream = new ZipInputStream(File.OpenRead(file));
                while ((entry = stream.GetNextEntry()) != null)
                {
                    bool flag2;
                    string directoryName = Path.GetDirectoryName(entry.Name);
                    string fileName = Path.GetFileName(entry.Name);
                    if (directoryName != string.Empty)
                    {
                        Directory.CreateDirectory(dir + directoryName);
                    }
                    if (!(fileName != string.Empty))
                    {
                        continue;
                    }
                    FileStream stream2 = File.Create(dir + entry.Name);
                    int count = 0x800;
                    byte[] buffer = new byte[0x800];
                    goto Label_00D0;
                Label_009D:
                    count = stream.Read(buffer, 0, buffer.Length);
                    if (count > 0)
                    {
                        stream2.Write(buffer, 0, count);
                    }
                    else
                    {
                        goto Label_00D5;
                    }
                Label_00D0:
                    flag2 = true;
                    goto Label_009D;
                Label_00D5:
                    stream2.Close();
                }
                stream.Close();
                return true;
            }
            catch (Exception exception)
            {
                LoggerHelper.Except(exception, null);
                return false;
            }
        }

        public static byte[] UnpackMemory(byte[] zipMemory)
        {
            ZipEntry entry;
            MemoryStream baseInputStream = new MemoryStream(zipMemory);
            baseInputStream.Seek(0L, SeekOrigin.Begin);
            ZipInputStream stream2 = new ZipInputStream(baseInputStream);
            List<byte> list = new List<byte>();
            while ((entry = stream2.GetNextEntry()) != null)
            {
                bool flag;
                if (!(Path.GetFileName(entry.Name) != string.Empty))
                {
                    continue;
                }
                int length = 0x800;
                byte[] buffer = new byte[0x800];
                goto Label_00A1;
            Label_005D:
                length = stream2.Read(buffer, 0, buffer.Length);
                if (length > 0)
                {
                    byte[] destinationArray = new byte[length];
                    Array.Copy(buffer, destinationArray, length);
                    list.AddRange(destinationArray);
                }
                else
                {
                    continue;
                }
            Label_00A1:
                flag = true;
                goto Label_005D;
            }
            stream2.Close();
            baseInputStream.Close();
            return list.ToArray();
        }
    }
}

