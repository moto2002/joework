
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

//注意：密钥和向量必须为8位，否则加密解密都不成功

public static class DESCrypto
{
    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="ToDecrypt"></param>
    /// <param name="keyStr">解密密钥,要求为8位</param>
    /// <param name="ivStr">可以和key相同</param>
    /// <returns></returns>
    public static byte[] Decrypt(byte[] ToDecrypt, string keyStr,string ivStr)
    {
        byte[] buffer;
        byte[] Key = Encoding.UTF8.GetBytes(keyStr.Substring(0, 8));//以防key超出8位，取前8位
        byte[] IV = Encoding.UTF8.GetBytes(ivStr.Substring(0, 8));
        DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
        using (MemoryStream stream = new MemoryStream())
        {
            using (CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(Key,IV), CryptoStreamMode.Write))
            {
                stream2.Write(ToDecrypt, 0, ToDecrypt.Length);
                stream2.FlushFinalBlock();
                buffer = stream.ToArray();
            }
        }
        return buffer;
    }

    /// <summary>
    /// 加密
    /// </summary>
    public static byte[] Encrypt(byte[] ToEncrypt, string keyStr, string ivStr)
    {
        byte[] buffer;
        byte[] Key = Encoding.UTF8.GetBytes(keyStr.Substring(0, 8));
        byte[] IV = Encoding.UTF8.GetBytes(ivStr.Substring(0, 8));
        DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
        using (MemoryStream stream = new MemoryStream())
        {
            using (CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
            {
                stream2.Write(ToEncrypt, 0, ToEncrypt.Length);
                stream2.FlushFinalBlock();
                buffer = stream.ToArray();
            }
        }
        return buffer;
    }
}


