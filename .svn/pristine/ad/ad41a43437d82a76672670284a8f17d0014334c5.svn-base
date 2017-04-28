
using System;
using System.Text.RegularExpressions;

public class Stringer
{
    /// <summary>
    /// 数字返回 True, 其它返回 False
    /// <para>数字前面有一个 +号 或 -号 返回 True</para>
    /// </summary>
    static public bool IsNumber(string inStr)
    {
        return new Regex(@"^[+-]?\d*(,\d{3})*(\.\d+)?$").IsMatch(inStr.Trim());
    }

    /// <summary>
    /// 首字母大写
    /// </summary>
    static public string FirstLetterUp(string s)
    {
        return s.Substring(0, 1).ToUpper() + s.Substring(1);
    }

    /// <summary>
    /// 首字母小写
    /// </summary>
    static public string FirstLetterLower(string inStr)
    {
        return inStr.Substring(0, 1).ToLower() + inStr.Substring(1);
    }

    /// <summary>
    /// 转 Type
    /// </summary>
    //static public Type ToType(string inStr)
    //{
    //    Type t = typeof(System.String);
    //    if (inStr.Contains("+"))// Excel 类型结构 eg.a+i+1
    //    {
    //        string[] ss = inStr.Split('+');
    //        if (ss[1] == "i")
    //        {
    //            t = typeof(System.UInt32);
    //        }
    //        else if (ss[1] == "f")
    //        {
    //            t = typeof(System.Single);
    //        }
    //        else if (ss[1] == "t" || ss[1] == "s")
    //        {

    //        }
    //        else
    //        {
    //            t = Type.GetType(ss[1]);
    //        }
    //    }
    //    else
    //    {
    //        if (inStr != "" && IsNumber(inStr))
    //        {
    //            t = inStr.Contains(".") ? typeof(System.Single) : typeof(System.UInt32);
    //        }
    //    }
    //    return t;
    //}

    static public Type ToType(string inStr)
    {
        Type t = typeof(System.String);

        switch (inStr)
        {
            case "int":
                t = typeof(System.Int32);
                break;
            case "string":
                t = typeof(System.String);
                break;
            case "float":
                t = typeof(System.Single);
                break;
            default:
                break;
        }
        return t;
    }

    /// <summary>
    /// 字符串转枚举
    /// </summary>
    static public T StrToEnum<T>(string inStr, T inType)
    {
        if (!Enum.IsDefined(typeof(T), inStr))
        {
            return inType;
        }
        return (T)Enum.Parse(typeof(T), inStr);
    }

    /// <summary>
    /// string -> Type
    /// </summary>
    /// <param name="s">字段名</param>
    /// <param name="isSelfDefine">是否为自定义类型（eg. vector3unit）</param>
    /// <returns></returns>
    static public Type GetFieldType(string s, ref bool isSelfDefine)
    {
        string b = s == "float" ? "System.Single" :
                   s == "float[]" ? "System.Single[]" :
                   s == "int32" ? "System.Int32" :
                   s == "int32[]" ? "System.Int32[]" :
                   s == "uint32" ? "System.UInt32" :
                   s == "uint32[]" ? "System.UInt32[]" :
                   s == "int64" ? "System.Int64" :
                   s == "int64[]" ? "System.Int64[]" :
                   s == "uint64" ? "System.UInt64" :
                   s == "uint64[]" ? "System.UInt64[]" :
                   s == "string" ? "System.String" :
                   s == "string[]" ? "System.String[]" :
                   s == "short" || s == "int8" || s == "byte" ? "System.Byte" :
                   s == "short[]" || s == "int8[]" || s == "byte[]" ? "System.Byte[]" : s;
        isSelfDefine = b == s;
        return Type.GetType(b);
    }

    /// <summary>
    /// string -> 转byte[]的方法名
    /// </summary>
    /// <param name="s">类型名</param>
    /// <returns></returns>
    static public string GetTobytesMethodName(string s)
    {
        string b = s == "float" || s == "System.Single" ? "ReadFloat()" :
                   s == "int16" || s == "short" || s == "System.Int16" ? "ReadUnsignedShort()" :
                   s == "int32" || s == "System.Int32" ? "ReadInt()" :
                   s == "int64" || s == "System.Int64" ? "ReadInt64()" :
                   s == "uint32" || s == "System.UInt32" ? "ReadUnsignedInt32()" :
                   s == "uint64" || s == "System.UInt64" ? "ReadUnsignedInt64()" :
                   s == "string" || s == "System.String" ? "ReadString()" :
                   s == "byte" || s == "int8" || s == "System.SByte" ? "ReadByte()" : "";
        return b;
    }
}