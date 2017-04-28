/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：Assist
 * 简    述： 
 * 创建时间：2015/8/10 16:47:12
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Text.RegularExpressions;

namespace AutoCSharp
{
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
            GetObjPaths(".xml", inPath).ForEach(delegate(string i)
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

        static public bool isArray(string inStr)
        {
            Regex rgx = new Regex(@"[\[\]]{1}");
            return rgx.IsMatch(inStr);
        }

        /// <summary>
        /// 字符串转类型
        /// </summary>
        static public object stringToType(string inStr)
        {
            object t = inStr;
            if (inStr.Contains("+"))// Excel 类型结构 eg.a+i+1
            {
                string[] ss = inStr.Split('+');
                string strType = ss[1];

                Regex rgx = new Regex(@"[\[\]]{1}");
                if (rgx.IsMatch(strType))
                {
                    strType = rgx.Replace(strType, "");
                    if (strType == "i")
                    {
                        t = typeof(List<System.Int32>);
                    }
                    else if (strType == "ui")
                    {
                        t = typeof(List<System.UInt32>);
                    }
                    else if (strType == "f")
                    {
                        t = typeof(List<System.Single>);
                    }
                    else if (strType == "l")
                    {
                        t = typeof(List<System.Int64>);
                    }
                    else if (strType == "s")
                    {
                        t = typeof(List<System.String>);
                    }
                    else
                    {
                        //CustomType
                        t = "List<Cfg"+Assist.FirstLetterUp(strType)+">";
                    }
                }
                else
                {
                    if (strType == "i")
                    {
                        t = typeof(System.Int32);
                    }
                    else if (strType == "ui")
                    {
                        t = typeof(System.UInt32);
                    }
                    else if (strType == "f")
                    {
                        t = typeof(System.Single);
                    }
                    else if (strType == "l")
                    {
                        t = typeof(System.Int64);
                    }
                    else if (strType == "s")
                    {
                        t = typeof(System.String);
                    }
                    else
                    {
                        //CustomType
                        t = "Cfg"+Assist.FirstLetterUp(strType);
                    }
                }
            }
            else
            {
                if (inStr == "long")
                {
                    t = typeof(System.Int64);
                }
                else if (inStr != "" && Assist.IsNumber(inStr))
                {
                    t = inStr.Contains(".") ? typeof(System.Single) : typeof(System.UInt32);
                }
            }
            return t;
        }

        /// <summary>
        /// 数字返回 True, 其它返回 False
        /// <para>数字前面有一个 +号 或 -号 返回 True</para>
        /// </summary>
        static public bool IsNumber(string inStr)
        {
            System.Text.RegularExpressions.Regex s = new System.Text.RegularExpressions.Regex(@"^[+-]?\d*(,\d{3})*(\.\d+)?$");
            return s.IsMatch(inStr.Trim());
        }

        /// <summary>
        /// 字符串首字母大写
        /// </summary>
        static public string FirstLetterUp(string inStr)
        {
            return inStr.Substring(0, 1).ToUpper() + inStr.Substring(1);
        }

        /// <summary>
        /// 字符串首字母小写
        /// </summary>
        static public string FirstLetterLower(string inStr)
        {
            return inStr.Substring(0, 1).ToLower() + inStr.Substring(1);
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
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return null;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                ret.Add(item.Name, item.PropertyType.ToString());
            }
            return ret;
        }

        /// <summary>
        /// Excel -> DataSet
        /// </summary>
        static public DataSet ExcelToData(string path)
        {
            DataSet ds = new DataSet();

            string strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + path + ";Extended Properties='Excel 12.0; HDR=NO; IMEX=1'";
            OleDbConnection conn = new OleDbConnection(strConn);

            conn.Open();

            string tableName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0][2].ToString().Trim();

            OleDbDataAdapter adapter = null;
            adapter = new OleDbDataAdapter("select * from [" + tableName + "]", strConn);
            adapter.Fill(ds, tableName.Replace("$", ""));
            conn.Close();

            return ds;
        }

        /// <summary>
        /// Txt -> DataSet
        /// </summary>
        static public DataTable TxtToData(string path)
        {
            int startIndex = path.LastIndexOf("/") + 1;
            int endIndex = path.IndexOf("（");
            if (endIndex == -1)
            {
                endIndex = path.IndexOf("(");
            }
            string tableName = path.Substring(startIndex,endIndex-startIndex);

            DataTable dt = new DataTable();
            dt.TableName = tableName;

            using (FileStream fs = new FileStream(path,FileMode.Open,FileAccess.Read,FileShare.ReadWrite))
            {
                StreamReader reader = new StreamReader(fs, System.Text.Encoding.Default);
                string strLine;
                int row = 0;
                int columnCnt = 0;
                while( (strLine = reader.ReadLine()) != null)
                {
                    string[] arrStr = strLine.Split('\t');
                    if (row == 0)
                    {
                        columnCnt = arrStr.Length;
                        for (int i = 0; i < columnCnt; i++)
                        {
                            dt.Columns.Add(new DataColumn(arrStr[i], typeof(string)));
                        }
                    }

                    DataRow dr = dt.NewRow();
                    dr.ItemArray = arrStr;
                    dt.Rows.Add(dr);
                    row ++;
                }
                reader.Close();
            }

            return dt;
        }

        /// <summary>
        /// Excel -> txt
        /// </summary>
        static public bool ExcelToTxt(string path, DataSet ds)
        {
            path = path.Replace("Excel//", "Txt//");
            path = path.Replace(".xlsx", ".txt");

            using (FileStream fs = System.IO.File.Create(path))
            {
                StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.Default);

                DataTable dt = ds.Tables[0];
                int columnCnt;
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    columnCnt = dt.Columns.Count;
                    if (row == 3)
                    {
                        for (int column = 0; column < columnCnt; column++)
                        {
                            if (column == columnCnt - 1)
                            {
                                writer.Write("");
                            }
                            else
                            {
                                writer.Write("\t");
                            }
                        }
                        writer.Write("\n");
                    }
                    for (int column = 0; column < columnCnt; column++)
                    {

                        if (column == columnCnt - 1)
                        {
                            writer.Write(dt.Rows[row][column].ToString());
                        }
                        else
                        {
                            writer.Write(dt.Rows[row][column].ToString() + "\t");
                        }
                    }
                    writer.Write("\n");
                }
                writer.Flush();
                writer.Close();
            }
            
            return true;
        }

        /// <summary>
        /// txt -> xml
        /// </summary>
        static public bool TxtToXML(string path, DataTable dt)
        {
            //保持注释，只替换内容
            path = path.Replace("Txt//", "Xml//");
            path = path.Replace(".txt", ".xml");
            path = Regex.Replace(path, @"[(（].+[)）]", ""); //去掉(***)

            bool bExist = System.IO.File.Exists(path);

            using (FileStream fs = System.IO.File.Open(path,FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.ReadWrite))
            {
                XmlDocument doc = new XmlDocument();
                
                XmlNode root;
                if (bExist)
                {
                    doc.Load(fs);
                    root = doc.SelectSingleNode("Config");
                    if (doc.InnerXml.IndexOf("<?xml") == -1)
                    {
                        XmlDeclaration xmlDec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                        doc.InsertBefore(xmlDec, root);
                    }
                    root.RemoveAll();
                }
                else
                {
                    XmlDeclaration xmlDec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                    doc.AppendChild(xmlDec);
                    root = doc.CreateNode(XmlNodeType.Element, "Config", "");
                    doc.AppendChild(root);
                };
                
                XmlElement element;

                int columnCnt;
                string attribName, attribValue;
                bool bDeleted;
                for (int row = 6; row < dt.Rows.Count; row++)
                {
                    element = doc.CreateElement("item");

                    bDeleted = false;
                    columnCnt = dt.Columns.Count;
                    for (int column = 0; column < columnCnt; column++)
                    {
                        attribName = dt.Rows[0][column].ToString();
                        if (attribName == "")
                        {
                            break;
                        }
                        attribValue = dt.Rows[row][column].ToString();
                        if (column == 0 && attribValue == "")
                        {
                            break;
                        }
                        attribValue = Regex.Replace(attribValue, "\\\"", "");
                        if (attribName == "deleted")
                        {
                            try
                            {
                                if (int.Parse(attribValue) == 1)
                                {
                                    bDeleted = true;
                                    break;
                                }
                            }
                            catch(Exception exp)
                            {
                                throw new Exception("出现空行，attribName:" + attribName + ",row:" + row + ",attribValue:" + attribValue);
                            }
                        }
                        element.SetAttribute(attribName, attribValue);
                    }

                    if (bDeleted == false)
                    {
                        root.AppendChild((XmlNode)element);
                    }
                }
                fs.Seek(0,SeekOrigin.Begin);
                fs.SetLength(0);

                doc.Save(fs);

                fs.Flush();
                fs.Close();
            }

            return true;
        }

        /// <summary>
        /// 检查路径是否存在，如果不存在则创建
        /// </summary>
        /// <param name="inFolderName">目标文件夹名</param>
        static public void CheckFolderExist(string inFolderName)
        {
            string p = System.Environment.CurrentDirectory + "/" + inFolderName;
            if (!System.IO.Directory.Exists(p))
                System.IO.Directory.CreateDirectory(p);
        }

        /// <summary>
        /// 清空目录下所有文件
        /// </summary>
        /// <param name="inFolderName">目标文件夹名</param>
        static public void DeleteFilesInFolder(string inFolderName)
        {
            string p = System.Environment.CurrentDirectory + "\\" + inFolderName;
            string[] arrFile = System.IO.Directory.GetFiles(p);
            foreach (string filePath in arrFile)
            {
                System.IO.File.Delete(filePath);
            }
        }

        /// <summary>
        /// 字符串转枚举
        /// </summary>
        static public T StrToEnum<T>(string inStr,T inType)
        {
            if (!Enum.IsDefined(typeof(T), inStr))
            {
                return inType;
            }
            return (T)Enum.Parse(typeof(T), inStr);
        }

        static public string GetFieldType(string s)
        {
            string b = s == "float" ? "System.Single" :
                       s == "float[]" ? "System.Single[]" :
                       s == "int32" ? "System.Int32" :
                       s == "int32[]" ? "System.Int32[]" :
                       s == "int64" ? "System.Int64" :
                       s == "int64[]" ? "System.Int64[]" :
                       s == "string" ? "System.String" :
                       s == "string[]" ? "System.String[]" :
                       s == "uint32" ? "System.UInt32" :
                       s == "uint32[]" ? "System.UInt32[]" :
                       s == "short" ? "System.Int16" :
                       s == "short[]" ? "System.Int16[]" :
                       s == "ushort" ? "System.UInt16" :
                       s == "ushort[]" ? "System.UInt16[]" :
                       s == "int8" ? "System.Byte" :
                       s == "int8[]" ? "System.Byte[]" :
                       s == "uint8" ? "System.Byte" :
                       s == "uint8[]" ? "System.Byte[]" :
                       s == "byte" ? "System.Byte" :
                       s == "byte[]" ? "System.Byte[]" :
                       s.Contains("Dictionary") ? s : "SMGame." + s;//modify by wk Dictionary不需要带上SMGame命名空间
                       
            return b;
        }
    }
}
