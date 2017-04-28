using UnityEngine;
using System.Collections;
using System.Text;

public class testDES : MonoBehaviour {

	void Start () {
        string content = "测试一下DES加密,hahhaha";
        string key= "abcdefgh454365"; //key，iv 可以相同
        string iv= "abcdefgh35345";
        byte[] contentBytes = Encoding.UTF8.GetBytes(content);
        byte[] encryBytes = DESCrypto.Encrypt(contentBytes, key, iv);
        string encryStr = Encoding.UTF8.GetString(encryBytes);
        Debug.Log(encryStr);

        byte[] decryBytes = DESCrypto.Decrypt(encryBytes, key, iv);
        string decryStr = Encoding.UTF8.GetString(decryBytes);
        Debug.Log(decryStr);
	}
}
