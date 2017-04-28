/*
 * Copyright (c) shihuanjue.com
 * All rights reserved.
 * 
 * 文 件 名：Excel2Json
 * 简    述： 
 * 创建时间：2015/11/17 22:45:20
 * 创 建 人：洪峤
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.Windows;
using System.IO;
using System.Data;
using Excel;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Excel2Json
{
    private string m_inPath = "output/Excels";
    private string m_outPath = "output/Jsons";

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

                    string jsonFileName = tableName + ".json";

                    //准备一个列表存储整个表的数据
                    Dictionary<string, Dictionary<string, object>> table = new Dictionary<string, Dictionary<string, object>>();

                    for (int i = contentBeginIndex; i < rowCount; i++)//行
                    {
                        //准备一个字典存储每一行的数据
                        Dictionary<string, object> row = new Dictionary<string, object>();

                        string MainKey = "";

                        for (int j = 0; j < colCount; j++)//列
                        {
                            string key = mSheet.Rows[titleIndex][j].ToString();
                            if (key == "")
                            {
                                Assist.Log("下标" + "(" + titleIndex + "," + j + ")" + "不能为空");
                                return;
                            }

                            string strData = mSheet.Rows[typeIndex][j].ToString();
                            string[] strs = strData.Split('+');
                            string keyType = strs[0];
                            int isMainkey = int.Parse(strs[1]);
                            string linkTableName = strs[2];

                            string value = mSheet.Rows[i][j].ToString();

                            switch (keyType)
                            {
                                case "int":
                                    row.Add(key, int.Parse(value));
                                    break;
                                case "string":
                                    row.Add(key, value);
                                    break;
                                case "float":
                                    row.Add(key, float.Parse(value));
                                    break;
                                default:
                                    break;
                            }

                            //是主键，或者是合成主键的一部分字段
                            if (isMainkey == 1)
                            {
                                MainKey += ("_" + value);
                            }

                        }

                        //添加到表数据中
                        // table.Add(mSheet.Rows[i][0].ToString(), row);
                        table.Add(MainKey.Trim('_'), row);
                    }

                    //生成Json字符串
                    string json = JsonConvert.SerializeObject(table, Newtonsoft.Json.Formatting.Indented);

                    string savePath = Path.Combine(outPath, jsonFileName);

                    //写入文件
                    using (FileStream fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                    {
                        using (TextWriter textWriter = new StreamWriter(fileStream, System.Text.Encoding.UTF8))
                        {
                            textWriter.Write(json);
                        }
                    }
                }
            }
        }

        Assist.Log("Gen OK");
    }
}

