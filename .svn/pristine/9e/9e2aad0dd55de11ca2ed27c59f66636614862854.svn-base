namespace Mogo.RPC
{
    using Mogo.Util;
    using MsgPack;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;

    public class Utils
    {
        private Utils()
        {
        }

        public static bool DecodeKey(byte[] inputString, ref int index, out string key, out bool isString)
        {
            if (inputString[index] == 0x73)
            {
                int num = int.Parse(Encoding.UTF8.GetString(inputString, index + 1, 3));
                if (num > 0)
                {
                    index += 4;
                    key = Encoding.UTF8.GetString(inputString, index, num);
                    isString = true;
                    index += num;
                    return true;
                }
                key = "";
                isString = true;
                LoggerHelper.Error(string.Concat(new object[] { "Decode Lua Table Key Error: ", (int) index, " ", inputString }), true);
                return false;
            }
            int count = 0;
            while (((index + count) < inputString.Length) && (inputString[index + count] != 0x3d))
            {
                count++;
            }
            if (count > 0)
            {
                key = Encoding.UTF8.GetString(inputString, index, count);
                index += count;
                isString = false;
                return true;
            }
            key = "-1";
            isString = false;
            LoggerHelper.Error(string.Concat(new object[] { "Decode Lua Table Key Error: ", (int) index, " ", inputString }), true);
            return false;
        }

        public static bool DecodeKey(string inputString, ref int index, out string key, out bool isString)
        {
            int num;
            if (inputString[index] == 's')
            {
                num = int.Parse(inputString.Substring(index + 1, 3));
                if (num > 0)
                {
                    index += 4;
                    key = inputString.Substring(index, num);
                    isString = true;
                    index += num;
                    return true;
                }
                key = "";
                isString = true;
                LoggerHelper.Error(string.Concat(new object[] { "Decode Lua Table Key Error: ", (int) index, " ", inputString }), true);
                return false;
            }
            int num2 = inputString.IndexOf('=', index);
            if (num2 > -1)
            {
                num = num2 - index;
                key = inputString.Substring(index, num);
                index = num2;
                isString = false;
                return true;
            }
            key = "-1";
            isString = false;
            LoggerHelper.Error(string.Concat(new object[] { "Decode Lua Table Key Error: ", (int) index, " ", inputString }), true);
            return false;
        }

        private static bool DecodeLuaTable(string inputString, ref int index, out object result)
        {
            LuaTable table = new LuaTable();
            result = table;
            if (!WaitChar(inputString, '{', ref index))
            {
                return false;
            }
            try
            {
                if (!WaitChar(inputString, '}', ref index))
                {
                    while (index < inputString.Length)
                    {
                        string str;
                        bool flag;
                        object obj2;
                        DecodeKey(inputString, ref index, out str, out flag);
                        WaitChar(inputString, '=', ref index);
                        if (DecodeLuaValue(inputString, ref index, out obj2))
                        {
                            table.Add(str, flag, obj2);
                        }
                        if (!WaitChar(inputString, ',', ref index))
                        {
                            break;
                        }
                    }
                    WaitChar(inputString, '}', ref index);
                }
                return true;
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Parse LuaTable error: " + inputString + exception.ToString(), true);
                return false;
            }
        }

        private static bool DecodeLuaTable(byte[] inputString, ref int index, out object result)
        {
            LuaTable table = new LuaTable();
            result = table;
            if (!WaitChar(inputString, '{', ref index))
            {
                return false;
            }
            try
            {
                if (!WaitChar(inputString, '}', ref index))
                {
                    while (index < inputString.Length)
                    {
                        string str;
                        bool flag;
                        object obj2;
                        DecodeKey(inputString, ref index, out str, out flag);
                        WaitChar(inputString, '=', ref index);
                        if (DecodeLuaValue(inputString, ref index, out obj2))
                        {
                            table.Add(str, flag, obj2);
                        }
                        if (!WaitChar(inputString, ',', ref index))
                        {
                            break;
                        }
                    }
                    WaitChar(inputString, '}', ref index);
                }
                return true;
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("Parse LuaTable error: " + inputString + exception.ToString(), true);
                return false;
            }
        }

        private static bool DecodeLuaValue(string inputString, ref int index, out object value)
        {
            int num2;
            switch (inputString[index])
            {
                case 's':
                {
                    int length = int.Parse(inputString.Substring(index + 1, 3));
                    index += 4;
                    if (length > 0)
                    {
                        value = inputString.Substring(index, length);
                        index += length;
                        return true;
                    }
                    value = "";
                    return true;
                }
                case '{':
                    return DecodeLuaTable(inputString, ref index, out value);

                default:
                    num2 = index;
                    break;
            }
        Label_00D7:
            if ((index += 1) < inputString.Length)
            {
                if (((inputString[index] == ',') || (inputString[index] == '}')) && (index > num2))
                {
                    value = inputString.Substring(num2, index - num2);
                    return true;
                }
                goto Label_00D7;
            }
            LoggerHelper.Error(string.Concat(new object[] { "Decode Lua Table Value Error: ", (int) index, " ", inputString }), true);
            value = null;
            return false;
        }

        private static bool DecodeLuaValue(byte[] inputString, ref int index, out object value)
        {
            int num3;
            switch (inputString[index])
            {
                case 0x73:
                {
                    int count = int.Parse(Encoding.UTF8.GetString(inputString, index + 1, 3));
                    index += 4;
                    if (count > 0)
                    {
                        value = Encoding.UTF8.GetString(inputString, index, count);
                        index += count;
                        return true;
                    }
                    value = "";
                    return true;
                }
                case 0x7b:
                    return DecodeLuaTable(inputString, ref index, out value);

                default:
                    num3 = index;
                    break;
            }
        Label_00DA:
            if ((index += 1) < inputString.Length)
            {
                if (((inputString[index] == 0x2c) || (inputString[index] == 0x7d)) && (index > num3))
                {
                    value = Encoding.UTF8.GetString(inputString, num3, index - num3);
                    return true;
                }
                goto Label_00DA;
            }
            LoggerHelper.Error(string.Concat(new object[] { "Decode Lua Table Value Error: ", (int) index, " ", inputString }), true);
            value = null;
            return false;
        }

        private static string EncodeString(string value)
        {
            if (value.Length > 0x3e7)
            {
                LoggerHelper.Warning("PackLuaTable EncodeString overflow: " + value, true);
            }
            int length = Encoding.UTF8.GetBytes(value).Length;
            return ('s' + length.ToString("000") + value);
        }

        public static byte[] FillLengthHead(byte[] srcData)
        {
            return FillLengthHead(srcData, (ushort) srcData.Length);
        }

        public static byte[] FillLengthHead(byte[] srcData, ushort length)
        {
            byte[] bytes = BitConverter.GetBytes(length);
            byte[] dst = new byte[length + bytes.Length];
            Buffer.BlockCopy(bytes, 0, dst, 0, bytes.Length);
            Buffer.BlockCopy(srcData, 0, dst, bytes.Length, length);
            return dst;
        }

        private static bool IsBaseType(Type type)
        {
            return ((((((type == typeof(byte)) || (type == typeof(sbyte))) || ((type == typeof(short)) || (type == typeof(ushort)))) || (((type == typeof(int)) || (type == typeof(uint))) || ((type == typeof(long)) || (type == typeof(ulong))))) || (((type == typeof(float)) || (type == typeof(double))) || (type == typeof(string)))) || (type == typeof(bool)));
        }

        public static bool MsgPackLuaTable(ref LuaTable luatable, ref Packer pker)
        {
            if (luatable == null)
            {
                pker = null;
                return false;
            }
            pker.PackMapHeader(luatable.Count);
            foreach (KeyValuePair<string, object> pair in luatable)
            {
                if (luatable.IsKeyString(pair.Key))
                {
                    pker.PackRawHeader(pair.Key.Length);
                    pker.PackRawBody(Encoding.UTF8.GetBytes(pair.Key));
                }
                else
                {
                    pker.Pack(double.Parse(pair.Key.ToString()));
                }
                Type type = pair.Value.GetType();
                if (type == typeof(string))
                {
                    pker.PackRawHeader(pair.Value.ToString().Length);
                    pker.PackRawBody(Encoding.UTF8.GetBytes(pair.Value.ToString()));
                }
                else if (type == typeof(LuaTable))
                {
                    LuaTable table = pair.Value as LuaTable;
                    MsgPackLuaTable(ref table, ref pker);
                }
                else if (type == typeof(bool))
                {
                    pker.Pack(bool.Parse(pair.Value.ToString()));
                }
                else
                {
                    pker.Pack(double.Parse(pair.Value.ToString()));
                }
            }
            return true;
        }

        public static bool MsgUnPackTable(out LuaTable luatable, ref MessagePackObject pObj)
        {
            LuaTable table = new LuaTable();
            luatable = table;
            MessagePackObjectDictionary dictionary = pObj.AsDictionary();
            bool isString = false;
            foreach (KeyValuePair<MessagePackObject, MessagePackObject> pair in dictionary)
            {
                string str;
                object obj2;
                MessagePackObject key = pair.Key;
                if (key.IsRaw)
                {
                    str = key.AsString();
                    isString = true;
                }
                else if (key.IsTypeOf<double>() == true)
                {
                    str = key.AsDouble().ToString();
                }
                else
                {
                    LoggerHelper.Error("key type error", true);
                    return false;
                }
                MessagePackObject obj4 = pair.Value;
                if (obj4.IsRaw)
                {
                    obj2 = obj4.AsString();
                }
                else if (obj4.IsDictionary)
                {
                    LuaTable table2;
                    MsgUnPackTable(out table2, ref obj4);
                    obj2 = table2;
                }
                else if (obj4.IsTypeOf<bool>() == true)
                {
                    obj2 = obj4.AsBoolean();
                }
                else if (obj4.IsTypeOf<double>() == true)
                {
                    obj2 = obj4.AsDouble();
                }
                else
                {
                    LoggerHelper.Error("value type error", true);
                    return false;
                }
                table.Add(str, isString, obj2);
                isString = false;
            }
            return true;
        }

        public static string PackLuaTable(LuaTable luaTable)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('{');
            if (luaTable != null)
            {
                foreach (KeyValuePair<string, object> pair in luaTable)
                {
                    if (luaTable.IsKeyString(pair.Key))
                    {
                        builder.Append(EncodeString(pair.Key));
                    }
                    else
                    {
                        builder.Append(pair.Key);
                    }
                    builder.Append('=');
                    Type type = pair.Value.GetType();
                    if (type == typeof(string))
                    {
                        builder.Append(EncodeString(pair.Value as string));
                    }
                    else if (type == typeof(LuaTable))
                    {
                        builder.Append(PackLuaTable(pair.Value as LuaTable));
                    }
                    else
                    {
                        builder.Append(pair.Value.ToString());
                    }
                    builder.Append(',');
                }
                if (luaTable.Count != 0)
                {
                    builder.Remove(builder.Length - 1, 1);
                }
            }
            builder.Append('}');
            return builder.ToString();
        }

        public static bool PackLuaTable(IDictionary target, out LuaTable result)
        {
            Type[] genericArguments = target.GetType().GetGenericArguments();
            result = new LuaTable();
            try
            {
                foreach (DictionaryEntry entry in target)
                {
                    if (IsBaseType(genericArguments[1]))
                    {
                        object obj2;
                        if (genericArguments[1] == typeof(bool))
                        {
                            obj2 = ((bool) entry.Value) ? 1 : 0;
                        }
                        else
                        {
                            obj2 = entry.Value;
                        }
                        if (genericArguments[0] == typeof(int))
                        {
                            result.Add(entry.Key.ToString(), false, obj2);
                        }
                        else
                        {
                            result.Add(entry.Key.ToString(), obj2);
                        }
                    }
                    else
                    {
                        LuaTable table;
                        if (PackLuaTable(entry.Value, out table))
                        {
                            if (genericArguments[0] == typeof(int))
                            {
                                result.Add(entry.Key.ToString(), false, table);
                            }
                            else
                            {
                                result.Add(entry.Key.ToString(), table);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("PackLuaTable dictionary error: " + exception.Message, true);
            }
            return true;
        }

        public static bool PackLuaTable(IList target, out LuaTable result)
        {
            Type[] genericArguments = target.GetType().GetGenericArguments();
            result = new LuaTable();
            try
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (IsBaseType(genericArguments[0]))
                    {
                        if (genericArguments[0] == typeof(bool))
                        {
                            result.Add((int) (i + 1), ((bool) target[i]) ? 1 : 0);
                        }
                        else
                        {
                            result.Add((int) (i + 1), target[i]);
                        }
                    }
                    else
                    {
                        LuaTable table;
                        if (PackLuaTable(target[i], out table))
                        {
                            result.Add((int) (i + 1), table);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("PackLuaTable list error: " + exception.Message, true);
            }
            return true;
        }

        public static bool PackLuaTable(object target, out LuaTable result)
        {
            Type type = target.GetType();
            if (type == typeof(LuaTable))
            {
                result = target as LuaTable;
                return true;
            }
            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    return PackLuaTable(target as IDictionary, out result);
                }
                return PackLuaTable(target as IList, out result);
            }
            result = new LuaTable();
            try
            {
                PropertyInfo[] properties = type.GetProperties(~BindingFlags.Static);
                for (int i = 0; i < properties.Length; i++)
                {
                    PropertyInfo info = properties[i];
                    if (IsBaseType(info.PropertyType))
                    {
                        result.Add((int) (i + 1), info.GetGetMethod().Invoke(target, null));
                    }
                    else
                    {
                        LuaTable table;
                        if (PackLuaTable(info.GetGetMethod().Invoke(target, null), out table))
                        {
                            result.Add((int) (i + 1), table);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Error("PackLuaTable entity error: " + exception.Message, true);
            }
            return true;
        }

        public static bool ParseLuaTable(string inputString, out LuaTable result)
        {
            object obj2;
            string str = inputString.Trim();
            if ((str[0] != '{') || (str[str.Length - 1] != '}'))
            {
                result = null;
                return false;
            }
            if (str.Length == 2)
            {
                result = new LuaTable();
                return true;
            }
            int index = 0;
            bool flag = DecodeLuaTable(inputString, ref index, out obj2);
            if (flag)
            {
                result = obj2 as LuaTable;
            }
            else
            {
                result = null;
            }
            return flag;
        }

        public static bool ParseLuaTable(byte[] inputString, out LuaTable result)
        {
            object obj2;
            if ((inputString[0] != 0x7b) || (inputString[inputString.Length - 1] != 0x7d))
            {
                result = null;
                return false;
            }
            if (inputString.Length == 2)
            {
                result = new LuaTable();
                return true;
            }
            int index = 0;
            bool flag = DecodeLuaTable(inputString, ref index, out obj2);
            if (flag)
            {
                result = obj2 as LuaTable;
            }
            else
            {
                result = null;
            }
            return flag;
        }

        public static bool WaitChar(string inputString, char c, ref int index)
        {
            if (inputString.IndexOf(c, index) == index)
            {
                index++;
                return true;
            }
            return false;
        }

        public static bool WaitChar(byte[] inputString, char c, ref int index)
        {
            if (((byte) c) == inputString[index])
            {
                index++;
                return true;
            }
            return false;
        }
    }
}

