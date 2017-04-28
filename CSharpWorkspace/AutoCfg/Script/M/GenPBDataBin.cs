/*
 * Copyright (c) shihuanjue.com
 * All rights reserved.
 * 
 * 文 件 名：GenPBDataBin
 * 简    述： 
 * 创建时间：2015/12/6 16:44:57
 * 创 建 人：洪峤
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.Reflection;
using System.IO;
using Excel;
using System.Data;
using System.Collections.Generic;
using System.Collections;
//using HuanJueData;
public class GenPBDataBin
{
    //PBData Inst
    object obj;

    //PBData type
    Type type;
    Assembly assembly;

    private string m_inPath = "output/Excels";
    private string m_outPath = "output/ExcelsCS";

    int titleIndex = 1;//第二行：字段名
    int contentBeginIndex = 3;//第四行：内容开始
    int typeIndex = 2;//第三行：是类型


    public GenPBDataBin()
    {
        //load CfgData.dll
        string path = Path.Combine(Environment.CurrentDirectory, "output/ExcelsCS/CfgData.dll");
        assembly = Assembly.LoadFrom(path);
        type = assembly.GetType("HuanJueData.PBData");
        obj = Activator.CreateInstance(type);
    }
    public void Gen()
    {
        string currDir = Environment.CurrentDirectory;
        this.m_inPath = Path.Combine(currDir, m_inPath);
        Assist.CreateDir(this.m_inPath);

        this.m_outPath = Path.Combine(currDir, m_outPath);
        Assist.CreateDir(this.m_outPath);

        ForGen(m_inPath, m_outPath);

        string savePath = Path.Combine(m_outPath, "PBData.bytes");

        using (var file = File.Create(savePath))
        {
            try
            {
                ProtoBuf.Serializer.Serialize(file, obj);
            }
            catch (Exception e)
            {
                Assist.Log(e.ToString());
            }
        }

        Assist.Log("Gen OK");

        //FileStream fs = new FileStream(savePath, FileMode.Open, FileAccess.Read);
        //int len = (int)fs.Length;
        //byte[] array = new byte[len];
        //fs.Read(array, 0, len);

        //MemoryStream ms = new MemoryStream(array);
        //PBData pbData = null;
        //try
        //{
        //    pbData  = ProtoBuf.Serializer.Deserialize<PBData>(ms);
        //}
        //catch (Exception e)
        //{
        //    Assist.Log(e.ToString());
        //}
     
        //int c = pbData.HeroInfoDic.Count;

        //// string s = pbData.HeroInfoDic[]

        //foreach (var item in pbData.HeroInfoDic)
        //{
        //    string s = item.Key;

        //    Assist.Log(s);

        //    string sda = item.Value.Name;

        //    Assist.Log(sda);
        //}

        //foreach (var item in pbData.SkillInfoDic)
        //{
        //    string s = item.Key;

        //    Assist.Log(s);

        //    string sda = item.Value.SkillName;

        //    Assist.Log(sda);
        //}


    }

    private void ForGen(string inPath, string outPath)
    {
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


                    FieldInfo _fieldDic = type.GetField(tableName + "Dic");
                    IDictionary _iDic = _fieldDic.GetValue(obj) as IDictionary;

                    Type _chileType = assembly.GetType("HuanJueData." + tableName);
                  

                    for (int i = contentBeginIndex; i < rowCount; i++)//行
                    {
                        object _childObj = Activator.CreateInstance(_chileType);

                        PropertyInfo pro = null;
                        List<string> mainKeyList = new List<string>();

                        for (int j = 0; j < colCount; j++)//列
                        {
                            string strData = mSheet.Rows[typeIndex][j].ToString();
                            string[] strs = strData.Split('+');
                            string keyType = strs[0];
                            int isMainkey = int.Parse(strs[1]);
                            string linkTableName = strs[2];

                            string key = mSheet.Rows[titleIndex][j].ToString();
                            pro = _chileType.GetProperty(Stringer.FirstLetterUp(key));

                            string value = mSheet.Rows[i][j].ToString();

                            if (keyType == "int")
                            {
                                pro.SetValue(_childObj, int.Parse(value));
                            }
                            else if (keyType == "float")
                            {
                                pro.SetValue(_childObj, float.Parse(value));
                            }
                            else if (keyType == "string")
                            {
                                pro.SetValue(_childObj, value.ToString());
                            }

                            //是主键，或者是合成主键的一部分字段
                            if (isMainkey == 1)
                            {
                                mainKeyList.Add(value);
                            }
                        }

                        //最多由3个字段组成主键
                        string MainKey = "";
                        if (mainKeyList.Count == 1)
                        {
                            MainKey = mainKeyList[0];
                        }
                        else if (mainKeyList.Count == 2)
                        {
                            MainKey = mainKeyList[0] + "_" + mainKeyList[1];
                        }
                        else if (mainKeyList.Count == 3)
                        {
                            MainKey = mainKeyList[0] + "_" + mainKeyList[1] + "_" + mainKeyList[2];
                        }

                        _iDic.Add(MainKey, _childObj);
                    }
                }
            }
        }
    }
}

