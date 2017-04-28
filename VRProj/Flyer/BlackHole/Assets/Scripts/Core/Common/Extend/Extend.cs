using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

public static class KLExtend
{
    #region List

    /// <summary>
    /// 随机排列
    /// </summary>
    public static List<T> RandomList<T>(this List<T> list)
    {
        System.Random random = new System.Random();
        List<T> back = new List<T>();
        for (int i = 0; i < list.Count; i++)
        {
            back.Insert(random.Next(back.Count + 1), list[i]);
        }
        return back;
    }

    /// <summary>
    /// 对数组进行组合操作，选取selectCount个元素进行组合
    /// </summary>
    public static List<List<T>> combination<T>(this List<T> objArray, int selectCount)
    {
        List<List<T>> itemList = new List<List<T>>();
        int totolcount = objArray.Count;
        int[] currectselect = new int[selectCount];
        int last = selectCount - 1;

        for (int i = 0; i < selectCount; i++)
        {
            currectselect[i] = i;
        }

        while (true)
        {
            List<T> items = new List<T>();
            for (int i = 0; i < selectCount; i++)
            {
                items.Add(objArray[currectselect[i]]);
            }

            itemList.Add(items);

            if (currectselect[last] < totolcount - 1)
            {
                currectselect[last]++;
            }
            else
            {
                int pos = last;
                while (pos > 0 && currectselect[pos - 1] == currectselect[pos] - 1)
                {
                    pos--;
                }
                if (pos == 0) return itemList;
                currectselect[pos - 1]++;
                for (int i = pos; i < selectCount; i++)
                {
                    currectselect[i] = currectselect[i - 1] + 1;
                }
            }
        }
    }

    #endregion

    #region int

    /// <summary>
    /// 转换大小，以M为单位
    /// </summary>
    public static float CalSize(this int value)
    {
        return value / 1048576.0f;
    }

    /// <summary>
    /// int to txt file
    /// </summary>
    /// <param name="content">内容</param>
    /// <param name="fullPath">eg. c://temp/test.txt</param>
    public static void ToTxtFile(this int content, string fullPath)
    {
        ToTxtFile(content.ToString(), fullPath);
    }

    #endregion

    #region float  

    /// <summary>
    /// 转换大小，以M为单位
    /// </summary>
    public static float CalSize(this float value)
    {
        return value / 1048576.0f;
    }

    /// <summary>
    /// float to txt file
    /// </summary>
    /// <param name="content">内容</param>
    /// <param name="fullPath">eg. c://temp/test.txt</param>
    public static void ToTxtFile(this float content, string fullPath)
    {
        ToTxtFile(content.ToString(), fullPath);
    }

    #endregion

    #region String

    /// <summary>
    /// string to txt file
    /// </summary>
    /// <param name="content">内容</param>
    /// <param name="fullPath">eg. c://temp/test.txt</param>
    public static void ToTxtFile(this string content, string fullPath)
    {
        string foldName = Path.GetDirectoryName(fullPath);
        if (!Directory.Exists(foldName))
        {
            Directory.CreateDirectory(foldName);
        }
        StreamWriter sw;
        FileInfo t = new FileInfo(fullPath);
        if (!t.Exists)
        {
            sw = t.CreateText();
        }
        else
        {
            sw = t.AppendText();
        }
        sw.WriteLine(content);
        sw.Close();
        sw.Dispose();
    }

    /// <summary>
    /// 读取 txt 文件内容 
    /// </summary>
    /// <param name="fullPath">eg. c://temp/test.txt</param>
    /// <returns></returns>
    public static ArrayList ReadToArray(this string fullPath)
    {
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(fullPath);
        }
        catch (Exception e)
        {
            Debug.LogError("FullTxtPathToArray Error: " + e.ToString() + "\n");
            return null;
        }
        string line;
        ArrayList arrlist = new ArrayList();
        while ((line = sr.ReadLine()) != null)
        {
            arrlist.Add(line);
        }
        sr.Close();
        sr.Dispose();
        return arrlist;
    }

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

    #region DES3  ???
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

    #endregion

    #region Stream

    /// <summary>
    /// byte[] to Stream
    /// </summary>
    public static Stream ToStream(this byte[] bytes)
    {
        return new MemoryStream(bytes);
    }

    /// <summary>
    /// FileStream to File
    /// </summary>
    /// <param name="target">Stream</param>
    /// <param name="desPath">绝对路径，不包含"file://"</param>
    public static void ToFile(this FileStream target, string desPath)
    {
        if (target.Length > 0)
        {
            FileStream fs = File.Create(desPath);
            byte[] bs = target.ToBytes();
            fs.Write(bs, 0, bs.Length);
            fs.Seek(0, SeekOrigin.Begin);
            fs.Close();
        }
    }

    /// <summary>
    /// stream to byte[]
    /// </summary>
    public static byte[] ToBytes(this Stream stream)
    {
        byte[] bytes = new byte[stream.Length];
        stream.Read(bytes, 0, bytes.Length);
        stream.Seek(0, SeekOrigin.Begin);
        return bytes;
    }

    #endregion

    #region Unity Component

    /// <summary>
    /// 高斯模糊 Component
    /// </summary>
    public static IGaussianBlur toGaussianBlur(this Component com, IDisposable old = null)
    {
        if (old != null)
            old.Dispose();
        Renderer r = com.GetComponent<Renderer>();
        if (r == null)
        {
            Debug.LogError("Component object does not have a Renderer ! Can not use GaussianBlur \n");
            return null;
        }
        return new Core.ShaderAssist.GaussianBlur_Com(r);
    }

    /// <summary>
    /// 灰色 Component
    /// </summary>
    public static IDisposable toGray(this Component com, IDisposable old = null)
    {
        if (old != null)
            old.Dispose();
        if (!com.GetComponent<Renderer>())
        {
            Debug.LogError("Can not set Gray, case " + com.name + " does not have a Renderer. \n");
            return null;
        }
        return new Core.ShaderAssist.Gray_Com(com.GetComponent<Renderer>());
    }

    #endregion

    #region Unity Transform

    public static T GetComponentForce<T>(this Transform gameObject) where T : Component
    {
        if (!gameObject.GetComponent<T>())
        {
            gameObject.gameObject.AddComponent<T>();
        }
        return gameObject.GetComponent<T>();
    }

    #endregion

    #region Unity GameObject

    public static T GetComponentForce<T>(this GameObject gameObject) where T : Component
    {
        if (!gameObject.GetComponent<T>())
        {
            gameObject.AddComponent<T>();
        }
        return gameObject.GetComponent<T>();
    }

    public static RaycastHit SendRay(this GameObject target, Vector3 direction, int lay = 0, float distance = 100f)
    {
        Ray ray = new Ray(target.transform.position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance, 1 << lay))
        {
            return hit;
        }
        return hit;
    }

    #endregion

    #region Unity Image

    /// <summary>
    /// 高斯模糊 UI
    /// </summary>
    public static IGaussianBlur toGaussianBlur(this Image image)
    {
        if (image == null || image.sprite == null)
        {
            Debug.LogError("Image or sprite is null, Can not use GaussianBlur ! \n");
            return null;
        }
        return new Core.ShaderAssist.GaussianBlur_UI(image);
    }

    /// <summary>
    /// 灰色 UI
    /// </summary>
    public static void toGray(this Image image)
    {
        if (image == null || image.sprite == null)
        {
            Debug.LogError("Cann't set Gray, case the image or sprite is null!");
            return;
        }
        image.material = new Material(Shader.Find(Core.ShaderAssist.ShaderConfig.Gray));
    }

    #region Texture2D/Texture -> Sprite __&__ Color[] -> Texture2D

    public static Sprite toSprite(this Texture2D texture2d)
    {
        return Sprite.Create(texture2d,
               new Rect(Vector2.zero, new Vector2(texture2d.width, texture2d.height)),
               new Vector2(.5f, .5f));
    }

    public static Sprite toSprite(this Texture texture)
    {
        return texture == null ? null : Sprite.Create(texture as Texture2D,
               new Rect(Vector2.zero, new Vector2(texture.width, texture.height)),
               new Vector2(.5f, .5f));
    }

    public static Texture2D toTexture2D(this Color[] colors, int width, int height)
    {
        Texture2D croppedTexture = new Texture2D(width, height);
        croppedTexture.SetPixels(colors);
        croppedTexture.Apply();
        return croppedTexture;
    }

    #endregion

    #endregion

    #region Unity Text

    public static ITextTyper TypeEffect(this Text text)
    {
        return new TextType(text) as ITextTyper;
    }

    #endregion

    #region Unity Vector3

    /// <summary> 
    /// 平铺 
    /// </summary> 
    /// <param name="sp">起始坐标点</param> 
    /// <param name="objCounts">平铺数量</param> 
    /// <param name="columns">列数</param> 
    /// <param name="radiu">物体直径</param> 
    /// <param name="span">物体间距</param> 
    /// <param name="lines">行数(默认为0，若不为0则往Y轴正方向叠加)</param> 
    /// <returns>ArrayList Vector3坐标列表</returns> 
    public static ArrayList Tile(this Vector3 sp, int objCounts, int columns, float radiu, float span, int lines = 0)
    {
        float px = 0;
        float pz = 0;
        ArrayList al = new ArrayList(objCounts);
        for (int i = 1; i <= objCounts; i++)
        {
            float xx = i % columns == 0 ? (columns - 1) * radiu + columns * span : (i % columns - 1) * radiu + span * (i % columns);
            float yy = radiu;
            float zz = i % columns == 0 ? (((int)i / columns - 1) * radiu + span * (int)((i / columns - 1) + radiu)) :
                    ((int)i / columns) * radiu + span * (int)(i / columns + radiu);
            if (i == 1)
            {
                px = xx;
                pz = zz;
            }
            if (lines != 0)
            {
                yy += (i - 1) / (columns * lines) * (radiu + span);
                zz -= (i - 1) / (columns * lines) * lines * (span + radiu);
            }
            al.Add(new Vector3(xx - px, yy, zz - pz) + sp);
        }
        return al;
    }

    #endregion
}