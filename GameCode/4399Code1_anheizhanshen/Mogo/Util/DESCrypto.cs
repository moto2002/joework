namespace Mogo.Util
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public static class DESCrypto
    {
        public static byte[] Decrypt(byte[] ToDecrypt, byte[] Key)
        {
            byte[] buffer;
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider {
                Key = Key,
                IV = Key
            };
            using (MemoryStream stream = new MemoryStream())
            {
                using (CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    stream2.Write(ToDecrypt, 0, ToDecrypt.Length);
                    stream2.FlushFinalBlock();
                    buffer = stream.ToArray();
                }
            }
            return buffer;
        }

        public static byte[] Encrypt(byte[] ToEncrypt, byte[] Key)
        {
            byte[] buffer;
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider {
                Key = Key,
                IV = Key
            };
            using (MemoryStream stream = new MemoryStream())
            {
                using (CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    stream2.Write(ToEncrypt, 0, ToEncrypt.Length);
                    stream2.FlushFinalBlock();
                    buffer = stream.ToArray();
                }
            }
            return buffer;
        }

        public static string GenerateKey()
        {
            DES des = DES.Create();
            return Encoding.ASCII.GetString(des.Key);
        }
    }
}

