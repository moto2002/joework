/*
 * Copyright (c) shihuanjue.com
 * All rights reserved.
 * 
 * 文 件 名：Excel2CSV
 * 简    述： 
 * 创建时间：2015/11/18 22:52:33
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

public class Excel2CSV
{
    private string m_inPath = "output/Excels";
    private string m_outPath = "output/ExcelsCS";

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

                    string csvFileName = tableName + ".txt";

                    string content = "";

                    for (int i = contentBeginIndex; i < rowCount; i++)//行
                    {
                        for (int j = 0; j < colCount; j++)//列
                        {
                            content += mSheet.Rows[i][j].ToString();

                            if (j != colCount - 1)
                            {
                                content += ",";
                            }
                        }
                        if (i != rowCount - 1)
                        {
                            content += "\n";
                        }
                    }

                  
                    string savePath = Path.Combine(outPath, csvFileName);

                    //写入文件
                    using (FileStream fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                    {
                        using (TextWriter textWriter = new StreamWriter(fileStream, System.Text.Encoding.UTF8))
                        {
                            textWriter.Write(content.Trim());
                        }
                    }

                }
            }
        }

        Assist.Log("Gen OK");
    }

   
}

