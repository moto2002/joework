using UnityEngine;
using System.Text;

public class testRSA : MonoBehaviour
{
    void Start()
    {
        RSACrypto.GenKey();

        //Create a UnicodeEncoder to convert between byte array and string.
        UnicodeEncoding ByteConverter = new UnicodeEncoding();

        //Create byte arrays to hold original, encrypted, and decrypted data.
        byte[] dataBytes = ByteConverter.GetBytes("Data to Encrypt,hahaha");
        byte[] encryptedData;
        byte[] decryptedData;

        encryptedData = RSACrypto.RSAEncrypt(dataBytes, RSACrypto.m_publickey, false);

        decryptedData = RSACrypto.RSADecrypt(encryptedData, RSACrypto.m_privateKey, false);

        Debug.Log("message:" + ByteConverter.GetString(decryptedData));

    }
}
