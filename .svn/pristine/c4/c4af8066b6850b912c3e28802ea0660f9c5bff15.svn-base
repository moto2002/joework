using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System.Text;
using System;
using UnityEngine;
using System.Collections;

public class Utils
{
    /// <summary>
    /// 功能：压缩字文件
    /// </summary>
    /// <param name="infile">被压缩的文件路径</param>
    /// <param name="outfile">生成压缩文件的路径</param>
    public static void CompressFile(string infile, string outfile)
    {
        Stream gs = new GZipOutputStream(File.Create(outfile)); //ZipInputStream
        FileStream fs = File.OpenRead(infile);
        byte[] writeData = new byte[fs.Length];
        fs.Read(writeData, 0, (int)fs.Length);
        gs.Write(writeData, 0, writeData.Length);
        gs.Close(); 
        fs.Close();
    }

    /// <summary>
    /// 压缩字符串
    /// </summary>
    public static string Compress(string source)
    {
        byte[] data = Encoding.UTF8.GetBytes(source);
        MemoryStream ms = null;
        using (ms = new MemoryStream())
        {
            using (Stream stream = new GZipOutputStream(ms))
            {
                try
                {
                    stream.Write(data, 0, data.Length);
                }
                finally
                {
                    stream.Close();
                    ms.Close();
                }
            }
        }
        return Convert.ToBase64String(ms.ToArray());
    }

    /// <summary>
    /// 解压字符串
    /// </summary>
    public static string Decompress(string source)
    {
        string result = string.Empty;
        byte[] buffer = null;
        try
        {
            buffer = Convert.FromBase64String(source);
        }
        catch
        {
            Debug.LogError("Decompress---->>>>" + source);
        }
        using (MemoryStream ms = new MemoryStream(buffer))
        {
            using (Stream sm = new GZipInputStream(ms))
            {
                StreamReader reader = new StreamReader(sm, Encoding.UTF8);
                try
                {
                    result = reader.ReadToEnd();
                }
                finally
                {
                    sm.Close();
                    ms.Close();
                }
            }
        }
        return result;
    }


    /// <summary>
    /// 功能：输入文件路径，返回解压后的字符串
    /// </summary>
    public static string DecompressFile(string infile)
    {
        string result = string.Empty;
        Stream gs = new GZipInputStream(File.OpenRead(infile));
        MemoryStream ms = new MemoryStream();
        int size = 2048;
        byte[] writeData = new byte[size];
        while (true)
        {
            size = gs.Read(writeData, 0, size);
            if (size > 0)
            {
                ms.Write(writeData, 0, size);
            }
            else
            {
                break;
            }
        }
        result = new UTF8Encoding().GetString(ms.ToArray());
        gs.Close();
        ms.Close();
        return result;
    }

}
