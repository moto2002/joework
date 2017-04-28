using UnityEngine;
using System.Collections;
using com.game.net.buffer;

//发送包
public class LoginRequest
{
    public string username;
    public string passworld;
    public int age;

    public void Write(ByteBuffer bf)
    {
        bf.writeInt(age);
        bf.writeString(username);
        bf.writeString(passworld);
    }
}

//接收包
public class LoginPackage
{
	public string username;
	public string passworld;
	public int age;

    public void Read(ByteBuffer bf)
    {
        age = bf.readInt();
        username = bf.readString();
        passworld = bf.readString();
    }
}



/// <summary>
/// 这个bytebuffer有点类似于java里面的ByteBuffer的操作
/// </summary>
public class test : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
       
        ByteBuffer bf = new ByteBuffer();

        LoginRequest pkt = new LoginRequest();
		pkt.username = "Joe";
		pkt.passworld = "123";
		pkt.age = 22;
        pkt.Write(bf);

        //toArray操作其实是read的操作，读取指针会往后移哦。前面的会被重用哦。太棒了！！
        byte[] _bb = bf.toArray();
        Debug.Log(_bb.Length);
        //然后就可以发出去了。


        //模拟一下接收
        ByteBuffer bf2 = new ByteBuffer();
        bf2.writeBytes(_bb);//可以无限写入哦！！！！

        LoginPackage _TTT = new LoginPackage();
        _TTT.Read(bf2);
        Debug.Log("username:"+_TTT.username +"pwd:"+ _TTT.passworld +"age:"+ _TTT.age);



        ////可以无限写入哦！！！！
        //bf2.writeBytes(_bb);
        ////然后我就可以读取了吧
        ////Debug.Log("bf2:"+bf2.readInt());
        ////Debug.Log("bf2:"+bf2.readString());
        ////Debug.Log("bf2:"+bf2.readString());

        //Debug.Log(bf2.readableBytes());

        ////可用字节的长度
        //int packageSize = bf.readableBytes();
        //Debug.Log("packageSize:"+bf.readableBytes());

        //int i =bf.readInt();
        //Debug.Log(i);
        //string _s =bf.readString();
        //Debug.Log(_s);
        //string _s2 = bf.readString();
        //Debug.Log(_s2);

        ////可读取的字节数,=0的话,说明写入的已经被读取完了
        //Debug.Log(bf.readableBytes());

        ////整个buffer容器大小
        //Debug.Log("buffer contain:"+ bf.getCapacity());

        ////buffer容器的名字，用逗号隔开
        //bf.writeString("hello joe");
        //Debug.Log(bf.remainBufferString());


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
