
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Windows;
public class Assist
{
    static private string rootPath;
    /// <summary>
    /// 根路径
    /// </summary>
    static public string RootPath
    {
        get
        {
            if (rootPath == null)
                rootPath = Assembly.GetExecutingAssembly().Location.Replace("AutoCSharp.exe", "");
            return rootPath;
        }
    }

    /// <summary>
    /// 获取文件夹内的所有xml文件
    /// </summary>
    static public Dictionary<string, XmlDocument> GetXml(string inPath)
    {
        Dictionary<string, XmlDocument> doc = new Dictionary<string, XmlDocument>();
        GetObjPaths(".xml", inPath).ForEach(delegate (string i)
        {
            XmlDocument d = new XmlDocument();
            d.Load(i);
            string[] s = i.Split('/');
            string fileName = s[s.Length - 1].Split('.')[0];
            doc.Add(fileName, d);
        });
        return doc;
    }

    /// <summary>
    /// 通过指定路径获取该目录下的所有指定类型文件的路径列表
    /// </summary>
    /// <returns>路径列表</returns>
    static public List<string> GetObjPaths(string inType, string inPath)
    {
        List<string> bl = new List<string>();
        string[] strlist1 = Directory.GetFiles(inPath + "/");
        for (int i = 0; i < strlist1.Length; i++)
        {
            FileInfo f = new FileInfo(strlist1[i]);
            if (f.Extension == inType)
            {
                bl.Add(strlist1[i]);
            }
        }
        return bl;
    }

    /// <summary>
    /// 打开文件夹
    /// </summary>
    static public string OpenFolder()
    {
        Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();
        op.InitialDirectory = @"c:\";
        op.RestoreDirectory = true;
        op.Filter = "文本文件(*.xml)|*.xml|所有文件(*.*)|*.*";
        op.ShowDialog();
        return op.FileName;
    }

    /// <summary>
    /// 属性名
    /// <para>属性类型</para>
    /// </summary>
    public static Dictionary<string, string> GetProperties<T>(T t)
    {
        Dictionary<string, string> ret = new Dictionary<string, string>();

        if (t == null)
        {
            return null;
        }
        PropertyInfo[] properties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

        if (properties.Length <= 0)
        {
            return null;
        }
        foreach (PropertyInfo item in properties)
        {
            ret.Add(item.Name, item.PropertyType.ToString());
        }
        return ret;
    }

    /// <summary>
    /// Excel2DataSet
    /// </summary>
    static public DataSet ExcelToDataSet(string path)
    {
        DataSet ds = new DataSet();
        OleDbConnection conn = new OleDbConnection();
        try
        {
            //HDR=Yes/NO 表示是否将首行做标题,若设了YES 数据集中自动忽略了第一行。 IMEX　表示是否强制转换为文本

            //string sql = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+path+";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1;\"";//2003版

            string sql = "Provider=Microsoft.Ace.OleDb.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=No;IMEX=1;\"";//2007版
            conn.ConnectionString = sql;
            conn.Open();
            DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            //string tableName = dt.Rows[0][2].ToString().Trim();//获得第一张工作区间表名
            //OleDbCommand cmd = new OleDbCommand("select * from  [" + tableName + "]", conn);//工作表名就是Excel显示区下面的工作区名
            OleDbCommand cmd = new OleDbCommand("select * from [Sheet1$]", conn);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);    //读取Excel表格             
            da.Fill(ds);
        }
        catch (Exception ex)
        {
            Assist.Log(ex.StackTrace);
        }

        return ds;
    }

    /// <summary>
    /// 检查路径是否存在，如果不存在则创建
    /// </summary>
    /// <param name="inFolderName">目标文件夹名</param>
    static public void CheckFolderExist(string inFolderName)
    {
        string p = Environment.CurrentDirectory + "/" + inFolderName;
        if (!Directory.Exists(p))
            Directory.CreateDirectory(p);
    }
    public static void CreateDir(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    public static void Log(string msg)
    {
        MessageBox.Show(msg);
    }

    public static void ClearDirFiles(string path)
    {
        DirectoryInfo folder = new DirectoryInfo(path);
        FileSystemInfo[] files = folder.GetFileSystemInfos();
        int length = files.Length;
        for (int index = 0; index < length; index++)
        {
            files[index].Delete();
        }
    }

}
