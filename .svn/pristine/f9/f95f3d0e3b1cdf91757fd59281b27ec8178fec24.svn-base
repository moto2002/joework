using UnityEngine;
using System.Collections.Generic;
using MyProtoBuf;
using ProtoBuf;
using System.IO;
using System;

public class TestProtoParse : MonoBehaviour
{

    Dictionary<int, Action<byte[]>> ptIdBindingData = new Dictionary<int, Action<byte[]>>();

    // Use this for initialization
    void Start()
    {
        ptIdBindingData.Add(1, TestMsgParse);

        AddressBook adress = new AddressBook();
        adress.address = "joehome";
        MemoryStream memStream = new MemoryStream();
        Serializer.Serialize<AddressBook>(memStream, adress);
        byte[] x = memStream.ToArray();
        Debug.Log(x.Length);

        ptIdBindingData[1](x);

    }

    void TestMsgParse(byte[] data)
    {
        MemoryStream m2 = new MemoryStream(data);
        AddressBook result = Serializer.Deserialize<AddressBook>(m2);
        Debug.Log("result:" + result.address);

        //string proTypeName = "AddressBook";
        //Type t = Type.GetType(proTypeName);
        //object obj = Activator.CreateInstance(t);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
