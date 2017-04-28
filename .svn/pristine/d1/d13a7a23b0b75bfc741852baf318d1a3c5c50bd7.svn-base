using UnityEngine;
using System.Collections;

public class TestCRC : MonoBehaviour
{
    void Start()
    {
         uint num =   CRC32.CalcCRC("Hello crc");
         Debug.Log(num);

         //不区分大小写,把大写字母全部转换成小写字母进行计算crc
         uint num3 = CRC32.CalcCRCNoCase("Hello crc");


         uint num2 = CRC32.CalcCRC("hello crc");
         Debug.Log(num2+ "/" + num3);
    }
}
