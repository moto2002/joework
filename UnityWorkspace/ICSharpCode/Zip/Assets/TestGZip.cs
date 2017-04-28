using UnityEngine;
using System.Collections;

public class TestGZip : MonoBehaviour
{
    void Start()
    {
        //C:\zsqp\music
        Utils.CompressFile("music/Map_01.wav", "2");
    }

    void Update()
    {

    }
}


/*
 *   //这里通过File.OpenRead方法读取指定文件，并通过其返回的FileStream构造ZipInputStream对象；
        using ( ZipInputStream s = new ZipInputStream(File.OpenRead(args[0]))) {

           //每个包含在Zip压缩包中的文件都被看成是ZipEntry对象，并通过ZipInputStream的GetNextEntry方法
     //依次遍历所有包含在压缩包中的文件。
            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null) {
                int size = 2048;
                byte[] data = new byte[2048];
                //然后以文件数据块的方式将数据打印在控制台上；
                Console.Write("Show contents (y/n) ?");
                if (Console.ReadLine() == "y") {
                    while (true) {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0) {
                            Console.Write(new ASCIIEncoding().GetString(data, 0, size));
                        } else {
                            break;
                        }
                    }
                }
            }
        }
 * 
 * 
 * 
 * */
