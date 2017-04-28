using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class MyTools
{
    #region Parse
    public static string Int2IP(uint ipCode)
    {
        byte a = (byte)((ipCode & 0xFF000000) >> 24);
        byte b = (byte)((ipCode & 0x00FF0000) >> 16);
        byte c = (byte)((ipCode & 0x0000FF00) >> 8);
        byte d = (byte)(ipCode & 0x000000FF);
        string ipStr = string.Format("{0}.{1}.{2}.{3}", d, c, b, a);
        return ipStr;
    }
    public static uint IP2Int(string ipStr)
    {
        string[] ip = ipStr.Split('.');
        uint ipCode = 0xFFFFFF00 | byte.Parse(ip[3]);
        ipCode = ipCode & 0xFFFF00FF | (uint.Parse(ip[2]) << 0x8);
        ipCode = ipCode & 0xFF00FFFF | (uint.Parse(ip[1]) << 0xF);
        ipCode = ipCode & 0x00FFFFFF | (uint.Parse(ip[0]) << 0x18);
        return ipCode;
    }
    /// <summary>
    /// 解析Quaternion
    /// </summary>
    /// <param name="_inputString"></param>
    /// <param name="result"></param>
    /// <returns></returns>
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
            Debug.LogError("Parse Quaternion error: " + str + exception.ToString());
            return false;
        }
    }

    public static Vector3 ParserVector3(string _PutIn)
    {
        Vector3 NewVec = new Vector3(0.0f, 0.0f, 0.0f);
        char[] delimiterChars = { ' ', ',', ':', '-', '\t' };
        string[] words = _PutIn.Split(delimiterChars);
        int index = 0;
        foreach (string s in words)
        {
            if (index == 0)
                NewVec.x = float.Parse(s);
            else if (index == 1)
                NewVec.y = float.Parse(s);
            else if (index == 2)
                NewVec.z = float.Parse(s);
            index++;
        }
        return NewVec;
    }

    public static Vector2 ParserVector2(string _PutIn)
    {
        Vector2 NewVec = new Vector2(0.0f, 0.0f);
        char[] delimiterChars = { ' ', ',', ':', '-', '\t' };
        string[] words = _PutIn.Split(delimiterChars);
        int ii = 0;
        foreach (string s in words)
        {
            if (ii == 0)
                NewVec.x = float.Parse(s);
            else if (ii == 1)
                NewVec.y = float.Parse(s);
            ii++;
        }
        return NewVec;
    }

    public static Color ParserColor(string _PutIn)
    {
        Color NewVec = new Color();
        char[] delimiterChars = { ' ', ',', ':', '-', '\t' };
        string[] words = _PutIn.Split(delimiterChars);
        int ii = 0;
        foreach (string s in words)
        {
            if (ii == 0)
                NewVec.a = float.Parse(s);
            else if (ii == 1)
                NewVec.r = float.Parse(s);
            else if (ii == 2)
                NewVec.g = float.Parse(s);
            else if (ii == 3)
                NewVec.b = float.Parse(s);
            ii++;
        }
        return NewVec;
    }

    public static List<int> ParserIntString(string _PutIn)
    {
        List<int> newList = new List<int>();
        char[] delimiterChars = { ' ', ',', ':', '-', '\t' };
        string[] words = _PutIn.Split(delimiterChars);
        foreach (string s in words)
        {
            newList.Add(int.Parse(s));
        }
        return newList;
    }
    public static List<float> ParserFloatString(string _PutIn)
    {
        List<float> newList = new List<float>();
        char[] delimiterChars = { ' ', ',', ':', '-', '\t' };
        string[] words = _PutIn.Split(delimiterChars);
        foreach (string s in words)
        {
            newList.Add(float.Parse(s));
        }
        return newList;
    }
    #endregion

    #region Math

    public static Vector2 DirRihgt = new Vector2(1.0f, 0.0f).normalized;
    public const int DirR = 1;
    public static Vector2 DirRightTop = new Vector2(1.0f, 1.0f).normalized;
    public const int DirRT = 2;
    public static Vector2 DirTop = new Vector2(0.0f, 1.0f).normalized;
    public const int DirT = 3;
    public static Vector2 DirLeftTop = new Vector2(-1.0f, 1.0f).normalized;
    public const int DirLT = 4;
    public static Vector2 DirLeft = new Vector2(-1.0f, 0.0f).normalized;
    public const int DirL = 5;
    public static Vector2 DirLeftBottom = new Vector2(-1.0f, -1.0f).normalized;
    public const int DirLB = 6;
    public static Vector2 DirBottom = new Vector2(0.0f, -1.0f).normalized;
    public const int DirB = 7;
    public static Vector2 DirRihgtBottom = new Vector2(1.0f, -1.0f).normalized;
    public const int DirRB = 8;

    public static Vector3 TDirRihgt = new Vector3(1.0f, 0.0f).normalized;
    public static Vector3 TDirRightTop = new Vector3(1.0f, 1.0f).normalized;
    public static Vector3 TDirTop = new Vector3(0.0f, 1.0f).normalized;
    public static Vector3 TDirLeftTop = new Vector3(-1.0f, 1.0f).normalized;
    public static Vector3 TDirLeft = new Vector3(-1.0f, 0.0f).normalized;
    public static Vector3 TDirLeftBottom = new Vector3(-1.0f, -1.0f).normalized;
    public static Vector3 TDirBottom = new Vector3(0.0f, -1.0f).normalized;
    public static Vector3 TDirRihgtBottom = new Vector3(1.0f, -1.0f).normalized;

    public static Vector2 Get4XDir(Vector2 Dir, out int IntDir)
    {
        Vector2 newdir = new Vector2(0.0f, 0.0f);
        float thr = 3.14f / 4f;
        if (Dir.x > 0.0f)
        {
            if (Dir.y >= -Mathf.Sin(thr) && Dir.y <= Mathf.Sin(thr))
            {
                newdir = DirRihgt;
                IntDir = DirR;
            }
            else
            {
                if (Dir.y > 0.0f)
                {
                    newdir = DirTop;
                    IntDir = DirT;
                }
                else
                {
                    newdir = DirBottom;
                    IntDir = DirB;
                }
            }
        }
        else if (Dir.x < 0.0f)
        {
            if (Dir.y >= -Mathf.Sin(thr) && Dir.y <= Mathf.Sin(thr))
            {
                newdir = DirLeft;
                IntDir = DirL;
            }
            else
            {
                if (Dir.y > 0.0f)
                {
                    newdir = DirTop;
                    IntDir = DirT;
                }
                else
                {
                    newdir = DirBottom;
                    IntDir = DirB;
                }
            }
        }
        else
        {
            if (Dir.y > 0.0f)
            {
                newdir = DirTop;
                IntDir = DirT;
            }
            else
            {
                newdir = DirBottom;
                IntDir = DirB;
            }
        }
        return newdir;
    }
    public static Vector2 Get8XDir(Vector2 Dir, out int IntDir)
    {
        Vector2 newdir = new Vector2(0.0f, 0.0f);
        float thr = 3.14f / 8f;
        if (Dir.x > 0.0f)
        {
            if (Dir.y >= -Mathf.Sin(thr) && Dir.y <= Mathf.Sin(thr))
            {
                newdir = DirRihgt;
                IntDir = DirR;
            }
            else if (Dir.y >= -Mathf.Sin(thr * 3) && Dir.y <= Mathf.Sin(thr * 3))
            {
                if (Dir.y > 0.0f)
                {
                    newdir = DirRightTop;
                    IntDir = DirRT;
                }
                else
                {
                    newdir = DirRihgtBottom;
                    IntDir = DirRB;
                }
            }
            else
            {
                if (Dir.y > 0.0f)
                {
                    newdir = DirTop;
                    IntDir = DirT;
                }
                else
                {
                    newdir = DirBottom;
                    IntDir = DirB;
                }
            }
        }
        else if (Dir.x < 0.0f)
        {
            if (Dir.y >= -Mathf.Sin(thr) && Dir.y <= Mathf.Sin(thr))
            {
                newdir = DirLeft;
                IntDir = DirL;
            }
            else if (Dir.y >= -Mathf.Sin(thr * 3) && Dir.y <= Mathf.Sin(thr * 3))
            {
                if (Dir.y > 0.0f)
                {
                    newdir = DirLeftTop;
                    IntDir = DirLT;
                }
                else
                {
                    newdir = DirLeftBottom;
                    IntDir = DirLB;
                }
                newdir = newdir.normalized;
            }
            else
            {
                if (Dir.y > 0.0f)
                {
                    newdir = DirTop;
                    IntDir = DirLT;
                }
                else
                {
                    newdir = DirBottom;
                    IntDir = DirB;
                }
            }
        }
        else
        {
            if (Dir.y > 0.0f)
            {
                newdir = DirTop;
                IntDir = DirT;
            }
            else
            {
                newdir = DirBottom;
                IntDir = DirB;
            }
        }
        return newdir;
    }

    /// <summary>
    /// 叉乘
    /// </summary>
    /// <param name="pV1"></param>
    /// <param name="pV2"></param>
    /// <returns></returns>
    public static Vector3 D3DXVec3Cross(Vector3 pV1, Vector3 pV2)
    {
        Vector3 v;
        v.x = pV1.y * pV2.z - pV1.z * pV2.y;
        v.y = pV1.z * pV2.x - pV1.x * pV2.z;
        v.z = pV1.x * pV2.y - pV1.y * pV2.x;
        return v;
    }


    //是否在三角形内
    private static float triangleArea(float v0x, float v0y, float v1x, float v1y, float v2x, float v2y)
    {
        return Mathf.Abs((v0x * v1y + v1x * v2y + v2x * v0y
            - v1x * v0y - v2x * v1y - v0x * v2y) / 2f);
    }
    public static bool isINTriangle(Vector3 point, Vector3 v0, Vector3 v1, Vector3 v2)
    {
        float x = point.x;
        float y = point.z;

        float v0x = v0.x;
        float v0y = v0.z;

        float v1x = v1.x;
        float v1y = v1.z;

        float v2x = v2.x;
        float v2y = v2.z;

        float t = triangleArea(v0x, v0y, v1x, v1y, v2x, v2y);
        float a = triangleArea(v0x, v0y, v1x, v1y, x, y) + triangleArea(v0x, v0y, x, y, v2x, v2y) + triangleArea(x, y, v1x, v1y, v2x, v2y);

        if (Mathf.Abs(t - a) <= 0.01f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //是否在矩形内
    private static float Multiply(float p1x, float p1y, float p2x, float p2y, float p0x, float p0y)
    {
        return ((p1x - p0x) * (p2y - p0y) - (p2x - p0x) * (p1y - p0y));
    }
    public static bool isINRect(Vector3 point, Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        float x = point.x;
        float y = point.z;

        float v0x = v0.x;
        float v0y = v0.z;

        float v1x = v1.x;
        float v1y = v1.z;

        float v2x = v2.x;
        float v2y = v2.z;

        float v3x = v3.x;
        float v3y = v3.z;

        if (Multiply(x, y, v0x, v0y, v1x, v1y) * Multiply(x, y, v3x, v3y, v2x, v2y) <= 0 && Multiply(x, y, v3x, v3y, v0x, v0y) * Multiply(x, y, v2x, v2y, v1x, v1y) <= 0)
            return true;
        else
            return false;

    }

    #endregion

    #region Date
    /// <summary>
    /// 获取时间
    /// </summary>
    /// <returns>距1970.1.1相差的秒数</returns>
    public static long GetTime()
    {
        TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
        return (long)ts.TotalMilliseconds;
    }
    #endregion

    #region 文件
    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="path">文件创建目录</param>
    /// <param name="filename">文件的名称</param>
    /// <param name="info">写入的内容</param>
    public static void CreateFile(string path, string filename, string info)
    {
        DeleteFile(path, filename);
        FileStream fs = new FileStream(path + filename, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(info.Trim());
        sw.Flush();
        sw.Close();
        fs.Close();
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="path">删除文件的路径</param>
    /// <param name="name">删除文件的名称</param>
    public static void DeleteFile(string path, string name)
    {
        if (File.Exists(path + name))
            File.Delete(path + name);
    }

    public static string ReadStringFromFile(string fileName)
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
    public static byte[] ReadByteFromFile(string fileName)
    {
        if (File.Exists(fileName))
        {
            return File.ReadAllBytes(fileName);
        }
        return null;
    }
    #endregion

    #region MD5
    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    public static string md5file(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }

    /// <summary>
    /// 计算字符串的MD5值
    /// </summary>
    public static string md5(string source)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
        md5.Clear();

        string destString = "";
        for (int i = 0; i < md5Data.Length; i++)
        {
            destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
        }
        destString = destString.PadLeft(32, '0');
        return destString;
    }

    /// <summary>
    /// HashToMD5Hex
    /// </summary>
    public static string HashToMD5Hex(string sourceStr)
    {
        byte[] Bytes = Encoding.UTF8.GetBytes(sourceStr);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] result = md5.ComputeHash(Bytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
                builder.Append(result[i].ToString("x2"));
            return builder.ToString();
        }
    }
    #endregion

    #region 匹配类型(eg. 数字 邮箱 电话...)

    private static Regex RegUint = new Regex("^[0-9]+$");
    private static Regex RegInt = new Regex("^[+-]?[0-9]+$");
    private static Regex RegUfloat = new Regex("^[0-9]+[.]?[0-9]+$");
    private static Regex RegFloat = new Regex("^[+-]?[0-9]+[.]?[0-9]+$");
    private static Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");
    private static Regex RegChinese = new Regex("[\u4e00-\u9fa5]");
    private static Regex RegTellPhone = new Regex("^(([0-9]{3,4}-)|[0-9]{3.4}-)?[0-9]{7,8}$");
    private static Regex RegMobilePhone = new Regex("^13|15|18[0-9]{9}$");
    private static Regex RegSend = new Regex("[0-9]{1}([0-9]+){5}");
    private static Regex RegURL = new Regex("^[a-zA-z]+://(\\w+(-\\w+)*)(\\.(\\w+(-\\w+)*))*(\\?\\S*)?|[a-zA-z]+://((?:(?:25[0-5]|2[0-4]\\d|[01]?\\d?\\d)\\.){3}(?:25[0-5]|2[0-4]\\d|[01]?\\d?\\d))$");
    private static Regex RegMoney = new Regex("^[0-9]+|[0-9]+[.]?[0-9]+$");

    private static bool Match(string content, Regex reg)
    {
        if (!string.IsNullOrEmpty(content))
        {
            return reg.Match(content).Success;
        }
        return false;
    }

    /// <summary>
    /// 是否为数字字符串
    /// </summary>
    public static bool IsUint(this string content)
    {
        return Match(content, RegUint);
    }
    /// <summary>
    /// 是否为数字字符串，可带正负号
    /// </summary>
    public static bool IsInt(this string content)
    {
        return Match(content, RegInt);
    }
    /// <summary>
    /// 是否为浮点数
    /// </summary>
    public static bool IsUfloat(this string content)
    {
        return Match(content, RegUfloat);
    }
    /// <summary>
    /// 是否为浮点数，可带正负号
    /// </summary>
    public static bool IsFloat(this string content)
    {
        return Match(content, RegFloat);
    }
    /// <summary>
    /// 是否为邮箱
    /// </summary>
    static public bool IsEmail(this string content)
    {
        return Match(content, RegEmail);
    }
    /// <summary>
    /// 是否包含中文字符
    /// </summary>
    public static bool HasChinese(this string content)
    {
        return Match(content, RegChinese);
    }
    /// <summary>
    /// 是否为座机电话号码
    /// </summary>
    public static bool IsTellPhone(this string content)
    {
        return Match(content, RegTellPhone);
    }
    /// <summary>
    /// 是否为手机电话号码
    /// </summary>
    public static bool IsMobilePhone(this string content)
    {
        return Match(content, RegMobilePhone);
    }
    /// <summary>
    /// 是否为邮政编码
    /// </summary>
    public static bool IsSend(this string content)
    {
        return Match(content, RegSend);
    }
    /// <summary>
    /// 是否为网络地址
    /// </summary>
    public static bool IsURL(this string content)
    {
        return Match(content, RegURL);
    }
    /// <summary>
    /// 是否为价格
    /// </summary>
    public static bool IsMoney(this string content)
    {
        return Match(content, RegMoney);
    }

    #endregion

    #region string 加密解密

    #region Base64
    public static string Encrypt_Base64(this string content)
    {
        return Encrypt_Base64(content, new UTF8Encoding());
    }
    public static string Decrypt_Base64(this string content)
    {
        return Decrypt_Base64(content, new UTF8Encoding());
    }
    public static string Encrypt_Base64(this string content, Encoding encode)
    {
        return Convert.ToBase64String(encode.GetBytes(content));
    }
    public static string Decrypt_Base64(this string content, Encoding encode)
    {
        return encode.GetString(Convert.FromBase64String(content));
    }
    #endregion

    #region md5
    public static string Encrypt_MD5(this string content, ref string key)
    {
        DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DES.Create();
        key = Encoding.ASCII.GetString(desCrypto.Key);

        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        byte[] inputByteArray = Encoding.Default.GetBytes(content);
        des.Key = Encoding.ASCII.GetBytes(key);
        des.IV = Encoding.ASCII.GetBytes(key);
        MemoryStream ms = new MemoryStream();
        CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();
        StringBuilder ret = new StringBuilder();
        foreach (byte b in ms.ToArray())
        {
            ret.AppendFormat("{0:X2}", b);
        }
        ret.ToString();
        return ret.ToString();
    }
    public static string Decrypt_MD5(this string content, string key)
    {
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        byte[] inputByteArray = new byte[content.Length / 2];
        for (int x = 0; x < content.Length / 2; x++)
        {
            int i = (Convert.ToInt32(content.Substring(x * 2, 2), 16));
            inputByteArray[x] = (byte)i;
        }

        des.Key = Encoding.ASCII.GetBytes(key);
        des.IV = Encoding.ASCII.GetBytes(key);
        MemoryStream ms = new MemoryStream();
        CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();
        StringBuilder ret = new StringBuilder();
        return Encoding.Default.GetString(ms.ToArray());
    }
    #endregion

    #region DES
    /// <summary>
    /// DES加密
    /// </summary>
    /// <param name="data">加密数据</param>
    /// <param name="key">8位字符的密钥字符串</param>
    /// <param name="iv">8位字符的初始化向量字符串</param>
    /// <returns></returns>
    public static string Encrypt_DES(this string data, string key, string iv)
    {
        byte[] byKey = Encoding.ASCII.GetBytes(key);
        byte[] byIV = Encoding.ASCII.GetBytes(iv);

        DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        int i = cryptoProvider.KeySize;
        MemoryStream ms = new MemoryStream();
        CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);

        StreamWriter sw = new StreamWriter(cst);
        sw.Write(data);
        sw.Flush();
        cst.FlushFinalBlock();
        sw.Flush();
        return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
    }

    /// <summary>
    /// DES解密
    /// </summary>
    /// <param name="data">解密数据</param>
    /// <param name="key">8位字符的密钥字符串(需要和加密时相同)</param>
    /// <param name="iv">8位字符的初始化向量字符串(需要和加密时相同)</param>
    /// <returns></returns>
    public static string Decrypt_DES(this string data, string key, string iv)
    {
        byte[] byKey = Encoding.ASCII.GetBytes(key);
        byte[] byIV = Encoding.ASCII.GetBytes(iv);

        byte[] byEnc;
        try
        {
            byEnc = Convert.FromBase64String(data);
        }
        catch
        {
            return null;
        }
        DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        MemoryStream ms = new MemoryStream(byEnc);
        CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
        StreamReader sr = new StreamReader(cst);
        return sr.ReadToEnd();
    }
    #endregion

    #region RFC
    public static string Encrypt_RFC(this string inText, string key)
    {
        byte[] bytesBuff = Encoding.Unicode.GetBytes(inText);
        using (Aes aes = Aes.Create())
        {
            Rfc2898DeriveBytes crypto = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            aes.Key = crypto.GetBytes(32);
            aes.IV = crypto.GetBytes(16);
            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cStream.Write(bytesBuff, 0, bytesBuff.Length);
                    cStream.Close();
                }
                inText = Convert.ToBase64String(mStream.ToArray());
            }
        }
        return inText;
    }
    public static string Decrypt_RFC(this string cryptTxt, string key)
    {
        cryptTxt = cryptTxt.Replace(" ", "+");
        byte[] bytesBuff = Convert.FromBase64String(cryptTxt);
        using (Aes aes = Aes.Create())
        {
            Rfc2898DeriveBytes crypto = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            aes.Key = crypto.GetBytes(32);
            aes.IV = crypto.GetBytes(16);
            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cStream.Write(bytesBuff, 0, bytesBuff.Length);
                    cStream.Close();
                }
                cryptTxt = Encoding.Unicode.GetString(mStream.ToArray());
            }
        }
        return cryptTxt;
    }
    #endregion

    #region 3DES 
    public static string EncryptDES3(this string content, string key)
    {
        TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

        DES.Key = Encoding.ASCII.GetBytes(key);
        DES.Mode = CipherMode.CBC;
        DES.Padding = PaddingMode.PKCS7;

        ICryptoTransform DESEncrypt = DES.CreateEncryptor();

        byte[] Buffer = Encoding.ASCII.GetBytes(content);
        return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
    }
    public static string DecryptDES3(this string content, string key)
    {
        TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

        DES.Key = ASCIIEncoding.ASCII.GetBytes(key);
        DES.Mode = CipherMode.CBC;
        DES.Padding = PaddingMode.PKCS7;

        ICryptoTransform DESDecrypt = DES.CreateDecryptor();

        string result = "";
        try
        {
            byte[] Buffer = Convert.FromBase64String(content);
            result = ASCIIEncoding.ASCII.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }
        catch (Exception e)
        {

        }
        return result;
    }
    #endregion

    #endregion

}









