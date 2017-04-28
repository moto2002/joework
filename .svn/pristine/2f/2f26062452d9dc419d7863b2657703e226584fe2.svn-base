/*
 * Copyright (c) shihuanjue.com
 * All rights reserved.
 * 
 * 文 件 名：Excel2Xml
 * 简    述： 
 * 创建时间：2015/11/17 22:44:55
 * 创 建 人：洪峤
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.Windows;
using System.IO;
using System.Data;
using System.Xml;
using Excel;

public class Excel2Xml
{
    private string m_inPath = "output/Excels";
    private string m_outPath = "output/Xmls";

    int titleIndex = 1;//第二行：字段名
    int contentBeginIndex = 3;//第四行：内容开始
    int typeIndex = 2;//第三行：是类型

    public void Gen()
    {
        string currDir = Environment.CurrentDirectory;
        this.m_inPath = Path.Combine(currDir, m_inPath);
        Assist.CreateDir(this.m_inPath);

        this.m_outPath = Path.Combine(currDir, m_outPath);
        Assist.CreateDir(this.m_outPath);

        ForGen(m_inPath, m_outPath);
    }


    private void ForGen(string inPath, string outPath)
    {
        Assist.ClearDirFiles(outPath);

        DirectoryInfo folder = new DirectoryInfo(inPath);
        FileSystemInfo[] files = folder.GetFileSystemInfos();
        int length = files.Length;
        for (int index = 0; index < length; index++)
        {
            if (files[index] is DirectoryInfo)
            {
                //this.ForGen(files[index].FullName);
            }
            else
            {
                if (files[index].Name.EndsWith(".xlsx"))
                {
                    //DataSet mResultSet = Assist.ExcelToDataSet(files[index].FullName);
                    FileStream stream = File.Open(files[index].FullName, FileMode.Open, FileAccess.Read);
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    DataSet mResultSet = excelReader.AsDataSet();

                    //判断Excel文件中是否存在数据表
                    if (mResultSet.Tables.Count < 1)
                        return;

                    //默认读取第一个数据表
                    DataTable mSheet = mResultSet.Tables[0];

                    string tableName = mSheet.TableName;

                    //读取数据表行数和列数
                    int rowCount = mSheet.Rows.Count;
                    int colCount = mSheet.Columns.Count;

                    string xmlFileName = tableName + ".xml";
                    string xmlNodeName = tableName;

                    XmlDocument xmlDoc = new XmlDocument(); ;
                    XmlNode root = xmlDoc.CreateElement("Root"); ;
                    xmlDoc.AppendChild(root);

                    XmlNode parent = xmlDoc.CreateElement("List");
                    root.AppendChild(parent);


                    XmlElement node;

                

                    for (int i = contentBeginIndex; i < rowCount; i++)//行
                    {
                        node = xmlDoc.CreateElement(xmlNodeName);

                        for (int j = 0; j < colCount; j++)//列
                        {
                            string key = mSheet.Rows[titleIndex][j].ToString();
                            if (key == "")
                            {
                                Assist.Log("下标" + "(" + titleIndex + "," + j + ")" + "不能为空");
                                return;
                            }

                            string value = mSheet.Rows[i][j].ToString();
                            node.SetAttribute(key, value);
                            parent.AppendChild(node);
                        }
                    }
                    string savePath = Path.Combine(outPath, xmlFileName);
                    xmlDoc.Save(savePath);
                    xmlDoc = null;
                }
            }
        }

        Assist.Log("Gen OK");
    }


}

